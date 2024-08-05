using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public partial class ContentBaseForm : Form
    {
        [DefaultValue(typeof(AutoScaleMode), "None")]
        public new AutoScaleMode AutoScaleMode
        {
            get { return base.AutoScaleMode; }
            set { base.AutoScaleMode = value; }
        }

        public Size SizeOriginal;
        public bool isDesignMode = true;

        bool _firstLoad = true;
        //public event EventHandler FirstLoad;
        public event EventVoid FinalLoadOnce;
        public event EventTaskAsync FinalLoadOnceAsync;

        public ContentBaseForm()
        {
            InitializeComponent();

            HandleCreated += (sender, e) =>
            {
                Init();
                if (isDesignMode) return;

                ResizeBegin += (s, ev) => { TurnOffFormLevelDoubleBuffering(); };
                ResizeEnd += (s, ev) => { TurnOnFormLevelDoubleBuffering(); };
                ThemeBase.CheckTheme(this);
            };

            Shown += (sender, e) =>
            {
                foreach (Control control in Controls)
                    control.Enter += Control_Enter;

                isDesignMode = DesignMode;
                if (isDesignMode) return;

                //ThemeBase.CheckTheme(this);
            };

            Resize += OnResize;

            TopLevel = false;
        }

        public void FinalLoadOnShow()
        {
            if (_firstLoad)
            {
                _firstLoad = false;

                if (FinalLoadOnce.NotNull())
                    FinalLoadOnce();

                if (FinalLoadOnceAsync.NotNull())
                    FinalLoadOnceAsync().TryAwait();
            }
        }

        void Init()
        {
            SizeOriginal = Size;

            var panels = Controls.OfType<FlatPanel>();

            int tabIndex = 0;

            panels = panels.Reverse();
            panels.ForEach(x =>
            {
                x.TabIndex = tabIndex;
                tabIndex++;
            });
            //KeyPreview = true;
        }

        void Control_Enter(object sender, EventArgs e)
        {
            ScrollControlIntoView((Control)sender);
        }

        public void OnResize(object sender, EventArgs e)
        {
            var tableLayouts = Controls.OfType<FlatTableLayoutPanel>();
            foreach (var tbl in tableLayouts)
            {
                if (tbl.FillOnFormResize == false) continue;

                if (Height >= tbl.SizeOriginal.Height)
                    tbl.Dock = DockStyle.Fill;
                else if (Height < tbl.SizeOriginal.Height)
                    tbl.Dock = DockStyle.Top;
            }
        }

        #region Fix_Flickering_Controls
        bool enableFormLevelDoubleBuffering = true;
        int originalExStyle = -1;

        void TurnOnFormLevelDoubleBuffering()
        {
            enableFormLevelDoubleBuffering = true;
            UpdateStyles();
        }

        void TurnOffFormLevelDoubleBuffering()
        {
            enableFormLevelDoubleBuffering = false;
            UpdateStyles();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                if (originalExStyle == -1)
                    originalExStyle = base.CreateParams.ExStyle;

                CreateParams cp = base.CreateParams;
                if (enableFormLevelDoubleBuffering && DesignMode == false)
                    cp.ExStyle |= 0x02000000;   // WS_EX_COMPOSITED
                else
                    cp.ExStyle = originalExStyle;

                return cp;
            }
        }
        #endregion
    }
}