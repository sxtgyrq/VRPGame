using CommonClass;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager4_0.RoomMainF
{
    public partial class RoomMain
    {
        private void DrawGoodsSelection(Player player, FastonPosition fp, List<Data.detailmodel> modelsNeedToShow, ref List<string> notifyMsg)
        {
            //    throw new NotImplementedException();
            var url = player.FromUrl;
            List<string> selections = new List<string>();
            List<double> positions = new List<double>();
            for (int i = 0; i < modelsNeedToShow.Count; i++)
            {
                selections.Add(modelsNeedToShow[i].modelID);
                positions.Add(modelsNeedToShow[i].x);
                positions.Add(modelsNeedToShow[i].y);
                positions.Add(modelsNeedToShow[i].z);
            }
            double x, y, z;
            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(fp.positionLongitudeOnRoad, fp.positionLatitudeOnRoad, fp.Height, out x, out y, out z);
            GoodsSelectionNotify tn = new GoodsSelectionNotify()
            {
                c = "GoodsSelectionNotify",
                WebSocketID = player.WebSocketID,
                x = x,
                z = -y,
                y = z,
                selections = selections.ToArray(),
                positions = positions.ToArray()
            };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(tn);
            notifyMsg.Add(url);
            notifyMsg.Add(json);
        }
    }
}
