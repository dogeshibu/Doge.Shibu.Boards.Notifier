using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Doge.Shibu.Boards.Notifier.Models.Interfaces;

namespace Doge.Shibu.Boards.Notifier.Models.Implementations
{
    public class ThreadFinderNotifier : INotifier
    {
        private readonly IThreadFinder _threadFinder;
        private readonly Action<ThreadReachedPageArgs> _onReached;
        private readonly CancellationTokenSource _cancellationTokenSource;

        private bool IsStarted { get { return _running > 0; } }

        private int _running;
        private string _url;
        private TimeSpan _span;
        private int _page;
        private Task _task;
        private bool _isCancelled;

        public ThreadFinderNotifier(IThreadFinder threadFinder, Action<ThreadReachedPageArgs> onReached)
        {
            _threadFinder = threadFinder;
            _onReached = onReached;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void Dispose()
        {
            if(IsStarted)
                Cancel();

            _cancellationTokenSource.Dispose();
        }

        public bool CanTrack(string url)
        {
            return _threadFinder.CanFind(url);
        }

        public async void Track(object _)
        {
            while (true)
            {
                if (_cancellationTokenSource.IsCancellationRequested) break;

                await Task.Delay(_span);

                try
                {
                    if (_cancellationTokenSource.IsCancellationRequested) break;

                    var found = await _threadFinder.Find(_url);

                    if (!found.HasValue || found > _page)
                        _onReached(new ThreadReachedPageArgs(_url, found));
                }
                catch (Exception)
                {
                    Cancel();
                    throw;
                }

            }            
        }

        public void StartTracking(string url, TimeSpan interval, int page)
        {
            if(Interlocked.Increment(ref _running) > 1) return;

            if(_isCancelled) throw new InvalidOperationException();

            _url = url;
            _span = interval;
            _page = page;

            _task = Task.Factory.StartNew(Track,TaskCreationOptions.LongRunning, _cancellationTokenSource.Token);
        }

        public bool Tracking(string url)
        {
            return IsStarted && _url == url;
        }

        public void StopTracking(string url)
        {
            if (IsStarted && _url == url)
            {
                Cancel();
            }
        }

        private void Cancel()
        {
            _cancellationTokenSource.Cancel();
            _running = 0;

            if (_task != null)
                _task.Dispose();

            _isCancelled = true;
        }
    }
}
