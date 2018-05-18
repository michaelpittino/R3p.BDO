using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.GameInternals.Structs.ItemMarket;

namespace R3p.bdo.GameExternals.Structs.AutoItemBuy
{
    public class ItemBuyObject
    {
        public bool Enabled { get; set; }
        public int ItemId { get; set; }
        public int EnchantLevel { get; set; }
        public int SessionMax { get; set; }
        public int SessionCurrent { get; set; }
        public long MaxPrice { get; set; }
        public bool IsStack { get; set; }
        public MarketItemData MarketItemData { get; set; }

        public ItemBuyObject(bool enabled, int itemId, int enchantLevel, int sessionMax, long maxPrice, bool isStack)
        {
            Enabled = enabled;
            ItemId = itemId;
            EnchantLevel = enchantLevel;
            SessionMax = sessionMax;
            MaxPrice = maxPrice;
            IsStack = isStack;
        }
        
    }
}
