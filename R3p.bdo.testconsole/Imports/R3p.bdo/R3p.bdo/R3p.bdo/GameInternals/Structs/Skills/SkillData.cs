using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Skills
{
    public class SkillData : MemoryObject
    {
        public SkillData(long address)
        {
            Address = address;
        }

        public SkillData Next => new SkillData(ReadPointer8b(0x00));
        public SkillData Previous => new SkillData(ReadPointer8b(0x08));
        public int SkillId => ReadInt16(0x10);
        public long CooldownEndTick => ReadInt64(0x20);

        public SkillInfo SkillInfo => new SkillInfo(ReadPointer8b(0x38));

        public bool isCooldown()
        {
            if (CooldownEndTick == 0)
                return false;

            return Collection.Base.Ticks.BaseTicks.BaseTick <= CooldownEndTick;
        }
    }
}
