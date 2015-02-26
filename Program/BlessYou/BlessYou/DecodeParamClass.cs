using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BlessYou
{
    public static class DecodeParamClass
    {
        //Usage:
        //BlessYou P1 [P2] where
        //P1 = name of text file with names of all .wav­files to be examined
        //P2 = path to directory for created .ftr­files (optional)
        public static void DecodeParam(string[] i_Param, out List<SoundFileClass> o_FileNameList)
        {
            string samplesFileName;
            string outputFilePath;
            string tempStr;
            string[] tempStrArr;
            o_FileNameList = new List<SoundFileClass>();

            if (1 == i_Param.Length)
            {
                samplesFileName = i_Param[0];
            } // if
            else if (2 == i_Param.Length)
            {
                samplesFileName = i_Param[0];
                outputFilePath = i_Param[1];
            }
            else
            {
                tempStr = "Usage:\n" +
                                 "BlessYou P1 [P2] where\n" +
                                 "P1 = name of text file with names of all .wav­files to be examined\n" +
                                 "P2 = path to directory for created .ftr­files (optional)\n";
                throw new System.Exception(tempStr);
            }

            // Decode sampleFilenames (one per row)
            // Format of list file used as P1: one line per .wav­file:
            string paramStr = System.IO.File.ReadAllText(samplesFileName);
            char[] lineFeedSep = { '\n' };
            tempStrArr = paramStr.Split(lineFeedSep);
            
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

                soundFileObj.FileName = Path.GetFullPath(str[1]);

                o_FileNameList.Add(soundFileObj);
            } // for i

        } // DecodeParam
    } // DecodeParamClass
}
