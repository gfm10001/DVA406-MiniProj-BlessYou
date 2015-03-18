namespace BlessYouGUI
{
    partial class frmBlessYouMain
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
            this.btnEnter = new System.Windows.Forms.Button();
            this.txtCLParams = new System.Windows.Forms.TextBox();
            this.rtxtbConsoleWindow = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btnEnter
            // 
            this.btnEnter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEnter.Location = new System.Drawing.Point(872, 6);
            this.btnEnter.Name = "btnEnter";
            this.btnEnter.Size = new System.Drawing.Size(75, 23);
            this.btnEnter.TabIndex = 3;
            this.btnEnter.Text = "btnEnter";
            this.btnEnter.UseVisualStyleBackColor = true;
            this.btnEnter.Click += new System.EventHandler(this.btnEnter_Click);
            // 
            // txtCLParams
            // 
            this.txtCLParams.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCLParams.Location = new System.Drawing.Point(12, 9);
            this.txtCLParams.Name = "txtCLParams";
            this.txtCLParams.Size = new System.Drawing.Size(854, 20);
            this.txtCLParams.TabIndex = 2;
            this.txtCLParams.Text = "txtCLParams";
            this.txtCLParams.TextChanged += new System.EventHandler(this.txtCLParams_TextChanged);
            // 
            // rtxtbConsoleWindow
            // 
            this.rtxtbConsoleWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtxtbConsoleWindow.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtxtbConsoleWindow.Location = new System.Drawing.Point(12, 35);
            this.rtxtbConsoleWindow.Name = "rtxtbConsoleWindow";
            this.rtxtbConsoleWindow.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.rtxtbConsoleWindow.Size = new System.Drawing.Size(934, 495);
            this.rtxtbConsoleWindow.TabIndex = 4;
            this.rtxtbConsoleWindow.Text = "rtxtbConsoleWindow";
            this.rtxtbConsoleWindow.WordWrap = false;
            // 
            // frmBlessYouMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(954, 539);
            this.Controls.Add(this.rtxtbConsoleWindow);
            this.Controls.Add(this.btnEnter);
            this.Controls.Add(this.txtCLParams);
            this.Name = "frmBlessYouMain";
            this.Text = "frmBlessYouMain";
            this.Load += new System.EventHandler(this.BlessYouMainGui_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnEnter;
        private System.Windows.Forms.TextBox txtCLParams;
        private System.Windows.Forms.RichTextBox rtxtbConsoleWindow;
    }
}

