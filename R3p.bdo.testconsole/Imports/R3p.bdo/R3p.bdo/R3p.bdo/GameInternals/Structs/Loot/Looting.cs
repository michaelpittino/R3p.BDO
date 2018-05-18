using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.GameInternals.Structs.UI;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.Loot
{
    public class Looting : MemoryObject
    {
        public int Count { get; set; }

        public UIControl PanelLooting { get; set; }

        public Looting()
        {
            Address = Offsets._lootBase;
            PanelLooting =
                Collection.UI.Base.UIBase.AllPanels.FirstOrDefault(x => x.GetName().ToLower().Contains("panel_loot"));
        }

        private long LootList_Start => ReadPointer8b(Address);
        private long LootList_End => ReadPointer8b(0x8);

        public List<LootItem> LootItems => GetList();

        private List<LootItem> GetList()
        {
            if (!PanelLooting.isVisible())
            {
                Count = 0;
                return new List<LootItem>();
            }

            Count = (int) ((LootList_End - LootList_Start)/0x88);

            List<LootItem> List = new List<LootItem>();

            for (int i = 0; i < Count; i++)
            {
                List.Add(new LootItem(LootList_Start + (i * 0x88), i));
            }

            return List;
        }
    }
}
