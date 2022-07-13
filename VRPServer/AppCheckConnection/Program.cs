using AppCheckConnection.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppCheckConnection
{
    internal class Program
    {
        static Data dt;
        static void Main(string[] args)
        {
            Console.WriteLine("测试连通性!按任意键继续！");
            Console.ReadLine();
            Program.dt = new Data();
            LoadRoadInfomation();
            var connection = CheckConnection();
            WriteResult(connection);
        }

        private static void WriteResult(bool connection)
        {
            if (connection)
            {
                Program.dt.SaveData();
                Console.WriteLine($"检验结果合格，写入数据！");
            }
            else
            {
                Console.WriteLine($"检验结果不合格，未能写入数据！");
            }
            Console.WriteLine($"按任意键结束！");
            Console.ReadLine();
            //throw new NotImplementedException();
        }

        private static bool CheckConnection()
        {
            var success = true;
            Dictionary<string, Model.FastonPosition> fpSuccess = new Dictionary<string, Model.FastonPosition>();
            // throw new NotImplementedException();
            Model.FastonPosition fp;
            int findIndexOffp = 1;
            Console.WriteLine("开始检验商店");

            {
                //int faltureCount = 0;
                if (Map.FastonPosition.Get(0, out fp))
                {
                    Model.FastonPosition fpCheck;
                    while (Map.FastonPosition.Get(findIndexOffp, out fpCheck))
                    {
                        if (fpSuccess.Count > 0)
                        {
                            var fps = fpSuccess.Values.ToList();
                            var x = (from item in fps orderby Geography.getLengthOfTwoPoint.GetDistance(fpCheck.Latitde, fpCheck.Longitude, item.Latitde, item.Longitude) ascending select item).ToList();
                            fp = x[0];
                        }
                        bool found1, found2;
                        {
                            found1 = CheckM(fpCheck, fp, 10000);
                        }
                        {
                            found2 = CheckM(fp, fpCheck, 10000);
                        }
                        if (found1 && found2)
                        {
                            if (!fpSuccess.ContainsKey(fpCheck.FastenPositionID))
                            {
                                fpSuccess.Add(fpCheck.FastenPositionID, fpCheck);
                            }
                            if (!fpSuccess.ContainsKey(fp.FastenPositionID))
                            {
                                fpSuccess.Add(fp.FastenPositionID, fp);
                            }
                            Console.Write(string.Format(
                                "检验成功：从{0},{1}({2},{3})至{4},{5}({6},{7})",
                                fp.FastenPositionID,
                                fp.FastenPositionName,
                                fp.Longitude,
                                fp.Latitde,
                                fpCheck.FastenPositionID,
                                fpCheck.FastenPositionName,
                                fpCheck.Longitude,
                                fpCheck.Latitde), 1);
                        }
                        else
                        {
                            {
                                Console.Write(string.Format("检验失败：从{0},{1}({2},{3})至{4},{5}({6},{7})",
                                    fp.FastenPositionID,
                                    fp.FastenPositionName,
                             fp.Longitude,
                             fp.Latitde,
                             fpCheck.FastenPositionID,
                             fpCheck.FastenPositionName,
                             fpCheck.Longitude,
                             fpCheck.Latitde), 1);
                                Console.Write(string.Format("检验失败：从{0},{1}({2},{3})至{4},{5}({6},{7})",
                                    fp.FastenPositionID,
                                    fp.FastenPositionName,
                                    fp.Longitude,
                                    fp.Latitde,
                                    fpCheck.FastenPositionID,
                                    fpCheck.FastenPositionName,
                                    fpCheck.Longitude,
                                    fpCheck.Latitde), 2);
                            }
                            while (Console.ReadLine().ToLower() != "c")
                            {
                                Console.WriteLine($"按c继续");
                            }
                            //Console.WriteLine();
                            success = false;
                        }
                        Console.Write(string.Format("{0}检验次数:{1} ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), findIndexOffp), 1);
                        Console.Write("" + Environment.NewLine, 1);
                        findIndexOffp++;
                    }
                }
                else { }
            }
            {
                Console.WriteLine($"开始检验道路");
                List<string> AllRoadsCode = new List<string>();
                foreach (var item in dt.roads)
                {
                    if (!AllRoadsCode.Contains(item.Key.roadCode))
                    {
                        AllRoadsCode.Add(item.Key.roadCode);
                    }
                }
                for (int i = 0; i < AllRoadsCode.Count; i++)
                {
                    Console.WriteLine(string.Format("{0}检验道路{1} 开始检验", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), AllRoadsCode[i]), 1);
                    if (fpSuccess.Count > 0)
                    {
                        var fps = fpSuccess.Values.ToList();
                        var x = (from item in fps orderby Geography.getLengthOfTwoPoint.GetDistance(dt.roads[new MapGo.passedRoad(AllRoadsCode[i], 0)].startLatitude, dt.roads[new MapGo.passedRoad(AllRoadsCode[i], 0)].startLongitude, item.Latitde, item.Longitude) ascending select item).ToList();
                        fp = x[0];
                    }

                    var found = CheckM(fp, AllRoadsCode[i], 10000);
                    //Path.PathAnt pa = new Path.PathAnt(this.City, AllRoadsCode[i], 0, 0, fp.FastenPositionID, gXYP, gs, existTheRoad);
                    //string msg;
                    //bool found;
                    //pa.FindPosition(out msg, out found);
                    if (!found)
                    {
                        Console.WriteLine($"循环{i}--{AllRoadsCode[i]}校验失败！");
                        while (Console.ReadLine().ToLower() != "c")
                        {
                            Console.WriteLine($"按c继续");
                        }
                        success = false;
                        //sm(msg, 2);
                        //faltureCount++;
                    }
                    else
                    {
                        Console.WriteLine($"循环{i}--{AllRoadsCode[i]}校验成功！");
                    }
                }
            }
            return success;
        }
        class roadForCheck
        {
            public roadForCheck(double limit, int round)
            {
                // this.Code = code;
                this.Limit = limit;
                this.Round = round;
            }
            //public string Code { get; set; }
            public double Limit { get; set; }
            public int Round { get; set; }
        }
        private static bool CheckM(FastonPosition fpCheck, string roadCodeOfCheckingRoad, int maxCount)
        {
            Dictionary<string, roadForCheck> roadCodes = new Dictionary<string, roadForCheck>();
            double limitOfFp = 0;
            roadCodes.Add(roadCodeOfCheckingRoad, new roadForCheck(limitOfFp, 0));

            for (int indexOfCalRound = 0; indexOfCalRound < maxCount; indexOfCalRound++)
            {
                var roadCodesneedOperate = (from roadCode in roadCodes.ToArray() where roadCode.Value.Round == indexOfCalRound select roadCode.Key).ToList();
                if (roadCodesneedOperate.Count == 0)
                {
                    return false;
                }
                for (int indexOfRoadCode = 0; indexOfRoadCode < roadCodesneedOperate.Count; indexOfRoadCode++)
                {
                    List<string> roadCodesOfItem;
                    List<double> limitsOfItem;
                    dt.GetCrosses(roadCodesneedOperate[indexOfRoadCode], roadCodes[roadCodesneedOperate[indexOfRoadCode]].Limit, out roadCodesOfItem, out limitsOfItem);
                    for (int indexOfResult = 0; indexOfResult < roadCodesOfItem.Count; indexOfResult++)
                    {
                        if (roadCodes.ContainsKey(roadCodesOfItem[indexOfResult]))
                        {
                            if (limitsOfItem[indexOfResult] < roadCodes[roadCodesOfItem[indexOfResult]].Limit)
                            {
                                roadCodes[roadCodesOfItem[indexOfResult]] = new roadForCheck(limitsOfItem[indexOfResult], indexOfCalRound + 1);
                            }
                        }
                        else
                        {
                            roadCodes.Add(roadCodesOfItem[indexOfResult], new roadForCheck(limitsOfItem[indexOfResult], indexOfCalRound + 1));
                        }
                    }

                    List<string> FastonPositionPass;
                    dt.getFastonPosition(roadCodesneedOperate[indexOfRoadCode], roadCodes[roadCodesneedOperate[indexOfRoadCode]].Limit, out FastonPositionPass);
                    if (FastonPositionPass.Contains(fpCheck.FastenPositionID))
                        return true;
                }
            }
            return false;
        }

        private static bool CheckM(FastonPosition fpCheck, FastonPosition fp, int maxCount)
        {
            Dictionary<string, roadForCheck> roadCodes = new Dictionary<string, roadForCheck>();
            //Dictionary<string, int> rounds = new Dictionary<string, int>();
            //List<string> roadCodes = new List<string>();
            //Dictionary<string, double> limitPosition = new Dictionary<string, double>();
            double limitOfFp;
            if (dt.roads[new Model.MapGo.passedRoad(fp.RoadCode, fp.RoadOrder)].CarInOpposeDirection == 0)
                limitOfFp = fp.RoadOrder + fp.RoadPercent;
            else
                limitOfFp = 0;
            roadCodes.Add(fp.RoadCode, new roadForCheck(limitOfFp, 0));

            //rounds.Add(fp.RoadCode, 0);

            for (int i = 0; i < maxCount; i++)
            {
                var roadCodesneedOperate = (from roadCode in roadCodes.ToArray() where roadCode.Value.Round == i select roadCode.Key).ToList();
                if (roadCodesneedOperate.Count == 0)
                {
                    return false;
                }
                for (int j = 0; j < roadCodesneedOperate.Count; j++)
                {
                    List<string> roadCodesOfItem;
                    List<double> limitsOfItem;
                    dt.GetCrosses(roadCodesneedOperate[j], roadCodes[roadCodesneedOperate[j]].Limit, out roadCodesOfItem, out limitsOfItem);
                    for (int k = 0; k < roadCodesOfItem.Count; k++)
                    {
                        if (roadCodes.ContainsKey(roadCodesOfItem[k]))
                        {

                            if (limitsOfItem[k] < roadCodes[roadCodesOfItem[k]].Limit)
                            {
                                roadCodes[roadCodesOfItem[k]] = new roadForCheck(limitsOfItem[k], i + 1);
                            }
                        }
                        else
                        {
                            roadCodes.Add(roadCodesOfItem[k], new roadForCheck(limitsOfItem[k], i + 1));
                        }
                    }

                    List<string> FastonPositionPass;
                    dt.getFastonPosition(roadCodesneedOperate[j], roadCodes[roadCodesneedOperate[j]].Limit, out FastonPositionPass);
                    if (FastonPositionPass.Contains(fpCheck.FastenPositionID))
                        return true;
                }
            }
            return false;
        }

        static void LoadRoadInfomation()
        {
            // this.button1.Enabled = false;
            Program.dt.LoadRoadInfomation();
            Console.WriteLine("数据加载完成！按任意键-开始计算！");

        }
    }
}
