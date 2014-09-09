using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doge.Shibu.Boards.Notifier.Common
{
    public static class Extensions
    {
        public static bool IsInt(this string str)
        {
            int _;
            return int.TryParse(str, out _);
        }

        public static bool IsDouble(this string str)
        {
            double _;
            return double.TryParse(str, out _);
        }

        public static double ToDouble(this string str)
        {
            return double.Parse(str);
        }

        public static int ToInt(this string str)
        {
            return int.Parse(str);
        }
    }
}
