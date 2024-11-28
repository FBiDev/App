using System.Web;
using System.Windows.Forms;

namespace App.Core
{
    public static class AppType
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