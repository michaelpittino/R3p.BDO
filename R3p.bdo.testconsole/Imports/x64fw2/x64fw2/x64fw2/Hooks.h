#pragma once

#pragma comment(lib, "Libs/lua5.1.lib")

#include "Lua/LuaIntf.h"

#ifdef DEBUG
#pragma comment(lib, "Libs/MinHook_MTd.lib")
#else
#pragma comment(lib, "Libs/MinHook_MT.lib")
#endif

#include <vector>

#include "MinHook/MinHook.h"
#include "Addresses.h"
#include <fstream>
#include <experimental/filesystem>

typedef void(__fastcall* __lua_dostring)(lua_State* L, const char* str, __int64 strLength);
__lua_dostring olua_dostring = (__lua_dostring)_lua_dostring;

typedef const char *(__fastcall* __lua_tostring)(lua_State *L, int index);
__lua_tostring olua_tostring = (__lua_tostring)_lua_tostring;

typedef __int64(__fastcall* __lua_gettop)(lua_State* L);
__lua_gettop olua_gettop = reinterpret_cast<__lua_gettop>(_lua_gettop);
__lua_gettop fplua_gettop = NULL;

__int64 lastTick = 0;
std::vector<std::string> _queuedLuaCode;

void appendLineToFile(std::string line)
{
	std::string filepath;

	TCHAR buffer[MAX_PATH]; 

	auto readChars = GetModuleFileNameA(NULL, buffer, MAX_PATH);

	filepath = std::string(buffer, buffer + readChars) + "/lualog.txt";

    std::ofstream file;
    
    file.open(filepath, std::ios::out | std::ios::app);
    /*if (file.fail())
        throw std::ios_base::failure(std::strerror(errno));*/
	   
    file.exceptions(file.exceptions() | std::ios::failbit | std::ifstream::badbit);

    file << line << std::endl;
}

__int64 mlua_gettop(lua_State* L)
{
	if (_queuedLuaCode.size() > 0 && lastTick + 200 <= GetTickCount64())
	{
		lastTick = GetTickCount64();

		//LuaIntf::LuaState _L = LuaIntf::LuaState(L);

		std::string code = _queuedLuaCode.front();

		//_L.doString(code.c_str());

		olua_dostring(L, code.c_str(), code.length());
						
		try
		{
			if (_queuedLuaCode.size() > 0)
				_queuedLuaCode.erase(_queuedLuaCode.begin());
		}
		catch (std::exception &e)
		{

		}
	}

	return fplua_gettop(L);
}

bool Load()
{

	if (MH_Initialize() != MH_OK)
	{
		return false;
	}

	if (MH_CreateHook(olua_gettop, mlua_gettop, reinterpret_cast<LPVOID*>(&fplua_gettop)) != MH_OK)
	{
		return false;
	}

	if (MH_EnableHook(MH_ALL_HOOKS) != MH_OK)
	{
		return false;
	}

	return true;
}

bool Unload()
{
	if (MH_DisableHook(MH_ALL_HOOKS) != MH_OK)
	{
		return false;
	}

	if (MH_Uninitialize() != MH_OK)
	{
		return false;
	}

	return true;
}