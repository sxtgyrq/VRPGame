using CommonClass;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using OssModel = Model;

namespace HouseManager4_0.RoomMainF
{
    public partial class RoomMain : interfaceOfHM.MapEditor
    {
        public string CalMD5(string _str)
        {
            //字符串转成字节数组
            byte[] bytes = System.Text.Encoding.Default.GetBytes(_str);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] jmbytes = md5.ComputeHash(bytes);

            string str = BitConverter.ToString(jmbytes);
            return str.Replace("-", "");
        }
        public string CreateNew(MapEditor.CreateNew cn)
        {
            string amID = CalMD5(cn.imageBase64 + cn.objText + cn.mtlText);
            string modelType = cn.modelType;
            string imageBase64 = cn.imageBase64;
            string objText = cn.objText;
            string mtlText = cn.mtlText;
            string animation = cn.animation;
            string author = cn.author;
            int amState = cn.amState;
            string modelName = cn.modelName;

            CommonClass.databaseModel.detailmodel dm = new CommonClass.databaseModel.detailmodel()
            {
                amodel = amID,
                modelID = cn.modelID,
                rotatey = Convert.ToSingle(cn.rotatey),
                x = Convert.ToSingle(cn.x),
                y = Convert.ToSingle(cn.y),
                z = Convert.ToSingle(cn.z),
                dmState = 0,

            };

            DalOfAddress.AbtractModels.Insert(amID, modelType, imageBase64, objText, mtlText, animation, author, amState, modelName, dm);
            return "";
            // throw new NotImplementedException();
        }

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
            var item = DalOfAddress.AbtractModels.GetAbtractModelItem(gam.amID);
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

        public string GetModelDetail(MapEditor.GetModelDetail cn)
        {
            var obj = DalOfAddress.detailmodel.GetItem(cn.modelID);
            if (obj == null)
            {
                return "";
            }
            else
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
        }

