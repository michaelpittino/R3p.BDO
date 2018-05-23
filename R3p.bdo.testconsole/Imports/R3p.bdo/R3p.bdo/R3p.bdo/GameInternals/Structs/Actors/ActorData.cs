using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using R3p.bdo.GameInternals.Enums;
using R3p.bdo.GameInternals.Structs.Actors;
using R3p.bdo.GameInternals.Structs.Region;
using R3p.bdo.GameInternals.Structs.Skills;
using R3p.bdo.Memory;

namespace R3p.bdo
{
    public class ActorData : MemoryObject
    {
        public static ActorData GetLocalPlayerData()
        {
            return new LocalPlayer().PlayerData;
        }

        public class Offsets
        {
            public static int OVTable = 0x0;//-
            public static int OInteractionFrag = 0x20;//-
            public static int OActorKey = 0x58;//-
            public static int OActorType = 0x5C;//-
            public static int OCharacterName = 0x68;//-
            public static int OCurrentHp = 0x98;//-
            public static int OMaxHp = 0xB8;//-
            public static int OMaxMp = 0xE4;//-
            public static int OWorldPosition = 0x110;//-
            public static int OCurrentRegion = 0x120;//-
            public static int[] OCharacterClass = {0x128, 0x198, 1};//-
            public static int[] ONonPlayerLevel = {0x128, 0x2F4};//-
            public static int[] OServantType = { 0x128, 0x32 };
            public static int[] OServantKind = { 0x128, 0x8 };
            public static int OActorId = 0x130;//-
            public static int OActiveBuffCount = 0x168;//-
            public static int OActiveBuffList = 0x170;//-
            public static int OIsDead = 0x198;//-
            public static int OWorldPositionCollectables = 0x360;//-
            public static int OCharacterController = 0x380;//-
            public static int OLootIdentifier = 0x3C8;//-
            public static int OIsCombatStance = 0x3EC;//-
            public static int OEquipmentStart = 0x4A0;//-
            public static int ODialogIndex = 0xBE0;
            public static int OMovementSpeed = 0xC00;//-
            public static int OAttackSpeed = OMovementSpeed + 4;//-
            public static int OCastSpeed = OAttackSpeed + 4;//-
            public static int OMovementSpeed2 = OCastSpeed + 16;//-
            public static int OAttackSpeed2 = OMovementSpeed2 + 4;//-
            public static int OCastSpeed2 = OAttackSpeed2 + 4;//-
            public static int OActionIndex = 0xC2C;
            public static int OIsAggro = 0xEF2;//-
            public static int OMaxWeight = 0x1068;//---
            public static int OCurExp = 0x1010;
            public static int OPrevExp = 0x1030;
            public static int ONeedExp = 0x1038;
            public static int OIsPossibleManufactureAtWarehouse = 0x10B8;//-
            public static int OIsCharacterNameTagVisible = 0x10BC;//-
            public static int OIsGuildNameTagVisible = 0x10BE;//-
            public static int OGuildName = 0x10CA;//-
            public static int OLevel = 0x1120;//-
            public static int OFamilyName = 0x12A8;//-
            public static int OSkillListFirstNode = 0x15C8;//-
            public static int OSkillListCount = OSkillListFirstNode + 0x8;//-
            public static int OLastItemUseStartTick = 0x1640;//-
            public static int OCurrentMp = 0x1B28;//-
            public static int OCurrentStamina = 0x19D1;//
            public static int OIsReadyToPlay = 0x1CAD;//-
            public static int OWeightItemsAndSilver = 0x1CB0;//-
            public static int OInventory = 0x1CB8;//-
            public static int OFreeInventorySlots = 0x1CD0;//-
            public static int OMaxInventorySlots = 0x1CD1;//-
            public static int OWeightEquippedItems = 0x1DB0;//-
            public static int OMainHand = 0x1DB8;//-
            public static int OFishGrade = 0x4110;//-
            public static int ONextFishBite = OFishGrade - 16;//-
            public static int ONextAutoFishAutoCatch = OFishGrade - 8;//-
            public static int OIsFishing = OFishGrade + 4;//-
            public static int OFishHooked = OFishGrade + 21;//-
            public static int OLootingState = 0x40BC;//
            public static int OCurMovementSpeed = 0x4760;//-
            public static int OCurAttackSpeed = OCurMovementSpeed + 4;//-
            public static int OCurCastSpeed = OCurMovementSpeed + 8;//-
            public static int OMaxMovementSpeed = OCurMovementSpeed + 12;//-
            public static int OMaxAttackSpeed = OCurMovementSpeed + 16;//-
            public static int OMaxCastSpeed = OCurMovementSpeed + 20;//-
            public static int OCurCrit = OCurMovementSpeed + 24;//-
            public static int OMaxCrit = OCurMovementSpeed + 28;//-
            public static int OCurLuck = OCurMovementSpeed + 32;//-
            public static int OMaxLuck = OCurMovementSpeed + 36;//-
            public static int OCurFishing = OCurMovementSpeed + 40;//-
            public static int OMaxFishing = OCurMovementSpeed + 44;//-
            public static int OCurGathering = OCurMovementSpeed + 48;//-
            public static int OMaxGathering = OCurMovementSpeed + 52;//-
            public static int OHackedGathering = OMaxGathering + 0x4;
        }

