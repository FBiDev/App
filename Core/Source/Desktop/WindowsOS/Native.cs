﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public static class Native
    {
        // GETOS
        #region OSVERSIONINFOEX
        [DllImport("kernel32.dll")]
        internal static extern bool GetVersionEx(ref OSVERSIONINFOEX osVersionInfo);

        [StructLayout(LayoutKind.Sequential)]
        internal struct OSVERSIONINFOEX
        {
            public int dwOSVersionInfoSize;
            public int dwMajorVersion;
            public int dwMinorVersion;
            public int dwBuildNumber;
            public int dwPlatformId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szCSDVersion;
            public short wServicePackMajor;
            public short wServicePackMinor;
            public short wSuiteMask;
            public byte wProductType;
            public byte wReserved;
        }
        #endregion OSVERSIONINFOEX

        internal struct Message
        {
            // WM Windows Message
            // MN Menu Notification

            public const int
                WM_NULL = 0x0000,
                WM_CREATE = 0x0001,
                WM_DESTROY = 0x0002,
                WM_MOVE = 0x0003,
                WM_SIZE = 0x0005,
                WM_ACTIVATE = 0x0006,
                WM_SETFOCUS = 0x0007,
                WM_KILLFOCUS = 0x0008,
                WM_ENABLE = 0x000A,
                WM_SETREDRAW = 0x000B,
                WM_SETTEXT = 0x000C,
                WM_GETTEXT = 0x000D,
                WM_GETTEXTLENGTH = 0x000E,
                WM_PAINT = 0x000F,
                WM_CLOSE = 0x0010,
                WM_QUERYENDSESSION = 0x0011,
                WM_QUERYOPEN = 0x0013,
                WM_ENDSESSION = 0x0016,
                WM_QUIT = 0x0012,
                WM_ERASEBKGND = 0x0014,
                WM_SYSCOLORCHANGE = 0x0015,
                WM_SHOWWINDOW = 0x0018,
                WM_WININICHANGE = 0x001A,
                WM_SETTINGCHANGE = WM_WININICHANGE,
                WM_DEVMODECHANGE = 0x001B,
                WM_ACTIVATEAPP = 0x001C,
                WM_FONTCHANGE = 0x001D,
                WM_TIMECHANGE = 0x001E,
                WM_CANCELMODE = 0x001F,
                WM_SETCURSOR = 0x0020,
                WM_MOUSEACTIVATE = 0x0021,
                WM_CHILDACTIVATE = 0x0022,
                WM_QUEUESYNC = 0x0023,
                WM_GETMINMAXINFO = 0x0024,
                WM_PAINTICON = 0x0026,
                WM_ICONERASEBKGND = 0x0027,
                WM_NEXTDLGCTL = 0x0028,
                WM_SPOOLERSTATUS = 0x002A,
                WM_DRAWITEM = 0x002B,
                WM_MEASUREITEM = 0x002C,
                WM_DELETEITEM = 0x002D,
                WM_VKEYTOITEM = 0x002E,
                WM_CHARTOITEM = 0x002F,
                WM_SETFONT = 0x0030,
                WM_GETFONT = 0x0031,
                WM_SETHOTKEY = 0x0032,
                WM_GETHOTKEY = 0x0033,
                WM_QUERYDRAGICON = 0x0037,
                WM_COMPAREITEM = 0x0039,
                WM_GETOBJECT = 0x003D,
                WM_COMPACTING = 0x0041,
                WM_COMMNOTIFY = 0x0044,
                WM_WINDOWPOSCHANGING = 0x0046,
                WM_WINDOWPOSCHANGED = 0x0047,
                WM_POWER = 0x0048,
                WM_COPYDATA = 0x004A,
                WM_CANCELJOURNAL = 0x004B,
                WM_NOTIFY = 0x004E,
                WM_INPUTLANGCHANGEREQUEST = 0x0050,
                WM_INPUTLANGCHANGE = 0x0051,
                WM_TCARD = 0x0052,
                WM_HELP = 0x0053,
                WM_USERCHANGED = 0x0054,
                WM_NOTIFYFORMAT = 0x0055,
                WM_CONTEXTMENU = 0x007B,
                WM_STYLECHANGING = 0x007C,
                WM_STYLECHANGED = 0x007D,
                WM_DISPLAYCHANGE = 0x007E,
                WM_GETICON = 0x007F,
                WM_SETICON = 0x0080,
                WM_NCCREATE = 0x0081,
                WM_NCDESTROY = 0x0082,
                WM_NCCALCSIZE = 0x0083,
                WM_NCHITTEST = 0x0084,
                WM_NCPAINT = 0x0085,
                WM_NCACTIVATE = 0x0086,
                WM_GETDLGCODE = 0x0087,
                WM_SYNCPAINT = 0x0088,

                WM_NCMOUSEMOVE = 0x00A0,
                WM_NCLBUTTONDOWN = 0x00A1,
                WM_NCLBUTTONUP = 0x00A2,
                WM_NCLBUTTONDBLCLK = 0x00A3,
                WM_NCRBUTTONDOWN = 0x00A4,
                WM_NCRBUTTONUP = 0x00A5,
                WM_NCRBUTTONDBLCLK = 0x00A6,
                WM_NCMBUTTONDOWN = 0x00A7,
                WM_NCMBUTTONUP = 0x00A8,
                WM_NCMBUTTONDBLCLK = 0x00A9,
                WM_NCXBUTTONDOWN = 0x00AB,
                WM_NCXBUTTONUP = 0x00AC,
                WM_NCXBUTTONDBLCLK = 0x00AD,

                WM_INPUT_DEVICE_CHANGE = 0x00FE,
                WM_INPUT = 0x00FF,

                WM_KEYFIRST = 0x0100,
                WM_KEYDOWN = 0x0100,
                WM_KEYUP = 0x0101,
                WM_CHAR = 0x0102,
                WM_DEADCHAR = 0x0103,
                WM_SYSKEYDOWN = 0x0104,
                WM_SYSKEYUP = 0x0105,
                WM_SYSCHAR = 0x0106,
                WM_SYSDEADCHAR = 0x0107,
                WM_UNICHAR = 0x0109,
                WM_KEYLAST = 0x0109,

                WM_IME_STARTCOMPOSITION = 0x010D,
                WM_IME_ENDCOMPOSITION = 0x010E,
                WM_IME_COMPOSITION = 0x010F,
                WM_IME_KEYLAST = WM_IME_COMPOSITION,

                WM_INITDIALOG = 0x0110,
                WM_COMMAND = 0x0111,
                WM_SYSCOMMAND = 0x0112,
                WM_TIMER = 0x0113,
                WM_HSCROLL = 0x0114,
                WM_VSCROLL = 0x0115,
                WM_INITMENU = 0x0116,
                WM_INITMENUPOPUP = 0x0117,
                WM_MENUSELECT = 0x011F,
                WM_MENUCHAR = 0x0120,
                WM_ENTERIDLE = 0x0121,
                WM_MENURBUTTONUP = 0x0122,
                WM_MENUDRAG = 0x0123,
                WM_MENUGETOBJECT = 0x0124,
                WM_UNINITMENUPOPUP = 0x0125,
                WM_MENUCOMMAND = 0x0126,

                WM_CHANGEUISTATE = 0x0127,
                WM_UPDATEUISTATE = 0x0128,
                WM_QUERYUISTATE = 0x0129,

                WM_CTLCOLORMSGBOX = 0x0132,
                WM_CTLCOLOREDIT = 0x0133,
                WM_CTLCOLORLISTBOX = 0x0134,
                WM_CTLCOLORBTN = 0x0135,
                WM_CTLCOLORDLG = 0x0136,
                WM_CTLCOLORSCROLLBAR = 0x0137,
                WM_CTLCOLORSTATIC = 0x0138,
                MN_GETHMENU = 0x01E1,

                WM_MOUSEFIRST = 0x0200,
                WM_MOUSEMOVE = WM_MOUSEFIRST,
                WM_LBUTTONDOWN = 0x0201,
                WM_LBUTTONUP = 0x0202,
                WM_LBUTTONDBLCLK = 0x0203,
                WM_RBUTTONDOWN = 0x0204,
                WM_RBUTTONUP = 0x0205,
                WM_RBUTTONDBLCLK = 0x0206,
                WM_MBUTTONDOWN = 0x0207,
                WM_MBUTTONUP = 0x0208,
                WM_MBUTTONDBLCLK = 0x0209,
                WM_MOUSEWHEEL = 0x020A,
                WM_XBUTTONDOWN = 0x020B,
                WM_XBUTTONUP = 0x020C,
                WM_XBUTTONDBLCLK = 0x020D,
                WM_MOUSEHWHEEL = 0x020E,

                WM_PARENTNOTIFY = 0x0210,
                WM_ENTERMENULOOP = 0x0211,
                WM_EXITMENULOOP = 0x0212,

                WM_NEXTMENU = 0x0213,
                WM_SIZING = 0x0214,
                WM_CAPTURECHANGED = 0x0215,
                WM_MOVING = 0x0216,

                WM_POWERBROADCAST = 0x0218,

                WM_DEVICECHANGE = 0x0219,

                WM_MDICREATE = 0x0220,
                WM_MDIDESTROY = 0x0221,
                WM_MDIACTIVATE = 0x0222,
                WM_MDIRESTORE = 0x0223,
                WM_MDINEXT = 0x0224,
                WM_MDIMAXIMIZE = 0x0225,
                WM_MDITILE = 0x0226,
                WM_MDICASCADE = 0x0227,
                WM_MDIICONARRANGE = 0x0228,
                WM_MDIGETACTIVE = 0x0229,

                WM_MDISETMENU = 0x0230,
                WM_ENTERSIZEMOVE = 0x0231,
                WM_EXITSIZEMOVE = 0x0232,
                WM_DROPFILES = 0x0233,
                WM_MDIREFRESHMENU = 0x0234,

                WM_IME_SETCONTEXT = 0x0281,
                WM_IME_NOTIFY = 0x0282,
                WM_IME_CONTROL = 0x0283,
                WM_IME_COMPOSITIONFULL = 0x0284,
                WM_IME_SELECT = 0x0285,
                WM_IME_CHAR = 0x0286,
                WM_IME_REQUEST = 0x0288,
                WM_IME_KEYDOWN = 0x0290,
                WM_IME_KEYUP = 0x0291,

                WM_MOUSEHOVER = 0x02A1,
                WM_MOUSELEAVE = 0x02A3,
                WM_NCMOUSEHOVER = 0x02A0,
                WM_NCMOUSELEAVE = 0x02A2,

                WM_WTSSESSION_CHANGE = 0x02B1,

                WM_TABLET_FIRST = 0x02c0,
                WM_TABLET_LAST = 0x02df,

                WM_CUT = 0x0300,
                WM_COPY = 0x0301,
                WM_PASTE = 0x0302,
                WM_CLEAR = 0x0303,
                WM_UNDO = 0x0304,
                WM_RENDERFORMAT = 0x0305,
                WM_RENDERALLFORMATS = 0x0306,
                WM_DESTROYCLIPBOARD = 0x0307,
                WM_DRAWCLIPBOARD = 0x0308,
                WM_PAINTCLIPBOARD = 0x0309,
                WM_VSCROLLCLIPBOARD = 0x030A,
                WM_SIZECLIPBOARD = 0x030B,
                WM_ASKCBFORMATNAME = 0x030C,
                WM_CHANGECBCHAIN = 0x030D,
                WM_HSCROLLCLIPBOARD = 0x030E,
                WM_QUERYNEWPALETTE = 0x030F,
                WM_PALETTEISCHANGING = 0x0310,
                WM_PALETTECHANGED = 0x0311,
                WM_HOTKEY = 0x0312,

                WM_PRINT = 0x0317,
                WM_PRINTCLIENT = 0x0318,

                WM_APPCOMMAND = 0x0319,

                WM_THEMECHANGED = 0x031A,

                WM_CLIPBOARDUPDATE = 0x031D,

                WM_DWMCOMPOSITIONCHANGED = 0x031E,
                WM_DWMNCRENDERINGCHANGED = 0x031F,
                WM_DWMCOLORIZATIONCOLORCHANGED = 0x0320,
                WM_DWMWINDOWMAXIMIZEDCHANGE = 0x0321,

                WM_GETTITLEBARINFOEX = 0x033F,

                WM_HANDHELDFIRST = 0x0358,
                WM_HANDHELDLAST = 0x035F,

                WM_AFXFIRST = 0x0360,
                WM_AFXLAST = 0x037F,

                WM_PENWINFIRST = 0x0380,
                WM_PENWINLAST = 0x038F,

                WM_USER = 0x0400,
                WM_APP = 0x8000,

                WM_REFLECT = WM_USER + 0x1C00;
        }

        public static class Mouse
        {
            [Flags]
            private enum MouseEventFlags
            {
                MOVE = 0x00000001,
                LEFTDOWN = 0x00000002,
                LEFTUP = 0x00000004,
                RIGHTDOWN = 0x00000008,
                RIGHTUP = 0x00000010,
                MIDDLEDOWN = 0x00000020,
                MIDDLEUP = 0x00000040,
                ABSOLUTE = 0x00008000,
            }

            public static void LeftClick(Point location)
            {
                var prevLocation = Cursor.Position;
                Cursor.Position = new Point(location.X, location.Y);
                mouse_event((int)MouseEventFlags.LEFTDOWN, 0, 0, 0, 0);
                mouse_event((int)MouseEventFlags.LEFTUP, 0, 0, 0, 0);
                Cursor.Position = prevLocation;
            }

            [DllImport("user32.dll")]
            private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
        }

        public static class Process
        {
            [DllImport("psapi.dll")]
            public static extern int EmptyWorkingSet(IntPtr hwProc);
        }

        public static class FileDialog
        {
            ////#pragma warning disable IDE1006 // Naming Styles
            internal const int CANCELED = unchecked((int)0x800704C7);
            ////#pragma warning restore IDE1006 // Naming Styles

            ////#pragma warning disable CA1712 // Do not prefix enum values with type name
            internal enum SIGDN : uint
            {
                SIGDN_DESKTOPABSOLUTEEDITING = 0x8004c000,
                SIGDN_DESKTOPABSOLUTEPARSING = 0x80028000,
                SIGDN_FILESYSPATH = 0x80058000,
                SIGDN_NORMALDISPLAY = 0,
                SIGDN_PARENTRELATIVE = 0x80080001,
                SIGDN_PARENTRELATIVEEDITING = 0x80031001,
                SIGDN_PARENTRELATIVEFORADDRESSBAR = 0x8007c001,
                SIGDN_PARENTRELATIVEPARSING = 0x80018001,
                SIGDN_URL = 0x80068000
            }

            [Flags]
            internal enum FOS
            {
                FOS_OVERWRITEPROMPT = 0x2,
                FOS_STRICTFILETYPES = 0x4,
                FOS_NOCHANGEDIR = 0x8,
                FOS_PICKFOLDERS = 0x20,
                FOS_FORCEFILESYSTEM = 0x40,
                FOS_ALLNONSTORAGEITEMS = 0x80,
                FOS_NOVALIDATE = 0x100,
                FOS_ALLOWMULTISELECT = 0x200,
                FOS_PATHMUSTEXIST = 0x800,
                FOS_FILEMUSTEXIST = 0x1000,
                FOS_CREATEPROMPT = 0x2000,
                FOS_SHAREAWARE = 0x4000,
                FOS_NOREADONLYRETURN = 0x8000,
                FOS_NOTESTFILECREATE = 0x10000,
                FOS_HIDEMRUPLACES = 0x20000,
                FOS_HIDEPINNEDPLACES = 0x40000,
                FOS_NODEREFERENCELINKS = 0x100000,
                FOS_OKBUTTONNEEDSINTERACTION = 0x200000,
                FOS_DONTADDTORECENT = 0x2000000,
                FOS_FORCESHOWHIDDEN = 0x10000000,
                FOS_DEFAULTNOMINIMODE = 0x20000000,
                FOS_FORCEPREVIEWPANEON = 0x40000000,
                FOS_SUPPORTSTREAMABLEITEMS = unchecked((int)0x80000000)
            }
            ////#pragma warning restore CA1712 // Do not prefix enum values with type name

            [ComImport, Guid("d57c7288-d4ad-4768-be02-9d969532d960"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
            internal interface IFileOpenDialog
            {
                [PreserveSig]
                int Show(IntPtr parent); // IModalWindow
                [PreserveSig]
                int SetFileTypes();  // not fully defined
                [PreserveSig]
                int SetFileTypeIndex(int iFileType);
                [PreserveSig]
                int GetFileTypeIndex(out int piFileType);
                [PreserveSig]
                int Advise(); // not fully defined
                [PreserveSig]
                int Unadvise();
                [PreserveSig]
                int SetOptions(FOS fos);
                [PreserveSig]
                int GetOptions(out FOS pfos);
                [PreserveSig]
                int SetDefaultFolder(IShellItem psi);
                [PreserveSig]
                int SetFolder(IShellItem psi);
                [PreserveSig]
                int GetFolder(out IShellItem ppsi);
                [PreserveSig]
                int GetCurrentSelection(out IShellItem ppsi);
                [PreserveSig]
                int SetFileName([MarshalAs(UnmanagedType.LPWStr)] string pszName);
                [PreserveSig]
                int GetFileName([MarshalAs(UnmanagedType.LPWStr)] out string pszName);
                [PreserveSig]
                int SetTitle([MarshalAs(UnmanagedType.LPWStr)] string pszTitle);
                [PreserveSig]
                int SetOkButtonLabel([MarshalAs(UnmanagedType.LPWStr)] string pszText);
                [PreserveSig]
                int SetFileNameLabel([MarshalAs(UnmanagedType.LPWStr)] string pszLabel);
                [PreserveSig]
                int GetResult(out IShellItem ppsi);
                [PreserveSig]
                int AddPlace(IShellItem psi, int alignment);
                [PreserveSig]
                int SetDefaultExtension([MarshalAs(UnmanagedType.LPWStr)] string pszDefaultExtension);
                [PreserveSig]
                int Close(int hr);
                [PreserveSig]
                int SetClientGuid();  // not fully defined
                [PreserveSig]
                int ClearClientData();
                [PreserveSig]
                int SetFilter([MarshalAs(UnmanagedType.IUnknown)] object pFilter);
                [PreserveSig]
                int GetResults(out IShellItemArray ppenum);
                [PreserveSig]
                int GetSelectedItems([MarshalAs(UnmanagedType.IUnknown)] out object ppsai);
            }

            [ComImport, Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
            internal interface IShellItem
            {
                [PreserveSig]
                int BindToHandler(); // not fully defined
                [PreserveSig]
                int GetParent(); // not fully defined
                [PreserveSig]
                int GetDisplayName(SIGDN sigdnName, [MarshalAs(UnmanagedType.LPWStr)] out string ppszName);
                [PreserveSig]
                int GetAttributes();  // not fully defined
                [PreserveSig]
                int Compare();  // not fully defined
            }

            [ComImport, Guid("b63ea76d-1f85-456f-a19c-48159efa858b"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
            internal interface IShellItemArray
            {
                [PreserveSig]
                int BindToHandler();  // not fully defined
                [PreserveSig]
                int GetPropertyStore();  // not fully defined
                [PreserveSig]
                int GetPropertyDescriptionList();  // not fully defined
                [PreserveSig]
                int GetAttributes();  // not fully defined
                [PreserveSig]
                int GetCount(out int pdwNumItems);
                [PreserveSig]
                int GetItemAt(int dwIndex, out IShellItem ppsi);
                [PreserveSig]
                int EnumItems();  // not fully defined
            }

            internal static KeyValuePair<int, IShellItem> CreateShellItem(string path)
            {
                IShellItem item;
                int hr = SHCreateItemFromParsingName(path, null, typeof(IShellItem).GUID, out item);
                return new KeyValuePair<int, IShellItem>(hr, item);
            }

            [DllImport("shell32.dll")]
            private static extern int SHCreateItemFromParsingName([MarshalAs(UnmanagedType.LPWStr)] string pszPath, IBindCtx pbc, [MarshalAs(UnmanagedType.LPStruct)] Guid riid, out IShellItem ppv);
        }

        internal class SubNativeWindow : NativeWindow
        {
            private bool isSubClassed;

            public SubNativeWindow(IntPtr handle, bool subClass)
            {
                AssignHandle(handle);
                isSubClassed = subClass;
            }

            public delegate int SubClassWndProcEventHandler(ref System.Windows.Forms.Message m);

            public event SubClassWndProcEventHandler SubClassedWndProc;

            public bool SubClassed
            {
                get { return isSubClassed; }
                set { isSubClassed = value; }
            }

            public void CallDefaultWndProc(ref System.Windows.Forms.Message m)
            {
                base.WndProc(ref m);
            }

            #region HiWord Message Cracker
            public int HiWord(int number)
            {
                return (number >> 16) & 0xffff;
            }
            #endregion

            #region LoWord Message Cracker
            public int LoWord(int number)
            {
                return number & 0xffff;
            }
            #endregion

            #region MakeLong Message Cracker
            public int MakeLong(int loWord, int hiWord)
            {
                return (hiWord << 16) | (loWord & 0xffff);
            }
            #endregion

            #region MakeLParam Message Cracker
            public IntPtr MakeLParam(int loWord, int hiWord)
            {
                return (IntPtr)((hiWord << 16) | (loWord & 0xffff));
            }
            #endregion

            protected override void WndProc(ref System.Windows.Forms.Message m)
            {
                if (isSubClassed)
                {
                    if (OnSubClassedWndProc(ref m) != 0)
                    {
                        return;
                    }
                }

                base.WndProc(ref m);
            }

            private int OnSubClassedWndProc(ref System.Windows.Forms.Message m)
            {
                if (SubClassedWndProc != null)
                {
                    return SubClassedWndProc(ref m);
                }

                return 0;
            }
        }

        public static class Window
        {
            // HT Hit Test
            // GW Get Window
            // GWL Get Window Long
            // WS Window Style
            // WS_EX Extended Window Style
            // SWP Set Window Posistion
            // DWMWA Desktop Window Manager Window Attribute

            public struct Flag
            {
                public static readonly IntPtr HT_CAPTION = new IntPtr(0x0002);
                public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

                public const int
                    // GW_HWNDFIRST = 0,
                    // GW_HWNDLAST = 1,
                GW_HWNDNEXT = 2,
                    // GW_HWNDPREV = 3,
                    // GW_OWNER = 4,
                GW_CHILD = 5;

                public const int
                GWL_EXSTYLE = -20;

                public const int
                WS_EX_CLIENTEDGE = 0x0200,
                WS_EX_COMPOSITED = 0x02000000;

                public const int
                SWP_ASYNCWINDOWPOS = 0x4000,    // Posts the request to the window's thread if threads differ
                SWP_DEFERERASE = 0x2000,        // Prevents generation of WM_SYNCPAINT
                SWP_DRAWFRAME = 0x0020,         // Draws a frame around the window (same as SWP_FRAMECHANGED)
                SWP_FRAMECHANGED = 0x0020,      // Applies new frame styles and sends WM_NCCALCSIZE
                SWP_HIDEWINDOW = 0x0080,        // Hides the window
                SWP_NOACTIVATE = 0x0010,        // Prevents the window from being activated
                SWP_NOCOPYBITS = 0x0100,        // Discards the contents of the client area
                SWP_NOMOVE = 0x0002,            // Retains the current position
                SWP_NOOWNERZORDER = 0x0200,     // Does not change owner window's Z order
                SWP_NOREDRAW = 0x0008,          // Suppresses redraw of the window
                SWP_NOREPOSITION = 0x0200,      // Same as SWP_NOOWNERZORDER
                SWP_NOSENDCHANGING = 0x0400,    // Prevents sending WM_WINDOWPOSCHANGING
                SWP_NOSIZE = 0x0001,            // Retains the current size
                SWP_NOZORDER = 0x0004,          // Retains the current Z order
                SWP_SHOWWINDOW = 0x0040;        // Makes the window visible

                public enum DWMWINDOWATTRIBUTE : uint
                {
                    DWMWA_NCRENDERING_ENABLED,
                    DWMWA_NCRENDERING_POLICY,
                    DWMWA_TRANSITIONS_FORCEDISABLED,
                    DWMWA_ALLOW_NCPAINT,
                    DWMWA_CAPTION_BUTTON_BOUNDS,
                    DWMWA_NONCLIENT_RTL_LAYOUT,
                    DWMWA_FORCE_ICONIC_REPRESENTATION,
                    DWMWA_FLIP3D_POLICY,
                    DWMWA_EXTENDED_FRAME_BOUNDS,
                    DWMWA_HAS_ICONIC_BITMAP,
                    DWMWA_DISALLOW_PEEK,
                    DWMWA_EXCLUDED_FROM_PEEK,
                    DWMWA_CLOAK,
                    DWMWA_CLOAKED,
                    DWMWA_FREEZE_REPRESENTATION,
                    DWMWA_PASSIVE_UPDATE_MODE,
                    DWMWA_USE_HOSTBACKDROPBRUSH,
                    DWMWA_USE_IMMERSIVE_DARK_MODE = 20,
                    DWMWA_WINDOW_CORNER_PREFERENCE = 33,
                    DWMWA_BORDER_COLOR,
                    DWMWA_CAPTION_COLOR,
                    DWMWA_TEXT_COLOR,
                    DWMWA_VISIBLE_FRAME_BORDER_THICKNESS,
                    DWMWA_SYSTEMBACKDROP_TYPE,
                    DWMWA_LAST
                }
            }

            public static void AltMenuDisable(Form f)
            {
                f.KeyDown -= SuppressAltKey;
                f.KeyDown += SuppressAltKey;
            }

            public static void SendKey(Keys key)
            {
                switch (key)
                {
                    case Keys.Enter: SendKeys.Send("{ENTER}");
                        break;
                    case Keys.Tab: SendKeys.Send("{TAB}");
                        break;
                    case Keys.Escape: SendKeys.Send("{ESCAPE}");
                        break;
                }
            }

            public static IntPtr SendMessageInternal(IntPtr handle)
            {
                return SendMessage(handle, Message.WM_NCLBUTTONDOWN, Flag.HT_CAPTION, IntPtr.Zero);
            }

            public static void SetWindowDarkMode(IntPtr hwnd, int dark_mode = 1)
            {
                DwmSetWindowAttribute(hwnd, Flag.DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE, ref dark_mode, sizeof(uint));
            }

            private static void SuppressAltKey(object sender, KeyEventArgs e)
            {
                if (e.KeyData == (Keys.RButton | Keys.ShiftKey | Keys.Alt))
                {
                    e.SuppressKeyPress = true;
                }
            }

            [DllImport("Gdi32.dll", CharSet = CharSet.Auto)]
            public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

            [DllImport("dwmapi.dll", CharSet = CharSet.Unicode, PreserveSig = true)]
            private static extern void DwmSetWindowAttribute(IntPtr hwnd, Flag.DWMWINDOWATTRIBUTE attribute, ref int pvAttribute, uint cbAttribute);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern bool InvalidateRect(IntPtr hwnd, ref Rectangle rect, bool bErase);

            [DllImport("User32.dll", CharSet = CharSet.Auto)]
            public static extern bool IsWindowVisible(IntPtr hwnd);

            [DllImport("User32.dll", CharSet = CharSet.Auto)]
            public static extern int GetClassName(IntPtr hwnd, char[] className, int maxCount);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern int GetClientRect(IntPtr hwnd, ref RECT lpRect);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern int GetClientRect(IntPtr hwnd, [In, Out] ref Rectangle rect);

            [DllImport("user32.dll")]
            internal static extern IntPtr GetDesktopWindow();

            [DllImport("User32.dll", CharSet = CharSet.Auto)]
            public static extern IntPtr GetWindow(IntPtr hwnd, int uCmd);

            [DllImport("User32.dll", CharSet = CharSet.Auto)]
            public static extern IntPtr GetWindowDC(IntPtr handle);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            internal static extern bool GetWindowRect(IntPtr hWnd, [In, Out] ref Rectangle rect);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern bool MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool bRepaint);

            [DllImport("user32.dll")]
            private static extern bool ReleaseCapture();

            [DllImport("User32.dll", CharSet = CharSet.Auto)]
            public static extern IntPtr ReleaseDC(IntPtr handle, IntPtr hDC);

            [DllImport("user32.dll")]
            private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

            [DllImport("user32.dll", SetLastError = true)]
            public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int uFlags);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern bool UpdateWindow(IntPtr hwnd);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern bool ValidateRect(IntPtr hwnd, ref Rectangle rect);

            [StructLayout(LayoutKind.Sequential)]
            public struct NCCALCSIZE_PARAMS
            {
                public RECT Rgc;
                public WINDOWPOS Wndpos;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct RECT
            {
                public int Left;
                public int Top;
                public int Right;
                public int Bottom;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct WINDOWPOS
            {
                public IntPtr Hwnd;
                public IntPtr HwndAfter;
                public int X;
                public int Y;
                public int Cx;
                public int Cy;
                public uint Flags;
            }
        }

        public static class Console
        {
            public struct Flag
            {
                public const int
                STD_INPUT_HANDLE = -10,
                ENABLE_QUICK_EDIT_MODE = 0x0040,
                ENABLE_EXTENDED_FLAGS = 0x0080;
            }

            [DllImport("kernel32.dll")]
            public static extern bool AllocConsole();

            [DllImport("kernel32.dll")]
            public static extern bool FreeConsole();

            [DllImport("kernel32.dll")]
            public static extern IntPtr GetConsoleWindow();

            [DllImport("kernel32.dll")]
            public static extern IntPtr GetStdHandle(int nStdHandle);

            [DllImport("kernel32.dll")]
            public static extern bool SetConsoleMode(IntPtr hConsoleHandle, int dwMode);
        }
    }
}