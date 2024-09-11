using System.Windows.Forms;

namespace App.Core.Desktop
{
    public class ExeInnerButton : Button
    {
        protected override bool ShowFocusCues
        {
            get { return Focused; }
        }
    }
}