using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Doge.Shibu.Boards.Notifier.Messages;
using Doge.Shibu.Boards.Notifier.Models.Interfaces;

namespace Doge.Shibu.Boards.Notifier.ViewModels
{
    public class ThreadNotificationViewModel : PropertyChangedBase, INotificationViewModel
    {
        private readonly ThreadReachedPageArgs _threadReachedPageArgs;
        private readonly INotifier _notifier;
        private readonly IEventAggregator _eventAggregator;

        public ThreadNotificationViewModel(ThreadReachedPageArgs threadReachedPageArgs, INotifier notifier, IEventAggregator eventAggregator)
        {
            _threadReachedPageArgs = threadReachedPageArgs;
            _notifier = notifier;
            _eventAggregator = eventAggregator;
        }

        public string Title { get { return "Notification"; } }
        public string Message { 
            get
            {
                return String.Format("Thread on page: {0}",_threadReachedPageArgs.Page);
            } 
        }
        public TimeSpan CloseAfter { get { return TimeSpan.FromSeconds(10); } }

        public bool CanCancel
        {
            get { return _notifier.Tracking(_threadReachedPageArgs.OriginalUrl); }
        }

        public void Cancel()
        {
            _notifier.StopTracking(_threadReachedPageArgs.OriginalUrl);
            _eventAggregator.PublishOnUIThread(new NotificationsCancelled(_threadReachedPageArgs.OriginalUrl));
        }
    }
}
