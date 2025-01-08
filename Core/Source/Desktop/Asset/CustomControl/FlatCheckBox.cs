using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using App.Core.Properties;

namespace App.Core.Desktop
{
    public partial class FlatCheckBox : UserControl
    {
        private Color _backgroundColor = Color.White;
        private Color _borderColor = Color.FromArgb(213, 223, 229);
        private Color _borderColorFocus = Color.FromArgb(108, 132, 199);
        private bool _checkboxImage = false;

        public FlatCheckBox()
        {
            InitializeComponent();

            Click += (sender, e) => BackgroundPanel_Click(null, null);
            InnerCheckBox.CheckedChanged += CheckBox_CheckedChanged;            
            InnerCheckBox.Click += (sender, e) => RefreshCheckboxColors();

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
            get { return InnerCheckBox.Checked; }
            set { InnerCheckBox.Checked = value; }
        }

        [DefaultValue(CheckState.Unchecked)]
        public CheckState CheckState
        {
            get { return InnerCheckBox.CheckState; }
            set { InnerCheckBox.CheckState = value; }
        }

        [DefaultValue(false)]
        public bool ThreeState
        {
            get
            {
                return InnerCheckBox.ThreeState;
            }

            set
            {
                InnerCheckBox.ThreeState = value;

                if (InnerCheckBox.ThreeState == false && InnerCheckBox.CheckState == CheckState.Indeterminate)
                {
                    InnerCheckBox.CheckState = CheckState.Checked;
                    RefreshCheckboxColors();
                }
            }
        }

        [DefaultValue(false)]
        public bool CheckboxImage
        {
            get
            {
                return _checkboxImage;
            }

            set
            {
                _checkboxImage = value;

                if (_checkboxImage)
                {
                    CheckboxAsImage();
                }
                else
                {
                    CheckboxAsBox();
                }
            }
        }

        public new bool Focused
        {
            get { return MainCheckBox.Focused; }
        }

        private CheckBox InnerCheckBox
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

        public void RefreshCheckboxColors()
        {
            if (CheckboxImage)
            {
                InnerCheckBox.BackColor = BackgroundColor;
                InnerCheckBox.FlatAppearance.CheckedBackColor = BackgroundColor;
                InnerCheckBox.FlatAppearance.MouseOverBackColor = BackgroundColor;
                InnerCheckBox.FlatAppearance.MouseDownBackColor = BackgroundColor;

                if (CheckState == CheckState.Checked)
                {
                    InnerCheckBox.Image = Resources.img_true_ico;
                }
                else if (CheckState == CheckState.Unchecked)
                {
                    InnerCheckBox.Image = Resources.img_false_ico;
                }
                else
                {
                    InnerCheckBox.Image = Resources.img_indeterminate_ico;
                }
            }
            else
            {
                InnerCheckBox.Image = null;

                if (CheckState == CheckState.Checked)
                {
                    InnerCheckBox.BackColor = Color.LightGreen;
                }
                else if (CheckState == CheckState.Unchecked)
                {
                    InnerCheckBox.BackColor = Color.LightCoral;
                }
                else
                {
                    InnerCheckBox.BackColor = Color.Gray;
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            ResetColors();
            RefreshCheckboxColors();
        }

        private void AlignControl()
        {
            var newLocation = new Point((BackgroundPanel.Width - InnerCheckBox.Width) / 2, InnerCheckBox.Location.Y);
            InnerCheckBox.Location = newLocation;

            newLocation = new Point((BackgroundPanel.Width - LegendLabel.Width) / 2, LegendLabel.Location.Y);
            LegendLabel.Location = newLocation;
        }

        private void CheckboxAsImage()
        {
            InnerCheckBox.FlatAppearance.BorderSize = 0;
            InnerCheckBox.Appearance = Appearance.Button;
            InnerCheckBox.Padding = new Padding(0, 0, 2, 2);
            RefreshCheckboxColors();
        }

        private void CheckboxAsBox()
        {
            InnerCheckBox.FlatAppearance.BorderSize = 1;
            InnerCheckBox.Appearance = Appearance.Normal;
            InnerCheckBox.Padding = new Padding(0, 0, 0, 0);
            RefreshCheckboxColors();
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

            RefreshCheckboxColors();
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
            InnerCheckBox.Focus();

            if (ThreeState)
            {
                CheckState = (CheckState)((((int)CheckState) + 1) % 3);

                if (CheckState == CheckState.Indeterminate)
                {
                    RefreshCheckboxColors();
                }
            }
            else
            {
                Checked = !InnerCheckBox.Checked;
            }
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