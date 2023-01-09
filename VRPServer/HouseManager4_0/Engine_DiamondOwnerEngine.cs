using CommonClass;
using HouseManager4_0.RoomMainF;
using Model;
using System;
using System.Collections.Generic;
using static HouseManager4_0.Car;
using static HouseManager4_0.RoomMainF.RoomMain;

namespace HouseManager4_0
{
    public class Engine_DiamondOwnerEngine : Engine, interfaceOfEngine.engine, interfaceOfEngine.startNewThread
    {
        public Engine_DiamondOwnerEngine(RoomMain roomMain)
        {
            this.roomMain = roomMain;
        }

        public void newThreadDo(commandWithTime.baseC dObj, GetRandomPos grp)
        {
            if (dObj.c == "diamondOwner")
            {
                commandWithTime.diamondOwner dOwner = (commandWithTime.diamondOwner)dObj;
                this.setDiamondOwner(dOwner, grp);
            }
            //throw new NotImplementedException();
        }

        internal void StartDiamondOwnerThread(int startT, int step, RoleInGame player, Car car, SetPromote sp, RoomMainF.RoomMain.commandWithTime.ReturningOjb ro, int goMile, Node goPath, GetRandomPos grp)
        {

            System.Threading.Thread th = new System.Threading.Thread(() =>
            {
                if (step >= goPath.path.Count - 1)
                    this.startNewThread(startT + 100, new commandWithTime.diamondOwner()
                    {
                        c = "diamondOwner",
                        key = sp.Key,
                        target = car.targetFpIndex,//新的起点
                        changeType = commandWithTime.returnning.ChangeType.BeforeTax,
                        costMile = goMile,
                        returningOjb = ro,
                        diamondType = sp.pType
                    }, this, grp);
                //that.debtE.setDebtT(startT, car, sa, goMile, ro);
                //this.startNewThread(startT, new commandWithTime.defenseSet()
                //{
                //    c = command,
                //    changeType = commandWithTime.returnning.ChangeType.BeforeTax,
                //    costMile = goMile,
                //    key = ms.Key,
                //    returningOjb = ro,
                //    target = car.targetFpIndex,
                //    beneficiary = ms.targetOwner
                //}, this);
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
                                newStartT = 0;

                            car.setState(player, ref notifyMsg, CarState.working);
                            this.sendMsg(notifyMsg);
                            //string command, int startT, int step, RoleInGame player, Car car, MagicSkill ms, int goMile, Node goPath, commandWithTime.ReturningOjb ro
                            StartDiamondOwnerThread(newStartT, step, player, car, sp, ro, goMile, goPath, grp);

                        };
                    this.loop(p, step, startT, player, goPath);
                }
            });
            th.Start();
            //throw new NotImplementedException();

            //Thread th = new Thread(() => setDiamondOwner(startT, ));
            //th.Name = $"{sp.Key}-DiamondOwner";
            //th.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startT"></param>
        /// <param name="dor"></param>
        private void setDiamondOwner(commandWithTime.diamondOwner dor, GetRandomPos grp)
        {
            List<string> notifyMsg = new List<string>();
            bool needUpdatePromoteState = false;
            lock (that.PlayerLock)
            {
                var player = that._Players[dor.key];
                var car = that._Players[dor.key].getCar();
                {
                    if (car.state == CarState.working)
                    {
                        if (car.targetFpIndex == -1)
                        {
                            throw new Exception("居然来了一个没有目标的车！！！");
                        }
                        if (car.ability.diamondInCar != "")
                        {
                            /*
                             * 重复收集，立即返回！
                             */
                            car.setState(player, ref notifyMsg, CarState.returning);
                            that.retutnE.SetReturnT(0, new commandWithTime.returnning()
                            {
                                c = "returnning",
                                changeType = commandWithTime.returnning.ChangeType.BeforeTax,
                                key = dor.key,
                                returningOjb = dor.returningOjb,
                                target = dor.target
                            }, grp);
                        }
                        else if (dor.target == that.getPromoteState(dor.diamondType))
                        {
                            /*
                             * 这里，整个流程，保证玩家在开始任务的时候，钱是够的。如果不够，要爆异常的。
                             */
                            // var needMoney = that.promotePrice[dor.diamondType];
                            //if (car.ability.SumMoneyCanForPromote < needMoney)
                            //{
                            //    /*
                            //     * 这里，在逻辑上保证了car.ability.SumMoneyCanForPromote >=needMoney
                            //     * 首先在出发的时候就进行判断
                            //     * 其次，在promote地点选择的时候，会避免使用玩家的target.
                            //     * 最后保证了dor.target == this.getPromoteState(dor.changeType) 条件下，
                            //     * 肯定car.ability.SumMoneyCanForPromote >= needMoney
                            //     */
                            //    throw new Exception("钱不够，还让执行setDiamondOwner");
                            //}
                            //   $"需要用钱支付"); 
                            // car.ability.payForPromote(needMoney, player, car, ref notifyMsg);//用汽车上的钱支付

                            that.setPromtePosition(dor.diamondType);
                            //this.promoteMilePosition = GetRandomPosition();
                            needUpdatePromoteState = true;
                            car.ability.setDiamondInCar(dor.diamondType, player, car, ref notifyMsg);
                            // car.ability.diamondInCar = dor.changeType;
                            //car.setState(player, ref notifyMsg, CarState.waitOnRoad);
                            //car.ability.setCostMiles(car.ability.costMiles + dor.costMile, player, car, ref notifyMsg);
                            car.ability.setCostMiles(car.ability.costMiles + dor.costMile, player, car, ref notifyMsg);
                            // carParkOnRoad(dor.target, ref car, player, ref notifyMsg);
                            car.setState(player, ref notifyMsg, CarState.returning);
                            that.retutnE.SetReturnT(0, new commandWithTime.returnning()
                            {
                                c = "returnning",
                                changeType = commandWithTime.returnning.ChangeType.BeforeTax,
                                key = dor.key,
                                returningOjb = dor.returningOjb,
                                target = dor.target
                            }, grp);
                            that._Players[dor.key].returningOjb = dor.returningOjb;
                            if (player.playerType == RoleInGame.PlayerType.player)
                                ((Player)player).RefererCount++;
                        }
                        else
                        {
                            WebNotify(player, "车来迟了，宝石被别人取走啦！");
                            carParkOnRoad(dor.target, ref car, player, ref notifyMsg);
                            car.setState(player, ref notifyMsg, CarState.waitOnRoad);
                            that._Players[dor.key].returningOjb = dor.returningOjb;

                            //player.

                        }


                    }
                    else
                    {
                        throw new Exception("car.state == CarState.buying!或者 dor.changeType不是四种类型");
                    }
                }

            }
            for (var i = 0; i < notifyMsg.Count; i += 2)
            {
                var url = notifyMsg[i];
                var sendMsg = notifyMsg[i + 1];
                Startup.sendMsg(url, sendMsg);
            }

            if (needUpdatePromoteState)
            {
                that.CheckAllPlayersPromoteState(dor.diamondType);
            }
        }
    }
}
