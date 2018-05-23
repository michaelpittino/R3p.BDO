using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using R3p.bdo.GameInternals.Structs.UI;

namespace R3p.bdo.Automation.Hacks
{
    public class UI
    {
        public bool Enabled { get; set; }

        public UIControl ItemSetPanel { get; set; }
        private UIControl Btn_GetAllItems { get; set; }
        private UIControl Frame_GuildQuests { get; set; }

        public UI()
        {
            Frame_GuildQuests =
                Collection.UI.Base.UIBase.AllPanels.FirstOrDefault(x => x.GetName() == "Panel_Window_Guild");

            ItemSetPanel = Collection.UI.Base.UIBase.AllPanels.FirstOrDefault(x => x.GetName().ToLower().Contains("itemset"));

            //if(ItemSetPanel != null)
            //    Btn_GetAllItems = ItemSetPanel.Children.FirstOrDefault(x => x.GetName().ToLower().Contains("getallitem"));
        }

        public void Run()
        {
            if (!Enabled)
                return;

            hackButtonInteraction();
            //enableGetAllButton();
            TransformGuildQuests();
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

                    Log.Post("Enabled GetAll-Button", LogModule.Hack_UI);
                }
            }
        }

        private void TransformGuildQuests()
        {
            if (Frame_GuildQuests == null)
                return;

            if (!Frame_GuildQuests.isVisible())
                return;

            //bool isFixed = false;

            foreach (var quest in Collection.Quests.Base.GuildQuests.List)
            {
                quest.GuildQuestData.FixDescription();

                //if (b && !isFixed)
                //    isFixed = true;
            }

            //if (isFixed)
            //{
            //    Pipe.Call.DoString("GuildQuestInfoPage:UpdateData()");
            //    Thread.Sleep(250);
            //}
        }
    }
}
