using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.SystemVariables
{
    public class SystemVariable : MemoryObject
    {
        public string Name { get; set; }

        public SystemVariable(long address)
        {
            Address = address;
            Name = ReadStringASCII(ReadPointer8b(0x18));
        }

        public long someAdr => ReadPointer8b(0x0);
        public bool Enabled => ReadByte(0x8) != 0;
        public int ValueInt => ReadInt32(0x10);
        public float ValueFloat => ReadFloat(0x10);
        
        public void SetEnabled(byte value)
        {
            Write(0x8, BitConverter.GetBytes(value));
        }

        public void SetValue(int value)
        {
            Write(0x10, BitConverter.GetBytes(value));
        }

        public void SetValue(float value)
        {
            Write(0x10, BitConverter.GetBytes(value));
        }
    }
}
