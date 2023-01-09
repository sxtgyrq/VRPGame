using CommonClass;
using HouseManager4_0.RoomMainF;
using System;
using System.Collections.Generic;
using static HouseManager4_0.Car;
using static HouseManager4_0.RoomMainF.RoomMain;
using static HouseManager4_0.RoomMainF.RoomMain.commandWithTime;

namespace HouseManager4_0
{
    public class Engine_Return : Engine, interfaceOfEngine.engine, interfaceOfEngine.tryCatchAction, interfaceOfEngine.startNewThread
    {
        public Engine_Return(RoomMain roomMain)
        {
            this.roomMain = roomMain;
        }

        internal void SetReturnT(int t, commandWithTime.returnning returnning, GetRandomPos grp)
        {
            this.startNewThread(t + 1, returnning, this, grp);
            //Thread th = new Thread(() => setReturn(t, returnning));
            //th.Start();
        }

        void setReturn(commandWithTime.returnning rObj, GetRandomPos grp)
        {
            List<string> notifyMsg = new List<string>();
            lock (that.PlayerLock)
            {
                var player = that._Players[rObj.key];
                var car = that._Players[rObj.key].getCar();
                car.targetFpIndexSet(that._Players[rObj.key].StartFPIndex, ref notifyMsg);
                ReturnThenSetComeBack(player, car, rObj, grp, ref notifyMsg);
            }
            this.sendSeveralMsgs(notifyMsg);
        }

        private void ReturnThenSetComeBack(RoleInGame player, Car car, commandWithTime.returnning cmp, GetRandomPos grp, ref List<string> notifyMsg)
        {
            if (cmp.returningOjb.NeedToReturnBoss)
            {
                ReturnToBoss(player, car, cmp, grp, ref notifyMsg);

            }
            else
            {
                ReturnToSelf(player, car, cmp, grp, ref notifyMsg);
            }
        }

