using CommonClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager4_0.RoomMainF
{
    public partial class RoomMain
    {
        public void TheLargestHolderKeyChanged(string keyFrom, string keyTo, string roleKey, ref List<string> notifyMsg)
        {
            return;
            /*
             * lock 机制，保证了所有的key 都存在
             */
            var roleName = this._Players[roleKey].PlayerName;
            var nameTo = this._Players[keyTo].PlayerName;
            if (this._Players.ContainsKey(keyFrom))
            {
                var role = this._Players[keyFrom];
                if (role.playerType == RoleInGame.PlayerType.player)
                {
                    var player = (Player)role;
                    TheLargestHolderChangedNotify holder = new TheLargestHolderChangedNotify()
                    {
                        c = "TheLargestHolderChangedNotify",
                        WebSocketID = player.WebSocketID,
                        operateKey = roleKey,
                        operateName = roleName,
                        ChangeTo = keyTo,
                        nameTo = nameTo
                    };
                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(holder);
                    var url = player.FromUrl;
                    notifyMsg.Add(url);
                    notifyMsg.Add(sendMsg);
                }

            }
            if (keyTo != keyFrom && this._Players.ContainsKey(keyTo))
            {
                var role = this._Players[keyTo];
                if (role.playerType == RoleInGame.PlayerType.player)
                {
                    var player = (Player)role;
                    TheLargestHolderChangedNotify holder = new TheLargestHolderChangedNotify()
                    {
                        c = "TheLargestHolderChangedNotify",
                        WebSocketID = player.WebSocketID,
                        operateKey = roleKey,
                        operateName = roleName,
                        ChangeTo = keyTo,
                        nameTo = nameTo
                    };
                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(holder);
                    var url = player.FromUrl;
                    notifyMsg.Add(url);
                    notifyMsg.Add(sendMsg);
                }

            }
            if (roleKey != keyFrom && roleKey != keyTo && this._Players.ContainsKey(roleKey))
            {
                var role = this._Players[roleKey];
                if (role.playerType == RoleInGame.PlayerType.player)
                {
                    var player = (Player)role;
                    TheLargestHolderChangedNotify holder = new TheLargestHolderChangedNotify()
                    {
                        c = "TheLargestHolderChangedNotify",
                        WebSocketID = player.WebSocketID,
                        operateKey = roleKey,
                        operateName = roleName,
                        ChangeTo = keyTo,
                        nameTo = nameTo
                    };
                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(holder);
                    var url = player.FromUrl;
                    notifyMsg.Add(url);
                    notifyMsg.Add(sendMsg);
                }
            }
        }

        public bool isAtTheSameGroup(RoleInGame player, RoleInGame victim)
        {
            if (player.TheLargestHolderKey == victim.Key)
            {
                return true;
            }
            else if (victim.TheLargestHolderKey == player.Key)
            {
                //Msg = $"[{victim.PlayerName}]是你的小弟，只能发挥出攻击效率的10%";
                return true;
            }
            else if (victim.TheLargestHolderKey == player.TheLargestHolderKey)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool isAtTheSameGroup(string player, string victim)
        {
            return isAtTheSameGroup(this._Players[player], this._Players[victim]);
        }
    }
}
