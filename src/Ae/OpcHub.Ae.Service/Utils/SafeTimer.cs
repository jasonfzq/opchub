using System;
using System.Threading;

namespace OpcHub.Ae.Service.Utils
{
    public class SafeTimer : IDisposable
    {
        private int _intervals;
        private Action _action;
        private Timer _timer;

        public SafeTimer(Action action, int intervals)
        {
            _action = action;
            _intervals = intervals;
        }

        public void Run()
        {
            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }

            _timer = new Timer(state =>
            {
                _timer.Change(-1, -1);
                _action?.Invoke();
                _timer?.Change(_intervals, _intervals);
            }, null, _intervals, _intervals);
        }

        public void Change(int intervals)
        {
            if (_intervals == -1 && intervals != -1)
                _timer.Change(intervals, intervals);

            _intervals = intervals;

        }

        public void Dispose()
        {
            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }

            _action = null;
        }
    }
}
