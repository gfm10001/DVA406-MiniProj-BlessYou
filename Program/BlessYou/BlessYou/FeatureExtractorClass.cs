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

        public static void _loadFeatureList(CaseLibraryClass i_CaseLibraryObj, List<SoundFileClass> i_FileNameList)
        {
            for (int i = 0; i < i_FileNameList.Count; ++i)
            {
                CaseClass caseClassObj = new CaseClass();
                caseClassObj.ExtractWavFileFeatures(i_FileNameList[i].FileName);

                i_CaseLibraryObj.AddCase(caseClassObj);
            } // for i

            throw new System.NotImplementedException();
        } // _loadFeatureList

        // ====================================================================
    } // FeatureExtractorClass
}
