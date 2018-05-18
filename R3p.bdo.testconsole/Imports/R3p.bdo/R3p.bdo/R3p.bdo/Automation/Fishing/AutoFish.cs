using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using R3p.bdo.Automation.Hacks;
using R3p.bdo.GameInternals.Enums;
using R3p.bdo.GameInternals.Structs.Actors;
using R3p.bdo.GameInternals.Structs.Loot;
using R3p.bdo.GameInternals.Structs.UI;
using R3p.bdo.Memory;

namespace R3p.bdo
{
    public class AutoFish
    {
        public static AutoFish Instance;

        public bool Enabled { get; set; }
        public bool PredictMode { get; set; }
        public bool HighLatencyMode { get; set; }

        public bool FishDataLog { get; set; }

        public ItemGrade[] catchGrade { get; set; }
        public int[] itemIdFilter_White { get; set; }
        public int[] itemIdFilter_Green { get; set; }
        public int[] itemIdFilter_Blue { get; set; }
        public int[] itemIdFilter_Yellow { get; set; }
        
        public string[] familyNameWhiteList { get; set; }

        private EquipmentItem MainHand { get; set; }
        private bool noMoreRodsToSwitch { get; set; }
        
        public AutoFish()
        {
            Instance = this;

            PatchItemSwapCooldown();

            noMoreRodsToSwitch = false;

            MainHand = Collection.Actors.Local.PlayerData.GetEquipmentItem(EquipSlotNo.rightHand);
            
            Collection.Fishing.Log.startTime = DateTime.Now;
            Collection.Fishing.Log._catches = 0;
            Collection.Fishing.Log._switches = 0;
            Collection.Fishing.Log._throws = 0;
            Collection.Fishing.Log.looted_Blue = new Dictionary<int, int>();
            Collection.Fishing.Log.looted_Green = new Dictionary<int, int>();
            Collection.Fishing.Log.looted_White = new Dictionary<int, int>();
            Collection.Fishing.Log.looted_Yellow = new Dictionary<int, int>();
            Collection.Fishing.Log.CatchTimes = new List<TimeSpan>();
            Collection.Fishing.Log.FishGrades = new List<ItemGrade>();
        }

        private UIControl sinGaugePanel;
        private UIControl sinGaugeBar;

        //private int failCount = 0;
        private bool otherPlayersAround = false;

        private bool _stopFishing = false;

        private float[] _totalMinPos;
        private float[] _totalMaxPos;
        private float _totalMinRotation;
        private float _totalMaxRotation;

        private int _durabilityLeft;

        public bool isFishing;

        //public bool waitingForLoot = false;

        private void PatchItemSwapCooldown()
        {
            //if (MemoryReader.ReadByte(Offsets._itemSwapCooldown) == 0x75)
            //{
            //    MemoryWriter.Write(Offsets._itemSwapCooldown, BitConverter.GetBytes((byte) 0xEB));
            //    MemoryWriter.Write(Offsets._itemSwapCooldown + 1, BitConverter.GetBytes((byte)0x14));
            //}
        }

