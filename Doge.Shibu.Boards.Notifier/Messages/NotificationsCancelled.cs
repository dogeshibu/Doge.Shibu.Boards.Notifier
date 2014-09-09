using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doge.Shibu.Boards.Notifier.Messages
{
    public class NotificationsCancelled
    {
        private readonly string _url;

        public NotificationsCancelled(string url)
        {
            _url = url;
        }

        public string Url
        {
            get { return _url; }
        }
    }
}
