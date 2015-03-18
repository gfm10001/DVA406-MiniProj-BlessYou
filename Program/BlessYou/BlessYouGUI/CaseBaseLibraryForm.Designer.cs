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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.LB_sneezes = new System.Windows.Forms.ListBox();
            this.lblBannerSneeze = new System.Windows.Forms.Label();
            this.LB_nonesneeze = new System.Windows.Forms.ListBox();
            this.lblBannerNoneSneeze = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.LB_sneezes);
            this.splitContainer1.Panel1.Controls.Add(this.lblBannerSneeze);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.LB_nonesneeze);
            this.splitContainer1.Panel2.Controls.Add(this.lblBannerNoneSneeze);
            this.splitContainer1.Size = new System.Drawing.Size(407, 356);
            this.splitContainer1.SplitterDistance = 203;
            this.splitContainer1.TabIndex = 0;
            // 
            // LB_sneezes
            // 
            this.LB_sneezes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LB_sneezes.FormattingEnabled = true;
            this.LB_sneezes.Location = new System.Drawing.Point(17, 24);
            this.LB_sneezes.MultiColumn = true;
            this.LB_sneezes.Name = "LB_sneezes";
            this.LB_sneezes.Size = new System.Drawing.Size(173, 317);
            this.LB_sneezes.TabIndex = 4;
            // 
            // lblBannerSneeze
            // 
            this.lblBannerSneeze.AutoSize = true;
            this.lblBannerSneeze.Location = new System.Drawing.Point(17, 4);
            this.lblBannerSneeze.Name = "lblBannerSneeze";
            this.lblBannerSneeze.Size = new System.Drawing.Size(87, 13);
            this.lblBannerSneeze.TabIndex = 3;
            this.lblBannerSneeze.Text = "lblBannerSneeze";
            // 
            // LB_nonesneeze
            // 
            this.LB_nonesneeze.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LB_nonesneeze.FormattingEnabled = true;
            this.LB_nonesneeze.Location = new System.Drawing.Point(17, 24);
            this.LB_nonesneeze.MultiColumn = true;
            this.LB_nonesneeze.Name = "LB_nonesneeze";
            this.LB_nonesneeze.Size = new System.Drawing.Size(174, 317);
            this.LB_nonesneeze.TabIndex = 4;
            // 
            // lblBannerNoneSneeze
            // 
            this.lblBannerNoneSneeze.AutoSize = true;
            this.lblBannerNoneSneeze.Location = new System.Drawing.Point(17, 4);
            this.lblBannerNoneSneeze.Name = "lblBannerNoneSneeze";
            this.lblBannerNoneSneeze.Size = new System.Drawing.Size(113, 13);
            this.lblBannerNoneSneeze.TabIndex = 3;
            this.lblBannerNoneSneeze.Text = "lblBannerNoneSneeze";
            // 
            // frmCaseBaseLibrary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 356);
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmCaseBaseLibrary";
            this.Text = "frmCaseBaseLibrary";
            this.Load += new System.EventHandler(this.frmCaseBaseLibrary_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox LB_sneezes;
        private System.Windows.Forms.Label lblBannerSneeze;
        private System.Windows.Forms.ListBox LB_nonesneeze;
        private System.Windows.Forms.Label lblBannerNoneSneeze;



    }
}