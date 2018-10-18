using System;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Text;
using System.Text.RegularExpressions;
using LibUsbDotNet;
using LibUsbDotNet.Main;
using System.Runtime.InteropServices;
using System.Reflection;

namespace HarpSoundCard
{
    public class ReadWrite
    {
        public static UsbDevice MyUsbDevice;

        #region USB Vendor and Product ID
        public static UsbDeviceFinder MyUsbFinder = new UsbDeviceFinder(0x04D8, 0xEE6A);

        public static object Extensions { get; private set; }
        #endregion

        #region public static void ReadMetadataFromDevice
        public static void readMetadataFromDevice(
            UsbEndpointWriter writer,
            UsbEndpointReader reader,
            int index,
            Array metadataArray,
            ref UInt32 bitMask,
            ref int soundLength,
            ref int dataType,
            ref int sampleRate,
            ref string soundFilename,
            ref string metadataFilename,
            ref bool hasSound,
            ref bool hasMetadata)
        {
            /*************************************
            * Create byte arrays for command
            ************************************/
            /* Read metadata command lenght: 'c' 'm' 'd' '0x84' + random + soundIndex + 'f' */
            var readMetadataCmd = new byte[4 + sizeof(int) + sizeof(int) + 1];
            byte readMetadataCmdHeader = 0x84;

            Random randomInt = new Random();
            int randomSent = randomInt.Next();

            readMetadataCmd[0] = Convert.ToByte('c');
            readMetadataCmd[1] = Convert.ToByte('m');
            readMetadataCmd[2] = Convert.ToByte('d');
            readMetadataCmd[3] = readMetadataCmdHeader;
            System.Buffer.BlockCopy(BitConverter.GetBytes(randomSent), 0, readMetadataCmd, 4, sizeof(int));
            System.Buffer.BlockCopy(BitConverter.GetBytes(index), 0, readMetadataCmd, 8, sizeof(int));
            readMetadataCmd[readMetadataCmd.Length - 1] = Convert.ToByte('f');

            /*************************************
             * Create byte array to receive replies
             ************************************/
            /* Read metadata command reply: 'c' 'm' 'd' '0x84' + random + error + availableBitMask + soundLength + dataType + sampleRate + 2048 */
            byte[] readMetadataReply = new byte[4 + 6 * sizeof(int) + 2048];

            /*************************************
            * Send read metadata command and receive reply
            ************************************/
            int bytesSent;
            int bytesRead;
            int randomReceived;
            int errorReceived;

            ErrorCode ec = ErrorCode.None;

            int writeTimeout = 2000;
            int readTimeout = 2000;

            reader.Flush();

            ec = writer.Write(readMetadataCmd, 0, readMetadataCmd.Length, writeTimeout, out bytesSent);
            if (ec != ErrorCode.None) throw new Exception("NotAbleToSendReadMetadata");

            ec = reader.Read(readMetadataReply, readTimeout, out bytesRead);
            if (ec != ErrorCode.None) throw new Exception("NotAbleToReadReadMetadataCommandRepply");

            randomReceived = BitConverter.ToInt32(readMetadataReply, 4);
            errorReceived = BitConverter.ToInt32(readMetadataReply, 8);

            if (randomSent != randomReceived) throw new Exception("ReadMetadataCommandReplyNotCorrect");    // Redundant code -- not needed
            for (int i = 0; i < 8; i++)
                if (readMetadataCmd[i] != readMetadataReply[i]) throw new Exception("ReadMetadataCommandReplyNotCorrect");

            if ((SoundCardErrorCode)errorReceived != SoundCardErrorCode.Ok)
            {
                Console.WriteLine("Error: " + (SoundCardErrorCode)errorReceived);
                throw new Exception("Error: " + (SoundCardErrorCode)errorReceived);
            }

            bitMask = BitConverter.ToUInt32(readMetadataReply, 12);

            if ((bitMask & ((UInt32) 1 << index)) != ((UInt32)1 << index))
            {
                hasSound = false;
                return;
            }
            else
            {
                hasSound = true;
            }

            soundLength = BitConverter.ToInt32(readMetadataReply, 16);
            sampleRate = BitConverter.ToInt32(readMetadataReply, 20);
            dataType = BitConverter.ToInt32(readMetadataReply, 24);                        
            System.Buffer.BlockCopy(readMetadataReply, 28 + 256 + 256, metadataArray, 0, 2048 - 256 - 256);
            soundFilename = System.Text.Encoding.Default.GetString(readMetadataReply, 28, 256).TrimEnd((char)0);

            if (readMetadataReply[28 + 256] != 0)
            {
                hasMetadata = true;
                metadataFilename = System.Text.Encoding.Default.GetString(readMetadataReply, 28 + 256, 256).TrimEnd((char)0);
            }
            else
            {
                hasMetadata = false;
            }

            //Console.Write(" bitMask: " + Convert.ToString(bitMask, 2));
            //Console.Write(" soundLength: " + soundLength);
            //Console.Write(" dataType: " + dataType);
            //Console.Write(" sampleRate: " + sampleRate);
            //Console.WriteLine("");
        }
        #endregion

