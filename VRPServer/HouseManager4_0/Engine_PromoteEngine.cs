using CommonClass;
using HouseManager4_0.RoomMainF;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using System;
using System.Collections.Generic;
using static HouseManager4_0.Car;
using static HouseManager4_0.RoomMainF.RoomMain;
using OssModel = Model;

namespace HouseManager4_0
{
    public class Engine_PromoteEngine : Engine, interfaceOfEngine.engine, interfaceOfEngine.tryCatchAction
    {
        public Engine_PromoteEngine(RoomMain roomMain)
        {
            this.roomMain = roomMain;
        }

        public bool carAbilitConditionsOk(RoleInGame player, Car car, Command c, GetRandomPos grp)
        {
            if (car.ability.diamondInCar == "")
            {
                return car.state == CarState.waitAtBaseStation || car.state == CarState.waitOnRoad;
            }
            else
            {
                return false;
            }
        }

        public bool conditionsOk(Command c, GetRandomPos grp, out string reason)
        {
            if (c.c == "SetPromote")
            {
                var sp = (SetPromote)c;
                if (string.IsNullOrEmpty(sp.pType))
                {
                    reason = $"wrong pType:{sp.pType}";
                    return false;
                }
                else if (!(sp.pType == "mile" || sp.pType == "business" || sp.pType == "volume" || sp.pType == "speed"))
                {
                    reason = $"wrong pType:{sp.pType}";
                    return false;
                }
                else
                {
                    reason = "";
                    return true;
                }
            }
            else
            {
                reason = $"wrong c.c:{c.c}";
                return false;
            }

        }

        internal string updatePromote(SetPromote sp, GetRandomPos grp)
        {
            return this.updateAction(this, sp, grp, sp.Key);
        }

        public void failedThenDo(Car car, RoleInGame player, Command c, ref List<string> notifyMsg)
        {
            if (c.c == "SetPromote")
            {
                // SetAttack sa = (SetAttack)c;
                this.carDoActionFailedThenMustReturn(car, player, ref notifyMsg);
                //this.carsAttackFailedThenMustReturn(car, player, sa, ref notifyMsg);
            }
            //throw new NotImplementedException();
        }

        public commandWithTime.ReturningOjb maindDo(RoleInGame player, Car car, Command c, GetRandomPos grp, ref List<string> notifyMsg, out MileResultReason mrr)
        {
            // throw new NotImplementedException();
            if (c.c == "SetPromote")
            {
                var sp = (SetPromote)c;
                return this.promote(player, car, sp, grp, ref notifyMsg, out mrr);
            }
            else
            {
                throw new Exception($"数据传输错误！(传出类型为{c.c})");
            }
        }
        //class PromoteObj : interfaceOfHM.ContactInterface
        //{
        //    public delegate void SetPromoteArrivalThreadM(int startT, Car car, SetPromote sp, int goMile, Node goPath, commandWithTime.ReturningOjb ro);
        //    private SetPromote _sp;

        //    SetPromoteArrivalThreadM _setPromoteArrivalThread;
        //    public PromoteObj(SetPromote sp, SetPromoteArrivalThreadM spthread)
        //    {
        //        this._sp = sp;
        //        this._setPromoteArrivalThread = spthread;
        //    }

        //    public string targetOwner
        //    {
        //        get { return ""; }
        //        // get { return this._sp.ta}
        //    }

        //    public int target => throw new NotImplementedException();

