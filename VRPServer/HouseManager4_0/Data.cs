using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using static CommonClass.PathCal;
using OssModel = Model;
namespace HouseManager4_0
{

    public interface GetRandomPos
    {
        //GetFpCount
        public int GetFpCount();
        public OssModel.FastonPosition GetFpByIndex(int indexValule);
        public List<OssModel.MapGo.nyrqPosition> GetAFromB(int start, int end);
        public OssModel.SaveRoad.RoadInfo GetItemRoadInfo(OssModel.MapGo.nyrqPosition nyrqPosition);
        public OssModel.SaveRoad.RoadInfo GetItemRoadInfo(string roadCode, int roadOrder);
        public OssModel.SaveRoad.RoadInfo GetItemRoadInfo(string roadCode, int roadOrder, out bool existed);
        public void GetAFromBPoint(List<OssModel.MapGo.nyrqPosition> dataResult, OssModel.MapGo.nyrqPosition position, int speed, ref List<int> result, ref int startT, bool speedImproved, RoomMainF.RoomMain rmain);
    }

    public partial class Data : GetRandomPos
    {
        public int GetFpCount()
        {
            return this._allFp.Count;
        }

        public OssModel.FastonPosition GetFpByIndex(int indexValule)
        {

            if (indexValule >= this._allFp.Count || indexValule < 0)
            {
                return null;
            }
            var fp = this._allFp[indexValule];
            return fp;
        }

        public List<OssModel.MapGo.nyrqPosition> GetAFromB(int start, int end)
        {
            return this.pathCal.GetAFromB(start, end);
        }
        public OssModel.SaveRoad.RoadInfo GetItemRoadInfo(OssModel.MapGo.nyrqPosition nyrqPosition)
        {
            return this._road[nyrqPosition.roadCode][nyrqPosition.roadOrder];
        }
        public OssModel.SaveRoad.RoadInfo GetItemRoadInfo(string roadCode, int roadOrder)
        {
            return this._road[roadCode][roadOrder];
        }
        public OssModel.SaveRoad.RoadInfo GetItemRoadInfo(string roadCode, int roadOrder, out bool existed)
        {
            if (this._road.ContainsKey(roadCode))
            {
                if (this._road[roadCode].ContainsKey(roadOrder))
                {
                    existed = true;
                    return this._road[roadCode][roadOrder];
                }
                else
                {
                    existed = false;
                    return null;
                }
            }
            else
            {
                existed = false;
                return null;
            }

        }
        public void GetAFromBPoint(List<OssModel.MapGo.nyrqPosition> dataResult, OssModel.MapGo.nyrqPosition position, int speed, ref List<int> result, ref int startT, bool speedImproved, RoomMainF.RoomMain rmain)
        {
            for (var i = 0; i < dataResult.Count; i++)
            {
                if (i == 0)
                {
                    //var startX = result.Last().x1;
                    //var startY = result.Last().y1;
                    double startX, startY, startZ;
                    CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(position.BDlongitude, position.BDlatitude, position.BDheight, out startX, out startY, out startZ);

                    //var length=

                    double endX, endY, endZ;
                    CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(dataResult[i].BDlongitude, dataResult[i].BDlatitude, dataResult[i].BDheight, out endX, out endY, out endZ);
                    //  var interview =
                    var interview = Convert.ToInt32(CommonClass.Geography.getLengthOfTwoPoint.GetDistance(position.BDlatitude, position.BDlongitude, position.BDheight, dataResult[i].BDlatitude, dataResult[i].BDlongitude, dataResult[i].BDheight) / dataResult[i].maxSpeed * 3.6 / 20 * 1000 * 50 / speed);

                    interview = rmain.magicE.shotTime(interview, speedImproved);

                    if (result.Count == 0)
                    {
                        var animate0 = new PathResult4()
                        {
                            t = startT,
                            x = 0,
                            y = 0,
                            z = 0
                        };
                        {
                            result.Add(animate0.x);
                            result.Add(animate0.y);
                            result.Add(animate0.z);
                            result.Add(animate0.t);
                        }
                        // result.Add(animate0);
                    }
                    var animate1 = new PathResult4()
                    {
                        t = interview,
                        x = Convert.ToInt32((endX - startX) * 256),
                        y = Convert.ToInt32((endY - startY) * 256),
                        z = Convert.ToInt32((endZ - startZ) * 256),
                    };
                    if (animate1.x != 0 || animate1.y != 0)  // if (animate1.t != 0)
                    {
                        result.Add(animate1.x);
                        result.Add(animate1.y);
                        result.Add(animate1.z);
                        result.Add(animate1.t);
                    }
                    startT += interview;
                }
                else if (dataResult[i].roadCode == dataResult[i - 1].roadCode)
                {

                    double startX, startY, startZ;
                    CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(dataResult[i - 1].BDlongitude, dataResult[i - 1].BDlatitude, dataResult[i - 1].BDheight, out startX, out startY, out startZ);

                    double endX, endY, endZ;
                    CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(dataResult[i].BDlongitude, dataResult[i].BDlatitude, dataResult[i].BDheight, out endX, out endY, out endZ);

                    // var length = CommonClass.Geography.getLengthOfTwoPoint.
                    //  var interview =
                    var interview = Convert.ToInt32(CommonClass.Geography.getLengthOfTwoPoint.GetDistance(dataResult[i - 1].BDlatitude, dataResult[i - 1].BDlongitude, dataResult[i - 1].BDheight, dataResult[i].BDlatitude, dataResult[i].BDlongitude, dataResult[i].BDheight) / dataResult[i].maxSpeed * 3.6 / 20 * 1000 * 50 / speed);

                    interview = rmain.magicE.shotTime(interview, speedImproved);

                    var animate1 = new Data.PathResult4()
                    {
                        x = Convert.ToInt32((endX - startX) * 256),
                        y = Convert.ToInt32((endY - startY) * 256),
                        z = Convert.ToInt32((endZ - startZ) * 256),
                        t = interview
                    };
                    startT += interview;
                    //if (animate1.t != 0)
                    if (animate1.x != 0 || animate1.y != 0)
                    {
                        result.Add(animate1.x);
                        result.Add(animate1.y);
                        result.Add(animate1.z);
                        result.Add(animate1.t);
                    }
                    // result.Add(animate1);
                }
            }
        }
    }
    public partial class Data
    {

        public class PathCal
        {
            List<int> allDirect;
            List<int> fpDirect;
            int countFpCount { get; set; }
            List<CommonClass.PathCal.CalModel_V2> calMaterial = new List<CommonClass.PathCal.CalModel_V2>();
            int countOFMaterial;
            Dictionary<string, Dictionary<int, OssModel.SaveRoad.RoadInfo>> road;
            Data data;

            string dataDictionary;

            public PathCal(Data data_, string dataDictionary_)
            {
                this.data = data_;
                this.countFpCount = this.data.GetFpCount();
                this.data.GetData(out road);
                this.dataDictionary = dataDictionary_;
            }

