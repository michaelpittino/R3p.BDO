using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Actors
{
    public class VTable : MemoryObject
    {
        public VTable(long address)
        {
            Address = address;
        }
    }
}
