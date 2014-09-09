using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doge.Shibu.Boards.Notifier.ViewModels
{
    interface INotificationViewModel
    {
        string Title { get; }

        TimeSpan CloseAfter { get; }
    }
}
