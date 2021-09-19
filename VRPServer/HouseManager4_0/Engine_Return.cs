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

        internal void SetReturnT(int t, commandWithTime.returnning returnning)
        {
            this.startNewThread(t + 1, returnning, this);
            //Thread th = new Thread(() => setReturn(t, returnning));
            //th.Start();
        }

        void setReturn(commandWithTime.returnning rObj)
        {
            List<string> notifyMsg = new List<string>();
            lock (that.PlayerLock)
            {
                var player = that._Players[rObj.key];
                var car = that._Players[rObj.key].getCar();
                car.targetFpIndex = that._Players[rObj.key].StartFPIndex;
                ReturnThenSetComeBack(player, car, rObj, ref notifyMsg);
            }
            for (var i = 0; i < notifyMsg.Count; i += 2)
            {
                var url = notifyMsg[i];
                var sendMsg = notifyMsg[i + 1];
                Startup.sendMsg(url, sendMsg);
            }
        }

        private void ReturnThenSetComeBack(RoleInGame player, Car car, commandWithTime.returnning cmp, ref List<string> notifyMsg)
        {
            if (cmp.returningOjb.NeedToReturnBoss)
            {
                ReturnToBoss(player, car, cmp, ref notifyMsg);

            }
            else
            {
                ReturnToSelf(player, car, cmp, ref notifyMsg);
            }
        }

        private void ReturnToSelf(RoleInGame player, Car car, returnning cmp, ref List<string> notifyMsg)
        {
            var speed = car.ability.Speed;
            int startT = 0;
            var result = new List<int>();
            //RoleInGame boss = cmp.returningOjb.Boss;
            //  that.getStartPositon(Program.dt.GetFpByIndex(cmp.target), (boss.positionInStation + 1) % 5, ref startT);
            var boss = cmp.returningOjb.Boss;

            Program.dt.GetAFromBPoint(cmp.returningOjb.returnToSelfAddrPath, Program.dt.GetFpByIndex(cmp.target), speed, ref result, ref startT, player.improvementRecord.speedValue > 0);
            var self = player;
            that.getEndPositon(Program.dt.GetFpByIndex(self.StartFPIndex), self.positionInStation, ref result, ref startT, player.improvementRecord.speedValue > 0);
            // result.RemoveAll(item => item.t == 0);

            car.setState(that._Players[cmp.key], ref notifyMsg, CarState.returning);
            car.targetFpIndex = self.StartFPIndex;

            Data.PathStartPoint2 startPosition;
            if (cmp.returningOjb.returnToSelfAddrPath.Count == 0)
            {
                that.getStartPositionByFp(out startPosition, Program.dt.GetFpByIndex(player.StartFPIndex));
            }
            else
            {
                that.getStartPositionByGoPath(out startPosition, cmp.returningOjb.returnToSelfAddrPath);
            }

            car.setAnimateData(player, ref notifyMsg,
                new AnimateData2(startPosition, result, DateTime.Now, false));
            this.startNewThread(startT, new commandWithTime.comeBack()
            {
                c = "comeBack",
                //car = cmp.car,
                key = cmp.key
            }, this);
        }

        private void ReturnToBoss(RoleInGame player, Car car, returnning cmp, ref List<string> notifyMsg)
        {
            switch (cmp.changeType)
            {
                case returnning.ChangeType.AfterTax:
                    {
                        var speed = car.ability.Speed;
                        int startT = 0;
                        var result = new List<int>();
                        //RoleInGame boss = cmp.returningOjb.Boss;
                        //  that.getStartPositon(Program.dt.GetFpByIndex(cmp.target), (boss.positionInStation + 1) % 5, ref startT);
                        var boss = cmp.returningOjb.Boss;

                        Data.PathStartPoint2 startPosition;
                        var fp1 = Program.dt.GetFpByIndex(boss.StartFPIndex);
                        result = that.getStartPositon(fp1, boss.positionInStation + 1, ref startT, out startPosition, player.improvementRecord.speedValue > 0);
                        Program.dt.GetAFromBPoint(cmp.returningOjb.returnToSelfAddrPath, Program.dt.GetFpByIndex(cmp.target), speed, ref result, ref startT, player.improvementRecord.speedValue > 0);
                        var self = player;
                        that.getEndPositon(Program.dt.GetFpByIndex(self.StartFPIndex), self.positionInStation, ref result, ref startT, player.improvementRecord.speedValue > 0);
                        // result.RemoveAll(item => item.t == 0);

                        car.setState(that._Players[cmp.key], ref notifyMsg, CarState.returning);
                        car.targetFpIndex = self.StartFPIndex;
                        car.setAnimateData(player, ref notifyMsg,
                            new AnimateData2(startPosition, result, DateTime.Now, false));
                        this.startNewThread(startT, new commandWithTime.comeBack()
                        {
                            c = "comeBack",
                            //car = cmp.car,
                            key = cmp.key
                        }, this);
                    }; break;
                case returnning.ChangeType.BeforeTax:
                    {
                        var speed = car.ability.Speed;
                        int startT = 0;
                        var result = new List<int>();
                        //RoleInGame boss = cmp.returningOjb.Boss;
                        //  that.getStartPositon(Program.dt.GetFpByIndex(cmp.target), (boss.positionInStation + 1) % 5, ref startT);
                        var boss = cmp.returningOjb.Boss;

                        Program.dt.GetAFromBPoint(cmp.returningOjb.returnToBossAddrPath, Program.dt.GetFpByIndex(cmp.target), speed, ref result, ref startT, player.improvementRecord.speedValue > 0);
                        that.getEndPositon(Program.dt.GetFpByIndex(boss.StartFPIndex), boss.positionInStation + 1, ref result, ref startT, player.improvementRecord.speedValue > 0);
                        // result.RemoveAll(item => item.t == 0);

                        car.setState(that._Players[cmp.key], ref notifyMsg, CarState.returning);
                        car.targetFpIndex = boss.StartFPIndex;

                        /* 
                         * A:这里的if 是正常情况
                         * B:else 考虑的是攻击自己的老大的情况！位置就是处于老大大本营时的情况
                         */
                        if (cmp.returningOjb.returnToBossAddrPath.Count > 0)
                        {
                            Data.PathStartPoint2 startPosition;
                            that.getStartPositionByGoPath(out startPosition, cmp.returningOjb.returnToBossAddrPath);
                            car.setAnimateData(player, ref notifyMsg,
                                new AnimateData2(startPosition, result, DateTime.Now, false));
                        }
                        else
                        {

                        }
                        that.taxE.CollectTax(startT, new taxSet()
                        {
                            c = "taxSet",
                            changeType = returnning.ChangeType.BeforeTax,
                            key = cmp.key,
                            target = cmp.target,
                            returningOjb = cmp.returningOjb
                        });
                    }; break;
                default:
                    {
                        throw new Exception($"cmp.changeType 没有赋正确值");
                    }
            }

        }

        private void setBack(commandWithTime.comeBack comeBack)
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
                            that.SendPromoteCountOfPlayer(car.ability.diamondInCar, player.PromoteDiamondCount[car.ability.diamondInCar], (Player)player, ref notifyMsg);
                    }
                    car.ability.Refresh(player, car, ref notifyMsg);
                    car.Refresh(player, ref notifyMsg);

                    if (that.driverM.controlledByMagic(player, car, ref notifyMsg))
                    {

                    }
                    if (player.playerType == RoleInGame.PlayerType.NPC)
                    {
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

            for (var i = 0; i < notifyMsg.Count; i += 2)
            {
                var url = notifyMsg[i];
                var sendMsg = notifyMsg[i + 1];
                this.sendMsg(url, sendMsg);
            }
        }

        internal string OrderToReturn(OrderToReturn otr)
        {

            if (otr.c == "OrderToReturn")
                //   return this.
                return this.updateAction(this, otr, otr.Key);
            else if (otr.c == "OrderToReturnBySystem")
            {
                OrderToReturnBySystem otrbs = (OrderToReturnBySystem)otr;
                return this.updateActionBySys(this, otrbs, otrbs.Key);
            }
            else
            {
                throw new Exception($"{otr.c}__没有注册！！！");
            }
            //throw new NotImplementedException();
        }

        string updateActionBySys(interfaceOfEngine.tryCatchAction actionDo, OrderToReturnBySystem c, string operateKey)
        {
            string conditionNotReason;
            if (actionDo.conditionsOk(c, out conditionNotReason))
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
                                        if (actionDo.carAbilitConditionsOk(player, car, c))
                                        {
                                            car.setState(player, ref notifyMsg, CarState.returning);
                                            setReturn(new returnning()
                                            {
                                                c = "returnning",
                                                changeType = returnning.ChangeType.BeforeTax,
                                                key = player.Key,
                                                returningOjb = player.returningOjb,
                                                target = car.targetFpIndex
                                            });
                                        }
                                    }; break;
                            }
                        }
                    }
                }

                for (var i = 0; i < notifyMsg.Count; i += 2)
                {
                    var url = notifyMsg[i];
                    var sendMsg = notifyMsg[i + 1];
                    //Console.WriteLine($"url:{url}");
                    if (!string.IsNullOrEmpty(url))
                    {
                        Startup.sendMsg(url, sendMsg);
                    }
                }
                return "";
            }
            else
            {
                return conditionNotReason;
            }
        }


        public ReturningOjb maindDo(RoleInGame player, Car car, Command c, ref List<string> notifyMsg, out MileResultReason mrr)
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
            });
            mrr = MileResultReason.Abundant;
            return player.returningOjb;
        }

        public void failedThenDo(Car car, RoleInGame player, Command c, ref List<string> notifyMsg)
        {
        }

        public bool conditionsOk(Command c, out string reason)
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

        public bool carAbilitConditionsOk(RoleInGame player, Car car, Command c)
        {
            return car.state == CarState.waitOnRoad;
        }

        internal void SetReturnFromBoss(int v, RoleInGame boss, returnning returnning)
        {
            this.startNewThread(v, returnning, this);
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

        public void newThreadDo(baseC dObj)
        {
            if (dObj.c == "returnning")
            {
                returnning obj = (returnning)dObj;
                this.setReturn(obj);
            }
            else if (dObj.c == "comeBack")
            {
                comeBack obj = (comeBack)dObj;
                this.setBack(obj);
            }
            //throw new NotImplementedException();
        }
    }
}
