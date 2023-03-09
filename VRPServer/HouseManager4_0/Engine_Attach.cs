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

        public bool carAbilitConditionsOk(RoleInGame player, Car car, Command c, GetRandomPos grp)
        {
            if (c.c == "DialogMsg")
            {
                if (car.state == Car.CarState.waitAtBaseStation)
                {
                    return true;
                }
                else
                {
                    this.WebNotify(player, "战车还没有回营地。");
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

        public bool conditionsOk(Command c, GetRandomPos grp, out string reason)
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

        public void failedThenDo(Car car, RoleInGame player, Command c, GetRandomPos grp, ref List<string> notifyMsg)
        {
            // throw new NotImplementedException();
        }

        public RoomMain.commandWithTime.ReturningOjb maindDo(RoleInGame player, Car car, Command c, GetRandomPos grp, ref List<string> notifyMsg, out RoomMain.MileResultReason mrr)
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
                    if (that._Players[dm.To].playerType == RoleInGame.PlayerType.player)
                    {
                        var boss = (Player)that._Players[dm.To];
                        if (boss.Key == boss.TheLargestHolderKey)
                        {
                            if (player.TheLargestHolderKey == boss.Key)
                            {
                                this.WebNotify(player, $"【{that._Players[dm.To].PlayerName}】已经是你老大！");
                            }
                            else if (player.TheLargestHolderKey == player.Key)
                            {
                                List<Player> children;
                                player.SetTheLargestHolder(boss, ref notifyMsg, out children);
                                for (int i = 0; i < children.Count; i++)
                                {
                                    this.WebNotify(children[i], "你所在的队伍解散了");
                                }
                                this.WebNotify(player, $"你拜了{boss.PlayerName}为老大！");
                                this.WebNotify(boss, $"{player.PlayerName}拜了你为老大！");
                            }
                            else
                            {
                                player.SetTheLargestHolder(boss, ref notifyMsg);
                                this.WebNotify(player, $"你拜了{boss.PlayerName}为老大！");
                                this.WebNotify(boss, $"{player.PlayerName}拜了你为老大！");
                            }
                            mrr = RoomMain.MileResultReason.Abundant;
                            return player.returningOjb;
                        }
                        else
                        {
                            var boss2 = (Player)that._Players[boss.TheLargestHolderKey];
                            this.WebNotify(player, $"【{boss2.PlayerName}】是{boss.PlayerName}老大！");
                            mrr = RoomMain.MileResultReason.Abundant;
                            return player.returningOjb;
                        }
                    }
                    else if (that._Players[dm.To].playerType == RoleInGame.PlayerType.NPC)
                    {
                        // var player = that._Players[dm.Key];
                        this.WebNotify(player, $"【{that._Players[dm.To].PlayerName}】是NPC,不能拜其为老大！");
                        mrr = RoomMain.MileResultReason.Abundant;
                        return player.returningOjb;
                    }
                    else
                    {
                        mrr = RoomMain.MileResultReason.Abundant;
                        return player.returningOjb;
                    }
                }
            }

        }

        internal void DealWithMsg(DialogMsg dm, GetRandomPos grp)
        {
            this.updateAction(this, dm, grp, dm.Key);
            //  throw new NotImplementedException();
        }
    }
}
