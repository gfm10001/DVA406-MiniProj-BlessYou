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
            const string C_THIS_VERSION = "Bless You v.0.6/2 of 2015-03-15";

            //Usage:
            //BlessYou P1 P2 [P3] where
            //P1 = name of text file with names of all .wav­files to be examined
            //P2 = File name for new problem | "all" : all files in Case Library run in sequence
            //P3 = path to directory for created .ftr­files (optional)

            Console.WriteLine(C_THIS_VERSION);
            Console.WriteLine("Starting: " + DateTime.Now.ToString() + "\n");

            CBRSystemClass CBRSystemClass = new CBRSystemClass();
            ConfigurationStatClass config = null;// = CBRSystemClass.GenerateRandomConfig(100);
            List<SoundFileClass> soundfileObjList;
            //List<SoundFileClass> Liblist;
            CaseLibraryClass caseLibraryObj;
            string newProblemFileName; // If empty run all problems
            string ftrFilePath; // If empty no storage of ftr files
            List<RetrievedCaseClass> retrievedMatchesList = new List<RetrievedCaseClass>();


            // 1. Decode Params
            //DecodeParamClass.DecodeParam2(args, out Liblist, out retrievedMatchesList);
            DecodeParamClass.DecodeParam(args, out soundfileObjList, out newProblemFileName, out ftrFilePath);



            // 2. Create CASE-library
            FeatureExtractorClass._loadFeatureList(out caseLibraryObj, soundfileObjList, config);


            // Dump calculated features 
            Console.Write("Dump all features to files... ");
            caseLibraryObj.DumpAllFeatureValuesOfAllCasesToFiles("Feature");

            Console.WriteLine();

            //CBRSystemClass.EvaluateFeatureOneByOne(caseLibraryObj);


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


                // While loop introduced as an alternative for using similarityvalue and majority vote
                int numberofCases = 1;
                while (numberofCases <= ConfigurationStatClass.C_NUMBER_OF_CASES_TO_REUSE)
                {
                    Console.WriteLine("\n\nNumber of cases to vote from: {0}", numberofCases);
                    correctSneezes = 0;
                    inCorrectSneezes = 0;
                    correctNoneSneezes = 0;
                    inCorrectNoneSneezes = 0;
                    double lowestCorrect = 1;
                    double lowestWrong = 1;
                    double highestCorrect = 0;
                    double highestWrong = 0;

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
                        CBRSystemClass.RetrieveUsingSimilarityfunction(selectedProblemObj, caseLibaryMinusOneCaseList, ConfigurationStatClass.C_NR_OF_RETRIEVED_CASES, out retrievedMatchesList);

                        //4. Start reuse function
                        EnumCaseStatus caseStatus;

                        // Alternative functioncall using majority vote
                        // Original:
                        //CBRSystemClass.Reuse(retrievedMatchesList, out caseStatus);
                        // Alternative:
                        CBRSystemClass.ReuseUsingMajorityVote(retrievedMatchesList, numberofCases, out caseStatus);
                        if (selectedProblemObj.SneezeStatus == EnumCaseStatus.csIsConfirmedSneeze)
                        {
                            if (caseStatus == EnumCaseStatus.csIsProposedSneeze)
                            {
                                    if (lowestCorrect > retrievedMatchesList[0].SimilarityValue)
                                    {
                                        lowestCorrect = retrievedMatchesList[0].SimilarityValue;
                                    }
                                    if (highestCorrect < retrievedMatchesList[0].SimilarityValue)
                                    {
                                        highestCorrect = retrievedMatchesList[0].SimilarityValue;
                                    }
                                correctList.Add(selectedProblemObj.WavFile_FullPathAndFileNameStr);
                                correctSneezes++;
                            }
                            else
                            {
                                if (lowestWrong > retrievedMatchesList[0].SimilarityValue)
                                {
                                    lowestWrong = retrievedMatchesList[0].SimilarityValue;
                                }
                                if (highestWrong < retrievedMatchesList[0].SimilarityValue)
                                {
                                    highestWrong = retrievedMatchesList[0].SimilarityValue;
                                }
                                wronglist.Add(selectedProblemObj.WavFile_FullPathAndFileNameStr);
                                inCorrectSneezes++;
                                //Console.WriteLine("GUESSED WRONG HERE on SNEEZE!");
                            }
                        }
                        if (selectedProblemObj.SneezeStatus == EnumCaseStatus.csIsConfirmedNoneSneeze)
                        {
                            if (caseStatus == EnumCaseStatus.csIsProposedNoneSneeze)
                            {
                                if (lowestCorrect > retrievedMatchesList[0].SimilarityValue)
                                {
                                    lowestCorrect = retrievedMatchesList[0].SimilarityValue;
                                }
                                if (highestCorrect < retrievedMatchesList[0].SimilarityValue)
                                {
                                    highestCorrect = retrievedMatchesList[0].SimilarityValue;
                                }
                                correctList.Add(selectedProblemObj.WavFile_FullPathAndFileNameStr);
                                correctNoneSneezes++;
                            }
                            else
                            {
                                if (lowestWrong > retrievedMatchesList[0].SimilarityValue)
                                {
                                    lowestWrong = retrievedMatchesList[0].SimilarityValue;
                                }
                                if (highestWrong < retrievedMatchesList[0].SimilarityValue)
                                {
                                    highestWrong = retrievedMatchesList[0].SimilarityValue;
                                }
                                //System.Diagnostics.Debugger.Break();
                                wronglist.Add(selectedProblemObj.WavFile_FullPathAndFileNameStr);
                                inCorrectNoneSneezes++;
                                //Console.WriteLine("GUESSED WRONG HERE on None-SNEEZE!");
                            }
                        }
                    } // for ix


                    double total = correctSneezes + correctNoneSneezes + inCorrectSneezes + inCorrectNoneSneezes;

                    caseLibraryObj.CountNrOfDifferentCases(out nrOfConfirmedSneezes, out nrOfConfirmedNoneSneezes);

                    Console.WriteLine();
                    Console.WriteLine("highestCorrect = {0}, lowestCorrect = {1}", highestCorrect, lowestCorrect);
                    Console.WriteLine("highestWrong = {0}, lowestWrong = {1}", highestWrong, lowestWrong);
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
                    // ToDo: utvärdera alla retrievedMatchesList för varje loop omgång
                    //ToDo throw new System.NotImplementedException();
                    numberofCases += 2;
                } // While loop introduced as an alternative for using similarityvalue and majority vote
            } // else



            // 5. Skriv ut rapport
            Console.WriteLine("Number of matches = {0}", retrievedMatchesList.Count);
            for (int ix = 0; ix < retrievedMatchesList.Count; ++ix)
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
            Console.Write("\nPress any key to exit! ");
            Console.ReadKey();

            // throw new System.NotImplementedException();
        } // Main

        // ====================================================================

    }
    // BlessYouMain
}
