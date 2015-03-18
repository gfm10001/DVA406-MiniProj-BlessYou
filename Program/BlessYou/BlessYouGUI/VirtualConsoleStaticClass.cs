using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlessYouGUI
{
    public static class VirtualConsoleStaticClass
    {

        static System.Windows.Forms.RichTextBox FRtxtbConsoleWindow;

        // ====================================================================

        public static void SetUpRichTextBoxOutput(System.Windows.Forms.RichTextBox ref_RtxtbConsoleWindow)
        {
            FRtxtbConsoleWindow = ref_RtxtbConsoleWindow;
        } // SetUpRichTextBoxOutput

        // ====================================================================

        public static void Clear(string i_String)
        {
            // FRtxtbConsoleWindow.Lines.
            System.Windows.Forms.Application.DoEvents();
            FRtxtbConsoleWindow.Text = "";
            FRtxtbConsoleWindow.ScrollToCaret();
        } // Clear

        // ====================================================================
      
        public static void Write(string i_String)
        {
            // FRtxtbConsoleWindow.Lines.
            System.Windows.Forms.Application.DoEvents();
            FRtxtbConsoleWindow.Text = FRtxtbConsoleWindow.Text + i_String + Environment.NewLine;
            FRtxtbConsoleWindow.SelectionStart = FRtxtbConsoleWindow.Text.Length;
            FRtxtbConsoleWindow.ScrollToCaret();
        } // Write

        // ====================================================================

        public static void WriteLine(string i_String)
        {
            VirtualConsoleStaticClass.Write(i_String + Environment.NewLine);
        } // WriteLine

        // ====================================================================

        public static string ReadKey()
        {
            return "ToDo";
        } // ReadKey

        // ====================================================================

    } // public static class VirtualConsoleStaticClass

} // namespace BlessYouGUI
