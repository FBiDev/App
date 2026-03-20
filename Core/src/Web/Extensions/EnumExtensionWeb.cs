using System.Web.UI.WebControls;

namespace App.Core.Web
{
    public static class EnumExtensionWeb
    {
        public static string ToSQL(this SortDirection s)
        {
            return s == SortDirection.Ascending ? " ASC " : " DESC ";
        }
    }
}