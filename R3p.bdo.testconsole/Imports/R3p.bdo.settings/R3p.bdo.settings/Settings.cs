using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.GameExternals.Enums;
using R3p.bdo.GameExternals.Structs.AutoItemBuy;
using R3p.bdo.GameExternals.Structs.AutoItemRegister;
using R3p.bdo.GameExternals.Structs.AutoProcessing;
using R3p.bdo.GameExternals.Structs.Overlay;
using R3p.bdo.GameInternals.Enums;

namespace R3p.bdo.settings
{
    public class Settings
    {
        public class HotKeys
        {
            public static Dictionary<string, string> List = new Dictionary<string, string>()
            {
                {"Reload", "KeyCode_SHIFT,KeyCode_MENU,KeyCode_R"},
                {"AutoBuy", "KeyCode_SHIFT,KeyCode_MENU,KeyCode_B"},
                {"ReloadUi", "KeyCode_SHIFT,KeyCode_MENU,KeyCode_U"},
                {"Minimize", "KeyCode_SHIFT,KeyCode_MENU,KeyCode_M"},
                {"Escape", "KeyCode_SHIFT,KeyCode_MENU,KeyCode_E"},
                {"Dump", "KeyCode_SHIFT,KeyCode_MENU,KeyCode_D"},
                {"Waypoint", "KeyCode_SHIFT,KeyCode_MENU,KeyCode_W"},
                {"Speedhack", "KeyCode_SHIFT,KeyCode_MENU,KeyCode_S"}
            };

            public static VirtualKeyCode[] Reload;
            public static VirtualKeyCode[] AutoBuy;
            public static VirtualKeyCode[] ReloadUi;
            public static VirtualKeyCode[] Minimize;
            public static VirtualKeyCode[] Escape;
            public static VirtualKeyCode[] Dump;
            public static VirtualKeyCode[] Waypoint;
            public static VirtualKeyCode[] Speedhack;

            public static void Transform()
            {
                var reloadSplit = List["Reload"].Split(',');

                Reload = new VirtualKeyCode[reloadSplit.Length];

                for (int i = 0; i < reloadSplit.Length; i++)
                {
                    Reload[i] = (VirtualKeyCode) Enum.Parse(typeof(VirtualKeyCode), reloadSplit[i]);
                }

                Console.WriteLine("Hotkey Reload: " + String.Join(" + ", Reload));

                var autobuySplit = List["AutoBuy"].Split(',');

                AutoBuy = new VirtualKeyCode[autobuySplit.Length];

                for (int i = 0; i < autobuySplit.Length; i++)
                {
                    AutoBuy[i] = (VirtualKeyCode)Enum.Parse(typeof(VirtualKeyCode), autobuySplit[i]);
                }

                Console.WriteLine("Hotkey AutoBuy: " + String.Join(" + ", AutoBuy));

                var reloadUiSplit = List["ReloadUi"].Split(',');

                ReloadUi = new VirtualKeyCode[reloadUiSplit.Length];

                for (int i = 0; i < reloadUiSplit.Length; i++)
                {
                    ReloadUi[i] = (VirtualKeyCode)Enum.Parse(typeof(VirtualKeyCode), reloadUiSplit[i]);
                }

                Console.WriteLine("Hotkey ReloadUi: " + String.Join(" + ", ReloadUi));

                var minimizeSplit = List["Minimize"].Split(',');

                Minimize = new VirtualKeyCode[minimizeSplit.Length];

                for (int i = 0; i < minimizeSplit.Length; i++)
                {
                    Minimize[i] = (VirtualKeyCode)Enum.Parse(typeof(VirtualKeyCode), minimizeSplit[i]);
                }

                Console.WriteLine("Hotkey Minimize: " + String.Join(" + ", Minimize));

                var escapeSplit = List["Escape"].Split(',');

                Escape = new VirtualKeyCode[escapeSplit.Length];

                for (int i = 0; i < escapeSplit.Length; i++)
                {
                    Escape[i] = (VirtualKeyCode)Enum.Parse(typeof(VirtualKeyCode), escapeSplit[i]);
                }

                Console.WriteLine("Hotkey Escape: " + String.Join(" + ", Escape));

                var dumpSplit = List["Dump"].Split(',');

                Dump = new VirtualKeyCode[dumpSplit.Length];

                for (int i = 0; i < dumpSplit.Length; i++)
                {
                    Dump[i] = (VirtualKeyCode)Enum.Parse(typeof(VirtualKeyCode), dumpSplit[i]);
                }

                Console.WriteLine("Hotkey Dump: " + String.Join(" + ", Dump));

                var waypointSplit = List["Waypoint"].Split(',');

                Waypoint = new VirtualKeyCode[waypointSplit.Length];

                for (int i = 0; i < waypointSplit.Length; i++)
                {
                    Waypoint[i] = (VirtualKeyCode)Enum.Parse(typeof(VirtualKeyCode), waypointSplit[i]);
                }

                Console.WriteLine("Hotkey Waypoint: " + String.Join(" + ", Waypoint));

                var speedhackSplit = List["Speedhack"].Split(',');

                Speedhack = new VirtualKeyCode[speedhackSplit.Length];

                for (int i = 0; i < speedhackSplit.Length; i++)
                {
                    Speedhack[i] = (VirtualKeyCode)Enum.Parse(typeof(VirtualKeyCode), speedhackSplit[i]);
                }

                Console.WriteLine("Hotkey Speedhack: " + String.Join(" + ", Speedhack));
            }
        }

