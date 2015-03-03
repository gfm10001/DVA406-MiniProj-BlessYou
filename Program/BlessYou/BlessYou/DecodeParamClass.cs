using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BlessYou
{
    public static class DecodeParamClass
    {
        // o_NewProblemFileName parameter: if empty means run all cases if not run this specific new problem 
        public static void DecodeParam(string[] i_Param, out List<SoundFileClass> o_FileNameList, out string o_NewProblemFileName, out string o_FtrFilePath)
        {
            string samplesFileName;
            string tempStr;
            string[] tempStrArr;
            o_FileNameList = new List<SoundFileClass>();

            o_FtrFilePath = "";
            if (2 == i_Param.Length)
            {
                samplesFileName = i_Param[0];
                o_NewProblemFileName = i_Param[1];
            } // if
            else if (3 == i_Param.Length)
            {
                samplesFileName = i_Param[0];
                o_NewProblemFileName = i_Param[1];
                o_FtrFilePath = i_Param[2];
            }
            else
            {
                tempStr = "Usage:\n" +
                                 "BlessYou P1 P2 [P3] where\n" +
                                 "P1 = name of text file with names of all .wav­files to be examined\n" +
                                 "P2 = File name for new problem | \"all\" : all files in Case Library run in sequence\n" + 
                                 "P3 = path to directory for created .ftr­files (optional)\n";
                throw new System.Exception(tempStr);
            }

            if ("all" == o_NewProblemFileName.ToLower())
            {
                o_NewProblemFileName = "";
            } // if

            // Decode sampleFilenames (one per row)
            // Format of list file used as P1: one line per .wav­file:
            string paramStr = System.IO.File.ReadAllText(samplesFileName);
            char[] lineFeedSep = { '\n' };
            tempStrArr = paramStr.Split(lineFeedSep);

            string tempS1 = Path.GetFullPath(samplesFileName);
            string tempS2 = Path.GetDirectoryName(tempS1);
            tempS2 = tempS2 + "\\";

            // Decode each sampleFile
            // line = <marker for type of sound> TAB [<path>]<filename of .wav­file>
            char[] tabSep = { '\t' };
            string[] str;
            for (int i = 0; i < tempStrArr.Length; ++i)
            {
                SoundFileClass soundFileObj = new SoundFileClass();

                str = tempStrArr[i].Split(tabSep);

                if ("0" == str[0])
                {
                    soundFileObj.Marker = EnumSneezeMarker.smNoSneeze;
                }
                else if ("1" == str[0])
                {
                    soundFileObj.Marker = EnumSneezeMarker.smSneeze;
                }

                string s = tempS2 + str[1];
                soundFileObj.FileName = Path.GetFullPath(s);

                o_FileNameList.Add(soundFileObj);
            } // for i

        } // DecodeParam
    } // DecodeParamClass
}