        //    public bool carLeftConditions(Car car)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public void SetArrivalThread(int startT, Car car, int goMile, Node goPath, commandWithTime.ReturningOjb returningOjb)
        //    {
        //        throw new NotImplementedException();
        //    }
        //}
        private commandWithTime.ReturningOjb promote(RoleInGame player, Car car, SetPromote sp, GetRandomPos grp, ref List<string> notifyMsg, out MileResultReason mrr)
        {

            RoleInGame boss;
            if (player.HasTheBoss(roomMain._Players, out boss))
            {
                return promotePassBossAddress(player, boss, car, sp, grp, ref notifyMsg, out mrr);
                //return promotePassBossAddress(player, boss, car, sp, ref notifyMsg, out reason);
            }
            else
            {
                //if(sp.pType=="mi")
                switch (sp.pType)
                {
                    case "mile":
                    case "business":
                    case "volume":
                    case "speed":
                        {
                            switch (car.state)
                            {
                                case CarState.waitAtBaseStation:
                                    {
                                        // if(player.Money<)
                                        OssModel.FastonPosition fpResult;
                                        var distanceIsEnoughToStart = that.theNearestToDiamondIsCarNotMoney(player, car, sp.pType, Program.dt, out fpResult);
                                        if (distanceIsEnoughToStart)
                                        {
                                            var from = this.getFromWhenAction(player, car);
                                            var to = that.GetPromotePositionTo(sp.pType);//  this.promoteMilePosition;

                                            var fp1 = Program.dt.GetFpByIndex(from);
                                            var fp2 = Program.dt.GetFpByIndex(to);
                                            var baseFp = Program.dt.GetFpByIndex(player.StartFPIndex);
                                            var goPath = that.GetAFromB_v2(from, to, player, grp, ref notifyMsg);
                                            // var returnPath = Program.dt.GetAFromB(to, player.StartFPIndex);
                                            var returnPath = that.GetAFromB_v2(to, player.StartFPIndex, player, grp, ref notifyMsg);

                                            var goMile = that.GetMile(goPath);
                                            var returnMile = that.GetMile(returnPath);


                                            //第一步，计算去程和回程。
                                            if (car.ability.leftMile >= goMile + returnMile)
                                            {
                                                int startT;
                                                this.EditCarStateWhenActionStartOK(player, ref car, to, fp1, goPath, grp, ref notifyMsg, out startT);

                                                RoomMainF.RoomMain.commandWithTime.ReturningOjb ro = commandWithTime.ReturningOjb.ojbWithoutBoss(returnPath);
                                                that.diamondOwnerE.StartDiamondOwnerThread(startT, 0, player, car, sp, ro, goMile, goPath);
                                                //  getAllCarInfomations(sp.Key, ref notifyMsg);
                                                mrr = MileResultReason.Abundant;
                                                return ro;
                                            }

                                            else if (car.ability.leftMile >= goMile)
                                            {
                                                WebNotify(player, $"去程{goMile}km，回程{returnMile}km,去了回不来");
                                                mrr = MileResultReason.CanNotReturn;
                                                return player.returningOjb;
                                            }
                                            else
                                            {
                                                WebNotify(player, $"去程{goMile}km，回程{returnMile}km,去不了");
                                                // printState(player, car, $"去程{goMile}，回程{returnMile},去不了");
                                                mrr = MileResultReason.CanNotReach;
                                                return player.returningOjb;
                                            }
                                        }
                                        else
                                        {
                                            mrr = MileResultReason.NearestIsMoneyWhenPromote;
                                            this.WebNotify(player, $"离宝石最近的是[{fpResult.FastenPositionName}]处的钱，不是你的车。请离宝石再近点儿！");
                                            return player.returningOjb;
                                        }
                                    };
                                case CarState.waitOnRoad:
                                    {
                                        OssModel.FastonPosition fpResult;
                                        var distanceIsEnoughToStart = that.theNearestToDiamondIsCarNotMoney(player, car, sp.pType, Program.dt, out fpResult);
                                        if (distanceIsEnoughToStart)
                                        {
                                            var from = this.getFromWhenAction(player, car);
                                            var to = that.GetPromotePositionTo(sp.pType);//  this.promoteMilePosition;

                                            var fp1 = Program.dt.GetFpByIndex(from);
                                            var fp2 = Program.dt.GetFpByIndex(to);
                                            var baseFp = Program.dt.GetFpByIndex(player.StartFPIndex);
                                            var goPath = that.GetAFromB_v2(from, to, player, grp, ref notifyMsg);
                                            var returnPath = that.GetAFromB_v2(to, player.StartFPIndex, player, grp, ref notifyMsg);

                                            var goMile = that.GetMile(goPath);
                                            var returnMile = that.GetMile(returnPath);


                                            //第一步，计算去程和回程。
                                            if (car.ability.leftMile >= goMile + returnMile)
                                            {
                                                int startT;
                                                this.EditCarStateWhenActionStartOK(player, ref car, to, fp1, goPath, grp, ref notifyMsg, out startT);

                                                RoomMainF.RoomMain.commandWithTime.ReturningOjb ro = commandWithTime.ReturningOjb.ojbWithoutBoss(returnPath);
                                                that.diamondOwnerE.StartDiamondOwnerThread(startT, 0, player, car, sp, ro, goMile, goPath);
                                                mrr = MileResultReason.Abundant;
                                                return ro;
                                            }

                                            else if (car.ability.leftMile >= goMile)
                                            {
                                                WebNotify(player, $"去程{goMile}km，回程{returnMile}km,去了回不来");
                                                mrr = MileResultReason.CanNotReturn;
                                                return player.returningOjb;
                                            }
                                            else
                                            {
                                                WebNotify(player, $"去程{goMile}km，回程{returnMile}km,去不了");
                                                mrr = MileResultReason.CanNotReach;
                                                return player.returningOjb;
                                            }
                                        }
                                        else
                                        {
                                            mrr = MileResultReason.NearestIsMoneyWhenPromote;
                                            this.WebNotify(player, $"离宝石最近的是[{fpResult.FastenPositionName}]处的钱，不是你的车。请离宝石再近点儿！");
                                            return player.returningOjb;
                                        }
                                    };
                                default:
                                    {
                                        throw new Exception($"{Enum.GetName(typeof(CarState), car.state)}不是注册的类型！");
                                    }
                            }

                        };
                    default:
                        {
                            throw new Exception($"{sp.pType}-不是规定的输入！");
                        };
                }
            }
        }

