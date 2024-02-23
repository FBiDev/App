namespace App.Core.Desktop
{
    partial class FlatComboBox
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
            this.pnlBorder = new System.Windows.Forms.Panel();
            this.pnlBg = new System.Windows.Forms.Panel();
            this.picDrop = new System.Windows.Forms.PictureBox();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.cboFlat = new App.Core.Desktop.FlatComboBoxNew();
            this.pnlBorder.SuspendLayout();
            this.pnlBg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDrop)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlBorder
            // 
            this.pnlBorder.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlBorder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(223)))), ((int)(((byte)(229)))));
            this.pnlBorder.Controls.Add(this.pnlBg);
            this.pnlBorder.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlBorder.Location = new System.Drawing.Point(0, 0);
            this.pnlBorder.Margin = new System.Windows.Forms.Padding(0);
            this.pnlBorder.Name = "pnlBorder";
            this.pnlBorder.Size = new System.Drawing.Size(206, 34);
            this.pnlBorder.TabIndex = 1;
            // 
            // pnlBg
            // 
            this.pnlBg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlBg.BackColor = System.Drawing.Color.White;
            this.pnlBg.Controls.Add(this.picDrop);
            this.pnlBg.Controls.Add(this.lblSubtitle);
            this.pnlBg.Controls.Add(this.cboFlat);
            this.pnlBg.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlBg.Location = new System.Drawing.Point(1, 1);
            this.pnlBg.Margin = new System.Windows.Forms.Padding(0);
            this.pnlBg.Name = "pnlBg";
            this.pnlBg.Size = new System.Drawing.Size(204, 32);
            this.pnlBg.TabIndex = 1;
            // 
            // picDrop
            // 
            this.picDrop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picDrop.BackgroundImage = global::App.Core.Properties.Resources.img_drop_arrow;
            this.picDrop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.picDrop.InitialImage = null;
            this.picDrop.Location = new System.Drawing.Point(171, 1);
            this.picDrop.Name = "picDrop";
            this.picDrop.Size = new System.Drawing.Size(32, 32);
            this.picDrop.TabIndex = 5;
            this.picDrop.TabStop = false;
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.BackColor = System.Drawing.Color.White;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(132)))), ((int)(((byte)(199)))));
            this.lblSubtitle.Location = new System.Drawing.Point(1, 2);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(34, 13);
            this.lblSubtitle.TabIndex = 0;
            this.lblSubtitle.Text = "Label";
            // 
            // cboFlat
            // 
            this.cboFlat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboFlat.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboFlat.BackColor = System.Drawing.Color.White;
            this.cboFlat.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboFlat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFlat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboFlat.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cboFlat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.cboFlat.IntegralHeight = false;
            this.cboFlat.ItemHeight = 18;
            this.cboFlat.Location = new System.Drawing.Point(-1, 11);
            this.cboFlat.Margin = new System.Windows.Forms.Padding(0);
            this.cboFlat.MaxDropDownItems = 10;
            this.cboFlat.Name = "cboFlat";
            this.cboFlat.Size = new System.Drawing.Size(204, 24);
            this.cboFlat.TabIndex = 0;
            // 
            // FlatComboBox
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.pnlBorder);
            this.DoubleBuffered = true;
            this.MaximumSize = new System.Drawing.Size(0, 34);
            this.MinimumSize = new System.Drawing.Size(100, 34);
            this.Name = "FlatComboBox";
            this.Size = new System.Drawing.Size(206, 34);
            this.pnlBorder.ResumeLayout(false);
            this.pnlBg.ResumeLayout(false);
            this.pnlBg.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDrop)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblSubtitle;
        private FlatComboBoxNew cboFlat;
        private System.Windows.Forms.Panel pnlBorder;
        private System.Windows.Forms.Panel pnlBg;
        private System.Windows.Forms.PictureBox picDrop;
    }
}
