using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public partial class FlatTextBox : FlatTextBoxBase
    {
        private Color _backgroundColorFocus = Color.FromArgb(252, 245, 237);
        private Color _textColor = Color.FromArgb(47, 47, 47);
        private Color _textColorFocus = Color.FromArgb(47, 47, 47);

        public FlatTextBox()
        {
            InitializeComponent();

            lblPlaceholder.MouseEnter += LblPlaceholder_MouseEnter;

            ContentPanel.Click += PnlBg_Click;
            SubtitleLabel.Click += LblSubtitle_Click;
            lblPlaceholder.Click += LblPlaceholder_Click;

            TextBox.KeyPress += TextBox_KeyPress;
            TextBox.KeyDown += TextBox_KeyDown;
            TextBox.KeyUp += TextBox_KeyUp;
            TextBox.MouseUp += TextBox_MouseUp;

            TextBox.GotFocus += TextBox_GotFocus;
            TextBox.LostFocus += TextBox_LostFocus;
            TextBox.TextChanged += TextBox_TextChanged;

            TextBox.Enter += TextBox_Enter;
            TextBox.Leave += TextBox_Leave;

            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            Size = new Size(206, 34);
            MaximumSize = new Size(1500, 34);
            MinimumSize = new Size(58, 34);
        }

        public new event EventHandler TextChanged;

        public new event EventHandler Enter;

        public new event EventHandler Leave;

        public new event KeyPressEventHandler KeyPress;

        #region Defaults
        [DefaultValue(typeof(Size), "1500, 34")]
        public new Size MaximumSize
        {
            get { return base.MaximumSize; }
            set { base.MaximumSize = value; }
        }

        [DefaultValue(typeof(Size), "58, 34")]
        public new Size MinimumSize
        {
            get { return base.MinimumSize; }
            set { base.MinimumSize = value; }
        }
        #endregion

        #region Properties

        [Category("_Colors"), DefaultValue(typeof(Color), "252, 245, 237")]
        public Color BackgroundColorFocus
        {
            get { return _backgroundColorFocus; }
            set { _backgroundColorFocus = value; }
        }

        [Category("_Colors"), DefaultValue(typeof(Color), "47, 47, 47")]
        public Color TextColor
        {
            get { return _textColor; }
            set { _textColor = value; }
        }

        [Category("_Colors"), DefaultValue(typeof(Color), "47, 47, 47")]
        public Color TextColorFocus
        {
            get { return _textColorFocus; }
            set { _textColorFocus = value; }
        }

        [Category("_Colors"), DefaultValue(typeof(string), "")]
        public string DefaultText
        {
            get { return TextBox.Text; }
            set { TextBox.Text = value; }
        }

        [Category("_Colors"), DefaultValue(typeof(string), "")]
        public string PlaceholderText
        {
            get { return lblPlaceholder.Text; }
            set { lblPlaceholder.Text = value; }
        }
        #endregion

        #region TextBox
        public new string Text
        {
            get { return TextBox.Text; }
            set { TextBox.Text = value; }
        }

        [DefaultValue(0)]
        public int SelectionStart
        {
            get { return TextBox.SelectionStart; }
            set { TextBox.SelectionStart = value; }
        }

        [DefaultValue(0)]
        public int SelectionLength
        {
            get { return TextBox.SelectionLength; }
            set { TextBox.SelectionLength = value; }
        }

        [DefaultValue(false)]
        public bool ReadOnly
        {
            get { return TextBox.ReadOnly; }
            set { TextBox.ReadOnly = value; }
        }

        public new bool Enabled
        {
            get
            {
                return TextBox.Enabled;
            }

            set
            {
                TextBox.Enabled = value;
                TabStop = value;
                ChangeCursor();
            }
        }

        [DefaultValue(false)]
        public bool Multiline
        {
            get { return TextBox.Multiline; }
            set { TextBox.Multiline = value; }
        }

        [DefaultValue(32767)]
        public int MaxLength
        {
            get { return TextBox.MaxLength; }
            set { TextBox.MaxLength = value; }
        }

        [DefaultValue(typeof(ScrollBars), "None")]
        public ScrollBars ScrollBars
        {
            get { return TextBox.ScrollBars; }
            set { TextBox.ScrollBars = value; }
        }

        public override bool Focused
        {
            get { return txtMain.Focused; }
        }

        public string PreviousText { get; set; }

        private string PreviousTextBackup { get; set; }

        private bool PreviousTextChanged { get; set; }

        private TextBox TextBox
        {
            get { return txtMain; }
        }
        #endregion

        public new bool Focus()
        {
            txtMain.Focus();
            return txtMain.Focused;
        }

        public override string ToString()
        {
            return Name + ": " + Formatter.Quote(txtMain.Text);
        }

        public void ResetColors()
        {
            BackColor = BorderColor;
            ContentPanel.BackColor = BackgroundColor;

            SubtitleLabel.BackColor = BackgroundColor;
            SubtitleLabel.ForeColor = LabelTextColor;

            lblPlaceholder.BackColor = BackgroundColor;

            TextBox.BackColor = BackgroundColor;
            TextBox.ForeColor = TextColor;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            ResetColors();

            PreviousText = TextBox.Text;
            PreviousTextBackup = TextBox.Text;

            if (TextBox.ScrollBars == ScrollBars.None)
            {
                TextBox.Height += 14;
            }
            else
            {
                TextBox.Height -= 14;
            }
        }

        private void LblPlaceholder_MouseEnter(object sender, EventArgs e)
        {
            ChangeCursor();
        }

        private void PnlBg_Click(object sender, EventArgs e)
        {
            TextBox.Focus();
        }

        private void LblSubtitle_Click(object sender, EventArgs e)
        {
            TextBox.Focus();
        }

        private void LblPlaceholder_Click(object sender, EventArgs e)
        {
            TextBox.Focus();
        }

        private void TextBox_Enter(object sender, EventArgs e)
        {
            if (Enter.NotNull())
            {
                Enter(sender, e);
            }
        }

        private void TextBox_Leave(object sender, EventArgs e)
        {
            if (Leave.NotNull())
            {
                Leave(sender, e);
            }
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPress.NotNull())
            {
                KeyPress(sender, e);
            }

            OnKeyPress(e);
        }

        // bool prevent;
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // e.SuppressKeyPress = prevent;
            OnKeyDown(e);
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            // prevent = false;
            OnKeyUp(e);
        }

        private void TextBox_MouseUp(object sender, MouseEventArgs e)
        {
            OnMouseUp(e);
        }

        private void TextBox_GotFocus(object sender, EventArgs e)
        {
            BackColor = BorderColorFocus;
            ContentPanel.BackColor = BackgroundColorFocus;
            SubtitleLabel.BackColor = BackgroundColorFocus;

            TextBox.BackColor = BackgroundColorFocus;
            TextBox.ForeColor = TextColorFocus;

            lblPlaceholder.Visible = false;
        }

        private void TextBox_LostFocus(object sender, EventArgs e)
        {
            BackColor = BorderColor;
            ContentPanel.BackColor = BackgroundColor;
            SubtitleLabel.BackColor = BackgroundColor;

            TextBox.BackColor = BackgroundColor;
            TextBox.ForeColor = TextColor;

            lblPlaceholder.Visible = TextBox.Text.Length == 0;
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            if (!TextBox.Focused)
            {
                lblPlaceholder.Visible = TextBox.Text.Length == 0;
            }

            if (TextChanged.IsNull())
            {
                return;
            }

            ////prevent = true;

            if (PreviousTextBackup != TextBox.Text)
            {
                PreviousTextChanged = !PreviousTextChanged;

                if (PreviousTextChanged)
                {
                    PreviousTextChanged = false;
                    PreviousText = PreviousTextBackup;
                }

                PreviousTextBackup = TextBox.Text;
            }

            TextChanged(sender, e);
        }
    }
}