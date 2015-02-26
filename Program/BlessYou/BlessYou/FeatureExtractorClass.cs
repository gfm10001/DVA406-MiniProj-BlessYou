// FeatureExtractor.cs
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

namespace BlessYou
{
    public static class FeatureExtractorClass
    {

        // ====================================================================

        public static void _loadFeatureList(List<SoundFileClass> fileNameList)
        {
            for (int i = 0; i < fileNameList.Count; ++i)
            {
                CaseClass caseClassObj = new CaseClass();

                //caseClassObj.
                // 1. Läs filenamns-listan
                // 2. För varje rad i listan:
                //      Skapa ett case med: CaseClass.cs
                //      placera i FListOfCases
            } // for i

            throw new System.NotImplementedException();
        } // _loadFeatureList

        // ====================================================================
    } // FeatureExtractorClass
}
