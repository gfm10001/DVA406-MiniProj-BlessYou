﻿using System;
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

        } // FeatureAverageClass

        //=====================================================================

        public override void calculateFeatureValues(List<double> i_WaveFileContents44p1KHz16bitSamples, int i_FirstListIx, int i_Count)
        {
            int startIx = i_FirstListIx;
            double average = 0.0;

            for (int ix = i_FirstListIx; ix < i_FirstListIx + i_Count; ++ix)
            {
                average = average + Math.Abs(i_WaveFileContents44p1KHz16bitSamples[ix]);

            }
            average = average / i_Count;
            FFeatureValueVector.Add(average);
        } // calculateFeatureValues

        //=====================================================================

    } // FeatureAverageClass
}