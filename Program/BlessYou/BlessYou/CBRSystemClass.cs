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
using System.Reflection;


namespace BlessYou
{
    public class CBRSystemClass
    {
        //public const int CaseConsiderationLimit = 3;

        // ====================================================================

        /// <summary>
        /// Retrieve most similar match for a specifiled case vs. the case library
        /// </summary>
        /// 

        public static void Retrieve(CaseClass i_NewCase, List<CaseClass> i_CaseLibraryList, int i_MaxRetrievedMatchesCount, out List<RetrievedCaseClass> o_RetrievedMatches)
        {
            // 1. För varje case i case library:
            //      1.1 Beräkna Simularity Function (CalculateSimilarity), save each value locally
            // 2. Sort the list descending order to get top max in Similarity
            // 3. Transfer the i_MaxRetrievedMatchesCount cases to the output 
            o_RetrievedMatches = new List<RetrievedCaseClass>();
            List<RetrievedCaseClass> similarityCaseList = new List<RetrievedCaseClass>();


            // Cal
            for (int ix = 0; ix < i_CaseLibraryList.Count; ++ix)
            {
                CaseClass currentCase;
                currentCase = i_CaseLibraryList[ix];
                RetrievedCaseClass theCase = new RetrievedCaseClass(currentCase);
                theCase.SimilarityDistance = currentCase.CalculateDistanceValue(i_NewCase); // If equal 1 perfect match if equal 0 no match at all
                similarityCaseList.Add(theCase);
            } // for ix

            List<RetrievedCaseClass> sortedCaseList = similarityCaseList.OrderBy(x => x.SimilarityDistance).ToList();


            //for (int ix = 0; ix < sortedCaseList.Count; ix++)
            //{
            //    o_RetrievedMatches.Add(sortedCaseList[ix]);

            //}

            for (int ix = 0; ix < i_MaxRetrievedMatchesCount && ix<sortedCaseList.Count; ix++)
            {
                o_RetrievedMatches.Add(sortedCaseList[ix]);
            } // for ix

            for (int ix = 0; ix < o_RetrievedMatches.Count; ++ix)
            {
                //Console.WriteLine("ix: " + ix);
                // Console.WriteLine(o_RetrievedMatches[ix].ToString());
                //Console.WriteLine("SimilarityValue: " + o_RetrievedMatches[ix].SimilarityDistance /*+ " Proposed Sneeze: " + o_RetrievedMatches[ix].ProposedStatus */ + " Sneeze: " + o_RetrievedMatches[ix].SneezeStatus);
            } // for ix
            // ToDo throw new System.NotImplementedException();
        } // Retrieve

        // ====================================================================

