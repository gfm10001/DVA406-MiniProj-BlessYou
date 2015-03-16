// BlessYouMain.cs
//
// DVA406 Intelligent Systems, Mdh, vt15
// Group 1:
//      Simon P.
//      Niclas S.
//      Göran FMarker.
//
// Mini project "Bless You" - a CASE-Based Sneeze Detector
//
// History:
// 2015-02-24       Introduced.
// 2015-03-12/GF    Refactored file dump of features to CaseLibraryClass
//


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlessYou
{
    class BlessYouMain
    {
        // ====================================================================

        static void Main(string[] args)
        {
            const string C_THIS_VERSION = "Bless You v.0.7/0 of 2015-03-16";
            DateTime startTime;

            // Usage:
            // BlessYou P1 P2 [P3] where
            // P1 = name of text file with names of all .wav­files to be examined
            // P2 = File name for new problem | "all" : all files in Case Library run in sequence
            // P3 = path to directory for created .ftr­files (optional)

            Console.WriteLine(C_THIS_VERSION + " (Par: " + ConfigurationStatClass.USE_PARALLEL_EXECUTION + ")");

            CBRSystemClass CBRSystemClass = new CBRSystemClass();
            ConfigurationDynClass config = new ConfigurationDynClass(); // CBRSystemClass.GenerateRandomConfig(100);
            List<SoundFileClass> allSoundFilesObjList;
            List<SoundFileClass> usedSoundFilesObjList;
            CaseLibraryClass caseLibraryObj;
            string newProblemFileName; // If empty run all problems
            string ftrFilePath; // If empty no storage of ftr files
            List<RetrievedCaseClass> retrievedMatchesList = new List<RetrievedCaseClass>();
            List<RetrievedCaseClass> accumulatedSimilarityValuesRetrievedMatchesList = new List<RetrievedCaseClass>();


            // 1. Decode Params
            //DecodeParamClass.DecodeParam2(args, out Liblist, out retrievedMatchesList);
            DecodeParamClass.DecodeParam(args, out allSoundFilesObjList, out newProblemFileName, out ftrFilePath);
            startTime = DateTime.Now;
            Console.WriteLine("Starting: " + startTime.ToString() + ", config file: " + args[0] + " \n");

            // 2. Create CASE-library
            // First extract 50+50 random files for use as first library
            HelperStaticClass.GetRandomSelection(allSoundFilesObjList, usedSoundFilesObjList);
            FeatureExtractorClass._loadFeatureList(out caseLibraryObj, usedSoundFilesObjList, config);

            // Choose 50 sneezes and 50 nonsneezes for case library

            // Prepare for revise and retain
            foreach (CaseClass c in caseLibraryObj.ListOfCases)
            {
                RetrievedCaseClass rCCObj = new RetrievedCaseClass(c);
                accumulatedSimilarityValuesRetrievedMatchesList.Add(rCCObj);
            }

            // Dump calculated features 

            //Console.Write("Dump all features to files... ");
            //caseLibraryObj.DumpAllFeatureValuesOfAllCasesToFiles("Feature");

            Console.WriteLine();

           // CBRSystemClass.EvaluateFeatureOneByOne(caseLibraryObj);


            // 3. Evaluate cases
            if ("" != newProblemFileName)
            {
                //foreach (SoundFileClass sfc in soundfileObjList)
                //{
                SoundFileClass newProblemSoundFileObj = new SoundFileClass();
                newProblemSoundFileObj.SoundFileName = newProblemFileName;
                newProblemSoundFileObj.SoundFileSneezeMarker = EnumSneezeMarker.smUnKnown;

                CaseClass newProblemObj = new CaseClass();
                newProblemObj.ExtractWavFileFeatures(newProblemSoundFileObj, config);

                List<CaseClass> caseList = new List<CaseClass>();
                caseList.AddRange(caseLibraryObj.ListOfCases);
                CBRSystemClass.Retrieve(newProblemObj, caseList, ConfigurationStatClass.C_NR_OF_RETRIEVED_CASES, out retrievedMatchesList);

                //4. Start reuse function
                EnumCaseStatus caseStatus;
                CBRSystemClass.Reuse(retrievedMatchesList, out caseStatus);
                //}
            } // if
            else
            {
                int nrOfConfirmedSneezes;
                int nrOfConfirmedNoneSneezes;
                int correctSneezes = 0;
                int inCorrectSneezes = 0;
                int correctNoneSneezes = 0;
                int inCorrectNoneSneezes = 0;

                List<string> correctList = new List<string>();

                List<string> wronglist = new List<string>();

                CaseClass selectedProblemObj = new CaseClass();



                int numberofCasesForMajorityVote;
                if (ConfigurationStatClass.RUN_ALL_MAJORITY_VOTE_CASE_NUMBERS)
                {
                    numberofCasesForMajorityVote = 1;
                }
                else
                {
                    numberofCasesForMajorityVote = ConfigurationStatClass.C_NUMBER_OF_CASES_TO_USE_FOR_MAJORITY_VOTE;
                }
                                
                // do - while loop introduced to test number of cases for majority vote
                do
                {
                    Console.WriteLine("\n\nNumber of cases to vote from: {0}", numberofCasesForMajorityVote);
                    correctSneezes = 0;
                    inCorrectSneezes = 0;
                    correctNoneSneezes = 0;
                    inCorrectNoneSneezes = 0;
                    List<double> CorrectSneezesSimilarityValue = new List<double>();
                    List<double> WrongSneezesSimilarityValue = new List<double>();
                    CBRSystemClass.ClearSimilarityValuesInList(accumulatedSimilarityValuesRetrievedMatchesList);

                    for (int ix = 0; ix < caseLibraryObj.ListOfCases.Count; ++ix)
                    {

                        selectedProblemObj = caseLibraryObj.ListOfCases[ix];
                        //Console.WriteLine("\nFilename: " + selectedProblemObj.WavFile_FullPathAndFileNameStr);
                        List<CaseClass> caseLibaryMinusOneCaseList = new List<CaseClass>();
                        for (int jx = 0; jx < caseLibraryObj.ListOfCases.Count; ++jx)
                        {
                            if (jx != ix)
                            {
                                caseLibaryMinusOneCaseList.Add(caseLibraryObj.ListOfCases[jx]);
                            }
                        } // for jx

                        // Alternative functioncall using similarity value
                        // Original:
                        // CBRSystemClass.Retrieve(selectedProblemObj, caseLibaryMinusOneCaseList, ConfigurationStatClass.C_NR_OF_RETRIEVED_CASES, out retrievedMatchesList);
                        // Alternative:
                        CBRSystemClass.RetrieveUsingSimilarityfunction(selectedProblemObj, caseLibaryMinusOneCaseList, out retrievedMatchesList);

                        // 4. Start reuse function
                        EnumCaseStatus caseStatus;

                        // Alternative functioncall using majority vote
                        // Original:
                        //CBRSystemClass.Reuse(retrievedMatchesList, out caseStatus);
                        // Alternative:
                        CBRSystemClass.ReuseUsingMajorityVote(retrievedMatchesList, numberofCasesForMajorityVote, selectedProblemObj.SneezeStatus, out caseStatus);
                        
                        // Evaluate selectedProblem
                        if (selectedProblemObj.SneezeStatus == EnumCaseStatus.csIsConfirmedSneeze)
                        {
                            if (caseStatus == EnumCaseStatus.csIsProposedSneeze)
                            {
                                CorrectSneezesSimilarityValue.Add(retrievedMatchesList[0].SimilarityValue);
                                correctList.Add(selectedProblemObj.WavFile_FullPathAndFileNameStr);
                                correctSneezes++;
                            }
                            else
                            {
                                WrongSneezesSimilarityValue.Add(retrievedMatchesList[0].SimilarityValue);
                                wronglist.Add(selectedProblemObj.WavFile_FullPathAndFileNameStr);
                                inCorrectSneezes++;
                                //Console.WriteLine("GUESSED WRONG HERE on SNEEZE!");
                            }
                        }
                        if (selectedProblemObj.SneezeStatus == EnumCaseStatus.csIsConfirmedNoneSneeze)
                        {
                            if (caseStatus == EnumCaseStatus.csIsProposedNoneSneeze)
                            {
                                CorrectSneezesSimilarityValue.Add(retrievedMatchesList[0].SimilarityValue);
                                correctList.Add(selectedProblemObj.WavFile_FullPathAndFileNameStr);
                                correctNoneSneezes++;
                            }
                            else
                            {
                                WrongSneezesSimilarityValue.Add(retrievedMatchesList[0].SimilarityValue);
                                //System.Diagnostics.Debugger.Break();
                                wronglist.Add(selectedProblemObj.WavFile_FullPathAndFileNameStr);
                                inCorrectNoneSneezes++;
                                //Console.WriteLine("GUESSED WRONG HERE on None-SNEEZE!");
                            }
                        }
                        CBRSystemClass.AccumulateSimilarityValuesInList(retrievedMatchesList, accumulatedSimilarityValuesRetrievedMatchesList);
                    } // for ix

                    double total = correctSneezes + correctNoneSneezes + inCorrectSneezes + inCorrectNoneSneezes;

                    caseLibraryObj.CountNrOfDifferentCases(out nrOfConfirmedSneezes, out nrOfConfirmedNoneSneezes);

                    Console.WriteLine();
                    double xMax = Double.NaN;
                    double xMin = Double.NaN;
                    if (0 < CorrectSneezesSimilarityValue.Count)
                    {
                        xMax = CorrectSneezesSimilarityValue.Max();
                        xMin = CorrectSneezesSimilarityValue.Min();
                    } // if
                    Console.WriteLine("highestCorrect = {0}, lowestCorrect = {1}", xMax, xMin);

                    xMax = Double.NaN;
                    xMin = Double.NaN;
                    if (0 < WrongSneezesSimilarityValue.Count)
                    {
                        xMax = WrongSneezesSimilarityValue.Max();
                        xMin = WrongSneezesSimilarityValue.Min();
                    } // if
                    Console.WriteLine("highestWrong = {0}, lowestWrong = {1}", xMax, xMin);

                    Console.WriteLine();
                    Console.WriteLine("In Total Case Library: Nr of confirmed sneezes:      {0, 4:0}", nrOfConfirmedSneezes);
                    Console.WriteLine("In Total Case Library: Nr of confirmed none-sneezes: {0, 4:0}", nrOfConfirmedNoneSneezes);

                    Console.WriteLine("Number of correct guesses:    >>> >>> >>>> >>> >>>   {0, 4:0} = {1, 3:0.0}%", correctSneezes + correctNoneSneezes, ((double)(correctSneezes + correctNoneSneezes) / total) * 100.0);

                    Console.WriteLine("Number of correct SNEEZE guesses:                    {0, 4:0} = {1, 3:0.0}%", correctSneezes, ((double)correctSneezes / total) * 100.0);
                    Console.WriteLine("Number of correct NONE SNEEZES guesses:              {0, 4:0} = {1, 3:0.0}%", correctNoneSneezes, ((double)correctNoneSneezes / total) * 100.0);
                    Console.WriteLine("Number of incorrect SNEEZE guesses:                  {0, 4:0} = {1, 3:0.0}%", inCorrectSneezes, ((double)inCorrectSneezes / total) * 100.0);
                    Console.WriteLine("Number of incorrect NONE SNEEZES guesses:            {0, 4:0} = {1, 3:0.0}%", inCorrectNoneSneezes, ((double)inCorrectNoneSneezes / total) * 100.0);

                    System.IO.File.WriteAllLines("./Wrongs.txt", wronglist);
                    System.IO.File.WriteAllLines("./Corrects.txt", correctList);
                    numberofCasesForMajorityVote += 2;
                    // ToDo: utvärdera alla retrievedMatchesList för varje loop omgång
                    //ToDo throw new System.NotImplementedException();
                } // While loop introduced as an alternative for using similarityvalue and majority vote
                while (ConfigurationStatClass.RUN_ALL_MAJORITY_VOTE_CASE_NUMBERS && numberofCasesForMajorityVote <= ConfigurationStatClass.C_NUMBER_OF_CASES_TO_USE_FOR_MAJORITY_VOTE);
            } // else


            RetrievedCaseClass caseToRemoveFromCaseLibrary;
            CBRSystemClass.Revise(accumulatedSimilarityValuesRetrievedMatchesList, out caseToRemoveFromCaseLibrary);

            // 5. Skriv ut rapport
            Console.WriteLine("Number of matches = {0}", retrievedMatchesList.Count);
            for (int ix = 0; ix < accumulatedSimilarityValuesRetrievedMatchesList.Count; ++ix)
            {
                //ToDo Console.WriteLine("ix: {0} {1}", ix, retrievedMatchesList[ix].GetCurrentMatchingString());
            } // for ix

            // 6. Optionally dump case info
            if (1 == 1)
            {
                Console.WriteLine("Dump configuration report to file '{0}'...", ConfigurationStatClass.C_CONFIGURATION_REPORT_FILE_NAME);
                ConfigurationStatClass.DumpConfiguration(C_THIS_VERSION, ConfigurationStatClass.C_CONFIGURATION_REPORT_FILE_NAME);
                Console.WriteLine("Dump case library report to file  '{0}'...", ConfigurationStatClass.C_CLASS_LIBRARY_REPORT_FILE_NAME);
                List<string> classReportStringList;
                caseLibraryObj.GenerateReportOfAllCases(out classReportStringList);
                System.IO.File.WriteAllLines(ConfigurationStatClass.C_CLASS_LIBRARY_REPORT_FILE_NAME, classReportStringList);
            } // if

            // 7. Finish.
            Console.WriteLine("Finished: " + DateTime.Now.ToString() + ", elapsed: " + (DateTime.Now - startTime).ToString() + "\n");
            Console.Write("\nPress any key to exit! ");
            Console.ReadKey();

            // throw new System.NotImplementedException();
        } // Main

        // ====================================================================

    }
    // BlessYouMain
}
