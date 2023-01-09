using CommonClass.databaseModel;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static CommonClass.ModelTranstraction;
using System.Linq;
using System.Text.RegularExpressions;
using System.Runtime.Intrinsics.X86;
using System.Net.Http.Headers;

namespace WsOfWebClient
{
    public partial class Room
    {
        class ReceiveResult : CommonClass.ModelTranstraction.GetModelByID.Result
        {
            public string c { get; set; }
            //public string objText { get; set; }
            //public string mtlText { get; set; }
            //public string imageBase64 { get; set; }
        }
        internal async static Task<State> receiveState2(State s, LookForBuildings joinType, WebSocket webSocket)
        {
            // try
            {
                var index = s.roomIndex;
                {
                    var grn = new GetRoadNearby()
                    {
                        c = "GetRoadNearby",
                        x = joinType.x,
                        z = joinType.z,
                        key = s.Key
                    };
                    var msg = Newtonsoft.Json.JsonConvert.SerializeObject(grn);
                    await Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                }
                if (CommonClass.Format.IsModelID(joinType.selectObjName))
                {
                    var gfma = new GetModelByID()
                    {
                        c = "GetModelByID",
                        modelID = joinType.selectObjName
                    };
                    var msg = Newtonsoft.Json.JsonConvert.SerializeObject(gfma);
                    var info = await Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                    if (string.IsNullOrEmpty(info))
                    {
                        return s;
                    }
                    else
                    {
                        string addr;
                        ReceiveResult r;
                        {
                            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.GetModelByID.Result>(info);
                            r = new ReceiveResult()
                            {
                                x = obj.x,
                                bussinessAddress = obj.bussinessAddress,
                                y = obj.y,
                                amodel = obj.amodel,
                                modelID = obj.modelID,
                                rotatey = obj.rotatey,
                                z = obj.z,
                                c = "ReceiveResult",
                                author = obj.author,
                            };
                            var returnMsg = Newtonsoft.Json.JsonConvert.SerializeObject(r);
                            var sendData = Encoding.UTF8.GetBytes(returnMsg);
                            await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                            addr = obj.bussinessAddress;
                        }
                        var tdr = await getTradeDetail(s, webSocket, addr);
                        return tdr;
                    }
                }
                else
                {
                    //Consol.WriteLine($"{joinType.selectObjName}不符合要求！");
                    return s;
                }
            }
            //catch
            //{
            //    return s;
            //} 
        }


