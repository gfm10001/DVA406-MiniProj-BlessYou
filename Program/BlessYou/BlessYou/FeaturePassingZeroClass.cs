// FeaturePassingZeroClass.cs
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
    class FeaturePassingZeroClass : FeatureBaseClass
    {

        //=====================================================================

        public FeaturePassingZeroClass() :
            base("PassingZero")
        {
            base.FFeatureWeight = ConfigurationStatClass.C_DEFAULT_PASSING_ZERO_WEIGHT;
        } // FeaturePassingZeroClass

        //=====================================================================

        public FeaturePassingZeroClass(ConfigurationDynClass i_config) :
            base("PassingZero")
        {
            base.FFeatureWeight = i_config.C_M_PASSING_ZERO_WEIGHT;
        } // FeaturePassingZeroClass

        //=====================================================================

        private bool _IsPositive(double value)
        {
            return value > 0; 
        } // _IsPositive

        //=====================================================================

        public override void calculateFeatureValuesFromSamples(double[] i_WaveFileContents44p1KHz16bitSamples, int i_FirstListIx, int i_Count, int i_CurrentRound)
        {
            int startIx = i_FirstListIx;
            int changes=0;

            for (int ix = i_FirstListIx+1; ix < i_FirstListIx + i_Count; ++ix)
            {
                if(_IsPositive(i_WaveFileContents44p1KHz16bitSamples[ix]) != _IsPositive(i_WaveFileContents44p1KHz16bitSamples[ix-1]))
                {
                    changes++;                
                }
            } // for ix
            FFeatureValueRawVector.Add(changes);
        } // calculateFeatureValuesFromSamples

        //=====================================================================

        public override void UpdateFeatureWeight(ConfigurationDynClass i_config)
        {
            base.FFeatureWeight = i_config.C_M_PASSING_ZERO_WEIGHT;
        } // UpdateFeatureWeight

        //=====================================================================

    } // FeatureCrestFactorClass
}
