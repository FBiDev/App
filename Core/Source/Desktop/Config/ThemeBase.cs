using System;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public static class ThemeBase
    {
        private static ThemeNames selectedTheme;

        private static bool selectedThemeChanged;

        private static ThemeColor _colorSet;

        public enum ThemeNames
        {
            Light,
            Dark
        }

        public static ThemeColor ColorSet
        {
            get
            {
                return _colorSet;
            }

            set
            {
                _colorSet = value;

                if (_colorSet == null || selectedThemeChanged == false)
                {
                    return;
                }

                foreach (Form f in Application.OpenForms)
                {
                    CheckTheme(f);
                }
            }
        }

        public static bool ToggleDarkMode()
        {
            if (selectedTheme != ThemeNames.Dark)
            {
                SetTheme(ThemeNames.Dark);
                return true;
            }

            SetTheme(ThemeNames.Light);
            return false;
        }

        public static void SetTheme(ThemeNames newTheme)
        {
            selectedTheme = newTheme;
            selectedThemeChanged = true;

            switch (selectedTheme)
            {
                case ThemeNames.Light: ColorSet = new ThemeColorLight();
                    break;
                case ThemeNames.Dark: ColorSet = new ThemeColorDark();
                    break;
            }
        }

        public static void CheckControlTheme(Control control)
        {
            if (ColorSet.IsNull())
            {
                ColorSet = new ThemeColorLight();
            }

            if (control is FlatPanel)
            {
                ColorSet.FlatPanel(control as FlatPanel);
            }
            else if (control is FlatLabel)
            {
                ColorSet.FlatLabel(control as FlatLabel);
            }
            else if (control is FlatButton)
            {
                control = control as FlatButton;
                ColorSet.FlatButton(control as FlatButton);
                ((FlatButton)control).ResetColors();
            }
            else if (control is FlatCheckBox)
            {
                ColorSet.FlatCheckBox(control as FlatCheckBox);
                ((FlatCheckBox)control).ResetColors();
                ((FlatCheckBox)control).RefreshCheckboxColors();
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
            {
                ColorSet.FlatStatusBar(control as FlatStatusBar);
            }
            else if (control is FlatTabControl)
            {
                ColorSet.FlatTabControl(control as FlatTabControl);
            }
            else if (control is FlatPictureBox)
            {
                ColorSet.FlatPictureBox(control as FlatPictureBox);
            }
            else if (control is FlatGroupBox)
            {
                ColorSet.FlatGroupBox(control as FlatGroupBox);
            }
            else if (control is FlatListView)
            {
                ColorSet.FlatListView(control as FlatListView);
            }
            else if (control is FlatDataGrid)
            {
                ColorSet.FlatDataGrid(control as FlatDataGrid);
            }
            else if (control is ExeButton)
            {
                ColorSet.ButtonExe(control as ExeButton);
            }
            else if (control is RichTextBox)
            {
                ColorSet.RichTextBox(control as RichTextBox);
            }
        }

        public static void CheckTheme(Form f)
        {
            if (ColorSet == null)
            {
                return;
            }

            ColorSet.WindowForm(f);

            if (f is MainBaseForm)
            {
                ColorSet.MainBaseForm(f as MainBaseForm);
            }
            else if (f is ContentBaseForm)
            {
                ColorSet.ContentBaseForm(f as ContentBaseForm);
            }
            else if (f is Form)
            {
                ColorSet.Form(f);
            }

            foreach (var control in f.GetControls<Control>())
            {
                CheckControlTheme(control);
            }
        }

        public static void SetWindowDarkMode(IntPtr handle, int dark = 1)
        {
            Native.Window.SetWindowDarkMode(handle, dark);
        }
    }
}