        private BuffList BuffList { get; set; }

        public ActorData(long address)
        {
            Address = address;
            BuffList = new BuffList(Address + Offsets.OActiveBuffList);
        }

        
        public VTable VTable => new VTable(ReadPointer8b(Offsets.OVTable));

        public bool hasLoot => false;//VTable.ReadPointer8b(0x1B0) == bdo.Offsets._lootIdentifier && ReadByte(0x3C8) == 0;
        
        public bool isReadyToPlay => ReadByte(Offsets.OIsReadyToPlay) != 0;

        public int DialogIndex => ReadInt16(Offsets.ODialogIndex);
        public int ActionIndex => ReadInt16(Offsets.OActionIndex);

        public ServantType ServantType => (ServantType)ReadByte(ReadPointer8b(Offsets.OServantType[0]) + Offsets.OServantType[1]);
        public ServantKind ServantKind => (ServantKind)ReadByte(ReadPointer8b(Offsets.OServantKind[0]) + Offsets.OServantKind[1]);
        public int ActorKey => ReadInt32(Offsets.OActorKey);        //                          
        public ActorType ActorType => (ActorType) ReadByte(Offsets.OActorType);   //
        public ClassType CharacterClass => (ClassType) ReadByte(ReadPointer8b(ReadPointer8b(Offsets.OCharacterClass[0]) + Offsets.OCharacterClass[1]) + Offsets.OCharacterClass[2]);   //
        public CombatResouceType ResourceType => GetResourceType(); //
        public string Name => GetCharacterName();   //
        public float[] WorldPosition => GetWorldPosition(); //
        public double CurrentWeight => GetCurrentWeight();  //
        public double MaxWeight => GetMaxWeight();  //
        public double MaxMp => ReadInt32(Offsets.OMaxMp); //
        public double CurMp => GetCurMp();  //
        public double CurStamina => GetCurStamina();
        public bool isCombatStance => ReadByte(Offsets.OIsCombatStance) != 0;
        public bool isDead => ReadByte(Offsets.OIsDead) != 0;
        public RegionData CurrentRegion => new RegionData(ReadPointer8b(Offsets.OCurrentRegion));    //
        public int ActorId => ReadInt16(Offsets.OActorId); //
        public long ActiveBuffCount => ReadInt64(Offsets.OActiveBuffCount);    //
        public Dictionary<int, BuffData> ActiveBuffList => BuffList.List;   //

        public CharacterControl CharacterControl => getCharacterControl();  //

        public int CurrentGatheringLevel => ReadInt32(Offsets.OCurGathering);

        public void SetCurrentGatheringLevel(int value)
        {
            Write(Offsets.OCurGathering, BitConverter.GetBytes(value));
        }

        public int GetMaxGatheringLevel => ReadInt32(Offsets.OMaxGathering);

