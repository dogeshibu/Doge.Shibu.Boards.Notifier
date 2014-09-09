using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Doge.Shibu.Boards.Notifier.Models.Interfaces;
using Doge.Shibu.Boards.Notifier.ViewModels;

namespace Doge.Shibu.Boards.Notifier.Handlers
{
    public class ThreadNotificationHandler : IHandle<ThreadReachedPageArgs>
    {
        private readonly IWindowManager _windowManager;
        private readonly INotifier _notifier;
        private readonly IEventAggregator _eventAggregator;

        public ThreadNotificationHandler(IWindowManager windowManager, INotifier notifier, IEventAggregator eventAggregator)
        {
            _windowManager = windowManager;
            _notifier = notifier;
            _eventAggregator = eventAggregator;
        }

        public void Handle(ThreadReachedPageArgs message)
        {
            _windowManager.ShowWindow(new ThreadNotificationViewModel(message,_notifier,_eventAggregator));
        }
    }
}