        public class AutoFish
        {
            public static Dictionary<string, string> List = new Dictionary<string, string>()
            {
                {"Enabled", "1"},
                {"FishDataLog", "0"},
                {"HighLatencyMode", "0"},
                {"PredictMode", "1"},
                {"CatchGrades", "Green"},
                {"IdFilterWhite", "0"},
                {"IdFilterGreen", "44165, 40218, 43801, 43802, 16382"},
                {"IdFilterBlue", "0"},
                {"IdFilterYellow", "0"},
                {"FamilyNameWhiteList", "-"}
            };

            public static bool Enabled;
            public static bool FishDataLog;
            public static bool HighLatencyMode;
            public static bool PredictMode;
            public static ItemGrade[] catchGrade;
            public static int[] itemIdFilter_White;
            public static int[] itemIdFilter_Green;
            public static int[] itemIdFilter_Blue;
            public static int[] itemIdFilter_Yellow;
            public static string[] familyNameWhiteList;

            public static void Transform()
            {
                Enabled = Convert.ToBoolean(Convert.ToInt32(List["Enabled"]));
                FishDataLog = Convert.ToBoolean(Convert.ToInt32(List["FishDataLog"]));
                HighLatencyMode = Convert.ToBoolean(Convert.ToInt32(List["HighLatencyMode"]));
                PredictMode = Convert.ToBoolean(Convert.ToInt32(List["PredictMode"]));

                List<ItemGrade> grades = new List<ItemGrade>();
                var gradeSplit = List["CatchGrades"].Split(',');

                for (int i = 0; i < gradeSplit.Length; i++)
                {
                    grades.Add((ItemGrade)Enum.Parse(typeof(ItemGrade), gradeSplit[i]));    
                }

                catchGrade = grades.ToArray();

                List<int> filterWhite = new List<int>();
                var whiteSplit = List["IdFilterWhite"].Split(',');

                for (int i = 0; i < whiteSplit.Length; i++)
                {
                    filterWhite.Add(Convert.ToInt32(whiteSplit[i]));
                }

                itemIdFilter_White = filterWhite.ToArray();

                List<int> filterGreen = new List<int>();
                var greenSplit = List["IdFilterGreen"].Split(',');

                for (int i = 0; i < greenSplit.Length; i++)
                {
                    filterGreen.Add(Convert.ToInt32(greenSplit[i]));
                }

                itemIdFilter_Green = filterGreen.ToArray();

                List<int> filterBlue = new List<int>();
                var blueSplit = List["IdFilterBlue"].Split(',');

                for (int i = 0; i < blueSplit.Length; i++)
                {
                    filterBlue.Add(Convert.ToInt32(blueSplit[i]));
                }

                itemIdFilter_Blue = filterBlue.ToArray();

                List<int> filterYellow = new List<int>();
                var yellowSplit = List["IdFilterYellow"].Split(',');

                for (int i = 0; i < yellowSplit.Length; i++)
                {
                    filterYellow.Add(Convert.ToInt32(yellowSplit[i]));
                }

                itemIdFilter_Yellow = filterYellow.ToArray();

                List<string> nameList = new List<string>();
                var nameSplit = List["FamilyNameWhiteList"].Split(',');

                for (int i = 0; i < nameSplit.Length; i++)
                {
                    nameList.Add(nameSplit[i].ToLower());
                }

                familyNameWhiteList = nameList.ToArray();
            }
        }

