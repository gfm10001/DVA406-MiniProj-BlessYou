using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BlessYou
{
    public class FeatureLomontFFTClass : FeatureBaseClass
    {
        private double FNumberOfSamplesAsValuePowerOfTwo;
        private double[] FDataFFTAnalysisDone;
        private double[] FDataFFTAnalysisDoneInDB;
        private double[] FFrequencyArr;
        string FFilePathAndName;

        //=====================================================================

        public double NumberOfSamplesAsValuePowerOfTwo
        {
            get
            {
                return FNumberOfSamplesAsValuePowerOfTwo;
            }
            set
            {
                FNumberOfSamplesAsValuePowerOfTwo = value;
            }
        } // NumberOfSamplesInPowerOfTwo

        //=====================================================================

        public double[] DataFFTAnalysisDoneInDB
        {
            get
            {
                return FDataFFTAnalysisDoneInDB;
            }
            set
            {
                FDataFFTAnalysisDoneInDB = value;
            }
        } // DataFFTAnalysisDoneInDB

        //=====================================================================

        public double[] FrequencyArr
        {
            get
            {
                return FFrequencyArr;
            }
            set
            {
                FFrequencyArr = value;
            }
        } // FrequencyArr

        //=====================================================================

        public FeatureLomontFFTClass(double i_NumberOfSamplesAsValuePowerOfTwo, string i_FilePathAndName) :
            base("LomontFFT - " + i_NumberOfSamplesAsValuePowerOfTwo)
        {
            NumberOfSamplesAsValuePowerOfTwo = i_NumberOfSamplesAsValuePowerOfTwo;
            base.FFeatureWeight = ConfigurationStatClass.C_DEFAULT_LOMONT_FFT_FEATURE_WEIGHT;
            FFilePathAndName = i_FilePathAndName;
        } // FeaturePeakClass

        public FeatureLomontFFTClass(double i_NumberOfSamplesAsValuePowerOfTwo,ConfigurationStatClass i_config) :
            base("LomontFFT - " + i_NumberOfSamplesAsValuePowerOfTwo)
        {
            NumberOfSamplesAsValuePowerOfTwo = i_NumberOfSamplesAsValuePowerOfTwo;
            base.FFeatureWeight = i_config.C_M_LOMONT_FFT_FEATURE_WEIGHT;
        } // FeaturePeakClass

        public FeatureLomontFFTClass(double i_NumberOfSamplesAsValuePowerOfTwo,ConfigurationStatClass i_config) :
            base("LomontFFT - " + i_NumberOfSamplesAsValuePowerOfTwo)
        {
            NumberOfSamplesAsValuePowerOfTwo = i_NumberOfSamplesAsValuePowerOfTwo;
            base.FFeatureWeight = i_config.C_M_LOMONT_FFT_FEATURE_WEIGHT;
        } // FeaturePeakClass

        //=====================================================================

        public override void calculateFeatureValuesFromSamples(double[] i_WaveFileContents44p1KHz16bitSamples, int i_FirstListIx, int i_Count, int i_CurrentRound)
        {
            bool sendRawDataTOFile = false; // true

            int samplingFrequency = 1000 * (int) ConfigurationStatClass.C_SOUND_SAMPLE_FREQUENCY_IN_kHz;
            int maxNrOfSamples = 65536; // 2^16
         
            LomontFFTClass LFFTObj = new LomontFFTClass();
            bool forward = true;
            int analysisStartIndex = 0;
            int analysisEndIndex = 0;

            int nrOfSamples = (int)Math.Pow(2, NumberOfSamplesAsValuePowerOfTwo);
            int nrOfBins = nrOfSamples / 2;
            double[] dataForFFTAnalysis = new double[maxNrOfSamples];
            FDataFFTAnalysisDone = new double[nrOfSamples / 2];
            FDataFFTAnalysisDoneInDB = new double[nrOfSamples / 2];
            FFrequencyArr = new double[nrOfSamples / 2];



            int firstListIx = i_FirstListIx;
            if (i_FirstListIx + nrOfSamples > i_WaveFileContents44p1KHz16bitSamples.Length)
            {
                firstListIx = i_WaveFileContents44p1KHz16bitSamples.Length - nrOfSamples - 1;
            }

            if (firstListIx < 0)
            {
                FFeatureValueVector.Add(0.0);
                return;
            }
            // Pad input as needed
            int modfactor = (int)(Math.Pow(2, (16 - (int)NumberOfSamplesAsValuePowerOfTwo)));
            for (int ix = 0; ix < nrOfSamples; ++ix)
            {
                if (NumberOfSamplesAsValuePowerOfTwo != 16.0 && (ix % modfactor) != 0)
                {
                    dataForFFTAnalysis[ix] = 0.0;
                } // if
                else if (NumberOfSamplesAsValuePowerOfTwo == 16.0)
                {
                    dataForFFTAnalysis[ix] = i_WaveFileContents44p1KHz16bitSamples[firstListIx + ix];
                } // else if
                else
                {
                    dataForFFTAnalysis[ix] = i_WaveFileContents44p1KHz16bitSamples[firstListIx + ix / modfactor];
                }
            } // for ix

            // The Lomont FFT has the following parameters
            // data: real values of wave file data
            // Length of the data must be a power of 2, chosen 66536
            // forward: a bool  that is set to true (forward is done)
            LFFTObj.RealFFT(dataForFFTAnalysis, forward);

            // remove all imaginary parts and set up output
            for (int ix = 0; ix < nrOfSamples; ix += 2)
            {
                // Amplitude
                FDataFFTAnalysisDone[ix / 2] = dataForFFTAnalysis[ix];
                
                // Convert amplitude to dB values formula
                // dB_val = 10.0 * log10(re * re + im * im) + dB_correction
                FDataFFTAnalysisDoneInDB[ix / 2] = 10 * Math.Log10(dataForFFTAnalysis[ix] * dataForFFTAnalysis[ix] + dataForFFTAnalysis[ix + 1] * dataForFFTAnalysis[ix + 1]);

                // retrieve frequencies
                FFrequencyArr[ix / 2] = ix / (double)nrOfSamples / 2 * samplingFrequency;

                // Retrieve startindex corresponding to C_STARTING_FFT_ANALYSIS_FREQUENCY
                if (FFrequencyArr[ix / 2] < ConfigurationStatClass.C_STARTING_FFT_ANALYSIS_FREQUENCY_IN_HERTZ)
                {
                    analysisStartIndex = ix / 2;
                } // if
                // Retrieve startindex corresponding to C_ENDING_FFT_ANALYSIS_FREQUENCY
                if (FFrequencyArr[ix / 2] < ConfigurationStatClass.C_ENDING_FFT_ANALYSIS_FREQUENCY_IN_HERTZ)
                {
                    analysisEndIndex = ix / 2;
                } // if
            } // for ix


            // Calulate the nrOfDominantFrequencies
            int bin = FDataFFTAnalysisDoneInDB.ToList().IndexOf(FDataFFTAnalysisDoneInDB.ToList().Max());
            //int[] binArray = new int[nrOfMaxDescendingFrequencies];
            //binArray = FDataFFTAnalysisDoneInDB.Select((value, index) => new { value, index })
            //        .OrderByDescending(item => item.value)
            //        .Take(nrOfMaxDescendingFrequencies)
            //        .Select(item => item.index)
            //        .ToArray();
            //double[] frequencyArray = new double[nrOfMaxDescendingFrequencies];
            //int jx = 0; 
            //foreach (var x in binArray)
            //{
            //    frequencyArray[jx] = x / (double)maxNrOfSamples * samplingFrequency;
            //    jx++;
            //}

            //Console.Write("Lomont===============================================samples= " + NumberOfSamplesAsValuePowerOfTwo);
            //Console.WriteLine("\nDominant bin: " + bin + " Dominant Frequency: " + FFrequencyArr[bin]);
            //Console.Write("Data: ");

            string[] strArr = new string[100000];
            for (int ix = 0; ix < nrOfSamples / 2; ++ix)
            {
                //Console.WriteLine(frequencyArr[ix] + "\t" + dataFFTAnalysisDone[ix] + "\t" + dataFFTAnalysisDoneInDB[ix] + "\n");
                strArr[ix] = FFrequencyArr[ix] + "\t" + FDataFFTAnalysisDoneInDB[ix] + "\t" + FDataFFTAnalysisDone[ix];
            }

            string fileName = Path.GetFileName(FFilePathAndName);
            if (sendRawDataTOFile)
            {
                System.IO.File.WriteAllLines("FFToutput_" + fileName + "_S_" + NumberOfSamplesAsValuePowerOfTwo + "_R_" + i_CurrentRound + ".xls", strArr);
            } // if

            // Setup the Feature Value and return it to the vector
            double featureValue = FDataFFTAnalysisDoneInDB.ToList().GetRange(analysisStartIndex, analysisEndIndex - analysisStartIndex).Average();
            FFeatureValueVector.Add(featureValue);
        } // calculateFeatureValuesFromSamples

        //=====================================================================

    } // FeatureLomontFFTClass
}