        public void Run(bool requestReload)
        {
            if (requestReload)
                _stopFishing = true;

            //if (_stopFishing)
            //    return;

            if (Enabled)
            {
                if (getFreeSlots() == 0)
                {
                    return;
                }

                if (!isFishingRodEquipped())
                {
                    //_totalMaxPos = null;
                    //_totalMinPos = null;
                    //_totalMaxRotation = 0;
                    //_totalMinRotation = 0;

                    isFishing = false;

                    if (SpeedHack.Instance.hackedAnimation)
                        return;

                    if (
                            !Collection.Actors.Local.PlayerData.CharacterControl.CharacterScene.AnimationSpeed.Equals(
                                1.0f))
                        Collection.Actors.Local.PlayerData.CharacterControl.CharacterScene.SetAnimationSpeed(1.0f);

                    return;
                }
                else
                {
                    isFishing = true;

                    if (!HighLatencyMode && !otherPlayersNearby() && !_stopFishing)
                    {
                        if (
                            !Collection.Actors.Local.PlayerData.CharacterControl.CharacterScene.AnimationSpeed.Equals(
                                10.0f))
                            Collection.Actors.Local.PlayerData.CharacterControl.CharacterScene.SetAnimationSpeed(10.0f);
                    }
                    else
                        if (
                            !Collection.Actors.Local.PlayerData.CharacterControl.CharacterScene.AnimationSpeed.Equals(
                                1.0f))
                        Collection.Actors.Local.PlayerData.CharacterControl.CharacterScene.SetAnimationSpeed(1.0f);
                }
                
                if (isFishingRodEquipped() && getFishingRodDurability() == 0 && !_stopFishing)
                {
                    Log.Post("Rod Durability = 0, Switching Rod",LogModule.AutoFish);
                    SwitchRod();
                }

                if (PredictMode && !otherPlayersNearby() && !_stopFishing)
                {

                    if (HighLatencyMode)
                    {
                        if (Animation_Fishing_CatchingWait())
                        {
                            if (!isNextFishDesiredGrade())
                            {
                                SwitchRod();
                            }
                        }
                    }
                    else
                    {
                        if (Animation_Fishing_Start3() || Animation_Fishing_CatchingWait())
                        {
                            if (!isNextFishDesiredGrade())
                            {
                                SwitchRod();
                            }
                        }
                    }
                }

                DeleteNonRepairableRods();
                //getPositonData();
                checkSafety();

                if (!_stopFishing)
                {
                    //if(!waitingForLoot)
                    ThrowRod();

                    Catch();
                    
                    Loot();
                }
            }
        }
        
        private void checkSafety()
        {
            //if (!otherPlayersAround)
            //    return;

            if (Collection.Actors.Local.PlayerData.PercentageCurHitpoints < 90)
            {
                Console.Clear();
                Console.WriteLine("Hitpoints < 90%, Stopped Fishing");

                Log.Post("Hitpoints < 90%, Stopped Fishing", LogModule.AutoFish);
                _stopFishing = true;
                return;
            }

            if (isTeleporting())
            {
                Console.Clear();
                Console.WriteLine("Teleport detected, Stopped Fishing");

                Log.Post("Teleport detected, Stopped Fishing", LogModule.AutoFish);
                _stopFishing = true;
                return;
            }

            var questionPanel =
                Collection.UI.Base.UIBase.AllPanels.FirstOrDefault(x => x.GetName() == "Panel_Window_MacroCheckQuiz");

            if (questionPanel != null && questionPanel.isVisible())
            {
                Console.Clear();
                Console.WriteLine("GM QuestionPanel detected, Stopped Fishing");

                Log.Post("GM QuestionPanel detected, Stopped Fishing", LogModule.AutoFish);
                _stopFishing = true;
                return;
            }

            //if (isOrdinaryMovement())
            //{
            //    Log.Post("Ordinary Movement Detected, Stopped Fishing", LogModule.AutoFish);
            //    _stopFishing = true;
            //}

        }

        private bool isTeleporting()
        {
            if (!Collection.Actors.Local.PlayerData.isReadyToPlay)
                return true;

            return false;
        }

        private bool isOrdinaryMovement()
        {
            if (Math.Abs(Collection.Actors.Local.PlayerData.WorldPosition[0]) > _totalMaxPos[0] ||
                Math.Abs(Collection.Actors.Local.PlayerData.WorldPosition[0]) < _totalMinPos[0])
                return true;

            if (Math.Abs(Collection.Actors.Local.PlayerData.WorldPosition[1]) > _totalMaxPos[1] ||
                         Math.Abs(Collection.Actors.Local.PlayerData.WorldPosition[1]) < _totalMinPos[1])
                return true;

            if (Math.Abs(Collection.Actors.Local.PlayerData.WorldPosition[2]) > _totalMaxPos[2] ||
                         Math.Abs(Collection.Actors.Local.PlayerData.WorldPosition[2]) < _totalMinPos[2])
                return true;

            if (Math.Abs(Collection.Actors.Local.PlayerData.CharacterControl.CharacterScene.Rotation) > _totalMaxRotation ||
                         Math.Abs(Collection.Actors.Local.PlayerData.CharacterControl.CharacterScene.Rotation) < _totalMinRotation)
                return true;

            return false;
        }

