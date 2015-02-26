using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlessYou
{
    public abstract class FeatureBaseClass
    {
        protected string FFeatureName;
        protected List<double> FFeatureValueVector;

        //=====================================================================

        public string FeatureName
        {
            get
            {
                return FFeatureName;
            }
        } // FeatureName

        //=====================================================================

        public List<double> FeatureValueVector
        {
            get
            {
                return FFeatureValueVector;
            }
        } // FeatureValueVector

        //=====================================================================

        public FeatureBaseClass(string i_FeatureName)
        {
            FFeatureValueVector = new List<double>();
            FFeatureName = i_FeatureName;
        } // FeatureBaseClass

        //=====================================================================

        public abstract void calculateFeatureValues(List<double> i_WaveFileContents44p1KHz16bitSamples, int i_FirstListIx, int i_Count);

        //=====================================================================

    } // FeatureBaseClass
}
