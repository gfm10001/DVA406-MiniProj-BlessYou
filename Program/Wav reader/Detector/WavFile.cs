﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;
using System.Media;
using NAudio;

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
        double[] _truedata;

        #region Properties
        public double[] Data
        {
            get
            {
                return _truedata;
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

        static double[] Normalize(int[] data, int limit)
        {
            int hightest = 0;
            int maxat = 0;
            for (int i = 0; i < data.Length; i++)
            {
                if (hightest < Math.Abs(data[i]))
                {
                    hightest = Math.Abs(data[i]);
                    maxat = i;
                }
            }
            double mod = Math.Abs((double)limit / (double)hightest);
            double[] retval = new double[data.Length];
            for (int i = 0; i < retval.Length; i++)
            {
                retval[i] = data[i] * mod;
            }

            


            return retval;
        
        }

        static int[] GetSingleChannelData(WavFile file)
        {

            NAudio.Wave.WaveFormat format = new NAudio.Wave.WaveFormat(44100, 16, 1);
            NAudio.Wave.WaveStream stream = new NAudio.Wave.WaveFileReader(new MemoryStream(file.filedata));
            NAudio.Wave.WaveFormatConversionStream str = new NAudio.Wave.WaveFormatConversionStream(format, stream);
            //NAudio.Wave.WaveFileWriter.CreateWaveFile("TEST.wav", str);

            BinaryReader br = new BinaryReader(str);
            byte[] temp = br.ReadBytes((int)str.Length);


            int[] retval = new int[str.Length];

            for (int i = 0, z = 0; i < temp.Length; i += 2, z++)
            {
                retval[z] = BitConverter.ToInt16(temp, i);
            }

            return retval;
        }

        static unsafe byte[] _GenerateFileHeader(fileheader h, fmtchunk fmt, datachunk d)
        {
           // fmt.wChannels = 1;
            fileheader nh = h;
            fmtchunk nfmt = fmt;
            datachunk nd = d;

            int size = Marshal.SizeOf(nh); //12

            fmt.wChannels = 1;

            byte[] harr = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(nh, ptr, true);
            Marshal.Copy(ptr, harr, 0, size);
            Marshal.FreeHGlobal(ptr);

            size = Marshal.SizeOf(nfmt); //28
            byte[] farr = new byte[size];
            ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(nfmt, ptr, true);
            Marshal.Copy(ptr, farr, 0, size);
            Marshal.FreeHGlobal(ptr);

            size = Marshal.SizeOf(nd);
            byte[] darr = new byte[size];
            ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(nd, ptr, true);
            Marshal.Copy(ptr, darr, 0, size);
            Marshal.FreeHGlobal(ptr);

            string test = Encoding.Default.GetString(farr, 0, 4);

            byte[] retval = new byte[harr.Length + farr.Length + darr.Length];

            harr.CopyTo(retval, 0);
            farr.CopyTo(retval, harr.Length);
            darr.CopyTo(retval, harr.Length + farr.Length);


            //If data needs to be evaluated, use this code
            //int errors = 0;
            /*
            for (int i = 0; i < 44; i++)
            {
                if (retval[i] != filedata[i])
                    throw new InvalidDataException("Error detected when copying header!");

            }
             * */

            return retval;
        }


        unsafe private void _readFile(string filepath)
        {
            if (File.Exists(filepath) == false)
                throw new InvalidDataException("File can not be found!");
            filedata = File.ReadAllBytes(filepath); //Load file into memory
            fixed (fileheader* pheader = &_header) //Extract file header
            {

                for(int i=0; i<4;i++)
                    pheader->sGroupID[i] = filedata[i];
                
                _header.dwFileLength = BitConverter.ToUInt32(filedata, 4);
               
                for (int i = 0; i < 4; i++)
                    pheader->sRiffType[i] = filedata[i+8];
            }
            fixed (fmtchunk* pfmt = &_fmt) //extract fmt header
            {
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
                
                for (int i = 0; i < 4; i++)
                    d->sChunkID[i] = filedata[i+36];
                d->dwChunkSize = BitConverter.ToUInt32(filedata, 40);
            }

            if (_header.dwFileLength > Int32.MaxValue) 
                throw new InvalidDataException("File too big to be analyzed!");

                int pointer = 44;
                int limit = filedata.Length;
                _rawdata = new int[filedata.Length - 44];

                 int index =0;

                while (pointer < limit) //extract data
                {
                    _rawdata[index]=BitConverter.ToInt16(filedata,pointer);
                    pointer += 4;
                    index++;       
                }

                _rawdata = WavFile.GetSingleChannelData(this);
                double[] truedata = WavFile.Normalize(_rawdata,10000);
        }
    }
}
