using CommonClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager4_0.RoomMainF
{
    public partial class RoomMain
    {
        public void WebNotify(RoleInGame role, string msg)
        {
            Console.WriteLine($"{msg}");
            if (role.playerType == RoleInGame.PlayerType.player)
            {
                var player = ((Player)role);
                var url = player.FromUrl;

                WMsg wMsg = new WMsg()
                {
                    c = "WMsg",
                    WebSocketID = player.WebSocketID,
                    Msg = msg
                };

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(wMsg);

                Startup.sendMsg(url, json);
            }
        }
    }
}
