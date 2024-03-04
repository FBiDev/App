using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public partial class FlatMaskedTextBox : FlatTextBoxBase
    {
        #region Defaults
        [DefaultValue(typeof(Size), "1500, 34")]
        public new Size MaximumSize
        {
            get { return base.MaximumSize; }
            set { base.MaximumSize = value; }
        }

        [DefaultValue(typeof(Size), "64, 34")]
        public new Size MinimumSize
        {
            get { return base.MinimumSize; }
            set { base.MinimumSize = value; }
        }

        [DefaultValue(typeof(AnchorStyles), "Top, Left, Right")]
        public new AnchorStyles Anchor
        {
            get { return base.Anchor; }
            set { base.Anchor = value; }
        }
        #endregion

        #region Properties
        [Category("_Properties")]
        protected Color _BackgroundColorFocus = Color.FromArgb(252, 245, 237);
        [Category("_Colors"), DefaultValue(typeof(Color), "252, 245, 237")]
        public Color BackgroundColorFocus { get { return _BackgroundColorFocus; } set { _BackgroundColorFocus = value; } }

        protected Color _TextColor = Color.FromArgb(47, 47, 47);
        [Category("_Colors"), DefaultValue(typeof(Color), "47, 47, 47")]
        public Color TextColor { get { return _TextColor; } set { _TextColor = value; } }

        protected Color _TextColorFocus = Color.FromArgb(47, 47, 47);
        [Category("_Colors"), DefaultValue(typeof(Color), "47, 47, 47")]
        public Color TextColorFocus { get { return _TextColorFocus; } set { _TextColorFocus = value; } }

        [Category("_Colors"), DefaultValue(typeof(string), "")]
        public string DefaultText { get { return TextBox.Text; } set { TextBox.Text = value; } }

        [Category("_Colors"), DefaultValue(typeof(string), "")]
        public string PlaceholderText { get { return lblPlaceholder.Text; } set { lblPlaceholder.Text = value; } }

        protected Color _PlaceholderColor = Color.FromArgb(178, 178, 178);
        [Category("_Colors"), DefaultValue(typeof(Color), "178, 178, 178")]
        public Color PlaceholderColor { get { return _PlaceholderColor; } set { _PlaceholderColor = value; } }

        [DefaultValue(false)]
        public bool ReadOnly { get { return TextBox.ReadOnly; } set { TextBox.ReadOnly = value; } }

        public new bool Enabled
        {
            get { return TextBox.Enabled; }
            set
            {
                TextBox.Enabled = value;
                btnAction.Enabled = value;
                TabStop = value;
                ChangeCursor();
            }
        }

        int MaxLength;
        TextMask _Mask_;

        public bool MaskCompleted { get { return txtMain.MaskCompleted; } }

        [DefaultValue(2)]
        public int DecimalPlaces { get; set; }

        [Category("_Properties")]
        [DefaultValue(typeof(string), "")]
        public TextMask Mask
        {
            get { return _Mask_; }
            set
            {
                _Mask_ = value;

                TextBox.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                txtMain.Mask = "";
                txtMain.PromptChar = '_';

                MaxLength = 50;

                switch (_Mask_)
                {
                    case TextMask.None:
                        lblPlaceholder.Text = "";
                        break;
                    case TextMask.CPF:
                        txtMain.Mask = "000.000.000-00";
                        lblPlaceholder.Text = "000.000.000-00";
                        break;
                    case TextMask.CNPJ:
                        txtMain.Mask = "00.000.000/0000-00";
                        lblPlaceholder.Text = "00.000.000/0000-00";
                        break;
                    case TextMask.DATA:
                        txtMain.Mask = "00/00/0000";
                        lblPlaceholder.Text = "00/00/0000";
                        txtMain.TextMaskFormat = MaskFormat.IncludeLiterals;
                        btnAction.Visible = true;
                        break;
                    case TextMask.HORA:
                        txtMain.Mask = "00:00";
                        lblPlaceholder.Text = "00:00";
                        break;
                    case TextMask.CELULAR:
                        txtMain.Mask = "(00) 00000-0009";
                        lblPlaceholder.Text = "(00) 00000-0000";
                        break;
                    case TextMask.NUMERO:
                        lblPlaceholder.Text = "000";
                        MaxLength = 1000;
                        break;
                    case TextMask.DINHEIRO:
                        lblPlaceholder.Text = LanguageManager.CurrencySymbol + " 000" + LanguageManager.CurrencyDecimalSeparator + "00";
                        break;
                    case TextMask.CEP:
                        txtMain.Mask = "00000-000";
                        lblPlaceholder.Text = "00000-000";
                        break;
                }
            }
        }
        #endregion

        MaskedTextBox TextBox { get { return txtMain; } }
        public new string Text { get { return TextBox.Text; } set { TextBox.Text = value; } }

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

        public enum TextAlignTypes { Left, Right, Center }
        TextAlignTypes _TextAlign;
        public TextAlignTypes TextAlign
        {
            get { return _TextAlign; }
            set
            {
                _TextAlign = value;
                switch (_TextAlign)
                {
                    case TextAlignTypes.Left:
                        lblPlaceholder.TextAlign = ContentAlignment.MiddleLeft;
                        txtMain.TextAlign = HorizontalAlignment.Left;
                        break;
                    case TextAlignTypes.Right:
                        lblPlaceholder.TextAlign = ContentAlignment.MiddleRight;
                        txtMain.TextAlign = HorizontalAlignment.Right;
                        break;
                    case TextAlignTypes.Center:
                        lblPlaceholder.TextAlign = ContentAlignment.MiddleCenter;
                        txtMain.TextAlign = HorizontalAlignment.Center;
                        break;
                }
            }
        }

        DateTimePicker dtPicker { get; set; }
        public event EventHandler ClickButton;
        public new event EventHandler TextChanged;
        public override bool Focused { get { return txtMain.Focused; } }

        public FlatMaskedTextBox()
        {
            InitializeComponent();

            TextBox.Culture = System.Globalization.CultureInfo.InvariantCulture;

            lblPlaceholder.MouseEnter += LblPlaceholder_MouseEnter;

            pnlContent.Click += PnlBg_Click;
            lblSubtitle.Click += LblSubtitle_Click;
            lblPlaceholder.Click += LblPlaceholder_Click;

            TextBox.KeyPress += TextBox_KeyPress;
            TextBox.KeyDown += TextBox_KeyDown;

            TextBox.GotFocus += TextBox_GotFocus;
            TextBox.LostFocus += TextBox_LostFocus;
            TextBox.TextChanged += TextBox_TextChanged;

            btnAction.Click += btnAction_Click;
            btnAction.MouseEnter += btnAction_MouseEnter;
            btnAction.MouseLeave += btnAction_MouseLeave;
            btnAction.TabStop = true;

            dtPicker = new DateTimePicker
            {
                Visible = false,
                Size = new Size(0, 0),
                Location = new Point(9, 8),
                Format = DateTimePickerFormat.Short,
                TabStop = false
            };
            dtPicker.ValueChanged += dtPicker_ValueChanged;
            dtPicker.CloseUp += dtPicker_CloseUp;
            pnlContent.Controls.Add(dtPicker);

            Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
            Size = new Size(206, 34);
            MaximumSize = new Size(1500, 34);
            MinimumSize = new Size(64, 34);

            DecimalPlaces = 2;
        }

        public new bool Focus()
        {
            txtMain.Focus();
            return txtMain.Focused;
        }

        public override string ToString()
        {
            return Name + ": \"" + txtMain.Text + "\"";
        }

        public void ResetColors()
        {
            BackColor = BorderColor;
            pnlContent.BackColor = BackgroundColor;

            lblSubtitle.BackColor = BackgroundColor;
            lblSubtitle.ForeColor = LabelTextColor;

            TextBox.BackColor = BackgroundColor;
            TextBox.ForeColor = TextColor;

            lblPlaceholder.BackColor = BackgroundColor;
            lblPlaceholder.ForeColor = PlaceholderColor;

            btnAction.ForeColor = BackgroundColor;

            if (Mask == TextMask.DINHEIRO && Text.Length > 0)
                lblPlaceholder.ForeColor = TextColor;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            ResetColors();

            if (Mask == TextMask.DINHEIRO)
            {
                lblPlaceholder.Text = LanguageManager.CurrencySymbol + " 000" + LanguageManager.CurrencyDecimalSeparator + "00";
                lblPlaceholder.Location = new Point(lblPlaceholder.Location.X - 1, lblPlaceholder.Location.Y);
            }
        }

        void dtPicker_ValueChanged(object sender, EventArgs e)
        {
            var date = Cast.ToDateTimeNull(dtPicker.Text);
            if (date is DateTime)
            {
                txtMain.Text = dtPicker.Text;
            }
        }

        void dtPicker_CloseUp(object sender, EventArgs e)
        {
            if (txtMain.MaskCompleted == false)
            {
                txtMain.Text = dtPicker.Text;
            }
            btnAction.Focus();
        }

        public void btnAction_SetImage(Image img)
        {
            btnAction.BackgroundImage = img;
        }

        void btnAction_Click(object sender, EventArgs e)
        {
            if (Mask == TextMask.DATA)
            {
                var date = Cast.ToDateTimeNull(txtMain.Text);
                if (txtMain.MaskCompleted)
                {
                    if (dtPicker.TryParse(date))
                        dtPicker.Open();
                }
                else
                    dtPicker.Open();
            }
            if (ClickButton == null || btnAction.Visible == false) return;
            ClickButton(sender, e);
        }

        void btnAction_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        void btnAction_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        protected void LblPlaceholder_MouseEnter(object sender, EventArgs e)
        {
            ChangeCursor();
        }

        protected void PnlBg_Click(object sender, EventArgs e)
        {
            TextBox.Focus();
        }

        protected void LblSubtitle_Click(object sender, EventArgs e)
        {
            TextBox.Focus();
        }
        protected void LblPlaceholder_Click(object sender, EventArgs e)
        {
            TextBox.Focus();
        }

        void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnKeyPress(e);
        }

        void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(e);
        }

        void TextBox_GotFocus(object sender, EventArgs e)
        {
            BackColor = BorderColorFocus;
            pnlContent.BackColor = BackgroundColorFocus;
            lblSubtitle.BackColor = BackgroundColorFocus;

            TextBox.BackColor = BackgroundColorFocus;
            TextBox.ForeColor = TextColorFocus;

            btnAction.ForeColor = BackgroundColorFocus;

            TextBox_TextChanged(null, null);
            OnGotFocus(e);
        }

        public void TextBox_LostFocus(object sender, EventArgs e)
        {
            BackColor = BorderColor;
            pnlContent.BackColor = BackgroundColor;
            lblSubtitle.BackColor = BackgroundColor;

            TextBox.BackColor = BackgroundColor;
            TextBox.ForeColor = TextColor;

            btnAction.ForeColor = BackgroundColor;

            TextBox_TextChanged(null, null);

            if (Mask == TextMask.DINHEIRO)
            {
                lblPlaceholder.BackColor = Color.Transparent;

                var isEmpty = !TextBox.Focused && (TextBox.Text.Length == 0);
                if (isEmpty)
                {
                    lblPlaceholder.Text = LanguageManager.CurrencySymbol + " 000" + LanguageManager.CurrencyDecimalSeparator + "00";
                    lblPlaceholder.ForeColor = PlaceholderColor;
                    lblPlaceholder.Width = pnlContent.Width - 10;
                    lblPlaceholder.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                }
                else
                {
                    txtMain.Text = txtMain.Text.ToMoney();
                }
            }
        }

        protected void TextBox_TextChanged(object sender, EventArgs e)
        {
            string txt = txtMain.Text;

            if (Mask == TextMask.DATA)
            {
                txt = txt.Replace("/", "").Trim();
            }
            else if (_Mask_ == TextMask.NUMERO || Mask == TextMask.DINHEIRO)
            {
                string TextNew = "";
                int SelStart = txtMain.SelectionStart;

                var decimalSeparator = true;
                int decimalIndex = 999;

                for (int i = 0; i < txtMain.Text.Length; i++)
                {
                    var c = txtMain.Text[i];

                    int result;
                    if (int.TryParse(c.ToString(), out result)
                        || Mask == TextMask.DINHEIRO
                        && c.ToString() == LanguageManager.CurrencyDecimalSeparator && decimalSeparator)
                    {
                        if (c.ToString() == LanguageManager.CurrencyDecimalSeparator)
                        {
                            decimalSeparator = false;
                            decimalIndex = i;
                        }

                        if (i <= (decimalIndex + DecimalPlaces))
                            TextNew = TextNew.Insert(TextNew.Length, c.ToString());
                    }
                }

                if (TextNew.Length > MaxLength)
                {
                    TextNew = TextNew.Substring(0, MaxLength);
                }

                txtMain.Text = TextNew;
                txtMain.SelectionStart = SelStart;
            }

            var isEmpty = !TextBox.Focused && (TextBox.Text.Length == 0 || txt.Length == 0);
            lblPlaceholder.Visible = isEmpty;

            if (Mask == TextMask.DINHEIRO)
            {
                lblPlaceholder.Visible = true;
                lblPlaceholder.BackColor = Color.Transparent;

                if (isEmpty == false)
                {
                    lblPlaceholder.Text = LanguageManager.CurrencySymbol;
                    lblPlaceholder.ForeColor = txtMain.ForeColor;
                    lblPlaceholder.Width = lblPlaceholder.PreferredWidth;
                    lblPlaceholder.Anchor = AnchorStyles.Top | AnchorStyles.Left;

                    txtMain.Dock = DockStyle.None;
                    txtMain.Location = new Point(lblPlaceholder.Width + 4, 0);
                    txtMain.Size = new Size(Size.Width - 32, txtMain.Size.Height);
                }
            }

            if (TextChanged.IsNull()) { return; }
            TextChanged(sender, e);
        }
    }
}