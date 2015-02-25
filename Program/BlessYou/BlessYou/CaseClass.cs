// Case.cs
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


    public class CaseClass
    {
        public static List<double> FFeatureWeights;

        WaveFileClass _WaveFileWorkArea;

        string _WavFile_FullPathAndFileNameStr;
        CaseStatusEnum _SneezeStatus;

        List<List<double>> _featureVector; // Each list element in the FV is a type of feature, each element consists of a number of values, one per time interval

        // ====================================================================

        void Case()
        {
            // Create FFeatureWeights and make sure sum = 1
            // Tips: anv. const declr i egen fil som ingångs data
            throw new System.NotImplementedException();
        } // Case

        // ====================================================================

        void _extractWavFileFeatures(string i_WavFile_FullPathAndFileNameStr)
        {
            // 1. Läs filen 
            //      utvärdera om fel
            // 2. Analysera filen
            // 3. Beräkna FV -> i _featureVector
            throw new System.NotImplementedException();
        } // _extractWavFileFeatures

        // ====================================================================

    }
}
