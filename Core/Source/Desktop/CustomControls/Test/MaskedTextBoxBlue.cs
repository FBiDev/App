namespace App.Core.Desktop
{
    public partial class MaskedTextBoxBlue : FlatTextBoxBase
    {
        //private int MaxLength;

        //#region Properties
        //[Category("_Data")]
        //public string _Text { get { return txtBlue.Text; } set { txtBlue.Text = value; } }

        //private TextMask _Mask_;
        //[Category("_Data")]
        //public TextMask _Mask
        //{
        //    get { return _Mask_; }
        //    set
        //    {
        //        _Mask_ = value;

        //        lblPlaceholder.TextAlign = ContentAlignment.MiddleCenter;

        //        TextBox.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
        //        txtBlue.TextAlign = HorizontalAlignment.Center;
        //        txtBlue.Mask = "";
        //        txtBlue.PromptChar = '_';

        //        MaxLength = 50;

        //        if (_Mask_ == TextMask.None)
        //        {
        //            lblPlaceholder.Text = "Placeholder";
        //        }
        //        else if (_Mask_ == TextMask.CPF)
        //        {
        //            txtBlue.Mask = "000.000.000-00";
        //            lblPlaceholder.Text = "000.000.000-00";
        //        }
        //        else if (_Mask_ == TextMask.CNPJ)
        //        {
        //            txtBlue.Mask = "00.000.000/0000-00";
        //            lblPlaceholder.Text = "00.000.000/0000-00";
        //        }
        //        else if (_Mask_ == TextMask.DATA)
        //        {
        //            txtBlue.Mask = "00/00/0000";
        //            lblPlaceholder.Text = "00/00/0000";
        //            txtBlue.TextMaskFormat = MaskFormat.IncludeLiterals;
        //        }
        //        else if (_Mask_ == TextMask.HORA)
        //        {
        //            txtBlue.Mask = "00:00";
        //            lblPlaceholder.Text = "00:00";
        //        }
        //        else if (_Mask_ == TextMask.CELULAR)
        //        {
        //            txtBlue.Mask = "(00) 00000-0009";
        //            lblPlaceholder.Text = "(00) 00000-0000";
        //        }
        //        else if (_Mask_ == TextMask.NUMERO)
        //        {
        //            lblPlaceholder.Text = "999";
        //            MaxLength = 10;
        //        }
        //        else if (_Mask_ == TextMask.DINHEIRO)
        //        {
        //            lblPlaceholder.Text = "999,00";
        //        }
        //        else if (_Mask_ == TextMask.CEP)
        //        {
        //            txtBlue.Mask = "00000-000";
        //            lblPlaceholder.Text = "80000-100";
        //        }
        //    }
        //}
        //#endregion

        //public MaskedTextBox TextBox
        //{
        //    get { return txtBlue; }
        //}

        //public MaskedTextBoxBlue()
        //{
        //    InitializeComponent();

        //    TextBox.Culture = System.Globalization.CultureInfo.InvariantCulture;

        //    TxtTextChanged += txtBlue_event;
        //}

        //// Assuming you need a custom signature for your event. If not, use an existing standard event delegate
        //public delegate void txtTextChangedDelegate(object sender, EventArgs e);

        //// Expose the event off your component
        //public event txtTextChangedDelegate TxtTextChanged;

        //private void txtBlue_KeyUp(object sender, KeyEventArgs e)
        //{
        //    if (txtBlue.MaskCompleted && txtBlue.MaskedTextProvider != null)
        //    {
        //        //txtBlue.SelectionStart = txtBlue.MaskedTextProvider.LastAssignedPosition + 1;
        //    }
        //    //apply MASK
        //    //if (_Mask == TextMask.CPF)
        //    //{
        //    //    if (txtBlue.Text.Length >= 3)
        //    //    {
        //    //        if (txtBlue.Text.Length >= 4 && txtBlue.Text[3] != '.')
        //    //        {
        //    //            txtBlue.Text = txtBlue.Text.Insert(3, ".");
        //    //            txtBlue.SelectionStart = txtBlue.Text.Length;
        //    //        }
        //    //    }

        //    //    if (txtBlue.Text.Length >= 7)
        //    //    {
        //    //        if (txtBlue.Text.Length >= 8 && txtBlue.Text[7] != '.')
        //    //        {
        //    //            txtBlue.Text = txtBlue.Text.Insert(7, ".");
        //    //            txtBlue.SelectionStart = txtBlue.Text.Length;
        //    //        }
        //    //    }

        //    //    if (txtBlue.Text.Length >= 11)
        //    //    {
        //    //        if (txtBlue.Text.Length >= 12 && txtBlue.Text[11] != '-')
        //    //        {
        //    //            txtBlue.Text = txtBlue.Text.Insert(11, "-");
        //    //            txtBlue.SelectionStart = txtBlue.Text.Length;
        //    //        }
        //    //    }
        //    //}
        //}

        //private void txtBlue_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    //if (e.KeyChar == 8 || e.KeyChar == 3 || e.KeyChar == 22 || e.KeyChar == 24 || e.KeyChar == 25 || e.KeyChar == 26)
        //    //{
        //    //    string clip = Clipboard.GetText();
        //    //    if (e.KeyChar == 22 && clip.Length > txtBlue.SelectionLength)
        //    //    {
        //    //        e.Handled = true;
        //    //        return;
        //    //    }

        //    //    e.Handled = false;
        //    //    return;
        //    //}

        //    //if (_Mask == TextMask.NUMERO)
        //    //{
        //    //    //check if a number pressed
        //    //    if (Char.IsDigit(e.KeyChar))
        //    //    {

        //    //    }
        //    //    else
        //    //    {
        //    //        e.Handled = true;
        //    //    }
        //    //}

        //    //if (e.KeyChar == 8 || e.KeyChar == 3 || e.KeyChar == 22 || e.KeyChar == 24 || e.KeyChar == 25 || e.KeyChar == 26)
        //    //{
        //    //    string clip = Clipboard.GetText();
        //    //    if (e.KeyChar == 22 && clip.Length > txtBlue.SelectionLength)
        //    //    {
        //    //        e.Handled = true;
        //    //        return;
        //    //    }

        //    //    e.Handled = false;
        //    //    return;
        //    //}

        //    ////check if a number pressed
        //    //if (Char.IsDigit(e.KeyChar))
        //    //{
        //    //    if (txtBlue.Text.Length >= 0 && txtBlue.Text.Length < 14)
        //    //    {
        //    //        if (txtBlue.Text.Length >= 11 && txtBlue.Text[3] != '.')
        //    //        {
        //    //            e.Handled = true;
        //    //        }

        //    //        if (txtBlue.Text.Length >= 12 && txtBlue.Text[7] != '.')
        //    //        {
        //    //            e.Handled = true;
        //    //        }

        //    //        if (txtBlue.Text.Length >= 13 && txtBlue.Text[11] != '-')
        //    //        {
        //    //            e.Handled = true;
        //    //        }

        //    //    }
        //    //    else
        //    //    {
        //    //        e.Handled = true;
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    e.Handled = true;
        //    //}
        //}

        //private void txtBlue_TextChanged(object sender, EventArgs e)
        //{
        //    //lblPlaceholder.Visible = !(txtBlue.Text.Length > 0);

        //    if (_Mask_ == TextMask.NUMERO)
        //    {
        //        string TextNew = "";
        //        int SelStart = txtBlue.SelectionStart;
        //        foreach (char c in txtBlue.Text)
        //        {
        //            int result;
        //            if (int.TryParse(c.ToString(), out result))
        //            {
        //                TextNew = TextNew.Insert(TextNew.Length, c.ToString());
        //            }
        //        }
        //        if (TextNew.Length > MaxLength)
        //        {
        //            TextNew = TextNew.Substring(0, MaxLength);
        //        }
        //        txtBlue.Text = TextNew;
        //        txtBlue.SelectionStart = SelStart;
        //    }

        //    TxtTextChanged(sender, e);

        //    //
        //    if (txtBlue.Focused)
        //    {
        //        lblPlaceholder.Visible = false;
        //    }
        //    else if (txtBlue.Text.Length == 0)
        //    {
        //        lblPlaceholder.Visible = true;
        //    }
        //    else
        //    {
        //        lblPlaceholder.Visible = false;
        //    }
        //}

        //private void txtBlue_event(object sender, EventArgs e)
        //{
        //}

        //#region txtBlueEvents

        //protected void pnlBgWhite_MouseLeave(object sender, EventArgs e)
        //{
        //    if (!txtBlue.Focused)
        //    {
        //        _MouseLeave();
        //    }
        //}

        //protected void pnlBgWhite_Click(object sender, EventArgs e)
        //{
        //    txtBlue.Focus();
        //}

        //protected void lblLegend_Click(object sender, EventArgs e)
        //{
        //    txtBlue.Focus();
        //}

        //protected void lblPlaceholder_Click(object sender, EventArgs e)
        //{
        //    txtBlue.Focus();
        //}

        //protected void txtBlue_Enter(object sender, EventArgs e)
        //{
        //    _MouseHover();
        //    lblPlaceholder.Visible = false;
        //}

        //protected void txtBlue_Leave(object sender, EventArgs e)
        //{
        //    _MouseLeave();

        //    string txt = txtBlue.Text;

        //    //DATA
        //    if (_Mask == TextMask.DATA)
        //    {
        //        txt = txt.Replace("/", "");
        //    }

        //    txt = txt.Trim();

        //    if (txt.Length == 0)
        //    {
        //        lblPlaceholder.Visible = true;
        //    }
        //}

        //#endregion
    }
}