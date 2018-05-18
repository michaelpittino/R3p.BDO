using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3p.bdo.GameExternals.Structs.Overlay
{
    public class ActorObject
    {
        public bool Enabled { get; set; }
        public ActorType ActorType { get; set; }
        public int[] ActorIds { get; set; }
        public int[] ColorCode { get; set; }
        public bool DrawCircle { get; set; }
        public bool DrawLine { get; set; }
        public int Thickness { get; set; }
        public bool ShowName { get; set; }
        public bool ShowActorId { get; set; }
        public bool ShowDistance { get; set; }
        public bool ShowOnWorldMap { get; set; }
        public int MinDistance { get; set; }
        public int MaxDistance { get; set; }
        public bool ShowLevel { get; set; }
        public bool ShowHp { get; set; }

        public ActorObject(bool enabled, ActorType actorType, int[] actorIds, int[] colorCode, bool drawCircle, bool drawLine, int thickness, bool showName, bool showActorId, bool showDistance, bool showOnWorldMap, int minDistance, int maxDistance, bool showLevel, bool showHp)
        {
            Enabled = enabled;
            ActorType = actorType;
            ActorIds = actorIds;
            ColorCode = colorCode;
            DrawCircle = drawCircle;
            DrawLine = drawLine;
            Thickness = thickness;
            ShowName = showName;
            ShowActorId = showActorId;
            ShowDistance = showDistance;
            ShowOnWorldMap = showOnWorldMap;
            MinDistance = minDistance;
            MaxDistance = maxDistance;
            ShowLevel = showLevel;
            ShowHp = showHp;
        }
    }
}
