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
        string _WavFile_FullPathAndFileNameStr;
        EnumCaseStatus _SneezeStatus;
        List<FeatureBaseClass> FFeatureTypeVector; // Each list element in the FV is a type of feature, each element consists of a number of values, one per time interval
        
        //=====================================================================

        public string WavFile_FullPathAndFileNameStr
        {
            get { return _WavFile_FullPathAndFileNameStr; }

            set { _WavFile_FullPathAndFileNameStr = value; }

        }

        //=====================================================================

        public EnumCaseStatus SneezeStatus
        {
            get
            {
                return _SneezeStatus;
            }
            set
            {
                _SneezeStatus = value;
            }
        } // SneezeStatus

        //=====================================================================

        public List<FeatureBaseClass> FeatureTypeVector
        {
            get
            {
                return FFeatureTypeVector;
            }
            set
            {
                FFeatureTypeVector = value;
            }
        } // FeatureTypeVector

        // ====================================================================

        public CaseClass()
        {
            FFeatureTypeVector = new List<FeatureBaseClass>();
            // Create FFeatureWeights and make sure sum = 1
            // Tips: anv. const declr i egen fil som ingångs data
            // ToDo throw new System.NotImplementedException();
        } // Case

        // ====================================================================

        public void ExtractWavFileFeatures(SoundFileClass i_SoundFileObj)
        {
           
            WaveFileClass waveFileObj = new WaveFileClass();
            switch (i_SoundFileObj.SoundFileSneezeMarker)
            {
                case EnumSneezeMarker.smNone:
                    _SneezeStatus = EnumCaseStatus.csNone;
                    break;
                case EnumSneezeMarker.smUnKnown:
                    _SneezeStatus = EnumCaseStatus.csUnknown;
                    break;
                case EnumSneezeMarker.smNoSneeze:
                    _SneezeStatus = EnumCaseStatus.csIsConfirmedNoneSneeze;
                    break;
                case EnumSneezeMarker.smSneeze:
                    _SneezeStatus = EnumCaseStatus.csIsProposedSneeze;
                    break;
                default:
                    _SneezeStatus = EnumCaseStatus.csNone;
                    break;
            } // switch


            waveFileObj.ReadWaveFile(i_SoundFileObj.SoundFileName);
            waveFileObj.NormalizeWaveFileContents();
            waveFileObj.AnalyseWaveFileContents();

            FeaturePeakClass featurePeakObj = new FeaturePeakClass();
            waveFileObj.CalculateFeatureVector(featurePeakObj);
            FFeatureTypeVector.Add(featurePeakObj);

            FeatureAverageClass featureAverageObj = new FeatureAverageClass();
            waveFileObj.CalculateFeatureVector(featureAverageObj);
            FFeatureTypeVector.Add(featureAverageObj);

            FeatureRMSClass featureRMSObj = new FeatureRMSClass();
            waveFileObj.CalculateFeatureVector(featureRMSObj);
            FFeatureTypeVector.Add(featureRMSObj);

            FeaturePeak2PeakClass featurePeak2PeakObj = new FeaturePeak2PeakClass();
            waveFileObj.CalculateFeatureVector(featurePeak2PeakObj);
            FFeatureTypeVector.Add(featurePeak2PeakObj);

            FeatureCrestFactorClass featureCrestFactorObj = new FeatureCrestFactorClass();
            waveFileObj.CalculateFeatureVector(featureCrestFactorObj);
            FFeatureTypeVector.Add(featureCrestFactorObj);

            FeaturePassingZero featurePassingZeroObj = new FeaturePassingZero();
            waveFileObj.CalculateFeatureVector(featurePassingZeroObj);
            FFeatureTypeVector.Add(featurePassingZeroObj);



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

            // ToDo    throw new System.NotImplementedException();
        } // ExtractWavFileFeatures

        // ====================================================================

        public double CalculateRawSimilarityValue(CaseClass i_NewCase)
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


        public void CalculateScaledSimilarityValue(CaseClass i_NewCase, List<RetrievedCaseClass> nearby)
        {
            double highest = nearby[0].RawSimilarityValue;
            for (int i = 0; i < nearby.Count; i++)
            {
                if (nearby[i].RawSimilarityValue > highest)
                {
                    highest = nearby[i].RawSimilarityValue;
                }
            }
            
            double mod = 1.0 / highest;
            for (int i = 0; i< nearby.Count; i++)
            {
                nearby[i].ScaledSimilarityValue = nearby[i].RawSimilarityValue * mod;
            }
        
        
        }

        public override string ToString()
        {
            string resStr = "CaseClass - dump:\n";
            foreach (FeatureBaseClass fbc in FFeatureTypeVector)
            {
                resStr = resStr + "Feature Type = " + fbc.FeatureName + "\n";
                for (int ix = 0; ix < fbc.FeatureValueVector.Count; ++ix)
                {
                    resStr = resStr + " " + String.Format("{0:000000.0}", fbc.FeatureValueVector[ix]);
                } // for
                resStr = resStr + "\n";
            }
            return resStr;
        } // ToString

        // ====================================================================

        public string FeatureTypeToString(int i_FeatureTypeIx)
        {
            string resStr = "";
            FeatureBaseClass fbc;

            fbc = FFeatureTypeVector[i_FeatureTypeIx];
            resStr = resStr + "Feature Type = " + fbc.FeatureName + " _SneezeStatus=" + _SneezeStatus.ToString() + "\n";
            for (int ix = 0; ix < fbc.FeatureValueVector.Count; ++ix)
            {
                resStr = resStr + " " + String.Format("{0:000000.0}", fbc.FeatureValueVector[ix]);
            } // for
            resStr = resStr + "\n";

            return resStr;
        } // FeatureTypeToString

        // ====================================================================
    } // CaseClass
}