        private void ReturnToSelf(RoleInGame player, Car car, returnning cmp, GetRandomPos grp, ref List<string> notifyMsg)
        {
            var privateKeys = BitCoin.GamePathEncryption.PathEncryption.MainC.GetPrivateKeys(ref Program.rm.rm, cmp.returningOjb.returnToSelfAddrPath.path.Count);
            List<AnimateDataItem> animations = new List<AnimateDataItem>();
            int startT_FirstPath = 0;
            {
                var speed = car.ability.Speed;

                var result = new List<int>();
                //RoleInGame boss = cmp.returningOjb.Boss;
                //  that.getStartPositon(Program.dt.GetFpByIndex(cmp.target), (boss.positionInStation + 1) % 5, ref startT);
                var boss = cmp.returningOjb.Boss;

                // Program.dt.GetAFromBPoint(cmp.returningOjb.returnToSelfAddrPath, Program.dt.GetFpByIndex(cmp.target), speed, ref result, ref startT, player.improvementRecord.speedValue > 0);

                if (cmp.returningOjb.returnToSelfAddrPath.path.Count > 0)
                {
                    Program.dt.GetAFromBPoint(cmp.returningOjb.returnToSelfAddrPath.path[0].path, cmp.returningOjb.returnToSelfAddrPath.path[0].position, speed, ref result, ref startT_FirstPath, player.improvementRecord.speedValue > 0, that);
                }
                var self = player;
                //  that.getEndPositon(Program.dt.GetFpByIndex(self.StartFPIndex), self.positionInStation, ref result, ref startT, player.improvementRecord.speedValue > 0);
                // result.RemoveAll(item => item.t == 0);

                car.setState(that._Players[cmp.key], ref notifyMsg, CarState.returning);
                car.targetFpIndexSet(self.StartFPIndex, ref notifyMsg);

                Data.PathStartPoint3 startPosition;
                if (cmp.returningOjb.returnToSelfAddrPath.path.Count == 0)
                {
                    //that.getStartPositionByFp(out startPosition, cmp.returningOjb.returnToSelfAddrPath.path[0].position);
                    // that.getStartPositionByFp(out startPosition, Program.dt.GetFpByIndex(player.StartFPIndex));
                    var fp = Program.dt.GetFpByIndex(player.StartFPIndex);
                    double x, y, z;
                    CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(fp.positionLongitudeOnRoad, fp.positionLatitudeOnRoad, fp.Height, out x, out y, out z);
                    startPosition = new Data.PathStartPoint3()
                    {
                        x = Convert.ToInt32(x * 256),
                        y = Convert.ToInt32(y * 256),
                        z = Convert.ToInt32(z * 256)
                    };
                }
                else
                {
                    that.getStartPositionByGoPath(out startPosition, cmp.returningOjb.returnToSelfAddrPath.path[0]);
                }
                car.setState(player, ref notifyMsg, CarState.returning);

                var animation = new AnimateDataItem(startPosition, result, false, startT_FirstPath, cmp.returningOjb.returnToSelfAddrPath.path.Count > 0 ? privateKeys[0] : 255, ref that.rm);
                animations.Add(animation);
            }
            for (int i = 1; i < cmp.returningOjb.returnToSelfAddrPath.path.Count - 1; i++)
            {
                var goPath = cmp.returningOjb.returnToSelfAddrPath;
                var indexValue = i;
                var speed = car.ability.Speed;
                int startT_PathLast = 0;
                List<int> result;
                Data.PathStartPoint3 startPosition;
                {
                    result = new List<int>();
                    that.getStartPositionByGoPath(out startPosition, goPath.path[indexValue]);
                }
                Program.dt.GetAFromBPoint(goPath.path[indexValue].path, goPath.path[indexValue].path[0], speed, ref result, ref startT_PathLast, player.improvementRecord.speedValue > 0, that);
                var animation = new AnimateDataItem(startPosition, result, false, startT_PathLast, privateKeys[i], ref that.rm);
                animations.Add(animation);
            }
            EndWithRightPosition(cmp.returningOjb.returnToSelfAddrPath, car.ability.Speed, player, player, ref animations, privateKeys);
            car.setAnimateData(player, ref notifyMsg, animations, DateTime.Now);
            car.setState(player, ref notifyMsg, CarState.returning);
            this.StartArriavalThread(startT_FirstPath, 0, player, car, cmp.returningOjb.returnToSelfAddrPath,
                (newStartT) =>
                {
                    this.startNewThread(newStartT, new commandWithTime.comeBack()
                    {
                        c = "comeBack",
                        //car = cmp.car,
                        key = cmp.key
                    }, this, grp);
                }, player);

        }

