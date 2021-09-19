using CommonClass;
using System;
using System.Collections.Generic;
using static HouseManager4_0.RoomMainF.RoomMain;
using OssModel = Model;

namespace HouseManager4_0
{
    public abstract class Engine_ContactEngine : Engine
    {

        private RoomMainF.RoomMain.commandWithTime.ReturningOjb contactPassBossAddress(RoleInGame player, RoleInGame boss, Car car, interfaceOfHM.ContactInterface ci, ref List<string> notifyMsg, out MileResultReason mrr)
        {
            if (that._Players.ContainsKey(ci.targetOwner))
            {
                var victimOrBeneficiary = that._Players[ci.targetOwner];
                OssModel.FastonPosition fpResult;
                var distanceIsEnoughToStart = that.theNearestToPlayerIsCarNotMoney(player, car, victimOrBeneficiary, out fpResult);
                if (distanceIsEnoughToStart)
                {
                    if (ci.carLeftConditions(car))
                    {
                        var from = this.getFromWhenAction(player, car);
                        var to = ci.target;
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
                            ci.SetArrivalThread(startT, car, goMile, ro);
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
                    this.WebNotify(player, $"离【{victimOrBeneficiary.PlayerName}】最近的是[{fpResult.FastenPositionName}]处的钱，不是你的车，攻击失败！");
                    return player.returningOjb;
                }
            }
            else
            {
                throw new Exception("判断条件异常！");
            }
        }


        public commandWithTime.ReturningOjb randomWhenConfused(RoleInGame player, RoleInGame boss, Car car, interfaceOfHM.ContactInterface ci, ref List<string> notifyMsg, out MileResultReason Mrr)
        {
            var randomValue = that.rm.Next(0, 5);
            if (randomValue == 4)
            {
                return contactPassBossAddress(player, boss, car, ci, ref notifyMsg, out Mrr);
            }
            else
            {
                var victim = that._Players[ci.targetOwner];
                var target = Program.dt.GetFpByIndex(victim.StartFPIndex);
                var collectIndexTarget = that.getCollectPositionsByDistance(target)[randomValue];
                var sc = new SetCollect()
                {
                    c = "SetCollect",
                    collectIndex = collectIndexTarget,
                    cType = "findWork",
                    fastenpositionID = Program.dt.GetFpByIndex(that._collectPosition[collectIndexTarget]).FastenPositionID,
                    Key = player.Key
                };
                return that.collectE.collectPassBossAddress(player, boss, car, sc, ref notifyMsg, out Mrr);
            }
        }

        public RoomMainF.RoomMain.commandWithTime.ReturningOjb contact(RoleInGame player, Car car, interfaceOfHM.ContactInterface ci, ref List<string> notifyMsg, out MileResultReason Mrr)
        {
            //sa = attackTargetChange(player, sa);
            RoleInGame boss;
            if (player.HasTheBoss(roomMain._Players, out boss))
            {
                if (player.confuseRecord.IsBeingControlled())
                {
                    if (player.confuseRecord.getControlType() == Manager_Driver.ConfuseManger.ControlAttackType.Confuse)
                    {
                        return randomWhenConfused(player, boss, car, ci, ref notifyMsg, out Mrr);
                    }
                    else
                    {
                        return contactPassBossAddress(player, boss, car, ci, ref notifyMsg, out Mrr);
                    }
                }
                else
                    return contactPassBossAddress(player, boss, car, ci, ref notifyMsg, out Mrr);
                //return promotePassBossAddress(player, boss, car, sp, ref notifyMsg, out reason);
            }
            else
            {
                if (that._Players.ContainsKey(ci.targetOwner))
                {
                    //beneficiary 
                    var victimOrBeneficiary = that._Players[ci.targetOwner];
                    OssModel.FastonPosition fpResult;
                    var distanceIsEnoughToStart = that.theNearestToPlayerIsCarNotMoney(player, car, victimOrBeneficiary, out fpResult);
                    if (distanceIsEnoughToStart)
                        if (ci.carLeftConditions(car))
                        {
                            var from = this.getFromWhenAction(player, car);
                            var to = ci.target;
                            var fp1 = Program.dt.GetFpByIndex(from);
                            //var fp2 = Program.dt.GetFpByIndex(to);
                            //var baseFp = Program.dt.GetFpByIndex(player.StartFPIndex);
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
                                ci.SetArrivalThread(startT, car, goMile, commandWithTime.ReturningOjb.ojbWithoutBoss(returnPath));

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
                        this.WebNotify(player, $"离【{victimOrBeneficiary.PlayerName}】最近的是[{fpResult.FastenPositionName}]处的钱，不是你的车，攻击失败！");
                        return player.returningOjb;
                    }
                }
                else
                {
                    throw new Exception("准备运行条件里没有筛查？");
                }

            }
        }

    }
}
