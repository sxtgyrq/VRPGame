using CommonClass;
using HouseManager4_0.RoomMainF;
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

        public bool carAbilitConditionsOk(RoleInGame player, Car car, Command c)
        {
            if (c.c == "SetCollect")
                if (car.ability.leftVolume > 0)
                {
                    SetCollect sc = (SetCollect)c;
                    if (Program.dt.GetFpByIndex(that._collectPosition[sc.collectIndex]).FastenPositionID == sc.fastenpositionID)
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
                    this.WebNotify(player, "小车已经没有多余手机容量了，已经安排返回！");
                    return false;
                }
            else
            {
                return false;
            }
        }

        public bool conditionsOk(Command c, out string reason)
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
                else if (Program.dt.GetFpByIndex(that._collectPosition[sc.collectIndex]).FastenPositionID != sc.fastenpositionID)
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

        public void failedThenDo(Car car, RoleInGame player, Command c, ref List<string> notifyMsg)
        {
            if (c.c == "SetCollect")
                this.carDoActionFailedThenMustReturn(car, player, ref notifyMsg);
        }

        public commandWithTime.ReturningOjb maindDo(RoleInGame player, Car car, Command c, ref List<string> notifyMsg, out MileResultReason mrr)
        {
            if (c.c == "SetCollect")
            {
                var sc = (SetCollect)c;
                return this.collect(player, car, sc, ref notifyMsg, out mrr);
            }
            else
            {
                throw new Exception($"数据传输错误！(传出类型为{c.c})");
            }
        }

        internal string updateCollect(SetCollect sc)
        {
            return this.updateAction(this, sc, sc.Key);
        }

        RoomMainF.RoomMain.commandWithTime.ReturningOjb collect(RoleInGame player, Car car, SetCollect sc, ref List<string> notifyMsg, out MileResultReason Mrr)
        {
            RoleInGame boss;
            if (player.HasTheBoss(roomMain._Players, out boss))
            {
                return collectPassBossAddress(player, boss, car, sc, ref notifyMsg, out Mrr);
                //return promotePassBossAddress(player, boss, car, sp, ref notifyMsg, out reason);
            }
            else
            {
                if (car.ability.leftVolume > 0)
                {
                    var from = this.GetFromWhenUpdateCollect(that._Players[sc.Key], sc.cType, car);
                    var to = getCollectPositionTo(sc.collectIndex);//  this.promoteMilePosition;
                    var fp1 = Program.dt.GetFpByIndex(from);
                    var fp2 = Program.dt.GetFpByIndex(to);
                    var fbBase = Program.dt.GetFpByIndex(player.StartFPIndex);
                    //var goPath = Program.dt.GetAFromB(fp1, fp2.FastenPositionID);
                    //var goPath = Program.dt.GetAFromB(from, to);
                    var goPath = that.GetAFromB(from, to, player, ref notifyMsg);
                    //var returnPath = Program.dt.GetAFromB(to, player.StartFPIndex);
                    var returnPath = that.GetAFromB(to, player.StartFPIndex, player, ref notifyMsg);
                    var goMile = that.GetMile(goPath);
                    var returnMile = that.GetMile(returnPath);
                    if (car.ability.leftMile >= goMile + returnMile)
                    {
                        int startT;
                        this.EditCarStateWhenActionStartOK(player, ref car, to, fp1, goPath, ref notifyMsg, out startT);
                        var ro = commandWithTime.ReturningOjb.ojbWithoutBoss(returnPath);
                        StartArriavalThread(startT, car, sc, ro, goMile);
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


        private void StartArriavalThread(int startT, Car car, SetCollect sc, commandWithTime.ReturningOjb ro, int goMile)
        {
            this.startNewThread(startT + 100, new commandWithTime.placeArriving()
            {
                c = "placeArriving",
                key = sc.Key,
                //car = sc.car,
                returningOjb = ro,
                target = car.targetFpIndex,
                costMile = goMile
            }, this);
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
            //Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}开始执行setArrive");
            List<string> notifyMsg = new List<string>();
            bool needUpdateCollectState = false;
            lock (that.PlayerLock)
            {
                var player = that._Players[pa.key];
                var car = that._Players[pa.key].getCar();
                if (car.state == CarState.working)
                {
                    arriveThenDoCollect(ref player, ref car, pa, ref notifyMsg, out needUpdateCollectState);

                }

            }
            for (var i = 0; i < notifyMsg.Count; i += 2)
            {
                var url = notifyMsg[i];
                var sendMsg = notifyMsg[i + 1];
                Startup.sendMsg(url, sendMsg);
            }
            //Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}执行setReturn结束");
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
                    that.NPCM.Moleste((Player)role, pa.target, ref notifyMsg);
                long sumCollect = collectReWard; //DealWithTheFrequcy(this.CollectReWard);
                var selfGet = sumCollect;
                //  long sumDebet = 0;
                car.ability.setCostVolume(car.ability.costVolume + selfGet, role, car, ref notifyMsg);

                this.setCollectPosition(pa.target);
                //  this.collectPosition = this.GetRandomPosition(true);
                needUpdateCollectState = true;

                Console.WriteLine("----Do the collect process----！");

                if (role.playerType == RoleInGame.PlayerType.player)
                    that.GetMusic((Player)role, ref notifyMsg);
                if (role.playerType == RoleInGame.PlayerType.player)
                    that.GetBackground((Player)role, ref notifyMsg);
            }
            else
            {
                Console.WriteLine("----Not do the collect process----！");
            }
            //收集完，留在原地。
            //var car = this._Players[cmp.key].getCar(cmp.car);
            // car.ability.costMiles += pa.costMile;//

            var newCostMile = car.ability.costMiles + pa.costMile;
            car.ability.setCostMiles(newCostMile, role, car, ref notifyMsg);
            // AbilityChanged(player, car, ref notifyMsg, "mile");


            carParkOnRoad(pa.target, ref car, role, ref notifyMsg);
            car.setState(role, ref notifyMsg, CarState.waitOnRoad);
            that._Players[pa.key].returningOjb = pa.returningOjb;

            if (role.playerType == RoleInGame.PlayerType.player)
            {
                that.frequencyM.addFrequencyRecord();
            }
            // NPCAutoControlCollect(role);

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
                that._collectPosition[key] = that.GetRandomPosition(true);
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
        private commandWithTime.ReturningOjb collectPassBossAddress(RoleInGame player, RoleInGame boss, Car car, SetCollect sc, ref List<string> notifyMsg, out MileResultReason mrr)
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
                var goPath = that.GetAFromB(from, to, player, ref notifyMsg);
                var returnToBossPath = that.GetAFromB(to, boss.StartFPIndex, player, ref notifyMsg);
                var returnToSelfPath = that.GetAFromB(boss.StartFPIndex, player.StartFPIndex, player, ref notifyMsg);
                var goMile = that.GetMile(goPath);
                var returnToBossMile = that.GetMile(returnToBossPath);
                var returnToSelfMile = that.GetMile(returnToSelfPath);

                if (car.ability.leftMile >= goMile + returnToBossMile + returnToSelfMile)
                {
                    int startT;
                    this.EditCarStateWhenActionStartOK(player, ref car, to, fp1, goPath, ref notifyMsg, out startT);
                    var ro = commandWithTime.ReturningOjb.ojbWithBoss(returnToBossPath, returnToSelfPath, boss);
                    StartArriavalThread(startT, car, sc, ro, goMile);
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

        private void ActionInBoss(int from, int to)
        {
            throw new NotImplementedException();
        }

        public void newThreadDo(commandWithTime.baseC dObj)
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