        public void SetMaxGatheringLevel(int value)
        {
            Write(Offsets.OMaxGathering, BitConverter.GetBytes(value));
        }

        public bool IsHackedGathering => ReadInt32(Offsets.OHackedGathering) != 0;

        public void SetIsHackedGathering()
        {
            Write(Offsets.OHackedGathering, BitConverter.GetBytes((int)1));
        }

        private CharacterControl _cCBuffer;
        private CharacterControl getCharacterControl()
        {
            if (_cCBuffer == null)
                _cCBuffer = new CharacterControl(ReadPointer8b(Offsets.OCharacterController));

            return _cCBuffer;
        }
        
        private long Equipment_Start => Address + Offsets.OEquipmentStart;    //
        public int MovementSpeed => ReadInt32(Offsets.OMovementSpeed) / 10000;   //
        public int AttackSpeed => ReadInt32(Offsets.OAttackSpeed) / 10000; //
        public int CastSpeed => ReadInt32(Offsets.OCastSpeed) / 10000;   //

        public int MovementSpeed2 => ReadInt32(Offsets.OMovementSpeed2) / 10000;   //
        public int AttackSpeed2 => ReadInt32(Offsets.OAttackSpeed2) / 10000; //
        public int CastSpeed2 => ReadInt32(Offsets.OCastSpeed2) / 10000;   //

        public void setSpeeds(int mvmt, int atk, int cst, bool advMode)
        {
            mvmt *= 10000;
            atk *= 10000;
            cst *= 10000;

            int curMvmt = MovementSpeed * 10000;
            int curAtk = AttackSpeed * 10000;
            int curCst = CastSpeed * 10000;

            if (curMvmt != mvmt)
            {
                if (advMode)
                {
                    Pipe.Call.SetPlayerSpeeds(mvmt, curAtk, curCst);
                }
                else
                {
                    Pipe.Call.SetPlayerSpeeds(mvmt, atk, cst);
                }
            }
        }

        public void setMovementSpeed(int value)
        {
            if (MovementSpeed != value)
                Write(Offsets.OMovementSpeed, BitConverter.GetBytes(value * 10000));

            //if (MovementSpeed2 != value)
            //    Write(Offsets.OMovementSpeed2, BitConverter.GetBytes(value * 10000));
        }

        public void setAttackSpeed(int value)
        {
            if (AttackSpeed != value)
                Write(Offsets.OAttackSpeed, BitConverter.GetBytes(value * 10000));

            //if (AttackSpeed2 != value)
            //    Write(Offsets.OAttackSpeed2, BitConverter.GetBytes(value * 10000));
        }

        public void setCastSpeed(int value)
        {
            if (CastSpeed != value)
                Write(Offsets.OCastSpeed, BitConverter.GetBytes(value * 10000));

            //if (CastSpeed2 != value)
            //    Write(Offsets.OCastSpeed2, BitConverter.GetBytes(value * 10000));
        }

        public bool hasAggro()
        {
            return ReadByte(Offsets.OIsAggro) != 0;
        }

        public void setAggro()
        {
            Write(Offsets.OIsAggro, BitConverter.GetBytes((byte)1));
        }
        
        public bool isPossibleManufactureAtWarehouse => ReadByte(Offsets.OIsPossibleManufactureAtWarehouse) != 0;  //
        public bool IsCharacterNameTagVisible => ReadByte(Offsets.OIsCharacterNameTagVisible) != 1; //
        public bool IsGuildNameTagVisible => ReadByte(Offsets.OIsGuildNameTagVisible) != 1; //
        public string GuildName => GetGuildName();  //
        public int Level => GetLevel(); //
        public string FamilyName => GetFamilyName();    //
        
        public long lastItemUseStartTick => ReadInt64(ReadPointer8b(Offsets.OLastItemUseStartTick));   //
        
        public long getLastItemUseEndTick(int slot)
        {
            return ReadInt64(ReadPointer8b(Offsets.OLastItemUseStartTick) + (0x8 * slot));  //
        }
        
