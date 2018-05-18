using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.ItemMarket
{
    public class SellInfo : MemoryObject
    {
        public bool _joinedBid = false;

        public SellInfo(long address)
        {
            Address = address;
        }

        public SellInfo Next => new SellInfo(ReadPointer8b(0x0));
        public SellInfo Previous => new SellInfo(ReadPointer8b(0x8));
        public long UniqueKey => ReadInt64(0x18);
        public int Count => ReadInt32(0x28);
        public long Price => ReadInt64(0x40);
        public long BiddingResultTick => ReadInt64(0xB0);
        public long BiddingFreeForAllTickAll => ReadInt64(0xA8);

        public bool canJoinBid()
        {
            return Collection.Base.Ticks.BaseTicks.BaseTick <= BiddingResultTick;
        }

        public bool canBuyResult()
        {
            return !canJoinBid();
        }

        public bool isBid()
        {
            return Collection.Base.Ticks.BaseTicks.BaseTick <= BiddingFreeForAllTickAll;
        }
    }
}
