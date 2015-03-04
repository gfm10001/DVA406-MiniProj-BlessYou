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

        //=====================================================================




        #region Properties
        public EnumCaseStatus CaseStatus
        {
            get { return FProposedStatus; }

            set { FProposedStatus = value; }
        
        }


        public double SimilarityValue
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
        #endregion

        public RetrievedCaseClass()
        {

        }

        //=====================================================================
    
        public string GetCurrentMatchingString()
        {
            return "Filename: " + base._WavFile_FullPathAndFileNameStr + " Similarityvalue: " + FSimilarityValue.ToString() + " Proposed result: " + FProposedStatus.ToString()
                                + " Actual: " + base._SneezeStatus.ToString();
        }

        void CalculateSimilarityValue(CaseClass i_NewCase)
        {

        } // CalculateSimilarityValue
    } // RetrievedCaseClass

}
