using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using static CommonClass.MapEditor;

namespace WsOfWebClient.MapEditor
{
    class Editor
    {
        class SaveObj : CommonClass.Command
        {
            public double x { get; set; }
            public double y { get; set; }
            public double z { get; set; }
            public double rotationY { get; set; }
        }
        class EditModel : CommonClass.Command
        {
            public string name { get; set; }
        }
        class DeleteModel : EditModel
        {
            public double x { get; set; }
            public double z { get; set; }
        }

        const int webWsSize = 1024 * 3;
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IWebHostEnvironment env)
        {



            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseWebSockets();
            // app.useSt(); // For the wwwroot folder
            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(
            //"F:\\MyProject\\VRPWithZhangkun\\MainApp\\VRPWithZhangkun\\VRPServer\\WebApp\\webHtml"),
            //    RequestPath = "/StaticFiles"
            //});

            //app.Map("/postinfo", HandleMapdownload);
            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(3600 * 24),
                ReceiveBufferSize = webWsSize
            };
            app.UseWebSockets(webSocketOptions);

            app.Map("/websocket", WebSocketF);

            // app.Map("/notify", notify);

            //Console.WriteLine($"启动TCP连接！{ ConnectInfo.tcpServerPort}");
            //Thread th = new Thread(() => startTcp());
            //th.Start();
        }
        private static void WebSocketF(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                if (context.WebSockets.IsWebSocketRequest)
                {

                    var webSocket = await context.WebSockets.AcceptWebSocketAsync();

                    await Echo(webSocket);
                }
            });
        }
        public class ReceiveObj
        {
            public WebSocketReceiveResult wr { get; set; }
            public string result { get; set; }
        }
        public static async Task<ReceiveObj> ReceiveStringAsync(System.Net.WebSockets.WebSocket socket, int size, CancellationToken ct = default(CancellationToken))
        {
            var buffer = new ArraySegment<byte>(new byte[size]);
            WebSocketReceiveResult result;
            using (var ms = new MemoryStream())
            {
                do
                {
                    // ct.IsCancellationRequested
                    ct.ThrowIfCancellationRequested();

                    result = await socket.ReceiveAsync(buffer, ct);

                    ms.Write(buffer.Array, buffer.Offset, result.Count);
                }
                while (!result.EndOfMessage);

                ms.Seek(0, SeekOrigin.Begin);
                if (result.MessageType != WebSocketMessageType.Text)
                {
                    return new ReceiveObj()
                    {
                        result = null,
                        wr = result
                    };
                }
                using (var reader = new StreamReader(ms, Encoding.UTF8))
                {
                    var strValue = await reader.ReadToEndAsync();
                    return new ReceiveObj()
                    {
                        result = strValue,
                        wr = result
                    };
                }
            }
        }

        enum stateOfSelection
        {
            roadCross,
            modelEdit
        }
        class ModeManger
        {
            public bool addModel { get; set; }
            public ModeManger()
            {
                this.indexOfSelect = 0;
                this.material = new List<aModel>();
                this.addModel = false;
            }

            public string ID { get; set; }
            int indexOfSelect { get; set; }
            List<aModel> material { get; set; }
            //  private static async Task<CommonClass.MapEditor.Position>
            internal async Task GetCatege(Random rm)
            {
                var index = rm.Next(0, roomUrls.Count);
                var roomUrl = roomUrls[index];
                var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(
                    new
                    {
                        c = "GetCatege",
                    });
                var json = await Startup.sendInmationToUrlAndGetRes(roomUrl, sendMsg);
                Console.WriteLine(json);
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(json);
                // return obj;
                //  throw new NotImplementedException();
                for (var i = 0; i < obj.Count; i += 2)
                {
                    aModel am = new aModel(obj[i], obj[i + 1]);
                    material.Add(am);
                }
                var x = (from item in material
                         orderby item.modelType, item.modelName ascending
                         select item).ToList();
                material = x.ToList();
            }
            public async Task AddModel(aModel m, WebSocket webSocket)
            {
                this.ID = "n" + Guid.NewGuid().ToString("N");
                var sn = new
                {
                    c = "AddModel",
                    WebSocketID = 0,
                    aModel = m,
                    id = this.ID
                };
                var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(sn);
                var sendData = Encoding.ASCII.GetBytes(sendMsg);
                await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            public async Task DrawModel(aModel m, CommonClass.databaseModel.detailmodel dm, WebSocket webSocket)
            {
                //this.ID = "n" + Guid.NewGuid().ToString("N");
                var sn = new
                {
                    c = "DrawModel",
                    WebSocketID = 0,
                    aModel = m,
                    id = dm.modelID,
                    x = dm.x,
                    y = dm.y,
                    z = dm.z,
                    r = dm.rotatey
                };
                var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(sn);
                var sendData = Encoding.ASCII.GetBytes(sendMsg);
                await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            public async Task SaveObjF(SaveObj so, WebSocket webSocket, Random rm)
            {
                if (this.addModel)
                {
                    var index = rm.Next(0, roomUrls.Count);
                    var roomUrl = roomUrls[index];
                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(new SaveObjInfo()
                    {
                        c = "SaveObjInfo",
                        amodel = material[this.indexOfSelect].modelName,
                        modelID = this.ID,
                        rotatey = so.rotationY,
                        x = so.x,
                        y = so.y,
                        z = so.z
                    });
                    await Startup.sendInmationToUrlAndGetRes(roomUrl, sendMsg);
                }
                else
                {
                    var index = rm.Next(0, roomUrls.Count);
                    var roomUrl = roomUrls[index];
                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(new UpdateObjInfo()
                    {
                        c = "UpdateObjInfo",
                        modelID = this.ID,
                        rotatey = so.rotationY,
                        x = so.x,
                        y = so.y,
                        z = so.z
                    });
                    await Startup.sendInmationToUrlAndGetRes(roomUrl, sendMsg);

                    ShowOBJFile sf = new ShowOBJFile()
                    {
                        c = "ShowOBJFile",
                        x = so.x,
                        z = so.z
                    };
                    await this.ShowObj(sf, webSocket, rm);
                }
                //var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.Position>(json);
                //return obj;
                //  throw new NotImplementedException();
            }
            internal async Task<aModel> GetModel(Random rm)
            {
                if (this.material[this.indexOfSelect].initialized)
                {

                }
                else
                {
                    await this.material[this.indexOfSelect].initialize(rm);
                    this.material[this.indexOfSelect].initialized = true;
                }
                return this.material[this.indexOfSelect];
                // throw new NotImplementedException();
            }

            public async Task ShowObj(ShowOBJFile sf, WebSocket webSocket, Random rm)
            {
                var index = rm.Next(0, roomUrls.Count);
                var roomUrl = roomUrls[index];
                var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(sf);
                var json = await Startup.sendInmationToUrlAndGetRes(roomUrl, sendMsg);
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.ObjResult>(json);



                for (int i = 0; i < obj.detail.Count; i++)
                {
                    var objInfomation = this.material.Find(item => item.modelName == obj.detail[i].amodel);
                    await objInfomation.initialize(rm);
                    await this.DrawModel(objInfomation, obj.detail[i], webSocket);
                }
                // return obj;
                //  throw new NotImplementedException();
            }

            internal void PreviousModel()
            {
                if (this.addModel)
                {
                    this.indexOfSelect--;
                    if (this.indexOfSelect < 0)
                    {
                        this.indexOfSelect = this.material.Count - 1;
                    }
                }
                //  throw new NotImplementedException();
            }
            internal void NextModel()
            {
                if (this.addModel)
                {
                    this.indexOfSelect++;
                    if (this.indexOfSelect >= this.material.Count)
                    {
                        this.indexOfSelect = 0;
                    }
                }
            }
            internal void PreviousCategory()
            {
                if (this.addModel)
                {
                    if ((from item in this.material group item by item.modelType).Count() < 1)
                    { }
                    else
                    {
                        var current = this.material[this.indexOfSelect];
                        do
                        {
                            this.indexOfSelect--;
                            this.indexOfSelect = (this.indexOfSelect + this.material.Count) % this.material.Count;
                        } while (this.material[this.indexOfSelect].modelType == current.modelType);
                    }
                }
            }
            internal void NextCategory()
            {
                if (this.addModel)
                {
                    if ((from item in this.material group item by item.modelType).Count() < 1)
                    { }
                    else
                    {
                        var current = this.material[this.indexOfSelect];
                        do
                        {
                            this.indexOfSelect++;
                            this.indexOfSelect = (this.indexOfSelect + this.material.Count) % this.material.Count;
                        } while (this.material[this.indexOfSelect].modelType == current.modelType);
                    }
                }
            }
            internal async Task EditModel(WebSocket webSocket)
            {
                var sn = new
                {
                    c = "EditModel",
                    id = this.ID
                };
                var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(sn);
                var sendData = Encoding.ASCII.GetBytes(sendMsg);
                await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                // throw new NotImplementedException();
            }

            internal async Task DeleteModel(WebSocket webSocket, DeleteModel dm, Random rm)
            {
                var index = rm.Next(0, roomUrls.Count);
                var roomUrl = roomUrls[index];
                var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(new DelObjInfo()
                {
                    c = "DelObjInfo",
                    modelID = this.ID,
                });
                await Startup.sendInmationToUrlAndGetRes(roomUrl, sendMsg);

                ShowOBJFile sf = new ShowOBJFile()
                {
                    c = "ShowOBJFile",
                    x = dm.x,
                    z = dm.z,
                };
                await this.ShowObj(sf, webSocket, rm);
            }
        }
        class aModel : abtractmodels
        {
            public aModel(string v1, string v2)
            {
                this.modelName = v1;
                this.modelType = v2;
                this.initialized = false;
            }
            public bool initialized { get; set; }

            internal async Task initialize(Random rm)
            {
                var index = rm.Next(0, roomUrls.Count);
                var roomUrl = roomUrls[index];
                var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(
                    new CommonClass.MapEditor.GetAbtractModels
                    {
                        c = "GetAbtractModels",
                        modelName = this.modelName
                    });
                var json = await Startup.sendInmationToUrlAndGetRes(roomUrl, sendMsg);
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<abtractmodels>(json);
                this.imageBase64 = obj.imageBase64;
                this.objText = obj.objText;
                this.mtlText = obj.mtlText;
                this.animation = obj.animation;

            }
        }
        class abtractmodels : CommonClass.databaseModel.abtractmodels
        {
        }
        private static async Task Echo(System.Net.WebSockets.WebSocket webSocket)
        {
            WebSocketReceiveResult wResult;
            {
                var returnResult = await ReceiveStringAsync(webSocket, webWsSize);
                wResult = returnResult.wr;
                Console.WriteLine($"receive from web:{returnResult.result}");
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
                Console.WriteLine($"receive from web:{returnResult.result}");

                List<string> allowedAddress = new List<string>()
                {
                    "1NyrqneGRxTpCohjJdwKruM88JyARB2Ljr",
                    "1NyrqNVai7uCP4CwZoDfQVAJ6EbdgaG6bg"
                };
                bool signIsRight = false;
                //if (CommonClass.Format.IsBase64(returnResult.result))
                //{
                //    for (var i = 0; i < allowedAddress.Count; i++)
                //    {

                //        var address = allowedAddress[i];
                //        signIsRight = BitCoin.Sign.checkSign(returnResult.result, hash, address);
                //        if (signIsRight)
                //        {
                //            break;
                //        }
                //    }
                //}
                signIsRight = true;
                if (signIsRight)
                {
                    Random rm = new Random(DateTime.Now.GetHashCode());
                    ModeManger mm = new ModeManger();
                    await mm.GetCatege(rm);

                    Dictionary<string, bool> roads = new Dictionary<string, bool>();
                    //stateOfSelection ss = stateOfSelection.roadCross;
                    //string roadCode;
                    //int roadOrder;
                    //string anotherRoadCode;
                    //int anotherRoadOrder;

                    var firstRoad = await getFirstRoad(rm);
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
                            Console.WriteLine($"receive from web:{returnResult.result}");
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
                                await mm.ShowObj(sf, webSocket, rm);
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
                                                    firstRoad = await getNextCross(firstRoad, rm);
                                                }; break;
                                            case "previousCross":
                                                {
                                                    firstRoad = await getPreviousCross(firstRoad, rm);
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
                                                    var m = await mm.GetModel(rm);
                                                    await mm.AddModel(m, webSocket);
                                                }; break;
                                            case "PreviousModel":
                                                {
                                                    mm.PreviousModel();
                                                    if (mm.addModel)
                                                    {
                                                        var m = await mm.GetModel(rm);
                                                        await mm.AddModel(m, webSocket);
                                                    }
                                                }; break;
                                            case "NextModel":
                                                {
                                                    mm.NextModel();
                                                    if (mm.addModel)
                                                    {
                                                        var m = await mm.GetModel(rm);
                                                        await mm.AddModel(m, webSocket);
                                                    }
                                                }; break;
                                            case "PreviousCategory":
                                                {
                                                    mm.PreviousCategory();
                                                    if (mm.addModel)
                                                    {
                                                        var m = await mm.GetModel(rm);
                                                        await mm.AddModel(m, webSocket);
                                                    }
                                                }; break;
                                            case "NextCategory":
                                                {
                                                    mm.NextCategory();
                                                    if (mm.addModel)
                                                    {
                                                        var m = await mm.GetModel(rm);
                                                        await mm.AddModel(m, webSocket);
                                                    }
                                                }; break;
                                            case "SaveObj":
                                                {
                                                    SaveObj so = Newtonsoft.Json.JsonConvert.DeserializeObject<SaveObj>(returnResult.result);
                                                    await mm.SaveObjF(so, webSocket, rm);
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
                                                    await mm.DeleteModel(webSocket, dm, rm);
                                                }; break;
                                        }
                                    }; break;
                            }

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"{ Newtonsoft.Json.JsonConvert.SerializeObject(e)}");
                            //await Room.setOffLine(s);
                            //removeWs(s.WebsocketID);
                            // Console.WriteLine($"step2：webSockets数量：{   BufferImage.webSockets.Count}");
                            // return;
                            throw e;
                        }
                    }
                    while (!wResult.CloseStatus.HasValue);

                }
            };
        }



        private static async Task SetState(stateOfSelection ss, WebSocket webSocket)
        {
            //  mm.ID = Guid.NewGuid().ToString();
            var sn = new
            {
                c = "SetState",
                state = ss.ToString()
            };
            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(sn);
            var sendData = Encoding.ASCII.GetBytes(sendMsg);
            await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
            // throw new NotImplementedException();
        }



        private static async Task<CommonClass.MapEditor.Position> getPreviousCross(Position firstRoad, Random rm)
        {
            var index = rm.Next(0, roomUrls.Count);
            var roomUrl = roomUrls[index];
            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(new NextCross()
            {
                c = "PreviousCross",
                roadCode = firstRoad.roadCode,
                roadOrder = firstRoad.roadOrder,
                anotherRoadCode = firstRoad.anotherRoadCode,
                anotherRoadOrder = firstRoad.anotherRoadOrder
            });
            var json = await Startup.sendInmationToUrlAndGetRes(roomUrl, sendMsg);
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.Position>(json);
            return obj;
        }
        private static async Task<CommonClass.MapEditor.Position> getNextCross(Position firstRoad, Random rm)
        {
            var index = rm.Next(0, roomUrls.Count);
            var roomUrl = roomUrls[index];
            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(new NextCross()
            {
                c = "NextCross",
                roadCode = firstRoad.roadCode,
                roadOrder = firstRoad.roadOrder,
                anotherRoadCode = firstRoad.anotherRoadCode,
                anotherRoadOrder = firstRoad.anotherRoadOrder
            });
            var json = await Startup.sendInmationToUrlAndGetRes(roomUrl, sendMsg);
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.Position>(json);
            return obj;
        }

        private static async Task<Dictionary<string, bool>> Draw(Position firstRoad, Dictionary<string, bool> roads, WebSocket webSocket, Random rm)
        {
            var roadCode = firstRoad.roadCode;
            var roadOrder = firstRoad.roadOrder;
            var anotherRoadCode = firstRoad.anotherRoadCode;
            var anotherRoadOrder = firstRoad.anotherRoadOrder;
            if (roads.ContainsKey(roadCode))
            {

            }
            else
            {
                roads.Add(roadCode, true);
                var msg = await drawRoad(roadCode, rm);
                {
                    var sendData = Encoding.ASCII.GetBytes(msg);
                    await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
            if (roads.ContainsKey(anotherRoadCode))
            {

            }
            else
            {
                roads.Add(anotherRoadCode, true);
                var msg = await drawRoad(anotherRoadCode, rm);
                {
                    var sendData = Encoding.ASCII.GetBytes(msg);
                    await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
            return roads;
        }

        private static async Task SetView(Position firstRoad, WebSocket webSocket)
        {
            double MacatuoX, MacatuoY;
            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(firstRoad.longitude, firstRoad.latitude, out MacatuoX, out MacatuoY);
            CommonClass.ViewSearch sn = new CommonClass.ViewSearch()
            {
                c = "ViewSearch",
                WebSocketID = 0,
                mctX = Convert.ToInt32(MacatuoX * 256),
                mctY = Convert.ToInt32(MacatuoY * 256),
            };
            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(sn);
            var sendData = Encoding.ASCII.GetBytes(sendMsg);
            await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        }
        private static async Task<string> drawRoad(string roadCode, Random rm)
        {
            var index = rm.Next(0, roomUrls.Count);
            var roomUrl = roomUrls[index];
            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(new DrawRoad()
            {
                c = "DrawRoad",
                roadCode = roadCode
            });
            var json = await Startup.sendInmationToUrlAndGetRes(roomUrl, sendMsg);
            return json;
        }

        public static List<string> roomUrls
        {
            get { return Room.roomUrls; }
        }
        private static async Task<CommonClass.MapEditor.Position> getFirstRoad(Random rm)
        {
            var index = rm.Next(0, roomUrls.Count);
            var roomUrl = roomUrls[index];
            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                c = "GetFirstRoad",

            });
            var json = await Startup.sendInmationToUrlAndGetRes(roomUrl, sendMsg);
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.Position>(json);
            //roadCode = obj.roadCode;
            //roadOrder = obj.roadOrder;
            //anotherRoadCode = obj.anotherRoadCode;
            //anotherRoadOrder = obj.anotherRoadOrder;
            return obj;
        }
        //private static async Task<CommonClass.MapEditor.Position> getFrequency(string roomUrl)
        //{

        //    var result = await Startup.sendInmationToUrlAndGetRes(roomUrl, sendMsg);


        //}
    }
}