        private void ReturnToBoss(RoleInGame player, Car car, returnning cmp, GetRandomPos grp, ref List<string> notifyMsg)
        {
            switch (cmp.changeType)
            {
                case returnning.ChangeType.AfterTax:
                    {
                        List<AnimateDataItem> animations = new List<AnimateDataItem>();
                        int startT_FirstPath;

                        var privateKeys = BitCoin.GamePathEncryption.PathEncryption.MainC.GetPrivateKeys(ref Program.rm.rm, cmp.returningOjb.returnToSelfAddrPath.path.Count);
                        var self = player;
                        {
                            var speed = car.ability.Speed;
                            startT_FirstPath = 0;
                            var result = new List<int>();
                            //RoleInGame boss = cmp.returningOjb.Boss;
                            //  that.getStartPositon(Program.dt.GetFpByIndex(cmp.target), (boss.positionInStation + 1) % 5, ref startT);
                            var boss = cmp.returningOjb.Boss;

                            Data.PathStartPoint3 startPosition;
                            var fp1 = Program.dt.GetFpByIndex(boss.StartFPIndex);
                            result = that.getStartPositon(fp1, boss.positionInStation + 1, ref startT_FirstPath, out startPosition, player.improvementRecord.speedValue > 0);
                            Program.dt.GetAFromBPoint(cmp.returningOjb.returnToSelfAddrPath.path[0].path, cmp.returningOjb.returnToSelfAddrPath.path[0].position, speed, ref result, ref startT_FirstPath, player.improvementRecord.speedValue > 0, that);


                            // that.getEndPositon(Program.dt.GetFpByIndex(self.StartFPIndex), self.positionInStation, ref result, ref startT, player.improvementRecord.speedValue > 0);
                            // result.RemoveAll(item => item.t == 0);

                            car.setState(that._Players[cmp.key], ref notifyMsg, CarState.returning);
                            car.targetFpIndexSet(self.StartFPIndex, ref notifyMsg);
                            var animation = new AnimateDataItem(startPosition, result, false, startT_FirstPath, cmp.returningOjb.returnToSelfAddrPath.path.Count > 0 ? privateKeys[0] : 255, ref that.rm);
                            animations.Add(animation);
                        }
                        var goPath = cmp.returningOjb.returnToSelfAddrPath;
                        for (int i = 1; i < goPath.path.Count - 1; i++)
                        {
                            var indexValue = i;
                            var speed = car.ability.Speed;
                            int startT_PathLast = 0;
                            List<int> result;
                            Data.PathStartPoint3 startPosition;
                            {
                                result = new List<int>();
                                that.getStartPositionByGoPath(out startPosition, goPath.path[indexValue]);
                            }
                            Program.dt.GetAFromBPoint(goPath.path[indexValue].path, goPath.path[indexValue].path[0], speed, ref result, ref startT_PathLast, player.improvementRecord.speedValue > 0, that);
                            var animation = new AnimateDataItem(startPosition, result, false, startT_PathLast, privateKeys[i], ref that.rm);
                            animations.Add(animation);
                        }
                        EndWithRightPosition(goPath, car.ability.Speed, player, player, ref animations, privateKeys);

                        car.setAnimateData(player, ref notifyMsg, animations, DateTime.Now);
                        car.setState(player, ref notifyMsg, CarState.returning);
                        this.StartArriavalThread(startT_FirstPath, 0, player, car, cmp.returningOjb.returnToSelfAddrPath, (int newStart) =>
                        {
                            this.startNewThread(newStart, new commandWithTime.comeBack()
                            {
                                c = "comeBack",
                                //car = cmp.car,
                                key = cmp.key
                            }, this, grp);
                        }, self);

                    }; break;
                case returnning.ChangeType.BeforeTax:
                    {
                        List<AnimateDataItem> animations = new List<AnimateDataItem>();
                        var speed = car.ability.Speed;
                        var boss = cmp.returningOjb.Boss;
                        var privateKeys = BitCoin.GamePathEncryption.PathEncryption.MainC.GetPrivateKeys(ref Program.rm.rm, cmp.returningOjb.returnToBossAddrPath.path.Count);

                        int startT_FirstPath;
                        {
                            startT_FirstPath = 0;
                            var result = new List<int>();
                            //RoleInGame boss = cmp.returningOjb.Boss;
                            //  that.getStartPositon(Program.dt.GetFpByIndex(cmp.target), (boss.positionInStation + 1) % 5, ref startT);


                            if (cmp.returningOjb.returnToBossAddrPath.path.Count > 0)
                                Program.dt.GetAFromBPoint(cmp.returningOjb.returnToBossAddrPath.path[0].path, cmp.returningOjb.returnToBossAddrPath.path[0].position, speed, ref result, ref startT_FirstPath, player.improvementRecord.speedValue > 0, that);
                            //  that.getEndPositon(Program.dt.GetFpByIndex(boss.StartFPIndex), boss.positionInStation + 1, ref result, ref startT, player.improvementRecord.speedValue > 0);
                            // result.RemoveAll(item => item.t == 0);

                            car.setState(that._Players[cmp.key], ref notifyMsg, CarState.returning);
                            car.targetFpIndexSet(boss.StartFPIndex, ref notifyMsg);

                            /* 
                             * A:这里的if 是正常情况
                             * B:else 考虑的是攻击自己的老大的情况！位置就是处于老大大本营时的情况
                             */
                            if (cmp.returningOjb.returnToBossAddrPath.path.Count > 0)
                            {
                                Data.PathStartPoint3 startPosition;
                                that.getStartPositionByGoPath(out startPosition, cmp.returningOjb.returnToBossAddrPath.path[0]);

                                animations.Add(new AnimateDataItem(startPosition, result, false, startT_FirstPath, cmp.returningOjb.returnToBossAddrPath.path.Count > 0 ? privateKeys[0] : 255, ref that.rm));
                            }
                            else
                            {

                            }

                        }

                        if (cmp.returningOjb.returnToBossAddrPath.path.Count > 0)
                        {
                            var goPath = cmp.returningOjb.returnToBossAddrPath;
                            for (int i = 1; i < goPath.path.Count - 1; i++)
                            {
                                var indexValue = i;
                                int startT_PathLast = 0;
                                List<int> result;
                                Data.PathStartPoint3 startPosition;
                                {
                                    result = new List<int>();
                                    that.getStartPositionByGoPath(out startPosition, goPath.path[indexValue]);
                                }
                                Program.dt.GetAFromBPoint(goPath.path[indexValue].path, goPath.path[indexValue].path[0], speed, ref result, ref startT_PathLast, player.improvementRecord.speedValue > 0, that);
                                var animation = new AnimateDataItem(startPosition, result, false, startT_PathLast, privateKeys[i], ref that.rm);
                                animations.Add(animation);
                            }
                            EndWithRightPosition(goPath, car.ability.Speed, player, boss, ref animations, privateKeys);
                            car.setAnimateData(player, ref notifyMsg, animations, DateTime.Now);
                        }
                        car.setState(player, ref notifyMsg, CarState.returning);
                        StartArriavalThread(startT_FirstPath, 0, player, car, cmp.returningOjb.returnToBossAddrPath, (int newStartT) =>
                        {
                            that.taxE.CollectTax(newStartT, new taxSet()
                            {
                                c = "taxSet",
                                changeType = returnning.ChangeType.BeforeTax,
                                key = cmp.key,
                                target = cmp.target,
                                returningOjb = cmp.returningOjb
                            }, grp);
                        }, boss);
                        //startNewThread(startT,)

                    }; break;
                default:
                    {
                        throw new Exception($"cmp.changeType 没有赋正确值");
                    }
            }

        }
        delegate void ArriavalF(int newStartT);
        private void StartArriavalThread(int startT, int step, RoleInGame player, Car car, Node goPath, ArriavalF f, RoleInGame targetPlayer)
        {

            System.Threading.Thread th = new System.Threading.Thread(() =>
            {

                if (step >= goPath.path.Count - 1)
                {
                    f(startT);
                }
                else
                {
                    if (step == 0)
                    {
                        Action p = () =>
                        {
                            step++;
                            List<string> notifyMsg = new List<string>();
                            int newStartT;
                            if (step < goPath.path.Count)
                                EditCarStateAfterSelect(step, player, ref car, ref notifyMsg, out newStartT);
                            else
                                newStartT = 0;

                            car.setState(player, ref notifyMsg, CarState.returning);
                            this.sendSeveralMsgs(notifyMsg);
                            StartArriavalThread(newStartT, step, player, car, goPath, f, targetPlayer);

                        };
                        this.loop(p, step, startT, player, goPath);
                    }
                    else
                    {
                        Action p = () =>
                        {
                            step++;
                            List<string> notifyMsg = new List<string>();
                            int newStartT;

                            if (step == goPath.path.Count - 1)
                            {
                                //  car.setState(player, ref notifyMsg, CarState.returning);
                                int positionInStation;
                                if (player.Key == targetPlayer.Key)
                                {
                                    positionInStation = targetPlayer.positionInStation;
                                }
                                else
                                    positionInStation = targetPlayer.positionInStation + 1;
                                EditCarStateAfterSelect(step, player, ref car, ref notifyMsg, out newStartT);
                            }
                            else
                            {

                                EditCarStateAfterSelect(step, player, ref car, ref notifyMsg, out newStartT);
                            }
                            car.setState(player, ref notifyMsg, CarState.returning);
                            this.sendSeveralMsgs(notifyMsg);
                            StartArriavalThread(newStartT, step, player, car, goPath, f, targetPlayer);
                        };
                        this.loop(p, step, startT, player, goPath);
                    }
                }
            });
            th.Start();

            //Thread th = new Thread(() => setArrive(startT, ));
            //th.Start();
        }



