using System;
using System.Threading;
using System.Threading.Tasks;

namespace App.Core
{
    static class TaskExtension
    {
#pragma warning disable
        // Asynchronous methods should return a Task instead of void
        public static async void TryAwait(this Task task)
#pragma warning restore
        {
            try
            {
                await task;
            }
            catch (Exception)
            {
                throw;
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