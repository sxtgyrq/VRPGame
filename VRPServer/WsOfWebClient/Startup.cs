using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace WsOfWebClient
{
    internal class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors(options =>
            {
                //options.AddPolicy(name: "MyPolicy",
                //    builder =>
                //    {
                //        builder.WithOrigins("http://*");
                //    });
                //options.AddPolicy("AllowSpecificOrigins",
                //builder =>
                //{
                //    builder.WithOrigins("http://www.nyrq123.com", "http://localhost:1978", "https://www.nyrq123.com", "*");
                //});
                options.AddPolicy("AllowAny", p => p.AllowAnyOrigin()
                                                                          .AllowAnyMethod()
                                                                          .AllowAnyHeader());
            });

            //↓↓↓此处代码是设置App console等级↓↓↓
            services.AddLogging(builder =>
            {
                builder.AddFilter("Microsoft", LogLevel.Error)
                .AddFilter("System", LogLevel.Error)
                .AddFilter("NToastNotify", LogLevel.Error)
                .AddConsole();
            });
        }

        const int webWsSize = 1024 * 3;
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IWebHostEnvironment env)
        {



            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //app.log

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
                ReceiveBufferSize = webWsSize,
            };
            app.UseWebSockets(webSocketOptions);

            app.Map("/websocket", WebSocketF);
            // app.UseCors("AllowAny");
            app.Map("/bgimg", BackGroundImg);
            //app.Map("/websocket", WebSocketF);
            // app.Map("/notify", notify);

            //Consol.WriteLine($"启动TCP连接！{ ConnectInfo.tcpServerPort}");
            Thread th = new Thread(() => startTcp());
            th.Start();
        }
        private void startTcp()
        {
            var dealWithF = new TcpFunction.WithResponse.DealWith(StartTcpDealWithF);
            TcpFunction.WithResponse.ListenIpAndPort(ConnectInfo.HostIP, ConnectInfo.tcpServerPort, dealWithF);
        }
        //private async void startTcp()
        string StartTcpDealWithF(string notifyJson, int tcpPort)
        {
            try
            {
                CommonClass.CommandNotify c = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.CommandNotify>(notifyJson);
                int timeOut = 0;
                switch (c.c)
                {
                    case "WhetherOnLine":
                        {
                            WebSocket ws = null;
                            lock (ConnectInfo.connectedWs_LockObj)
                            {
                                if (ConnectInfo.connectedWs.ContainsKey(c.WebSocketID))
                                {
                                    if (!ConnectInfo.connectedWs[c.WebSocketID].ws.CloseStatus.HasValue)
                                    {
                                        ws = ConnectInfo.connectedWs[c.WebSocketID].ws;
                                    }
                                    else
                                    {
                                        ConnectInfo.connectedWs.Remove(c.WebSocketID);
                                        return "off";
                                    }
                                }
                            }
                            // await context.Response.WriteAsync("ok");
                            if (ws != null)
                            {
                                if (ws.State == WebSocketState.Open)
                                {
                                    return "on";
                                }
                                else
                                {
                                    return "off";
                                }
                            }
                            else
                            {
                                return "off";
                            }
                        };
                    default:
                        {
                            WebSocket ws = null;
                            lock (ConnectInfo.connectedWs_LockObj)
                            {
                                if (ConnectInfo.connectedWs.ContainsKey(c.WebSocketID))
                                {
                                    if (!ConnectInfo.connectedWs[c.WebSocketID].ws.CloseStatus.HasValue)
                                    {
                                        ws = ConnectInfo.connectedWs[c.WebSocketID].ws;
                                    }
                                    else
                                    {
                                        ConnectInfo.connectedWs.Remove(c.WebSocketID);
                                    }
                                }
                            }
                            // await context.Response.WriteAsync("ok");
                            if (ws != null)
                            {

                                if (ws.State == WebSocketState.Open)
                                {
                                    try
                                    {

                                        //ws.
                                        //  var sendData = Encoding.UTF8.GetBytes(notifyJson);

                                        switch (c.c)
                                        {
                                            case "": { }; break;
                                            default:
                                                {
                                                    CommonF.SendData(notifyJson, ws, timeOut);
                                                    //await ws.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                                                }; break;
                                        }

                                    }
                                    catch
                                    {
                                        //Consol.WriteLine("websocket 异常");
                                    }
                                }
                            }
                            return "";
                        };
                };
            }
            catch (Exception e)
            {

                throw e;
            }
            // return "";
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
        static int indexOfCall = 0;

        private static void BackGroundImg(IApplicationBuilder app)
        {
            app.UseCors("AllowAny");
            app.Run(async context =>
            {
                try
                {
                    // throw new NotImplementedException();
                    var pathValue = context.Request.Path.Value;
                    //Consoe.Write($" {context.Request.Path.Value}");
                    var regex = new System.Text.RegularExpressions.Regex("^/[a-zA-Z0-9]{32}/[np]{1}[xyz]{1}.jpg$");
                    if (regex.IsMatch(pathValue))
                    {
                        var filePath = $"{Room.ImgPath}{pathValue}";
                        if (File.Exists(filePath))
                        {
                            await getImage(filePath, context.Response);
                        }
                        {
                            indexOfCall++;
                            indexOfCall = indexOfCall % Room.roomUrls.Count;
                            var fileName = pathValue.Split('/').Last();
                            var dataGetFromDB = Room.getImg(indexOfCall, pathValue.Split('/')[1], fileName);
                            if (dataGetFromDB.Length > 0)
                            {
                                await getImage(dataGetFromDB, context.Response);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    //throw e;
                } 
            });
        }

        private static async Task getImage(string path, HttpResponse Response)
        {
            Response.ContentType = "image/jpeg";
            {
                var bytes = await File.ReadAllBytesAsync(path);
                await Response.Body.WriteAsync(bytes, 0, bytes.Length);
            }
        }
        private static async Task getImage(byte[] bytes, HttpResponse Response)
        {
            Response.ContentType = "image/jpeg";
            {
                await Response.Body.WriteAsync(bytes, 0, bytes.Length);
            }
        }
        //BackGroundImg

        private static async Task Echo(System.Net.WebSockets.WebSocket webSocket)
        {
            WebSocketReceiveResult wResult;
            {
                //byte[] buffer = new byte[size];
                //var buffer = new ArraySegment<byte>(new byte[8192]);
                IntroState iState = new IntroState()
                {
                    randomCharacterCount = 4,
                    randomValue = ""
                };
                State s = new State();
                ConnectInfo.webSocketID++;
                s.WebsocketID = ConnectInfo.webSocketID;
                s.Ls = LoginState.empty;
                s.roomIndex = -1;
                s.mapRoadAndCrossMd5 = "";
                removeWsIsNotOnline();
                addWs(webSocket, s.WebsocketID);

                var carsNames = new string[] { "车1", "车2", "车3", "车4", "车5" };
                var playerName = "玩家" + Math.Abs(DateTime.Now.GetHashCode() % 10000);


                //if(s.Ls== LoginState.)

                do
                {
                    try
                    {

                        var returnResult = ReceiveStringAsync(webSocket, webWsSize);

                        wResult = returnResult.wr;
                        // Consoe.WriteLine(returnResult.result);
                        CommonClass.Command c = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.Command>(returnResult.result);
                        switch (c.c)
                        {
                            case "MapRoadAndCrossMd5":
                                {
                                    if (s.Ls == LoginState.empty)
                                    {
                                        MapRoadAndCrossMd5 mapRoadAndCrossMd5 = Newtonsoft.Json.JsonConvert.DeserializeObject<MapRoadAndCrossMd5>(returnResult.result);
                                        s.mapRoadAndCrossMd5 = mapRoadAndCrossMd5.mapRoadAndCrossMd5;
                                    }
                                }; break;
                            case "CheckSession":
                                {
                                    if (s.Ls == LoginState.empty)
                                    {
                                        CheckSession checkSession = Newtonsoft.Json.JsonConvert.DeserializeObject<CheckSession>(returnResult.result);
                                        var checkResult = BLL.CheckSessionBLL.checkIsOK(checkSession, s);
                                        if (checkResult.CheckOK)
                                        {
                                            s.Key = checkResult.Key;
                                            s.roomIndex = checkResult.roomIndex;
                                            s = Room.setOnLine(s, webSocket);
                                        }
                                        else
                                        {
                                            s = Room.setState(s, webSocket, LoginState.selectSingleTeamJoin);
                                        }
                                    }
                                }; break;
                            case "JoinGameSingle":
                                {
                                    JoinGameSingle joinType = Newtonsoft.Json.JsonConvert.DeserializeObject<JoinGameSingle>(returnResult.result);
                                    if (s.Ls == LoginState.selectSingleTeamJoin)
                                    {
                                        s = Room.GetRoomThenStart(s, webSocket, playerName, joinType.RefererAddr);

                                    }

                                }; break;
                            case "QueryReward":
                                {
                                    if (s.Ls == LoginState.selectSingleTeamJoin)
                                    {
                                        s = Room.setState(s, webSocket, LoginState.QueryReward);
                                    }
                                }; break;
                            case "RewardBuildingShow":
                                {
                                    if (s.Ls == LoginState.QueryReward)
                                    {
                                        CommonClass.ModelTranstraction.RewardBuildingShow rbs = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.RewardBuildingShow>(returnResult.result);
                                        var dataCount = Room.RewardBuildingShowF(s, webSocket, rbs);
                                    }
                                }; break;
                            case "QueryRewardCancle":
                                {
                                    if (s.Ls == LoginState.QueryReward)
                                    {
                                        s = Room.setState(s, webSocket, LoginState.selectSingleTeamJoin);
                                    }
                                }; break;
                            case "CreateTeam":
                                {
                                    CreateTeam ct = Newtonsoft.Json.JsonConvert.DeserializeObject<CreateTeam>(returnResult.result);
                                    if (s.Ls == LoginState.selectSingleTeamJoin)
                                    {
                                        {
                                            string command_start;
                                            CommonClass.TeamResult team;
                                            {
                                                s = Room.setState(s, webSocket, LoginState.WaitingToStart);
                                            }
                                            {
                                                //
                                                command_start = CommonClass.Random.GetMD5HashFromStr(s.WebsocketID.ToString() + s.WebsocketID);
                                                team = Team.createTeam2(s.WebsocketID, playerName, command_start);
                                            }
                                            {
                                                //var command_start = CommonClass.Random.GetMD5HashFromStr(s.WebsocketID.ToString() + s.WebsocketID); 
                                                returnResult = ReceiveStringAsync(webSocket, webWsSize);

                                                wResult = returnResult.wr;
                                                if (returnResult.result == command_start)
                                                {
                                                    s = Room.GetRoomThenStartAfterCreateTeam(s, webSocket, team, playerName, ct.RefererAddr);
                                                }
                                                else if (returnResult.result == command_start + "exit")
                                                {
                                                    s = Room.CancelAfterCreateTeam(s, webSocket, team, playerName, ct.RefererAddr);
                                                } 
                                                else
                                                {
                                                    return;
                                                }
                                            }
                                        }
                                    }
                                }; break;
                            case "JoinTeam":
                                {
                                    JoinTeam ct = Newtonsoft.Json.JsonConvert.DeserializeObject<JoinTeam>(returnResult.result);
                                    if (s.Ls == LoginState.selectSingleTeamJoin)
                                    {
                                        {
                                            string command_start;
                                            {
                                                //将状态设置为等待开始和等待加入
                                                s = Room.setState(s, webSocket, LoginState.WaitingToGetTeam);
                                            }
                                            {
                                                returnResult = ReceiveStringAsync(webSocket, webWsSize);

                                                wResult = returnResult.wr;
                                                var teamID = returnResult.result;
                                                command_start = CommonClass.Random.GetMD5HashFromStr(s.WebsocketID.ToString() + s.WebsocketID + DateTime.Now.ToString());
                                                var result = Team.findTeam2(s.WebsocketID, playerName, command_start, teamID);

                                                if (result == "ok")
                                                {
                                                    s.CommandStart = command_start;
                                                    s.teamID = teamID;
                                                }
                                                else if (result == "game has begun")
                                                {
                                                    s = Room.setState(s, webSocket, LoginState.selectSingleTeamJoin);
                                                    Room.Alert(webSocket, $"他们已经开始了！");
                                                }
                                                else if (result == "is not number")
                                                {
                                                    s = Room.setState(s, webSocket, LoginState.selectSingleTeamJoin);
                                                    Room.Alert(webSocket, $"请输入数字");
                                                }
                                                else if (result == "not has the team")
                                                {
                                                    s = Room.setState(s, webSocket, LoginState.selectSingleTeamJoin);
                                                    Room.Alert(webSocket, $"没有该队伍({teamID})");
                                                }
                                                else if (result == "team is full")
                                                {
                                                    s = Room.setState(s, webSocket, LoginState.selectSingleTeamJoin);
                                                    Room.Alert(webSocket, "该队伍已满员");
                                                }
                                                else if (result == "need to back") 
                                                {
                                                    s = Room.setState(s, webSocket, LoginState.selectSingleTeamJoin);
                                                }
                                                else
                                                {
                                                    s = Room.setState(s, webSocket, LoginState.selectSingleTeamJoin);
                                                }
                                            }
                                        }
                                    }
                                }; break;
                            case "TeamNumWithSecret":
                                {
                                    if (s.Ls == LoginState.WaitingToGetTeam)
                                    {
                                        var command_start = s.CommandStart;
                                        int roomIndex;
                                        string refererAddr;
                                        if (Room.CheckSecret(returnResult.result, command_start, out roomIndex, out refererAddr))
                                        {
                                            //   Consoe.WriteLine("secret 正确");
                                            s = Room.GetRoomThenStartAfterJoinTeam(s, webSocket, roomIndex, playerName, refererAddr);
                                            //exitTeam
                                        }
                                        else if (Room.CheckSecretIsExit(returnResult.result, command_start, out refererAddr))
                                        {
                                            s = Room.setState(s, webSocket, LoginState.selectSingleTeamJoin);
                                            // s = Room.setOnLine(s, webSocket);
                                        }
                                        else
                                        {
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        //Consoe.WriteLine("错误的状态");
                                        return;
                                    }
                                }; break;
                            case "LeaveTeam":
                                {
                                    if (s.Ls == LoginState.WaitingToGetTeam)
                                    {
#warning 这里因该先从房价判断，房主有没有点开始！

                                        var r = Team.leaveTeam(s.teamID, s.WebsocketID);
                                        if (r)
                                            s = Room.setState(s, webSocket, LoginState.selectSingleTeamJoin);
                                    }
                                }; break;
                            case "SetCarsName":
                                {
                                    if (s.Ls == LoginState.selectSingleTeamJoin)
                                    {
                                        SetCarsName setCarsName = Newtonsoft.Json.JsonConvert.DeserializeObject<SetCarsName>(returnResult.result);
                                        for (var i = 0; i < 5; i++)
                                        {
                                            if (!string.IsNullOrEmpty(setCarsName.Names[i]))
                                            {
                                                if (setCarsName.Names[i].Trim().Length >= 2 && setCarsName.Names[i].Trim().Length < 7)
                                                {
                                                    carsNames[i] = setCarsName.Names[i].Trim();
                                                }
                                            }
                                        }
                                    }
                                }; break;
                            case "LookForBuildings":
                                {
                                    LookForBuildings joinType = Newtonsoft.Json.JsonConvert.DeserializeObject<LookForBuildings>(returnResult.result);
                                    if (s.Ls == LoginState.OnLine)
                                    {

                                        s = Room.setState(s, webSocket, LoginState.LookForBuildings);
                                        s = Room.receiveState2(s, joinType, webSocket);
                                        // s = await Room.GetAllModelPosition(s, webSocket);
                                        // s = await Room.receiveState(s, webSocket);
                                    }
                                }; break;
                            case "GetRewardFromBuildings":
                                {

                                    /*
                                     * 求福
                                     */
                                    GetRewardFromBuildings grfb = Newtonsoft.Json.JsonConvert.DeserializeObject<GetRewardFromBuildings>(returnResult.result);
                                    if (s.Ls == LoginState.OnLine)
                                    {
                                        s = await Room.GetRewardFromBuildingF(s, grfb, webSocket);
                                    }

                                }; break;
                            case "CancleLookForBuildings":
                                {
                                    CancleLookForBuildings cancle = Newtonsoft.Json.JsonConvert.DeserializeObject<CancleLookForBuildings>(returnResult.result);

                                    if (s.Ls == LoginState.LookForBuildings)
                                    {
                                        s = Room.setState(s, webSocket, LoginState.OnLine);
                                    }
                                }; break;
                            case "GetCarsName":
                                {
                                    if (s.Ls == LoginState.selectSingleTeamJoin)
                                    {
                                        var msg = Newtonsoft.Json.JsonConvert.SerializeObject(new { c = "GetCarsName", names = carsNames });
                                        CommonF.SendData(msg, webSocket, 0);
                                        //var sendData = Encoding.UTF8.GetBytes(msg);
                                        //await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                                    }
                                }; break;
                            case "SetPlayerName":
                                {
                                    if (s.Ls == LoginState.selectSingleTeamJoin)
                                    {
                                        var regex = new Regex("^[\u4e00-\u9fa5]{1}[a-zA-Z0-9\u4e00-\u9fa5]{1,8}$");

                                        SetPlayerName setPlayerName = Newtonsoft.Json.JsonConvert.DeserializeObject<SetPlayerName>(returnResult.result);
                                        if (regex.IsMatch(setPlayerName.Name))
                                            playerName = setPlayerName.Name;
                                    }
                                }; break;
                            case "GetName":
                                {
                                    if (s.Ls == LoginState.selectSingleTeamJoin)

                                    {
                                        var msg = Newtonsoft.Json.JsonConvert.SerializeObject(new { c = "GetName", name = playerName });
                                        var sendData = Encoding.UTF8.GetBytes(msg);
                                        await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                                    }
                                }; break;
                            case "Promote":
                                {
                                    if (s.Ls == LoginState.OnLine)
                                    {
                                        Promote promote = Newtonsoft.Json.JsonConvert.DeserializeObject<Promote>(returnResult.result);
                                        Room.setPromote(s, promote);
                                    }
                                }; break;
                            case "Collect":
                                {
                                    if (s.Ls == LoginState.OnLine)
                                    {
                                        Collect collect = Newtonsoft.Json.JsonConvert.DeserializeObject<Collect>(returnResult.result);
                                        Room.setCollect(s, collect);
                                    }
                                }; break;
                            case "Attack":
                                {
                                    if (s.Ls == LoginState.OnLine)
                                    {
                                        Attack attack = Newtonsoft.Json.JsonConvert.DeserializeObject<Attack>(returnResult.result);
                                        Room.setAttack(s, attack);
                                    }
                                }; break;
                            case "Tax":
                                {
                                    if (s.Ls == LoginState.OnLine)
                                    {
                                        Tax tax = Newtonsoft.Json.JsonConvert.DeserializeObject<Tax>(returnResult.result);
                                        Room.setToCollectTax(s, tax);
                                    }
                                }; break;
                            case "Msg":
                                {
                                    if (s.Ls == LoginState.OnLine)
                                    {
                                        Msg msg = Newtonsoft.Json.JsonConvert.DeserializeObject<Msg>(returnResult.result);
                                        if (msg.MsgPass.Length < 120)
                                        {
                                            Room.passMsg(s, msg);
                                        }
                                    }
                                }; break;
                            case "Ability":
                                {
                                    if (s.Ls == LoginState.OnLine)
                                    {
                                        Ability a = Newtonsoft.Json.JsonConvert.DeserializeObject<Ability>(returnResult.result);
                                        Room.setCarAbility(s, a);
                                    }
                                }; break;
                            case "SetCarReturn":
                                {
                                    if (s.Ls == LoginState.OnLine)
                                    {
                                        SetCarReturn scr = Newtonsoft.Json.JsonConvert.DeserializeObject<SetCarReturn>(returnResult.result);
                                        Room.setCarReturn(s, scr);
                                    }
                                }; break;
                            case "Donate":
                                {
                                    if (s.Ls == LoginState.OnLine)
                                    {
                                        Donate donate = Newtonsoft.Json.JsonConvert.DeserializeObject<Donate>(returnResult.result);
                                        Room.Donate(s, donate);
                                    }
                                }; break;
                            case "GetSubsidize":
                                {
                                    if (s.Ls == LoginState.OnLine)
                                    {
                                        GetSubsidize getSubsidize = Newtonsoft.Json.JsonConvert.DeserializeObject<GetSubsidize>(returnResult.result);
                                        await Room.GetSubsidize(s, getSubsidize);
                                    }
                                }; break;
                            case "OrderToSubsidize":
                                {
                                    if (s.Ls == LoginState.OnLine)
                                    {
                                        GetSubsidize getSubsidize = Newtonsoft.Json.JsonConvert.DeserializeObject<GetSubsidize>(returnResult.result);
                                        await Room.GetSubsidize(s, getSubsidize);
                                    }
                                }; break;
                            case "Bust":
                                {
                                    if (s.Ls == LoginState.OnLine)
                                    {
                                        Bust bust = Newtonsoft.Json.JsonConvert.DeserializeObject<Bust>(returnResult.result);
                                        await Room.setBust(s, bust);
                                    }
                                }; break;
                            case "BuyDiamond":
                                {
                                    if (s.Ls == LoginState.OnLine)
                                    {
                                        BuyDiamond bd = Newtonsoft.Json.JsonConvert.DeserializeObject<BuyDiamond>(returnResult.result);
                                        await Room.buyDiamond(s, bd);
                                    }
                                }; break;
                            case "SellDiamond":
                                {
                                    if (s.Ls == LoginState.OnLine)
                                    {
                                        BuyDiamond bd = Newtonsoft.Json.JsonConvert.DeserializeObject<BuyDiamond>(returnResult.result);
                                        await Room.sellDiamond(s, bd);
                                    }
                                }; break;
                            case "DriverSelect":
                                {
                                    if (s.Ls == LoginState.OnLine)
                                    {
                                        DriverSelect ds = Newtonsoft.Json.JsonConvert.DeserializeObject<DriverSelect>(returnResult.result);
                                        await Room.selectDriver(s, ds);
                                    }
                                }; break;
                            case "Skill1":
                                {
                                    if (s.Ls == LoginState.OnLine)
                                    {
                                        Skill1 s1 = Newtonsoft.Json.JsonConvert.DeserializeObject<Skill1>(returnResult.result);
                                        await Room.magic(s, s1);
                                    }
                                }; break;
                            case "Skill2":
                                {
                                    if (s.Ls == LoginState.OnLine)
                                    {
                                        Skill2 s2 = Newtonsoft.Json.JsonConvert.DeserializeObject<Skill2>(returnResult.result);
                                        Room.magic(s, s2);
                                    }
                                }; break;
                            case "ViewAngle":
                                {
                                    if (s.Ls == LoginState.OnLine)
                                    {
                                        ViewAngle va = Newtonsoft.Json.JsonConvert.DeserializeObject<ViewAngle>(returnResult.result);
                                        await Room.view(s, va);
                                    }
                                }; break;
                            case "GetBuildings":
                                {

                                }; break;
                            case "GenerateAgreement":
                                {
                                    GenerateAgreement ga = Newtonsoft.Json.JsonConvert.DeserializeObject<GenerateAgreement>(returnResult.result);
                                    await Room.GenerateAgreementF(s, webSocket, ga);
                                }; break;
                            case "ModelTransSign":
                                {
                                    // if (s.Ls == LoginState.OnLine)
                                    {
                                        ModelTransSign mts = Newtonsoft.Json.JsonConvert.DeserializeObject<ModelTransSign>(returnResult.result);
                                        Room.ModelTransSignF(s, webSocket, mts);
                                    }
                                }; break;
                            case "RewardPublicSign":
                                {
                                    RewardPublicSign rps = Newtonsoft.Json.JsonConvert.DeserializeObject<RewardPublicSign>(returnResult.result);
                                    Room.PublicReward(s, webSocket, rps);
                                }; break;
                            case "CheckCarState":
                                {
                                    CommonClass.CheckCarState ccs = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.CheckCarState>(returnResult.result);
                                    Room.checkCarState(s, ccs);
                                }; break;
                            //getResistance
                            case "GetResistance":
                                {
                                    GetResistance gr = Newtonsoft.Json.JsonConvert.DeserializeObject<GetResistance>(returnResult.result);
                                    var r = Room.GetResistanceF(s, gr);
                                    if (!string.IsNullOrEmpty(r))
                                    {
                                        Room.GetMaterial(r, webSocket);
                                    }
                                }; break;
                            case "TakeApart":
                                {
                                    await Room.TakeApart(s);
                                }; break;
                            case "UpdateLevel":
                                {
                                    if (s.Ls == LoginState.OnLine)
                                    {
                                        UpdateLevel uL = Newtonsoft.Json.JsonConvert.DeserializeObject<UpdateLevel>(returnResult.result);
                                        Room.UpdateLevelF(s, uL);
                                    }
                                }; break;
                            case "AllBusinessAddr"://AllBusinessAddr
                                {
                                    RewardSet rs = Newtonsoft.Json.JsonConvert.DeserializeObject<RewardSet>(returnResult.result);
                                    var r = await Room.GetAllBusinessAddr(webSocket, rs);
                                    //  Consoe.WriteLine(r);
                                }; break;
                            case "AllStockAddr":
                                {
                                    AllStockAddr asa = Newtonsoft.Json.JsonConvert.DeserializeObject<AllStockAddr>(returnResult.result);
                                    await Room.GetAllStockAddr(webSocket, asa);
                                }; break;
                            case "GenerateRewardAgreement":
                                {
                                    GenerateRewardAgreement ga = Newtonsoft.Json.JsonConvert.DeserializeObject<GenerateRewardAgreement>(returnResult.result);
                                    await Room.GenerateRewardAgreementF(webSocket, ga);
                                }; break;
                            case "RewardInfomation":
                                {
                                    RewardInfomation gra = Newtonsoft.Json.JsonConvert.DeserializeObject<RewardInfomation>(returnResult.result);
                                    Room.GetRewardInfomation(webSocket, gra);
                                }; break;
                            case "RewardApply":
                                {
                                    RewardApply rA = Newtonsoft.Json.JsonConvert.DeserializeObject<RewardApply>(returnResult.result);
                                    Room.RewardApply(webSocket, rA);
                                }; break;
                            case "AwardsGiving":
                                {
                                    CommonClass.ModelTranstraction.AwardsGiving ag = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.AwardsGiving>(returnResult.result);
                                    Room.GiveAward(webSocket, ag);
                                }; break;
                            case "Guid":
                                {
                                    if (s.Ls == LoginState.selectSingleTeamJoin)
                                    {
                                        iState.randomValue = Room.GetRandom(iState.randomCharacterCount);
                                        Room.setRandomPic(iState, webSocket);
                                        s = Room.setState(s, webSocket, LoginState.Guid);

                                    }
                                }; break;
                            case "QueryGuid":
                                {
                                    if (s.Ls == LoginState.Guid)
                                    {
                                        s = Room.setState(s, webSocket, LoginState.selectSingleTeamJoin);
                                    }
                                }; break;
                            case "BindWordInfo":
                                {
                                    if (s.Ls == LoginState.Guid)
                                    {
                                        CommonClass.ModelTranstraction.BindWordInfo bwi = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.BindWordInfo>(returnResult.result);
                                        Room.BindWordInfoF(iState, webSocket, bwi);
                                    }
                                }; break;
                            case "LookForBindInfo":
                                {
                                    if (s.Ls == LoginState.Guid)
                                    {
                                        CommonClass.ModelTranstraction.LookForBindInfo lbi = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.LookForBindInfo>(returnResult.result);
                                        Room.LookForBindInfoF(iState, webSocket, lbi);
                                    }
                                }; break;
                            case "GetFightSituation":
                                {
                                    if (s.Ls == LoginState.OnLine)
                                    {
                                        s = Room.GetFightSituation(s, webSocket);
                                    }
                                }; break;
                            case "GetTaskCopy":
                                {
                                    if (s.Ls == LoginState.OnLine)
                                    {
                                        s = Room.GetTaskCopy(s, webSocket);
                                    }
                                }; break;
                            case "RemoveTaskCopy":
                                {
                                    if (s.Ls == LoginState.OnLine)
                                    {
                                        WsOfWebClient.RemoveTaskCopy rtc = Newtonsoft.Json.JsonConvert.DeserializeObject<WsOfWebClient.RemoveTaskCopy>(returnResult.result);
                                        //var r = 
                                        Room.RemoveTaskCopy(s, rtc);
                                        //if (rtc.c == r + "aa")
                                        //{
                                        //    Console.WriteLine("");
                                        //}
                                    }
                                }; break;
                            case "Exit":
                                {
                                    if (s.Ls == LoginState.OnLine)
                                    {
                                        var success = Room.ExitF(ref s, webSocket);
                                        if (success)
                                        {

                                            var ws = ConnectInfo.connectedWs[s.WebsocketID];
                                            ConnectInfo.connectedWs.Remove(s.WebsocketID);
                                            ConnectInfo.webSocketID++;

                                            s.WebsocketID = ConnectInfo.webSocketID;
                                            addWs(ws.ws, s.WebsocketID);
                                        }
                                    }
                                }; break;
                            case "GetOnLineState":
                                {
                                    if (s.Ls == LoginState.OnLine)
                                    {
                                        Room.GetOnLineState(s);
                                    }
                                }; break;
                        }
                    }
                    catch (Exception e)
                    {

                        await Room.setOffLine(s);
                        removeWs(s.WebsocketID);
                        throw e;
                    }
                }
                while (!wResult.CloseStatus.HasValue);
                await Room.setOffLine(s);
                removeWs(s.WebsocketID);
                //try
                //{
                //    // await webSocket.CloseAsync(wResult.CloseStatus.Value, wResult.CloseStatusDescription, CancellationToken.None);
                //    // ConnectInfo.connectedWs.Remove(c.WebSocketID);

                //}
                //catch (Exception e)
                //{
                //    throw e;
                //    // ConnectInfo.connectedWs.Remove(c.WebSocketID);
                //    //  return;
                //}
            };
        }

        //private static List<WebsocketClient> _clients = new List<WebsocketClient>();
        //static Dictionary<string, ClientWebSocket> _sockets = new Dictionary<string, ClientWebSocket>();
        public static string sendInmationToUrlAndGetRes(string roomUrl, string sendMsg)
        {
            var t1 = TcpFunction.WithResponse.SendInmationToUrlAndGetRes(roomUrl, sendMsg);
            return t1.GetAwaiter().GetResult();
        }

        private static void removeWs(int websocketID)
        {
            try
            {
                lock (ConnectInfo.connectedWs_LockObj)
                {
                    if (ConnectInfo.connectedWs.ContainsKey(websocketID))
                    {
                        ConnectInfo.connectedWs[websocketID].ws.Dispose();
                        ConnectInfo.connectedWs.Remove(websocketID);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static void addWs(System.Net.WebSockets.WebSocket webSocket, int websocketID)
        {
            lock (ConnectInfo.connectedWs_LockObj)
            {
                ConnectInfo.connectedWs.Add(websocketID, new ConnectInfo.ConnectInfoDetail(webSocket));
            }
        }

        private static void removeWsIsNotOnline()
        {
            lock (ConnectInfo.connectedWs_LockObj)
            {
                List<int> keys = new List<int>();

                foreach (var item in ConnectInfo.connectedWs)
                {
                    if (item.Value.ws.CloseStatus.HasValue)
                    {
                        keys.Add(item.Key);
                    }
                }
                for (int i = 0; i < keys.Count; i++)
                {
                    ConnectInfo.connectedWs.Remove(keys[i]);
                }
            }
        }

        public class ReceiveObj
        {
            public WebSocketReceiveResult wr { get; set; }
            public string result { get; set; }
        }


        public static ReceiveObj ReceiveStringAsync(System.Net.WebSockets.WebSocket socket, CancellationToken ct = default(CancellationToken))
        {
            return ReceiveStringAsync(socket, webWsSize, ct);
        }
        public static ReceiveObj ReceiveStringAsync(System.Net.WebSockets.WebSocket socket, int size, CancellationToken ct = default(CancellationToken))
        {
            var buffer = new ArraySegment<byte>(new byte[size]);
            WebSocketReceiveResult result;
            using (var ms = new MemoryStream())
            {
                do
                {
                    // ct.IsCancellationRequested
                    ct.ThrowIfCancellationRequested();

                    var t1 = socket.ReceiveAsync(buffer, ct);
                    result = t1.GetAwaiter().GetResult();

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
                    var t2 = reader.ReadToEndAsync();

                    var strValue = t2.GetAwaiter().GetResult();//await reader.ReadToEndAsync();
                    return new ReceiveObj()
                    {
                        result = strValue,
                        wr = result
                    };
                }
            }
        }


        const double roadZoomValue = 0.0000003;

        public static LoginState WaitingToStart { get; private set; }

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

        //private static string getStrFromByte(ref byte[] buffer)
        //{
        //    string str = "";

        //    for (var i = buffer.Length - 1; i >= 0; i--)
        //    {
        //        if (buffer[i] != 0)
        //        {
        //            str = Encoding.UTF8.GetString(buffer.Take(i + 1).ToArray()).Trim();
        //            break;
        //        }

        //    }
        //    buffer = new byte[size];
        //    return str;

        //}



        public static string getBodyStr(HttpContext context)
        {
            string requestContent;
            using (var requestReader = new StreamReader(context.Request.Body, Encoding.UTF8))
            {
                requestContent = requestReader.ReadToEnd();

            }
            return requestContent;

        }
    }
}
