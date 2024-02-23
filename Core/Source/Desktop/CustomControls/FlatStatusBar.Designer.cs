namespace App.Core.Desktop
{
    partial class FlatStatusBar
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
            this.pnlStatus1 = new System.Windows.Forms.Panel();
            this.lblStatus1 = new System.Windows.Forms.Label();
            this.pnlStatus2 = new System.Windows.Forms.Panel();
            this.lblStatus2 = new System.Windows.Forms.Label();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlBorder = new System.Windows.Forms.Panel();
            this.pnlStatus1.SuspendLayout();
            this.pnlStatus2.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlStatus1
            // 
            this.pnlStatus1.BackColor = System.Drawing.Color.Transparent;
            this.pnlStatus1.Controls.Add(this.lblStatus1);
            this.pnlStatus1.Location = new System.Drawing.Point(0, 4);
            this.pnlStatus1.Name = "pnlStatus1";
            this.pnlStatus1.Padding = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.pnlStatus1.Size = new System.Drawing.Size(116, 15);
            this.pnlStatus1.TabIndex = 1;
            // 
            // lblStatus1
            // 
            this.lblStatus1.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus1.ForeColor = System.Drawing.Color.DimGray;
            this.lblStatus1.Location = new System.Drawing.Point(0, 0);
            this.lblStatus1.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.lblStatus1.Name = "lblStatus1";
            this.lblStatus1.Size = new System.Drawing.Size(115, 15);
            this.lblStatus1.TabIndex = 3;
            this.lblStatus1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlStatus2
            // 
            this.pnlStatus2.BackColor = System.Drawing.Color.Transparent;
            this.pnlStatus2.Controls.Add(this.lblStatus2);
            this.pnlStatus2.Location = new System.Drawing.Point(122, 4);
            this.pnlStatus2.Name = "pnlStatus2";
            this.pnlStatus2.Size = new System.Drawing.Size(116, 15);
            this.pnlStatus2.TabIndex = 5;
            // 
            // lblStatus2
            // 
            this.lblStatus2.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus2.ForeColor = System.Drawing.Color.DimGray;
            this.lblStatus2.Location = new System.Drawing.Point(0, 0);
            this.lblStatus2.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.lblStatus2.Name = "lblStatus2";
            this.lblStatus2.Size = new System.Drawing.Size(115, 15);
            this.lblStatus2.TabIndex = 3;
            this.lblStatus2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlContent
            // 
            this.pnlContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.pnlContent.Controls.Add(this.pnlStatus1);
            this.pnlContent.Controls.Add(this.pnlStatus2);
            this.pnlContent.Location = new System.Drawing.Point(0, 1);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(238, 22);
            this.pnlContent.TabIndex = 6;
            // 
            // pnlBorder
            // 
            this.pnlBorder.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlBorder.BackColor = System.Drawing.Color.Transparent;
            this.pnlBorder.Location = new System.Drawing.Point(0, 0);
            this.pnlBorder.Name = "pnlBorder";
            this.pnlBorder.Size = new System.Drawing.Size(238, 1);
            this.pnlBorder.TabIndex = 4;
            // 
            // FlatStatusBar
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(145)))), ((int)(((byte)(145)))), ((int)(((byte)(145)))));
            this.Controls.Add(this.pnlBorder);
            this.Controls.Add(this.pnlContent);
            this.Name = "FlatStatusBar";
            this.Size = new System.Drawing.Size(238, 23);
            this.pnlStatus1.ResumeLayout(false);
            this.pnlStatus2.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlStatus1;
        protected System.Windows.Forms.Label lblStatus1;
        private System.Windows.Forms.Panel pnlStatus2;
        protected System.Windows.Forms.Label lblStatus2;
        private System.Windows.Forms.Panel pnlContent;
        protected System.Windows.Forms.Panel pnlBorder;
    }
}
