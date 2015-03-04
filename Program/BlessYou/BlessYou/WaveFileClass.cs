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

        const double C_MAX_POSSIBLE_VALUE = 1000000; // was 0x7FFF; // The maximum absolute value in a sound file recoded at 16 bit 

        // ====================================================================

        public WaveFileClass()
        {
            FNrOfIntevals = ConfigurationStatClass.C_NR_OF_INTERVALS;
        } // WaveFileClass

        public void ReadWaveFile(string i_WaveFileName)
        {
            FWaveFileName = i_WaveFileName;
            BlessYou.WavFile wf = new WavFile(i_WaveFileName);
            FWaveFileContents44p1KHz16bitSamples = wf.Data;


            // 1. Öppna i_WaveFileName
            // 2. Analyser antal kanaler (1/2)
            // 3. Läs data 
            //   för varje 16bit sample:
            //      Mono:   läs integer 16 bit och placera som double i FWaveFileContents 
            //      Stereo: läs 2 (L, R)  integer 16 bit, konvertera till double och medelvärdesbilda, placera som double i FWaveFileContents44p1KHz16bitSamples 
            // 4. Utvärdera ev. fel kasta exception om fel
         
            //throw new System.NotImplementedException();
        } // ReadWaveFile

        // ====================================================================

        public void NormalizeWaveFileContents()
        {
            double scaleFactor;
            double maxValue;

            // 1. Leta upp absoluta max värdet (maxValue)
            // 2: Calculate: scalefactor = C_MAX_POSSIBLE_VALUE / maxValue;
            // 2. Skala alla värden: 
            //      i = [0, FWaveFileContents44p1KHz16bitSamples.Count - 1]
            //      FWaveFileContents44p1KHz16bitSamples[i] = FWaveFileContents44p1KHz16bitSamples[i] * scalefactor

            //throw new System.NotImplementedException();
        } // AnalyseWaveFileContents


        // ====================================================================

        public void AnalyseWaveFileContents()
        {
            double triggerLevel = ( ConfigurationStatClass.C_TRIGGER_LEVEL_IN_PERCENT / 100.0 ) * C_MAX_POSSIBLE_VALUE;

            // 1. Analyze sample data and calculate 
            // 2. Find ix of first sample with an absolute level higher than the triggerLevel
            //    FStartOfFirstIntervalIx = ix
            // 3. FNrOfIntevals; DONE
            // 4. FIntervalSampleCount = (FWaveFileContents44p1KHz16bitSamples.Count - FStartOfFirstIntervalIx) / FNrOfIntevals

            throw new System.NotImplementedException();
        } // AnalyseWaveFileContents

        // ====================================================================

        public void CalculateFeatureVector(FeatureBaseClass i_FeatureObj)
        {
          
            // 1. calculate featurevector
            // 2. Type of feature depends on i_FeatureObj
            Console.WriteLine("Feature: {0}", i_FeatureObj.FeatureName); // ToDo
            int soundSampleIx;
            

            soundSampleIx = FStartOfFirstIntervalIx;
            for (int intervalIx = 0; intervalIx < FNrOfIntevals; ++intervalIx)
            {
                i_FeatureObj.calculateFeatureValuesFromSamples(FWaveFileContents44p1KHz16bitSamples, soundSampleIx, FIntervalSampleCount);
                soundSampleIx = soundSampleIx + FIntervalSampleCount;
            } // for intervalIx

            throw new System.NotImplementedException();
        } // CalculateFeatureVector

        // ====================================================================

    } // WaveFileClass
}
