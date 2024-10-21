using System.Windows.Forms;

namespace App.Core.Desktop
{
    public class FlatListViewItem : ListViewItem
    {
        public FlatListViewItem()
        {
            Hover = false;
        }

        public bool Hover { get; set; }
    }
}