using CommonClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager4_0.RoomMainF
{
    public partial class RoomMain : interfaceOfHM.Bust
    {
        public void BustChangedF(RoleInGame role, bool bustValue, ref List<string> msgsWithUrl)
        {
            // if (role.playerType == RoleInGame.PlayerType.player)
            foreach (var item in this._Players)
            {
                if (item.Value.playerType == RoleInGame.PlayerType.player)
                {
                    var player = (Player)item.Value;
                    msgsWithUrl.Add(player.FromUrl);
                    BustStateNotify tn = new BustStateNotify()
                    {
                        c = "BustStateNotify",
                        Bust = bustValue,
                        WebSocketID = player.WebSocketID,
                        Key = player.Key,
                        KeyBust = role.Key,
                        Name = role.PlayerName
                    };
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(tn);
                    msgsWithUrl.Add(json);
                }

            }
        }


    }
}
