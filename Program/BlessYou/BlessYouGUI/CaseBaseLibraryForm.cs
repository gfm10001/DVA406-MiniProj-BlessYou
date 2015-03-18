// CaseBaseLibraryForm.cs
//
// DVA406 Intelligent Systems, MdH, vt15
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
using BlessYou;

namespace BlessYouGUI
{
    public partial class frmCaseBaseLibrary : Form
    {
        public frmCaseBaseLibrary()
        {
            InitializeComponent();
        }

        List<List<CaseClass>> CaseHistory = new List<List<CaseClass>>();
        List<CaseClass> removed;
        List<CaseClass> added;

        private void frmCaseBaseLibrary_Load(object sender, EventArgs e)
        {
            LB_sneezes.Sorted = true;
            LB_nonesneeze.Sorted = true;
            LB_nonesneeze.DrawMode = DrawMode.OwnerDrawFixed;
            LB_sneezes.DrawMode = DrawMode.OwnerDrawFixed;

            LB_sneezes.DrawItem += listBox_DrawItem;
            LB_nonesneeze.DrawItem += listBox_DrawItem;
        }

        public void Update_Lists(List<CaseClass> list)
        {
            return;
            CaseHistory.Add(list);
            LB_sneezes.Items.Clear();
            LB_nonesneeze.Items.Clear();
            added.Clear();
            removed.Clear();

            HashSet<CaseClass> rmset = new HashSet<CaseClass>(CaseHistory[CaseHistory.Count - 1]);

            foreach (CaseClass c in list)
            {
                if (c.SneezeStatus == EnumCaseStatus.csIsConfirmedSneeze)
                    LB_sneezes.Items.Add(c);
                else if (c.SneezeStatus == EnumCaseStatus.csIsConfirmedNoneSneeze)
                    LB_nonesneeze.Items.Add(c);

                if (CaseHistory[CaseHistory.Count - 1].Contains(c) == false)
                    added.Add(c);

                rmset.Remove(c);
            }
            removed.AddRange(rmset);


        }

        private void listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            Graphics g = e.Graphics;

            // draw the background color you want
            // mine is set to olive, change it to whatever you want
            if (added.Contains(sender))
                g.FillRectangle(new SolidBrush(Color.Green), e.Bounds);
            else if (removed.Contains(sender))
                g.FillRectangle(new SolidBrush(Color.Red), e.Bounds);
            else
                g.FillRectangle(new SolidBrush(Color.White), e.Bounds);

            // draw the text of the list item, not doing this will only show
            // the background color
            // you will need to get the text of item to display
            g.DrawString(sender.ToString(), e.Font, new SolidBrush(e.ForeColor), new PointF(e.Bounds.X, e.Bounds.Y));

            e.DrawFocusRectangle();
        }

        private void LB_sneezes_DrawItem(object sender, DrawItemEventArgs e)
        {

        }

    }
}
