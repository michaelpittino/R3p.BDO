using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Base
{
    public class GameState : MemoryObject
    {
        public GameState()
        {
            Address = ReadPointer8b(Offsets._base);
        }

        public bool isLoading => ReadByte(0x90) != 0;//
        public bool isLoaded => ReadByte(0x1F0) == 13 || ReadByte(0x1F0) == 17;
        public bool isChangingChannel => ReadByte(0x91) != 0;//
        public int selectedCharacterNo => ReadInt32(0x118);
    }
}
