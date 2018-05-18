using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Quests
{
    public class GuildQuestData : MemoryObject
    {
        public GuildQuestData(long address)
        {
            Address = address;
        }

        public string ConditionRaw => ReadStringUnicode(ReadPointer8b(0x0));
        public string ConditionText => ReadStringUnicode(ReadPointer8b(0x20));
        public int QuestGrade => ReadInt16(0x98);
        public string Title => ReadStringUnicode(ReadPointer8b(0xA0));
        public string Description => ReadStringUnicode(ReadPointer8b(0xC0));
        public string IconPath => ReadStringUnicode(ReadPointer8b(0xE0));
        public int TimeLimit => ReadInt32(0x100);
        public long PreGoldCount => ReadInt64(0x110);
        public int QuestId => ReadInt16(0x12C);

        public bool FixDescription()
        {
            if (ReadPointer8b(0xC0) != ReadPointer8b(0x20))
            {
                Write(0xC0, BitConverter.GetBytes(ReadPointer8b(0x20)));

                Log.Post("Transformed GuildQuest Descriptiion", LogModule.Hack_UI);

                return true;
            }

            return false;
        }
    }
}
