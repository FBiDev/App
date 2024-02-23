namespace App.Core.Desktop
{
    partial class FlatCheckBox
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
            this.pnlBgWhite = new System.Windows.Forms.Panel();
            this.lblLegend = new System.Windows.Forms.Label();
            this.chkBox = new System.Windows.Forms.CheckBox();
            this.pnlBgWhite.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBgWhite
            // 
            this.pnlBgWhite.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlBgWhite.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlBgWhite.BackColor = System.Drawing.Color.White;
            this.pnlBgWhite.Controls.Add(this.lblLegend);
            this.pnlBgWhite.Controls.Add(this.chkBox);
            this.pnlBgWhite.Location = new System.Drawing.Point(1, 1);
            this.pnlBgWhite.Margin = new System.Windows.Forms.Padding(0);
            this.pnlBgWhite.Name = "pnlBgWhite";
            this.pnlBgWhite.Size = new System.Drawing.Size(98, 32);
            this.pnlBgWhite.TabIndex = 0;
            this.pnlBgWhite.Click += new System.EventHandler(this.PnlBgWhite_Click);
            this.pnlBgWhite.DoubleClick += new System.EventHandler(this.PnlBgWhite_DoubleClick);
            this.pnlBgWhite.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PnlBgWhite_MouseDown);
            this.pnlBgWhite.MouseEnter += new System.EventHandler(this.PnlBgWhite_MouseEnter);
            this.pnlBgWhite.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PnlBgWhite_MouseUp);
            // 
            // lblLegend
            // 
            this.lblLegend.AutoSize = true;
            this.lblLegend.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLegend.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(132)))), ((int)(((byte)(199)))));
            this.lblLegend.Location = new System.Drawing.Point(1, 2);
            this.lblLegend.Margin = new System.Windows.Forms.Padding(0);
            this.lblLegend.Name = "lblLegend";
            this.lblLegend.Size = new System.Drawing.Size(45, 13);
            this.lblLegend.TabIndex = 2;
            this.lblLegend.Text = "Legend";
            this.lblLegend.Click += new System.EventHandler(this.LblLegend_Click);
            this.lblLegend.DoubleClick += new System.EventHandler(this.LblLegend_DoubleClick);
            this.lblLegend.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LblLegend_MouseDown);
            this.lblLegend.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LblLegend_MouseUp);
            // 
            // chkBox
            // 
            this.chkBox.BackColor = System.Drawing.Color.LightCoral;
            this.chkBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(132)))), ((int)(((byte)(199)))));
            this.chkBox.Location = new System.Drawing.Point(4, 18);
            this.chkBox.Name = "chkBox";
            this.chkBox.Size = new System.Drawing.Size(11, 11);
            this.chkBox.TabIndex = 1;
            this.chkBox.UseVisualStyleBackColor = false;
            this.chkBox.Enter += new System.EventHandler(this.ChkBox_Enter);
            // 
            // FlatCheckBox
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(223)))), ((int)(((byte)(229)))));
            this.Controls.Add(this.pnlBgWhite);
            this.Name = "FlatCheckBox";
            this.Size = new System.Drawing.Size(100, 34);
            this.SizeChanged += new System.EventHandler(this.CheckBoxBlue_SizeChanged);
            this.Leave += new System.EventHandler(this.CheckBoxBlue_Leave);
            this.pnlBgWhite.ResumeLayout(false);
            this.pnlBgWhite.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBgWhite;
        private System.Windows.Forms.CheckBox chkBox;
        private System.Windows.Forms.Label lblLegend;

    }
}
