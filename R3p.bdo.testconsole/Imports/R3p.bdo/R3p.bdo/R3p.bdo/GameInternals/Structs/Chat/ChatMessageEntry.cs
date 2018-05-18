using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Chat
{
    public class ChatMessageEntry : MemoryObject
    {
        public ChatMessageEntry(long address)
        {
            Address = address;
        }

        public ChatType x0000_ChatType
        {
            get
            {
                int type = ReadByte(0x0000);
                if (Enum.IsDefined(typeof(ChatType), type))
                    return (ChatType)type;
                return default(ChatType);
            }
        }

        public ChatSystemType x0001_ChatSystemType
        {
            get
            {
                int type = ReadByte(0x0001);
                if (Enum.IsDefined(typeof(ChatSystemType), type))
                    return (ChatSystemType)type;
                return default(ChatSystemType);
            }
        }

        public EChatNoticeType x0003_ChatNoticeType
        {
            get
            {
                int type = ReadByte(0x0003);
                if (Enum.IsDefined(typeof(EChatNoticeType), type))
                    return (EChatNoticeType)type;
                return default(EChatNoticeType);
            }
        }

        public long x0008_Time => ReadInt64(0x008);
        public string x0010_Sender => ReadStringUnicode(0x0010);
        public string x0030_Text => ReadStringUnicode(ReadPointer8b(0x0030));
        public bool x0050_IsMe => ReadByte(0x0050) != 0;
        public int x0058_LinkedItemIndex => ReadInt32(0x0058);
        public int x008C_LinkedItemStartIndex => ReadInt32(0x008C);
        public int x0090_LinkedItemEndIndex => ReadInt32(0x0090);
        public bool x0098_IsSameChannel => ReadByte(0x0098) != 0;
        public bool x0099_IsGuildMaster => ReadByte(0x0099) != 0;
        public bool x009A_IsGameMaster => ReadByte(0x009A) != 0;
        public int MessageId => ReadInt32(0x00AC);

        public bool isLinkedItem => x0058_LinkedItemIndex != 0;

        private long _tBuffer;
        private long GetTime()
        {
            if (_tBuffer == 0)
            {
                _tBuffer = ReadInt64(0x008);
            }

            return _tBuffer;
        }

        private string _sBuffer;
        private string GetSender()
        {
            if (_sBuffer == null)
            {
                _sBuffer = ReadStringUnicode(0x0010);
            }

            return _sBuffer;
        }

        private string _txBuffer;
        private string GetText()
        {
            if (_txBuffer == null)
            {
                _txBuffer = ReadStringUnicode(ReadPointer8b(0x0030));
            }

            return _txBuffer;
        }

        private int _idBuffer;
        private int GetID()
        {
            if (_idBuffer == 0)
            {
                _idBuffer = ReadInt32(0x00AC);
            }

            return _idBuffer;
        }
    }
}
