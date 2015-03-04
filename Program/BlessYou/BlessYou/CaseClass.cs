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
        protected string _WavFile_FullPathAndFileNameStr;
        protected EnumCaseStatus _SneezeStatus;

        List<FeatureBaseClass> FFeatureTypeVector; // Each list element in the FV is a type of feature, each element consists of a number of values, one per time interval

        // ====================================================================

        public void Case()
        {
            FFeatureTypeVector = new List<FeatureBaseClass>();
            // Create FFeatureWeights and make sure sum = 1
            // Tips: anv. const declr i egen fil som ingångs data
            throw new System.NotImplementedException();
        } // Case

        // ====================================================================

        public void ExtractWavFileFeatures(string i_WavFile_FullPathAndFileNameStr)
        {
            WaveFileClass waveFileObj = new WaveFileClass();

            waveFileObj.ReadWaveFile(i_WavFile_FullPathAndFileNameStr);
            waveFileObj.NormalizeWaveFileContents(); 
            waveFileObj.AnalyseWaveFileContents();

            FeaturePeakClass featurePeakObj = new FeaturePeakClass(); 
            waveFileObj.CalculateFeatureVector(featurePeakObj);
            FFeatureTypeVector.Add(featurePeakObj);

            FeatureAverageClass featureAverageObj = new FeatureAverageClass(); 
            waveFileObj.CalculateFeatureVector(featureAverageObj);
            FFeatureTypeVector.Add(featureAverageObj);
            
            // Todo för övriga features


            // At last normalize feature weights
            double sum = 0;
            foreach (FeatureBaseClass fbc in FFeatureTypeVector)
            {
                sum = sum + fbc.FeatureWeight;
            }
            foreach (FeatureBaseClass fbc in FFeatureTypeVector)
            {
                fbc.FeatureWeight = fbc.FeatureWeight / sum;
            }

            throw new System.NotImplementedException();
        } // ExtractWavFileFeatures

        // ====================================================================

        public double calculateSimilarityFunction(CaseClass i_NewCase)
        {
            double sum = 0;
            for (int jx = 0; jx < FFeatureTypeVector.Count; ++jx)
            {
                for (int ix = 0; ix < FFeatureTypeVector[jx].FeatureValueVector.Count; ++ix)
                {
                    sum = sum + FFeatureTypeVector[jx].SimilarityFunctionForAttribute(i_NewCase.FFeatureTypeVector[jx].FeatureValueVector[ix], FFeatureTypeVector[jx].FeatureValueVector[ix]);
                } // for ix
            } // for jx
            return sum;
        } // calculateSimilarityFunction

        // ====================================================================

    } // CaseClass
}
