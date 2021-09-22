using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager4_0.interfaceOfHM
{
    public interface ContactInterface
    {
        string targetOwner { get; }
        int target { get; }

        void SetArrivalThread(int startT, HouseManager4_0.Car car, int goMile, RoomMainF.RoomMain.Node goPath, RoomMainF.RoomMain.commandWithTime.ReturningOjb returningOjb);
        bool carLeftConditions(HouseManager4_0.Car car);
    }
}
