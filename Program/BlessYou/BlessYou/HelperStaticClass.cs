﻿// HelperStaticClass.cs
//
// DVA406 Intelligent Systems, Mdh, vt15
//
// History:
// 2015-03-14       Introduced.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlessYou
{
    public static class HelperStaticClass
    {
        //=====================================================================

        public static void GetRandomSelection(List<SoundFileClass> i_AllFilesList, out List<SoundFileClass> o_SelectedFilesList)
        {
            Random rnd = new Random();
            o_SelectedFilesList = new List<SoundFileClass>();

            for (int i = 0; i < ConfigurationStatClass.C_NR_OF_RANDOM_SNEEZE_FILES; ++i)
            {
                int countToNext = rnd.Next(2 * i_AllFilesList.Count, 2 * i_AllFilesList.Count + 1000); //  creates a number between low and high
                do
                {
                    for (int ix = 0; ix < i_AllFilesList.Count; ++ix)
                    {
                        if ((false == i_AllFilesList[ix].IsUsedMarker) & (EnumSneezeMarker.smSneeze == i_AllFilesList[ix].SoundFileSneezeMarker))
                        {
                            countToNext--;
                            if (0 == countToNext)
                            {
                                i_AllFilesList[ix].IsUsedMarker = true;
                                o_SelectedFilesList.Add(i_AllFilesList[ix]);
                            }
                        }
                    } // for ix
                } while (countToNext > 0);
            } // for i

            for (int i = 0; i < ConfigurationStatClass.C_NR_OF_RANDOM_NONE_SNEEZE_FILES; ++i)
            {
                int countToNext = rnd.Next(2 * i_AllFilesList.Count, 2 * i_AllFilesList.Count + 1000); // creates a number between low and high
                do
                {
                    for (int ix = 0; ix < i_AllFilesList.Count; ++ix)
                    {
                        if ((false == i_AllFilesList[ix].IsUsedMarker) & (EnumSneezeMarker.smNoSneeze == i_AllFilesList[ix].SoundFileSneezeMarker))
                        {
                            countToNext--;
                            if (0 == countToNext)
                            {
                                i_AllFilesList[ix].IsUsedMarker = true;
                                o_SelectedFilesList.Add(i_AllFilesList[ix]);
                            }
                        }
                    } // for ix
                } while (countToNext > 0);
            } // for i

        } // GetRandomSelection

        //=====================================================================

    } // HelperStaticClass

}
