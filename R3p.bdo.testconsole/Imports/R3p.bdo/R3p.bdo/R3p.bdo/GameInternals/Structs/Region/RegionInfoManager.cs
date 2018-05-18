using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Region
{
    public class RegionInfoManager : MemoryObject
    {
        public RegionInfoManager()
        {
            Address = Offsets._regionInfoManager;
        }

        private RegionListEntry RegionList_FirstEntry => new RegionListEntry(ReadPointer8b(0x10));
        private int RegionList_Count => ReadInt32(0x18);
        private long RegionList_ListStart => ReadPointer8b(0x20);
        private long RegionList_ListEnd => ReadPointer8b(0x28);

        public List<RegionData> RegionList => GetRegionList();

        private List<RegionData> _lBuffer;
        public List<RegionData> GetRegionList()
        {
            if (_lBuffer != null)
                return _lBuffer;

            List<RegionData> list = new List<RegionData>();

            var start = RegionList_ListStart;
            var end = RegionList_ListEnd;
            int maxSize = (int)(end - start) / 0x08;

            RegionListEntry firstNode = RegionList_FirstEntry;

            RegionListEntry curNode = firstNode;

            for (int i = 0; i < maxSize; i++)
            {
                RegionListEntry nextNode = curNode.NextNode;

                if (nextNode.PreviousNode.Address == curNode.Address && nextNode.Address != firstNode.Address)
                {
                    list.Add(curNode.RegionData);

                    curNode = nextNode;
                }
                else
                {
                    break;
                }
            }

            if (_lBuffer == null)
                _lBuffer = list;

            return list;
        }
    }
}
