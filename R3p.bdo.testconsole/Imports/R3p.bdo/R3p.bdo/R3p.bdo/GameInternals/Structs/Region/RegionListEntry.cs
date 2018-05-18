using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Region
{
    public class RegionListEntry : MemoryObject
    {
        public RegionListEntry(long address)
        {
            Address = address;
        }

        public RegionListEntry NextNode => new RegionListEntry(ReadPointer8b(Address));
        public RegionListEntry PreviousNode => new RegionListEntry(ReadPointer8b(0x8));
        public RegionData RegionData => new RegionData(ReadPointer8b(0x18));
    }
}
