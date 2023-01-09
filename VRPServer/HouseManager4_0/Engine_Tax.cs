using HouseManager4_0.RoomMainF;
using System;
using System.Collections.Generic;
using static HouseManager4_0.RoomMainF.RoomMain.commandWithTime;

namespace HouseManager4_0
{
    public class Engine_Tax : Engine, interfaceOfEngine.engine, interfaceOfEngine.startNewThread
    {
        public Engine_Tax(RoomMain roomMain)
        {
            this.roomMain = roomMain;
        }

        public void newThreadDo(baseC dObj, GetRandomPos grp)
        {
            if (dObj.c == "taxSet")
            {
                var taxSet = (taxSet)dObj;
                this.collectTaxT(taxSet, grp, new notifyMsg());
            }
            //throw new NotImplementedException();
        }

        internal void CollectTax(int startT, taxSet taxSet, GetRandomPos grp)
        {
            this.startNewThread(startT + 1, taxSet, this, grp);
        }

        void collectTaxT(taxSet taxSet, GetRandomPos grp, notifyMsg n)
        {
            var player = that._Players[taxSet.key];
            lock (that.PlayerLock)
            {

                var boss = taxSet.returningOjb.Boss;
                if (!boss.Bust)
                {
                    if (boss.StartFPIndex == taxSet.returningOjb.Boss.StartFPIndex)
                    {
                        if (player.confuseRecord.IsBeingControlled())
                        {
                            var cType = player.confuseRecord.getControlType();
                            var car = player.getCar();
                            long tax = 0;
                            long indemnity = 0;

                            indemnity = player.confuseRecord.getIndemnity();

                            {
                                long newCostBusiness;
                                if (indemnity > car.ability.costBusiness)
                                {
                                    newCostBusiness = 0;
                                }
                                else
                                {
                                    newCostBusiness = car.ability.costBusiness - indemnity;
                                }
                                var reduceValue = car.ability.costBusiness - newCostBusiness;
                                car.ability.setCostBusiness(newCostBusiness, player, car, ref n.notifyMsgs);
                                player.confuseRecord.reduceValue(reduceValue);
                                tax += reduceValue;
                            }
                            indemnity = player.confuseRecord.getIndemnity();

                            if (indemnity > 0)
                            {
                                long newCostVolume;
                                if (indemnity > car.ability.costVolume)
                                {
                                    newCostVolume = 0;
                                }
                                else
                                    newCostVolume = car.ability.costVolume - indemnity;
                                var reduceValue = car.ability.costVolume - newCostVolume;
                                car.ability.setCostVolume(newCostVolume, player, car, ref n.notifyMsgs);
                                player.confuseRecord.reduceValue(reduceValue);
                                tax += reduceValue;
                            }
                            if (tax > 0)
                                boss.MoneySet(boss.Money + tax, ref n.notifyMsgs);
                            if (player.confuseRecord.IsBeingControlled())
                            { }
                            else
                            {
                                switch (cType)
                                {
                                    case Manager_Driver.ConfuseManger.ControlAttackType.Confuse:
                                        {
                                            player.confuseMagicChanged(player, ref n.notifyMsgs);
                                        }; break;
                                    case Manager_Driver.ConfuseManger.ControlAttackType.Lost:
                                        {
                                            player.loseMagicChanged(player, ref n.notifyMsgs);
                                        }; break;
                                }
                            }
                        }
                        else
                        {
                            var car = player.getCar();
                            long tax = 0;

                            var newCostBusiness = car.ability.costBusiness * 4 / 5;
                            tax += (car.ability.costBusiness - newCostBusiness);
                            car.ability.setCostBusiness(newCostBusiness, player, car, ref n.notifyMsgs);

                            var newCostVolume = car.ability.costVolume * 4 / 5;
                            tax += (car.ability.costVolume - newCostVolume);
                            car.ability.setCostVolume(newCostVolume, player, car, ref n.notifyMsgs);

                            if (tax > 0)
                                boss.MoneySet(boss.Money + tax, ref n.notifyMsgs);
                        }
                        //var boss =
                    }
                }
                that.retutnE.SetReturnFromBoss(1000, boss, new returnning()
                {
                    c = "returnning",
                    changeType = returnning.ChangeType.AfterTax,
                    key = taxSet.key,
                    returningOjb = taxSet.returningOjb,
                    target = boss.StartFPIndex
                }, grp);
            }
            n.send(this);
        }
    }
}
