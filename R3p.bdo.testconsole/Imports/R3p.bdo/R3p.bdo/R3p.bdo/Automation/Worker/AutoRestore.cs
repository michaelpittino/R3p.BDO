using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using R3p.bdo.Pipe;

namespace R3p.bdo.Automation.Worker
{
    public class AutoRestore
    {
        public bool Enabled { get; set; }

        private bool itemSelected = false;
        private bool workerRestored = false;
        private bool workerRepeated = false;

        public AutoRestore()
        {
            
        }

        private long lastRun;

        public void Run()
        {
            if (!Enabled)
                return;

            if (lastRun + 5000 <= Collection.Base.Ticks.BaseTicks.BaseTick)
                lastRun = Collection.Base.Ticks.BaseTicks.BaseTick;
            else
            {
                return;
            }

            if (!shouldRestore())
                return;

            if (!hasRestoreItems())
                return;

            selectRestoreItem(0);
            restoreAllWorker(0);
            repeatAllWorker();
            ResetState();
        }

        private bool shouldRestore()
        {
            if (Collection.Worker.Base.WorkerList.List == null)
                return false;

            return Collection.Worker.Base.WorkerList.List.Count(x => x.x0042_CurrentStamina <= 1) != 0;
        }

        private bool hasRestoreItems()
        {
            for (int i = 0; i < Collection.Worker.RestoreItems.List.Count; i++)
            {
                if (
                    Collection.Actors.Local.PlayerData.Inventory.Items.Any(
                        x => (int) x.x0000_PTR_ItemData.ItemIndex == Collection.Worker.RestoreItems.List[i]))
                    return true;
            }

            return false;
        }

        private void openWorkerRestore()
        {
            Log.Post("Open WorkerRestore", LogModule.AutoWorkerRestore);

            Pipe.Call.DoString(LuaFunctions.WorkerManager_RecoverAll);
            Thread.Sleep(500);
        }

        private void closeWorkerRestore()
        {
            Log.Post("Close WorkerRestore", LogModule.AutoWorkerRestore);

            Pipe.Call.DoString(LuaFunctions.WorkerRestore_Close);
            Thread.Sleep(500);
        }

        private void selectRestoreItem(int slotNo)
        {
            if (!itemSelected)
            {
                openWorkerRestore();

                Log.Post("Select RestoreItem", LogModule.AutoWorkerRestore);

                Pipe.Call.DoString(LuaFunctions.WorkerRestore_SelectItem(slotNo));
                Thread.Sleep(500);

                itemSelected = true;
            }
        }

        private void restoreAllWorker(int slotNo)
        {
            if (!workerRestored && itemSelected)
            {
                Log.Post("Restore All Workers", LogModule.AutoWorkerRestore);

                Pipe.Call.DoString(LuaFunctions.WorkerRestore_Confirm(slotNo));
                Thread.Sleep(2000);

                workerRestored = true;
            }
        }

        private void repeatAllWorker()
        {
            if (!workerRepeated && workerRestored)
            {
                closeWorkerRestore();

                Log.Post("Repeat All Workers", LogModule.AutoWorkerRestore);

                Pipe.Call.DoString(LuaFunctions.WorkerManager_RepeatAll);
                Thread.Sleep(500);

                workerRepeated = true;
            }
        }

        private void ResetState()
        {
            if (itemSelected && workerRestored && workerRepeated)
            {
                Log.Post("Reset State", LogModule.AutoWorkerRestore);

                itemSelected = false;
                workerRestored = false;
                workerRepeated = false;
            }
        }
    }
}
