using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlessYou
{
    public class FeatureLomontFFTClass : FeatureBaseClass
    {
        private double FNumberOfSamplesAsValuePowerOfTwo;

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

        public FeatureLomontFFTClass(double i_NumberOfSamplesAsValuePowerOfTwo) :
            base("LomontFFT - " + i_NumberOfSamplesAsValuePowerOfTwo)
        {
            NumberOfSamplesAsValuePowerOfTwo = i_NumberOfSamplesAsValuePowerOfTwo;
            base.FFeatureWeight = ConfigurationStatClass.C_DEFAULT_LOMONT_FFT_FEATURE_WEIGHT;
        } // FeaturePeakClass

        //=====================================================================

        public override void calculateFeatureValuesFromSamples(double[] i_WaveFileContents44p1KHz16bitSamples, int i_FirstListIx, int i_Count)
        {
            int samplingFrequency = 1000 * (int) ConfigurationStatClass.C_SOUND_SAMPLE_FREQUENCY_IN_kHz;
            int maxNrOfSamples = 65536; // 2^16

            LomontFFTClass LFFTObj = new LomontFFTClass();
            bool forward = true;
            int nrOfMaxDescendingFrequencies = 10;

            int nrOfSamples = (int)Math.Pow(2, NumberOfSamplesAsValuePowerOfTwo);
            int nrOfBins = nrOfSamples / 2;
            double[] dataForFFTAnalysis = new double[maxNrOfSamples];
            double[] dataFFTAnalysisDone = new double[maxNrOfSamples / 2];
            double[] dataFFTAnalysisDoneInDB = new double[maxNrOfSamples / 2];
            double[] frequencyArr = new double[maxNrOfSamples / 2];

            if (i_FirstListIx + nrOfSamples > i_WaveFileContents44p1KHz16bitSamples.Length)
            {
                i_FirstListIx = i_WaveFileContents44p1KHz16bitSamples.Length - nrOfSamples - 1;
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
                    dataForFFTAnalysis[ix] = i_WaveFileContents44p1KHz16bitSamples[i_FirstListIx + ix];
                } // else if
                else
                {
                    dataForFFTAnalysis[ix] = i_WaveFileContents44p1KHz16bitSamples[i_FirstListIx + ix / modfactor];
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
                dataFFTAnalysisDone[ix / 2] = dataForFFTAnalysis[ix];
                
                // Convert amplitude to dB values formula
                // dB_val = 10.0 * log10(re * re + im * im) + dB_correction
                dataFFTAnalysisDoneInDB[ix / 2] = 10 * Math.Log10(dataForFFTAnalysis[ix] * dataForFFTAnalysis[ix] + dataForFFTAnalysis[ix + 1] * dataForFFTAnalysis[ix + 1]);

                // retrieve frequencies
                frequencyArr[ix / 2] = ix / (double)nrOfSamples / 2 * samplingFrequency;
            } // for ix


            // Calulate the nrOfDominantFrequencies
            int bin = dataFFTAnalysisDoneInDB.ToList().IndexOf(dataFFTAnalysisDoneInDB.ToList().Max());
            int[] binArray = new int[nrOfMaxDescendingFrequencies];
            binArray = dataFFTAnalysisDoneInDB.Select((value, index) => new { value, index })
                    .OrderByDescending(item => item.value)
                    .Take(nrOfMaxDescendingFrequencies)
                    .Select(item => item.index)
                    .ToArray();
            double[] frequencyArray = new double[nrOfMaxDescendingFrequencies];
            int jx = 0; 
            foreach (var x in binArray)
            {
                frequencyArray[jx] = x / (double)maxNrOfSamples * samplingFrequency;
                jx++;
            }

            //Console.Write("Lomont===============================================samples= " + NumberOfSamplesAsValuePowerOfTwo);
            //Console.WriteLine("\nDominant bin: " + bin + " Dominant Frequency: " + frequencyArr[bin]);
            //Console.Write("Data: ");

            string[] strArr = new string[100000];
            for (int ix = 0; ix < nrOfSamples / 2; ++ix)
            {
                //Console.WriteLine(frequencyArr[ix] + "\t" + dataFFTAnalysisDone[ix] + "\t" + dataFFTAnalysisDoneInDB[ix] + "\n");
                strArr[ix] = frequencyArr[ix] + "\t" + dataFFTAnalysisDone[ix] + "\t" + dataFFTAnalysisDoneInDB[ix] + "\n";
            }
            System.IO.File.WriteAllLines("FFToutput" + NumberOfSamplesAsValuePowerOfTwo + ".xls", strArr);

            Console.WriteLine("");


            FFeatureValueVector.Add(frequencyArray.Max());
        } // calculateFeatureValuesFromSamples

        //=====================================================================

    } // FeatureLomontFFTClass
}
