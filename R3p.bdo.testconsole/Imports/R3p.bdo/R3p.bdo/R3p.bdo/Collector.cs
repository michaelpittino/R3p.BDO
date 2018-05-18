using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using R3p.bdo.GameInternals.Structs.Actors;
using R3p.bdo.GameInternals.Structs.Base;
using R3p.bdo.GameInternals.Structs.Camera;
using R3p.bdo.GameInternals.Structs.Chat;
using R3p.bdo.GameInternals.Structs.DeezNutz;
using R3p.bdo.GameInternals.Structs.ItemMarket;
using R3p.bdo.GameInternals.Structs.Loot;
using R3p.bdo.GameInternals.Structs.MainWindow;
using R3p.bdo.GameInternals.Structs.Map;
using R3p.bdo.GameInternals.Structs.Mounts;
using R3p.bdo.GameInternals.Structs.Objects;
using R3p.bdo.GameInternals.Structs.Quests;
using R3p.bdo.GameInternals.Structs.Region;
using R3p.bdo.GameInternals.Structs.SystemVariables;
using R3p.bdo.GameInternals.Structs.UI;
using R3p.bdo.GameInternals.Structs.Warehouse;
using R3p.bdo.GameInternals.Structs.Worker;
using R3p.bdo.GameInternals.Structs.World;
using R3p.bdo.Memory;

namespace R3p.bdo
{
    public class Collector
    {
        public static bool requestReload = false;
        public static bool actorCollectionChanged = false;

        private static long lastTick;
        
        public static void Collect(bool forceReload = false)
        {
            if (requestReload)
                requestReload = false;

            if (forceReload)
                requestReload = true;

            GameState();
            BaseTicks();

            if (Collection.Base.Ticks.BaseTicks.BaseTick > lastTick)
                lastTick = Collection.Base.Ticks.BaseTicks.BaseTick;
            else
            {
                Thread.Sleep(1);
            }

            //if (Collection.Base.States.GameState.isLoading || Collection.Base.States.GameState.isChangingChannel)
            //    requestReload = true;

            if (Collection.Actors.Local.PlayerData == null || !Collection.Actors.Local.PlayerData.isReadyToPlay)
                requestReload = true;

            if (requestReload)
            {
                while (!ActorData.GetLocalPlayerData().isReadyToPlay)
                    Thread.Sleep(1000);

                Thread.Sleep(3000);
            }

            BaseValues();
            SystemVariables();

            LocalPlayer();
            ActorList();
            Warehouse();
            UIBase();
            Region();
            WorkerList();
            Mounts();
            ChatList();
            Quests();
            DeezNutz();
            MapTiles();
            ItemMarketList();
            CameraManager();
            ClientRect();
            WorldMap();
            ViewMatrix();
            InputEventListener();
            Loot();
            MainWindow();
        }

        private static void BaseTicks()
        {
            if(Collection.Base.Ticks.BaseTicks == null)
                Collection.Base.Ticks.BaseTicks = new Ticks();
        }

        private static void CameraManager()
        {
            if (Collection.Camera.CameraManager.Base == null || requestReload)
                Collection.Camera.CameraManager.Base = new CameraManager();
        }

        private static void MainWindow()
        {
            if (Collection.MainWindow.Base.Handle == null || requestReload)
                Collection.MainWindow.Base.Handle = new MainWindow();
        }

        private static void BaseValues()
        {
            if(Collection.Base.Values._Values == null || requestReload)
                Collection.Base.Values._Values = new Values();
        }

        private static void LocalPlayer()
        {
            if (Collection.Actors.Local.PlayerData == null || requestReload)
                Collection.Actors.Local.PlayerData = ActorData.GetLocalPlayerData();
        }

        private static void MapTiles()
        {
            if (Collection.Map.MapTiles.Base == null || requestReload)
                Collection.Map.MapTiles.Base = new MapTiles();
        }

