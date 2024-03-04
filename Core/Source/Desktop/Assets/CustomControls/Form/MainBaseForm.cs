using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public partial class MainBaseForm : Form
    {
        [DefaultValue(typeof(AutoScaleMode), "None")]
        public new AutoScaleMode AutoScaleMode
        {
            get { return base.AutoScaleMode; }
            set { base.AutoScaleMode = value; }
        }

        protected bool _statusBarEnable;

        public bool StatusBarEnable
        {
            get { return _statusBarEnable; }

            set
            {
                _statusBarEnable = value;
                pnlFoot.Visible = value;
                pnlHead.Padding = new Padding
                {
                    All = pnlHead.Padding.Left,
                    Bottom = value == false ? pnlHead.Padding.Left : 8
                };
            }
        }

        public static Icon Ico { get; set; }
        public bool isDesignMode = true;
        public static bool DebugMode;

        public MainBaseForm()
        {
            InitializeComponent();
            HandleCreated += (sender, e) =>
            {
                Init();
                if (isDesignMode) return;

                ThemeBase.CheckTheme(this);
            };

            Shown += (sender, e) =>
            {
                isDesignMode = DesignMode;
                if (isDesignMode) return;

                AwaitShown().TryAwait();
                //ThemeBase.CheckTheme(this);
            };

            if (DesignMode == false)
                Opacity = 0;

            ResizeRedraw = true;
            StatusBarEnable = true;
            DoubleBuffered = true;
        }

        async Task AwaitShown()
        {
            await Task.Delay(50);
            Opacity = 1;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            AutoScaleDimensions = new SizeF(0F, 0F);
            AutoScaleMode = AutoScaleMode.None;
        }

        public event Action TabPressed = delegate { };

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Tab)
            {
                if (TabPressed.NotNull())
                {
                    TabPressed();
                }
            }
            else if (DebugMode && keyData == (Keys.F1))
            {
                DebugManager.Open();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        void Init()
        {
            if (Ico is Icon)
                Icon = Ico;
        }

        public void SetMainFormContent(Form frm)
        {
            pnlContent.Controls.Clear();
            pnlContent.Controls.Add(frm);

            frm.Dock = DockStyle.Fill;
            frm.Show();
        }

        public static bool AutoResizeWindow = true;
        public static bool AutoCenterWindow = true;

        public void CenterWindow()
        {
            if (AutoCenterWindow == false) return;

            this.InvokeIfRequired(CenterToScreen);
        }
    }
}