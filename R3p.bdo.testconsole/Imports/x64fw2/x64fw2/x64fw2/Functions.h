#pragma once
#include "Addresses.h"
#include <Windows.h>
#include "Hooks.h"
#include "ByteBuffer.h"

typedef void(__fastcall* SetLocalPlayerSpeeds)(__int64 localActor, unsigned __int8 speedIndex, int speedValue, int a4);
SetLocalPlayerSpeeds f_SetSpeeds = NULL;

typedef __int64(__fastcall* SetVehicleStats)(__int64 VTableServantStats, __int64 VTableActorRoot, char* PacketData, WORD PacketDataLength);
SetVehicleStats f_SetVehicleStats = (SetVehicleStats)0x140911A00;

typedef void(__fastcall* PickItems)(int a1);
PickItems f_PickItems = (PickItems)_looting_PickItems;

typedef void(__fastcall* SellItemAtItemMarket)(char a2, unsigned __int8 a3, signed __int64 a4, __int64 a5);
SellItemAtItemMarket f_SellItemAtItemmarket = (SellItemAtItemMarket)_sellItemAtItemMarket;

typedef void(__fastcall* LootingSlotClick)(unsigned __int8 a1, __int64 a2);
LootingSlotClick f_LootingSlotClick = (LootingSlotClick)_looting_SlotClick;

typedef void(__fastcall* UseInventoryItem)(unsigned __int8 a2, unsigned __int8 a3, char a4, char a6, char a7);
UseInventoryItem f_UseInventoryItem = (UseInventoryItem)_useInventoryItem;

typedef void(__fastcall* _WarehouseToInventory)(unsigned __int8 warehouseItemSlot, signed __int64 itemCount, int someValue);
_WarehouseToInventory f_WarehouseToInventory = (_WarehouseToInventory)_warehouseToInventory;

typedef void(__fastcall* ActorToActor)(unsigned int actorKeyFrom, unsigned int actorKeyTo, unsigned int type, unsigned int slotNo, signed __int64 itemCount);
ActorToActor f_ActorToActor = (ActorToActor)_moveItemActorToActor;

void set_vehiclespeeds(int actorGameObjectId, int accel = 0, int speed = 0, int turn = 0, int brake = 0)
{	
	__int64 VTableServantStats = 0x1418CCC40;
	__int64 VTableActorRoot = 0x1418DDC30;

	ByteBuffer* buffer = new ByteBuffer(36);
	buffer->putShort(0);		//opCode
	buffer->putInt(actorGameObjectId);
	buffer->putInt(accel);
	buffer->putInt(speed);
	buffer->putInt(turn);
	buffer->putInt(brake);
	buffer->put((BYTE)10);		//MatingCount
	buffer->putInt(0);			//DeathCount
	buffer->put((BYTE)0);		//IsImprint
	buffer->put((BYTE)0);		//IsClearedMatingCount
	buffer->put((BYTE)0);		//IsClearedDeathCount
	buffer->putInt(0);
	buffer->putShort(0);		//FormIndex

	f_SetVehicleStats((__int64)&VTableServantStats, (__int64)&VTableActorRoot, (char*)&buffer->buf[0], buffer->size());

	delete buffer;
}

void set_playerspeeds(int mvmt = 0, int atk = 0, int cst = 0)
{
	__int64 _setCharacterSpeeds = *(__int64*)(*(__int64*)(*(__int64*)(_localPlayer) + 0x00) + 0x378);
		
	f_SetSpeeds = (SetLocalPlayerSpeeds)_setCharacterSpeeds;
			
	
	f_SetSpeeds(*(__int64*)_localPlayer, 0, mvmt, 0);	
	f_SetSpeeds(*(__int64*)_localPlayer, 1, atk, 0);
	f_SetSpeeds(*(__int64*)_localPlayer, 2, cst, 0);
}

void looting_PickItems(int lootingType)
{
	f_PickItems(lootingType);
}

void sellItemAtItemMarket(__int64 ItemMarketbase, char itemFromWhereType, unsigned __int8 slotNo, signed __int64 itemCount, __int64 priceperone)
{
	f_SellItemAtItemmarket(itemFromWhereType, slotNo, itemCount, priceperone);
}

void looting_SlotClick(unsigned __int8 slot, __int64 count)
{	
	f_LootingSlotClick(slot, count);
}

void useInventoryItem(__int64 localPlayerBase, unsigned __int8 whereType, unsigned __int8 slotNo, char _a4, char _a6, char _a7)
{
	f_UseInventoryItem(whereType, slotNo, _a4, _a6, _a7);
}

void WarehouseToInventory(unsigned __int8 _warehouseItemSlot, signed __int64 _itemCount, int _someValue)
{
	f_WarehouseToInventory(_warehouseItemSlot, _itemCount, _someValue);
}

void MoveItemActorToActor(unsigned int _actorKeyFrom, unsigned int _actorKeyTo, unsigned int _type, unsigned int _slotNo, signed __int64 _itemCount)
{
	f_ActorToActor(_actorKeyFrom, _actorKeyTo, _type, _slotNo, _itemCount);
}

void lua_dostring(int mainThreadId, __int64 lua_state, __int64 string, __int64 strLength)
{
	_queuedLuaCode.push_back(std::string(reinterpret_cast<CHAR*>(string)));
}