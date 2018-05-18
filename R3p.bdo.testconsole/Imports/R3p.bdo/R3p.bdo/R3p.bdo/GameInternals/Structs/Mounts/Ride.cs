using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Mounts
{
    public class Ride : MemoryObject
    {
        private int defaultAccel = 100;
        private int defaultSpeed = 100;
        private int defaultTurn = 100;
        private int defaultBrake = 100;

        public Ride()
        {
            Address = ReadPointer8b(Offsets.CurrentMount);
        }

        public ServantType ServantType => (ServantType) ReadByte(Offsets.CurrentMount + 0x14);

        public int Acceleration => ReadInt32(0x21B0);
        public int Speed => ReadInt32(0x21B4);
        public int Turn => ReadInt32(0x21B8);
        public int Brake => ReadInt32(0x21BC);

        public MountInventory Inventory => new MountInventory(ReadPointer8b(0xB4));

        public void SetDefaults()
        {
            SetAccel(defaultAccel);
            SetSpeed(defaultSpeed);
            SetTurn(defaultTurn);
            SetBrake(defaultBrake);
        }

        public void SetAccel(int value)
        {
            Write(0x21B0, BitConverter.GetBytes(value * 10000));
        }

        public void SetSpeed(int value)
        {
            Write(0x21B4, BitConverter.GetBytes(value * 10000));
        }

        public void SetTurn(int value)
        {
            Write(0x21B8, BitConverter.GetBytes(value * 10000));
        }

        public void SetBrake(int value)
        {
            Write(0x21BC, BitConverter.GetBytes(value * 10000));
        }
    }
}
