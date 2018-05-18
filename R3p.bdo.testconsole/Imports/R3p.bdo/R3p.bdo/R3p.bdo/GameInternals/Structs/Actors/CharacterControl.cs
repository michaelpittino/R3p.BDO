using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Actors
{
    public class CharacterControl : MemoryObject
    {
        public CharacterControl(long address)
        {
            Address = address;
        }

        public CharacterScene CharacterScene => new CharacterScene(ReadPointer8b(0x10));
        public AnimationData CurrentAnimation => new AnimationData(ReadPointer8b(ReadPointer8b(0x38) + 0x20));
        public float JumpHeightIncrease => ReadFloat(0x744);

        public void SetInteractionPoint(float[] pos)
        {
            byte[] buffer = new byte[12];
            Array.Copy(BitConverter.GetBytes(pos[0]), 0, buffer, 0, 4);
            Array.Copy(BitConverter.GetBytes(pos[1]), 0, buffer, 4, 4);
            Array.Copy(BitConverter.GetBytes(pos[2]), 0, buffer, 8, 4);

            Write((ReadPointer8b(0x8) + 0xB0), buffer);
        }
    }
}