        #region static void SaveToFiles
        public static UInt32 SaveToFiles(
            UsbEndpointWriter writer,
            UsbEndpointReader reader,
            int soundIndex,
            bool isMetadata,
            bool isSound,
            string directory)
        {
            var metadataArray = new byte[2048 - 256 - 256];
            UInt32 bitMask = 0;
            int soundLength = 0;
            int dataType = 0;
            int sampleRate = 0;
            string soundFilename = String.Empty;
            string metadataFilename = String.Empty;
            bool hasSound = false;
            bool hasMetadata = false;
            

            readMetadataFromDevice(
                writer,
                reader,
                soundIndex,
                metadataArray,
                ref bitMask,
                ref soundLength,
                ref dataType,
                ref sampleRate,
                ref soundFilename,
                ref metadataFilename,
                ref hasSound,
                ref hasMetadata);

            string suffix = "i";
            if (soundIndex< 9)
                suffix += "0" + soundIndex + "_";
            else
                suffix += soundIndex + "_";

            if (soundFilename.IndexOf(suffix) != 0)
                soundFilename = suffix + soundFilename;
            if (metadataFilename.IndexOf(suffix) != 0)
                metadataFilename = suffix + metadataFilename;

            if (hasSound & isSound)
            {
                FileStream fs;

                fs = File.Open(System.IO.Path.Combine(directory, soundFilename), FileMode.Create);
                //Read the sound
                fs.Close();
            }

            if (hasMetadata && isMetadata)
            {
                FileStream fs;

                fs = File.Open(System.IO.Path.Combine(directory, metadataFilename), FileMode.Create);
                fs.Write(metadataArray, 0, 2048 - 256 - 256);
                fs.Close();
            }
            
            if (hasSound)
            {
                StreamWriter sw;

                sw = File.CreateText(System.IO.Path.Combine(directory, soundFilename + ".metadata.txt"));
                sw.WriteLine("SOUND_INDEX = " + soundIndex);
                sw.WriteLine("SOUND_LENGTH_SAMPLES = " + soundLength);
                sw.WriteLine("SOUND_LENGTH_MS = " + soundLength/2/((float)sampleRate)*1000);
                sw.WriteLine("SAMPLE_RATE = " + sampleRate);

                if (dataType == 0)
                    sw.WriteLine("DATA_TYPE = Int32");
                else
                    sw.WriteLine("DATA_TYPE = Float32");

                sw.WriteLine("SOUND_FILENAME = " + soundFilename);

                if (hasMetadata)
                {
                    sw.WriteLine("USER_METADATA_FILENAME = " + metadataFilename);
                }

                sw.Close();
            }

            return bitMask;
        }
        #endregion

