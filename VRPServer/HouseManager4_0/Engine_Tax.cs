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

        public void newThreadDo(baseC dObj)
        {
            if (dObj.c == "taxSet")
            {
                var taxSet = (taxSet)dObj;
                this.collectTaxT(taxSet, new notifyMsg());
            }
            //throw new NotImplementedException();
        }

        internal void CollectTax(int startT, taxSet taxSet)
        {
            this.startNewThread(startT + 1, taxSet, this);
        }

        void collectTaxT(taxSet taxSet, notifyMsg n)
        {
            var player = that._Players[taxSet.key];
            lock (that.PlayerLock)
            {

                var boss = taxSet.returningOjb.Boss;
                if (!boss.Bust)
                {
                    if (boss.StartFPIndex == taxSet.returningOjb.Boss.StartFPIndex)
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
                });
            }
            n.send(this);
        }
    }
}