        private void getPositonData()
        {
            if (_totalMinPos == null)
            {
                _totalMinPos = new float[]{ Math.Abs(Collection.Actors.Local.PlayerData.WorldPosition[0]) * 0.80f, Math.Abs(Collection.Actors.Local.PlayerData.WorldPosition[1]) * 0.80f, Math.Abs(Collection.Actors.Local.PlayerData.WorldPosition[2]) * 0.80f};
                _totalMaxPos = new float[] { Math.Abs(Collection.Actors.Local.PlayerData.WorldPosition[0]) * 1.20f, Math.Abs(Collection.Actors.Local.PlayerData.WorldPosition[1]) * 1.20f, Math.Abs(Collection.Actors.Local.PlayerData.WorldPosition[2]) * 1.20f };
                _totalMinRotation = Math.Abs(Collection.Actors.Local.PlayerData.CharacterControl.CharacterScene.Rotation) * 0.80f;
                _totalMaxRotation = Math.Abs(Collection.Actors.Local.PlayerData.CharacterControl.CharacterScene.Rotation) * 1.20f;
            }

            //if (otherPlayersAround)
            //    return;

            //if (Collection.Actors.Local.PlayerData.WorldPosition[0] > _totalMaxPos[0])
            //    _totalMaxPos[0] = Collection.Actors.Local.PlayerData.WorldPosition[0];
            //if (Collection.Actors.Local.PlayerData.WorldPosition[0] < _totalMinPos[0])
            //    _totalMinPos[0] = Collection.Actors.Local.PlayerData.WorldPosition[0];

            //if (Collection.Actors.Local.PlayerData.WorldPosition[1] > _totalMaxPos[1])
            //    _totalMaxPos[1] = Collection.Actors.Local.PlayerData.WorldPosition[1];
            //if (Collection.Actors.Local.PlayerData.WorldPosition[1] < _totalMinPos[1])
            //    _totalMinPos[1] = Collection.Actors.Local.PlayerData.WorldPosition[1];

            //if (Collection.Actors.Local.PlayerData.WorldPosition[2] > _totalMaxPos[2])
            //    _totalMaxPos[2] = Collection.Actors.Local.PlayerData.WorldPosition[2];
            //if (Collection.Actors.Local.PlayerData.WorldPosition[2] < _totalMinPos[2])
            //    _totalMinPos[2] = Collection.Actors.Local.PlayerData.WorldPosition[2];

            //if (Collection.Actors.Local.PlayerData.CharacterControl.CharacterScene.Rotation > _totalMaxRotation)
            //    _totalMaxRotation = Collection.Actors.Local.PlayerData.CharacterControl.CharacterScene.Rotation;
            //if (Collection.Actors.Local.PlayerData.CharacterControl.CharacterScene.Rotation < _totalMinRotation)
            //    _totalMinRotation = Collection.Actors.Local.PlayerData.CharacterControl.CharacterScene.Rotation;
        }

        private bool isNextFishDesiredGrade()
        {
            return catchGrade.Contains(getNextFishGrade());
        }

        private ItemGrade getNextFishGrade()
        {
            return Collection.Actors.Local.PlayerData.FishGrade;
        }

        private bool otherPlayersNearby()
        {
            bool state = Collection.Actors.Global.ActorList.Count(
                    x =>
                        x.ActorType == ActorType.ActorType_Player &&
                        x.ActorKey != Collection.Actors.Local.PlayerData.ActorKey &&
                        !familyNameWhiteList.Contains(x.FamilyName.ToLower())) != 0;

            otherPlayersAround = state;

            return state;

        }

        private bool isFishingRodEquipped()
        {
            return Collection.Fishing.Rods.List.Contains((int)MainHand.ItemData.ItemIndex);
        }

        private int getFishingRodDurability()
        {
            return MainHand.Durability;
        }

        private double GetFishingResources()
        {
            return Collection.Actors.Local.PlayerData.CurrentRegion.x0110_RegionInfo.x0048_FishingProductivityPercentage;
        }

        private bool canLoot()
        {
            return Collection.Loot.Base.Loot.PanelLooting.isVisible() && Collection.Loot.Base.Loot.LootItems.Count != 0;
        }

        private int getFreeSlots()
        {
            return Collection.Actors.Local.PlayerData.x19B8_FreeInventorySlots;
        }

