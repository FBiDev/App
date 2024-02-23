using System.Drawing;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public static class PictureBoxExtension
    {
        public static void ScaleTo(this PictureBox picBox, Bitmap bit)
        {
            picBox.Size = bit.MaxSize(picBox.MaximumSize);
        }

        public static void ScaleTo(this PictureBox picBox, Image img)
        {
            picBox.Size = ((Bitmap)img).MaxSize(picBox.MaximumSize);
        }

        public static void AlignBox(this FlatPictureBox picBox)
        {
            var xCenter = picBox.Parent.Width / 2 - picBox.Width / 2;
            var xLeft = 0;
            var xRight = picBox.Parent.Width - picBox.Width;
            var yCenter = (picBox.Parent.Height / 2) - (picBox.Height / 2);
            var yTop = 0;
            var yBottom = picBox.Parent.Height - picBox.Height;

            switch (picBox.Align)
            {
                case Align.Center: picBox.Location = new Point(xCenter, yCenter); break;
                case Align.Left: picBox.Location = new Point(xLeft, yCenter); break;
                case Align.Right: picBox.Location = new Point(xRight, yCenter); break;
                case Align.Top: picBox.Location = new Point(xCenter, yTop); break;
                case Align.Bottom: picBox.Location = new Point(xCenter, yBottom); break;
            }
        }
    }
}