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
    
    } // CaseLibraryClass
}
