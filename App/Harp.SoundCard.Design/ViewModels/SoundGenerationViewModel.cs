using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Harp.SoundCard.Design.SoundBuilders;
using NWaves.Signals;
using NWaves.Signals.Builders;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ScottPlot;
using SkiaSharp;


namespace Harp.SoundCard.Design.ViewModels;

public enum NoiseType
{
    UniformWhiteNoise,
    GaussianWhiteNoise
}

public enum WindowType
{
    Hanning,
    Hamming,
    Blackman
}

public enum AmplitudeMode
{
    Amplitude,
    Dbfs
}

public class SoundGenerationViewModel : ViewModelBase
{
    // Pure tone properties
    [Reactive] public double FrequencyLeft { get; set; } = 5000;
    [Reactive] public double FrequencyRight { get; set; } = 5000;
    [Reactive] public double DurationLeft { get; set; } = 100.0;
    [Reactive] public double DurationRight { get; set; } = 100.0;
    [Reactive] public double AmplitudeLeft { get; set; } = 0.5;
    [Reactive] public double AmplitudeRight { get; set; } = 0.5;
    [Reactive] public double DbfsLeft { get; set; } = -6.0;
    [Reactive] public double DbfsRight { get; set; } = -6.0;
    [Reactive] public float PhaseLeft { get; set; } = 0.0f;
    [Reactive] public float PhaseRight { get; set; } = 0.0f;
    [Reactive] public bool UseWindowLeft { get; set; }
    [Reactive] public bool UseWindowRight { get; set; }
    [Reactive] public SampleRate SampleRate { get; set; } = SampleRate.SampleRate96000Hz;
    [Reactive] public AmplitudeMode AmplitudeModePulse { get; set; } = AmplitudeMode.Amplitude;
    
    // Noise properties
    [Reactive] public double NoiseDurationLeft { get; set; } = 2000.0;
    [Reactive] public double NoiseDurationRight { get; set; } = 2000.0;
    [Reactive] public int NoiseSeedLeft { get; set; }
    [Reactive] public int NoiseSeedRight { get; set; }
    [Reactive] public double NoiseAmplitudeLeft { get; set; } = 0.057;
    [Reactive] public double NoiseAmplitudeRight { get; set; } = 0.057;
    [Reactive] public double NoiseDbfsLeft { get; set; } = -24.88;
    [Reactive] public double NoiseDbfsRight { get; set; } = -24.88;
    [Reactive] public bool NoiseUseWindowLeft { get; set; } = true;
    [Reactive] public bool NoiseUseWindowRight { get; set; } = true;
    [Reactive] public NoiseType NoiseTypeLeft { get; set; } = NoiseType.UniformWhiteNoise;
    [Reactive] public NoiseType NoiseTypeRight { get; set; } = NoiseType.UniformWhiteNoise;
    [Reactive] public SampleRate NoiseSampleRate { get; set; } = SampleRate.SampleRate96000Hz;
    [Reactive] public AmplitudeMode AmplitudeModeNoise { get; set; } = AmplitudeMode.Amplitude;

    // Window configuration
    [Reactive] public int WindowDurationLeft { get; set; } = 10;
    [Reactive] public int WindowDurationRight { get; set; } = 10;
    [Reactive] public bool WindowApplyBeginLeft { get; set; } = true;
    [Reactive] public bool WindowApplyBeginRight { get; set; } = true;
    [Reactive] public bool WindowApplyEndLeft { get; set; } = true;
    [Reactive] public bool WindowApplyEndRight { get; set; } = true;
    [Reactive] public WindowType WindowTypeLeft { get; set; } = WindowType.Hanning;
    [Reactive] public WindowType WindowTypeRight { get; set; } = WindowType.Hanning;

    // Charts
    public IEnumerable<NoiseType> NoiseTypes => (NoiseType[])System.Enum.GetValues(typeof(NoiseType));
    public IEnumerable<WindowType> WindowTypes => (WindowType[])System.Enum.GetValues(typeof(WindowType));

