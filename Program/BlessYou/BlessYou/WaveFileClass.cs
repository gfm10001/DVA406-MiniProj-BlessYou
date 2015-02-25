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
        List<double> FWaveFileContents44p1KHz16bitSamples;
        int FStartOfFirstIntervalIx;
        int FNrOfIntevals;
        int FIntervalSampleCount;

        // ====================================================================

        public void ReadWaveFile(string i_WaveFileName)
        {
            FWaveFileName = i_WaveFileName;

            // 1. Öppna i_WaveFileName
            // 2. Analyser antal kanaler (1/2)
            // 3. Läs data 
            //   för varje 16bit sample:
            //      Mono:   läs integer 16 bit och placera som double i FWaveFileContents 
            //      Stereo: läs 2 (L, R)  integer 16 bit, konevrta till double och medelvärdesbilda, placera som double i FWaveFileContents 
            // 4. Utvärdera ev. fel
         
            throw new System.NotImplementedException();
        } // ReadWaveFile

        // ====================================================================

        public void AnalyseWaveFileContents()
        {

            // 1. Analysera sample data och beräkna 
            //      FStartOfFirstIntervalIx;
            //      FNrOfIntevals;
            //      FIntervalSampleCount;

            throw new System.NotImplementedException();
        } // AnalyseWaveFileContents

        // ====================================================================

        public void CalculateFeatureVector(out List<List<double>> o_FeatureVector)
        {

            // 1. Skapa o_FeatureVector
            //   - för varje feature, beräkna data med:
            //      CalculatePeakColumn
            //      CalculateMeanColumn
            // etc...

            throw new System.NotImplementedException();
        } // CalculateFeatureVector

        // ====================================================================

    }
}
