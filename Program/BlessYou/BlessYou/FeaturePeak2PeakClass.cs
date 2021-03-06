﻿// FeaturePeak2PeakClass.cs
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
    public class FeaturePeak2PeakClass : FeatureBaseClass
    {

        //=====================================================================

        public FeaturePeak2PeakClass() :
               base("Peak2Peak")
        {
            base.FFeatureWeight = ConfigurationStatClass.C_DEFAULT_PEAK2PEAK_FEATURE_WEIGHT;
        } // FeatureAverageClass

        //=====================================================================

        public FeaturePeak2PeakClass(ConfigurationDynClass i_config) :
            base("Peak2Peak")
        {
            base.FFeatureWeight = i_config.M_PEAK2PEAK_FEATURE_WEIGHT;
        } // FeatureAverageClass

        //=====================================================================

        public override void calculateFeatureValuesFromSamples(double[] i_WaveFileContents44p1KHz16bitSamples, int i_FirstListIx, int i_Count, int i_CurrentRound)
        {
            int startIx = i_FirstListIx;
            double xmin = double.MaxValue;
            double xmax = double.MinValue;
            double p2p;

            //RMS Formula: abs(xmin) + abs(xmax). 

            for (int ix = i_FirstListIx; ix < i_FirstListIx + i_Count; ++ix)
            {
                if (xmin > i_WaveFileContents44p1KHz16bitSamples[ix])
                {
                    xmin = i_WaveFileContents44p1KHz16bitSamples[ix];
                }
                if (xmax < i_WaveFileContents44p1KHz16bitSamples[ix])
                {
                    xmax = i_WaveFileContents44p1KHz16bitSamples[ix];
                }
            } // for ix
            p2p = Math.Abs(xmin) + Math.Abs(xmax);
            FFeatureValueRawVector.Add(p2p);
        } // calculateFeatureValues

        //=====================================================================

        public override void UpdateFeatureWeight(ConfigurationDynClass i_config)
        {
            base.FFeatureWeight = i_config.M_PEAK2PEAK_FEATURE_WEIGHT;
        }

        //=====================================================================

    } // FeaturePeak2PeakClass
}