        public class AutoRestore
        {
            public static Dictionary<string, string> List = new Dictionary<string, string>()
            {
                {"Enabled", "1"}
            };

            public static bool Enabled;

            public static void Transform()
            {
                Enabled = Convert.ToBoolean(Convert.ToInt32(List["Enabled"]));
            }
        }

        public class UIHack
        {
            public static Dictionary<string, string> List = new Dictionary<string, string>()
            {
                {"Enabled", "1"}
            };

            public static bool Enabled;

            public static void Transform()
            {
                Enabled = Convert.ToBoolean(Convert.ToInt32(List["Enabled"]));
            }
        }

        public class SpeedHack
        {
            public static Dictionary<string, string> List = new Dictionary<string, string>()
                {
                    {"GhillieMode", "1"},
                };

            public static bool GhillieMode;

            public class Horse
            {
                public static Dictionary<string, string> List = new Dictionary<string, string>()
                {
                    {"Enabled", "0"},
                    {"Accel", "500"},
                    {"Speed", "500"},
                    {"Turn", "500"},
                    {"Brake", "500"},
                    {"DefaultAccel", "149"},
                    {"DefaultSpeed", "159"},
                    {"DefaultTurn", "145"},
                    {"DefaultBrake", "145"}
                };

                public static bool Enabled;
                public static int Accel;
                public static int Speed;
                public static int Turn;
                public static int Brake;
                public static int DefaultAccel;
                public static int DefaultSpeed;
                public static int DefaultTurn;
                public static int DefaultBrake;
            }

            public class Ship
            {
                public static Dictionary<string, string> List = new Dictionary<string, string>()
                {
                    {"Enabled", "0"},
                    {"Accel", "500"},
                    {"Speed", "500"},
                    {"Turn", "500"},
                    {"Brake", "500"},
                    {"DefaultAccel", "100"},
                    {"DefaultSpeed", "100"},
                    {"DefaultTurn", "100"},
                    {"DefaultBrake", "100"}
                };

                public static bool Enabled;
                public static int Accel;
                public static int Speed;
                public static int Turn;
                public static int Brake;
                public static int DefaultAccel;
                public static int DefaultSpeed;
                public static int DefaultTurn;
                public static int DefaultBrake;
            }

            public class Player
            {
                public static Dictionary<string, string> List = new Dictionary<string, string>()
                {
                    {"Enabled", "0"},
                    {"Movement", "100"},
                    {"Attack", "500"},
                    {"Cast", "500"},
                    {"AdvancedMode", "0"},
                    {"Factor", "20"}
                };

                public static bool Enabled;
                public static int Movement;
                public static int Attack;
                public static int Cast;
                public static bool AdvancedMode;
                public static int Factor;
            }

