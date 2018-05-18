using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using R3p.bdo.Automation.Hacks;
using R3p.bdo.Automation.ItemMarket;
using R3p.bdo.Automation.LifeSkills;
using R3p.bdo.Automation.Loot;
using R3p.bdo.Automation.Potion;
using R3p.bdo.Automation.Worker;
using R3p.bdo.GameExternals.Structs.Overlay;
using R3p.bdo.GameInternals.Structs.Actors;
using R3p.bdo.GameInternals.Structs.Region;
using R3p.bdo.GameInternals.Structs.Skills;
using R3p.bdo.GameInternals.Structs.UI;
using R3p.bdo.Memory;
using R3p.bdo.Pipe;
using R3p.bdo.settings;
using R3p.injector;
using FluentFTP;
using R3p.bdo.GUIloader;
using MainWindow = R3p.bdo.ui.MainWindow;

namespace R3p.bdo.testconsole
{
    public class Program
    {
        public static Thread MainThread;
        public static MainWindow Overlay;

        static void Main(string[] args)
        {
#if !DEBUG
            if (Convert.ToInt32(args[0]) != Engine._supportedVersion)
                Terminate();
#endif

            Engine.Create("BlackDesert64");
            
            if (Engine.Instance.SupportedVersion)
            {
                string iError;
                if (!DllInjector.DoInject(Engine.Instance.Process, new FileInfo(@".\x64fw2.dll").FullName, out iError))
                {
                    Console.WriteLine("Dll Injection failed!");
                    Console.ReadLine();
                }
                
                Console.WriteLine("Dll injected!");

                LoadSettings();

                Console.WriteLine("Starting Main - Thread");
                
                MainThread = new Thread(mThread);
                MainThread.Start();

                StartOverlay();
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Client-Version isn't supported. Please wait for an updated version of the tool.");
                Console.ReadLine();
            }
        }

        private static void ExecuteAsAdmin(string fileName, string[] args)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = fileName;
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.Verb = "runas";
            proc.StartInfo.Arguments = String.Join(" ", args);
            proc.Start();
        }

        private static void Terminate()
        {
            Engine.Create("BlackDesert64");
            Engine.Instance.Process.Kill();

            Engine.Instance.Process.WaitForExit();

            Environment.Exit(0);
        }

        private static AutoFish fishing;
        private static AutoRestore workerRestore;
        private static UI hackUi;
        private static AutoPotion autoPot;
        private static AutoItemRegister autoItemRegister;
        private static Navigation hackNavigation;
        private static SpeedHack speedHack;
        private static AutoProcessing autoProcessing;
        private static AutoItemBuy autoItemBuy;
        private static AutoLoot autoLoot;

        private static bool _autoItemBuyTrigger = false;
        private static bool _record = false;
        private static bool _grinder = false;

        static void sThread()
        {
            while (true)
            {
                if (Collection.MainWindow.Base.Handle != null)
                    if (!Collection.MainWindow.Base.Handle.hasFocus)
                        Collection.MainWindow.Base.Handle.setFocus();

                Thread.Sleep(1);
            }
        }
        
        static void ActivityLog()
        {
            using (FtpClient conn = new FtpClient())
            {
                conn.Host = FtpCredendtials.FTPHost;
                conn.Credentials = FtpCredendtials.FtpCredential;
                conn.Connect();

                if (!conn.DirectoryExists("/Log/" + Engine._supportedVersion))
                    conn.CreateDirectory("/Log/" + Engine._supportedVersion);

                string fileName = Collection.Actors.Local.PlayerData.FamilyName.GetHashCode().ToString() + ".log";

                if (!conn.FileExists("/Log/" + fileName))
                {
                    if (!File.Exists(@"./" + fileName))
                        File.Create(@"./" + fileName).Close();

                    conn.UploadFile(@"./" + fileName, "/Log/" + Engine._supportedVersion + "/" + fileName);
                }
            }
        }

        static void mThread()
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";

            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            Collector.Collect();

