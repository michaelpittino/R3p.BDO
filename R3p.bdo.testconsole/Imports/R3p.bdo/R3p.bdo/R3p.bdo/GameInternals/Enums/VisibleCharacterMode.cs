using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3p.bdo
{
	public enum VisibleCharacterMode
	{
		eVisibleCharacterMode_None = 0,
		 eVisibleCharacterMode_All = 1,
		 eVisibleCharacterMode_Intimaciable = 2,
		 eVisibleCharacterMode_Interactable = 3,
		 eVisibleCharacterMode_Cutscene = 4,
		 eVisibleCharacterMode_Cutscene_With_SelfPlayer = 5,
		 eVisibleCharacterMode_Housing = 6,
		 eVisibleCharacterMode_Count = 7,
	}
}