            internal void cal(bool unitTest)
            {
                this.calMaterial = new List<CalModel_V2>();
                for (int indexOfFP = 0; indexOfFP < countFpCount; indexOfFP++)
                {
                    var fp = data.GetFpByIndex(indexOfFP);
                    // var end = dt.GetFpByIndex(98);

                    calMaterial.Add(new CalModel_V2()
                    {
                        RoadCode = fp.RoadCode,
                        RoadOrder = fp.RoadOrder,
                        Percent = fp.RoadPercent,
                        AnotherRoadCode = "",
                        AnotherRoadOrder = -1,
                        AnotherIndex = -1,
                        Last = -1,
                        MType = ModelType.End,
                        ToNext = -1,
                        ToPrevious = -1,
                        Pass = 0,
                        FastenPositionID = fp.FastenPositionID
                    });
                }
                foreach (var roadsItem in this.road)
                {
                    string RoadCode = roadsItem.Key;
                    var roads = roadsItem.Value;
                    foreach (var roadItem in roads)
                    {
                        int RoadOrder = roadItem.Key;
                        var road = roadItem.Value;
                        foreach (var cross in road.Cross1)
                        {
                            if (cross.CrossState > 0)
                            {
                                /*
                                 * CrossState与entranceORExit有对应关系
                                 * 术语出口，这里的出口代表一条线路的出口
                                 * 属于入口，这里的入口代表另一条陷入的入口
                                 * 即从出口离开一条线路，从入口进入另一条线路。
                                 * 
                                 * CrossState=1,代表这个东西既是入口也是出口；
                                 * CrossState=2,代表在roadCode1是出口，roadcode2是入口
                                 * CrossState=3,代表在roadcode2是出口，roadCode1是入口
                                 * 
                                 * entranceORExit=6,代表可出可入
                                 * entranceORExit=3,代表入口
                                 * entranceORExit=2,代表出口
                                 * 
                                 * CrossState=1,可出可入,entranceORExit=6;
                                 * 
                                 * CrossState=2,在roadCode1,表示出口,entranceORExit=2;
                                 * --------反之,在roadCode2,表示入口,entranceORExit=3;
                                 * 
                                 * CrossState=3,在roadCode2,表示出口,entranceORExit=2;
                                 * --------反之,在roadCode1,表示入口,entranceORExit=3;
                                 */
                                int entranceORExit;
                                switch (cross.CrossState)
                                {
                                    case 1: entranceORExit = 6; break;
                                    case 2: entranceORExit = 2; break;
                                    case 3: entranceORExit = 3; break;
                                    default: throw new Exception("");
                                }
                                var Percent = cross.Percent1;
                                var AnotherRoadCode = cross.RoadCode2;
                                var AnotherRoadOrder = cross.RoadOrder2;
                                calMaterial.Add(new CalModel_V2()
                                {
                                    RoadCode = RoadCode,
                                    RoadOrder = RoadOrder,
                                    Percent = Percent,
                                    AnotherRoadCode = AnotherRoadCode,
                                    AnotherRoadOrder = AnotherRoadOrder,
                                    AnotherIndex = -1,
                                    Last = -1,
                                    MType = ModelType.Cross,
                                    ToNext = -1,
                                    ToPrevious = -1,
                                    Pass = 0,
                                    FastenPositionID = "",
                                    EntranceORExit = entranceORExit
                                });
                            }
                        }
                        foreach (var cross in road.Cross2)
                        {
                            if (cross.CrossState > 0)
                            {
                                int entranceORExit;
                                switch (cross.CrossState)
                                {
                                    case 1: entranceORExit = 6; break;
                                    case 2: entranceORExit = 3; break;
                                    case 3: entranceORExit = 2; break;
                                    default: throw new Exception("");
                                }
                                var Percent = cross.Percent2;
                                var AnotherRoadCode = cross.RoadCode1;
                                var AnotherRoadOrder = cross.RoadOrder1;
                                calMaterial.Add(new CalModel_V2()
                                {
                                    RoadCode = RoadCode,
                                    RoadOrder = RoadOrder,
                                    Percent = Percent,
                                    AnotherRoadCode = AnotherRoadCode,
                                    AnotherRoadOrder = AnotherRoadOrder,
                                    AnotherIndex = -1,
                                    Last = -1,
                                    MType = ModelType.Cross,
                                    ToNext = -1,
                                    ToPrevious = -1,
                                    Pass = 0,
                                    FastenPositionID = "",
                                    EntranceORExit = entranceORExit
                                });
                            }
                        }
                    }
                }
                var newMaterial = (from item in calMaterial orderby item.RoadCode ascending, item.RoadOrder ascending, item.Percent ascending select item).ToList();
                this.countOFMaterial = newMaterial.Count;
                this.calMaterial = newMaterial;
                var conten1 = File.ReadAllText($"{ this.dataDictionary}fpOrder.json");
                this.fpDirect = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(conten1);
                var conten2 = File.ReadAllText($"{ this.dataDictionary}resultommm22x.txt");
                this.allDirect = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(conten2);
                Console.WriteLine($"加载的数据{ this.fpDirect.Count},{this.allDirect.Count}{"\r\n"}任意键继续");
                if (unitTest) { }
                else
                {
                    Console.ReadLine();
                }
            }

            internal List<OssModel.MapGo.nyrqPosition> GetAFromB(int start, int end)
            {
                if (start == end)
                {
                    return new List<OssModel.MapGo.nyrqPosition>();
                }
                else
                {
                    // var result = new List<OssModel.MapGo.nyrqPosition>();
                    var endIndex = this.fpDirect[end];//目标地点 
                    var startIndex = start;//起始地点
                    var listResult = new List<int>();
                    int direct = endIndex;
                    int calCount = 0;
                    do
                    {
                        listResult.Insert(0, direct + 0);
                        direct = this.allDirect[direct + startIndex * this.countOFMaterial];
                        calCount++;
                        if (calCount >= this.countOFMaterial)
                        {
                            throw new Exception("错误");
                        }
                    }
                    while (direct >= 0);
                    if (direct == -2)
                    {
                        //表示结果正确
                    }
                    else
                    {
                        throw new Exception("数据错误！");
                    }

                    List<CommonClass.PathCal.CalModel_V2> pathCal = new List<CommonClass.PathCal.CalModel_V2>();
                    for (int i = 0; i < listResult.Count; i++)
                    {
                        pathCal.Add(this.calMaterial[listResult[i]]);
                    }
                    delMidMoreInfo(ref pathCal);
                    var result = changeType(pathCal);
                    addMidUsefulInfo(ref result);
                    return result;
                }
            }

            private List<OssModel.MapGo.nyrqPosition> changeType(List<CalModel_V2> pathCal)
            {
                var result = new List<OssModel.MapGo.nyrqPosition>();
                for (int i = 0; i < pathCal.Count; i++)
                {
                    var io = pathCal[i];
                    var lon = this.road[io.RoadCode][io.RoadOrder].startLongitude + (this.road[io.RoadCode][io.RoadOrder].endLongitude - this.road[io.RoadCode][io.RoadOrder].startLongitude) * io.Percent;
                    var lat = this.road[io.RoadCode][io.RoadOrder].startLatitude + (this.road[io.RoadCode][io.RoadOrder].endLatitude - this.road[io.RoadCode][io.RoadOrder].startLatitude) * io.Percent;
                    var height = this.road[io.RoadCode][io.RoadOrder].startHeight + (this.road[io.RoadCode][io.RoadOrder].endHeight - this.road[io.RoadCode][io.RoadOrder].startHeight) * io.Percent;
                    var maxSpeed = this.road[io.RoadCode][io.RoadOrder].MaxSpeed;
                    result.Add(new OssModel.MapGo.nyrqPosition(io.RoadCode, io.RoadOrder, io.Percent, lon, lat, height, maxSpeed));
                }
                return result;
            }

