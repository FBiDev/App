using System;
using System.Threading;
using System.Threading.Tasks;

namespace App.Core
{
    public class TaskController
    {
        private CancellationTokenSource source;

        public TaskController()
        {
            Reset();
        }

        public bool IsCanceled { get; set; }

        public static async Task Delay(double secondsDelay)
        {
            int milliseconds = (int)(secondsDelay * 1000);
            await Task.Delay(milliseconds);
        }

        public void Reset()
        {
            source = new CancellationTokenSource();
            IsCanceled = false;
        }

        public void Cancel()
        {
            source.Cancel();
        }

        public async Task DelayStart(double secondsDelay)
        {
            try
            {
                int milliseconds = (int)(secondsDelay * 1000);
                await Task.Delay(milliseconds, source.Token);
            }
            catch (Exception)
            {
                IsCanceled = source.IsCancellationRequested;
                return;
            }
        }
    }
}