using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Camera
{
    public class CameraManager : MemoryObject
    {
        public class Offsets
        {
            public static int OCameraRoll = 0x58;
            public static int OCameraPich = 0x5C;
            public static int OCameraYaw = 0x68;
        }

        public CameraManager()
        {
            Address = ReadPointer8b(ReadPointer8b(bdo.Offsets._base) + 0x30);
        }

        public float CameraRoll => ReadFloat(Offsets.OCameraRoll);
        public float CameraPich => ReadFloat(Offsets.OCameraPich);
        public float CameraYaw => ReadFloat(Offsets.OCameraYaw);

        public void setCameraYaw(float value)
        {
            Write(Offsets.OCameraYaw, BitConverter.GetBytes(value));
        }

        public void setCameraPich(float value)
        {
            Write(Offsets.OCameraPich, BitConverter.GetBytes(value));
        }
    }
}
