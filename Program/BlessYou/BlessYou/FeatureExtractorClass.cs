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

        public static void _loadFeatureList(out CaseLibraryClass o_CaseLibraryObj, List<SoundFileClass> i_FileNameList)
        {
            o_CaseLibraryObj = new CaseLibraryClass();
            for (int i = 0; i < i_FileNameList.Count; ++i)
            {
                CaseClass caseClassObj = new CaseClass();
                caseClassObj.ExtractWavFileFeatures(i_FileNameList[i].FileName);

                o_CaseLibraryObj.AddCase(caseClassObj);
            } // for i
        } // _loadFeatureList

        // ====================================================================
    } // FeatureExtractorClass
}