        public static void RetrieveUsingSimilarityfunction(CaseClass i_NewCase, List<CaseClass> i_CaseLibraryList, out List<RetrievedCaseClass> o_RetrievedMatches)
        {
            // 1. För varje case i case library:
            //      1.1 Beräkna Simularity Function (CalculateSimilarity), save each value locally
            // 2. Sort the list descending order to get top max in Similarity
            // 3. Transfer the i_MaxRetrievedMatchesCount cases to the output 
            o_RetrievedMatches = new List<RetrievedCaseClass>();
            List<RetrievedCaseClass> similarityCaseList = new List<RetrievedCaseClass>();

            //List<string> dumpSimilarityValues = new List<string>();


            // Calculate indvidual similarityvalues
            for (int ix = 0; ix < i_CaseLibraryList.Count; ++ix)
            {
                CaseClass currentCase;
                currentCase = i_CaseLibraryList[ix];
                RetrievedCaseClass theCase = new RetrievedCaseClass(currentCase);

                // Alternative function  ==> Similar result
                //theCase.SimilarityValue = currentCase.CalculateSimilarityValue(i_NewCase); // If equal 1 perfect match if equal 0 no match at all
                theCase.SimilarityValue = currentCase.CalculateSimilarityValueExt(i_NewCase);
                similarityCaseList.Add(theCase);

                // Debug print all similarity values camparing i_NewCase to i_CaseLibraryList one by one
                //string str = GetSimilarityValuesToString(i_NewCase, theCase);
                //dumpSimilarityValues.Add(str);
            } // for ix

            // System.IO.File.WriteAllLines(System.IO.Path.GetFileName(i_NewCase.WavFile_FullPathAndFileNameStr) + "_SF.txt", dumpSimilarityValues);

            // Sort the values best similarity case last
            List<RetrievedCaseClass> sortedCaseList = similarityCaseList.OrderBy(x => x.SimilarityValue).ToList();
            sortedCaseList.Reverse();

            // Return all similarity case from best to worst
            //for (int ix = 0, jx = sortedCaseList.Count - 1; jx >= 0 && ix < i_MaxRetrievedMatchesCount; ++ix, --jx) 
            for (int ix = 0; ix < sortedCaseList.Count; ++ix)
            {
                o_RetrievedMatches.Add(sortedCaseList[ix]);
            } // for ix

            // Write all SF numbers to file
            //List<string> resultString;
            //GetAllSimilarityValuesToString(i_NewCase, sortedCaseList, out resultString);
            //System.IO.File.WriteAllLines(System.IO.Path.GetFileName(i_NewCase.WavFile_FullPathAndFileNameStr) + "_SF.txt", resultString);

            // Debug prints
            //string correctAnswer;
            //if (o_RetrievedMatches[0].SneezeStatus == i_NewCase.SneezeStatus)
            //{
            //    correctAnswer = "Correct";
            //}
            //else
            //{

            //    correctAnswer = "Wrong";
            //}
            //string sFLargeStr;
            //if (o_RetrievedMatches[0].SimilarityValue > 0.7)
            //{
            //    sFLargeStr = ">0.70";
            //}
            //else
            //{
            //    sFLargeStr = "";
            //}
            // Debug print similarityfunction values
            //Console.WriteLine("SF={0:0.000000} between current {1,-50} and {2,-50} {3} {4}", o_RetrievedMatches[0].SimilarityValue, System.IO.Path.GetFileName(o_RetrievedMatches[0].WavFile_FullPathAndFileNameStr),
            //                                                               System.IO.Path.GetFileName(i_NewCase.WavFile_FullPathAndFileNameStr), correctAnswer, sFLargeStr);
        } // RetrieveUsingSimilarityfunction

        // ====================================================================

        /// <summary>
        /// Reuse most similar match for a specified case vs. the case library
        /// </summary>
        /// 

