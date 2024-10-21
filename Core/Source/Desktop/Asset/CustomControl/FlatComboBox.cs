using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using App.Core.Properties;

namespace App.Core.Desktop
{
    public partial class FlatComboBox : UserControl
    {
        private Color _borderColor = Color.FromArgb(213, 223, 229);
        private Color _borderColorFocus = Color.FromArgb(108, 132, 199);
        private Color _backgroundColor = Color.White;
        private Color _itemBackColorFocus = Color.FromArgb(108, 132, 199);
        private Color _itemTextColor = Color.FromArgb(47, 47, 47);
        private Color _itemTextColorFocus = Color.White;
        private Color _labelTextColor = Color.FromArgb(108, 132, 199);

        public FlatComboBox()
        {
            InitializeComponent();

            pnlBg.Click += BackgroundPanel_Click;
            lblSubtitle.Click += BackgroundPanel_Click;
            picDrop.Click += BackgroundPanel_Click;

            Combo.GotFocus += Combo_GotFocus;
            Combo.LostFocus += Combo_LostFocus;
            Combo.DrawItem += Combo_DrawItem;
            Combo.DropDown += Combo_DropDown;
            Combo.DropDownClosed += Combo_DropDownClosed;
            Combo.MouseWheel += Combo_MouseWheel;
            Combo.SelectedIndexChanged += Combo_SelectedIndexChanged;
            Combo.SelectionChangeCommitted += Combo_SelectionChangeCommitted;
            Combo.KeyDown += Combo_KeyDown;

            Cursor = Cursors.Hand;
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            Size = new Size(206, 34);
            MaximumSize = new Size(1500, 34);
            MinimumSize = new Size(64, 34);
            Margin = new Padding(2);

            Combo.Sorted = false;
        }

        public event EventHandler SelectedIndexChanged;

        public event EventHandler SelectionChangeCommitted;

        #region Defaults
        [DefaultValue(typeof(Padding), "2, 2, 2, 2")]
        public new Padding Margin
        {
            get { return base.Margin; }
            set { base.Margin = value; }
        }

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

        [DefaultValue(typeof(Cursor), "Hand")]
        public new Cursor Cursor
        {
            get { return base.Cursor; }
            set { base.Cursor = value; }
        }
        #endregion

        #region Properties
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
        public Color ItemBackColorFocus
        {
            get { return _itemBackColorFocus; }
            set { _itemBackColorFocus = value; }
        }

        [Category("_Colors"), DefaultValue(typeof(Color), "47, 47, 47")]
        public Color ItemTextColor
        {
            get { return _itemTextColor; }
            set { _itemTextColor = value; }
        }

        [Category("_Colors"), DefaultValue(typeof(Color), "White")]
        public Color ItemTextColorFocus
        {
            get { return _itemTextColorFocus; }
            set { _itemTextColorFocus = value; }
        }

        [Category("_Colors"), DefaultValue(typeof(Color), "108, 132, 199")]
        public Color LabelTextColor
        {
            get { return _labelTextColor; }
            set { _labelTextColor = value; }
        }

        [Category("_Colors")]
        public string LabelText
        {
            get { return lblSubtitle.Text; }
            set { lblSubtitle.Text = value; }
        }
        #endregion

        #region ComboBox
        public FlatComboBoxNew Combo
        {
            get { return MainCombo; }
        }

        public new string Text
        {
            get { return MainCombo.Text; }
            set { MainCombo.Text = value; }
        }

        [DefaultValue("")]
        public string DisplayMember
        {
            get { return MainCombo.DisplayMember; }
            set { MainCombo.DisplayMember = value; }
        }

        [DefaultValue("")]
        public string ValueMember
        {
            get { return MainCombo.ValueMember; }
            set { MainCombo.ValueMember = value; }
        }

        [DefaultValue(null)]
        public object DataSource
        {
            get
            {
                return MainCombo.DataSource;
            }

            set
            {
                MainCombo.BindingContext = new BindingContext();
                MainCombo.DataSource = null;
                MainCombo.Items.Clear();
                MainCombo.DataSource = value;
                ResetIndex();
            }
        }

        [DefaultValue(-1)]
        public int SelectedIndex
        {
            get
            {
                return MainCombo.SelectedIndex;
            }

            set
            {
                if (MainCombo.Items.Count > 0 && value <= MainCombo.Items.Count)
                {
                    MainCombo.SelectedIndex = value;
                }
            }
        }

        [DefaultValue("")]
        public object SelectedValue
        {
            get
            {
                if (MainCombo.SelectedValue.NotNull())
                {
                    return MainCombo.SelectedValue;
                }

                return string.Empty;
            }

            set
            {
                if (value.NotNull() && !value.ToString().Equals(string.Empty))
                {
                    MainCombo.SelectedValue = value;
                }
                else
                {
                    ResetIndex();
                }
            }
        }

        [DefaultValue("")]
        public string SelectedText
        {
            get
            {
                if (MainCombo.SelectedValue.NotNull())
                {
                    return MainCombo.Text;
                }

                return string.Empty;
            }

            set
            {
                if (value.NotNull() && !value.Equals(string.Empty))
                {
                    SelectedIndex = MainCombo.FindStringExact(value);
                }
                else
                {
                    ResetIndex();
                }
            }
        }

