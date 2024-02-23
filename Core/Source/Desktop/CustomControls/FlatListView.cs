using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public partial class FlatListView : ListView
    {
        public List<Bitmap> ImagesOriginal = new List<Bitmap>();
        public List<Image> Images = new List<Image>();
        public List<string> Titles = new List<string>();
        public List<string> Descriptions = new List<string>();

        public Size ImagesSize = new Size(32, 32);
        public int ImagesBorder = 2;
        public Color ImagesBorderColor = Color.Gold;
        public int ImagesPerRow = 5;
        public int ImagesMargin = 6;

        public FlatListView()
        {
            DoubleBuffered = true;

            View = View.Tile;
            TileSize = new Size(32, 32);
            AutoArrange = true;

            BackColor = SystemColors.Control;

            OwnerDraw = true;
            DrawItem += ListView_DrawItem;
            DrawSubItem += ListView_DrawSubItem;
            DrawColumnHeader += ListView_DrawColumnHeader;

            ItemMouseHover += FlatListView_ItemMouseHover;
        }

        public event ScrollEventHandler Scroll;
        protected virtual void OnScroll(ScrollEventArgs e)
        {
            ScrollEventHandler handler = Scroll;
            if (handler == null)
                return;

            handler(this, e);
        }

        const uint WM_MOUSEWHEEL = 0x020A;
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_MOUSEWHEEL)
            { // Trap WM_VSCROLL
                //OnScroll(new ScrollEventArgs((ScrollEventType)(m.WParam.ToInt32() & 0xffff), 0));
                OnScroll(new ScrollEventArgs((ScrollEventType)1, 0));
            }
        }

        void FlatListView_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {
            var item = (FlatListViewItem)e.Item;
            item.Hover = true;
        }

        void ListView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            int x = (e.Bounds.Left) + 2 + 4;
            int y = (e.Bounds.Top - 2) + 2;

            if (e.ItemIndex >= Items.Count + 1 - ImagesPerRow)
            {
                //TileSize = new Size((ImagesSize.Width + ImagesMargin) - 4, (ImagesSize.Height + ImagesMargin) - 4);
            }

            Image currentImage = Images[e.Item.ImageIndex];
            var rectAll = new Rectangle(x, y, currentImage.Width, currentImage.Height);
            var rectImage = new Rectangle(x + ImagesBorder, y + ImagesBorder, currentImage.Width - (ImagesBorder * 2), currentImage.Height - (ImagesBorder * 2));

            e.Graphics.DrawImage(currentImage, rectAll);

            //using srcImg As Image = Image.FromFile(fn)

            //using (var srcImg = Image.FromFile(imagePaths[e.ItemIndex]))
            //{
            //    var imgBounds = new Rectangle(Point.Empty, LargeImageList.ImageSize);

            //    var bit = new Bitmap(imgBounds.Width, imgBounds.Height, PixelFormat.Format32bppArgb);
            //    using (var gx = Graphics.FromImage(bit))
            //    {
            //        gx.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //        gx.DrawImage(srcImg, imgBounds);
            //    }
            //}

            //e.Graphics.DrawString(e.Item.Text, e.Item.Font, Brushes.Black,
            //    new Rectangle(e.Bounds.Left - 2, e.Bounds.Top - 4, e.Bounds.Width, e.Bounds.Height));
            //using (Brush brush = new SolidBrush((e.State.HasFlag(ListViewItemStates.Focused)) ? SystemColors.Highlight : e.Item.BackColor))
            //    e.Graphics.FillRectangle(brush, e.Bounds);

            //Color textColor = SystemColors.WindowText;
            if (e.Item.Selected)
            {
                if (Focused)
                {
                    //textColor = SystemColors.HighlightText;
                    Brush brush = new SolidBrush(Color.FromArgb(120, 0, 120, 215));
                    e.Graphics.FillRectangle(brush, rectImage);
                }
                else if (!HideSelection)
                {
                    //textColor = SystemColors.ControlText;
                    //e.Graphics.FillRectangle(SystemBrushes.Control, e.Bounds);
                }
            }
            //else
            //{
            //using (SolidBrush br = new SolidBrush(BackColor))
            //{
            //e.Graphics.FillRectangle(br, e.Bounds);
            //}
            //}

            //TextRenderer.DrawText(e.Graphics, e.Item.Text, Font, e.Bounds,
            //                      textColor, Color.Empty,
            //                      TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        void ListView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        void ListView_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            if (e.ColumnIndex > 0)
                e.DrawDefault = true;
        }

        public async Task AddImageList(List<Bitmap> imageList, Size newImagesSize, List<string> titles = null, List<string> descriptions = null)
        {
            ImagesOriginal = imageList;

            Items.Clear();
            Images.Clear();
            AutoScrollOffset = Point.Empty;

            if (newImagesSize.IsEmpty || newImagesSize.Width == 1 && newImagesSize.Height == 1) newImagesSize = new Size(2, 2);
            if (ImagesBorder < 0) ImagesBorder = 0;
            if (ImagesMargin < 0) ImagesMargin = 0;
            if (ImagesPerRow <= 0) ImagesPerRow = 1;

            ImagesSize = newImagesSize;
            Titles = titles;
            Descriptions = descriptions;

            await Task.Run(() =>
            {
                //Convert Images
                foreach (var img in imageList)
                {
                    var newImage = new Bitmap(ImagesSize.Width, ImagesSize.Height, PixelFormat.Format24bppRgb);
                    newImage.MakeTransparent();

                    using (Graphics g = Graphics.FromImage(newImage))
                    {
                        if (ImagesBorderColor != Color.Transparent)
                            g.Clear(ImagesBorderColor);

                        //copy in High Quality
                        g.CompositingMode = CompositingMode.SourceCopy;
                        g.CompositingQuality = CompositingQuality.HighQuality;
                        g.SmoothingMode = SmoothingMode.HighQuality;

                        //prevents ghosting around the image borders
                        var wrapMode = new ImageAttributes();
                        wrapMode.SetWrapMode(WrapMode.TileFlipXY);

                        g.InterpolationMode = (img.Height > ImagesSize.Height) || (img.Width > ImagesSize.Width) ? InterpolationMode.HighQualityBicubic : InterpolationMode.NearestNeighbor;
                        g.PixelOffsetMode = (img.Height > ImagesSize.Height) || (img.Width > ImagesSize.Width) ? PixelOffsetMode.HighQuality : PixelOffsetMode.Half;

                        //var rectAll = new Rectangle(0, 0, newImagesSize.Width, newImagesSize.Height);
                        var rectImage = new Rectangle(ImagesBorder, ImagesBorder, ImagesSize.Width - (ImagesBorder * 2), ImagesSize.Height - (ImagesBorder * 2));

                        g.DrawImage(img, rectImage, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, wrapMode);

                        //DrawToBitmap(img, rectAll);
                        Images.Add(newImage);
                    }
                }
                //images = imageList;

                //if (size.IsEmpty) size = new Size(32, 32);
                //if (ImagePadding < 6) ImagePadding = 6;
            });

            var pics = new ImageList
            {
                ImageSize = ImagesSize,
                ColorDepth = ColorDepth.Depth24Bit
            };

            pics.Images.AddRange(Images.ToArray());

            int index = 0;
            foreach (var imageItem in Images)
            {
                var viewItem = new FlatListViewItem
                {
                    ImageIndex = index
                };

                index++;

                Items.Add(viewItem);
            }

            TileSize = new Size(ImagesSize.Width + ImagesMargin, ImagesSize.Height + ImagesMargin);
            LargeImageList = pics;
            SmallImageList = pics;
        }
    }
}