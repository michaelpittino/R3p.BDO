using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.MainWindow
{
    public class MainWindow : MemoryObject
    {
        public MainWindow()
        {
            Address = ReadPointer8b(Offsets._mainWindowBase);

            PatchCheckFocus();
        }

        public bool hasFocus => ReadByte(0x1478) != 0;

        public void setFocus()
        {
            Write(0x1478, BitConverter.GetBytes(1));
        }

        public void PatchCheckFocus()
        {
            if(ReadByte(Offsets._checkWindowFocus + 0x33) != 0xC6)
                Write(Offsets._checkWindowFocus + 0x33, new byte[]{0xC6, 0x80, 0x78, 0x14, 0x00, 0x00, 0x01, 0x90, 0x90, 0x90});
        }
    }
}
