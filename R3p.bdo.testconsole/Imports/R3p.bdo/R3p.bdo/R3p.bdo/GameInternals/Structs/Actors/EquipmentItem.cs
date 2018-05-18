using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Actors
{
    public class EquipmentItem: MemoryObject
    {
        public EquipmentItem(long address)
        {
            Address = address;
        }

        public ItemData ItemData => new ItemData(ReadPointer8b(Address));
        public byte Durability => ReadByte(Address + 0x8);
    }
}