    public ReactiveCommand<Unit, Unit> GenerateToneCommand { get; }
    public ReactiveCommand<Unit, Unit> GenerateNoiseCommand { get; }

    public event Action? PlotUpdated;

    [Reactive] public Plot? Plot { get; set; }
    [Reactive] public bool ShowLeftChannel { get; set; } = true;
    [Reactive] public bool ShowRightChannel { get; set; } = true;

    private DiscreteSignal? _currentSignalLeft;
    private DiscreteSignal? _currentSignalRight;

    public SoundGenerationViewModel()
    {
        GenerateToneCommand = ReactiveCommand.Create(GenerateTone);
        GenerateNoiseCommand = ReactiveCommand.Create(GenerateNoise);

        this.WhenAnyValue(x => x.Plot)
            .Subscribe(plot =>
            {
                if (Plot == null || plot == null)
                    return;

                // Configure common plot settings
                Plot.FigureBackground.Color = Colors.Transparent;
                Plot.DataBackground.Color = Colors.Transparent;
                Plot.Axes.Bottom.Label.Text = "Samples";
                Plot.Axes.Left.Label.Text = "Amplitude";

                // Configure the plot for the current theme
                ConfigurePlotForTheme();
            });

        this.WhenAnyValue(x => x.ShowLeftChannel, x => x.ShowRightChannel)
            .Subscribe(_ =>
            {
                UpdateSignalSeries();
            });

        this.WhenAnyValue(x => x.IsDarkMode)
            .Subscribe(isDarkMode =>
            {
                ConfigurePlotForTheme();
            });

        // update Dbfs and Amplitude properties automatically when either is changed
        this.WhenAnyValue(x => x.AmplitudeLeft)
            .Skip(1)
            .Subscribe(a => DbfsLeft = AmplitudeToDbfs(a));
        
        this.WhenAnyValue(x => x.AmplitudeRight)
            .Skip(1)
            .Subscribe(a => DbfsRight = AmplitudeToDbfs(a));

        this.WhenAnyValue(x => x.DbfsLeft)
            .Skip(1)
            .Subscribe(d => AmplitudeLeft = DbfsToAmplitude(d));

        this.WhenAnyValue(x => x.DbfsRight)
            .Skip(1)
            .Subscribe(d => AmplitudeRight = DbfsToAmplitude(d));
        
        this.WhenAnyValue(x => x.NoiseAmplitudeLeft)
            .Skip(1)
            .Subscribe(a => NoiseDbfsLeft = AmplitudeToDbfs(a));
        
        this.WhenAnyValue(x => x.NoiseAmplitudeRight)
            .Skip(1)
            .Subscribe(a => NoiseDbfsRight = AmplitudeToDbfs(a));

        this.WhenAnyValue(x => x.NoiseDbfsLeft)
            .Skip(1)
            .Subscribe(d => NoiseAmplitudeLeft = DbfsToAmplitude(d));

        this.WhenAnyValue(x => x.NoiseDbfsRight)
            .Skip(1)
            .Subscribe(d => NoiseAmplitudeRight = DbfsToAmplitude(d));
    }

