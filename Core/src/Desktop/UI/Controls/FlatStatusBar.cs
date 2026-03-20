using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public partial class FlatStatusBar : UserControl
    {
        private int? _registros;
        private Movimento _movimento;
        private bool _borderEnable = true;

        public FlatStatusBar()
        {
            InitializeComponent();

            TabStop = false;
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            Load += FlatStatusBar_Load;
            BackColorChanged += StatusBar_BackColorChanged;

            lblStatus1.TextChanged += LblStatus1_TextChanged;
            lblStatus2.TextChanged += LblStatus2_TextChanged;
        }

        [DefaultValue(false)]
        public new bool TabStop
        {
            get { return base.TabStop; }
            set { base.TabStop = value; }
        }

        public int? Registros
        {
            get
            {
                return _registros;
            }

            set
            {
                _registros = value;

                if (value == null || value < 0)
                {
                    lblStatus1.Text = string.Empty;
                }
                else
                {
                    lblStatus1.Text = value.ToString() + " registro";

                    if (Cast.ToInt(value) > 1 || value == 0)
                    {
                        lblStatus1.Text += "s";
                    }
                }
            }
        }

        public Movimento Movimento
        {
            get
            {
                return _movimento;
            }

            set
            {
                _movimento = value;

                if (value == Movimento.Nenhum)
                {
                    lblStatus2.Text = string.Empty;
                }
                else
                {
                    lblStatus2.Text = value.ToString();
                }
            }
        }

        public bool BorderEnable
        {
            get
            {
                return _borderEnable;
            }

            set
            {
                _borderEnable = value;
                pnlBorder.Visible = _borderEnable;
            }
        }

        public Color BackColorContent
        {
            get
            {
                return pnlContent.BackColor;
            }

            set
            {
                pnlContent.BackColor = value;
                lblStatus1.BackColor = BackColorContent;
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
        }

        private void StatusBar_BackColorChanged(object sender, EventArgs e)
        {
            pnlStatus1.BackColor = BackColor;
        }

        private void FlatStatusBar_Load(object sender, EventArgs e)
        {
            LblStatus1_TextChanged(null, null);
            LblStatus2_TextChanged(null, null);
        }

        private void LblStatus2_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(lblStatus2.Text.Trim()))
            {
                pnlStatus2.Visible = false;
                return;
            }

            pnlStatus2.Visible = true;
        }

        private void LblStatus1_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(lblStatus1.Text.Trim()))
            {
                pnlStatus1.Visible = false;
                if (BorderEnable)
                {
                    // pnlBorder.Visible = false;
                }

                return;
            }

            pnlStatus1.Visible = true;

            if (BorderEnable)
            {
                // pnlBorder.Visible = true;
            }
        }
    }
}