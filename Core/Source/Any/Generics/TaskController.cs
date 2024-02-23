using System;
using System.Threading;
using System.Threading.Tasks;

namespace App.Core
{
    public class TaskController
    {
        CancellationTokenSource Source;
        public bool IsCanceled;

        public TaskController()
        {
            Reset();
        }

        public void Reset()
        {
            Source = new CancellationTokenSource();
            IsCanceled = false;
        }

        public void Cancel()
        {
            Source.Cancel();
        }

        public async Task DelayStart(int secondsDelay)
        {
            try
            {
                await Task.Delay(secondsDelay * 1000, Source.Token);
            }
            catch (Exception)
            {
                IsCanceled = Source.IsCancellationRequested;
                return;
            }
        }

        public static async Task Delay(int secondsDelay)
        {
            await Task.Delay(secondsDelay * 1000);
        }
    }
}