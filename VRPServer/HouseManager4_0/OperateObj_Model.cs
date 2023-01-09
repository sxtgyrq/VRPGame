using CommonClass;
using HouseManager4_0.RoomMainF;
using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager4_0
{
    internal class OperateObj_Model : OperateObj
    {
        public OperateObj_Model(RoomMain roomMain)
        {
            this.roomMain = roomMain;
        }

        internal void GetRoadNearby(ModelTranstraction.GetRoadNearby grn)
        {
            List<string> notifyMsgs = new List<string>();
            var result = Program.dt.GetRoadNearby(grn.x, grn.z);
            if (that._Players.ContainsKey(grn.key))
            {
                if (that._Players[grn.key].playerType == RoleInGame.PlayerType.player)
                {
                    for (int i = 0; i < result.Count; i++)
                    {
                        ((Player)that._Players[grn.key]).addUsedRoad(result[i], ref notifyMsgs);
                    }
                }
            }
            this.sendSeveralMsgs(notifyMsgs);
        }
    }
}
