// Case.cs
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


    public class CaseClass
    {
        public static List<double> FFeatureWeights;

        WaveFileClass _WaveFileWorkArea;

        string _WavFile_FullPathAndFileNameStr;
        EnumCaseStatus _SneezeStatus;

        List<FeatureBaseClass> _featureVector; // Each list element in the FV is a type of feature, each element consists of a number of values, one per time interval

        // ====================================================================

        public void Case()
        {
            _featureVector = new List<FeatureBaseClass>();
            // Create FFeatureWeights and make sure sum = 1
            // Tips: anv. const declr i egen fil som ingångs data
            throw new System.NotImplementedException();
        } // Case

        // ====================================================================

        void _extractWavFileFeatures(string i_WavFile_FullPathAndFileNameStr)
        {
            WaveFileClass waveFileObj = new WaveFileClass();

            waveFileObj.ReadWaveFile(i_WavFile_FullPathAndFileNameStr);
            waveFileObj.NormalizeWaveFileContents();
            waveFileObj.AnalyseWaveFileContents();

            FeaturePeakClass featurePeakObj = new FeaturePeakClass(); 
            waveFileObj.CalculateFeatureVector(featurePeakObj);
            _featureVector.Add(featurePeakObj);

            FeatureAverageClass featureAverageObj = new FeatureAverageClass(); 
            waveFileObj.CalculateFeatureVector(featureAverageObj);
            _featureVector.Add(featureAverageObj);
            
            // Todo för övriga features

            throw new System.NotImplementedException();
        } // _extractWavFileFeatures

        // ====================================================================

    } // CaseClass
}
