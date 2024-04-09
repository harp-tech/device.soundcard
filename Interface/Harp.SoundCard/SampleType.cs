namespace Harp.SoundCard
{
    /// <summary>
    /// Specifies the available bit depths for sound waveform samples.
    /// </summary>
    public enum SampleType : int
    {
        /// <summary>
        /// Specifies signed 32-bit samples.
        /// </summary>
        Int32 = 0,

        /// <summary>
        /// Specifies single-precision 32-bit floating-point samples.
        /// </summary>
        Float32 = 1
    }
}
