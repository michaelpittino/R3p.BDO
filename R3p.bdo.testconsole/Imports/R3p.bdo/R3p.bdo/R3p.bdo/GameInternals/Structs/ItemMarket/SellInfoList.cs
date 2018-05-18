using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.ItemMarket
{
    public class SellInfoList : MemoryObject
    {
        public SellInfoList(long address)
        {
            Address = address;
        }

        public List<SellInfo> List => GetList();

        private List<SellInfo> GetList()
        {
            List<SellInfo> itemList = new List<SellInfo>();
            
            int maxSize = 30;

            SellInfo firstNode = new SellInfo(ReadPointer8b(0x0));

            SellInfo curNode = firstNode;

            for (int i = 0; i < maxSize; i++)
            {
                SellInfo nextNode = curNode.Next;

                if (curNode.Address != Address)
                {
                    itemList.Add(curNode);

                    curNode = nextNode;
                }
                else
                {
                    break;
                }
            }
            
            return itemList;
        } 
    }
}
