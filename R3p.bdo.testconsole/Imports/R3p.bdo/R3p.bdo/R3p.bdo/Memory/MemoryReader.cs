using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3p.bdo.Memory
{
    public class MemoryReader
    {
        public static byte ReadByte(long address)
        {
            byte[] buffer = new byte[sizeof(byte)];
            int bytesRead = 0;

            Win32.ReadProcessMemory((int)Engine.Instance.hProcess, address, buffer, buffer.Length, ref bytesRead);

            return buffer[0];
        }

        public static float[,] ReadMatrix(long address)
        {
            float[,] matrix = new float[4, 4];

            byte[] buffer = new byte[16 * sizeof(float)];
            int bytesRead = 0;
            Win32.ReadProcessMemory((int)Engine.Instance.hProcess, address, buffer, buffer.Length, ref bytesRead);

            matrix[0, 0] = BitConverter.ToSingle(buffer, 0);
            matrix[1, 0] = BitConverter.ToSingle(buffer, 4);
            matrix[2, 0] = BitConverter.ToSingle(buffer, 8);
            matrix[3, 0] = BitConverter.ToSingle(buffer, 12);

            matrix[0, 1] = BitConverter.ToSingle(buffer, 16);
            matrix[1, 1] = BitConverter.ToSingle(buffer, 20);
            matrix[2, 1] = BitConverter.ToSingle(buffer, 24);
            matrix[3, 1] = BitConverter.ToSingle(buffer, 28);

            matrix[0, 2] = BitConverter.ToSingle(buffer, 32);
            matrix[1, 2] = BitConverter.ToSingle(buffer, 36);
            matrix[2, 2] = BitConverter.ToSingle(buffer, 40);
            matrix[3, 2] = BitConverter.ToSingle(buffer, 44);

            matrix[0, 3] = BitConverter.ToSingle(buffer, 48);
            matrix[1, 3] = BitConverter.ToSingle(buffer, 52);
            matrix[2, 3] = BitConverter.ToSingle(buffer, 56);
            matrix[3, 3] = BitConverter.ToSingle(buffer, 60);

            return matrix;
        }

        public static byte[] ReadByteArray(long address, int Size)
        {
            byte[] buffer = new byte[Size];
            int bytesRead = 0;

            Win32.ReadProcessMemory((int)Engine.Instance.hProcess, address, buffer, buffer.Length, ref bytesRead);

            return buffer;
        }

        public static UInt16 ReadInt16(long address)
        {
            byte[] buffer = new byte[sizeof(Int16)];
            int bytesRead = 0;

            Win32.ReadProcessMemory((int)Engine.Instance.hProcess, address, buffer, buffer.Length, ref bytesRead);

            return BitConverter.ToUInt16(buffer, 0);
        }

        public static int ReadInt32(long address)
        {
            byte[] buffer = new byte[sizeof(Int32)];
            int bytesRead = 0;

            Win32.ReadProcessMemory((int)Engine.Instance.hProcess, address, buffer, buffer.Length, ref bytesRead);

            return BitConverter.ToInt32(buffer, 0);
        }

        public static Int64 ReadInt64(long address)
        {
            byte[] buffer = new byte[sizeof(Int64)];
            int bytesRead = 0;

            Win32.ReadProcessMemory((int)Engine.Instance.hProcess, address, buffer, buffer.Length, ref bytesRead);

            return BitConverter.ToInt64(buffer, 0);
        }

        public static Double ReadDouble(long address)
        {
            byte[] buffer = new byte[sizeof(Double)];
            int bytesRead = 0;

            Win32.ReadProcessMemory((int)Engine.Instance.hProcess, address, buffer, buffer.Length, ref bytesRead);

            return BitConverter.ToDouble(buffer, 0);
        }

        public static float ReadFloat(long address)
        {
            byte[] buffer = new byte[sizeof(float)];
            int bytesRead = 0;

            Win32.ReadProcessMemory((int)Engine.Instance.hProcess, address, buffer, buffer.Length, ref bytesRead);

            return BitConverter.ToSingle(buffer, 0);
        }

        public static long ReadPointer8b(long address)
        {
            byte[] buffer = new byte[sizeof(long)];
            int bytesRead = 0;

            Win32.ReadProcessMemory((int)Engine.Instance.hProcess, address, buffer, buffer.Length, ref bytesRead);

            return BitConverter.ToInt64(buffer, 0);
        }
        
        public static float[] ReadVec3(long address)
        {
            byte[] buffer = new byte[sizeof(float) * 3];
            int bytesRead = 0;

            Win32.ReadProcessMemory((int)Engine.Instance.hProcess, address, buffer, buffer.Length, ref bytesRead);

            float[] vec = new float[3];
            vec[0] = BitConverter.ToSingle(buffer, 0);
            vec[1] = BitConverter.ToSingle(buffer, 4);
            vec[2] = BitConverter.ToSingle(buffer, 8);

            return vec;
        }

        public static float[] ReadVec2(long address)
        {
            byte[] buffer = new byte[sizeof(float) * 2];
            int bytesRead = 0;

            Win32.ReadProcessMemory((int)Engine.Instance.hProcess, address, buffer, buffer.Length, ref bytesRead);

            float[] vec = new float[2];
            vec[0] = BitConverter.ToSingle(buffer, 0);
            vec[1] = BitConverter.ToSingle(buffer, 4);

            return vec;
        }

        public static string ReadStringUnicode(long address)
        {
            string rtn = "";

            byte[] buffer = new byte[512];
            int bytesRead = 0;

            byte[] b = new byte[1];

            int count = 0;
            int countB = 0;

            for (int i = 0; i < 512; i++)
            {
                Win32.ReadProcessMemory((int)Engine.Instance.hProcess, address + i, b, b.Length, ref bytesRead);

                buffer[i] = b[0];

                if (b[0] == 0)
                    count++;
                if (b[0] != 0)
                {
                    count--;
                    countB++;
                }

                if (count >= 2)
                    break;

                if (count < -1)
                    return rtn;
            }

            byte[] bufferB = new byte[countB * 2];
            Array.Copy(buffer, bufferB, countB * 2);

            rtn = Encoding.Unicode.GetString(bufferB);

            return rtn;
        }

        public static string ReadStringASCII(long address)
        {
            byte[] buffer = new byte[512];
            int bytesRead = 0;

            Win32.ReadProcessMemory((int)Engine.Instance.hProcess, address, buffer, buffer.Length, ref bytesRead);

            int counter = 0;

            for (int i = 0; i < buffer.Length; i++)
            {
                if (buffer[i] == 0)
                    break;

                counter++;
            }

            byte[] a = new byte[counter];
            Array.Copy(buffer, 0, a, 0, counter);

            string output = Encoding.ASCII.GetString(a);

            return output;
        }
    }
}
