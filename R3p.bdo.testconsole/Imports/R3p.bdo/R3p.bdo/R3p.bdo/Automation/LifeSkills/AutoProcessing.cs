using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using R3p.bdo.GameExternals.Enums;
using R3p.bdo.GameExternals.Structs.AutoProcessing;
using R3p.bdo.GameInternals.Enums;
using R3p.bdo.GameInternals.Structs.Actors;
using R3p.bdo.GameInternals.Structs.UI;
using R3p.bdo.GameInternals.Structs.Warehouse;
using R3p.bdo.Memory;

namespace R3p.bdo.Automation.LifeSkills
{
    public class AutoProcessing
    {
        public bool Enabled { get; set; }
        public List<ProcessingObject> Items { get; set; }

        private EquipmentItem Chest { get; set; }
        private ProcessingObject CurrentProcessingItem;
        private int SlotNo;

        private UIControl Panel_Manufacture;

        public bool isProcessing = false;
        public bool init = false;
        private bool clear = false;

        private DateTime _lastCheck;
        private DateTime _nextRun;

        public AutoProcessing()
        {
            Chest = Collection.Actors.Local.PlayerData.GetEquipmentItem(EquipSlotNo.chest);
            Panel_Manufacture =
                Collection.UI.Base.UIBase.AllPanels.FirstOrDefault(x => x.GetName().ToLower() == "panel_manufacture");

            _lastCheck = DateTime.Now;
            _nextRun = DateTime.Now;
        }
        
        public void Run()
        {
            if (!Enabled)
                return;
            
            if (!canProcess())
                return;

            if (init && !isWarehouseInRange())
            {
                init = false;
                isProcessing = false;
                CurrentProcessingItem = null;
                _nextRun = DateTime.Now;
            }


            if (!clear && init)
                clear = true;

            if (DateTime.Now < _nextRun)
                return;

            if (!init && isWarehouseInRange())
            {
                if (Panel_Manufacture.isVisible())
                {
                    Log.Post("Init", LogModule.AutoProcessing);

                    Thread.Sleep(1000);

                    init = true;
                    clear = false;

                    //Collection.Actors.Local.PlayerData.doAction("WAIT");
                    //Thread.Sleep(200);
                }
            }
            
            if (init)
            {
                if (DateTime.Now <= _lastCheck.AddSeconds(1))
                    return;

                _lastCheck = DateTime.Now;

                if (isIdleAnimation() && !isOverWeight() && CurrentProcessingItem == null && !isProcessing)
                {
                    if (isWarehouseInRange())
                    {
                        Log.Post("Start Processing", LogModule.AutoProcessing);
                        doProcessing();
                    }
                }

                else if (isIdleAnimation() && isOverWeight() && CurrentProcessingItem != null && isProcessing)
                {
                    if (isWarehouseInRange())
                    {
                        Log.Post("Start ItemTransfer - Overweight", LogModule.AutoProcessing);
                        doItemTransfer();
                        _nextRun = DateTime.Now.AddSeconds(Helpers.Timers.RandomTimes.GetRandomSeconds(120,600));

                        Log.Post("Set new NextRun at " + _nextRun, LogModule.AutoProcessing);
                    }
                }

                else if (isIdleAnimation() && !isOverWeight() && CurrentProcessingItem != null && isProcessing)
                {
                    if (isWarehouseInRange())
                    {
                        Log.Post("Start ItemTransfer - Processing Done", LogModule.AutoProcessing);
                        doItemTransfer();
                        _nextRun = DateTime.Now.AddSeconds(Helpers.Timers.RandomTimes.GetRandomSeconds(120, 600));

                        Log.Post("Set new NextRun at " + _nextRun, LogModule.AutoProcessing);
                    }
                }
            }
        }

        private bool isWarehouseInRange()
        {
            var whManagerKey = Collection.Actors.Local.PlayerData.CurrentRegion.x0258_WarehouseManagerKey;

            var whManager = Collection.Actors.Global.ActorList.FirstOrDefault(x => x.ActorId == whManagerKey);

            if (whManager == null)
                return false;

            return whManager.Distance <= 300;
        }

        private bool canProcess()
        {
            return Collection.Actors.Local.PlayerData.isPossibleManufactureAtWarehouse;// && Collection.LifeSkills.Clothes.Processing.Contains((int) Chest.ItemData.ItemIndex);
        }

        private bool isIdleAnimation()
        {
            return Collection.Actors.Local.PlayerData.CharacterControl.CurrentAnimation.Animation ==
                   CharacterAnimation.Idle;
        }

        private bool isOverWeight()
        {
            return (Collection.Actors.Local.PlayerData.CurrentWeight/Collection.Actors.Local.PlayerData.MaxWeight) >= 1.00;
        }

        private void doProcessing()
        {
            var warehouse = new CurrentWarehouse().Inventory.Items;

            foreach (var item in Items)
            {
                if(!item.Enabled)
                    continue;

                var wItem = warehouse.FirstOrDefault(x => x.x0008_PTR_ItemData.ItemIndex == item.ItemId);

                if (wItem != null)
                {
                    if(wItem.x0010_ItemCount < item.MinCount)
                        continue;

                    Log.Post("WarehouseItem found", LogModule.AutoProcessing);

                    CurrentProcessingItem = item;
                    SlotNo = wItem.SlotNO;
                    break;
                }
            }

            if (CurrentProcessingItem == null)
            {
                Log.Post("No WarehouseItem found", LogModule.AutoProcessing);
                return;
            }

            //while (!Panel_Manufacture.isVisible())
            //{
            //    Pipe.Call.DoString("openManufacture()");

            //    Thread.Sleep(1000);
            //}

            CurrentProcessingItem.InitProcessing(SlotNo, false);

            isProcessing = true;
        }

        private void doItemTransfer()
        {
            foreach (var item in Collection.Actors.Local.PlayerData.Inventory.Items)
            {
                if(CurrentProcessingItem.ResultItemIds.Contains((int)item.x0000_PTR_ItemData.ItemIndex))
                    item.SendToWarehouse(item.x0008_ItemCount);
            }

            CurrentProcessingItem = null;
            isProcessing = false;
        }
    }
}
