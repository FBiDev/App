using System.Drawing;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public static class GraphicsExtension
    {
        public static void DrawRoundBorder(this Graphics g, Control control, Color borderColor, int borderSize = 1, bool borderRound = true)
        {
            if (borderSize <= 0) { return; }

            Rectangle recInner = control.ClientRectangle;
            if (control is GroupBox) recInner = new Rectangle(0, 6, control.Width, control.Height - 6);

            //ControlPaint.DrawBorder(g, recInner, borderColor, ButtonBorderStyle.Solid);
            ControlPaint.DrawBorder(g, recInner,
                borderColor, borderSize, ButtonBorderStyle.Solid,
                borderColor, borderSize, ButtonBorderStyle.Solid,
                borderColor, borderSize, ButtonBorderStyle.Solid,
                borderColor, borderSize, ButtonBorderStyle.Solid);

            if (!borderRound) { return; }

            var pBlank = new Pen(control.Parent.BackColor);

            if (pBlank.Color == Color.Transparent)
                pBlank = new Pen(control.BackColor);

            var pBorder = new Pen(borderColor);

            int WidthB = control.Width - 1;
            int HeightB = control.Height - 1;
            int Height0 = 0;

            if (control is GroupBox) Height0 = 6;

            //TopL
            g.DrawLine(pBlank, 0, Height0, 2, Height0);
            g.DrawLine(pBlank, 0, Height0, 0, Height0 + 2);
            g.DrawLine(pBlank, 0, Height0, 1, Height0 + 1);
            g.DrawLine(pBorder, 0, Height0 + 3, 3, Height0);

            //TopR
            g.DrawLine(pBlank, WidthB, Height0, WidthB - 2, Height0);
            g.DrawLine(pBlank, WidthB, Height0, WidthB, Height0 + 2);
            g.DrawLine(pBlank, WidthB, Height0, WidthB - 1, Height0 + 1);
            g.DrawLine(pBorder, WidthB, Height0 + 3, WidthB - 3, Height0);

            //BottomL
            g.DrawLine(pBlank, 0, HeightB, 2, HeightB);
            g.DrawLine(pBlank, 0, HeightB, 0, HeightB - 2);
            g.DrawLine(pBlank, 0, HeightB, 1, HeightB - 1);
            g.DrawLine(pBorder, 0, HeightB - 3, 3, HeightB);

            //BottomR
            g.DrawLine(pBlank, WidthB, HeightB, WidthB - 2, HeightB);
            g.DrawLine(pBlank, WidthB, HeightB, WidthB, HeightB - 2);
            g.DrawLine(pBlank, WidthB, HeightB, WidthB - 1, HeightB - 1);
            g.DrawLine(pBorder, WidthB, HeightB - 3, WidthB - 3, HeightB);
        }
    }
}