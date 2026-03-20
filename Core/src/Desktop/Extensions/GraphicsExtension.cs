using System.Drawing;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public static class GraphicsExtension
    {
        public enum BorderSide
        {
            All,
            TopBottom,
            LeftRight,
            Top,
            Bottom,
            Left,
            Right
        }

        public static void DrawRoundBorder(this Graphics g, Control control, Color borderColor, int borderSize = 1, bool borderRound = true, BorderSide sides = BorderSide.All)
        {
            if (borderSize <= 0)
            {
                return;
            }

            Rectangle recInner = control.ClientRectangle;

            if (control is GroupBox)
            {
                recInner = new Rectangle(0, 6, control.Width, control.Height - 6);
            }

            // ControlPaint.DrawBorder(g, recInner, borderColor, ButtonBorderStyle.Solid);
            switch (sides)
            {
                case BorderSide.All:
                    ControlPaint.DrawBorder(g, recInner, borderColor, borderSize, ButtonBorderStyle.Solid, borderColor, borderSize, ButtonBorderStyle.Solid, borderColor, borderSize, ButtonBorderStyle.Solid, borderColor, borderSize, ButtonBorderStyle.Solid);
                    break;
                case BorderSide.TopBottom:
                    ControlPaint.DrawBorder(g, recInner, borderColor, 0, ButtonBorderStyle.Solid, borderColor, borderSize, ButtonBorderStyle.Solid, borderColor, 0, ButtonBorderStyle.Solid, borderColor, borderSize, ButtonBorderStyle.Solid);
                    break;
            }

            if (!borderRound || sides != BorderSide.All)
            {
                return;
            }

            var pBlank = new Pen(control.Parent.BackColor);

            if (pBlank.Color == Color.Transparent)
            {
                pBlank = new Pen(control.BackColor);
            }

            var pBorder = new Pen(borderColor);

            int widthB = control.Width - 1;
            int heightB = control.Height - 1;
            int height0 = 0;

            if (control is GroupBox)
            {
                height0 = 6;
            }

            // TopL
            g.DrawLine(pBlank, 0, height0, 2, height0);
            g.DrawLine(pBlank, 0, height0, 0, height0 + 2);
            g.DrawLine(pBlank, 0, height0, 1, height0 + 1);
            g.DrawLine(pBorder, 0, height0 + 3, 3, height0);

            // TopR
            g.DrawLine(pBlank, widthB, height0, widthB - 2, height0);
            g.DrawLine(pBlank, widthB, height0, widthB, height0 + 2);
            g.DrawLine(pBlank, widthB, height0, widthB - 1, height0 + 1);
            g.DrawLine(pBorder, widthB, height0 + 3, widthB - 3, height0);

            // BottomL
            g.DrawLine(pBlank, 0, heightB, 2, heightB);
            g.DrawLine(pBlank, 0, heightB, 0, heightB - 2);
            g.DrawLine(pBlank, 0, heightB, 1, heightB - 1);
            g.DrawLine(pBorder, 0, heightB - 3, 3, heightB);

            // BottomR
            g.DrawLine(pBlank, widthB, heightB, widthB - 2, heightB);
            g.DrawLine(pBlank, widthB, heightB, widthB, heightB - 2);
            g.DrawLine(pBlank, widthB, heightB, widthB - 1, heightB - 1);
            g.DrawLine(pBorder, widthB, heightB - 3, widthB - 3, heightB);
        }
    }
}