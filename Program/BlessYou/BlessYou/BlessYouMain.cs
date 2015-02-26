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

            CBRSystem cbrSystem = new CBRSystem();
            List<SoundFileClass> fileNameList;
            
            // 1. Decode Params
            DecodeParamClass.DecodeParam(args, out fileNameList);

            // 2. Create CASE-library
            FeatureExtractorClass._loadFeatureList(fileNameList);
 
            // 2. Create CASE-library
            // 3. Skapa CBR System
            // 4. Skriv ut rapport

            throw new System.NotImplementedException();
        } // Main

        // ====================================================================

    } // BlessYouMain
}
