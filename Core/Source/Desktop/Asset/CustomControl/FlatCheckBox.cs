using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public partial class FlatCheckBox : UserControl
    {
        private Color _backgroundColor = Color.White;
        private Color _borderColor = Color.FromArgb(213, 223, 229);
        private Color _borderColorFocus = Color.FromArgb(108, 132, 199);

        public FlatCheckBox()
        {
            InitializeComponent();

            CheckBox.CheckedChanged += CheckBox_CheckedChanged;
            Click += (sender, e) => BackgroundPanel_Click(null, null);

            AlignControl();
        }

        public event EventHandler CheckedChanged = delegate { };

        [DefaultValue(typeof(AutoSizeMode), "GrowAndShrink")]
        public new AutoSizeMode AutoSizeMode
        {
            get { return base.AutoSizeMode; }
            set { base.AutoSizeMode = value; }
        }

        [Category("_Colors"), DefaultValue(typeof(Color), "White")]
        public new Color BackColor
        {
            get { return BackgroundPanel.BackColor; }
            set { BackgroundPanel.BackColor = value; }
        }

        #region Properties

        [Category("_Colors"), DefaultValue(typeof(Color), "White")]
        public Color BackgroundColor
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; }
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

        [Category("_Colors"), DefaultValue(typeof(Color), "213, 223, 229")]
        public Color BorderColorLeave
        {
            get;
            set;
        }
        #endregion

        [Category("_Data"), DefaultValue("")]
        public string TextLegend
        {
            get
            {
                return LegendLabel.Text;
            }

            set
            {
                LegendLabel.Text = value;
                AlignControl();
            }
        }

        [DefaultValue(false)]
        public bool Checked
        {
            get { return CheckBox.Checked; }
            set { CheckBox.Checked = value; }
        }

        public new bool Focused
        {
            get { return MainCheckBox.Focused; }
        }

        private CheckBox CheckBox
        {
            get { return MainCheckBox; }
        }

        public new bool Focus()
        {
            MainCheckBox.Focus();
            return MainCheckBox.Focused;
        }

        public void ResetColors()
        {
            if (Focused)
            {
                base.BackColor = BorderColorFocus;
            }
            else
            {
                base.BackColor = BorderColor;
            }

            BackColor = BackgroundColor;
            BorderColorFocus = _borderColorFocus;
            BorderColorLeave = BorderColor;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            ResetColors();
        }

        private void AlignControl()
        {
            var newLocation = new Point((BackgroundPanel.Width - CheckBox.Width) / 2, CheckBox.Location.Y);
            CheckBox.Location = newLocation;

            newLocation = new Point((BackgroundPanel.Width - LegendLabel.Width) / 2, LegendLabel.Location.Y);
            LegendLabel.Location = newLocation;
        }

        private void CheckBox_SizeChanged(object sender, EventArgs e)
        {
            AlignControl();
        }

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
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

        private void CheckBox_Enter(object sender, EventArgs e)
        {
            base.BackColor = BorderColorFocus;
        }

        private void CheckBox_Leave(object sender, EventArgs e)
        {
            base.BackColor = BorderColorLeave;
        }

        private void BackgroundPanel_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void BackgroundPanel_Click(object sender, EventArgs e)
        {
            CheckBox.Focus();
            Checked = !CheckBox.Checked;
        }

        private void BackgroundPanel_DoubleClick(object sender, EventArgs e)
        {
            BackgroundPanel_Click(null, null);
        }

        private void BackgroundPanel_MouseDown(object sender, MouseEventArgs e)
        {
            CheckBox_Enter(null, null);
        }

        private void BackgroundPanel_MouseUp(object sender, MouseEventArgs e)
        {
            CheckBox_Leave(null, null);
        }

        private void LegendLabel_Click(object sender, EventArgs e)
        {
            BackgroundPanel_Click(null, null);
        }

        private void LegendLabel_DoubleClick(object sender, EventArgs e)
        {
            BackgroundPanel_Click(null, null);
        }

        private void LegendLabel_MouseDown(object sender, MouseEventArgs e)
        {
            CheckBox_Enter(null, null);
        }

        private void LegendLabel_MouseUp(object sender, MouseEventArgs e)
        {
            CheckBox_Leave(null, null);
        }
    }
}