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

        private void setReturn(commandWithTime.returnning rObj)
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
                throw new Exception("错误的调用");
            }
            var speed = car.ability.Speed;
            int startT = 0;
            var result = new List<int>();
            //RoleInGame boss = cmp.returningOjb.Boss;
            //  that.getStartPositon(Program.dt.GetFpByIndex(cmp.target), (boss.positionInStation + 1) % 5, ref startT);

            Program.dt.GetAFromBPoint(cmp.returningOjb.returnToSelfAddrPath, Program.dt.GetFpByIndex(cmp.target), speed, ref result, ref startT);
            that.getEndPositon(Program.dt.GetFpByIndex(that._Players[cmp.key].StartFPIndex), that._Players[cmp.key].positionInStation, ref result, ref startT);
            // result.RemoveAll(item => item.t == 0);

            car.setState(that._Players[cmp.key], ref notifyMsg, CarState.returning);
            // car.state = CarState.returning;
            if (cmp.returningOjb.NeedToReturnBoss)
            {
                that.taxE.CollectTax(startT, new taxSet()
                {
                    c = "taxSet",
                    changeType = returnning.ChangeType.Tax,
                    key = cmp.key,
                    target = cmp.target,
                    returningOjb = cmp.returningOjb
                });
            }
            else
            {
                this.startNewThread(startT, new commandWithTime.comeBack()
                {
                    c = "comeBack",
                    //car = cmp.car,
                    key = cmp.key
                }, this);
            }
            Data.PathStartPoint2 startPosition;
            that.getStartPositionByGoPath(out startPosition, cmp.returningOjb.returnToSelfAddrPath);
            //var player = this._Players[cmp.key];
            car.setAnimateData(player, ref notifyMsg, new AnimateData2()
            {
                start = startPosition,
                animateData = result,
                recordTime = DateTime.Now
            });
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
            return this.updateAction(this, otr, otr.Key);
            //throw new NotImplementedException();
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
                changeType = returnning.ChangeType.OrderToReturn,
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
            throw new NotImplementedException();
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
