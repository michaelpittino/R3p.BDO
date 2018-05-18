using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using R3p.bdo.GameExternals.Enums;
using R3p.bdo.GameExternals.Structs.AutoItemBuy;
using R3p.bdo.GameInternals.Structs.Actors;
using R3p.bdo.GameInternals.Structs.UI;

namespace R3p.bdo.Automation.ItemMarket
{
    public class AutoItemBuy
    {
        public bool Enabled { get; set; }
        public List<ItemBuyObject> Filters { get; set; }
        public UIControl ItemMarket { get; set; }
        public UIControl NakMessage { get; set; }
        public UIControl NakMessage_Text { get; set; }

        private bool _cleared = false;
        private bool _init = false;
        //private double _sleepBetweenPurchases = 0.5;
        private DateTime _lastBuy = DateTime.Now;

        private static bool _trigger;

        private ItemBuyObject _currentItem;
        private bool _isSelected = false;
        private List<long> _currentBids = new List<long>();
        private int _currentId = 0;

        public AutoItemBuy()
        {
            ItemMarket =
                Collection.UI.Base.UIBase.AllPanels.FirstOrDefault(x => x.GetName() == "Panel_Window_ItemMarket");

            NakMessage = Collection.UI.Base.UIBase.AllPanels.FirstOrDefault(x => x.GetName() == "Panel_NakMessage");
            NakMessage_Text = NakMessage.Children.FirstOrDefault();
        }

        public bool _reseted = false;

        public void Run(bool trigger)
        {
            _trigger = trigger;

            if (!_reseted)
            {
                ResetStats();
                _reseted = true;
            }

            if (Enabled && !_init)
            {
                ObtainItemMarketData();
            }

            if (Enabled && trigger)
            {
                if (_cleared)
                    _cleared = false;

                if(IsAbleToBuy())
                    CheckItems();
            }

            if (Enabled && !trigger)
            {
                if (!_cleared)
                {
                    ClearSessionCurrent();
                    ResetCurrentItem();
                }
            }
        }

        private void ObtainItemMarketData()
        {
            foreach (var filter in Filters)
            {
                var list = Collection.ItemMarket.Base.ItemMarketList.List.FirstOrDefault(x => x.Key == filter.ItemId).Value;

                if (list != null)
                {
                    var item = list.FirstOrDefault(x => x.x0012_EnchantLevel == filter.EnchantLevel);

                    if(item != null)
                        filter.MarketItemData = item;
                }
            }

            _init = true;
        }

        private void ClearSessionCurrent()
        {
            foreach (var filter in Filters)
            {
                if(filter.SessionCurrent == 0)
                    continue;

                filter.SessionCurrent = 0;
            }

            _cleared = true;
        }

        private BuyState GetBuyState()
        {
            var text = NakMessage_Text.GetText();

            if (text == "Item has been purchased." || text == "Successfully purchased the item.")
                return BuyState.Purchased;

            return BuyState.Failed;
        }

        private bool isNakMessageVisible()
        {
            return NakMessage.isVisible();
        }

        private void ResetNakMessage()
        {
            Pipe.Call.DoString("Panel_NakMessage:SetShow(false)");
            NakMessage_Text.SetText("--------------");
        }

        private void CheckItems()
        {
            if (_currentItem == null)
            {
                foreach (var filter in Filters)
                {
                    if (!filter.Enabled)
                        continue;

                    if(filter.MarketItemData == null)
                        continue;

                    if (filter.MarketItemData.x0060_RegistredCount == 0)
                        continue;

                    if (filter.SessionCurrent >= filter.SessionMax)
                        continue;

                    _currentItem = filter;
                    break;
                }
            }
            else
            {
                BuyItems();
            }
        }
        
