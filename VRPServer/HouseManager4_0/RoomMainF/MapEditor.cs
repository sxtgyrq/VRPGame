using CommonClass;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OssModel = Model;

namespace HouseManager4_0.RoomMainF
{
    public partial class RoomMain : interfaceOfHM.MapEditor
    {
        public string DelObjInfo(MapEditor.DelObjInfo doi)
        {
            DalOfAddress.detailmodel.Delete(doi.modelID);
            return "";
        }

        public string DrawRoad(MapEditor.DrawRoad dr)
        {
            List<int> meshPoints;
            List<int> basePoint;
            Program.dt.getSingle(dr.roadCode, out meshPoints, out basePoint);
            SingleRoadPathData srpd = new SingleRoadPathData()
            {
                c = "SingleRoadPathData",
                WebSocketID = 0,
                meshPoints = meshPoints,
                basePoint = basePoint
            };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(srpd);
            return json;
        }

        public string GetAbtractModels(MapEditor.GetAbtractModels gam)
        {
            var item = DalOfAddress.AbtractModels.GetAbtractModelItem(gam.modelName);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(item);
            return json;
        }

        public string GetCatege(CommonClass.MapEditor.GetCatege gc)
        {
            var list = DalOfAddress.AbtractModels.GetCatege();
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(list);
            return json;
        }

        public string GetFirstRoad()
        {
            string roadCode;
            var road = Program.dt.GetFirst(out roadCode);
            int indexValue = 0;

            do
            {
                var roadItem = road[indexValue];
                var a1 = (from item in roadItem.Cross1
                          where item.CrossState == 1
                          && item.RoadCode1 == roadCode
                          orderby item.RoadOrder1 + item.Percent1 ascending
                          select item).ToList();

                var a2 = (from item in roadItem.Cross2
                          where item.CrossState == 1
                          && item.RoadCode2 == roadCode
                          orderby item.RoadOrder2 + item.Percent2 ascending
                          select item).ToList();
                if (a1.Count > 0 && a2.Count > 0)
                {
                    if (a1.First().RoadOrder1 + a1.First().Percent1 < a2.First().RoadOrder2 + a2.First().Percent2)
                    {
                        var obj = new CommonClass.MapEditor.Position()
                        {
                            c = "Position",
                            anotherRoadCode = a1.First().RoadCode2,
                            anotherRoadOrder = a1.First().RoadOrder2,
                            roadCode = a1.First().RoadCode1,
                            roadOrder = a1.First().RoadOrder1,
                            longitude = a1.First().BDLongitude,
                            latitude = a1.First().BDLatitude
                        };
                        var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                        return sendMsg;
                    }
                    else
                    {
                        var obj = new CommonClass.MapEditor.Position()
                        {
                            c = "Position",
                            anotherRoadCode = a2.First().RoadCode1,
                            anotherRoadOrder = a2.First().RoadOrder1,
                            roadCode = a2.First().RoadCode2,
                            roadOrder = a2.First().RoadOrder2,
                            longitude = a2.First().BDLongitude,
                            latitude = a2.First().BDLatitude
                        };
                        var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                        return sendMsg;
                    }
                }
                else if (a1.Count > 0)
                {
                    var obj = new CommonClass.MapEditor.Position()
                    {
                        c = "Position",
                        anotherRoadCode = a1.First().RoadCode2,
                        anotherRoadOrder = a1.First().RoadOrder2,
                        roadCode = a1.First().RoadCode1,
                        roadOrder = a1.First().RoadOrder1,
                        longitude = a1.First().BDLongitude,
                        latitude = a1.First().BDLatitude
                    };
                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                    return sendMsg;
                }
                else if (a2.Count > 0)
                {
                    var obj = new CommonClass.MapEditor.Position()
                    {
                        c = "Position",
                        anotherRoadCode = a2.First().RoadCode1,
                        anotherRoadOrder = a2.First().RoadOrder1,
                        roadCode = a2.First().RoadCode2,
                        roadOrder = a2.First().RoadOrder2,
                        longitude = a2.First().BDLongitude,
                        latitude = a2.First().BDLatitude
                    };
                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                    return sendMsg;
                }

                indexValue++;
            }
            while (road.ContainsKey(indexValue));
            throw new Exception("");
        }

