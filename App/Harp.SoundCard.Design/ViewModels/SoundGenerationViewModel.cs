using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using Harp.SoundCard.Design.SoundBuilders;
using Harp.SoundCard.Design.Views;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using NWaves.Signals;
using NWaves.Signals.Builders;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
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
    [Reactive] public float PhaseLeft { get; set; }
    [Reactive] public float PhaseRight { get; set; }
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
    public IEnumerable<NoiseType> NoiseTypes => (NoiseType[])Enum.GetValues(typeof(NoiseType));
    public IEnumerable<WindowType> WindowTypes => (WindowType[])Enum.GetValues(typeof(WindowType));

    public ReactiveCommand<Unit, Unit> GenerateToneCommand { get; }
    public ReactiveCommand<Unit, Unit> GenerateNoiseCommand { get; }
    public ReactiveCommand<Unit, Unit> SendToDeviceCommand { get; }
    public ReactiveCommand<Unit, Unit> SaveToFileCommand { get; }
    public ReactiveCommand<Unit, Unit> ImportFromFileCommand { get; }

    public event Action? PlotUpdated;

    [Reactive] public Plot? Plot { get; set; }
    [Reactive] public bool ShowLeftChannel { get; set; } = true;
    [Reactive] public bool ShowRightChannel { get; set; } = true;
    [Reactive] public string SoundFileName { get; set; } = string.Empty;
    [Reactive] public int SaveSoundIndex { get; set; } = 2;

    [ObservableAsProperty] public bool IsSendingToDevice { get; }
    [ObservableAsProperty] public bool IsSavingToFile { get; }
    [ObservableAsProperty] public bool IsImportingToFile { get; }

    [Reactive] private DiscreteSignal? CurrentSignalLeft { get; set; }
    [Reactive] private DiscreteSignal? CurrentSignalRight { get; set; }

    public SoundGenerationViewModel()
    {
        // Validation rules
        // Left frequency must be <= Nyquist for current SampleRate
        this.ValidationRule(
            vm => vm.FrequencyLeft,
            this.WhenAnyValue(vm => vm.FrequencyLeft, vm => vm.SampleRate,
                (freq, sr) => freq >= 0 && freq <= ((int)sr / 2.0)),
            "Left frequency must be between 0 and Nyquist (SampleRate/2)."
        );

        // Right frequency must be <= Nyquist for current SampleRate
        this.ValidationRule(
            vm => vm.FrequencyRight,
            this.WhenAnyValue(vm => vm.FrequencyRight, vm => vm.SampleRate,
                (freq, sr) => freq >= 0 && freq <= ((int)sr / 2.0)),
            "Right frequency must be between 0 and Nyquist (SampleRate/2)."
        );
        
        // duration must be positive
        this.ValidationRule(
            vm => vm.DurationLeft,
            this.WhenAnyValue(vm => vm.DurationLeft,
                duration => duration > 0),
            "Left duration must be positive.");
        this.ValidationRule(
            vm => vm.DurationRight,
            this.WhenAnyValue(vm => vm.DurationRight,
                duration => duration > 0),
            "Right duration must be positive.");
        
        var canGenerateTone = this.ValidationContext.Valid;
        
        GenerateToneCommand = ReactiveCommand.Create(GenerateTone, canGenerateTone);
        GenerateNoiseCommand = ReactiveCommand.Create(GenerateNoise);

        var canSendToDevice = this.WhenAnyValue(x => x.CurrentSignalLeft, x => x.CurrentSignalRight)
            .Select(tuple => tuple.Item1 != null && tuple.Item2 != null)
            .CombineLatest(this.ValidationContext.Valid, (hasSignals, isValid) => hasSignals && isValid);
        
        SendToDeviceCommand = ReactiveCommand.Create(SendToDevice, canSendToDevice);
        SendToDeviceCommand.IsExecuting.ToPropertyEx(this, x => x.IsSendingToDevice);
        SendToDeviceCommand.ThrownExceptions
            .Subscribe(ex => Console.WriteLine($"Error sending to device: {ex.Message}"));

        var canSaveToFile = this.WhenAnyValue(x => x.CurrentSignalLeft, x => x.CurrentSignalRight)
            .Select(tuple => tuple.Item1 != null && tuple.Item2 != null);
        SaveToFileCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            if (CurrentSignalLeft == null || CurrentSignalRight == null)
                throw new InvalidOperationException("Cannot save to file: no signal generated.");

            if (CurrentSignalLeft.SamplingRate != CurrentSignalRight.SamplingRate)
                throw new InvalidOperationException(
                    "Cannot save to file: left and right signals have different sampling rates.");

            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var activeWindow = desktop.Windows.FirstOrDefault(w => w.IsActive) ??
                                   desktop.MainWindow;
                if (activeWindow == null)
                    return;
                var soundName = string.IsNullOrWhiteSpace(SoundFileName) ? "sound" : SoundFileName;
                var options = new FilePickerSaveOptions
                {
                    Title = "Save Sound Waveform",
                    DefaultExtension = "bin",
                    SuggestedFileName = $"i{SaveSoundIndex}_{soundName}",
                    ShowOverwritePrompt = true
                };

                var file = await activeWindow.StorageProvider.SaveFilePickerAsync(options);
                if (file != null)
                {
                    var bytesToWrite =
                        ConvertToInt32PcmAndInterleaveChannels(CurrentSignalLeft.Samples, CurrentSignalRight.Samples);
                    await using var stream = await file.OpenWriteAsync();
                    await stream.WriteAsync(bytesToWrite, 0, bytesToWrite.Length);
                    Console.WriteLine($"Sound waveform saved to {file.Name}");
                }
            }
        }, canSaveToFile);
        SaveToFileCommand.IsExecuting.ToPropertyEx(this, x => x.IsSavingToFile);
        SaveToFileCommand.ThrownExceptions
            .Subscribe(ex => Console.WriteLine($"Error saving to file: {ex.Message}"));

        ImportFromFileCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var activeWindow = desktop.Windows.FirstOrDefault(w => w.IsActive) ??
                                   desktop.MainWindow;
                if (activeWindow == null)
                    return;

                var options = new FilePickerOpenOptions
                {
                    Title = "Import Sound Waveform (.bin)",
                    AllowMultiple = false,
                    FileTypeFilter = new List<FilePickerFileType>
                    {
                        new FilePickerFileType("Binary Files") { Patterns = ["*.bin"] }
                    }
                };

                var file = (await activeWindow.StorageProvider.OpenFilePickerAsync(options)).FirstOrDefault();
                if (file == null || !file.Name.EndsWith(".bin", StringComparison.OrdinalIgnoreCase))
                {
                    // show dialog to user that file is invalid
                    var messageBoxStandardWindow = MessageBoxManager
                        .GetMessageBoxStandard("Invalid file",
                            "Please select a valid .bin file containing interleaved stereo int32 PCM data.",
                            icon: Icon.Error);
                    await messageBoxStandardWindow.ShowAsync();
                    return;
                }

                await using var binStream = await file.OpenReadAsync();
                var bytesData = new byte[binStream.Length];
                await binStream.ReadExactlyAsync(bytesData, 0, bytesData.Length);

                var (leftSamples, rightSamples) = DecodeStereoInt32Pcm(bytesData);

                var dialog = new ImportOptionsDialog
                {
                    DataContext = new ImportOptionsDialogViewModel()
                };

                var importOptions = await dialog.ShowDialog<ImportOptionsResult?>(activeWindow);
                if (importOptions == null)
                    return; // user cancelled

                ApplyAmplificationInPlace(leftSamples, importOptions.Amplification, importOptions.SampleRate);
                ApplyAmplificationInPlace(rightSamples, importOptions.Amplification, importOptions.SampleRate);

                if (importOptions.ApplyWindowSettings)
                {
                    var sr = (int)importOptions.SampleRate;

                    var leftFadeSamples = (int)(WindowDurationLeft * sr / 1000.0);
                    var rightFadeSamples = (int)(WindowDurationRight * sr / 1000.0);

                    var leftSignalForWindow = new DiscreteSignal(sr, leftSamples);
                    var rightSignalForWindow = new DiscreteSignal(sr, rightSamples);

                    ApplyFadeWindows(leftSignalForWindow, leftFadeSamples, WindowTypeLeft, WindowApplyBeginLeft, WindowApplyEndLeft);
                    ApplyFadeWindows(rightSignalForWindow, rightFadeSamples, WindowTypeRight, WindowApplyBeginRight, WindowApplyEndRight);

                    leftSamples = leftSignalForWindow.Samples;
                    rightSamples = rightSignalForWindow.Samples;
                }

                SampleRate = importOptions.SampleRate;
                NoiseSampleRate = importOptions.SampleRate;

                CurrentSignalLeft = new DiscreteSignal((int)importOptions.SampleRate, leftSamples);
                CurrentSignalRight = new DiscreteSignal((int)importOptions.SampleRate, rightSamples);

                UpdateSignalSeries();
            }
        });
        ImportFromFileCommand.IsExecuting.ToPropertyEx(this, x => x.IsImportingToFile);
        ImportFromFileCommand.ThrownExceptions
            .Subscribe(ex => Console.WriteLine($"Error importing file: {ex.Message}"));

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
                UpdateSignalSeries(false);
            });

        this.WhenAnyValue(x => x.IsDarkMode)
            .Subscribe(_ =>
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
        
        // handle constraints to SoundFilename
        this.WhenAnyValue(x => x.SoundFileName)
            .Skip(1)
            .Subscribe(filename =>
            {
                const int maxLength = 170;
                var ascii = new string(filename.Where(c => c <= 127).ToArray());
                var sanitized = ascii.Length > maxLength ? ascii.Substring(0, maxLength) : ascii;
                if (sanitized != filename)
                    SoundFileName = sanitized;
            });
    }

    private static (float[] left, float[] right) DecodeStereoInt32Pcm(byte[] bytesData)
    {
        const int bytesPerFrame = 8; // 2 x int32 (stereo)
        if (bytesData.Length == 0 || bytesData.Length % bytesPerFrame != 0)
            throw new InvalidOperationException("Invalid .bin format: expected interleaved stereo int32 PCM.");

        int frameCount = bytesData.Length / bytesPerFrame;
        float[] left = new float[frameCount];
        float[] right = new float[frameCount];

        // 2 ^ 31 - 1 -> 24 bit PCM max value
        double max31 = Math.Pow(2, 31) - 1;
        float invMax31 = 1f / (float)max31;
        for (int i = 0; i < frameCount; i++)
        {
            int baseIndex = i * bytesPerFrame;
            int li = BitConverter.ToInt32(bytesData, baseIndex);
            int ri = BitConverter.ToInt32(bytesData, baseIndex + 4);
            left[i] = Math.Clamp(li * invMax31, -1f, 1f);
            right[i] = Math.Clamp(ri * invMax31, -1f, 1f);
        }

        return (left, right);
    }

    private static void ApplyAmplificationInPlace(float[] samples, float coefficient,
        SampleRate sampleRate = SampleRate.SampleRate96000Hz)
    {
        if (coefficient <= 0)
            return;

        var signal = new DiscreteSignal((int)sampleRate, samples);
        signal.Amplify(coefficient);
    }

    private void SendToDevice()
    {
        if (CurrentSignalLeft == null || CurrentSignalRight == null)
            throw new InvalidOperationException("Cannot send to device: no signal generated.");

        if (CurrentSignalLeft.SamplingRate != CurrentSignalRight.SamplingRate)
            throw new InvalidOperationException(
                "Cannot send to device: left and right signals have different sampling rates.");

        // get data converted to 24-bit PCM represented as int32 and interleave channels in a single byte array
        var soundWaveform =
            ConvertToInt32PcmAndInterleaveChannels(CurrentSignalLeft.Samples, CurrentSignalRight.Samples);

        var updater = new UpdateSoundWaveform
        {
            DeviceIndex = null,
            SoundIndex = SaveSoundIndex,
            SoundName = SoundFileName,
            SampleRate = (SampleRate)CurrentSignalLeft.SamplingRate
        };
        updater.Process(Observable.Return(soundWaveform))
            .Subscribe(
                _ => Console.WriteLine("Sound waveform sent successfully."),
                async void (ex) =>
                {
                    if (ex is not SoundCardException)
                        return;

                    Console.WriteLine($"Error sending sound waveform: {ex.Message}");
                    // show error dialog to user
                    var messageBoxStandardWindow = MessageBoxManager
                        .GetMessageBoxStandard("Error sending to device",
                            $"SoundCard not detected.{Environment.NewLine}{Environment.NewLine}Is it properly connected and configured?",
                            icon: Icon.Error);
                    await messageBoxStandardWindow.ShowAsync();
                });
    }

    private void GenerateTone()
    {
        double amplitudeLeft = GetAmplitudeFromMode(AmplitudeLeft, DbfsLeft, AmplitudeModePulse);
        double amplitudeRight = GetAmplitudeFromMode(AmplitudeRight, DbfsRight, AmplitudeModePulse);

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

        CurrentSignalLeft = signalLeft;
        CurrentSignalRight = signalRight;

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
        CurrentSignalLeft = noiseLeft;
        CurrentSignalRight = noiseRight;
        
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

    private void UpdateSignalSeries(bool autoScale = true)
    {
        if (CurrentSignalLeft == null || CurrentSignalRight == null || Plot == null)
            return;

        var signalLeft = CurrentSignalLeft.Samples;
        var signalRight = CurrentSignalRight.Samples;
        
        // save plot zoom and pan state
        var limits = Plot.Axes.GetLimits();
        Plot.Clear();
        if (ShowLeftChannel)
            Plot.Add.Signal(signalLeft, color: Color.FromSKColor(SKColors.DarkCyan));
        if (ShowRightChannel)
            Plot.Add.Signal(signalRight, color: Color.FromSKColor(SKColors.Red));

        // set X and Y limits according to the data we have
        if(signalLeft != null && signalRight != null)
        {
            Plot.Axes.SetLimitsY(Math.Min(signalLeft.Min(), signalRight.Min()),
                Math.Max(signalLeft.Max(), signalRight.Max()));
            Plot.Axes.SetLimitsX(0, Math.Max(signalLeft.Length, signalRight.Length) - 1);
        }
        
        if(autoScale)
        {
            Plot.Axes.AutoScale();
        }
        else
        {
            // compare current limits and if they are different, restore the old zoom and pan state
            var newLimits = Plot.Axes.GetLimits();
            if (limits != newLimits)
            {
                Plot.Axes.SetLimits(limits);
            }            
        }

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

    private static byte[] ConvertToInt32PcmAndInterleaveChannels(float[] leftSamples, float[] rightSamples)
    {
        int frameCount = Math.Max(leftSamples.Length, rightSamples.Length);

        const int channels = 2;
        const int bytesPerSample = 4; // int32
        byte[] bytes = new byte[frameCount * channels * bytesPerSample];

        // 2 ^ 31 - 1 -> 24 bit PCM max value
        double max31 = Math.Pow(2, 31) - 1;

        for (int i = 0; i < frameCount; i++)
        {
            float l = i < leftSamples.Length ? Math.Clamp(leftSamples[i], -1f, 1f) : 0f;
            float r = i < rightSamples.Length ? Math.Clamp(rightSamples[i], -1f, 1f) : 0f;

            int li = (int)Math.Round(l * max31);
            int ri = (int)Math.Round(r * max31);

            int baseIndex = i * channels * bytesPerSample;

            // little-endian int32: L then R
            bytes[baseIndex + 0] = (byte)(li & 0xFF);
            bytes[baseIndex + 1] = (byte)((li >> 8) & 0xFF);
            bytes[baseIndex + 2] = (byte)((li >> 16) & 0xFF);
            bytes[baseIndex + 3] = (byte)((li >> 24) & 0xFF);

            bytes[baseIndex + 4] = (byte)(ri & 0xFF);
            bytes[baseIndex + 5] = (byte)((ri >> 8) & 0xFF);
            bytes[baseIndex + 6] = (byte)((ri >> 16) & 0xFF);
            bytes[baseIndex + 7] = (byte)((ri >> 24) & 0xFF);
        }

        return bytes;
    }
}
