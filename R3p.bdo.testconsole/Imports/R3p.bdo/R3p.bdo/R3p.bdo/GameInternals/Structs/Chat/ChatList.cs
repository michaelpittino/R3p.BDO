using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Chat
{
    public class ChatList : MemoryObject
    {
        public ChatList()
        {
            //Address = Offsets.ChatList;
        }

        private long ListStart => Address;
        public List<ChatMessageEntry[]> FilterList => GetChatList();

        private List<ChatMessageEntry[]> _clBuffer;
        private List<ChatMessageEntry[]> GetChatList()
        {
            if(_clBuffer != null)
                return _clBuffer;

            List<ChatMessageEntry[]> list = new List<ChatMessageEntry[]>();

            List<ChatMessageEntry> buffer = new List<ChatMessageEntry>();

            for (long i = ListStart + 0x08; i < ListStart + 0x10A50; i += 0x08)
            {
                if (ReadInt64(i) < 1000)
                {
                    list.Add(buffer.ToArray());
                    buffer = new List<ChatMessageEntry>();
                    continue;
                }

                buffer.Add(new ChatMessageEntry(ReadPointer8b(i)));
            }

            if (_clBuffer == null)
                _clBuffer = list;

            return list;
        }
    }
}
