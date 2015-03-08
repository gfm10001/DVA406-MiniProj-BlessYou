﻿// BlessYouMain.cs
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
            const string C_THIS_VERSION = "Bless You v.0.2 of 2015-03-05";

            //Usage:
            //BlessYou P1 P2 [P3] where
            //P1 = name of text file with names of all .wav­files to be examined
            //P2 = File name for new problem | "all" : all files in Case Library run in sequence
            //P3 = path to directory for created .ftr­files (optional)

            Console.WriteLine(C_THIS_VERSION);
            Console.WriteLine("Starting: " + DateTime.Now.ToString() + "\n");

            CBRSystemClass cbrSystemObj = new CBRSystemClass();
            List<SoundFileClass> soundfileObjList;
            CaseLibraryClass caseLibraryObj;
            string newProblemFileName; // If empty run all problems
            string ftrFilePath; // If empty no storage of ftr files
            List<RetrievedCaseClass> retrievedMatchesList = new List<RetrievedCaseClass>();


            // 1. Decode Params
            DecodeParamClass.DecodeParam(args, out soundfileObjList, out newProblemFileName, out ftrFilePath);

            
            // 2. Create CASE-library
            FeatureExtractorClass._loadFeatureList(out caseLibraryObj, soundfileObjList);


            // Display calculated features
            CaseClass dummyCaseObj = caseLibraryObj.ListOfCases[0];

            int featureTypeIx = 0;
            foreach (FeatureBaseClass fbc in dummyCaseObj.FeatureTypeVector)
            {
                Console.WriteLine("\nfeatureTypeIx = {0} = '{1}'\n", featureTypeIx, fbc.FeatureName);
                foreach (CaseClass caseObj in caseLibraryObj.ListOfCases)
                {
                    string s = caseObj.FeatureTypeToString(featureTypeIx);
                    Console.WriteLine(s);
                } // foreach CaseClass
                featureTypeIx++;
            } // foreach FeatureBaseClass
            Console.WriteLine();

 
            // 3. Evaluate cases
            if ("" != newProblemFileName)
            {
                SoundFileClass newProblemSoundFileObj = new SoundFileClass();
                newProblemSoundFileObj.SoundFileName = newProblemFileName;
                newProblemSoundFileObj.SoundFileSneezeMarker = EnumSneezeMarker.smUnKnown;
                
                CaseClass newProblemObj = new CaseClass();
                newProblemObj.ExtractWavFileFeatures(newProblemSoundFileObj);

                List<CaseClass> caseList = new List<CaseClass>();
                caseList.AddRange(caseLibraryObj.ListOfCases);
                cbrSystemObj.Retrieve(newProblemObj, caseList, ConfigurationStatClass.C_NR_OF_RETRIEVED_CASES, out retrievedMatchesList);
                
                //4. Start reuse function
                EnumCaseStatus caseStatus; 
                cbrSystemObj.Reuse(retrievedMatchesList,out caseStatus);
            } // if
            else
            {
                CaseClass selectedProblemObj = new CaseClass();
                for (int ix = 0; ix < caseLibraryObj.ListOfCases.Count; ++ix)
                {
                    selectedProblemObj = caseLibraryObj.ListOfCases[ix];
                    List<CaseClass> caseMinusOneList = new List<CaseClass>();
                    for (int jx = 0; jx < caseLibraryObj.ListOfCases.Count; ++jx)
                    {
                        if (jx != ix)
                        {
                            caseMinusOneList.Add(caseLibraryObj.ListOfCases[jx]);
                        }
                    } // for jx
                    cbrSystemObj.Retrieve(selectedProblemObj, caseMinusOneList, ConfigurationStatClass.C_NR_OF_RETRIEVED_CASES, out retrievedMatchesList);
                    
                    //4. Start reuse function
                    EnumCaseStatus caseStatus;
                    cbrSystemObj.Reuse(retrievedMatchesList, out caseStatus);
                } // for ix
                // ToDo: utvärdera alla retrievedMatchesList för varje loop omgång
                //ToDo throw new System.NotImplementedException();
            }

             

            // 5. Skriv ut rapport
            Console.WriteLine("Number of matches = {0}", retrievedMatchesList.Count);
            for (int ix = 0; ix < retrievedMatchesList.Count; ++ix)
            {
                //ToDo Console.WriteLine("ix: {0} {1}", ix, retrievedMatchesList[ix].GetCurrentMatchingString());
            } // for ix


           // throw new System.NotImplementedException();
        } // Main

        // ====================================================================

    } // BlessYouMain
}
