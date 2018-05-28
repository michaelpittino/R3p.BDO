#include <Windows.h>
#include <string>
#include <sstream>

#include "Functions.h"
#include "Threading/ThreadContext.h"
#include "Threading/Thread.h"
#include "Hooks.h"

std::string eventName;
HANDLE hMainThread;


void MainThread(HMODULE hModule)
{
	auto suspendedThreads = Infinity::Utilities::Threading::ThreadContext::GetInstance().SuspendAllThreads();
	 
	Load();
			
	for (auto thread : suspendedThreads)
		thread->ResumeThread();

	HANDLE hPipe;
	char buffer[1024];
	DWORD dwRead;
	bool exit = false;

	hPipe = CreateNamedPipe(TEXT("\\\\.\\pipe\\PipesOfPiece"),
		PIPE_ACCESS_DUPLEX | PIPE_TYPE_BYTE | PIPE_READMODE_BYTE,
		PIPE_WAIT,
		1,
		1024 * 16,
		1024 * 16,
		NMPWAIT_USE_DEFAULT_WAIT,
		NULL);
	while (hPipe != NULL)
	{
		if (exit)
			hPipe = NULL;

		if (ConnectNamedPipe(hPipe, NULL) != FALSE) // wait for someone to connect to the pipe
		{
			//MessageBoxA(0, "Client connected", "", 0);

			while (!ReadFile(hPipe, buffer, sizeof(buffer), &dwRead, NULL))
			{

			}

			std::string str = buffer;
			std::string word;

			std::istringstream iss(str, std::istringstream::in);

			DWORD count = 0;

			int mainThreadId;
			__int64 arg1, arg2, arg3, arg4, arg5, arg6, arg7;
			std::string _arg1, _arg2, _arg3, _arg4, _arg5, _arg6, _arg7;

			while (iss >> word)
			{
				if (count == 0)
					eventName = word;
				if (count == 1)
					mainThreadId = std::stoi(word, 0, 0);
				if (count == 2)
				{
					arg1 = _atoi64(word.c_str());
					_arg1 = word;
				}
				if (count == 3)
				{
					arg2 = _atoi64(word.c_str());
					_arg2 = word;
				}
				if (count == 4)
				{
					arg3 = _atoi64(word.c_str());
					_arg3 = word;
				}
				if (count == 5)
				{
					arg4 = _atoi64(word.c_str());
					_arg4 = word;
				}
				if (count == 6)
				{
					arg5 = _atoi64(word.c_str());
					_arg5 = word;
				}
				if (count == 7)
				{
					arg6 = _atoi64(word.c_str());
					_arg6 = word;
				}
				if (count == 8)
				{
					arg7 = _atoi64(word.c_str());
					_arg7 = word;
				}

				count++;
			}

			if (eventName == "LuaEvent")
			{
				//MessageBoxA(0, to_string((__int64)(&_arg2[0])).c_str(), "", 0);
				lua_dostring(mainThreadId, arg1, (__int64)(&_arg2[0]), arg3);
			}
			else if (eventName == "GetItemCooldown")
			{
				//__int64 ret = GetItemCooldown((unsigned char)arg1, (unsigned char)arg2);
				//			
				//string s = to_string(GetValueFromXMM0());
				//				
				////string text = "Some Random Text returns!";

				//while (!WriteFile(hPipe, s.c_str(), s.length() + 1, &dwRead, NULL))
				//{

				//}
			}
			else if (eventName == "PopWarehouseToInventory")
			{
				WarehouseToInventory((unsigned __int8)arg1, (signed __int64)arg2, (int)arg3);
			}			
			else if (eventName == "MoveActorToActor")
			{
				MoveItemActorToActor((unsigned int)arg1, (unsigned int)arg2, (unsigned int)arg3, (unsigned int)arg4, (signed __int64)arg5);
			}
			else if (eventName == "LootingPickItems")
			{
				looting_PickItems((int)arg1);
			}
			else if (eventName == "LootingSlotClick")
			{
				looting_SlotClick((unsigned char)arg1, arg2);
			}
			else if (eventName == "UseInventoryItem")
			{
				useInventoryItem(arg1, (unsigned char)arg2, (unsigned char)arg3, (char)arg4, (char)arg5, (char)arg6);
			}			
			else if (eventName == "SellItemAtItemMarket")
			{
				sellItemAtItemMarket(arg1, (char)arg2, (unsigned char)arg3, arg4, arg5);
			}
			else if (eventName == "SetPlayerSpeeds")
			{
				set_playerspeeds((int)arg1, (int)arg2, (int)arg3);
			}
			else if (eventName == "SetVehicleSpeeds")
			{
				set_vehiclespeeds((int)arg1, (int)arg2, (int)arg3, (int)arg4, (int)arg5);
			}
			else if (eventName == "Eject")
			{
				exit = true;
			}
		}

		DisconnectNamedPipe(hPipe);
	}

	FreeLibraryAndExitThread(hModule, 0);
	ExitThread(0);
}

BOOL APIENTRY DllMain(HMODULE hModule,
	DWORD  ul_reason_for_call,
	LPVOID lpReserved
)
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
		CreateThread(0, 0, (LPTHREAD_START_ROUTINE)MainThread, hModule, 0, 0);
		//CreateThread(0, 0, (LPTHREAD_START_ROUTINE)PacketProcessor, hModule, 0, 0);
		break;
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
		break;
	}
	return TRUE;
}