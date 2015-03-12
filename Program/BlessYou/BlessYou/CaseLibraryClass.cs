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
            FListOfCases.Remove(i_Case);
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

    } // CaseLibraryClass
}
