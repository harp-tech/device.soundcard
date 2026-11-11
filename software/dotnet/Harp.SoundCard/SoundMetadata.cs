namespace Harp.SoundCard
{
    /// <summary>
    /// Represents the metadata of a sound in the SoundCard device.
    /// </summary>
    public class SoundMetadata
    {
        internal SoundMetadata(
            int soundIndex,
            string soundName,
            int soundLength,
            SampleRate sampleRate,
            string description)
        {
            SoundIndex = soundIndex;
            SoundName = soundName;
            SoundLength = soundLength;
            SampleRate = sampleRate;
            Description = description;
        }

        /// <summary>
        /// Gets the index of the sound in the device.
        /// </summary>
        public int SoundIndex { get; }

        /// <summary>
        /// Gets the name stored with the sound, or an empty string if no name was stored.
        /// </summary>
        public string SoundName { get; }

        /// <summary>
        /// Gets the number of 32-bit samples in the sound, counting both channels.
        /// </summary>
        public int SoundLength { get; }

        /// <summary>
        /// Gets the sample rate used to play back the sound.
        /// </summary>
        public SampleRate SampleRate { get; }

        /// <summary>
        /// Gets the description stored with the sound, or an empty string if no description
        /// was stored.
        /// </summary>
        public string Description { get; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{SoundIndex}: {SoundName} ({SoundLength} samples, {SampleRate})";
        }
    }
}
