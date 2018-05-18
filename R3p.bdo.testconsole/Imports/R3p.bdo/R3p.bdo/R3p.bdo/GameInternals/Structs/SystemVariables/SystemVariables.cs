using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.SystemVariables
{
    public class SystemVariables : MemoryObject
    {
        public SystemVariables()
        {
            Address = Offsets._systemVariables;
        }

        public List<SystemVariable> List => GetList();

        private List<SystemVariable> GetList()
        {
            List<SystemVariable> list = new List<SystemVariable>();

            int someValue = 0;

            for (int i = 0; i < 1000; i++)
            {
                SystemVariable s = new SystemVariable(Address + (0x20 * i));

                if (s.Name == "")
                {
                    s.Name = "sysVar_" + someValue;

                    someValue++;
                }

                if (s.Name == "_use_object_loadrange")
                {
                    list.Add(s);
                    break;
                }

                list.Add(s);
            }

            return list;
        } 
    }
}
