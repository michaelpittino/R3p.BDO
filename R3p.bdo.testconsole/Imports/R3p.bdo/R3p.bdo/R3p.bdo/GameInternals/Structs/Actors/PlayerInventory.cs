using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Actors
{
    public class PlayerInventory : MemoryObject
    {
        public PlayerInventory(long address)
        {
            Address = address;
        }

        public List<PlayerInventoryItem> Items => GetAllInventoryItems(); 

        private List<PlayerInventoryItem> GetAllInventoryItems()
        {
            int startOffset = 0x08;
            int sizeOfEach = 0x80;
            int count = 194;

            List<PlayerInventoryItem> AllSlots = new List<PlayerInventoryItem>();

            for (int i = 0; i < count; i++)
            {
                var item = new PlayerInventoryItem(Address + (startOffset + (i * sizeOfEach)), i);

                AllSlots.Add(item);
            }

            return AllSlots;
        }
        
    }
}
