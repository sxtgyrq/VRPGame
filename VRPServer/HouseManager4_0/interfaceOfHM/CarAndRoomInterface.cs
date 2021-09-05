using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager4_0.interfaceOfHM
{
    interface CarAndRoomInterface
    {
        void SetAnimateChanged(RoleInGame player, HouseManager4_0.Car car, ref List<string> notifyMsg);
        void AbilityChanged2_0(HouseManager4_0.Player player, HouseManager4_0.Car car, ref List<string> notifyMsgs, string pType);
        void DiamondInCarChanged(HouseManager4_0.Player player, HouseManager4_0.Car car, ref List<string> notifyMsgs, string value);
        void DriverSelected(RoleInGame player, HouseManager4_0.Car car, ref List<string> notifyMsgs);
    }
}
