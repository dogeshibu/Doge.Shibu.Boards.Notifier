using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doge.Shibu.Boards.Notifier.Models.Interfaces
{
    public interface INotifier : IDisposable
    {
        bool CanTrack(string url);

        void StartTracking(string url, TimeSpan interval, int page);

        bool Tracking(string url);

        void StopTracking(string url); 
    }
}