        private static void SystemVariables()
        {
            if (Collection.SystemVariables.Base.origList == null || requestReload)
                Collection.SystemVariables.Base.origList = new SystemVariables();
        }

        private static int _aListCount;
        private static void ActorList()
        {
            if(Collection.Actors.Global._ActorList == null || requestReload)
                Collection.Actors.Global._ActorList = new ActorList();

            var curCount = Collection.Actors.Global._ActorList.ActorListCount;

            if (!curCount.Equals(_aListCount))
            {
                Collection.Actors.Global.ActorList = Collection.Actors.Global._ActorList.List.ToArray();

                _aListCount = curCount;

                actorCollectionChanged = true;
            }
            else
            {
                actorCollectionChanged = false;
            }
        }

        private static void UIBase()
        {
            if(Collection.UI.Base.UIBase == null || requestReload)
                Collection.UI.Base.UIBase = new UIBase();
        }

        private static void Region()
        {
            if(Collection.Region.Base.CurrentRegion == null || requestReload)
                Collection.Region.Base.CurrentRegion = new RegionBase();

            if (Collection.Region.Base.RegionInfoManager == null || requestReload)
                Collection.Region.Base.RegionInfoManager = new RegionInfoManager();
        }

        private static void WorkerList()
        {
            if(Collection.Worker.Base.WorkerList == null || requestReload)
                Collection.Worker.Base.WorkerList = new WorkerList();
        }

        private static void ChatList()
        {
            if(Collection.Chat.Base.ChatList == null || requestReload)
                Collection.Chat.Base.ChatList = new ChatList();
        }

        private static void ItemMarketList()
        {
            if(Collection.ItemMarket.Base.ItemMarketList == null || requestReload)
                Collection.ItemMarket.Base.ItemMarketList = new MarketList();
        }

        private static void ActorObjects()
        {
            if(Collection.Objects.ActorObjects.ActorObjectDataList == null || requestReload)
                Collection.Objects.ActorObjects.ActorObjectDataList = new ActorObjectDataList();
        }

        private static void ClientRect()
        {
            Win32.GetClientRect(Engine.Instance.Process.MainWindowHandle, out Collection.Client.Base.ClientRect);
            Win32.ClientToScreen(Engine.Instance.Process.MainWindowHandle, ref Collection.Client.Base.ClientRect);
        }

        private static void ViewMatrix()
        {
            if(Collection.Camera.ViewMatrix.PVMatrix == null || requestReload)
                Collection.Camera.ViewMatrix.PVMatrix = new ViewMatrix();
        }

        private static void WorldMap()
        {
            if(Collection.World.Base.WorldMap == null || requestReload)
                Collection.World.Base.WorldMap = new WorldMap();
        }

        private static void Warehouse()
        {
            if(Collection.Warehouse.Base.Current == null || requestReload || Collection.Warehouse.Base.Current.Address == 0)
                Collection.Warehouse.Base.Current = new CurrentWarehouse();
        }

        private static void Mounts()
        {
            if(Collection.Mounts.Rides.CurrentVehicle == null || requestReload)
                Collection.Mounts.Rides.CurrentVehicle = new Vehicle();
        }

        private static void InputEventListener()
        {
            if(Collection.Base.Events.InputEventListener == null || requestReload)
                Collection.Base.Events.InputEventListener = new InputEventListener();
        }

        private static void Loot()
        {
            if(Collection.Loot.Base.Loot == null || requestReload)
                Collection.Loot.Base.Loot = new Looting();
        }

        private static void GameState()
        {
            if(Collection.Base.States.GameState == null)
                Collection.Base.States.GameState = new GameState();
        }

        private static void Quests()
        {
            if (Collection.Quests.Base.GuildQuests == null || requestReload)
                Collection.Quests.Base.GuildQuests = new GuildQuests();
        }

        private static void DeezNutz()
        {
            if (Collection.DeezNutz.Base.List == null || requestReload)
                Collection.DeezNutz.Base.List = new DeezNutz();
        }
    }
}
