// CaseClass.cs
//
// DVA406 Intelligent Systems, Mdh, vt15
//
// History:
// 2015-02-24       Introduced.
// 2015-03-05/GF    FeatureTypeToString: merged to single line

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

        // CaseClass Default Constructor
        public CaseClass()
        {
            FFeatureTypeVector = new List<FeatureBaseClass>();
            // Create FFeatureWeights and make sure sum = 1
            // Tips: anv. const declr i egen fil som ingångs i_Data
            // ToDo throw new System.NotImplementedException();
        } // CaseClass

        // ====================================================================

        // CaseClass Copy Constructor
        public CaseClass(CaseClass i_CaseClassObj)
        {
            this.WavFile_FullPathAndFileNameStr = i_CaseClassObj.WavFile_FullPathAndFileNameStr;
            this.FeatureTypeVector = i_CaseClassObj.FeatureTypeVector;
            this.SneezeStatus = i_CaseClassObj.SneezeStatus;
        } // CaseClass

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
                    _SneezeStatus = EnumCaseStatus.csIsConfirmedSneeze;
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

            FeaturePassingZeroClass featurePassingZeroObj = new FeaturePassingZeroClass();
            waveFileObj.CalculateFeatureVector(featurePassingZeroObj);
            FFeatureTypeVector.Add(featurePassingZeroObj);

            FeatureFFTClass featureFFTObj = new FeatureFFTClass();
            waveFileObj.CalculateFeatureVector(featureFFTObj);
            FFeatureTypeVector.Add(featureFFTObj);


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
                    sum = sum + FFeatureTypeVector[jx].AbsDiffForAttribute(i_NewCase.FFeatureTypeVector[jx].FeatureValueVector[ix], FFeatureTypeVector[jx].FeatureValueVector[ix]);
                } // for ix
                sum = sum / FFeatureTypeVector[jx].FeatureValueVector.Count;
                sum = sum * FFeatureTypeVector[jx].FeatureWeight;
            } // for jx
            return sum;
        } // calculateSimilarityFunction

        // ====================================================================

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

            resStr = String.Format("{0,-40}", System.IO.Path.GetFileName(_WavFile_FullPathAndFileNameStr));

            for (int ix = 0; ix < fbc.FeatureValueVector.Count; ++ix)
            {
                resStr = resStr + " " + String.Format("{0, 10:0.000}", fbc.FeatureValueVector[ix]);
            } // for
            return resStr;
        } // FeatureTypeToString

        // ====================================================================
    } // CaseClass
}
