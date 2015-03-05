// RetrievedCaseClass.cs
//
// DVA406 Intelligent Systems, Mdh, vt15
//
// History:
// 2015-02-24   Introduced.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlessYou
{
    public class RetrievedCaseClass : CaseClass
    {
        double FSimilarityValue;
        EnumCaseStatus FProposedStatus;
        double _ScaledSimilarity;

        //=====================================================================

        #region Properties
        public EnumCaseStatus CaseStatus
        {
            get { return FProposedStatus; }

            set { FProposedStatus = value; }
        
        }

        //=====================================================================

        public double RawSimilarityValue
        {
            get
            {
                return FSimilarityValue;
            }
            set
            {
                FSimilarityValue = value;
            }
        } // FeatureName


        public double ScaledSimilarityValue
        {
            get { return _ScaledSimilarity; }
            set { _ScaledSimilarity = value; }
        
        
        }
        #endregion

        //=====================================================================

        public RetrievedCaseClass()
        {

        }

        //=====================================================================

        public RetrievedCaseClass(CaseClass i_CaseClassObj)
        {
            this.WavFile_FullPathAndFileNameStr = i_CaseClassObj.WavFile_FullPathAndFileNameStr;
            this.SneezeStatus = i_CaseClassObj.SneezeStatus;
            this.FeatureTypeVector = i_CaseClassObj.FeatureTypeVector;
        }

        //=====================================================================
    
        public string GetCurrentMatchingString()
        {
            return "Filename: " + base.WavFile_FullPathAndFileNameStr + " Similarityvalue: " + FSimilarityValue.ToString() + " Proposed result: " + FProposedStatus.ToString()
                                + " Actual: " + base.SneezeStatus.ToString();
        }

        void CalculateSimilarityValue(CaseClass i_NewCase)
        {

        } // CalculateSimilarityValue
    } // RetrievedCaseClass

}
