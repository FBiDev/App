using System;
using System.Threading;
using System.Threading.Tasks;

namespace App.Core
{
    public static class TaskExtension
    {
        public static void Cancel(this TaskController task)
        {
            if (task is TaskController)
            {
                task.TokenSource.Cancel();
            }
        }

        public static void TryAwait(this Task task)
        {
            if (AppType.IsDesktop)
            {
                Desktop.TaskExtension.TryAwait(task);
            }
            else
            {
                Web.TaskExtension.TryAwait(task);
            }
        }

        public static void RunIgnoreException(this Task task)
        {
            task.ContinueWith(_ => { return; });
        }

        public static async Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, int seconds)
        {
            using (var timeoutCancellationTokenSource = new CancellationTokenSource())
            {
                var timeout = new TimeSpan(0, 0, seconds);
                var completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token));
                if (completedTask == task)
                {
                    timeoutCancellationTokenSource.Cancel();
                    return await task;  // Very important in order to propagate exceptions
                }

                throw new TimeoutException("The operation has timed out.");
            }
        }
    }
}