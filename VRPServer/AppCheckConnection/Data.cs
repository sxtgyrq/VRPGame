using AppCheckConnection.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AppCheckConnection
{
    internal partial class Data
    {
        // static string rootPath = "E:\\DB\\DBPublish";
        public Dictionary<Model.MapGo.passedRoad, Model.SaveRoad.RoadInfo> roads;

        internal void LoadRoadInfomation()
        {
            roads = new Dictionary<MapGo.passedRoad, SaveRoad.RoadInfo>();
            int index = 0;
            string code;
            while (HasTheIndex(index, out code))
            {
                var strings = code.Split(',');
                string roadPresentCode = strings[0];
                int roadOrder = Convert.ToInt32(strings[1]);

                var item = Map.Road.Get(roadPresentCode, roadOrder);
                roads.Add(new Model.MapGo.passedRoad(roadPresentCode, roadOrder), item);
                index++;
            }
            Console.WriteLine($"加载了{index}条数据！");
        }
        private bool HasTheIndex(int index, out string presentCode)
        {
            return Map.RoadTag.GetIndexPresentCode(index, out presentCode);
        }

        internal void GetCrosses(string roadCode, double limit, out List<string> roadCodes, out List<double> limits)
        {
            roadCodes = new List<string>();
            limits = new List<double>();
            int startRoadOrder = 0;
            while (this.roads.ContainsKey(new Model.MapGo.passedRoad(roadCode, startRoadOrder)))
            {
                var c1 = this.roads[new Model.MapGo.passedRoad(roadCode, startRoadOrder)].Cross1;
                checkCross(c1, roadCode, limit, ref roadCodes, ref limits);
                var c2 = this.roads[new Model.MapGo.passedRoad(roadCode, startRoadOrder)].Cross2;
                checkCross(c2, roadCode, limit, ref roadCodes, ref limits);
                startRoadOrder++;
            }

        }

        private void checkCross(SaveRoad.DictCross[] c1, string roadCode, double limit, ref List<string> roadCodes, ref List<double> limits)
        {
            for (int i = 0; i < c1.Length; i++)
            {
                //if (c1[i].CrossState == 1) 
                //{
                if (c1[i].CrossState == 1)
                    checkCrossOfIndex(roadCode, limit, c1[i].RoadCode1, c1[i].RoadOrder1 + c1[i].Percent1, c1[i].RoadCode2, c1[i].RoadOrder2, c1[i].Percent2, ref roadCodes, ref limits);
                else if (c1[i].CrossState == 3)//新道路的入口在cross  roadCode2上，入口在roadCode1上；
                    checkCrossOfIndex(roadCode, limit, c1[i].RoadCode1, c1[i].RoadOrder1 + c1[i].Percent1, c1[i].RoadCode2, c1[i].RoadOrder2, c1[i].Percent2, ref roadCodes, ref limits);
                //}
                if (c1[i].CrossState == 1)
                    checkCrossOfIndex(roadCode, limit, c1[i].RoadCode2, c1[i].RoadOrder2 + c1[i].Percent2, c1[i].RoadCode1, c1[i].RoadOrder1, c1[i].Percent1, ref roadCodes, ref limits);
                else if (c1[i].CrossState == 2)
                    checkCrossOfIndex(roadCode, limit, c1[i].RoadCode2, c1[i].RoadOrder2 + c1[i].Percent2, c1[i].RoadCode1, c1[i].RoadOrder1, c1[i].Percent1, ref roadCodes, ref limits);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roadCode">用于判断是否为要检验</param>
        /// <param name="limit">用于判断是否为要检验</param>
        /// <param name="crossRoadCodeEqual">用于判断是否为要检验</param>
        /// <param name="crossRoadOrderAndPercent">用于判断是否为要检验</param>
        /// <param name="crossAnotherRoadCode"></param>
        /// <param name="crossAnotherRoadOrder"></param>
        /// <param name="crossAnotherPercent"></param>
        /// <param name="roadCodes"></param>
        /// <param name="limits"></param>
        private void checkCrossOfIndex(string roadCode, double limit, string crossRoadCodeEqual, double crossRoadOrderAndPercent, string crossAnotherRoadCode, int crossAnotherRoadOrder, double crossAnotherPercent, ref List<string> roadCodes, ref List<double> limits)
        {
            if (roadCode == crossRoadCodeEqual)
            {
                if (crossRoadOrderAndPercent > limit)
                {
                    roadCodes.Add(crossAnotherRoadCode);
                    if (this.roads[new Model.MapGo.passedRoad(crossAnotherRoadCode, crossAnotherRoadOrder)].CarInOpposeDirection == 0)
                    {
                        limits.Add(crossAnotherRoadOrder + crossAnotherPercent);
                    }
                    else
                    {
                        limits.Add(0);
                    }
                }
            }
        }

        internal void getFastonPosition(string roadCode, double limit, out List<string> fastonPositionPass)
        {
            fastonPositionPass = new List<string>();
            int startRoadOrder = 0;
            while (this.roads.ContainsKey(new Model.MapGo.passedRoad(roadCode, startRoadOrder)))
            {
                var ps = this.roads[new Model.MapGo.passedRoad(roadCode, startRoadOrder)].PassShop;
                for (int i = 0; i < ps.Length; i++)
                {
                    if (ps[i].RoadOrder + ps[i].RoadPercent > limit)
                    {
                        fastonPositionPass.Add(ps[i].FastenPositionID);
                    }
                }
                startRoadOrder++;
            }
        }

        public void SaveData()
        {
            Dictionary<string, Dictionary<int, SaveRoad.RoadInfo>> roadsForSave = new Dictionary<string, Dictionary<int, SaveRoad.RoadInfo>>();
            foreach (var item in this.roads)
            {
                if (roadsForSave.ContainsKey(this.roads[item.Key].RoadCode))
                {
                }
                else
                {
                    roadsForSave.Add(this.roads[item.Key].RoadCode, new Dictionary<int, SaveRoad.RoadInfo>());
                }
                roadsForSave[this.roads[item.Key].RoadCode].Add(this.roads[item.Key].RoadOrder, item.Value);
            }
            var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(roadsForSave);
            //  throw new Exception("");

            string path = $"{Config.rootPath}\\allroaddata.txt";
            File.WriteAllText(path, jsonData);
        }
    }
}
