using CommonClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager4_0.RoomMainF
{
    public partial class RoomMain
    {
        public void GetBackground(Player player, ref List<string> notifyMsg)
        {
            var ti = player.getCar().targetFpIndex;
            if (ti >= 0)
            {
                var fs = Program.dt.GetFpByIndex(ti);
                var code = this.bg.getPathByRegion(fs.FastenPositionID, fs.FastenType, fs.region);
                var infomation = Program.rm.GetBackgroundInfomation(player.WebSocketID, code);
                var url = player.FromUrl;
                var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(infomation);
                notifyMsg.Add(url);
                notifyMsg.Add(sendMsg);
            }
        }

        private BradCastBackground GetBackgroundInfomation(int webSocketID, string code)
        {
            var obj = new BradCastBackground
            {
                c = "BradCastBackground",
                WebSocketID = webSocketID,
                path = code
            };
            return obj;
        }
    }
}