        private long getNextBite_Ticks()
        {
            return Collection.Actors.Local.PlayerData.NextFishBite - Collection.Base.Ticks.BaseTicks.BaseTick;
        }

        private bool Animation_Fishing_Idle()
        {
            return Collection.Actors.Local.PlayerData.CharacterControl.CurrentAnimation.Animation == CharacterAnimation.Idle;
        }

        private bool Animation_Fishing_Start1()
        {
            return Collection.Actors.Local.PlayerData.CharacterControl.CurrentAnimation.Animation == CharacterAnimation.Fishing_Start1;
        }

        private bool Animation_Fishing_Start2()
        {
            return Collection.Actors.Local.PlayerData.CharacterControl.CurrentAnimation.Animation == CharacterAnimation.Fishing_Start2;
        }

        private bool Animation_Fishing_Start3()
        {
            return Collection.Actors.Local.PlayerData.CharacterControl.CurrentAnimation.Animation == CharacterAnimation.Fishing_Start3;
        }

        private bool Animation_Fishing_CatchingWait()
        {
            return Collection.Actors.Local.PlayerData.CharacterControl.CurrentAnimation.Animation == CharacterAnimation.Fishing_CatchingWait;
        }

        private bool Animation_Fishing_HookReady()
        {
            return Collection.Actors.Local.PlayerData.CharacterControl.CurrentAnimation.Animation == CharacterAnimation.Fishing_HookReady;
        }

        private bool Animation_Fishing_Hook()
        {
            return Collection.Actors.Local.PlayerData.CharacterControl.CurrentAnimation.Animation == CharacterAnimation.Fishing_Hook;
        }

        private bool Animation_Fishing_Fail()
        {
            return Collection.Actors.Local.PlayerData.CharacterControl.CurrentAnimation.Animation == CharacterAnimation.Fishing_Impossible;
        }

        private void ThrowRod()
        {
            if (Animation_Fishing_Idle())
            {
                //Collection.Actors.Local.PlayerData.CharacterControl.CharacterScene.SetAnimationSpeed(1.0f);

                while (Collection.Actors.Local.PlayerData.NextFishBite != 0)
                {
                    Collection.Actors.Local.PlayerData.Write(ActorData.Offsets.ONextFishBite,
                        BitConverter.GetBytes((long) 0));

                    Thread.Sleep(1);
                }

                DateTime start = DateTime.Now;

                //Log.Post("Throwing Rod", LogModule.AutoFish);

                while (Animation_Fishing_Idle())
                {
                    Collection.Base.Events.InputEventListener.ChangeKeyState_KeyDown(VirtualKeyCode.KeyCode_SPACE);
                }
                Collection.Base.Events.InputEventListener.ChangeKeyState_KeyUp(VirtualKeyCode.KeyCode_SPACE);


                var newTime = (getNextBite_Ticks() / 1000);

                while (newTime <= 0)
                {
                    if (start.AddSeconds(3) <= DateTime.Now)
                        break;

                    newTime = (getNextBite_Ticks() / 1000);

                    Thread.Sleep(1);
                }
                
                LogFishData(DateTime.Now + "\t" + "Throw\t" + String.Join("\t", Collection.Actors.Local.PlayerData.WorldPosition) + "\t" + getNextFishGrade() + "\t" + newTime + "sec" + "\t" + GetFishingResources().ToString("0.0") + "%");

                Collection.Fishing.Log.CatchTimes.Add(TimeSpan.FromSeconds(newTime));
                Collection.Fishing.Log.FishGrades.Add(getNextFishGrade());
                Collection.Fishing.Log._throws++;
                Collection.Fishing.Log.updated = true;

                //Console.WriteLine("Initiated Throw");

                //Thread.Sleep(2000);
            }
        }

        Random rng = new Random();
        private long nextRandomCatch;
        private bool newRandomCatch;

