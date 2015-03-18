namespace BlessYouGUI
{
    partial class frmCaseBaseLibrary
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlSneeze = new System.Windows.Forms.Panel();
            this.lblBannerSneeze = new System.Windows.Forms.Label();
            this.pnlNoneSneeze = new System.Windows.Forms.Panel();
            this.lblBannerNoneSneeze = new System.Windows.Forms.Label();
            this.LB_sneezes = new System.Windows.Forms.ListBox();
            this.LB_nonesneeze = new System.Windows.Forms.ListBox();
            this.pnlSneeze.SuspendLayout();
            this.pnlNoneSneeze.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSneeze
            // 
            this.pnlSneeze.Controls.Add(this.LB_sneezes);
            this.pnlSneeze.Controls.Add(this.lblBannerSneeze);
            this.pnlSneeze.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSneeze.Location = new System.Drawing.Point(0, 0);
            this.pnlSneeze.Name = "pnlSneeze";
            this.pnlSneeze.Size = new System.Drawing.Size(487, 517);
            this.pnlSneeze.TabIndex = 0;
            // 
            // lblBannerSneeze
            // 
            this.lblBannerSneeze.AutoSize = true;
            this.lblBannerSneeze.Location = new System.Drawing.Point(12, 9);
            this.lblBannerSneeze.Name = "lblBannerSneeze";
            this.lblBannerSneeze.Size = new System.Drawing.Size(87, 13);
            this.lblBannerSneeze.TabIndex = 0;
            this.lblBannerSneeze.Text = "lblBannerSneeze";
            // 
            // pnlNoneSneeze
            // 
            this.pnlNoneSneeze.Controls.Add(this.LB_nonesneeze);
            this.pnlNoneSneeze.Controls.Add(this.lblBannerNoneSneeze);
            this.pnlNoneSneeze.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlNoneSneeze.Location = new System.Drawing.Point(487, 0);
            this.pnlNoneSneeze.Name = "pnlNoneSneeze";
            this.pnlNoneSneeze.Size = new System.Drawing.Size(521, 517);
            this.pnlNoneSneeze.TabIndex = 1;
            // 
            // lblBannerNoneSneeze
            // 
            this.lblBannerNoneSneeze.AutoSize = true;
            this.lblBannerNoneSneeze.Location = new System.Drawing.Point(6, 9);
            this.lblBannerNoneSneeze.Name = "lblBannerNoneSneeze";
            this.lblBannerNoneSneeze.Size = new System.Drawing.Size(113, 13);
            this.lblBannerNoneSneeze.TabIndex = 1;
            this.lblBannerNoneSneeze.Text = "lblBannerNoneSneeze";
            // 
            // LB_sneezes
            // 
            this.LB_sneezes.FormattingEnabled = true;
            this.LB_sneezes.Location = new System.Drawing.Point(183, 61);
            this.LB_sneezes.Name = "LB_sneezes";
            this.LB_sneezes.Size = new System.Drawing.Size(120, 394);
            this.LB_sneezes.TabIndex = 2;
            this.LB_sneezes.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.LB_sneezes_DrawItem);
            // 
            // LB_nonesneeze
            // 
            this.LB_nonesneeze.FormattingEnabled = true;
            this.LB_nonesneeze.Location = new System.Drawing.Point(200, 61);
            this.LB_nonesneeze.Name = "LB_nonesneeze";
            this.LB_nonesneeze.Size = new System.Drawing.Size(120, 394);
            this.LB_nonesneeze.TabIndex = 2;
            this.LB_nonesneeze.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.LB_sneezes_DrawItem);
            // 
            // frmCaseBaseLibrary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 517);
            this.Controls.Add(this.pnlNoneSneeze);
            this.Controls.Add(this.pnlSneeze);
            this.Name = "frmCaseBaseLibrary";
            this.Text = "frmCaseBaseLibrary";
            this.Load += new System.EventHandler(this.frmCaseBaseLibrary_Load);
            this.pnlSneeze.ResumeLayout(false);
            this.pnlSneeze.PerformLayout();
            this.pnlNoneSneeze.ResumeLayout(false);
            this.pnlNoneSneeze.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlSneeze;
        private System.Windows.Forms.Panel pnlNoneSneeze;
        private System.Windows.Forms.Label lblBannerSneeze;
        private System.Windows.Forms.Label lblBannerNoneSneeze;
        private System.Windows.Forms.ListBox LB_sneezes;
        private System.Windows.Forms.ListBox LB_nonesneeze;

    }
}