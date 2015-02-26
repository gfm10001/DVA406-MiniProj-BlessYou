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
    public class CBRSystem
    {

        // ====================================================================

        /// <summary>
        /// Retrieve most similar match for a specifiled case vs. the case library
        /// </summary>
        /// 

        public void Retrieve(CaseClass i_NewProblem, List<CaseClass> i_CaseLibrary, out int o_TopListCount, out List<RetrievedCaseClass> o_RetrievedMatches)
        {
            // 1. För varje case i case library:
            //      1.1 Beräkna Simularity Function (CalculateSimilarity), save each value locally
            // 2. Sort the list descending order to get top max in Similarity
            // 3. Transfer the o_TopListCount cases to the output 
            throw new System.NotImplementedException();
        } // Retrieve

        // ====================================================================

        /// <summary>
        /// Reuse most similar match for a specifiled case vs. the case library
        /// </summary>
        /// 

        public void Reuse(List<RetrievedCaseClass> i_RetrievedMatches, out EnumCaseStatus o_CaseStatus)
        {
            // Evaluate data in i_RetrievedMatches to find out suitable status (proposed sneeze or not)
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