        public PlayerInventory Inventory => new PlayerInventory(ReadPointer8b(Offsets.OInventory)); //
        public byte x19B8_FreeInventorySlots => ReadByte(Offsets.OFreeInventorySlots);   //
        public byte x19B9_MaxInventorySlots => ReadByte(Offsets.OMaxInventorySlots);    //
        public long NextFishBite => ReadInt64(Offsets.ONextFishBite);  //
        public long NextAutoFishAutoCatch => ReadInt64(Offsets.ONextAutoFishAutoCatch); //
        public ItemGrade FishGrade => (ItemGrade) ReadInt16(Offsets.OFishGrade);    //
        public bool isFishing => ReadByte(Offsets.OIsFishing) != 0; //

        public bool isFishHooked => ReadByte(Offsets.OFishHooked) != 0;
        
        public bool isLooting => ReadByte(Offsets.OLootingState) != 0;

        public void setNotLooting()
        {
            Write(Offsets.OLootingState, BitConverter.GetBytes(0));
        }
        
        public double Distance => GetDistance();    //
        public double MaxHitpoints => GetMaxHp();   //
        public double CurHitpoints => GetCurHp();   //
        public double PercentageCurHitpoints => (CurHitpoints / MaxHitpoints) * 100;    //
        public double PercentageCurResource => (CurMp/MaxMp)*100;   //
        
        public bool IsOnScreen(bool WorldMap, out int[] ScreenPosition)
        {
            return Helpers.ViewTransform.ToScreen.WorldToScreen(WorldPosition, out ScreenPosition, WorldMap);
        }

        private string _cNameBuffer;
        private string GetCharacterName()
        {
            if(_cNameBuffer == null)
                _cNameBuffer = ReadStringUnicode(ReadPointer8b(Offsets.OCharacterName));

            return _cNameBuffer;
        }

        private string _fNameBuffer;
        private string GetFamilyName()
        {
            if (_fNameBuffer == null)
                _fNameBuffer = ReadStringUnicode(Offsets.OFamilyName);

            return _fNameBuffer;
        }

        private string _gNameBuffer;
        private string GetGuildName()
        {
            if (_gNameBuffer == null)
                _gNameBuffer = ReadStringUnicode(Offsets.OGuildName);

            return _gNameBuffer;
        }

        private int GetLevel()
        {
            if(ActorType == ActorType.ActorType_Player)
                return ReadByte(Offsets.OLevel);

            return ReadByte(ReadPointer8b(Offsets.ONonPlayerLevel[0]) + Offsets.ONonPlayerLevel[1]);
        }

        private float[] GetWorldPosition()
        {
            if (ActorType == ActorType.ActorType_Collect || ActorType == ActorType.ActorType_Deadbody)
                return ReadVec3(Offsets.OWorldPositionCollectables);

            return ReadVec3(Offsets.OWorldPosition);
        }

        public void SetWorldPosition(float[] value)
        {
            byte[] buffer = new byte[12];
            Array.Copy(BitConverter.GetBytes(value[0]), 0, buffer, 0, 4);
            Array.Copy(BitConverter.GetBytes(value[1]), 0, buffer, 4, 4);
            Array.Copy(BitConverter.GetBytes(value[2]), 0, buffer, 8, 4);

            Write(Offsets.OWorldPosition, buffer);
        }
        
        public EquipmentItem GetEquipmentItem(EquipSlotNo equipSlot)
        {
            return new EquipmentItem(Equipment_Start + ((int)equipSlot * 0x30));
        }

        private double GetDistance()
        {
            var localWPos = Collection.Actors.Local.PlayerData.WorldPosition;
            var target = WorldPosition;

            float xD = target[0] - localWPos[0];
            float yD = target[1] - localWPos[1];
            float zD = target[2] - localWPos[2];

            return (float)Math.Sqrt(xD * xD + yD * yD + zD * zD);
        }

