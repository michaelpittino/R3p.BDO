using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.DeezNutz
{
    public class NameEntry : MemoryObject
    {
        public NameEntry(long address)
        {
            Address = address;
        }

        public string Text => GetText();
        //public long TextLength => ReadInt64(0x18);
        //public long TextType => ReadInt64(0x20);

        private string GetText()
        {
            //string text = "";

            //if (TextType == 15)
            //    return ReadStringASCII(0x8);
            //else if (TextType == 31)
                return ReadStringUnicode(ReadPointer8b(0x0));

            //return text;
        }
    }
}
