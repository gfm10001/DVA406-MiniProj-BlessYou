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
        double FDistanceValue;
        double FSimilarityValue; 
        EnumCaseStatus FProposedStatus;
        double FCaseSimilarityRankingValue;
        int FNrOfCorrectRetrievesRankingValue;
        int FNrOfWrongRetrievesRankingValue;

        //=====================================================================

        #region Properties
        public EnumCaseStatus ProposedStatus
        {
            get { return FProposedStatus; }

            set { FProposedStatus = value; }
        
        } // ProposedStatus

        //=====================================================================

        public double SimilarityDistance
        {
            get
            {
                return FDistanceValue;
            }
            set
            {
                FDistanceValue = value;
            }
        } // SimilarityDistance

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
        } // SimilarityValue

        //=====================================================================

        public double CaseSimilarityRankingValue
        {
            get
            {
                return FCaseSimilarityRankingValue;
            }
            set
            {
                FCaseSimilarityRankingValue = value;
            }
        } // CaseRankingValue
 
        //=====================================================================

        public int NrOfCorrectRetrievesRankingValue
        {
            get
            {
                return FNrOfCorrectRetrievesRankingValue;
            }
            set
            {
                FNrOfCorrectRetrievesRankingValue = value;
            }
        } // NrOfCorrectRetrievesRankingValue


        //=====================================================================

        public int NrOfWrongRetrievesRankingValue
        {
            get
            {
                return FNrOfWrongRetrievesRankingValue;
            }
            set
            {
                FNrOfWrongRetrievesRankingValue = value;
            }
        } // NrOfWrongRetrievesRankingValue

        #endregion

        //=====================================================================

        public RetrievedCaseClass()
        {
            CaseSimilarityRankingValue = 0.0;
            NrOfCorrectRetrievesRankingValue = 0;
            NrOfWrongRetrievesRankingValue = 0;
        }

        //=====================================================================

        public RetrievedCaseClass(CaseClass i_CaseClassObj) :
               base (i_CaseClassObj)
        {
            // ToDo better implementation
            this.WavFile_FullPathAndFileNameStr = i_CaseClassObj.WavFile_FullPathAndFileNameStr;
            this.ProposedStatus = EnumCaseStatus.csUnknown;
            CaseSimilarityRankingValue = 0;
        } // RetrievedCaseClass

        //=====================================================================

        public RetrievedCaseClass(RetrievedCaseClass i_RetrievedCaseClassObj)
        {
            base.WavFile_FullPathAndFileNameStr = i_RetrievedCaseClassObj.WavFile_FullPathAndFileNameStr;
            this.ProposedStatus = i_RetrievedCaseClassObj.ProposedStatus;
            this.FDistanceValue = i_RetrievedCaseClassObj.FDistanceValue;
            this.SimilarityValue = i_RetrievedCaseClassObj.SimilarityValue;
            this.CaseSimilarityRankingValue = i_RetrievedCaseClassObj.CaseSimilarityRankingValue;
            this.NrOfCorrectRetrievesRankingValue = i_RetrievedCaseClassObj.NrOfCorrectRetrievesRankingValue;
            this.NrOfWrongRetrievesRankingValue = i_RetrievedCaseClassObj.NrOfWrongRetrievesRankingValue;
        } // RetrievedCaseClass

        //=====================================================================
    
        public string GetCurrentMatchingString()
        {
            return "Filename: " + base.WavFile_FullPathAndFileNameStr + " Similarityvalue: " + FDistanceValue.ToString() + " Proposed result: " + FProposedStatus.ToString()
                                + " Actual: " + base.SneezeStatus.ToString();
        }

        //void CalculateSimilarityValue(CaseClass i_NewCase)
        //{

        //} // CalculateSimilarityValue
    } // RetrievedCaseClass

}
