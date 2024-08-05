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
        readonly IContainer components;
        readonly ContextMenuStrip mnuPicture;
        readonly ToolStripMenuItem mniCopyImage;
        readonly ToolStripMenuItem mniRemoveImage;

        [DefaultValue(false)]
        public bool AutoScale { get; set; }

        [Browsable(true)]
        public override bool AllowDrop { get { return base.AllowDrop; } set { base.AllowDrop = value; } }

        public new Image Image
        {
            get { return base.Image; }
            set
            {
                base.Image = value;

                if (value is Image)
                {
                    if (AutoScale)
                        this.ScaleTo(value);

                    ContextMenuStrip = mnuPicture;
                    mniRemoveImage.Visible = AllowDrop;
                }
                else
                {
                    ContextMenuStrip = null;
                }
            }
        }

        Align AlignValue { get; set; }

        [DefaultValue(typeof(Align), "None")]
        public Align Align
        {
            get { return AlignValue; }
            set
            {
                AlignValue = value;

                if (Parent is Control)
                    this.AlignBox();
            }
        }

        //DragDrop
        bool IsValidDropFile;
        public string DropFilePath;
        Image NewImage;
        Thread getDropImageThread;

        bool IsValidDragImage;
        Thread getDragImageThread;

        bool MovedImage;

        Dictionary<ImageFormats, string> filterMap = new Dictionary<ImageFormats, string>
        {
            { ImageFormats.Jpg, ".jpg" },
            { ImageFormats.Jpeg, ".jpeg" },
            { ImageFormats.Png, ".png" },
            { ImageFormats.Bmp, ".bmp" },
            { ImageFormats.Gif, ".gif" }
        };

        [Browsable(false)]
        public ImageFormats Filter { get; set; }

        bool GetFilter(ImageFormats value, string ext)
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

        public FlatPictureBox()
        {
            //SubMenu
            components = new Container();
            SizeChanged += OnSizeChanged;
            ParentChanged += OnParentChanged;
            DragEnter += OnDragEnter;
            DragDrop += OnDragDrop;
            MouseDown += OnMouseDown;

            Filter = (ImageFormats.Jpg | ImageFormats.Jpeg | ImageFormats.Png);

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
            mnuPicture.Items.AddRange(new ToolStripItem[]{
                mniCopyImage,
                mniRemoveImage
            });
            mnuPicture.ResumeLayout(false);

            mniCopyImage.MouseDown += MniCopyImage_MouseDown;
            mniRemoveImage.MouseDown += mniRemoveImage_MouseDown;
        }

        void OnParentChanged(object sender, EventArgs e)
        {
            if (Parent is Control)
            {
                this.AlignBox();
                Parent.SizeChanged += OnSizeChanged;
            }
        }

        void OnSizeChanged(object sender, EventArgs e)
        {
            if (Parent is Control)
            {
                this.AlignBox();
            }
        }

        void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || Image == null) return;

            var effect = DoDragDrop(Image, DragDropEffects.Move);

            if (effect != DragDropEffects.None && MovedImage)
                Image = null;
        }

        void MniCopyImage_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            ClipboardSafe.SetImage(Image);
        }

        void mniRemoveImage_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            Image = null;
        }

        #region Interpolation Property
        /// <summary>Backing Field</summary>
        InterpolationMode interpolation = InterpolationMode.NearestNeighbor;

        /// <summary>
        /// The interpolation used to render the image.
        /// </summary>
        [DefaultValue(typeof(InterpolationMode), "NearestNeighbor"),
        Description("The interpolation used to render the image.")]
        public InterpolationMode Interpolation
        {
            get { return interpolation; }
            set
            {
                if (value == InterpolationMode.Invalid)
                    throw new ArgumentException("\"Invalid\" is not a valid value."); // (Duh!)

                interpolation = value;
                Invalidate(); // Image should be redrawn when a different interpolation is selected
            }
        }
        #endregion

        /// <summary>
        /// Overridden to modify rendering behavior.
        /// </summary>
        /// <param name="pe">Painting event args.</param>
        protected override void OnPaint(PaintEventArgs pe)
        {
            // Before the PictureBox renders the image, we modify the
            // graphics object to change the interpolation.

            // Set the selected interpolation.
            pe.Graphics.InterpolationMode = interpolation;
            // Certain interpolation modes (such as nearest neighbor) need
            // to be offset by half a pixel to render correctly.
            if (interpolation == InterpolationMode.NearestNeighbor)
                pe.Graphics.PixelOffsetMode = PixelOffsetMode.Half;
            else
                pe.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            // Allow the PictureBox to draw.
            base.OnPaint(pe);
        }

        void OnDragEnter(object sender, DragEventArgs e)
        {
            string filename;
            IsValidDropFile = GetDropFilename(out filename, e);

            if (IsValidDropFile)
            {
                MovedImage = true;
                DropFilePath = filename;
                getDropImageThread = new Thread(new ThreadStart(LoadDroppedImage));
                getDropImageThread.Start();
                e.Effect = DragDropEffects.Copy;
                return;
            }

            IsValidDragImage = GetDragImage(out NewImage, e);

            if (IsValidDragImage)
            {
                MovedImage = true;
                getDragImageThread = new Thread(new ThreadStart(LoadDragImage));
                getDragImageThread.Start();
                e.Effect = DragDropEffects.Move;
                return;
            }

            e.Effect = DragDropEffects.None;
        }

        void OnDragDrop(object sender, DragEventArgs e)
        {
            if (!IsValidDropFile && !IsValidDragImage) return;

            if (IsValidDropFile)
            {
                while (getDropImageThread.IsAlive)
                {
                    Application.DoEvents();
                    Thread.Sleep(0);
                }
            }

            if (IsValidDragImage)
            {
                while (getDragImageThread.IsAlive)
                {
                    Application.DoEvents();
                    Thread.Sleep(0);
                }
            }

            if (NewImage.Size.Width <= Width && NewImage.Size.Height <= Height)
                Interpolation = InterpolationMode.NearestNeighbor;
            else
                Interpolation = InterpolationMode.HighQualityBicubic;

            MovedImage = false;
            Image = NewImage;
        }

        bool GetDragImage(out Image dragImage, DragEventArgs e)
        {
            dragImage = default(Bitmap);
            if ((e.AllowedEffect & DragDropEffects.Move) == DragDropEffects.Move)
            {
                var dataDrag = e.Data.GetData(DataFormats.Bitmap) as Bitmap;
                if (dataDrag == null) return false;

                dragImage = dataDrag;
                return true;
            }
            return false;
        }

        bool GetDropFilename(out string filename, DragEventArgs e)
        {
            filename = string.Empty;
            if ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
            {
                var dataDrop = e.Data.GetData(DataFormats.FileDrop) as Array;

                if (dataDrop == null) return false;

                if ((dataDrop.Length == 1) && (dataDrop.GetValue(0) is string))
                {
                    filename = ((string[])dataDrop)[0];
                    var ext = Path.GetExtension(filename).ToLower();
                    if (GetFilter(Filter, ext))
                        return true;
                }
            }
            return false;
        }

        void LoadDroppedImage()
        {
            NewImage = BitmapExtension.SuperFastLoad(DropFilePath);
        }

        void LoadDragImage()
        {
        }
    }
}