// BlessYouMainForm.cs
//
// DVA406 Intelligent Systems, MdH, vt15
//
// Group 1:
//      Simon P.
//      Niclas S.
//      Göran FMarker.
//
// History:
// 2015-03-18/GF    Introduced.
//      

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace BlessYouGUI
{
    public partial class frmBlessYouMain : Form
    {
        frmCaseBaseLibrary FCaseBaseLibraryForm;
        public frmBlessYouMain()
        {
            InitializeComponent();
        } // frmBlessYouMain

        // ====================================================================

        private void BlessYouMainGui_Load(object sender, EventArgs e)
        {
            txtCLParams.Text = "<enter command line parameters here>";
            txtCLParams.Text = "..\\..\\..\\samplesFileNames-all.txt allx";

            btnEnter.Text = "Enter!";
            this.AcceptButton = btnEnter; 
            
            rtxtbConsoleWindow.BackColor = Color.Black;
            rtxtbConsoleWindow.ForeColor = Color.White;
            rtxtbConsoleWindow.Text = txtCLParams.Text;

            Font font = new Font("Times New Roman", 12.0f);
           // ??+Read only ??? (även .font!)  rtxtbConsoleWindow.SelectionFont = font; //  new Font("COURIER NEW", 8);

            FCaseBaseLibraryForm = new frmCaseBaseLibrary();
            FCaseBaseLibraryForm.Show(); // ToDo - position ???

            VirtualConsoleStaticClass.SetUpRichTextBoxOutput(rtxtbConsoleWindow);

        } // BlessYouMainGui_Load

        // ====================================================================

        private void txtCLParams_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            string[] paramStrArr = txtCLParams.Text.Split(' ');
            GenericMainClass.GenericMain(FCaseBaseLibraryForm, paramStrArr);

        } // btnEnter_Click

        // ====================================================================

    }
}
