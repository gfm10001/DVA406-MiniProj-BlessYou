﻿// RetrievedCaseClass.cs
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
        public EnumCaseStatus ProposedStatus
        {
            get { return FProposedStatus; }

            set { FProposedStatus = value; }
        
        }

        //=====================================================================

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

        //=====================================================================

        public RetrievedCaseClass()
        {

        }

        //=====================================================================

        public RetrievedCaseClass(CaseClass i_CaseClassObj)
        {
            // ToDo better implementation
            base.WavFile_FullPathAndFileNameStr = i_CaseClassObj.WavFile_FullPathAndFileNameStr;
            base.FeatureTypeVector = i_CaseClassObj.FeatureTypeVector;
            base.SneezeStatus = i_CaseClassObj.SneezeStatus;
            base.SneezeStatus = i_CaseClassObj.SneezeStatus;
            this.WavFile_FullPathAndFileNameStr = i_CaseClassObj.WavFile_FullPathAndFileNameStr;
            this.ProposedStatus = EnumCaseStatus.csUnknown;
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
