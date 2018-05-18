using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using R3p.bdo.GameInternals.Structs.Actors;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Loot
{
    public class LootItem : MemoryObject
    {
        private int SlotNO { get; set; }
        
        public LootItem(long address, int slotNo)
        {
            Address = address;
            SlotNO = slotNo;
        }

        public ItemData ItemData => new ItemData(ReadPointer8b(0x8));
        public int Count => ReadInt32(0x10);

        public void Pickup()
        {
            Pipe.Call.lootingSlotClick(SlotNO, Count);
            Thread.Sleep(50);
        }
    }
}