        private void BuyItems()
        {
            if(!_isSelected)
                SelectItem(_currentItem);

            if (!IsAbleToBuy() || GetRegistredAmount(_currentItem) == 0 ||
                _currentItem.SessionCurrent >= _currentItem.SessionMax)
            {
                ResetCurrentItem();
                return;
            }

            var listSellInfo = _currentItem.MarketItemData.SellInfoList.List;

            if (listSellInfo.Count == 0 && _currentItem.MarketItemData.x0060_RegistredCount > 0)
            {
                _isSelected = false;
                return;
            }

            if (_currentId > listSellInfo.Count - 1)
            {
                _currentId = 0;
                return;
            }

            var item = listSellInfo[_currentId];

            if (item.isBid())
            {
                if (!_currentBids.Contains(item.UniqueKey) && item.canJoinBid())
                {
                    BuyItem(_currentItem.MarketItemData.x0010_MarketItemId, _currentId, 1);
                    _currentBids.Add(item.UniqueKey);
                }
                else if (_currentBids.Contains(item.UniqueKey) && item.canBuyResult())
                {
                    var prevInven = GetInventoryItemCount(_currentItem);
                    var prevCount = _currentItem.MarketItemData.x0060_RegistredCount;

                    BuyItem(_currentItem.MarketItemData.x0010_MarketItemId, _currentId, GetPurchaseAmount(_currentItem, item.Count));
                    _currentBids.Remove(item.UniqueKey);

                    addAttempt();

                    if (!WaitUntilBought(_currentItem, prevInven, prevCount))
                    {
                        addFail();
                        // do nothing here if failed to insta skip to next item bid
                    }
                    else
                    {
                        addSucceed();
                    }
                }
            }
            else
            {
                if (_currentId == 0)
                {
                    var prevInven = GetInventoryItemCount(_currentItem);
                    var prevCount = _currentItem.MarketItemData.x0060_RegistredCount;

                    BuyItem(_currentItem.MarketItemData.x0010_MarketItemId, _currentId, GetPurchaseAmount(_currentItem, item.Count));
                    _currentBids.Remove(item.UniqueKey);

                    addAttempt();

                    if (!WaitUntilBought(_currentItem, prevInven, prevCount))
                    {
                        addFail();
                        // if failed refresh the list and start again from id 0
                        _isSelected = false;
                        return;
                    }
                    else
                    {
                        addSucceed();
                    }
                }
            }

            _currentId++;
        }

        private void addAttempt()
        {
            if (Collection.AutoItemBuy.Log._attempts.ContainsKey(_currentItem.ItemId))
            {
                Collection.AutoItemBuy.Log._attempts[_currentItem.ItemId]++;
            }
            else
            {
                Collection.AutoItemBuy.Log._attempts.Add(_currentItem.ItemId, 1);
            }

            Collection.AutoItemBuy.Log.updated = true;
        }

        private void addFail()
        {
            if (Collection.AutoItemBuy.Log._fails.ContainsKey(_currentItem.ItemId))
            {
                Collection.AutoItemBuy.Log._fails[_currentItem.ItemId]++;
            }
            else
            {
                Collection.AutoItemBuy.Log._fails.Add(_currentItem.ItemId, 1);
            }

            Collection.AutoItemBuy.Log.updated = true;
        }

        private void addSucceed()
        {
            if (Collection.AutoItemBuy.Log._succeeds.ContainsKey(_currentItem.ItemId))
            {
                Collection.AutoItemBuy.Log._succeeds[_currentItem.ItemId]++;
            }
            else
            {
                Collection.AutoItemBuy.Log._succeeds.Add(_currentItem.ItemId, 1);
            }

            Collection.AutoItemBuy.Log.updated = true;
        }

        private void ResetCurrentItem()
        {
            _currentItem = null;
            _isSelected = false;
            _currentBids = new List<long>();
            _currentId = 0;
        }

        private int GetRegistredAmount(ItemBuyObject item)
        {
            return (int)item.MarketItemData.x0060_RegistredCount;
        }

        private int GetPurchaseAmount(ItemBuyObject item, int desiredCount)
        {
            if (item.SessionCurrent + desiredCount > item.SessionMax)
                return item.SessionMax - item.SessionCurrent;

            return desiredCount;
        }

        private int GetInventoryItemCount(ItemBuyObject item)
        {
            int count = 0;

            if (item.IsStack)
            {
                var inventoryItem = GetInventoryItem(item.ItemId, item.EnchantLevel);

                if (inventoryItem != null)
                    count = inventoryItem.x0008_ItemCount;
            }
            else
            {
                count = GetInventoryItems(item.ItemId, item.EnchantLevel).Count;
            }

            return count;
        }

