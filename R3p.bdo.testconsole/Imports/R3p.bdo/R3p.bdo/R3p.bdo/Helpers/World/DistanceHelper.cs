using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3p.bdo.Helpers.World
{
    public class DistanceHelper
    {
        public static double getDistance(float[] origin, float[] destination)
        {
            float xD = destination[0] - origin[0];
            float yD = destination[1] - origin[1];
            float zD = destination[2] - origin[2];

            return (float)Math.Sqrt(xD * xD + yD * yD + zD * zD);
        }
    }
}
