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
            int samplingFrequency = 44100;
            int startIx = i_FirstListIx;
            LomontFFTClass LFFTObj = new LomontFFTClass();
            bool forward = true;
            int nrOfMaxDescendingFrequencies = 10;

            int nrOfSamples = (int)Math.Pow(2, 16);
            int nrOfBins = nrOfSamples / 2;
            double[] dataForFFTAnalysis = new double[nrOfSamples];
            double[] dataFFTAnalysisDone = new double[nrOfSamples / 2];

            if (i_FirstListIx + nrOfSamples > i_WaveFileContents44p1KHz16bitSamples.Length)
            {
                i_FirstListIx = i_WaveFileContents44p1KHz16bitSamples.Length - nrOfSamples - 1;
            }

            for (int ix = 0; ix < nrOfSamples; ++ix)
            {
                dataForFFTAnalysis[ix] = i_WaveFileContents44p1KHz16bitSamples[i_FirstListIx + ix];
            }

            // The Lomont FFT has the following parameters
            // data: real values of wave file data
            // Length of the data must be a power of 2, chosen 66536
            // forward: a bool  that is set to true (forward is done)
            LFFTObj.RealFFT(dataForFFTAnalysis, forward);

            // remove all imagine parts
            for (int ix = 0; ix < nrOfSamples; ix += 2)
            {
                dataFFTAnalysisDone[ix / 2] = dataForFFTAnalysis[ix];
            } // for ix


            // Calulate the nrOfDominantFrequencies
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
                frequencyArray[jx] = Math.Round(x / (double)nrOfSamples * samplingFrequency);
                jx++;
            }

            // ToDo Remove debug prints
            Console.WriteLine("\nDominant Bin: " + binArray[0] + " Dominant Frequency: " + frequencyArray[0]);
            Console.Write("Data: ");
            for (int ix = bin; ix <= bin + 10; ++ix)
            {
                Console.Write(dataFFTAnalysisDone[ix] + " | ");
            }
            Console.WriteLine("");

            Console.WriteLine("TopTen dominant bins: ");
            foreach (int x in binArray)
            {
                Console.Write(x + " | ");
            }
            Console.WriteLine(""); 
            Console.WriteLine("TopTen dominant frequencies: ");
            foreach (double x in frequencyArray)
            {
                Console.Write(x + " | ");
            }
            Console.WriteLine("");


            FFeatureValueVector.Add(frequencyArray.Max());
        } // calculateFeatureValuesFromSamples

        //=====================================================================

    } // FeatureCrestFactorClass
}
