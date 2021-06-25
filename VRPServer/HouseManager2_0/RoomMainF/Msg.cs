using CommonClass;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HouseManager2_0.RoomMainF
{
    public partial class RoomMain
    {
        internal void SendMsg(DialogMsg dm)
        {
            List<string> notifyMsg = new List<string>();
            lock (this.PlayerLock)
                if (this._Players.ContainsKey(dm.Key))
                {
                    if (this._Players.ContainsKey(dm.To))
                    {
                        if (this._Players[dm.Key].playerType == RoleInGame.PlayerType.player)
                        {
                            notifyMsg.Add(((Player)this._Players[dm.Key]).FromUrl);
                            dm.WebSocketID = ((Player)this._Players[dm.Key]).WebSocketID;
                            notifyMsg.Add(Newtonsoft.Json.JsonConvert.SerializeObject(dm));
                        }
                        if (this._Players[dm.To].playerType == RoleInGame.PlayerType.player)
                        {

                            notifyMsg.Add(((Player)this._Players[dm.To]).FromUrl);
                            dm.WebSocketID = ((Player)this._Players[dm.To]).WebSocketID;
                            notifyMsg.Add(Newtonsoft.Json.JsonConvert.SerializeObject(dm));
                        }
                    }
                }
            for (var i = 0; i < notifyMsg.Count; i += 2)
            {
                var url = notifyMsg[i];
                var sendMsg = notifyMsg[i + 1];
                Startup.sendMsg(url, sendMsg);
            }
        }
    }
}
