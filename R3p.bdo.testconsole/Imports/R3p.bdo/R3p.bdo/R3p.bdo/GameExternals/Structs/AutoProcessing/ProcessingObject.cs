using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using R3p.bdo.GameExternals.Enums;
using R3p.bdo.Memory;

namespace R3p.bdo.GameExternals.Structs.AutoProcessing
{
    public class ProcessingObject : MemoryObject
    {
        public bool Enabled { get; set; }
        public int ItemId { get; set; }
        public ProcessingType ProcessingType { get; set; }
        public int MinCount { get; set; }
        public int[] ResultItemIds { get; set; }
        public string Comment { get; set; }

        public ProcessingObject(bool enabled, int itemId, ProcessingType processingType, int minCount, int[] resultItemIds, string comment)
        {
            Enabled = enabled;
            ItemId = itemId;
            ProcessingType = processingType;
            MinCount = minCount;
            ResultItemIds = resultItemIds;
            Comment = comment;
        }

        public void InitProcessing(int slotNo, bool reset)
        {
            Log.Post("Initiating Processing - Type(" + ProcessingType + ") SlotNo(" + slotNo + ") Reset(" + reset + ") ItemId(" + ItemId + ")", LogModule.AutoProcessing);

            //Log.Post("Opening Manufacture Panel", LogModule.AutoProcessing);
            //Pipe.Call.DoString("openManufacture()");
            Thread.Sleep(1000);

            //Log.Post("Selecting Processing Type", LogModule.AutoProcessing);

            switch (ProcessingType)
            {
                    case ProcessingType.Shaking:
                    Pipe.Call.DoString("warehouseShake(" + slotNo + ")");
                    break;

                case ProcessingType.Grinding:
                    Pipe.Call.DoString("warehouseGrind(" + slotNo + ")");
                    break;

                case ProcessingType.Chopping:
                    Pipe.Call.DoString("warehouseChop(" + slotNo + ")");
                    break;

                case ProcessingType.Drying:
                    Pipe.Call.DoString("warehouseDry(" + slotNo + ")");
                    break;

                case ProcessingType.Thinning:
                    Pipe.Call.DoString("warehouseThinning(" + slotNo + ")");
                    break;

                case ProcessingType.Heating:
                    Pipe.Call.DoString("warehouseHeat(" + slotNo + ")");
                    break;
            }

            Thread.Sleep(1000);

            //Log.Post("Pushing WarehouseItem to Manufacture Panel", LogModule.AutoProcessing);
            //Pipe.Call.DoString("pushItemFromWarehouse(" + slotNo + ")");
            //Thread.Sleep(2000);

            //Log.Post("Starting Manufacture", LogModule.AutoProcessing);
            //Pipe.Call.DoString("manufactureStart()");
            //Thread.Sleep(2000);
        }
    }
}
