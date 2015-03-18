namespace BlessYou
{
    partial class Form1
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
            this.LB_nonesneeze = new System.Windows.Forms.ListBox();
            this.LB_sneezes = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // LB_nonesneeze
            // 
            this.LB_nonesneeze.FormattingEnabled = true;
            this.LB_nonesneeze.Location = new System.Drawing.Point(475, 12);
            this.LB_nonesneeze.Name = "LB_nonesneeze";
            this.LB_nonesneeze.Size = new System.Drawing.Size(120, 901);
            this.LB_nonesneeze.TabIndex = 0;
            // 
            // LB_sneezes
            // 
            this.LB_sneezes.FormattingEnabled = true;
            this.LB_sneezes.Location = new System.Drawing.Point(12, 12);
            this.LB_sneezes.Name = "LB_sneezes";
            this.LB_sneezes.Size = new System.Drawing.Size(120, 901);
            this.LB_sneezes.TabIndex = 1;
            this.LB_sneezes.SelectedIndexChanged += new System.EventHandler(this.LB_sneezes_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(607, 930);
            this.Controls.Add(this.LB_sneezes);
            this.Controls.Add(this.LB_nonesneeze);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox LB_nonesneeze;
        private System.Windows.Forms.ListBox LB_sneezes;
    }
}