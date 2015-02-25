using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;

namespace Detector
{
    class WavReader
    {
         unsafe struct fileheader
        {
            public fixed char sGroupID[4];       //Surprisingly enough, this is always "RIFF"
            public uint dwFileLength;   //File length in bytes, measured from offset 8
            public fixed char sRiffType[4];      //In wave files, this is always "WAVE"
        }
        unsafe struct fmtchunk
        {
            public fixed char sChunkID[4];        //Four bytes: "fmt "
            public uint dwChunkSize;     //Length of header in bytes
            public ushort wFormatTag;      //1 if uncompressed Microsoft PCM audio
            public ushort wChannels;       //Number of channels
            public uint dwSamplesPerSec; //Frequency of the audio in Hz
            public uint dwAvgBytesPerSec;//For estimating RAM allocation
            public ushort wBlockAlign;     //Sample frame size in bytes
            public uint dwBitsPerSample; //Bits per sample
        }
        unsafe struct datachunk
        {
            public fixed char sChunkID[4];       //Four bytes: "data"
            public uint dwChunkSize;    //Length of header in bytes
            //Different arrays for the different frame sizes
            //public fixed byte byteArray[8];     //8 bit unsigned data; or...
            public fixed short shortArray[16];    //16 bit signed data
        }

        static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        byte[] data;
        fileheader header;
        fmtchunk fmt;
        


        public WavReader()
        { }
        unsafe public void ReadFile(string filepath)
        {
            data = File.ReadAllBytes(filepath); //Load file into memory
            fixed (fileheader* pheader = &header) //Extract file header
            {

                char[] temp = new char[4];
                temp = Encoding.Default.GetString(data, 0, 4).ToCharArray();
                for(int i=0; i<4;i++)
                    pheader->sGroupID[i] = temp[i];
                
                header.dwFileLength = BitConverter.ToUInt32(data, 4);
               
                temp = Encoding.Default.GetString(data, 8, 4).ToCharArray();
                for (int i = 0; i < 4; i++)
                    pheader->sRiffType[i] = temp[i];
            }
            fixed (fmtchunk* pfmt = &fmt) //extract fmt header
            {
                char[] temp = Encoding.Default.GetString(data, 12, 4).ToCharArray();
                for (int i = 0; i < 4; i++)
                    pfmt->sChunkID[i] = temp[i];

                fmt.dwChunkSize = BitConverter.ToUInt32(data, 16);
                fmt.wFormatTag = BitConverter.ToUInt16(data, 20);
                fmt.wChannels = BitConverter.ToUInt16(data, 22);
                fmt.dwSamplesPerSec = BitConverter.ToUInt32(data, 24);
                fmt.dwAvgBytesPerSec = BitConverter.ToUInt32(data, 28);
                fmt.wBlockAlign = BitConverter.ToUInt16(data, 32);
                fmt.dwBitsPerSample = BitConverter.ToUInt16(data, 34);
            }

            if (header.dwFileLength > Int32.MaxValue) 
                throw new InvalidDataException("File too big to be analyzed!");

                int pointer = 36;
                int limit = data.Length;
                datachunk sound;
                BinaryFormatter formatter = new BinaryFormatter();
                MemoryStream ms = new MemoryStream(data);
                while (pointer < limit) //extract data
                {
                    string chunkid = Encoding.Default.GetString(data, pointer, 4);
                    string temp = Encoding.Default.GetString(data, 0, data.Length);
                    if (chunkid == "data")
                    {
                        datachunk chunk = new datachunk();
                        
                    
                    }
                
                
                }
                //formatter.Deserialize(ms);

                //MemoryStream mstream = new MemoryStream();
                //BinaryFormatter formatter = new BinaryFormatter();
                //mstream.Write(data, 0, data.Length);
                //mstream.Seek(16, SeekOrigin.Begin);
                //fmt = (fmtchunk)formatter.Deserialize(mstream);
        }
    }
}
