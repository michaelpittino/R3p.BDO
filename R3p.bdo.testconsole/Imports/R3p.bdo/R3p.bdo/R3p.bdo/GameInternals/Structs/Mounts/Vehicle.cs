using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Mounts
{
    public class Vehicle : MemoryObject
    {
        public static int oAccel = 0x22C8;//-
        public static int oSpeed = oAccel + 4;//-
        public static int oTurn = oSpeed + 4;//-
        public static int oBrake = oTurn + 4;//-

        public Vehicle()
        {

            Address = Offsets._currentVehicle;
        }

        public ActorData ActorData => new ActorData(ReadPointer8b(0x30));

        public ServantType ServantType => ActorData.ServantType;

        public int Acceleration => ActorData.ReadInt32(oAccel);
        public int Speed => ActorData.ReadInt32(oSpeed);
        public int Turn => ActorData.ReadInt32(oTurn);
        public int Brake => ActorData.ReadInt32(oBrake);

        public MountInventory Inventory => new MountInventory(ReadPointer8b(0xB4));

        public void SetSpeeds(int accel, int speed, int turn, int brake)
        {
            Pipe.Call.SetVehicleSpeeds(ActorData.ActorId, accel * 10000, speed * 10000, turn * 10000, brake * 10000);
        }

        public void SetAccel(int value)
        {
            ActorData.Write(oAccel, BitConverter.GetBytes(value * 10000));
        }

        public void SetSpeed(int value)
        {
            ActorData.Write(oSpeed, BitConverter.GetBytes(value * 10000));
        }

        public void SetTurn(int value)
        {
            ActorData.Write(oTurn, BitConverter.GetBytes(value * 10000));
        }

        public void SetBrake(int value)
        {
            ActorData.Write(oBrake, BitConverter.GetBytes(value * 10000));
        }
    }
}
