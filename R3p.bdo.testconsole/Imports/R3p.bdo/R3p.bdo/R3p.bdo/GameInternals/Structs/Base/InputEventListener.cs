using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Base
{
    public class InputEventListener : MemoryObject
    {
        public InputEventListener()
        {
            Address = ReadPointer8b(ReadPointer8b(Offsets._base) + 0x8);
            Patch_ForegroundCheck();
        }

        private long ForegroundCheck => Offsets._getKeyInputState_Pressed + 0x24;
        private long ListStart_KeyPressed => Address + 0x840;

        private void Patch_ForegroundCheck()
        {
            if(ReadByte(ForegroundCheck) != 0xEB && ReadByte(ForegroundCheck + 0x1) != 0x40)
                InsertJMP(ForegroundCheck ,0x40, 0x7);
        }

        private void InsertJMP(long address, byte jmpOffset, int size)
        {
            byte[] nInstruction = new byte[size];
            
            nInstruction[0] = 0xEB;
            nInstruction[1] = jmpOffset;
            
            for (int i = 2; i < size; i++)
            {
                nInstruction[i] = 0x90;
            }

            Write(address, nInstruction);
        }

        public bool GetKeyState_IsPressed(VirtualKeyCode keyCode)
        {
            return ReadByte(ListStart_KeyPressed + ((byte) keyCode*4)) != 0;
        }

        public void ChangeKeyState_KeyDown(VirtualKeyCode keyCode)
        {
            //Collection.MainWindow.Base.Handle.setFocus();
            Write(ListStart_KeyPressed + ((byte)keyCode * 4), BitConverter.GetBytes((byte)1));
        }

        public void ChangeKeyState_KeyUp(VirtualKeyCode keyCode)
        {
            //Collection.MainWindow.Base.Handle.setFocus();
            Write(ListStart_KeyPressed + ((byte)keyCode * 4), BitConverter.GetBytes((byte)0));
        }

        public void ChangeKeyState_PressOnce(VirtualKeyCode keyCode, int sleep = 25)
        {
            ChangeKeyState_KeyDown(keyCode);
            Thread.Sleep(sleep);
            ChangeKeyState_KeyUp(keyCode);
        }
    }
}
