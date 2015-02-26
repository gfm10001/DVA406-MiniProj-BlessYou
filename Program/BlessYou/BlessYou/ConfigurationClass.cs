using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlessYou
{
    public static class ConfigurationClass
    {
        public static int NrOfIntervals = 10; // The interesting part of the sound file is split into this number of equal size intervals
        public static double triggerLevelInPercent = 50;
    } // ConfigurationClass
}
