using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Base
{
    public class Ticks : MemoryObject
    {
        public Ticks()
        {
            
        }

        public int BaseTick => GetBaseTick();
       
        private int GetBaseTick()
        {
            var tick = System.Environment.TickCount;
            var baseValue = ReadInt32(Offsets._tickBase);

            return baseValue + tick;
        }
    }
}
