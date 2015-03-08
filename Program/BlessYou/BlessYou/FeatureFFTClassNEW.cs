using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlessYou
{
    public class FeatureFFTClass : FeatureBaseClass
    {
        //
        //=====================================================================

        public FeatureFFTClass() : 
               base("FFT")
        {
            base.FFeatureWeight = ConfigurationStatClass.C_DEFAULT_FFT_FEATURE_WEIGHT;
        } // FeaturePeakClass

        //=====================================================================

        public override void calculateFeatureValuesFromSamples(double[] i_WaveFileContents44p1KHz16bitSamples, int i_FirstListIx, int i_Count)
        {
            int samplingFrequency = (int) ConfigurationStatClass.C_SOUND_SAMPLE_FREQUENCY_IN_kHz * 1000;
            LomontFFTClass LFFTObj = new LomontFFTClass();
            bool forward = true;
            int nrOfMaxDescendingFrequencies = 10;

            double sampleFactorToPowerOfTwo = Math.Pow(2, samplefactor); 

            // Sample size must be a power of 2
            // Base sampleSize  is 2^16 = 65536 ==> sample time is 1,49 s
            // If shorter dataForFFTAnalysis have to be padded with 0's
            int sampleSize = (int) Math.Pow(2, 16);
            int nrOfBins = sampleSize / 2;
            double[] dataForFFTAnalysis = new double[sampleSize];

            if (i_FirstListIx + sampleSize > i_WaveFileContents44p1KHz16bitSamples.Length)
            {
                i_FirstListIx = i_WaveFileContents44p1KHz16bitSamples.Length - sampleSize - 1;
            }

            for (int ix = 0; ix < sampleSize; ++ix)
            {
                dataForFFTAnalysis[ix] = 0;
            }

            int waveFileSampleIx = 0;
            for (int ix = 0; ix < sampleSize; ix += (int) sampleFactorToPowerOfTwo)
            {
                dataForFFTAnalysis[ix] = i_WaveFileContents44p1KHz16bitSamples[i_FirstListIx + waveFileSampleIx];
                waveFileSampleIx++;
            }

            // The Lomont FFT has the following parameters
            // data: real values of wave file data
            // Length of the data must be a power of 2, chosen 66536
            // forward: a bool  that is set to true (forward is done)
            LFFTObj.RealFFT(dataForFFTAnalysis, forward);

            // remove first two items and all imaginary parts
            double[] dataFFTAnalysisDone = new double[sampleSize / (2 *  (int) sampleFactorToPowerOfTwo)];
            for (int ix = 2; ix < sampleSize; ix += 2)
            {
                dataFFTAnalysisDone[(ix - 2) / (2 * (int)sampleFactorToPowerOfTwo)] = dataForFFTAnalysis[ix];
            } // for ix

            // Calulate the nrOfMaxDescendingFrequencies DominantFrequencies
            int bin = dataFFTAnalysisDone.ToList().IndexOf(dataFFTAnalysisDone.ToList().Max());
            int[] binArray = new int[nrOfMaxDescendingFrequencies];
            binArray = dataFFTAnalysisDone.Select((value, index) => new { value, index })
                    .OrderByDescending(item => item.value)
                    .Take(nrOfMaxDescendingFrequencies)
                    .Select(item => item.index)
                    .ToArray();

            double[] frequencyArray = new double[nrOfMaxDescendingFrequencies];
            int jx = 0; 
            foreach (var x in binArray)
            {
                frequencyArray[jx] = Math.Round(x / (double)sampleSize * samplingFrequency * sampleFactorToPowerOfTwo);
                jx++;
            }

            // ToDo Remove debug prints
            Console.WriteLine("\nDominant Bin: " + binArray[0] + "Bin: " + bin + " Dominant Frequency: " + frequencyArray[0]);
            Console.Write("Data: ");
            for (int ix = bin - 2; ix <= bin + 2; ++ix)
            {
                Console.Write(dataFFTAnalysisDone[ix] + " | ");
            }
            Console.WriteLine("");

            //Console.WriteLine("TopTen dominant bins: ");
            //foreach (int x in binArray)
            //{
            //    Console.Write(x + " | ");
            //}
            //Console.WriteLine(""); 
            //Console.WriteLine("TopTen dominant frequencies: ");
            //foreach (double x in frequencyArray)
            //{
            //    Console.Write(x + " | ");
            //}
            //Console.WriteLine("");


            FFeatureValueVector.Add(frequencyArray[0]);
        } // calculateFeatureValuesFromSamples

        //=====================================================================

    } // FeatureCrestFactorClass
}
