using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.GameInternals.Structs.Warehouse;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Mounts
{
    public class MountInventory : MemoryObject
    {
        public MountInventory(long address)
        {
            Address = address;
        }

        public List<MountInventoryItem> Items => GetAllInventoryItems();

        private List<MountInventoryItem> GetAllInventoryItems()
        {
            int startOffset = 0x00;
            int sizeOfEach = 0x80;
            int count = 20;

            var start = Address + startOffset;

            List<MountInventoryItem> AllSlots = new List<MountInventoryItem>();

            for (int i = 0; i < count; i++)
            {
                var item = new MountInventoryItem(start);

                AllSlots.Add(item);

                start += sizeOfEach;
            }

            return AllSlots;
        }
    }
}
