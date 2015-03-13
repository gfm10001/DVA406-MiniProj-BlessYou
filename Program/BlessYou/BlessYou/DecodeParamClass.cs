// DecodeParamClass.cs
//
// DVA406 Intelligent Systems, Mdh, vt15
//
// History:
// 2015-02-24       Introduced.
// 2015-03-04/GF    DecodeParam: Fixed handling of too short lines.
// 2015-03-10/GF    DecodeParam: Added handling of comment-lines

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BlessYou
{
    public static class DecodeParamClass
    {
        public static void DecodeParam2(string[] i_Param, out List<SoundFileClass> o_LibList, out List<RetrievedCaseClass> o_NewProblemFileNames)
        {

            o_LibList = new List<SoundFileClass>();
            o_NewProblemFileNames = new List<RetrievedCaseClass>();

            string lib = i_Param[0].ToLower();
            string testcases = i_Param[1].ToLower();

            string paramListOfFileNamesStr = System.IO.File.ReadAllText(lib);
            char[] lineFeedSep = { '\n' };
            string[] listOfLibFiles;
            listOfLibFiles = paramListOfFileNamesStr.Split(lineFeedSep);

            paramListOfFileNamesStr = System.IO.File.ReadAllText(testcases);
            string[] listOfCaseFiles;
            listOfCaseFiles = paramListOfFileNamesStr.Split(lineFeedSep);

            string tempS1 = Path.GetFullPath(lib);
            string tempS2 = Path.GetDirectoryName(tempS1);
            tempS2 = tempS2 + "\\";


            char[] tabSep = { '\t' };
            string[] linePartsArr;
            for (int i = 0; i < listOfLibFiles.Length; ++i)
            {
                SoundFileClass soundFileObj = new SoundFileClass();

                // Skip too short lines.
                if (listOfLibFiles[i].Length < 2)
                {
                    continue;
                }

                // Skip any comment lines (starts with ";")
                if (';' == listOfLibFiles[i][0])
                {
                    continue;
                }


                linePartsArr = listOfLibFiles[i].Split(tabSep);

                if ("0" == linePartsArr[0])
                {
                    soundFileObj.SoundFileSneezeMarker = EnumSneezeMarker.smNoSneeze;
                }
                else if ("1" == linePartsArr[0])
                {
                    soundFileObj.SoundFileSneezeMarker = EnumSneezeMarker.smSneeze;
                }

                string s = tempS2 + linePartsArr[1];
                soundFileObj.SoundFileName = Path.GetFullPath(s);

                o_LibList.Add(soundFileObj);

            }



            for (int i = 0; i < listOfCaseFiles.Length; ++i)
            {
                RetrievedCaseClass soundFileObj = new RetrievedCaseClass();

                // Skip too short lines.
                if (listOfCaseFiles[i].Length < 2)
                {
                    continue;
                }

                // Skip any comment lines (starts with ";")
                if (';' == listOfCaseFiles[i][0])
                {
                    continue;
                }


                linePartsArr = listOfCaseFiles[i].Split(tabSep);
                string s = tempS2 + linePartsArr[1];
                soundFileObj.WavFile_FullPathAndFileNameStr = Path.GetFullPath(s);

                o_NewProblemFileNames.Add(soundFileObj);
            }
        }



        // o_NewProblemFileName parameter: if empty means run all cases if not run this specific new problem 
        public static void DecodeParam(string[] i_Param, out List<SoundFileClass> o_FileNameList, out string o_NewProblemFileName, out string o_FtrFilePath)
        {
            string samplesFileName;
            string tempStr;

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
                throw new System.Exception(tempStr);                // Set i Project Properties -> Debug: command line params.
                                                                    // Typical examples as below:
                                                                    //    ..\..\..\samplesFileNames.txt all 
                                                                    //    ..\..\..\samplesFileNames-15-sneeses.txt all 
                                                                    //    ..\..\..\samplesFileNames.txt  ..\..\..\..\Data\GF\Sneezes\sneeze-1-4.wav
                                                                    //    ..\..\..\samplesFileNames-all.txt all (For all samples)
            }

            if ("all" == o_NewProblemFileName.ToLower())
            {
                o_NewProblemFileName = "";
            } // if

            // Decode sampleFilenames (one per row)
            // Format of list file used as P1: one line per .wav­file:
            string paramListOfFileNamesStr = System.IO.File.ReadAllText(samplesFileName);
            char[] lineFeedSep = { '\n' };
            string[] listOfFileNamesStrArr;
            listOfFileNamesStrArr = paramListOfFileNamesStr.Split(lineFeedSep);

            string tempS1 = Path.GetFullPath(samplesFileName);
            string tempS2 = Path.GetDirectoryName(tempS1);
            tempS2 = tempS2 + "\\";

            // Decode each sampleFile
            // line = <marker for type of sound> TAB [<path>]<filename of .wav­file>
            char[] tabSep = { '\t' };
            string[] linePartsArr;
            for (int i = 0; i < listOfFileNamesStrArr.Length; ++i)
            {
                SoundFileClass soundFileObj = new SoundFileClass();

                // Skip too short lines.
                if (listOfFileNamesStrArr[i].Length < 2)
                {
                    continue;
                }

                // Skip any comment lines (starts with ";")
                if (';' == listOfFileNamesStrArr[i][0])
                {
                    continue;
                }


                linePartsArr = listOfFileNamesStrArr[i].Split(tabSep);

                if ("0" == linePartsArr[0])
                {
                    soundFileObj.SoundFileSneezeMarker = EnumSneezeMarker.smNoSneeze;
                }
                else if ("1" == linePartsArr[0])
                {
                    soundFileObj.SoundFileSneezeMarker = EnumSneezeMarker.smSneeze;
                }

                string s = tempS2 + linePartsArr[1];
                soundFileObj.SoundFileName = Path.GetFullPath(s);

                o_FileNameList.Add(soundFileObj);
            } // for i

        } // DecodeParam
    } // DecodeParamClass
}
