using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Objects
{
    public class ObjectData : MemoryObject
    {
        public ObjectData(long address)
        {
            Address = address;
        }

        public ObjectData x0000_NextEntry => new ObjectData(ReadPointer8b(0x0000));
        public ObjectData x0008_PreviousEntry => new ObjectData(ReadPointer8b(0x0008));
        public ActorObjectData X0010_3DActorObjectData => new ActorObjectData(ReadPointer8b(0x0010));
    }
}
