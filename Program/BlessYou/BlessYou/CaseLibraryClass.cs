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
            throw new System.NotImplementedException();
        } // AddCase

        // ====================================================================
        
        public void RemoveCase(CaseClass i_NewCase)
        {
            throw new System.NotImplementedException();
        } // RemoveCase

        // ====================================================================
    
    } // CaseLibraryClass
}
