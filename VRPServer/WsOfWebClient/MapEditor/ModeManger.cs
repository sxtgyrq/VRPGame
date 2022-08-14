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
using System.Drawing;

namespace WsOfWebClient.MapEditor
{
    partial class Editor
    {
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
                //Consol.WriteLine(json);
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(json);
                // return obj;
                //  throw new NotImplementedException();
                this.material.Clear();
                for (var i = 0; i < obj.Count; i += 2)
                {
                    aModel am = new aModel(obj[i], obj[i + 1]);
                    material.Add(am);
                }
                var x = (from item in material
                         orderby item.author, item.amID ascending
                         select item).ToList();
                material = x.ToList();
                // return obj;
            }
            internal async Task<List<string>> GetModelType(Random rm)
            {
                var index = rm.Next(0, roomUrls.Count);
                var roomUrl = roomUrls[index];
                var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(
                    new
                    {
                        c = "GetModelType",
                    });
                var json = await Startup.sendInmationToUrlAndGetRes(roomUrl, sendMsg);
                //Consol.WriteLine(json);
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(json);
                return obj;
            }
            public async Task SendModelTypes(List<string> modelTypes, WebSocket webSocket)
            {
                // this.ID = "n" + Guid.NewGuid().ToString("N");
                var sn = new
                {
                    c = "ShowModelTypes",
                    modelTypes = modelTypes
                };
                var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(sn);
                var sendData = Encoding.UTF8.GetBytes(sendMsg);
                await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
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
                var sendData = Encoding.UTF8.GetBytes(sendMsg);
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
            public async Task SaveObjF(SaveObj so, WebSocket webSocket, Random rm, string author, bool isAdministartor)
            {
                if (material[this.indexOfSelect].author == author)
                { }
                else if (isAdministartor)
                {

                }
                else
                {
                    await ShowMsg(webSocket, "只有管理员与作者才能编辑该模型！");
                    return;
                }
                if (this.addModel)
                {
                    var index = rm.Next(0, roomUrls.Count);
                    var roomUrl = roomUrls[index];
                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(new SaveObjInfo()
                    {
                        c = "SaveObjInfo",
                        amID = material[this.indexOfSelect].amID,
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

            internal void AddDiction(Position baseCross)
            {
                string firstRoadcode, secondRoadcode;
                int firstRoadorder, secondRoadorder;
                if (baseCross.roadCode.CompareTo(baseCross.anotherRoadCode) > 0)
                {
                    firstRoadcode = baseCross.roadCode;
                    secondRoadcode = baseCross.anotherRoadCode;
                    firstRoadorder = baseCross.roadOrder;
                    secondRoadorder = baseCross.anotherRoadOrder;
                }
                else
                {
                    firstRoadcode = baseCross.anotherRoadCode;
                    secondRoadcode = baseCross.roadCode;
                    firstRoadorder = baseCross.anotherRoadOrder;
                    secondRoadorder = baseCross.roadOrder;
                }
                var crossName = $"{firstRoadcode}{firstRoadorder}{secondRoadcode}{secondRoadorder}";
                // var respon = await Startup.sendInmationToUrlAndGetRes(roomUrl, sendMsg);
                {
                    if (Directory.Exists($"imgT/{crossName}/"))
                    {

                    }
                    else
                    {
                        Directory.CreateDirectory($"imgT/{crossName}/");
                    }
                }
            }

            //internal Task GetCrossBG(Position firstRoad)
            //{
            //    throw new NotImplementedException();
            //}

            internal async Task GetCrossBG(Position baseCross, WebSocket webSocket, Random rm)
            {
                string firstRoadcode, secondRoadcode;
                int firstRoadorder, secondRoadorder;
                if (baseCross.roadCode.CompareTo(baseCross.anotherRoadCode) > 0)
                {
                    firstRoadcode = baseCross.roadCode;
                    secondRoadcode = baseCross.anotherRoadCode;
                    firstRoadorder = baseCross.roadOrder;
                    secondRoadorder = baseCross.anotherRoadOrder;
                }
                else
                {
                    firstRoadcode = baseCross.anotherRoadCode;
                    secondRoadcode = baseCross.roadCode;
                    firstRoadorder = baseCross.anotherRoadOrder;
                    secondRoadorder = baseCross.roadOrder;
                }
                // if (true) { }
                GetBackgroundScene ubs = new GetBackgroundScene()
                {
                    c = "GetBackgroundScene",
                    firstRoadcode = firstRoadcode,
                    secondRoadorder = secondRoadorder,
                    firstRoadorder = firstRoadorder,
                    secondRoadcode = secondRoadcode
                };
                var index = rm.Next(0, roomUrls.Count);
                var roomUrl = roomUrls[index];
                var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(ubs);
                var crossName = $"{firstRoadcode}{firstRoadorder}{secondRoadcode}{secondRoadorder}";
                var respon = await Startup.sendInmationToUrlAndGetRes(roomUrl, sendMsg);
                {
                    if (Directory.Exists($"imgT/{crossName}/"))
                    {

                    }
                    else
                    {
                        Directory.CreateDirectory($"imgT/{crossName}/");
                    }
                }

                //Consol.WriteLine($"respon:{respon}");
                GetBackgroundScene.Result r = Newtonsoft.Json.JsonConvert.DeserializeObject<GetBackgroundScene.Result>(respon);
                var sm = new
                {
                    c = "SetBackgroundScene",
                    r = r,
                    name = crossName
                    //  name=$"{ubs.firstRoadcode}{ubs.}{}{}"
                };
                sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(sm);
                var sendData = Encoding.UTF8.GetBytes(sendMsg);
                await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);

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
                    var objInfomation = this.material.Find(item => item.amID == obj.detail[i].amodel);
                    await objInfomation.initialize(rm);
                    await this.DrawModel(objInfomation, obj.detail[i], webSocket);
                }
            }



            static Image LoadBase64(string base64)
            {
                byte[] bytes = Convert.FromBase64String(base64);
                Image image;
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    image = Image.FromStream(ms);
                }

                return new Bitmap(image, 512, 512);
            }

            public string ImageToBase64(string imagePath)
            {
                string result;
                using (Image image = Image.FromFile(imagePath))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        byte[] imageBytes = ms.ToArray();
                        string base64String = Convert.ToBase64String(imageBytes);
                        result = $"data:image/jpeg;base64,{base64String}";
                    }
                }
                return result;
            }
            internal async Task SetBackground(bool isUsed, Position baseCross, string address, Random rm, WebSocket webSocket)
            {
                string firstRoadcode, secondRoadcode;
                int firstRoadorder, secondRoadorder;
                if (baseCross.roadCode.CompareTo(baseCross.anotherRoadCode) > 0)
                {
                    firstRoadcode = baseCross.roadCode;
                    secondRoadcode = baseCross.anotherRoadCode;
                    firstRoadorder = baseCross.roadOrder;
                    secondRoadorder = baseCross.anotherRoadOrder;
                }
                else
                {
                    firstRoadcode = baseCross.anotherRoadCode;
                    secondRoadcode = baseCross.roadCode;
                    firstRoadorder = baseCross.anotherRoadOrder;
                    secondRoadorder = baseCross.roadOrder;
                }
                {
                    UseBackgroundScene ubs = new UseBackgroundScene()
                    {
                        c = "UseBackgroundScene",
                        firstRoadcode = firstRoadcode,
                        firstRoadorder = firstRoadorder,
                        secondRoadcode = secondRoadcode,
                        secondRoadorder = secondRoadorder,
                        used = isUsed,
                    };
                    var index = rm.Next(0, roomUrls.Count);
                    var roomUrl = roomUrls[index];
                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(ubs);
                    var respon = await Startup.sendInmationToUrlAndGetRes(roomUrl, sendMsg);
                    //GetBackgroundScene.Result r = Newtonsoft.Json.JsonConvert.DeserializeObject<GetBackgroundScene.Result>(respon);
                    //var sm = new
                    //{
                    //    c = "SetBackgroundScene",
                    //    r = r,
                    //};
                    //sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(sm);
                    //var sendData = Encoding.UTF8.GetBytes(sendMsg);
                    //await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
            internal async Task SetBackground(SetBG sb, Position baseCross, string address, Random rm, WebSocket webSocket)
            {
                string firstRoadcode, secondRoadcode;
                int firstRoadorder, secondRoadorder;
                if (baseCross.roadCode.CompareTo(baseCross.anotherRoadCode) > 0)
                {
                    firstRoadcode = baseCross.roadCode;
                    secondRoadcode = baseCross.anotherRoadCode;
                    firstRoadorder = baseCross.roadOrder;
                    secondRoadorder = baseCross.anotherRoadOrder;
                    // crossKey = $"{baseCross.roadCode}{baseCross.roadOrder}{baseCross.anotherRoadCode}{baseCross.anotherRoadOrder}";
                }
                else
                {
                    firstRoadcode = baseCross.anotherRoadCode;
                    secondRoadcode = baseCross.roadCode;
                    firstRoadorder = baseCross.anotherRoadOrder;
                    secondRoadorder = baseCross.roadOrder;
                    //  crossKey = $"{baseCross.anotherRoadCode}{baseCross.anotherRoadOrder}{baseCross.roadCode}{baseCross.roadOrder}";
                }

                var crossName = $"{firstRoadcode}{firstRoadorder}{secondRoadcode}{secondRoadorder}";
                // var respon = await Startup.sendInmationToUrlAndGetRes(roomUrl, sendMsg);
                {
                    if (Directory.Exists($"imgT/{crossName}/"))
                    {
                        if (File.Exists($"imgT/{crossName}/px.jpg") &&
                            File.Exists($"imgT/{crossName}/nx.jpg") &&
                            File.Exists($"imgT/{crossName}/py.jpg") &&
                            File.Exists($"imgT/{crossName}/ny.jpg") &&
                            File.Exists($"imgT/{crossName}/pz.jpg") &&
                            File.Exists($"imgT/{crossName}/nz.jpg"))
                        {
                            SetBackgroundScene_BLL sbgs = new SetBackgroundScene_BLL()
                            {
                                c = "SetBackgroundScene",
                                firstRoadcode = firstRoadcode,
                                firstRoadorder = firstRoadorder,
                                secondRoadcode = secondRoadcode,
                                secondRoadorder = secondRoadorder,
                                author = address,
                                nx = ImageToBase64($"imgT/{crossName}/nx.jpg"),
                                ny = ImageToBase64($"imgT/{crossName}/ny.jpg"),
                                nz = ImageToBase64($"imgT/{crossName}/nz.jpg"),
                                px = ImageToBase64($"imgT/{crossName}/px.jpg"),
                                py = ImageToBase64($"imgT/{crossName}/py.jpg"),
                                pz = ImageToBase64($"imgT/{crossName}/pz.jpg"),
                            };
                            var index = rm.Next(0, roomUrls.Count);
                            var roomUrl = roomUrls[index];
                            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(sbgs);
                            await Startup.sendInmationToUrlAndGetRes(roomUrl, sendMsg);
                        }
                    }
                    else
                    {
                        //   Directory.CreateDirectory($"imgT/{crossName}/");
                    }
                }
                {

                    //GetBackgroundScene.Result r = Newtonsoft.Json.JsonConvert.DeserializeObject<GetBackgroundScene.Result>(respon);
                    //var sm = new
                    //{
                    //    c = "SetBackgroundScene",
                    //    r = r,
                    //};
                    //sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(sm);
                    //var sendData = Encoding.UTF8.GetBytes(sendMsg);
                    //await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                }
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
            internal void PreviousUser(string address)
            {
                if (this.addModel)
                {
                    var current = this.material[this.indexOfSelect];
                    for (int i = 1; i < this.material.Count; i++)
                    {
                        var newIndex = (this.indexOfSelect - i + this.material.Count) % this.material.Count;
                        if (this.material[newIndex].author == address)
                        {
                            this.indexOfSelect = newIndex;
                            break;
                        }
                    }
                }
            }
            internal void NextUser(string address)
            {
                if (this.addModel)
                {
                    var current = this.material[this.indexOfSelect];
                    for (int i = 1; i < this.material.Count; i++)
                    {
                        var newIndex = (this.indexOfSelect + i) % this.material.Count;
                        if (this.material[newIndex].author == address)
                        {
                            this.indexOfSelect = newIndex;
                            break;
                        }
                    }
                }
            }
            internal async Task ShowMsg(WebSocket webSocket, string msg)
            {
                var sm = new
                {
                    c = "ShowMsg",
                    Msg = msg
                };
                var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(sm);
                var sendData = Encoding.UTF8.GetBytes(sendMsg);
                await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                // throw new NotImplementedException();
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

            internal async Task DeleteModel(WebSocket webSocket, DeleteModel dm, Random rm, bool isAdministartor)
            {
                if (isAdministartor)
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
                else
                {
                    await this.ShowMsg(webSocket, "只有管理员才能删除");
                }
            }
            readonly char[] splits = new char[] { '\r', '\n' };
            internal async Task<string> CreateNew(CreateNewObj cno, string author, List<string> modelTypesInput, Random rm)
            {
                var modelTypes = new List<string>();
                for (int i = 0; i < modelTypesInput.Count; i += 2)
                {
                    modelTypes.Add(modelTypesInput[i]);
                }
                if (cno.objNew == null)
                {
                    return "上传失败";
                }
                else if (cno.objNew.Length == 5)
                {
                    var mID = "n" + Guid.NewGuid().ToString("N");
                    var cn = new CreateNew()
                    {
                        c = "CreateNew",
                        objText = cno.objNew[0],
                        mtlText = cno.objNew[1],
                        imageBase64 = cno.objNew[2],
                        modelType = cno.objNew[3],
                        amState = 0,
                        animation = "",
                        author = author,
                        modelID = mID,
                        modelName = cno.objNew[4],
                        rotatey = cno.rotationY,
                        x = cno.x,
                        y = cno.y,
                        z = cno.z,
                    };

                    if (string.IsNullOrEmpty(cn.objText))
                    {
                        return "objText为空";
                    }
                    else if (cn.objText.Split(splits, StringSplitOptions.RemoveEmptyEntries).Length < 2)
                    {
                        return "objText格式错误";
                    }
                    else if (string.IsNullOrEmpty(cn.mtlText))
                    {
                        return "mtlText为空";
                    }
                    else if (cn.mtlText.Split(splits, StringSplitOptions.RemoveEmptyEntries).Length < 2)
                    {
                        return "mtlText格式错误";
                    }
                    else if (string.IsNullOrEmpty(cn.imageBase64))
                    {
                        return "imageBase64为空";
                    }
                    else if (string.IsNullOrEmpty(cn.modelType))
                    {
                        return "modelType为空";
                    }
                    else if (!modelTypes.Contains(cn.modelType))
                    {
                        return "modelType为空";
                    }
                    else
                    {
                        var index = rm.Next(0, roomUrls.Count);
                        var roomUrl = roomUrls[index];
                        var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(cn);
                        return await Startup.sendInmationToUrlAndGetRes(roomUrl, sendMsg);

                    }
                }
                else
                {
                    return "格式错误";
                }
                //throw new NotImplementedException();
            }

            internal async Task GetModelDetail(WebSocket webSocket, GetModelDetail gmd, Random rm)
            {


                var index = rm.Next(0, roomUrls.Count);
                var roomUrl = roomUrls[index];
                var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(new CommonClass.MapEditor.GetModelDetail()
                {
                    c = "GetModelDetail",
                    modelID = gmd.name
                });
                var json = await Startup.sendInmationToUrlAndGetRes(roomUrl, sendMsg);

                if (string.IsNullOrEmpty(json)) { }
                else
                {
                    var sendData = Encoding.UTF8.GetBytes(json);
                    await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }

            internal async Task UseModelObj(WebSocket webSocket, UseModelObj umo, Random rm, bool isAdministartor)
            {
                if (isAdministartor)
                {
                    var index = rm.Next(0, roomUrls.Count);
                    var roomUrl = roomUrls[index];
                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(new CommonClass.MapEditor.UseModelObj()
                    {
                        c = umo.c,
                        modelID = umo.name,
                        Used = umo.used
                    });
                    await Startup.sendInmationToUrlAndGetRes(roomUrl, sendMsg);
                }
                else
                {
                    await this.ShowMsg(webSocket, "只有管理员才能进行此操作！");
                }

            }

            internal async Task ClearModelObj(WebSocket webSocket, Random rm, bool isAdministartor)
            {
                if (isAdministartor)
                {
                    var index = rm.Next(0, roomUrls.Count);
                    var roomUrl = roomUrls[index];
                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        c = "ClearModelObj"
                    });
                    await Startup.sendInmationToUrlAndGetRes(roomUrl, sendMsg);
                }
                else
                {
                    await this.ShowMsg(webSocket, "只有管理元才能进行此操作！");
                }

            }

            internal async Task DownloadModel(WebSocket webSocket, GetModelDetail gmd, Random rm)
            {
                var index = rm.Next(0, roomUrls.Count);
                var roomUrl = roomUrls[index];
                var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(new CommonClass.MapEditor.GetModelDetail()
                {
                    c = "GetModelDetail",
                    modelID = gmd.name
                });
                var json = await Startup.sendInmationToUrlAndGetRes(roomUrl, sendMsg);

                if (string.IsNullOrEmpty(json)) { }
                else
                {
                    var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.ModelDetail>(json);
                    obj.c = "DownloadModel";
                    json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                    var sendData = Encoding.UTF8.GetBytes(json);
                    await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }

            int startIndex = 0;
            internal async Task GetUnLockedModelID(WebSocket webSocket, Random rm, string direction)
            {
                var index = rm.Next(0, roomUrls.Count);
                var roomUrl = roomUrls[index];
                var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(new CommonClass.MapEditor.GetUnLockedModel()
                {
                    c = "GetUnLockedModel",
                    startIndex = startIndex,
                    direction = direction
                });
                var json = await Startup.sendInmationToUrlAndGetRes(roomUrl, sendMsg);
                //Consol.WriteLine(json);
                if (!string.IsNullOrEmpty(json))
                {
                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.GetUnLockedModelResult>(json);
                    if (result.hasValue)
                    {
                        this.startIndex = result.newStartIndex;
                        CommonClass.ViewSearch sn = new CommonClass.ViewSearch()
                        {
                            c = "ViewSearch",
                            WebSocketID = 0,
                            mctX = Convert.ToInt32(result.x * 256),
                            mctY = Convert.ToInt32(result.z * -256),
                        };
                        sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(sn);
                        var sendData = Encoding.ASCII.GetBytes(sendMsg);
                        await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);

                        ShowOBJFile sf = new ShowOBJFile()
                        {
                            x = result.x,
                            z = result.z,
                            c = "ShowOBJFile"
                        };
                        await this.ShowObj(sf, webSocket, rm);
                    }
                }
            }
        }
    }
}
