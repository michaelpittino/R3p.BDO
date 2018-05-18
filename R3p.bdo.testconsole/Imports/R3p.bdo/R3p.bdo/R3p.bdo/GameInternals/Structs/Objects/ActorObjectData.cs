using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Objects
{
    public class ActorObjectData : MemoryObject
    {
        public ActorObjectData(long address)
        {
            Address = address;
        }

        public float[] HitBox => ReadVec3(0x001C);
        public float Rotation => ReadFloat(0x28);
        public int ActorKey => ReadInt32(0x03F0);
        public int ActorId => ReadInt32(0x03F4);
        public float AnimationSpeed => ReadFloat(0x0494);
        public bool isSelected => ReadByte(0x0565) != 0;
        public bool isGlow => ReadByte(0x0567) != 0;
        public float glowA => ReadFloat(0x056C);
        public float[] glowRGB => ReadVec3(0x0570);
    }
}
