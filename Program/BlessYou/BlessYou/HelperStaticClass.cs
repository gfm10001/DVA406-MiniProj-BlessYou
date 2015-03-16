// HelperStaticClass.cs
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

        public static void GetRandomSelection(List<SoundFileClass> i_AllFilesList, List<SoundFileClass> o_SelectedFilesList)
        {
            int x = ConfigurationStatClass.C_NR_OF_RANDOM_NONE_SNEEZE_FILES;
            o_SelectedFilesList.Clear();
            string markerStr = new string(' ', i_AllFilesList.Count);

            for (int i = 1; i < ConfigurationStatClass.C_NR_OF_RANDOM_SNEEZE_FILES; ++i)
            {
            } // for i


        } // GetRandomSelection

        //=====================================================================

    } // HelperStaticClass

}
