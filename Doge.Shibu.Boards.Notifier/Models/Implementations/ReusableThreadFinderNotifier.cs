using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Doge.Shibu.Boards.Notifier.Models.Interfaces;

namespace Doge.Shibu.Boards.Notifier.Models.Implementations
{
    public class ReusableThreadFinderNotifier : INotifier
    {
        private readonly IThreadFinder _finder;
        private readonly Action<ThreadReachedPageArgs> _action;

        private INotifier _current;
        private int _running;
        private string _url;

        public ReusableThreadFinderNotifier(IThreadFinder finder, Action<ThreadReachedPageArgs> action)
        {
            _finder = finder;
            _action = action;
        }

        private void Cancel()
        {
            if(_running == 0) return;

            _current.Dispose();
            _running = 0;
        }

        public void Dispose()
        {
            Cancel();
        }

        public bool CanTrack(string url)
        {
            return _finder.CanFind(url);
        }

        public void StartTracking(string url, TimeSpan interval, int page)
        {
            if(Interlocked.Increment(ref _running) > 1) return;

            _url = url;

            _current = new ThreadFinderNotifier(_finder,_action);
            _current.StartTracking(url,interval,page);
        }

        public bool Tracking(string url)
        {
            return _running > 0 && url == _url;
        }

        public void StopTracking(string url)
        {
            Cancel();
        }
    }
}