        private commandWithTime.ReturningOjb promotePassBossAddress(RoleInGame player, RoleInGame boss, Car car, SetPromote sp, GetRandomPos grp, ref List<string> notifyMsg, out MileResultReason mrr)
        {
            switch (sp.pType)
            {
                case "mile":
                case "business":
                case "volume":
                case "speed":
                    {
                        switch (car.state)
                        {
                            case CarState.waitAtBaseStation:
                                {
                                    // if(player.Money<)
                                    //int collectIndex;
                                    OssModel.FastonPosition fpResult;
                                    var distanceIsEnoughToStart = that.theNearestToDiamondIsCarNotMoney(player, car, sp.pType, Program.dt, out fpResult);
                                    if (distanceIsEnoughToStart)
                                    {
                                        var from = this.getFromWhenAction(player, car);
                                        var to = that.GetPromotePositionTo(sp.pType);//  this.promoteMilePosition;

                                        var fp1 = Program.dt.GetFpByIndex(from);
                                        var fp2 = Program.dt.GetFpByIndex(to);
                                        var baseFp = Program.dt.GetFpByIndex(player.StartFPIndex);
                                        var goPath = that.GetAFromB_v2(from, to, player, grp, ref notifyMsg);
                                        // var returnPath = Program.dt.GetAFromB(to, player.StartFPIndex);
                                        var returnToBossPath = that.GetAFromB_v2(to, boss.StartFPIndex, player, grp, ref notifyMsg);
                                        var returnToSelfPath = that.GetAFromB_v2(boss.StartFPIndex, player.StartFPIndex, player, grp, ref notifyMsg);

                                        var goMile = that.GetMile(goPath);
                                        var returnToBossMile = that.GetMile(returnToBossPath);
                                        var returnToSelfMile = that.GetMile(returnToSelfPath);

                                        //第一步，计算去程和回程。
                                        if (car.ability.leftMile >= goMile + returnToBossMile + returnToSelfMile)
                                        {
                                            int startT;
                                            this.EditCarStateWhenActionStartOK(player, ref car, to, fp1, goPath, grp, ref notifyMsg, out startT);
                                            RoomMainF.RoomMain.commandWithTime.ReturningOjb ro = commandWithTime.ReturningOjb.ojbWithBoss(returnToBossPath, returnToSelfPath, boss);
                                            that.diamondOwnerE.StartDiamondOwnerThread(startT, 0, player, car, sp, ro, goMile, goPath);
                                            //  getAllCarInfomations(sp.Key, ref notifyMsg);
                                            mrr = MileResultReason.Abundant;
                                            return ro;
                                        }

                                        else if (car.ability.leftMile >= goMile)
                                        {
                                            WebNotify(player, $"去程{goMile}km，回程{returnToBossMile + returnToSelfMile}km,去了回不来");
                                            mrr = MileResultReason.CanNotReturn;
                                            return player.returningOjb;
                                        }
                                        else
                                        {
                                            WebNotify(player, $"去程{goMile}km，回程{returnToBossMile + returnToSelfMile}km,去不了");
                                            // printState(player, car, $"去程{goMile}，回程{returnMile},去不了");
                                            mrr = MileResultReason.CanNotReach;
                                            return player.returningOjb;
                                        }
                                    }
                                    else
                                    {
                                        mrr = MileResultReason.NearestIsMoneyWhenPromote;
                                        this.WebNotify(player, $"离宝石最近的是[{fpResult.FastenPositionName}]处的钱，不是你的车。请离宝石再近点儿！");
                                        return player.returningOjb;
                                        //printState(player, car, "钱不够了,由于本身待在基地，不用返回。");
                                    }
                                };
                            case CarState.waitOnRoad:
                                {
                                    OssModel.FastonPosition fpResult;
                                    var distanceIsEnoughToStart = that.theNearestToDiamondIsCarNotMoney(player, car, sp.pType, Program.dt, out fpResult);
                                    if (distanceIsEnoughToStart)
                                    {
                                        var from = this.getFromWhenAction(player, car);
                                        var to = that.GetPromotePositionTo(sp.pType);//  this.promoteMilePosition;

                                        var fp1 = Program.dt.GetFpByIndex(from);
                                        var fp2 = Program.dt.GetFpByIndex(to);
                                        var baseFp = Program.dt.GetFpByIndex(player.StartFPIndex);
                                        var goPath = that.GetAFromB_v2(from, to, player, grp, ref notifyMsg);

                                        var returnToBossPath = that.GetAFromB_v2(to, boss.StartFPIndex, player, grp, ref notifyMsg);
                                        var returnToSelfPath = that.GetAFromB_v2(boss.StartFPIndex, player.StartFPIndex, player, grp, ref notifyMsg);

                                        var goMile = that.GetMile(goPath);
                                        var returnToBossMile = that.GetMile(returnToBossPath);
                                        var returnToSelfMile = that.GetMile(returnToSelfPath);


                                        //第一步，计算去程和回程。
                                        if (car.ability.leftMile >= goMile + returnToBossMile + returnToSelfMile)
                                        {
                                            int startT;
                                            this.EditCarStateWhenActionStartOK(player, ref car, to, fp1, goPath, grp, ref notifyMsg, out startT);

                                            RoomMainF.RoomMain.commandWithTime.ReturningOjb ro = commandWithTime.ReturningOjb.ojbWithBoss(returnToBossPath, returnToSelfPath, boss);
                                            that.diamondOwnerE.StartDiamondOwnerThread(startT, 0, player, car, sp, ro, goMile, goPath);
                                            mrr = MileResultReason.Abundant;
                                            return ro;
                                        }

                                        else if (car.ability.leftMile >= goMile)
                                        {
                                            WebNotify(player, $"去程{goMile}km，回程{returnToBossMile + returnToSelfMile}km,去了回不来");
                                            mrr = MileResultReason.CanNotReturn;
                                            return player.returningOjb;
                                        }
                                        else
                                        {
                                            WebNotify(player, $"去程{goMile}km，回程{returnToBossMile + returnToSelfMile}km,去不了");
                                            mrr = MileResultReason.CanNotReach;
                                            return player.returningOjb;
                                        }
                                    }
                                    else
                                    {
                                        mrr = MileResultReason.NearestIsMoneyWhenPromote;
                                        this.WebNotify(player, $"离宝石最近的是[{fpResult.FastenPositionName}]处的钱，不是你的车。请离宝石再近点儿！");
                                        return player.returningOjb;
                                    }
                                };
                            default:
                                {
                                    throw new Exception($"{Enum.GetName(typeof(CarState), car.state)}不是注册的类型！");
                                }
                        }
                    };
                default:
                    {
                        throw new Exception($"{sp.pType}-不是规定的输入！");
                    };
            }
        }

