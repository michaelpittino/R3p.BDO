using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Actors
{
    public class ActorListEntry : MemoryObject
    {
        public ActorListEntry(long address)
        {
            Address = address;
        }

        public ActorListEntry Next => new ActorListEntry(ReadPointer8b(0x00));
        public ActorListEntry Previous => new ActorListEntry(ReadPointer8b(0x08));

        public ActorData ActorData => new ActorData(ReadPointer8b(0x18));
    }

    public class ActorList : MemoryObject
    {
        public ActorList()
        {
            Address = Offsets._actorList;
        }
        
        private long ActorListFirstEntry => ReadPointer8b(0x00);

        public int ActorListCount => ReadInt32(0x08);

        public ActorData CurrentInteractee => new ActorData(ReadPointer8b(0x3B0));
        public ActorData OriginalInteractee => new ActorData(ReadPointer8b(0x3C8));

        public void SetInteractee(ActorData actor)
        {
            Write(0x3B0, BitConverter.GetBytes(actor.Address));
        }

        public List<ActorData> List => GetList();

        private List<ActorData> GetList()
        {
            List<ActorData> buffer = new List<ActorData>();

            ActorListEntry firstEntry = new ActorListEntry(ActorListFirstEntry);
            ActorListEntry currentEntry = firstEntry.Next;

            while (currentEntry.Address != firstEntry.Address)
            {
                buffer.Add(currentEntry.ActorData);

                currentEntry = currentEntry.Next;
            }

            return buffer;
        }
    }
}
