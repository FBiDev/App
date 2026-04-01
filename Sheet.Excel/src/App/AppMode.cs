using System.Web;
using System.Windows.Forms;

namespace App.Sheet
{
    internal static class AppMode
    {
        public static bool IsWeb
        {
            get { return HttpContext.Current != null && Application.OpenForms.Count == 0; }
        }

        public static bool IsDesktop
        {
            get { return !IsWeb; }
        }
    }
}