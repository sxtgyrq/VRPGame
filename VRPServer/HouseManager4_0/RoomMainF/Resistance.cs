using CommonClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager4_0.RoomMainF
{

    public partial class RoomMain : interfaceOfHM.Resistance
    {
        public string GetResistance(GetResistanceObj r)
        {
            this.modelR.Display(r);
            return "";
            //throw new NotImplementedException();
        }
    }
}