        private void Catch()
        {
            if (Collection.Actors.Local.PlayerData.isFishHooked)
            {
                if (!newRandomCatch && otherPlayersAround)
                {
                    Log.Post("New Random Catch, Players nearby", LogModule.AutoFish);

                    var nextafkcatch = Collection.Actors.Local.PlayerData.NextAutoFishAutoCatch;
                    var nextbite = Collection.Actors.Local.PlayerData.NextFishBite;
                    var dif = (nextafkcatch - nextbite)/1000;

                    nextRandomCatch = rng.Next((int)(dif*0.8),(int)(dif*0.95)) * 1000;

                    newRandomCatch = true;
                }

                if (Collection.Base.Ticks.BaseTicks.BaseTick < Collection.Actors.Local.PlayerData.NextFishBite + nextRandomCatch && otherPlayersAround)
                    return;

                Log.Post("Catching Fish", LogModule.AutoFish);

                //Collection.Actors.Local.PlayerData.CharacterControl.CharacterScene.SetAnimationSpeed(1.0f);

                Collection.Base.Events.InputEventListener.ChangeKeyState_PressOnce(VirtualKeyCode.KeyCode_SPACE);

                //DateTime start = DateTime.Now;

                //while (!Collection.Loot.Base.Loot.PanelLooting.isVisible())
                //{
                //    if (start.AddSeconds(3) <= DateTime.Now)
                //        break;

                //    Thread.Sleep(1);
                //}

                //Collection.Actors.Local.PlayerData.CharacterControl.CharacterScene.SetAnimationSpeed(5.0f);

                Collection.Fishing.Log._catches++;

                looted = false;
                //waitingForLoot = true;
                newRandomCatch = false;

                //failCount = 0;

               // Console.WriteLine("Initiated Catch\t" + getNextFishGrade());
            }
        }

        private long GetCurrentMainHandId()
        {
            return Collection.Actors.Local.PlayerData.ReadInt64(ActorData.Offsets.OMainHand);
        }

        private void DeleteNonRepairableRods()
        {
            var rods =
                Collection.Actors.Local.PlayerData.Inventory.Items.Where(
                    x => x.isFishingRod() && x.x0018_CurrentDurability == 0 && !x.isRepairable() && x.x0000_PTR_ItemData.EnchantLevel == 0);

            foreach (var rod in rods)
            {
                rod.DeleteItem(rod.x0008_ItemCount);
                Log.Post("Rod deleted (itemId " + rod.x0000_PTR_ItemData.ItemIndex + ", enchantLevel " + rod.x0000_PTR_ItemData.EnchantLevel + ", slot " + rod.SlotNo + ", count " + rod.x0008_ItemCount + ")", LogModule.AutoFish);
            }
        }

        private DateTime lastUse;

        private void SwitchRod()
        {
            var rods =
                Collection.Actors.Local.PlayerData.Inventory.Items.Where(
                    x => x.isFishingRod() && x.x0018_CurrentDurability > 0);

            _durabilityLeft = rods.Sum(x => (int) x.x0018_CurrentDurability) + MainHand.Durability;

            if (rods.Count() == 0)
            {
                //noMoreRodsToSwitch = true;
                return;
            }

            var rod = rods.FirstOrDefault();

            if (rod == null)
                return;

            //if (lastUse.AddMilliseconds(700) <= DateTime.Now)
            //{
                long lastMainHandId = GetCurrentMainHandId();

                rod.UseItem(0);

                DateTime start = DateTime.Now;

                while (GetCurrentMainHandId() == lastMainHandId)
                {
                    if (start.AddSeconds(1) <= DateTime.Now)
                        break;

                    Thread.Sleep(10);
                }

                Collection.Actors.Local.PlayerData.Write(ActorData.Offsets.OIsFishing, BitConverter.GetBytes((byte) 0));

                lastUse = DateTime.Now;

                Collection.Fishing.Log._switches++;
                
                //Log.Post("Switched Rod", LogModule.AutoFish);

                //Console.WriteLine("Switched Rod\t" + getNextFishGrade());
            //}

        }

        private bool looted = false;

        private void Loot()
        {
            if (canLoot() && !looted)
            {
                foreach (var item in Collection.Loot.Base.Loot.LootItems)
                {
                    LogFishData(DateTime.Now + "\t" + "Item\t" + String.Join("\t", Collection.Actors.Local.PlayerData.WorldPosition) + "\t" + item.ItemData.ItemIndex + "\t" + item.ItemData.Name);

                    if (catchGrade.Contains((ItemGrade) item.ItemData.GradeType))
                        if (shouldLoot(item))
                        {
                            addLootLog(item);

                            item.Pickup();

                            Log.Post("Looted ItemId(" + item.ItemData.ItemIndex + ")", LogModule.AutoFish);

                            //Console.WriteLine("Looted");
                        }

                }

                looted = true;
                //waitingForLoot = false;

                Thread.Sleep(500);
            }
        }

