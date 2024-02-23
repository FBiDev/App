using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public partial class FlatCheckBox : UserControl
    {
        [DefaultValue(typeof(AutoSizeMode), "GrowAndShrink")]
        public new AutoSizeMode AutoSizeMode
        {
            get { return base.AutoSizeMode; }
            set { base.AutoSizeMode = value; }
        }

        [Category("_Colors"), DefaultValue(typeof(Color), "White")]
        public new Color BackColor
        {
            get { return pnlBgWhite.BackColor; }
            set { pnlBgWhite.BackColor = value; }
        }

        #region Properties
        protected Color _BackgroundColor = Color.White;
        [Category("_Colors"), DefaultValue(typeof(Color), "White")]
        internal Color BackgroundColor { get { return _BackgroundColor; } set { _BackgroundColor = value; } }

        protected Color _BorderColor = Color.FromArgb(213, 223, 229);
        [Category("_Colors"), DefaultValue(typeof(Color), "213, 223, 229")]
        public Color BorderColor { get { return _BorderColor; } set { _BorderColor = value; } }

        protected Color _BorderColorFocus = Color.FromArgb(108, 132, 199);
        [Category("_Colors"), DefaultValue(typeof(Color), "108, 132, 199")]
        public Color BorderColorFocus { get { return _BorderColorFocus; } set { _BorderColorFocus = value; } }

        [Category("_Colors"), DefaultValue(typeof(Color), "213, 223, 229")]
        Color BorderColorLeave { get; set; }
        #endregion

        [Category("_Data"), DefaultValue("")]
        public string TextLegend
        {
            get { return lblLegend.Text; }
            set
            {
                lblLegend.Text = value;
                AlignControl();
            }
        }

        CheckBox CheckBox { get { return chkBox; } }

        [DefaultValue(false)]
        public bool Checked { get { return CheckBox.Checked; } set { CheckBox.Checked = value; } }
        public event EventHandler CheckedChanged = delegate { };
        public new bool Focused { get { return chkBox.Focused; } }

        public FlatCheckBox()
        {
            InitializeComponent();

            CheckBox.CheckedChanged += ChkBox_CheckedChanged;
            Click += (sender, e) => PnlBgWhite_Click(null, null);

            AlignControl();
        }

        public new bool Focus()
        {
            chkBox.Focus();
            return chkBox.Focused;
        }

        public void ResetColors()
        {
            if (Focused)
                base.BackColor = BorderColorFocus;
            else
                base.BackColor = BorderColor;

            BackColor = BackgroundColor;
            BorderColorFocus = _BorderColorFocus;
            BorderColorLeave = BorderColor;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            ResetColors();
        }

        void AlignControl()
        {
            var newLocation = new Point((pnlBgWhite.Width - CheckBox.Width) / 2, CheckBox.Location.Y);
            CheckBox.Location = newLocation;

            newLocation = new Point((pnlBgWhite.Width - lblLegend.Width) / 2, lblLegend.Location.Y);
            lblLegend.Location = newLocation;
        }

        void CheckBoxBlue_SizeChanged(object sender, EventArgs e)
        {
            AlignControl();
        }

        void PnlBgWhite_Click(object sender, EventArgs e)
        {
            CheckBox.Focus();
            Checked = !CheckBox.Checked;
        }

        void PnlBgWhite_DoubleClick(object sender, EventArgs e)
        {
            PnlBgWhite_Click(null, null);
        }

        void LblLegend_Click(object sender, EventArgs e)
        {
            PnlBgWhite_Click(null, null);
        }

        void LblLegend_DoubleClick(object sender, EventArgs e)
        {
            PnlBgWhite_Click(null, null);
        }

        void PnlBgWhite_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        void ChkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckedChanged.NotNull())
            {
                CheckedChanged(sender, e);
            }

            if (CheckBox.Checked)
            {
                CheckBox.BackColor = Color.LightGreen;
            }
            else
            {
                CheckBox.BackColor = Color.LightCoral;
            }
        }

        void ChkBox_Enter(object sender, EventArgs e)
        {
            base.BackColor = BorderColorFocus;
        }

        void CheckBoxBlue_Leave(object sender, EventArgs e)
        {
            base.BackColor = BorderColorLeave;
        }

        void PnlBgWhite_MouseDown(object sender, MouseEventArgs e)
        {
            ChkBox_Enter(null, null);
        }

        void PnlBgWhite_MouseUp(object sender, MouseEventArgs e)
        {
            CheckBoxBlue_Leave(null, null);
        }

        void LblLegend_MouseDown(object sender, MouseEventArgs e)
        {
            ChkBox_Enter(null, null);
        }

        void LblLegend_MouseUp(object sender, MouseEventArgs e)
        {
            CheckBoxBlue_Leave(null, null);
        }
    }
}