            public static void Transform()
            {
                SpeedHack.GhillieMode = Convert.ToBoolean(Convert.ToInt32(SpeedHack.List["GhillieMode"]));
                
                Horse.Enabled = Convert.ToBoolean(Convert.ToInt32(Horse.List["Enabled"]));
                Horse.Accel = Convert.ToInt32(Horse.List["Accel"]);
                Horse.Speed = Convert.ToInt32(Horse.List["Speed"]);
                Horse.Turn = Convert.ToInt32(Horse.List["Turn"]);
                Horse.Brake = Convert.ToInt32(Horse.List["Brake"]);
                Horse.DefaultAccel = Convert.ToInt32(Horse.List["DefaultAccel"]);
                Horse.DefaultSpeed = Convert.ToInt32(Horse.List["DefaultSpeed"]);
                Horse.DefaultTurn = Convert.ToInt32(Horse.List["DefaultTurn"]);
                Horse.DefaultBrake = Convert.ToInt32(Horse.List["DefaultBrake"]);

                Ship.Enabled = Convert.ToBoolean(Convert.ToInt32(Ship.List["Enabled"]));
                Ship.Accel = Convert.ToInt32(Ship.List["Accel"]);
                Ship.Speed = Convert.ToInt32(Ship.List["Speed"]);
                Ship.Turn = Convert.ToInt32(Ship.List["Turn"]);
                Ship.Brake = Convert.ToInt32(Ship.List["Brake"]);
                Ship.DefaultAccel = Convert.ToInt32(Ship.List["DefaultAccel"]);
                Ship.DefaultSpeed = Convert.ToInt32(Ship.List["DefaultSpeed"]);
                Ship.DefaultTurn = Convert.ToInt32(Ship.List["DefaultTurn"]);
                Ship.DefaultBrake = Convert.ToInt32(Ship.List["DefaultBrake"]);

                Player.Enabled = Convert.ToBoolean(Convert.ToInt32(Player.List["Enabled"]));
                Player.Movement = Convert.ToInt32(Player.List["Movement"]);
                Player.Attack = Convert.ToInt32(Player.List["Attack"]);
                Player.Cast = Convert.ToInt32(Player.List["Cast"]);
                Player.AdvancedMode = Convert.ToBoolean(Convert.ToInt32(Player.List["AdvancedMode"]));
                Player.Factor = Convert.ToInt32(Player.List["Factor"]);
            }
        }

        public class AutoPotion
        {
            public static Dictionary<string, string> List = new Dictionary<string, string>()
            {
                {"Enabled", "1"},
                {"HP", "80"},
                {"MP", "30"}
            };

            public static bool Enabled;
            public static double HPPercent;
            public static double MPPercent;

            public static void Transform()
            {
                Enabled = Convert.ToBoolean(Convert.ToInt32(List["Enabled"]));
                HPPercent = Convert.ToInt32(List["HP"]);
                MPPercent = Convert.ToInt32(List["MP"]);
            }
        }

        public class AutoItemRegister
        {
            public static Dictionary<string, string> List = new Dictionary<string, string>()
            {
                {"Enabled", "1"}
            };

            public static List<ItemRegisterObject> Items = new List<ItemRegisterObject>()
            {
                new ItemRegisterObject(true, 40218, 0)
            };

            public static bool Enabled;
            
            public static void Transform()
            {
                Enabled = Convert.ToBoolean(Convert.ToInt32(List["Enabled"]));
                
            }
        }

        public class AutoItemBuy
        {
            public static Dictionary<string, string> List = new Dictionary<string, string>()
            {
                {"Enabled", "1"}
            };

            public static List<ItemBuyObject> Items = new List<ItemBuyObject>()
            {
                new ItemBuyObject(true, 9213, 0, 500, 0, true)
            };

            public static bool Enabled;

            public static void Transform()
            {
                Enabled = Convert.ToBoolean(Convert.ToInt32(List["Enabled"]));

            }
        }

        public class Overlay
        {
            public static Dictionary<string, string> List = new Dictionary<string, string>()
            {
                {"Enabled", "1"},
                {"GoldenChests", "0"}
            };

            public static List<ActorObject> Actors = new List<ActorObject>()
            {
                new ActorObject(true, ActorType.ActorType_All, new int[] {0}, new int[] {255,0,0,255}, true, false, 1, true, true, true, true, 0, 0, true, true),
                new ActorObject(false, ActorType.ActorType_Monster, new int[] {0}, new int[] {255,0,0,255}, true, false, 1, false, false, false, false, 0, 0, false, false),
                new ActorObject(false, ActorType.ActorType_Collect, new int[] {0}, new int[] {0,255,0,255}, false, true, 1, false, false, false, false, 0, 0, false, false),
                new ActorObject(false, ActorType.ActorType_Player, new int[] {0}, new int[] {255,0,100,255}, true, false, 1, false, false, false, false, 0, 0, false, false),
                new ActorObject(false, ActorType.ActorType_Collect, new int[] {37082, 37097, 37103, 37105}, new int[] {0,200,150,255}, true, true, 3, false, false, false, false, 0, 0, false, false)
            }; 

