using System;

namespace App.Core.Web
{
    internal static class ExceptionManager
    {
        public static string Resolve(Exception ex)
        {
            var exProc = Core.ExceptionManagerAny.Process(ex);

            return exProc.Message;
        }
    }
}