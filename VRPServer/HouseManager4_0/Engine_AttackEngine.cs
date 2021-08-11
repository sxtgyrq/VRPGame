using CommonClass;
using HouseManager4_0.RoomMainF;
using System;
using System.Collections.Generic;
using System.Text;
using static HouseManager4_0.Car;
using static HouseManager4_0.RoomMainF.RoomMain;
using OssModel = Model;

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
            return this.updateAction(this, sa, sa.Key); 
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
                if (car.state == CarState.waitAtBaseStation)
                {
                    if (player.playerType == RoleInGame.PlayerType.NPC)
                    {
                        ((NPC)player).SetBust(true, ref notifyMsg);
                    }
                }
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
                if (that._Players.ContainsKey(sa.targetOwner))
                {
                    var victim = that._Players[sa.targetOwner];
                    OssModel.FastonPosition fpResult;
                    var distanceIsEnoughToStart = that.theNearestToPlayerIsCarNotMoney(player, car, victim, out fpResult);
                    if (distanceIsEnoughToStart)
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
                    else
                    {
                        Mrr = MileResultReason.NearestIsMoneyWhenAttack;
                        this.WebNotify(player, $"离【{victim.PlayerName}】最近的是[{fpResult.FastenPositionName}]处的钱，不是你的车，攻击失败！");
                        return player.returningOjb;
                    }
                }
                else
                {
                    throw new Exception("准备运行条件里没有筛查？");
                }

            }
        }


        private RoomMainF.RoomMain.commandWithTime.ReturningOjb attackPassBossAddress(RoleInGame player, RoleInGame boss, Car car, SetAttack sa, ref List<string> notifyMsg, out MileResultReason mrr)
        {
            if (that._Players.ContainsKey(sa.targetOwner))
            {
                var victim = that._Players[sa.targetOwner];
                OssModel.FastonPosition fpResult;
                var distanceIsEnoughToStart = that.theNearestToPlayerIsCarNotMoney(player, car, victim, out fpResult);
                if (distanceIsEnoughToStart)
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
                else
                {
                    mrr = MileResultReason.NearestIsMoneyWhenAttack;
                    this.WebNotify(player, $"离【{victim.PlayerName}】最近的是[{fpResult.FastenPositionName}]处的钱，不是你的车，攻击失败！");
                    return player.returningOjb;
                }
            }
            else
            {
                throw new Exception("判断条件异常！");
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