        private void addLootLog(LootItem item)
        {
            switch (item.ItemData.GradeType)
            {
                case 0:
                    if (!Collection.Fishing.Log.looted_White.ContainsKey((int) item.ItemData.ItemIndex))
                        Collection.Fishing.Log.looted_White.Add((int) item.ItemData.ItemIndex, 1);
                    else
                    {
                        Collection.Fishing.Log.looted_White[(int) item.ItemData.ItemIndex] += 1;
                    }

                    Collection.Fishing.Log.updated = true;
                    break;

                case 1:
                    if (!Collection.Fishing.Log.looted_Green.ContainsKey((int)item.ItemData.ItemIndex))
                        Collection.Fishing.Log.looted_Green.Add((int)item.ItemData.ItemIndex, 1);
                    else
                    {
                        Collection.Fishing.Log.looted_Green[(int)item.ItemData.ItemIndex] += 1;
                    }

                    Collection.Fishing.Log.updated = true;
                    break;

                case 2:
                    if (!Collection.Fishing.Log.looted_Blue.ContainsKey((int)item.ItemData.ItemIndex))
                        Collection.Fishing.Log.looted_Blue.Add((int)item.ItemData.ItemIndex, 1);
                    else
                    {
                        Collection.Fishing.Log.looted_Blue[(int)item.ItemData.ItemIndex] += 1;
                    }

                    Collection.Fishing.Log.updated = true;
                    break;

                case 3:
                    if (!Collection.Fishing.Log.looted_Yellow.ContainsKey((int)item.ItemData.ItemIndex))
                        Collection.Fishing.Log.looted_Yellow.Add((int)item.ItemData.ItemIndex, 1);
                    else
                    {
                        Collection.Fishing.Log.looted_Yellow[(int)item.ItemData.ItemIndex] += 1;
                    }

                    Collection.Fishing.Log.updated = true;
                    break;
            }
        }

        private bool shouldLoot(LootItem item)
        {
            if (item.ItemData.GradeType == 0)
                if (itemIdFilter_White[0] == 0)
                    return true;
                else
                {
                    
                    return itemIdFilter_White.Contains((int) item.ItemData.ItemIndex);
                }

            if (item.ItemData.GradeType == 1)
                if (itemIdFilter_Green[0] == 0)
                    return true;
                else
                {
                    return itemIdFilter_Green.Contains((int)item.ItemData.ItemIndex);
                }

            if (item.ItemData.GradeType == 2)
                if (itemIdFilter_Blue[0] == 0)
                    return true;
                else
                {
                    return itemIdFilter_Blue.Contains((int)item.ItemData.ItemIndex);
                }

            if (item.ItemData.GradeType == 3)
                if (itemIdFilter_Yellow[0] == 0)
                    return true;
                else
                {
                    return itemIdFilter_Yellow.Contains((int)item.ItemData.ItemIndex);
                }

            return false;
        }

        private void LogFishData(string text)
        {
            if (!FishDataLog)
                return;

           File.AppendAllText(@".\fishdata.txt", text + "\n");
        }