        public string NextCross(MapEditor.NextCross nc)
        {
            Dictionary<string, Dictionary<int, OssModel.SaveRoad.RoadInfo>> road;
            Program.dt.GetData(out road);


            double percent;
            double lon, lat;
            {
                var roadItem = road[nc.roadCode][nc.roadOrder];
                var a1 = (from item in roadItem.Cross1
                          where item.CrossState == 1
                          && item.RoadCode2 == nc.anotherRoadCode
                          && item.RoadOrder2 == nc.anotherRoadOrder
                          select item).ToList();
                var a2 = (from item in roadItem.Cross2
                          where item.CrossState == 1
                          && item.RoadCode1 == nc.anotherRoadCode
                          && item.RoadOrder1 == nc.anotherRoadOrder
                          select item).ToList();

                if (a1.Count == 1)
                {
                    percent = a1[0].Percent1;
                    lon = a1[0].BDLongitude;
                    lat = a1[0].BDLatitude;
                }
                else
                {
                    percent = a2[0].Percent2;
                    lon = a2[0].BDLongitude;
                    lat = a2[0].BDLatitude;
                }
            }
            double minValue = 1000000;
            int roadOrder = nc.roadOrder;
            CommonClass.MapEditor.Position positionResult = null;
            do
            {
                var roadItem = road[nc.roadCode][roadOrder];
                {
                    var a1 = (from item in roadItem.Cross1
                              where item.CrossState == 1
                              && (item.RoadCode2 != nc.anotherRoadCode || item.RoadOrder2 != nc.anotherRoadOrder)
                              && item.RoadOrder1 + item.Percent1 > percent + nc.roadOrder
                              select item).ToList();

                    for (var i = 0; i < a1.Count; i++)
                    {
                        if (a1[i].Percent1 + a1[i].RoadOrder1 < minValue)
                        {
                            minValue = a1[i].Percent1 + a1[i].RoadOrder1;
                            positionResult = new CommonClass.MapEditor.Position()
                            {
                                c = "Position",
                                anotherRoadCode = a1[i].RoadCode2,
                                anotherRoadOrder = a1[i].RoadOrder2,
                                roadCode = a1[i].RoadCode1,
                                roadOrder = a1[i].RoadOrder1,
                                longitude = a1[i].BDLongitude,
                                latitude = a1[i].BDLatitude
                            };
                        }
                    }
                }
                {
                    var a2 = (from item in roadItem.Cross2
                              where item.CrossState == 1
                              && (item.RoadCode1 != nc.anotherRoadCode || item.RoadOrder1 != nc.anotherRoadOrder)
                              && item.RoadOrder2 + item.Percent2 > percent + nc.roadOrder
                              select item).ToList();
                    for (var i = 0; i < a2.Count; i++)
                    {
                        if (a2[i].Percent2 + a2[i].RoadOrder2 < minValue)
                        {
                            minValue = a2[i].Percent2 + a2[i].RoadOrder2;
                            positionResult = new CommonClass.MapEditor.Position()
                            {
                                c = "Position",
                                anotherRoadCode = a2[i].RoadCode1,
                                anotherRoadOrder = a2[i].RoadOrder1,
                                roadCode = a2[i].RoadCode2,
                                roadOrder = a2[i].RoadOrder2,
                                longitude = a2[i].BDLongitude,
                                latitude = a2[i].BDLatitude
                            };
                        }
                    }
                }
                roadOrder++;
            }
            while (road[nc.roadCode].ContainsKey(roadOrder));

            if (positionResult == null)
            {
                positionResult = new CommonClass.MapEditor.Position()
                {
                    c = "Position",
                    anotherRoadCode = nc.anotherRoadCode,
                    anotherRoadOrder = nc.anotherRoadOrder,
                    roadCode = nc.roadCode,
                    roadOrder = nc.roadOrder,
                    longitude = lon,
                    latitude = lat
                };
            }
            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(positionResult);
            return sendMsg;
        }

