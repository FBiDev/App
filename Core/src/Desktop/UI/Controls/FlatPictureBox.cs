using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    /// <summary>
    /// A PictureBox control extended to allow a variety of interpolations.
    /// </summary>
    public class FlatPictureBox : PictureBox
    {
        private readonly IContainer components;
        private readonly ContextMenuStrip mnuPicture;
        private readonly ToolStripMenuItem mniCopyImage;
        private readonly ToolStripMenuItem mniRemoveImage;

        // DragDrop
        private Image newImage;
        private bool movedImage;

        private string dropFilePath;
        private bool isValidDropFile;
        private Thread getDropImageThread;

        private bool isValidDragImage;
        private Thread getDragImageThread;

        private Dictionary<ImageFormats, string> filterMap = new Dictionary<ImageFormats, string>
        {
            { ImageFormats.Jpg, ".jpg" },
            { ImageFormats.Jpeg, ".jpeg" },
            { ImageFormats.Png, ".png" },
            { ImageFormats.Bmp, ".bmp" },
            { ImageFormats.Gif, ".gif" }
        };

        private Align _align;
        private InterpolationMode interpolation = InterpolationMode.NearestNeighbor;

        public FlatPictureBox()
        {
            // SubMenu
            components = new Container();
            SizeChanged += OnSizeChanged;
            ParentChanged += OnParentChanged;
            DragEnter += OnDragEnter;
            DragDrop += OnDragDrop;
            MouseDown += OnMouseDown;

            Filter = ImageFormats.Jpg | ImageFormats.Jpeg | ImageFormats.Png;

            mniCopyImage = new ToolStripMenuItem
            {
                Name = "mniCopyImage",
                Size = new Size(227, 22),
                Text = "Copy image"
            };

            mniRemoveImage = new ToolStripMenuItem
            {
                Name = "mniRemoveImage",
                Size = new Size(227, 22),
                Text = "Remove image"
            };

            mnuPicture = new ContextMenuStrip(components)
            {
                Name = "mnuPicture",
                Size = new Size(228, 48),
                ShowImageMargin = false
            };

            mnuPicture.SuspendLayout();
            mnuPicture.Items.AddRange(new ToolStripItem[]
            {
                mniCopyImage,
                mniRemoveImage
            });
            mnuPicture.ResumeLayout(false);

            mniCopyImage.MouseDown += MniCopyImage_MouseDown;
            mniRemoveImage.MouseDown += RemoveImageMenuItem_MouseDown;
        }

        [DefaultValue(false)]
        public bool AutoScale { get; set; }

        [Browsable(true)]
        public override bool AllowDrop
        {
            get { return base.AllowDrop; }
            set { base.AllowDrop = value; }
        }

        [Browsable(false)]
        public ImageFormats Filter { get; set; }

        public new Image Image
        {
            get
            {
                return base.Image;
            }

            set
            {
                base.Image = value;

                if (value is Image)
                {
                    if (AutoScale)
                    {
                        this.ScaleTo(value);
                    }

                    ContextMenuStrip = mnuPicture;
                    mniRemoveImage.Visible = AllowDrop;
                }
                else
                {
                    ContextMenuStrip = null;
                }
            }
        }

        [DefaultValue(typeof(Align), "None")]
        public Align Align
        {
            get
            {
                return _align;
            }

            set
            {
                _align = value;

                if (Parent is Control)
                {
                    this.AlignBox();
                }
            }
        }

        #region Interpolation Property
        [DefaultValue(typeof(InterpolationMode), "NearestNeighbor"),
        Description("The interpolation used to render the image.")]
        public InterpolationMode Interpolation
        {
            get
            {
                return interpolation;
            }

            set
            {
                if (value == InterpolationMode.Invalid)
                {
                    throw new ArgumentException("\"Invalid\" is not a valid value."); // (Duh!)
                }

                interpolation = value;
                Invalidate(); // Image should be redrawn when a different interpolation is selected
            }
        }
        #endregion

        protected override void OnPaint(PaintEventArgs pe)
        {
            // Before the PictureBox renders the image, we modify the
            // graphics object to change the interpolation.

            // Set the selected interpolation.
            pe.Graphics.InterpolationMode = interpolation;

            // Certain interpolation modes (such as nearest neighbor) need
            // to be offset by half a pixel to render correctly.
            if (interpolation == InterpolationMode.NearestNeighbor)
            {
                pe.Graphics.PixelOffsetMode = PixelOffsetMode.Half;
            }
            else
            {
                pe.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            }

            // Allow the PictureBox to draw.
            base.OnPaint(pe);
        }

        private bool GetFilter(ImageFormats value, string ext)
        {
            foreach (var kvp in filterMap)
            {
                if ((value & kvp.Key) == kvp.Key && ext == kvp.Value)
                {
                    return true;
                }
            }

            return false;
        }

        private void OnParentChanged(object sender, EventArgs e)
        {
            if (Parent is Control)
            {
                this.AlignBox();
                Parent.SizeChanged += OnSizeChanged;
            }
        }

        private void OnSizeChanged(object sender, EventArgs e)
        {
            if (Parent is Control)
            {
                this.AlignBox();
            }
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || Image == null)
            {
                return;
            }

            var effect = DoDragDrop(Image, DragDropEffects.Move);

            if (effect != DragDropEffects.None && movedImage)
            {
                Image = null;
            }
        }

        private void MniCopyImage_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            ClipboardSafe.SetImage(Image);
        }

        private void RemoveImageMenuItem_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            Image = null;
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            string filename;
            isValidDropFile = GetDropFilename(out filename, e);

            if (isValidDropFile)
            {
                movedImage = true;
                dropFilePath = filename;
                getDropImageThread = new Thread(new ThreadStart(LoadDroppedImage));
                getDropImageThread.Start();
                e.Effect = DragDropEffects.Copy;
                return;
            }

            isValidDragImage = GetDragImage(out newImage, e);

            if (isValidDragImage)
            {
                movedImage = true;
                getDragImageThread = new Thread(new ThreadStart(LoadDragImage));
                getDragImageThread.Start();
                e.Effect = DragDropEffects.Move;
                return;
            }

            e.Effect = DragDropEffects.None;
        }

        private void OnDragDrop(object sender, DragEventArgs e)
        {
            if (!isValidDropFile && !isValidDragImage)
            {
                return;
            }

            if (isValidDropFile)
            {
                while (getDropImageThread.IsAlive)
                {
                    Application.DoEvents();
                    Thread.Sleep(0);
                }
            }

            if (isValidDragImage)
            {
                while (getDragImageThread.IsAlive)
                {
                    Application.DoEvents();
                    Thread.Sleep(0);
                }
            }

            if (newImage.Size.Width <= Width && newImage.Size.Height <= Height)
            {
                Interpolation = InterpolationMode.NearestNeighbor;
            }
            else
            {
                Interpolation = InterpolationMode.HighQualityBicubic;
            }

            movedImage = false;
            Image = newImage;
        }

        private bool GetDragImage(out Image dragImage, DragEventArgs e)
        {
            dragImage = default(Bitmap);
            if ((e.AllowedEffect & DragDropEffects.Move) == DragDropEffects.Move)
            {
                var dataDrag = e.Data.GetData(DataFormats.Bitmap) as Bitmap;
                if (dataDrag == null)
                {
                    return false;
                }

                dragImage = dataDrag;
                return true;
            }

            return false;
        }

        private bool GetDropFilename(out string filename, DragEventArgs e)
        {
            filename = string.Empty;
            if ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
            {
                var dataDrop = e.Data.GetData(DataFormats.FileDrop) as Array;

                if (dataDrop == null)
                {
                    return false;
                }

                if ((dataDrop.Length == 1) && (dataDrop.GetValue(0) is string))
                {
                    filename = ((string[])dataDrop)[0];
                    var ext = Path.GetExtension(filename).ToLower();

                    if (GetFilter(Filter, ext))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void LoadDroppedImage()
        {
            newImage = BitmapExtension.SuperFastLoad(dropFilePath);
        }

        private void LoadDragImage()
        {
        }
    }
}