        private bool WaitUntilBought(ItemBuyObject item, int prevInven, long prevAmount)
        {
            bool failed = false;

            while(!isNakMessageVisible())
                Thread.Sleep(10);

            Thread.Sleep(500);

            int dif = 0;

            DateTime start = DateTime.Now;

            if (GetBuyState() == BuyState.Purchased)
            {
                var curInv = GetInventoryItemCount(item);

                while (curInv == prevInven)
                {
                    if (start.AddSeconds(3) <= DateTime.Now)
                        break;

                    curInv = GetInventoryItemCount(item);

                    Thread.Sleep(1);
                }

                dif = curInv - prevInven;
            }
            else
                failed = true;

            _currentItem.SessionCurrent += dif;

            ResetNakMessage();

            if (failed)
                return false;

            return true;
        }

        private bool IsAbleToBuy()
        {
            return Collection.Actors.Local.PlayerData.x19B8_FreeInventorySlots >= 1 &&
                   ((Collection.Actors.Local.PlayerData.CurrentWeight / Collection.Actors.Local.PlayerData.MaxWeight) * 100) < 130;
        }

        private PlayerInventoryItem GetInventoryItem(int itemId, int enchantLevel)
        {
            return Collection.Actors.Local.PlayerData.Inventory.Items.FirstOrDefault(
                    x =>
                        x.x0000_PTR_ItemData.ItemIndex == itemId &&
                        x.x0000_PTR_ItemData.EnchantLevel == enchantLevel);
        }

        private List<PlayerInventoryItem> GetInventoryItems(int itemId, int enchantLevel)
        {
            return Collection.Actors.Local.PlayerData.Inventory.Items.Where(
                    x =>
                        x.x0000_PTR_ItemData.ItemIndex == itemId &&
                        x.x0000_PTR_ItemData.EnchantLevel == enchantLevel).ToList();
        }
        
        private void SelectItem(ItemBuyObject item)
        {
            var oldAdr = item.MarketItemData.SellInfoList.ReadPointer8b(0x0);
            
            while (item.MarketItemData.SellInfoList.ReadPointer8b(0x0) == oldAdr)
            {
                Pipe.Call.DoString("HandleClicked_ItemMarket_GroupItem(1," + item.MarketItemData.x0010_MarketItemId + ")");

                Thread.Sleep(500);
            }

            _isSelected = true;
        }

        private void BuyItem(int ItemId, int Slot, int Count)
        {
            Pipe.Call.DoString(String.Format("requestBuyItemForItemMarket({0},{1},{2},{3})", "(CppEnums.ItemWhereType).eWarehouse", ItemId, Slot, Count));
        }

        private void ResetStats()
        {
            Collection.AutoItemBuy.Log._attempts.Clear();
            Collection.AutoItemBuy.Log._fails.Clear();
            Collection.AutoItemBuy.Log._succeeds.Clear();

            Collection.AutoItemBuy.Log.updated = true;
        }

        public void PostStats()
        {
            if (!_trigger)
                return;

            if (!Collection.AutoItemBuy.Log.updated)
                return;

            Console.Title = "R3p.BDO AutoItemBuy";

            Console.Clear();

            Console.WriteLine("{0,5}\t{1,10}\t{2,10}\t{3,10}\t{4,10}\n", "ItemId", "EnchantLevel", "Attempts", "Fail", "Succeed");

            foreach (var item in Filters.Where(x => x.Enabled))
            {
                var attempts = 0;
                var fail = 0;
                var succeed = 0;

                if (Collection.AutoItemBuy.Log._attempts.ContainsKey(item.ItemId))
                    attempts = Collection.AutoItemBuy.Log._attempts[item.ItemId];

                if (Collection.AutoItemBuy.Log._fails.ContainsKey(item.ItemId))
                    fail = Collection.AutoItemBuy.Log._fails[item.ItemId];

                if (Collection.AutoItemBuy.Log._succeeds.ContainsKey(item.ItemId))
                    succeed = Collection.AutoItemBuy.Log._succeeds[item.ItemId];

                Console.WriteLine("{0,5}\t{1,10}\t{2,10}\t{3,10}\t{4,10}\n", item.ItemId, item.EnchantLevel, attempts, fail, succeed);
            }

            Collection.AutoItemBuy.Log.updated = false;
        }
    }
}
