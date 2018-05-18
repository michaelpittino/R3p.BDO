using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3p.bdo
{
    public enum LogModule
    {
        Global = 0,
        AutoFish = 1,
        Hack_Navigation = 2,
        Hack_Speedhack = 3,
        Hack_UI = 4,
        AutoItemRegister = 5,
        AutoProcessing = 6,
        AutoPotion = 7,
        AutoWorkerRestore = 8,
    }

    public class Log
    {
        public static void Post(string text, LogModule logModule)
        {
            DeleteLog();

            string output = "#####\t" + DateTime.Now + "\t#####\t" + logModule + "\n" + text + "\n@@@@@@@@@@\n";

            File.AppendAllText(@".\Log.txt", output);
        }

        private static void DeleteLog()
        {
            if (File.Exists(@".\Log.txt"))
            {
                FileInfo fi = new FileInfo(@".\Log.txt");

                if(fi.Length >= 1024000)
                    File.Delete(@".\Log.txt");
            }
        }
    }
}
