using System;

namespace App.Core
{
    public static class ExceptionManager
    {
        public static string Error { get; set; }

        public static void Resolve(Exception ex, string customMessage = "")
        {
            if (AppType.IsDesktop)
            {
                Error = Desktop.ExceptionManager.Resolve(ex, customMessage);
            }
            else
            {
                Error = Web.ExceptionManager.Resolve(ex);
            }
        }
    }
}