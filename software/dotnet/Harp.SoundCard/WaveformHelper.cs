using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Harp.SoundCard.Usb;

namespace Harp.SoundCard
{
    internal class WaveformHelper
    {
        const ushort VendorId = 0x04D8;
        const ushort ProductId = 0xEE6A;
        const int Configuration = 1;
        const int Interface = 0;
        const byte WriteEndpoint = 0x01;
        const byte ReadEndpoint = LibUsb.EndpointIn | 0x01;
        const uint FlushTimeout = 10;
        const uint TransferTimeout = 2000;
        const int MinSoundIndex = 2;
        const int MaxSoundIndex = 31;
        const int UserMetadataSize = 2048;
        const int SoundNameSize = 170;
        const int DescriptionSize = UserMetadataSize - SoundNameSize;
        const int ReadMetadataReplyHeaderSize = 28;

        public static unsafe SoundCardErrorCode WriteSoundWaveform(
            int? deviceIndex,
            int soundIndex,
            SampleRate sampleRate,
            SampleType sampleType,
            byte[] soundWaveform,
            string? soundName = null,
            string? description = null)
        {
            const int MaxBufferSize = 32768;

            /* [0:169]    sound name  */
            /* [170:2047] description */
            var userMetadata = new byte[UserMetadataSize];
            WriteString(soundName, userMetadata, 0, SoundNameSize, nameof(soundName));
            WriteString(description, userMetadata, SoundNameSize, DescriptionSize, nameof(description));

            var usbDeviceIndex = deviceIndex.GetValueOrDefault();
            if (usbDeviceIndex < 0)
            {
                return SoundCardErrorCode.HarpSoundCardNotDetected;
            }

            using var context = UsbContext.Create();
            using var handle = OpenDevice(context, usbDeviceIndex);
            if (handle == null)
            {
                return SoundCardErrorCode.HarpSoundCardNotDetected;
            }

            try
            {
                /*************************************
                 * Build sound header
                 ************************************/
                WaveformHeader waveformHeader;
                waveformHeader.SoundIndex = soundIndex;
                waveformHeader.SoundLength = soundWaveform.Length / 4;
                waveformHeader.SampleRate = sampleRate;
                waveformHeader.SampleType = sampleType;

                /*************************************
                 * Create auxiliary parameters
                 ************************************/
                long soundFileSizeInSamples = waveformHeader.SoundLength;
                int commandsToBeSent = (int)(
                    soundFileSizeInSamples * 4 / MaxBufferSize +
                    (((soundFileSizeInSamples * 4 % MaxBufferSize) != 0) ? 1 : 0));

                /*************************************
                 * Create byte arrays for commands
                 ************************************/
                /* Metadata command lenght: 'c' 'm' 'd' '0x80' + random + metadata  + 32768 + 2048 + 'f' */
                /* Data command lenght:     'c' 'm' 'd' '0x81' + random + dataIndex + 32768 + 'f'        */
                var metadataCmd = new byte[4 + sizeof(int) + sizeof(WaveformHeader) + MaxBufferSize + UserMetadataSize + 1];
                var dataCmd = new byte[4 + sizeof(int) + sizeof(int) + MaxBufferSize + 1];

                int metadataCmdDataIndex = 4 + sizeof(int) + sizeof(WaveformHeader);
                int dataCmdDataIndex = 4 + sizeof(int) + sizeof(int);

                byte metadataCmdHeader = 0x80;
                byte dataCmdHeader = 0x81;

                /*************************************
                 * Create byte array to receive replies
                 ************************************/
                /* Metadata command reply: 'c' 'm' 'd' '0x80' + random + error */
                /* Data command reply:     'c' 'm' 'd' '0x81' + random + error */
                int commandReplyLength = 4 + sizeof(int) + sizeof(int);
                byte[] commandReply = new byte[commandReplyLength];

                /*************************************
                 * Prepare metadata command
                 ************************************/
                var soundMetadataValidation = waveformHeader.Validate();
                if (soundMetadataValidation != SoundCardErrorCode.Ok)
                {
                    return soundMetadataValidation;
                }

                metadataCmd[0] = Convert.ToByte('c');
                metadataCmd[1] = Convert.ToByte('m');
                metadataCmd[2] = Convert.ToByte('d');
                metadataCmd[3] = metadataCmdHeader;
                Marshal.Copy(new IntPtr(&waveformHeader), metadataCmd, 8, sizeof(WaveformHeader));
                Buffer.BlockCopy(userMetadata, 0, metadataCmd, 8 + sizeof(WaveformHeader) + MaxBufferSize, userMetadata.Length);
                metadataCmd[metadataCmd.Length - 1] = Convert.ToByte('f');

                /*************************************
                 * Prepare data command
                 ************************************/
                dataCmd[0] = Convert.ToByte('c');
                dataCmd[1] = Convert.ToByte('m');
                dataCmd[2] = Convert.ToByte('d');
                dataCmd[3] = dataCmdHeader;
                dataCmd[dataCmd.Length - 1] = Convert.ToByte('f');

                /*************************************
                 * Send metadata command and receive reply
                 ************************************/
                int randomReceived;
                int errorReceived;
                uint writeTimeout = 2000;
                uint readTimeout = 2000;
                Random randomInt = new();
                int randomSent = randomInt.Next();

                using var soundFileStream = new MemoryStream(soundWaveform);
                Buffer.BlockCopy(BitConverter.GetBytes(randomSent), 0, metadataCmd, 4, sizeof(int));
                soundFileStream.Read(metadataCmd, metadataCmdDataIndex, MaxBufferSize);
                ReadFlush(handle);

                if (!TryTransfer(handle, WriteEndpoint, metadataCmd, writeTimeout))
                    return SoundCardErrorCode.NotAbleToSendMetadata;

                if (!TryTransfer(handle, ReadEndpoint, commandReply, readTimeout))
                    return SoundCardErrorCode.NotAbleToReadMetadataCommandReply;

                randomReceived = BitConverter.ToInt32(commandReply, 4);
                errorReceived = BitConverter.ToInt32(commandReply, 8);
                if (randomSent != randomReceived) return SoundCardErrorCode.MetadataCommandReplyNotCorrect;

                for (int i = 0; i < 8; i++)
                {
                    if (metadataCmd[i] != commandReply[i])
                    {
                        return SoundCardErrorCode.MetadataCommandReplyNotCorrect;
                    }
                }

                if ((SoundCardErrorCode)errorReceived != SoundCardErrorCode.Ok)
                {
                    return waveformHeader.Validate();
                }

                /*************************************
                 * Send data commands and receive replies
                 ************************************/
                int dataIndex = 0;
                int bytesFromFile;
                while (--commandsToBeSent > 0)
                {
                    randomSent = randomInt.Next();
                    Buffer.BlockCopy(BitConverter.GetBytes(randomSent), 0, dataCmd, 4, sizeof(int));
                    Buffer.BlockCopy(BitConverter.GetBytes(++dataIndex), 0, dataCmd, 8, sizeof(int));
                    bytesFromFile = soundFileStream.Read(dataCmd, dataCmdDataIndex, MaxBufferSize);

                    if (!TryTransfer(handle, WriteEndpoint, dataCmd, writeTimeout))
                        return SoundCardErrorCode.NotAbleToSendData;

                    if (!TryTransfer(handle, ReadEndpoint, commandReply, readTimeout))
                        return SoundCardErrorCode.NotAbleToReadDataCommandReply;

                    randomReceived = BitConverter.ToInt32(commandReply, 4);
                    errorReceived = BitConverter.ToInt32(commandReply, 8);
                    if (randomSent != randomReceived) return SoundCardErrorCode.DataCommandReplyNotCorrect;

                    for (int i = 0; i < 8; i++)
                    {
                        if (dataCmd[i] != commandReply[i])
                        {
                            return SoundCardErrorCode.DataCommandReplyNotCorrect;
                        }
                    }

                    if ((SoundCardErrorCode)errorReceived != SoundCardErrorCode.Ok)
                    {
                        return waveformHeader.Validate();
                    }
                }

                return SoundCardErrorCode.Ok;
            }
            finally
            {
                handle.ReleaseInterface(Interface);
            }
        }

