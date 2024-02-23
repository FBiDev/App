using System;

namespace App.Core.Web
{
    public static class ExceptionManager
    {
        public static string Resolve(Exception ex)
        {
            var exProc = Core.ExceptionManager.Process(ex);

            return exProc.Message;
        }
    }
}