﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public partial class ExeButton : UserControl
    {
        private Color _textColor = Color.FromArgb(47, 47, 47);

        public ExeButton()
        {
            InitializeComponent();

            MainButton.MouseEnter += OnMouseEnter;
            MainButton.MouseLeave += OnMouseLeave;
            MainButton.Click += OnClick;
            MainButton.SizeChanged += OnSizeChanged;
            SizeChanged += OnSizeChanged;

            BackColor = Colors.RGB(151, 184, 243);
            BorderColor = Color.WhiteSmoke;
            TextColor = _textColor;
        }

        public new event EventHandler Click;

        [DefaultValue(typeof(Size), "76, 69")]
        public new Size Size
        {
            get { return base.Size; }
            set { base.Size = value; }
        }

        [DefaultValue(typeof(Padding), "1, 1, 1, 1")]
        public new Padding Padding
        {
            get { return base.Padding; }
            set { base.Padding = value; }
        }

        [DefaultValue(typeof(Padding), "2, 2, 2, 2")]
        public new Padding Margin
        {
            get { return base.Margin; }
            set { base.Margin = value; }
        }

        [DefaultValue(typeof(Color), "Transparent")]
        public Color BorderColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        [DefaultValue(typeof(Color), "Transparent")]
        public new Color BackColor
        {
            get { return pnlBack.BackColor; }
            set { pnlBack.BackColor = value; }
        }

        public Button MainButton
        {
            get { return InnerButton; }
        }

        [DefaultValue(typeof(Size), "48, 48")]
        public Size SizeButton
        {
            get { return InnerButton.Size; }
            set { InnerButton.Size = value; }
        }

        public new Image BackgroundImage
        {
            get { return InnerButton.BackgroundImage; }
            set { InnerButton.BackgroundImage = value; }
        }

        public new ImageLayout BackgroundImageLayout
        {
            get { return InnerButton.BackgroundImageLayout; }
            set { InnerButton.BackgroundImageLayout = value; }
        }

        public new Font Font
        {
            get { return ExeLabel.Font; }
            set { ExeLabel.Font = value; }
        }

        public string TextLabel
        {
            get { return ExeLabel.Text; }
            set { ExeLabel.Text = value; }
        }

        [Category("_Colors"), DefaultValue(typeof(Color), "47, 47, 47")]
        public Color TextColor
        {
            get { return ExeLabel.ForeColor; }
            set { ExeLabel.ForeColor = value; }
        }

        public new bool Focused
        {
            get { return InnerButton.Focused; }
        }

        public new bool Focus()
        {
            InnerButton.Focus();
            return InnerButton.Focused;
        }

        private void AlignControl()
        {
            var newLocation = new Point((pnlBack.Width - InnerButton.Width) / 2, InnerButton.Location.Y);
            InnerButton.Location = newLocation;
        }

        private void OnClick(object sender, EventArgs e)
        {
            if (Click.IsNull())
            {
                return;
            }

            Click(sender, e);
        }

        private void OnSizeChanged(object sender, EventArgs e)
        {
            AlignControl();
        }

        private void OnMouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void OnMouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }
    }
}