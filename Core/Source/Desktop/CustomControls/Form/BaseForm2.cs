using System;
using System.Drawing;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public partial class BaseForm2 : Form
    {
        public BaseForm2()
        {
            InitializeComponent();
            ResizeRedraw = true;
            lblCenter.Enabled = false;

            AutoScaleMode = AutoScaleMode.None;

            Load += FormBase_Load;
            Resize += FormBase_Resize;
        }

        void FormBase_Load(object sender, EventArgs e) { }

        protected override void OnHandleCreated(EventArgs e)
        {
            ResizeMargins();
        }

        void FormBase_Resize(object sender, EventArgs e)
        {
            ResizeMargins();
        }

        public void ResizeMargins()
        {
            pnlBorder1.Location = new Point(1, 1);
            pnlBorder1.Size = new Size(
                ClientSize.Width - (pnlBorder1.Location.X * 2),
                (ClientSize.Height) - (pnlBorder1.Location.Y * 2));

            pnlBorder2.Location = new Point(1, 1);
            pnlBorder2.Size = new Size(
                pnlBorder1.Width - (pnlBorder2.Location.X * 2),
                pnlBorder1.Height - (pnlBorder2.Location.Y * 2));

            pnlContent.Location = new Point(0, 0);
            pnlContent.Size = new Size(
                pnlBorder2.Width - (pnlContent.Location.X * 2),
                pnlBorder2.Height - (pnlContent.Location.Y * 2));

            pnlMargin.Location = new Point(6, 6);
            pnlMargin.Size = new Size(
                pnlContent.Width - (pnlMargin.Location.X * 2),
                pnlContent.Height - (pnlMargin.Location.Y * 2));

            lblCenter.Location = new Point(6, 0);
            lblCenter.Size = new Size(pnlContent.Width / 2 - 6, 6);
        }
    }
}