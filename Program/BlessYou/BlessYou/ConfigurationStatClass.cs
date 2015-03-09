using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlessYou
{
    public static class ConfigurationStatClass
    {
        public static double C_MAX_POSSIBLE_VALUE = 100000;             // was 0x7FFF; // The maximum absolute value in a sound file recoded at 16 bit 

        public static int C_NR_OF_INTERVALS = 10;                       // The interesting part of the sound file is split into this number of equal size intervals
        public static double C_TRIGGER_LEVEL_IN_PERCENT = 50;
        public static double C_TRIGGER_PREFETCH_IN_MILLI_SECS = 500;
        public static double C_TRIGGER_OFF_LEVEL_IN_PERCENT = 10;
        public static double C_TRIGGER_OFF_DURATION_IN_MILLI_SECS = 1000;
        public static double C_SOUND_SAMPLE_FREQUENCY_IN_kHz = 44.1;
        
        public static int C_NR_OF_RETRIEVED_CASES = 10;
        public static double C_DEFAULT_AVERAGE_FEATURE_WEIGHT = 0.2;
        public static double C_DEFAULT_PEAK_FEATURE_WEIGHT = 0.2;
        public static double C_DEFAULT_RMS_FEATURE_WEIGHT = 0.2;
        public static double C_DEFAULT_PEAK2PEAK_FEATURE_WEIGHT = 0.2;

        public static double C_EPSILON = 0.000001;
		// ToDo add missing consts
    } // ConfigurationClass
}
