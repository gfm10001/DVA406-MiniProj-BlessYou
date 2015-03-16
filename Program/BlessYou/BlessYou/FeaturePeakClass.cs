using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlessYou
{
    public class FeaturePeakClass : FeatureBaseClass
    {

        //=====================================================================

        public FeaturePeakClass() : 
               base("Peak")
        {
            base.FFeatureWeight = ConfigurationStatClass.C_DEFAULT_PEAK_FEATURE_WEIGHT;
        } // FeaturePeakClass

        public FeaturePeakClass(ConfigurationDynClass i_config) :
            base("Peak")
        {
            base.FFeatureWeight = i_config.C_M_PEAK_FEATURE_WEIGHT;
        } // FeaturePeakClass

        //=====================================================================

        public override void calculateFeatureValuesFromSamples(double[] i_WaveFileContents44p1KHz16bitSamples, int i_FirstListIx, int i_Count, int i_CurrentRound)
        {
            int startIx = i_FirstListIx;
            double peak = -1.0;

            for (int ix = i_FirstListIx; ix < i_FirstListIx + i_Count; ++ix)
            {
                if (Math.Abs(i_WaveFileContents44p1KHz16bitSamples[ix]) > peak)
                {
                    peak = Math.Abs(i_WaveFileContents44p1KHz16bitSamples[ix]);
                }
            } // for ix
            FFeatureValueRawVector.Add(peak);
        } // calculateFeatureValuesFromSamples

        public override void UpdateFeatureWeight(ConfigurationDynClass i_config)
        {
            base.FFeatureWeight = i_config.C_M_PEAK_FEATURE_WEIGHT;
        }
        //=====================================================================

    } // FeaturePeakClass
}
