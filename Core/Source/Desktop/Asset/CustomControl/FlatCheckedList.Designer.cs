﻿// <auto-generated/>
namespace App.Core.Desktop
{
    partial class FlatCheckedList
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
            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlList = new System.Windows.Forms.Panel();
            this.chkList = new System.Windows.Forms.CheckedListBox();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.pnlContent.SuspendLayout();
            this.pnlList.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContent
            // 
            this.pnlContent.BackColor = System.Drawing.Color.White;
            this.pnlContent.Controls.Add(this.pnlList);
            this.pnlContent.Controls.Add(this.lblSubtitle);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(1, 1);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Padding = new System.Windows.Forms.Padding(1, 2, 1, 1);
            this.pnlContent.Size = new System.Drawing.Size(118, 58);
            this.pnlContent.TabIndex = 0;
            // 
            // pnlList
            // 
            this.pnlList.Controls.Add(this.chkList);
            this.pnlList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlList.Location = new System.Drawing.Point(1, 15);
            this.pnlList.Name = "pnlList";
            this.pnlList.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.pnlList.Size = new System.Drawing.Size(116, 42);
            this.pnlList.TabIndex = 2;
            // 
            // chkList
            // 
            this.chkList.BackColor = System.Drawing.Color.White;
            this.chkList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.chkList.CheckOnClick = true;
            this.chkList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.chkList.FormattingEnabled = true;
            this.chkList.IntegralHeight = false;
            this.chkList.Location = new System.Drawing.Point(3, 0);
            this.chkList.Name = "chkList";
            this.chkList.Size = new System.Drawing.Size(110, 42);
            this.chkList.TabIndex = 1;
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(132)))), ((int)(((byte)(199)))));
            this.lblSubtitle.Location = new System.Drawing.Point(1, 2);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(34, 13);
            this.lblSubtitle.TabIndex = 0;
            this.lblSubtitle.Text = "Label";
            // 
            // FlatCheckedList
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(223)))), ((int)(((byte)(229)))));
            this.Controls.Add(this.pnlContent);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(64, 34);
            this.Name = "FlatCheckedList";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.Size = new System.Drawing.Size(120, 60);
            this.pnlContent.ResumeLayout(false);
            this.pnlContent.PerformLayout();
            this.pnlList.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlContent;
        protected System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.CheckedListBox chkList;
        private System.Windows.Forms.Panel pnlList;
    }
}