            public static List<WaypointObject> Waypoints= new List<WaypointObject>(); 

            public static bool Enabled;
            public static bool GoldenChests;
            
            public static void Transform()
            {
                Enabled = Convert.ToBoolean(Convert.ToInt32(List["Enabled"]));
                GoldenChests = Convert.ToBoolean(Convert.ToInt32(List["GoldenChests"]));
            }
        }

        public class AutoProcessing
        {
            public static Dictionary<string, string> List = new Dictionary<string, string>()
            {
                {"Enabled", "0"}
            };

            public static List<ProcessingObject> Items = new List<ProcessingObject>()
            {
                new ProcessingObject(true, 4601, ProcessingType.Chopping, 5, new int[] {4651, 4652}, "Ash Timber"),
                new ProcessingObject(true, 4602, ProcessingType.Chopping, 5, new int[] {4654, 4655}, "Maple Timber"),
                new ProcessingObject(true, 4603, ProcessingType.Chopping, 5, new int[] {4657, 4658}, "Pine Timber"),
                new ProcessingObject(true, 4604, ProcessingType.Chopping, 5, new int[] {4660, 4661}, "Birch Timber"),
                new ProcessingObject(true, 4605, ProcessingType.Chopping, 10, new int[] { 249, 4685}, "Log"),
                new ProcessingObject(true, 4606, ProcessingType.Chopping, 5, new int[] {4663, 4664}, "Fir Timber"),
                new ProcessingObject(true, 4607, ProcessingType.Chopping, 5, new int[] {4666, 4667}, "Cedar Timber"),
                new ProcessingObject(true, 4608, ProcessingType.Chopping, 5, new int[] {4676, 4677}, "White Cedar Timber"),
                new ProcessingObject(true, 4609, ProcessingType.Chopping, 5, new int[] {4680, 4681}, "Acacia Timber"),
                new ProcessingObject(true, 4610, ProcessingType.Chopping, 5, new int[] {4670, 4671}, "Palm Timber"),
                new ProcessingObject(true, 4611, ProcessingType.Chopping, 5, new int[] {4673, 4674}, "Elder Tree Timber"),

                new ProcessingObject(true, 4651, ProcessingType.Chopping, 10, new int[] {4652}, "Ash Plank"),
                new ProcessingObject(true, 4654, ProcessingType.Chopping, 10, new int[] {4655}, "Maple Plank"),
                new ProcessingObject(true, 4657, ProcessingType.Chopping, 10, new int[] {4658}, "Pine Plank"),
                new ProcessingObject(true, 4660, ProcessingType.Chopping, 10, new int[] {4661}, "Birch Plank"),
                new ProcessingObject(false, 249, ProcessingType.Chopping, 10, new int[] { 4685}, "Usable Scantling"),
                new ProcessingObject(true, 4663, ProcessingType.Chopping, 10, new int[] {4664}, "Fir Plank"),
                new ProcessingObject(true, 4666, ProcessingType.Chopping, 10, new int[] {4667}, "Cedar Plank"),
                new ProcessingObject(true, 4676, ProcessingType.Chopping, 10, new int[] {4677}, "White Cedar Plank"),
                new ProcessingObject(true, 4680, ProcessingType.Chopping, 10, new int[] {4681}, "Acacia Plank"),
                new ProcessingObject(true, 4670, ProcessingType.Chopping, 10, new int[] {4671}, "Palm Plank"),
                new ProcessingObject(true, 4673, ProcessingType.Chopping, 10, new int[] {4674}, "Elder Tree Plank"),
            };

            public static bool Enabled;

            public static void Transform()
            {
                Enabled = Convert.ToBoolean(Convert.ToInt32(List["Enabled"]));
            }
        }
        
    }
}
