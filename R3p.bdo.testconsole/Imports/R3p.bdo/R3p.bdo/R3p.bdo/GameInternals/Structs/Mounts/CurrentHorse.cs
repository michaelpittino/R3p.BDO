using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Mounts
{
    public class CurrentHorse : MemoryObject
    {
        public CurrentHorse()
        {
            Address = Offsets.CurrentMount;
        }

        public MountInventory Inventory => new MountInventory(ReadPointer8b(0xB4));
    }
}
