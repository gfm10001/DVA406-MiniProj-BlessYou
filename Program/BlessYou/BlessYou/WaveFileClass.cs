﻿// WaveFileClass.cs
//
// DVA406 Intelligent Systems, MdH, vt15
//
// History:
// 2015-02-24       Introduced.
// 2015-03-08/GF    Added: DumpWaveFileContents, moved Normalise o this module.
// 2015-03-09/GF    AnalyseWaveFileContents: back off after found trigg ("prefetch").
// 2015-03-10/GF    DumpWaveFileIntervalContents: handle prefetch

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BlessYou
{


    public class WaveFileClass
    {
        bool FDoWaveDump = true;
        string FWaveFileName;
        double[] FWaveFileContents44p1KHz16bitSamples;
        int FStartOfFirstIntervalIx;
        int FNrOfIntevals;
        int FIntervalSampleCount;

        // ====================================================================

        public WaveFileClass()
        {
            FNrOfIntevals = ConfigurationStatClass.C_NR_OF_INTERVALS;
        } // WaveFileClass

        // ====================================================================

        public void ReadWaveFile(string i_WaveFileName)
        {

            // 1. Öppna i_WaveFileName
            // 2. Analyser antal kanaler (1/2)
            // 3. Läs data 
            //   för varje 16bit sample:
            //      Mono:   läs integer 16 bit och placera som double i FWaveFileContents 
            //      Stereo: läs 2 (L, R)  integer 16 bit, konvertera till double och medelvärdesbilda, placera som double i FWaveFileContents44p1KHz16bitSamples 
            // 4. Utvärdera ev. fel kasta exception om fel

            FWaveFileName = i_WaveFileName;
            WavFile _wavFile = new WavFile(FWaveFileName);

            FWaveFileContents44p1KHz16bitSamples = new double[_wavFile.RawData.Length];
            for (int ix = 0; ix < _wavFile.RawData.Length; ++ix)
            {
                FWaveFileContents44p1KHz16bitSamples[ix] = (double) _wavFile.RawData[ix];
            } // for ix

            DumpWaveFileContents("Raw", 0, FWaveFileContents44p1KHz16bitSamples.Length);

        } // ReadWaveFile

        // ====================================================================

        public void NormalizeWaveFileContents()
        {
            //double scaleFactor;
            //double hightest;
            //_wavFile.Normalize(C_MAX_POSSIBLE_VALUE);

            // 1. Leta upp absoluta max värdet (hightest)
            // 2: Calculate: scalefactor = C_MAX_POSSIBLE_VALUE / hightest;
            // 2. Skala alla värden: 
            //      i = [0, FWaveFileContents44p1KHz16bitSamples.Count - 1]
            //      FWaveFileContents44p1KHz16bitSamples[i] = FWaveFileContents44p1KHz16bitSamples[i] * scalefactor

            double hightest = 0;

            for (int i = 0; i < FWaveFileContents44p1KHz16bitSamples.Length; i++)
            {
                if (hightest < Math.Abs(FWaveFileContents44p1KHz16bitSamples[i]))
                {
                    hightest = Math.Abs(FWaveFileContents44p1KHz16bitSamples[i]);
                }
            }

            double scalefactor = Math.Abs(ConfigurationStatClass.C_MAX_POSSIBLE_VALUE / hightest);
            for (int i = 0; i < FWaveFileContents44p1KHz16bitSamples.Length; i++)
            {
                FWaveFileContents44p1KHz16bitSamples[i] = FWaveFileContents44p1KHz16bitSamples[i] * scalefactor;
            }

            DumpWaveFileContents("Normalized", 0, FWaveFileContents44p1KHz16bitSamples.Length);

        } // NormalizeWaveFileContents


        // ====================================================================

        public void AnalyseWaveFileContents()
        {
            double triggerOnLevel = (ConfigurationStatClass.C_TRIGGER_LEVEL_IN_PERCENT / 100.0) * ConfigurationStatClass.C_MAX_POSSIBLE_VALUE;
            double triggerOffLevel = (ConfigurationStatClass.C_TRIGGER_OFF_LEVEL_IN_PERCENT / 100.0) * ConfigurationStatClass.C_MAX_POSSIBLE_VALUE;
            int sampleCountOfTriggerOffDuration = (int)Math.Round(ConfigurationStatClass.C_TRIGGER_OFF_DURATION_IN_MILLI_SECS * ConfigurationStatClass.C_SOUND_SAMPLE_FREQUENCY_IN_kHz);
            int triggerOffIx;
            int nrOfSamplesBelowTriggerOff;



            // 1. Analyze sample data and calculate 
            // 2. Find ix of first sample with an absolute level higher than the triggerLevel
            //    FStartOfFirstIntervalIx = ix - ix(C_TRIGGER_PREFETCH_IN_MILLI_SECS)
            // 3. FNrOfIntevals; DONE
            // 4. FIntervalSampleCount = (FWaveFileContents44p1KHz16bitSamples.Count - FStartOfFirstIntervalIx) / FNrOfIntevals

            FStartOfFirstIntervalIx = 0;
            triggerOffIx = 0;
            for (int ix = 0; ix < FWaveFileContents44p1KHz16bitSamples.Length; ++ix)
            {
                // ToDo filter with more samples?
                if (Math.Abs(FWaveFileContents44p1KHz16bitSamples[ix]) > triggerOnLevel)
                {
                    FStartOfFirstIntervalIx = ix - (int)(ConfigurationStatClass.C_TRIGGER_PREFETCH_IN_MILLI_SECS * ConfigurationStatClass.C_SOUND_SAMPLE_FREQUENCY_IN_kHz);
                    if (FStartOfFirstIntervalIx < 0)
                    {
                        FStartOfFirstIntervalIx = 0;
                    }
                    break;
                } // if
            } // for ix;


            nrOfSamplesBelowTriggerOff = 0;
            triggerOffIx = FStartOfFirstIntervalIx + 1;
            for (int ix = FStartOfFirstIntervalIx + 1; ix < FWaveFileContents44p1KHz16bitSamples.Length; ++ix)
            {
                nrOfSamplesBelowTriggerOff++;
                if (Math.Abs(FWaveFileContents44p1KHz16bitSamples[ix]) > triggerOffLevel)
                {
                    nrOfSamplesBelowTriggerOff = 0;
                    triggerOffIx = ix;
                    continue;
                } // if

                if (nrOfSamplesBelowTriggerOff > sampleCountOfTriggerOffDuration)
                {
                    break;
                }
            } // for ix;

            // Calculate intevall length
            FIntervalSampleCount = (triggerOffIx - FStartOfFirstIntervalIx) / FNrOfIntevals;

            Console.WriteLine("{0,-40} - Tot: {1, 6:0}ms triggOn: {2, 6:0}ms triggOff: {3, 6:0}ms Int: {4, 6:0}ms = {5, 6:0}%",
                              System.IO.Path.GetFileName(FWaveFileName), FWaveFileContents44p1KHz16bitSamples.Length / ConfigurationStatClass.C_SOUND_SAMPLE_FREQUENCY_IN_kHz,
                              FStartOfFirstIntervalIx / ConfigurationStatClass.C_SOUND_SAMPLE_FREQUENCY_IN_kHz,
                              triggerOffIx / ConfigurationStatClass.C_SOUND_SAMPLE_FREQUENCY_IN_kHz,
                              FIntervalSampleCount / ConfigurationStatClass.C_SOUND_SAMPLE_FREQUENCY_IN_kHz,
                              100.00 * FIntervalSampleCount * ConfigurationStatClass.C_NR_OF_INTERVALS / FWaveFileContents44p1KHz16bitSamples.Length);

            //// Dump each interval as a seperate file.
            //for (int ix = 0; ix < ConfigurationStatClass.C_NR_OF_INTERVALS; ++ix)
            //{
            //    DumpWaveFileContents("Interval_" + ix.ToString(), FStartOfFirstIntervalIx + ix * FIntervalSampleCount, FStartOfFirstIntervalIx + (ix + 1) * FIntervalSampleCount - 1);
            //} // for ix

            // DumpWaveFileContents split into intervalls.
            DumpWaveFileIntervalContents(FStartOfFirstIntervalIx, FIntervalSampleCount);
        
        } // AnalyseWaveFileContents

        // ====================================================================

        public void CalculateFeatureVector(FeatureBaseClass i_FeatureObj)
        {

            // 1. calculate featurevector
            // 2. Type of feature depends on i_FeatureObj
            // Console.WriteLine("Feature: {0}", i_FeatureObj.FeatureName); // ToDo
            int soundSampleIx;


            soundSampleIx = FStartOfFirstIntervalIx;
            for (int intervalIx = 0; intervalIx < FNrOfIntevals; ++intervalIx)
            {
                i_FeatureObj.calculateFeatureValuesFromSamples(FWaveFileContents44p1KHz16bitSamples, soundSampleIx, FIntervalSampleCount);
                soundSampleIx = soundSampleIx + FIntervalSampleCount;
            } // for intervalIx


        } // CalculateFeatureVector

        // ====================================================================

        public void DumpWaveFileContents(string i_FileNameModifier, int i_BegIx, int i_EndIx)
        {
            // Dump wave file contents as x, y pairs for use in Excel.

            int soundSampleIx;
            double x;
            double y;
            string usedFileName;
            int theUsedEndIx = i_EndIx;
            int zeroFlag;

            if (!FDoWaveDump)
            {
                return;
            }

            // ToDo: - only use a part at debug - or too many samples!
            if (theUsedEndIx > 1000000)
            {
                theUsedEndIx = 1000000; // Max  1,048,576 rows in Excel!
            }

            string[] lineArr = new string[theUsedEndIx - i_BegIx + 2];

            usedFileName = System.IO.Path.GetFileNameWithoutExtension(FWaveFileName) + "_" + i_FileNameModifier + ".xls";
            Console.WriteLine("Dumping: " + usedFileName, ", from " + i_BegIx + " to " + i_EndIx);

            // Scale Excel byte adding two samples with min/max
            y = -100000;
            x = -2;
            lineArr[0] = x.ToString() + " \t " + y.ToString() + "\t" + i_FileNameModifier + "\t" + i_BegIx + "\t" + i_EndIx;

            y = 100000;
            x = -1;
            lineArr[1] = x.ToString() + " \t " + y.ToString();

            // Convert the samples to text lines
            for (soundSampleIx = i_BegIx; soundSampleIx < theUsedEndIx; ++soundSampleIx)
            {
                x = soundSampleIx - i_BegIx;        
                y = FWaveFileContents44p1KHz16bitSamples[soundSampleIx];
                zeroFlag = 0;
                if (0 == y)
                {
                    zeroFlag = 10000;
                }
                lineArr[soundSampleIx - i_BegIx + 2] = x.ToString() + " \t " + y.ToString() + "\t" + zeroFlag;

                if (soundSampleIx % 1000000 == 0)
                {
                    Console.WriteLine("sample {0}", soundSampleIx);
                }
            } // for soundSampleIx


            try
            {
                Console.WriteLine("saveing: " + usedFileName);
                System.IO.File.WriteAllLines(usedFileName, lineArr);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("DumpWaveFileContents - ERR: " + ex.Message);
            }

        } // DumpWaveFileContents

        // ====================================================================

        public void DumpWaveFileIntervalContents(int i_StartIx, int i_IntervalLengthInSamples)
        {
            // Dump wave file contents as x, y, interval-nr triplets for use in Excel.
            // From Excel:
            //  Excel cannot exceed the limit of 1,048,576 rows and 16,384 columns.
            // interval-nr  = 0 before start
            //              = <0 and incremented during prefetch (in C_TRIGGER_PREFETCH_IN_MILLI_SECS)
            //              = >0 and incremented at each interval at trig

            int soundSampleIx;
            double x;
            double y;
            string usedFileName;
            int theUsedEndIx = FWaveFileContents44p1KHz16bitSamples.Length;
            int intervalIx;
            string fileNameModifier = "Intervals";
            int realTriggIx = i_StartIx + (int) (ConfigurationStatClass.C_TRIGGER_PREFETCH_IN_MILLI_SECS * ConfigurationStatClass.C_SOUND_SAMPLE_FREQUENCY_IN_kHz);

            if (!FDoWaveDump)
            {
                return;
            }

            // ToDo: - only use a part at debug - or too many samples!
            if (theUsedEndIx > 1000000)
            {
                theUsedEndIx = 1000000; // Max  1,048,576 rows in Excel!
            }

            string[] lineArr = new string[theUsedEndIx + 2];

            usedFileName = System.IO.Path.GetFileNameWithoutExtension(FWaveFileName) + "_" + fileNameModifier + ".xls";
            Console.WriteLine("Dumping: " + usedFileName, ", from " + i_StartIx + " to " + theUsedEndIx);

            // Scale Excel byte adding two samples with min/max
            y = -100000;
            x = -2;
            lineArr[0] = x.ToString() + " \t " + y.ToString() + "\t0\t\t" + usedFileName + "\t" + fileNameModifier + "\t" + i_StartIx + "\t" + theUsedEndIx;
            y = 100000;
            x = -1;
            lineArr[1] = x.ToString() + " \t " + y.ToString() + "\t0\t\t" + ConfigurationStatClass.C_TRIGGER_PREFETCH_IN_MILLI_SECS * ConfigurationStatClass.C_SOUND_SAMPLE_FREQUENCY_IN_kHz;
            
            // Convert the samples to text lines
            for (soundSampleIx = 0; soundSampleIx < theUsedEndIx; ++soundSampleIx)
            {
                x = soundSampleIx;        
                y = FWaveFileContents44p1KHz16bitSamples[soundSampleIx];
                intervalIx = 0;
                if (soundSampleIx > i_StartIx)
                {
                    intervalIx = 1 + (int)((soundSampleIx - i_StartIx) / i_IntervalLengthInSamples);

                    if (soundSampleIx < realTriggIx)
                    {
                        intervalIx = -intervalIx;
                    }
                    else
                    {
                        if (intervalIx < 0)
                        {
                            intervalIx = 0;
                        }
                        
                    }
                }
                if (intervalIx > 10)
                {
                    intervalIx = 0;
                }
                intervalIx = intervalIx * 10000;
                lineArr[soundSampleIx + 2] = x.ToString() + " \t " + y.ToString() + " \t " + intervalIx.ToString(); ;
 
                if (soundSampleIx % 1000000 == 0)
                {
                    Console.WriteLine("sample {0}", soundSampleIx);
                }
            } // for soundSampleIx


            try
            {
                Console.WriteLine("saving: " + usedFileName);
                System.IO.File.WriteAllLines(usedFileName, lineArr);

            }
            catch (Exception ex)
            {
                Console.WriteLine("DumpWaveFileIntervalContents - ERR: " + ex.Message);
            }

        } // DumpWaveFileIntervalContents

        // ====================================================================

    } // WaveFileClass
}
