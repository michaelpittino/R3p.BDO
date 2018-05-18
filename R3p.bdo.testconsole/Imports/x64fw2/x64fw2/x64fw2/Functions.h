#pragma once
#include "Addresses.h"
#include <Windows.h>
#include "Hooks.h"

void set_playerspeeds(int mvmt = 0, int atk = 0, int cst = 0)
{
	__int64 _setCharacterSpeeds = *(__int64*)(*(__int64*)(*(__int64*)(_localPlayer) + 0x00) + 0x378);

	typedef __int64(__fastcall* SetLocalPlayerSpeeds)(__int64 localActor, __int64 packetBody);
	SetLocalPlayerSpeeds setSpeeds = (SetLocalPlayerSpeeds)_setCharacterSpeeds;

	int* data = new int[3]{ mvmt, atk, cst };

	setSpeeds(*(__int64*)_localPlayer, (__int64)data);
}

void looting_PickItems(int lootingType)
{
	typedef void(__fastcall* InternalFunction)(int a1);
	InternalFunction InFunc = (InternalFunction)_looting_PickItems;

	InFunc(lootingType);
}

void sellItemAtItemMarket(__int64 ItemMarketbase, char itemFromWhereType, unsigned __int8 slotNo, signed __int64 itemCount, __int64 priceperone)
{
	typedef void(__fastcall* InternalFunction)(char a2, unsigned __int8 a3, signed __int64 a4, __int64 a5);
	InternalFunction InFunc = (InternalFunction)_sellItemAtItemMarket;

	InFunc(itemFromWhereType, slotNo, itemCount, priceperone);
}

void looting_SlotClick(unsigned __int8 slot, __int64 count)
{
	typedef void(__fastcall* InternalFunction)(unsigned __int8 a1, __int64 a2);
	InternalFunction InFunc = (InternalFunction)_looting_SlotClick;

	InFunc(slot, count);
}

void useInventoryItem(__int64 localPlayerBase, unsigned __int8 whereType, unsigned __int8 slotNo, char _a4, char _a6, char _a7)
{
	typedef void(__fastcall* InternalFunction)(unsigned __int8 a2, unsigned __int8 a3, char a4, char a6, char a7);
	InternalFunction InFunc = (InternalFunction)_useInventoryItem;

	InFunc(whereType, slotNo, _a4, _a6, _a7);
}

void WarehouseToInventory(unsigned __int8 _warehouseItemSlot, signed __int64 _itemCount, int _someValue)
{
	typedef void(__fastcall* InternalFunction)(unsigned __int8 warehouseItemSlot, signed __int64 itemCount, int someValue);
	InternalFunction InFunc = (InternalFunction)_warehouseToInventory;

	InFunc(_warehouseItemSlot, _itemCount, _someValue);
}

void MoveItemActorToActor(unsigned int _actorKeyFrom, unsigned int _actorKeyTo, unsigned int _type, unsigned int _slotNo, signed __int64 _itemCount)
{
	typedef void(__fastcall* InternalFunction)(unsigned int actorKeyFrom, unsigned int actorKeyTo, unsigned int type, unsigned int slotNo, signed __int64 itemCount);
	InternalFunction InFunc = (InternalFunction)_moveItemActorToActor;

	InFunc(_actorKeyFrom, _actorKeyTo, _type, _slotNo, _itemCount);
}

void lua_dostring(int mainThreadId, __int64 lua_state, __int64 string, __int64 strLength)
{
	_queuedLuaCode.push_back(std::string(reinterpret_cast<CHAR*>(string)));
}