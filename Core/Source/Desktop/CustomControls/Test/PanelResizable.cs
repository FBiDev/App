using System;
using System.Drawing;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    enum ResizeFace
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

    public partial class PanelResizable : Panel
    {
        //[Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Bindable(true)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        //public override string Text { get; set; }

        //public string Text2 { get; set; }

        //[Description("Miau"), Category("Data")]
        //public string Text3 { get; set; }

        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        var cp = base.CreateParams;
        //        cp.Style |= 0x840000;  // Turn on WS_BORDER + WS_THICKFRAME
        //        return cp;
        //    }
        //}


        public PanelResizable()
        {
            InitializeComponent();

            Anchor = (((AnchorStyles.Top | AnchorStyles.Bottom)
            | AnchorStyles.Left)
            | AnchorStyles.Right);
        }

        bool isResizeMode;
        ResizeFace Face;
        readonly int PanelBorderSize = 8;

        void PanelResizable_MouseDown(object sender, MouseEventArgs e)
        {
            if (((Form)Parent).WindowState == FormWindowState.Maximized)
            {
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                isResizeMode = true;

                if (e.Y <= PanelBorderSize && e.X >= Location.X + Size.Width - PanelBorderSize)
                {
                    Face = ResizeFace.TopRight;
                    Cursor = Cursors.SizeNESW;
                }
                else if (e.Y <= PanelBorderSize && e.X <= PanelBorderSize)
                {
                    Face = ResizeFace.TopLeft;
                    Cursor = Cursors.SizeNWSE;
                }
                else if (e.X >= Location.X + Size.Width - PanelBorderSize && e.Y >= Location.Y + Size.Height - PanelBorderSize)
                {
                    Face = ResizeFace.BottomRight;
                    Cursor = Cursors.SizeNWSE;
                }
                else if (e.X <= Location.X + PanelBorderSize && e.Y >= Location.Y + Size.Height - PanelBorderSize)
                {
                    Face = ResizeFace.BottomLeft;
                    Cursor = Cursors.SizeNESW;
                }
                else if (e.X >= Location.X + Size.Width - PanelBorderSize)
                {
                    Face = ResizeFace.Right;
                    Cursor = Cursors.SizeWE;
                }
                else if (e.Y >= Location.Y + Size.Height - PanelBorderSize)
                {
                    Face = ResizeFace.Bottom;
                    Cursor = Cursors.SizeNS;
                }
                else if (e.Y <= PanelBorderSize)
                {
                    Face = ResizeFace.Top;
                    Cursor = Cursors.SizeNS;
                }
                else if (e.X <= PanelBorderSize)
                {
                    Face = ResizeFace.Left;
                    Cursor = Cursors.SizeWE;
                }
            }
        }

        void PanelResizable_MouseMove(object sender, MouseEventArgs e)
        {
            if (((Form)Parent).WindowState == FormWindowState.Maximized)
            {
                return;
            }

            if (isResizeMode)
            {
                Size NewSize;
                Point NewLocation;

                if (Face == ResizeFace.Top)
                {
                    NewLocation = new Point(Parent.Location.X, Parent.Location.Y - (-e.Y));
                    NewSize = new Size(Parent.Width, -e.Y + Parent.Height);
                    if (NewSize.Height >= Parent.MinimumSize.Height)
                    {
                        Parent.Location = NewLocation;
                        Parent.Size = NewSize;
                    }
                    return;
                }

                if (Face == ResizeFace.Bottom)
                {
                    NewSize = new Size(Parent.Width, e.Y);
                    if (NewSize.Height >= Parent.MinimumSize.Height)
                    {
                        Parent.Size = NewSize;
                    }
                    return;
                }

                if (Face == ResizeFace.Right)
                {
                    NewSize = new Size(e.X, Parent.Height);
                    if (NewSize.Width >= Parent.MinimumSize.Width)
                    {
                        Parent.Size = NewSize;
                    }
                    return;
                }

                if (Face == ResizeFace.Left)
                {
                    NewLocation = new Point(Parent.Location.X - (-e.X), Parent.Location.Y);
                    NewSize = new Size(-e.X + Parent.Width, Parent.Height);
                    if (NewSize.Width >= Parent.MinimumSize.Width)
                    {
                        Parent.Location = NewLocation;
                        Parent.Size = NewSize;
                    }
                    return;
                }

                if (Face == ResizeFace.BottomRight)
                {
                    NewSize = new Size(e.X, e.Y);
                    if (NewSize.Height >= Parent.MinimumSize.Height)
                    {
                        Parent.Size = new Size(Parent.Size.Width, NewSize.Height);
                    }

                    if (NewSize.Width >= Parent.MinimumSize.Width)
                    {
                        Parent.Size = new Size(NewSize.Width, Parent.Size.Height);
                    }
                    return;
                }

                if (Face == ResizeFace.BottomLeft)
                {
                    NewLocation = new Point(Parent.Location.X - (-e.X), Parent.Location.Y);
                    NewSize = new Size(-e.X + Parent.Width, e.Y);
                    if (NewSize.Height >= Parent.MinimumSize.Height)
                    {
                        Parent.Size = new Size(Parent.Size.Width, NewSize.Height);
                    }

                    if (NewSize.Width >= Parent.MinimumSize.Width)
                    {
                        Parent.Location = NewLocation;
                        Parent.Size = new Size(NewSize.Width, Parent.Size.Height);
                    }
                    return;
                }

                if (Face == ResizeFace.TopRight)
                {
                    NewLocation = new Point(Parent.Location.X, Parent.Location.Y - (-e.Y));
                    NewSize = new Size(e.X, -e.Y + Parent.Height);
                    if (NewSize.Height >= Parent.MinimumSize.Height)
                    {
                        Parent.Location = NewLocation;
                        Parent.Size = new Size(Parent.Size.Width, NewSize.Height);
                    }

                    if (NewSize.Width >= Parent.MinimumSize.Width)
                    {
                        Parent.Size = new Size(NewSize.Width, Parent.Size.Height);
                    }
                    return;
                }

                if (Face == ResizeFace.TopLeft)
                {
                    NewLocation = new Point(Parent.Location.X - (-e.X), Parent.Location.Y - (-e.Y));
                    NewSize = new Size(-e.X + Parent.Width, -e.Y + Parent.Height);
                    if (NewSize.Height >= Parent.MinimumSize.Height)
                    {
                        Parent.Location = new Point(Parent.Location.X, NewLocation.Y);
                        Parent.Size = new Size(Parent.Size.Width, NewSize.Height);
                    }

                    if (NewSize.Width >= Parent.MinimumSize.Width)
                    {
                        Parent.Location = new Point(NewLocation.X, Parent.Location.Y);
                        Parent.Size = new Size(NewSize.Width, Parent.Size.Height);
                    }
                    return;
                }
            }
            else
            {
                if (e.Y <= PanelBorderSize && e.X >= Location.X + Size.Width - PanelBorderSize)
                {
                    Cursor = Cursors.SizeNESW;
                }
                else if (e.Y <= PanelBorderSize && e.X <= PanelBorderSize)
                {
                    Cursor = Cursors.SizeNWSE;
                }
                else if (e.X >= Location.X + Size.Width - PanelBorderSize && e.Y >= Location.Y + Size.Height - PanelBorderSize)
                {
                    Cursor = Cursors.SizeNWSE;
                }
                else if (e.X <= Location.X + PanelBorderSize && e.Y >= Location.Y + Size.Height - PanelBorderSize)
                {
                    Cursor = Cursors.SizeNESW;
                }
                else if (e.X >= Location.X + Size.Width - PanelBorderSize)
                {
                    Cursor = Cursors.SizeWE;
                }
                else if (e.Y >= Location.Y + Size.Height - PanelBorderSize)
                {
                    Cursor = Cursors.SizeNS;
                }
                else if (e.Y <= PanelBorderSize)
                {
                    Cursor = Cursors.SizeNS;
                }
                else if (e.X <= PanelBorderSize)
                {
                    Cursor = Cursors.SizeWE;
                }
            }
        }

        void PanelResizable_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            isResizeMode = false;
        }

        void PanelResizable_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
            Refresh();
        }
    }
}