        private void setBack(commandWithTime.comeBack comeBack, GetRandomPos grp)
        {
            List<string> notifyMsg = new List<string>();
            lock (that.PlayerLock)
            {
                var player = that._Players[comeBack.key];
                var car = player.getCar();
                if (car.state == CarState.returning)
                {
                    //  var moneyCanSave1 = player.GetMoneyCanSave();

                    player.MoneySet(player.Money + car.ability.costBusiness + car.ability.costVolume, ref notifyMsg);

                    player.improvementRecord.reduceSpeed(player, car.ability.costBusiness + car.ability.costVolume, ref notifyMsg);
                    //player.Money += car.ability.costBusiness;
                    //player.Money += car.ability.costVolume;

                    //if (car.ability.subsidize > 0)
                    //{
                    //    player.setSupportToPlayMoney(player.SupportToPlayMoney + car.ability.subsidize, ref notifyMsg);
                    //    //player.SupportToPlay.Money += car.ability.subsidize;
                    //}
                    if (!string.IsNullOrEmpty(car.ability.diamondInCar))
                    {
                        player.PromoteDiamondCount[car.ability.diamondInCar]++;
                        if (player.playerType == RoleInGame.PlayerType.player)
                        {
                            that.SendPromoteCountOfPlayer(car.ability.diamondInCar, player.PromoteDiamondCount[car.ability.diamondInCar], (Player)player, ref notifyMsg);
                            that.taskM.DiamondCollected((Player)player);
                        }
                    }
                    car.ability.Refresh(player, car, ref notifyMsg);
                    car.Refresh(player, ref notifyMsg);

                    if (that.driverM.controlledByMagic(player, car, grp, ref notifyMsg))
                    {

                    }
                    if (player.playerType == RoleInGame.PlayerType.NPC)
                    {
                        //that.
                        that.GetMaxHarmInfomation((NPC)player, Program.dt);
                        ///  NPC
                        ((NPC)player).dealWithReturnedNPC(ref notifyMsg);
                    }

                    //AbilityChanged(player, car, ref notifyMsg, "business");
                    //AbilityChanged(player, car, ref notifyMsg, "volume");
                    //AbilityChanged(player, car, ref notifyMsg, "mile");

                    //  printState(player, car, "执行了归位");
                    //  var moneyCanSave2 = player.GetMoneyCanSave();
                    //if (moneyCanSave1 != moneyCanSave2)
                    {
                        // MoneyCanSaveChanged(player, moneyCanSave2, ref notifyMsg);
                    }
                    //if (player.playerType == RoleInGame.PlayerType.NPC)
                    //{
                    //    this.SetNPCToDoSomeThing((NPC)player, NPCAction.Bust);
                    //}
                }
                else
                {
                    throw new Exception($"小车返回是状态为{that._Players[comeBack.key].getCar().state}");
                }
            }
            this.sendSeveralMsgs(notifyMsg);
        }

