using CommonClass;
using HouseManager4_0.RoomMainF;
using System;
using System.Collections.Generic;
using System.Text;
using static HouseManager4_0.Car;
using static HouseManager4_0.RoomMainF.RoomMain;

namespace HouseManager4_0
{
    public class Engine_AttackEngine : Engine, interfaceOfEngine.engine, interfaceOfEngine.tryCatchAction
    {

        public Engine_AttackEngine(RoomMain roomMain)
        {
            this.roomMain = roomMain;
        }
        internal string updateAttack(SetAttack sa)
        {
            //if (this.conditionsOk())
            //{

            //}
            return this.updateAction(this, sa, sa.Key);

            //            if (!(that._Players.ContainsKey(sa.targetOwner)))
            //            {
            //                return "";
            //            }
            //            else if (that._Players[sa.targetOwner].StartFPIndex != sa.target)
            //            {
            //                return "";
            //            }
            //            else if (sa.targetOwner == sa.Key)
            //            {
            //#warning 这里要加日志，出现了自己攻击自己！！！
            //                return "";
            //            }
            //            else
            //            {

            //                List<string> notifyMsg = new List<string>();
            //                lock (that.PlayerLock)
            //                {
            //                    if (that._Players.ContainsKey(sa.Key))
            //                    {
            //                        if (that._Players[sa.Key].Bust) { }
            //                        else
            //                        {
            //                            //  case "findWork":
            //                            {
            //                                var player = that._Players[sa.Key];
            //                                var car = that._Players[sa.Key].getCar();
            //                                switch (car.state)
            //                                {
            //                                    case CarState.waitAtBaseStation:
            //                                        {
            //                                            {
            //                                                if (car.ability.leftBusiness > 0)
            //                                                {
            //                                                    var state = CheckTargetState(sa.targetOwner);
            //                                                    if (state == CarStateForBeAttacked.CanBeAttacked)
            //                                                    {
            //                                                        MileResultReason mrr;
            //                                                        attack(player, car, sa, ref notifyMsg, out mrr);
            //                                                        if (mrr == MileResultReason.Abundant)
            //                                                        {

            //                                                        }
            //                                                        else
            //                                                        {
            //                                                            if (mrr == MileResultReason.CanNotReach)
            //                                                            {
            //                                                                // this.WebNotify(player, "");
            //                                                                // IfIsNPCBuyThenUseDiamond(player, car);
            //                                                            }
            //                                                            else if (mrr == MileResultReason.CanNotReturn)
            //                                                            {
            //                                                                //IfIsNPCBuyThenUseDiamond(player, car);
            //                                                            }
            //                                                            else if (mrr == MileResultReason.MoneyIsNotEnougt)
            //                                                            { }
            //                                                        }
            //                                                        // doAttack(player, car, sa, ref notifyMsg);
            //                                                    }
            //                                                    else if (state == CarStateForBeAttacked.HasBeenBust)
            //                                                    {
            //                                                        //Console.WriteLine($"攻击对象已经破产！");
            //                                                        WebNotify(player, $"{that._Players[sa.targetOwner].PlayerName}已经破产，没有攻击的必要了！");

            //                                                    }
            //                                                    else if (state == CarStateForBeAttacked.NotExisted)
            //                                                    {
            //                                                        // Console.WriteLine($"攻击对象已经退出了游戏！");
            //                                                        WebNotify(player, $"攻击对象已经退出了游戏");
            //                                                    }
            //                                                    else
            //                                                    {
            //                                                        throw new Exception($"{state.ToString()}未注册！");
            //                                                    }
            //                                                }
            //                                                else
            //                                                {
            //                                                    WebNotify(player, $"小车没有多余的业务空间");
            //                                                }
            //                                            }
            //                                        }; break;
            //                                    case CarState.waitOnRoad:
            //                                        {
            //                                            /*
            //                                             * 在接收到攻击指令时，如果小车在路上，说明，
            //                                             * 其上一个任务是抢能力宝石，结果是没抢到。其
            //                                             * 目的应该应该为purpose=null 
            //                                             */

            //                                            {
            //                                                var state = CheckTargetState(sa.targetOwner);
            //                                                if (state == CarStateForBeAttacked.CanBeAttacked)
            //                                                {
            //                                                    MileResultReason mrr;
            //                                                    attack(player, car, sa, ref notifyMsg, out mrr);
            //                                                    if (mrr == MileResultReason.Abundant) { }
            //                                                    else if (mrr == MileResultReason.CanNotReach)
            //                                                    {
            //                                                        this.carDoActionFailedThenMustReturn(car, player, ref notifyMsg);
            //                                                    }
            //                                                    else if (mrr == MileResultReason.CanNotReturn)
            //                                                    {
            //                                                        this.carDoActionFailedThenMustReturn(car, player, ref notifyMsg);
            //                                                    }
            //                                                    else if (mrr == MileResultReason.MoneyIsNotEnougt)
            //                                                    {
            //                                                        this.carDoActionFailedThenMustReturn(car, player, ref notifyMsg);
            //                                                    }
            //                                                    // doAttack(player, car, sa, ref notifyMsg);
            //                                                }
            //                                                else if (state == CarStateForBeAttacked.HasBeenBust)
            //                                                {
            //                                                    this.carDoActionFailedThenMustReturn(car, player, ref notifyMsg);
            //                                                    Console.WriteLine($"攻击对象已经破产！");
            //                                                }
            //                                                else if (state == CarStateForBeAttacked.NotExisted)
            //                                                {
            //                                                    this.carDoActionFailedThenMustReturn(car, player, ref notifyMsg);
            //                                                    Console.WriteLine($"攻击对象已经退出了游戏！");
            //                                                }
            //                                                else
            //                                                {
            //                                                    throw new Exception($"{state.ToString()}未注册！");
            //                                                }
            //                                            }

            //                                        }; break;
            //                                    case CarState.working:
            //                                        {
            //                                            WebNotify(player, "您的小车正在赶往目标！");
            //                                        }; break;
            //                                    case CarState.returning:
            //                                        {
            //                                            WebNotify(player, "您的小车正在赶往目标！");
            //                                        }; break;


            //                                }
            //                                //  MeetWithNPC(sa);
            //                            };
            //                        }
            //                    }
            //                }

            //                for (var i = 0; i < notifyMsg.Count; i += 2)
            //                {
            //                    var url = notifyMsg[i];
            //                    var sendMsg = notifyMsg[i + 1];
            //                    //Console.WriteLine($"url:{url}");
            //                    if (!string.IsNullOrEmpty(url))
            //                    {
            //                        Startup.sendMsg(url, sendMsg);
            //                    }
            //                }
            //                return "";
            //            }
        }

