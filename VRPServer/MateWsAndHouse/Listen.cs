using CommonClass;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MateWsAndHouse
{
    public class Listen
    {
        static int NumOfIndex = 0;
        internal static void IpAndPort(string hostIP, int tcpPort)
        {
            var dealWith = new TcpFunction.WithResponse.DealWith(DealWith);
            TcpFunction.WithResponse.ListenIpAndPort(hostIP, tcpPort, dealWith);
        }
        class TeamDisplayItem
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string GUID { get; set; }
        }
        private static string DealWith(string notifyJson, int tcpPort)
        {
            //Consol.WriteLine($"notify receive:{notifyJson}");
            //File.AppendAllText("log/d.txt", $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}-{notifyJson}{Environment.NewLine}");
            //File.AppendText("",)
            // CommonClass.TeamCreateFinish teamCreateFinish = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.TeamCreateFinish>(notifyJson);
            string outPut = "haveNothingToReturn";
            {
                CommonClass.Command c = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.Command>(notifyJson);

                switch (c.c)
                {
                    case "TeamCreate":
                        {
                            CommonClass.TeamCreate teamCreate = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.TeamCreate>(notifyJson);
                            int indexV;
                            lock (Program.teamLock)
                            {
                                int maxValue = 10;
                                //int indexV;
                                do
                                {
                                    indexV = Program.rm.Next(1, maxValue);
                                    maxValue *= 2;
                                } while (Program.allTeams.ContainsKey(indexV));

                                Program.allTeams.Add(indexV, new Team()
                                {
                                    captain = teamCreate,
                                    CreateTime = DateTime.Now,
                                    TeamID = indexV,
                                    member = new List<CommonClass.TeamJoin>(),
                                    IsBegun = false
                                });

                                List<int> keysNeedToRemove = new List<int>();
                                foreach (var item in Program.allTeams)
                                {
                                    if (item.Value.IsBegun)
                                    {
                                        keysNeedToRemove.Add(item.Key);
                                    }
                                    else if (item.Value.CreateTime.AddHours(2) < DateTime.Now)
                                    {
                                        keysNeedToRemove.Add(item.Key);
                                    }
                                }
                                for (int i = 0; i < keysNeedToRemove.Count; i++)
                                {
                                    Program.allTeams.Remove(keysNeedToRemove[i]);
                                } 
                            }
                            var teamCreateFinish = new CommonClass.TeamCreateFinish()
                            {
                                c = "TeamCreateFinish",
                                CommandStart = teamCreate.CommandStart,
                                TeamNum = indexV,
                                WebSocketID = teamCreate.WebSocketID,
                                PlayerDetail = new CommonClass.TeamDisplayItem()
                                {
                                    Description = "队长",
                                    GUID = CommonClass.Random.getGUID(),
                                    Name = teamCreate.PlayerName
                                }
                            };
                            var msg = sendMsg(teamCreate.FromUrl, Newtonsoft.Json.JsonConvert.SerializeObject(teamCreateFinish));
                            //await (prot)
                            // await sendInmationToUrl(addItem.FromUrl, notifyJson);

                            CommonClass.TeamResult t = new CommonClass.TeamResult()
                            {
                                c = "TeamResult",
                                FromUrl = teamCreate.FromUrl,
                                TeamNumber = indexV,
                                WebSocketID = teamCreate.WebSocketID,
                                Hash = msg.GetHashCode(),
                            };
                            outPut = Newtonsoft.Json.JsonConvert.SerializeObject(t);
                        }; break;
                    case "TeamBegain":
                        {
                            CommonClass.TeamBegain teamBegain = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.TeamBegain>(notifyJson);

                            Team t = null;
                            lock (Program.teamLock)
                            {

                                if (Program.allTeams.ContainsKey(teamBegain.TeamNum))
                                {
                                    t = Program.allTeams[teamBegain.TeamNum];
                                }
                                //Program.allTeams.Add()
                            }
                            if (t == null)
                            {
                                outPut = "ng";
                            }
                            else
                            {
                                int hash = 0;
                                for (var i = 0; i < t.member.Count; i++)
                                {
                                    var secret = CommonClass.AES.AesEncrypt("team:" + teamBegain.RoomIndex.ToString(), t.member[i].CommandStart);
                                    CommonClass.TeamNumWithSecret teamNumWithSecret = new CommonClass.TeamNumWithSecret()
                                    {
                                        c = "TeamNumWithSecret",
                                        WebSocketID = t.member[i].WebSocketID,
                                        Secret = secret
                                    };
                                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(teamNumWithSecret);
                                    var url = t.member[i].FromUrl;
                                    var msg = sendMsg(url, json);
                                    Console.WriteLine(msg);

                                    hash += msg.GetHashCode();
                                }
                                t.IsBegun = true;
                                outPut = $"ok{hash}";
                            }
                        }; break;
                    case "TeamJoin":
                        {
                            /*
                             * TeamJoin,如果传入的字段不是数字，返回 is not number
                             * TeamJoin,如果传入的字段如果是数字，不存在这个队伍 返回not has the team
                             * TeamJoin,如果队伍中人数已满，不存在这个队伍 返回team is full
                             *  
                             */
                            CommonClass.TeamJoin teamJoin = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.TeamJoin>(notifyJson);
                            int teamIndex;
                            if (int.TryParse(teamJoin.TeamIndex, out teamIndex))
                            {
                                if (teamIndex < 0)
                                {
                                    outPut = "need to back";
                                }
                                else
                                {
                                    bool memberIsFull = false;
                                    bool notHasTheTeam = false;
                                    Team t = null;
                                    lock (Program.teamLock)
                                    {
                                        if (Program.allTeams.ContainsKey(teamIndex))
                                        {
                                            if (Program.allTeams[teamIndex].member.Count > 4)
                                            {
                                                memberIsFull = true;
                                            }
                                            else
                                            {
                                                Program.allTeams[teamIndex].member.Add(teamJoin);
                                                teamJoin.Guid = CommonClass.Random.getGUID();

                                                t = Program.allTeams[teamIndex];
                                            }

                                        }
                                        else
                                        {
                                            notHasTheTeam = true;
                                        }
                                    }
                                    if (memberIsFull)
                                    {
                                        outPut = "team is full";
                                        // await context.Response.WriteAsync("");
                                    }
                                    else if (notHasTheTeam)
                                    {
                                        outPut = "not has the team";
                                        //await context.Response.WriteAsync("");
                                    }
                                    else if (t.IsBegun)
                                    {
                                        outPut = "game has begun";
                                        //t.IsBegun 必须在判断 notHasTheTeam 之后。否则t可能为null
                                        //  await context.Response.WriteAsync("");
                                    }
                                    else
                                    {

                                        var PlayerNames = new List<string>();
                                        CommonClass.TeamJoinFinish teamJoinFinish = new CommonClass.TeamJoinFinish()
                                        {
                                            c = "TeamJoinFinish",
                                            Players = new List<CommonClass.TeamDisplayItem>(),
                                            TeamNum = t.TeamID,
                                            WebSocketID = teamJoin.WebSocketID
                                            //  PlayerNames = 
                                        };
                                        {
                                            CommonClass.TeamJoinBroadInfo addInfomation = new CommonClass.TeamJoinBroadInfo()
                                            {
                                                c = "TeamJoinBroadInfo",
                                                Player = new CommonClass.TeamDisplayItem()
                                                {
                                                    Name = teamJoin.PlayerName,
                                                    Description = "队员",
                                                    GUID = teamJoin.Guid
                                                },
                                                WebSocketID = t.captain.WebSocketID,
                                            };
                                            sendMsg(t.captain.FromUrl, Newtonsoft.Json.JsonConvert.SerializeObject(addInfomation));
                                        }
                                        teamJoinFinish.Players.Add(
                                            new CommonClass.TeamDisplayItem()
                                            {
                                                Name = t.captain.PlayerName,
                                                Description = "队长",
                                                GUID = "",
                                                IsSelf = false,
                                            }
                                            );

                                        for (var i = 0; i < t.member.Count; i++)
                                        {
                                            CommonClass.TeamDisplayItem displayItem;
                                            if (t.member[i].FromUrl == teamJoin.FromUrl && t.member[i].WebSocketID == teamJoin.WebSocketID)
                                            {
                                                displayItem = new CommonClass.TeamDisplayItem()
                                                {
                                                    Name = t.member[i].PlayerName,
                                                    Description = "队员",
                                                    GUID = t.member[i].Guid,
                                                    IsSelf = true,
                                                };
                                            }
                                            else
                                            {
                                                displayItem = new CommonClass.TeamDisplayItem()
                                                {
                                                    Name = t.member[i].PlayerName,
                                                    Description = "队员",
                                                    GUID = t.member[i].Guid,
                                                    IsSelf = false,
                                                };
                                                CommonClass.TeamJoinBroadInfo addInfomation = new CommonClass.TeamJoinBroadInfo()
                                                {
                                                    c = "TeamJoinBroadInfo",
                                                    Player = new CommonClass.TeamDisplayItem()
                                                    {
                                                        Name = teamJoin.PlayerName,
                                                        GUID = teamJoin.Guid,
                                                        Description = "队员",
                                                        IsSelf = false
                                                    },
                                                    WebSocketID = t.member[i].WebSocketID,
                                                };
                                                sendMsg(t.member[i].FromUrl, Newtonsoft.Json.JsonConvert.SerializeObject(addInfomation));
                                            }
                                            teamJoinFinish.Players.Add(displayItem);
                                        }
                                        sendMsg(teamJoin.FromUrl, Newtonsoft.Json.JsonConvert.SerializeObject(teamJoinFinish));
                                        outPut = "ok";
                                    }
                                }
                            }
                            else
                            {
                                outPut = "is not number";
                                // await context.Response.WriteAsync("");
                            }
                        }; break;
                    case "LeaveTeam":
                        {
                            CommonClass.LeaveTeam leaveTeam = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.LeaveTeam>(notifyJson);
                            int teamIndex;
                            if (int.TryParse(leaveTeam.TeamIndex, out teamIndex))
                            {
                                Team t = null;
                                lock (Program.teamLock)
                                {
                                    if (Program.allTeams.ContainsKey(teamIndex))
                                    {
                                        t = Program.allTeams[teamIndex];
                                        if (!t.IsBegun)
                                        {
                                            var index = t.member.FindIndex(item => item.WebSocketID == leaveTeam.WebSocketID && item.FromUrl == leaveTeam.FromUrl);
                                            if (index != -1)
                                            {
                                                var removeItem = t.member[index];
                                                outPut = "success";
                                                t.member.Remove(removeItem);
                                                {
                                                    CommonClass.TeamJoinRemoveInfo RemoveInfomation = new CommonClass.TeamJoinRemoveInfo()
                                                    {
                                                        c = "TeamJoinRemoveInfo",
                                                        WebSocketID = t.captain.WebSocketID,
                                                        Guid = removeItem.Guid
                                                    };
                                                    sendMsg(t.captain.FromUrl, Newtonsoft.Json.JsonConvert.SerializeObject(RemoveInfomation));
                                                }
                                                for (var i = 0; i < t.member.Count; i++)
                                                {
                                                    if (t.member[i].FromUrl == removeItem.FromUrl && t.member[i].WebSocketID == removeItem.WebSocketID)
                                                    {

                                                    }
                                                    else
                                                    {
                                                        CommonClass.TeamJoinRemoveInfo RemoveInfomation = new CommonClass.TeamJoinRemoveInfo()
                                                        {
                                                            c = "TeamJoinRemoveInfo",
                                                            WebSocketID = t.member[i].WebSocketID,
                                                            Guid = removeItem.Guid
                                                        };
                                                        sendMsg(t.member[i].FromUrl, Newtonsoft.Json.JsonConvert.SerializeObject(RemoveInfomation));
                                                    }
                                                }

                                            }
                                            else
                                                outPut = "not has the mumber";
                                        }
                                        else
                                        {
                                            outPut = "game has begun";
                                        }

                                    }
                                    else
                                    {
                                        outPut = "not has the team";
                                    }
                                }
                            }
                            else
                            {
                                outPut = "is not number";
                            }
                        }; break;
                    case "TeamExit":
                        {
                            CommonClass.TeamExit teamBegain = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.TeamExit>(notifyJson);

                            Team t = null;
                            lock (Program.teamLock)
                            {

                                if (Program.allTeams.ContainsKey(teamBegain.TeamNum))
                                {
                                    t = Program.allTeams[teamBegain.TeamNum];
                                }
                                //Program.allTeams.Add()
                            }
                            if (t == null)
                            {
                                outPut = "ng";
                            }
                            else
                            {
                                int hash = 0;
                                for (var i = 0; i < t.member.Count; i++)
                                {
                                    var secret = CommonClass.AES.AesEncrypt("exitTeam:0", t.member[i].CommandStart);
                                    CommonClass.TeamNumWithSecret teamNumWithSecret = new CommonClass.TeamNumWithSecret()
                                    {
                                        c = "TeamNumWithSecret",
                                        WebSocketID = t.member[i].WebSocketID,
                                        Secret = secret
                                    };
                                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(teamNumWithSecret);
                                    var url = t.member[i].FromUrl;
                                    var msg = sendMsg(url, json);
                                    Console.WriteLine(msg);

                                    hash += msg.GetHashCode();
                                }
                                t.IsBegun = true;
                                outPut = $"ok{hash}";
                            }
                        }; break;
                }
            }
            {
                return outPut;
            }
        }

        private static string sendMsg(string fromUrl, string json)
        {
            // var r = await Task.Run<string>(() => TcpFunction.WithResponse.SendInmationToUrlAndGetRes(fromUrl, json));
            var t = TcpFunction.WithResponse.SendInmationToUrlAndGetRes(fromUrl, json);

            return t.GetAwaiter().GetResult();
        }
    }
}
