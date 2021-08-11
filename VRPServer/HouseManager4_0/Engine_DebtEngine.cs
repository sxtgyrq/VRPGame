using CommonClass;
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

        public void newThreadDo(baseC bObj)
        {
            if (bObj.c == "debtOwner")
            {
                var dOwner = (commandWithTime.debtOwner)bObj;
                this.setDebt(dOwner);
            }
            //throw new NotImplementedException();
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
        private void setDebt(commandWithTime.debtOwner dOwner)
        {
            List<string> notifyMsg = new List<string>();
            //  bool needUpdatePlayers = false;
            lock (that.PlayerLock)
            {
                var player = that._Players[dOwner.key];
                var car = that._Players[dOwner.key].getCar();
                // car.targetFpIndex = this._Players[dor.key].StartFPIndex;
                if (car.state == CarState.working)
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

                        var attackMoney = car.ability.Business;
                        if (that._Players.ContainsKey(dOwner.victim))
                        {
                            var victim = that._Players[dOwner.victim];
                            if (!victim.Bust)
                            {
                                var percentValue = getAttackPercentValue(player, victim);

                                //if(victim.Money*100/ car.ability.Business)
                                var m = victim.Money;
                                // var reduce = Math.Min(m, attackMoney);
                                var reduce = attackMoney * percentValue / 100;
                                if (reduce > victim.Money) 
                                {
                                    reduce = victim.Money;
                                }
                                victim.MoneySet(m - reduce, ref notifyMsg);
                                car.ability.setCostBusiness(car.ability.costBusiness + reduce, player, car, ref notifyMsg);
                                this.WebNotify(player, $"你对【{victim.PlayerName}】执行了攻击，攻击效率为{percentValue}%，获得{(reduce / 100.00).ToString("f2")}金币。其还有{(victim.Money/100.00).ToString("f2")}金币。");
                                this.WebNotify(victim, $"【{player.PlayerName}】对你执行了攻击，攻击效率为{percentValue}%，损失{(reduce / 100.00).ToString("f2")}金币 ");
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

        private long getAttackPercentValue(RoleInGame player, RoleInGame victim)
        {
            if (player.TheLargestHolderKey == victim.Key)
            {
                //Msg = $"[{victim.PlayerName}]是你的老大，只能发挥出攻击效率的10%";
                return 10;
            }
            else if (victim.TheLargestHolderKey == player.Key)
            {
                //Msg = $"[{victim.PlayerName}]是你的小弟，只能发挥出攻击效率的10%";
                return 10;
            }
            else if (victim.TheLargestHolderKey == player.TheLargestHolderKey)
            {
                return 20;
            }
            else 
            {
                return 100;
            } 
        }
    }
}
