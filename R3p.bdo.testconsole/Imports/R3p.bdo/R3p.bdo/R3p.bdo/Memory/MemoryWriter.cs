using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3p.bdo.Memory
{
    public class MemoryWriter
    {
        public static void Write(long address, byte[] data)
        {
            int writtenBytes;

            Win32.WriteProcessMemory((int)Engine.Instance.hProcess, address, data, data.Length, out writtenBytes);
        }
    }
}
