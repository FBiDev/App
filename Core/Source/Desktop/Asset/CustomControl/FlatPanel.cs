using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public partial class FlatPanel : Panel
    {
        private PanelType _backColorType;
        private int _borderSize;

        public FlatPanel()
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            DoubleBuffered = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;

            BorderSize = 0;
            BorderColor = ColorTranslator.FromHtml("#A0A0A0");
            BorderRound = false;

            BackColor = Color.Transparent;
        }

        [DefaultValue(false)]
        public bool NoScrollOnFocus { get; set; }

        [DefaultValue(typeof(Color), "Transparent")]
        public new Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        [DefaultValue(typeof(PanelType), "transparent")]
        public PanelType BackColorType
        {
            get
            {
                return _backColorType;
            }

            set
            {
                _backColorType = value;
                ThemeBase.CheckControlTheme(this);
            }
        }

        [DefaultValue(typeof(Color), "0xA0A0A0")]
        public Color BorderColor { get; set; }

        [DefaultValue(0)]
        public int BorderSize
        {
            get
            {
                return _borderSize;
            }

            set
            {
                _borderSize = value;
                Padding = new Padding(value);
            }
        }

        [DefaultValue(false)]
        public bool BorderRound { get; set; }

        [DefaultValue(GraphicsExtension.BorderDrawSide.All)]
        public GraphicsExtension.BorderDrawSide BorderSides { get; set; }

        [DefaultValue(typeof(Padding), "0, 0, 0, 0")]
        public new Padding Padding
        {
            get { return base.Padding; }
            set { base.Padding = value; }
        }

        [DefaultValue(typeof(AutoSizeMode), "GrowAndShrink")]
        public new AutoSizeMode AutoSizeMode
        {
            get { return base.AutoSizeMode; }
            set { base.AutoSizeMode = value; }
        }

        [DefaultValue(0)]
        public new int TabIndex
        {
            get { return base.TabIndex; }
            set { base.TabIndex = value; }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawRoundBorder(this, BorderColor, BorderSize, BorderRound, BorderSides);
        }

        protected override Point ScrollToControl(Control activeControl)
        {
            if (NoScrollOnFocus)
            {
                return DisplayRectangle.Location;
            }

            return base.ScrollToControl(activeControl);
        }
    }
}