        public static void Reuse(List<RetrievedCaseClass> i_RetrievedMatches, out EnumCaseStatus o_CaseStatus)
        {
            //Verify list
            if (i_RetrievedMatches == null || i_RetrievedMatches.Count == 0)
            {
                o_CaseStatus = EnumCaseStatus.csNone;
                return;
            }

            Dictionary<EnumCaseStatus, int> countResults = new Dictionary<EnumCaseStatus, int>(); //saves hits per case
            Dictionary<EnumCaseStatus, double> distanceResults = new Dictionary<EnumCaseStatus, double>(); //saves average distance per case
            RetrievedCaseClass BestSimCase = i_RetrievedMatches[0]; //Closest distance case found


            //Initial the dictinaories so we know they are in the set
            foreach (EnumCaseStatus ecs in Enum.GetValues(typeof(EnumCaseStatus)))
            {
                countResults[ecs] = 0;
                distanceResults[ecs] = 0.0;
            }

            //Count how many we got of each case and calculate distance values
            for (int i = 0; i < i_RetrievedMatches.Count; i++)
            {
                countResults[i_RetrievedMatches[i].SneezeStatus]++;
                distanceResults[i_RetrievedMatches[i].SneezeStatus] = distanceResults[i_RetrievedMatches[i].SneezeStatus] + i_RetrievedMatches[i].SimilarityDistance;
                if (i_RetrievedMatches[i].SimilarityDistance < BestSimCase.SimilarityDistance) //Find closest match
                    BestSimCase = i_RetrievedMatches[i];
            }
            foreach (EnumCaseStatus c in Enum.GetValues(typeof(EnumCaseStatus)))
            {
                distanceResults[c] /= countResults[c];

            }

            //Find best match based on the count and distance respecivley
            EnumCaseStatus topCountCase = EnumCaseStatus.csUnknown;
            int topCountValue = 0;
            EnumCaseStatus bestAvgDistanceCase = EnumCaseStatus.csUnknown;
            double bestDistanceValue = double.MaxValue;
            double totalAVGdistance = 0;
            
            foreach (EnumCaseStatus c in countResults.Keys)
            {
                if (countResults[c] > topCountValue)
                {
                    topCountValue = countResults[c];
                    topCountCase = c;
                }
            }
            foreach (RetrievedCaseClass c in i_RetrievedMatches)
            {
                totalAVGdistance += c.SimilarityDistance;
                if (bestDistanceValue > c.SimilarityDistance)
                {
                    bestDistanceValue = c.SimilarityDistance;
                    bestAvgDistanceCase = c.SneezeStatus;

                }
            }
            totalAVGdistance /= i_RetrievedMatches.Count;

            Console.WriteLine("Count:" + topCountCase + " got most matches with " + topCountValue + "/" + i_RetrievedMatches.Count);
            Console.WriteLine("Distance:" + BestSimCase.SneezeStatus + " was closest with a value of " + BestSimCase.SimilarityDistance);

            if (topCountCase == BestSimCase.SneezeStatus) //If both cases evaluate to the same, propose as solution
            {
                o_CaseStatus = ConfirmedToPropused(topCountCase);
                Console.WriteLine("Proposing " + o_CaseStatus + " as solution.");

                return;
            }
            //If count and similarity does not evaluate to same, determine witch is most reliable.
            //We do this by calculating the probability, higher is better with 1.0 being a perfect match.

            double countProbability = (double)topCountValue / (double)i_RetrievedMatches.Count;
            double worstDistance = 0.0;

            foreach (EnumCaseStatus c in distanceResults.Keys)
            {
                if (distanceResults[c] > worstDistance)
                    worstDistance = distanceResults[c];
            }

            double DistancePrbability = 1 - bestDistanceValue / totalAVGdistance;
            //if (DistancePrbability < 0.70)
            //    System.Diagnostics.Debugger.Break();

            Console.WriteLine("Uncertianty in finding solution.\nCount value:" + countProbability + "\nDistance value:" + DistancePrbability);

            if (countProbability > DistancePrbability)
            {
                o_CaseStatus = ConfirmedToPropused(topCountCase);
                Console.WriteLine("Proposing " + o_CaseStatus + " as solution.");
                return;
            }
            else if (countProbability < DistancePrbability)
            {
                o_CaseStatus = ConfirmedToPropused(bestAvgDistanceCase);
                Console.WriteLine("Proposing " + o_CaseStatus + " as solution.");
                return;
            }
            Console.WriteLine("Failed to determine case, all hope is lost!");
            o_CaseStatus = EnumCaseStatus.csUnknown;
            return;
        } // Reuse

        // ====================================================================

        public static void ReuseUsingMajorityVote(List<RetrievedCaseClass> i_RetrievedMatches, int i_NumberOfCasesToUse_K_Value, EnumCaseStatus i_SelectedProblemObjCaseStatus, out EnumCaseStatus o_CaseStatus)
        {
            //Verify list
            if (i_RetrievedMatches == null || i_RetrievedMatches.Count == 0)
            {
                o_CaseStatus = EnumCaseStatus.csNone;
                return;
            }

            List<RetrievedCaseClass> retrievedMatches = i_RetrievedMatches;
            int numberOfSneezes = 0;
            int numberOfNonSneezes = 0;
            for (int ix = 0; (ix < i_RetrievedMatches.Count) && (ix < i_NumberOfCasesToUse_K_Value); ++ix) // ToDo remove i_RetrievedMatches.Count add check and send error
            {
                i_RetrievedMatches[ix].CaseSimilarityRankingValue = i_RetrievedMatches[ix].SimilarityValue;
                if (i_SelectedProblemObjCaseStatus == EnumCaseStatus.csIsConfirmedSneeze)
                {
                    if (retrievedMatches[ix].SneezeStatus == EnumCaseStatus.csIsConfirmedSneeze)
                    {
                        numberOfSneezes++;
                        retrievedMatches[ix].NrOfCorrectRetrievesRankingValue++;
                    }
                    else if (i_RetrievedMatches[ix].SneezeStatus == EnumCaseStatus.csIsConfirmedNoneSneeze)
                    {
                        numberOfNonSneezes++;
                        retrievedMatches[ix].NrOfWrongRetrievesRankingValue++;
                    }
                    else
                    {
                        Console.WriteLine("ERROR case have no sneezestatus");
                        o_CaseStatus = EnumCaseStatus.csNone;
                        return;
                    }
                }
                if (i_SelectedProblemObjCaseStatus == EnumCaseStatus.csIsConfirmedNoneSneeze)
                {
                    if (retrievedMatches[ix].SneezeStatus == EnumCaseStatus.csIsConfirmedSneeze)
                    {
                        numberOfSneezes++;
                        retrievedMatches[ix].NrOfWrongRetrievesRankingValue++;
                    }
                    else if (i_RetrievedMatches[ix].SneezeStatus == EnumCaseStatus.csIsConfirmedNoneSneeze)
                    {
                        numberOfNonSneezes++;
                        retrievedMatches[ix].NrOfCorrectRetrievesRankingValue++;
                    }
                    else
                    {
                        Console.WriteLine("ERROR case have no sneezestatus");
                        o_CaseStatus = EnumCaseStatus.csNone;
                        return;
                    }
                }
            } // for ix

            if (numberOfSneezes >= numberOfNonSneezes)
            {
                o_CaseStatus = EnumCaseStatus.csIsProposedSneeze;
            } // if 
            else
            {
                o_CaseStatus = EnumCaseStatus.csIsProposedNoneSneeze;
            }
        } // ReuseUsingSimilarityValue

