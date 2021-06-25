using CommonClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager2_0.RoomMainF
{
    public partial class RoomMain
    {
        private void brokenParameterT1RecordChanged(string selfKey, string otherKey, long value, ref List<string> notifyMsg)
        {
            if (this._Players.ContainsKey(selfKey))
            {
                var self = this._Players[selfKey];
                if (this._Players.ContainsKey(otherKey))
                {
                    if (self.playerType == RoleInGame.PlayerType.player)
                    {
                        var other = this._Players[otherKey];
                        var obj = new BradCastSocialResponsibility
                        {
                            c = "BradCastSocialResponsibility",
                            socialResponsibility = value,
                            WebSocketID = ((Player)self).WebSocketID,
                            otherKey = otherKey
                        };
                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                        notifyMsg.Add(((Player)self).FromUrl);
                        notifyMsg.Add(json);
                    }
                }
            }



        }

        /// <summary>
        /// 获取所有债主的股份，这里包括自己的股份，获取的单位是万分之一
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        private static Dictionary<string, long> GetShares(RoleInGame player)
        {
            return player.Shares;

        }
    }

}
