using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public partial class ContentBaseForm : Form
    {
        private bool _firstLoad = true;

        // Fix_Flickering_Controls
        private bool enableFormLevelDoubleBuffering = true;
        private int originalExStyle = -1;

        public ContentBaseForm()
        {
            InitializeComponent();

            IsDesignMode = true;

            HandleCreated += (sender, e) =>
            {
                Init();

                if (IsDesignMode)
                {
                    return;
                }

                ResizeBegin += (s, ev) => { TurnOffFormLevelDoubleBuffering(); };
                ResizeEnd += (s, ev) => { TurnOnFormLevelDoubleBuffering(); };
                ThemeBase.CheckTheme(this);
            };

            Shown += (sender, e) =>
            {
                foreach (Control control in Controls)
                {
                    control.Enter += Control_Enter;
                }

                IsDesignMode = DesignMode;

                if (IsDesignMode)
                {
                    return;
                }

                // ThemeBase.CheckTheme(this);
            };

            ParentChanged += (s, e) =>
            {
                if (Parent != null)
                {
                    this.GetControls<FlatDataGrid>().ForEach(d =>
                    {
                        d.RefreshRows();
                    });
                }
            };

            Resize += OnResize;

            TopLevel = false;
        }

        public event EventVoid FinalLoadOnce;

        public event EventTaskAsync FinalLoadOnceAsync;

        ////public event EventHandler FirstLoad;

        [DefaultValue(typeof(AutoScaleMode), "None")]
        public new AutoScaleMode AutoScaleMode
        {
            get { return base.AutoScaleMode; }
            set { base.AutoScaleMode = value; }
        }

        public bool IsDesignMode { get; set; }

        public Size SizeOriginal { get; set; }

        #region Fix_Flickering_Controls
        protected override CreateParams CreateParams
        {
            get
            {
                if (originalExStyle == -1)
                {
                    originalExStyle = base.CreateParams.ExStyle;
                }

                CreateParams cp = base.CreateParams;
                if (enableFormLevelDoubleBuffering && DesignMode == false)
                {
                    cp.ExStyle |= Native.Window.Flag.WS_EX_COMPOSITED;
                }
                else
                {
                    cp.ExStyle = originalExStyle;
                }

                return cp;
            }
        }
        #endregion

        public void OnResize(object sender, EventArgs e)
        {
            var tableLayouts = Controls.OfType<FlatTable>();

            foreach (var tbl in tableLayouts)
            {
                if (tbl.FillOnFormResize == false)
                {
                    continue;
                }

                if (Height >= tbl.SizeOriginal.Height)
                {
                    tbl.Dock = DockStyle.Fill;
                }
                else if (Height < tbl.SizeOriginal.Height)
                {
                    tbl.Dock = DockStyle.Top;
                }
            }
        }

        public void FinalLoadOnShow()
        {
            if (_firstLoad == false)
            {
                return;
            }

            _firstLoad = false;

            if (FinalLoadOnce.NotNull())
            {
                FinalLoadOnce();
            }

            if (FinalLoadOnceAsync.NotNull())
            {
                FinalLoadOnceAsync().TryAwait();
            }
        }

        private void Init()
        {
            SizeOriginal = Size;

            int tabIndex = 0;

            var panels = Controls.OfType<FlatPanel>();

            panels = panels.Reverse();

            panels.ForEach(x =>
            {
                x.TabIndex = tabIndex;
                tabIndex++;
            });

            // KeyPreview = true;
        }

        private void Control_Enter(object sender, EventArgs e)
        {
            ScrollControlIntoView((Control)sender);
        }

        private void TurnOnFormLevelDoubleBuffering()
        {
            enableFormLevelDoubleBuffering = true;
            UpdateStyles();
        }

        private void TurnOffFormLevelDoubleBuffering()
        {
            enableFormLevelDoubleBuffering = false;
            UpdateStyles();
        }
    }
}