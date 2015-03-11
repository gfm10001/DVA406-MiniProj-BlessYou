// ConfigurationStatClass.cs
//
// DVA406 Intelligent Systems, Mdh, vt15
//
// History:
// 2015-02-24       Introduced.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlessYou
{
    public class ConfigurationStatClass
    {
        public static double C_MAX_POSSIBLE_VALUE = 100000;             // was 0x7FFF; // The maximum absolute value in a sound file recoded at 16 bit 

        public static int C_NR_OF_INTERVALS = 30;                       // The interesting part of the sound file is split into this number of equal size intervals
        public static double C_TRIGGER_LEVEL_IN_PERCENT = 50;
        public static double C_TRIGGER_PREFETCH_IN_MILLI_SECS = 100;    // Trigger is moved backwards this amount to get a prefetch 
        public static double C_TRIGGER_OFF_LEVEL_IN_PERCENT = 10;
        public static double C_TRIGGER_OFF_DURATION_IN_MILLI_SECS = 1000;
        public static double C_SOUND_SAMPLE_FREQUENCY_IN_kHz = 44.1;
        
        public static int C_NR_OF_RETRIEVED_CASES = 10;
        public static double C_DEFAULT_AVERAGE_FEATURE_WEIGHT = 0.2;
        public static double C_DEFAULT_PEAK_FEATURE_WEIGHT = 0.2;
        public static double C_DEFAULT_RMS_FEATURE_WEIGHT = 0.2;
        public static double C_DEFAULT_PEAK2PEAK_FEATURE_WEIGHT = 0.2;
        public static double C_DEFAULT_CREST_FACTOR_WEIGHT = 0.2;
        public static double C_DEFAULT_PASSING_ZERO_WEIGHT = 0.2;
        public static double C_DEFAULT_LOMONT_FFT_FEATURE_WEIGHT = 0.2;
        public static double C_DEFAULT_NAUDIO_FFT_FEATURE_WEIGHT = 0.2;


        //The fields bellow are used when generating custom weights. 
        //>>>>>>>
        //IMPORTANT: If you add a non-double field you need to chanage the GenerateRandomConfig call
        //>>>>>>>
        public double C_M_AVERAGE_FEATURE_WEIGHT = C_DEFAULT_AVERAGE_FEATURE_WEIGHT;
        public double C_M_PEAK_FEATURE_WEIGHT = C_DEFAULT_PEAK_FEATURE_WEIGHT;
        public double C_M_RMS_FEATURE_WEIGHT = C_DEFAULT_RMS_FEATURE_WEIGHT;
        public double C_M_PEAK2PEAK_FEATURE_WEIGHT = C_DEFAULT_PEAK2PEAK_FEATURE_WEIGHT;
        public double C_M_CREST_FACTOR_WEIGHT = C_DEFAULT_CREST_FACTOR_WEIGHT;
        public double C_M_PASSING_ZERO_WEIGHT = C_DEFAULT_PASSING_ZERO_WEIGHT;
        public double C_M_LOMONT_FFT_FEATURE_WEIGHT = C_DEFAULT_LOMONT_FFT_FEATURE_WEIGHT;
        public double C_M_NAUDIO_FFT_FEATURE_WEIGHT = C_DEFAULT_NAUDIO_FFT_FEATURE_WEIGHT;

        public static double C_EPSILON = 0.000001;
		// ToDo add missing consts
    } // ConfigurationClass
}
