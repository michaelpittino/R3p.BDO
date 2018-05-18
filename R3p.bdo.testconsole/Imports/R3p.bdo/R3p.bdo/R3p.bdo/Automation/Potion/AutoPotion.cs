using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.GameInternals.Enums;
using R3p.bdo.GameInternals.Structs.Actors;

namespace R3p.bdo.Automation.Potion
{
    public class AutoPotion
    {
        public bool Enabled { get; set; }
        public double HPPercent { get; set; }
        public double MPPercent { get; set; }

        private EquipmentItem MainHand { get; set; }

        public AutoPotion()
        {
            MainHand = Collection.Actors.Local.PlayerData.GetEquipmentItem(EquipSlotNo.rightHand);
        }

        public void Run()
        {
            if (!Enabled || Collection.Actors.Local.PlayerData.isDead)
                return;

            if (isFishingRodEquipped())
                return;

            if (Collection.Actors.Local.PlayerData.CurrentRegion.x0018_IsSafezone)
                return;

            if (getCurHPPercentage() <= HPPercent)
                RefillHP();

            if(getCurResourcePercentage() <= MPPercent)
                RefillResource();
        }

        private bool isFishingRodEquipped()
        {
            return Collection.Fishing.Rods.List.Contains((int)MainHand.ItemData.ItemIndex);
        }

        private double getCurHPPercentage()
        {
            return Collection.Actors.Local.PlayerData.PercentageCurHitpoints;
        }

        private double getCurResourcePercentage()
        {
            return Collection.Actors.Local.PlayerData.PercentageCurResource;
        }

        private long hpItemEndTick;

        private void RefillHP()
        {
            if (Collection.Base.Ticks.BaseTicks.BaseTick < hpItemEndTick)
                return;

            var pots =
                Collection.Actors.Local.PlayerData.Inventory.Items.Where(x => x.isRegularHpPotion() || x.isRbfHpPotion());

            var pot = pots.FirstOrDefault(x => !x.isRbfHpPotion());
            var rbfPot = pots.FirstOrDefault(x => x.isRbfHpPotion());

            Log.Post("Using Hp Potion", LogModule.AutoPotion);

            if (Collection.Actors.Local.PlayerData.CurrentRegion.isRedBattleField && rbfPot != null)
            {
                    if(!rbfPot.isOnCooldown())
                    rbfPot.UseItem(1);
                hpItemEndTick = rbfPot.itemUseEndTick;
                return;
            }

            if (pot == null)
                return;

            if (!pot.isOnCooldown())
            {
                pot.UseItem(1);
                hpItemEndTick = pot.itemUseEndTick;
            }
        }

        private long resourceItemEndTick;

        private void RefillResource()
        {
            if (Collection.Base.Ticks.BaseTicks.BaseTick < resourceItemEndTick)
                return;

            var pots =
                Collection.Actors.Local.PlayerData.Inventory.Items.Where(x => x.isRegularMpPotion() || x.isRbfMpPotion());

            var pot = pots.FirstOrDefault(x => !x.isRbfMpPotion());
            var rbfPot = pots.FirstOrDefault(x => x.isRbfMpPotion());

            if (Collection.Actors.Local.PlayerData.ResourceType == CombatResouceType.WP)
            {
                pots = Collection.Actors.Local.PlayerData.Inventory.Items.Where(x => x.isRegularWpPotion() || x.isRbfWpPotion());

                pot = pots.FirstOrDefault(x => !x.isRbfWpPotion());
                rbfPot = pots.FirstOrDefault(x => x.isRbfWpPotion());
            }

            if (pots.Count() == 0)
                return;
            
            Log.Post("Using Mp Potion", LogModule.AutoPotion);
            
            if (Collection.Actors.Local.PlayerData.CurrentRegion.isRedBattleField && rbfPot != null)
            {
                if (!rbfPot.isOnCooldown())
                    rbfPot.UseItem(2);
                resourceItemEndTick = rbfPot.itemUseEndTick;
                return;
            }

            if (pot == null)
                return;

            if (!pot.isOnCooldown())
            {
                pot.UseItem(2);
                resourceItemEndTick = pot.itemUseEndTick;
            }
        }
    }
}
