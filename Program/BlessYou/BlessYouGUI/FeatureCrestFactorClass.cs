// FeatureCrestFactorClass.cs
//
// DVA406 Intelligent Systems, Mdh, vt15
//
// History:
// 2015-02-24       Introduced.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlessYou
{
    class FeatureCrestFactorClass : FeatureBaseClass
    {

        //=====================================================================

        public FeatureCrestFactorClass() : 
               base("CF")
        {
            base.FFeatureWeight = ConfigurationStatClass.C_DEFAULT_CREST_FACTOR_WEIGHT;
        } // FeatureCrestFactorClass

        //=====================================================================

        public FeatureCrestFactorClass(ConfigurationDynClass i_stats)
            : base("CF")
        {
            base.FFeatureWeight = i_stats.M_CREST_FACTOR_WEIGHT;
        
        } // FeatureCrestFactorClass

        //=====================================================================

        public override void calculateFeatureValuesFromSamples(double[] i_WaveFileContents44p1KHz16bitSamples, int i_FirstListIx, int i_Count, int i_CurrentRound)
        {
            int startIx = i_FirstListIx;
            double rms = 0.0;
            double peak = 0.0;
            double cf;

            //CF Formula: Smax / RMS. Smax = Abs(peakvalue)

            for (int ix = i_FirstListIx; ix < i_FirstListIx + i_Count; ++ix)
            {
                if (Math.Abs(i_WaveFileContents44p1KHz16bitSamples[ix]) > peak)
                {
                    peak = Math.Abs(i_WaveFileContents44p1KHz16bitSamples[ix]);
                }
                rms = rms + i_WaveFileContents44p1KHz16bitSamples[ix] * i_WaveFileContents44p1KHz16bitSamples[ix];
            } // for ix
            rms = Math.Sqrt(rms / i_Count);
            cf = peak / rms;

            FFeatureValueRawVector.Add(cf);
        } // calculateFeatureValuesFromSamples

        //=====================================================================

        public override void UpdateFeatureWeight(ConfigurationDynClass i_config)
        {
            base.FeatureWeight = i_config.M_CREST_FACTOR_WEIGHT;
        }


        //=====================================================================

    } // FeatureCrestFactorClass
}
