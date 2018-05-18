using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3p.bdo.Helpers.NavMesh
{
    public class Grid
    {
        public static List<float[]> GenerateGrid(List<float[]> nodes)
        {
            List<float[]> grid = new List<float[]>();

            float minX = nodes.Min(x => x[0]);
            float minZ = nodes.Min(x => x[2]);

            int width = (int)(nodes.Max(x => x[0]) - minX);
            int depth = (int)(nodes.Max(x => x[2]) - minZ);

            int gridSize = 250;

            for (int w = 0; w < width/gridSize; w++)
            {
                for (int d = 0; d < depth/gridSize; d++)
                {
                    float[] b = new float[] { minX + (w * gridSize),0 , minZ + (d * gridSize)};

                    var nearbyNode = GetNearbyNode(nodes, b[0], b[2], 500);

                    if(!nearbyNode[1].Equals(0))
                        b[1] = nearbyNode[1];
                    else
                    {
                        if(grid.Count > 0)
                        b[1] = grid.Last()[1];
                        else
                        {
                            continue;
                        }
                    }

                    grid.Add(b);
                }
            }

            return grid;
        }

        private static float[] GetNearbyNode(List<float[]> nodes, float X, float Z, float tolerance)
        {
            float[] node = new float[] {0,0,0};

            for (int i = 0; i < nodes.Count; i++)
            {
                var n = nodes[i];

                if ((n[0] <= X + tolerance && n[0] >= X - tolerance) &&
                    n[2] <= Z + tolerance && n[2] >= Z - tolerance)
                    return n;
            }

            return node;
        }
    }
}
