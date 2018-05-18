using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Collection.NavMesh;

namespace R3p.bdo.Helpers.NavMesh
{
    public class Nodes
    {
        public static void LoadWalkable(string folder, string maptile)
        {
            Collection.NavMesh.Nodes.walkable.Clear();

            var file = Directory.GetFiles(folder, maptile + ".bin", SearchOption.TopDirectoryOnly).FirstOrDefault();

            if (file == null)
                return;

            Collection.NavMesh.Nodes.walkable = LoadVertices(File.ReadAllBytes(file));
        }

        public static void LoadBlocked(string folder, string maptile)
        {
            Collection.NavMesh.Nodes.blocked.Clear();

            var file = Directory.GetFiles(folder, maptile + ".bin", SearchOption.TopDirectoryOnly).FirstOrDefault();

            if (file == null)
                return;

            Collection.NavMesh.Nodes.blocked = LoadVertices(File.ReadAllBytes(file));
        }

        public static void LoadWorldGrid(string folder, string maptile)
        {
            Collection.NavMesh.Grid.CurrentWorldGrid = null;

            var file = Directory.GetFiles(folder, maptile + ".bin", SearchOption.TopDirectoryOnly).FirstOrDefault();

            if (file == null)
                return;

            Collection.NavMesh.Grid.CurrentWorldGrid = LoadVertexGrid(File.ReadAllBytes(file));
        }

        private static List<float[]> LoadVertices(byte[] buffer)
        {
            MemoryStream mStream = new MemoryStream(buffer);
            BinaryReader bReader = new BinaryReader(mStream, Encoding.Default, true);

            List<float[]> list = new List<float[]>();

            int count = bReader.ReadInt32();

            for (int i = 0; i < count; i++)
            {
                list.Add(new float[] {bReader.ReadSingle(), bReader.ReadSingle(), bReader.ReadSingle()});
            }

            return list;
        }

        private static Vector3[,] LoadVertexGrid(byte[] buffer)
        {
            MemoryStream mStream = new MemoryStream(buffer);
            BinaryReader bReader = new BinaryReader(mStream, Encoding.Default, true);
            
            int width = bReader.ReadInt32();
            int depth = bReader.ReadInt32();

            Vector3[,] list = new Vector3[width, depth];

            for (int w = 0; w < width; w++)
            {
                for (int d = 0; d < depth; d++)
                {
                    list[w,d] = new Vector3(bReader.ReadSingle(), bReader.ReadSingle(), bReader.ReadSingle());
                }
            }

            return list;
        }
    }
}