        //        public string updatePromote(SetPromote sp)
        //        {
        //            return this.promoteE.updatePromote(sp);
        //            if (string.IsNullOrEmpty(sp.pType))
        //            {
        //                return $"wrong pType:{sp.pType}";
        //            }
        //            else if (!(sp.pType == "mile" || sp.pType == "business" || sp.pType == "volume" || sp.pType == "speed"))
        //            {
        //                return $"wrong pType:{sp.pType}"; ;
        //            }
        //            else
        //            {
        //                if (this._Players.ContainsKey(sp.Key))
        //                {
        //                    List<string> notifyMsg = new List<string>();
        //                    var player = this._Players[sp.Key];
        //                    var car = player.getCar();
        //                    if (player.Bust)
        //                    {
        //                        WebNotify(player, "您已破产");
        //                        return $"{player.Key} go bust!";
        //#warning 这里要提示前台，已经进行破产清算了。
        //                    }
        //                    else
        //                    {
        //                        lock (this.PlayerLock)
        //                        {
        //                            if (this._Players.ContainsKey(sp.Key))
        //                            {
        //                                //if(sp.pType=="mi")
        //                                switch (sp.pType)
        //                                {
        //                                    case "mile":
        //                                    case "business":
        //                                    case "volume":
        //                                    case "speed":
        //                                        {

        //                                            {
        //                                                {
        //                                                    switch (car.state)
        //                                                    {
        //                                                        case CarState.waitAtBaseStation:
        //                                                            {
        //                                                                // if(player.Money<)
        //                                                                var moneyIsEnoughToStart = giveMoneyFromPlayerToCarForPromoting(player, car, sp.pType, ref notifyMsg);

