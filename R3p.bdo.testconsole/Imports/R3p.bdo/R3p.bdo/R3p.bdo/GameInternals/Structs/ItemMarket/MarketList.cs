using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.ItemMarket
{
    public class MarketList : MemoryObject
    {
        public MarketList()
        {
            Address = Offsets._marketBase;
        }

        private int x0000_ListCount => ReadInt32(0x50);
        private long x0008_ItemListStart => ReadPointer8b(0x58);
        private long x0010_ItemListEnd => ReadPointer8b(0x60);

        private long RegistredItemList_Start => ReadPointer8b(0xA0);
        private long RegistredItemList_End => ReadPointer8b(0xA8);

        public int RegistredItemCount => (int)(RegistredItemList_End - RegistredItemList_Start)/0xD0;

        public Dictionary<int, List<MarketItemData>> List => GetItemList();

        private Dictionary<int, List<MarketItemData>> _lBuffer;
        private Dictionary<int, List<MarketItemData>> GetItemList()
        {
            if(_lBuffer != null)
                return _lBuffer;

            Dictionary<int, List<MarketItemData>> itemList = new Dictionary<int, List<MarketItemData>>();

            var start = x0008_ItemListStart;
            var end = x0010_ItemListEnd;
            int maxSize = (int)(end - start) / 0x08;

            MarketItemData firstNode = new MarketItemData(ReadPointer8b(0x48));

            MarketItemData curNode = firstNode;

            for (int i = 0; i < maxSize; i++)
            {
                MarketItemData nextNode = curNode.x0008_NextItem;

                if (nextNode.x0000_PreviousItem.Address == curNode.Address && nextNode.Address != firstNode.Address)
                {
                    if (!itemList.ContainsKey(curNode.x0010_ItemId))
                        itemList.Add(curNode.x0010_ItemId, new List<MarketItemData>() { curNode });
                    else
                    {
                        itemList[curNode.x0010_ItemId].Add(curNode);
                    }

                    curNode = nextNode;
                }
                else
                {
                    break;
                }
            }

            if (_lBuffer == null)
                _lBuffer = itemList;

            return itemList;
        }
    }
}
