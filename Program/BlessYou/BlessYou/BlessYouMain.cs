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
// 2015-02-24   Introduced.
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
            const string C_THIS_VERSION = "Bless You v.0.4/1 of 2015-03-11";

            //Usage:
            //BlessYou P1 P2 [P3] where
            //P1 = name of text file with names of all .wav­files to be examined
            //P2 = File name for new problem | "all" : all files in Case Library run in sequence
            //P3 = path to directory for created .ftr­files (optional)

            Console.WriteLine(C_THIS_VERSION);
            Console.WriteLine("Starting: " + DateTime.Now.ToString() + "\n");

            CBRSystemClass CBRSystemClass = new CBRSystemClass();
            ConfigurationStatClass config = CBRSystemClass.GenerateRandomConfig(100);
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


            // Display calculated features
            CaseClass dummyCaseObj = caseLibraryObj.ListOfCases[0];

            int featureTypeIx = 0;
            foreach (FeatureBaseClass fbc in dummyCaseObj.FeatureTypeVector)
            {
                List<string> dumpListOfFeatures = new List<string>();

                Console.WriteLine("\nfeatureTypeIx = {0} = '{1}'\n", featureTypeIx, fbc.FeatureName);
                foreach (CaseClass caseObj in caseLibraryObj.ListOfCases)
                {
                    string s = caseObj.FeatureTypeToString(featureTypeIx);
                    dumpListOfFeatures.Add(s);
                   // Console.WriteLine(s);
                } // foreach CaseClass
                System.IO.File.WriteAllLines(fbc.FeatureName + ".xls", dumpListOfFeatures);
                featureTypeIx++;
            } // foreach FeatureBaseClass
            Console.WriteLine();

            CBRSystemClass.EvaluateFeatureOneByOne(caseLibraryObj);


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
                int nrOfConfiremedNoneSneezes;
                int correctSneezes = 0;
                int inCorrectSneezes = 0;
                int correctNoneSneezes = 0;
                int inCorrectNoneSneezes = 0;

                List<string> correctList = new List<string>();

                List<string> wronglist = new List<string>();

                CaseClass selectedProblemObj = new CaseClass();
                for (int ix = 0; ix < caseLibraryObj.ListOfCases.Count; ++ix)
                {

                    selectedProblemObj = caseLibraryObj.ListOfCases[ix];
                    Console.WriteLine("\nFilename: " + selectedProblemObj.WavFile_FullPathAndFileNameStr);
                    List<CaseClass> caseLibaryMinusOneCaseList = new List<CaseClass>();
                    for (int jx = 0; jx < caseLibraryObj.ListOfCases.Count; ++jx)
                    {
                        if (jx != ix)
                        {
                            caseLibaryMinusOneCaseList.Add(caseLibraryObj.ListOfCases[jx]);
                        }
                    } // for jx
                    CBRSystemClass.Retrieve(selectedProblemObj, caseLibaryMinusOneCaseList, ConfigurationStatClass.C_NR_OF_RETRIEVED_CASES, out retrievedMatchesList);

                    //4. Start reuse function
                    EnumCaseStatus caseStatus;
                    CBRSystemClass.Reuse(retrievedMatchesList, out caseStatus);
                    if (selectedProblemObj.SneezeStatus == EnumCaseStatus.csIsConfirmedSneeze)
                    {
                        if (caseStatus == EnumCaseStatus.csIsProposedSneeze)
                        {
                            correctList.Add(selectedProblemObj.WavFile_FullPathAndFileNameStr);
                            correctSneezes++;
                        }
                        else
                        {
                            wronglist.Add(selectedProblemObj.WavFile_FullPathAndFileNameStr);
                            inCorrectSneezes++;
                            Console.WriteLine("GUESSED WRONG HERE on SNEEZE!");
                        }
                    }
                    if (selectedProblemObj.SneezeStatus == EnumCaseStatus.csIsConfirmedNoneSneeze)
                    {
                        if (caseStatus == EnumCaseStatus.csIsProposedNoneSneeze)
                        {
                            correctList.Add(selectedProblemObj.WavFile_FullPathAndFileNameStr);
                            correctNoneSneezes++;
                        }
                        else
                        {
                            //System.Diagnostics.Debugger.Break();
                            wronglist.Add(selectedProblemObj.WavFile_FullPathAndFileNameStr);
                            inCorrectNoneSneezes++;
                            Console.WriteLine("GUESSED WRONG HERE on None-SNEEZE!");
                        }
                    }

                } // for ix

                double total = correctSneezes + correctNoneSneezes + inCorrectSneezes + inCorrectNoneSneezes;

                caseLibraryObj.CountNrOfDifferentCases(out nrOfConfirmedSneezes, out nrOfConfiremedNoneSneezes);

                Console.WriteLine();
				Console.WriteLine("In Total Case Library: Nr of confirmed sneezes:      {0, 4:0}", nrOfConfirmedSneezes);
                Console.WriteLine("In Total Case Library: Nr of confirmed none-sneezes: {0, 4:0}", nrOfConfiremedNoneSneezes);

                Console.WriteLine("Number of correct guesses:                           {0, 4:0} = {1, 3:0.0}%", correctSneezes + correctNoneSneezes, ((double)(correctSneezes + correctNoneSneezes) / total) * 100.0);
                
                Console.WriteLine("Number of correct SNEEZE guesses:                    {0, 4:0} = {1, 3:0.0}%", correctSneezes, ((double)correctSneezes / total) * 100.0);
                Console.WriteLine("Number of correct NONE SNEEZES guesses:              {0, 4:0} = {1, 3:0.0}%", correctNoneSneezes, ((double)correctNoneSneezes / total) * 100.0);
                Console.WriteLine("Number of incorrect SNEEZE guesses:                  {0, 4:0} = {1, 3:0.0}%", inCorrectSneezes, ((double)inCorrectSneezes / total) * 100.0);
                Console.WriteLine("Number of incorrect NONE SNEEZES guesses:            {0, 4:0} = {1, 3:0.0}%", inCorrectNoneSneezes, ((double)inCorrectNoneSneezes / total) * 100.0);
                
				System.IO.File.WriteAllLines("./Wrongs.txt", wronglist);
                System.IO.File.WriteAllLines("./Corrects.txt", correctList);
                // ToDo: utvärdera alla retrievedMatchesList för varje loop omgång
                //ToDo throw new System.NotImplementedException();
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
                Console.WriteLine("Dump configuratiion report to file '{0}'...", ConfigurationStatClass.C_CONFIGURATION_REPORT_FILE_NAME);
                ConfigurationStatClass.DumpConfiguration("Main", ConfigurationStatClass.C_CONFIGURATION_REPORT_FILE_NAME);
                Console.WriteLine("Dump case library report to file '{0}'...", ConfigurationStatClass.C_CLASS_LIBRARY_REPORT_FILE_NAME);
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
