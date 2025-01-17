﻿// <auto-generated/>
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
            this.BackgroundPanel = new System.Windows.Forms.Panel();
            this.LegendLabel = new System.Windows.Forms.Label();
            this.MainCheckBox = new FlatCheckBoxInner();
            this.BackgroundPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // BackgroundPanel
            // 
            this.BackgroundPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BackgroundPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackgroundPanel.BackColor = System.Drawing.Color.White;
            this.BackgroundPanel.Controls.Add(this.LegendLabel);
            this.BackgroundPanel.Controls.Add(this.MainCheckBox);
            this.BackgroundPanel.Location = new System.Drawing.Point(1, 1);
            this.BackgroundPanel.Margin = new System.Windows.Forms.Padding(0);
            this.BackgroundPanel.Name = "BackgroundPanel";
            this.BackgroundPanel.Size = new System.Drawing.Size(98, 32);
            this.BackgroundPanel.TabIndex = 0;
            this.BackgroundPanel.Click += new System.EventHandler(this.BackgroundPanel_Click);
            this.BackgroundPanel.DoubleClick += new System.EventHandler(this.BackgroundPanel_DoubleClick);
            this.BackgroundPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BackgroundPanel_MouseDown);
            this.BackgroundPanel.MouseEnter += new System.EventHandler(this.BackgroundPanel_MouseEnter);
            this.BackgroundPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BackgroundPanel_MouseUp);
            // 
            // LegendLabel
            // 
            this.LegendLabel.AutoSize = true;
            this.LegendLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LegendLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(132)))), ((int)(((byte)(199)))));
            this.LegendLabel.Location = new System.Drawing.Point(1, 2);
            this.LegendLabel.Margin = new System.Windows.Forms.Padding(0);
            this.LegendLabel.Name = "LegendLabel";
            this.LegendLabel.Size = new System.Drawing.Size(45, 13);
            this.LegendLabel.TabIndex = 2;
            this.LegendLabel.Text = "Legend";
            this.LegendLabel.Click += new System.EventHandler(this.LegendLabel_Click);
            this.LegendLabel.DoubleClick += new System.EventHandler(this.LegendLabel_DoubleClick);
            this.LegendLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LegendLabel_MouseDown);
            this.LegendLabel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LegendLabel_MouseUp);
            // 
            // MainCheckBox
            // 
            this.MainCheckBox.BackColor = System.Drawing.Color.LightCoral;
            this.MainCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MainCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(132)))), ((int)(((byte)(199)))));
            this.MainCheckBox.Location = new System.Drawing.Point(4, 18);
            this.MainCheckBox.Name = "MainCheckBox";
            this.MainCheckBox.Size = new System.Drawing.Size(11, 11);
            this.MainCheckBox.TabIndex = 1;
            this.MainCheckBox.UseVisualStyleBackColor = false;
            this.MainCheckBox.Enter += new System.EventHandler(this.CheckBox_Enter);
            // 
            // FlatCheckBox
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(223)))), ((int)(((byte)(229)))));
            this.Controls.Add(this.BackgroundPanel);
            this.Name = "FlatCheckBox";
            this.Size = new System.Drawing.Size(100, 34);
            this.SizeChanged += new System.EventHandler(this.CheckBox_SizeChanged);
            this.Leave += new System.EventHandler(this.CheckBox_Leave);
            this.BackgroundPanel.ResumeLayout(false);
            this.BackgroundPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel BackgroundPanel;
        private FlatCheckBoxInner MainCheckBox;
        private System.Windows.Forms.Label LegendLabel;

    }
}
