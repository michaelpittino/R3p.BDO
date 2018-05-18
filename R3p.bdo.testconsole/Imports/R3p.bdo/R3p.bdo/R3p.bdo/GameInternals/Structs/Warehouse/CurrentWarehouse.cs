using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Warehouse
{
    public class CurrentWarehouse : MemoryObject
    {
        public CurrentWarehouse()
        {
            Address = ReadPointer8b(Offsets._currentWarehouse);
        }

        public WarehouseInventory Inventory => new WarehouseInventory(ReadPointer8b(0x08));
    }
}
