using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public static class ThemeBase
    {
        public enum ThemeNames
        {
            Light,
            Dark
        }

        public static ThemeNames SelectedTheme { get; set; }
        static bool SelectedThemeChanged;

        static ThemeColor _ColorSet;
        public static ThemeColor ColorSet
        {
            get
            {
                return _ColorSet;
            }
            set
            {
                _ColorSet = value;

                if (_ColorSet == null) return;

                if (SelectedThemeChanged == false) return;

                foreach (Form f in Application.OpenForms)
                {
                    CheckTheme(f);
                }
            }
        }

        public static bool ToggleDarkMode()
        {
            if (SelectedTheme != ThemeNames.Dark)
            {
                SetTheme(ThemeNames.Dark);
                return true;
            }

            SetTheme(ThemeNames.Light);
            return false;
        }

        public static void SetTheme(ThemeNames newTheme)
        {
            SelectedTheme = newTheme;
            SelectedThemeChanged = true;

            switch (SelectedTheme)
            {
                case ThemeNames.Light: ColorSet = new ThemeColorLight(); break;
                case ThemeNames.Dark: ColorSet = new ThemeColorDark(); break;
            }
        }

        public static void CheckControlTheme(Control control)
        {
            if (ColorSet.IsNull()) ColorSet = new ThemeColorLight();

            if (control is FlatPanel)
                ColorSet.FlatPanel(control as FlatPanel);
            else if (control is FlatLabel)
                ColorSet.FlatLabel(control as FlatLabel);

            if (control is FlatButton)
            {
                control = control as FlatButton;
                ColorSet.FlatButton(control as FlatButton);
                ((FlatButton)control).ResetColors();
            }
            else if (control is FlatCheckBox)
            {
                ColorSet.FlatCheckBox(control as FlatCheckBox);
                ((FlatCheckBox)control).ResetColors();
            }
            else if (control is FlatCheckedList)
            {
                ColorSet.FlatCheckedList(control as FlatCheckedList);
                ((FlatCheckedList)control).ResetColors();
            }
            else if (control is FlatTextBox)
            {
                ColorSet.FlatTextBox(control as FlatTextBox);
                ((FlatTextBox)control).ResetColors();
            }
            else if (control is FlatMaskedTextBox)
            {
                ColorSet.FlatMaskedTextBox(control as FlatMaskedTextBox);
                ((FlatMaskedTextBox)control).ResetColors();
            }
            else if (control is FlatComboBox)
            {
                ColorSet.FlatComboBox(control as FlatComboBox);
                ((FlatComboBox)control).ResetColors();
            }
            else if (control is FlatStatusBar)
                ColorSet.FlatStatusBar(control as FlatStatusBar);
            else if (control is FlatTabControl)
                ColorSet.FlatTabControl(control as FlatTabControl);
            else if (control is FlatPictureBox)
                ColorSet.FlatPictureBox(control as FlatPictureBox);
            else if (control is FlatGroupBox)
                ColorSet.FlatGroupBox(control as FlatGroupBox);
            else if (control is FlatListView)
                ColorSet.FlatListView(control as FlatListView);
            else if (control is FlatDataGrid)
                ColorSet.FlatDataGrid(control as FlatDataGrid);
            else if (control is ExeButton)
                ColorSet.ButtonExe(control as ExeButton);
            else if (control is RichTextBox)
                ColorSet.RichTextBox(control as RichTextBox);
        }

        public static void CheckTheme(Form f)
        {
            if (ColorSet == null) return;

            ColorSet.WindowForm(f);

            if (f is MainBaseForm)
                ColorSet.MainBaseForm(f as MainBaseForm);
            else if (f is ContentBaseForm)
                ColorSet.ContentBaseForm(f as ContentBaseForm);
            else if (f is Form)
                ColorSet.Form(f);

            foreach (var control in f.GetControls<Control>())
            {
                CheckControlTheme(control);
            }
        }

        public static void SetWindowDark(IntPtr handle, int dark = 1)
        {
            DwmSetWindowAttribute(handle, DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE, ref dark, sizeof(uint));
        }

        [DllImport("dwmapi.dll", CharSet = CharSet.Unicode, PreserveSig = true)]
        static extern void DwmSetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE attribute, ref int pvAttribute, uint cbAttribute);

        enum DWMWINDOWATTRIBUTE : uint
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
}