        internal string OrderToReturn(OrderToReturn otr, GetRandomPos grp)
        {

            if (otr.c == "OrderToReturn")
                //   return this.
                return this.updateAction(this, otr, grp, otr.Key);
            else if (otr.c == "OrderToReturnBySystem")
            {
                OrderToReturnBySystem otrbs = (OrderToReturnBySystem)otr;
                return this.updateActionBySys(this, otrbs, grp, otrbs.Key);
            }
            else
            {
                throw new Exception($"{otr.c}__没有注册！！！");
            }
            //throw new NotImplementedException();
        }

        string updateActionBySys(interfaceOfEngine.tryCatchAction actionDo, OrderToReturnBySystem c, GetRandomPos grp, string operateKey)
        {
            string conditionNotReason;
            if (actionDo.conditionsOk(c, grp, out conditionNotReason))
            {
                List<string> notifyMsg = new List<string>();
                lock (that.PlayerLock)
                {
                    if (that._Players.ContainsKey(operateKey))
                    {
                        if (that._Players[operateKey].Bust)
                        {
                            var player = that._Players[operateKey];
                            var car = that._Players[operateKey].getCar();
                            switch (car.state)
                            {
                                case CarState.waitOnRoad:
                                    {
                                        if (actionDo.carAbilitConditionsOk(player, car, c, grp))
                                        {
                                            car.setState(player, ref notifyMsg, CarState.returning);
                                            setReturn(new returnning()
                                            {
                                                c = "returnning",
                                                changeType = returnning.ChangeType.BeforeTax,
                                                key = player.Key,
                                                returningOjb = player.returningOjb,
                                                target = car.targetFpIndex
                                            }, grp);
                                        }
                                    }; break;
                            }
                        }
                    }
                }

                this.sendSeveralMsgs(notifyMsg); 
                return "";
            }
            else
            {
                return conditionNotReason;
            }
        }


