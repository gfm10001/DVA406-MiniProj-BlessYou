// WaveFileClass.cs
//
// DVA406 Intelligent Systems, MdH, vt15
//
// History:
// 2015-02-24       Introduced.
// 2015-03-08/GF    Added: DumpWaveFileContents, moved Normalise o this module.
// 2015-03-09/GF    AnalyseWaveFileContents: back off after found trigg ("prefetch").
// 2015-03-10/GF    DumpWaveFileIntervalContents: handle prefetch
// 2015-03-11/GF    Display of trig position corrected.
//                  FTrigPositionIx, WaveFileIntervalBegAtMilliSecs: added
//                  DumpWaveFileContents, DumpWaveFileIntervalContents: corrected
//                  AnalyseWaveFileContents: Add display of interval length in samples.
// 2015-03-12/GF    Added FOrderNr for dump display
// 2015-03-13/GF    CalculateFeatureVector: Added normalize of feature vector values in 2nd vector.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BlessYou
{


    public class WaveFileClass
    {
        bool FDoWaveDump = true; // Use DoWaveDump to control dumps
        string FWaveFileName;
        double[] FWaveFileContents44p1KHz16bitSamples;
        int FStartOfFirstIntervalIx;
        int FTrigPositionIx;
        int FNrOfIntevals;
        int FIntervalSampleCount;
        int FNumberOfChannelsInOrgininalWaveFile;
        static int statLastUsedOrderNr; // Used hold latest nr
        int FOrderNr; // Used to simplify display

        // ====================================================================

        public bool DoWaveDump
        {
            set {FDoWaveDump = value;}
        } // DoWaveDump

        //=====================================================================
        
        public WaveFileClass()
        {
            FNrOfIntevals = ConfigurationStatClass.C_NR_OF_INTERVALS;
            FOrderNr = statLastUsedOrderNr;
            statLastUsedOrderNr++;
        } // WaveFileClass

        //=====================================================================

        public int OrderNr
        {
            get { return FOrderNr; }
        } // OrderNr

        //=====================================================================

        public double WaveFileLengthInMilliSecs
        {
            get { return FWaveFileContents44p1KHz16bitSamples.Length / ConfigurationStatClass.C_SOUND_SAMPLE_FREQUENCY_IN_kHz; }
        } // WaveFileLengthInMilliSecs

        // ====================================================================

        public double WaveFileTrigAtMilliSecs
        {
            get { return FStartOfFirstIntervalIx / ConfigurationStatClass.C_SOUND_SAMPLE_FREQUENCY_IN_kHz; }
        } // WaveFileTrigAtMilliSecs

        // ====================================================================        

        public double WaveFileIntervalBegAtMilliSecs
        {
            get { return FStartOfFirstIntervalIx / ConfigurationStatClass.C_SOUND_SAMPLE_FREQUENCY_IN_kHz; }
        } // WaveFileIntervalBegAtMilliSecs

        // ====================================================================

        public double WaveFileIntervalLengthInMilliSecs
        {
            get { return FIntervalSampleCount / ConfigurationStatClass.C_SOUND_SAMPLE_FREQUENCY_IN_kHz; }
        } // WaveFileIntervalLengthInMilliSecs

        // ====================================================================

        public int NumberOfChannelsInOrgininalWaveFile
        {
            get { return FNumberOfChannelsInOrgininalWaveFile; }
        } // NumberOfChannelsInOrgininalWaveFile

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

            //string s = System.IO.Directory.GetCurrentDirectory();
            //Console.WriteLine("cd='" + s + "'");
            

            FWaveFileName = i_WaveFileName;
            WavFile _wavFile = new WavFile(FWaveFileName);

            FWaveFileContents44p1KHz16bitSamples = new double[_wavFile.RawData.Length];
            for (int ix = 0; ix < _wavFile.RawData.Length; ++ix)
            {
                FWaveFileContents44p1KHz16bitSamples[ix] = (double) _wavFile.RawData[ix];
            } // for ix

            FNumberOfChannelsInOrgininalWaveFile = _wavFile.NumberOfChannelsInWaveFile;

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
            int nrOfSamplesBelowTriggerOff;
            int triggerOffIx;


            // 1. Analyze sample data and calculate 
            // 2. Find ix of first sample with an absolute level higher than the triggerLevel
            //    FStartOfFirstIntervalIx = ix - ix(C_TRIGGER_PREFETCH_IN_MILLI_SECS)
            // 3. FNrOfIntevals from C_NR_OF_INTERVALS
            // 4. FIntervalSampleCount = (FWaveFileContents44p1KHz16bitSamples.Count - FStartOfFirstIntervalIx) / FNrOfIntevals

            FStartOfFirstIntervalIx = 0;
            FTrigPositionIx = 0;
            for (int ix = 0; ix < FWaveFileContents44p1KHz16bitSamples.Length; ++ix)
            {
                // ToDo filter with more samples?
                if (Math.Abs(FWaveFileContents44p1KHz16bitSamples[ix]) > triggerOnLevel)
                {
                    FTrigPositionIx = ix;
                    FStartOfFirstIntervalIx = FTrigPositionIx - (int)(ConfigurationStatClass.C_TRIGGER_PREFETCH_IN_MILLI_SECS * ConfigurationStatClass.C_SOUND_SAMPLE_FREQUENCY_IN_kHz);
                    if (FStartOfFirstIntervalIx < 0)
                    {
                        FStartOfFirstIntervalIx = 0;
                    }
                    break;
                } // if
            } // for ix;


            nrOfSamplesBelowTriggerOff = 0;
            triggerOffIx = FTrigPositionIx + 1;
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

            // Calculate intervall length
            FIntervalSampleCount = (triggerOffIx - FStartOfFirstIntervalIx) / FNrOfIntevals;
            Console.WriteLine("{0, 4:0} - Tot: {1, 6:0}ms IBeg: {2, 6:0}ms Trigg: {3, 6:0}ms IEnd: {4, 6:0}ms IntAll: {5, 4:0}ms Int: {6, 4:0}ms {7, 6:0} = {8, 2:0}%, of whole: {9, 2:0}%, {10}",
                              FOrderNr,
                              WaveFileLengthInMilliSecs,
                              FStartOfFirstIntervalIx,
                              FTrigPositionIx,
                              triggerOffIx / ConfigurationStatClass.C_SOUND_SAMPLE_FREQUENCY_IN_kHz,
                              ConfigurationStatClass.C_NR_OF_INTERVALS * WaveFileIntervalLengthInMilliSecs,
                              WaveFileIntervalLengthInMilliSecs,
                              "(" + FIntervalSampleCount + ")",
                              100.00 * (WaveFileIntervalLengthInMilliSecs / (FWaveFileContents44p1KHz16bitSamples.Length / ConfigurationStatClass.C_SOUND_SAMPLE_FREQUENCY_IN_kHz)),
                              100.00 * (ConfigurationStatClass.C_NR_OF_INTERVALS * WaveFileIntervalLengthInMilliSecs / (FWaveFileContents44p1KHz16bitSamples.Length / ConfigurationStatClass.C_SOUND_SAMPLE_FREQUENCY_IN_kHz)),
                              System.IO.Path.GetFileName(FWaveFileName)
                              );

            //// Dump each interval as a separate file.
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
            double maxValueOfThisFeature = 0;
            int round = 0;
            soundSampleIx = FStartOfFirstIntervalIx;
            for (int intervalIx = 0; intervalIx < FNrOfIntevals; ++intervalIx)
            {
                i_FeatureObj.calculateFeatureValuesFromSamples(FWaveFileContents44p1KHz16bitSamples, soundSampleIx, FIntervalSampleCount, round);
                if (i_FeatureObj.FeatureValueRawVector[intervalIx] > maxValueOfThisFeature)
                {
                    maxValueOfThisFeature = i_FeatureObj.FeatureValueRawVector[intervalIx];
                }
                soundSampleIx = soundSampleIx + FIntervalSampleCount;
                round++;
            } // for intervalIx

            // All intervals have data! Normalize...
            for (int intervalIx = 0; intervalIx < FNrOfIntevals; ++intervalIx)
            {
                if (ConfigurationStatClass.C_EPSILON > maxValueOfThisFeature)
                {
                    // Too small scale vcalue ??? 
                    i_FeatureObj.FeatureValueNormlizedVector.Add(i_FeatureObj.FeatureValueRawVector[intervalIx]); // ???? Not Scalable???
                }
                else
                {
                    i_FeatureObj.FeatureValueNormlizedVector.Add(i_FeatureObj.FeatureValueRawVector[intervalIx] / maxValueOfThisFeature);
                }
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
            int currLineIx;

            if (!FDoWaveDump)
            {
                return;
            }

            // ToDo: - only use a part at debug - or too many samples!
            if (theUsedEndIx > 1000000)
            {
                theUsedEndIx = 1000000; // Max  1,048,576 rows in Excel!
            }

            string[] lineArr = new string[theUsedEndIx - i_BegIx + 3];

            usedFileName = FOrderNr + "_" + System.IO.Path.GetFileNameWithoutExtension(FWaveFileName) + "_" + i_FileNameModifier + ".xls";
            Console.WriteLine("Dumping: " + usedFileName, ", from " + i_BegIx + " to " + i_EndIx);

            currLineIx = 0;

            // Start with colum captions
            lineArr[currLineIx] = "x" + " \t " + "y" + "\tZeroFlag\t" + "fileNameModifier" + "\t" + "i_BegIx" + "\t" + "i_EndIx";
            currLineIx++;

            // Scale Excel byte adding two samples with min/max
            y = -100000;
            x = -2;
            lineArr[currLineIx] = x.ToString() + " \t " + y.ToString() + "\t0\t" + i_FileNameModifier + "\t" + i_BegIx + "\t" + i_EndIx;
            currLineIx++;
            y = 100000;
            x = -1;
            lineArr[currLineIx] = x.ToString() + " \t " + y.ToString() + "\t0";
            currLineIx++;


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
                lineArr[currLineIx] = x.ToString() + " \t " + y.ToString() + "\t" + zeroFlag;
                currLineIx++;

                if (soundSampleIx % 1000000 == 0)
                {
                    if (soundSampleIx > 0)
                    {
                        Console.WriteLine("sample {0}", soundSampleIx);
                    }
                }
            } // for soundSampleIx


            try
            {
                System.IO.File.WriteAllLines(usedFileName, lineArr);
            }
            catch (Exception ex)
            {
                Console.WriteLine("DumpWaveFileContents to {0} - ERR: " + ex.Message, usedFileName);
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
            int currLineIx;

            if (!FDoWaveDump)
            {
                return;
            }

            // ToDo: - only use a part at debug - or too many samples!
            if (theUsedEndIx > 1000000)
            {
                theUsedEndIx = 1000000; // Max  1,048,576 rows in Excel!
            }

            string[] lineArr = new string[theUsedEndIx + 3];

            usedFileName = FOrderNr + "_" + System.IO.Path.GetFileNameWithoutExtension(FWaveFileName) + "_" + fileNameModifier + ".xls";
            Console.WriteLine("Dumping: " + usedFileName, ", from " + i_StartIx + " to " + theUsedEndIx);

            currLineIx = 0;

            // Start with colum captions
            lineArr[currLineIx] = "x"+ " \t " + "y"+ "\tInterval\t\t" + "File Name" + "\t" + "fileNameModifier" + "\t" + "i_StartIx" + "\t" + "theUsedEndIx";
            currLineIx++;

            // Scale Excel byte adding two samples with min/max
            y = -100000;
            x = -2;
            lineArr[currLineIx] = x.ToString() + " \t " + y.ToString() + "\t0\t\t" + usedFileName + "\t" + fileNameModifier + "\t" + i_StartIx + "\t" + theUsedEndIx;
            currLineIx++;
            y = 100000;
            x = -1;
            lineArr[currLineIx] = x.ToString() + " \t " + y.ToString() + "\t0\t\t" + ConfigurationStatClass.C_TRIGGER_PREFETCH_IN_MILLI_SECS * ConfigurationStatClass.C_SOUND_SAMPLE_FREQUENCY_IN_kHz;
            currLineIx++;

            // Convert the samples to text lines
            for (soundSampleIx = 0; soundSampleIx < theUsedEndIx; ++soundSampleIx)
            {
                x = soundSampleIx;        
                y = FWaveFileContents44p1KHz16bitSamples[soundSampleIx];
                intervalIx = 0;
                if (soundSampleIx > i_StartIx)
                {
                    intervalIx = 1 + (int)((soundSampleIx - i_StartIx) / i_IntervalLengthInSamples);

                    if (soundSampleIx < FTrigPositionIx)
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

                if (soundSampleIx == FTrigPositionIx)
                {
                    // Mark trigg position
                    intervalIx = (int)ConfigurationStatClass.C_MAX_POSSIBLE_VALUE;
                }
                else
                {
                    intervalIx = intervalIx * 10000;
                }
                lineArr[currLineIx] = x.ToString() + " \t " + y.ToString() + " \t " + intervalIx.ToString(); ;
                currLineIx++;

                if (soundSampleIx % 1000000 == 0)
                {
                    if (soundSampleIx > 0)
                    {
                        Console.WriteLine("sample {0}", soundSampleIx);
                    }
                }
            } // for soundSampleIx


            try
            {
                System.IO.File.WriteAllLines(usedFileName, lineArr);

            }
            catch (Exception ex)
            {
                Console.WriteLine("DumpWaveFileIntervalContents to {0}- ERR: " + ex.Message, usedFileName);
            }

        } // DumpWaveFileIntervalContents

        // ====================================================================

    } // WaveFileClass
}
