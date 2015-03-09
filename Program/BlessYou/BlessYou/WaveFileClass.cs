// WaveFileClass.cs
//
// DVA406 Intelligent Systems, Mdh, vt15
//
// History:
// 2015-02-24   Introduced.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BlessYou
{

    
    public class WaveFileClass
    {
        string FWaveFileName;
        double[] FWaveFileContents44p1KHz16bitSamples;
        int FStartOfFirstIntervalIx;
        int FNrOfIntevals;
        int FIntervalSampleCount;
        WavFile _wavFile;

        const double C_MAX_POSSIBLE_VALUE = 100000; // was 0x7FFF; // The maximum absolute value in a sound file recoded at 16 bit 

        // ====================================================================

        public WaveFileClass()
        {
            FNrOfIntevals = ConfigurationStatClass.C_NR_OF_INTERVALS;
        } // WaveFileClass

        public WaveFileClass(string filepath, int limit)
        {
            _wavFile = new WavFile(filepath, limit);
            //FWaveFileContents44p1KHz16bitSamples = _wavFile.Data;
        }

        public void ReadWaveFile(string i_WaveFileName)
        {
            FWaveFileName = i_WaveFileName;
            _wavFile = new WavFile(i_WaveFileName);
            //FWaveFileContents44p1KHz16bitSamples = wf.Data;


            // 1. Öppna i_WaveFileName
            // 2. Analyser antal kanaler (1/2)
            // 3. Läs i_Data 
            //   för varje 16bit sample:
            //      Mono:   läs integer 16 bit och placera som double i FWaveFileContents 
            //      Stereo: läs 2 (L, R)  integer 16 bit, konvertera till double och medelvärdesbilda, placera som double i FWaveFileContents44p1KHz16bitSamples 
            // 4. Utvärdera ev. fel kasta exception om fel
         
            //throw new System.NotImplementedException();
        } // ReadWaveFile

        // ====================================================================

        public void NormalizeWaveFileContents()
        {
            //double scaleFactor;
            //double maxValue;
             //_wavFile.Normalize(C_MAX_POSSIBLE_VALUE);
             FWaveFileContents44p1KHz16bitSamples = _wavFile.Data;


            // 1. Leta upp absoluta max värdet (maxValue)
            // 2: CalculateFFT: scalefactor = C_MAX_POSSIBLE_VALUE / maxValue;
            // 2. Skala alla värden: 
            //      i = [0, FWaveFileContents44p1KHz16bitSamples.Count - 1]
            //      FWaveFileContents44p1KHz16bitSamples[i] = FWaveFileContents44p1KHz16bitSamples[i] * scalefactor

            //throw new System.NotImplementedException();
        } // AnalyseWaveFileContents


        // ====================================================================

        public void AnalyseWaveFileContents()
        {
            double triggerOnLevel = (ConfigurationStatClass.C_TRIGGER_LEVEL_IN_PERCENT / 100.0) * C_MAX_POSSIBLE_VALUE;
            double triggerOffLevel = (ConfigurationStatClass.C_TRIGGER_OFF_LEVEL_IN_PERCENT / 100.0) * C_MAX_POSSIBLE_VALUE;
            int sampleCountOfTriggerOffDuration = (int)Math.Round(ConfigurationStatClass.C_TRIGGER_OFF_DURATION_IN_MILLI_SECS * ConfigurationStatClass.C_SOUND_SAMPLE_FREQUENCY_IN_kHz);
            int triggerOffIx;
            int nrOfSamplesBelowTriggerOff;
          
            

            // 1. Analyze sample i_Data and calculate 
            // 2. Find ix of first sample with an absolute level higher than the triggerLevel
            //    FStartOfFirstIntervalIx = ix
            // 3. FNrOfIntevals; DONE
            // 4. FIntervalSampleCount = (FWaveFileContents44p1KHz16bitSamples.Count - FStartOfFirstIntervalIx) / FNrOfIntevals

            FStartOfFirstIntervalIx = 0 ;
            triggerOffIx = 0;
            for (int ix = 0; ix < FWaveFileContents44p1KHz16bitSamples.Length ; ++ix)
            {
                // ToDo filter with more samples?
                if (Math.Abs(FWaveFileContents44p1KHz16bitSamples[ix]) > triggerOnLevel)
                {
                    FStartOfFirstIntervalIx = ix;
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

            // CalculateFFT intevall length
            FIntervalSampleCount = (triggerOffIx - FStartOfFirstIntervalIx) / FNrOfIntevals;

            Console.WriteLine("{0,-40} - Tot: {1, 6:0}ms triggOn: {2, 6:0}ms triggOff: {3, 6:0}ms Int: {4, 6:0}ms = {5, 6:0}%",
                              System.IO.Path.GetFileName(FWaveFileName), FWaveFileContents44p1KHz16bitSamples.Length / ConfigurationStatClass.C_SOUND_SAMPLE_FREQUENCY_IN_kHz,
                              FStartOfFirstIntervalIx / ConfigurationStatClass.C_SOUND_SAMPLE_FREQUENCY_IN_kHz,
                              triggerOffIx / ConfigurationStatClass.C_SOUND_SAMPLE_FREQUENCY_IN_kHz,
                              FIntervalSampleCount / ConfigurationStatClass.C_SOUND_SAMPLE_FREQUENCY_IN_kHz,
                              100.00 * FIntervalSampleCount * ConfigurationStatClass.C_NR_OF_INTERVALS / FWaveFileContents44p1KHz16bitSamples.Length);
        } // AnalyseWaveFileContents

        // ====================================================================

        public void CalculateFeatureVector(FeatureBaseClass i_FeatureObj)
        {
          
            // 1. calculate featurevector
            // 2. Type of feature depends on i_FeatureObj
           // Console.WriteLine("Feature: {0}", i_FeatureObj.FeatureName); // ToDo
            int soundSampleIx;
            

            soundSampleIx = FStartOfFirstIntervalIx;
            //for (int intervalIx = 0; intervalIx < FNrOfIntevals; ++intervalIx)
            {
                i_FeatureObj.calculateFeatureValuesFromSamples(FWaveFileContents44p1KHz16bitSamples, soundSampleIx, FIntervalSampleCount);
                soundSampleIx = soundSampleIx + FIntervalSampleCount;
            } // for intervalIx


        } // CalculateFeatureVector

        // ====================================================================

    } // WaveFileClass
}
