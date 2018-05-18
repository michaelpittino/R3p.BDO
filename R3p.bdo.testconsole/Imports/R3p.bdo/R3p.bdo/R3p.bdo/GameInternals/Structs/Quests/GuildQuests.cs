using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Quests
{
    public class GuildQuests : MemoryObject
    {
        public GuildQuests()
        {
            Address = Offsets._guildQuestBase;
        }

        private long ListStart => ReadPointer8b(0x00);
        private long ListEnd => ReadPointer8b(0x08);

        public List<GuildQuest> List => GetList();
        public GuildQuestData CurrentGuildQuest => new GuildQuestData(ReadPointer8b(0x20));

        private List<GuildQuest> GetList()
        {
            var Entries = (ListEnd - ListStart)/24;

            List<GuildQuest> list = new List<GuildQuest>();

            for(int i = 0; i < Entries; i++)
            {
                list.Add(new GuildQuest(ListStart + (i*24)));
            }

            return list;
        } 
    }
}
