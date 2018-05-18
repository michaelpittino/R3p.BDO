using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using R3p.bdo.Memory;
using R3p.bdo.Pipe;

namespace R3p.bdo.GameInternals.Structs.Actors
{
    public class PlayerInventoryItem : MemoryObject
    {
        public int SlotNo { get; set; }

        public PlayerInventoryItem(long address, int slotNo)
        {
            Address = address;
            SlotNo = slotNo;
        }

        public ItemData x0000_PTR_ItemData => new ItemData(ReadPointer8b(0x0000));
        public int x0008_ItemCount => ReadInt32(Address + 0x0008);
        public uint x0018_CurrentDurability => ReadInt16(Address + 0x0018);
        public uint x001A_MaxDurability => ReadInt16(Address + 0x001A);
        public uint x0030_N29D0079B => ReadInt16(Address + 0x0030);
        public uint x0032_N29EB256F => ReadInt16(Address + 0x0032);
        public uint x0034_N29EB339B => ReadInt16(Address + 0x0034);
        public ItemData x0050_PTR_SocketedCrystal0 => new ItemData(ReadPointer8b(0x0050));
        public ItemData x0058_PTR_SocketedCrystal1 => new ItemData(ReadPointer8b(0x0058));

        public void MakeGhostItem(int itemId, int enchantLevel, int count)
        {
            var realItem =
                Collection.ItemMarket.Base.ItemMarketList.List[itemId].FirstOrDefault(
                    x => x.x0012_EnchantLevel == enchantLevel);

            if (realItem == null)
                return;

            Write(0x0, BitConverter.GetBytes(realItem.ItemData.Address));
            Write(0x8, BitConverter.GetBytes(count));
            Write(0x10, BitConverter.GetBytes(-1));
            Write(0x18, BitConverter.GetBytes(100));
            Write(0x1A, BitConverter.GetBytes(100));
        }

        public void MakeGhostItem(long address, int count)
        {
            Write(0x0, BitConverter.GetBytes(address));
            Write(0x8, BitConverter.GetBytes(count));
            Write(0x10, BitConverter.GetBytes(-1));
            Write(0x18, BitConverter.GetBytes(100));
            Write(0x1A, BitConverter.GetBytes(100));
        }

        public long itemUseStartTick;
        public long itemUseEndTick;
        public void UseItem(int slot)
        {
            Pipe.Call.useInventoryItem(FromWhereType.Inventory, SlotNo);
            Thread.Sleep(200);

            itemUseStartTick = Collection.Actors.Local.PlayerData.lastItemUseStartTick;
            itemUseEndTick = Collection.Actors.Local.PlayerData.getLastItemUseEndTick(slot);

            //Thread.Sleep(100);
        }

        public void DeleteItem(int count)
        {
            Pipe.Call.DoString("deleteItem((getSelfPlayer()):getActorKey(),(CppEnums.ItemWhereType).eInventory," + SlotNo + "," + count + ")");

            Thread.Sleep(100);
        }

        public void SellItem(FromWhereType fromWhereType, int sellValue)
        {
            var mi = Collection.ItemMarket.Base.ItemMarketList.List[
                                (int)x0000_PTR_ItemData.ItemIndex].First(
                                    x => x.x0012_EnchantLevel == x0000_PTR_ItemData.EnchantLevel);

            var count = x0008_ItemCount;

            if (x0008_ItemCount > x0000_PTR_ItemData.MaxRegisterCountForItemMarket)
                count = x0000_PTR_ItemData.MaxRegisterCountForItemMarket;

            long price = sellValue;

            if (sellValue == 0)
                price = mi.x0038_RecentPrice;
            else
            {
                if (sellValue < mi.x0050_MinPrice)
                    price = mi.x0050_MinPrice;
                else if (sellValue > mi.x0048_MaxPrice)
                    price = mi.x0048_MaxPrice;
            }

            Pipe.Call.SellItemAtItemMarket(fromWhereType, SlotNo, count, price);

            Log.Post("Registered Item At ItemMarket - ItemId(" + x0000_PTR_ItemData.ItemIndex + ") Count(" + count + ") SlotNo(" + SlotNo + ") Price(" + price + ")", LogModule.AutoItemRegister);

            Thread.Sleep(100);
        }

        public void SendToWarehouse(int count)
        {
            Log.Post("Send Item To Warehouse - ItemId(" + x0000_PTR_ItemData.ItemIndex + ") SlotNo(" + SlotNo + ") Count(" + count +")", LogModule.Global);

            Pipe.Call.DoString("pushItemToWarehouse(" + SlotNo + "," + count + "," + Collection.Actors.Local.PlayerData.ActorKey + ")");
            Thread.Sleep(2000);
        }

        public bool isOnCooldown()
        {
            if (Collection.Base.Ticks.BaseTicks.BaseTick > itemUseEndTick)
                return false;

            return true;
        }

