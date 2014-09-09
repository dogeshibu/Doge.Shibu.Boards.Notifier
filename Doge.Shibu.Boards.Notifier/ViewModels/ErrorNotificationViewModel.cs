using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace Doge.Shibu.Boards.Notifier.ViewModels
{
    public class ErrorNotificationViewModel : PropertyChangedBase, INotificationViewModel
    {
        private readonly Exception _exception;

        public ErrorNotificationViewModel(Exception exception)
        {
            _exception = exception;
        }

        public string Title { get { return "Error"; }}

        public TimeSpan CloseAfter { get { return TimeSpan.FromSeconds(15); } }

        public string Message { get { return "Notifications cancelled because of error: " + _exception.Message; } }
    }
}
