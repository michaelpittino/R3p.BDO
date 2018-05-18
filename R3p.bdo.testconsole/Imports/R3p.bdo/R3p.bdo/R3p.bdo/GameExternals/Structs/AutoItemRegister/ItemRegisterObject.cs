using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3p.bdo.GameExternals.Structs.AutoItemRegister
{
    public class ItemRegisterObject
    {
        public bool Enabled { get; set; }
        public int ItemId { get; set; }
        public int SellValue { get; set; }

        public ItemRegisterObject(bool enabled, int itemId, int sellValue)
        {
            Enabled = enabled;
            ItemId = itemId;
            SellValue = sellValue;
        }
    }
}
