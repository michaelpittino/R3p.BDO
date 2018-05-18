using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.UI
{
    public class UIBase : MemoryObject
    {
        private class ListObject : MemoryObject
        {
            public ListObject(long address)
            {
                Address = address;
            }

            public ListObject next => new ListObject(ReadPointer8b(0x00));
            public ListObject previous => new ListObject(ReadPointer8b(0x08));
            public UIControl panel => new UIControl(ReadPointer8b(0x10));
        }

        public UIBase()
        {
            Address = ReadPointer8b(Offsets._uIBase);
        }

        public long LuaState => ReadPointer8b(ReadPointer8b((0x160)));

        public UIControl x0188_HoveredUIControl => new UIControl(ReadPointer8b(0x190));
        //public UIControl x0190_MouseClickedUIControl => new UIControl(ReadPointer8b(0x0198));
        //public UIControl x01A0_HoveredUIControl => new UIControl(ReadPointer8b(0x01A8));
        
        public bool x17A9_isCursorActive => ReadByte(0x1A50) != 0;

        public void SetCursorInactive()
        {
            if(x17A9_isCursorActive)
                Write(0x1A50, BitConverter.GetBytes((byte)0));
        }

        public void SetCursorActive()
        {
            Write(0x1A50, BitConverter.GetBytes((byte)1));
        }

        public List<UIControl> AllPanels => GetPanels();

        private List<UIControl> _pBuffer;
        private List<UIControl> GetPanels()
        {
            if(_pBuffer != null)
                return _pBuffer;

            List<UIControl> list = new List<UIControl>();

            ListObject firstNode = new ListObject(ReadPointer8b(ReadPointer8b(0x2E8)));

            ListObject curNode = firstNode;

            for (int i = 0; i < 2000; i++)
            {
                ListObject nextNode = curNode.next;

                if (nextNode.previous.Address == curNode.Address && nextNode.Address != firstNode.Address)
                {
                    list.Add(curNode.panel);

                    curNode = nextNode;
                }
                else
                {
                    break;
                }
            }

            if (_pBuffer == null)
                _pBuffer = list;

            return list;
        }
    }
}
