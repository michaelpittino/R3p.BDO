using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3p.bdo.GameExternals.Structs.Overlay
{
    public class WaypointObject
    {
        public bool Enabled { get; set; }
        public int[] ColorCode { get; set; }
        public int Thickness { get; set; }
        public float[] Position { get; set; }
        public string Name { get; set; }

        public WaypointObject(bool enabled, int[] colorCode, int thickness, float[] position, string name)
        {
            Enabled = enabled;
            ColorCode = colorCode;
            Thickness = thickness;
            Position = position;
            Name = name;
        }
    }
}