        public ReturningOjb maindDo(RoleInGame player, Car car, Command c, GetRandomPos grp, ref List<string> notifyMsg, out MileResultReason mrr)
        {
            //throw new NotImplementedException();
            //    car.state = CarState.returning;
            OrderToReturn otr = (OrderToReturn)c;
            car.setState(player, ref notifyMsg, CarState.returning);
            setReturn(new returnning()
            {
                c = "returnning",
                changeType = returnning.ChangeType.BeforeTax,
                key = otr.Key,
                returningOjb = player.returningOjb,
                target = car.targetFpIndex
            }, grp);
            mrr = MileResultReason.Abundant;
            return player.returningOjb;
        }

        public void failedThenDo(Car car, RoleInGame player, Command c, GetRandomPos grp, ref List<string> notifyMsg)
        {
        }

        public bool conditionsOk(Command c, GetRandomPos grp, out string reason)
        {
            if (c.c == "OrderToReturn")
            {
                reason = "";
                return true;
            }
            else if (c.c == "OrderToReturnBySystem")
            {
                reason = "";
                return true;
            }
            else
            {
                reason = "typeIsError";
                return false;
            }
        }

        public bool carAbilitConditionsOk(RoleInGame player, Car car, Command c, GetRandomPos grp)
        {
            return car.state == CarState.waitOnRoad;
        }

        internal void SetReturnFromBoss(int v, RoleInGame boss, returnning returnning, GetRandomPos grp)
        {
            this.startNewThread(v, returnning, this, grp);
            //this.newt
            //returnning returnningObj = new returnning()
            //{
            //    c = "returnning",
            //    changeType = returnning.ChangeType.AfterTax,
            //    key = returnning.key,
            //    returningOjb = returnning.returningOjb
            //};
            //this.newThreadDo(returnningObj);
            //throw new NotImplementedException();
        }

        public void newThreadDo(baseC dObj, GetRandomPos grp)
        {
            if (dObj.c == "returnning")
            {
                returnning obj = (returnning)dObj;
                this.setReturn(obj, grp);
            }
            else if (dObj.c == "comeBack")
            {
                comeBack obj = (comeBack)dObj;
                this.setBack(obj, grp);
            }
            //throw new NotImplementedException();
        }
    }
}
