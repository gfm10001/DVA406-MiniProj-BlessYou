﻿// FeatureRMSClass.cs
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
    public class FeatureRMSClass : FeatureBaseClass
    {

        //=====================================================================

        public FeatureRMSClass() : 
               base("RMS")
        {
            base.FFeatureWeight = ConfigurationStatClass.C_DEFAULT_RMS_FEATURE_WEIGHT;
        } // FeaturePeakClass

        //=====================================================================

        public FeatureRMSClass(ConfigurationDynClass i_config) :
            base("RMS")
        {
            base.FFeatureWeight = i_config.M_RMS_FEATURE_WEIGHT;
        } // FeaturePeakClass

        //=====================================================================

        public override void calculateFeatureValuesFromSamples(double[] i_WaveFileContents44p1KHz16bitSamples, int i_FirstListIx, int i_Count, int i_CurrentRound)
        {
            int startIx = i_FirstListIx;
            double rms = 0;

            //RMS Formula: sqrt{ (x1^2 + x2^2 + ... + xn^2) / n }. 

            for (int ix = i_FirstListIx; ix < i_FirstListIx + i_Count; ++ix)
            {
                rms = rms + i_WaveFileContents44p1KHz16bitSamples[ix] * i_WaveFileContents44p1KHz16bitSamples[ix];
            } // for ix
            rms = Math.Sqrt(rms / i_Count);

            FFeatureValueRawVector.Add(rms);
        } // calculateFeatureValuesFromSamples

        //=====================================================================

        public override void UpdateFeatureWeight(ConfigurationDynClass i_config)
        {
            base.FFeatureWeight = i_config.M_RMS_FEATURE_WEIGHT;
        }
        //=====================================================================

    } // FeatureRMSClass
}
