using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using Doge.Shibu.Boards.Notifier.Common;
using Doge.Shibu.Boards.Notifier.Messages;
using Doge.Shibu.Boards.Notifier.Models.Interfaces;

namespace Doge.Shibu.Boards.Notifier.ViewModels
{
    public class ShellViewModel : PropertyChangedBase, IHandle<NotificationsCancelled>
    {
        private readonly INotifier _notifier;

        private string _url;

        public string Url
        {
            get { return _url; }
            set
            {
                _url = value;
                NotifyOfPropertyChange(() => Url);
                NotifyOfPropertyChange(() => CanStart);
            }
        }

        private string _boardName;

        public string BoardName
        {
            get { return _boardName; }
            set
            {
                _boardName = value;
                NotifyOfPropertyChange(() => BoardName);
                NotifyOfPropertyChange(() => CanStart);
            }
        }

        private string _checkTime;

        public string CheckTime
        {
            get { return _checkTime; }
            set
            {
                _checkTime = value;
                NotifyOfPropertyChange(() => CheckTime);
                NotifyOfPropertyChange(() => CanStart);
            }
        }

        private string _checkPage;

        public ShellViewModel(INotifier notifier, IEventAggregator eventAggregator)
        {
            _notifier = notifier;

            eventAggregator.Subscribe(this);
        }

        public string CheckPage
        {
            get { return _checkPage; }
            set
            {
                _checkPage = value;
                NotifyOfPropertyChange(() => CheckPage);
                NotifyOfPropertyChange(() => CanStart);
            }
        }

        public bool CanStart
        {
            get
            {
                return CheckTime.IsDouble() && CheckTime.IsInt() && _notifier.CanTrack(Url) && !_notifier.Tracking(Url);
            }
        }

        public void Start()
        {
            _notifier.StartTracking(Url, TimeSpan.FromSeconds(CheckTime.ToDouble()), CheckPage.ToInt());

            NotifyOfPropertyChange(() => CanStart);
            NotifyOfPropertyChange(() => CanStop);
        }


        public bool CanStop
        {
            get
            {
                return _notifier.Tracking(Url);
            }
        }

        public void Stop()
        {
            _notifier.StopTracking(Url);

            NotifyOfPropertyChange(() => CanStart);
            NotifyOfPropertyChange(() => CanStop);
        }

        public void Handle(NotificationsCancelled message)
        {
            NotifyOfPropertyChange(() => CanStart);
            NotifyOfPropertyChange(() => CanStop);
        }
    }
}
