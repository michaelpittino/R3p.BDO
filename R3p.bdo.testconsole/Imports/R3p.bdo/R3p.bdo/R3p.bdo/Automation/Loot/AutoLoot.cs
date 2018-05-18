using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using R3p.bdo.GameInternals.Structs.UI;
using R3p.bdo.Pipe;

namespace R3p.bdo.Automation.Loot
{
    public class AutoLoot
    {
        public bool Enabled { get; set; }
        private List<int> BlackList = new List<int>();

        public AutoLoot()
        {
            
        }

        public void Run()
        {
            if (!Enabled)
                return;

            LootNearbyBodies();
        }

        private void LootNearbyBodies()
        {
            foreach (var actor in Collection.Actors.Global.ActorList.Where(
                x => x.hasLoot && x.Distance <= 300))
            {
                Pipe.Call.GetDroppedItemList(actor.ActorKey);
                Thread.Sleep(50);

                //BlackList.Add(actor.ActorKey);
            }
        }
    }
}
