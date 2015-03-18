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
            this.pnlNoneSneeze = new System.Windows.Forms.Panel();
            this.lblBannerSneeze = new System.Windows.Forms.Label();
            this.lblBannerNoneSneeze = new System.Windows.Forms.Label();
            this.pnlSneeze.SuspendLayout();
            this.pnlNoneSneeze.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSneeze
            // 
            this.pnlSneeze.Controls.Add(this.lblBannerSneeze);
            this.pnlSneeze.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSneeze.Location = new System.Drawing.Point(0, 0);
            this.pnlSneeze.Name = "pnlSneeze";
            this.pnlSneeze.Size = new System.Drawing.Size(487, 517);
            this.pnlSneeze.TabIndex = 0;
            // 
            // pnlNoneSneeze
            // 
            this.pnlNoneSneeze.Controls.Add(this.lblBannerNoneSneeze);
            this.pnlNoneSneeze.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlNoneSneeze.Location = new System.Drawing.Point(487, 0);
            this.pnlNoneSneeze.Name = "pnlNoneSneeze";
            this.pnlNoneSneeze.Size = new System.Drawing.Size(521, 517);
            this.pnlNoneSneeze.TabIndex = 1;
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
            // lblBannerNoneSneeze
            // 
            this.lblBannerNoneSneeze.AutoSize = true;
            this.lblBannerNoneSneeze.Location = new System.Drawing.Point(6, 9);
            this.lblBannerNoneSneeze.Name = "lblBannerNoneSneeze";
            this.lblBannerNoneSneeze.Size = new System.Drawing.Size(113, 13);
            this.lblBannerNoneSneeze.TabIndex = 1;
            this.lblBannerNoneSneeze.Text = "lblBannerNoneSneeze";
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

    }
}