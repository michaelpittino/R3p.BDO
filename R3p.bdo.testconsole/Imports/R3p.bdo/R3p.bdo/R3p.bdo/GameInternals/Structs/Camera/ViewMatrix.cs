using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Camera
{
    public class ViewMatrix : MemoryObject
    {
        public ViewMatrix()
        {
            Address = 0;//Offsets._vMatrix;
        }

        public float[,] WorldViewMatrix => ReadMatrix(0x00);
        //public bool FlickerCheck => !ReadFloat(0x74).Equals(0f);
        public float[,] WorldMapViewMatrix => ReadMatrix(Collection.World.Base.WorldMap.PTR_WorldMapViewMatrix);
    }
}
