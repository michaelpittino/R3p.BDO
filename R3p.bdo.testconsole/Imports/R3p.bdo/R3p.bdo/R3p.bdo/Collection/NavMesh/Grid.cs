using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace R3p.bdo.Collection.NavMesh
{
    public class Vector3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3(float[] vec)
        {
            X = vec[0];
            Y = vec[1];
            Z = vec[2];
        }

        public float[] aFloat => new float[] {X, Y, Z};
    }

    public class Grid
    {
        public static Vector3[,] CurrentWorldGrid;
    }
}
