using System.Runtime.InteropServices;

namespace Harp.SoundCard
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct SoundMetadata
    {
        public int SoundIndex;
        public int SoundLength;
        public SampleRate SampleRate;
        public SampleType SampleType;

        internal readonly SoundCardErrorCode Validate()
        {
            if (SoundIndex < 2 || SoundIndex > 32)
            {
                return SoundCardErrorCode.BadSoundIndex;
            }

            if (SoundLength < 16)
            {
                return SoundCardErrorCode.BadSoundLength;
            }

            if (SampleRate != SampleRate.SampleRate96000Hz && SampleRate != SampleRate.SampleRate192000Hz)
            {
                return SoundCardErrorCode.BadSampleRate;
            }

            if (SampleType != SampleType.Int32 && SampleType != SampleType.Float32)
            {
                return SoundCardErrorCode.BadDataType;
            }

            if (SoundIndex == 0 && SampleType != SampleType.Float32)
            {
                return SoundCardErrorCode.DataTypeDoNotMatch;
            }

            if (SoundIndex == 1 && SampleType != SampleType.Float32)
            {
                return SoundCardErrorCode.DataTypeDoNotMatch;
            }

            if (SoundIndex > 1 && SampleType != SampleType.Int32)
            {
                return SoundCardErrorCode.DataTypeDoNotMatch;
            }

            return SoundCardErrorCode.Ok;
        }
    }
}
