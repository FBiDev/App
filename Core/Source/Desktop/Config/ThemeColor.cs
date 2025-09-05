using System.Drawing;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public abstract class ThemeColor
    {
        public static readonly Color C160160160 = Colors.RGB(160, 160, 160);

        public abstract void WindowForm(Form f);

        public abstract void MainBaseForm(MainBaseForm f);

        public abstract void ContentBaseForm(ContentBaseForm f);

        public abstract void Form(Form f);

        public abstract void FlatPanel(FlatPanel c);

        public abstract void FlatLabel(FlatLabel c);

        public abstract void FlatButton(FlatButton c);

        public abstract void FlatCheckBox(FlatCheckBox c);

        public abstract void FlatCheckedList(FlatCheckedList c);

        public abstract void FlatTextBox(FlatTextBox c);

        public abstract void FlatMaskedTextBox(FlatMaskedTextBox c);

        public abstract void FlatComboBox(FlatComboBox c);

        public abstract void FlatStatusBar(FlatStatusBar c);

        public abstract void FlatTabControl(FlatTabControl c);

        public abstract void FlatPictureBox(FlatPictureBox c);

        public abstract void FlatGroupBox(FlatGroupBox c);

        public abstract void FlatListView(FlatListView c);

        public abstract void FlatDataGrid(FlatDataGrid c);

        public abstract void ButtonExe(ExeButton c);

        public abstract void RichTextBox(RichTextBox c);
    }
}