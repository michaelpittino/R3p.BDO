using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Skills
{
    public class SkillInfo : MemoryObject
    {
        public SkillInfo(long address)
        {
            Address = address;
        }
    }
}
