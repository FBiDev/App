﻿// <auto-generated/>
namespace App.Core.Desktop
{
    partial class FlatTextBox
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
            this.txtMain = new System.Windows.Forms.TextBox();
            this.lblPlaceholder = new System.Windows.Forms.Label();
            this.pnlText = new System.Windows.Forms.Panel();
            this.ContentPanel.SuspendLayout();
            this.pnlText.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContent
            // 
            this.ContentPanel.Controls.Add(this.pnlText);
            this.ContentPanel.Controls.SetChildIndex(this.SubtitleLabel, 0);
            this.ContentPanel.Controls.SetChildIndex(this.pnlText, 0);
            // 
            // txtMain
            // 
            this.txtMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMain.BackColor = System.Drawing.Color.White;
            this.txtMain.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMain.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.txtMain.Location = new System.Drawing.Point(8, 0);
            this.txtMain.Margin = new System.Windows.Forms.Padding(0);
            this.txtMain.Name = "txtMain";
            this.txtMain.Size = new System.Drawing.Size(190, 16);
            this.txtMain.TabIndex = 0;
            // 
            // lblPlaceholder
            // 
            this.lblPlaceholder.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPlaceholder.BackColor = System.Drawing.Color.Transparent;
            this.lblPlaceholder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblPlaceholder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(178)))), ((int)(((byte)(178)))));
            this.lblPlaceholder.Location = new System.Drawing.Point(5, -1);
            this.lblPlaceholder.Margin = new System.Windows.Forms.Padding(0);
            this.lblPlaceholder.Name = "lblPlaceholder";
            this.lblPlaceholder.Size = new System.Drawing.Size(194, 15);
            this.lblPlaceholder.TabIndex = 0;
            // 
            // pnlText
            // 
            this.pnlText.BackColor = System.Drawing.Color.Transparent;
            this.pnlText.Controls.Add(this.lblPlaceholder);
            this.pnlText.Controls.Add(this.txtMain);
            this.pnlText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlText.Location = new System.Drawing.Point(1, 15);
            this.pnlText.Name = "pnlText";
            this.pnlText.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.pnlText.Size = new System.Drawing.Size(202, 16);
            this.pnlText.TabIndex = 1;
            // 
            // FlatTextBox
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Name = "FlatTextBox";
            this.ContentPanel.ResumeLayout(false);
            this.ContentPanel.PerformLayout();
            this.pnlText.ResumeLayout(false);
            this.pnlText.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.TextBox txtMain;
        protected System.Windows.Forms.Label lblPlaceholder;
        private System.Windows.Forms.Panel pnlText;

    }
}
