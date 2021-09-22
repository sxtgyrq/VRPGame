using CommonClass;
using CommonClass.driversource;
using HouseManager4_0.interfaceOfEngine;
using HouseManager4_0.RoomMainF;
using System;
using System.Collections.Generic;
using static HouseManager4_0.Car;
using static HouseManager4_0.RoomMainF.RoomMain;
using static HouseManager4_0.RoomMainF.RoomMain.commandWithTime;

namespace HouseManager4_0
{
    public class Engine_DebtEngine : Engine, interfaceOfEngine.engine, interfaceOfEngine.startNewThread
    {
        public Engine_DebtEngine(RoomMain roomMain)
        {
            this.roomMain = roomMain;
        }
        internal class attackTool : interfaceOfHM.AttackT
        {
            public bool isMagic { get { return false; } }

            public bool CheckCarState(Car car)
            {
                return car.state == CarState.working;
            }


            public long DealWithPercentValue(long percentValue, RoleInGame player, RoleInGame victim, RoomMain that)
            {
                return percentValue;
            }

            public Engine_DebtEngine.DebtCondition getCondition()
            {
                return Engine_DebtEngine.DebtCondition.attack;
            }

            public int GetDefensiveValue(Driver driver)
            {
                if (driver == null)
                {
                    return 0;
                }
                else
                {
                    return driver.defensiveOfPhysics;
                }
            }

            public int GetDefensiveValue(Driver driver, bool defended)
            {
                if (defended)
                {
                    if (driver == null)
                    {
                        return Program.rm.magicE.DefencePhysicsAdd;
                    }
                    else
                    {
                        return driver.defensiveOfPhysics + Program.rm.magicE.DefencePhysicsAdd;
                    }
                }
                else
                {
                    return GetDefensiveValue(driver);
                }
            }

            public string GetSkillName()
            {
                return "业务切磋";
            }

            //public long getVolumeOrBussiness(Manager_Driver.ConfuseManger.AmbushInfomation ambushInfomation)
            //{
            //    return 0;
            //    // return ambushInfomation.bussinessValue;
            //}

            public long ImproveAttack(RoleInGame role, long attackMoney, ref List<string> notifyMsgs)
            {
                if (role.improvementRecord.attackValue > 0)
                {
                    var larger = Program.rm.magicE.enlarge(attackMoney);
                    if (larger - attackMoney > 0)
                        role.improvementRecord.reduceAttack(role, larger, ref notifyMsgs);
                    return larger;
                }
                else
                    return attackMoney;
                //throw new NotImplementedException();
            }

            public long ImproveAttack(RoleInGame role, long attackMoney)
            {
                if (role.improvementRecord.attackValue > 0)
                {
                    var larger = Program.rm.magicE.enlarge(attackMoney);
                    return larger;
                }
                else
                    return attackMoney;
            }

            public long leftValue(AbilityAndState ability)
            {
                return ability.leftBusiness;
            }

            public void MagicAnimateShow(RoleInGame player, RoleInGame victim, ref List<string> notifyMsgs)
            {
                // throw new NotImplementedException();
            }

            public void setCost(long reduce, RoleInGame player, Car car, ref List<string> notifyMsg)
            {
                car.ability.setCostBusiness(car.ability.costBusiness + reduce, player, car, ref notifyMsg);
            }
        }
        public void newThreadDo(baseC bObj)
        {
            if (bObj.c == "debtOwner")
            {
                var dOwner = (commandWithTime.debtOwner)bObj;
                this.setDebt(dOwner, new attackTool());
            }
            //throw new NotImplementedException();
        }
        public enum DebtCondition
        {
            attack,
            magic
        }
        internal void setDebtT(int startT, Car car, SetAttack sa, int goMile, RoomMainF.RoomMain.commandWithTime.ReturningOjb ro)
        {
            this.startNewThread(startT, new commandWithTime.debtOwner()
            {
                c = "debtOwner",
                key = sa.Key,
                //  car = sa.car,
                // returnPath = returnPath,
                target = car.targetFpIndex,//新的起点
                changeType = returnning.ChangeType.BeforeTax,
                victim = sa.targetOwner,
                costMile = goMile,
                returningOjb = ro
            }, this);
            //Thread th = new Thread(() => setDebt(startT,);
            //th.Start();
        }



