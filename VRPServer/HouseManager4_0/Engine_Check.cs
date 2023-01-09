using CommonClass;
using HouseManager4_0.RoomMainF;
using System;
using System.Collections.Generic;
using static HouseManager4_0.Car;

namespace HouseManager4_0
{
    public class Engine_Check : OperateObj
    {
        public Engine_Check(RoomMain roomMain)
        {
            this.roomMain = roomMain;
        }

        internal string CheckCarStateF(CheckCarState ccs)
        {
            List<string> notifyMsg = new List<string>();
            {
                if (that._Players.ContainsKey(ccs.Key))
                {
                    var role = that._Players[ccs.Key];
                    if (role.playerType == RoleInGame.PlayerType.player)
                    {
                        var player = (Player)that._Players[ccs.Key];
                        if (Enum.GetName(typeof(CarState), player.getCar().state) == ccs.State) { }
                        else
                        {
                            lock (that.PlayerLock)
                            {
                                var car = player.getCar();
                                var state = player.getCar().state;
                                that.SendStateOfCar(player, car, ref notifyMsg);
                            }
                        }
                    }
                }
            }
            this.sendSeveralMsgs(notifyMsg);
            return "";
        }
    }
}
