using System.Windows.Forms;

namespace App.Core.Desktop
{
    public class FlatListViewItem : ListViewItem
    {
        public bool Hover { get; set; }

        public FlatListViewItem()
        {
            Hover = false;
        }
    }
}