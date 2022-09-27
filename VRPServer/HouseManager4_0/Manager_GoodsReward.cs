using HouseManager4_0.RoomMainF;
using System.Collections.Generic;

namespace HouseManager4_0
{
    public class Manager_GoodsReward : Manager
    {
        public Manager_GoodsReward(RoomMain roomMain, DrawLine d)
        {
            this.roomMain = roomMain;
            this.drawLine = d;
        }

        internal void ShowConnectionModels(RoleInGame player, int target, ref List<string> notifyMsg)
        {
            var models = Program.dt.models;
            //var fp = Program.dt.GetFpByIndex(target);
            Dictionary<string, double> minLength = new Dictionary<string, double>();
            foreach (var model in models)
            {
                minLength.Add(model.modelID, double.MaxValue);
                foreach (var fpIndex in that._collectPosition)
                {
                    var fp = Program.dt.GetFpByIndex(fpIndex.Value);
                    var length = CommonClass.Geography.getLengthOfTwoPoint.GetDistance(fp.Latitde, fp.Longitude, fp.Height, model.lat, model.lon, 0);
                    if (length < minLength[model.modelID])
                    {
                        minLength[model.modelID] = length;
                    }
                }
            }
            {
                var fp = Program.dt.GetFpByIndex(target);
                List<Data.detailmodel> modelsNeedToShow = new List<Data.detailmodel>();
                {
                    foreach (var model in models)
                    {
                        var length = CommonClass.Geography.getLengthOfTwoPoint.GetDistance(fp.Latitde, fp.Longitude, fp.Height, model.lat, model.lon, 0);
                        // model.
                        if (length < minLength[model.modelID])
                            modelsNeedToShow.Add(model);
                    }
                }
                Program.rm.modelM.setModels(player, modelsNeedToShow, ref notifyMsg);
                Program.rm.goodsM.drawSelect(player, fp, modelsNeedToShow, ref notifyMsg);
            }
        }

        private void drawSelect(RoleInGame role, Model.FastonPosition fp, List<Data.detailmodel> modelsNeedToShow, ref List<string> notifyMsg)
        {
            // throw new System.NotImplementedException();
            if (role.playerType == RoleInGame.PlayerType.player)
                this.drawLine((Player)role, fp, modelsNeedToShow, ref notifyMsg);
        }

        public delegate void DrawLine(Player player, Model.FastonPosition fp, List<Data.detailmodel> modelsNeedToShow, ref List<string> notifyMsg);
        DrawLine drawLine;

        internal List<Data.detailmodel> GetConnectionModels(int startFPIndex)
        {
            List<Data.detailmodel> modelsNeedToShow = new List<Data.detailmodel>();
            var models = Program.dt.models;
            //var fp = Program.dt.GetFpByIndex(target);
            Dictionary<string, double> minLength = new Dictionary<string, double>();
            foreach (var model in models)
            {
                minLength.Add(model.modelID, double.MaxValue);
                foreach (var fpIndex in that._collectPosition)
                {
                    var fp = Program.dt.GetFpByIndex(fpIndex.Value);
                    var length = CommonClass.Geography.getLengthOfTwoPoint.GetDistance(fp.Latitde, fp.Longitude, fp.Height, model.lat, model.lon, 0);
                    if (length < minLength[model.modelID])
                    {
                        minLength[model.modelID] = length;
                    }
                }
            }
            {
                var fp = Program.dt.GetFpByIndex(startFPIndex);
                {
                    foreach (var model in models)
                    {
                        var length = CommonClass.Geography.getLengthOfTwoPoint.GetDistance(fp.Latitde, fp.Longitude, fp.Height, model.lat, model.lon, 0);
                        // model.
                        if (length < minLength[model.modelID])
                            modelsNeedToShow.Add(model);
                    }
                }
            }
            return modelsNeedToShow;
        }
    }
}
