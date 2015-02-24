using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Detector
{
    public partial class Form1 : Form
    {

        CBRSystem cbr = new CBRSystem();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FileDialog fd = new OpenFileDialog();
            fd.ShowDialog();
            string f = fd.FileName;
            if (File.Exists(f) == false)
            {
                label1.Text = "INVALID FILE PATH";
                return;            
            }
            label1.Text = f;
            bool results = cbr.Evaluate(f);
            label1.Text = "Results: " + results.ToString();
        }
    }
}
