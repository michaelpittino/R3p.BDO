using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Actors
{
    public class BuffTableData : MemoryObject
    {
        public BuffTableData(long address)
        {
            Address = address;
        }

        public int x0000_Index => ReadInt16(0x0000);
        public int x0002_Value13 => ReadInt16(0x0002);
        public int x0008_Value14 => ReadInt16(0x0008);
        public int x000A_Value15 => ReadInt16(0x000A);
        public byte x0018_Value0 => ReadByte(0x0018);
        public byte x0020_Value1 => ReadByte(0x0020);
        public byte x0028_Level => ReadByte(0x0028);
        public byte x002C_Group => ReadByte(0x002C);
        public byte x002D_Value16 => ReadByte(0x002D);
        public byte x0038_ModuleType => ReadByte(0x0038);
        public int x0040_Value17 => ReadInt16(0x0040);
        public int x0042_Value18 => ReadInt16(0x0042);
        public byte x0048_Value2 => ReadByte(0x0048);
        public byte x0049_Value19 => ReadByte(0x0049);
        public int x00B0_Value20 => ReadInt16(0x00B0);
        public int x00B2_Value21 => ReadInt16(0x00B2);
        public byte x00C0_Value3 => ReadByte(0x00C0);
        public bool x00C8_IsDebuff => ReadByte(0x00C8) != 0;
        public bool x00CB_IsOverlappable => ReadByte(0x00CB) != 0;
        public byte x00E7_Value22 => ReadByte(0x00E7);
        public byte x00E8_Value4 => ReadByte(0x00E8);
        public byte x00E9_Value23 => ReadByte(0x00E9);
        public byte x00EA_Value24 => ReadByte(0x00EA);
        public byte x00EE_Value25 => ReadByte(0x00EE);
        public byte x00F0_Value5 => ReadByte(0x00F0);
        public byte x00F8_Value26 => ReadByte(0x00F8);
        public byte x0100_Value6 => ReadByte(0x0100);
        public byte x0108_Value7 => ReadByte(0x0108);
        public string x0110_BuffIcon => ReadStringUnicode(ReadPointer8b(0x0110));
        public byte x0120_Value8 => ReadByte(0x0120);
        public byte x0128_Value9 => ReadByte(0x0128);
        public byte x0130_Value10 => ReadByte(0x0130);
        public string x0138_Description => ReadStringUnicode(ReadPointer8b(0x0138));
        public byte x0148_Value11 => ReadByte(0x0148);
        public byte x0150_Value12 => ReadByte(0x0150);
        public int x0158_Value27 => ReadInt16(0x0158);
        public int x015A_Value28 => ReadInt16(0x015A);
        public int x015C_Value29 => ReadInt16(0x015C);
        public int x015E_Value30 => ReadInt16(0x015E);
    }
}
