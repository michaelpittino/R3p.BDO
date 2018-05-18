using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Worker
{
    public class WorkerList : MemoryObject
    {
        public WorkerList()
        {
            Address = Offsets._workerList;
        }

        private byte WorkerCount => ReadByte(0x0010);
        private long ListStart => ReadPointer8b(0x0018);
        private long ListEnd => ReadPointer8b(0x0020);

        public WorkerData[] List => GetWorkers();

        private WorkerData[] _wBuffer;
        private int _oldCount;
        private WorkerData[] GetWorkers()
        {
            if (WorkerCount == 0)
                return null;

            if (WorkerCount > _oldCount)
            {
                _wBuffer = null;
                _oldCount = WorkerCount;
            }
            else
            {
                return _wBuffer;
            }

            WorkerData[] list = new WorkerData[WorkerCount];

            var start = ListStart;
            var end = ListEnd;
            int maxSize = (int)(end - start) / 0x08;

            WorkerData firstNode = new WorkerData(ReadPointer8b(0x8));

            WorkerData curNode = firstNode.x0000_Next;

            for (int i = 0; i < maxSize; i++)
            {
                WorkerData nextNode = curNode.x0000_Next;

                if (nextNode.x0008_Previous.Address == curNode.Address && curNode.Address != firstNode.Address)
                {
                    list[i] = curNode;

                    curNode = nextNode;
                }
                else
                {
                    break;
                }
            }

            if (_wBuffer == null)
                _wBuffer = list;

            return list;
        }
    }
}
