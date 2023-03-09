using CommonClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager4_0.RoomMainF
{
    public partial class RoomMain
    {
        public void SendMsg(DialogMsg dm)
        {
            List<string> notifyMsg = new List<string>();
            lock (this.PlayerLock)
                if (this._Players.ContainsKey(dm.Key))
                {
                    if (this._Players.ContainsKey(dm.To))
                    {
                        //   if()
                        if (dm.Msg.Trim() == "认你做老大")
                        {
                            if (this._Players[dm.Key].playerType == RoleInGame.PlayerType.player)
                            {
                                if (this._Players[dm.To].playerType == RoleInGame.PlayerType.player)
                                {
                                    this.attachE.DealWithMsg(dm, Program.dt); 
                                }
                                else if (this._Players[dm.To].playerType == RoleInGame.PlayerType.NPC)
                                {
                                    this.WebNotify(this._Players[dm.Key], "玩家不可拜NPC为老大！"); 
                                }
                            }
                        }
                        else
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
                }
            Startup.sendSeveralMsgs(notifyMsg);
        }
        /// <summary>
        /// 发送时，采用Key的URL与WebSocekt
        /// </summary>
        /// <param name="dm"></param>
        public void ResponMsg(DialogMsg dm)
        {
            List<string> notifyMsg = new List<string>();
            lock (this.PlayerLock)
                if (this._Players.ContainsKey(dm.Key))
                {
                    if (this._Players[dm.Key].playerType == RoleInGame.PlayerType.player)
                    {
                        notifyMsg.Add(((Player)this._Players[dm.Key]).FromUrl);
                        dm.WebSocketID = ((Player)this._Players[dm.Key]).WebSocketID;
                        notifyMsg.Add(Newtonsoft.Json.JsonConvert.SerializeObject(dm));


                    }
                }
            Startup.sendSeveralMsgs(notifyMsg);
        }

        /// <summary>
        /// 发送时，采用DialogMsg dm 中To 参数的WebSocket
        /// </summary>
        /// <param name="dm"></param>
        public void RequstMsg(DialogMsg dm)
        {
            List<string> notifyMsg = new List<string>();
            lock (this.PlayerLock)
            {
                if (this._Players.ContainsKey(dm.To))
                {
                    if (this._Players[dm.To].playerType == RoleInGame.PlayerType.player)
                    {
                        notifyMsg.Add(((Player)this._Players[dm.To]).FromUrl);
                        dm.WebSocketID = ((Player)this._Players[dm.To]).WebSocketID;
                        notifyMsg.Add(Newtonsoft.Json.JsonConvert.SerializeObject(dm));
                    }
                }
            }
            Startup.sendSeveralMsgs(notifyMsg);
        }
    }
}
