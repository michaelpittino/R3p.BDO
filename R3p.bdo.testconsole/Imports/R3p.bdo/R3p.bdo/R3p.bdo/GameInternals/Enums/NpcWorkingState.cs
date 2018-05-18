using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3p.bdo
{
	public enum NpcWorkingState
	{
		eNpcWorkingState_Undefined = -1,
		 eNpcWorkingState_HarvestWorking_MoveTo = 900,
		 eNpcWorkingState_HarvestWorking_Working = 901,
		 eNpcWorkingState_HarvestWorking_Return = 902,
	}
}