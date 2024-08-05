using System;
using System.Drawing;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public partial class FormBase : Form
    {
        protected bool _statusBar;

        public bool StatusBar
        {
            get { return _statusBar; }

            set
            {
                _statusBar = value;
                pnlFoot.Visible = value;
                pnlHead.Padding = new Padding
                {
                    All = pnlHead.Padding.Left,
                    Bottom = value == false ? pnlHead.Padding.Left : 32
                };
            }
        }

        public FormBase()
        {
            InitializeComponent();
            ResizeRedraw = true;
            StatusBar = true;

            DoubleBuffered = true;
        }

        public void Init(Form frm)
        {
            if (Ico is Icon)
                Icon = Ico;

            ThemeBase.CheckTheme(frm);
        }

        public static Icon Ico { get; set; }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            AutoScaleDimensions = new SizeF(0F, 0F);
            AutoScaleMode = AutoScaleMode.None;
        }
    }
}