            ActivityLog();

            LoadStaticSpawnData();

            InitSystemVariables();

            LoadLuaMod();

            InitModules();

            while (true)
            {
                Collector.Collect();

                GetHotKeys();

                if (Collector.requestReload)
                {
                    InitModules();
                    LoadLuaMod();
                }

                if (!fishing.isFishing && !_autoItemBuyTrigger)
                    Console.Title = String.Format("X:{0} Y:{1} Z:{2}", Collection.Actors.Local.PlayerData.WorldPosition[0], Collection.Actors.Local.PlayerData.WorldPosition[1], Collection.Actors.Local.PlayerData.WorldPosition[2]);

                workerRestore.Run();
                hackUi.Run();
                speedHack.Run();
                autoPot.Run();
                autoItemRegister.Run();

                if (_autoItemBuyTrigger && !autoItemBuy.ItemMarket.isVisible())
                    _autoItemBuyTrigger = false;

                autoItemBuy.Run(_autoItemBuyTrigger);
                autoItemBuy.PostStats();

                autoProcessing.Run();
                fishing.Run(Collector.requestReload);
                fishing.PostStats();

                if (Settings.Overlay.Enabled)
                {
                    Overlay._draw = true;

                    while (Overlay._draw)
                        Thread.Sleep(1);
                }
            }
        }

        private static void LoadStaticSpawnData()
        {
            Console.WriteLine("Loading Static SpawnData");

            if (File.Exists(@".\GoldenChests.bin"))
            {
                MemoryStream ms = new MemoryStream(File.ReadAllBytes(@".\GoldenChests.bin"));
                BinaryReader br = new BinaryReader(ms);

                int count = br.ReadInt32();

                List<float[]> buffer = new List<float[]>();

                for (int i = 0; i < count; i++)
                {
                    buffer.Add(new float[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() });
                }

                Collection.StaticSpawnData.GoldenChests.List = buffer;
            }
            else
            {
                Console.WriteLine("Couldnt find file GoldenChests.bin");
            }
        }

        private static void StartOverlay()
        {
            if (Settings.Overlay.Enabled)
            {
                Console.WriteLine("Starting Overlay MainWindow");

                Overlay = new MainWindow();
                Overlay.Enabled = Settings.Overlay.Enabled;
                Overlay.settingsActors = Settings.Overlay.Actors;
                Overlay.GoldenChests = Settings.Overlay.GoldenChests;

                Overlay.ShowDialog();
            }
        }

        private static void LoadSettings()
        {
            if (!File.Exists(Files.fileSettings))
                Xml.Writer.SaveSettings();

            Console.WriteLine("Loading Settings from settings.xml");

            Xml.Reader.LoadSettings();

            Settings.HotKeys.Transform();
            Settings.AutoFish.Transform();
            Settings.AutoRestore.Transform();
            Settings.UIHack.Transform();
            Settings.AutoPotion.Transform();
            Settings.AutoItemRegister.Transform();
            Settings.AutoItemBuy.Transform();
            Settings.Overlay.Transform();
            Settings.SpeedHack.Transform();
            Settings.AutoProcessing.Transform();
        }

        private static void DumpToFiles()
        {
            if (!Directory.Exists(@".\dumps"))
                Directory.CreateDirectory(@".\dumps");

            DumpInventory();
            DumpWarehouse();
            DumpMarketPlace();
        }

        private static void DumpInventory()
        {
            List<string> output = new List<string>();

            if (Collection.Actors.Local.PlayerData == null)
                output.Add("PlayerData not found!");
            else
            {

                foreach (var item in Collection.Actors.Local.PlayerData.Inventory.Items)
                {
                    string itemData = "";

                    itemData += "Slot\t" + item.SlotNo + "\n";
                    itemData += "Name\t" + item.x0000_PTR_ItemData.Name + "\n";
                    itemData += "ItemId\t" + item.x0000_PTR_ItemData.ItemIndex + "\n";
                    itemData += "Count\t" + item.x0008_ItemCount + "\n";
                    itemData += "----------------------------------------------------------\n";

                    output.Add(itemData);
                }

            }

            File.WriteAllLines(@".\dumps\inventory.txt", output);
        }

        private static void DumpWarehouse()
        {
            List<string> output = new List<string>();

            if (Collection.Warehouse.Base.Current.Inventory == null)
                output.Add("WarehouseData not found!");
            else
            {

                foreach (var item in Collection.Warehouse.Base.Current.Inventory.Items)
                {
                    string itemData = "";

                    itemData += "Slot\t" + item.SlotNO + "\n";
                    itemData += "Name\t" + item.x0008_PTR_ItemData.Name + "\n";
                    itemData += "ItemId\t" + item.x0008_PTR_ItemData.ItemIndex + "\n";
                    itemData += "Count\t" + item.x0010_ItemCount + "\n";
                    itemData += "----------------------------------------------------------\n";

                    output.Add(itemData);
                }

            }

            File.WriteAllLines(@".\dumps\warehouse.txt", output);
        }

        private static void DumpMarketPlace()
        {
            List<string> output = new List<string>();

            if (Collection.ItemMarket.Base.ItemMarketList == null)
                output.Add("MarketPlaceData not found!");
            else
            {

                foreach (var item in Collection.ItemMarket.Base.ItemMarketList.List)
                {
                    string itemData = "";

                    itemData += "ItemId\t" + item.Key + "\n";
                    itemData += "Name\t" + item.Value.First().ItemData.Name + "\n";
                    itemData += "EnchantMin\t" + item.Value.Min(x => x.x0012_EnchantLevel) + "\n";
                    itemData += "EnchantMax\t" + item.Value.Max(x => x.x0012_EnchantLevel) + "\n";
                    itemData += "----------------------------------------------------------\n";

                    output.Add(itemData);
                }

            }

            File.WriteAllLines(@".\dumps\marketplace.txt", output);
        }

        private static void CreateWaypoint()
        {
            Settings.Overlay.Waypoints.Add(new WaypointObject(true, new int[] { 255, 255, 255, 255 }, 1, Collection.Actors.Local.PlayerData.WorldPosition, "Waypoint_" + Settings.Overlay.Waypoints.Count));
            Xml.Writer.SaveSettings();
        }

        private static void GetHotKeys()
        {
            if (GetKeysPressed(Settings.HotKeys.Speedhack))
            {
                string notifyText = "Proc_ShowMessage_Ack('<PAColor0xFFB69A80>Speedhack:<PAOldColor><PAColor0xFFCC0000>Enabled<PAOldColor>')";


                if (speedHack.globalEnabled)
                {
                    speedHack.RestoreRide();
                    speedHack.ResetPlayerSpeeds();
                    speedHack.ResetAnimation();

                    speedHack.globalEnabled = false;

                    notifyText = "Proc_ShowMessage_Ack('<PAColor0xFFB69A80>Speedhack:<PAOldColor><PAColor0xFFCC0000>Disabled<PAOldColor>')";
                }
                else
                    speedHack.globalEnabled = true;

                Pipe.Call.DoString(notifyText);

                Thread.Sleep(1000);
            }

            if (GetKeysPressed(Settings.HotKeys.Waypoint))
            {
                CreateWaypoint();
                Thread.Sleep(1000);
            }

            if (GetKeysPressed(Settings.HotKeys.Dump))
            {
                DumpToFiles();
                Thread.Sleep(3000);
            }

            if (GetKeysPressed(Settings.HotKeys.Escape))
            {
                Pipe.Call.DoString("callRescue()");
                Thread.Sleep(3000);
            }

            if (GetKeysPressed(Settings.HotKeys.ReloadUi))
            {
                Pipe.Call.DoString("ToClient_excuteReloadUI()");
                Thread.Sleep(3000);
            }

            if (GetKeysPressed(Settings.HotKeys.Minimize))
            {
                Pipe.Call.DoString("ToClient_CheckTrayIcon()");
                Thread.Sleep(3000);
            }

            if (GetKeysPressed(Settings.HotKeys.Reload))
            {
                LoadSettings();
                InitSystemVariables();
                InitModules();
                LoadLuaMod();

                if (Settings.Overlay.Enabled)
                    Overlay.settingsActors = Settings.Overlay.Actors;

                Pipe.Call.DoString("Proc_ShowMessage_Ack('<PAColor0xFFB69A80>..Settings..-..<PAOldColor><PAColor0xFFFFFFFF>Reloaded..<PAOldColor>')");
                Thread.Sleep(500);
            }

            if (GetKeysPressed(Settings.HotKeys.AutoBuy))
            {
                if (autoItemBuy.ItemMarket != null && autoItemBuy.ItemMarket.isVisible())
                {
                    if (_autoItemBuyTrigger)
                    {
                        _autoItemBuyTrigger = false;

                        Pipe.Call.DoString(
                            "Proc_ShowMessage_Ack('<PAColor0xFFB69A80>..AutoItemBuy..-..<PAOldColor><PAColor0xFFFFFFFF>Disabled..<PAOldColor>')");
                    }
                    else
                    {
                        _autoItemBuyTrigger = true;

                        autoItemBuy._reseted = false;

                        Pipe.Call.DoString(
                            "Proc_ShowMessage_Ack('<PAColor0xFFB69A80>..AutoItemBuy..-..<PAOldColor><PAColor0xFFFFFFFF>Enabled..<PAOldColor>')");
                    }
                }

                Thread.Sleep(500);
            }
        }

        private static bool GetKeysPressed(VirtualKeyCode[] keys)
        {
            if (keys.Length == 0)
                return false;

            for (int i = 0; i < keys.Length; i++)
            {
                if (!Collection.Base.Events.InputEventListener.GetKeyState_IsPressed(keys[i]))
                    return false;
            }

            return true;
        }

        private static void InitSystemVariables()
        {
            if (!File.Exists(Files.fileSystemVariables))
            {
                Console.WriteLine("Dumping SystemVariables to systemvariables.xml");
                Xml.Writer.DumpSystemVariables();
            }

            Console.WriteLine("Loading SystemVariables from systemvariables.xml");
            Xml.Reader.LoadSystemVariables();
        }

        private static void InitModules()
        {
            Collector.Collect();

            Console.WriteLine("Initiating Modules");

            fishing = new AutoFish();
            fishing.Enabled = Settings.AutoFish.Enabled;
            fishing.FishDataLog = Settings.AutoFish.FishDataLog;
            fishing.HighLatencyMode = Settings.AutoFish.HighLatencyMode;
            fishing.PredictMode = Settings.AutoFish.PredictMode;
            fishing.catchGrade = Settings.AutoFish.catchGrade;
            fishing.itemIdFilter_White = Settings.AutoFish.itemIdFilter_White;
            fishing.itemIdFilter_Green = Settings.AutoFish.itemIdFilter_Green;
            fishing.itemIdFilter_Blue = Settings.AutoFish.itemIdFilter_Blue;
            fishing.itemIdFilter_Yellow = Settings.AutoFish.itemIdFilter_Yellow;
            fishing.familyNameWhiteList = Settings.AutoFish.familyNameWhiteList;

            workerRestore = new AutoRestore();
            workerRestore.Enabled = Settings.AutoRestore.Enabled;

            hackUi = new UI();
            hackUi.Enabled = Settings.UIHack.Enabled;

            autoPot = new AutoPotion();
            autoPot.Enabled = Settings.AutoPotion.Enabled;
            autoPot.HPPercent = Settings.AutoPotion.HPPercent;
            autoPot.MPPercent = Settings.AutoPotion.MPPercent;

            autoItemRegister = new AutoItemRegister();
            autoItemRegister.Enabled = Settings.AutoItemRegister.Enabled;
            autoItemRegister.Filters = Settings.AutoItemRegister.Items;

            autoItemBuy = new AutoItemBuy();
            autoItemBuy.Enabled = Settings.AutoItemBuy.Enabled;
            autoItemBuy.Filters = Settings.AutoItemBuy.Items;

            speedHack = new SpeedHack();
            speedHack.GhillieMode = Settings.SpeedHack.GhillieMode;
            speedHack.Horse = new SpeedHack.SpeedHackActor(
                Settings.SpeedHack.Horse.Enabled,
                Settings.SpeedHack.Horse.Accel,
                Settings.SpeedHack.Horse.Speed,
                Settings.SpeedHack.Horse.Turn,
                Settings.SpeedHack.Horse.Brake,
                Settings.SpeedHack.Horse.DefaultAccel,
                Settings.SpeedHack.Horse.DefaultSpeed,
                Settings.SpeedHack.Horse.DefaultTurn,
                Settings.SpeedHack.Horse.DefaultBrake);
            speedHack.Ship = new SpeedHack.SpeedHackActor(
                Settings.SpeedHack.Ship.Enabled,
                Settings.SpeedHack.Ship.Accel,
                Settings.SpeedHack.Ship.Speed,
                Settings.SpeedHack.Ship.Turn,
                Settings.SpeedHack.Ship.Brake,
                Settings.SpeedHack.Ship.DefaultAccel,
                Settings.SpeedHack.Ship.DefaultSpeed,
                Settings.SpeedHack.Ship.DefaultTurn,
                Settings.SpeedHack.Ship.DefaultBrake);
            speedHack.Player = new SpeedHack.SpeedHackPlayerActor(
                Settings.SpeedHack.Player.Enabled,
                Settings.SpeedHack.Player.Movement,
                Settings.SpeedHack.Player.Attack,
                Settings.SpeedHack.Player.Cast,
                Settings.SpeedHack.Player.AdvancedMode,
                Settings.SpeedHack.Player.Factor);
            speedHack.familyNameWhiteList = Settings.AutoFish.familyNameWhiteList;

            autoProcessing = new AutoProcessing();
            autoProcessing.Enabled = Settings.AutoProcessing.Enabled;
            autoProcessing.Items = Settings.AutoProcessing.Items;

            hackNavigation = new Navigation();
            hackNavigation.SetFlags();

            if (Overlay != null)
            {
                Overlay.Enabled = Settings.Overlay.Enabled;
                Overlay.settingsActors = Settings.Overlay.Actors;
                Overlay.GoldenChests = Settings.Overlay.GoldenChests;
            }
        }

        private static void LoadLuaMod()
        {
            Console.WriteLine("Attempting to load Lua-Mods");

            if (reloadRequired())
                ReloadUi();

            Thread.Sleep(1000);

            var files = Directory.GetFiles(@".\lua", "*.lua");

            string b = "";

            foreach (var file in files)
            {
                Console.WriteLine("Lua-Mod loading " + Path.GetFileNameWithoutExtension(file) + ".lua");

                string filePath = new FileInfo(file).FullName.Replace(@"\", @"/");
                Pipe.Call.DoString("loadfile([[" + filePath + "]])()");
                Thread.Sleep(500);
            }

        }

        private static bool reloadRequired()
        {
            var dmgMeter = Collection.UI.Base.UIBase.AllPanels.FirstOrDefault(x => x.GetName() == "DmgIcon");

            return dmgMeter != null;
        }

        private static void ReloadUi()
        {
            Console.WriteLine("Lua-Mod Unload required");

            var files = Directory.GetFiles(@".\lua", "*.lua");

            foreach (var file in files)
            {
                var lines = File.ReadAllText(file);

                Match m = Regex.Match(lines, @"Unload(.*)");

                var unloadFunction = m.Groups[0].Value;

                Console.WriteLine(Path.GetFileNameWithoutExtension(file) + ".lua - " + unloadFunction);

                if (unloadFunction != "")
                    Pipe.Call.DoString(unloadFunction.Replace("\r", "").Replace(" ", ""));
            }
        }

    }
}
