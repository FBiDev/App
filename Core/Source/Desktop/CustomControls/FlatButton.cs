using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public class FlatButton : Button
    {
        #region Defaults
        [DefaultValue(typeof(Padding), "2, 2, 2, 2")]
        public new Padding Margin
        {
            get { return base.Margin; }
            set { base.Margin = value; }
        }

        [Category("_Colors"), DefaultValue(typeof(Color), "230, 230, 230")]
        public new Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        [Category("_Colors"), DefaultValue(typeof(Color), "47, 47, 47")]
        public new Color ForeColor
        {
            get { return base.ForeColor; }
            set { base.ForeColor = value; }
        }

        [DefaultValue(typeof(Font), "Segoe UI, 9")]
        public new Font Font
        {
            get { return base.Font; }
            set { base.Font = value; }
        }

        [DefaultValue(typeof(Size), "82, 34")]
        public new Size Size
        {
            get { return base.Size; }
            set { base.Size = value; }
        }

        [DefaultValue(typeof(bool), "false")]
        public new bool UseVisualStyleBackColor
        {
            get { return base.UseVisualStyleBackColor; }
            set { base.UseVisualStyleBackColor = value; }
        }

        [DefaultValue(typeof(ContentAlignment), "MiddleLeft")]
        public new ContentAlignment ImageAlign
        {
            get { return base.ImageAlign; }
            set { base.ImageAlign = value; }
        }

        [DefaultValue(typeof(AnchorStyles), "Top, Left")]
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

        [DefaultValue(typeof(FlatStyle), "Flat")]
        public new FlatStyle FlatStyle
        {
            get { return base.FlatStyle; }
            set { base.FlatStyle = value; }
        }

        [DefaultValue(typeof(Size), "24, 24")]
        public new Size MinimumSize
        {
            get { return base.MinimumSize; }
            set { base.MinimumSize = value; }
        }
        #endregion

        #region Properties
        protected Color _BackgroundColor = Color.FromArgb(230, 230, 230);
        [Category("_Colors"), DefaultValue(typeof(Color), "230, 230, 230")]
        public Color BackgroundColor
        {
            get { return _BackgroundColor; }
            set { _BackgroundColor = value; }
        }

        protected Color _BackgroundColorFocus = Color.FromArgb(213, 213, 213);
        [Category("_Colors"), DefaultValue(typeof(Color), "213, 213, 213")]
        public Color BackgroundColorFocus
        {
            get { return _BackgroundColorFocus; }
            set { _BackgroundColorFocus = value; }
        }

        public Color BorderColorDefault = Color.FromArgb(213, 223, 229);
        protected Color _BorderColor = Color.FromArgb(213, 223, 229);
        [Category("_Colors"), DefaultValue(typeof(Color), "213, 223, 229")]
        public Color BorderColor
        {
            get { return _BorderColor; }
            set { _BorderColor = value; }
        }

        protected Color _BorderColorFocus = Color.FromArgb(108, 132, 199);
        [Category("_Colors"), DefaultValue(typeof(Color), "108, 132, 199")]
        public Color BorderColorFocus
        {
            get { return _BorderColorFocus; }
            set { _BorderColorFocus = value; }
        }

        protected Color _TextColor = Color.FromArgb(47, 47, 47);
        [Category("_Colors"), DefaultValue(typeof(Color), "47, 47, 47")]
        public Color TextColor
        {
            get { return _TextColor; }
            set { _TextColor = value; }
        }

        protected Color _SelectedColor = Color.FromArgb(203, 223, 254);
        [Category("_Colors"), DefaultValue(typeof(Color), "203, 223, 254")]
        public Color SelectedColor
        {
            get { return _SelectedColor; }
            set { _SelectedColor = value; }
        }

        [Category("_Colors"), DefaultValue(false)]
        public bool LockedColors { get; set; }

        public bool _Selected;
        [Category("_Colors"), DefaultValue(false)]
        public bool Selected
        {
            get { return _Selected; }
            set
            {
                _Selected = value;
                if (_Selected)
                {
                    BackColor = SelectedColor;
                    BorderColor = BorderColorFocus;
                }
                else
                {
                    BackColor = BackgroundColor;
                    BorderColor = BorderColorDefault;
                    OnLostFocus(null);
                }

                FlatAppearance.BorderColor = BorderColor;
            }
        }

        bool _Enabled = true;

        public new bool Enabled
        {
            get { return _Enabled; }
            set
            {
                _Enabled = value;
                TabStop = value;
                SetStyle(ControlStyles.Selectable, value);

                if (_Enabled)
                {
                    Cursor = Cursors.Hand;
                    FlatAppearance.MouseOverBackColor = BackColor;
                }
                else
                {
                    Cursor = Cursors.No;
                    FlatAppearance.MouseOverBackColor = BackColor;
                }

                Refresh();
            }
        }
        #endregion

        public new event EventHandler Click;

        public FlatButton()
        {
            Cursor = Cursors.Hand;
            Anchor = AnchorStyles.Top | AnchorStyles.Left;
            ImageAlign = ContentAlignment.MiddleLeft;
            Font = new Font("Segoe UI", 9);

            MinimumSize = new Size(24, 24);
            Size = new Size(82, 34);
            Margin = new Padding(2);

            base.UseVisualStyleBackColor = false;
            FlatStyle = FlatStyle.Flat;

            //foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(this))
            //    property.ResetValue(this);

            Enabled = true;
            MouseDown += OnMouseDown;
            base.Click += OnClick;
        }

        void OnMouseDown(object sender, MouseEventArgs e)
        {
            //if (!Enabled) ((Control)sender).Parent.Focus();
        }

        void OnClick(object sender, EventArgs e)
        {
            if (!Enabled || Click == null) return;
            Click(sender, e);
        }

        public void ResetColors()
        {
            if (LockedColors) return;

            // Prevent the button from drawing its own border
            FlatAppearance.BorderSize = 0;
            FlatAppearance.BorderColor = BorderColor;
            FlatAppearance.MouseOverBackColor = BackgroundColorFocus;

            BackColor = BackgroundColor;
            ForeColor = TextColor;
            Selected = Selected;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            ResetColors();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            if (!Enabled) return;

            base.OnMouseEnter(e);
            FlatAppearance.BorderColor = BorderColorFocus;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (!Focused) { FlatAppearance.BorderColor = BorderColor; }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            if (!Enabled) return;

            FlatAppearance.BorderColor = BorderColorFocus;
        }

        protected override void OnLostFocus(EventArgs e)
        {
            FlatAppearance.BorderColor = BorderColor;
        }

        protected override bool ShowFocusCues
        {
            get { return false; }
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);

            // Draw Border using color specified in Flat Appearance
            var pen = new Pen(FlatAppearance.BorderColor, 1);
            var rectangle = new Rectangle(0, 0, Size.Width - 1, Size.Height - 1);
            pevent.Graphics.DrawRectangle(pen, rectangle);

            if (Enabled && Focused)
            {
                var penDot = new Pen(SystemColors.ControlDarkDark, 1)
                {
                    DashStyle = System.Drawing.Drawing2D.DashStyle.Dot
                };
                var rectangleDot = new Rectangle(2, 2, Size.Width - 5, Size.Height - 5);
                pevent.Graphics.DrawRectangle(penDot, rectangleDot);
            }

            if (Enabled == false)
            {
                var rectangleAll = new Rectangle(0, 0, Size.Width, Size.Height);
                var disabledColor = new SolidBrush(Color.FromArgb(128, BackColor));
                pevent.Graphics.FillRectangle(disabledColor, rectangleAll);
            }
        }
    }
}