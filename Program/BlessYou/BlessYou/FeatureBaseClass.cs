// FeatureBaseClass.cs
//
// DVA406 Intelligent Systems, Mdh, vt15
//
// History:
// 2015-02-24       Introduced.
// 2015-03-13/GF    Added normalized vector (all values 0.0..1.0)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlessYou
{
    public abstract class FeatureBaseClass
    {
        protected string FFeatureName;
        protected List<double> FFeatureValueRawVector;
        protected List<double> FFeatureValueNormlizedVector;
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

        public List<double> FeatureValueRawVector
        {
            get
            {
                return FFeatureValueRawVector;
            }
        } // FeatureValueRawVector

        //=====================================================================

        public List<double> FeatureValueNormlizedVector
        {
            get
            {
                return FFeatureValueNormlizedVector;
            }
        } // FeatureValueNormlizedVector

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
            FFeatureValueRawVector = new List<double>();
            FFeatureValueNormlizedVector = new List<double>();
            FFeatureName = i_FeatureName;
        } // FeatureBaseClass


        public FeatureBaseClass(string i_FeatureName,ConfigurationStatClass i_config)
        {
            FFeatureValueRawVector = new List<double>();
            FFeatureValueNormlizedVector = new List<double>();
            FFeatureName = i_FeatureName;
        } // FeatureBaseClass

        //=====================================================================

        public abstract void calculateFeatureValuesFromSamples(double[] i_WaveFileContents44p1KHz16bitSamples, int i_FirstListIx, int i_Count, int i_CurrentRound);

        //=====================================================================

        public double AbsDiffForAttribute(double i_NewValue, double i_RetrievedValue)
        {
            // The goal is to return 1 if the difference is very small
            double absDiff;
            //double retDouble = 1;
            absDiff =  Math.Abs(i_NewValue - i_RetrievedValue);
            //if (absDiff > ConfigurationStatClass.C_EPSILON)
            //{
            //    retDouble = 1.0 / absDiff;
            //}
            return absDiff;
        }
        public virtual void UpdateFeatureWeight(ConfigurationStatClass i_config)
        {
            throw new NotImplementedException();
        
        }

    } // FeatureBaseClass
}
