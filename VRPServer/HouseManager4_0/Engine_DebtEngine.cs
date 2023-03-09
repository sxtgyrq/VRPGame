using CommonClass;
using CommonClass.driversource;
using HouseManager4_0.interfaceOfEngine;
using HouseManager4_0.RoomMainF;
using System;
using System.Collections.Generic;
using System.Data;
using static HouseManager4_0.Car;
using static HouseManager4_0.Engine_MagicEngine;
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
        internal partial class attackTool : interfaceOfHM.AttackT
        {
            public bool isMagic { get { return false; } }

            int IgnorePhysicsValue
            {
                get
                {
                    return GetIgnorePhysicsValue(this._role);
                }
            }

            internal static int GetIgnorePhysicsProbability(RoleInGame role)
            {
                return role.buildingReward[1];
            }

            internal static int GetIgnorePhysicsValue(RoleInGame role)
            {
                return role.buildingReward[1];
            }

            public bool CheckCarState(Car car)
            {
                return car.state == CarState.working;
            }


            public long DealWithPercentValue(long percentValue, RoleInGame player, RoleInGame victim, RoomMain that, GetRandomPos grp, ref List<string> notifyMsg)
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
                    if (this.PhysicsIsIgnored())
                        return Math.Max(0, driver.defensiveOfPhysics - this.IgnorePhysicsValue);
                    else
                        return driver.defensiveOfPhysics;
                }
            }

            public int GetDefensiveValue(Driver driver, bool defended)
            {
                if (defended)
                {
                    if (driver == null)
                    {
                        return Math.Max(0, Engine_MagicEngine.DefencePhysicsAdd - (this.PhysicsIsIgnored() ? this.IgnorePhysicsValue : 0));
                    }
                    else
                    {
                        return Math.Max(0, driver.defensiveOfPhysics + Engine_MagicEngine.DefencePhysicsAdd - (this.PhysicsIsIgnored() ? this.IgnorePhysicsValue : 0));
                    }
                }
                else
                {
                    return GetDefensiveValue(driver);
                }
            }

            public string GetSkillName()
            {
                return "普攻";
            }

            //public bool Ignore(ref RoleInGame role, ref System.Random rm)
            //{

            //}

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

            public int MagicImprovedProbabilityAndValue(RoleInGame player, ref System.Random rm)
            {
                return 0;
            }

            public int MagicImprovedValue(RoleInGame player)
            {
                return 0;
                //throw new NotImplementedException();
            }

            public void ReduceMagicImprovedValue(RoleInGame player)
            {
                // throw new NotImplementedException();
            }

            public void setCost(long reduce, RoleInGame player, Car car, ref List<string> notifyMsg)
            {
                car.ability.setCostBusiness(car.ability.costBusiness + reduce, player, car, ref notifyMsg);
            }
        }
        internal partial class attackTool
        {
            bool _physicsIsIgnored = false;
            public bool PhysicsIsIgnored()
            {
                return _physicsIsIgnored;
                // throw new NotImplementedException();
            }
            RoleInGame _role;
            public void IgnorePhysics(ref RoleInGame role, ref System.Random rm)
            {
                var driver = role.getCar().ability.driver;
                if (driver == null) _physicsIsIgnored = false;
                else if (GetIgnorePhysicsProbability(role) > rm.Next(0, 100))
                    if (driver.race == Race.devil) _physicsIsIgnored = true;
                    else _physicsIsIgnored = false;
                else _physicsIsIgnored = false;
                this._role = role;
            }

            public void ReduceIgnorePhysics(ref RoleInGame role)
            {
                /*
                 * 这里不进行衰减了。一次祈福，属性即拥有。
                 */
            }
        }

        internal partial class attackTool : interfaceOfHM.AttackIgnore
        {
            public void Ignore(ref RoleInGame role, ref System.Random rm)
            {
                this.IgnorePhysics(ref role, ref rm);
            }

            public bool Ignored()
            {
                return this.PhysicsIsIgnored();
            }

            public void ReduceIgnore(ref RoleInGame role)
            {
                this.ReduceIgnorePhysics(ref role);
            }
        }

        public void newThreadDo(baseC bObj, GetRandomPos grp)
        {
            if (bObj.c == "debtOwner")
            {
                var dOwner = (commandWithTime.debtOwner)bObj;
                var at = new attackTool();
                this.setDebt(dOwner, at, grp);
            }
            //throw new NotImplementedException();
        }
        public enum DebtCondition
        {
            attack,
            magic
        }
        internal void setDebtT(int startT, Car car, SetAttack sa, int goMile, RoomMainF.RoomMain.commandWithTime.ReturningOjb ro, GetRandomPos grp)
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
            }, this, grp);
            //Thread th = new Thread(() => setDebt(startT,);
            //th.Start();
        }

        internal void setDebt(commandWithTime.debtOwner dOwner, interfaceOfHM.AttackT at, GetRandomPos grp)
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
                                percentValue = at.DealWithPercentValue(percentValue, player, victim, this.that, grp, ref notifyMsg);
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
                                    that.magicE.AmbushSelf(victim, at, grp, ref notifyMsg, ref reduceSum);
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
                                    ((NPC)victim).BeingAttackedF(dOwner.key, ref notifyMsg, Program.rm, Program.dt);
                                }
                                if (player.playerType == RoleInGame.PlayerType.player)
                                    ((Player)player).RefererCount++;
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
                            that.retutnE.SetReturnT(5, new commandWithTime.returnning()
                            {
                                c = "returnning",
                                key = dOwner.key,
                                // car = dOwner.car,
                                //returnPath = dOwner.returnPath,//returnPath_Record,
                                target = dOwner.target,
                                changeType = dOwner.changeType,
                                returningOjb = dOwner.returningOjb
                            }, grp);

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
            this.sendSeveralMsgs(notifyMsg);
        }

        internal long DealWithReduceWhenSimulationWithoutDefendMagic(interfaceOfHM.AttackT at, RoleInGame player, Car car, RoleInGame victim, long percentValue)
        {
            var attackMoneyBeforeDefend = (at.leftValue(car.ability) * (100 - at.GetDefensiveValue(victim.getCar().ability.driver)) / 100) * percentValue / 100;
            return attackMoneyBeforeDefend;
        }

        internal void DealWithReduceWhenAttack(interfaceOfHM.AttackT at, RoleInGame player, Car car, RoleInGame victim, long percentValue, ref List<string> notifyMsg, out long reduce, long m, ref long reduceSum)
        {
            /*
             * 1.这个方法中的Ignore针对普攻、雷法、火法、水法中的忽视
             * 2.这个方法中的Improve针对雷法、火法、水法，强50%
             */
            var improvedV = at.MagicImprovedProbabilityAndValue(player, ref that.rm);
            at.Ignore(ref player, ref that.rm);

            var attackMoneyBeforeDefend = ((at.leftValue(car.ability) * (100 - at.GetDefensiveValue(victim.getCar().ability.driver)) / 100) * percentValue / 100) * (100 + improvedV) / 100;
            var attackMoneyAfterDefend = ((at.leftValue(car.ability) * (100 - at.GetDefensiveValue(victim.getCar().ability.driver, victim.improvementRecord.defenceValue > 0)) / 100) * percentValue / 100) * (100 + improvedV) / 100;
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
            if (!at.isMagic)
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
            if (at.Ignored())
            {
                if (at.isMagic)
                {
                    var amt = (attackMagicTool)at;
                    that.WebNotify(player, $"你忽略了其抵抗{amt.GetSkillName()}的能力！");
                    that.WebNotify(victim, $"【{player.PlayerName}】忽略了你的抵抗{amt.GetSkillName()}的能力{amt.IgnoreValue}点！");
                    at.ReduceIgnore(ref player);
                }
                else
                {
                    that.WebNotify(player, $"你忽略了其抵抗{at.GetSkillName()}的能力！");
                    that.WebNotify(victim, $"【{player.PlayerName}】忽略了你的抵抗{at.GetSkillName()}的能力！");
                    at.ReduceIgnore(ref player);
                }
            }
            if (improvedV > 0)
            {

            }
            //at.ReduceMagicImprovedValue(player);
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

        //由于
        internal void DirectAttackThenMagic(attackMagicTool at, RoleInGame player, RoleInGame victim, GetRandomPos grp)
        {
            List<string> notifyMsg = new List<string>();
            //  bool needUpdatePlayers = false;
            lock (that.PlayerLock)
            {
                //   var player = that._Players[dOwner.key];
                var car = player.getCar();
                // car.targetFpIndex = this._Players[dor.key].StartFPIndex;
                ;
                if (car.state == CarState.working && car.DirectAttack)//这里要
                {
                    {
                        /*
                         * 当到达地点时，有可能攻击对象不存在。
                         * 也有可能攻击对象已破产。
                         * 还有正常情况。
                         * 这三种情况都要考虑到。
                         */
                        //attackTool at = new attackTool();
                        // var attackMoney = car.ability.Business;
                        if (that._Players.ContainsKey(victim.Key))
                        {
                            // var victim = that._Players[dOwner.victim];
                            if (!victim.Bust)
                            {
                                var percentValue = getAttackPercentValue(player, victim);
                                percentValue = at.DealWithPercentValue(percentValue, player, victim, this.that, grp, ref notifyMsg);
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
                                    that.magicE.AmbushSelf(victim, at, grp, ref notifyMsg, ref reduceSum);
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
                                    ((NPC)victim).BeingAttackedF(player.Key, ref notifyMsg, Program.rm, Program.dt);
                                }
                                if (player.playerType == RoleInGame.PlayerType.player)
                                    ((Player)player).RefererCount++;
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

                        //else
                        //{
                        //    car.setState(player, ref notifyMsg, CarState.waitOnRoad);
                        //}
                    }
                }

            }
            this.sendSeveralMsgs(notifyMsg);
        }
    }
}
