﻿// FeatureExtractor.cs
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
using BlessYouGUI;

namespace BlessYou
{
    public static class FeatureExtractorClass
    {

        // ====================================================================

        public static void _loadFeatureList(frmCaseBaseLibrary i_CaseBaseLibraryForm, out CaseLibraryClass o_CaseLibraryObj, List<SoundFileClass> i_FileNameList, ConfigurationDynClass i_config = null)
        {
            o_CaseLibraryObj = new CaseLibraryClass();
            for (int i = 0; i < i_FileNameList.Count; ++i)
            {
                CaseClass caseClassObj = new CaseClass();
                caseClassObj.WavFile_FullPathAndFileNameStr = i_FileNameList[i].SoundFileName;
                caseClassObj.ExtractWavFileFeatures(i_FileNameList[i], true, i_config);

                o_CaseLibraryObj.AddCase(caseClassObj);
                i_CaseBaseLibraryForm.Update_Lists(o_CaseLibraryObj.ListOfCases);
            } // for i
        } // _loadFeatureList

        // ====================================================================

    } // FeatureExtractorClass
}