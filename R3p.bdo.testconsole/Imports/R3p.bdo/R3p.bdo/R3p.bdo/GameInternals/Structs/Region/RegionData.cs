using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Region
{
    public class RegionData : MemoryObject
    {
        public RegionData(long address)
        {
            Address = address;
        }

        public int x0000_RegionKey => ReadInt16(0x0000);
        public int x0008_RegionType => ReadInt32(0x0008);
        public int x000C_VillageSiegeType => ReadInt32(0x000C);
        public bool x0018_IsSafezone => ReadByte(0x0018) != 0;
        public byte x0019_IsArenaZone => ReadByte(0x0019);
        public bool x001B_IsDesert => ReadByte(0x001B) != 0;
        public bool x001D_IsOcean => ReadByte(0x001D) != 0;
        public byte x001E_IsPrison => ReadByte(0x001E);
        public byte x0021_IsVillageWarZone => ReadByte(0x0021);
        public byte x0022_IsKingOrLordWarZone => ReadByte(0x0022);
        public byte x0023_IsMainTown => ReadByte(0x0023);
        public byte x0024_IsMinorTown => ReadByte(0x0024);
        public byte x0025_IsMainOrMinorTown => ReadByte(0x0025);
        public bool x0026_IsAccessableArea => ReadByte(0x0026) != 0;
        public byte x0028_VillageTaxLevel => ReadByte(0x0028);
        public byte x0029_DropGroupRerollCountOfSieger => ReadByte(0x0029);
        public int x002C_RespawnWaypointKey => ReadInt32(0x002C);
        public float[] x0030_RespawnWorldPos => ReadVec3(0x0030);
        public byte x003C_IsAncientDungeon => ReadByte(0x003C);
        public long x0088_PTR_SiegeWrapper => ReadPointer8b(0x0088);
        public long x0090_PTR_MinorSiegeWrapper => ReadPointer8b(0x0090);
        public int x0098_TerritoryKeyRaw => ReadInt16(0x0098);
        public int x00E8_AffiliatedTownRegionKey => ReadInt16(0x00E8);
        public long x00F0_PTR_AffiliatedTownRegion => ReadPointer8b(0x00F0);
        public int x00F8_AffiliatedRegionKey => ReadInt16(0x00F8);
        public long x0100_PTR_N1024F728 => ReadPointer8b(0x0100);
        public int x0108_RegionGroupKey => ReadInt16(0x0108);
        public RegionInfo x0110_RegionInfo => new RegionInfo(ReadPointer8b(0xD8));
        public int x011C_AreaKey => ReadInt32(0x011C);
        public long x0128_PTR_MoreInfo => ReadPointer8b(0x0128);
        public int x0148_ExplorationKey => ReadInt32(0x0148);
        public long x0150_PTR_ExplorationStaticStatusWrapper => ReadPointer8b(0x0150);
        public float[] x0168_ReturnWorldPos => ReadVec3(0x0168);
        public int x0258_WarehouseManagerKey => ReadInt16(0x220);
        public int x0260_WorkerManagerKey => ReadInt16(0x0260);
        public int x0268_StableManagerKEy => ReadInt16(0x0268);
        public int x0278_ItemMarketManagerKey => ReadInt16(0x0278);
        public int x0280_DeliveryManagerKey => ReadInt16(0x0280);
        public byte x0288_IsFreeRevivalArea => ReadByte(0x0288);

        public bool isRedBattleField => x0000_RegionKey == 801 || x0000_RegionKey == 436;

        public void SetNonDesert()
        {
            if(x001B_IsDesert)
                Write(0x1B, BitConverter.GetBytes((byte)0));
        }

        public void SetNonOcean()
        {
            if (x001D_IsOcean)
                Write(0x1D, BitConverter.GetBytes((byte)0));
        }

        public void SetAccessable()
        {
            if(!x0026_IsAccessableArea)
                Write(0x26, BitConverter.GetBytes((byte)1));
        }
    }
}
