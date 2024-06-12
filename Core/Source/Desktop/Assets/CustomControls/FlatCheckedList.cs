using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public partial class FlatCheckedList : UserControl
    {
        #region Defaults
        [DefaultValue(typeof(AutoScaleMode), "None")]
        public new AutoScaleMode AutoScaleMode
        {
            get { return base.AutoScaleMode; }
            set { base.AutoScaleMode = value; }
        }

        [DefaultValue(typeof(AutoSizeMode), "GrowAndShrink")]
        public new AutoSizeMode AutoSizeMode
        {
            get { return base.AutoSizeMode; }
            set { base.AutoSizeMode = value; }
        }

        [DefaultValue(typeof(Font), "Segoe UI, 9")]
        public new Font Font
        {
            get { return base.Font; }
            set { base.Font = value; }
        }

        [DefaultValue(typeof(Size), "64, 34")]
        public new Size MinimumSize
        {
            get { return base.MinimumSize; }
            set { base.MinimumSize = value; }
        }

        [DefaultValue(typeof(Padding), "2, 2, 2, 2")]
        public new Padding Margin
        {
            get { return base.Margin; }
            set { base.Margin = value; }
        }

        [DefaultValue(typeof(Padding), "1, 1, 1, 1")]
        public new Padding Padding
        {
            get { return base.Padding; }
            set { base.Padding = value; }
        }
        #endregion

        #region Properties
        [Category("_Colors"), DefaultValue(typeof(Color), "213, 223, 229")]
        public override Color BackColor { get { return base.BackColor; } set { base.BackColor = value; } }

        protected Color _BorderColor = Color.FromArgb(213, 223, 229);
        [Category("_Colors"), DefaultValue(typeof(Color), "213, 223, 229")]
        public Color BorderColor { get { return _BorderColor; } set { _BorderColor = value; } }

        protected Color _BackgroundColor = Color.White;
        [Category("_Colors"), DefaultValue(typeof(Color), "White")]
        public Color BackgroundColor { get { return _BackgroundColor; } set { _BackgroundColor = value; } }

        protected Color _TextColor = Color.FromArgb(47, 47, 47);
        [Category("_Colors"), DefaultValue(typeof(Color), "47, 47, 47")]
        public Color TextColor { get { return _TextColor; } set { _TextColor = value; } }

        [Category("_Data"), DefaultValue("")]
        public string TextLegend
        {
            get { return lblSubtitle.Text; }
            set
            {
                lblSubtitle.Text = value;
            }
        }

        public event ItemCheckEventHandler ItemCheck;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [Localizable(true)]
        public CheckedListBox.ObjectCollection Items
        {
            get { return chkList.Items; }
            set { chkList.Items.AddRange(value); }
        }

        public void ColumnWidth(int value)
        {
            chkList.ColumnWidth = value;
        }

        public void MultiColumn(bool value)
        {
            chkList.MultiColumn = value;
        }

        public CheckedListBox.CheckedItemCollection CheckedItems
        {
            get { return chkList.CheckedItems; }
        }

        public void SetItemChecked(int index, bool value)
        {
            chkList.SetItemChecked(index, value);
        }

        public bool GetItemChecked(int index)
        {
            return chkList.GetItemChecked(index);
        }

        public void SetItemsChecked(bool value)
        {
            for (int i = 0; i < chkList.Items.Count; i++)
            {
                chkList.SetItemChecked(i, value);
            }
        }

        public bool GetItemChecked(string value)
        {
            if (chkList.Items != null && chkList.Items.Count > 0)
            {
                for (int i = 0; i < chkList.Items.Count; i++)
                {
                    bool indexFound = chkList.GetItemChecked(i) && chkList.Items[i].ToString() == value;
                    if (indexFound)
                        return true;
                }
            }
            return false;
        }
        #endregion

        public FlatCheckedList()
        {
            InitializeComponent();
            chkList.ItemCheck += chkList_ItemCheck;

            Font = new Font("Segoe UI", 9);
        }

        void chkList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (ItemCheck == null) return;

            ParentForm.BeginInvoke((MethodInvoker)(() =>
                ItemCheck(sender, e)
            ));
        }

        public void ResetColors()
        {
            BackColor = BorderColor;
            pnlContent.BackColor = BackgroundColor;
            chkList.BackColor = BackgroundColor;
            chkList.ForeColor = TextColor;
        }
    }
}