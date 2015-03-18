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
using System.Runtime.InteropServices;

namespace BlessYouGUI
{
    public partial class frmCaseBaseLibrary : Form
    {
        internal static class NativeWinAPI
        {
            internal static readonly int GWL_EXSTYLE = -20;
            internal static readonly int WS_EX_COMPOSITED = 0x02000000;

            [DllImport("user32")]
            internal static extern int GetWindowLong(IntPtr hWnd, int nIndex);

            [DllImport("user32")]
            internal static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        }


        public frmCaseBaseLibrary()
        {
            InitializeComponent();
            //InitializeComponent();

            int style = NativeWinAPI.GetWindowLong(this.Handle, NativeWinAPI.GWL_EXSTYLE);
            style |= NativeWinAPI.WS_EX_COMPOSITED;
            NativeWinAPI.SetWindowLong(this.Handle, NativeWinAPI.GWL_EXSTYLE, style);

        }

        List<List<CaseClass>> CaseHistory = new List<List<CaseClass>>();
        List<CaseClass> removed = new List<CaseClass>();
        List<CaseClass> added = new List<CaseClass>();

        private void frmCaseBaseLibrary_Load(object sender, EventArgs e)
        {
            LB_sneezes.Sorted = true;
            LB_nonesneeze.Sorted = true;
            LB_nonesneeze.DrawMode = DrawMode.OwnerDrawFixed;
            LB_sneezes.DrawMode = DrawMode.OwnerDrawFixed;

            LB_sneezes.DrawItem += LB_SneezeDrawItem;
            LB_nonesneeze.DrawItem += LB_NoneSneezeDrawItem;

            CaseHistory.Add(new List<CaseClass>());
            DoubleBuffered = true;
            //added.Add(new List<CaseClass>());
            //removed.Add(new List<CaseClass>());

        }

        public void Update_Lists(List<CaseClass> list)
        {
            
            
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

            List<CaseClass> nlist = new List<CaseClass>(list);
            foreach (CaseClass c in rmset)
            {
                if (c.SneezeStatus == EnumCaseStatus.csIsConfirmedNoneSneeze)
                    LB_nonesneeze.Items.Add(c);
                if (c.SneezeStatus == EnumCaseStatus.csIsConfirmedSneeze)
                    LB_sneezes.Items.Add(c);
            
            }
           
            CaseHistory.Add(nlist);
            removed.AddRange(rmset);

            lblBannerNoneSneeze.Text = LB_nonesneeze.Items.Count.ToString();
            lblBannerSneeze.Text = LB_sneezes.Items.Count.ToString();


            LB_sneezes.ColumnWidth = LB_sneezes.Width / 2;
            LB_nonesneeze.ColumnWidth = LB_nonesneeze.Width / 2;

        }

        private void LB_SneezeDrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;
            e.DrawBackground();
            Graphics g = e.Graphics;
            object drawobj = LB_sneezes.Items[e.Index];

            // draw the background color you want
            // mine is set to olive, change it to whatever you want
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(Brushes.CornflowerBlue, e.Bounds);
            }
            else if (added.Contains(drawobj))
                g.FillRectangle(new SolidBrush(Color.Green), e.Bounds);
            else if (removed.Contains(drawobj))
                g.FillRectangle(new SolidBrush(Color.Red), e.Bounds);
            else
                g.FillRectangle(new SolidBrush(Color.White), e.Bounds);

            // draw the text of the list item, not doing this will only show
            // the background color
            // you will need to get the text of item to display
            g.DrawString(LB_sneezes.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), new PointF(e.Bounds.X, e.Bounds.Y));

            e.DrawFocusRectangle();
        }

        private void LB_NoneSneezeDrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;
            e.DrawBackground();
            Graphics g = e.Graphics;

            object drawobj = LB_nonesneeze.Items[e.Index];

            // draw the background color you want
            // mine is set to olive, change it to whatever you want
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(Brushes.CornflowerBlue, e.Bounds);
            }
            else if (added.Contains(drawobj))
                g.FillRectangle(new SolidBrush(Color.Green), e.Bounds);
            else if (removed.Contains(drawobj))
                g.FillRectangle(new SolidBrush(Color.Red), e.Bounds);
            else
                g.FillRectangle(new SolidBrush(Color.White), e.Bounds);

            // draw the text of the list item, not doing this will only show
            // the background color
            // you will need to get the text of item to display
            g.DrawString(LB_nonesneeze.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), new PointF(e.Bounds.X, e.Bounds.Y));

            e.DrawFocusRectangle();
        }

        private void splitter1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

    }
}
