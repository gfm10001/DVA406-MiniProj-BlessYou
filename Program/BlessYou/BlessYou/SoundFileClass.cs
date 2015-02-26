using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlessYou
{
    public class SoundFileClass
    {
        private string FFileName;
        private EnumSneezeMarker FMarker;

        //=====================================================================

        public string FileName
        {
            get
            {
                return FFileName;
            }
            set
            {
                FFileName = value;
            }
        } // FileName

        //=====================================================================

        public EnumSneezeMarker Marker
        {
            get
            {
                return FMarker;
            }
            set
            {
                FMarker = value;
            }
        } // Marker

        //=====================================================================

    } // SoundFileClass
}
