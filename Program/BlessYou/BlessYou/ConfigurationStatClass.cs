// ConfigurationStatClass.cs
//
// DVA406 Intelligent Systems, Mdh, vt15
//
// History:
// 2015-02-24       Introduced.
// 2015-03-16/SP    DumpConfiguration: Added use of relection
// 2015-03-16/GF    Extracted dynamic parts moved to ConfigurationDynClass
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace BlessYou
{
    public static class ConfigurationStatClass
    {
        public const double C_MAX_POSSIBLE_VALUE = 100000;             // was 0x7FFF; // The maximum absolute value in a sound file recoded at 16 bit 

        public const int C_NR_OF_INTERVALS = 10;                       // The interesting part of the sound file is split into this number of equal size intervals
        public const double C_TRIGGER_LEVEL_IN_PERCENT = 50;
        public const double C_TRIGGER_PREFETCH_IN_MILLI_SECS = 100;    // Trigger is moved backwards this amount to get a prefetch 
        public const double C_TRIGGER_OFF_LEVEL_IN_PERCENT = 10;
        public const double C_TRIGGER_OFF_DURATION_IN_MILLI_SECS = 1000;
        public const double C_SOUND_SAMPLE_FREQUENCY_IN_kHz = 44.1;

        public const int C_NR_OF_RETRIEVED_CASES = 1;
        public const double C_DEFAULT_AVERAGE_FEATURE_WEIGHT = 0.2;
        public const double C_DEFAULT_PEAK_FEATURE_WEIGHT = 0.2;
        public const double C_DEFAULT_RMS_FEATURE_WEIGHT = 0.2;
        public const double C_DEFAULT_PEAK2PEAK_FEATURE_WEIGHT = 0.2;
        public const double C_DEFAULT_CREST_FACTOR_WEIGHT = 0.2;
        public const double C_DEFAULT_PASSING_ZERO_WEIGHT = 0.2;
        public const double C_DEFAULT_LOMONT_FFT_16_FEATURE_WEIGHT = 0.2;
        public const double C_DEFAULT_LOMONT_FFT_14_FEATURE_WEIGHT = 0.2;
        public const double C_DEFAULT_LOMONT_FFT_12_FEATURE_WEIGHT = 0.2;


        //ToDo implement interval weights
        public const double C_FEATURE_INTERVAL_0_WEIGHT = 1;
        public const double C_FEATURE_INTERVAL_1_WEIGHT = 1;
        public const double C_FEATURE_INTERVAL_2_WEIGHT = 1;
        public const double C_FEATURE_INTERVAL_3_WEIGHT = 1;
        public const double C_FEATURE_INTERVAL_4_WEIGHT = 1;
        public const double C_FEATURE_INTERVAL_5_WEIGHT = 1;
        public const double C_FEATURE_INTERVAL_6_WEIGHT = 1;
        public const double C_FEATURE_INTERVAL_7_WEIGHT = 1;
        public const double C_FEATURE_INTERVAL_8_WEIGHT = 1;
        public const double C_FEATURE_INTERVAL_9_WEIGHT = 1;


        public const int C_NR_OF_SAMPLES_2_POWER_12 = 12;
        public const int C_NR_OF_SAMPLES_2_POWER_14 = 14;
        public const int C_NR_OF_SAMPLES_2_POWER_16 = 16;

        public static bool USE_PARALLEL_EXECUTION = true;
        public const int C_STARTING_FFT_ANALYSIS_FREQUENCY_IN_HERTZ = 1000;
        public const int C_ENDING_FFT_ANALYSIS_FREQUENCY_IN_HERTZ = 5000;

        public static bool USE_EUCLID_SUMMATION = true;

        public const double C_EPSILON = 0.000001;

        public const int C_NUMBER_OF_CASES_TO_USE_FOR_MAJORITY_VOTE = 5;  // 1, 3, 5 or 7 according to teachers
        public static bool RUN_ALL_MAJORITY_VOTE_CASE_NUMBERS = false;   // if true run 1, 3, 5  and 7 if C_NUMBER_OF_CASES_TO_USE_FOR_MAJORITY_VOTE = 7

        public const string C_CONFIGURATION_REPORT_FILE_NAME = "ConfigurationReport.txt";
        public const string C_CLASS_LIBRARY_REPORT_FILE_NAME = "ClassLibraryReport.txt";
        // ToDo add missing consts

        // ====================================================================

        public static void DumpConfiguration(string i_Banner, string i_FileName)
        {

            List<string> outval = new List<string>();
            Type t = MethodBase.GetCurrentMethod().DeclaringType;

            FieldInfo[] finfo = t.GetFields();
            foreach (FieldInfo f in finfo)
            {
                if(f.IsStatic)
                    outval.Add(f.Name + "\t" + f.GetValue(null));
                //else
                //    outval.Add(f.Name + "\t" + f.GetValue(this));
            } // foreach
            
            
            // Fix tabulation...
            int maxVariableNamelength = 0;
            foreach (string s in outval)
            {
                int p = s.IndexOf("\t");
                if (p > maxVariableNamelength)
                {
                    maxVariableNamelength = p;
                }
            } // foreach


            // Create report...
            string totText = i_Banner + " at " + DateTime.Now.ToString() + Environment.NewLine;
            for (int ix = 0; ix < outval.Count; ++ix)
            {
                string[] parts = outval[ix].Split('\t');
                string tabStr = new string(' ', maxVariableNamelength - parts[0].Length + 1);
                totText = totText + parts[0] + tabStr + " = " + parts[1] + Environment.NewLine;
            } // foreach


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
