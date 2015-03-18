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
// 2015-03-17/GF    Added informational printouts.
// 2015-03-18/GF    New param option: "addx" -> opens new dialoge after maintanance loop.
//


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace BlessYou
{
    class BlessYouMain
    {
        // ====================================================================
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Form1 form = new Form1();
            //Application.Run(new Form1());

            Thread t = new Thread(() => Application.Run(form));
            t.Start();

            //Form1 form = new Form1();
            //form.Show();
            //form.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;

            const string C_THIS_VERSION = "Bless You v.0.8/0 of 2015-03-18";
            DateTime startTime;

            Console.WriteLine(C_THIS_VERSION + " (Parallell Execution: " + ConfigurationStatClass.USE_PARALLEL_EXECUTION + ")");

            CBRSystemClass CBRSystemClass = new CBRSystemClass();
            ConfigurationDynClass config = new ConfigurationDynClass(); // CBRSystemClass.GenerateRandomConfig(100);
            List<SoundFileClass> allSoundFilesObjList;
            List<SoundFileClass> usedSoundFilesObjList;
            CaseLibraryClass caseLibraryObj;
            string newProblemFileName; // If empty run all problems
            string ftrFilePath; // If empty no storage of ftr files
            List<RetrievedCaseClass> retrievedMatchesList = new List<RetrievedCaseClass>();
            int nrOfConfirmedSneezes;
            int nrOfConfirmedNoneSneezes;
            bool interactionIsOn;


            // 1. Decode Params
            //DecodeParamClass.DecodeParam2(args, out Liblist, out retrievedMatchesList);
            DecodeParamClass.DecodeParam(args, out allSoundFilesObjList, out newProblemFileName, out ftrFilePath, out interactionIsOn);
            startTime = DateTime.Now;
            Console.WriteLine("Starting: " + startTime.ToString() + ", config file: " + args[0] + " \n");

            // 2. Create CASE-library
            // First extract 50+50 random files for use as first library and load those...
            Console.WriteLine("1. Read default Case Library: limit to 50 Sneezes and 50 None-Sneezes selected randomly from " + allSoundFilesObjList.Count + " files...\n");
            HelperStaticClass.GetRandomSelection(allSoundFilesObjList, out usedSoundFilesObjList);
            FeatureExtractorClass._loadFeatureList(out caseLibraryObj, usedSoundFilesObjList, config);



            // Dump calculated features 
            //Console.Write("Dump all features to files... ");
            //caseLibraryObj.DumpAllFeatureValuesOfAllCasesToFiles("Feature");

            Console.WriteLine();

            // CBRSystemClass.EvaluateFeatureOneByOne(caseLibraryObj);

            //config.C_M_AVERAGE_FEATURE_WEIGHT = 0;
            //config.C_M_CREST_FACTOR_WEIGHT = 0;
            //config.C_M_LOMONT_FFT_12_FEATURE_WEIGHT = 0;
            //config.C_M_LOMONT_FFT_14_FEATURE_WEIGHT = 1;
            //config.C_M_LOMONT_FFT_16_FEATURE_WEIGHT = 0;
            //config.C_M_PASSING_ZERO_WEIGHT = 0;
            //config.C_M_PEAK_FEATURE_WEIGHT = 0;
            //config.C_M_PEAK2PEAK_FEATURE_WEIGHT = 0;
            //config.C_M_RMS_FEATURE_WEIGHT = 0;
            //CBRSystemClass.EvaluateFeatureVectors(caseLibraryObj, config);


            // 3. Evaluate cases
            if ("" != newProblemFileName)
            {
                // Evaluate single case, then prompt operator for a new case.
                do
                {
                    EvaluateSingleCase(interactionIsOn, config, caseLibraryObj, ref newProblemFileName, ref retrievedMatchesList);
                    interactionIsOn = true;
                } while (newProblemFileName != "");

            } // if

            else
            {
                // Walk through improvments in Case Library
                int correctSneezes = 0;
                int inCorrectSneezes = 0;
                int correctNoneSneezes = 0;
                int inCorrectNoneSneezes = 0;

                List<string> correctList = new List<string>();
                List<string> wronglist = new List<string>();

                int numberofCasesForMajorityVote;
                if (ConfigurationStatClass.RUN_ALL_MAJORITY_VOTE_CASE_NUMBERS)
                {
                    numberofCasesForMajorityVote = 1;
                }
                else
                {
                    numberofCasesForMajorityVote = ConfigurationStatClass.C_NUMBER_OF_CASES_TO_USE_FOR_MAJORITY_VOTE;
                }

                Console.WriteLine("\n2. Add one random unused case. Evalute 1-out-of-all and remove worst case in Case Library.");
                Console.WriteLine("   Number of cases (k) to vote from: {0}\n", numberofCasesForMajorityVote);

                // do - while to read new cases and evaluate against current library, exchange worst case in library with the current case.
                bool isMoreToDo = true;
                bool checkPhase = false;
                while (isMoreToDo)
                {
                    List<RetrievedCaseClass> accumulatedSimilarityValuesRetrievedMatchesList = new List<RetrievedCaseClass>();

                    // Prepare for revise and retain
                    foreach (CaseClass c in caseLibraryObj.ListOfCases)
                    {
                        RetrievedCaseClass rCCObj = new RetrievedCaseClass(c);
                        accumulatedSimilarityValuesRetrievedMatchesList.Add(rCCObj);
                    }


                    CaseClass selectedProblemObj = new CaseClass();

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

                        // Alternative function call using similarity value
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
                                wronglist.Add(selectedProblemObj.WavFile_FullPathAndFileNameStr);
                                inCorrectNoneSneezes++;
                            }
                        }
                        CBRSystemClass.AccumulateSimilarityValuesInList(retrievedMatchesList, accumulatedSimilarityValuesRetrievedMatchesList);
                    } // for ix

                    double total = correctSneezes + correctNoneSneezes + inCorrectSneezes + inCorrectNoneSneezes;

                    caseLibraryObj.CountNrOfDifferentCases(out nrOfConfirmedSneezes, out nrOfConfirmedNoneSneezes);

                    double xMax = Double.NaN;
                    double xMin = Double.NaN;
                    if (0 < CorrectSneezesSimilarityValue.Count)
                    {
                        xMax = CorrectSneezesSimilarityValue.Max();
                        xMin = CorrectSneezesSimilarityValue.Min();
                    } // if
                    //Console.WriteLine("highestCorrect = {0}, lowestCorrect = {1}", xMax, xMin);

                    xMax = Double.NaN;
                    xMin = Double.NaN;
                    if (0 < WrongSneezesSimilarityValue.Count)
                    {
                        xMax = WrongSneezesSimilarityValue.Max();
                        xMin = WrongSneezesSimilarityValue.Min();
                    } // if
                    //Console.WriteLine("highestWrong = {0}, lowestWrong = {1}", xMax, xMin);

                    //Console.WriteLine();
                    //Console.WriteLine("In Total Case Library: Nr of confirmed sneezes:      {0, 4:0}", nrOfConfirmedSneezes);
                    //Console.WriteLine("In Total Case Library: Nr of confirmed none-sneezes: {0, 4:0}", nrOfConfirmedNoneSneezes);

                  //  Console.WriteLine("Number of correct guesses:    >>> >>> >>>> >>> >>>   {0, 4:0} = {1, 3:0.0}%", correctSneezes + correctNoneSneezes, ((double)(correctSneezes + correctNoneSneezes) / total) * 100.0);

                    //Console.WriteLine("Number of correct SNEEZE guesses:                    {0, 4:0} = {1, 3:0.0}%", correctSneezes, ((double)correctSneezes / total) * 100.0);
                    //Console.WriteLine("Number of correct NONE SNEEZES guesses:              {0, 4:0} = {1, 3:0.0}%", correctNoneSneezes, ((double)correctNoneSneezes / total) * 100.0);
                    //Console.WriteLine("Number of incorrect SNEEZE guesses:                  {0, 4:0} = {1, 3:0.0}%", inCorrectSneezes, ((double)inCorrectSneezes / total) * 100.0);
                    //Console.WriteLine("Number of incorrect NONE SNEEZES guesses:            {0, 4:0} = {1, 3:0.0}%", inCorrectNoneSneezes, ((double)inCorrectNoneSneezes / total) * 100.0);

                    System.IO.File.WriteAllLines("./Wrongs.txt", wronglist);
                    System.IO.File.WriteAllLines("./Corrects.txt", correctList);
                    //    numberofCasesForMajorityVote += 2;

                    RetrievedCaseClass caseToRemoveFromCaseLibrary = new RetrievedCaseClass();
                    CBRSystemClass.Revise(accumulatedSimilarityValuesRetrievedMatchesList, out caseToRemoveFromCaseLibrary);

                    if (true == checkPhase)
                    {
                        // In this phase - remove the worst case

                        // Remove worst case to give space for new
                        Console.WriteLine("Correct: {0, 3:0.0}%  - {1,40}\n", ((double)(correctSneezes + correctNoneSneezes) / total) * 100.0, 
                                         System.IO.Path.GetFileName(caseToRemoveFromCaseLibrary.WavFile_FullPathAndFileNameStr));
                        CBRSystemClass.Retain(caseToRemoveFromCaseLibrary, caseLibraryObj);

                        checkPhase = false;
                    }
                    else
                    {
                        // In this phase: add another file to the case library
                        // Try to get a random unused sound file--- 
                        SoundFileClass unusedProblemSoundFileObj;
                        HelperStaticClass.GetUnusedRandomFile(allSoundFilesObjList, out unusedProblemSoundFileObj);
                        if (null == unusedProblemSoundFileObj)
                        {
                            isMoreToDo = false;
                            break;
                        }

                        // ... treat as new case
                        CaseClass unusedProblemObj = new CaseClass();
                        unusedProblemObj.WavFile_FullPathAndFileNameStr = unusedProblemSoundFileObj.SoundFileName;
                        unusedProblemObj.ExtractWavFileFeatures(unusedProblemSoundFileObj, false, config);
                        Console.WriteLine("Correct: {0, 3:0.0}%  + {1, 40}", ((double)(correctSneezes + correctNoneSneezes) / total) * 100.0, 
                                           System.IO.Path.GetFileName(unusedProblemSoundFileObj.SoundFileName));
                        caseLibraryObj.AddCase(unusedProblemObj);
                        checkPhase = true;
                    } // else


                }  // IsMoreToDo

                // Evaluate single case, then prompt operator for a new case.
                if (true == interactionIsOn)
                {
                    do
                    {
                        newProblemFileName = "";
                        EvaluateSingleCase(interactionIsOn, config, caseLibraryObj, ref newProblemFileName, ref retrievedMatchesList);
                    } while (newProblemFileName != "");
                }
            
            } // else




            Console.WriteLine("\n3. Summary reports.\n");
 
            // 5. Skriv ut rapport
            caseLibraryObj.CountNrOfDifferentCases(out nrOfConfirmedSneezes, out nrOfConfirmedNoneSneezes);
            Console.WriteLine();
            Console.WriteLine("In Total Case Library: Nr of confirmed sneezes:      {0, 4:0}", nrOfConfirmedSneezes);
            Console.WriteLine("In Total Case Library: Nr of confirmed none-sneezes: {0, 4:0}", nrOfConfirmedNoneSneezes);

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

        private static void EvaluateSingleCase(bool i_InteractionIsOn, ConfigurationDynClass i_Config, CaseLibraryClass i_CaseLibraryObj, ref string io_NewProblemFileName, ref List<RetrievedCaseClass> i_RetrievedMatchesList)
        {
            if (true == i_InteractionIsOn)
            {
                Console.Write("\n - Enter a new problem file name (or empty line to quit) : ");
                io_NewProblemFileName = Console.ReadLine();

            }

            if ("" == io_NewProblemFileName)
            {
                return;
            }

            SoundFileClass newProblemSoundFileObj = new SoundFileClass();
            newProblemSoundFileObj.SoundFileName = io_NewProblemFileName;
            newProblemSoundFileObj.SoundFileSneezeMarker = EnumSneezeMarker.smUnKnown;

            CaseClass newProblemObj = new CaseClass();
            newProblemObj.WavFile_FullPathAndFileNameStr = newProblemSoundFileObj.SoundFileName;
            try
            {
                newProblemObj.ExtractWavFileFeatures(newProblemSoundFileObj, true, i_Config);
                CBRSystemClass.RetrieveUsingSimilarityfunction(newProblemObj, i_CaseLibraryObj.ListOfCases, out i_RetrievedMatchesList);

                // 4. Start reuse function
                EnumCaseStatus caseStatus;
                CBRSystemClass.ReuseUsingMajorityVote(i_RetrievedMatchesList, 5, EnumCaseStatus.csUnknown, out caseStatus);

                Console.WriteLine("new Problem detected as " + caseStatus);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nERROR - msg='" + ex.Message + "' - retry!");
            }

        } // EvaluateSingleCase

        // ====================================================================
    }
    // BlessYouMain
}