        public bool isFishingRod()
        {
            return Collection.Fishing.Rods.List.Contains((int) x0000_PTR_ItemData.ItemIndex);
        }

        public bool isRepairable()
        {
            return x0000_PTR_ItemData.RepairPrice != 0;
        }

        public bool isRegularHpPotion()
        {
            return Collection.Potion.Potions.HP_Regular.Contains((int) x0000_PTR_ItemData.ItemIndex);
        }

        public bool isRbfHpPotion()
        {
            return Collection.Potion.Potions.HP_RBF.Contains((int)x0000_PTR_ItemData.ItemIndex);
        }

        public bool isRegularMpPotion()
        {
            return Collection.Potion.Potions.MP_Regular.Contains((int)x0000_PTR_ItemData.ItemIndex);
        }

        public bool isRbfMpPotion()
        {
            return Collection.Potion.Potions.MP_RBF.Contains((int)x0000_PTR_ItemData.ItemIndex);
        }

        public bool isRegularWpPotion()
        {
            return Collection.Potion.Potions.WP_Regular.Contains((int)x0000_PTR_ItemData.ItemIndex);
        }

        public bool isRbfWpPotion()
        {
            return Collection.Potion.Potions.WP_RBF.Contains((int)x0000_PTR_ItemData.ItemIndex);
        }

        public long sub_14049EDF0()
        {
            long v4; // rbx@1
            long v5; // rdi@1
            long result; // rax@1
            long v7; // r9@1
            long v8; // r10@2
            long v9; // r8@2
            long v10; // rax@3
            short v11; // ax@5
            long v12; // rcx@6
            long v13; // rax@6

            long usedItemStruct = Collection.Actors.Local.PlayerData.Address + 0x1430;
            int someValue = ReadInt32(ReadPointer8b(x0000_PTR_ItemData.Address + 0x1B8) + 4 * 4);
            long a3 = 1;
            long currentTick = Collection.Base.Ticks.BaseTicks.BaseTick;

            v4 = currentTick;
            v5 = usedItemStruct;
            result = sub_14049ED40(usedItemStruct, (short)someValue, (int)a3);
            v7 = result;
            if (result == 0)
            {
                v8 = ReadPointer8b(result + 32);
                v9 = v4;
                if (ReadInt32(v8 + 220) == 0)
                {
                    v10 = ReadPointer8b(ReadPointer8b(v5 + 0x88));
                    if (v4 < v10)
                        v9 = v10;
                }
                v11 = (short)ReadInt16(v8 + 224);
                if (v11 > 0u)
                {
                    v12 = v11;
                    v13 = ReadPointer8b(v5 + 136);
                    if ((ulong)v9 < (ulong)ReadPointer8b(v13 + 8 * v12) )
                    v9 = ReadPointer8b(v13 + 8 * v12);
                }
                if (v4 < ReadPointer8b(v7 + 8) && ReadInt32(v8 + 228) != 0 && (ulong)v9 < (ulong)ReadPointer8b(v7 + 8) )
                v9 = ReadPointer8b(v7 + 8);
                if (v9 == v4)
                    v9 = 0;
                result = v9;
            }
            return result;
        }

        private long sub_14049ED40(long a1, short a2, int a3)
        {
            short v3; // r9@1
            ulong v4; // rbx@1
            ulong v5; // rax@1
            ulong v6; // r8@1
            ulong v7; // rdx@1
            ulong v8; // rcx@1
            long v9; // r8@1
            long v10; // rax@1
            long v11; // r10@1
            ulong v12; // r11@1
            long v13; // rcx@3
            long v14; // rcx@12

            v3 = a2;
            v4 = (ulong)a1 + ((ulong)a3 << 6);
            v5 = (byte)(a2 >> 8);
            v6 = 1099511628211 * ((byte)a2 ^ 0xCBF29CE484222325);
            v7 = (ulong)ReadPointer8b((long)v4 + 32);
            v8 = 1099511628211 * (v5 ^ v6) & (ulong)ReadPointer8b((long)v4 + 56);
            v9 = ReadPointer8b((long)v4 + 16);
            v8 *= 2;
            v10 = ReadPointer8b((long)(v7 + 8 * v8));
            v11 = ReadPointer8b((long)(v7 + 8 * v8));
            v12 = v7 + 8 * v8;
            while (true)
            {
                if (v11 == v9)
                    v13 = ReadPointer8b((long)v4 + 16);
                else
                    v13 = ReadPointer8b(ReadPointer8b((long)v12 + 8));
                if (v10 == v13)
                {
                    v10 = ReadPointer8b((long)v4 + 16);
                    goto LABEL_12;
                }
                if (v3 == ReadInt16(v10 + 16))
                    break;
                v10 = ReadPointer8b(v10);
            }
            if (v3 != ReadInt16(v10 + 16))
                v10 = ReadPointer8b((long)v4 + 16);
            LABEL_12:
            v14 = v10 + 24;
            if (v9 == v10)
                v14 = 0;
            return v14;
        }
    }
}
