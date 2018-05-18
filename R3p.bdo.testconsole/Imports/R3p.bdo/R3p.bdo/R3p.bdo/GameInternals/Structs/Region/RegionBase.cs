using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Region
{
    public class RegionBase : MemoryObject
    {
        public RegionBase()
        {
            //Address = Offsets.RegionBase;
        }

        public RegionData x0080_CurrentRegion => new RegionData(ReadPointer8b(0x0080));
    }
}
