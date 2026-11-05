using System;

namespace HarpSoundCard
{
    public enum SoundCardErrorCode : int
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

    public class CheckError
    {
        public int ShowErrorOnConsole(SoundCardErrorCode code, bool writeToConsole)
        {
            switch (code)
            {
                case SoundCardErrorCode.Ok:
                default:
                    return 0;

                case SoundCardErrorCode.BadUserInput:
                    if (writeToConsole) Console.WriteLine("User input not correct. The format should be \"filename\" [index] [type] [sample rate] \n -> [index] from 0 to 31\n -> [type] 0: Int32, 1: Float32\n -> [sample rate] 96000 or 192000");
                    return (int)code;

                case SoundCardErrorCode.HarpSoundCardNotDetected:
                    if (writeToConsole) Console.WriteLine("Sound card not detected. Is any connected to computer?");
                    return (int)code;

                case SoundCardErrorCode.NotAbleToSendMetadata:
                    if (writeToConsole) Console.WriteLine("Not able to start comunication and send sound metadata.");
                    return (int)code;

                case SoundCardErrorCode.NotAbleToReadMetadataCommandReply:
                    if (writeToConsole) Console.WriteLine("Sound metadata reply not received.");
                    return (int)code;

                case SoundCardErrorCode.MetadataCommandReplyNotCorrect:
                    if (writeToConsole) Console.WriteLine("Metadata command reply received is not correct.");
                    return (int)code;

                case SoundCardErrorCode.NotAbleToSendData:
                    if (writeToConsole) Console.WriteLine("Not able to start comunication and send sound data.");
                    return (int)code;

                case SoundCardErrorCode.NotAbleToReadDataCommandReply:
                    if (writeToConsole) Console.WriteLine("Sound data reply not received.");
                    return (int)code;

                case SoundCardErrorCode.DataCommandReplyNotCorrect:
                    if (writeToConsole) Console.WriteLine("Data command reply received is not correct.");
                    return (int)code;

                case SoundCardErrorCode.BadSoundIndex:
                    if (writeToConsole) Console.WriteLine("Sound index not correct. Must be beween 0 and 32.");
                    return (int)code;

                case SoundCardErrorCode.BadSoundLength:
                    if (writeToConsole) Console.WriteLine("Sound length not correct.");
                    return (int)code;

                case SoundCardErrorCode.BadSampleRate:
                    if (writeToConsole) Console.WriteLine("Sample rate not correct or Harp Sound Card is not compatible. Available options are 96 and 192.");
                    return (int)code;

                case SoundCardErrorCode.BadDataType:
                    if (writeToConsole) Console.WriteLine("Data type not correct. Available options are 0 (for integer with 32 bits) and 1 (for float with 32 bits).");
                    return (int)code;

                case SoundCardErrorCode.DataTypeDoNotMatch:
                    if (writeToConsole) Console.WriteLine("Data type don't match with selected sound index.");
                    return (int)code;

                case SoundCardErrorCode.BadDataIndex:
                    if (writeToConsole) Console.WriteLine("Attempt to write outside memory boundaries.");
                    return (int)code;

                case SoundCardErrorCode.ProducingSound:
                    if (writeToConsole) Console.WriteLine("The Sound Board is producing a sound and is not able to receive the new sound.");
                    return (int)code;

                case SoundCardErrorCode.StartedProducingSound:
                    if (writeToConsole) Console.WriteLine("The Sound Board started producing a sound which makes it not able to receive the new sound.");
                    return (int)code;

                case SoundCardErrorCode.NotAbleToOpenFile:
                    if (writeToConsole) Console.WriteLine("File doesn't exist or is empty.");
                    return (int)code;
            }
        }
    }
}