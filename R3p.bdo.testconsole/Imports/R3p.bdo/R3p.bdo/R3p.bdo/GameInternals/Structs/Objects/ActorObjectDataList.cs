using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Objects
{
    public class ActorObjectDataList : MemoryObject
    {
        public ActorObjectDataList()
        {
            //Address = Offsets.ObjectDataList;
        }

        private int Count => ReadInt32(0x0000);
        private long ListStart => ReadPointer8b(0x0008);
        private long ListEnd => ReadPointer8b(0x0010);

        public List<ObjectData> List => GetList();

        private List<ObjectData> _lBuffer;
        private int _oldCount;
        private List<ObjectData> GetList()
        {
            if (Count != _oldCount)
            {
                _oldCount = Count;
                _lBuffer = null;
            }
            else
            {
                return _lBuffer;
            }

            var start = ListStart;
            var end = ListEnd;
            int maxSize = (int)(end - start) / 0x08;

            List<ObjectData> list = new List<ObjectData>();

            ObjectData firstNode = new ObjectData(ReadPointer8b(Address - 0x0008));

            ObjectData curNode = firstNode;

            for (int i = 0; i < maxSize; i++)
            {
                ObjectData nextNode = curNode.x0000_NextEntry;

                if (nextNode.x0008_PreviousEntry.Address == curNode.Address && nextNode.Address != firstNode.Address)
                {
                    list.Add(curNode);
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
