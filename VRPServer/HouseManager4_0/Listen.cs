using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HouseManager4_0
{
    public class Listen
    {
        internal static void IpAndPort(string hostIP, int tcpPort)
        {
            var dealWith = new TcpFunction.WithResponse.DealWith(DealWith);
            TcpFunction.WithResponse.ListenIpAndPort(hostIP, tcpPort, dealWith);
        }
        private static async Task<string> DealWith(string notifyJson)
        {
            try
            {
                CommonClass.Command c = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.Command>(notifyJson);
                return DealWithInterfaceAndObj(Program.rm, c, notifyJson);
            }
            catch
            {
                //Consol.WriteLine($"notify receive:{notifyJson}");
                File.AppendAllText("log/d.txt", $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}-{notifyJson}{Environment.NewLine}");
                return "haveNothingToReturn";
            }
        }

        static string DealWithInterfaceAndObj(interfaceOfHM.ListenInterface objI, CommonClass.Command c, string notifyJson)
        {
            string outPut = "haveNothingToReturn";
            {
                // CommonClass.Command c = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.Command>(notifyJson);
                Console.WriteLine(c.c);
                switch (c.c)
                {
                    case "PlayerAdd_V2":
                        {
                            CommonClass.PlayerAdd_V2 addItem = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.PlayerAdd_V2>(notifyJson);
                            var result = objI.AddPlayer(addItem);
                            outPut = result;
                        }; break;
                    case "GetPosition":
                        {
                            CommonClass.GetPosition getPosition = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.GetPosition>(notifyJson);
                            //string fromUrl; 
                            var GPResult = objI.GetPosition(getPosition);
                            if (GPResult.Success)
                            {
                                CommonClass.GetPositionNotify_v2 notify = new CommonClass.GetPositionNotify_v2()
                                {
                                    c = "GetPositionNotify_v2",
                                    fp = GPResult.Fp,
                                    WebSocketID = GPResult.WebSocketID,
                                    key = getPosition.Key,
                                    PlayerName = GPResult.PlayerName,
                                    positionInStation = GPResult.positionInStation,
                                    fPIndex = GPResult.fPIndex
                                };

                                Startup.sendMsg(GPResult.FromUrl, Newtonsoft.Json.JsonConvert.SerializeObject(notify));
                                var notifyMsgs = GPResult.NotifyMsgs;
                                for (var i = 0; i < notifyMsgs.Count; i += 2)
                                {
                                    Startup.sendMsg(notifyMsgs[i], notifyMsgs[i + 1]);
                                }
                            }
                            outPut = "ok";
                        }; break;
                    case "SetPromote":
                        {
                            CommonClass.SetPromote sp = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.SetPromote>(notifyJson);
                            var result = objI.updatePromote(sp);
                            outPut = "ok";
                            //await context.Response.WriteAsync("ok");
                        }; break;
                    case "SetAbility":
                        {
                            CommonClass.SetAbility sa = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.SetAbility>(notifyJson);
                            objI.SetAbility(sa);
                            outPut = "ok";
                        }; break;
                    case "PlayerCheck":
                        {
                            CommonClass.PlayerCheck checkItem = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.PlayerCheck>(notifyJson);
                            var result = objI.UpdatePlayer(checkItem);
                            outPut = result;
                        }; break;
                    case "SetCollect":
                        {
                            CommonClass.SetCollect sc = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.SetCollect>(notifyJson);
                            var result = objI.updateCollect(sc);
                            outPut = "ok";
                            //await context.Response.WriteAsync("ok");
                        }; break;
                    case "SetAttack":
                        {
                            CommonClass.SetAttack sa = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.SetAttack>(notifyJson);
                            var result = objI.updateAttack(sa);
                            outPut = "ok";
                            //await context.Response.WriteAsync("ok");
                        }; break;
                    //case "SetBust":
                    //    {
                    //        CommonClass.SetBust sa = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.SetBust>(notifyJson);
                    //        var result = objI.updateBust(sa);
                    //        outPut = "ok";
                    //        //await context.Response.WriteAsync("ok");
                    //    }; break;
                    case "GetFrequency":
                        {
                            outPut = Program.rm.GetFrequency().ToString();
                        }; break;
                    case "OrderToReturn":
                        {
                            CommonClass.OrderToReturn otr = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.OrderToReturn>(notifyJson);
                            objI.OrderToReturn(otr);
                            outPut = "ok";
                        }; break;
                    case "SaveMoney":
                        {
                            CommonClass.SaveMoney saveMoney = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.SaveMoney>(notifyJson);
                            objI.SaveMoney(saveMoney);
                            outPut = "ok";
                        }; break;
                    case "MarketPrice":
                        {
                            CommonClass.MarketPrice sa = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MarketPrice>(notifyJson);

                            objI.MarketUpdate(sa);
                        }; break;
                    case "ModelStock":
                        {
                            CommonClass.ModelStock sa = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelStock>(notifyJson);
                            objI.UpdateModelStock(sa);
                        }; break;
                    case "SystemBradcast":
                        {
                            CommonClass.SystemBradcast sb = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.SystemBradcast>(notifyJson);
                            objI.SystemBradcast(sb);
                        }; break;
                    case "SetBuyDiamond":
                        {
                            CommonClass.SetBuyDiamond bd = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.SetBuyDiamond>(notifyJson);

                            objI.Buy(bd);
                        }; break;
                    case "SetSellDiamond":
                        {
                            CommonClass.SetSellDiamond ss = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.SetSellDiamond>(notifyJson);

                            objI.Sell(ss);
                        }; break;
                    case "OrderToSubsidize":
                        {
                            CommonClass.OrderToSubsidize ots = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.OrderToSubsidize>(notifyJson);
                            objI.OrderToSubsidize(ots);
                            outPut = "ok";
                        }; break;
                    case "OrderToUpdateLevel":
                        {
                            //CommonClass.OrderToUpdateLevel oul = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.OrderToUpdateLevel>(notifyJson);
                            //objI.OrderToUpdateLevel(oul);
                            outPut = "ok";
                        }; break;
                    case "DialogMsg":
                        {
                            CommonClass.DialogMsg dm = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.DialogMsg>(notifyJson);
                            objI.SendMsg(dm);
                            outPut = "ok";
                        }; break;
                    case "SetSelectDriver":
                        {
                            CommonClass.SetSelectDriver dm = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.SetSelectDriver>(notifyJson);
                            objI.SelectDriver(dm);
                            outPut = "ok";
                        }; break;
                    case "MagicSkill":
                        {
                            CommonClass.MagicSkill ms = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MagicSkill>(notifyJson);
                            var result = objI.updateMagic(ms);
                            outPut = "ok";
                        }; break;
                    case "View":
                        {
                            CommonClass.View v = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.View>(notifyJson);
                            var result = objI.updateView(v);
                            outPut = "ok";
                        }; break;
                    case "GetFirstRoad":
                        {
                            //CommonClass.View v = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.View>(notifyJson);
                            var result = objI.GetFirstRoad();
                            outPut = result;
                        }; break;
                    case "DrawRoad":
                        {
                            CommonClass.MapEditor.DrawRoad v = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.DrawRoad>(notifyJson);
                            outPut = objI.DrawRoad(v);
                        }; break;
                    case "NextCross":
                        {
                            CommonClass.MapEditor.NextCross v = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.NextCross>(notifyJson);
                            outPut = objI.NextCross(v);
                        }; break;
                    case "PreviousCross":
                        {
                            CommonClass.MapEditor.PreviousCross v = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.PreviousCross>(notifyJson);
                            outPut = objI.PreviousCross(v);
                        }; break;
                    case "GetCatege":
                        {
                            CommonClass.MapEditor.GetCatege gc = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.GetCatege>(notifyJson);
                            outPut = objI.GetCatege(gc);
                        }; break;
                    case "GetModelType":
                        {
                            CommonClass.MapEditor.GetCatege gc = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.GetCatege>(notifyJson);
                            outPut = objI.GetModelType(gc);
                        }; break;
                    case "GetAbtractModels":
                        {
                            CommonClass.MapEditor.GetAbtractModels gam = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.GetAbtractModels>(notifyJson);
                            outPut = objI.GetAbtractModels(gam);
                        }; break;
                    case "SaveObjInfo":
                        {
                            CommonClass.MapEditor.SaveObjInfo soi = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.SaveObjInfo>(notifyJson);
                            outPut = objI.SaveObjInfo(soi);
                        }; break;
                    case "ShowOBJFile":
                        {
                            CommonClass.MapEditor.ShowOBJFile sof = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.ShowOBJFile>(notifyJson);
                            outPut = objI.ShowOBJFile(sof);
                        }; break;
                    case "UpdateObjInfo":
                        {
                            CommonClass.MapEditor.UpdateObjInfo uoi = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.UpdateObjInfo>(notifyJson);
                            outPut = objI.UpdateObjInfo(uoi);
                        }; break;
                    case "DelObjInfo":
                        {
                            CommonClass.MapEditor.DelObjInfo doi = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.DelObjInfo>(notifyJson);
                            outPut = objI.DelObjInfo(doi);
                        }; break;
                    case "CreateNew":
                        {
                            CommonClass.MapEditor.CreateNew cn = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.CreateNew>(notifyJson);
                            outPut = objI.CreateNew(cn);
                        }; break;
                    case "GetModelDetail":
                        {
                            CommonClass.MapEditor.GetModelDetail cn = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.GetModelDetail>(notifyJson);
                            outPut = objI.GetModelDetail(cn);
                        }; break;
                    case "UseModelObj":
                        {
                            CommonClass.MapEditor.UseModelObj cn = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.UseModelObj>(notifyJson);
                            outPut = objI.UseModelObj(cn);
                        }; break;
                    case "LockModelObj":
                        {
                            CommonClass.MapEditor.UseModelObj cn = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.UseModelObj>(notifyJson);
                            outPut = objI.LockModelObj(cn);
                        }; break;
                    case "ClearModelObj":
                        {
                            outPut = objI.ClearModelObj();
                        }; break;
                    case "GetUnLockedModel":
                        {
                            //GetUnLockedModel
                            CommonClass.MapEditor.GetUnLockedModel gulm = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.GetUnLockedModel>(notifyJson);
                            outPut = objI.GetUnLockedModel(gulm);
                        }; break;
                    case "GetModelByID":
                        {
                            CommonClass.ModelTranstraction.GetModelByID gmbid = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.GetModelByID>(notifyJson);
                            outPut = objI.GetModelByID(gmbid);
                        }; break;
                    case "GetAllModelPosition":
                        {
                            CommonClass.ModelTranstraction.GetAllModelPosition gfm = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.GetAllModelPosition>(notifyJson);
                            outPut = objI.GetAllModelPosition();
                        }; break;
                    case "GetTransctionModelDetail":
                        {
                            CommonClass.ModelTranstraction.GetTransctionModelDetail gtmd = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.GetTransctionModelDetail>(notifyJson);
                            outPut = objI.GetTransctionModelDetail(gtmd);
                        }; break;
                    case "GetTransctionFromChain":
                        {
                            CommonClass.ModelTranstraction.GetTransctionFromChain gtfc = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.GetTransctionFromChain>(notifyJson);
                            outPut = objI.GetTransctionFromChainF(gtfc);
                        }; break;
                    case "GetRoadNearby":
                        {
                            CommonClass.ModelTranstraction.GetRoadNearby grn = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.GetRoadNearby>(notifyJson);
                            outPut = objI.GetRoadNearby(grn);
                        }; break;
                    case "TradeCoin":
                        {
                            CommonClass.ModelTranstraction.TradeCoin tc = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.TradeCoin>(notifyJson);
                            outPut = objI.TradeCoinF(tc);
                        }; break;
                    case "TradeIndex":
                        {
                            CommonClass.ModelTranstraction.TradeIndex tc = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.TradeIndex>(notifyJson);
                            outPut = objI.TradeIndex(tc);
                        }; break;
                    case "SetBackgroundScene":
                        {
                            //SetBackgroundScene
                            CommonClass.MapEditor.SetBackgroundScene_BLL sbs = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.SetBackgroundScene_BLL>(notifyJson);
                            outPut = objI.SetBackgroundSceneF(sbs);
                        }; break;
                    case "GetBackgroundScene":
                        {
                            CommonClass.MapEditor.GetBackgroundScene gbs = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.GetBackgroundScene>(notifyJson);
                            outPut = objI.GetBackgroundSceneF(gbs);
                        }; break;
                    case "UseBackgroundScene":
                        {
                            CommonClass.MapEditor.UseBackgroundScene sbs = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.UseBackgroundScene>(notifyJson);
                            outPut = objI.UseBackgroundSceneF(sbs);
                        }; break;
                    case "CheckCarState":
                        {
                            CommonClass.CheckCarState ccs = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.CheckCarState>(notifyJson);
                            outPut = objI.CheckCarStateF(ccs);
                        }; break;
                    case "GetRewardFromBuildingM":
                        {
                            CommonClass.GetRewardFromBuildingM m = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.GetRewardFromBuildingM>(notifyJson);
                            outPut = objI.GetRewardFromBuildingF(m);
                        }; break;
                    case "GetResistanceObj":
                        {
                            CommonClass.GetResistanceObj r = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.GetResistanceObj>(notifyJson);
                            outPut = objI.GetResistance(r);
                        }; break;
                    case "TakeApart":
                        {
                            CommonClass.TakeApart t = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.TakeApart>(notifyJson);
                            outPut = objI.TakeApartF(t);
                        }; break;
                    case "ServerStatictis":
                        {
                            CommonClass.ServerStatictis ss = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ServerStatictis>(notifyJson);
                            outPut = objI.Statictis(ss);
                        }; break;
                    case "GetCrossBG":
                        {
                            CommonClass.SetCrossBG ss = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.SetCrossBG>(notifyJson);
                            outPut = objI.GetBG(ss);
                        }; break;
                }
            }
            {
                return outPut;
            }
        }

        internal static void IpAndPortMonitor(string hostIP, int tcpPort)
        {
            var dealWith = new TcpFunction.WithResponse.DealWith(DealWithMonitor);
            TcpFunction.WithResponse.ListenIpAndPort(hostIP, tcpPort, dealWith);
        }
        private static async Task<string> DealWithMonitor(string notifyJson)
        {
            return await Task.Run(() => DealWithMonitorValue(notifyJson));
        }

        private static string DealWithMonitorValue(string notifyJson)
        {
            //Consol.WriteLine($"Monitor notify receive:{notifyJson}");

            string outPut = "haveNothingToReturn";
            //{
            //    //  var notifyJson = returnResult.result;

            //    // Console.WriteLine($"json:{notifyJson}");


            //    //Consol.WriteLine($"monitor receive:{notifyJson}");
            //    CommonClass.Monitor m = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.Monitor>(notifyJson);

            //    switch (m.c)
            //    {
            //        case "CheckPlayersCarState":
            //            {
            //                CommonClass.CheckPlayersCarState cpcs = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.CheckPlayersCarState>(notifyJson);
            //                var result = BaseInfomation.rm.Monitor(cpcs);
            //                outPut = result;
            //            }; break;
            //        case "CheckPlayersMoney":
            //            {
            //                CommonClass.CheckPlayersMoney cpcs = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.CheckPlayersMoney>(notifyJson);
            //                var result = BaseInfomation.rm.Monitor(cpcs);
            //                outPut = result;
            //            }; break;
            //        case "CheckPlayerCostBusiness":
            //            {
            //                CommonClass.CheckPlayerCostBusiness cpcs = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.CheckPlayerCostBusiness>(notifyJson);
            //                var result = BaseInfomation.rm.Monitor(cpcs);
            //                outPut = result;
            //            }; break;
            //        case "CheckPromoteDiamondCount":
            //            {
            //                CommonClass.CheckPromoteDiamondCount cpcs = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.CheckPromoteDiamondCount>(notifyJson);
            //                var result = BaseInfomation.rm.Monitor(cpcs);
            //                outPut = result;
            //            }; break;
            //        case "CheckPlayerCarPuporse":
            //            {
            //                CommonClass.CheckPlayerCarPuporse cpcs = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.CheckPlayerCarPuporse>(notifyJson);
            //                var result = BaseInfomation.rm.Monitor(cpcs);
            //                outPut = result;
            //            }; break;
            //        case "CheckPlayerCostVolume":
            //            {
            //                CommonClass.CheckPlayerCostVolume cpcs = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.CheckPlayerCostVolume>(notifyJson);
            //                var result = BaseInfomation.rm.Monitor(cpcs);
            //                outPut = result;
            //            }; break;
            //        case "All":
            //            {
            //                CommonClass.All all = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.All>(notifyJson);
            //                var result = BaseInfomation.rm.Monitor(all);
            //                return result;
            //            }; break;

            //    }
            //}
            return outPut;
        }
    }
}
