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
        protected double FFeatureWeight;

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

        public double FeatureWeight
        {
            get
            {
                return FFeatureWeight;
            }
            set
            {
                FFeatureWeight = value;
            }
        } // FeatureName

        //=====================================================================

        public FeatureBaseClass(string i_FeatureName)
        {
            FFeatureValueVector = new List<double>();
            FFeatureName = i_FeatureName;
        } // FeatureBaseClass

        //=====================================================================

        public abstract void calculateFeatureValuesFromSamples(List<double> i_WaveFileContents44p1KHz16bitSamples, int i_FirstListIx, int i_Count);

        //=====================================================================

        public double SimilarityFunctionForAttribute(double i_NewValue, double i_RetrievedValue)
        {
            return FFeatureWeight * Math.Abs(i_NewValue - i_RetrievedValue);
        }

    } // FeatureBaseClass
}
