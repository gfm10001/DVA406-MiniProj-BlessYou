// SoundFileClass.cs
//
// DVA406 Intelligent Systems, Mdh, vt15
//
// History:
// 2015-02-24       Introduced.
// 2015-03-04/GF    Added constructors

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlessYou
{
    public class SoundFileClass
    {
        private string FSoundFileName;
        private EnumSneezeMarker FSoundFileSneezeMarker;

        //=====================================================================

        public SoundFileClass()
        {
            FSoundFileName = "";
            FSoundFileSneezeMarker = EnumSneezeMarker.smNone;
        } // SoundFileClass

        //=====================================================================

        public SoundFileClass(string i_FileName, EnumSneezeMarker i_FileSneezeMarker)
        {
            FSoundFileName = i_FileName;
            FSoundFileSneezeMarker = i_FileSneezeMarker;
        } // SoundFileClass

        //=====================================================================

        public string SoundFileName
        {
            get
            {
                return FSoundFileName;
            }
            set
            {
                FSoundFileName = value;
            }
        } // SoundFileName

        //=====================================================================

        public EnumSneezeMarker SoundFileSneezeMarker
        {
            get
            {
                return FSoundFileSneezeMarker;
            }
            set
            {
                FSoundFileSneezeMarker = value;
            }
        } // SoundFileSneezeMarker

        //=====================================================================

    } // SoundFileClass
}
