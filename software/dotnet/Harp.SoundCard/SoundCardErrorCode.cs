namespace Harp.SoundCard
{
    internal enum SoundCardErrorCode : int
    {
        Ok = 0,

        HarpSoundCardNotDetected = -1000,
        NotAbleToSendMetadata,
        NotAbleToReadMetadataCommandReply,
        MetadataCommandReplyNotCorrect,
        NotAbleToSendData,
        NotAbleToReadDataCommandReply,
        DataCommandReplyNotCorrect,
        NotAbleToSendReadMetadata,
        NotAbleToReadReadMetadataCommandRepply,
        ReadMetadataCommandReplyNotCorrect,

        BadSoundIndex = -1020,
        BadSoundLength,
        BadSampleRate,
        BadDataType,
        DataTypeDoNotMatch,
        BadDataIndex,

        ProducingSound = -1030
    }

    internal static class SoundCardErrorHelper
    {
        public static void ThrowExceptionForErrorCode(SoundCardErrorCode code)
        {
            switch (code)
            {
                case SoundCardErrorCode.Ok:
                    return;

                case SoundCardErrorCode.HarpSoundCardNotDetected:
                    throw new SoundCardException("Sound Card not detected. Check that its soundbank USB port is connected to the computer and that its driver is installed.");

                case SoundCardErrorCode.NotAbleToSendMetadata:
                    throw new SoundCardException("Not able to start communication and send sound metadata.");

                case SoundCardErrorCode.NotAbleToReadMetadataCommandReply:
                    throw new SoundCardException("Sound metadata reply not received.");

                case SoundCardErrorCode.MetadataCommandReplyNotCorrect:
                    throw new SoundCardException("Metadata command reply received is not correct.");

                case SoundCardErrorCode.NotAbleToSendData:
                    throw new SoundCardException("Not able to start communication and send sound data.");

                case SoundCardErrorCode.NotAbleToReadDataCommandReply:
                    throw new SoundCardException("Sound data reply not received.");

                case SoundCardErrorCode.DataCommandReplyNotCorrect:
                    throw new SoundCardException("Data command reply received is not correct.");

                case SoundCardErrorCode.NotAbleToSendReadMetadata:
                    throw new SoundCardException("Not able to start communication and request the stored sounds.");

                case SoundCardErrorCode.NotAbleToReadReadMetadataCommandRepply:
                    throw new SoundCardException("Stored sounds reply not received.");

                case SoundCardErrorCode.ReadMetadataCommandReplyNotCorrect:
                    throw new SoundCardException("Stored sounds reply received is not correct.");

                case SoundCardErrorCode.BadSoundIndex:
                    throw new SoundCardException("Sound index not correct. Must be between 2 and 31.");

                case SoundCardErrorCode.BadSoundLength:
                    throw new SoundCardException("Sound length not correct.");

                case SoundCardErrorCode.BadSampleRate:
                    throw new SoundCardException("Sample rate not correct or Harp Sound Card is not compatible. Available options are 96 and 192.");

                case SoundCardErrorCode.BadDataType:
                    throw new SoundCardException("Data type not correct. Available options are 0 (for integer with 32 bits) and 1 (for float with 32 bits).");

                case SoundCardErrorCode.DataTypeDoNotMatch:
                    throw new SoundCardException("Data type does not match the selected sound index.");

                case SoundCardErrorCode.BadDataIndex:
                    throw new SoundCardException("Attempt to write outside memory boundaries.");

                case SoundCardErrorCode.ProducingSound:
                    throw new SoundCardException("The Sound Card is playing a sound at 192 kHz and cannot receive a new sound.");

                default:
                    throw new SoundCardException($"The sound card reported error {(int)code}.");
            }
        }
    }
}
