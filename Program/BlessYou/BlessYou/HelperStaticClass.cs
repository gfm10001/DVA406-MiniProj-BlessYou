// HelperStaticClass.cs
//
// DVA406 Intelligent Systems, Mdh, vt15
//
// History:
// 2015-03-14       Introduced.
// 2015-03-17/GF    GetRandomSelection: fix when not enough files.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlessYou
{
    public static class HelperStaticClass
    {
        //=====================================================================

        // Get a selection of typically 50 + 50 sneezes resp. none-sneezes as a start Case Library.
        // 
        public static void GetRandomSelection(List<SoundFileClass> i_AllFilesList, out List<SoundFileClass> o_SelectedFilesList)
        {
            Random rnd = new Random(0); // Use argument to keep each execution using same sequence of random numbers!
            o_SelectedFilesList = new List<SoundFileClass>();
            bool freeFileIsFound; 

            freeFileIsFound = false;
            for (int i = 0; i < Math.Min(ConfigurationStatClass.C_NR_OF_RANDOM_SNEEZE_FILES, i_AllFilesList.Count); ++i) // ToDo: should have been nr of sneezes count...
            {
                int countToNext = rnd.Next(2 * i_AllFilesList.Count, 2 * i_AllFilesList.Count + 1000); //  creates a number between low and high
                do
                {
                    freeFileIsFound = false;
                    for (int ix = 0; ix < i_AllFilesList.Count; ++ix)
                    {
                        if ((false == i_AllFilesList[ix].IsUsedMarker) & (EnumSneezeMarker.smSneeze == i_AllFilesList[ix].SoundFileSneezeMarker))
                        {
                            freeFileIsFound = true;
                            countToNext--; 
                            if (0 == countToNext)
                            {
                                i_AllFilesList[ix].IsUsedMarker = true;
                                o_SelectedFilesList.Add(i_AllFilesList[ix]);
                            }
                        } // if
                    } // for ix

                    if (false == freeFileIsFound) 
                    {
                        // No free file found in complete round -> finished!
                        break;
                    }
                } while (countToNext > 0);

                if (false == freeFileIsFound)
                {
                    // No free file found in complete round -> finished!
                    break;
                }
            } // for i


            for (int i = 0; i < Math.Min(ConfigurationStatClass.C_NR_OF_RANDOM_NONE_SNEEZE_FILES, i_AllFilesList.Count); ++i)
            {
                int countToNext = rnd.Next(2 * i_AllFilesList.Count, 2 * i_AllFilesList.Count + 1000); // creates a number between low and high
                do
                {
                    freeFileIsFound = false; 
                    for (int ix = 0; ix < i_AllFilesList.Count; ++ix)
                    {
                        if ((false == i_AllFilesList[ix].IsUsedMarker) & (EnumSneezeMarker.smNoSneeze == i_AllFilesList[ix].SoundFileSneezeMarker))
                        {
                            freeFileIsFound = true;
                            countToNext--;
                            if (0 == countToNext)
                            {
                                i_AllFilesList[ix].IsUsedMarker = true;
                                o_SelectedFilesList.Add(i_AllFilesList[ix]);

                            }
                        }
                    } // for ix

                    if (false == freeFileIsFound)
                    {
                        // No free file found in complete round -> finished!
                        break;
                    }
                } while (countToNext > 0);

                if (false == freeFileIsFound)
                {
                    // No free file found in complete round -> finished!
                    break;
                }
            } // for i

        } // GetRandomSelection
        
        //=====================================================================

        // Get a random not-used file to the select list, returns null if not any more at all!
        public static void GetUnusedRandomFile(List<SoundFileClass> i_AllFilesList, out SoundFileClass o_NewSoundFile)
        {
            Random rnd = new Random(0); // Use argument to keep each execution using same sequence of random numbers!
            o_NewSoundFile = null;
            bool atLeastOneIsLeft = false;

            // First check if anything at all remains...
            for (int ix = 0; ix < i_AllFilesList.Count; ++ix)
            {
                if (false == i_AllFilesList[ix].IsUsedMarker)
                {
                    atLeastOneIsLeft = true;
                    break;
                }
            }

            if (true == atLeastOneIsLeft)
            {
                int countToNext = rnd.Next(2 * i_AllFilesList.Count, 2 * i_AllFilesList.Count + 1000); //  creates a number between low and high
                do
                {
                    for (int ix = 0; ix < i_AllFilesList.Count; ++ix)
                    {
                        if (false == i_AllFilesList[ix].IsUsedMarker)
                        {
                            countToNext--;
                            if (0 == countToNext)
                            {
                                i_AllFilesList[ix].IsUsedMarker = true;
                                o_NewSoundFile = i_AllFilesList[ix];
                                break;
                            }
                        }
                    } // for ix
                } while (countToNext > 0);
            } // if

        } // GetUnusedRandomFile

        //=====================================================================

    } // HelperStaticClass

}
