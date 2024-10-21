using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public partial class FlatTextBoxBase : UserControl
    {
        private Color _borderColor = Color.FromArgb(213, 223, 229);
        private Color _borderColorFocus = Color.FromArgb(108, 132, 199);
        private Color _backgroundColor = Color.White;
        private Color _labelTextColor = Color.FromArgb(108, 132, 199);

        public FlatTextBoxBase()
        {
            InitializeComponent();

            ContentPanel.MouseEnter += PnlBg_MouseEnter;
            SubtitleLabel.MouseEnter += LblSubtitle_MouseEnter;

            Font = new Font("Segoe UI", 9);
            Margin = new Padding(2);
        }

        #region Defaults
        [DefaultValue(typeof(AnchorStyles), "Top, Left")]
        public new AnchorStyles Anchor
        {
            get { return base.Anchor; }
            set { base.Anchor = value; }
        }

        [DefaultValue(typeof(Font), "Segoe UI, 9")]
        public new Font Font
        {
            get { return base.Font; }
            set { base.Font = value; }
        }

        [DefaultValue(typeof(Padding), "2, 2, 2, 2")]
        public new Padding Margin
        {
            get { return base.Margin; }
            set { base.Margin = value; }
        }

        [DefaultValue(typeof(Padding), "1, 1, 1, 1")]
        public new Padding Padding
        {
            get { return base.Padding; }
            set { base.Padding = value; }
        }
        #endregion

        #region Properties
        [Category("_Colors"), DefaultValue(typeof(Color), "213, 223, 229")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        [Category("_Colors"), DefaultValue(typeof(Color), "213, 223, 229")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set { _borderColor = value; }
        }

        [Category("_Colors"), DefaultValue(typeof(Color), "108, 132, 199")]
        public Color BorderColorFocus
        {
            get { return _borderColorFocus; }
            set { _borderColorFocus = value; }
        }

        [Category("_Colors"), DefaultValue(typeof(Color), "White")]
        public Color BackgroundColor
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; }
        }

        [Category("_Colors"), DefaultValue(typeof(Color), "108, 132, 199")]
        public Color LabelTextColor
        {
            get { return _labelTextColor; }
            set { _labelTextColor = value; }
        }

        [Category("_Colors"), DefaultValue(typeof(string), "Label")]
        public string LabelText
        {
            get { return SubtitleLabel.Text; }
            set { SubtitleLabel.Text = value; }
        }
        #endregion

        protected override void OnHandleCreated(EventArgs e)
        {
            BackColor = BorderColor;
            ContentPanel.BackColor = BackgroundColor;

            SubtitleLabel.BackColor = BackgroundColor;
            SubtitleLabel.ForeColor = LabelTextColor;
        }

        protected void ChangeCursor()
        {
            Cursor = TabStop ? Cursors.IBeam : Cursors.No;
        }

        protected void ChangeCursor2()
        {
            Cursor = TabStop ? Cursors.Default : Cursors.No;
        }

        protected void TextBoxBase_Paint(object sender, PaintEventArgs e)
        {
            if (Parent != null)
            {
                // this.MaximumSize = new Size(Parent.Width, this.MaximumSize.Height);
                // if (Width > MaximumSize.Width)
                // {
                //    Width = MaximumSize.Width;
                // }
            }
        }

        protected void PnlBg_MouseEnter(object sender, EventArgs e)
        {
            ChangeCursor();
        }

        protected void LblSubtitle_MouseEnter(object sender, EventArgs e)
        {
            ChangeCursor2();
        }
    }
}
