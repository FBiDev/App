namespace App.Core.Desktop
{
    partial class FlatMaskedTextBoxOLD
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtMain = new System.Windows.Forms.MaskedTextBox();
            this.lblPlaceholder = new System.Windows.Forms.Label();
            this.pnlContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBg
            // 
            this.pnlContent.Controls.Add(this.lblPlaceholder);
            this.pnlContent.Controls.Add(this.txtMain);
            this.pnlContent.Controls.SetChildIndex(this.txtMain, 0);
            this.pnlContent.Controls.SetChildIndex(this.lblPlaceholder, 0);
            this.pnlContent.Controls.SetChildIndex(this.lblSubtitle, 0);
            // 
            // txtMain
            // 
            this.txtMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMain.BackColor = System.Drawing.Color.White;
            this.txtMain.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMain.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.txtMain.Location = new System.Drawing.Point(5, 15);
            this.txtMain.Margin = new System.Windows.Forms.Padding(0);
            this.txtMain.Name = "txtMain";
            this.txtMain.Size = new System.Drawing.Size(195, 16);
            this.txtMain.TabIndex = 3;
            // 
            // lblPlaceholder
            // 
            this.lblPlaceholder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lblPlaceholder.BackColor = System.Drawing.Color.Transparent;
            this.lblPlaceholder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblPlaceholder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(178)))), ((int)(((byte)(178)))));
            this.lblPlaceholder.Location = new System.Drawing.Point(5, 15);
            this.lblPlaceholder.Margin = new System.Windows.Forms.Padding(3);
            this.lblPlaceholder.Name = "lblPlaceholder";
            this.lblPlaceholder.Size = new System.Drawing.Size(195, 14);
            this.lblPlaceholder.TabIndex = 4;
            // 
            // FlatMaskedTextBox
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Name = "FlatMaskedTextBox";
            this.pnlContent.ResumeLayout(false);
            this.pnlContent.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.MaskedTextBox txtMain;
        protected System.Windows.Forms.Label lblPlaceholder;

    }
}
