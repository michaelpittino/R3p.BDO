using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Actors
{
    public class CharacterScene : MemoryObject
    {
        public CharacterScene(long address)
        {
            Address = address;
        }

        public float[] ObjectLocation => ReadVec3(0x43C);
        public float Rotation => ReadFloat(0x43C);
        public float AnimationSpeed => ReadFloat(0x4BC);

        public void SetAnimationSpeed(float value)
        {
            Write(0x4BC, BitConverter.GetBytes(value));
        }

        public void SetRotation(float value)
        {
            Write(0x204, BitConverter.GetBytes(value));
        }

        public void SetLocation(float[] pos)
        {
            byte[] buffer = new byte[12];
            Array.Copy(BitConverter.GetBytes(pos[0]),0,buffer,0,4);
            Array.Copy(BitConverter.GetBytes(pos[1]), 0, buffer, 4, 4);
            Array.Copy(BitConverter.GetBytes(pos[2]), 0, buffer, 8, 4);

            Write(0x43C, buffer);
        }
    }
}
