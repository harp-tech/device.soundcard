using System;
using System.Collections.Generic;
using ReactiveUI.Fody.Helpers;

namespace Harp.SoundCard.Design.ViewModels;

public class ImportOptionsDialogViewModel : ViewModelBase
{
    public IEnumerable<SampleRate> SampleRates => (SampleRate[])Enum.GetValues(typeof(SampleRate));

    [Reactive] public SampleRate SelectedSampleRate { get; set; } = SampleRate.SampleRate96000Hz;
    [Reactive] public float Amplification { get; set; } = 1.0f;
    [Reactive] public bool ApplyWindowSettings { get; set; }

    public ImportOptionsResult ToResult() => new()
    {
        SampleRate = SelectedSampleRate,
        Amplification = Amplification,
        ApplyWindowSettings = ApplyWindowSettings
    };
}
