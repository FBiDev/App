using System.Drawing;
using System.Windows.Forms;

namespace App.Core.Desktop
{
    public class FlatCheckBoxInner : CheckBox
    {
        public FlatCheckBoxInner()
        {
        }

        protected override bool ShowFocusCues
        {
            get { return false; }
        }
    }
}