        internal void setDebt(commandWithTime.debtOwner dOwner, interfaceOfHM.AttackT at)
        {
            List<string> notifyMsg = new List<string>();
            //  bool needUpdatePlayers = false;
            lock (that.PlayerLock)
            {
                var player = that._Players[dOwner.key];
                var car = that._Players[dOwner.key].getCar();
                // car.targetFpIndex = this._Players[dor.key].StartFPIndex;
                ;
                if (at.CheckCarState(car))
                {
                    if (car.targetFpIndex == -1)
                    {
                        throw new Exception("居然来了一个没有目标的车！！！");
                    }
                    else
                    {
                        /*
                         * 当到达地点时，有可能攻击对象不存在。
                         * 也有可能攻击对象已破产。
                         * 还有正常情况。
                         * 这三种情况都要考虑到。
                         */
                        //attackTool at = new attackTool();
                        // var attackMoney = car.ability.Business;
                        if (that._Players.ContainsKey(dOwner.victim))
                        {
                            var victim = that._Players[dOwner.victim];
                            if (!victim.Bust)
                            {
                                var percentValue = getAttackPercentValue(player, victim);
                                percentValue = at.DealWithPercentValue(percentValue, player, victim, this.that);
                                //if(victim.Money*100/ car.ability.Business)
                                long reduceSum = 0;
                                var m = victim.Money - reduceSum;
                                long reduce;
                                if (m > 0)
                                {
                                    this.DealWithReduceWhenAttack(at, player, car, victim, percentValue, ref notifyMsg, out reduce, m, ref reduceSum);
                                }
                                else
                                {
                                    reduceSum = 0;
                                    reduce = 0;
                                }
                                if (at.isMagic)
                                {
                                    that.magicE.AmbushSelf(victim, at, ref notifyMsg, ref reduceSum);
                                    at.MagicAnimateShow(player, victim, ref notifyMsg);
                                }

                                if (reduceSum > 0)
                                    victim.MoneySet(victim.Money - reduceSum, ref notifyMsg);
                                if (reduce > 0)
                                    this.WebNotify(player, $"你对【{victim.PlayerName}】进行了{at.GetSkillName()}，获得{(reduce / 100.00).ToString("f2")}金币。其还有{(victim.Money / 100.00).ToString("f2")}金币。");
                                if (victim.Money == 0)
                                {
                                    victim.SetBust(true, ref notifyMsg);
                                }
                                if (victim.playerType == RoleInGame.PlayerType.NPC)
                                {
                                    ((NPC)victim).BeingAttackedF(dOwner.key, ref notifyMsg);
                                }
                            }
                            else
                            {
                                //这种情况也有可能存在。
                            }

                        }
                        else
                        {
                            //这种情况有可能存在.
                        }
                        /*
                         * 无论什么情况，直接返回。
                         */
                        //  if (car.ability.leftBusiness <= 0 && car.ability.leftVolume <= 0)
                        {
                            car.setState(player, ref notifyMsg, CarState.returning);
                            that.retutnE.SetReturnT(0, new commandWithTime.returnning()
                            {
                                c = "returnning",
                                key = dOwner.key,
                                // car = dOwner.car,
                                //returnPath = dOwner.returnPath,//returnPath_Record,
                                target = dOwner.target,
                                changeType = dOwner.changeType,
                                returningOjb = dOwner.returningOjb
                            });

                        }
                        //else
                        //{
                        //    car.setState(player, ref notifyMsg, CarState.waitOnRoad);
                        //}
                    }
                }
                else
                {
                    throw new Exception("car.state == CarState.buying!或者 dor.changeType不是四种类型");
                }
            }
            for (var i = 0; i < notifyMsg.Count; i += 2)
            {
                var url = notifyMsg[i];
                var sendMsg = notifyMsg[i + 1];
                Console.WriteLine($"url:{url}");
                Startup.sendMsg(url, sendMsg);
            }
            // Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}执行setReturn结束");

        }
        internal long DealWithReduceWhenSimulationWithoutDefendMagic(interfaceOfHM.AttackT at, RoleInGame player, Car car, RoleInGame victim, long percentValue)
        {
            var attackMoneyBeforeDefend = (at.leftValue(car.ability) * (100 - at.GetDefensiveValue(victim.getCar().ability.driver)) / 100) * percentValue / 100;
            return attackMoneyBeforeDefend;
        }
        internal void DealWithReduceWhenAttack(interfaceOfHM.AttackT at, RoleInGame player, Car car, RoleInGame victim, long percentValue, ref List<string> notifyMsg, out long reduce, long m, ref long reduceSum)
        {
            var attackMoneyBeforeDefend = (at.leftValue(car.ability) * (100 - at.GetDefensiveValue(victim.getCar().ability.driver)) / 100) * percentValue / 100;
            var attackMoneyAfterDefend = (at.leftValue(car.ability) * (100 - at.GetDefensiveValue(victim.getCar().ability.driver, victim.improvementRecord.defenceValue > 0)) / 100) * percentValue / 100;
            attackMoneyBeforeDefend = Math.Max(1, attackMoneyBeforeDefend);
            attackMoneyAfterDefend = Math.Max(1, attackMoneyAfterDefend);

            long attackMoney;
            if (attackMoneyBeforeDefend == attackMoneyAfterDefend)
                attackMoney = attackMoneyBeforeDefend;
            else
            {
                attackMoney = attackMoneyAfterDefend;
                victim.improvementRecord.reduceDefend(victim, attackMoneyBeforeDefend, ref notifyMsg);
            }

            attackMoney = at.ImproveAttack(player, attackMoney, ref notifyMsg);
            reduce = attackMoney;
            if (reduce > m)
            {
                reduce = m;
            }
            reduce = Math.Max(1, reduce);
            at.setCost(reduce, player, car, ref notifyMsg);
            //this.WebNotify(victim, $"【{player.PlayerName}】对你进行了{at.GetSkillName()}，损失{(reduce / 100.00).ToString("f2")}金币 ");
            this.WebNotify(player, $"你对【{victim.PlayerName}】进行了{at.GetSkillName()}，获得{(reduce / 100.00).ToString("f2")}金币。其还有{(victim.Money / 100.00).ToString("f2")}金币。");
            this.WebNotify(victim, $"【{player.PlayerName}】对你进行了{at.GetSkillName()}，损失{(reduce / 100.00).ToString("f2")}金币 ");
            reduceSum += reduce;
        }



        internal long getAttackPercentValue(RoleInGame player, RoleInGame victim)
        {
            if (player.TheLargestHolderKey == victim.Key)
            {
                //Msg = $"[{victim.PlayerName}]是你的老大，只能发挥出攻击效率的1%";
                return 1;
            }
            else if (victim.TheLargestHolderKey == player.Key)
            {
                //Msg = $"[{victim.PlayerName}]是你的小弟，只能发挥出攻击效率的1%";
                return 1;
            }
            else if (victim.TheLargestHolderKey == player.TheLargestHolderKey)
            {
                return 1;
            }
            else
            {
                return 100;
            }
        }
    }
}
