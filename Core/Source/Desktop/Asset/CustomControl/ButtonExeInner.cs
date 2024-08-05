using System.Windows.Forms;

namespace App.Core.Desktop
{
    public class ButtonExeInner : Button
    {
        protected override bool ShowFocusCues
        {
            get { return Focused; }
        }
    }
}