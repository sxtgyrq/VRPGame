using CommonClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager4_0.RoomMainF
{
    public partial class RoomMain
    {
        public string updateMagic(MagicSkill ms)
        {
            return this.magicE.updateMagic(ms);
            //throw new NotImplementedException();
        }
        internal void speedMagicChanged(RoleInGame role, ref List<string> notifyMsgs)
        {
            foreach (var item in this._Players)
            {
                if (item.Value.playerType == RoleInGame.PlayerType.player)
                {
                    var player = (Player)item.Value;
                    var url = player.FromUrl;
                    SpeedNotify sn = new SpeedNotify()
                    {
                        c = "SpeedNotify",
                        WebSocketID = player.WebSocketID,
                        Key = role.Key,
                        On = role.improvementRecord.speedValue > 0
                    };

                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(sn);
                    notifyMsgs.Add(url);
                    notifyMsgs.Add(sendMsg);
                }
            }
        }
    }
}
