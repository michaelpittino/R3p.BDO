using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Worker
{
    public class WorkerData : MemoryObject
    {
        public WorkerData(long address)
        {
            Address = address;
        }

        public WorkerData x0000_Next => new WorkerData(ReadPointer8b(0x0000));
        public WorkerData x0008_Previous => new WorkerData(ReadPointer8b(0x0008));
        public long x0010_WorkerNo => ReadInt64(0x0010);
        public WorkerDetails x0028_WorkerDetails => new WorkerDetails(ReadPointer8b(0x0028));
        public int x0040_CharacterId => ReadInt16(0x0040);
        public byte x0042_CurrentStamina => ReadByte(0x0042);
        public byte x004A_Level => ReadByte(0x004A);
    }
}
