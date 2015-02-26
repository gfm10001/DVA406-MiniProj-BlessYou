﻿using System;
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

        } // FeaturePeakClass

        //=====================================================================

        public override void calculateFeatureValues(List<double> i_WaveFileContents44p1KHz16bitSamples, int i_FirstListIx, int i_Count)
        {
            int startIx = i_FirstListIx;
            double peak = -1.0;

            for (int ix = i_FirstListIx; ix < i_FirstListIx + i_Count; ++ix)
            {
                if (Math.Abs(i_WaveFileContents44p1KHz16bitSamples[ix]) > peak)
                {
                    peak = Math.Abs(i_WaveFileContents44p1KHz16bitSamples[ix]);
                }
            }
            FFeatureValueVector.Add(peak);
        } // calculateFeatureValues

        //=====================================================================

    } // FeaturePeakClass
}