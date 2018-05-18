using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.GameInternals.Structs.UI;

namespace R3p.bdo.Automation.Hacks
{
    public class UiInteraction
    {
        public bool Enabled { get; set; }

        public UIControl ItemSetPanel { get; set; }
        private UIControl Btn_GetAllItems { get; set; }

        public UiInteraction()
        {
            ItemSetPanel = Collection.UI.Base.UIBase.AllPanels.FirstOrDefault(x => x.GetName().ToLower().Contains("itemset"));

            if(ItemSetPanel != null)
                Btn_GetAllItems = ItemSetPanel.Children.FirstOrDefault(x => x.GetName().ToLower().Contains("getallitem"));
        }

        public void Run()
        {
            if (!Enabled)
                return;

            hackButtonInteraction();
            enableGetAllButton();
        }

        private void hackButtonInteraction()
        {
            if (!Collection.UI.Base.UIBase.x0188_HoveredUIControl.IsInteractable() && Collection.UI.Base.UIBase.x0188_HoveredUIControl.GetText() == "Buy")
                Collection.UI.Base.UIBase.x0188_HoveredUIControl.SetInteractable();
        }
        
        private void enableGetAllButton()
        {
            if (ItemSetPanel == null || Btn_GetAllItems == null)
                return;
            

            if (ItemSetPanel.isVisible())
            {
                if (!Btn_GetAllItems.isVisible())
                {
                    Btn_GetAllItems.SetVisible();
                    Btn_GetAllItems.SetScreenPos(new float[] { 500, 105 });
                }
            }
        }
    }
}
