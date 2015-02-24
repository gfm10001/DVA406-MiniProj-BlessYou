using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;

namespace Detector
{
    class WavReader
    {
        struct fileheader
        {
            public string sGroupID;       //Surprisingly enough, this is always "RIFF"
            public uint dwFileLength;   //File length in bytes, measured from offset 8
            public string sRiffType;      //In wave files, this is always "WAVE"
        }
        struct fmtchunk
        {
            public string sChunkID;        //Four bytes: "fmt "
            public uint dwChunkSize;     //Length of header in bytes
            public ushort wFormatTag;      //1 if uncompressed Microsoft PCM audio
            public ushort wChannels;       //Number of channels
            public uint dwSamplesPerSec; //Frequency of the audio in Hz
            public uint dwAvgBytesPerSec;//For estimating RAM allocation
            public ushort wBlockAlign;     //Sample frame size in bytes
            public uint dwBitsPerSample; //Bits per sample
        }
        struct datachunk
        {
            public string sChunkID;       //Four bytes: "data"
            public uint dwChunkSize;    //Length of header in bytes
            //Different arrays for the different frame sizes
            public byte[] byteArray;     //8 bit unsigned data; or...
            public short[] shortArray;    //16 bit signed data
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
        public void ReadFile(string filepath)
        {
            data = File.ReadAllBytes(filepath);
            //BinaryReader br = new BinaryReader(File.Open(filepath, FileMode.Open));

            header.sGroupID = Encoding.Default.GetString(data,0,4);
            header.dwFileLength = BitConverter.ToUInt32(data, 4);
            //header.sRiffType = BitConverter.ToString(

            header.sRiffType = Encoding.Default.GetString(data,8,4);        
        }

    }
}
