using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Doge.Shibu.Boards.Notifier.Models.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Doge.Shibu.Boards.Notifier.Models.Implementations.Finders
{
    public class HkThreadFinder : IThreadFinder
    {
        private readonly Regex _hkRegex = new Regex(@"https:\/\/2ch\.hk\/[^\/]+\/res\/[0-9]+\.html");
        private readonly Regex _hkPages = new Regex(@"(https:\/\/2ch\.hk\/[^\/]+\/)res\/[0-9]+\.html");
        private readonly Regex _hkNumber = new Regex(@"https:\/\/2ch\.hk\/[^\/]+\/res\/([0-9]+)\.html");

        public bool CanFind(string url)
        {
            return url != null && _hkRegex.IsMatch(url);
        }

        public async Task<bool> IsOnPage(string pageUrl, string number, int page)
        {
            using (var client = new WebClient())
            {
                var json = JObject.Parse(await client.DownloadStringTaskAsync(pageUrl + (page == 0 ? "index" : page.ToString()) + ".json"));

                return json["threads"].Any(x => x["thread_num"].Value<string>() == number);
            }
        }

        public async Task<int?> Find(string url)
        {
            var pageUrl = _hkPages.Match(url).Groups[1].ToString();
            var number = _hkNumber.Match(url).Groups[1].ToString();

            for (var i = 0; i < 10; i++)
            {
                if (await IsOnPage(pageUrl, number, i))
                    return i+1;
            }

            return null;
        }
    }
}
