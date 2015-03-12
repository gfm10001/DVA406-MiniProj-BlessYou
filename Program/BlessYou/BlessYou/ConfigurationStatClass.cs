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

        public static int C_NR_OF_INTERVALS = 10;                       // The interesting part of the sound file is split into this number of equal size intervals
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

        public static int C_NR_OF_SAMPLES_2_POWER_12 = 12;
        public static int C_NR_OF_SAMPLES_2_POWER_14 = 14;
        public static int C_NR_OF_SAMPLES_2_POWER_16 = 16;

        public static int C_STARTING_FFT_ANALYSIS_FREQUENCY_IN_HERTZ = 1000;
        public static int C_ENDING_FFT_ANALYSIS_FREQUENCY_IN_HERTZ = 5000;

        public static double C_EPSILON = 0.000001;

        public static string C_CONFIGURATION_REPORT_FILE_NAME = "ConfigurationReport.txt";
        public static string C_CLASS_LIBRARY_REPORT_FILE_NAME = "ClassLibraryReport.txt";
        // ToDo add missing consts

        // ====================================================================

        public static void DumpConfiguration(string i_Banner, string i_FileName)
        {
            // ToDo: Kan reflection användas här?
            string totText = "";

            totText = totText + i_Banner + " at " + DateTime.Now.ToString() + Environment.NewLine;
            totText = totText + "C_MAX_POSSIBLE_VALUE                   = " + C_MAX_POSSIBLE_VALUE + Environment.NewLine;
            totText = totText + "C_NR_OF_INTERVALS                      = " + C_NR_OF_INTERVALS + Environment.NewLine;

            totText = totText + "C_TRIGGER_LEVEL_IN_PERCENT             = " + C_TRIGGER_LEVEL_IN_PERCENT + Environment.NewLine;
            totText = totText + "C_TRIGGER_PREFETCH_IN_MILLI_SECS       = " + C_TRIGGER_PREFETCH_IN_MILLI_SECS + Environment.NewLine;

            totText = totText + "C_TRIGGER_OFF_LEVEL_IN_PERCENT         = " + C_TRIGGER_OFF_LEVEL_IN_PERCENT + Environment.NewLine;
            totText = totText + "C_TRIGGER_OFF_DURATION_IN_MILLI_SECS   = " + C_TRIGGER_OFF_DURATION_IN_MILLI_SECS + Environment.NewLine;

            try
            {
                System.IO.File.WriteAllText(i_FileName, totText);
            }
            catch (Exception ex)
            {
                Console.WriteLine("DumpConfiguration to {0} - ERR: " + ex.Message, i_FileName);
            }

        } // DumpConfiguration

        // ====================================================================

    } // ConfigurationClass
}