        // ====================================================================

        public static EnumCaseStatus ConfirmedToPropused(EnumCaseStatus state)
        {
            if (state == EnumCaseStatus.csIsConfirmedNoneSneeze)
                return EnumCaseStatus.csIsProposedNoneSneeze;
            if (state == EnumCaseStatus.csIsConfirmedSneeze)
                return EnumCaseStatus.csIsProposedSneeze;
            return EnumCaseStatus.csNone;
        }

        // ====================================================================

        public static void AccumulateSimilarityValuesInList(List<RetrievedCaseClass> i_RetrievedMatchesList, List<RetrievedCaseClass> i_AccumulatedSimilarityValuesMatchesList)
        {
            List<RetrievedCaseClass> accumulatedSimilarityValuesMatchesList = i_AccumulatedSimilarityValuesMatchesList;
            foreach (RetrievedCaseClass retMatchesObj in i_RetrievedMatchesList)
            {
                foreach (RetrievedCaseClass accSimObj in accumulatedSimilarityValuesMatchesList)
                {
                    if (retMatchesObj.WavFile_FullPathAndFileNameStr == accSimObj.WavFile_FullPathAndFileNameStr)
                    {
                        accSimObj.NrOfCorrectRetrievesRankingValue += retMatchesObj.NrOfCorrectRetrievesRankingValue;
                        accSimObj.NrOfWrongRetrievesRankingValue += retMatchesObj.NrOfWrongRetrievesRankingValue;
                        accSimObj.CaseSimilarityRankingValue += retMatchesObj.CaseSimilarityRankingValue;
                    }
                } // foreach
            } // foreach

            // Print case accumulatedSimilarityvalues and vote frequency
            //List<string> resultString;
            //CBRSystemClass.DumpAllAccumulatedSimilarityValuesToString(accumulatedSimilarityValuesMatchesList, out resultString);
            //foreach (string s in resultString)
            //{
            //    Console.WriteLine("{0}", s);
            //}
        } // AccumulateSimilarityValuesInList
        
        // ====================================================================

        public static void ClearSimilarityValuesInList(List<RetrievedCaseClass> i_AccumulatedSimilarityValuesMatchesList)
        {
            List<RetrievedCaseClass> accumulatedSimilarityValuesMatchesList = i_AccumulatedSimilarityValuesMatchesList;
            foreach (RetrievedCaseClass accSimObj in accumulatedSimilarityValuesMatchesList)
            {
                accSimObj.NrOfCorrectRetrievesRankingValue = 0;
                accSimObj.NrOfWrongRetrievesRankingValue = 0;
                accSimObj.CaseSimilarityRankingValue = 0;
            } // foreach
        } // ClearSimilarityValuesInList

        // ====================================================================

        public static void Revise()
        {

            // throw new System.NotImplementedException();
        } // Revise
        // ====================================================================

        public static void Retain()
        {
            
            // throw new System.NotImplementedException();
        } // Retain

        public static ConfigurationStatClass GenerateRandomConfig(double toplimit =1.0)
        // ====================================================================

