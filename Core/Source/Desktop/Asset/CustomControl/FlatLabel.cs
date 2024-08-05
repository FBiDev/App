using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public partial class FlatLabel : Label
    {
        [DefaultValue(false)]
        public new bool UseMnemonic
        {
            get { return base.UseMnemonic; }
            set { base.UseMnemonic = value; }
        }

        [DefaultValue(false)]
        public new bool AutoSize
        {
            get { return base.AutoSize; }
            set { base.AutoSize = value; }
        }

        [DefaultValue(typeof(Size), "48, 24")]
        public new Size MinimumSize
        {
            get { return base.MinimumSize; }
            set { base.MinimumSize = value; }
        }

        [DefaultValue(typeof(Size), "66, 24")]
        public new Size Size
        {
            get { return base.Size; }
            set { base.Size = value; }
        }

        [DefaultValue(typeof(Padding), "0, 0, 0, 1")]
        public new Padding Padding
        {
            get { return base.Padding; }
            set { base.Padding = value; }
        }

        [DefaultValue(typeof(ContentAlignment), "MiddleLeft")]
        public new ContentAlignment TextAlign
        {
            get { return base.TextAlign; }
            set { base.TextAlign = value; }
        }

        [DefaultValue(typeof(Font), "Segoe UI, 9pt")]
        public new Font Font
        {
            get { return base.Font; }
            set { base.Font = value; }
        }

        [DefaultValue(typeof(Color), "Black")]
        public new Color ForeColor
        {
            get { return base.ForeColor; }
            set { base.ForeColor = value; }
        }

        LabelType _ForeColorType;
        [DefaultValue(typeof(LabelType), "normal")]
        public LabelType ForeColorType
        {
            get { return _ForeColorType; }
            set
            {
                _ForeColorType = value;
                ThemeBase.CheckControlTheme(this);
            }
        }

        [DefaultValue(typeof(Color), "Transparent")]
        public new Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        public FlatLabel()
        {
            InitializeComponent();

            UseMnemonic = false;

            TextAlign = ContentAlignment.MiddleLeft;
            Font = new Font(new FontFamily("Segoe UI"), 9, FontStyle.Regular);
            ForeColor = Color.Black;
            BackColor = Color.Transparent;

            AutoSize = false;

            MinimumSize = new Size(48, 24);
            Size = new Size(66, 24);
            Padding = new Padding(0, 0, 0, 1);
        }

        public override string ToString()
        {
            return Name;
        }

        readonly int WM_LBUTTONDBLCLK = 0x203;
        string sSaved;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_LBUTTONDBLCLK)
            {
                this.InvokeIfRequired(() =>
                {
                    sSaved = ClipboardSafe.GetText();

                    if (!string.IsNullOrEmpty(sSaved))
                        ClipboardSafe.SetText(sSaved);
                    else
                        ClipboardSafe.Clear();
                });
            }
        }
    }
}