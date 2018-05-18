using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace R3p.bdo.Memory
{
    public class MemoryObject
    {
        public long Address { get; set; }

        public void Write(int offset,byte[] data)
        {
            MemoryWriter.Write(Address + offset, data);
        }

        public void Write(long address, byte[] data)
        {
            MemoryWriter.Write(address, data);
        }

        public byte ReadByte(int offset)
        {
            return MemoryReader.ReadByte(Address + offset);
        }

        public byte ReadByte(long address)
        {
            return MemoryReader.ReadByte(address);
        }

        public float[,] ReadMatrix(int offset)
        {
            return MemoryReader.ReadMatrix(Address + offset);
        }

        public float[,] ReadMatrix(long address)
        {
            return MemoryReader.ReadMatrix(address);
        }

        public byte[] ReadByteArray(int offset, int Size)
        {
            return MemoryReader.ReadByteArray(Address + offset, Size);
        }

        public byte[] ReadByteArray(long address, int Size)
        {
            return MemoryReader.ReadByteArray(address, Size);
        }

        public UInt16 ReadInt16(int offset)
        {
            return MemoryReader.ReadInt16(Address + offset);
        }

        public UInt16 ReadInt16(long address)
        {
            return MemoryReader.ReadInt16(address);
        }

        public  int ReadInt32(int offset)
        {
            return MemoryReader.ReadInt32(Address + offset);
        }

        public int ReadInt32(long address)
        {
            return MemoryReader.ReadInt32(address);
        }

        public Int64 ReadInt64(int offset)
        {
            return MemoryReader.ReadInt64(Address + offset);
        }

        public Int64 ReadInt64(long address)
        {
            return MemoryReader.ReadInt64(address);
        }

        public Double ReadDouble(int offset)
        {
            return MemoryReader.ReadDouble(Address + offset);
        }

        public Double ReadDouble(long address)
        {
            return MemoryReader.ReadDouble(address);
        }

        public float ReadFloat(int offset)
        {
            return MemoryReader.ReadFloat(Address + offset);
        }

        public float ReadFloat(long address)
        {
            return MemoryReader.ReadFloat(address);
        }

        public long ReadPointer8b(int offset)
        {
            return MemoryReader.ReadPointer8b(Address + offset);
        }

        public long ReadPointer8b(long address)
        {
            return MemoryReader.ReadPointer8b(address);
        }
        
        public float[] ReadVec3(int offset)
        {
            return MemoryReader.ReadVec3(Address + offset);
        }

        public float[] ReadVec3(long address)
        {
            return MemoryReader.ReadVec3(address);
        }

        public float[] ReadVec2(int offset)
        {
            return MemoryReader.ReadVec2(Address + offset);
        }

        public float[] ReadVec2(long address)
        {
            return MemoryReader.ReadVec2(address);
        }

        public string ReadStringUnicode(int offset)
        {
            return MemoryReader.ReadStringUnicode(Address + offset);
        }

        public string ReadStringUnicode(long address)
        {
            return MemoryReader.ReadStringUnicode(address);
        }

        public string ReadStringASCII(int offset)
        {
            return MemoryReader.ReadStringASCII(Address + offset);
        }

        public string ReadStringASCII(long address)
        {
            return MemoryReader.ReadStringASCII(address);
        }
    }
}