        public int SelectedValueInt
        {
            get { return Cast.ToInt(MainCombo.SelectedValue); }
        }

        [DefaultValue(null)]
        public object SelectedItem
        {
            get { return MainCombo.SelectedItem; }
            set { MainCombo.SelectedItem = value; }
        }

        public new bool Enabled
        {
            get
            {
                return Combo.Enabled;
            }

            set
            {
                Combo.Enabled = value;
                TabStop = value;
                SetStyle(ControlStyles.Selectable, value);
                ChangeCursor();
            }
        }

        public T SelectedObject<T>() where T : new()
        {
            return Cast.ToObject<T>(MainCombo.SelectedItem);
        }
        #endregion

        public override string ToString()
        {
            return Name;
        }

        public void ResetColors()
        {
            pnlBorder.BackColor = BorderColor;

            BackColor = BackgroundColor;
            pnlBg.BackColor = BackgroundColor;
            lblSubtitle.BackColor = BackgroundColor;
            lblSubtitle.ForeColor = LabelTextColor;

            Combo.BackColor = BackgroundColor;
            Combo.ForeColor = ItemTextColor;
        }

        public void Reload<T>(ListBind<T> listSource)
        {
            object previousValue = SelectedValue;

            DataSource = listSource;
            SelectedValue = previousValue;
        }

        public void Reload<T>(List<T> listSource)
        {
            object previousValue = SelectedValue;

            DataSource = listSource;
            SelectedValue = previousValue;
        }

        public void ResetIndex()
        {
            Combo.ResetIndex();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            ResetColors();
        }

        protected void ChangeCursor()
        {
            if (TabStop)
            {
                Cursor = Cursors.Hand;
            }
            else
            {
                Cursor = Cursors.No;
            }
        }

        private void BackgroundPanel_Click(object sender, EventArgs e)
        {
            if (Enabled == false)
            {
                return;
            }

            Combo.Focus();
            Combo.DroppedDown = true;
        }

        private void Combo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Space)
            {
                BackgroundPanel_Click(null, null);
            }
        }

        private void Combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedIndexChanged.NotNull())
            {
                SelectedIndexChanged(sender, e);
            }
        }

        private void Combo_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (SelectionChangeCommitted.NotNull())
            {
                SelectionChangeCommitted(sender, e);
            }
        }

        private void Combo_GotFocus(object sender, EventArgs e)
        {
            pnlBorder.BackColor = BorderColorFocus;
            picDrop.BackgroundImage = Resources.img_drop_arrow_focus;
        }

        private void Combo_LostFocus(object sender, EventArgs e)
        {
            pnlBorder.BackColor = BorderColor;
            picDrop.BackgroundImage = Resources.img_drop_arrow;
        }

        private void Combo_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = false;
        }

        private void Combo_DropDown(object sender, EventArgs e)
        {
        }

        private void Combo_DropDownClosed(object sender, EventArgs e)
        {
        }

        private void Combo_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1)
            {
                return;
            }

            ////BackgroundColor
            ////var combo = sender as ComboBox;
            ////combo.BackColor = Color.BlanchedAlmond;

            var textColorNormal = new SolidBrush(ItemTextColor);
            var textColorFocus = new SolidBrush(ItemTextColorFocus);

            SolidBrush fontColor = textColorNormal;

            var backColorNormal = new SolidBrush(BackgroundColor);
            var backColorFocus = new SolidBrush(ItemBackColorFocus);
            ////Color.BlueViolet

            ////int DrawYCenter = (int)Math.Floor((double)((((Combo.ItemHeight - 1) - (int)Combo.Font.Size) / 2) - 3));
            ////int DrawY = DrawYCenter;

            e.Graphics.FillRectangle(backColorFocus, e.Bounds);

            if (Combo.DroppedDown)
            {
                // e.DrawBackground();

                // user mouse is hovering over this drop-down item
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    // ItemSelected
                    fontColor = textColorFocus;

                    // BackColorSelected
                    e.Graphics.FillRectangle(backColorFocus, e.Bounds);
                }
                else
                {
                    // ItemNotSelected
                    // BackgroundColor
                    e.Graphics.FillRectangle(backColorNormal, e.Bounds);
                }
            }
            else
            {
                // DropDownClosed
                e.Graphics.FillRectangle(backColorNormal, e.Bounds);
            }

            // draw text strings
            // e.Graphics.DrawString(Combo.GetItemText(Combo.Items[e.Index]), e.Font, fontColor, new Point(e.Bounds.X + 5, e.Bounds.Y));
            // Better Color Render
            TextRenderer.DrawText(
                e.Graphics,
                Combo.GetItemText(Combo.Items[e.Index]),
                e.Font,
                new Point(e.Bounds.X + 4, e.Bounds.Y + 1),
                fontColor.Color,
                Color.Transparent,
                TextFormatFlags.TextBoxControl);

            if (Enabled == false)
            {
                var rectangleAll = new Rectangle(0, 0, Size.Width, Size.Height);
                var disabledColor = new SolidBrush(Color.FromArgb(128, BackColor));
                e.Graphics.FillRectangle(disabledColor, rectangleAll);
            }
        }
    }
}