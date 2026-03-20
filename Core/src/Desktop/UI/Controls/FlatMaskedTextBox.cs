using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public partial class FlatMaskedTextBox : FlatTextBoxBase
    {
        private TextMask _mask;
        private TextAlignTypes _textAlign;
        private Color _backgroundColorFocus = Color.FromArgb(252, 245, 237);
        private Color _textColor = Color.FromArgb(47, 47, 47);
        private Color _textColorFocus = Color.FromArgb(47, 47, 47);
        private Color _placeholderColor = Color.FromArgb(178, 178, 178);

        public FlatMaskedTextBox()
        {
            InitializeComponent();

            MainTextBox.Culture = System.Globalization.CultureInfo.InvariantCulture;

            PlaceholderLabel.MouseEnter += PlaceholderLabel_MouseEnter;

            ContentPanel.Click += BackgroundPanel_Click;
            SubtitleLabel.Click += SubtitleLabel_Click;
            PlaceholderLabel.Click += LblPlaceholder_Click;

            MainTextBox.KeyPress += TextBox_KeyPress;
            MainTextBox.KeyDown += TextBox_KeyDown;

            MainTextBox.GotFocus += TextBox_GotFocus;
            MainTextBox.LostFocus += TextBox_LostFocus;
            MainTextBox.TextChanged += TextBox_TextChanged;

            ActionButton.Click += ActionButton_Click;
            ActionButton.MouseEnter += ActionButton_MouseEnter;
            ActionButton.MouseLeave += ActionButton_MouseLeave;
            ActionButton.TabStop = true;

            PickDateTime = new DateTimePicker
            {
                Visible = false,
                Size = new Size(0, 0),
                Location = new Point(9, 8),
                Format = DateTimePickerFormat.Short,
                TabStop = false
            };
            PickDateTime.ValueChanged += PickDateTime_ValueChanged;
            PickDateTime.CloseUp += PickDateTime_CloseUp;
            ContentPanel.Controls.Add(PickDateTime);

            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            Size = new Size(206, 34);
            MaximumSize = new Size(1500, 34);
            MinimumSize = new Size(58, 34);

            MaxLength = 50;
            DecimalPlaces = 2;
        }

        public event EventHandler ClickButton;

        public new event EventHandler TextChanged;

        public enum TextAlignTypes
        {
            Left, Right, Center
        }

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
            get { return MainTextBox.Text; }
            set { MainTextBox.Text = value; }
        }

        [Category("_Colors"), DefaultValue(typeof(string), "")]
        public string PlaceholderText
        {
            get { return PlaceholderLabel.Text; }
            set { PlaceholderLabel.Text = value; }
        }

        [Category("_Colors"), DefaultValue(typeof(Color), "178, 178, 178")]
        public Color PlaceholderColor
        {
            get { return _placeholderColor; }
            set { _placeholderColor = value; }
        }

        [DefaultValue(false)]
        public bool ReadOnly
        {
            get { return MainTextBox.ReadOnly; }
            set { MainTextBox.ReadOnly = value; }
        }

        public new bool Enabled
        {
            get
            {
                return MainTextBox.Enabled;
            }

            set
            {
                MainTextBox.Enabled = value;
                ActionButton.Enabled = value;
                TabStop = value;
                ChangeCursor();
            }
        }

        public override bool Focused
        {
            get { return MainTextBox.Focused; }
        }

        [DefaultValue(50)]
        public int MaxLength
        {
            get
            {
                return MainTextBox.MxLength;
            }

            set
            {
                MainTextBox.MxLength = value;
                MainTextBox.SetMaxLength(value);
            }
        }

        [DefaultValue(2)]
        public int DecimalPlaces { get; set; }

        public bool MaskCompleted
        {
            get { return MainTextBox.MaskCompleted; }
        }

        [Category("_Properties")]
        [DefaultValue(typeof(string), "")]
        public TextMask Mask
        {
            get
            {
                return _mask;
            }

            set
            {
                _mask = value;

                MainTextBox.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                MainTextBox.Mask = string.Empty;
                MainTextBox.PromptChar = '_';

                switch (_mask)
                {
                    case TextMask.None:
                        PlaceholderLabel.Text = string.Empty;
                        break;
                    case TextMask.CPF:
                        MainTextBox.Mask = "000.000.000-00";
                        PlaceholderLabel.Text = "000.000.000-00";
                        break;
                    case TextMask.CNPJ:
                        MainTextBox.Mask = "00.000.000/0000-00";
                        PlaceholderLabel.Text = "00.000.000/0000-00";
                        break;
                    case TextMask.DATA:
                        MainTextBox.Mask = "00/00/0000";
                        PlaceholderLabel.Text = "00/00/0000";
                        MainTextBox.TextMaskFormat = MaskFormat.IncludeLiterals;
                        ActionButton.Visible = true;
                        break;
                    case TextMask.HORA:
                        MainTextBox.Mask = "00:00";
                        PlaceholderLabel.Text = "00:00";
                        break;
                    case TextMask.CELULAR:
                        MainTextBox.Mask = "(00) 00000-0009";
                        PlaceholderLabel.Text = "(00) 00000-0000";
                        break;
                    case TextMask.NUMERO:
                        PlaceholderLabel.Text = "000";
                        MaxLength = 1000;
                        break;
                    case TextMask.DINHEIRO:
                        PlaceholderLabel.Text = LanguageManager.CurrencySymbol + " 000" + LanguageManager.CurrencyDecimalSeparator + "00";
                        break;
                    case TextMask.CEP:
                        MainTextBox.Mask = "00000-000";
                        PlaceholderLabel.Text = "00000-000";
                        break;
                }
            }
        }
        #endregion

        public new string Text
        {
            get { return MainTextBox.Text; }
            set { MainTextBox.Text = value; }
        }

        [DefaultValue(0)]
        public int SelectionStart
        {
            get { return MainTextBox.SelectionStart; }
            set { MainTextBox.SelectionStart = value; }
        }

        [DefaultValue(0)]
        public int SelectionLength
        {
            get { return MainTextBox.SelectionLength; }
            set { MainTextBox.SelectionLength = value; }
        }

        public TextAlignTypes TextAlign
        {
            get
            {
                return _textAlign;
            }

            set
            {
                _textAlign = value;
                switch (_textAlign)
                {
                    case TextAlignTypes.Left:
                        PlaceholderLabel.TextAlign = ContentAlignment.MiddleLeft;
                        MainTextBox.TextAlign = HorizontalAlignment.Left;
                        break;
                    case TextAlignTypes.Right:
                        PlaceholderLabel.TextAlign = ContentAlignment.MiddleRight;
                        MainTextBox.TextAlign = HorizontalAlignment.Right;
                        break;
                    case TextAlignTypes.Center:
                        PlaceholderLabel.TextAlign = ContentAlignment.MiddleCenter;
                        MainTextBox.TextAlign = HorizontalAlignment.Center;
                        break;
                }
            }
        }

        private DateTimePicker PickDateTime { get; set; }

        public new bool Focus()
        {
            MainTextBox.Focus();
            return MainTextBox.Focused;
        }

        public override string ToString()
        {
            return Name + ": " + Formatter.Quote(MainTextBox.Text);
        }

        public void ResetColors()
        {
            BackColor = BorderColor;
            ContentPanel.BackColor = BackgroundColor;

            SubtitleLabel.BackColor = BackgroundColor;
            SubtitleLabel.ForeColor = LabelTextColor;

            MainTextBox.BackColor = BackgroundColor;
            MainTextBox.ForeColor = TextColor;

            PlaceholderLabel.BackColor = BackgroundColor;
            PlaceholderLabel.ForeColor = PlaceholderColor;

            ActionButton.ForeColor = BackgroundColor;

            if (Mask == TextMask.DINHEIRO && Text.Length > 0)
            {
                PlaceholderLabel.ForeColor = TextColor;
            }
        }

        public void ActionButton_SetImage(Image img)
        {
            ActionButton.BackgroundImage = img;
        }

        public void TextBox_LostFocus(object sender, EventArgs e)
        {
            BackColor = BorderColor;
            ContentPanel.BackColor = BackgroundColor;
            SubtitleLabel.BackColor = BackgroundColor;

            MainTextBox.BackColor = BackgroundColor;
            MainTextBox.ForeColor = TextColor;

            ActionButton.ForeColor = BackgroundColor;

            TextBox_TextChanged(null, null);

            if (Mask == TextMask.DINHEIRO)
            {
                PlaceholderLabel.BackColor = Color.Transparent;

                var isEmpty = !MainTextBox.Focused && (MainTextBox.Text.Length == 0);
                if (isEmpty)
                {
                    PlaceholderLabel.Text = LanguageManager.CurrencySymbol + " 000" + LanguageManager.CurrencyDecimalSeparator + "00";
                    PlaceholderLabel.ForeColor = PlaceholderColor;
                    PlaceholderLabel.Width = ContentPanel.Width - 10;
                    PlaceholderLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                }
                else
                {
                    MainTextBox.Text = MainTextBox.Text.ToMoney();
                }
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            ResetColors();

            if (Mask == TextMask.DINHEIRO)
            {
                PlaceholderLabel.Text = LanguageManager.CurrencySymbol + " 000" + LanguageManager.CurrencyDecimalSeparator + "00";
                PlaceholderLabel.Location = new Point(PlaceholderLabel.Location.X - 1, PlaceholderLabel.Location.Y);
            }
        }

        private void PickDateTime_ValueChanged(object sender, EventArgs e)
        {
            var date = Cast.ToDateTimeNull(PickDateTime.Text);
            if (date is DateTime)
            {
                MainTextBox.Text = PickDateTime.Text;
            }
        }

        private void PickDateTime_CloseUp(object sender, EventArgs e)
        {
            if (MainTextBox.MaskCompleted == false)
            {
                MainTextBox.Text = PickDateTime.Text;
            }

            ActionButton.Focus();
        }

        private void ActionButton_Click(object sender, EventArgs e)
        {
            if (Mask == TextMask.DATA)
            {
                var date = Cast.ToDateTimeNull(MainTextBox.Text);

                if (MainTextBox.MaskCompleted)
                {
                    if (PickDateTime.TryParse(date))
                    {
                        PickDateTime.Open();
                    }
                }
                else
                {
                    PickDateTime.Open();
                }
            }

            if (ClickButton == null || ActionButton.Visible == false)
            {
                return;
            }

            ClickButton(sender, e);
        }

        private void ActionButton_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void ActionButton_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void PlaceholderLabel_MouseEnter(object sender, EventArgs e)
        {
            ChangeCursor();
        }

        private void BackgroundPanel_Click(object sender, EventArgs e)
        {
            MainTextBox.Focus();
        }

        private void SubtitleLabel_Click(object sender, EventArgs e)
        {
            MainTextBox.Focus();
        }

        private void LblPlaceholder_Click(object sender, EventArgs e)
        {
            MainTextBox.Focus();
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnKeyPress(e);
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(e);
        }

        private void TextBox_GotFocus(object sender, EventArgs e)
        {
            BackColor = BorderColorFocus;
            ContentPanel.BackColor = BackgroundColorFocus;
            SubtitleLabel.BackColor = BackgroundColorFocus;

            MainTextBox.BackColor = BackgroundColorFocus;
            MainTextBox.ForeColor = TextColorFocus;

            ActionButton.ForeColor = BackgroundColorFocus;

            TextBox_TextChanged(null, null);
            OnGotFocus(e);
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            string txt = MainTextBox.Text;

            if (Mask == TextMask.DATA)
            {
                txt = txt.Replace("/", string.Empty).Trim();
            }
            else if (_mask == TextMask.NUMERO || Mask == TextMask.DINHEIRO)
            {
                string newText = string.Empty;
                int selectionStart = MainTextBox.SelectionStart;

                var decimalSeparator = true;
                int decimalIndex = 999;

                for (int i = 0; i < MainTextBox.Text.Length; i++)
                {
                    var c = MainTextBox.Text[i];

                    int result;
                    if (int.TryParse(c.ToString(), out result) ||
                        (Mask == TextMask.DINHEIRO && c.ToString() == LanguageManager.CurrencyDecimalSeparator && decimalSeparator))
                    {
                        if (c.ToString() == LanguageManager.CurrencyDecimalSeparator)
                        {
                            decimalSeparator = false;
                            decimalIndex = i;
                        }

                        if (i <= (decimalIndex + DecimalPlaces))
                        {
                            newText = newText.Insert(newText.Length, c.ToString());
                        }
                    }
                }

                if (newText.Length > MaxLength)
                {
                    newText = newText.Substring(0, MaxLength);
                }

                MainTextBox.Text = newText;
                MainTextBox.SelectionStart = selectionStart;
            }

            var isEmpty = !MainTextBox.Focused && (MainTextBox.Text.Length == 0 || txt.Length == 0);
            PlaceholderLabel.Visible = isEmpty;

            if (Mask == TextMask.DINHEIRO)
            {
                PlaceholderLabel.Visible = true;
                PlaceholderLabel.BackColor = Color.Transparent;

                if (isEmpty == false)
                {
                    PlaceholderLabel.Text = LanguageManager.CurrencySymbol;
                    PlaceholderLabel.ForeColor = MainTextBox.ForeColor;
                    PlaceholderLabel.Width = PlaceholderLabel.PreferredWidth;
                    PlaceholderLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left;

                    MainTextBox.Dock = DockStyle.None;
                    MainTextBox.Location = new Point(PlaceholderLabel.Width + 4, 0);
                    MainTextBox.Size = new Size(Size.Width - 32, MainTextBox.Size.Height);
                }
            }

            if (TextChanged.IsNull())
            {
                return;
            }

            if (Mask == TextMask.None && MainTextBox.TxtChanged == false)
            {
                return;
            }

            TextChanged(sender, e);
        }
    }
}