        public string GetModelType(MapEditor.GetCatege gc)
        {
            var list = DalOfAddress.AbtractModels.GetModelType();
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(list);
            return json;
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
                amodel = soi.amID,
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

        public string UseModelObj(MapEditor.UseModelObj cn)
        {
            DalOfAddress.detailmodel.UpdateUsed(cn);
            return "";
            // throw new NotImplementedException();
        }
        public string LockModelObj(MapEditor.UseModelObj cn)
        {
            DalOfAddress.detailmodel.UpdateLocked(cn);
            return "";
        }

        public string ClearModelObj()
        {
            DalOfAddress.AbtractModels.ClearModelObj();
            return "";
        }

        public string GetUnLockedModel(MapEditor.GetUnLockedModel gulm)
        {
            bool previous;
            if (gulm.direction == "up")
            {
                previous = true;
            }
            else
            {
                previous = false;
            }
            bool HasValue;
            var positon = DalOfAddress.detailmodel.GetPositionOfUnlockedObj(ref gulm.startIndex, previous, out HasValue);
            if (HasValue)
            {
                MapEditor.GetUnLockedModelResult r = new MapEditor.GetUnLockedModelResult()
                {
                    x = positon[0],
                    y = positon[1],
                    z = positon[2],
                    hasValue = HasValue,
                    newStartIndex = gulm.startIndex,
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(r);
            }
            else
            {
                MapEditor.GetUnLockedModelResult r = new MapEditor.GetUnLockedModelResult()
                {
                    x = positon[0],
                    y = positon[1],
                    z = positon[2],
                    hasValue = HasValue,
                    newStartIndex = gulm.startIndex,
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(r);
            }
        }

        public string SetBackgroundSceneF(MapEditor.SetBackgroundScene_BLL sbs)
        {
            Dictionary<string, Dictionary<int, OssModel.SaveRoad.RoadInfo>> road;
            Program.dt.GetData(out road);

            var road1 = road[sbs.firstRoadcode][sbs.firstRoadorder];
            var road2 = road[sbs.secondRoadcode][sbs.secondRoadorder];

            double x, y;
            var r = get_line_intersection(
                road1.startLongitude, road1.startLatitude,
                road1.endLongitude, road1.endLatitude,
                road2.startLongitude, road2.startLatitude,
                road2.endLongitude, road2.endLatitude, out x, out y);
            if (r)
            {
                string crossID = $"{x.ToString("F5")},{y.ToString("F5")}";
                MapEditor.SetBackgroundScene_DAL sbsd = new MapEditor.SetBackgroundScene_DAL()
                {
                    author = sbs.author,
                    crossID = crossID,
                    c = sbs.c,
                    nx = sbs.nx,
                    ny = sbs.ny,
                    nz = sbs.nz,
                    px = sbs.px,
                    py = sbs.py,
                    pz = sbs.pz,
                };
                DalOfAddress.backgroundjpg.Insert(sbsd);
            }

            return "";
            //
        }
        /// <summary>
        /// 线段1(p0，p1)与线段2(p2,p3)求交点
        /// </summary>
        /// <param name="p0_x">线段1</param>
        /// <param name="p0_y">线段1</param>
        /// <param name="p1_x">线段1</param>
        /// <param name="p1_y">线段1</param>
        /// <param name="p2_x">线段2</param>
        /// <param name="p2_y">线段2</param>
        /// <param name="p3_x">线段2</param>
        /// <param name="p3_y">线段2</param>
        /// <param name="x">结果</param>
        /// <param name="y">结果</param>
        /// <returns></returns>
        bool get_line_intersection(double p0_x, double p0_y, double p1_x, double p1_y, double p2_x, double p2_y, double p3_x, double p3_y, out double x, out double y)
        {
            double s1_x, s1_y, s2_x, s2_y;
            s1_x = p1_x - p0_x;
            s1_y = p1_y - p0_y;
            s2_x = p3_x - p2_x;
            s2_y = p3_y - p2_y;

            double s, t;
            s = (-s1_y * (p0_x - p2_x) + s1_x * (p0_y - p2_y)) / (-s2_x * s1_y + s1_x * s2_y);
            t = (s2_x * (p0_y - p2_y) - s2_y * (p0_x - p2_x)) / (-s2_x * s1_y + s1_x * s2_y);

            if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
            {
                x = p0_x + (t * s1_x);
                y = p0_y + (t * s1_y);
                return true;
            }
            else
            {
                x = p0_x + (t * s1_x);
                y = p0_y + (t * s1_y);
                return false;
            }

        }

        public string GetBackgroundSceneF(MapEditor.GetBackgroundScene gbs)
        {
            Dictionary<string, Dictionary<int, OssModel.SaveRoad.RoadInfo>> road;
            Program.dt.GetData(out road);

            var road1 = road[gbs.firstRoadcode][gbs.firstRoadorder];
            var road2 = road[gbs.secondRoadcode][gbs.secondRoadorder];

            double x, y;
            var r1 = get_line_intersection(
                road1.startLongitude, road1.startLatitude,
                road1.endLongitude, road1.endLatitude,
                road2.startLongitude, road2.startLatitude,
                road2.endLongitude, road2.endLatitude, out x, out y);
            if (r1)
            {
                string crossID = $"{x.ToString("F5")},{y.ToString("F5")}";
                MapEditor.GetBackgroundScene.Result r = DalOfAddress.backgroundjpg.Get(crossID);
                return Newtonsoft.Json.JsonConvert.SerializeObject(r);
            }
            else
            {
                return "";
            }
        }

        public string UseBackgroundSceneF(MapEditor.UseBackgroundScene sbs)
        {
            Dictionary<string, Dictionary<int, OssModel.SaveRoad.RoadInfo>> road;
            Program.dt.GetData(out road);

            var road1 = road[sbs.firstRoadcode][sbs.firstRoadorder];
            var road2 = road[sbs.secondRoadcode][sbs.secondRoadorder];

            double x, y;
            var r1 = get_line_intersection(
                road1.startLongitude, road1.startLatitude,
                road1.endLongitude, road1.endLatitude,
                road2.startLongitude, road2.startLatitude,
                road2.endLongitude, road2.endLatitude, out x, out y);
            if (r1)
            {
                string crossID = $"{x.ToString("F5")},{y.ToString("F5")}";
                DalOfAddress.backgroundjpg.SetUse(sbs, crossID);
            }
            else
            {
                return "";
            }


            return "";
        }

        public string GetBG(SetCrossBG ss)
        { 
            if (Program.dt.AllCrossesBGData_.ContainsKey(ss.Md5Key))
            {
                var r = DalOfAddress.backgroundjpg.GetItemDetail(Program.dt.AllCrossesBGData_[ss.Md5Key]);
                 return Newtonsoft.Json.JsonConvert.SerializeObject(r);
            }
            else
            {
                Console.WriteLine($"没有结果");
                return "";
            } 
        }
    }

    public partial class RoomMain : interfaceOfHM.ModelTranstractionI
    {
        public string GetAllModelPosition()
        {
            var result = DalOfAddress.detailmodel.GetAll();
            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }

        //public string GetFirstModelAddr(ModelTranstraction.GetFirstModelAddr gfm)
        //{

        //}

        public string GetModelByID(ModelTranstraction.GetModelByID gmbid)
        {
            if (CommonClass.Format.IsModelID(gmbid.modelID))
            {
                var result = DalOfAddress.detailmodel.GetByID(gmbid.modelID);
                if (result == null)
                {
                    return "";
                }
                else
                {
                    return Newtonsoft.Json.JsonConvert.SerializeObject(result);
                }
            }
            else
            {
                //Consol.WriteLine($"{gmbid.modelID}不符合规则");
                return "";
            }
        }

        public string GetRewardFromBuildingF(GetRewardFromBuildingM m)
        {
            if (CommonClass.Format.IsModelID(m.selectObjName))
            {
                return this.modelM.GetRewardFromBuildingF(m);
            }
            else
            {
                return "";
            }
            // throw new NotImplementedException();
        }

        public string GetRoadNearby(ModelTranstraction.GetRoadNearby grn)
        {

            HouseManager4_0.OperateObj_Model op = new OperateObj_Model(this);
            op.GetRoadNearby(grn);
            return "";
        }

        public string GetTransctionFromChainF(ModelTranstraction.GetTransctionFromChain gtfc)
        {
            if (this.Market.mile_Price == null)
                return Newtonsoft.Json.JsonConvert.SerializeObject(new Dictionary<string, long>());
            else
                return Market.Send(gtfc);
            //    throw new NotImplementedException();
        }

        public string GetTransctionModelDetail(ModelTranstraction.GetTransctionModelDetail gtmd)
        {
            var r = DalOfAddress.TradeRecord.GetAll(gtmd.bussinessAddr);
            return Newtonsoft.Json.JsonConvert.SerializeObject(r);
        }

        public string TradeCoinF(ModelTranstraction.TradeCoin tc)
        {
            DalOfAddress.TradeRecord.Add(tc.tradeIndex, tc.addrFrom, tc.addrBussiness, tc.sign, tc.msg, tc.passCoin);
            return "";
        }

        public string TradeIndex(ModelTranstraction.TradeIndex tc)
        {
            var Index = DalOfAddress.TradeRecord.GetCount(tc.addrBussiness, tc.addrFrom);
            return Index.ToString();
        }

        public void UpdateModelStock(ModelStock sa)
        {
            Program.dt.LoadStock(sa);
            // throw new NotImplementedException();
        }
    }
}