        public RoomMainF.RoomMain.commandWithTime.ReturningOjb maindDo(RoleInGame player, Car car, Command c, ref List<string> notifyMsg, out MileResultReason mrr)
        {
            if (c.c == "SetAttack")
            {
                var sa = (SetAttack)c;
                return attack(player, car, sa, ref notifyMsg, out mrr);
            }
            else
            {
                throw new Exception($"数据传输错误！(传出类型为{c.c})");
            }
        }
        public void failedThenDo(Car car, RoleInGame player, Command c, ref List<string> notifyMsg)
        {
            if (c.c == "SetAttack")
            {
                SetAttack sa = (SetAttack)c;
                this.carDoActionFailedThenMustReturn(car, player, ref notifyMsg);
                //this.carsAttackFailedThenMustReturn(car, player, sa, ref notifyMsg);
            }
        }

        public bool carAbilitConditionsOk(RoleInGame player, Car car, Command c)
        {
            if (c.c == "SetAttack")
                if (car.ability.leftBusiness > 0)
                {
                    SetAttack sa = (SetAttack)c;
                    var state = CheckTargetState(sa.targetOwner);
                    if (state == CarStateForBeAttacked.CanBeAttacked)
                    {
                        return true;
                        // doAttack(player, car, sa, ref notifyMsg);
                    }
                    else if (state == CarStateForBeAttacked.HasBeenBust)
                    {
                        this.WebNotify(player, "攻击的对象已经破产！");
                        return false;
                    }
                    else if (state == CarStateForBeAttacked.NotExisted)
                    {
                        this.WebNotify(player, "攻击的对象已经退出游戏！");
                        return false;
                    }
                    else
                    {
                        throw new Exception($"{state.ToString()}未注册！");
                    }
                }
                else
                {
                    this.WebNotify(player, "小车已经没有多余业务容量！");
                    return false;
                }
            else
            {
                return false;
            }
        }
        public bool conditionsOk(Command c, out string reason)
        {
            if (c.c == "SetAttack")
            {
                SetAttack sa = (SetAttack)c;
                if (!(that._Players.ContainsKey(sa.targetOwner)))
                {
                    reason = "";
                    return false;
                }
                else if (that._Players[sa.targetOwner].StartFPIndex != sa.target)
                {
                    reason = "";
                    return false;
                }
                else if (sa.targetOwner == sa.Key)
                {
#warning 这里要加日志，出现了自己攻击自己！！！
                    reason = "";
                    return false;
                }
                else
                {
                    reason = "";
                    return true;
                }
            }
            reason = "";
            return false;
        }

