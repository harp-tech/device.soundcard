namespace Harp.SoundCard.Design.ViewModels;

public sealed class ImportOptionsResult
{
    public SampleRate SampleRate { get; set; } = SampleRate.SampleRate96000Hz;
    public float Amplification { get; set; } = 1;
    public bool ApplyWindowSettings { get; set; } = true;
}