        public static SoundCardErrorCode ReadSoundMetadata(int? deviceIndex, int? soundIndex, List<SoundMetadata> sounds)
        {
            var usbDeviceIndex = deviceIndex.GetValueOrDefault();
            if (usbDeviceIndex < 0)
            {
                return SoundCardErrorCode.HarpSoundCardNotDetected;
            }

            if (soundIndex < MinSoundIndex || soundIndex > MaxSoundIndex)
            {
                return SoundCardErrorCode.BadSoundIndex;
            }

            using var context = UsbContext.Create();
            using var handle = OpenDevice(context, usbDeviceIndex);
            if (handle == null)
            {
                return SoundCardErrorCode.HarpSoundCardNotDetected;
            }

            try
            {
                var reply = new byte[ReadMetadataReplyHeaderSize + UserMetadataSize];
                var firstIndex = soundIndex ?? MinSoundIndex;
                var errorCode = ReadMetadata(handle, firstIndex, reply);
                if (errorCode != SoundCardErrorCode.Ok)
                {
                    return errorCode;
                }

                var storedSounds = BitConverter.ToUInt32(reply, 12);
                var lastIndex = soundIndex ?? MaxSoundIndex;
                for (int index = firstIndex; index <= lastIndex; index++)
                {
                    if ((storedSounds & (1u << index)) == 0)
                    {
                        continue;
                    }

                    if (index != firstIndex)
                    {
                        errorCode = ReadMetadata(handle, index, reply);
                        if (errorCode != SoundCardErrorCode.Ok)
                        {
                            return errorCode;
                        }
                    }

                    sounds.Add(ParseSoundMetadata(index, reply));
                }

                return SoundCardErrorCode.Ok;
            }
            finally
            {
                handle.ReleaseInterface(Interface);
            }
        }

