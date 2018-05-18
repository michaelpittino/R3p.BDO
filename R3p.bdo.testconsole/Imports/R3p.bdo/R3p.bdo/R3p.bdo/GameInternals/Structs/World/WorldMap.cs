using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.World
{
    public class WorldMap : MemoryObject
    {
        public WorldMap()
        {
            Address = ReadPointer8b(ReadPointer8b(ReadPointer8b(Offsets._base) + 0x0018) + 0x0160);
        }

        public float[] x00C0_CurrentLookAt => ReadVec3(0x00C0);
        public long PTR_WorldMapViewMatrix => Address + 0x05F4;
        public bool x0688_isOpened => ReadByte(0x700) == 1;
        public bool x0A23_isDesert => ReadByte(0x0A23) != 0;
        public bool x1848_isOcean => ReadByte(0x1848) != 0;

        public float[] CurrentNavigationTarget
            =>
                ReadVec3(
                    ReadPointer8b(ReadPointer8b(ReadPointer8b(ReadPointer8b(ReadPointer8b(Offsets._base) + 0x0018) + 0x0178) + 0x20) + 0x58));

        public bool isAutoNavigating
            =>
                ReadByte(ReadPointer8b(ReadPointer8b(ReadPointer8b(Offsets._base) + 0x0018) + 0x0178) + 0x18) != 0;

        public void setAutoNavigation(byte value)
        {
            Write(ReadPointer8b(ReadPointer8b(ReadPointer8b(Offsets._base) + 0x0018) + 0x0178) + 0x18, BitConverter.GetBytes((byte)value));
        }

        public void SetNewNavigation(float[] pos, bool autoPath)
        {
            Pipe.Call.WorldMapNaviStart(pos, (byte)Convert.ToInt32(autoPath), 0);
        }
    }
}
