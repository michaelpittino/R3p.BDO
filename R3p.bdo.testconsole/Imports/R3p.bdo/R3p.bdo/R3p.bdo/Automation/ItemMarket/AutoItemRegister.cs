using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.GameExternals.Structs.AutoItemRegister;
using R3p.bdo.GameInternals.Structs.UI;
using R3p.bdo.Pipe;

namespace R3p.bdo.Automation.ItemMarket
{
    public class AutoItemRegister
    {
        public bool Enabled { get; set; }
        public List<ItemRegisterObject> Filters { get; set; }

        private UIControl ItemSetPanel { get; set; }

        public AutoItemRegister()
        {
            ItemSetPanel = Collection.UI.Base.UIBase.AllPanels.FirstOrDefault(x => x.GetName().ToLower().Contains("itemset"));
        }

        public void Run()
        {
            if (!Enabled || ItemSetPanel == null)
                return;

            if(ItemSetPanel.isVisible() && getRegistredAmount() < 30)
                SellItems();
        }

        private int getRegistredAmount()
        {
            return Collection.ItemMarket.Base.ItemMarketList.RegistredItemCount;
        }

        private void SellItems()
        {
            foreach (var filter in Filters)
            {
                if(!filter.Enabled || filter.ItemId == 0)
                    continue;

                var items =
                    Collection.Actors.Local.PlayerData.Inventory.Items.Where(
                        x => x.x0000_PTR_ItemData.ItemIndex == filter.ItemId);

                if (items.Count() == 0)
                    continue;

                foreach (var item in items)
                {
                    if (getRegistredAmount() >= 30)
                        return;

                    if (item.x0008_ItemCount == 1)
                    {
                        item.SellItem(FromWhereType.Inventory, filter.SellValue);
                    }
                    else if (item.x0008_ItemCount > 1)
                    {
                        while(item.x0008_ItemCount > 0 && getRegistredAmount() < 30)
                            item.SellItem(FromWhereType.Inventory, filter.SellValue);
                    }

                }
            }
        }
    }
}
