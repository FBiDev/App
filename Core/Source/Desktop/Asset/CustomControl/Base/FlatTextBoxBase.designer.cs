﻿// <auto-generated/>
namespace App.Core.Desktop
{
    partial class FlatTextBoxBase
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
            this.ContentPanel = new System.Windows.Forms.Panel();
            this.SubtitleLabel = new System.Windows.Forms.Label();
            this.ContentPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContent
            // 
            this.ContentPanel.BackColor = System.Drawing.Color.White;
            this.ContentPanel.Controls.Add(this.SubtitleLabel);
            this.ContentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ContentPanel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ContentPanel.Location = new System.Drawing.Point(1, 1);
            this.ContentPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ContentPanel.Name = "pnlContent";
            this.ContentPanel.Padding = new System.Windows.Forms.Padding(1, 2, 1, 1);
            this.ContentPanel.Size = new System.Drawing.Size(204, 32);
            this.ContentPanel.TabIndex = 0;
            // 
            // lblSubtitle
            // 
            this.SubtitleLabel.AutoSize = true;
            this.SubtitleLabel.BackColor = System.Drawing.Color.White;
            this.SubtitleLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.SubtitleLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SubtitleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(132)))), ((int)(((byte)(199)))));
            this.SubtitleLabel.Location = new System.Drawing.Point(1, 2);
            this.SubtitleLabel.Name = "lblSubtitle";
            this.SubtitleLabel.Size = new System.Drawing.Size(34, 13);
            this.SubtitleLabel.TabIndex = 0;
            this.SubtitleLabel.Text = "Label";
            // 
            // FlatTextBoxBase
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(223)))), ((int)(((byte)(229)))));
            this.Controls.Add(this.ContentPanel);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximumSize = new System.Drawing.Size(0, 34);
            this.MinimumSize = new System.Drawing.Size(100, 34);
            this.Name = "FlatTextBoxBase";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.Size = new System.Drawing.Size(206, 34);
            this.ContentPanel.ResumeLayout(false);
            this.ContentPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.Panel ContentPanel;
        protected System.Windows.Forms.Label SubtitleLabel;


    }
}
