using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3p.bdo.Automation.Hacks
{
    public class Navigation
    {
        public Navigation()
        {
            
        }

        public void SetFlags()
        {
            var regions = Collection.Region.Base.RegionInfoManager.RegionList;

            foreach (var region in regions)
            {
                if (region.x001B_IsDesert)
                {
                    region.SetNonDesert();
                    Log.Post("Region " + region.x0000_RegionKey + " - set NonDesert", LogModule.Hack_Navigation);
                }

                if (region.x001D_IsOcean)
                {
                    region.SetNonOcean();
                    Log.Post("Region " + region.x0000_RegionKey + " - set NonOcean", LogModule.Hack_Navigation);
                }

                //if(!region.x0026_IsAccessableArea)
                //    region.SetAccessable();
            }
        }
    }
}
