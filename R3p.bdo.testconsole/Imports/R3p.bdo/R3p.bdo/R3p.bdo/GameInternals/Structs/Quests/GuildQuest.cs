using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Quests
{
    public class GuildQuest : MemoryObject
    {
        public GuildQuest(long address)
        {
            Address = address;
        }
        
        public GuildQuestData GuildQuestData => new GuildQuestData(ReadPointer8b(0x08));
    }
}
