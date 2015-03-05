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

        public abstract void calculateFeatureValuesFromSamples(double[] i_WaveFileContents44p1KHz16bitSamples, int i_FirstListIx, int i_Count);

        //=====================================================================

        public double AbsDiffForAttribute(double i_NewValue, double i_RetrievedValue)
        {
            // The goal is to return 1 if the difference is very small
            double absDiff;
            double retDouble = 1;
            absDiff =  Math.Abs(i_NewValue - i_RetrievedValue);
            if (absDiff > ConfigurationStatClass.C_EPSILON)
            {
                retDouble = 1.0 / absDiff;
            }
            return retDouble;
        }

    } // FeatureBaseClass
}