        {
            ConfigurationStatClass config = new ConfigurationStatClass();

            Type t = config.GetType();
            FieldInfo[] fio = t.GetFields();
            Random random = new Random();
            foreach (FieldInfo f in fio)
            {
                if (f.IsStatic)
                    continue;
                f.SetValue(config, random.NextDouble() * toplimit);
            }

            return config;

        }

        // ====================================================================

        public static void EvaluateFeatureOneByOne(CaseLibraryClass caseLibraryObj)
        {
            ConfigurationStatClass config = new ConfigurationStatClass();
            Type t = config.GetType();
            FieldInfo[] fionfo = t.GetFields();
            //List<List<RetrievedCaseClass>> debuglist = new List<List<RetrievedCaseClass>>();

            foreach (FieldInfo f in fionfo)
            {
                if (f.IsStatic)
                    continue;

                foreach (FieldInfo e in fionfo)
                {
                    if (e.IsStatic)
                        continue;
                    if (e == f)
                        e.SetValue(config, 1.0);
                    else
                        e.SetValue(config, 0.0);    
                        //f.SetValue(config, 0.0);
                }
                foreach (CaseClass c in caseLibraryObj.ListOfCases)
                {
                    c.UpdateFeatureVectors(config);
                }
                //foreach (CaseClass c in caseLibraryObj.ListOfCases)
                //{
                //    c.UpdateFeatureVectors(config);
                //    c.ca
                  
                //}

                int correct = 0, wrong = 0;
                List<string> correctList = new List<string>();
                List<string> wronglist = new List<string>();

                CaseClass selectedProblemObj = new CaseClass();
                for (int ix = 0; ix < caseLibraryObj.ListOfCases.Count; ++ix)
                {
                    selectedProblemObj = caseLibraryObj.ListOfCases[ix];
                    Console.WriteLine("\nFilename: " + selectedProblemObj.WavFile_FullPathAndFileNameStr);
                    List<CaseClass> caseMinusOneList = new List<CaseClass>();
                    for (int jx = 0; jx < caseLibraryObj.ListOfCases.Count; ++jx)
                    {
                        caseLibraryObj.ListOfCases[jx].UpdateFeatureVectors(config);
                       if (jx != ix)
                        {
                            caseMinusOneList.Add(caseLibraryObj.ListOfCases[jx]);
                        }
                    } // for jx

                    List<RetrievedCaseClass> retrievedMatchesList;
                    Retrieve(selectedProblemObj, caseMinusOneList, ConfigurationStatClass.C_NR_OF_RETRIEVED_CASES, out retrievedMatchesList);
                    

                    //4. Start reuse function
                    EnumCaseStatus caseStatus = EnumCaseStatus.csUnknown;
                    CBRSystemClass.Reuse(retrievedMatchesList, out caseStatus);
                    if (caseStatus == EnumCaseStatus.csIsProposedSneeze && selectedProblemObj.SneezeStatus == EnumCaseStatus.csIsConfirmedSneeze)
                    {
                        correctList.Add(selectedProblemObj.WavFile_FullPathAndFileNameStr);
                        correct++;
                    }
                    else if (caseStatus == EnumCaseStatus.csIsProposedNoneSneeze && selectedProblemObj.SneezeStatus == EnumCaseStatus.csIsConfirmedNoneSneeze)
                    {
                        correctList.Add(selectedProblemObj.WavFile_FullPathAndFileNameStr);
                        correct++;
                    }
                    else
                    {
                        //System.Diagnostics.Debugger.Break();
                        wronglist.Add(selectedProblemObj.WavFile_FullPathAndFileNameStr);
                        wrong++;
                        Console.WriteLine("GUESSED WRONG HERE!");
                    }
                }
                Console.WriteLine("\nNumber of correct guesses: " + correct + " / " + (correct / (double)(correct + wrong)) * 100 + "%");// for ix
                Console.WriteLine("Number of wrong guesses: " + wrong + " / " + (wrong / (double)(wrong + correct)) * 100 + "%\n");
                Console.WriteLine("Feature name:" + f.Name);
                correctList.Add("\nNumber of correct guesses: " + correct + " / " + (correct / (double)(correct + wrong)) * 100 + "%");
                wronglist.Add("Number of wrong guesses: " + wrong + " / " + (wrong / (double)(wrong + correct)) * 100 + "%\n");
                System.IO.File.WriteAllLines("./Wrongs-" + f.Name + ".txt", wronglist);
                System.IO.File.WriteAllLines("./Corrects-" + f.Name + ".txt", correctList);
            }
            // ====================================================================
        }
        