            enum Direction { Up, Down, Null }
            //class UsefulInfoMation
            //{
            //    public string roadCode;
            //    public int roadOrder;
            //    public Direction direction;
            //}
            private void addMidUsefulInfo(ref List<OssModel.MapGo.nyrqPosition> result)
            {
                string roadCode;
                int roadOrder;
                Direction direction;
                int indexFound;
                while (findIndexWhichNeedToFix(ref result, out indexFound, out roadCode, out roadOrder, out direction))
                {
                    {
                        double percent, lon, lat, height;
                        if (direction == Direction.Up)
                        {
                            percent = 0;
                            lon = this.road[roadCode][roadOrder].startLongitude;
                            lat = this.road[roadCode][roadOrder].startLatitude;
                            height = this.road[roadCode][roadOrder].startHeight;
                        }
                        else if (direction == Direction.Down)
                        {
                            percent = 1;
                            lon = this.road[roadCode][roadOrder].endLongitude;
                            lat = this.road[roadCode][roadOrder].endLatitude;
                            height = this.road[roadCode][roadOrder].endHeight;
                        }
                        else
                        {
                            throw new Exception("");
                        }
                        result.Insert(indexFound, new OssModel.MapGo.nyrqPosition(roadCode, roadOrder, percent, lon, lat, height, this.road[roadCode][roadOrder].MaxSpeed));
                    }
                    {
                        double percent, lon, lat, height;
                        // int roadOrder;
                        if (direction == Direction.Up)
                        {
                            roadOrder--;
                            percent = 1;
                            lon = this.road[roadCode][roadOrder].endLongitude;
                            lat = this.road[roadCode][roadOrder].endLatitude;
                            height = this.road[roadCode][roadOrder].endHeight;
                        }
                        else if (direction == Direction.Down)
                        {
                            roadOrder++;
                            percent = 0;
                            lon = this.road[roadCode][roadOrder].startLongitude;
                            lat = this.road[roadCode][roadOrder].startLatitude;
                            height = this.road[roadCode][roadOrder].startHeight;
                        }
                        else
                        {
                            throw new Exception("");
                        }
                        result.Insert(indexFound, new OssModel.MapGo.nyrqPosition(roadCode, roadOrder, percent, lon, lat, height, this.road[roadCode][roadOrder].MaxSpeed));
                    }
                }
                // throw new NotImplementedException();
            }

            private bool findIndexWhichNeedToFix(ref List<OssModel.MapGo.nyrqPosition> result, out int indexFound, out string roadCode, out int roadOrder, out Direction direction)
            {
                for (int i = 1; i < result.Count; i += 2)
                {
                    if (result[i].roadCode != result[i - 1].roadCode)
                    {
                        throw new Exception("格式错误");
                    }
                    else if (result[i].roadOrder != result[i - 1].roadOrder)
                    {
                        if (result[i].roadOrder > result[i - 1].roadOrder)
                        {
                            direction = Direction.Up;
                        }
                        else
                            direction = Direction.Down;
                        indexFound = i;
                        roadCode = result[i].roadCode;
                        roadOrder = result[i].roadOrder;
                        return true;
                    }
                    else
                    {
                        continue;
                    }
                }
                indexFound = -1;
                roadCode = null;
                roadOrder = -1;
                direction = Direction.Null;
                return false;
                //  throw new NotImplementedException();
            }

            private void delMidMoreInfo(ref List<CalModel_V2> pathCal)
            {
                int indexWhichIsMore;
                while (findIndexWhichIsMore(ref pathCal, out indexWhichIsMore))
                {
                    pathCal.RemoveAt(indexWhichIsMore);
                }
                if (pathCal.Count % 2 != 0)
                {
                    throw new Exception("意料之外");
                }
                // throw new NotImplementedException();
            }

            private bool findIndexWhichIsMore(ref List<CalModel_V2> pathCal, out int index)
            {
                for (int i = 1; i < pathCal.Count - 1; i++)
                {
                    if (pathCal[i].RoadCode == pathCal[i - 1].RoadCode && pathCal[i].RoadCode == pathCal[i + 1].RoadCode)
                    {
                        index = i;
                        return true;
                    }
                }
                index = -1;
                return false;
                // throw new NotImplementedException();
            }
        }



        public Dictionary<string, string> AllCrossesBGData { get; private set; }
        public Dictionary<string, string> AllCrossesBGData_ { get; private set; }
        public Dictionary<string, int> CrossesNotHaveBGData { get; private set; }
        internal void LoadCrossBackground()
        {
            this.AllCrossesBGData = DalOfAddress.backgroundjpg.GetAllKey();
            this.AllCrossesBGData_ = new Dictionary<string, string>();
            foreach (var item in this.AllCrossesBGData)
            {
                this.AllCrossesBGData_.Add(item.Value, item.Key);
            }

            this.CrossesNotHaveBGData = new Dictionary<string, int>();
        }

        /// <summary>
        /// all road infomation
        /// </summary>
        Dictionary<string, Dictionary<int, OssModel.SaveRoad.RoadInfo>> _road;

        /// <summary>
        /// all shop infomation
        /// </summary>
        List<OssModel.FastonPosition> _allFp;
        // string rewardRecord = "";

        public string IP { get; private set; }

        public Dictionary<int, OssModel.SaveRoad.RoadInfo> GetFirst(out string roadCode)
        {
            var first = this._road.First();
            roadCode = first.Key;
            return first.Value;
        }

        PathCal pathCal;

        public void LoadRoad()
        {
            this.LoadRoad(Program.boundary, false);
        }
        public void LoadRoad(Geometry.GetBoundryF f, bool unitTest)
        {
            var rootPath = System.IO.Directory.GetCurrentDirectory();
            //Consol.WriteLine($"path:{rootPath}");
            var roadPath = $"{rootPath}\\DBPublish\\allroaddata.txt";

            // "fpDictionary": "F:\\MyProject\\VRPWithZhangkun\\MainApp\\DBPublish\\",
            var fpDictionary = $"{rootPath}\\DBPublish\\";

            string json;
            using (StreamReader sr = new StreamReader(roadPath))
            {
                json = sr.ReadToEnd();
            }

            this._road = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Dictionary<int, OssModel.SaveRoad.RoadInfo>>>(json);

            this._allFp = GetAllFp(fpDictionary, f);

            //Consol.WriteLine($"{this._road.Count}_{this._allFp.Count}");

            this.pathCal = new PathCal(this, fpDictionary);
            this.pathCal.cal(unitTest);
        }




        static List<OssModel.FastonPosition> GetAllFp(string fpDictionary, Geometry.GetBoundryF f)
        {
            //  throw new Exception("");
            ////var fpConfig = Config.map.getFastonPositionConfigInfo(city, false);
            List<OssModel.FastonPosition> result = new List<OssModel.FastonPosition>();
            var index = 0;
            while (File.Exists($"{fpDictionary}fpindex\\fp_{index }.txt"))
            {
                var json = File.ReadAllText($"{fpDictionary}fpindex\\fp_{index }.txt");
                var item = Newtonsoft.Json.JsonConvert.DeserializeObject<OssModel.FastonPosition>(json);
                item.region = f.GetBoundry(item.Longitude, item.Latitde);
                // if(string.ins)
                Console.WriteLine($"{item.FastenPositionName}-{item.region}");
                result.Add(item);
                index++;
            }
            return result;
        }

