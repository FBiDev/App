using System.Drawing;
using System.Windows.Forms;
using App.Core.Properties;

namespace App.Core.Desktop
{
    public class ThemeColorDark : ThemeColor
    {
        public override void WindowForm(Form f) { ThemeBase.SetWindowDark(f.Handle, 1); }
        public override void MainBaseForm(MainBaseForm f) { f.BackColor = Colors.RGB(53, 53, 53); }
        public override void ContentBaseForm(ContentBaseForm f) { f.BackColor = Colors.RGB(53, 53, 53); }
        public override void Form(Form f) { f.BackColor = Colors.RGB(53, 53, 53); }

        public override void FlatPanel(FlatPanel c)
        {
            c.ForeColor = Colors.RGB(210, 210, 210);
            c.BorderColor = Colors.RGB(86, 86, 86);

            switch (c.BackColorType)
            {
                case PanelType.transparent: c.BackColor = Color.Transparent; break;
                case PanelType.control: c.BackColor = Colors.RGB(53, 53, 53); break;
                case PanelType.controlLight: c.BackColor = Colors.RGB(53, 53, 53); break;
                case PanelType.controlDark: c.BackColor = Colors.RGB(42, 42, 42); break;
                case PanelType.white: c.BackColor = Colors.RGB(42, 42, 42); break;
            }
        }

        public override void FlatLabel(FlatLabel c)
        {
            c.ForeColor = Colors.RGB(210, 210, 210);

            switch (c.ForeColorType)
            {
                case LabelType.primary: c.ForeColor = Colors.RGB(163, 178, 220); break;
                case LabelType.success: c.ForeColor = Colors.RGB(36, 155, 63); break;
                case LabelType.danger: c.ForeColor = Color.LightCoral; break;
            }
        }

        public override void FlatButton(FlatButton c)
        {
            c.BackgroundColor = Colors.RGB(80, 80, 80);
            c.BackgroundColorFocus = Colors.RGB(80, 80, 80);
            c.BorderColor = Colors.RGB(86, 86, 86);
            c.BorderColorDefault = Colors.RGB(86, 86, 86);
            c.TextColor = Colors.RGB(210, 210, 210);
            c.SelectedColor = Colors.RGB(30, 30, 30);
        }

        public override void FlatCheckBox(FlatCheckBox c)
        {
            c.BorderColor = Colors.RGB(86, 86, 86);
            c.BorderColorFocus = Colors.RGB(108, 132, 199);
            c.BackgroundColor = Colors.RGB(30, 30, 30);
        }

        public override void FlatTextBox(FlatTextBox c)
        {
            c.BackgroundColor = Colors.RGB(30, 30, 30);
            c.BackgroundColorFocus = Colors.RGB(50, 50, 60);
            c.LabelTextColor = Colors.RGB(163, 178, 220);
            c.TextColor = Colors.RGB(210, 210, 210);
            c.TextColorFocus = Colors.RGB(210, 210, 210);
            c.BorderColor = Colors.RGB(86, 86, 86);
        }

        public override void FlatMaskedTextBox(FlatMaskedTextBox c)
        {
            c.BackgroundColor = Colors.RGB(30, 30, 30);
            c.BackgroundColorFocus = Colors.RGB(50, 50, 60);
            c.LabelTextColor = Colors.RGB(163, 178, 220);
            c.TextColor = Colors.RGB(210, 210, 210);
            c.TextColorFocus = Colors.RGB(210, 210, 210);
            c.BorderColor = Colors.RGB(86, 86, 86);
            c.PlaceholderColor = Colors.RGB(108, 108, 108);

            c.btnAction_SetImage(Resources.img_calendar_white);
        }

        public override void FlatComboBox(FlatComboBox c)
        {
            c.BackgroundColor = Colors.RGB(30, 30, 30);
            c.LabelTextColor = Colors.RGB(163, 178, 220);
            c.ItemTextColor = Colors.RGB(210, 210, 210);
            c.BorderColor = Colors.RGB(86, 86, 86);
        }

        public override void FlatStatusBar(FlatStatusBar c)
        {
            c.BackColor = Colors.RGB(86, 86, 86);
            c.BackColorContent = Colors.RGB(53, 53, 53);
        }

        public override void FlatTabControl(FlatTabControl c)
        {
            c.MyBackColor = Colors.RGB(53, 53, 53);
            c.MyBackColor2 = Colors.RGB(30, 30, 30);
            c.MyBorderColor = Colors.RGB(86, 86, 86);

            foreach (TabPage page in c.TabPages)
            {
                page.ForeColor = Colors.RGB(210, 210, 210);
                page.BackColor = Colors.RGB(53, 53, 53);
            }
        }

        public override void FlatPictureBox(FlatPictureBox c)
        {
            c.BackColor = Colors.RGB(53, 53, 53);
        }

        public override void FlatGroupBox(FlatGroupBox c)
        {
            c.ForeColor = Colors.RGB(210, 210, 210);
            c.BackColor = Colors.RGB(53, 53, 53);
            c.BorderColor = Colors.RGB(86, 86, 86);
        }

        public override void FlatListView(FlatListView c)
        {
            c.BackColor = Colors.RGB(53, 53, 53);
        }

        public override void FlatDataGrid(FlatDataGrid c)
        {
            c.ColorBackground = ColorTranslator.FromHtml("#212121");
            c.ColorGrid = c.ColorBackground;

            c.ColorRow = c.ColorBackground;
            c.ColorRowAlternate = ColorTranslator.FromHtml("#242424");

            c.ColorRowSelection = ColorTranslator.FromHtml("#0F0F0F");
            c.ColorRowMouseHover = ColorTranslator.FromHtml("#353535");

            c.ColorFontRow = ColorTranslator.FromHtml("#D2D2D2");
            c.ColorFontRowSelection = c.ColorFontRow;

            c.ColorColumnHeader = ColorTranslator.FromHtml("#3F3F3F");
            c.ColorColumnSelection = c.ColorColumnHeader;

            c.ColorColumnHeaderMouseHover = Colors.RGB(50, 50, 60);
            c.ColorColumnHeaderReorderRec = Colors.RGB(172, 172, 172);
            c.ColorColumnHeaderReorderDiv = SystemColors.ControlDark;
        }

        public override void ButtonExe(ButtonExe c)
        {
            c.BackColor = Colors.RGB(80, 80, 80);
            c.BorderColor = Colors.RGB(86, 86, 86);
            c.TextColor = Colors.RGB(210, 210, 210);
        }

        public override void RichTextBox(RichTextBox c)
        {
            c.ForeColor = Colors.RGB(210, 210, 210);
            c.BackColor = Colors.RGB(42, 42, 42);
        }
    }
}