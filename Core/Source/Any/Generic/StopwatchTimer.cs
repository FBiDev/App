using System.Diagnostics;

namespace App.Core
{
    public class StopwatchTimer : Stopwatch
    {
        public StopwatchTimer(float secondsTimeout)
        {
            Timeout = secondsTimeout;
            Start();
        }

        public float Timeout { get; set; }

        public float ElapsedSeconds
        {
            get { return this.ElapsedSeconds(); }
        }

        public new long ElapsedMilliseconds
        {
            get { return base.ElapsedMilliseconds; }
        }

        public bool Stopped
        {
            get
            {
                if (this.ElapsedSeconds() >= Timeout)
                {
                    Stop();
                }

                return !IsRunning;
            }
        }
    }
}