        enum CarStateForBeAttacked
        {
            CanBeAttacked,
            NotExisted,
            HasBeenBust,
        }
        private CarStateForBeAttacked CheckTargetState(string targetOwner)
        {
            if (roomMain._Players.ContainsKey(targetOwner))
            {
                if (roomMain._Players[targetOwner].Bust)
                {
                    return CarStateForBeAttacked.HasBeenBust;
                }
                else
                {
                    return CarStateForBeAttacked.CanBeAttacked;
                }
            }
            else
            {
                return CarStateForBeAttacked.NotExisted;
            }
        }

        /// <summary>
        /// 此函数，必须在this._Players.ContainsKey(sa.targetOwner)=true且this._Players[sa.targetOwner].Bust=false情况下运行。请提前进行判断！
        /// </summary>
        /// <param name="player">玩家</param>
        /// <param name="car"></param>
        /// <param name="sa"></param>
        /// <param name="notifyMsg"></param>
        /// <param name="victimState"></param>
        /// <param name="reason"></param>
        RoomMainF.RoomMain.commandWithTime.ReturningOjb attack(RoleInGame player, Car car, SetAttack sa, ref List<string> notifyMsg, out MileResultReason Mrr)
        {
            //sa = attackTargetChange(player, sa);
            RoleInGame boss;
            if (player.HasTheBoss(roomMain._Players, out boss))
            {
                return attackPassBossAddress(player, boss, car, sa, ref notifyMsg, out Mrr);
                //return promotePassBossAddress(player, boss, car, sp, ref notifyMsg, out reason);
            }
            else
            {
                if (car.ability.leftBusiness > 0)
                {
                    var from = this.getFromWhenAction(player, car);
                    var to = sa.target;
                    var fp1 = Program.dt.GetFpByIndex(from);
                    var fp2 = Program.dt.GetFpByIndex(to);
                    var baseFp = Program.dt.GetFpByIndex(player.StartFPIndex);
                    var goPath = that.GetAFromB(from, to, player, ref notifyMsg);
                    //var returnPath = Program.dt.GetAFromB(fp2, baseFp.FastenPositionID);
                    //var returnPath = Program.dt.GetAFromB(to, player.StartFPIndex);
                    var returnPath = that.GetAFromB(to, player.StartFPIndex, player, ref notifyMsg);

                    var goMile = that.GetMile(goPath);
                    var returnMile = that.GetMile(returnPath);



                    //第一步，计算去程和回程。
                    if (car.ability.leftMile >= goMile + returnMile)
                    {
                        int startT;
                        this.EditCarStateWhenActionStartOK(player, ref car, to, fp1, goPath, ref notifyMsg, out startT);
                        SetAttackArrivalThread(startT, car, sa, goMile, commandWithTime.ReturningOjb.ojbWithoutBoss(returnPath));

                        // getAllCarInfomations(sa.Key, ref notifyMsg);
                        Mrr = MileResultReason.Abundant;
                        return commandWithTime.ReturningOjb.ojbWithoutBoss(returnPath);
                    }

                    else if (car.ability.leftMile >= goMile)
                    {
                        Mrr = MileResultReason.CanNotReturn;
                        return player.returningOjb;
                    }
                    else
                    {
                        Mrr = MileResultReason.CanNotReach;
                        return player.returningOjb;
                    }
                }
                else
                {
                    Mrr = MileResultReason.MoneyIsNotEnougt;
                    this.WebNotify(player, "你身上的剩余业务空间不够啦！");
                    return player.returningOjb;
                }
            }
        }
        [Obsolete]
        /// <summary>
        /// 此方法暂时作废！！！
        /// </summary>
        /// <param name="player"></param>
        /// <param name="sa"></param>
        /// <returns></returns> 
        private SetAttack attackTargetChange(RoleInGame player, SetAttack sa)
        {
            throw new Exception("");
            if (that._Players.ContainsKey(sa.targetOwner))
            {
                var victim = that._Players[sa.targetOwner];
                if (victim.Bust) { }
                else
                {
                    if (that._Players.ContainsKey(victim.TheLargestHolderKey))
                    {
                        if (victim.TheLargestHolderKey != victim.Key)
                        {
                            if (that._Players[victim.TheLargestHolderKey].Bust)
                            {

                            }
                            else
                            {
                                var attackBoss = that._Players[victim.TheLargestHolderKey];
                                this.WebNotify(player, $"你对【{victim.PlayerName}】的攻击被转移给了【{attackBoss.PlayerName}】");
                                this.WebNotify(victim, $"【{player.PlayerName}】对你的攻击被转移给了【{attackBoss.PlayerName}】");
                                this.WebNotify(attackBoss, $"【{player.PlayerName}】对【{victim.PlayerName}】的攻击被转移给了你");

                                SetAttack saNew = new SetAttack()
                                {
                                    c = "SetAttack",
                                    Key = sa.Key,
                                    target = attackBoss.StartFPIndex,
                                    targetOwner = attackBoss.Key
                                };
                                return saNew;
                            }
                        }
                    }
                }
            }
            return sa;
        }

