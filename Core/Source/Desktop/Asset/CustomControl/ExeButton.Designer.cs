namespace App.Core.Desktop
{
    partial class ExeButton
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
            this.pnlBack = new System.Windows.Forms.Panel();
            this.InnerButton = new ExeInnerButton();
            this.lblExe = new System.Windows.Forms.Label();
            this.pnlBack.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBack
            // 
            this.pnlBack.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlBack.BackColor = System.Drawing.Color.Transparent;
            this.pnlBack.Controls.Add(this.InnerButton);
            this.pnlBack.Controls.Add(this.lblExe);
            this.pnlBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBack.Location = new System.Drawing.Point(1, 1);
            this.pnlBack.Margin = new System.Windows.Forms.Padding(0);
            this.pnlBack.Name = "pnlBack";
            this.pnlBack.Padding = new System.Windows.Forms.Padding(2, 4, 2, 2);
            this.pnlBack.Size = new System.Drawing.Size(74, 67);
            this.pnlBack.TabIndex = 0;
            // 
            // InnerButton
            // 
            this.InnerButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.InnerButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.InnerButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InnerButton.Location = new System.Drawing.Point(11, 3);
            this.InnerButton.Margin = new System.Windows.Forms.Padding(0);
            this.InnerButton.Name = "InnerButton";
            this.InnerButton.Size = new System.Drawing.Size(48, 48);
            this.InnerButton.TabIndex = 0;
            this.InnerButton.UseVisualStyleBackColor = true;
            // 
            // lblExe
            // 
            this.lblExe.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblExe.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExe.ForeColor = System.Drawing.Color.White;
            this.lblExe.Location = new System.Drawing.Point(2, 51);
            this.lblExe.Name = "lblExe";
            this.lblExe.Size = new System.Drawing.Size(70, 12);
            this.lblExe.TabIndex = 1;
            this.lblExe.Text = "Exe";
            this.lblExe.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ButtonExe
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.pnlBack);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ExeButton";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.Size = new System.Drawing.Size(76, 69);
            this.pnlBack.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBack;
        private ExeInnerButton InnerButton;
        private System.Windows.Forms.Label lblExe;
    }
}
