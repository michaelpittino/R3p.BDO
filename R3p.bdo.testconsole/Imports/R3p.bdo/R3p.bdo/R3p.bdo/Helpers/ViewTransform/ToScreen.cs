using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using R3p.bdo.GameInternals.Structs.Camera;

namespace R3p.bdo.Helpers.ViewTransform
{
    public class ToScreen
    {
        public static bool WorldToScreen(float[] WorldPos, out int[] Screen, bool WorldMap = false)
        {
            float[,] worldViewMatrix = Collection.Camera.ViewMatrix.PVMatrix.WorldViewMatrix;
            if (WorldMap)
                worldViewMatrix = Collection.Camera.ViewMatrix.PVMatrix.WorldMapViewMatrix;

            Int32Rect cRect = Collection.Client.Base.ClientRect;
            Screen = new int[] {0,0};

            if (cRect == Int32Rect.Empty)
                return false;

            double[] bScreen = new double[2];

            var aspectRatio = cRect.Width / cRect.Height;

            float w = 0.0f;

            bScreen[0] = worldViewMatrix[0,0] * WorldPos[0] + worldViewMatrix[0,1] * WorldPos[1] + worldViewMatrix[0,2] * WorldPos[2] + worldViewMatrix[0,3];
            bScreen[1] = worldViewMatrix[1,0] * WorldPos[0] + worldViewMatrix[1,1] * WorldPos[1] + worldViewMatrix[1,2] * WorldPos[2] + worldViewMatrix[1,3];
            w = (float)(worldViewMatrix[3,0] * WorldPos[0] + worldViewMatrix[3,1] * WorldPos[1] + worldViewMatrix[3,2] * WorldPos[2] + worldViewMatrix[3,3]);

            if (w < 0.01f)
                return false;

            float invw = 1.0f / w;
            bScreen[0] *= invw;
            bScreen[1] *= invw;

            float x = (float)cRect.Width / 2;
            float y = (float)cRect.Height / 2;

            x += (float)(0.5 * bScreen[0] * cRect.Width + 0.5);
            y -= (float)(0.5 * bScreen[1] * cRect.Height + 0.5);

            bScreen[0] = x + cRect.X; //rect.left
            bScreen[1] = y + cRect.Y; //rect.top

            Screen[0] = (int) bScreen[0];
            Screen[1] = (int) bScreen[1];

            return true;
        }
    }
}