    private void GenerateTone()
    {
        double amplitudeLeft = GetAmplitudeFromMode(AmplitudeLeft, DbfsLeft, AmplitudeModeNoise);
        double amplitudeRight = GetAmplitudeFromMode(AmplitudeRight, DbfsRight, AmplitudeModeNoise);

        var signalLeft = new SineBuilder()
            .SetParameter("frequency", FrequencyLeft)
            .SetParameter("min", -amplitudeLeft)
            .SetParameter("max", amplitudeLeft)
            .SetParameter("phase", Math.PI * PhaseLeft / 180.0) // Convert degrees to radians
            .SampledAt((int)SampleRate) // Convert SampleRate enum to int
            .OfDuration(DurationLeft / 1000.0) // Convert milliseconds to seconds
            .Build();

        var signalRight = new SineBuilder()
            .SetParameter("frequency", FrequencyRight)
            .SetParameter("min", -amplitudeRight)
            .SetParameter("max", amplitudeRight)
            .SetParameter("phase", Math.PI * PhaseRight / 180.0) // Convert degrees to radians
            .SampledAt((int)SampleRate) // Convert SampleRate enum to int
            .OfDuration(DurationRight / 1000.0) // Convert milliseconds to seconds
            .Build();

        // Optionally apply windowing if UseWindowLeft/Right is true
        if (UseWindowLeft)
        {
            var windowLengthSamplesLeft = (int)(WindowDurationLeft * (int)SampleRate / 1000.0);

            ApplyFadeWindows(signalLeft, windowLengthSamplesLeft, WindowTypeLeft, WindowApplyBeginLeft,
                WindowApplyEndLeft);
        }
        if (UseWindowRight)
        {
            var windowLengthSamplesRight = (int)(WindowDurationRight * (int)SampleRate / 1000.0);

            ApplyFadeWindows(signalRight, windowLengthSamplesRight, WindowTypeRight, WindowApplyBeginRight,
                WindowApplyEndRight);
        }

        _currentSignalLeft = signalLeft;
        _currentSignalRight = signalRight;

        UpdateSignalSeries();
    }

    private void GenerateNoise()
    {
        double amplitudeLeft = GetAmplitudeFromMode(NoiseAmplitudeLeft, DbfsLeft, AmplitudeModePulse);
        double amplitudeRight = GetAmplitudeFromMode(NoiseAmplitudeRight, DbfsRight, AmplitudeModePulse);

        var noiseLeft = NoiseTypeLeft switch
        {
            NoiseType.UniformWhiteNoise => new UniformWhiteNoiseBuilder()
                .SetParameter("min", -amplitudeLeft)
                .SetParameter("max", amplitudeLeft)
                .SetParameter("seed", NoiseSeedLeft)
                .SampledAt((int)NoiseSampleRate) // Convert SampleRate enum to int
                .OfDuration(NoiseDurationLeft / 1000.0) // Convert milliseconds to seconds
                .Build(),
            NoiseType.GaussianWhiteNoise => new GaussianWhiteNoiseBuilder()
                .SetParameter("mean", 0.0)
                .SetParameter("stdDev", amplitudeLeft)
                .SetParameter("seed", NoiseSeedLeft)
                .SampledAt((int)NoiseSampleRate) // Convert SampleRate enum to int
                .OfDuration(NoiseDurationLeft / 1000.0) // Convert milliseconds to seconds
                .Build(),
            _ => throw new NotSupportedException($"Unsupported noise type: {NoiseTypeLeft}")
        };

        var noiseRight = NoiseTypeRight switch
        {
            NoiseType.UniformWhiteNoise => new UniformWhiteNoiseBuilder()
                .SetParameter("min", -amplitudeRight)
                .SetParameter("max", amplitudeRight)
                .SetParameter("seed", NoiseSeedRight)
                .SampledAt((int)NoiseSampleRate) // Convert SampleRate enum to int
                .OfDuration(NoiseDurationRight / 1000.0) // Convert milliseconds to seconds
                .Build(),
            NoiseType.GaussianWhiteNoise => new GaussianWhiteNoiseBuilder()
                .SetParameter("mean", 0.0)
                .SetParameter("stdDev", amplitudeRight)
                .SetParameter("seed", NoiseSeedRight)
                .SampledAt((int)NoiseSampleRate) // Convert SampleRate enum to int
                .OfDuration(NoiseDurationRight / 1000.0) // Convert milliseconds to seconds
                .Build(),
            _ => throw new NotSupportedException($"Unsupported noise type: {NoiseTypeRight}")
        };

        if (NoiseUseWindowLeft)
        {
            var windowLengthSamplesLeft = (int)(WindowDurationLeft * (int)SampleRate / 1000.0);

            ApplyFadeWindows(noiseLeft, windowLengthSamplesLeft, WindowTypeLeft, WindowApplyBeginLeft,
                WindowApplyEndLeft);
        }

        if (NoiseUseWindowRight)
        {
            var windowLengthSamplesRight = (int)(WindowDurationRight * (int)SampleRate / 1000.0);

            ApplyFadeWindows(noiseRight, windowLengthSamplesRight, WindowTypeRight, WindowApplyBeginRight,
                WindowApplyEndRight);
        }

        // update current signals
        _currentSignalLeft = noiseLeft;
        _currentSignalRight = noiseRight;
        
        UpdateSignalSeries();
    }