        public static int Main(string[] args)
        {
            ErrorCode ec = ErrorCode.None;

            try
            {
                #region Open USB device and endpoints

                // Find and open the usb device.
                MyUsbDevice = UsbDevice.OpenUsbDevice(MyUsbFinder);

                // If the device is open and ready
                if (MyUsbDevice == null) throw new Exception("Harp Sound Card not found.");

                // If this is a "whole" usb device (libusb-win32, linux libusb)
                // it will have an IUsbDevice interface. If not (WinUSB) the 
                // variable will be null indicating this is an interface of a 
                // device.
                IUsbDevice wholeUsbDevice = MyUsbDevice as IUsbDevice;
                if (!ReferenceEquals(wholeUsbDevice, null))
                {
                    // This is a "whole" USB device. Before it can be used, 
                    // the desired configuration and interface must be selected.

                    // Select config #1
                    wholeUsbDevice.SetConfiguration(1);

                    // Claim interface #0.
                    wholeUsbDevice.ClaimInterface(0);
                }

                // open read endpoint 1.
                UsbEndpointReader reader = MyUsbDevice.OpenEndpointReader(ReadEndpointID.Ep01);

                // open write endpoint 1.
                UsbEndpointWriter writer = MyUsbDevice.OpenEndpointWriter(WriteEndpointID.Ep01);

                #endregion

                #region Create folders if don't exist
                /*************************************
                 * Create folders if don't exist
                 ************************************/
                var currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var fromDirectory = System.IO.Path.Combine(currentDirectory, "fromSoundCard");

                if (Directory.Exists(fromDirectory) == false)
                    System.IO.Directory.CreateDirectory(fromDirectory);

                #endregion

                /*************************************
                 * User input
                 ************************************/
                bool isAll = false;
                bool isMetadata = false;
                bool isSound = false;
                bool isDeleteAll = false;
                int soundIndex = 2;

                if (args.GetLength(0) == 2 || args.GetLength(0) == 3)
                {
                    isMetadata = (args[0].IndexOf("-u") != -1);
                    isSound = (args[0].IndexOf("-s") != -1);
                    isAll = (args[1].IndexOf("a") != -1);

                    if (args[0].IndexOf("-b") != -1)
                    {
                        isMetadata = true;
                        isSound = true;
                    }

                    if (isAll == false)
                    {
                        soundIndex = Convert.ToInt32(args[1]);
                    }

                    if (args.GetLength(0) == 3)
                    {
                        isDeleteAll = (args[2].IndexOf("-d") != -1);
                    }
                }
                
                //isMetadata = true;
                //isAll = false;
                //isDeleteAll = false;
                //soundIndex = 2;

                if ((args.GetLength(0) != 2 && args.GetLength(0) != 3) || (isSound == false && isMetadata == false))
                {
                    Console.WriteLine("User input not correct.");
                    Console.WriteLine("The format should be one of the next options:");
                    Console.WriteLine("");
                    Console.WriteLine("  fromSoundCard [command] all [option]");
                    Console.WriteLine("  fromSoundCard [command] [index] [option]");
                    Console.WriteLine("");
                    Console.WriteLine("  -> [command]      -usermetadata");
                    Console.WriteLine("                    -sound               -- not implemented yet");
                    Console.WriteLine("                    -both");
                    Console.WriteLine("  -> [index]        from 0 to 31         -- 0 and 1 not implemented yet");
                    Console.WriteLine("  -> [option]       -deleteAll (deletes all files in the folder \\fromSoundCard)");
                    return (int) SoundCardErrorCode.BadUserInput;
                }

                if (soundIndex < 2 || soundIndex > 31)
                {
                    Console.WriteLine("Error: " + SoundCardErrorCode.BadSoundIndex);
                    return (int)SoundCardErrorCode.BadSoundIndex;
                }

                //Console.Write(isAll);
                //Console.Write(isMetadata);
                //Console.Write(isSound);
                //Console.Write(soundIndex);

                /*************************************
                 * Delete all files
                 ************************************/
                if (isDeleteAll)
                {
                    System.IO.DirectoryInfo directory = new DirectoryInfo(fromDirectory);

                    foreach (FileInfo file in directory.GetFiles())
                    {
                        file.Delete();
                    }
                }

                /*************************************
                 * Read metadata
                 ************************************/
                UInt32 bitMask;

                if (isAll)
                {
                    bitMask = SaveToFiles(writer, reader, 2, isMetadata, isSound, fromDirectory);

                    for (int i = 3; i < 32; i++)
                        if ((bitMask & ((UInt32)1 << i)) == ((UInt32)1 << i))
                            SaveToFiles(writer, reader, i, isMetadata, isSound, fromDirectory);
                }
                else
                {
                    SaveToFiles(writer, reader, soundIndex, isMetadata, isSound, fromDirectory);
                }


                /*************************************
                 * Send reset command
                 ************************************/
                //ec = writer.Write(resetCmd, 0, resetCmd.Length, writeTimeout, out bytesSent);
                //if (ec != ErrorCode.None) throw new Exception("NotAbleToSendData");

                /*************************************
                 * All data was sent
                 ************************************/
                //double bandwidth_MBps = (fileSizeInSamples *  4.0) / (stopwatch.ElapsedMilliseconds / 1000.0) / 1024 / 1024;
                //double bandwidth_Mbps = (soundFileSizeInSamples * 32.0) / (stopwatch.ElapsedMilliseconds / 1000.0) / 1024 / 1024;
                //Console.WriteLine("Ticks: " + stopwatch.ElapsedTicks);
                //Console.WriteLine("Elapsed time: " + stopwatch.ElapsedMilliseconds + " ms");
                //Console.WriteLine("Bandwidth: " + bandwidth_MBps.ToString("0.000") + " MB/s");
                //Console.WriteLine("Bandwidth: " + bandwidth_Mbps.ToString("0.0") + " Mb/s");

            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine((ec != ErrorCode.None ? ec + ":" : String.Empty) + ex.Message);
            }
            finally
            {
                if (MyUsbDevice != null) 
                {
                    if (MyUsbDevice.IsOpen)
                    {
                        // If this is a "whole" usb device (libusb-win32, linux libusb-1.0)
                        // it exposes an IUsbDevice interface. If not (WinUSB) the 
                        // 'wholeUsbDevice' variable will be null indicating this is 
                        // an interface of a device; it does not require or support 
                        // configuration and interface selection.
                        IUsbDevice wholeUsbDevice = MyUsbDevice as IUsbDevice;
                        if (!ReferenceEquals(wholeUsbDevice, null))
                        {
                            // Release interface #0.
                            wholeUsbDevice.ReleaseInterface(0);
                        }

                        MyUsbDevice.Close();
                    }
                    MyUsbDevice = null;

                    // Free usb resources
                    UsbDevice.Exit();

                }

                // Wait for user input..
                //Console.WriteLine("Done.");               
                //Console.ReadKey();
            }

            return 0;
        }
    }
}