        private double GetMaxHp()
        {
            long v1; // rbx@1
            int v2; // er8@1
            uint v3; // xmm0_4@2

            v1 = Address + Offsets.OMaxHp;

            v2 = ReadInt32(v1 + 8);

            var a = ReadInt32(v1 + 0xC);
            var b = ReadPointer8b(v1 + 0x08 * a + 0x18);

            v3 = (uint)(((uint)(v2 + a) >> 5) ^ ReadInt32(b));

            var v3f = BitConverter.ToSingle(BitConverter.GetBytes(v3), 0);

            var result = Math.Floor(v3f);

            return result;
        }

        private double GetCurHp()
        {
            long v1; // rbx@1
            int v2; // er8@1
            uint v3;

            v1 = Address + Offsets.OCurrentHp;

            v2 = ReadInt32(v1 + 8);

            var a = ReadInt32(v1 + 0xC);
            var b = ReadPointer8b(v1 + 0x08*a + 0x18);

            v3 = (uint) (((uint) (v2 + a) >> 5) ^ ReadInt32(b));


            var v3f = BitConverter.ToSingle(BitConverter.GetBytes(v3), 0);

            var result = Math.Floor(v3f);

            return result;
        }

        private long GetCurMp()
        {


            long PlayerBase = Address;
                long v1; // rbx@1
                int v2; // er8@1
                long result; // rax@1

                v1 = PlayerBase + Offsets.OCurrentMp;
                //sub_1406221B8(PlayerBase + 0x1820);
                v2 = ReadInt32(v1 + 8);
                result = 0;
                if (v2 != 0)
                    result = ReadInt32(ReadPointer8b(v1 + 8 * ReadInt32(v1 + 0xC) + 0x18)) ^ ((uint)(v2 + ReadInt32(v1 + 0xC)) >> 5);
                return result;
        }

        private long GetCurStamina()
        {


            long a1 = Address + Offsets.OCurrentStamina;

            long v1; // rbx@1
            int v2; // er8@1
            long result; // rax@1

            v1 = a1;
            //sub_1406221B8(PlayerBase + 0x1820);
            v2 = ReadInt32(v1 + 8);
            result = 0;
            if (v2 != 0)
                result = ReadInt32(ReadPointer8b(v1 + 8 * ReadInt32(v1 + 0xC) + 0x18)) ^ ((uint)(v2 + ReadInt32(v1 + 0xC)) >> 5);
            return result/1000;
        }

        private CombatResouceType GetResourceType()
        {
            switch (CharacterClass)
            {
                    case ClassType.ClassType_Warrior:
                    case ClassType.ClassType_Giant:
                    case ClassType.ClassType_BladeMaster:
                    case ClassType.ClassType_BladeMasterWomen:
                    case ClassType.ClassType_Kunoichi:
                    case ClassType.ClassType_NinjaMan:
                    case ClassType.ClassType_NinjaWomen:
                    return CombatResouceType.WP;

                default:
                    return CombatResouceType.MP;
            }
        }

        private double GetMaxWeight()
        {
            //public int x0F80_MaxWeightLimit => ReadInt32(0xF88);

            return ReadInt32(Offsets.OMaxWeight) / 10000;
        }

        private double GetCurrentWeight()
        {
            //public int x1998_WeightInventoryItemsAndSilver => ReadInt32(0x19B0);
            //public int x1A98_WeightEquippedItems => ReadInt32(0x1AB0);

            return (ReadInt32(Offsets.OWeightItemsAndSilver) / 10000) + (ReadInt32(Offsets.OWeightEquippedItems) / 10000);
        }
        
        public Dictionary<int, SkillData> GetSkillList()
        {
            Dictionary<int, SkillData> list = new Dictionary<int, SkillData>();

            
            int maxSize = ReadInt32(Offsets.OSkillListCount);

            SkillData firstNode = new SkillData(ReadPointer8b(Offsets.OSkillListFirstNode));

            SkillData curNode = firstNode;

            for (int i = 0; i < maxSize; i++)
            {
                SkillData nextNode = curNode.Next;

                if (nextNode.Previous.Address == curNode.Address && nextNode.Address != firstNode.Address)
                {
                    list.Add(curNode.SkillId, curNode);

                    curNode = nextNode;
                }
                else
                {
                    break;
                }
            }
            
            return list;
        } 
    }
}
