using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.DeezNutz
{
    public class DeezNutz : MemoryObject
    {
        public DeezNutz()
        {
            Address = Offsets._stringTable;
        }

        public DNEntry GetEntry(int index)
        {
            return new DNEntry(ReadPointer8b(ReadPointer8b(Address)) + (0x10 * index));
        }

        public NameEntry GetNameEntry(int index)
        {
            return new NameEntry(ReadPointer8b(ReadPointer8b(Address - 0x30)) + (0x8 * (index * 2)));
        }
    }
}
