using CommonClass;
using HouseManager4_0.RoomMainF;
using Microsoft.AspNetCore.Server.IIS.Core;
using System;
using System.Collections.Generic;
using static HouseManager4_0.Car;
using static HouseManager4_0.RoomMainF.RoomMain;

namespace HouseManager4_0
{
    public class Engine_CollectEngine : Engine, interfaceOfEngine.tryCatchAction, interfaceOfEngine.startNewThread
    {
        public Engine_CollectEngine(RoomMain roomMain)
        {
            this.roomMain = roomMain;
        }

        public bool carAbilitConditionsOk(RoleInGame player, Car car, Command c, GetRandomPos grp)
        {
            if (c.c == "SetCollect")
                if (car.ability.leftVolume > 0)
                {
                    SetCollect sc = (SetCollect)c;
                    if (grp.GetFpByIndex(that._collectPosition[sc.collectIndex]).FastenPositionID == sc.fastenpositionID)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    this.WebNotify(player, "小车已经没有多余手机容量了！");
                    return false;
                }
            else
            {
                return false;
            }
        }

        public bool conditionsOk(Command c, GetRandomPos grp, out string reason)
        {
            if (c.c == "SetCollect")
            {
                SetCollect sc = (SetCollect)c;
                if (string.IsNullOrEmpty(sc.cType))
                {
                    reason = "";
                    return false;
                }
                else if (!(sc.cType == "findWork"))
                {
                    reason = "";
                    return false;
                }
                else if (string.IsNullOrEmpty(sc.fastenpositionID))
                {
                    reason = "";
                    return false;
                }
                else if (!CityRunFunction.FormatLike.LikeFsPresentCode(sc.fastenpositionID))
                {
                    reason = "";
                    return false;
                }
                else if (sc.collectIndex < 0 || sc.collectIndex >= 38)
                {
                    reason = "";
                    return false;
                }
                else if (grp.GetFpByIndex(that._collectPosition[sc.collectIndex]).FastenPositionID != sc.fastenpositionID)
                {
                    reason = "";
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
                reason = "";
                return false;
            }
        }

        public void failedThenDo(Car car, RoleInGame player, Command c, GetRandomPos grp, ref List<string> notifyMsg)
        {
            if (c.c == "SetCollect")
                this.carDoActionFailedThenMustReturn(car, player, grp, ref notifyMsg);
        }

        public commandWithTime.ReturningOjb maindDo(RoleInGame player, Car car, Command c, GetRandomPos grp, ref List<string> notifyMsg, out MileResultReason mrr)
        {
            if (c.c == "SetCollect")
            {
                var sc = (SetCollect)c;
                return this.collect(player, car, sc, grp, ref notifyMsg, out mrr);
            }
            else
            {
                throw new Exception($"数据传输错误！(传出类型为{c.c})");
            }
        }

        internal string updateCollect(SetCollect sc, GetRandomPos grp)
        {
            return this.updateAction(this, sc, grp, sc.Key);
        }

        RoomMainF.RoomMain.commandWithTime.ReturningOjb collect(RoleInGame player, Car car, SetCollect sc, GetRandomPos grp, ref List<string> notifyMsg, out MileResultReason Mrr)
        {
            if (player.confuseRecord.IsBeingControlled())
            {
                if (player.confuseRecord.getControlType() == Manager_Driver.ConfuseManger.ControlAttackType.Lost)
                {
                    Model.FastonPosition target;
                    if (car.state == CarState.waitOnRoad)
                    {
                        target = grp.GetFpByIndex(car.targetFpIndex);

                    }
                    else if (car.state == CarState.waitAtBaseStation)
                    {
                        target = grp.GetFpByIndex(player.StartFPIndex);

                        // that.magicE.TakeHalfMoneyWhenIsControlled(player, car, ref notifyMsg);
                    }
                    else
                    {
                        throw new Exception("");
                    }
                    var positions = that.getCollectPositionsByDistance(target, Program.dt);
                    if (sc.collectIndex == positions[0] || sc.collectIndex == positions[1])
                    {

                    }
                    else
                    {
                        var position0 = grp.GetFpByIndex(that._collectPosition[positions[0]]);
                        var position1 = grp.GetFpByIndex(that._collectPosition[positions[1]]);
                        var distance0 = CommonClass.Geography.getLengthOfTwoPoint.GetDistance(target.Latitde, target.Longitude, target.Height, position0.Latitde, position0.Longitude, position0.Height);
                        var distance1 = CommonClass.Geography.getLengthOfTwoPoint.GetDistance(target.Latitde, target.Longitude, target.Height, position1.Latitde, position1.Longitude, position1.Height);
                        if (distance0 < distance1)
                        {
                            this.WebNotify(player, $"您处于迷失状态，未能到达远方！只能先到附近的[{position0.FastenPositionName}]执行收集任务！");
                            var newSc = new SetCollect()
                            {
                                c = "SetCollect",
                                collectIndex = positions[0],
                                fastenpositionID = position0.FastenPositionID,
                                cType = "findWork",
                                Key = sc.Key
                            };
                            return collect(player, car, newSc, grp, ref notifyMsg, out Mrr);
                        }
                        else
                        {
                            this.WebNotify(player, $"您处于迷失状态，未能到达远方！只能先到附近的[{position1.FastenPositionName}]执行收集任务！");
                            var newSc = new SetCollect()
                            {
                                c = "SetCollect",
                                collectIndex = positions[1],
                                fastenpositionID = position1.FastenPositionID,
                                cType = "findWork",
                                Key = sc.Key
                            };
                            return collect(player, car, newSc, grp, ref notifyMsg, out Mrr);
                        }
                    }
                    //var target = Program.dt.GetFpByIndex(victim.StartFPIndex);
                    //var collectIndexTarget = that.getCollectPositionsByDistance(target)[randomValue];
                }

                //if (car.state == CarState.waitAtBaseStation)
                //{
                //    // that.magicE.TakeMoneyWhenIsControlled(player,car,);
                //}
            }
            RoleInGame boss;
            if (player.HasTheBoss(roomMain._Players, out boss))
            {
                return collectPassBossAddress(player, boss, car, sc, grp, ref notifyMsg, out Mrr);
                //return promotePassBossAddress(player, boss, car, sp, ref notifyMsg, out reason);
            }
            else
            {
                if (car.ability.leftVolume > 0)
                {
                    var from = this.GetFromWhenUpdateCollect(that._Players[sc.Key], sc.cType, car);
                    var to = getCollectPositionTo(sc.collectIndex);//  this.promoteMilePosition;
                    var fp1 = grp.GetFpByIndex(from);
                    //var fp2 = grp.GetFpByIndex(to);
                    //var fbBase = grp.GetFpByIndex(player.StartFPIndex);
                    //var goPath = Program.dt.GetAFromB(fp1, fp2.FastenPositionID);
                    //var goPath = Program.dt.GetAFromB(from, to);
                    var goPath = that.GetAFromB_v2(from, to, player, grp, ref notifyMsg);

                    //var returnPath = Program.dt.GetAFromB(to, player.StartFPIndex);
                    var returnPath = that.GetAFromB_v2(to, player.StartFPIndex, player, grp, ref notifyMsg);
                    var goMile = that.GetMile(goPath);
                    var returnMile = that.GetMile(returnPath);
                    if (car.ability.leftMile >= goMile + returnMile || IsNPCsFirstCollect(player))
                    {
                        int startT;
                        this.EditCarStateWhenActionStartOK(player, ref car, to, fp1, goPath, grp, ref notifyMsg, out startT);
                        var ro = commandWithTime.ReturningOjb.ojbWithoutBoss(returnPath);
                        //  Thread th=new Thread() { }
                        car.setState(player, ref notifyMsg, CarState.working);
                        StartArriavalThread(startT, 0, player, car, sc, ro, goMile, goPath, grp);
                        //if (player.playerType == RoleInGame.PlayerType.NPC)
                        //    StartArriavalThread(startT, car, sc, ro, goMile);
                        //else
                        //    StartArriavalThread(startT, car, sc, ro, goMile);
                        //   StartSelectThread(0, startT, car, sc, ro, goMile, goPath);
                        //  getAllCarInfomations(sc.Key, ref notifyMsg);
                        Mrr = MileResultReason.Abundant;//返回原因
                        return ro;
                    }

                    else if (car.ability.leftMile >= goMile)
                    {
                        //  printState(player, car, $"剩余里程为{car.ability.leftMile}去程{goMile}，回程{returnMile},你去了回不来。所以安排返回");
                        Mrr = MileResultReason.CanNotReturn;
                        return player.returningOjb;
                    }
                    else
                    {
                        // printState(player, car, $"剩余里程为{car.ability.leftMile}去程{goMile}，回程{returnMile},去不了。所以安排返回");
                        Mrr = MileResultReason.CanNotReach;
                        return player.returningOjb;
                        //   return false;
                    }
                }
                else
                {
                    Mrr = MileResultReason.MoneyIsNotEnougt;
                    this.WebNotify(player, "你身上的剩余收集空间不够啦！");
                    return player.returningOjb;
                }
            }
        }
        // 这种状态是为了防止NPC陷入死循环，就是NPC距离不够，也能保持收集
        private bool IsNPCsFirstCollect(RoleInGame player)
        {
            return player.playerType == RoleInGame.PlayerType.NPC && player.getCar().state == CarState.waitAtBaseStation;
        }



        //private void StartArriavalThread(int startT, Car car, SetCollect sc, commandWithTime.ReturningOjb ro, int goMile)
        //{
        //    this.startNewThread(startT + 100, new commandWithTime.placeArriving()
        //    {
        //        c = "placeArriving",
        //        key = sc.Key,
        //        //car = sc.car,
        //        returningOjb = ro,
        //        target = car.targetFpIndex,
        //        costMile = goMile
        //    }, this);
        //    //Thread th = new Thread(() => setArrive(startT, ));
        //    //th.Start();
        //}
        private void StartArriavalThread(int startT, int step, RoleInGame player, Car car, SetCollect sc, commandWithTime.ReturningOjb ro, int goMile, Node goPath, GetRandomPos grp)
        {
            System.Threading.Thread th = new System.Threading.Thread(() =>
            {
                if (step >= goPath.path.Count - 1)

                    this.startNewThread(startT + 100, new commandWithTime.placeArriving()
                    {
                        c = "placeArriving",
                        key = sc.Key,
                        //car = sc.car,
                        returningOjb = ro,
                        target = car.targetFpIndex,
                        costMile = goMile
                    }, this, grp);
                else
                {
                    Action p = () =>
                    {
                        List<string> notifyMsg = new List<string>();
                        int newStartT;
                        step++;
                        if (step < goPath.path.Count)
                            EditCarStateAfterSelect(step, player, ref car, ref notifyMsg, out newStartT);
                        else
                            throw new Exception("这种情况不会出现");

                        car.setState(player, ref notifyMsg, CarState.working);
                        this.sendSeveralMsgs(notifyMsg);
                        StartArriavalThread(newStartT, step, player, car, sc, ro, goMile, goPath, grp);
                    };
                    this.loop(p, step, startT, player, goPath);
                    //if (step == 0)
                    //{
                    //    this.ThreadSleep(startT + 50);
                    //    Action p = () =>
                    //    {
                    //        List<string> notifyMsg = new List<string>();
                    //        int newStartT;
                    //        step++;
                    //        if (step < goPath.path.Count) 
                    //            EditCarStateAfterSelect(step, player, ref car, ref notifyMsg, out newStartT);
                    //        else
                    //            throw new Exception("这种情况不会出现");

                    //        car.setState(player, ref notifyMsg, CarState.working);
                    //        this.sendMsg(notifyMsg);
                    //        StartArriavalThread(newStartT, step, player, car, sc, ro, goMile, goPath);
                    //    };
                    //    if (player.playerType == RoleInGame.PlayerType.NPC || player.Bust)
                    //    {
                    //        p();
                    //    }
                    //    else
                    //    {
                    //        StartSelectThreadA(goPath.path[step].selections, goPath.path[step].selectionCenter, (Player)player, p, goPath);
                    //    }


                    //}
                    //else
                    //{
                    //    this.ThreadSleep(startT);
                    //    Action p = () =>
                    //    {
                    //        step++;
                    //        List<string> notifyMsg = new List<string>();
                    //        int newStartT;
                    //        if (step < goPath.path.Count)
                    //            EditCarStateAfterSelect(step, player, ref car, ref notifyMsg, out newStartT);
                    //        // else if(step==goPath.path.Count-1)
                    //        //EditCarStateAfterSelect(step,player,ref car,)
                    //        else
                    //            throw new Exception("这种情况不会出现");
                    //        //newStartT = 0;
                    //        car.setState(player, ref notifyMsg, CarState.working);
                    //        this.sendMsg(notifyMsg);
                    //        StartArriavalThread(newStartT, step, player, car, sc, ro, goMile, goPath);

                    //    };
                    //    if (player.playerType == RoleInGame.PlayerType.NPC || player.Bust)
                    //    {
                    //        this.ThreadSleep(500);
                    //        p();
                    //    }
                    //    else if (startT != 0)
                    //    {
                    //        StartSelectThreadA(goPath.path[step].selections, goPath.path[step].selectionCenter, (Player)player, p, goPath);
                    //    }
                    //}
                }
            });
            th.Start();
            //Thread th = new Thread(() => setArrive(startT, ));
            //th.Start();
        }


        /// <summary>
        /// 到达某一地点。变更里程，进行collcet交易。
        /// </summary>
        /// <param name="startT"></param>
        /// <param name="pa"></param>
        private void setArrive(commandWithTime.placeArriving pa)
        {
            /*
             * 到达地点某地点时，说明汽车在这个地点待命。
             */

            List<string> notifyMsg = new List<string>();
            bool needUpdateCollectState = false;
            lock (that.PlayerLock)
            {
                var player = that._Players[pa.key];
                player.canGetReward = true;
                var car = that._Players[pa.key].getCar();
                if (car.state == CarState.working)
                {
                    arriveThenDoCollect(ref player, ref car, pa, ref notifyMsg, out needUpdateCollectState);

                }

            }
            this.sendSeveralMsgs(notifyMsg); 
            if (needUpdateCollectState)
            {
                that.CheckAllPlayersCollectState();
            }
        }

        private void arriveThenDoCollect(ref RoleInGame role, ref Car car, commandWithTime.placeArriving pa, ref List<string> notifyMsg, out bool needUpdateCollectState)
        {
            needUpdateCollectState = false;
            if (car.targetFpIndex == -1)
            {
                throw new Exception("这个地点应该是等待的地点！");
            }
            if (that._collectPosition.ContainsValue(pa.target))
            {


                int taxPostion = pa.target;
                //拿到钱的单位是分！
                long collectReWard = getCollectReWardByReward(pa.target);//依据target来判断应该收入多少！
                if (role.playerType == RoleInGame.PlayerType.player)
                {
                    //  that.NPCM.Moleste((Player)role, pa.target, ref notifyMsg);
                }

                long sumCollect = collectReWard; //DealWithTheFrequcy(this.CollectReWard);
                var selfGet = sumCollect;
                //  long sumDebet = 0;
                car.ability.setCostVolume(car.ability.costVolume + selfGet, role, car, ref notifyMsg);

                this.setCollectPosition(pa.target);
                //  this.collectPosition = this.GetRandomPosition(true);
                needUpdateCollectState = true;

                if (role.playerType == RoleInGame.PlayerType.player)
                {
                    that.GetMusic((Player)role, ref notifyMsg);
                    that.GetBackground((Player)role, ref notifyMsg);
                    ((Player)role).RefererCount++;
                    ((Player)role).ActiveTime = DateTime.Now;
                }
            }
            else
            {
            }
            //收集完，留在原地。
            //var car = this._Players[cmp.key].getCar(cmp.car);
            // car.ability.costMiles += pa.costMile;//

            var newCostMile = car.ability.costMiles + pa.costMile;
            car.ability.setCostMiles(newCostMile, role, car, ref notifyMsg);
            // AbilityChanged(player, car, ref notifyMsg, "mile");


            carParkOnRoad(pa.target, ref car, role, ref notifyMsg);

            //在这个方法里，会安排NPC进行下一步工作。
            car.setState(role, ref notifyMsg, CarState.waitOnRoad);
            that._Players[pa.key].returningOjb = pa.returningOjb;
            that._Players[pa.key].canGetReward = true;

            if (role.playerType == RoleInGame.PlayerType.player)
            {
                that.frequencyM.addFrequencyRecord();
            }
            {
                if (role.playerType == RoleInGame.PlayerType.player)
                {
                    that.goodsM.ShowConnectionModels(role, pa.target, ref notifyMsg);
                }
                else if (role.playerType == RoleInGame.PlayerType.NPC)
                {
                    //that.modelM
                    that.modelM.GetRewardFromBuildingByNPC((NPC)role);
                }
            }
            if (role.playerType == RoleInGame.PlayerType.player)
            {
                var player = (Player)role;
                if (string.IsNullOrEmpty(player.BTCAddress))
                {
                    this.WebNotify(player, "您还没有登录！");
                }
                else
                {
                    if (that.rm.Next(100) < player.SendTransmitMsg)
                    {
                        this.WebNotify(player, "转发，能获得转发奖励！");
                        player.SendTransmitMsg = player.SendTransmitMsg / 3;
                    }
                }
                //that.goodsM.ShowConnectionModels(role, pa.target, ref notifyMsg);
            }
        }

        private void setCollectPosition(int target)
        {
            int key = -1;
            foreach (var item in that._collectPosition)
            {
                if (item.Value == target)
                {
                    key = item.Key;
                    break;
                    //  return this.GetCollectReWard(item.Key) * 100;
                }
            }
            if (key != -1)
            {
                that._collectPosition[key] = that.GetRandomPosition(true, Program.dt);
            }
            // return 0;
        }
        private long getCollectReWardByReward(int target)
        {
            foreach (var item in that._collectPosition)
            {
                if (item.Value == target)
                {
                    return that.GetCollectReWard(item.Key) * 100;
                }
            }
            return 0;
            // throw new NotImplementedException();
        }




        /// <summary>
        /// 获取出发地点
        /// </summary>
        /// <param name="player"></param>
        /// <param name="cType"></param>
        /// <param name="car"></param>
        /// <returns></returns>
        private int GetFromWhenUpdateCollect(RoleInGame player, string cType, Car car)
        {
            switch (cType)
            {
                case "findWork":
                    {
                        switch (car.state)
                        {
                            case CarState.waitAtBaseStation:
                                {
                                    if (car.targetFpIndex != -1)
                                    {
                                        //出现这种情况，应该是回了基站里没有初始
                                        throw new Exception("参数混乱");
                                    }
                                    else
                                    {
                                        return player.StartFPIndex;
                                    }
                                };
                            case CarState.working:
                                {
                                    //出现这种情况，应该是回了基站里没有初始
                                    throw new Exception("参数混乱");
                                };
                            case CarState.waitOnRoad:
                                {
                                    if (car.targetFpIndex == -1)
                                    {
                                        throw new Exception("参数混乱");
                                    }
                                    else
                                    {
                                        return car.targetFpIndex;
                                    }
                                };
                            case CarState.returning:
                                {
                                    throw new Exception("参数混乱");
                                };
                        };
                    }; break;
            }
            throw new Exception("非法调用");
        }

        private int getCollectPositionTo(int collectIndex)
        {
            if (collectIndex >= 0 && collectIndex < 38)
            {
                return that._collectPosition[collectIndex];
            }
            else
                throw new Exception("parameter is wrong!");
        }
        public commandWithTime.ReturningOjb collectPassBossAddress(RoleInGame player, RoleInGame boss, Car car, SetCollect sc, GetRandomPos grp, ref List<string> notifyMsg, out MileResultReason mrr)
        {
            if (car.ability.leftVolume > 0)
            {
                var from = this.GetFromWhenUpdateCollect(that._Players[sc.Key], sc.cType, car);
                var to = getCollectPositionTo(sc.collectIndex);//  this.promoteMilePosition;
                                                               //  return ActionInBoss(from, to);
                var fp1 = Program.dt.GetFpByIndex(from);
                var fp2 = Program.dt.GetFpByIndex(to);
                var fbBase = Program.dt.GetFpByIndex(player.StartFPIndex);
                //var goPath = Program.dt.GetAFromB(fp1, fp2.FastenPositionID);
                //var goPath = Program.dt.GetAFromB(from, to);
                var goPath = that.GetAFromB_v2(from, to, player, grp, ref notifyMsg);
                var returnToBossPath = that.GetAFromB_v2(to, boss.StartFPIndex, player, grp, ref notifyMsg);
                var returnToSelfPath = that.GetAFromB_v2(boss.StartFPIndex, player.StartFPIndex, player, grp, ref notifyMsg);
                var goMile = that.GetMile(goPath);
                var returnToBossMile = that.GetMile(returnToBossPath);
                var returnToSelfMile = that.GetMile(returnToSelfPath);

                if (car.ability.leftMile >= goMile + returnToBossMile + returnToSelfMile || IsNPCsFirstCollect(player))
                {
                    that.magicE.TakeHalfMoneyWhenIsControlled(player, car, ref notifyMsg);
                    int startT;
                    this.EditCarStateWhenActionStartOK(player, ref car, to, fp1, goPath, grp, ref notifyMsg, out startT);
                    var ro = commandWithTime.ReturningOjb.ojbWithBoss(returnToBossPath, returnToSelfPath, boss);
                    car.setState(player, ref notifyMsg, CarState.working);
                    StartArriavalThread(startT, 0, player, car, sc, ro, goMile, goPath, grp);
                    //  getAllCarInfomations(sc.Key, ref notifyMsg);
                    mrr = MileResultReason.Abundant;//返回原因
                    return ro;
                }
                else if (car.ability.leftMile >= goMile)
                {
                    //  printState(player, car, $"剩余里程为{car.ability.leftMile}去程{goMile}，回程{returnMile},你去了回不来。所以安排返回");
                    mrr = MileResultReason.CanNotReturn;
                    return player.returningOjb;
                }
                else
                {
                    // printState(player, car, $"剩余里程为{car.ability.leftMile}去程{goMile}，回程{returnMile},去不了。所以安排返回");
                    mrr = MileResultReason.CanNotReach;
                    return player.returningOjb;
                    //   return false;
                }
            }
            else
            {
                mrr = MileResultReason.MoneyIsNotEnougt;
                this.WebNotify(player, "你身上的剩余收集空间不够啦！");
                return player.returningOjb;
            }
        }


        public void newThreadDo(commandWithTime.baseC dObj, GetRandomPos grp)
        {
            if (dObj.c == "placeArriving")
            {
                commandWithTime.placeArriving pa = (commandWithTime.placeArriving)dObj;
                this.setArrive(pa);
            }
            //  throw new NotImplementedException();
        }


    }
}

