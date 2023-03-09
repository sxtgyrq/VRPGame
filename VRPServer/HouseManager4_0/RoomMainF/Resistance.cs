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
            var disPlay = this.modelR.Display(r);
            return disPlay;
            //throw new NotImplementedException();
        }
    }
}
