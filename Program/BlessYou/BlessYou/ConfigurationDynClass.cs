// ConfigurationDynClass.cs
//
// DVA406 Intelligent Systems, Mdh, vt15
//
// History:
// 2015-03-16       Introduced, based on ConfigurationStatClass of 2015-03-16/SP

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace BlessYou
{
    public class ConfigurationDynClass
    {
        //The fields bellow are used when generating custom weights. 
        //>>>>>>>
        //IMPORTANT: If you add a non-double field you need to chanage the GenerateRandomConfig call
        //>>>>>>>
        public double M_AVERAGE_FEATURE_WEIGHT = ConfigurationStatClass.C_DEFAULT_AVERAGE_FEATURE_WEIGHT;
        public double M_PEAK_FEATURE_WEIGHT = ConfigurationStatClass.C_DEFAULT_PEAK_FEATURE_WEIGHT;
        public double M_RMS_FEATURE_WEIGHT = ConfigurationStatClass.C_DEFAULT_RMS_FEATURE_WEIGHT;
        public double M_PEAK2PEAK_FEATURE_WEIGHT = ConfigurationStatClass.C_DEFAULT_PEAK2PEAK_FEATURE_WEIGHT;
        public double M_CREST_FACTOR_WEIGHT = ConfigurationStatClass.C_DEFAULT_CREST_FACTOR_WEIGHT;
        public double M_PASSING_ZERO_WEIGHT = ConfigurationStatClass.C_DEFAULT_PASSING_ZERO_WEIGHT;
        public double M_LOMONT_FFT_16_FEATURE_WEIGHT = ConfigurationStatClass.C_DEFAULT_LOMONT_FFT_16_FEATURE_WEIGHT;
        public double M_LOMONT_FFT_14_FEATURE_WEIGHT = ConfigurationStatClass.C_DEFAULT_LOMONT_FFT_14_FEATURE_WEIGHT;
        public double M_LOMONT_FFT_12_FEATURE_WEIGHT = ConfigurationStatClass.C_DEFAULT_LOMONT_FFT_12_FEATURE_WEIGHT;

        // ====================================================================

        public void DumpConfiguration(string i_Banner, string i_FileName)
        {
            List<string> outval = new List<string>();
            Type t = MethodBase.GetCurrentMethod().DeclaringType;

            FieldInfo[] finfo = t.GetFields();
            foreach (FieldInfo f in finfo)
            {
                if (f.IsStatic)
                    outval.Add(f.Name + "\t" + f.GetValue(null));
                else
                    outval.Add(f.Name + "\t" + f.GetValue(this));
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
                Console.WriteLine("DumpDynConfiguration to {0} - ERR: " + ex.Message, i_FileName);
            }

        } // DumpConfiguration

        // ====================================================================

    }


}
