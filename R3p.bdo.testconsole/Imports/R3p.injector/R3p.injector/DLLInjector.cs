using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace R3p.injector
{
    public enum DllInjectionResult
    {
        DllNotFound,
        GameProcessNotFound,
        InjectionFailed,
        Success
    }

    public class DllInjector
    {
        public static bool isModuleLoaded(string module, Process p)
        {
            //Console.WriteLine("Checking if module is alrdy loaded...");
            
            ProcessModule myProcessModule;
            //Get all the modules associated with 'myProcess".
            ProcessModuleCollection myProcessModuleCollection = p.Modules;

            //Display the properties of each of the modules
            for (int i = 0; i < myProcessModuleCollection.Count; i++)
            {
                myProcessModule = myProcessModuleCollection[i];

                if (myProcessModule.ModuleName == module)
                {
                    //Console.WriteLine("Module is alrdy loaded!");
                    return true;
                }
            }

            //Console.WriteLine("Module is not loaded!");
            return false;
        }

        private static void Eject()
        {
            //ManualResetEvent SyncClientServer = (ManualResetEvent)obj;
            using (NamedPipeClientStream pipeStream = new NamedPipeClientStream("PipesOfPiece"))
            {
                pipeStream.Connect();

                //MessageBox.Show(“[Client] Pipe connection established”);
                using (StreamWriter sw = new StreamWriter(pipeStream))
                {
                    sw.AutoFlush = true;
                    sw.WriteLine("Eject");
                }
            }
        }

        private static bool doCreateRemoteThread(Process pToBeInjected, string sDllPath, out string sError, out IntPtr hwnd)
        {
            try
            {
                sError = "";
                IntPtr hProcess = WINAPI.OpenProcess(0x43a, 1, (uint)pToBeInjected.Id);
                hwnd = hProcess;
                if (hProcess == IntPtr.Zero)
                {
                    Console.WriteLine("Error: dCRT Code: " + Marshal.GetLastWin32Error());
                    return false;
                }
                IntPtr procAddress = WINAPI.GetProcAddress(WINAPI.GetModuleHandle("kernel32.dll"), "LoadLibraryA");
                if (procAddress == IntPtr.Zero)
                {
                    Console.WriteLine("Unable to find address of \"LoadLibraryA\".\n");
                    Console.WriteLine("Error code: " + Marshal.GetLastWin32Error());
                    return false;
                }
                IntPtr lpBaseAddress = WINAPI.VirtualAllocEx(hProcess, IntPtr.Zero, (IntPtr)sDllPath.Length, 0x3000, 0x40);
                if ((lpBaseAddress == IntPtr.Zero) && (lpBaseAddress == IntPtr.Zero))
                {
                    Console.WriteLine("Unable to allocate memory to target process.\n");
                    Console.WriteLine("Error code: " + Marshal.GetLastWin32Error());
                    return false;
                }
                byte[] byteLength = GetByteLength(sDllPath);
                IntPtr zero = IntPtr.Zero;
                WINAPI.WriteProcessMemory(hProcess, lpBaseAddress, byteLength, (uint)byteLength.Length, out zero);
                //if (Marshal.GetLastWin32Error() != 0)
                //{
                //    sError = "Unable to write memory to process.";
                //    sError = sError + "Error code: " + Marshal.GetLastWin32Error();
                //    return false;
                //}
                if (WINAPI.CreateRemoteThread(hProcess, IntPtr.Zero, IntPtr.Zero, procAddress, lpBaseAddress, 0, IntPtr.Zero) == IntPtr.Zero)
                {
                    Console.WriteLine("Memoryspace Error on LOAD_DLL\n");
                    Console.WriteLine("Error code: " + Marshal.GetLastWin32Error());
                    return false;
                }
                WINAPI.VirtualFreeEx(hProcess, IntPtr.Zero, UIntPtr.Zero, 0x8000);
                return true;
            }
            catch (Exception exception)
            {
                sError = "";

                hwnd = IntPtr.Zero;
                Console.WriteLine(exception.ToString());
                return false;
            }
        }

        public static bool DoInject(Process pToBeInjected, string sDllPath, out string sError)
        {
            sError = "";

            if (isModuleLoaded(Path.GetFileNameWithoutExtension(sDllPath) + ".dll", pToBeInjected))
            {
                return true;
            }

            IntPtr zero = IntPtr.Zero;
            return doCreateRemoteThread(pToBeInjected, sDllPath, out sError, out zero);
        }

        private static byte[] GetByteLength(string sToConvert)
        {
            return Encoding.ASCII.GetBytes(sToConvert);
        }

        private static class WINAPI
        {
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern int CloseHandle(IntPtr hObject);
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttribute, IntPtr dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern IntPtr GetModuleHandle(string lpModuleName);
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern IntPtr OpenProcess(uint dwDesiredAccess, int bInheritHandle, uint dwProcessId);
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, uint flAllocationType, uint flProtect);
            [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
            public static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, UIntPtr dwSize, uint dwFreeType);
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern int WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] buffer, uint size, out IntPtr lpNumberOfBytesWritten);

            public static class VAE_Enums
            {
                public enum AllocationType
                {
                    MEM_COMMIT = 0x1000,
                    MEM_RESERVE = 0x2000,
                    MEM_RELEASE = 0x8000
                }

                public enum ProtectionConstants
                {
                    PAGE_EXECUTE = 0x10,
                    PAGE_EXECUTE_READ = 0x20,
                    PAGE_EXECUTE_READWRITE = 0x40,
                    PAGE_EXECUTE_WRITECOPY = 0x80,
                    PAGE_NOACCESS = 1
                }
            }
        }
    }
}
