using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Region
{
    public class RegionInfo : MemoryObject
    {
        public RegionInfo(long address)
        {
            Address = address;
        }

        public double x0044_CropProductivityPercentage => ReadInt32(0x0044) / 10000;
        public double x0048_FishingProductivityPercentage => ReadInt32(0x0048) / 10000;
        public double x004C_LoyalityPercentage => ReadInt32(0x004C) / 10000;
    }
}
