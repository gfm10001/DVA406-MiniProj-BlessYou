// FeatureAverageClass.cs
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
    public class FeatureAverageClass : FeatureBaseClass
    {

        //=====================================================================

        public FeatureAverageClass() :
               base("Average")
        {
            base.FFeatureWeight = ConfigurationStatClass.C_DEFAULT_AVERAGE_FEATURE_WEIGHT;
        } // FeatureAverageClass

        //=====================================================================

        public FeatureAverageClass(ConfigurationDynClass config)
            : base("Average")
        {

            base.FFeatureWeight = config.M_AVERAGE_FEATURE_WEIGHT;
        }

        //=====================================================================

        public override void calculateFeatureValuesFromSamples(double[] i_WaveFileContents44p1KHz16bitSamples, int i_FirstListIx, int i_Count, int i_CurrentRound)
        {
            int startIx = i_FirstListIx;
            double average = 0.0;

            for (int ix = i_FirstListIx; ix < i_FirstListIx + i_Count; ++ix)
            {
                average = average + Math.Abs(i_WaveFileContents44p1KHz16bitSamples[ix]);
            }
            average = average / i_Count;
            FFeatureValueRawVector.Add(average);
        } // calculateFeatureValues

        //=====================================================================

        public override void UpdateFeatureWeight(ConfigurationDynClass i_config)
        {
            base.FeatureWeight = i_config.M_AVERAGE_FEATURE_WEIGHT;
        }


        //=====================================================================

    } // FeatureAverageClass
}
