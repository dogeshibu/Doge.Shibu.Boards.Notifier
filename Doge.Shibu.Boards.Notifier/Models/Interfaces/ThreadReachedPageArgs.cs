using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doge.Shibu.Boards.Notifier.Models.Interfaces
{
    public class ThreadReachedPageArgs : EventArgs
    {
        private readonly string _originalUrl;
        private readonly int? _page;

        public ThreadReachedPageArgs(string originalUrl, int? page)
        {
            _originalUrl = originalUrl;
            _page = page;
        }

        public int? Page
        {
            get { return _page; }
        }

        public string OriginalUrl
        {
            get { return _originalUrl; }
        }
    }
}
