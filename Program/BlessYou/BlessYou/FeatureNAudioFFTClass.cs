using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio.Dsp;

namespace BlessYou
{
    class FeatureNAudioFFTClass : FeatureBaseClass
{
        //
        //=====================================================================

        public FeatureNAudioFFTClass() : 
               base("NAudioFFT")
        {
            base.FFeatureWeight = ConfigurationStatClass.C_DEFAULT_LOMONT_FFT_FEATURE_WEIGHT;
        } // FeaturePeakClass

        public FeatureNAudioFFTClass(ConfigurationStatClass i_config) :
            base("NAudioFFT")
        {
            base.FFeatureWeight = i_config.C_M_LOMONT_FFT_FEATURE_WEIGHT;
        } // FeaturePeakClass

        //=====================================================================

        public override void calculateFeatureValuesFromSamples(double[] i_WaveFileContents44p1KHz16bitSamples, int i_FirstListIx, int i_Count, int i_CurrentRound)
        {
            int samplingFrequency = 44100;
            int startIx = i_FirstListIx;
            bool forward = true;
            int nrOfMaxDescendingFrequencies = 10;

            int nrOfSamples = (int)Math.Pow(2, 16);
            int nrOfBins = nrOfSamples / 2;
            Complex[] dataForFFTAnalysis = new Complex[nrOfSamples];
            float[] dataFFTAnalysisDone = new float[nrOfSamples];
            double[] dataFFTAnalysisDoneInDB = new double[nrOfSamples];
            double[] frequencyArr = new double[nrOfSamples];

            if (i_FirstListIx + nrOfSamples > i_WaveFileContents44p1KHz16bitSamples.Length)
            {
                i_FirstListIx = i_WaveFileContents44p1KHz16bitSamples.Length - nrOfSamples - 1;
            }

            for (int ix = 0; ix < nrOfSamples; ++ix)
            {
                dataForFFTAnalysis[ix].X = (float) i_WaveFileContents44p1KHz16bitSamples[i_FirstListIx + ix];
                dataForFFTAnalysis[ix].Y = 0;
            }

            // The Lomont FFT has the following parameters
            // data: real values of wave file data
            // Length of the data must be a power of 2, chosen 66536
            // forward: a bool  that is set to true (forward is done)
            FastFourierTransform.FFT(forward, 16, dataForFFTAnalysis);

            // remove all imaginary parts
            for (int ix = 0; ix < nrOfSamples; ++ix)
            {
                // Convert power to dB values formula
                // dB_val = 10.0 * log10(re * re + im * im) + dB_correction
                dataFFTAnalysisDoneInDB[ix] =  10.0 * Math.Log10((double) (dataForFFTAnalysis[ix].X * dataForFFTAnalysis[ix].X + dataForFFTAnalysis[ix].Y * dataForFFTAnalysis[ix].Y));
                dataFFTAnalysisDone[ix] = Math.Abs(dataForFFTAnalysis[ix].X);
                frequencyArr[ix] = Math.Round(ix / (double)nrOfSamples * samplingFrequency);
            } // for ix



            //for (int ix = 0; ix < 20; ++ix)
            //{
            //    Console.WriteLine("i=" + ix + " data=" + dataFFTAnalysisDone[ix]);
            //    int x = nrOfSamples / 2 - ix;
            //    Console.WriteLine("x=" + x + " data=" + dataFFTAnalysisDone[x]);
            //} // for ix
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
            Console.Write("NAUDIO===============================================");
            //Console.WriteLine("\nDominant Bin: " + binArray[0] + " Dominant Frequency: " + frequencyArray[0]);
            //Console.Write("Data: ");
            //for (int ix = bin - 5; ix <= bin + 6; ++ix)
            //{
            //    Console.Write(dataFFTAnalysisDone[ix] + " | ");
            //}
            //Console.WriteLine("");

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

            for (int ix = 0; ix < nrOfSamples / 2; ++ix)
            {
                Console.WriteLine(dataFFTAnalysisDone[ix] + "\t" + dataFFTAnalysisDoneInDB[ix] + "\t" + frequencyArr[ix] + "\n");
            }
            Console.WriteLine("");


            FFeatureValueVector.Add(frequencyArray.Max());
        } // calculateFeatureValuesFromSamples

        //=====================================================================
        public override void UpdateFeatureWeight(ConfigurationStatClass i_config)
        {
            base.FFeatureWeight = i_config.C_M_LOMONT_FFT_FEATURE_WEIGHT;
        }
    } // FeatureLomontFFTClass
}
