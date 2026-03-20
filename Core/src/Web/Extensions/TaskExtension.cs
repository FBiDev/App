using System;
using System.Threading.Tasks;

namespace App.Core.Web
{
    internal static class TaskExtension
    {
#pragma warning disable
        public static async void TryAwait(this Task task)
#pragma warning restore
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                var exceptionMessage = "[Task Failed]" + Environment.NewLine + ex.Message + Environment.NewLine + Environment.NewLine;
                var stackCalls = ObjectManager.GetStackTrace(ex);
                var errorMessage = exceptionMessage + stackCalls;

                throw;
            }
        }
    }
}
