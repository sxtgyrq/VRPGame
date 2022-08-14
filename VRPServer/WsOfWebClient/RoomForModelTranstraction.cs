using CommonClass.databaseModel;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static CommonClass.ModelTranstraction;

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
                        tradeDetailList2.Add($"{(item.Value * 10000 / sumValue) / 100}.{ ((item.Value * 10000 / sumValue) % 100).ToString("D2")}%");
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
            var parameter = mts.msg.Split(new char[] { '@', '-', '>', ':' }, StringSplitOptions.RemoveEmptyEntries);
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
            BitCoin.Transtraction.TradeInfo t = new BitCoin.Transtraction.TradeInfo(addr);
            var tradeDetail = await t.GetTradeInfomationFromChain();

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
            {
                for (int i = 0; i < list.Count; i += 2)
                {
                    //Consol.WriteLine(list[i]);
                    var mtsMsg = list[i];
                    var parameter = mtsMsg.Split(new char[] { '@', '-', '>', ':' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parameter.Length == 5)
                    {
                        var sign = list[i + 1];
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



            }
            return tradeDetail;
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

       
    }
}
