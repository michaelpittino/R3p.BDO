using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Warehouse
{
    public class WarehouseInventory : MemoryObject
    {
        public WarehouseInventory(long address)
        {
            Address = address;
        }

        public List<WarehouseInventoryItem> Items => GetAllInventoryItems(); 

        private List<WarehouseInventoryItem> GetAllInventoryItems()
        {
            int startOffset = 0x00;
            int sizeOfEach = 0x88;
            int count = 192;

            var start = Address + startOffset;

            List<WarehouseInventoryItem> AllSlots = new List<WarehouseInventoryItem>();

            for (int i = 0; i < count; i++)
            {
                var item = new WarehouseInventoryItem(start, i);

                AllSlots.Add(item);

                start += sizeOfEach;
            }

            return AllSlots;
        }
    }
}
