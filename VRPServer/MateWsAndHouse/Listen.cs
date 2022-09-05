using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MateWsAndHouse
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
                                    indexV = Program.rm.Next(0, maxValue);
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

                                //Program.allTeams.Add()
                            }
                            var teamCreateFinish = new CommonClass.TeamCreateFinish()
                            {
                                c = "TeamCreateFinish",
                                CommandStart = teamCreate.CommandStart,
                                TeamNum = indexV,
                                WebSocketID = teamCreate.WebSocketID,
                                PlayerName = teamCreate.PlayerName
                            };
                            var msg = await sendMsg(teamCreate.FromUrl, Newtonsoft.Json.JsonConvert.SerializeObject(teamCreateFinish));
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
                                    var msg = await sendMsg(url, json);
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
                                bool memberIsFull = false;
                                bool notHasTheTeam = false;
                                Team t = null;
                                lock (Program.teamLock)
                                {
                                    if (Program.allTeams.ContainsKey(teamIndex))
                                    {
                                        if (Program.allTeams[teamIndex].member.Count >= 4)
                                        {
                                            memberIsFull = true;
                                        }
                                        else
                                        {
                                            Program.allTeams[teamIndex].member.Add(teamJoin);
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
                                        PlayerNames = new List<string>(),
                                        TeamNum = t.TeamID,
                                        WebSocketID = teamJoin.WebSocketID
                                        //  PlayerNames = 
                                    };
                                    {
                                        CommonClass.TeamJoinBroadInfo addInfomation = new CommonClass.TeamJoinBroadInfo()
                                        {
                                            c = "TeamJoinBroadInfo",
                                            PlayerName = teamJoin.PlayerName,
                                            WebSocketID = t.captain.WebSocketID
                                        };
                                        await sendMsg(t.captain.FromUrl, Newtonsoft.Json.JsonConvert.SerializeObject(addInfomation));
                                    }
                                    teamJoinFinish.PlayerNames.Add(t.captain.PlayerName);
                                    for (var i = 0; i < t.member.Count; i++)
                                    {
                                        teamJoinFinish.PlayerNames.Add(t.member[i].PlayerName);
                                        if (t.member[i].FromUrl == teamJoin.FromUrl && t.member[i].WebSocketID == teamJoin.WebSocketID)
                                        {

                                        }
                                        else
                                        {
                                            CommonClass.TeamJoinBroadInfo addInfomation = new CommonClass.TeamJoinBroadInfo()
                                            {
                                                c = "TeamJoinBroadInfo",
                                                PlayerName = teamJoin.PlayerName,
                                                WebSocketID = t.member[i].WebSocketID
                                            };
                                            await sendMsg(t.member[i].FromUrl, Newtonsoft.Json.JsonConvert.SerializeObject(addInfomation));
                                        }
                                    }

                                    await sendMsg(teamJoin.FromUrl, Newtonsoft.Json.JsonConvert.SerializeObject(teamJoinFinish));
                                    outPut = "ok";
                                    //  await context.Response.WriteAsync("ok");
                                    // t.captain.
                                    //await context.Response.WriteAsync("not has the team");
                                }
                            }
                            else
                            {
                                outPut = "is not number";
                                // await context.Response.WriteAsync("");
                            }
                        }; break;
                }
            }
            {
                return outPut;
            }
        }

        private static async Task<string> sendMsg(string fromUrl, string json)
        {
            var r = await Task.Run<string>(() => TcpFunction.WithResponse.SendInmationToUrlAndGetRes(fromUrl, json));
            return r;
        }
    }
}