        public string PreviousCross(MapEditor.PreviousCross pc)
        {
            Dictionary<string, Dictionary<int, OssModel.SaveRoad.RoadInfo>> road;
            Program.dt.GetData(out road);
            double percent;
            double lon, lat;
            {
                var roadItem = road[pc.roadCode][pc.roadOrder];
                var a1 = (from item in roadItem.Cross1
                          where item.CrossState == 1
                          && item.RoadCode2 == pc.anotherRoadCode
                          && item.RoadOrder2 == pc.anotherRoadOrder
                          select item).ToList();
                var a2 = (from item in roadItem.Cross2
                          where item.CrossState == 1
                          && item.RoadCode1 == pc.anotherRoadCode
                          && item.RoadOrder1 == pc.anotherRoadOrder
                          select item).ToList();

                if (a1.Count == 1)
                {
                    percent = a1[0].Percent1;
                    lon = a1[0].BDLongitude;
                    lat = a1[0].BDLatitude;
                }
                else
                {
                    percent = a2[0].Percent2;
                    lon = a2[0].BDLongitude;
                    lat = a2[0].BDLatitude;
                }
            }
            double maxValue = -1;
            int roadOrder = pc.roadOrder;
            CommonClass.MapEditor.Position positionResult = null;
            do
            {
                var roadItem = road[pc.roadCode][roadOrder];
                {
                    var a1 = (from item in roadItem.Cross1
                              where item.CrossState == 1
                              && (item.RoadCode2 != pc.anotherRoadCode || item.RoadOrder2 != pc.anotherRoadOrder)
                              && item.RoadOrder1 + item.Percent1 < percent + pc.roadOrder
                              select item).ToList();

                    for (var i = 0; i < a1.Count; i++)
                    {
                        if (a1[i].Percent1 + a1[i].RoadOrder1 > maxValue)
                        {
                            maxValue = a1[i].Percent1 + a1[i].RoadOrder1;
                            positionResult = new CommonClass.MapEditor.Position()
                            {
                                c = "Position",
                                anotherRoadCode = a1[i].RoadCode2,
                                anotherRoadOrder = a1[i].RoadOrder2,
                                roadCode = a1[i].RoadCode1,
                                roadOrder = a1[i].RoadOrder1,
                                longitude = a1[i].BDLongitude,
                                latitude = a1[i].BDLatitude
                            };
                        }
                    }
                }
                {
                    var a2 = (from item in roadItem.Cross2
                              where item.CrossState == 1
                              && (item.RoadCode1 != pc.anotherRoadCode || item.RoadOrder1 != pc.anotherRoadOrder)
                              && item.RoadOrder2 + item.Percent2 < percent + pc.roadOrder
                              select item).ToList();
                    for (var i = 0; i < a2.Count; i++)
                    {
                        if (a2[i].Percent2 + a2[i].RoadOrder2 > maxValue)
                        {
                            maxValue = a2[i].Percent2 + a2[i].RoadOrder2;
                            positionResult = new CommonClass.MapEditor.Position()
                            {
                                c = "Position",
                                anotherRoadCode = a2[i].RoadCode1,
                                anotherRoadOrder = a2[i].RoadOrder1,
                                roadCode = a2[i].RoadCode2,
                                roadOrder = a2[i].RoadOrder2,
                                longitude = a2[i].BDLongitude,
                                latitude = a2[i].BDLatitude
                            };
                        }
                    }
                }
                roadOrder--;
            }
            while (road[pc.roadCode].ContainsKey(roadOrder));

            if (positionResult == null)
            {
                positionResult = new CommonClass.MapEditor.Position()
                {
                    c = "Position",
                    anotherRoadCode = pc.anotherRoadCode,
                    anotherRoadOrder = pc.anotherRoadOrder,
                    roadCode = pc.roadCode,
                    roadOrder = pc.roadOrder,
                    longitude = lon,
                    latitude = lat
                };
            }
            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(positionResult);
            return sendMsg;
        }

        public string SaveObjInfo(MapEditor.SaveObjInfo soi)
        {
            CommonClass.databaseModel.detailmodel dm = new CommonClass.databaseModel.detailmodel()
            {
                amodel = soi.amodel,
                modelID = soi.modelID,
                rotatey = Convert.ToSingle(soi.rotatey),
                x = Convert.ToSingle(soi.x),
                y = Convert.ToSingle(soi.y),
                z = Convert.ToSingle(soi.z),
            };
            DalOfAddress.detailmodel.Add(dm);
            return "";
        }

        public string ShowOBJFile(MapEditor.ShowOBJFile sof)
        {
            var list = DalOfAddress.detailmodel.GetList(sof.x, sof.z);
            var result = new CommonClass.MapEditor.ObjResult()
            {
                c = "ObjResult",
                detail = list
            };
            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            return sendMsg;
            //throw new NotImplementedException();
        }

        public string UpdateObjInfo(MapEditor.UpdateObjInfo uoi)
        {
            CommonClass.databaseModel.detailmodel dm = new CommonClass.databaseModel.detailmodel()
            {
                modelID = uoi.modelID,
                rotatey = Convert.ToSingle(uoi.rotatey),
                x = Convert.ToSingle(uoi.x),
                y = Convert.ToSingle(uoi.y),
                z = Convert.ToSingle(uoi.z),
            };
            DalOfAddress.detailmodel.Update(dm);
            return "";
        }
    }
}
