using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlessYou
{
    public static class ConfigurationStatClass
    {
        public static int C_NR_OF_INTERVALS = 10; // The interesting part of the sound file is split into this number of equal size intervals
        public static double C_TRIGGER_LEVEL_IN_PERCENT = 50;
        public static int C_NR_OF_RETRIEVED_CASES = 10;
        public static double C_DEFAULT_AVERAGE_FEATURE_WEIGHT = 0.2;
        public static double C_DEFAULT_PEAK_FEATURE_WEIGHT = 0.2;
    } // ConfigurationClass
}
