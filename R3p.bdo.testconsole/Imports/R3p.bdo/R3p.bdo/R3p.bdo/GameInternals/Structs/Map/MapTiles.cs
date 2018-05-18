using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Map
{
    public class MapTiles : MemoryObject
    {
        public MapTiles()
        {
            //Address = Offsets.currentMapTile;
        }

        public int MapTileX => ReadInt32(0x0);
        public int MapTileY => ReadInt32(0x4);
        public int MapTileZ => ReadInt32(0x8);

        public string getMapTilePosition()
        {
            return String.Join("_", MapTileX, MapTileY, MapTileZ);
        }
    }
}
