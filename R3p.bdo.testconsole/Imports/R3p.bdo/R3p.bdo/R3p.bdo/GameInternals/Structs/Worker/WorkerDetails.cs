using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Worker
{
    public class WorkerDetails : MemoryObject
    {
        public WorkerDetails(long address)
        {
            Address = address;
        }

        public int x0000_CharacterId => ReadInt16(0x0000);
        public int x0014_MaxStamina => ReadInt32(0x0014);
    }
}
