using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using App.Core.Properties;

namespace App.Core
{
    public static class BitmapExtension
    {
        public static Size MaxSize(this Bitmap bitmap, Size maxSize)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;

            if (maxSize.Width == 0 && maxSize.Height == 0) { return new Size(width, height); }

            float factorW = height / (float)width;
            float factorH = width / (float)height;

            if (width > maxSize.Width)
            {
                width = maxSize.Width;
                height = (int)Math.Round(width * factorW);
            }

            if (height > maxSize.Height)
            {
                height = maxSize.Height;
                width = (int)Math.Round(height * factorH);
            }

            return new Size(width, height);
        }

        public static bool IsAnimatedGif(Bitmap bitmap)
        {
            var dimensions = new FrameDimension(bitmap.FrameDimensionsList[0]);
            return bitmap.GetFrameCount(dimensions) > 1;
        }

        public static Bitmap Clone32bpp(this Bitmap bitmap)
        {
            return bitmap.Clone(new Rectangle(Point.Empty, bitmap.Size), PixelFormat.Format32bppPArgb);
        }

        public static Bitmap SuperFastLoad(string path, Bitmap errorBitmap = null)
        {
            if (File.Exists(path) == false || new FileInfo(path).Length == 0)
            {
                if (errorBitmap == null)
                    return Resources.img_notfound;
                return errorBitmap;
            }

            using (MemoryStream ms = new MemoryStream(File.ReadAllBytes(path)))
            using (Bitmap bitmapFile = ((Bitmap)Image.FromStream(ms, false, false)))
            {
                if (IsAnimatedGif(bitmapFile))
                    return Image.FromFile(path) as Bitmap;

                return bitmapFile.Clone32bpp();
            }
        }

        public static Bitmap FromFile(string filePath, Size newSize, Bitmap errorBitmap = null)
        {
            using (var srcImage = SuperFastLoad(filePath))
            {
                if (srcImage == null) return errorBitmap;

                var dest = new Bitmap(newSize.Width, newSize.Height, PixelFormat.Format32bppPArgb);
                using (var g = Graphics.FromImage(dest))
                {
                    g.CompositingMode = CompositingMode.SourceCopy;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.SmoothingMode = SmoothingMode.HighQuality;

                    if ((srcImage.Height > newSize.Height) || (srcImage.Width > newSize.Width))
                    {
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    }
                    else
                    {
                        g.InterpolationMode = InterpolationMode.NearestNeighbor;
                        g.PixelOffsetMode = PixelOffsetMode.Half;
                    }

                    var wrapMode = new ImageAttributes();
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);

                    g.DrawImage(srcImage, new Rectangle(Point.Empty, newSize), 0, 0, srcImage.Width, srcImage.Height, GraphicsUnit.Pixel, wrapMode);
                }
                return dest;
            }
        }
    }
}