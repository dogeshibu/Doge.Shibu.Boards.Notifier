using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doge.Shibu.Boards.Notifier.Models.Interfaces
{
    public interface IThreadFinder
    {
        bool CanFind(string url);

        Task<int?> Find(string url);
    }
}
