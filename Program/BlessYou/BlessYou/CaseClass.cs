// CaseClass.cs
//
// DVA406 Intelligent Systems, Mdh, vt15
//
// History:
// 2015-02-24       Introduced.
// 2015-03-05/GF    FeatureTypeToString: merged to single line
// 2015-03-08/GF    Added dump of wave contents
// 2015-03-08/GF    AnalyseParamsToString: added
// 2015-03-11/GF    Correction: Trigg Position display was incorrect
//                  Addition: AnalyseParamsToString display also index

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlessYou
{


    public class CaseClass
    {
        string _WavFile_FullPathAndFileNameStr;
        double FWaveFileLengthInMilliSecs;
        double FWaveFileIntervalBegPositionInMilliSecs;
        double FWaveFileTriggPositionInMilliSecs;
        double FWaveFileIntervallLengthInMilliSecs;
        int FNumberOfChannelsInOrgininalWaveFile;

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
            // Tips: anv. const declr i egen fil som ingångs data
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

        public void ExtractWavFileFeatures(SoundFileClass i_SoundFileObj, ConfigurationStatClass i_config = null)
        {

            if (i_config == null)
                i_config = new ConfigurationStatClass();

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

            FWaveFileLengthInMilliSecs = waveFileObj.WaveFileLengthInMilliSecs;
            FWaveFileIntervalBegPositionInMilliSecs = waveFileObj.WaveFileIntervalBegAtMilliSecs;
            FWaveFileTriggPositionInMilliSecs = waveFileObj.WaveFileTrigAtMilliSecs;
            FWaveFileIntervallLengthInMilliSecs = waveFileObj.WaveFileIntervalLengthInMilliSecs;
            FNumberOfChannelsInOrgininalWaveFile = waveFileObj.NumberOfChannelsInOrgininalWaveFile;

            FeaturePeakClass featurePeakObj = new FeaturePeakClass(i_config);
            waveFileObj.CalculateFeatureVector(featurePeakObj);
            FFeatureTypeVector.Add(featurePeakObj);

            FeatureAverageClass featureAverageObj = new FeatureAverageClass(i_config);
            waveFileObj.CalculateFeatureVector(featureAverageObj);
            FFeatureTypeVector.Add(featureAverageObj);

            FeatureRMSClass featureRMSObj = new FeatureRMSClass(i_config);
            waveFileObj.CalculateFeatureVector(featureRMSObj);
            FFeatureTypeVector.Add(featureRMSObj);

            FeaturePeak2PeakClass featurePeak2PeakObj = new FeaturePeak2PeakClass(i_config);
            waveFileObj.CalculateFeatureVector(featurePeak2PeakObj);
            FFeatureTypeVector.Add(featurePeak2PeakObj);

            FeatureCrestFactorClass featureCrestFactorObj = new FeatureCrestFactorClass(i_config);
            waveFileObj.CalculateFeatureVector(featureCrestFactorObj);
            FFeatureTypeVector.Add(featureCrestFactorObj);

            FeaturePassingZeroClass featurePassingZeroObj = new FeaturePassingZeroClass(i_config);
            waveFileObj.CalculateFeatureVector(featurePassingZeroObj);
            FFeatureTypeVector.Add(featurePassingZeroObj);

            //ToDo Evaluationfunctions to be developed
            FeatureLomontFFTClass featureLomontFFT16Obj = new FeatureLomontFFTClass(ConfigurationStatClass.C_NR_OF_SAMPLES_2_POWER_16, _WavFile_FullPathAndFileNameStr, i_config);
            waveFileObj.CalculateFeatureVector(featureLomontFFT16Obj);
            FFeatureTypeVector.Add(featureLomontFFT16Obj);

            FeatureLomontFFTClass featureLomontFFT14Obj = new FeatureLomontFFTClass(ConfigurationStatClass.C_NR_OF_SAMPLES_2_POWER_14, _WavFile_FullPathAndFileNameStr, i_config);
            waveFileObj.CalculateFeatureVector(featureLomontFFT14Obj);
            FFeatureTypeVector.Add(featureLomontFFT14Obj);

            FeatureLomontFFTClass featureLomontFFT12Obj = new FeatureLomontFFTClass(ConfigurationStatClass.C_NR_OF_SAMPLES_2_POWER_12, _WavFile_FullPathAndFileNameStr, i_config);
            waveFileObj.CalculateFeatureVector(featureLomontFFT12Obj);
            FFeatureTypeVector.Add(featureLomontFFT12Obj);

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

        public void UpdateFeatureVectors(ConfigurationStatClass i_config)
        {
            FFeatureTypeVector.Clear();

            FeaturePeakClass featurePeakObj = new FeaturePeakClass(i_config);
            //waveFileObj.CalculateFeatureVector(featurePeakObj);
            FFeatureTypeVector.Add(featurePeakObj);

            FeatureAverageClass featureAverageObj = new FeatureAverageClass(i_config);
            //waveFileObj.CalculateFeatureVector(featureAverageObj);
            FFeatureTypeVector.Add(featureAverageObj);

            FeatureRMSClass featureRMSObj = new FeatureRMSClass();
            //waveFileObj.CalculateFeatureVector(featureRMSObj);
            FFeatureTypeVector.Add(featureRMSObj);

            FeaturePeak2PeakClass featurePeak2PeakObj = new FeaturePeak2PeakClass(i_config);
            //waveFileObj.CalculateFeatureVector(featurePeak2PeakObj);
            FFeatureTypeVector.Add(featurePeak2PeakObj);

            FeatureCrestFactorClass featureCrestFactorObj = new FeatureCrestFactorClass(i_config);
            //waveFileObj.CalculateFeatureVector(featureCrestFactorObj);
            FFeatureTypeVector.Add(featureCrestFactorObj);

            FeaturePassingZeroClass featurePassingZeroObj = new FeaturePassingZeroClass(i_config);
            //waveFileObj.CalculateFeatureVector(featurePassingZeroObj);
            FFeatureTypeVector.Add(featurePassingZeroObj);

            //ToDo Evaluationfunctions to be developed
            FeatureLomontFFTClass featureLomontFFT16Obj = new FeatureLomontFFTClass(ConfigurationStatClass.C_NR_OF_SAMPLES_2_POWER_16, _WavFile_FullPathAndFileNameStr, i_config);
            //waveFileObj.CalculateFeatureVector(featureLomontFFT16Obj);
            FFeatureTypeVector.Add(featureLomontFFT16Obj);

            FeatureLomontFFTClass featureLomontFFT14Obj = new FeatureLomontFFTClass(ConfigurationStatClass.C_NR_OF_SAMPLES_2_POWER_14, _WavFile_FullPathAndFileNameStr, i_config);
            //waveFileObj.CalculateFeatureVector(featureLomontFFT14Obj);
            FFeatureTypeVector.Add(featureLomontFFT14Obj);

            FeatureLomontFFTClass featureLomontFFT12Obj = new FeatureLomontFFTClass(ConfigurationStatClass.C_NR_OF_SAMPLES_2_POWER_12, _WavFile_FullPathAndFileNameStr, i_config);
            //waveFileObj.CalculateFeatureVector(featureLomontFFT12Obj);
            FFeatureTypeVector.Add(featureLomontFFT12Obj);

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

        }

        // ====================================================================

        public double CalculateDistanceValue(CaseClass i_NewCase)
        {
            double sum = 0;
            for (int jx = 0; jx < FFeatureTypeVector.Count; ++jx)
            {
                for (int ix = 0; ix < FFeatureTypeVector[jx].FeatureValueVector.Count; ++ix)
                {
                    sum = sum + FFeatureTypeVector[jx].AbsDiffForAttribute(i_NewCase.FFeatureTypeVector[jx].FeatureValueVector[ix], FFeatureTypeVector[jx].FeatureValueVector[ix]);
                } // for ix
                //sum = sum / FFeatureTypeVector[jx].FeatureValueVector.Count;
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
                resStr = resStr + "\t" + String.Format("{0, 10:0.000}", fbc.FeatureValueVector[ix]);
            } // for
            return resStr;
        } // FeatureTypeToString

        // ====================================================================

        public string AnalyseParamsToString()
        {
            string resStr = "";

            resStr = String.Format("{0,-60} - Tot: {1, 6:0}ms IBeg: {2, 6:0}ms Trigg: {3, 6:0}ms IEnd: {4, 6:0}ms Int: {5, 4:0}ms {6, 6:0} = {7, 3:0}%, of whole: {8, 2:0}% (was {9} channel(s)) Sneeze: {10}",
                                     System.IO.Path.GetFileName(_WavFile_FullPathAndFileNameStr),
                                     FWaveFileLengthInMilliSecs,
                                     FWaveFileIntervalBegPositionInMilliSecs,
                                     FWaveFileTriggPositionInMilliSecs,
                                     FWaveFileIntervalBegPositionInMilliSecs + FWaveFileIntervallLengthInMilliSecs * ConfigurationStatClass.C_NR_OF_INTERVALS,
                                     FWaveFileIntervallLengthInMilliSecs,
                                     "(" + (int)(FWaveFileIntervallLengthInMilliSecs * ConfigurationStatClass.C_SOUND_SAMPLE_FREQUENCY_IN_kHz) + ")",
                                     100.00 * (FWaveFileIntervallLengthInMilliSecs / FWaveFileLengthInMilliSecs),
                                     100.00 * (ConfigurationStatClass.C_NR_OF_INTERVALS * FWaveFileIntervallLengthInMilliSecs / FWaveFileLengthInMilliSecs),
                                     FNumberOfChannelsInOrgininalWaveFile,
                                     _SneezeStatus);
            return resStr;
        } // AnalyseParamsToString

        // ====================================================================

    } // CaseClass
}
