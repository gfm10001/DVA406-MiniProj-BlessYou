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
                theCase.SimilarityValue = currentCase.CalculateRawSimilarityValue(i_NewCase); // If equal 1 perfect match if equal 0 no match at all
                similarityCaseList.Add(theCase);
            } // for ix

            List<RetrievedCaseClass> sortedCaseList = similarityCaseList.OrderBy(x => x.SimilarityValue).ToList();

            for (int ix = 0, jx = sortedCaseList.Count - 1; jx >= 0 && ix < i_MaxRetrievedMatchesCount; ++ix, --jx)
            {
                o_RetrievedMatches.Add(sortedCaseList[jx]);
            } // for ix

            for (int ix = 0; ix < o_RetrievedMatches.Count; ++ix)
            {
                Console.WriteLine("ix: " + ix);
               // Console.WriteLine(o_RetrievedMatches[ix].ToString());
                Console.WriteLine("SimilarityValue: " + o_RetrievedMatches[ix].SimilarityValue + " Proposed Sneeze: " + o_RetrievedMatches[ix].ProposedStatus + " Sneeze: " + o_RetrievedMatches[ix].SneezeStatus);
            } // for ix
           // ToDo throw new System.NotImplementedException();
        } // Retrieve

        // ====================================================================

        /// <summary>
        /// Reuse most similar match for a specified case vs. the case library
        /// </summary>
        /// 

        public void Reuse(List<RetrievedCaseClass> i_RetrievedMatches, out EnumCaseStatus o_CaseStatus)
        {
            //Verify list
            if (i_RetrievedMatches == null || i_RetrievedMatches.Count == 0)
            {
                o_CaseStatus = EnumCaseStatus.csNone;
                return;
            }

            Dictionary<EnumCaseStatus, int> countResults = new Dictionary<EnumCaseStatus, int>();
            Dictionary<EnumCaseStatus, double> distanceResults = new Dictionary<EnumCaseStatus, double>();
            RetrievedCaseClass BestSimCase = i_RetrievedMatches[0];

            foreach (EnumCaseStatus ecs in Enum.GetValues(typeof(EnumCaseStatus)))
            {
                countResults[ecs] = 0;
                distanceResults[ecs] = 0.0;
            }
            
            for (int i = 0; i < i_RetrievedMatches.Count; i++)
            {
                countResults[i_RetrievedMatches[i].ProposedStatus]++;
                distanceResults[i_RetrievedMatches[i].ProposedStatus] = distanceResults[i_RetrievedMatches[i].ProposedStatus] + i_RetrievedMatches[i].SimilarityValue;
                if (i_RetrievedMatches[i].SimilarityValue > BestSimCase.SimilarityValue)
                    BestSimCase = i_RetrievedMatches[i];
            }

            //Calculate best match based on number of matches
            EnumCaseStatus topCountCase = EnumCaseStatus.csUnknown;
            int topCountValue = 0;
            EnumCaseStatus bestDistanceCase = EnumCaseStatus.csUnknown;
            double bestDistanceValue = double.MaxValue;
            foreach (EnumCaseStatus c in countResults.Keys)
            {
                if (countResults[c] > topCountValue)
                {
                    topCountValue = countResults[c];
                    topCountCase = c;
                }
                if (bestDistanceValue > distanceResults[c])
                {
                    bestDistanceValue = distanceResults[c];
                    bestDistanceCase = c;
                }
            }

            Console.WriteLine("\nCount:" + topCountCase + " got most matches with " + topCountValue + "/" + i_RetrievedMatches.Count);
            Console.WriteLine("\nDistance:" + bestDistanceCase + " was closest with a value of " + bestDistanceValue);

            if (topCountCase == bestDistanceCase) //If both cases evaluate to the same, propose as solution
            {
                o_CaseStatus = ConfirmedToPropused(topCountCase);
                Console.WriteLine("\nProposing " + o_CaseStatus + " as solution.");
                return;
            }
            //If count and similarity does not evaluate to same, determine witch is most reliable

            double countProbability = (double)i_RetrievedMatches.Count / (double)topCountValue;
            Console.WriteLine("Uncertianty in finding solution.\nCount value:"+countProbability +"\nSimilarity value:" + BestSimCase.SimilarityValue);

            if (countProbability > bestDistanceValue)
            {
                o_CaseStatus = ConfirmedToPropused(topCountCase);
                Console.WriteLine("\nProposing " + o_CaseStatus + " as solution.");
                return;
            }
            else if (countProbability < bestDistanceValue)
            {
                o_CaseStatus = ConfirmedToPropused(bestDistanceCase);
                Console.WriteLine("\nProposing " + o_CaseStatus + " as solution.");
                return;
            }
            Console.WriteLine("\nFailed to determine case, all hope is lost!");
            o_CaseStatus = EnumCaseStatus.csUnknown;
            return;

        } // Reuse

        // ====================================================================

        public static EnumCaseStatus ConfirmedToPropused(EnumCaseStatus state)
        {
            if (state == EnumCaseStatus.csIsConfirmedNoneSneeze)
                return EnumCaseStatus.csIsProposedNoneSneeze;
            if (state == EnumCaseStatus.csIsConfirmedSneeze)
                return EnumCaseStatus.csIsProposedSneeze;
            return EnumCaseStatus.csNone;
        }

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
