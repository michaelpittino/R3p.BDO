using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace R3p.bdo
{
    public class Engine
    {
        public static Engine Instance;
        public static  int _supportedVersion = 760;

        public static void Create(string pName)
        {
            Instance = new Engine(pName);
        }

        public IntPtr hProcess { get; set; }
        public ProcessThread mThread { get; set; }
        public Process Process { get; set; }
        public bool SupportedVersion { get; set; }
        
        public Engine(string pName)
        {
            Process = GetProcess(pName);

            if (Process != null)
            {
                hProcess = GetHandle();
                mThread = GetMainThread();

                SupportedVersion = isSupportedVersion();
            }
        }

        private Process GetProcess(string pName)
        {
            Process[] list = Process.GetProcessesByName(pName);

            if(list.Length > 0)
                return list[0];

            Log.Post("No Process found ("+pName+")", LogModule.Global);

            return null;
        }

        private IntPtr GetHandle()
        {
            if(Process != null)
                return Win32.OpenProcess(0x1F0FFF, false, Process.Id);

            Log.Post("No Handle created", LogModule.Global);

            return IntPtr.Zero;
        }

        private ProcessThread GetMainThread()
        {
            if(Process != null)
                return Process.Threads.OfType<ProcessThread>().OrderBy(x => x.StartTime).FirstOrDefault();

            Log.Post("No MainThread found", LogModule.Global);

            return null;
        }

        private bool isSupportedVersion()
        {
            var wDir = GetWorkingDirectory();
            
            return Convert.ToInt32(File.ReadAllText(wDir + "version.dat")) == _supportedVersion;
        }

        private string GetWorkingDirectory()
        {
            string wDir = "";

            Match m = Regex.Match(Process.MainModule.FileName, @"(.*)bin64");

            wDir = m.Groups[1].Value;

            return wDir;
        }
    }
}
