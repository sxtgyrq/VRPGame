using CommonClass;
using HouseManager4_0.RoomMainF;
using System;
using System.Collections.Generic;

namespace HouseManager4_0
{
    public class Manager_Level : Manager
    {
        public Manager_Level(RoomMain roomMain)
        {
            this.roomMain = roomMain;
        }

        internal void OrderToUpdateLevel(string key, string addr, string signature)
        {
            // ots.
            List<string> notifyMsg = new List<string>();
            if (BitCoin.Sign.checkSign(signature, key, addr))
            {

                lock (that.PlayerLock)
                    if (that._Players.ContainsKey(key))
                    {
                        if (!that._Players[key].Bust)
                        {
                            var role = that._Players[key];
                            if (role.playerType == RoleInGame.PlayerType.player)
                            {
                                var player = (Player)role;
                                if (string.IsNullOrEmpty(player.levelObj.BtcAddr))
                                {
                                    player.levelObj.SetAddr(player.levelObj.BtcAddr);
                                }
                                else if (player.levelObj.BtcAddr == addr)
                                {
                                }
                                else
                                {
                                    this.WebNotify(player, $"只能设置一次荣誉地址，且你的荣誉地址为{player.levelObj.BtcAddr}");
                                    return;
                                }
                                this.synchronize(player, ref notifyMsg);
                            }
                        }
                    }
            }
            else
            {
                //Consol.WriteLine($"检验签名失败,{ots.Key},{ots.signature},{ots.address}");
            }
            for (var i = 0; i < notifyMsg.Count; i += 2)
            {
                var url = notifyMsg[i];
                var sendMsg = notifyMsg[i + 1];
                //Consol.WriteLine($"url:{url}");
                Startup.sendMsg(url, sendMsg);
            }
        }

        private void synchronize(Player player, ref List<string> notifyMsg)
        {
            var result = DalOfAddress.LevelForSave.Update(player.levelObj.BtcAddr, player.levelObj.TimeStampStr, player.Level);
            if (result != null)
            {
                player.levelObj.SetLevel(result.Level);
                player.levelObj.SetTimeStamp(result.TimeStampStr);
            }
        }

        internal void OrderToUpdateLevel(RoleInGame role, ref List<string> notifyMsgs)
        {
            if (role.playerType == RoleInGame.PlayerType.player)
            {
                var player = (Player)role;
                if (!string.IsNullOrEmpty(player.levelObj.BtcAddr))
                {
                    synchronize(player, ref notifyMsgs);
                }
            }
        }
    }
}
