using System.Windows.Forms;

namespace App.Core.Desktop
{
    public class ThemeColor
    {
        public virtual void WindowForm(Form f) { }
        public virtual void MainBaseForm(MainBaseForm f) { }
        public virtual void ContentBaseForm(ContentBaseForm f) { }
        public virtual void Form(Form f) { }

        public virtual void FlatPanel(FlatPanel c) { }
        public virtual void FlatLabel(FlatLabel c) { }
        public virtual void FlatButton(FlatButton c) { }
        public virtual void FlatCheckBox(FlatCheckBox c) { }
        public virtual void FlatCheckedList(FlatCheckedList c) { }
        public virtual void FlatTextBox(FlatTextBox c) { }
        public virtual void FlatMaskedTextBox(FlatMaskedTextBox c) { }
        public virtual void FlatComboBox(FlatComboBox c) { }
        public virtual void FlatStatusBar(FlatStatusBar c) { }
        public virtual void FlatTabControl(FlatTabControl c) { }
        public virtual void FlatPictureBox(FlatPictureBox c) { }
        public virtual void FlatGroupBox(FlatGroupBox c) { }
        public virtual void FlatListView(FlatListView c) { }
        public virtual void FlatDataGrid(FlatDataGrid c) { }
        public virtual void ButtonExe(ExeButton c) { }
        public virtual void RichTextBox(RichTextBox c) { }
    }
}