        //                                                                if (moneyIsEnoughToStart)
        //                                                                {
        //                                                                    MileResultReason reason;
        //                                                                    var hasBeginToPromote = promote(player, car, sp, ref notifyMsg, out reason);
        //                                                                    if (hasBeginToPromote)
        //                                                                    {
        //                                                                        WebNotify(player, $"car-在路上寻找能力宝石的路上了！！！");
        //                                                                    }
        //                                                                    else
        //                                                                    {
        //                                                                        if (reason == MileResultReason.CanNotReach)
        //                                                                        {
        //                                                                            WebNotify(player, $"您车的剩余里程不足以支持到达目的地！");
        //                                                                            //WebNotify(player, $"资金不够,{car.name}未能上路！！！");
        //                                                                        }
        //                                                                        else if (reason == MileResultReason.CanNotReturn)
        //                                                                        {
        //                                                                            WebNotify(player, $"到达目的地后，您车的剩余里程不足以支持返回！");
        //                                                                        }

        //                                                                        giveMoneyFromCarToPlayer(player, car, ref notifyMsg);
        //                                                                    }
        //                                                                }
        //                                                                else
        //                                                                {
        //                                                                    WebNotify(player, $"钱不够了,由于本身待在基地，不用返回");
        //                                                                    //printState(player, car, "钱不够了,由于本身待在基地，不用返回。");
        //                                                                }
        //                                                            }; break;
        //                                                        case CarState.waitOnRoad:
        //                                                            {
        //                                                                if (car.ability.diamondInCar == "")
        //                                                                {
        //                                                                    if (car.ability.SumMoneyCanForPromote >= this.promotePrice[sp.pType])
        //                                                                    {
        //                                                                        MileResultReason reason;
        //                                                                        var hasBeginToPromote = promote(player, car, sp, ref notifyMsg, out reason);
        //                                                                        if (hasBeginToPromote)
        //                                                                        {
        //                                                                            WebNotify(player, $"您的车已经在寻找能力宝石的路上了！！！");
        //                                                                            // printState(player, car, $"已经在收集{sp.pType}宝石的路上了！");
        //                                                                        }
        //                                                                        else
        //                                                                        {
        //                                                                            if (reason == MileResultReason.CanNotReach)
        //                                                                            {
        //                                                                                WebNotify(player, $"您车的剩余里程不足以支持到达目的地！");
        //                                                                                //WebNotify(player, $"资金不够,{car.name}未能上路！！！");
        //                                                                            }
        //                                                                            else if (reason == MileResultReason.CanNotReturn)
        //                                                                            {
        //                                                                                WebNotify(player, $"到达目的地后，您车的剩余里程不足以支持返回！");
        //                                                                            }
        //                                                                            // printState(player, car, "收集宝石剩余里程不足，必须立即返回！");
        //                                                                            setReturnWhenPromoteFailed(sp, car);
        //                                                                        }
        //                                                                    }
        //                                                                    else
        //                                                                    {
        //                                                                        WebNotify(player, $"资金不够,您的车被安排返航！！！");
        //                                                                        printState(player, car, "在路上走的车，想找宝石，钱不够啊，必须立即返回！");
        //                                                                        //Consol.WriteLine($"宝石的价格{this.promotePrice[sp.pType]}，钱不够啊,{car.ability.costBusiness},{car.ability.costVolume}！");
        //#warning 在路上，由于资金不够，这里没有能测到。
        //                                                                        setReturnWhenPromoteFailed(sp, car);
        //                                                                    }
        //                                                                }
        //                                                                else
        //                                                                {
        //                                                                    //Consol.WriteLine("在路上走的车，有了宝石，居然没返回！");
        //                                                                    //throw new Exception();
        //                                                                }
        //                                                            }; break;
        //                                                        case CarState.working: { }; break;
        //                                                        case CarState.returning: { }; break;
        //                                                        default:
        //                                                            {
        //                                                                var msg = $"{car.state.ToString()}状态下不能提升能力！";
        //                                                                //Consol.WriteLine(msg);
        //                                                            }; break;

        //                                                    }
        //                                                }; break;
        //                                            }
        //                                        }; break;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                return $"not has player-{sp.Key}!";
        //                            }
        //                        }
        //                    }
        //                    for (var i = 0; i < notifyMsg.Count; i += 2)
        //                    {
        //                        var url = notifyMsg[i];
        //                        var sendMsg = notifyMsg[i + 1];
        //                        Startup.sendMsg(url, sendMsg);
        //                    }
        //                }

        //                return "ok";
        //            }
        //        }
    }
}
