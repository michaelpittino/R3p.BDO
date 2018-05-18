using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Actors
{
    public class ItemData : MemoryObject
    {
        public class Offsets
        {
            public static int _oEnchantLevel = 0x03;
            public static int _oBasePriceForItemMarket = 0x20;
            public static int _oName = 0xE4;
            public static int _oItemIndex = 0x10C;
            public static int _oItemType = 0x110;
            public static int _oGradeType = 0x112;
            public static int _oEquipType = 0x113;
            public static int _oEquipSlotNo = 0x11A;
            public static int _oItemClassify = 0x111;
            public static int _oWeight = 0x15C;
            public static int _oIsStack = 0x160;
            public static int _oOriginalPrice = 0x1A8;
            public static int _oSellPriceToNpc = 0x1B0;
            public static int _oRepairPrice = 0x1D0;
            public static int _oMaxRegisterCountForItemMarket = 0x1F0;
            public static int _oCollectToolType = 0x235;
        }

        public ItemData(long address)
        {
            Address = address;
        }
        
        public uint ItemIndex => ReadInt16(Offsets._oItemIndex);
        public byte EnchantLevel => ReadByte(Offsets._oEnchantLevel);
        
        public string Name => Collection.DeezNutz.Base.List.GetNameEntry(ReadInt32(Offsets._oName)).Text;
        
        public ItemType ItemType
        {
            get
            {
                int type = ReadByte(Offsets._oItemType);
                if (Enum.IsDefined(typeof(ItemType), type))
                    return (ItemType)type;
                return default(ItemType);
            }
        }

        public ItemClassifyType ItemClassify
        {
            get
            {
                int type = ReadByte(Offsets._oItemClassify);
                if (Enum.IsDefined(typeof(ItemClassifyType), type))
                    return (ItemClassifyType)type;
                return default(ItemClassifyType);
            }
        }

        public byte GradeType => ReadByte(Offsets._oGradeType);
        public byte EquipType => ReadByte(Offsets._oEquipType);
        public int Weight => ReadInt32(Offsets._oWeight) /10000;
        public byte IsStack => ReadByte(Offsets._oIsStack);
        public int OriginalPrice => ReadInt32(Offsets._oOriginalPrice);
        public int SellPriceToNPC => ReadInt32(Offsets._oSellPriceToNpc);
        public int RepairPrice => ReadInt32(Offsets._oRepairPrice);
        public int BasePriceForItemMarket => ReadInt32(Offsets._oBasePriceForItemMarket);
        public int MaxRegisterCountForItemMarket => ReadInt16(Offsets._oMaxRegisterCountForItemMarket);
        
        public CollectToolType CollectToolType
        {
            get
            {
                int type = ReadByte(Offsets._oCollectToolType);
                if (Enum.IsDefined(typeof(CollectToolType), type))
                    return (CollectToolType)type;
                return default(CollectToolType);
            }
        }
    }
}
