using System;
using System.Drawing;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public class FlatComboBoxNew : ComboBox
    {
        private const int WMPAINT = 0xF;

        private readonly int buttonWidth = SystemInformation.HorizontalScrollBarArrowWidth;

        public FlatComboBoxNew()
        {
            PreviousIndex = -1;

            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            AutoCompleteSource = AutoCompleteSource.ListItems;
            IntegralHeight = false;
            MaxDropDownItems = 10;
            ItemHeight = 18;

            Font = new Font("Segoe UI", 9);
            ForeColor = Color.FromArgb(47, 47, 47);
            BackColor = Color.Red;

            DropDownStyle = ComboBoxStyle.DropDownList;
            FlatStyle = FlatStyle.Standard;
            DrawMode = DrawMode.OwnerDrawFixed;
        }

        public int PreviousIndex { get; private set; }

        public void ResetIndex()
        {
            SelectedIndex = -1;
            SelectedIndex = -1;
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            if (SelectedIndex != PreviousIndex)
            {
                base.OnSelectedIndexChanged(e);
                PreviousIndex = SelectedIndex;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            Region = new Region(new Rectangle(1, 1, Width - 2, Height - 2));
        }
        #region Border color when disabled

        // Color borderColor = Color.Blue;
        // public Color BorderColor
        // {
        //    get { return borderColor; }
        //    set { borderColor = value; Invalidate(); }
        // }
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WMPAINT && DropDownStyle != ComboBoxStyle.Simple)
            {
                using (var g = Graphics.FromHwnd(Handle))
                {
                    var adjustMent = 0;
                    if (FlatStyle == FlatStyle.Popup || (FlatStyle == FlatStyle.Flat && DropDownStyle == ComboBoxStyle.DropDownList))
                    {
                        adjustMent = 1;
                    }

                    var innerBorderWisth = 3;
                    var innerBorderColor = BackColor;

                    // if (DropDownStyle == ComboBoxStyle.DropDownList &&
                    //    (FlatStyle == FlatStyle.System || FlatStyle == FlatStyle.Standard))
                    //    innerBorderColor = Color.FromArgb(0xCCCCCC);
                    // if (DropDownStyle == ComboBoxStyle.DropDown && !Enabled)
                    //    innerBorderColor = SystemColors.Control;
                    if (DropDownStyle == ComboBoxStyle.DropDown || Enabled == false)
                    {
                        using (var p = new Pen(innerBorderColor, innerBorderWisth))
                        {
                            p.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;

                            g.DrawRectangle(p, 1, 1, Width - buttonWidth - adjustMent - 1, Height - 1);
                        }
                    }

                    // using (var p = new Pen(BorderColor))
                    // {
                    //    g.DrawRectangle(p, 0, 0, Width - 1, Height - 1);
                    //    g.DrawLine(p, Width - buttonWidth - adjustMent,
                    //        0, Width - buttonWidth - adjustMent, Height);
                    // }
                }
            }
        }
        #endregion
    }
}