using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;
using System.Media;

namespace Detector
{
    class WavFile
    {
         unsafe public struct fileheader
        {
            public fixed byte sGroupID[4];       //Surprisingly enough, this is always "RIFF"
            public uint dwFileLength;   //File length in bytes, measured from offset 8
            public fixed byte sRiffType[4];      //In wave files, this is always "WAVE"
        }
        unsafe public struct fmtchunk
        {
            public fixed byte sChunkID[4];        //Four bytes: "fmt "
            public uint dwChunkSize;     //Length of header in bytes
            public ushort wFormatTag;      //1 if uncompressed Microsoft PCM audio
            public ushort wChannels;       //Number of channels
            public uint dwSamplesPerSec; //Frequency of the audio in Hz
            public uint dwAvgBytesPerSec;//For estimating RAM allocation
            public ushort wBlockAlign;     //Sample frame size in bytes
            public ushort dwBitsPerSample; //Bits per sample
        }
        unsafe public struct datachunk
        {
            public fixed byte sChunkID[4];       //Four bytes: "data"
            public uint dwChunkSize;    //Length of header in bytes
            //Different arrays for the different frame sizes
            //public fixed byte byteArray[8];     //8 bit unsigned data; or...
            //public fixed short shortArray[16];    //16 bit signed data
        }


        byte[] filedata;
        string _filepath;
        fileheader _header;
        fmtchunk _fmt;
        datachunk _dataheader;
        int[] _rawdata;

        #region Properties
        public int[] Data
        {
            get
            {
                return _rawdata;
            }
        
        }
        public string FilePath
        {
            get { return _filepath; }
        
        }
        public fmtchunk FMTHeader
        {
            get { return _fmt; }
        }
        public  fileheader FILEHeader
        {
            get { return _header; }
        }

        public datachunk DATAHeader
        {
            get { return _dataheader; }
        }

        public WavFile(string path)
        {
            _filepath = path;
            _readFile(path);
        }
        #endregion
        public static WavFile LoadFile(string path)
        {
            return new WavFile(path);
        }
        static int[] GetSingleChannelData(WavFile File)
        {
            if (File.FMTHeader.wChannels == 2) //If we got 2 channels, normalize to one
            {
                int[] _rawdata = File.Data;
                int[] avg = new int[File.Data.Length / 2];
                for (int i = 0; i < avg.Length; i += 2)
                {
                    avg[i] = (_rawdata[i * 2] + _rawdata[i * 2 + 2]) / 2;
                    avg[i + 1] = (_rawdata[i * 2 + 1] + _rawdata[i * 2 + 3]) / 2;
                }
                return avg;

            }
            return File.Data;
        
        }

        unsafe byte[] _GenerateFileHeader(fileheader h, fmtchunk fmt, datachunk d)
        {
           // fmt.wChannels = 1;

            int size = Marshal.SizeOf(h); //12
            
            //size = sizeof(char);

            byte[] harr = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(h, ptr, true);
            Marshal.Copy(ptr, harr, 0, size);
            Marshal.FreeHGlobal(ptr);

            size = Marshal.SizeOf(fmt); //28
            byte[] farr = new byte[size];
            ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(ptr, farr, 0, size);
            Marshal.FreeHGlobal(ptr);

            size = Marshal.SizeOf(d);
            byte[] darr = new byte[size];
            ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(ptr, darr, 0, size);
            Marshal.FreeHGlobal(ptr);

            byte[] retval = new byte[harr.Length + farr.Length + darr.Length];
            harr.CopyTo(retval, 0);
            farr.CopyTo(retval, harr.Length);
            darr.CopyTo(retval, harr.Length + farr.Length);

            int errors = 0;
            for (int i = 0; i < 44; i++)
            {
                if (retval[i] != filedata[i])
                    errors++;

            }
            

            return retval;
        }


        unsafe private void _readFile(string filepath)
        {
            if (File.Exists(filepath) == false)
                throw new InvalidDataException("File can not be found!");
            filedata = File.ReadAllBytes(filepath); //Load file into memory
            fixed (fileheader* pheader = &_header) //Extract file header
            {

                //byte[] temp = new byte[4];
                //temp = Encoding.Default.GetString(filedata, 0, 4).ToCharArray();
                for(int i=0; i<4;i++)
                    pheader->sGroupID[i] = filedata[i];
                
                _header.dwFileLength = BitConverter.ToUInt32(filedata, 4);
               
                //temp = Encoding.Default.GetString(filedata, 8, 4).ToCharArray();
                for (int i = 0; i < 4; i++)
                    pheader->sRiffType[i] = filedata[i+8];
            }
            fixed (fmtchunk* pfmt = &_fmt) //extract fmt header
            {
                //char[] temp = Encoding.Default.GetString(filedata, 12, 4).ToCharArray();


                for (int i = 0; i < 4; i++)
                    pfmt->sChunkID[i] = filedata[12 + i];

                _fmt.dwChunkSize = BitConverter.ToUInt32(filedata, 16);
                _fmt.wFormatTag = BitConverter.ToUInt16(filedata, 20);
                _fmt.wChannels = BitConverter.ToUInt16(filedata, 22);
                _fmt.dwSamplesPerSec = BitConverter.ToUInt32(filedata, 24);
                _fmt.dwAvgBytesPerSec = BitConverter.ToUInt32(filedata, 28);
                _fmt.wBlockAlign = BitConverter.ToUInt16(filedata, 32);
                _fmt.dwBitsPerSample = BitConverter.ToUInt16(filedata, 34);
            }
            fixed (datachunk* d = &_dataheader)
            {
                //char[] temp = Encoding.Default.GetString(filedata, 36, 4).ToCharArray();
                
                for (int i = 0; i < 4; i++)
                    d->sChunkID[i] = filedata[i+36];
                d->dwChunkSize = BitConverter.ToUInt32(filedata, 40);
            }

            if (_header.dwFileLength > Int32.MaxValue) 
                throw new InvalidDataException("File too big to be analyzed!");

                int pointer = 44;
                int limit = filedata.Length;
                _rawdata = new int[filedata.Length - 44 / 4];

                 int index =0;

                while (pointer < limit) //extract data
                {
                    _rawdata[index]=BitConverter.ToInt16(filedata,pointer);
                    pointer += 4;
                    index++;       
                }

                byte[] test = _GenerateFileHeader(_header, _fmt, _dataheader);




                _rawdata = WavFile.GetSingleChannelData(this);
     
                
                //formatter.Deserialize(ms);

                //MemoryStream mstream = new MemoryStream();
                //BinaryFormatter formatter = new BinaryFormatter();
                //mstream.Write(data, 0, data.Length);
                //mstream.Seek(16, SeekOrigin.Begin);
                //fmt = (fmtchunk)formatter.Deserialize(mstream);
        }
    }
}
