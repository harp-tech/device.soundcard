using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using LibUsbDotNet;
using LibUsbDotNet.Main;

namespace Harp.SoundCard
{
    internal class WaveformHelper
    {
        public static UsbDeviceFinder UsbFinder = new(0x04D8, 0xEE6A);

        public static unsafe SoundCardErrorCode WriteSoundWaveform(
            int? deviceIndex,
            int soundIndex,
            SampleRate sampleRate,
            SampleType sampleType,
            byte[] soundWaveform,
            string waveformName = null)
        {
            const int MetadataSize = 2048;
            const int MaxBufferSize = 32768;
            var usbDeviceIndex = deviceIndex.GetValueOrDefault();
            var usbDevices = UsbDevice.AllDevices.FindAll(UsbFinder);
            if (usbDevices.Count <= usbDeviceIndex)
            {
                return SoundCardErrorCode.HarpSoundCardNotDetected;
            }

            using var usbDevice = usbDevices[usbDeviceIndex].Device;
            try
            {
                // Check if usb device is open and ready
                if (usbDevice == null)
                {
                    return SoundCardErrorCode.HarpSoundCardNotDetected;
                }

                // If this is a "whole" usb device (libusb-win32, linux libusb)
                // it will have an IUsbDevice interface. If not (WinUSB) the 
                // variable will be null indicating this is an interface of a 
                // device.
                if (usbDevice is IUsbDevice wholeUsbDevice)
                {
                    // This is a "whole" USB device. Before it can be used, 
                    // the desired configuration and interface must be selected.

                    // Select config #1
                    wholeUsbDevice.SetConfiguration(1);

                    // Claim interface #0.
                    wholeUsbDevice.ClaimInterface(0);
                }

                using var reader = usbDevice.OpenEndpointReader(ReadEndpointID.Ep01);
                using var writer = usbDevice.OpenEndpointWriter(WriteEndpointID.Ep01);

                /*************************************
                 * Create user metadata byte array with 2048 bytes
                 ************************************/
                /* [0:169]     sound_filename               */
                /* [170:339]   metadata_filename            */
                /* [340:511]   description_filename         */
                /* [512:1535]  metadata_filename content    */
                /* [1536:2047] description_filename content */

                var userMetadata = new byte[MetadataSize];
                if (!string.IsNullOrEmpty(waveformName))
                {
                    Buffer.BlockCopy(Encoding.ASCII.GetBytes(waveformName), 0, userMetadata, 0, waveformName.Length);
                }

                /*************************************
                 * Build sound header
                 ************************************/
                SoundMetadata soundMetadata;
                soundMetadata.SoundIndex = soundIndex;
                soundMetadata.SoundLength = soundWaveform.Length / 4;
                soundMetadata.SampleRate = sampleRate;
                soundMetadata.SampleType = sampleType;

                /*************************************
                 * Create auxiliary parameters
                 ************************************/
                long soundFileSizeInSamples = soundMetadata.SoundLength;
                int commandsToBeSent = (int)(
                    soundFileSizeInSamples * 4 / MaxBufferSize +
                    (((soundFileSizeInSamples * 4 % MaxBufferSize) != 0) ? 1 : 0));

                /*************************************
                 * Create byte arrays for commands
                 ************************************/
                /* Metadata command lenght: 'c' 'm' 'd' '0x80' + random + metadata  + 32768 + 2048 + 'f' */
                /* Data command lenght:     'c' 'm' 'd' '0x81' + random + dataIndex + 32768 + 'f'        */
                /* Reset command lenght:    'c' 'm' 'd' '0x88' + 'f'                                     */
                var metadataCmd = new byte[4 + sizeof(int) + sizeof(SoundMetadata) + MaxBufferSize + MetadataSize + 1];
                var dataCmd = new byte[4 + sizeof(int) + sizeof(int) + MaxBufferSize + 1];
                var resetCmd = new byte[5];

                int metadataCmdDataIndex = 4 + sizeof(int) + sizeof(SoundMetadata);
                int dataCmdDataIndex = 4 + sizeof(int) + sizeof(int);

                byte metadataCmdHeader = 0x80;
                byte dataCmdHeader = 0x81;
                byte resetCmdHeader = 0x88;

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
                var soundMetadataValidation = soundMetadata.Validate();
                if (soundMetadataValidation != SoundCardErrorCode.Ok)
                {
                    return soundMetadataValidation;
                }

                metadataCmd[0] = Convert.ToByte('c');
                metadataCmd[1] = Convert.ToByte('m');
                metadataCmd[2] = Convert.ToByte('d');
                metadataCmd[3] = metadataCmdHeader;
                Marshal.Copy(new IntPtr(&soundMetadata), metadataCmd, 8, sizeof(SoundMetadata));
                Buffer.BlockCopy(userMetadata, 0, metadataCmd, 8 + sizeof(SoundMetadata) + MaxBufferSize, userMetadata.Length);
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
                 * Prepare reset command
                 ************************************/
                resetCmd[0] = Convert.ToByte('c');
                resetCmd[1] = Convert.ToByte('m');
                resetCmd[2] = Convert.ToByte('d');
                resetCmd[3] = resetCmdHeader;
                resetCmd[4] = Convert.ToByte('f');

                /*************************************
                 * Send metadata command and receive reply
                 ************************************/
                int bytesSent;
                int bytesRead;
                int randomReceived;
                int errorReceived;
                int writeTimeout = 2000;
                int readTimeout = 2000;
                Random randomInt = new();
                int randomSent = randomInt.Next();

                using var soundFileStream = new MemoryStream(soundWaveform);
                Buffer.BlockCopy(BitConverter.GetBytes(randomSent), 0, metadataCmd, 4, sizeof(int));
                soundFileStream.Read(metadataCmd, metadataCmdDataIndex, MaxBufferSize);
                reader.Flush();

                var ec = writer.Write(metadataCmd, 0, metadataCmd.Length, writeTimeout, out bytesSent);
                if (ec != 0) return SoundCardErrorCode.NotAbleToSendMetadata;

                ec = reader.Read(commandReply, readTimeout, out bytesRead);
                if (ec != 0) return SoundCardErrorCode.NotAbleToReadMetadataCommandReply;

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
                    return soundMetadata.Validate();
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

                    ec = writer.Write(dataCmd, 0, dataCmd.Length, writeTimeout, out bytesSent);
                    if (ec != 0) return SoundCardErrorCode.NotAbleToSendData;

                    ec = reader.Read(commandReply, readTimeout, out bytesRead);
                    if (ec != 0) return SoundCardErrorCode.NotAbleToReadDataCommandReply;

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
                        return soundMetadata.Validate();
                    }
                }

                return SoundCardErrorCode.Ok;
            }
            finally
            {
                // If this is a "whole" usb device (libusb-win32, linux libusb-1.0)
                // it exposes an IUsbDevice interface. If not (WinUSB) the 
                // 'wholeUsbDevice' variable will be null indicating this is 
                // an interface of a device; it does not require or support 
                // configuration and interface selection.
                if (usbDevice is IUsbDevice wholeUsbDevice)
                {
                    // Release interface #0.
                    wholeUsbDevice.ReleaseInterface(0);
                }
            }
        }
    }
}
