using System;
using System.Drawing;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public partial class PanelResizable : Panel
    {
        private readonly int borderSize = 8;
        private bool isResizeMode;
        private ResizeFace face;

        public PanelResizable()
        {
            InitializeComponent();

            Anchor = ((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right;
        }

        private enum ResizeFace
        {
            Top,
            TopRight,
            Right,
            BottomRight,
            Bottom,
            BottomLeft,
            Left,
            TopLeft
        }

        private void PanelResizable_MouseDown(object sender, MouseEventArgs e)
        {
            if (((Form)Parent).WindowState == FormWindowState.Maximized)
            {
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                isResizeMode = true;

                if (e.Y <= borderSize && e.X >= Location.X + Size.Width - borderSize)
                {
                    face = ResizeFace.TopRight;
                    Cursor = Cursors.SizeNESW;
                }
                else if (e.Y <= borderSize && e.X <= borderSize)
                {
                    face = ResizeFace.TopLeft;
                    Cursor = Cursors.SizeNWSE;
                }
                else if (e.X >= Location.X + Size.Width - borderSize && e.Y >= Location.Y + Size.Height - borderSize)
                {
                    face = ResizeFace.BottomRight;
                    Cursor = Cursors.SizeNWSE;
                }
                else if (e.X <= Location.X + borderSize && e.Y >= Location.Y + Size.Height - borderSize)
                {
                    face = ResizeFace.BottomLeft;
                    Cursor = Cursors.SizeNESW;
                }
                else if (e.X >= Location.X + Size.Width - borderSize)
                {
                    face = ResizeFace.Right;
                    Cursor = Cursors.SizeWE;
                }
                else if (e.Y >= Location.Y + Size.Height - borderSize)
                {
                    face = ResizeFace.Bottom;
                    Cursor = Cursors.SizeNS;
                }
                else if (e.Y <= borderSize)
                {
                    face = ResizeFace.Top;
                    Cursor = Cursors.SizeNS;
                }
                else if (e.X <= borderSize)
                {
                    face = ResizeFace.Left;
                    Cursor = Cursors.SizeWE;
                }
            }
        }

        private void PanelResizable_MouseMove(object sender, MouseEventArgs e)
        {
            if (((Form)Parent).WindowState == FormWindowState.Maximized)
            {
                return;
            }

            if (isResizeMode)
            {
                Size newSize;
                Point newLocation;

                if (face == ResizeFace.Top)
                {
                    newLocation = new Point(Parent.Location.X, Parent.Location.Y - (-e.Y));
                    newSize = new Size(Parent.Width, -e.Y + Parent.Height);
                    if (newSize.Height >= Parent.MinimumSize.Height)
                    {
                        Parent.Location = newLocation;
                        Parent.Size = newSize;
                    }

                    return;
                }

                if (face == ResizeFace.Bottom)
                {
                    newSize = new Size(Parent.Width, e.Y);
                    if (newSize.Height >= Parent.MinimumSize.Height)
                    {
                        Parent.Size = newSize;
                    }

                    return;
                }

                if (face == ResizeFace.Right)
                {
                    newSize = new Size(e.X, Parent.Height);
                    if (newSize.Width >= Parent.MinimumSize.Width)
                    {
                        Parent.Size = newSize;
                    }

                    return;
                }

                if (face == ResizeFace.Left)
                {
                    newLocation = new Point(Parent.Location.X - (-e.X), Parent.Location.Y);
                    newSize = new Size(-e.X + Parent.Width, Parent.Height);
                    if (newSize.Width >= Parent.MinimumSize.Width)
                    {
                        Parent.Location = newLocation;
                        Parent.Size = newSize;
                    }

                    return;
                }

                if (face == ResizeFace.BottomRight)
                {
                    newSize = new Size(e.X, e.Y);
                    if (newSize.Height >= Parent.MinimumSize.Height)
                    {
                        Parent.Size = new Size(Parent.Size.Width, newSize.Height);
                    }

                    if (newSize.Width >= Parent.MinimumSize.Width)
                    {
                        Parent.Size = new Size(newSize.Width, Parent.Size.Height);
                    }

                    return;
                }

                if (face == ResizeFace.BottomLeft)
                {
                    newLocation = new Point(Parent.Location.X - (-e.X), Parent.Location.Y);
                    newSize = new Size(-e.X + Parent.Width, e.Y);
                    if (newSize.Height >= Parent.MinimumSize.Height)
                    {
                        Parent.Size = new Size(Parent.Size.Width, newSize.Height);
                    }

                    if (newSize.Width >= Parent.MinimumSize.Width)
                    {
                        Parent.Location = newLocation;
                        Parent.Size = new Size(newSize.Width, Parent.Size.Height);
                    }

                    return;
                }

                if (face == ResizeFace.TopRight)
                {
                    newLocation = new Point(Parent.Location.X, Parent.Location.Y - (-e.Y));
                    newSize = new Size(e.X, -e.Y + Parent.Height);
                    if (newSize.Height >= Parent.MinimumSize.Height)
                    {
                        Parent.Location = newLocation;
                        Parent.Size = new Size(Parent.Size.Width, newSize.Height);
                    }

                    if (newSize.Width >= Parent.MinimumSize.Width)
                    {
                        Parent.Size = new Size(newSize.Width, Parent.Size.Height);
                    }

                    return;
                }

                if (face == ResizeFace.TopLeft)
                {
                    newLocation = new Point(Parent.Location.X - (-e.X), Parent.Location.Y - (-e.Y));
                    newSize = new Size(-e.X + Parent.Width, -e.Y + Parent.Height);
                    if (newSize.Height >= Parent.MinimumSize.Height)
                    {
                        Parent.Location = new Point(Parent.Location.X, newLocation.Y);
                        Parent.Size = new Size(Parent.Size.Width, newSize.Height);
                    }

                    if (newSize.Width >= Parent.MinimumSize.Width)
                    {
                        Parent.Location = new Point(newLocation.X, Parent.Location.Y);
                        Parent.Size = new Size(newSize.Width, Parent.Size.Height);
                    }

                    return;
                }
            }
            else
            {
                if (e.Y <= borderSize && e.X >= Location.X + Size.Width - borderSize)
                {
                    Cursor = Cursors.SizeNESW;
                }
                else if (e.Y <= borderSize && e.X <= borderSize)
                {
                    Cursor = Cursors.SizeNWSE;
                }
                else if (e.X >= Location.X + Size.Width - borderSize && e.Y >= Location.Y + Size.Height - borderSize)
                {
                    Cursor = Cursors.SizeNWSE;
                }
                else if (e.X <= Location.X + borderSize && e.Y >= Location.Y + Size.Height - borderSize)
                {
                    Cursor = Cursors.SizeNESW;
                }
                else if (e.X >= Location.X + Size.Width - borderSize)
                {
                    Cursor = Cursors.SizeWE;
                }
                else if (e.Y >= Location.Y + Size.Height - borderSize)
                {
                    Cursor = Cursors.SizeNS;
                }
                else if (e.Y <= borderSize)
                {
                    Cursor = Cursors.SizeNS;
                }
                else if (e.X <= borderSize)
                {
                    Cursor = Cursors.SizeWE;
                }
            }
        }

        private void PanelResizable_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            isResizeMode = false;
        }

        private void PanelResizable_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
            Refresh();
        }
    }
}