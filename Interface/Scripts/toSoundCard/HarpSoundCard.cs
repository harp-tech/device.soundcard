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
                var toDirectory = System.IO.Path.Combine(currentDirectory, "toSoundCard");
                
                if (Directory.Exists(toDirectory) == false)
                    System.IO.Directory.CreateDirectory(toDirectory);

                #endregion

                /*************************************
                 * User input
                 ************************************/
                bool userInputOK = true;

                if (args.GetLength(0) == 6)
                {
                    if ((args[4].IndexOf("-m") == -1) && (args[4].IndexOf("-d") == -1))
                        userInputOK = false;
                }

                if (args.GetLength(0) == 8)
                {
                    if ((args[4].IndexOf("-m") == -1) && (args[6].IndexOf("-m") == -1))
                        userInputOK = false;
                    if ((args[4].IndexOf("-d") == -1) && (args[6].IndexOf("-d") == -1))
                        userInputOK = false;
                }

                if (args.GetLength(0) != 4 && args.GetLength(0) != 6 && args.GetLength(0) != 8)
                {
                    userInputOK = false;
                }

                if (userInputOK == false)
                {
                    Console.WriteLine("User input not correct.");
                    Console.WriteLine("The format should be one of the next options:");
                    Console.WriteLine("");
                    Console.WriteLine("  toSoundCard \"sound_filename\" [index] [type] [sample rate]");
                    Console.WriteLine("  toSOundCard \"sound_filename\" [index] [type] [sample rate] -metadata \"metadata_filename\"");
                    Console.WriteLine("  toSOundCard \"sound_filename\" [index] [type] [sample rate] -description \"description_filename\"");
                    Console.WriteLine("  toSOundCard \"sound_filename\" [index] [type] [sample rate] -metadata \"metadata_filename\" -description \"description_filename\"");
                    Console.WriteLine("");
                    Console.WriteLine("  -> [index]        from 0 to 31             -- 0 and 1 not implemented yet");
                    Console.WriteLine("  -> [type]         0: Int32, 1: Float32     -- Float32 not implemented yet");
                    Console.WriteLine("  -> [sample rate]  96000 or 192000");
                    Console.WriteLine("");
                    Console.WriteLine("  Note: It's recommended that \"filenames\" should have an extension.");
                    return (int) SoundCardErrorCode.BadUserInput;
                }

                //string fileName = "..\\..\\1920K_samples.bin";
                //fileName = "..\\..\\4800K_samples.bin";
                //fileName = "..\\..\\9600_samples.bin";
                //fileName = "..\\..\\96000_samples.bin";                

                string fileName = args[0];
                int soundIndex = Convert.ToInt32(args[1]);
                DataType dataType = (DataType)Convert.ToInt32(args[2]);
                SampleRate sampleRate = (SampleRate)Convert.ToInt32(args[3]);

                var userMetadataExists = ((args.GetLength(0) == 6) && (args[4].IndexOf("-m") != -1)) || (args.GetLength(0) == 8);
                string userMetadataFileName = string.Empty;

                if (userMetadataExists)
                {
                    if (args[4].IndexOf("-m") != -1)
                        userMetadataFileName = args[5];

                    if (args.GetLength(0) == 8)
                        if (args[6].IndexOf("-m") != -1)
                            userMetadataFileName = args[7];
                }

                var userDescriptionExists = ((args.GetLength(0) == 6) && (args[4].IndexOf("-d") != -1)) || (args.GetLength(0) == 8);
                string userDescriptionFileName = string.Empty;

                if (userDescriptionExists)
                {
                    if (args[4].IndexOf("-d") != -1)
                        userDescriptionFileName = args[5];

                    if (args.GetLength(0) == 8)
                        if (args[6].IndexOf("-d") != -1)
                            userDescriptionFileName = args[7];
                }

                //Console.WriteLine(args[0]);
                //Console.WriteLine(soundIndex);
                //Console.WriteLine(dataType);
                //Console.WriteLine(sampleRate);

                /*************************************
                 * Open file that contains the sound or fade
                 ************************************/
                long soundFileSizeInSamples = new FileInfo(fileName).Length / 4;
                var soundFileStream = new FileStream(fileName, FileMode.Open);
                int commandsToBeSent = (int)soundFileSizeInSamples * 4 / 32768 + (((((int)soundFileSizeInSamples * 4) % 32768) != 0) ? 1 : 0);

                if (soundFileStream == null) throw new Exception("NotAbleToOpenFile");
                if (soundFileSizeInSamples == 0) throw new Exception("FileIsEmpty");

                fileName = Path.GetFileName(fileName);
                if (fileName == string.Empty || fileName == null) throw new Exception("NotAbleToOpenFile");

                //Console.WriteLine("fileSizeInSamples: " + (int) fileSizeInSamples);
                //Console.WriteLine("commandsToBeSent: (" + (int)fileSizeInSamples * 4 / 32768.0 + ") " + commandsToBeSent);
                //Console.WriteLine("fileSizeInBytes: " + (int)fileSizeInSamples * 4);

                /*************************************
                 * Open files that contains the users' metadata and description
                 ************************************/
                long fileSize = 0;
                FileStream metadataFileStream = null;
                FileStream descriptionFileStream = null;

                if (userMetadataExists)
                {
                    fileSize = new FileInfo(userMetadataFileName).Length;
                    metadataFileStream = new FileStream(userMetadataFileName, FileMode.Open);

                    if (metadataFileStream == null) throw new Exception("NotAbleToOpenFile");
                    if (fileSize == 0) throw new Exception("NotAbleToOpenFile");

                    userMetadataFileName = Path.GetFileName(userMetadataFileName);
                    if (userMetadataFileName == string.Empty || userMetadataFileName == null) throw new Exception("NotAbleToOpenFile");

                }

                if (userDescriptionExists)
                {
                    fileSize = new FileInfo(userDescriptionFileName).Length;
                    descriptionFileStream = new FileStream(userDescriptionFileName, FileMode.Open);

                    if (descriptionFileStream == null) throw new Exception("NotAbleToOpenFile");
                    if (fileSize == 0) throw new Exception("NotAbleToOpenFile");

                    userDescriptionFileName = Path.GetFileName(userDescriptionFileName);
                    if (userDescriptionFileName == string.Empty || userDescriptionFileName == null) throw new Exception("NotAbleToOpenFile");

                }



                //Console.WriteLine("fileSizeInSamples: " + (int) fileSizeInSamples);
                //Console.WriteLine("commandsToBeSent: (" + (int)fileSizeInSamples * 4 / 32768.0 + ") " + commandsToBeSent);
                //Console.WriteLine("fileSizeInBytes: " + (int)fileSizeInSamples * 4);


                /*************************************
                 * Create user metadata byte array with 2048 bytes
                 ************************************/
                /* [0:169]     sound_filename               */
                /* [170:339]   metadata_filename            */
                /* [340:511]   description_filename         */
                /* [512:1535]  metadata_filename content    */
                /* [1536:2047] description_filename content */

                var userMetadata = new byte[2048];

                System.Buffer.BlockCopy(Encoding.ASCII.GetBytes(fileName), 0, userMetadata, 0, fileName.Length);

                if (userMetadataExists)
                {
                    System.Buffer.BlockCopy(Encoding.ASCII.GetBytes(userMetadataFileName), 0, userMetadata, 170, userMetadataFileName.Length);                    
                    metadataFileStream.Read(userMetadata, 512, 1024);
                }

                if (userDescriptionExists)
                {
                    System.Buffer.BlockCopy(Encoding.ASCII.GetBytes(userDescriptionFileName), 0, userMetadata, 340, userDescriptionFileName.Length);
                    descriptionFileStream.Read(userMetadata, 512 + 1024, 512);
                }

                /*************************************
                 * Build sound header
                 ************************************/
                SoundMetadata soundMetadata;
                soundMetadata.soundIndex = soundIndex;
                soundMetadata.soundLength = (int) soundFileSizeInSamples; // samples per sound 1048576;
                soundMetadata.sampleRate = sampleRate;
                soundMetadata.dataType = dataType;

                /*************************************
                 * Create byte arrays for commands
                 ************************************/
                /* Metadata command lenght: 'c' 'm' 'd' '0x80' + random + metadata  + 32768 + 2048 + 'f' */
                /* Data command lenght:     'c' 'm' 'd' '0x81' + random + dataIndex + 32768 + 'f'        */
                /* Reset command lenght:    'c' 'm' 'd' '0x88' + 'f'                                     */
                var metadataCmd = new byte[4 + sizeof(int) + soundMetadata.GetSize() + 32768 + 2048 + 1];
                var dataCmd     = new byte[4 + sizeof(int) + sizeof(int) + 32768 + 1];
                var resetCmd    = new byte[5];

                int metadataCmdDataIndex = 4 + sizeof(int) + soundMetadata.GetSize();
                int dataCmdDataIndex     = 4 + sizeof(int) + sizeof(int);

                byte metadataCmdHeader = 0x80;
                byte dataCmdHeader     = 0x81;
                byte resetCmdHeader    = 0x88;

                /*************************************
                 * Create byte array to receive replies
                 ************************************/
                /* Metadata command reply: 'c' 'm' 'd' '0x80' + random + error */
                /* Data command reply:     'c' 'm' 'd' '0x81' + random + error */
                int commandReplyLenght = 4 + sizeof(int) + sizeof(int);
                byte[] commandReply = new byte[commandReplyLenght];

                /*************************************
                 * Prepare metadata command
                 ************************************/
                var soundMetadataByteArray = soundMetadata.ToByteArray();

                if (soundMetadata.CheckData() != SoundCardErrorCode.Ok)
                {
                    Console.WriteLine("Error: " + soundMetadata.CheckData());
                    return (int)soundMetadata.CheckData();
                }

                metadataCmd[0] = Convert.ToByte('c');
                metadataCmd[1] = Convert.ToByte('m');
                metadataCmd[2] = Convert.ToByte('d');
                metadataCmd[3] = metadataCmdHeader;
                System.Buffer.BlockCopy(soundMetadataByteArray, 0, metadataCmd, 8, soundMetadata.GetSize());
                System.Buffer.BlockCopy(userMetadata, 0, metadataCmd, 8 + soundMetadata.GetSize() + 32768, userMetadata.Length);
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
                 * Start stopwatch
                 ************************************/
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                /*
                while (stopwatch.ElapsedMilliseconds < 1250)    // A Warmup of 1000-1500 mS 
                {                                               // stabilizes the CPU cache and pipeline.
                    // Run the algorithm you want to test
                }

                stopwatch.Reset();
                stopwatch.Start();
                */

                /*************************************
                 * Send metadata command and receive reply
                 ************************************/
                int bytesSent;
                int bytesRead;
                int randomReceived;
                int errorReceived;

                int writeTimeout = 2000;
                int readTimeout = 2000;

                Random randomInt = new Random();

                int randomSent = randomInt.Next();
                System.Buffer.BlockCopy(BitConverter.GetBytes(randomSent), 0, metadataCmd, 4, sizeof(int));

                soundFileStream.Read(metadataCmd, metadataCmdDataIndex, 32768);

                reader.Flush();                

                ec = writer.Write(metadataCmd, 0, metadataCmd.Length, writeTimeout, out bytesSent);
                if (ec != ErrorCode.None) throw new Exception("NotAbleToSendMetadata");
                //Console.WriteLine("Sound metadata sent (" + bytesSent + " bytes sent). Random sent: " + randomSent);

                ec = reader.Read(commandReply, readTimeout, out bytesRead);
                if (ec != ErrorCode.None) throw new Exception("NotAbleToReadMetadataCommandReply");

                randomReceived = BitConverter.ToInt32(commandReply, 4);
                errorReceived = BitConverter.ToInt32(commandReply, 8);
                //Console.WriteLine("Sound metadata reply received (" + bytesRead + " bytes read). Error received: " + errorReceived + " Random received: " + randomReceived);

                if (randomSent != randomReceived) throw new Exception("MetadataCommandReplyNotCorrect");
                for (int i = 0; i < 8; i++)
                    if (metadataCmd[i] != commandReply[i]) throw new Exception("MetadataCommandReplyNotCorrect");

                if ((SoundCardErrorCode) errorReceived != SoundCardErrorCode.Ok)
                {
                    Console.WriteLine("Error: " + (SoundCardErrorCode) errorReceived);
                    return (int)soundMetadata.CheckData();
                }

                /*************************************
                 * Send data commands and receive replies
                 ************************************/
                int dataIndex = 0;
                int bytesFromFile;
                while (--commandsToBeSent > 0)
                {
                    randomSent = randomInt.Next();
                    System.Buffer.BlockCopy(BitConverter.GetBytes(randomSent), 0, dataCmd, 4, sizeof(int));

                    System.Buffer.BlockCopy(BitConverter.GetBytes(++dataIndex), 0, dataCmd, 8, sizeof(int));

                    bytesFromFile = soundFileStream.Read(dataCmd, dataCmdDataIndex, 32768);

                    /* NOT NEEDED -- THE DEVICE MUST HANDLE THIS
                    if (bytesFromFile != 32768)
                    {
                        // This code was not tested yet
                        for (int i = 0; i < 32768 - bytesFromFile; i++)
                            dataCmd[dataCmdDataIndex + 32768 - bytesFromFile + i] = 0;
                    }
                    */

                    ec = writer.Write(dataCmd, 0, dataCmd.Length, writeTimeout, out bytesSent);
                    if (ec != ErrorCode.None) throw new Exception("NotAbleToSendData");
                    //Console.WriteLine("Sound data sent (" + bytesSent + " bytes sent). Random sent: " + randomSent);

                    ec = reader.Read(commandReply, readTimeout, out bytesRead);
                    if (ec != ErrorCode.None) throw new Exception("NotAbleToReadDataCommandReply");

                    randomReceived = BitConverter.ToInt32(commandReply, 4);
                    errorReceived = BitConverter.ToInt32(commandReply, 8);
                    //Console.WriteLine("Sound metadata reply received (" + bytesRead + " bytes read). Error received: " + errorReceived + " Random received: " + randomReceived);

                    if (randomSent != randomReceived) throw new Exception("DataCommandReplyNotCorrect");
                    for (int i = 0; i < 8; i++)
                        if (dataCmd[i] != commandReply[i]) throw new Exception("DataCommandReplyNotCorrect");

                    if ((SoundCardErrorCode)errorReceived != SoundCardErrorCode.Ok)
                    {
                        Console.WriteLine("Error: " + (SoundCardErrorCode)errorReceived);
                        return (int)soundMetadata.CheckData();
                    }
                }

                /*************************************
                 * Send reset command
                 ************************************/
                ec = writer.Write(resetCmd, 0, resetCmd.Length, writeTimeout, out bytesSent);
                if (ec != ErrorCode.None) throw new Exception("NotAbleToSendData");
                //Console.WriteLine("Sound data sent (" + bytesSent + " bytes sent). Random sent: " + randomSent);

                // ADD DELAY TO ACCOMODATE THE PIC32 BOOT???

                /*************************************
                 * All data was sent
                 ************************************/
                //double bandwidth_MBps = (fileSizeInSamples *  4.0) / (stopwatch.ElapsedMilliseconds / 1000.0) / 1024 / 1024;
                double bandwidth_Mbps = (soundFileSizeInSamples * 32.0) / (stopwatch.ElapsedMilliseconds / 1000.0) / 1024 / 1024;
                //Console.WriteLine("Ticks: " + stopwatch.ElapsedTicks);
                Console.WriteLine("Elapsed time: " + stopwatch.ElapsedMilliseconds + " ms");
                //Console.WriteLine("Bandwidth: " + bandwidth_MBps.ToString("0.000") + " MB/s");
                Console.WriteLine("Bandwidth: " + bandwidth_Mbps.ToString("0.0") + " Mb/s");                
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