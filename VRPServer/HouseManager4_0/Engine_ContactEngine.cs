using CommonClass;
using CommonClass.driversource;
using System;
using System.Collections.Generic;
using static HouseManager4_0.Engine_MagicEngine;
using System.Data;
using static HouseManager4_0.RoomMainF.RoomMain;
using OssModel = Model;
using HouseManager4_0.interfaceOfHM;

namespace HouseManager4_0
{
    public abstract class Engine_ContactEngine : Engine
    {

        private RoomMainF.RoomMain.commandWithTime.ReturningOjb contactPassBossAddress(RoleInGame player, RoleInGame boss, Car car, interfaceOfHM.ContactInterface ci, GetRandomPos grp, ref List<string> notifyMsg, out MileResultReason mrr)
        {
            if (that._Players.ContainsKey(ci.targetOwner))
            {
                var victimOrBeneficiary = that._Players[ci.targetOwner];
                OssModel.FastonPosition fpResult;
                var distanceIsEnoughToStart = that.theNearestToPlayerIsCarNotMoney(player, car, victimOrBeneficiary, Program.dt, out fpResult);
                if (distanceIsEnoughToStart)
                {
                    if (ci.carLeftConditions(car))
                    {
                        var from = this.getFromWhenAction(player, car);
                        var to = ci.target;
                        var fp1 = Program.dt.GetFpByIndex(from);
                        //var fp2 = Program.dt.GetFpByIndex(to);
                        //var baseFp = Program.dt.GetFpByIndex(player.StartFPIndex);

                        // var goPath = Program.dt.GetAFromB(fp1, fp2.FastenPositionID);

                        //var goPath = Program.dt.GetAFromB(from, to);
                        var goPath = that.GetAFromB_v2(from, to, player, grp, ref notifyMsg);
                        //var returnPath = Program.dt.GetAFromB(fp2, baseFp.FastenPositionID);
                        //var returnPath = Program.dt.GetAFromB(to, player.StartFPIndex);
                        var returnToBossAddrPath = that.GetAFromB_v2(to, boss.StartFPIndex, player, grp, ref notifyMsg);
                        var returnToSelfAddrPath = that.GetAFromB_v2(boss.StartFPIndex, player.StartFPIndex, player, grp, ref notifyMsg);

                        var goMile = that.GetMile(goPath);
                        var returnToBossAddrPathMile = that.GetMile(returnToBossAddrPath);
                        var returnToSelfAddrPathMile = that.GetMile(returnToSelfAddrPath);

                        //第一步，计算去程和回程。
                        if (car.ability.leftMile >= goMile + returnToBossAddrPathMile + returnToSelfAddrPathMile)
                        {
                            int startT;
                            this.EditCarStateWhenActionStartOK(player, ref car, to, fp1, goPath, grp, ref notifyMsg, out startT);
                            var ro = commandWithTime.ReturningOjb.ojbWithBoss(returnToBossAddrPath, returnToSelfAddrPath, boss);
                            ci.SetArrivalThread(startT, car, goMile, goPath, ro);

                            DealWithDirectAttack(ref car, ref player, ref victimOrBeneficiary, grp);
                            
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
                    that.ViewPosition(player, fpResult, ref notifyMsg);
                    this.WebNotify(player, $"离【{victimOrBeneficiary.PlayerName}】最近的是[{fpResult.FastenPositionName}]处的钱，不是你的车，攻击失败！");
                    return player.returningOjb;
                }
            }
            else
            {
                throw new Exception("判断条件异常！");
            }
        }

        private void DealWithDirectAttack(ref Car car, ref RoleInGame player, ref RoleInGame victimOrBeneficiary, GetRandomPos grp)
        {
            if (car.DirectAttack)
            {
                if (car.ability.driver != null)
                {
                    if (car.ability.driver.race == CommonClass.driversource.Race.immortal)
                    {

                        var driver = car.ability.driver;
                        attackMagicTool at = new attackMagicTool(driver.skill2, player);
                        that.debtE.DirectAttackThenMagic(at, player, victimOrBeneficiary, grp);
                    }
                }
                car.DirectAttack = false;
            }
        }

        public commandWithTime.ReturningOjb randomWhenConfused(RoleInGame player, RoleInGame boss, Car car, interfaceOfHM.ContactInterface ci, GetRandomPos grp, ref List<string> notifyMsg, out MileResultReason Mrr)
        {
            var randomValue = that.rm.Next(0, 5);
            if (randomValue == 4)
            {
                return contactPassBossAddress(player, boss, car, ci, grp, ref notifyMsg, out Mrr);
            }
            else
            {
                var victim = that._Players[ci.targetOwner];
                var target = Program.dt.GetFpByIndex(victim.StartFPIndex);
                var collectIndexTarget = that.getCollectPositionsByDistance(target, Program.dt)[randomValue];
                var sc = new SetCollect()
                {
                    c = "SetCollect",
                    collectIndex = collectIndexTarget,
                    cType = "findWork",
                    fastenpositionID = Program.dt.GetFpByIndex(that._collectPosition[collectIndexTarget]).FastenPositionID,
                    Key = player.Key
                };
                return that.collectE.collectPassBossAddress(player, boss, car, sc, grp, ref notifyMsg, out Mrr);
            }
        }

        public RoomMainF.RoomMain.commandWithTime.ReturningOjb contact(RoleInGame player, Car car, interfaceOfHM.ContactInterface ci, GetRandomPos grp, ref List<string> notifyMsg, out MileResultReason Mrr)
        {
            //sa = attackTargetChange(player, sa);
            RoleInGame boss;
            if (player.HasTheBoss(roomMain._Players, out boss))
            {
                if (player.confuseRecord.IsBeingControlled())
                {
                    if (player.confuseRecord.getControlType() == Manager_Driver.ConfuseManger.ControlAttackType.Confuse)
                    {
                        return randomWhenConfused(player, boss, car, ci, grp, ref notifyMsg, out Mrr);
                    }
                    else
                    {
                        return contactPassBossAddress(player, boss, car, ci, grp, ref notifyMsg, out Mrr);
                    }
                }
                else
                    return contactPassBossAddress(player, boss, car, ci, grp, ref notifyMsg, out Mrr);
                //return promotePassBossAddress(player, boss, car, sp, ref notifyMsg, out reason);
            }
            else
            {
                if (that._Players.ContainsKey(ci.targetOwner))
                {
                    //beneficiary 
                    var victimOrBeneficiary = that._Players[ci.targetOwner];
                    OssModel.FastonPosition fpResult;
                    var distanceIsEnoughToStart = that.theNearestToPlayerIsCarNotMoney(player, car, victimOrBeneficiary, Program.dt, out fpResult);
                    if (distanceIsEnoughToStart)
                        if (ci.carLeftConditions(car))
                        {
                            var from = this.getFromWhenAction(player, car);
                            var to = ci.target;
                            var fp1 = Program.dt.GetFpByIndex(from);
                            //var fp2 = Program.dt.GetFpByIndex(to);
                            //var baseFp = Program.dt.GetFpByIndex(player.StartFPIndex);
                            var goPath = that.GetAFromB_v2(from, to, player, grp, ref notifyMsg);
                            //var returnPath = Program.dt.GetAFromB(fp2, baseFp.FastenPositionID);
                            //var returnPath = Program.dt.GetAFromB(to, player.StartFPIndex);
                            var returnPath = that.GetAFromB_v2(to, player.StartFPIndex, player, grp, ref notifyMsg);

                            var goMile = that.GetMile(goPath);
                            var returnMile = that.GetMile(returnPath);



                            //第一步，计算去程和回程。
                            if (car.ability.leftMile >= goMile + returnMile)
                            {
                                int startT;
                                this.EditCarStateWhenActionStartOK(player, ref car, to, fp1, goPath, grp, ref notifyMsg, out startT);
                                ci.SetArrivalThread(startT, car, goMile, goPath, commandWithTime.ReturningOjb.ojbWithoutBoss(returnPath));

                                DealWithDirectAttack(ref car, ref player, ref victimOrBeneficiary, grp);
                                 
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
                        that.ViewPosition(player, fpResult, ref notifyMsg);
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
