namespace App.Core.Desktop
{
    partial class MainBaseForm
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
            this.pnlHead = new App.Core.Desktop.FlatPanelBG();
            this.pnlBody = new App.Core.Desktop.FlatPanelBG();
            this.pnlContent = new App.Core.Desktop.FlatPanel();
            this.pnlFoot = new App.Core.Desktop.FlatPanelBG();
            this.staStatus = new App.Core.Desktop.FlatStatusBar();
            this.pnlHead.SuspendLayout();
            this.pnlBody.SuspendLayout();
            this.pnlFoot.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHead
            // 
            this.pnlHead.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.pnlHead.BackColorType = App.Core.Desktop.PanelType.control;
            this.pnlHead.Controls.Add(this.pnlBody);
            this.pnlHead.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHead.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.pnlHead.Location = new System.Drawing.Point(0, 0);
            this.pnlHead.Margin = new System.Windows.Forms.Padding(0);
            this.pnlHead.Name = "pnlHead";
            this.pnlHead.Padding = new System.Windows.Forms.Padding(8);
            this.pnlHead.Size = new System.Drawing.Size(480, 246);
            // 
            // pnlBody
            // 
            this.pnlBody.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.pnlBody.BackColorType = App.Core.Desktop.PanelType.white;
            this.pnlBody.BorderRound = true;
            this.pnlBody.BorderSize = 1;
            this.pnlBody.Controls.Add(this.pnlContent);
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.pnlBody.Location = new System.Drawing.Point(8, 8);
            this.pnlBody.Margin = new System.Windows.Forms.Padding(0);
            this.pnlBody.Name = "pnlBody";
            this.pnlBody.Padding = new System.Windows.Forms.Padding(5);
            this.pnlBody.Size = new System.Drawing.Size(464, 230);
            // 
            // pnlContent
            // 
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(5, 5);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(454, 220);
            // 
            // pnlFoot
            // 
            this.pnlFoot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.pnlFoot.BackColorType = App.Core.Desktop.PanelType.control;
            this.pnlFoot.Controls.Add(this.staStatus);
            this.pnlFoot.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFoot.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.pnlFoot.Location = new System.Drawing.Point(0, 246);
            this.pnlFoot.Margin = new System.Windows.Forms.Padding(0);
            this.pnlFoot.Name = "pnlFoot";
            this.pnlFoot.Size = new System.Drawing.Size(480, 24);
            this.pnlFoot.TabIndex = 1;
            // 
            // staStatus
            // 
            this.staStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(145)))), ((int)(((byte)(145)))), ((int)(((byte)(145)))));
            this.staStatus.BackColorContent = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.staStatus.BorderEnable = false;
            this.staStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.staStatus.Location = new System.Drawing.Point(0, 0);
            this.staStatus.Movimento = App.Core.Desktop.Movimento.Nenhum;
            this.staStatus.Name = "staStatus";
            this.staStatus.Registros = 0;
            this.staStatus.Size = new System.Drawing.Size(480, 24);
            this.staStatus.TabIndex = 0;
            // 
            // MainBaseForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(480, 270);
            this.Controls.Add(this.pnlHead);
            this.Controls.Add(this.pnlFoot);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(200, 200);
            this.Name = "MainBaseForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainBaseForm";
            this.pnlHead.ResumeLayout(false);
            this.pnlBody.ResumeLayout(false);
            this.pnlFoot.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private FlatPanelBG pnlFoot;
        private FlatPanelBG pnlHead;
        private FlatPanelBG pnlBody;
        protected FlatPanel pnlContent;
        public FlatStatusBar staStatus;

    }
}