        internal void GetData(out Dictionary<string, Dictionary<int, OssModel.SaveRoad.RoadInfo>> result)
        {
            result = this._road;
        }

        public int Get61Fp()
        {

            return this._allFp.Count;

        }




        public int getCarInOpposeDirection(string roadCode)
        {
            if (this._road.ContainsKey(roadCode)) if (this._road[roadCode].ContainsKey(0)) return this._road[roadCode][0].CarInOpposeDirection;
            return -1;
        }







        public static void SetRootPath()
        {
            //var rootPathDefault = "F:\\MyProject\\VRPWithZhangkun\\MainApp\\DBPublish\\";
            ////Consol.WriteLine("你好，请输入线路数据的路径，其默认值如下：");
            ////Consol.WriteLine("即bigData0.rqdt 与contentofdata 所在的目录");
            ////Consol.WriteLine(rootPathDefault);

            //var input = Console.ReadLine().Trim();
            //if (string.IsNullOrEmpty(input))
            //{
            //    Data.PathDataAll = rootPathDefault;
            //}
            //else
            //{
            //    Data.PathDataAll = input;
            //}
        }
        // static string PathDataAll = "";


        private static byte[] Decompress(byte[] data, int comPressLength)
        {
            try
            {
                MemoryStream ms = new MemoryStream(data);
                GZipStream zip = new GZipStream(ms, CompressionMode.Decompress, true);
                MemoryStream msreader = new MemoryStream();
                byte[] buffer = new byte[comPressLength * 50];
                while (true)
                {
                    int reader = zip.Read(buffer, 0, buffer.Length);
                    //zip.Read()
                    if (reader <= 0)
                    {
                        break;
                    }
                    msreader.Write(buffer, 0, reader);
                }
                zip.Close();
                ms.Close();
                msreader.Position = 0;
                buffer = msreader.ToArray();
                msreader.Close();
                return buffer;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        internal List<string> GetRoadNearby(double x, double z)
        {
            var lon = CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPositionLon(x);
            var lat = CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPositionLatWithAccuracy(-z, 0.0000001);

            List<string> roads = new List<string>();
            Dictionary<string, double> lengthOfRoad = new Dictionary<string, double>();
            foreach (var road in this._road)
            {
                double minLength = double.MaxValue;
                var roadsValue = road.Value;
                for (int i = 0; i < roadsValue.Count; i++)
                {
                    var lonMid = (roadsValue[i].startLongitude + roadsValue[i].endLongitude) / 2;
                    var latMid = (roadsValue[i].startLatitude + roadsValue[i].endLatitude) / 2;
                    var l = Math.Sqrt((lon - lonMid) * (lon - lonMid) + (lat - latMid) * (lat - latMid));
                    if (l < minLength)
                    {
                        minLength = l;
                    }
                }
                lengthOfRoad.Add(road.Key, minLength);
            }
            var closeRoads = lengthOfRoad.ToArray().OrderBy(item => item.Value).ToList();
            var length = Math.Min(15, closeRoads.Count);
            for (int i = 0; i < length; i++)
            {
                roads.Add(closeRoads[i].Key);
            }
            return roads;
        }

        //private static byte[] GetDataOfPath(int dataIndex, int startPositionInDB, int length)
        //{
        //    //using (var fileStream = new FileStream($"{PathDataAll}bigData{dataIndex}.rqdt", FileMode.OpenOrCreate))
        //    //{
        //    //    var data = new byte[length];
        //    //    fileStream.Seek(startPositionInDB, SeekOrigin.Begin);
        //    //    fileStream.Read(data, 0, length);
        //    //    return data;
        //    //}
        //}

        //private static bool Read(ref int startPosition, out byte[] data)
        //{
        //    //using (var fileStream = new FileStream($"{PathDataAll}contentofdata", FileMode.OpenOrCreate))
        //    //{
        //    //    data = new byte[8];
        //    //    if (startPosition < fileStream.Length)
        //    //    {
        //    //        fileStream.Seek(startPosition, SeekOrigin.Begin);
        //    //        fileStream.Read(data, 0, 8);
        //    //        startPosition += 8;
        //    //        return true;
        //    //    }
        //    //    else
        //    //    {
        //    //        return false;
        //    //    }
        //    //}
        //}
        public class PathStartPoint3
        {
            /// <summary>
            /// 要采用变换后的坐标 19级坐标×256
            /// </summary>
            public int x { get; set; }
            /// <summary>
            /// 要采用变换后的坐标 19级坐标×256
            /// </summary>
            public int y { get; set; }
            /// <summary>
            /// 要采用变换后的坐标 19级坐标×256
            /// </summary>
            public int z { get; set; }
        }
        public class PathResult4
        {
            /// <summary>
            /// 单位是10e-6°（经纬度）
            /// </summary>
            public int x { get; set; }//angle
            /// <summary>
            /// 单位是10e-6°（经纬度）
            /// </summary>
            public int y { get; set; }

            public int z { get; set; }
            public int t { get; set; }

            //public int a1 { get; set; }//angle
            //public int r1 { get; set; }
            //public int t1 { get; set; }
        }


        internal void GetSelection(List<OssModel.MapGo.nyrqPosition> goPath)
        {
            //  this._road[goPath[0].roadCode][goPath[0].roadOrder].
            //    throw new NotImplementedException();
        }

        //public class PathResult
        //{
        //    public double x0 { get; set; }
        //    public double y0 { get; set; }
        //    public int t0 { get; set; }

        //    public double x1 { get; set; }
        //    public double y1 { get; set; }
        //    public int t1 { get; set; }
        //}

        ///// <summary>
        ///// 获取路径
        ///// </summary>
        ///// <param name="dataResult">路径</param>
        ///// <param name="fpLast">起始点</param>
        ///// <param name="speed">速度</param>
        ///// <param name="result">ref path的结果。</param>
        ///// <param name="startT">ref 时间结果</param>
        //void GetAFromBPoint_(List<OssModel.MapGo.nyrqPosition> dataResult, OssModel.FastonPosition fpLast, int speed, ref List<int> result, ref int startT, bool speedImproved)
        //{
        //    for (var i = 0; i < dataResult.Count; i++)
        //    {
        //        if (i == 0)
        //        {
        //            //var startX = result.Last().x1;
        //            //var startY = result.Last().y1;
        //            double startX, startY;
        //            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(fpLast.positionLongitudeOnRoad, fpLast.positionLatitudeOnRoad, out startX, out startY);

        //            //var length=

        //            double endX, endY;
        //            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(dataResult[i].BDlongitude, dataResult[i].BDlatitude, out endX, out endY);
        //            //  var interview =
        //            var interview = Convert.ToInt32(CommonClass.Geography.getLengthOfTwoPoint.GetDistance(fpLast.positionLatitudeOnRoad, fpLast.positionLongitudeOnRoad, dataResult[i].BDlatitude, dataResult[i].BDlongitude) / dataResult[i].maxSpeed * 3.6 / 20 * 1000 * 50 / speed);

        //            interview = Program.rm.magicE.shotTime(interview, speedImproved);

        //            if (result.Count == 0)
        //            {
        //                var animate0 = new PathResult3()
        //                {
        //                    t = startT,
        //                    x = 0,
        //                    y = 0
        //                };
        //                // if (animate0.t != 0)
        //                {
        //                    result.Add(animate0.x);
        //                    result.Add(animate0.y);
        //                    result.Add(animate0.t);
        //                }
        //                // result.Add(animate0);
        //            }
        //            var animate1 = new PathResult3()
        //            {
        //                t = interview,
        //                x = Convert.ToInt32((endX - startX) * 256),
        //                y = Convert.ToInt32((endY - startY) * 256),
        //            };
        //            if (animate1.x != 0 || animate1.y != 0)  //  if (animate1.t != 0)
        //            {
        //                result.Add(animate1.x);
        //                result.Add(animate1.y);
        //                result.Add(animate1.t);
        //            }
        //            //result.Add(animate1);
        //            //var animate1 = new Data.PathResult()
        //            //{
        //            //    t0 = startT + 0,
        //            //    x0 = startX,
        //            //    y0 = startY,
        //            //    t1 = startT + interview,
        //            //    x1 = endX,
        //            //    y1 = endY
        //            //};
        //            startT += interview;
        //            // result.Add(animate1);
        //        }
        //        else if (dataResult[i].roadCode == dataResult[i - 1].roadCode)
        //        {

        //            double startX, startY;
        //            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(dataResult[i - 1].BDlongitude, dataResult[i - 1].BDlatitude, out startX, out startY);

        //            double endX, endY;
        //            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(dataResult[i].BDlongitude, dataResult[i].BDlatitude, out endX, out endY);

        //            // var length = CommonClass.Geography.getLengthOfTwoPoint.
        //            //  var interview =
        //            var interview = Convert.ToInt32(CommonClass.Geography.getLengthOfTwoPoint.GetDistance(dataResult[i - 1].BDlatitude, dataResult[i - 1].BDlongitude, dataResult[i].BDlatitude, dataResult[i].BDlongitude) / dataResult[i].maxSpeed * 3.6 / 20 * 1000 * 50 / speed);

        //            interview = Program.rm.magicE.shotTime(interview, speedImproved);

        //            var animate1 = new Data.PathResult3()
        //            {
        //                x = Convert.ToInt32((endX - startX) * 256),
        //                y = Convert.ToInt32((endY - startY) * 256),
        //                t = interview
        //            };
        //            //var animate1 = new Data.PathResult()
        //            //{
        //            //    t0 = startT + 0,
        //            //    x0 = startX,
        //            //    y0 = startY,
        //            //    t1 = startT + interview,
        //            //    x1 = endX,
        //            //    y1 = endY
        //            //};
        //            startT += interview;
        //            if (animate1.x != 0 || animate1.y != 0)//if (animate1.t != 0)
        //            {
        //                result.Add(animate1.x);
        //                result.Add(animate1.y);
        //                result.Add(animate1.t);
        //            }
        //            // result.Add(animate1);
        //        }
        //    }
        //}




        //internal void GetAFromBPoint(List<OssModel.MapGo.nyrqPosition> dataResult, OssModel.FastonPosition fpLast, int speed, ref List<int> result, ref int startT, bool speedImproved)
        //{
        //    for (var i = 0; i < dataResult.Count; i++)
        //    {
        //        if (i == 0)
        //        {
        //            //var startX = result.Last().x1;
        //            //var startY = result.Last().y1;
        //            double startX, startY;
        //            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(fpLast.positionLongitudeOnRoad, fpLast.positionLatitudeOnRoad, out startX, out startY);

        //            //var length=

        //            double endX, endY;
        //            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(dataResult[i].BDlongitude, dataResult[i].BDlatitude, out endX, out endY);
        //            //  var interview =
        //            var interview = Convert.ToInt32(CommonClass.Geography.getLengthOfTwoPoint.GetDistance(fpLast.positionLatitudeOnRoad, fpLast.positionLongitudeOnRoad, dataResult[i].BDlatitude, dataResult[i].BDlongitude) / dataResult[i].maxSpeed * 3.6 / 20 * 1000 * 50 / speed);

        //            interview = Program.rm.magicE.shotTime(interview, speedImproved);

        //            if (result.Count == 0)
        //            {
        //                var animate0 = new PathResult3()
        //                {
        //                    t = startT,
        //                    x = 0,
        //                    y = 0
        //                };
        //                if (animate0.t != 0)
        //                {
        //                    result.Add(animate0.x);
        //                    result.Add(animate0.y);
        //                    result.Add(animate0.t);
        //                }
        //                // result.Add(animate0);
        //            }
        //            var animate1 = new PathResult3()
        //            {
        //                t = interview,
        //                x = Convert.ToInt32((endX - startX) * 256),
        //                y = Convert.ToInt32((endY - startY) * 256),
        //            };
        //            if (animate1.t != 0)
        //            {
        //                result.Add(animate1.x);
        //                result.Add(animate1.y);
        //                result.Add(animate1.t);
        //            }
        //            //result.Add(animate1);
        //            //var animate1 = new Data.PathResult()
        //            //{
        //            //    t0 = startT + 0,
        //            //    x0 = startX,
        //            //    y0 = startY,
        //            //    t1 = startT + interview,
        //            //    x1 = endX,
        //            //    y1 = endY
        //            //};
        //            startT += interview;
        //            // result.Add(animate1);
        //        }
        //        else if (dataResult[i].roadCode == dataResult[i - 1].roadCode)
        //        {

        //            double startX, startY;
        //            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(dataResult[i - 1].BDlongitude, dataResult[i - 1].BDlatitude, out startX, out startY);

        //            double endX, endY;
        //            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(dataResult[i].BDlongitude, dataResult[i].BDlatitude, out endX, out endY);

        //            // var length = CommonClass.Geography.getLengthOfTwoPoint.
        //            //  var interview =
        //            var interview = Convert.ToInt32(CommonClass.Geography.getLengthOfTwoPoint.GetDistance(dataResult[i - 1].BDlatitude, dataResult[i - 1].BDlongitude, dataResult[i].BDlatitude, dataResult[i].BDlongitude) / dataResult[i].maxSpeed * 3.6 / 20 * 1000 * 50 / speed);

        //            interview = Program.rm.magicE.shotTime(interview, speedImproved);

        //            var animate1 = new Data.PathResult3()
        //            {
        //                x = Convert.ToInt32((endX - startX) * 256),
        //                y = Convert.ToInt32((endY - startY) * 256),
        //                t = interview
        //            };
        //            //var animate1 = new Data.PathResult()
        //            //{
        //            //    t0 = startT + 0,
        //            //    x0 = startX,
        //            //    y0 = startY,
        //            //    t1 = startT + interview,
        //            //    x1 = endX,
        //            //    y1 = endY
        //            //};
        //            startT += interview;
        //            if (animate1.t != 0)
        //            {
        //                result.Add(animate1.x);
        //                result.Add(animate1.y);
        //                result.Add(animate1.t);
        //            }
        //            // result.Add(animate1);
        //        }
        //    }
        //}

        //public void getAll(out List<double[]> meshPoints, out List<object> listOfCrosses)
        //{
        //    Dictionary<string, bool> Cs = new Dictionary<string, bool>();
        //    listOfCrosses = new List<object>();
        //    Dictionary<string, Dictionary<int, OssModel.SaveRoad.RoadInfo>> result;

        //    Program.dt.GetData(out result);
        //    meshPoints = new List<double[]>();
        //    //        //   List<int> colors = new List<int>();
        //    foreach (var item in result)
        //    {
        //        foreach (var itemj in item.Value)
        //        {
        //            var value = itemj.Value;
        //            var ps = getRoadRectangle(value, item.Value);
        //            meshPoints.Add(ps);


        //            for (var i = 0; i < value.Cross1.Length; i++)
        //            {
        //                var cross = value.Cross1[i];
        //                var key = cross.RoadCode1.CompareTo(cross.RoadCode2) > 0 ?
        //                    $"{cross.RoadCode1}_{cross.RoadOrder1}_{cross.RoadCode2}_{cross.RoadOrder2}" :
        //                    $"{cross.RoadCode2}_{cross.RoadOrder2}_{cross.RoadCode1}_{cross.RoadOrder1}";
        //                if (Cs.ContainsKey(key)) { }
        //                else
        //                {
        //                    Cs.Add(key, false);
        //                    listOfCrosses.Add(new { lon = cross.BDLongitude, lat = cross.BDLatitude, state = cross.CrossState });
        //                }
        //            }
        //        }
        //    }
        //}

        internal void getSingle(string roadCode, out List<int> meshPoints, out List<int> basePoint)
        {
            Dictionary<string, bool> Cs = new Dictionary<string, bool>();
            //listOfCrosses = new List<object>();
            Dictionary<string, Dictionary<int, OssModel.SaveRoad.RoadInfo>> result;

            Program.dt.GetData(out result);
            meshPoints = new List<int>();
            basePoint = new List<int>(2)
            {
                int.MaxValue,
                int.MaxValue,
                int.MaxValue
            };
            //        //   List<int> colors = new List<int>();
            //  foreach (var item in result)
            {
                Dictionary<int, OssModel.SaveRoad.rInfo> roads = new Dictionary<int, OssModel.SaveRoad.rInfo>();
                foreach (var itemj in result[roadCode])
                {
                    roads.Add(itemj.Key, itemj.Value);
                }

                foreach (var itemj in result[roadCode])
                {
#warning 这里优化时，要将getRoadRectangle计算移到前台！
                    var value = itemj.Value;
                    var ps = getRoadRectangle(value, roads);
                    for (var i = 0; i < ps.Length; i++)
                    {
                        var calValue = Convert.ToInt32(ps[i] * 1000000);
                        basePoint[i % 3] = Math.Min(calValue, basePoint[i % 3]);
                        meshPoints.Add(calValue);
                    }
                }
            }
            {
                for (var i = 0; i < meshPoints.Count; i++)
                {
                    meshPoints[i] = meshPoints[i] - basePoint[i % 3];
                }
            }
        }
        const double heightVitualToFact = 8.99266134E-6;
        private static double[] getRoadRectangle(OssModel.SaveRoad.rInfo info, Dictionary<int, OssModel.SaveRoad.rInfo> result)
        {
            double KofPointStretchFirstAndSecond = 1;
            double KofPointStretchThirdAndFourth = 1;
            if (info.CarInOpposeDirection == 0)
            {
                KofPointStretchFirstAndSecond = 1.5;
                KofPointStretchThirdAndFourth = 0.1;
            }

            double[] point1, point2, point3, point4;
            var vec = new double[] { info.endLongitude - info.startLongitude, info.endLatitude - info.startLatitude };
            vec = setToOne(vec);
            System.Numerics.Complex c = new System.Numerics.Complex(vec[0], vec[1]);
            if (!result.ContainsKey(info.RoadOrder - 1))
            {
                var c2 = new System.Numerics.Complex(0, 1);
                var c3 = c * c2;

                point1 = new double[] {
                    info.startLongitude + c3.Real * info.MaxSpeed * roadZoomValue*KofPointStretchFirstAndSecond,
                    info.startLatitude + c3.Imaginary * info.MaxSpeed * roadZoomValue*KofPointStretchFirstAndSecond,
                    info.startHeight*heightVitualToFact
                    };
                var c5 = c / c2;
                point2 = new double[] {
                    info.startLongitude + c5.Real * info.MaxSpeed * roadZoomValue*KofPointStretchFirstAndSecond,
                    info.startLatitude + c5.Imaginary * info.MaxSpeed * roadZoomValue*KofPointStretchFirstAndSecond ,
                    info.startHeight*heightVitualToFact
                };

            }
            else
            {
                var valueP = result[info.RoadOrder - 1];//previows
                var vecP = new double[] { valueP.endLongitude - valueP.startLongitude, valueP.endLatitude - valueP.startLatitude };
                vecP = setToOne(vecP);
                System.Numerics.Complex cP = new System.Numerics.Complex(-vecP[0], -vecP[1]);
                var vecDiv2 = c + cP;
                {
                    if (Math.Abs((vecDiv2 / c).Imaginary) < 1e-6)
                    {
                        var c2 = new System.Numerics.Complex(0, 1);
                        var c3 = c * c2;
                        point1 = new double[] {
                            info.startLongitude + c3.Real * info.MaxSpeed * roadZoomValue*KofPointStretchFirstAndSecond,
                            info.startLatitude + c3.Imaginary * info.MaxSpeed * roadZoomValue*KofPointStretchFirstAndSecond,
                            info.startHeight*heightVitualToFact
                        };
                        var c5 = c / c2;
                        point2 = new double[] {
                            info.startLongitude + c5.Real * info.MaxSpeed * roadZoomValue*KofPointStretchFirstAndSecond,
                            info.startLatitude + c5.Imaginary * info.MaxSpeed * roadZoomValue*KofPointStretchFirstAndSecond,
                            info.startHeight * heightVitualToFact
                        };
                    }
                    else
                    {
                        var k = 1 / (vecDiv2 / c).Imaginary;
                        var s = Math.Sqrt(k * k + 1 / k / k);

                        {
                            var vecDivOperate = s / k * setToOne(vecDiv2);
                            point1 = new double[] {
                                info.startLongitude + vecDivOperate.Real * info.MaxSpeed * roadZoomValue*KofPointStretchFirstAndSecond,
                                info.startLatitude + vecDivOperate.Imaginary * info.MaxSpeed * roadZoomValue  *KofPointStretchFirstAndSecond,
                                info.startHeight* heightVitualToFact
                            };
                        }
                        {
                            var vecDiv2_opp_diretion = -1 * (vecDiv2);
                            var vecDivOperate = s / k * setToOne(vecDiv2_opp_diretion);
                            point2 = new double[] {
                            info.startLongitude + vecDivOperate.Real * info.MaxSpeed * roadZoomValue*KofPointStretchFirstAndSecond,
                            info.startLatitude + vecDivOperate.Imaginary * info.MaxSpeed * roadZoomValue *KofPointStretchFirstAndSecond,
                            info.startHeight* heightVitualToFact
                            };

                        }
                    }
                }
            }
            c = new System.Numerics.Complex(-vec[0], -vec[1]);
            if (!result.ContainsKey(info.RoadOrder + 1))
            {
                var c2 = new System.Numerics.Complex(0, 1);
                var c3 = c * c2;
                point3 = new double[] {
                    info.endLongitude + c3.Real * info.MaxSpeed * roadZoomValue*KofPointStretchThirdAndFourth,
                    info.endLatitude + c3.Imaginary * info.MaxSpeed * roadZoomValue*KofPointStretchThirdAndFourth,
                    info.endHeight* heightVitualToFact
                };
                var c5 = c / c2;
                point4 = new double[] {
                    info.endLongitude + c5.Real * info.MaxSpeed * roadZoomValue*KofPointStretchThirdAndFourth,
                    info.endLatitude + c5.Imaginary * info.MaxSpeed * roadZoomValue*KofPointStretchThirdAndFourth,
                    info.endHeight* heightVitualToFact
                };
            }
            else
            {
                var valueN = result[info.RoadOrder + 1];//next
                var vecN = new double[] {
                    valueN.endLongitude - valueN.startLongitude,
                    valueN.endLatitude - valueN.startLatitude
                };
                vecN = setToOne(vecN);
                System.Numerics.Complex cP = new System.Numerics.Complex(vecN[0], vecN[1]);
                var vecDiv2 = c + cP;
                if (Math.Abs((vecDiv2 / c).Imaginary) < 1e-6)
                {
                    var c2 = new System.Numerics.Complex(0, 1);
                    var c3 = c * c2;
                    point3 = new double[] {
                    info.endLongitude + c3.Real * info.MaxSpeed * roadZoomValue*KofPointStretchThirdAndFourth,
                    info.endLatitude + c3.Imaginary * info.MaxSpeed * roadZoomValue  *KofPointStretchThirdAndFourth,
                    info.endHeight* heightVitualToFact
                    };
                    var c5 = c / c2;
                    point4 = new double[] {
                    info.endLongitude + c5.Real * info.MaxSpeed * roadZoomValue*KofPointStretchThirdAndFourth,
                    info.endLatitude + c5.Imaginary * info.MaxSpeed * roadZoomValue*KofPointStretchThirdAndFourth,
                    info.endHeight* heightVitualToFact
                    };
                }
                else
                {
                    var k = 1 / (vecDiv2 / c).Imaginary;
                    var s = Math.Sqrt(k * k + 1 / k / k);
                    {

                        var vecDivOperate = s / k * setToOne(vecDiv2);
                        point3 = new double[] {
                            info.endLongitude + vecDivOperate.Real * info.MaxSpeed * roadZoomValue*KofPointStretchThirdAndFourth,
                            info.endLatitude + vecDivOperate.Imaginary * info.MaxSpeed * roadZoomValue*KofPointStretchThirdAndFourth ,
                            info.endHeight* heightVitualToFact
                        };
                    }
                    {
                        var vecDiv2_opp_diretion = -1 * vecDiv2;
                        var vecDivOperate = s / k * setToOne(vecDiv2_opp_diretion);
                        point4 = new double[] {
                            info.endLongitude + vecDivOperate.Real * info.MaxSpeed * roadZoomValue*KofPointStretchThirdAndFourth,
                            info.endLatitude + vecDivOperate.Imaginary * info.MaxSpeed * roadZoomValue*KofPointStretchThirdAndFourth,
                            info.endHeight* heightVitualToFact
                        };
                    }
                }


            }
            return new double[]
            {
                Math.Round(point1[0],7),Math.Round(point1[1],7),Math.Round(point1[2],7),
                Math.Round(point2[0],7),Math.Round(point2[1],7),Math.Round(point2[2],7),
                Math.Round(point3[0],7),Math.Round(point3[1],7),Math.Round(point3[2],7),
                Math.Round(point4[0],7),Math.Round(point4[1],7),Math.Round(point4[2],7)
            };
        }

        const double roadZoomValue = 0.0000003;
        //private static double[] getRoadRectangle(OssModel.SaveRoad.RoadInfo value, Dictionary<int, OssModel.SaveRoad.RoadInfo> result)
        //{
        //    double KofPointStretchFirstAndSecond = 1;
        //    double KofPointStretchThirdAndFourth = 1;
        //    if (value.CarInOpposeDirection == 0)
        //    {
        //        KofPointStretchFirstAndSecond = 1.5;
        //        KofPointStretchThirdAndFourth = 0.1;
        //    }

        //    double[] point1, point2, point3, point4;
        //    var vec = new double[] { value.endLongitude - value.startLongitude, value.endLatitude - value.startLatitude };
        //    vec = setToOne(vec);
        //    System.Numerics.Complex c = new System.Numerics.Complex(vec[0], vec[1]);
        //    if (!result.ContainsKey(value.RoadOrder - 1))
        //    {
        //        var c2 = new System.Numerics.Complex(0, 1);
        //        var c3 = c * c2;

        //        point1 = new double[] {
        //            value.startLongitude + c3.Real * value.MaxSpeed * roadZoomValue*KofPointStretchFirstAndSecond,
        //            value.startLatitude + c3.Imaginary * value.MaxSpeed * roadZoomValue*KofPointStretchFirstAndSecond
        //            };
        //        var c5 = c / c2;
        //        point2 = new double[] {
        //            value.startLongitude + c5.Real * value.MaxSpeed * roadZoomValue*KofPointStretchFirstAndSecond,
        //            value.startLatitude + c5.Imaginary * value.MaxSpeed * roadZoomValue*KofPointStretchFirstAndSecond  };

        //    }
        //    else
        //    {
        //        var valueP = result[value.RoadOrder - 1];//previows
        //        var vecP = new double[] { valueP.endLongitude - valueP.startLongitude, valueP.endLatitude - valueP.startLatitude };
        //        vecP = setToOne(vecP);
        //        System.Numerics.Complex cP = new System.Numerics.Complex(-vecP[0], -vecP[1]);
        //        var vecDiv2 = c + cP;
        //        {
        //            if (Math.Abs((vecDiv2 / c).Imaginary) < 1e-6)
        //            {
        //                var c2 = new System.Numerics.Complex(0, 1);
        //                var c3 = c * c2;
        //                point1 = new double[] {
        //            value.startLongitude + c3.Real * value.MaxSpeed * roadZoomValue*KofPointStretchFirstAndSecond,
        //            value.startLatitude + c3.Imaginary * value.MaxSpeed * roadZoomValue*KofPointStretchFirstAndSecond  };
        //                var c5 = c / c2;
        //                point2 = new double[] {
        //            value.startLongitude + c5.Real * value.MaxSpeed * roadZoomValue*KofPointStretchFirstAndSecond,
        //            value.startLatitude + c5.Imaginary * value.MaxSpeed * roadZoomValue*KofPointStretchFirstAndSecond };
        //            }
        //            else
        //            {
        //                var k = 1 / (vecDiv2 / c).Imaginary;
        //                var s = Math.Sqrt(k * k + 1 / k / k);

        //                {
        //                    var vecDivOperate = s / k * setToOne(vecDiv2);
        //                    point1 = new double[] {
        //            value.startLongitude + vecDivOperate.Real * value.MaxSpeed * roadZoomValue*KofPointStretchFirstAndSecond,
        //            value.startLatitude + vecDivOperate.Imaginary * value.MaxSpeed * roadZoomValue  *KofPointStretchFirstAndSecond };
        //                }
        //                {
        //                    var vecDiv2_opp_diretion = -1 * (vecDiv2);
        //                    var vecDivOperate = s / k * setToOne(vecDiv2_opp_diretion);
        //                    point2 = new double[] {
        //                    value.startLongitude + vecDivOperate.Real * value.MaxSpeed * roadZoomValue*KofPointStretchFirstAndSecond,
        //                    value.startLatitude + vecDivOperate.Imaginary * value.MaxSpeed * roadZoomValue *KofPointStretchFirstAndSecond };
        //                }
        //            }
        //        }
        //    }
        //    c = new System.Numerics.Complex(-vec[0], -vec[1]);
        //    if (!result.ContainsKey(value.RoadOrder + 1))
        //    {
        //        var c2 = new System.Numerics.Complex(0, 1);
        //        var c3 = c * c2;
        //        point3 = new double[] {
        //            value.endLongitude + c3.Real * value.MaxSpeed * roadZoomValue*KofPointStretchThirdAndFourth,
        //            value.endLatitude + c3.Imaginary * value.MaxSpeed * roadZoomValue*KofPointStretchThirdAndFourth
        //        };
        //        var c5 = c / c2;
        //        point4 = new double[] {
        //            value.endLongitude + c5.Real * value.MaxSpeed * roadZoomValue*KofPointStretchThirdAndFourth,
        //            value.endLatitude + c5.Imaginary * value.MaxSpeed * roadZoomValue*KofPointStretchThirdAndFourth
        //        };
        //    }
        //    else
        //    {
        //        var valueN = result[value.RoadOrder + 1];//next
        //        var vecN = new double[] {
        //            valueN.endLongitude - valueN.startLongitude,
        //            valueN.endLatitude - valueN.startLatitude
        //        };
        //        vecN = setToOne(vecN);
        //        System.Numerics.Complex cP = new System.Numerics.Complex(vecN[0], vecN[1]);
        //        var vecDiv2 = c + cP;
        //        if (Math.Abs((vecDiv2 / c).Imaginary) < 1e-6)
        //        {
        //            var c2 = new System.Numerics.Complex(0, 1);
        //            var c3 = c * c2;
        //            point3 = new double[] {
        //            value.endLongitude + c3.Real * value.MaxSpeed * roadZoomValue*KofPointStretchThirdAndFourth,
        //            value.endLatitude + c3.Imaginary * value.MaxSpeed * roadZoomValue  *KofPointStretchThirdAndFourth};
        //            var c5 = c / c2;
        //            point4 = new double[] {
        //            value.endLongitude + c5.Real * value.MaxSpeed * roadZoomValue*KofPointStretchThirdAndFourth,
        //            value.endLatitude + c5.Imaginary * value.MaxSpeed * roadZoomValue*KofPointStretchThirdAndFourth };
        //        }
        //        else
        //        {
        //            var k = 1 / (vecDiv2 / c).Imaginary;
        //            var s = Math.Sqrt(k * k + 1 / k / k);
        //            {

        //                var vecDivOperate = s / k * setToOne(vecDiv2);
        //                point3 = new double[] {
        //                    value.endLongitude + vecDivOperate.Real * value.MaxSpeed * roadZoomValue*KofPointStretchThirdAndFourth,
        //                    value.endLatitude + vecDivOperate.Imaginary * value.MaxSpeed * roadZoomValue*KofPointStretchThirdAndFourth  };
        //            }
        //            {
        //                var vecDiv2_opp_diretion = -1 * vecDiv2;
        //                var vecDivOperate = s / k * setToOne(vecDiv2_opp_diretion);
        //                point4 = new double[] {
        //                    value.endLongitude + vecDivOperate.Real * value.MaxSpeed * roadZoomValue*KofPointStretchThirdAndFourth,
        //                    value.endLatitude + vecDivOperate.Imaginary * value.MaxSpeed * roadZoomValue*KofPointStretchThirdAndFourth  };
        //            }
        //        }


        //    }
        //    return new double[]
        //    {
        //      Math.Round(  point1[0],7),Math.Round(point1[1],7),
        //     Math.Round(   point2[0],7),Math.Round(point2[1],7),
        //     Math.Round(   point3[0],7),Math.Round(point3[1],7),
        //     Math.Round(   point4[0],7),Math.Round(point4[1],7)
        //    };
        //}

        private static System.Numerics.Complex setToOne(System.Numerics.Complex vecRes)
        {
            var data = setToOne(new double[] { vecRes.Real, vecRes.Imaginary });
            return new System.Numerics.Complex(data[0], data[1]);
        }

        private static double[] setToOne(double[] vec)
        {
            var l = Math.Sqrt(vec[0] * vec[0] + vec[1] * vec[1]);
            return new double[] { vec[0] / l, vec[1] / l };
        }
    }
    public partial class Data
    {


        public Dictionary<string, CommonClass.databaseModel.abtractmodelsPassData> material { get; set; }

        public class detailmodel : CommonClass.databaseModel.detailmodel
        {
            public double lon { get; set; }
            public double lat { get; set; }
        }

        internal Dictionary<string, long> GetDataOfOriginalStock(string bussinessAddr)
        {
            lock (modelStockLock)
            {
                if (modelsBussinessAddr.ContainsKey(bussinessAddr))
                {
                    return modelsStocks[modelsBussinessAddr[bussinessAddr]].stocksOriginal;
                }
                else
                {
                    return new Dictionary<string, long>();
                }
            }
            // throw new NotImplementedException();
        }
        Dictionary<string, string> modelsBussinessAddr = new Dictionary<string, string>();
        public Dictionary<string, CommonClass.ModelStock> modelsStocks = new Dictionary<string, CommonClass.ModelStock>();
        object modelStockLock = new object();
        internal void LoadStock(CommonClass.ModelStock sa)
        {
            lock (modelStockLock)
            {
                if (modelsStocks.ContainsKey(sa.modelID))
                {
                    modelsStocks[sa.modelID] = sa;
                }
                else
                {
                    modelsStocks.Add(sa.modelID, sa);
                    modelsBussinessAddr.Add(sa.bussinessAddress, sa.modelID);
                }
            }
            //Consol.WriteLine($"接受到stock信息");
        }
        public List<detailmodel> models { get; set; }
        // List<aModel> material { get; set; }
        internal void LoadModel()
        {
            this.material = new Dictionary<string, CommonClass.databaseModel.abtractmodelsPassData>();
            //throw new NotImplementedException();
            var list = DalOfAddress.AbtractModels.GetCategeAm1();
            for (int i = 0; i < list.Count; i += 2)
            {
                var amInfomationData = DalOfAddress.AbtractModels.GetAbtractModelItem(list[i].Trim());
                //  var amInfomation= DalOfAddress.AbtractModels.GetAbtractModelItem(amInfomationData);
                this.material.Add(list[i].Trim(), amInfomationData);
            }

            this.models = new List<detailmodel>();
            var modelList = DalOfAddress.detailmodel.GetList();
            for (int i = 0; i < modelList.Count; i++)
            {
                //   CommonClass.Geography.calculatBaideMercatorIndex
                models.Add(new detailmodel()
                {
                    amodel = modelList[i].amodel,
                    modelID = modelList[i].modelID,
                    rotatey = modelList[i].rotatey,
                    x = modelList[i].x,
                    y = modelList[i].y,
                    z = modelList[i].z,
                    lon = CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPositionLon(modelList[i].x),
                    lat = CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPositionLatWithAccuracy(0 - modelList[i].z, 1e-7),
                });
            }
        }
    }

    interface calRandom
    {

    }
}
