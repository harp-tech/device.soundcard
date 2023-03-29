using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace HarpSoundCard
{
    public enum SampleRate : int
    {
        _96000Hz = 96000,
        _192000Hz = 192000
    }

    public enum DataType : int
    {
        Int32 = 0,
        Float32 = 1
    }

    public struct SoundMetadata
    {
        public int soundIndex;
        public int soundLength;
        public SampleRate sampleRate;
        public DataType dataType;

        public byte[] ToByteArray()
        {
            // Note: "this" is the SoundHeader variable

            int sizeOfStruture = Marshal.SizeOf(this);          // Get structure size
            byte[] arr = new byte[sizeOfStruture];              // Create array of bytes with structure size

            IntPtr ptr = Marshal.AllocHGlobal(sizeOfStruture);  // Initialize unmanged memory to hold the struct
            Marshal.StructureToPtr(this, ptr, true);            // Copy the struct to unmanaged memory

            Marshal.Copy(ptr, arr, 0, sizeOfStruture);          // Copies data from an unmanaged memory pointer to
                                                                // a managed 8-bit unsigned integer array

            Marshal.FreeHGlobal(ptr);                           // Frees memory previously allocated from the
                                                                // unmanaged memory of the process

            return arr;
        }
        
        public void ToStructure(byte[] bytearray)
        {
            int sizeOfStruture = Marshal.SizeOf(this);

            IntPtr ptr = Marshal.AllocHGlobal(sizeOfStruture);

            Marshal.Copy(bytearray, 0, ptr, sizeOfStruture);            
            this = (SoundMetadata)Marshal.PtrToStructure(ptr, typeof(SoundMetadata));

            Marshal.FreeHGlobal(ptr);
        }

        public int GetSize()
        {
            return Marshal.SizeOf(this);
        }

        public SoundCardErrorCode CheckData()
        {
            if (this.soundIndex < 2 || this.soundIndex > 32)
            {
                //Console.WriteLine("Sound index is too big. Must be beween 0 and 32.");
                return SoundCardErrorCode.BadSoundIndex;
            }

            if (this.soundLength < 16)
            {
                return SoundCardErrorCode.BadSoundLength;
            }

            if (this.sampleRate != SampleRate._96000Hz && this.sampleRate != SampleRate._192000Hz)
            {
                //Console.WriteLine("The sample rate is not correct. Available options are 96000 and 192000.");
                return SoundCardErrorCode.BadSampleRate;
            }

            if (this.dataType != DataType.Int32 && this.dataType != DataType.Float32)
            {
                //Console.WriteLine("The sample rate is not correct. Available options are 0 (for integer with 32 bits) and 1 (for float with 32 bits).");
                return SoundCardErrorCode.BadDataType;
            }

            if (this.soundIndex == 0 && this.dataType != DataType.Float32)
            {
                //Console.WriteLine("Data type of sound index 0 must be float.");
                return SoundCardErrorCode.DataTypeDoNotMatch;
            }

            if (this.soundIndex == 1 && this.dataType != DataType.Float32)
            {
                //Console.WriteLine("Data type of sound index 1 must be float.");
                return SoundCardErrorCode.DataTypeDoNotMatch;
            }

            if (this.soundIndex > 1 && this.dataType != DataType.Int32)
            {
                //Console.WriteLine("Data type of sound index above 1 must be integer.");
                return SoundCardErrorCode.DataTypeDoNotMatch;
            }

            return 0;

        }
    }

    
}
