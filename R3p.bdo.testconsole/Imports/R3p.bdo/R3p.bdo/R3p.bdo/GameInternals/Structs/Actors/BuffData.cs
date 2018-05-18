using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Actors
{
    public class BuffData : MemoryObject
    {
        public BuffData(long address)
        {
            Address = address;
        }

        public BuffData x0000_NextActiveBuff => new BuffData(ReadPointer8b(0x0000));
        public BuffData x0000_PreviousActiveBuff => new BuffData(ReadPointer8b(0x0008));
        public string x0018_IconPath => ReadStringASCII(ReadPointer8b(0x0018));
        public BuffTableData x0038_BuffTableData => new BuffTableData(ReadPointer8b(0x0038));
        public int x0040_BuffEndTick => ReadInt32(0x0040);
        public bool x004C_IsActive => ReadByte(0x004C) != 0;
        
        public TimeSpan GetRemainingTime()
        {
            var curTick = Collection.Base.Ticks.BaseTicks.BaseTick;
            var buffEnd = x0040_BuffEndTick;
            var remaining = buffEnd - curTick;

            return TimeSpan.FromSeconds(remaining / 1000);
        }

        public string GetIconName()
        {
            if (x0038_BuffTableData.x0110_BuffIcon.Contains(@"\"))
                return x0038_BuffTableData.x0110_BuffIcon.Split('\\').Last().Replace(".dds", "").ToLower();

            return x0038_BuffTableData.x0110_BuffIcon.Split('/').Last().Replace(".dds", "").ToLower();
        }

        //public string GetName()
        //{
        //    if (!LanguageData._StringTable_BuffTable.ContainsKey(x0038_BuffTableData.x0000_Index))
        //        return "";

        //    return LanguageData._StringTable_BuffTable[x0038_BuffTableData.x0000_Index][0];
        //}
    }
}
