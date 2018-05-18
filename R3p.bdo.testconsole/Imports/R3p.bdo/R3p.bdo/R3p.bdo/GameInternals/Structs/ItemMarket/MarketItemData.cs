using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.GameInternals.Structs.Actors;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.ItemMarket
{
    public class MarketItemData : MemoryObject
    {
        public MarketItemData(long address)
        {
            Address = address;
        }

        public MarketItemData x0000_PreviousItem => new MarketItemData(ReadPointer8b(0x0000));
        public MarketItemData x0008_NextItem => new MarketItemData(ReadPointer8b(0x0008));
        public int x0010_ItemId => ReadInt16(0x0010);
        public int x0010_MarketItemId => ReadInt32(0x10);
        public byte x0012_EnchantLevel => ReadByte(0x0013);
        public ItemData ItemData => new ItemData(ReadPointer8b(ReadPointer8b(0x20)));
        public long x0038_RecentPrice => ReadInt64(0x0038);
        public long x0048_MaxPrice => ReadInt64(0x0048);
        public long x0050_MinPrice => ReadInt64(0x0050);
        public long x0060_RegistredCount => ReadInt64(0x0060);
        public long x0070_TotalTradeCount => ReadInt64(0x0070);
        public SellInfoList SellInfoList => new SellInfoList(ReadPointer8b(0x80));
    }
}