        static SoundCardErrorCode ReadMetadata(UsbDeviceHandle handle, int index, byte[] reply)
        {
            /* Read metadata command: 'c' 'm' 'd' '0x84' + random + soundIndex + 'f' */
            /* Read metadata reply:   'c' 'm' 'd' '0x84' + random + error + storedSounds + soundLength + sampleRate + sampleType + 2048 */
            var command = new byte[4 + sizeof(int) + sizeof(int) + 1];
            command[0] = Convert.ToByte('c');
            command[1] = Convert.ToByte('m');
            command[2] = Convert.ToByte('d');
            command[3] = 0x84;
            var randomSent = new Random().Next();
            Buffer.BlockCopy(BitConverter.GetBytes(randomSent), 0, command, 4, sizeof(int));
            Buffer.BlockCopy(BitConverter.GetBytes(index), 0, command, 8, sizeof(int));
            command[command.Length - 1] = Convert.ToByte('f');

            ReadFlush(handle);
            if (!TryTransfer(handle, WriteEndpoint, command, TransferTimeout))
                return SoundCardErrorCode.NotAbleToSendReadMetadata;

            if (!TryTransfer(handle, ReadEndpoint, reply, TransferTimeout))
                return SoundCardErrorCode.NotAbleToReadReadMetadataCommandRepply;

            for (int i = 0; i < 8; i++)
            {
                if (command[i] != reply[i])
                {
                    return SoundCardErrorCode.ReadMetadataCommandReplyNotCorrect;
                }
            }

            return (SoundCardErrorCode)BitConverter.ToInt32(reply, 8);
        }

        static SoundMetadata ParseSoundMetadata(int index, byte[] reply)
        {
            return new SoundMetadata(
                index,
                ReadString(reply, ReadMetadataReplyHeaderSize, SoundNameSize),
                BitConverter.ToInt32(reply, 16),
                (SampleRate)BitConverter.ToInt32(reply, 20),
                ReadString(reply, ReadMetadataReplyHeaderSize + SoundNameSize, DescriptionSize));
        }

        static void WriteString(string? value, byte[] buffer, int offset, int count, string paramName)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            var bytes = Encoding.ASCII.GetBytes(value);
            if (bytes.Length > count)
            {
                throw new ArgumentException($"The text cannot be longer than {count} characters.", paramName);
            }

            Buffer.BlockCopy(bytes, 0, buffer, offset, bytes.Length);
        }

        static string ReadString(byte[] buffer, int offset, int count)
        {
            var text = Encoding.ASCII.GetString(buffer, offset, count);
            var end = text.IndexOf('\0');
            return end < 0 ? text : text.Substring(0, end);
        }

        static UsbDeviceHandle? OpenDevice(UsbContext context, int index)
        {
            using var device = FindDevice(context, index);
            if (device == null)
            {
                return null;
            }

            var handle = device.Open();
            try
            {
                if (handle.GetConfiguration() != Configuration)
                {
                    handle.SetConfiguration(Configuration);
                }

                handle.ClaimInterface(Interface);
                return handle;
            }
            catch
            {
                handle.Dispose();
                throw;
            }
        }

        static UsbDevice? FindDevice(UsbContext context, int index)
        {
            UsbDevice? result = null;
            var devices = context.GetDevices();
            foreach (var device in devices)
            {
                var descriptor = device.GetDescriptor();
                if (result == null && descriptor.idVendor == VendorId && descriptor.idProduct == ProductId && index-- == 0)
                {
                    result = device;
                }
                else
                {
                    device.Dispose();
                }
            }

            return result;
        }

        static bool TryTransfer(UsbDeviceHandle handle, byte endpoint, byte[] data, uint timeout)
        {
            try
            {
                handle.BulkTransfer(endpoint, data, data.Length, timeout);
                return true;
            }
            catch (UsbException)
            {
                return false;
            }
        }

        static void ReadFlush(UsbDeviceHandle handle)
        {
            var buffer = new byte[512];
            try
            {
                while (handle.BulkTransfer(ReadEndpoint, buffer, buffer.Length, FlushTimeout) > 0)
                {
                }
            }
            catch (UsbException ex) when (ex.ErrorCode == LibUsb.ErrorTimeout)
            {
            }
        }
    }
}
