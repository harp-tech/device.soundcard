namespace Harp.SoundCard
{
    internal enum SoundCardErrorCode : int
    {
        Ok = 0,

        BadUserInput = -1,

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

        ProducingSound = -1030,
        StartedProducingSound,

        NotAbleToOpenFile = -1040
    }

    internal static class SoundCardErrorHelper
    {
        public static void ThrowExceptionForErrorCode(SoundCardErrorCode code)
        {
            switch (code)
            {
                case SoundCardErrorCode.Ok:
                default: return;

                case SoundCardErrorCode.BadUserInput:
                    throw new SoundCardException(
                        "User input not correct. The format should be \"filename\" [index] [type] [sample rate] \n" +
                        " -> [index] from 0 to 31\n" +
                        " -> [type] 0: Int32, 1: Float32\n" +
                        " -> [sample rate] 96000 or 192000");

                case SoundCardErrorCode.HarpSoundCardNotDetected:
                    throw new SoundCardException("Sound card not detected. Is any connected to computer?");

                case SoundCardErrorCode.NotAbleToSendMetadata:
                    throw new SoundCardException("Not able to start comunication and send sound metadata.");

                case SoundCardErrorCode.NotAbleToReadMetadataCommandReply:
                    throw new SoundCardException("Sound metadata reply not received.");

                case SoundCardErrorCode.MetadataCommandReplyNotCorrect:
                    throw new SoundCardException("Metadata command reply received is not correct.");

                case SoundCardErrorCode.NotAbleToSendData:
                    throw new SoundCardException("Not able to start comunication and send sound data.");

                case SoundCardErrorCode.NotAbleToReadDataCommandReply:
                    throw new SoundCardException("Sound data reply not received.");

                case SoundCardErrorCode.DataCommandReplyNotCorrect:
                    throw new SoundCardException("Data command reply received is not correct.");

                case SoundCardErrorCode.BadSoundIndex:
                    throw new SoundCardException("Sound index not correct. Must be beween 0 and 32.");

                case SoundCardErrorCode.BadSoundLength:
                    throw new SoundCardException("Sound length not correct.");

                case SoundCardErrorCode.BadSampleRate:
                    throw new SoundCardException("Sample rate not correct or Harp Sound Card is not compatible. Available options are 96 and 192.");

                case SoundCardErrorCode.BadDataType:
                    throw new SoundCardException("Data type not correct. Available options are 0 (for integer with 32 bits) and 1 (for float with 32 bits).");

                case SoundCardErrorCode.DataTypeDoNotMatch:
                    throw new SoundCardException("Data type don't match with selected sound index.");

                case SoundCardErrorCode.BadDataIndex:
                    throw new SoundCardException("Attempt to write outside memory boundaries.");

                case SoundCardErrorCode.ProducingSound:
                    throw new SoundCardException("The Sound Board is producing a sound and is not able to receive the new sound.");

                case SoundCardErrorCode.StartedProducingSound:
                    throw new SoundCardException("The Sound Board started producing a sound which makes it not able to receive the new sound.");

                case SoundCardErrorCode.NotAbleToOpenFile:
                    throw new SoundCardException("File doesn't exist or is empty.");
            }
        }
    }
}
