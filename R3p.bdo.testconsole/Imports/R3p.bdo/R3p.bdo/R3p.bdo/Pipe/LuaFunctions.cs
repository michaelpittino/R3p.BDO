using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3p.bdo.Pipe
{
    public class LuaFunctions
    {
        public enum InteractionButton
        {
            Button_CharInfo = 0,
            Button_Exchange = 1,
            Button_Party_Invite = 2,
            Button_Dialog = 3,
            Button_Ride = 4,
            Button_Control = 5,
            Button_Looting = 6,
            Button_Collect = 7,
            Button_OpenDoor = 8,
            Button_OpenWarehouseInTent = 9,
            Button_ReBuildTent = 10,
            Button_InstallationMode = 11,
            Button_ViewHouseInfo = 12,
            Button_Havest = 13,
            Button_ParkingHorse = 14,
            Button_EquipInstallation = 15,
            Button_UnequipInstallation = 16,
            Button_OpenInventory = 17,
            Button_HorseInfo = 18,
            Button_Bussiness = 19,
            Button_Guild_Invite = 20,
            Button_Guild_Alliance_Invite = 21,
            Button_UseItem = 22,
            Button_UnbuildPersonalTent = 23,
            Button_Manufacture = 24,
            Button_Greet = 25,
            Button_Steal = 26,
            Button_Lottery = 27,
            Button_HarvestSeed = 28,
            Button_TopHouse = 29,
            Button_HouseRank = 30,
            Button_Lop = 31,
            Button_KillBug = 32,
            Button_UninstallTrap = 33,
            Button_Sympathetic = 34,
            Button_Observe = 35,
            Button_HarvestInformation = 36,
            Button_Clan_Invite = 37,
            Button_SiegeGateOpen = 38,
            Button_UnbuildKingOrLordTent = 39,
            Button_Eavesdrop = 40,
            Button_WaitComment = 41,
            Button_TakedownCannon = 42,
            Button_OpenWindow = 43,
            Button_ChangeLook = 44,
            Button_ChangeName = 45,
            Button_RepairKingOrLordTent = 46,
            Button_UserIntroduction = 47,
            Button_FollowActor = 48,
            Button_BuildingUpgrade = 49,
            Button_PvPBattle = 50,
            Button_SiegeObjectStart = 51,
            Button_SiegeObjectFinish = 52,
            Button_GateOpen = 53,
            Button_GateClose = 54,
            Button_UninstallBarricade = 55,
            Button_ServantRepair = 56,
            Button_LanternOn = 57,
            Button_LanternOff = 58,
            Button_Escape = 59
        }

        public enum QuestConditionCheckType
        {
            Complete = 0,
            Progress = 1,
            NotAccept = 99
        }

        public const string Inventory_Show = "InventoryShowAni()";
        public const string Inventory_Hide = "InventoryHideAni()";

        public const string WorkerManager_Show = "WorkerManager_ShowAni()";
        public const string WorkerManager_Hide = "WorkerManager_HideAni()";
        public const string WorkerManager_RecoverAll = "HandleClicked_workerManager_RestoreAll()";
        public const string WorkerManager_RepeatAll = "HandleClicked_workerManager_ReDoAll()";

        public const string WorkerRestore_Close = "workerRestoreAll_Close()";

        public static string WorkerRestore_Confirm(int slotId)
        {
            return "workerRestoreAll_Confirm(" + slotId + ")";
        }

        public static string WorkerRestore_SelectItem(int slotId)
        {
            return "HandleClicked_restoreAll_SelectItem(" + slotId + ")";
        }

        public const string Warehouse_Show = "WarehouseShowAni()";
        public const string Warehouse_Hide = "WarehouseHideAni()";

        public const string Options_Show = "Option_ShowAni()";
        public const string Options_Hide = "Option_HideAni()";

        public const string ItemMarket_BuyConfirm_SetMax = "HandleClicked_ItemMarket_BuyConfirm_SetMax()";
        public const string ItemMarket_BuyConfirm_BuyItem = "HandleClicked_ItemMarket_BuyItem()";

        public const string MessageBox_YesButton = "messageBox_YesButtonUp()";

        public const string Manufacture_RepeatAction = "Manufacture_RepeatAction()";

        public static string Warehouse_Item_Click(int slot)
        {
            return "HandleClickedWarehouseItem(" + slot + ")";
        }

        public const string ItemMarket_GetAllItems = "ItemMarketSetItem_GetAllItemCheck()";

        public static string ItemMarket_SwitchItemGroup(int slot, int itemId)
        {
            return "HandleClicked_ItemMarket_GroupItem(" + slot + "," + itemId + ")";
        }

        public static string NewQuickSlot_Use(int panelId)
        {
            return "HandleClicked_NewQuickSlot_Use(" + panelId + ")";
        }

        public static string ItemMarket_BuyItem(int slot, int itemId, bool singleItem)
        {
            return "HandleClicked_ItemMarket_SingleItem(" + slot + "," + itemId + "," + singleItem + ")";
        }

        public static string ItemMarket_SetAlarm(int itemMarketId)
        {
            return "HandleClicked_ItemMarket_SetAlarm(" + itemMarketId + ")";
        }

        public static string ItemMarket_SetPreOrder(int itemMarketId)
        {
            return "FGlobal_ItemMarketPreBid_Open(" + itemMarketId + ",0)";
        }

        public static string Interaction_ButtonPush(InteractionButton btn)
        {
            return "Interaction_ButtonPushed(" + ((int)btn + 1) + ")";
        }

        public static string QuestWidget_FindTarget(int questGroupId, int questId, QuestConditionCheckType conditionType, bool autoNavi = false)
        {
            return "HandleClicked_QuestWidget_FindTarget(" + questGroupId + "," + questId + "," + (int)conditionType + "," + autoNavi + ")";
        }

        public static string Inventory_SlotRClick(int slotNo)
        {
            return "Inventory_SlotRClick(" + slotNo + ")";
        }
    }
}
