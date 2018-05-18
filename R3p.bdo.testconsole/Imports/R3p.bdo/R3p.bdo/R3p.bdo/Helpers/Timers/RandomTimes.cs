using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3p.bdo.Helpers.Timers
{
    public class RandomTimes
    {
        private static Random r = new Random();

        public static int GetRandomSeconds(int min, int max)
        {
            return r.Next(min, max);
        }
    }
}
