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

        public static async Task Delay(int secondsDelay)
        {
            await Task.Delay(secondsDelay * 1000);
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

        public async Task DelayStart(int secondsDelay)
        {
            try
            {
                await Task.Delay(secondsDelay * 1000, source.Token);
            }
            catch (Exception)
            {
                IsCanceled = source.IsCancellationRequested;
                return;
            }
        }
    }
}