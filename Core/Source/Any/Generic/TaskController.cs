using System;
using System.Threading;
using System.Threading.Tasks;

namespace App.Core
{
    public class TaskController
    {
        public TaskController()
        {
            Reset();
        }

        public CancellationTokenSource TokenSource { get; private set; }

        public bool IsCanceled { get; set; }

        public static async Task Delay(double seconds)
        {
            int milliseconds = (int)(seconds * 1000);
            await Task.Delay(milliseconds);
        }

        public void Reset()
        {
            TokenSource = new CancellationTokenSource();
            IsCanceled = false;
        }

        public async Task DelayStart(double seconds)
        {
            try
            {
                int milliseconds = (int)(seconds * 1000);
                await Task.Delay(milliseconds, TokenSource.Token);
            }
            catch (Exception)
            {
                IsCanceled = TokenSource.IsCancellationRequested;
                return;
            }
        }
    }
}