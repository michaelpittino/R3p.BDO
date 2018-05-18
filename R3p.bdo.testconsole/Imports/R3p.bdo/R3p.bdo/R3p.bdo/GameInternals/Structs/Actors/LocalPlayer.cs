using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Actors
{
    public class LocalPlayer : MemoryObject
    {
        public LocalPlayer()
        {
            Address = Offsets._localPlayer;
        }

        public ActorData PlayerData => new ActorData(ReadPointer8b(0x00));
    }
}
