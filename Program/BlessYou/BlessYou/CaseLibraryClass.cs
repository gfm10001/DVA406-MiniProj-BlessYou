// CaseLibraryClass.cs
//
// DVA406 Intelligent Systems, Mdh, vt15
//
// History:
// 2015-02-24       Introduced.
// 2015-03-12/GF    Addition: DumpAllFeatureValuesOfAllCasesToFiles
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlessYou
{
    public class CaseLibraryClass
    {
        List<CaseClass> FListOfCases;

        // ====================================================================

        public List<CaseClass> ListOfCases
        {
            get
            {
                return FListOfCases;
            }
        } // ListOfCases

        // ====================================================================

        public CaseLibraryClass()
        {
            FListOfCases = new List<CaseClass>();
        } // CaseLibraryClass

        // ====================================================================

        public void AddCase(CaseClass i_NewCase)
        {
            FListOfCases.Add(i_NewCase);

        } // AddCase

        // ====================================================================

        public void RemoveCase(CaseClass i_Case)
        {
            bool b = FListOfCases.Remove(i_Case);
            if (false == b)
            {
                throw new System.NotImplementedException("RemoveCase: fails!");
            }
        } // RemoveCase

        // ====================================================================

        public void CountNrOfDifferentCases(out int o_NrOfSneeze, out int o_NrOfNoneSneezes)
        {
            o_NrOfSneeze = 0;
            o_NrOfNoneSneezes = 0;
            foreach (CaseClass cObj in FListOfCases)
            {
                if (cObj.SneezeStatus == EnumCaseStatus.csIsConfirmedSneeze)
                {
                    o_NrOfSneeze++;
                }
                if (cObj.SneezeStatus == EnumCaseStatus.csIsConfirmedNoneSneeze)
                {
                    o_NrOfNoneSneezes++;
                }
            } // foreach

        } // CountNrOfDifferentCases

        // ====================================================================

        public void GenerateReportOfAllCases(out List<string> o_ClassReportStringList)
        {
            o_ClassReportStringList = new List<string>();

            int ix = 0;
            foreach (CaseClass cObj in FListOfCases)
            {
                string s;
                s = cObj.AnalyseParamsToString();
                o_ClassReportStringList.Add(s);
                ++ix;
            } // foreach

        } // GenerateReportOfAllCases

        // ====================================================================

        public void DumpAllFeatureValuesOfAllCasesToFiles(string i_BaseFileName)
        {
            CaseClass dummyCaseObj = ListOfCases[0];
            int featureTypeIx = 0;
            foreach (FeatureBaseClass fbc in dummyCaseObj.FeatureTypeVector)
            {
                List<string> dumpListOfFeatures = new List<string>();

                // Dump raw values of this feature
                dumpListOfFeatures.Add("File Name: \t Feature " + fbc.FeatureName + " raw values:");
                foreach (CaseClass caseObj in ListOfCases)
                {
                    string s = caseObj.GetAllValuesOfThisFeatureTypeToString(featureTypeIx, 1.0);
                    dumpListOfFeatures.Add(s);
                } // foreach CaseClass

                dumpListOfFeatures.Add("");
                dumpListOfFeatures.Add("");
                dumpListOfFeatures.Add("");

                // Dump normalized DumpAllFeatureValuesOfAllCasesToFiles of this feature
                dumpListOfFeatures.Add("File Name: \t Feature " + fbc.FeatureName + " normalized values:");
                foreach (CaseClass caseObj in ListOfCases)
                {
                    string s = "?";
                    double maxValueOfThisFeature = caseObj.GetMaxFeatureValueOfThisFeature(featureTypeIx);
                    if (ConfigurationStatClass.C_EPSILON > maxValueOfThisFeature)
                    {
                        // Too small scale vcalue ??? 
                        s = caseObj.GetAllValuesOfThisFeatureTypeToString(featureTypeIx, 1.0) + "???? Not Scalable???";
                    }
                    else
                    {
                        s = caseObj.GetAllValuesOfThisFeatureTypeToString(featureTypeIx, 1.0 / maxValueOfThisFeature);
                    }
                    dumpListOfFeatures.Add(s);
                } // foreach CaseClass


                dumpListOfFeatures.Add("");
                dumpListOfFeatures.Add("");
                dumpListOfFeatures.Add("");


                // Dump difference betweeen one case and the others in this and another varying dimension
                int secondFeatureTypeIx = 0;
                foreach (FeatureBaseClass secondFbc in dummyCaseObj.FeatureTypeVector)
                {
                    if (featureTypeIx != secondFeatureTypeIx)
                    {
                        dumpListOfFeatures.Add("File Name: \t 2ndFeature " + secondFbc.FeatureName + " diffs one - to one:");
                        foreach (CaseClass caseSelectedObj in ListOfCases)
                        {
                            // Compare selected case to all others
                            string s = caseSelectedObj.GetDiffsOfAllValuesNormalizedOfThisFeatureTypeToString(featureTypeIx, secondFeatureTypeIx, ListOfCases);
                            dumpListOfFeatures.Add(s);

                        } // foreach CaseClass
                        dumpListOfFeatures.Add("");
                    }
                    secondFeatureTypeIx++;
                } // foreach    

                dumpListOfFeatures.Add("");
                dumpListOfFeatures.Add("");
                dumpListOfFeatures.Add("");

                // Dump Derivative (derivata...)
                dumpListOfFeatures.Add("File Name: \t Feature " + fbc.FeatureName + " changes:");
                foreach (CaseClass caseObj in ListOfCases)
                {
                    string s = "?";
                    s = caseObj.GetChangesOfAllValuesOfThisFeatureTypeToString(featureTypeIx);
                    dumpListOfFeatures.Add(s);
                } // foreach CaseClass

                // Save list to disk...
                System.IO.File.WriteAllLines(i_BaseFileName + "_" + fbc.FeatureName + ".xls", dumpListOfFeatures);
                featureTypeIx++;
            } // foreach FeatureBaseClass

        } // DumpAllFeatureValuesOfAllCasesToFiles

    } // CaseLibraryClass
}
