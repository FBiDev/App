using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public partial class MainBaseForm : Form
    {
        private bool _statusBarEnable;

        public MainBaseForm()
        {
            InitializeComponent();

            IsDesignMode = true;
            AutoResizeWindow = true;
            AutoCenterWindow = true;

            HandleCreated += (sender, e) =>
            {
                Init();

                if (IsDesignMode)
                {
                    return;
                }

                ThemeBase.CheckTheme(this);
            };

            Shown += (sender, e) =>
            {
                IsDesignMode = DesignMode;

                if (IsDesignMode)
                {
                    return;
                }

                AwaitShown().TryAwait();

                // ThemeBase.CheckTheme(this);
            };

            if (DesignMode == false)
            {
                Opacity = 0;
            }

            ResizeRedraw = true;
            StatusBarEnable = true;
            DoubleBuffered = true;
        }

        public event Action TabPressed = delegate { };

        public static Icon Ico { get; set; }

        public static bool DebugMode { get; set; }

        public static bool AutoResizeWindow { get; set; }

        public static bool AutoCenterWindow { get; set; }

        [DefaultValue(typeof(AutoScaleMode), "None")]
        public new AutoScaleMode AutoScaleMode
        {
            get { return base.AutoScaleMode; }
            set { base.AutoScaleMode = value; }
        }

        public bool StatusBarEnable
        {
            get
            {
                return _statusBarEnable;
            }

            set
            {
                _statusBarEnable = value;
                FootPanel.Visible = value;
                HeadPanel.Padding = new Padding
                {
                    All = HeadPanel.Padding.Left,
                    Bottom = value == false ? HeadPanel.Padding.Left : 8
                };
            }
        }

        public bool IsDesignMode { get; set; }

        public void SetMainContentPage(Form mainContentPage)
        {
            ContentPanel.Controls.Clear();
            ContentPanel.Controls.Add(mainContentPage);

            mainContentPage.Dock = DockStyle.Fill;
            mainContentPage.Show();
        }

        public void CenterWindow()
        {
            if (AutoCenterWindow == false)
            {
                return;
            }

            this.InvokeIfRequired(CenterToScreen);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            AutoScaleDimensions = new SizeF(0F, 0F);
            AutoScaleMode = AutoScaleMode.None;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Tab)
            {
                if (TabPressed.NotNull())
                {
                    TabPressed();
                }
            }
            else if (DebugMode && keyData == Keys.F1)
            {
                DebugManager.Open();
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private async Task AwaitShown()
        {
            await Task.Delay(50);
            Opacity = 1;
        }

        private void Init()
        {
            if (Ico is Icon)
            {
                Icon = Ico;
            }
        }
    }
}