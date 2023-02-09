using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static CommonClass.MapEditor;

namespace WsOfWebClient.MapEditor
{
    partial class Editor
    {
        private static async Task Echo(System.Net.WebSockets.WebSocket webSocket)
        {
            WebSocketReceiveResult wResult;
            {
                var returnResult = await ReceiveStringAsync(webSocket, webWsSize);
                wResult = returnResult.wr;
                //Consol.WriteLine($"receive from web:{returnResult.result}");

                var address = await GetBTCAddress(webSocket);
                if (BitCoin.CheckAddress.CheckAddressIsUseful(address)) { }
                else
                {
                    return;
                }


                var hash = CommonClass.Random.GetMD5HashFromStr(DateTime.Now.ToString("yyyyMMddHHmmss") + "yrq");
                {
                    /*
                     * 前台的汽车模型
                     */
                    var msg = Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        c = "SetHash",
                        hash = hash
                    });
                    var sendData = Encoding.ASCII.GetBytes(msg);
                    await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                }
                returnResult = await ReceiveStringAsync(webSocket, webWsSize);
                wResult = returnResult.wr;
                //Consol.WriteLine($"receive from web:{returnResult.result}");

                bool signIsRight = false;

                if (CommonClass.Format.IsBase64(returnResult.result))
                {
                    signIsRight = BitCoin.Sign.checkSign(returnResult.result, hash, address);
                    if (signIsRight)
                    {
                        signIsRight = true;
                    }
                }
                if (signIsRight)
                {
                    bool isAdministartor = false;
                    List<string> administratorAddress = new List<string>()
                    {
                        "1NyrqneGRxTpCohjJdwKruM88JyARB2Ljr",
                        "1NyrqNVai7uCP4CwZoDfQVAJ6EbdgaG6bg"
                    };
                    //string administrator
                    for (var i = 0; i < administratorAddress.Count; i++)
                    {
                        var add = administratorAddress[i];
                        if (add == address)
                        {
                            isAdministartor = true;
                        }
                    }
                    {
                        Random rm = new Random(DateTime.Now.GetHashCode());
                        ModeManger mm = new ModeManger();
                        mm.GetCatege(rm);

                        var modelTypes = mm.GetModelType(rm);
                        mm.SendModelTypes(modelTypes, webSocket);

                        Dictionary<string, bool> roads = new Dictionary<string, bool>();
                        //stateOfSelection ss = stateOfSelection.roadCross;
                        //string roadCode;
                        //int roadOrder;
                        //string anotherRoadCode;
                        //int anotherRoadOrder;

                        var firstRoad = getFirstRoad(rm);
                        //roadCode = firstRoad.roadCode;
                        //roadOrder = firstRoad.roadOrder;
                        //anotherRoadCode = firstRoad.anotherRoadCode;
                        //anotherRoadOrder = firstRoad.anotherRoadOrder;
                        await SetView(firstRoad, webSocket);
                        roads = await Draw(firstRoad, roads, webSocket, rm);
                        stateOfSelection ss = stateOfSelection.roadCross;
                        do
                        {
                            try
                            {

                                returnResult = await ReceiveStringAsync(webSocket, webWsSize);

                                wResult = returnResult.wr;
                                //Consol.WriteLine($"receive from web:{returnResult.result}");
                                //CommonClass.Command
                                var c = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.Command>(returnResult.result);
                                if (c.c == "change")
                                {
                                    // if(ss==stateOfSelection)
                                    if (ss == stateOfSelection.roadCross)
                                    {
                                        ss = stateOfSelection.modelEdit;
                                    }
                                    else
                                        ss = stateOfSelection.roadCross;
                                    await SetState(ss, webSocket);
                                    continue;
                                }
                                else if (c.c == "ShowOBJFile")
                                {
                                    ShowOBJFile sf = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.ShowOBJFile>(returnResult.result);
                                    mm.ShowObj(sf, webSocket, rm);
                                    continue;
                                }
                                switch (ss)
                                {
                                    case stateOfSelection.roadCross:
                                        {
                                            // var c = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.Command>(returnResult.result);
                                            switch (c.c)
                                            {
                                                case "nextCross":
                                                    {
                                                        var r = getNextCross(firstRoad, rm);
                                                        if (r != null)
                                                        {
                                                            firstRoad = getNextCross(firstRoad, rm);
                                                            mm.AddDiction(firstRoad);
                                                        }
                                                        else
                                                        {
                                                            mm.ShowMsg(webSocket, "这是一个单向口，按 B 退出线路");
                                                        }
                                                        //  await mm.GetCrossBG(firstRoad, webSocket, rm);
                                                    }; break;
                                                case "previousCross":
                                                    {
                                                        var r = getPreviousCross(firstRoad, rm);
                                                        if (r != null)
                                                        {
                                                            firstRoad = getPreviousCross(firstRoad, rm);
                                                            mm.AddDiction(firstRoad);
                                                        }
                                                        else
                                                        {
                                                            mm.ShowMsg(webSocket, "这是一个单向口，按 B 退出线路");
                                                        }
                                                        //await mm.GetCrossBG(firstRoad, webSocket, rm);
                                                    }; break;
                                                case "changeRoad":
                                                    {
                                                        var secondRoad = new Position()
                                                        {
                                                            anotherRoadCode = firstRoad.roadCode,
                                                            anotherRoadOrder = firstRoad.roadOrder,
                                                            roadCode = firstRoad.anotherRoadCode,
                                                            roadOrder = firstRoad.anotherRoadOrder,
                                                            c = "Position",
                                                            latitude = firstRoad.latitude,
                                                            longitude = firstRoad.longitude
                                                        };
                                                        firstRoad = secondRoad;
                                                        // await mm.GetCrossBG(firstRoad, webSocket, rm);
                                                    }; break;
                                                case "SetBG":
                                                    {
                                                        var sb = Newtonsoft.Json.JsonConvert.DeserializeObject<SetBG>(returnResult.result);
                                                        mm.SetBackground(sb, firstRoad, address, rm, webSocket);
                                                        mm.GetCrossBG(firstRoad, webSocket, rm);
                                                    }; break;
                                                case "useBackground":
                                                    {
                                                        mm.SetBackground(true, firstRoad, address, rm, webSocket);
                                                        mm.GetCrossBG(firstRoad, webSocket, rm);
                                                    }; break;
                                                case "unuseBackground":
                                                    {
                                                        mm.SetBackground(false, firstRoad, address, rm, webSocket);
                                                        mm.GetCrossBG(firstRoad, webSocket, rm);
                                                    }; break;
                                                case "showBackground":
                                                    {
                                                        mm.GetCrossBG(firstRoad, webSocket, rm);
                                                    }; break;
                                                    //case "addModel":
                                                    //    {

                                                    //        var secondRoad = new Position()
                                                    //        {
                                                    //            anotherRoadCode = firstRoad.roadCode,
                                                    //            anotherRoadOrder = firstRoad.roadOrder,
                                                    //            roadCode = firstRoad.anotherRoadCode,
                                                    //            roadOrder = firstRoad.anotherRoadOrder,
                                                    //            c = "Position",
                                                    //            latitude = firstRoad.latitude,
                                                    //            longitude = firstRoad.longitude
                                                    //        };
                                                    //        firstRoad = secondRoad;
                                                    //    }; break;
                                            }
                                            await SetView(firstRoad, webSocket);
                                            roads = await Draw(firstRoad, roads, webSocket, rm);
                                        }; break;
                                    case stateOfSelection.modelEdit:
                                        {
                                            switch (c.c)
                                            {
                                                case "AddModel":
                                                    {
                                                        mm.addModel = true;
                                                        var m = mm.GetModel(rm);
                                                        await mm.AddModel(m, webSocket);
                                                    }; break;
                                                case "PreviousModel":
                                                    {
                                                        mm.PreviousModel();
                                                        if (mm.addModel)
                                                        {
                                                            var m = mm.GetModel(rm);
                                                            await mm.AddModel(m, webSocket);
                                                        }
                                                    }; break;
                                                case "NextModel":
                                                    {
                                                        mm.NextModel();
                                                        if (mm.addModel)
                                                        {
                                                            var m = mm.GetModel(rm);
                                                            await mm.AddModel(m, webSocket);
                                                        }
                                                    }; break;
                                                case "PreviousUser":
                                                    {
                                                        mm.PreviousUser(address);
                                                        if (mm.addModel)
                                                        {
                                                            var m = mm.GetModel(rm);
                                                            await mm.AddModel(m, webSocket);
                                                        }
                                                    }; break;
                                                case "NextUser":
                                                    {
                                                        mm.NextUser(address);
                                                        if (mm.addModel)
                                                        {
                                                            var m = mm.GetModel(rm);
                                                            await mm.AddModel(m, webSocket);
                                                        }
                                                    }; break;
                                                case "SaveObj":
                                                    {
                                                        SaveObj so = Newtonsoft.Json.JsonConvert.DeserializeObject<SaveObj>(returnResult.result);
                                                        mm.SaveObjF(so, webSocket, rm, address, isAdministartor);
                                                    }; break;
                                                case "EditModel":
                                                    {
                                                        mm.addModel = false;
                                                        EditModel em = Newtonsoft.Json.JsonConvert.DeserializeObject<EditModel>(returnResult.result);
                                                        mm.ID = em.name;
                                                        await mm.EditModel(webSocket);
                                                    }; break;
                                                case "DeleteModel":
                                                    {
                                                        mm.addModel = false;
                                                        DeleteModel dm = Newtonsoft.Json.JsonConvert.DeserializeObject<DeleteModel>(returnResult.result);
                                                        mm.ID = dm.name;
                                                        await mm.DeleteModel(webSocket, dm, rm, isAdministartor);
                                                    }; break;
                                                case "CreateNewObj":
                                                    {
                                                        CreateNewObj cno = Newtonsoft.Json.JsonConvert.DeserializeObject<CreateNewObj>(returnResult.result);
                                                        var msg = mm.CreateNew(cno, address, modelTypes, rm);
                                                        //Consol.WriteLine(msg);
                                                        mm.GetCatege(rm);
                                                    }; break;
                                                case "GetModelDetail":
                                                    {
                                                        GetModelDetail gmd = Newtonsoft.Json.JsonConvert.DeserializeObject<GetModelDetail>(returnResult.result);
                                                        await mm.GetModelDetail(webSocket, gmd, rm);
                                                    }; break;
                                                case "UseModelObj":
                                                    {
                                                        UseModelObj umo = Newtonsoft.Json.JsonConvert.DeserializeObject<UseModelObj>(returnResult.result);
                                                        mm.UseModelObj(webSocket, umo, rm, isAdministartor);
                                                    }; break;
                                                case "LockModelObj":
                                                    {
                                                        UseModelObj umo = Newtonsoft.Json.JsonConvert.DeserializeObject<UseModelObj>(returnResult.result);
                                                        mm.UseModelObj(webSocket, umo, rm, isAdministartor);
                                                    }; break;
                                                case "ClearModelObj":
                                                    {
                                                        mm.ClearModelObj(webSocket, rm, isAdministartor);
                                                    }; break;
                                                case "DownloadModel":
                                                    {
                                                        GetModelDetail gmd = Newtonsoft.Json.JsonConvert.DeserializeObject<GetModelDetail>(returnResult.result);
                                                        mm.DownloadModel(webSocket, gmd, rm);
                                                    }; break;
                                                case "PreviousUnLockedModel":
                                                    {
                                                        mm.GetUnLockedModelID(webSocket, rm, "up");
                                                    }; break;
                                                case "NextUnLockedModel":
                                                    {
                                                        mm.GetUnLockedModelID(webSocket, rm, "");
                                                    }; break;
                                                case "LookForHeight":
                                                    {
                                                        LookForHeight lfh = Newtonsoft.Json.JsonConvert.DeserializeObject<LookForHeight>(returnResult.result);
                                                        await mm.GetHeight(webSocket, lfh, rm);
                                                    }; break;
                                            }
                                        }; break;
                                }
                                switch (c.c)
                                {
                                    case "addChargingWithWord":
                                        {
                                            List<string> administratorOfFinace = new List<string>()
                                            {
                                                "1NyrqneGRxTpCohjJdwKruM88JyARB2Ljr",
                                                "1NyrqNVai7uCP4CwZoDfQVAJ6EbdgaG6bg"
                                            };
                                            if (administratorOfFinace.Contains(address))
                                            {
                                                CommonClass.Finance.Charging cObj = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.Finance.Charging>(returnResult.result);
                                                mm.ChargingSend(webSocket, rm, cObj, address);
                                                mm.ChargingRefresh(webSocket, rm);
                                            }
                                        }; break;
                                    case "chargingRefresh":
                                        {
                                            mm.ChargingRefresh(webSocket, rm);
                                        }; break;
                                    case "chargingNextPage":
                                        {
                                            mm.ChargingNextPage(webSocket, rm);
                                        }; break;
                                    case "chargingPreviousPage":
                                        {
                                            mm.ChargingPreviousPage(webSocket, rm);
                                        }; break;
                                    case "LookForTaskCopy":
                                        {
                                            CommonClass.Finance.LookForTaskCopy lftc = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.Finance.LookForTaskCopy>(returnResult.result);
                                             mm.LookForTaskCopyF(webSocket, lftc, rm);
                                        }; break;
                                    case "TaskCopyPassOrNG": 
                                        {
                                            CommonClass.Finance.TaskCopyPassOrNG pOrNG= Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.Finance.TaskCopyPassOrNG>(returnResult.result);
                                            mm.TaskCopyPassOrNGF(webSocket, pOrNG, rm);
                                        };break;
                                }

                            }
                            catch (Exception e)
                            {
                                throw e;
                            }
                        }
                        while (!wResult.CloseStatus.HasValue);

                    }
                }
            };
        }



        //private static async Task SetBackground(SetBG sb, Position firstRoad, string address, Random rm)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