        internal static async Task<State> getTradeDetail(State s, WebSocket webSocket, string addr)
        {
            Dictionary<string, long> tradeDetail;
            {
                var grn = new GetTransctionFromChain()
                {
                    c = "GetTransctionFromChain",
                    bussinessAddr = addr,
                };
                var index = rm.Next(0, roomUrls.Count);
                var msg = Newtonsoft.Json.JsonConvert.SerializeObject(grn);
                var data = await Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                tradeDetail = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, long>>(data);
            }

            long sumValue = 0;
            {
                var tradeDetailList = new List<string>();
                sumValue = 0;
                foreach (var item in tradeDetail)
                {
                    tradeDetailList.Add(item.Key);
                    tradeDetailList.Add($"{item.Value / 100000000}.{(item.Value % 100000000).ToString("D8")}");
                    sumValue += item.Value;
                }
                // return result;
                for (int i = 0; i < tradeDetailList.Count; i += 2)
                {
                    var addrStr = tradeDetailList[i];
                    var valueStr = tradeDetailList[i + 1];
                    var passObj = new
                    {
                        c = "TradeDetail",
                        addr = addrStr,
                        value = valueStr,
                        index = i.ToString(),
                    };
                    var msg = Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
                    var sendData = Encoding.UTF8.GetBytes(msg);
                    await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
            if (sumValue == 0)
            {
                return s;
            }
            List<string> list;
            {
                var grn = new GetTransctionModelDetail()
                {
                    c = "GetTransctionModelDetail",
                    bussinessAddr = addr,
                };
                var index = rm.Next(0, roomUrls.Count);
                var msg = Newtonsoft.Json.JsonConvert.SerializeObject(grn);
                var json = await Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(json);

                char[] SplitChars = new char[4] { ':', '-', '@', '>' };
                for (int i = 0; i < list.Count; i += 2)
                {
                    var itemValue = list[i];
                    var splitDetail = itemValue.Split(SplitChars, StringSplitOptions.RemoveEmptyEntries);
                    var mainAddr = "";
                    if (splitDetail.Length == 5)
                    {
                        mainAddr = splitDetail[3] + ',' + splitDetail[4];
                        var agreeMent = list[i];
                        var sign = list[i + 1];
                        var passObj = new
                        {
                            c = "TradeDetail2",
                            mainAddr = mainAddr,
                            agreeMent = agreeMent,
                            sign = sign,
                            index = i.ToString(),
                        };
                        var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
                        var sendData = Encoding.UTF8.GetBytes(sendMsg);
                        await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                }
            }
            {
                for (int i = 0; i < list.Count; i += 2)
                {
                    //Consol.WriteLine(list[i]);
                    var mtsMsg = list[i];
                    var parameter = mtsMsg.Split(new char[] { '@', '-', '>', ':' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parameter.Length == 5)
                    {
                        var sign = list[i + 1];
                        //    if (BitCoin.Sign.checkSign(sign, mtsMsg, parameter[1]))
                        {
                            var tradeIndex = int.Parse(parameter[0]);
                            var addrFrom = parameter[1];
                            var addrBussiness = parameter[2];
                            var addrTo = parameter[3];

                            var passCoinStr = parameter[4];
                            if (passCoinStr.Substring(passCoinStr.Length - 7, 7) == "Satoshi")
                            {
                                var passCoin = Convert.ToInt64(passCoinStr.Substring(0, passCoinStr.Length - 7));

                                if (tradeDetail.ContainsKey(addrFrom))
                                {
                                    if (tradeDetail[addrFrom] >= passCoin)
                                    {
                                        tradeDetail[addrFrom] -= passCoin;
                                        if (tradeDetail.ContainsKey(addrTo))
                                        {
                                            tradeDetail[addrTo] += passCoin;
                                        }
                                        else
                                        {
                                            tradeDetail.Add(addrTo, passCoin);
                                        }
                                    }
                                }

                            }


                        }
                    }
                }

                var tradeDetailList2 = new List<string>();
                foreach (var item in tradeDetail)
                {
                    if (item.Value > 0)
                    {
                        tradeDetailList2.Add(item.Key);
                        tradeDetailList2.Add($"{item.Value / 100000000}.{(item.Value % 100000000).ToString("D8")}");
                        tradeDetailList2.Add($"{(item.Value * 10000 / sumValue) / 100}.{((item.Value * 10000 / sumValue) % 100).ToString("D2")}%");
                    }

                }
                for (int i = 0; i < tradeDetailList2.Count; i += 3)
                {
                    var addrStr = tradeDetailList2[i];
                    var valueStr = tradeDetailList2[i + 1];
                    var percentValue = tradeDetailList2[i + 2];
                    var passObj3 = new
                    {
                        //detail = tradeDetailList2,
                        c = "TradeDetail3",
                        addrStr = addrStr,
                        valueStr = valueStr,
                        indexStr = i.ToString(),
                        percentValue = percentValue
                    };
                    var passMsg = Newtonsoft.Json.JsonConvert.SerializeObject(passObj3);
                    var sendData = Encoding.UTF8.GetBytes(passMsg);
                    await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
            return s;
        }
        private static async Task<string> drawRoad(string roadCode, Random rm)
        {
            var index = rm.Next(0, roomUrls.Count);
            var roomUrl = roomUrls[index];
            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(new CommonClass.MapEditor.DrawRoad()
            {
                c = "DrawRoad",
                roadCode = roadCode
            });
            var json = await Startup.sendInmationToUrlAndGetRes(roomUrl, sendMsg);
            return json;
        }

        static async Task<string[]> initialize(Random rm, string amID)
        {
            string[] result = new string[3] { "", "", "" };
            var index = rm.Next(0, roomUrls.Count);
            var roomUrl = roomUrls[index];
            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(
                new CommonClass.MapEditor.GetAbtractModels
                {
                    c = "GetAbtractModels",
                    amID = amID
                });
            var json = await Startup.sendInmationToUrlAndGetRes(roomUrl, sendMsg);
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<abtractmodelsPassData>(json);
            result[0] = obj.imageBase64;
            result[1] = obj.objText;
            result[2] = obj.mtlText;
            return result;
        }

        internal static async Task GenerateAgreementF(State s, WebSocket webSocket, GenerateAgreement ga)
        {
            if (
                BitCoin.CheckAddress.CheckAddressIsUseful(ga.addrBussiness) &&
                BitCoin.CheckAddress.CheckAddressIsUseful(ga.addrFrom) &&
                BitCoin.CheckAddress.CheckAddressIsUseful(ga.addrTo) &&
                ga.tranNum >= 0.00000001
                )
            {
                int indexNumber = 0;
                indexNumber = await GetIndexOfTrade(ga.addrBussiness, ga.addrFrom);
                if (indexNumber >= 0)
                {
                    //var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(new CommonClass.MapEditor.DrawRoad()
                    //{
                    //    c = "DrawRoad",
                    //    roadCode = roadCode
                    //});
                    //var json = await Startup.sendInmationToUrlAndGetRes(roomUrl, sendMsg);

                    var agreement = $"{indexNumber}@{ga.addrFrom}@{ga.addrBussiness}->{ga.addrTo}:{ga.tranNum * 100000000}Satoshi";
                    var passObj = new
                    {
                        agreement = agreement,
                        c = "ShowAgreement"
                    };
                    var returnMsg = Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
                    var sendData = Encoding.UTF8.GetBytes(returnMsg);
                    await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
            //throw new NotImplementedException();
        }

        private static async Task<int> GetIndexOfTrade(string addrBussiness, string addrFrom)
        {
            var ti = new TradeIndex()
            {
                c = "TradeIndex",
                addrFrom = addrFrom,
                addrBussiness = addrBussiness
            };
            var index = rm.Next(0, roomUrls.Count);
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(ti);
            var info = await Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
            return Convert.ToInt32(info);
        }

        internal static async Task ModelTransSignF(State s, WebSocket webSocket, ModelTransSign mts)
        {
            try
            {
                var parameter = mts.msg.Split(new char[] { '@', '-', '>', ':' }, StringSplitOptions.RemoveEmptyEntries);
                //  var agreement = $"{indexNumber}@{ga.addrFrom}@{ga.addrBussiness}->{ga.addrTo}:{ga.tranNum * 100000000}Satoshi";
                var regex = new Regex("^[0-9]{1,8}@[A-HJ-NP-Za-km-z1-9]{1,50}@[A-HJ-NP-Za-km-z1-9]{1,50}->[A-HJ-NP-Za-km-z1-9]{1,50}:[0-9]{1,13}Satoshi$");
                if (regex.IsMatch(mts.msg))
                {
                    if (parameter.Length == 5)
                    {
                        if (BitCoin.Sign.checkSign(mts.sign, mts.msg, parameter[1]))
                        {
                            var tradeIndex = int.Parse(parameter[0]);
                            var addrFrom = parameter[1];
                            var addrBussiness = parameter[2];
                            var addrTo = parameter[3];

                            var passCoinStr = parameter[4];
                            if (passCoinStr.Substring(passCoinStr.Length - 7, 7) == "Satoshi")
                            {
                                var trDetail = await getValueOfAddr(addrBussiness);
                                var passCoin = Convert.ToInt64(passCoinStr.Substring(0, passCoinStr.Length - 7));
                                if (passCoin > 0)
                                {
                                    if (trDetail.ContainsKey(addrFrom))
                                    {
                                        if (trDetail[addrFrom] >= passCoin)
                                        {
                                            var tc = new TradeCoin()
                                            {
                                                tradeIndex = tradeIndex,
                                                addrBussiness = addrBussiness,
                                                addrFrom = addrFrom,
                                                addrTo = addrTo,
                                                c = "TradeCoin",
                                                msg = mts.msg,
                                                passCoin = passCoin,
                                                sign = mts.sign,
                                            };
                                            var index = rm.Next(0, roomUrls.Count);
                                            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(tc);
                                            var info = await Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                                            var resultObj = Newtonsoft.Json.JsonConvert.DeserializeObject<TradeCoin.Result>(info);
                                            await NotifyMsg(webSocket, resultObj.msg);

                                            if (resultObj.success)
                                            {
                                                var ok = await clearInfomation(webSocket);
                                                if (ok)
                                                    s = await getTradeDetail(s, webSocket, addrBussiness);
                                            }
                                        }
                                        else
                                        {
                                            var notifyMsg = $"{addrFrom}没有足够的余额。";
                                            await NotifyMsg(webSocket, notifyMsg);
                                        }
                                    }
                                    else
                                    {
                                        var notifyMsg = $"{addrFrom}没有足够的余额。";
                                        await NotifyMsg(webSocket, notifyMsg);
                                    }
                                }
                            }
                        }
                        else
                        {
                            await NotifyMsg(webSocket, "无效的签名!");
                        }
                    }
                }
            }
            catch
            {
                await NotifyMsg(webSocket, "交易失败!");
            }
        }

        internal static async Task PublicReward(State s, WebSocket webSocket, RewardPublicSign rewardPub)
        {
            var parameter = rewardPub.msg.Split(new char[] { '@', '-', '>', ':' }, StringSplitOptions.RemoveEmptyEntries);
            var firstIndex = rewardPub.msg.IndexOf('@');
            var secondIndex = rewardPub.msg.IndexOf('@', firstIndex + 1);
            if (secondIndex > firstIndex)
            {
            }
            else
            {
                return;
            }

            if (parameter.Length == 5)
            {
                if (BitCoin.Sign.checkSign(rewardPub.signOfAddrReward, rewardPub.msg, parameter[1]))
                {
                    if (BitCoin.Sign.checkSign(rewardPub.signOfAddrBussiness, rewardPub.msg, parameter[2]))
                    {
                        var tradeIndex = int.Parse(parameter[0]);

                        var addrFrom = parameter[1];
                        var addrBussiness = parameter[2];
                        var addrTo = parameter[3];
                        if (addrTo == "SetAsReward")
                        {
                            var indexV = await GetIndexOfTrade(addrBussiness, addrFrom);
                            if (indexV < 0)
                            {
                                await NotifyMsg(webSocket, $"错误的addrBussiness:{addrBussiness}");
                            }
                            else if (tradeIndex == indexV)
                            {
                                var passCoinStr = parameter[4];

                                if (passCoinStr.Substring(passCoinStr.Length - 7, 7) == "Satoshi")
                                {
                                    var passCoin = Convert.ToInt64(passCoinStr.Substring(0, passCoinStr.Length - 7));
                                    if (passCoin > 0)
                                    {
                                        var trDetail = await getValueOfAddr(addrBussiness);
                                        if (trDetail.ContainsKey(addrFrom))
                                        {
                                            if (trDetail[addrFrom] >= passCoin)
                                            {
                                                var tc = new TradeSetAsReward()
                                                {
                                                    tradeIndex = tradeIndex,
                                                    addrBussiness = addrBussiness,
                                                    addrReward = addrFrom,
                                                    c = "TradeSetAsReward",
                                                    msg = rewardPub.msg,
                                                    passCoin = passCoin,
                                                    signOfaddrBussiness = rewardPub.signOfAddrBussiness,
                                                    signOfAddrReward = rewardPub.signOfAddrReward
                                                };
                                                var index = rm.Next(0, roomUrls.Count);
                                                var msg = Newtonsoft.Json.JsonConvert.SerializeObject(tc);
                                                var info = await Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);

                                                var resultObj = Newtonsoft.Json.JsonConvert.DeserializeObject<TradeSetAsReward.Result>(info);
                                                {
                                                    await NotifyMsg(webSocket, resultObj.msg);
                                                }
                                            }
                                            else
                                            {
                                                var notifyMsg = $"{addrFrom}没有足够的余额。";
                                                await NotifyMsg(webSocket, notifyMsg);
                                            }
                                        }
                                        else
                                        {
                                            var notifyMsg = $"{addrFrom}没有足够的余额。";
                                            await NotifyMsg(webSocket, notifyMsg);
                                        }
                                    }
                                }

                            }
                            else
                            {
                                await NotifyMsg(webSocket, $"错误的tradeIndex:{tradeIndex}");
                            }

                        }
                    }
                }

                //if (BitCoin.Sign.checkSign(mts.signOfAddr,))
                //{



                //    var addrTo = parameter[3];



                //    {
                //        // var trDetail = await getValueOfAddr(addrBussiness);

                //        // var passCoin = Convert.ToInt64(passCoinStr.Substring(0, passCoinStr.Length - 7));
                //        if (passCoin > 0)
                //        {
                //            if (trDetail.ContainsKey(addrFrom))
                //            {
                //                if (trDetail[addrFrom] >= passCoin)
                //                {
                //                    var tc = new TradeCoin()
                //                    {
                //                        tradeIndex = tradeIndex,
                //                        addrBussiness = addrBussiness,
                //                        addrFrom = addrFrom,
                //                        addrTo = addrTo,
                //                        c = "TradeCoin",
                //                        msg = mts.msg,
                //                        passCoin = passCoin,
                //                        sign = mts.sign,
                //                    };
                //                    var index = rm.Next(0, roomUrls.Count);
                //                    var msg = Newtonsoft.Json.JsonConvert.SerializeObject(tc);
                //                    var info = await Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                //                    if (string.IsNullOrEmpty(info))
                //                    {
                //                        var ok = await clearInfomation(webSocket);
                //                        if (ok)
                //                            s = await getTradeDetail(s, webSocket, addrBussiness);
                //                    }
                //                    else
                //                    {
                //                        await NotifyMsg(webSocket, info);
                //                    }
                //                }
                //                else
                //                {
                //                    var notifyMsg = $"{addrFrom}没有足够的余额。";
                //                    await NotifyMsg(webSocket, notifyMsg);
                //                }
                //            }
                //            else
                //            {
                //                var notifyMsg = $"{addrFrom}没有足够的余额。";
                //                await NotifyMsg(webSocket, notifyMsg);
                //            }
                //        }
                //    }
                //}
            }
        }
        internal static async Task GetAllStockAddr(WebSocket webSocket, AllStockAddr asa)
        {
            if (AdministratorBTCAddr._addresses.ContainsKey(asa.administratorAddr))
            {
                if (BitCoin.Sign.checkSign(asa.signOfAdministrator, DateTime.Now.ToString("yyyyMMdd"), asa.administratorAddr))
                {
                    if (BitCoin.CheckAddress.CheckAddressIsUseful(asa.bAddr))
                    {
                        //    Console.WriteLine(asa.bAddr);
                        //var index = rm.Next(0, roomUrls.Count);
                        //var msg = Newtonsoft.Json.JsonConvert.SerializeObject(asa);
                        //var info = await Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                        //var f = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(info);

                        var trDetail = await getValueOfAddr(asa.bAddr);
                        foreach (var i in trDetail)
                        {
                            //       Console.WriteLine($"{i.Key},{i.Value}");
                            var passObj = new
                            {
                                id = "stockAddrForAddReward",
                                c = "addOption",
                                value = $"{i.Key}:{i.Value}"
                            };
                            var returnMsg = Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
                            var sendData = Encoding.UTF8.GetBytes(returnMsg);
                            await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                        }
                    }
                }
            }
        }

        internal static async Task GenerateRewardAgreementF(WebSocket webSocket, GenerateRewardAgreement ga)
        {
            if (AdministratorBTCAddr._addresses.ContainsKey(ga.administratorAddr))
            {
                if (BitCoin.Sign.checkSign(ga.signOfAdministrator, DateTime.Now.ToString("yyyyMMdd"), ga.administratorAddr))
                {
                    if (
      BitCoin.CheckAddress.CheckAddressIsUseful(ga.addrBussiness) &&
      BitCoin.CheckAddress.CheckAddressIsUseful(ga.addrFrom) &&
      ga.tranNum >= 0.00000001
      )
                    {
                        int indexNumber = 0;
                        indexNumber = await GetIndexOfTrade(ga.addrBussiness, ga.addrFrom);
                        if (indexNumber >= 0)
                        {
                            //var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(new CommonClass.MapEditor.DrawRoad()
                            //{
                            //    c = "DrawRoad",
                            //    roadCode = roadCode
                            //});
                            //var json = await Startup.sendInmationToUrlAndGetRes(roomUrl, sendMsg);

                            var agreement = $"{indexNumber}@{ga.addrFrom}@{ga.addrBussiness}->SetAsReward:{ga.tranNum}Satoshi";
                            var passObj = new
                            {
                                agreement = agreement,
                                c = "ShowRewardAgreement"
                            };
                            var returnMsg = Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
                            var sendData = Encoding.UTF8.GetBytes(returnMsg);
                            await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                        }
                    }
                }
            }

        }
        internal static async Task<string> GetAllBusinessAddr(WebSocket webSocket, RewardSet rs)
        {
            string r = "";
            if (AdministratorBTCAddr._addresses.ContainsKey(rs.administratorAddr))
            {
                if (BitCoin.Sign.checkSign(rs.signOfAdministrator, DateTime.Now.ToString("yyyyMMdd"), rs.administratorAddr))
                {
                    var ti = new AllBuiisnessAddr()
                    {
                        c = "AllBuiisnessAddr"
                    };
                    r = r.GetHashCode().ToString();
                    var index = rm.Next(0, roomUrls.Count);
                    var msg = Newtonsoft.Json.JsonConvert.SerializeObject(ti);
                    var info = await Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                    var f = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(info);
                    for (int i = 0; i < f.Count; i++)
                    {
                        var passObj = new
                        {
                            id = "buidingAddrForAddReward",
                            c = "addOption",
                            value = f[i]
                        };
                        var returnMsg = Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
                        var sendData = Encoding.UTF8.GetBytes(returnMsg);
                        await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                        r = r.GetHashCode().ToString() + f[i];
                    }
                }
            }
            return r;
        }
        internal static async Task RewardPublicSignF(State s, WebSocket webSocket, RewardPublicSign rewardPub)
        {
            //var parameter = rewardPub.msg.Split(new char[] { '@', '-', '>', ':' }, StringSplitOptions.RemoveEmptyEntries);
            //var firstIndex = rewardPub.msg.IndexOf('@');
            //var secondIndex = rewardPub.msg.IndexOf('@', firstIndex + 1);
            //if (secondIndex > firstIndex)
            //{
            //}
            //else
            //{
            //    return;
            //}
            var regex = new Regex("^[0-9]{1,8}@[A-HJ-NP-Za-km-z1-9]{1,50}@[A-HJ-NP-Za-km-z1-9]{1,50}->SetAsReward:[0-9]{1,13}Satoshi$");
            if (regex.IsMatch(rewardPub.msg))
            {
                var parameter = rewardPub.msg.Split(new char[] { '@', '-', '>', ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (BitCoin.Sign.checkSign(rewardPub.signOfAddrReward, rewardPub.msg, parameter[1]))
                {
                    if (BitCoin.Sign.checkSign(rewardPub.signOfAddrBussiness, rewardPub.msg, parameter[2]))
                    {
                        var tradeIndex = int.Parse(parameter[0]);

                        var addrFrom = parameter[1];
                        var addrBussiness = parameter[2];
                        var addrTo = parameter[3];
                        if (addrTo == "SetAsReward")
                        {
                            if (tradeIndex == await GetIndexOfTrade(addrBussiness, addrFrom))
                            {
                                var passCoinStr = parameter[4];

                                if (passCoinStr.Substring(passCoinStr.Length - 7, 7) == "Satoshi")
                                {
                                    var passCoin = Convert.ToInt64(passCoinStr.Substring(0, passCoinStr.Length - 7));
                                    if (passCoin > 0)
                                    {
                                        var trDetail = await getValueOfAddr(addrBussiness);
                                        if (trDetail.ContainsKey(addrFrom))
                                        {
                                            if (trDetail[addrFrom] >= passCoin)
                                            {
                                                var tc = new TradeSetAsReward()
                                                {
                                                    tradeIndex = tradeIndex,
                                                    addrBussiness = addrBussiness,
                                                    addrReward = addrFrom,
                                                    c = "TradeSetAsReward",
                                                    msg = rewardPub.msg,
                                                    passCoin = passCoin,
                                                    signOfaddrBussiness = rewardPub.signOfAddrBussiness,
                                                    signOfAddrReward = rewardPub.signOfAddrReward
                                                };
                                                var index = rm.Next(0, roomUrls.Count);
                                                var msg = Newtonsoft.Json.JsonConvert.SerializeObject(tc);
                                                var info = await Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                                                if (string.IsNullOrEmpty(info))
                                                {
                                                    var ok = await clearInfomation(webSocket);
                                                    if (ok)
                                                        s = await getTradeDetail(s, webSocket, addrBussiness);
                                                }
                                                else
                                                {
                                                    await NotifyMsg(webSocket, info);
                                                }
                                            }
                                            else
                                            {
                                                var notifyMsg = $"{addrFrom}没有足够的余额。";
                                                await NotifyMsg(webSocket, notifyMsg);
                                            }
                                        }
                                        else
                                        {
                                            var notifyMsg = $"{addrFrom}没有足够的余额。";
                                            await NotifyMsg(webSocket, notifyMsg);
                                        }
                                    }
                                }

                            }


                        }
                    }
                }

                //if (BitCoin.Sign.checkSign(mts.signOfAddr,))
                //{



                //    var addrTo = parameter[3];



                //    {
                //        // var trDetail = await getValueOfAddr(addrBussiness);

                //        // var passCoin = Convert.ToInt64(passCoinStr.Substring(0, passCoinStr.Length - 7));
                //        if (passCoin > 0)
                //        {
                //            if (trDetail.ContainsKey(addrFrom))
                //            {
                //                if (trDetail[addrFrom] >= passCoin)
                //                {
                //                    var tc = new TradeCoin()
                //                    {
                //                        tradeIndex = tradeIndex,
                //                        addrBussiness = addrBussiness,
                //                        addrFrom = addrFrom,
                //                        addrTo = addrTo,
                //                        c = "TradeCoin",
                //                        msg = mts.msg,
                //                        passCoin = passCoin,
                //                        sign = mts.sign,
                //                    };
                //                    var index = rm.Next(0, roomUrls.Count);
                //                    var msg = Newtonsoft.Json.JsonConvert.SerializeObject(tc);
                //                    var info = await Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                //                    if (string.IsNullOrEmpty(info))
                //                    {
                //                        var ok = await clearInfomation(webSocket);
                //                        if (ok)
                //                            s = await getTradeDetail(s, webSocket, addrBussiness);
                //                    }
                //                    else
                //                    {
                //                        await NotifyMsg(webSocket, info);
                //                    }
                //                }
                //                else
                //                {
                //                    var notifyMsg = $"{addrFrom}没有足够的余额。";
                //                    await NotifyMsg(webSocket, notifyMsg);
                //                }
                //            }
                //            else
                //            {
                //                var notifyMsg = $"{addrFrom}没有足够的余额。";
                //                await NotifyMsg(webSocket, notifyMsg);
                //            }
                //        }
                //    }
                //}
            }
        }

        //internal static async Task ModelTransSignAsReward(State s, WebSocket webSocket, ModelTransSign mts)
        //{
        //    var parameter = mts.msg.Split(new char[] { '@', '-', '>', ':' }, StringSplitOptions.RemoveEmptyEntries);
        //    if (parameter.Length == 5)
        //    {
        //        if (BitCoin.Sign.checkSign(mts.sign, mts.msg, parameter[1]))
        //            if (BitCoin.Sign.checkSign(mts.sign, mts.msg, parameter[1]))
        //            {
        //                var tradeIndex = int.Parse(parameter[0]);
        //                var addrFrom = parameter[1];
        //                var addrBussiness = parameter[2];
        //                var addrTo = parameter[3];
        //                if (addrTo == "GameReward")
        //                {
        //                    var passCoinStr = parameter[4];
        //                    if (passCoinStr.Substring(passCoinStr.Length - 7, 7) == "Satoshi")
        //                    {
        //                        var trDetail = await getValueOfAddr(addrBussiness);

        //                        var passCoin = Convert.ToInt64(passCoinStr.Substring(0, passCoinStr.Length - 7));
        //                        if (passCoin > 0)
        //                        {
        //                            if (trDetail.ContainsKey(addrFrom))
        //                            {
        //                                if (trDetail[addrFrom] >= passCoin)
        //                                {
        //                                    var tc = new TradeSetAsReward()
        //                                    {

        //                                        tradeIndex = tradeIndex,
        //                                        addrBussiness = addrBussiness,
        //                                        addrFrom = addrFrom,
        //                                        addrTo = addrTo,
        //                                        c = "TradeSetAsReward",
        //                                        msg = mts.msg,
        //                                        passCoin = passCoin,
        //                                        sign = mts.sign,
        //                                    };
        //                                    var index = rm.Next(0, roomUrls.Count);
        //                                    var msg = Newtonsoft.Json.JsonConvert.SerializeObject(tc);
        //                                    var info = await Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);

        //                                    if (string.IsNullOrEmpty(info))
        //                                    {
        //                                        var ok = await clearInfomation(webSocket);
        //                                        if (ok)
        //                                            s = await getTradeDetail(s, webSocket, addrBussiness);
        //                                    }
        //                                    else
        //                                    {
        //                                        await NotifyMsg(webSocket, info);
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    var notifyMsg = $"{addrFrom}没有足够的余额。";
        //                                    await NotifyMsg(webSocket, notifyMsg);
        //                                }
        //                            }
        //                            else
        //                            {
        //                                var notifyMsg = $"{addrFrom}没有足够的余额。";
        //                                await NotifyMsg(webSocket, notifyMsg);
        //                            }
        //                        }
        //                    }
        //                }


        //            }
        //    }
        //}

        static async Task<bool> clearInfomation(WebSocket webSocket)
        {
            // var notifyMsg = info;
            var passObj = new
            {
                c = "ClearTradeInfomation"
            };
            var returnMsg = Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
            var sendData = Encoding.UTF8.GetBytes(returnMsg);
            await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
            var ok = await CheckRespon(webSocket, "ClearTradeInfomation");
            return ok;
        }

        private static async Task NotifyMsg(WebSocket webSocket, string info)
        {
            var notifyMsg = info;
            var passObj = new
            {
                msg = notifyMsg,
                c = "ShowAgreementMsg"
            };
            var returnMsg = Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
            var sendData = Encoding.UTF8.GetBytes(returnMsg);
            await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        internal static async Task<State> GetAllModelPositionF(State s, WebSocket webSocket)
        {
            var gfma = new GetAllModelPosition()
            {
                c = "GetAllModelPosition",

            };
            var index = rm.Next(0, roomUrls.Count);
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(gfma);
            var info = await Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
            if (string.IsNullOrEmpty(info))
            {
                return s;
            }
            else
            {
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GetAllModelPosition.Result>>(info);
                if (result.Count == 0)
                {
                    return s;
                }
                else
                {
                    var minX = double.MaxValue;
                    var minY = double.MaxValue;
                    var maxX = double.MinValue;
                    var maxY = double.MinValue;
                    for (int i = 0; i < result.Count; i++)
                    {
                        minX = Math.Min(result[i].x, minX);
                        maxX = Math.Max(result[i].x, maxX);
                        minY = Math.Min(result[i].z, minY);
                        maxY = Math.Max(result[i].z, maxY);
                    }

                }
                //var minX = double.MinValue;
                //var passObj = new
                //{
                //    list = result,
                //    c = "ShowAllPts"
                //};
                //var returnMsg = Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
                //var sendData = Encoding.UTF8.GetBytes(returnMsg);
                //await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                return s;
            }
        }

        internal async static Task<Dictionary<string, long>> getValueOfAddr(string addr)
        {
            // BitCoin.Transtraction.TradeInfo t = new BitCoin.Transtraction.TradeInfo(addr);
            var tradeDetail = await ConsoleBitcoinChainApp.GetData.GetTradeInfomationFromChain(addr);

            List<string> list;
            {
                var grn = new GetTransctionModelDetail()
                {
                    c = "GetTransctionModelDetail",
                    bussinessAddr = addr,
                };
                var index = rm.Next(0, roomUrls.Count);
                var msg = Newtonsoft.Json.JsonConvert.SerializeObject(grn);
                var json = await Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(json);
            }
            var r = ConsoleBitcoinChainApp.GetData.SetTrade(ref tradeDetail, list);
            return r;
        }

        internal static async Task<string> GetResistanceF(State s, GetResistance gr)
        {
            var grn = new CommonClass.GetResistanceObj()
            {
                c = "GetResistanceObj",
                KeyLookfor = gr.KeyLookfor,
                key = s.Key,
                RequestType = gr.RequestType
            };
            var index = s.roomIndex;
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(grn);
            var respon = await Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
            return respon;
        }

        internal static async Task GetRewardInfomation(WebSocket webSocket, RewardInfomation gra)
        {
            var date = DateTime.Now;
            if (gra.Page > 52)
            {
                gra.Page = 52;
            }
            else if (gra.Page < -52)
            {
                gra.Page = -52;
            }
            date = date.AddDays(gra.Page * 7);
            while (date.DayOfWeek != DayOfWeek.Monday)
            {
                date = date.AddDays(-1);
            }
            var objGet = await exitPageF(date.ToString("yyyyMMdd"));

            if (objGet == null)
            {
                var passObj = new
                {
                    c = "GetRewardInfomationHasNotResult",
                    title = $"{date.ToString("yyyyMMdd")}期"
                };
                var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
                var sendData = Encoding.UTF8.GetBytes(sendMsg);
                await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);

            }
            else
            {
                var passObj = await getResultObj(objGet, date);
                if (passObj != null)
                {
                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
                    var sendData = Encoding.UTF8.GetBytes(sendMsg);
                    await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                }
                //int indexNumber = 0;
                //indexNumber = await GetIndexOfTrade(objGet.bussinessAddr, objGet.tradeAddress);
                //List<CommonClass.databaseModel.traderewardapply> list;
                //{
                //    var grn = new CommonClass.ModelTranstraction.RewardInfomation()
                //    {
                //        c = "RewardApplyInfomation",
                //        startDate = int.Parse(date.ToString("yyyyMMdd"))
                //    };
                //    var index = rm.Next(0, roomUrls.Count);
                //    var msg = Newtonsoft.Json.JsonConvert.SerializeObject(grn);
                //    Console.WriteLine(msg);
                //    var respon = await Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                //    Console.WriteLine(respon);
                //    list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CommonClass.databaseModel.traderewardapply>>(respon);
                //}
                //int sumStock = 0;
                //{
                //    for (int i = 0; i < list.Count; i++)
                //    {
                //        sumStock += list[i].applyLevel;
                //    }

                //}
                //if (sumStock <= objGet.passCoin)
                //{
                //    var satoshiPerStock = objGet.passCoin / sumStock;
                //    var remainder = objGet.passCoin % sumStock;
                //    var orderR = (from item in list
                //                  orderby item.applyLevel descending
                //                  select item).ToList();
                //    list = orderR;
                //    List<RewardApplyInDB> raList = new List<RewardApplyInDB>();
                //    for (int i = 0; i < list.Count; i++)
                //    {
                //        int satoshiShouldGet = list[i].applyLevel * satoshiPerStock;
                //        if (remainder > list[i].applyLevel)
                //        {
                //            satoshiShouldGet += list[i].applyLevel;
                //            remainder -= list[i].applyLevel;
                //        }
                //        else if (remainder > 0)
                //        {
                //            satoshiShouldGet += remainder;
                //            remainder = 0;
                //        }
                //        int percent = (satoshiShouldGet * 10000 / objGet.passCoin);
                //        var percentStr = $"{percent / 100}.{(percent % 100).ToString("D2")}%";
                //        raList.Add(new RewardApplyInDB()
                //        {
                //            applyAddr = list[i].applyAddr,
                //            applyLevel = list[i].applyLevel,
                //            applySign = list[i].applySign,
                //            rankIndex = list[i].rankIndex,
                //            startDate = list[i].startDate,
                //            satoshiShouldGet = satoshiShouldGet,
                //            percentStr = percentStr,
                //        });
                //    }

                //    var passObj = new
                //    {
                //        c = "GetRewardInfomationHasResult",
                //        title = $"{date.ToString("yyyyMMdd")}期",
                //        data = objGet,
                //        list = raList,
                //        indexNumber = indexNumber
                //    };
                //    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
                //    var sendData = Encoding.UTF8.GetBytes(sendMsg);
                //    await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                //}
            }
        }

        static async Task<RewardInfoHasResultObj> getResultObj(tradereward objGet, DateTime date)
        {
            int indexNumber = 0;
            indexNumber = await GetIndexOfTrade(objGet.bussinessAddr, objGet.tradeAddress);
            List<CommonClass.databaseModel.traderewardapply> list;
            {
                var grn = new CommonClass.ModelTranstraction.RewardInfomation()
                {
                    c = "RewardApplyInfomation",
                    startDate = int.Parse(date.ToString("yyyyMMdd"))
                };
                var index = rm.Next(0, roomUrls.Count);
                var msg = Newtonsoft.Json.JsonConvert.SerializeObject(grn);
                Console.WriteLine(msg);
                var respon = await Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                Console.WriteLine(respon);
                list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CommonClass.databaseModel.traderewardapply>>(respon);
            }
            int sumStock = 0;
            {
                for (int i = 0; i < list.Count; i++)
                {
                    sumStock += list[i].applyLevel;
                }

            }
            if (sumStock == 0)
            {
                List<RewardApplyInDB> raList = new List<RewardApplyInDB>();
                var passObj = new RewardInfoHasResultObj()
                {
                    c = "GetRewardInfomationHasResult",
                    title = $"{date.ToString("yyyyMMdd")}期",
                    data = objGet,
                    list = raList,
                    indexNumber = indexNumber
                };
                return passObj;
            }
            else if (sumStock <= objGet.passCoin)
            {
                var satoshiPerStock = objGet.passCoin / sumStock;
                var remainder = objGet.passCoin % sumStock;
                var orderR = (from item in list
                              orderby item.applyLevel descending
                              select item).ToList();
                list = orderR;
                List<RewardApplyInDB> raList = new List<RewardApplyInDB>();
                for (int i = 0; i < list.Count; i++)
                {
                    int satoshiShouldGet = list[i].applyLevel * satoshiPerStock;
                    if (remainder > list[i].applyLevel)
                    {
                        satoshiShouldGet += list[i].applyLevel;
                        remainder -= list[i].applyLevel;
                    }
                    else if (remainder > 0)
                    {
                        satoshiShouldGet += remainder;
                        remainder = 0;
                    }
                    int percent = (satoshiShouldGet * 10000 / objGet.passCoin);
                    var percentStr = $"{percent / 100}.{(percent % 100).ToString("D2")}%";
                    raList.Add(new RewardApplyInDB()
                    {
                        applyAddr = list[i].applyAddr,
                        applyLevel = list[i].applyLevel,
                        applySign = list[i].applySign,
                        rankIndex = list[i].rankIndex,
                        startDate = list[i].startDate,
                        satoshiShouldGet = satoshiShouldGet,
                        percentStr = percentStr,
                    });
                }

                var passObj = new RewardInfoHasResultObj()
                {
                    c = "GetRewardInfomationHasResult",
                    title = $"{date.ToString("yyyyMMdd")}期",
                    data = objGet,
                    list = raList,
                    indexNumber = indexNumber
                };
                return passObj;
                //var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
                //var sendData = Encoding.UTF8.GetBytes(sendMsg);
                //await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            else
            {
                return null;
            }
        }
        class RewardInfoHasResultObj : CommonClass.Command
        {
            public string title { get; set; }
            public tradereward data { get; set; }
            public List<RewardApplyInDB> list { get; set; }
            public int indexNumber { get; set; }
        }
        internal static async Task GiveAward(WebSocket webSocket, AwardsGiving ag)
        {
            var objGet = await exitPageF(ag.time);
            if (objGet == null) { }
            else
            {
                var startInt = objGet.startDate;
                var dt = new DateTime(startInt / 10000, (startInt / 100) % 100, startInt % 100);
                var r = await getResultObj(objGet, dt);
                if (r == null)
                { }
                else
                {
                    List<string> msgsToTransfer = new List<string>();
                    bool IsRight = true;
                    List<string> msgs = new List<string>();
                    List<int> ranks = new List<int>();
                    List<string> applyAddr = new List<string>();

                    if (r.list.Count == ag.list.Count)
                        for (int i = 0; i < r.list.Count; i++)
                        {

                            var msg = $"{r.indexNumber + i}@{objGet.tradeAddress}@{objGet.bussinessAddr}->{r.list[i].applyAddr}:{r.list[i].satoshiShouldGet}Satoshi";
                            var signature = ag.list[i];
                            if (BitCoin.Sign.checkSign(signature, msg, objGet.tradeAddress))
                            { }
                            else { IsRight = false; }
                            msgs.Add(msg);
                            ranks.Add(r.list[i].rankIndex);
                            applyAddr.Add(r.list[i].applyAddr);
                        }
                    if (IsRight)
                    {
                        AwardsGivingPass awardsGivingPass = new AwardsGivingPass()
                        {
                            c = "AwardsGivingPass",
                            list = ag.list,
                            time = ag.time,
                            msgs = msgs,
                            ranks = ranks,
                            applyAddr = applyAddr
                        };
                        var index = rm.Next(0, roomUrls.Count);
                        var msg = Newtonsoft.Json.JsonConvert.SerializeObject(awardsGivingPass);
                        var json = await Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                    }
                }
            }
            //  var passObj = await getResultObj(objGet, date);
            // throw new NotImplementedException();
        }

        internal static async Task<bool> BindWordInfoF(IntroState iState, WebSocket webSocket, CommonClass.ModelTranstraction.BindWordInfo bwi)
        {
            if (bwi.verifyCodeValue != null && iState.randomValue.Trim().ToLower() == bwi.verifyCodeValue.Trim().ToLower())
            {
                bwi.bindWordSign = bwi.bindWordSign.Trim();
                bwi.bindWordMsg = bwi.bindWordMsg.Trim();
                bwi.bindWordAddr = bwi.bindWordAddr.Trim();
                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("^[\u4e00-\u9fa5]{2,10}$");
                if (reg.IsMatch(bwi.bindWordMsg))
                {
                    if (BitCoin.Sign.checkSign(bwi.bindWordSign, bwi.bindWordMsg, bwi.bindWordAddr))
                    {
                        var index = rm.Next(0, roomUrls.Count);
                        var msg = Newtonsoft.Json.JsonConvert.SerializeObject(bwi);
                        var msgRequested = await Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                        if (!string.IsNullOrEmpty(msgRequested))
                        {
                            var requestObj = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.BindWordInfo.Result>(msgRequested);
                            if (requestObj.success)
                            {
                                await NotifyMsg(webSocket, $"绑定成功！{requestObj.msg}");
                            }
                            else
                            {
                                await NotifyMsg(webSocket, $"绑定失败！{requestObj.msg}");
                            }
                        }
                        else
                        {
                            await NotifyMsg(webSocket, "程序异常！");
                        }
                        iState.randomCharacterCount = 4;
                        iState.randomValue = Room.GetRandom(iState.randomCharacterCount);
                        await Room.setRandomPic(iState, webSocket);
                    }
                    else
                    {
                        iState.randomCharacterCount++;
                        iState.randomValue = Room.GetRandom(iState.randomCharacterCount);
                        await Room.setRandomPic(iState, webSocket);
                        await NotifyMsg(webSocket, "绑定词，您的签名错误，绑定失败！");
                    }
                }
                else
                {
                    iState.randomCharacterCount++;
                    iState.randomValue = Room.GetRandom(iState.randomCharacterCount);
                    await Room.setRandomPic(iState, webSocket);
                    await NotifyMsg(webSocket, "绑定词，须由2-10个汉字组成！");
                }
                return true;
            }
            else
            {
                iState.randomCharacterCount++;
                iState.randomValue = Room.GetRandom(iState.randomCharacterCount);
                await Room.setRandomPic(iState, webSocket);
                await NotifyMsg(webSocket, "验证码输入错误");
                return false;
            }
        }


        internal static async Task<bool> LookForBindInfoF(IntroState iState, WebSocket webSocket, CommonClass.ModelTranstraction.LookForBindInfo lbi)
        {
            if (lbi.verifyCodeValue != null && iState.randomValue.Trim().ToLower() == lbi.verifyCodeValue.Trim().ToLower())
            {
                var index = rm.Next(0, roomUrls.Count);
                lbi.infomation = lbi.infomation.Trim();
                var msg = Newtonsoft.Json.JsonConvert.SerializeObject(lbi);
                var msgRequested = await Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                if (!string.IsNullOrEmpty(msgRequested))
                {
                    var requestObj = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.LookForBindInfo.Result>(msgRequested);
                    if (requestObj.success)
                    {
                        await NotifyMsg(webSocket, $"{requestObj.msg}");
                    }
                    else
                    {
                        await NotifyMsg(webSocket, $"没有查询到绑定关系");
                    }
                }
                else
                {
                    await NotifyMsg(webSocket, "程序异常！");
                }
                iState.randomCharacterCount = 4;
                iState.randomValue = Room.GetRandom(iState.randomCharacterCount);
                await Room.setRandomPic(iState, webSocket);
                return true;
            }
            else
            {
                iState.randomCharacterCount++;
                iState.randomValue = Room.GetRandom(iState.randomCharacterCount);
                await Room.setRandomPic(iState, webSocket);
                await NotifyMsg(webSocket, "验证码输入错误");
                return false;
            }
        }

        internal static async Task RewardApply(WebSocket webSocket, RewardApply rA)
        {
            var date = DateTime.Now;
            while (date.DayOfWeek != DayOfWeek.Monday)
            {
                date = date.AddDays(-1);
            }
            var dateStr = date.ToString("yyyyMMdd");
            if (dateStr == rA.msgNeedToSign)
            {
                if (BitCoin.Sign.checkSign(rA.signature, rA.msgNeedToSign, rA.addr))
                {
                    var index = rm.Next(0, roomUrls.Count);
                    var msg = Newtonsoft.Json.JsonConvert.SerializeObject(rA);
                    var msgRequested = await Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                    if (!string.IsNullOrEmpty(msgRequested))
                    {
                        var requestObj = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.RewardApply.Result>(msgRequested);
                        //  Console.WriteLine(msgRequested);
                        if (requestObj.success)
                        {
                            await NotifyMsg(webSocket, requestObj.msg);
                        }
                        else
                        {
                            await NotifyMsg(webSocket, requestObj.msg);
                        }
                    }
                    else
                        await NotifyMsg(webSocket, "系统错误");
                }
                else
                {
                    await NotifyMsg(webSocket, "错误的签名");
                }
            }
            else
            {
                await NotifyMsg(webSocket, $"现在只能申请{date.ToString("yyyyMMdd")}期奖励。");
            }
            //if(rA.msgNeedToSign)

            //var grn = new CommonClass.ModelTranstraction.RewardInfomation()
            //{
            //    c = "RewardInfomation",
            //    startDate = Convert.ToInt32(v)
            //};
            //var index = rm.Next(0, roomUrls.Count);
            //var msg = Newtonsoft.Json.JsonConvert.SerializeObject(grn);
            //var json = await Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
            //if (string.IsNullOrEmpty(json))
            //{
            //    return null;
            //}
            //return Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.databaseModel.tradereward>(json);

        }
        private async static Task<CommonClass.databaseModel.tradereward> exitPageF(string v)
        {
            var grn = new CommonClass.ModelTranstraction.RewardInfomation()
            {
                c = "RewardInfomation",
                startDate = Convert.ToInt32(v)
            };
            var index = rm.Next(0, roomUrls.Count);
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(grn);
            var json = await Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }
            return Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.databaseModel.tradereward>(json);
        }
    }

    public partial class Room
    {
        internal async static Task<int> RewardBuildingShowF(State s, WebSocket webSocket, RewardBuildingShow rbs)
        {
            // try
            {
                var index = rm.Next(0, roomUrls.Count);
                var msg = Newtonsoft.Json.JsonConvert.SerializeObject(rbs);
                var info = await Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                List<string> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(info);

                for (int i = 0; i < list.Count; i++)
                {
                    var dataItem = list[i].Trim();
                    var sendData = Encoding.UTF8.GetBytes(dataItem);
                    await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                }
                return list.Count;
            }
        }
    }
}
