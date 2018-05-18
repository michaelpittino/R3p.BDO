using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3p.bdo.Collection.AutoItemBuy
{
    public class Log
    {
        public static bool updated = false;

        public static Dictionary<int, int> _attempts = new Dictionary<int, int>();
        public static Dictionary<int,int> _fails = new Dictionary<int, int>();
        public static Dictionary<int, int> _succeeds = new Dictionary<int, int>();   
    }
}
