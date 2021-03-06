﻿// FeatureBaseClass.cs
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
        } // FeatureWeight

        //=====================================================================

        public FeatureBaseClass(string i_FeatureName)
        {
            FFeatureValueRawVector = new List<double>();
            FFeatureValueNormlizedVector = new List<double>();
            FFeatureName = i_FeatureName;
        } // FeatureBaseClass

        //=====================================================================

        public FeatureBaseClass(string i_FeatureName, ConfigurationDynClass i_config)
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
            absDiff =  Math.Abs(i_NewValue - i_RetrievedValue);
            return absDiff;
        } // AbsDiffForAttribute

        //=====================================================================

        public virtual void UpdateFeatureWeight(ConfigurationDynClass i_config)
        {
            throw new NotImplementedException();

        } // UpdateFeatureWeight

    } // FeatureBaseClass
}
