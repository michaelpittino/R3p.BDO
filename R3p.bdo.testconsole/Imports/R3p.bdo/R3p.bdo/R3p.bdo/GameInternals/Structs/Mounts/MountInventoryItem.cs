using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.GameInternals.Structs.Actors;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Mounts
{
    public class MountInventoryItem : MemoryObject
    {
        public MountInventoryItem(long address)
        {
            Address = address;
        }

        public ItemData x0008_PTR_ItemData => new ItemData(ReadPointer8b(0x0008));
        public int x0010_ItemCount => ReadInt32(0x0010);
        public uint x001A_CurrentDurability => ReadInt16(0x001A);
        public uint x0030_MaxDurability => ReadInt16(0x0030);
        public uint x0032_N29D0079B => ReadInt16(0x0032);
        public uint x0034_N29EB256F => ReadInt16(0x0034);
        public uint x0050_N29EB339B => ReadInt16(0x0050);
        public ItemData x0058_PTR_SocketedCrystal0 => new ItemData(ReadPointer8b(0x0058));
        public ItemData x0060_PTR_SocketedCrystal1 => new ItemData(ReadPointer8b(0x0060));
    }
}