        private RoomMainF.RoomMain.commandWithTime.ReturningOjb attackPassBossAddress(RoleInGame player, RoleInGame boss, Car car, SetAttack sa, ref List<string> notifyMsg, out MileResultReason mrr)
        {
            if (car.ability.leftBusiness > 0)
            {
                var from = this.getFromWhenAction(player, car);
                var to = sa.target;
                var fp1 = Program.dt.GetFpByIndex(from);
                var fp2 = Program.dt.GetFpByIndex(to);
                var baseFp = Program.dt.GetFpByIndex(player.StartFPIndex);

                // var goPath = Program.dt.GetAFromB(fp1, fp2.FastenPositionID);

                //var goPath = Program.dt.GetAFromB(from, to);
                var goPath = that.GetAFromB(from, to, player, ref notifyMsg);
                //var returnPath = Program.dt.GetAFromB(fp2, baseFp.FastenPositionID);
                //var returnPath = Program.dt.GetAFromB(to, player.StartFPIndex);
                var returnToBossAddrPath = that.GetAFromB(to, boss.StartFPIndex, player, ref notifyMsg);
                var returnToSelfAddrPath = that.GetAFromB(boss.StartFPIndex, player.StartFPIndex, player, ref notifyMsg);

                var goMile = that.GetMile(goPath);
                var returnToBossAddrPathMile = that.GetMile(returnToBossAddrPath);
                var returnToSelfAddrPathMile = that.GetMile(returnToSelfAddrPath);

                //第一步，计算去程和回程。
                if (car.ability.leftMile >= goMile + returnToBossAddrPathMile + returnToSelfAddrPathMile)
                {
                    int startT;
                    this.EditCarStateWhenActionStartOK(player, ref car, to, fp1, goPath, ref notifyMsg, out startT);
                    var ro = commandWithTime.ReturningOjb.ojbWithBoss(returnToBossAddrPath, returnToSelfAddrPath, boss);
                    SetAttackArrivalThread(startT, car, sa, goMile, ro);
                    mrr = MileResultReason.Abundant;
                    return ro;
                }

                else if (car.ability.leftMile >= goMile)
                {
                    mrr = MileResultReason.CanNotReturn;
                    return player.returningOjb;
                }
                else
                {
                    mrr = MileResultReason.CanNotReach;
                    return player.returningOjb;
                }
            }
            else
            {
                this.WebNotify(player, "你身上的剩余业务空间不够啦！");
                mrr = MileResultReason.MoneyIsNotEnougt;
                return player.returningOjb;
            }
        }







        private void SetAttackArrivalThread(int startT, Car car, SetAttack sa, int goMile, commandWithTime.ReturningOjb ro)
        {
            that.debtE.setDebtT(startT, car, sa, goMile, ro);

        }

        //private void SetAttackArrivalThread(int startT, Car car, SetAttack sa, List<Model.MapGo.nyrqPosition> returnToSelfAddrPath, int goMile)
        //{
        //    SetAttackArrivalThread(startT, car, sa, null, returnToSelfAddrPath, goMile, false, null);
        //    //Thread th = new Thread(() => setDebt(startT, new commandWithTime.debtOwner()
        //    //{
        //    //    c = "debtOwner",
        //    //    key = sa.Key,
        //    //    //  car = sa.car,
        //    //    returnPath = returnPath,
        //    //    target = car.targetFpIndex,//新的起点
        //    //    changeType = "Attack",
        //    //    victim = sa.targetOwner
        //    //}));
        //    //th.Start();
        //}

    }
}
