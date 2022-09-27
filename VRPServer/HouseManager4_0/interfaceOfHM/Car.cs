using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager4_0.interfaceOfHM
{
  public  interface Car
    {
        void SendStateOfCar(HouseManager4_0.Player player, HouseManager4_0.Car car, ref List<string> notifyMsg);
        void SetAnimateChanged(RoleInGame player, HouseManager4_0.Car car, ref List<string> notifyMsg);


    }
}