        public void PostStats()
        {
            if (!isFishing)
                return;

                var newTime = (getNextBite_Ticks()/1000);

            string timeString = newTime.ToString();
            if (newTime < 0)
                timeString = "--";

            Console.Title = "R3p.bdo.autoFish - - Next Catch in " + newTime + "sec - - Resources " + GetFishingResources().ToString("0.0") + "%";//Next Fishgrad " + getNextFishGrade();// + " - - FailCount " + failCount;
           
            if (Collection.Fishing.Log.updated)
            {
                if (Collection.Fishing.Log.CatchTimes.Count == 0)
                    return;

                Console.Clear();

                TimeSpan duration = new TimeSpan(DateTime.Now.Ticks - Collection.Fishing.Log.startTime.Ticks);

                double totalGrades = Collection.Fishing.Log.FishGrades.Count;
                double totalWhite = Collection.Fishing.Log.FishGrades.Count(x => x == ItemGrade.White);
                double totalGreen = Collection.Fishing.Log.FishGrades.Count(x => x == ItemGrade.Green);
                double totalBlue = Collection.Fishing.Log.FishGrades.Count(x => x == ItemGrade.Blue);
                double totalYellow = Collection.Fishing.Log.FishGrades.Count(x => x == ItemGrade.Yellow);

                Console.WriteLine("Duration\t{0}:{1}:{2}\nUnusedDurability\t{3}\nFree InventorySlots\t{4}\n", duration.Hours, duration.Minutes, duration.Seconds, _durabilityLeft, Collection.Actors.Local.PlayerData.x19B8_FreeInventorySlots);
                Console.WriteLine("Throws: {0}\tMinCatch: {1}sec", Collection.Fishing.Log._throws, Collection.Fishing.Log.CatchTimes.Min(x => x.TotalSeconds).ToString("0"));
                Console.WriteLine("Catches: {0}\tAvgCatch: {1}sec", Collection.Fishing.Log._catches, Collection.Fishing.Log.CatchTimes.Average(x => x.TotalSeconds).ToString("0"));
                Console.WriteLine("Switches: {0}\tMaxCatch: {1}sec\n", Collection.Fishing.Log._switches, Collection.Fishing.Log.CatchTimes.Max(x => x.TotalSeconds).ToString("0"));

                Console.WriteLine("White {0}({1}%)", totalWhite, ((totalWhite/totalGrades)*100).ToString("0.0"));
                Console.WriteLine("Green {0}({1}%)", totalGreen, ((totalGreen / totalGrades) * 100).ToString("0.0"));
                Console.WriteLine("Blue {0}({1}%)", totalBlue, ((totalBlue / totalGrades) * 100).ToString("0.0"));
                Console.WriteLine("Yellow {0}({1}%)\n", totalYellow, ((totalYellow / totalGrades) * 100).ToString("0.0"));

                Console.WriteLine("{0}\t{1}\t{2}\t{3}\n", "Grade", "ItemId", "Count", "Items/Hour");

                if (Collection.Fishing.Log.looted_White.Count > 0)
                {
                    Console.WriteLine("{0}\t{1}", "Total", Collection.Fishing.Log.looted_White.Sum(x => x.Value));
                    foreach (var item in Collection.Fishing.Log.looted_White)
                    {
                        Console.WriteLine("{0}\t{1}\t{2}\t{3}", "White", item.Key, item.Value,
                            (item.Value/duration.TotalHours).ToString("0.0"));
                    }

                    Console.WriteLine("\n");
                }

                if (Collection.Fishing.Log.looted_Green.Count > 0)
                {
                    Console.WriteLine("{0}\t{1}", "Total", Collection.Fishing.Log.looted_Green.Sum(x => x.Value));
                    foreach (var item in Collection.Fishing.Log.looted_Green)
                    {
                        Console.WriteLine("{0}\t{1}\t{2}\t{3}", "Green", item.Key, item.Value,
                            (item.Value/duration.TotalHours).ToString("0.0"));
                    }

                    Console.WriteLine("\n");
                }

                if (Collection.Fishing.Log.looted_Blue.Count > 0)
                {
                    Console.WriteLine("{0}\t{1}", "Total", Collection.Fishing.Log.looted_Blue.Sum(x => x.Value));
                    foreach (var item in Collection.Fishing.Log.looted_Blue)
                    {
                        Console.WriteLine("{0}\t{1}\t{2}\t{3}", "Blue", item.Key, item.Value,
                            (item.Value/duration.TotalHours).ToString("0.0"));
                    }

                    Console.WriteLine("\n");
                }

                if (Collection.Fishing.Log.looted_Yellow.Count > 0)
                {
                    Console.WriteLine("{0}\t{1}", "Total", Collection.Fishing.Log.looted_Yellow.Sum(x => x.Value));
                    foreach (var item in Collection.Fishing.Log.looted_Yellow)
                    {
                        Console.WriteLine("{0}\t{1}\t{2}\t{3}", "Yellow", item.Key, item.Value,
                            (item.Value/duration.TotalHours).ToString("0.0"));
                    }

                    Console.WriteLine("\n");
                }

                Collection.Fishing.Log.updated = false;
            }
        }
        
    }
}