        // CBRSystem
        // ====================================================================

        public static string GetSimilarityValueToString(CaseClass i_NewCase, RetrievedCaseClass i_CurrentCase)
        {
            string resStr = "";

            resStr = String.Format("SF={0:0.000000} between current {1,-20} and {2,-20}", i_CurrentCase.SimilarityValue, System.IO.Path.GetFileName(i_CurrentCase.WavFile_FullPathAndFileNameStr),
                                                                           System.IO.Path.GetFileName(i_NewCase.WavFile_FullPathAndFileNameStr));

            return resStr;
        } // GetSimilarityValueToString

        // ====================================================================

        public static void GetAllSimilarityValuesToString(CaseClass i_NewCase, List<RetrievedCaseClass> i_SortedList, out List<string> o_ResultString)
        {
            string resStr = "";
            o_ResultString = new List<string>();
            for (int ix = 0; ix < i_SortedList.Count; ++ix)
            {
                resStr = GetSimilarityValueToString(i_NewCase, i_SortedList[ix]);
                o_ResultString.Add(resStr);
            }
        } // GetAllSimilarityValuesToString

        // ====================================================================

        public static void DumpAllAccumulatedSimilarityValuesToString(List<RetrievedCaseClass> i_AccumulatedSimilarityValuesMatchesList, out List<string> o_ResultString)
        {
            List<RetrievedCaseClass> accumulatedSimilarityValuesMatchesList = i_AccumulatedSimilarityValuesMatchesList;
            o_ResultString = new List<string>(); 
            int nrOfCasesCorrectAndWrong = 0;
            int nrOfCasesCorrectOnly = 0;
            int nrOfCasesWrongOnly = 0;
            int nrOfCasesNeverUsedInEvaluation = 0;
            foreach (RetrievedCaseClass rCC in accumulatedSimilarityValuesMatchesList)
            {
                //ToDo Console.WriteLine("ix: {0} {1}", ix, retrievedMatchesList[ix].GetCurrentMatchingString());
                if (rCC.NrOfCorrectRetrievesRankingValue > 0 && rCC.NrOfWrongRetrievesRankingValue > 0)
                {
                    nrOfCasesCorrectAndWrong++;
                }
                else if (rCC.NrOfCorrectRetrievesRankingValue > 0 && rCC.NrOfWrongRetrievesRankingValue == 0)
                {
                    nrOfCasesCorrectOnly++;
                }
                if (rCC.NrOfCorrectRetrievesRankingValue == 0 && rCC.NrOfWrongRetrievesRankingValue > 0)
                {
                    nrOfCasesWrongOnly++;
                }
                if (rCC.NrOfCorrectRetrievesRankingValue == 0 && rCC.NrOfWrongRetrievesRankingValue == 0)
                {
                    nrOfCasesNeverUsedInEvaluation++;
                }
                o_ResultString.Add(String.Format("File: {0}", rCC.WavFile_FullPathAndFileNameStr));
                o_ResultString.Add(String.Format("Dump nr of times used to detect sound correct {0}", rCC.NrOfCorrectRetrievesRankingValue));
                o_ResultString.Add(String.Format("Dump nr of times used to detect sound incorrect {0}", rCC.NrOfWrongRetrievesRankingValue));
                o_ResultString.Add(String.Format("Summarized similarity value {0}\n", rCC.CaseSimilarityRankingValue));
            }

            o_ResultString.Add(String.Format("\nNr of cases with problems: {0}\n", nrOfCasesCorrectAndWrong));
            o_ResultString.Add(String.Format("Nr of cases only Correct:  {0}\n", nrOfCasesCorrectOnly));
            o_ResultString.Add(String.Format("Nr of cases only Wrong:    {0}\n", nrOfCasesWrongOnly));
            o_ResultString.Add(String.Format("Nr of cases never used:    {0}\n", nrOfCasesNeverUsedInEvaluation));

        } // DumpAllAccumulatedSimilarityValuesToString
    } // CBRSystem
}
