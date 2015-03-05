// CBRSystem.cs
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
    public class CBRSystemClass
    {

        // ====================================================================

        /// <summary>
        /// Retrieve most similar match for a specifiled case vs. the case library
        /// </summary>
        /// 

        public void Retrieve(CaseClass i_NewCase, List<CaseClass> i_CaseLibraryList, int i_MaxRetrievedMatchesCount, out List<RetrievedCaseClass> o_RetrievedMatches)
        {
            // 1. För varje case i case library:
            //      1.1 Beräkna Simularity Function (CalculateSimilarity), save each value locally
            // 2. Sort the list descending order to get top max in Similarity
            // 3. Transfer the i_MaxRetrievedMatchesCount cases to the output 
            o_RetrievedMatches = new List<RetrievedCaseClass>();
            List<RetrievedCaseClass> similarityCaseList = new List<RetrievedCaseClass>();

            for (int ix = 0; ix < i_CaseLibraryList.Count; ++ix)
            {
                CaseClass currentCase;
                currentCase = i_CaseLibraryList[ix];
                RetrievedCaseClass theCase = new RetrievedCaseClass(currentCase);
                theCase.SimilarityValue = currentCase.calculateSimilarityFunction(i_NewCase);
                similarityCaseList.Add(theCase);
            } // for ix

            List<RetrievedCaseClass> sortedCaseList = similarityCaseList.OrderBy(x => x.SimilarityValue).ToList();

            for (int ix = 0, jx = sortedCaseList.Count - 1; jx >= 0 && ix < i_MaxRetrievedMatchesCount; ++ix, --jx)
            {
                o_RetrievedMatches.Add(sortedCaseList[jx]);
            } // for ix

           // ToDo throw new System.NotImplementedException();
        } // Retrieve

        // ====================================================================

        /// <summary>
        /// Reuse most similar match for a specifiled case vs. the case library
        /// </summary>
        /// 

        public void Reuse(List<RetrievedCaseClass> i_RetrievedMatches, out EnumCaseStatus o_CaseStatus)
        {
            
            // Evaluate data in i_RetrievedMatches to find out suitable status (proposed sneeze or not)

            //i_RetrievedMatches[0].


            throw new System.NotImplementedException();
        } // Reuse

        // ====================================================================

        public void Revise()
        {
            // TBA
            throw new System.NotImplementedException();
        } // Revise

        // ====================================================================

        public void Retain()
        {
            // TBA
            throw new System.NotImplementedException();
        } // Retain

        // ====================================================================

    } // CBRSystem
}
