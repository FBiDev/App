using System.Diagnostics;

namespace App.Core
{
    public class StopwatchTimer : Stopwatch
    {
        public float Timeout { get; set; }
        public new long ElapsedMilliseconds { get { return base.ElapsedMilliseconds; } }
        public float ElapsedSeconds { get { return this.ElapsedSeconds(); } }

        public bool Stopped
        {
            get
            {
                if (this.ElapsedSeconds() >= Timeout)
                    Stop();
                return !IsRunning;
            }
        }

        public StopwatchTimer(float timeout)
        {
            Timeout = timeout;
            Start();
        }
    }
}