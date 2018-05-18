using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Actors
{
    public class BuffList : MemoryObject
    {
        public long Address { get; set; }
        public int Count { get; set; }
        public BuffData FirstNode { get; set; }
        public long Start { get; set; }
        public long End { get; set; }

        public BuffList(long address)
        {
            Address = address;
            Count = ReadInt32(Address-0x08);
            FirstNode = new BuffData(ReadPointer8b(Address-0x10));
            Start = ReadPointer8b(Address);
            End = ReadPointer8b(0x08);
        }

        public Dictionary<int, BuffData> List => GetActiveBuffs();


        private Dictionary<int, BuffData> _buffer = new Dictionary<int, BuffData>();
        private int _oldCount;
        private Dictionary<int, BuffData> GetActiveBuffs()
        {
            if (!Count.Equals(_oldCount))
            {
                _oldCount = Count;
                _buffer = null;
            }
            else
            {
                return _buffer;
            }

            Dictionary<int, BuffData> list = new Dictionary<int, BuffData>();

            var start = Start;
            var end = End;
            int maxSize = 500;

            BuffData curNode = FirstNode;

            for (int i = 0; i < maxSize; i++)
            {
                BuffData nextNode = curNode.x0000_NextActiveBuff;

                if (nextNode.x0000_PreviousActiveBuff.Address == curNode.Address)
                {
                    if (!list.ContainsKey(curNode.x0038_BuffTableData.x0000_Index) && curNode.Address != FirstNode.Address)
                        list.Add(curNode.x0038_BuffTableData.x0000_Index, curNode);

                    curNode = nextNode;
                }
                else
                {
                    break;
                }
            }

            if (_buffer == null)
                _buffer = list;

            return list;
        }
    }
}
