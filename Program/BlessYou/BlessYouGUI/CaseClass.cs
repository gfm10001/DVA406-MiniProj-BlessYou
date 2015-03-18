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
// 2015-03-12/GF    Added: FOrderNr for dump display
// 2015-03-13/GF    Added: GetChangesOfAllValuesOfThisFeatureTypeToString, GetDiffsOfAllValuesNormalizedOfThisFeatureTypeToString, 
//                         CalculateSimilarityValueExt, CalculateSimilarityFunctionOfThisFeature

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; // Par.Invore

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
        int FOrderNr; // Used at dump display

        EnumCaseStatus _SneezeStatus;
        List<FeatureBaseClass> FFeatureTypeVector; // Each list element in the FV is a type of feature, each element consists of a number of values, one per time interval


        //=====================================================================

        public int OrderNr
        {
            get { return FOrderNr; }
        } // OrderNr

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

        public void ExtractWavFileFeatures(SoundFileClass i_SoundFileObj, bool i_DoDisplayAnalyseLog, ConfigurationDynClass i_config = null)
        {

            if (i_config == null)
                i_config = new ConfigurationDynClass();

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
            waveFileObj.DisplayOfFileAnalyseLog = i_DoDisplayAnalyseLog;
            waveFileObj.AnalyseWaveFileContents();

            FWaveFileLengthInMilliSecs = waveFileObj.WaveFileLengthInMilliSecs;
            FWaveFileIntervalBegPositionInMilliSecs = waveFileObj.WaveFileIntervalBegAtMilliSecs;
            FWaveFileTriggPositionInMilliSecs = waveFileObj.WaveFileTrigAtMilliSecs;
            FWaveFileIntervallLengthInMilliSecs = waveFileObj.WaveFileIntervalLengthInMilliSecs;
            FNumberOfChannelsInOrgininalWaveFile = waveFileObj.NumberOfChannelsInOrgininalWaveFile;
            FOrderNr = waveFileObj.OrderNr;

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

            if (ConfigurationStatClass.USE_PARALLEL_EXECUTION)
            {

                // Exp. par. version...
                FeatureLomontFFTClass featureLomontFFT16Obj = new FeatureLomontFFTClass(ConfigurationStatClass.C_NR_OF_SAMPLES_2_POWER_16, _WavFile_FullPathAndFileNameStr, i_config);
                FeatureLomontFFTClass featureLomontFFT14Obj = new FeatureLomontFFTClass(ConfigurationStatClass.C_NR_OF_SAMPLES_2_POWER_14, _WavFile_FullPathAndFileNameStr, i_config);
                FeatureLomontFFTClass featureLomontFFT12Obj = new FeatureLomontFFTClass(ConfigurationStatClass.C_NR_OF_SAMPLES_2_POWER_12, _WavFile_FullPathAndFileNameStr, i_config);

                #region ParallelTasks
                Parallel.Invoke(
                            () =>
                            {
                                // Console.WriteLine("Begin first task...");
                                waveFileObj.CalculateFeatureVector(featureLomontFFT16Obj);
                            },  // close first Action

                            () =>
                            {
                                // Console.WriteLine("Begin second task...");
                                waveFileObj.CalculateFeatureVector(featureLomontFFT14Obj);
                            }, //close second Action

                            () =>
                            {
                                // Console.WriteLine("Begin third task...");
                                waveFileObj.CalculateFeatureVector(featureLomontFFT12Obj);
                            } //close third Action
                         ); //close parallel.invoke


                //Console.WriteLine("Returned from Parallel.Invoke");
                #endregion
                FFeatureTypeVector.Add(featureLomontFFT16Obj);
                FFeatureTypeVector.Add(featureLomontFFT14Obj);
                FFeatureTypeVector.Add(featureLomontFFT12Obj);
            }
            else
            {
                // Normal sequential version...
                FeatureLomontFFTClass featureLomontFFT16Obj = new FeatureLomontFFTClass(ConfigurationStatClass.C_NR_OF_SAMPLES_2_POWER_16, _WavFile_FullPathAndFileNameStr, i_config);
                waveFileObj.CalculateFeatureVector(featureLomontFFT16Obj);
                FFeatureTypeVector.Add(featureLomontFFT16Obj);

                FeatureLomontFFTClass featureLomontFFT14Obj = new FeatureLomontFFTClass(ConfigurationStatClass.C_NR_OF_SAMPLES_2_POWER_14, _WavFile_FullPathAndFileNameStr, i_config);
                waveFileObj.CalculateFeatureVector(featureLomontFFT14Obj);
                FFeatureTypeVector.Add(featureLomontFFT14Obj);

                FeatureLomontFFTClass featureLomontFFT12Obj = new FeatureLomontFFTClass(ConfigurationStatClass.C_NR_OF_SAMPLES_2_POWER_12, _WavFile_FullPathAndFileNameStr, i_config);
                waveFileObj.CalculateFeatureVector(featureLomontFFT12Obj);
                FFeatureTypeVector.Add(featureLomontFFT12Obj);
            }
            

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

        public void UpdateFeatureVectors(ConfigurationDynClass i_config)
        {
            //FFeatureTypeVector.Clear();

            foreach (FeatureBaseClass f in FFeatureTypeVector)
            {
                f.UpdateFeatureWeight(i_config);
            }

            //FeaturePeakClass featurePeakObj = new FeaturePeakClass(i_config);
            ////waveFileObj.CalculateFeatureVector(featurePeakObj);
            //FFeatureTypeVector.Add(featurePeakObj);

            //FeatureAverageClass featureAverageObj = new FeatureAverageClass(i_config);
            ////waveFileObj.CalculateFeatureVector(featureAverageObj);
            //FFeatureTypeVector.Add(featureAverageObj);

            //FeatureRMSClass featureRMSObj = new FeatureRMSClass(i_config);
            ////waveFileObj.CalculateFeatureVector(featureRMSObj);
            //FFeatureTypeVector.Add(featureRMSObj);

            //FeaturePeak2PeakClass featurePeak2PeakObj = new FeaturePeak2PeakClass(i_config);
            ////waveFileObj.CalculateFeatureVector(featurePeak2PeakObj);
            //FFeatureTypeVector.Add(featurePeak2PeakObj);

            //FeatureCrestFactorClass featureCrestFactorObj = new FeatureCrestFactorClass(i_config);
            ////waveFileObj.CalculateFeatureVector(featureCrestFactorObj);
            //FFeatureTypeVector.Add(featureCrestFactorObj);

            //FeaturePassingZeroClass featurePassingZeroObj = new FeaturePassingZeroClass(i_config);
            ////waveFileObj.CalculateFeatureVector(featurePassingZeroObj);
            //FFeatureTypeVector.Add(featurePassingZeroObj);

            ////ToDo Evaluationfunctions to be developed
            //FeatureLomontFFTClass featureLomontFFT16Obj = new FeatureLomontFFTClass(ConfigurationStatClass.C_NR_OF_SAMPLES_2_POWER_16, _WavFile_FullPathAndFileNameStr, i_config);
            ////waveFileObj.CalculateFeatureVector(featureLomontFFT16Obj);
            //FFeatureTypeVector.Add(featureLomontFFT16Obj);

            //FeatureLomontFFTClass featureLomontFFT14Obj = new FeatureLomontFFTClass(ConfigurationStatClass.C_NR_OF_SAMPLES_2_POWER_14, _WavFile_FullPathAndFileNameStr, i_config);
            ////waveFileObj.CalculateFeatureVector(featureLomontFFT14Obj);
            //FFeatureTypeVector.Add(featureLomontFFT14Obj);

            //FeatureLomontFFTClass featureLomontFFT12Obj = new FeatureLomontFFTClass(ConfigurationStatClass.C_NR_OF_SAMPLES_2_POWER_12, _WavFile_FullPathAndFileNameStr, i_config);
            ////waveFileObj.CalculateFeatureVector(featureLomontFFT12Obj);
            //FFeatureTypeVector.Add(featureLomontFFT12Obj);

            //// At last normalize feature weights
            //double sum = 0;
            //foreach (FeatureBaseClass fbc in FFeatureTypeVector)
            //{
            //    sum = sum + fbc.FeatureWeight;
            //}
            //foreach (FeatureBaseClass fbc in FFeatureTypeVector)
            //{
            //    fbc.FeatureWeight = fbc.FeatureWeight / sum;
            //}

        }

        // ====================================================================

        public double CalculateDistanceToNewCaseForThisFeature(CaseClass i_NewCase, int i_FeatureIx)
        {
            double intervalSum = 0.0;

            intervalSum = 0.0;
            if (ConfigurationStatClass.USE_EUCLID_SUMMATION)
            {
                for (int ix = 0; ix < FFeatureTypeVector[i_FeatureIx].FeatureValueNormlizedVector.Count; ++ix)
                {
                    intervalSum = intervalSum + Math.Pow(FFeatureTypeVector[i_FeatureIx].AbsDiffForAttribute(i_NewCase.FFeatureTypeVector[i_FeatureIx].FeatureValueNormlizedVector[ix], FFeatureTypeVector[i_FeatureIx].FeatureValueNormlizedVector[ix]), 2.0);
                } // for ix
                intervalSum = Math.Sqrt(intervalSum);
            }
            else
            {
                for (int ix = 0; ix < FFeatureTypeVector[i_FeatureIx].FeatureValueNormlizedVector.Count; ++ix)
                {
                    intervalSum = intervalSum + 
                                    FFeatureTypeVector[i_FeatureIx].AbsDiffForAttribute(i_NewCase.FFeatureTypeVector[i_FeatureIx].FeatureValueNormlizedVector[ix], FFeatureTypeVector[i_FeatureIx].FeatureValueNormlizedVector[ix]);
                } // for ix
            }

            // ToDo: divide ??
            // intervalSum = intervalSum / ConfigurationStatClass.C_NR_OF_INTERVALS;
            return intervalSum;
        } // CalculateDistanceToNewCaseForThisFeature

        // ====================================================================

        public double CalculateSimilarityFunctionOfNewCaseForThisFeature(CaseClass i_NewCase, int i_FeatureIx)
        {
            double distance = CalculateDistanceToNewCaseForThisFeature(i_NewCase, i_FeatureIx);
            return 1 / (1 + distance);
        } // CalculateSimilarityFunctionOfNewCaseForThisFeature

        // ====================================================================

        public double CalculateDistanceValue(CaseClass i_NewCase)
        {
            double sum = 0;

            for (int jx = 0; jx < FFeatureTypeVector.Count; ++jx)
            {
                //for (int ix = 0; ix < FFeatureTypeVector[jx].FeatureValueVector.Count; ++ix)
                //{
                   // double temp = FFeatureTypeVector[jx].AbsDiffForAttribute(i_NewCase.FFeatureTypeVector[jx].FeatureValueVector[ix], FFeatureTypeVector[jx].FeatureValueVector[ix]);
                  //  sum = sum + temp * FFeatureTypeVector[jx].FeatureWeight;
                //} // for ix
                //sum = sum / FFeatureTypeVector[jx].FeatureValueVector.Count;
                //sum = sum * FFeatureTypeVector[jx].FeatureWeight;
                sum = sum + FFeatureTypeVector[jx].FeatureWeight * CalculateDistanceToNewCaseForThisFeature(i_NewCase, jx);
            } // for jx
            return sum;
        } // CalculateDistanceValue



        // ====================================================================

        public double CalculateSimilarityValue(CaseClass i_NewCase)
        {
            double distance = CalculateDistanceValue(i_NewCase);
            return 1 / (1 + distance);
        } // CalculateSimilarityValue     

        // ====================================================================

        public double CalculateSimilarityValueExt(CaseClass i_NewCase)
        {
            double simularity = 0;

            for (int jx = 0; jx < FFeatureTypeVector.Count; ++jx)
            {
                simularity = simularity + FFeatureTypeVector[jx].FeatureWeight * CalculateSimilarityFunctionOfNewCaseForThisFeature(i_NewCase, jx);
            } // for jx
            return simularity;
        } // CalculateSimilarityValueExt       
        
        // ====================================================================

        public override string ToString()
        {
            string resStr = "CaseClass - dump:\n";
            foreach (FeatureBaseClass fbc in FFeatureTypeVector)
            {
                resStr = resStr + "Feature Type = " + fbc.FeatureName + "\n";
                for (int ix = 0; ix < fbc.FeatureValueRawVector.Count; ++ix)
                {
                    resStr = resStr + " " + String.Format("{0:000000.0}", fbc.FeatureValueRawVector[ix]);
                } // for
                resStr = resStr + "\n";
            }
            return resStr;
        } // ToString

        // ====================================================================

        public string GetAllValuesOfThisFeatureTypeToString(int i_FeatureTypeIx, double i_ScaleFactor)
        {
            string resStr = "";
            FeatureBaseClass fbc;

            fbc = FFeatureTypeVector[i_FeatureTypeIx];

            resStr = String.Format("{0, 4:0} - {1,-40}", FOrderNr, System.IO.Path.GetFileName(_WavFile_FullPathAndFileNameStr));

            for (int ix = 0; ix < fbc.FeatureValueRawVector.Count; ++ix)
            {
                resStr = resStr + "\t" + String.Format("{0, 10:0.000}", fbc.FeatureValueRawVector[ix] * i_ScaleFactor);
            } // for
            return resStr;
        } // GetAllValuesOfThisFeatureTypeToString

        // ====================================================================

        public string GetChangesOfAllValuesOfThisFeatureTypeToString(int i_FeatureTypeIx)
        {
            string resStr = "";
            FeatureBaseClass fbc;

            fbc = FFeatureTypeVector[i_FeatureTypeIx];

            resStr = String.Format("{0, 4:0} - {1,-40}", FOrderNr, System.IO.Path.GetFileName(_WavFile_FullPathAndFileNameStr));

            for (int ix = 0; ix < fbc.FeatureValueRawVector.Count; ++ix)
            {
                // Even out the diffs into both sides intervalls.
                if (0 == ix)
                {
                    resStr = resStr + "\t" + String.Format("{0, 10:0.000}", (fbc.FeatureValueRawVector[ix + 1] - fbc.FeatureValueRawVector[ix]) / 2.0);
                }
                else if (fbc.FeatureValueRawVector.Count - 1 == ix)
                {
                    resStr = resStr + "\t" + String.Format("{0, 10:0.000}", (fbc.FeatureValueRawVector[ix] - fbc.FeatureValueRawVector[ix - 1]) / 2.0);
                }
                else
                {
                    resStr = resStr + "\t" + String.Format("{0, 10:0.000}", (fbc.FeatureValueRawVector[ix - 1] - fbc.FeatureValueRawVector[ix]) / 2.0 
                                                                             + 
                                                                            (fbc.FeatureValueRawVector[ix + 1] - fbc.FeatureValueRawVector[ix]) / 2.0);
                }

            } // for
            return resStr;
        } // GetChangesOfAllValuesOfThisFeatureTypeToString

        // ====================================================================

        public string GetDiffsOfAllValuesNormalizedOfThisFeatureTypeToString(int i_FirstFeatureTypeIx, int i_SecondFeatureTypeIx, List<CaseClass> i_TheCaseList)
        {
            // Return string with coordinate pairs: "x, y", x = similarity in first feature, y = similarity in second feature
            string resStr = "";

            resStr = String.Format("{0, 4:0} - {1,-40}", FOrderNr, System.IO.Path.GetFileName(_WavFile_FullPathAndFileNameStr));


            for (int otherCaseIx = 0; otherCaseIx < i_TheCaseList.Count; ++otherCaseIx)
            {
                resStr = resStr + "\t f:" + String.Format("{0, 4:0}", i_TheCaseList[otherCaseIx].OrderNr);

                double firstFeatureSimilarityValue = CalculateSimilarityFunctionOfNewCaseForThisFeature(i_TheCaseList[otherCaseIx], i_FirstFeatureTypeIx);
                double secondFeatureSimilarityValue = CalculateSimilarityFunctionOfNewCaseForThisFeature(i_TheCaseList[otherCaseIx], i_SecondFeatureTypeIx);
                resStr = resStr + "\t" + String.Format("{0, 10:0.000} \t {1, 10:0.000}",
                                                        firstFeatureSimilarityValue,      // "x"
                                                        secondFeatureSimilarityValue);    // "y"
                resStr = resStr + Environment.NewLine;

            } // for otherCaseIx
            return resStr;
        } // GetDiffsOfAllValuesNormalizedOfThisFeatureTypeToString

        // ====================================================================

        public double GetMaxFeatureValueOfThisFeature(int i_FeatureTypeIx)
        {
            double resDouble = 0.0;
            FeatureBaseClass fbc;

            fbc = FFeatureTypeVector[i_FeatureTypeIx];

            for (int ix = 0; ix < fbc.FeatureValueRawVector.Count; ++ix)
            {
                if (fbc.FeatureValueRawVector[ix] < 0)
                {
                    throw new System.NotImplementedException("GetMaxFeatureValueOfThisFeature, i_FeatureTypeIx=" + i_FeatureTypeIx + " ERROR < 0 at ix=" + ix + "!!!");
                }

                if (resDouble < fbc.FeatureValueRawVector[ix])
                {
                    resDouble = fbc.FeatureValueRawVector[ix];
                }
            } // for
            return resDouble;
        } // GetMaxFeatureValueOfThisFeature
        
        // ====================================================================

        public string AnalyseParamsToString()
        {
            string resStr = "";

            resStr = String.Format("{0, 4:0} - Tot: {1, 6:0}ms IBeg: {2, 6:0}ms Trigg: {3, 6:0}ms IEnd: {4, 6:0}ms Int: {5, 4:0}ms {6, 7:0} = {7, 3:0}%, of whole: {8, 3:0}% (was {9} channel(s)) {10} - {11}",
                                     FOrderNr,
                                     FWaveFileLengthInMilliSecs,
                                     FWaveFileIntervalBegPositionInMilliSecs,
                                     FWaveFileTriggPositionInMilliSecs,
                                     FWaveFileIntervalBegPositionInMilliSecs + FWaveFileIntervallLengthInMilliSecs * ConfigurationStatClass.C_NR_OF_INTERVALS,
                                     FWaveFileIntervallLengthInMilliSecs,
                                     "(" + (int)(FWaveFileIntervallLengthInMilliSecs * ConfigurationStatClass.C_SOUND_SAMPLE_FREQUENCY_IN_kHz) + ")",
                                     100.00 * (FWaveFileIntervallLengthInMilliSecs / FWaveFileLengthInMilliSecs),
                                     100.00 * (ConfigurationStatClass.C_NR_OF_INTERVALS * FWaveFileIntervallLengthInMilliSecs / FWaveFileLengthInMilliSecs),
                                     FNumberOfChannelsInOrgininalWaveFile,
                                     _SneezeStatus,
                                     System.IO.Path.GetFileName(_WavFile_FullPathAndFileNameStr));
            return resStr;
        } // AnalyseParamsToString

        // ====================================================================

    } // CaseClass
}
