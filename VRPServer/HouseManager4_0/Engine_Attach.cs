using CommonClass;
using HouseManager4_0.RoomMainF;
using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager4_0
{
    public class Engine_Attach : Engine, interfaceOfEngine.engine, interfaceOfEngine.tryCatchAction
    {
        public Engine_Attach(RoomMain roomMain)
        {
            this.roomMain = roomMain;
        }

        public bool carAbilitConditionsOk(RoleInGame player, Car car, Command c)
        {
            if (c.c == "DialogMsg")
            {
                if (car.state == Car.CarState.waitAtBaseStation)
                {
                    return true;
                }
                else
                {
                    DialogMsg dm = (DialogMsg)c;
                    dm.Msg = $"你的小车还没有回营地！";
                    that.ResponMsg(dm);
                    return false;
                }
            }
            else
            {
                return false;
            }
            //if (car.state == Car.CarState.waitAtBaseStation) 
            //{
            //    return 
            //}
            //  throw new NotImplementedException();
        }

        public bool conditionsOk(Command c, out string reason)
        {
            if (c.c == "DialogMsg")
            {
                DialogMsg dm = (DialogMsg)c;
                if (dm.Msg.Trim() == "认你做老大")
                {
                    if (that._Players.ContainsKey(dm.Key))
                    {
                        if (!that._Players[dm.Key].Bust)
                        {
                            if (that._Players.ContainsKey(dm.To))
                            {
                                if (!that._Players[dm.To].Bust)
                                {
                                    reason = "";
                                    return true;
                                }
                                else
                                {
                                    dm.Msg = $"【{that._Players[dm.To].PlayerName}】已经破产，不能拜他做老大!";
                                    that.ResponMsg(dm);
                                    //this.WebNotify(that._Players[dm.Key], );
                                }
                            }
                        }
                    }
                }
            }
            reason = "";
            return false;

        }

        public void failedThenDo(Car car, RoleInGame player, Command c, ref List<string> notifyMsg)
        {
            // throw new NotImplementedException();
        }

        public RoomMain.commandWithTime.ReturningOjb maindDo(RoleInGame player, Car car, Command c, ref List<string> notifyMsg, out RoomMain.MileResultReason mrr)
        {
            //notifyMsg.Add(((Player)this._Players[dm.Key]).FromUrl);
            //dm.WebSocketID = ((Player)this._Players[dm.Key]).WebSocketID;
            //dm.Msg = "不可以拜NPC为老大！";
            //notifyMsg.Add(Newtonsoft.Json.JsonConvert.SerializeObject(dm));

            lock (this.that.PlayerLock)
            {
                DialogMsg dm = (DialogMsg)c;
                if (dm.Key.Trim() == dm.To.Trim())
                {
                    var boss = (Player)that._Players[dm.To];
                    // boss.TheLargestHolderKey = dm.Key;
                    // boss.InitializeTheLargestHolder
                    boss.InitializeTheLargestHolder(ref notifyMsg);
                    mrr = RoomMain.MileResultReason.Abundant;
                    return player.returningOjb;
                }
                else
                {
                    var boss = (Player)that._Players[dm.To];
                    if (boss.Key == boss.TheLargestHolderKey)
                    {
                        if (player.TheLargestHolderKey == boss.Key)
                        {
                            var dm1 = new DialogMsg()
                            {
                                c = dm.c,
                                Key = dm.Key,
                                Msg = $"【{that._Players[dm.To].PlayerName}】已经是你老大！",
                                To = dm.To,
                                WebSocketID = dm.WebSocketID
                            };
                            that.ResponMsg(dm1);
                        }
                        else if (player.TheLargestHolderKey == player.Key)
                        {
                            player.SetTheLargestHolder(boss, ref notifyMsg);
                            {
                                var dm1 = new DialogMsg()
                                {
                                    c = dm.c,
                                    Key = dm.Key,
                                    Msg = $"[系统]你拜了【{boss.PlayerName}】为老大！",
                                    To = dm.To,
                                    WebSocketID = dm.WebSocketID
                                };
                                that.ResponMsg(dm1);
                            }
                            {
                                var dm1 = new DialogMsg()
                                {
                                    c = dm.c,
                                    Key = dm.Key,
                                    Msg = $"[系统]【{boss.PlayerName}】拜了你为老大！",
                                    To = dm.To,
                                    WebSocketID = dm.WebSocketID
                                };
                                that.RequstMsg(dm1);
                            }
                        }
                        else
                        {
                            var oldBoss = that._Players[player.TheLargestHolderKey];
                            {
                                var dm1 = new DialogMsg()
                                {
                                    c = dm.c,
                                    Key = dm.Key,
                                    Msg = $"[系统]【{player.PlayerName}】拜了别人为老大！",
                                    To = oldBoss.Key,
                                    WebSocketID = dm.WebSocketID
                                };
                                that.RequstMsg(dm1);
                            }
                            player.SetTheLargestHolder(boss, ref notifyMsg);
                            {
                                var dm1 = new DialogMsg()
                                {
                                    c = dm.c,
                                    Key = dm.Key,
                                    Msg = $"[系统]你拜了【{boss.PlayerName}】为老大！",
                                    To = dm.To,
                                    WebSocketID = dm.WebSocketID
                                };
                                that.ResponMsg(dm1);
                            }
                            {
                                var dm1 = new DialogMsg()
                                {
                                    c = dm.c,
                                    Key = dm.Key,
                                    Msg = $"[系统]【{boss.PlayerName}】拜了你为老大！",
                                    To = dm.To,
                                    WebSocketID = dm.WebSocketID
                                };
                                that.RequstMsg(dm1);
                            }
                        }
                        mrr = RoomMain.MileResultReason.Abundant;
                        return player.returningOjb;
                    }
                    else
                    {
                        var boss2 = (Player)that._Players[boss.TheLargestHolderKey];
                        var dm1 = new DialogMsg()
                        {
                            c = dm.c,
                            Key = dm.Key,
                            Msg = $"【{boss2.PlayerName}】是{boss.PlayerName}老大！",
                            To = dm.To,
                            WebSocketID = dm.WebSocketID
                        };
                        that.ResponMsg(dm1);
                        mrr = RoomMain.MileResultReason.Abundant;
                        return player.returningOjb;
                    }
                }
            }

        }

        internal void DealWithMsg(DialogMsg dm)
        {
            this.updateAction(this, dm, dm.Key);
            //  throw new NotImplementedException();
        }
    }
}
