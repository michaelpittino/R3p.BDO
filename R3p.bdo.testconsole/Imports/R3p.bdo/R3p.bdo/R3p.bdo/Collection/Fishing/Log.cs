using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.GameInternals.Enums;

namespace R3p.bdo.Collection.Fishing
{
    public class Log
    {
        public static bool updated = false;

        public static DateTime startTime;

        public static Dictionary<int, int> looted_White = new Dictionary<int, int>();
        public static Dictionary<int, int> looted_Green = new Dictionary<int, int>();
        public static Dictionary<int, int> looted_Blue = new Dictionary<int, int>();
        public static Dictionary<int, int> looted_Yellow = new Dictionary<int, int>();
        public static List<TimeSpan> CatchTimes = new List<TimeSpan>();
        public static List<ItemGrade> FishGrades = new List<ItemGrade>();
        public static int _throws;
        public static int _catches;
        public static int _switches;
    }
}
