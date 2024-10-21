using System.Drawing;
using System.Windows.Forms;
using App.Core.Properties;

namespace App.Core.Desktop
{
    public class ThemeColorLight : ThemeColor
    {
        public override void WindowForm(Form f)
        {
            ThemeBase.SetWindowDark(f.Handle, 0);
        }

        public override void MainBaseForm(MainBaseForm f)
        {
            f.BackColor = Colors.RGB(244, 244, 244);
        }

        public override void ContentBaseForm(ContentBaseForm f)
        {
            f.BackColor = Colors.RGB(244, 244, 244);
        }

        public override void Form(Form f)
        {
            f.BackColor = Colors.RGB(244, 244, 244);
        }

        public override void FlatPanel(FlatPanel c)
        {
            c.ForeColor = Colors.RGB(0, 0, 0);
            c.BorderColor = Colors.RGB(160, 160, 160);

            switch (c.BackColorType)
            {
                case PanelType.transparent: c.BackColor = Color.Transparent;
                    break;
                case PanelType.control: c.BackColor = Colors.RGB(240, 240, 240);
                    break;
                case PanelType.controlLight: c.BackColor = Colors.RGB(244, 244, 244);
                    break;
                case PanelType.controlDark: c.BackColor = Colors.RGB(230, 230, 230);
                    break;
                case PanelType.white: c.BackColor = Colors.RGB(252, 252, 252);
                    break;
            }
        }

        public override void FlatLabel(FlatLabel c)
        {
            c.ForeColor = Color.Black;

            switch (c.ForeColorType)
            {
                case LabelType.primary: c.ForeColor = Colors.RGB(100, 149, 237);
                    break;
                case LabelType.success: c.ForeColor = Colors.RGB(0, 128, 0);
                    break;
                case LabelType.danger: c.ForeColor = Color.Firebrick;
                    break;
            }
        }

        public override void FlatButton(FlatButton c)
        {
            c.BackgroundColor = Colors.RGB(230, 230, 230);
            c.BackgroundColorFocus = Colors.RGB(213, 213, 213);
            c.BorderColor = Colors.RGB(213, 223, 229);
            c.BorderColorDefault = Colors.RGB(213, 223, 229);
            c.TextColor = Colors.RGB(47, 47, 47);
            c.SelectedColor = Colors.RGB(203, 223, 254);
        }

        public override void FlatCheckBox(FlatCheckBox c)
        {
            c.BorderColor = Colors.RGB(213, 223, 229);
            c.BorderColorFocus = Colors.RGB(108, 132, 199);
            c.BackgroundColor = Colors.RGB(255, 255, 255);
        }

        public override void FlatCheckedList(FlatCheckedList c)
        {
            c.BorderColor = Colors.RGB(213, 223, 229);
            c.BackgroundColor = Colors.RGB(255, 255, 255);
            c.TextColor = Colors.RGB(47, 47, 47);
        }

        public override void FlatTextBox(FlatTextBox c)
        {
            c.BackgroundColor = Colors.RGB(255, 255, 255);
            c.BackgroundColorFocus = Colors.RGB(252, 245, 237);
            c.LabelTextColor = Colors.RGB(108, 132, 199);
            c.TextColor = Colors.RGB(47, 47, 47);
            c.TextColorFocus = Colors.RGB(47, 47, 47);
            c.BorderColor = Colors.RGB(213, 223, 229);
        }

        public override void FlatMaskedTextBox(FlatMaskedTextBox c)
        {
            c.BackgroundColor = Color.White;
            c.BackgroundColorFocus = Colors.RGB(252, 245, 237);
            c.LabelTextColor = Colors.RGB(108, 132, 199);
            c.TextColor = Colors.RGB(47, 47, 47);
            c.TextColorFocus = Colors.RGB(47, 47, 47);
            c.BorderColor = Colors.RGB(213, 223, 229);
            c.PlaceholderColor = Colors.RGB(178, 178, 178);

            c.ActionButton_SetImage(Resources.img_calendar_dark);
        }

        public override void FlatComboBox(FlatComboBox c)
        {
            c.BackgroundColor = Color.White;
            c.LabelTextColor = Colors.RGB(108, 132, 199);
            c.ItemTextColor = Colors.RGB(47, 47, 47);
            c.BorderColor = Colors.RGB(213, 223, 229);
        }

        public override void FlatStatusBar(FlatStatusBar c)
        {
            c.BackColor = Colors.RGB(160, 160, 160);
            c.BackColorContent = Colors.RGB(244, 244, 244);
        }

        public override void FlatTabControl(FlatTabControl c)
        {
            c.MyBackColor = Colors.RGB(244, 244, 244);
            c.MyBackColor2 = Colors.RGB(212, 208, 200);
            c.MyBorderColor = Colors.RGB(160, 160, 160);

            foreach (TabPage page in c.TabPages)
            {
                page.ForeColor = Colors.RGB(0, 0, 0);
                page.BackColor = Colors.RGB(244, 244, 244);
            }
        }

        public override void FlatPictureBox(FlatPictureBox c)
        {
            c.BackColor = Colors.RGB(244, 244, 244);
        }

        public override void FlatGroupBox(FlatGroupBox c)
        {
            c.ForeColor = Colors.RGB(0, 0, 0);
            c.BackColor = Colors.RGB(244, 244, 244);
            c.BorderColor = Colors.RGB(160, 160, 160);
        }

        public override void FlatListView(FlatListView c)
        {
            c.BackColor = Colors.RGB(244, 244, 244);
        }

        public override void FlatDataGrid(FlatDataGrid c)
        {
            c.ColorBackground = Colors.RGB(244, 244, 244);
            c.ColorGrid = Colors.RGB(192, 192, 192);

            c.ColorRow = Colors.RGB(255, 255, 255);
            c.ColorRowAlternate = c.ColorRow;

            c.ColorRowSelection = Colors.RGB(229, 226, 244);
            c.ColorRowMouseHover = Colors.RGB(189, 237, 255);

            c.ColorFontRow = Colors.RGB(64, 64, 64);
            c.ColorFontRowSelection = c.ColorFontRow;

            c.ColorColumnHeader = Colors.RGB(108, 132, 199);
            c.ColorColumnSelection = c.ColorColumnHeader;

            c.ColorColumnHeaderMouseHover = Colors.RGB(128, 152, 229);
            c.ColorColumnHeaderReorderRec = Colors.RGB(0, 0, 0);
            c.ColorColumnHeaderReorderDiv = SystemColors.Highlight;
        }

        public override void ButtonExe(ExeButton c)
        {
            c.BackColor = Colors.RGB(230, 230, 230);
            c.BorderColor = Colors.RGB(213, 223, 229);
            c.TextColor = Colors.RGB(47, 47, 47);
        }

        public override void RichTextBox(RichTextBox c)
        {
            c.ForeColor = Colors.RGB(0, 0, 0);
            c.BackColor = Colors.RGB(230, 230, 230);
        }
    }
}