    private static void ApplyFadeWindows(DiscreteSignal signal, int fadeLength, WindowType windowType, bool applyBegin,
        bool applyEnd)
    {
        var window = windowType switch
        {
            WindowType.Hamming => NWaves.Windows.Window.Hamming(fadeLength * 2),
            WindowType.Blackman => NWaves.Windows.Window.Blackman(fadeLength * 2),
            _ => NWaves.Windows.Window.Hann(fadeLength * 2)
        };

        int half = fadeLength;

        if (applyBegin)
        {
            // Fade-in: apply the first half of the window
            for (int i = 0; i < half && i < signal.Length; i++)
                signal[i] *= window[i];
        }

        if (applyEnd)
        {
            for (int i = 0; i < half && i < signal.Length; i++)
                signal[signal.Length - half + i] *= window[half - i];
        }
    }

    private void UpdateSignalSeries()
    {
        if (_currentSignalLeft == null || _currentSignalRight == null || Plot == null)
            return;

        var signalLeft = _currentSignalLeft.Samples;
        var signalRight = _currentSignalRight.Samples;

        Plot.Clear();
        if (ShowLeftChannel)
            Plot.Add.Signal(signalLeft, color: Color.FromSKColor(SKColors.DarkCyan));
        if (ShowRightChannel)
            Plot.Add.Signal(signalRight, color: Color.FromSKColor(SKColors.Red));

        // set X and Y limits according to the data we have
        Plot.Axes.SetLimitsY(Math.Min(signalLeft.Min(), signalRight.Min()),
            Math.Max(signalLeft.Max(), signalRight.Max()));
        Plot.Axes.SetLimitsX(0, Math.Max(signalLeft.Length, signalRight.Length) - 1);

        PlotUpdated?.Invoke();
    }

    private void ConfigurePlotForTheme()
    {
        if (Plot == null)
            return;

        // NOTE: due to a bug in ScottPlot, we need to set these properties as default values
        Plot.Axes.Color(Color.FromSKColor(SKColors.DarkGray));
        Plot.Grid.MajorLineColor = Color.FromSKColor(SKColors.DarkGray);

        // NOTE: reactivate this when ScottPlot supports dark mode properly
        // if (IsDarkMode)
        // {
        //     Plot.Axes.Color(Color.FromSKColor(SKColors.DarkGray));
        //     Plot.Grid.MajorLineColor = Color.FromSKColor(SKColors.DarkGray);
        // }
        // else
        // {
        //     Plot.Axes.Color(Color.FromSKColor(SKColors.Black));
        //     Plot.Grid.MajorLineColor = Color.FromSKColor(SKColors.Black);
        // }
    }

    private double AmplitudeToDbfs(double amplitude)
    {
        return 20.0 * Math.Log10(Math.Max(amplitude, 1e-10));
    }

    private double DbfsToAmplitude(double dbfs)
    {
        return Math.Pow(10.0, dbfs / 20.0);
    }
    
    private double GetAmplitudeFromMode(double amplitude, double dbfs, AmplitudeMode mode)
    {
        // If Amplitude mode, use amplitude directly
        if (mode == AmplitudeMode.Amplitude)
            return amplitude;

        return DbfsToAmplitude(dbfs);
    }
}
