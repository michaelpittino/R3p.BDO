using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3p.bdo.GameExternals.Structs.DataLog
{
    public class ActorDataLogEntry
    {
        public int ActorId { get; set; }
        public int ActorKey { get; set; }
        public ActorType ActorType { get; set; }
        public string Name { get; set; }
        public float[] Pos { get; set; }
        public int MaxHp { get; set; }

        public ActorDataLogEntry(int actorId, int actorKey, ActorType actorType, string name, float[] pos, int maxHp)
        {
            ActorId = actorId;
            ActorKey = actorKey;
            ActorType = actorType;
            Name = name;
            Pos = pos;
            MaxHp = maxHp;
        }
    }
}
