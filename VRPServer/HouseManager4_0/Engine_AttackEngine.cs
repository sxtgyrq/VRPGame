using CommonClass;
using CommonClass.driversource;
using HouseManager4_0.RoomMainF;
using System;
using System.Collections.Generic;
using System.Text;
using static HouseManager4_0.Car;
using static HouseManager4_0.RoomMainF.RoomMain;

namespace HouseManager4_0
{
    public class Engine_AttackEngine : Engine_ContactEngine, interfaceOfEngine.engine, interfaceOfEngine.tryCatchAction
    {

        public Engine_AttackEngine(RoomMain roomMain)
        {
            this.roomMain = roomMain;
        }
        internal string updateAttack(SetAttack sa)
        {
            return this.updateAction(this, sa, sa.Key);
        }

        public RoomMainF.RoomMain.commandWithTime.ReturningOjb maindDo(RoleInGame player, Car car, Command c, ref List<string> notifyMsg, out MileResultReason mrr)
        {
            if (c.c == "SetAttack")
            {
                var sa = (SetAttack)c;
                return attack(player, car, sa, ref notifyMsg, out mrr);
            }
            else
            {
                throw new Exception($"数据传输错误！(传出类型为{c.c})");
            }
        }
        public void failedThenDo(Car car, RoleInGame player, Command c, ref List<string> notifyMsg)
        {
            if (c.c == "SetAttack")
            {
                SetAttack sa = (SetAttack)c;
                this.carDoActionFailedThenMustReturn(car, player, ref notifyMsg);
                if (car.state == CarState.waitAtBaseStation)
                {
                    /*
                     * 在起始地点，攻击失败，说明最大里程内不能到达，故要重新换NPC.
                     */
                    if (player.playerType == RoleInGame.PlayerType.NPC)
                    {
#warning 这里要考虑是否直接提升玩家等级。
                        ((NPC)player).SetBust(true, ref notifyMsg);
                    }
                }
                //this.carsAttackFailedThenMustReturn(car, player, sa, ref notifyMsg);
            }
        }

        public bool carAbilitConditionsOk(RoleInGame player, Car car, Command c)
        {
            if (c.c == "SetAttack")
                if (car.ability.leftBusiness > 0)
                {
                    SetAttack sa = (SetAttack)c;
                    var state = CheckTargetState(sa.targetOwner);
                    if (state == CarStateForBeAttacked.CanBeAttacked)
                    {
                        return true;
                        // doAttack(player, car, sa, ref notifyMsg);
                    }
                    else if (state == CarStateForBeAttacked.HasBeenBust)
                    {
                        this.WebNotify(player, "攻击的对象已经破产！");
                        return false;
                    }
                    else if (state == CarStateForBeAttacked.NotExisted)
                    {
                        this.WebNotify(player, "攻击的对象已经退出游戏！");
                        return false;
                    }
                    else
                    {
                        throw new Exception($"{state.ToString()}未注册！");
                    }
                }
                else
                {
                    this.WebNotify(player, "小车已经没有多余业务容量！");
                    return false;
                }
            else
            {
                return false;
            }
        }
        public bool conditionsOk(Command c, out string reason)
        {
            if (c.c == "SetAttack")
            {
                SetAttack sa = (SetAttack)c;
                if (!(that._Players.ContainsKey(sa.targetOwner)))
                {
                    reason = "";
                    return false;
                }
                else if (that._Players[sa.targetOwner].StartFPIndex != sa.target)
                {
                    reason = "";
                    return false;
                }
                else if (sa.targetOwner == sa.Key)
                {
#warning 这里要加日志，出现了自己攻击自己！！！
                    reason = "";
                    return false;
                }
                else
                {
                    reason = "";
                    return true;
                }
            }
            reason = "";
            return false;
        }

        enum CarStateForBeAttacked
        {
            CanBeAttacked,
            NotExisted,
            HasBeenBust,
        }
        private CarStateForBeAttacked CheckTargetState(string targetOwner)
        {
            if (roomMain._Players.ContainsKey(targetOwner))
            {
                if (roomMain._Players[targetOwner].Bust)
                {
                    return CarStateForBeAttacked.HasBeenBust;
                }
                else
                {
                    return CarStateForBeAttacked.CanBeAttacked;
                }
            }
            else
            {
                return CarStateForBeAttacked.NotExisted;
            }
        }


        class AttackObj : interfaceOfHM.ContactInterface
        {
            private SetAttack _sa;
            SetAttackArrivalThreadM _setAttackArrivalThread;


            public AttackObj(SetAttack sa, SetAttackArrivalThreadM setAttackArrivalThread)
            {
                this._sa = sa;
                this._setAttackArrivalThread = setAttackArrivalThread;
            }

            public string targetOwner
            {
                get { return this._sa.targetOwner; }
            }

            public int target
            {
                get { return this._sa.target; }
            }
            public delegate void SetAttackArrivalThreadM(int startT, Car car, SetAttack sa, int goMile, commandWithTime.ReturningOjb ro);
            public void SetArrivalThread(int startT, Car car, int goMile, commandWithTime.ReturningOjb returningOjb)
            {
                this._setAttackArrivalThread(startT, car, this._sa, goMile, returningOjb);
            }

            public bool carLeftConditions(Car car)
            {
                return car.ability.leftBusiness > 0;
            }
        }
        // delegate 
        /// <summary>
        /// 此函数，必须在this._Players.ContainsKey(sa.targetOwner)=true且this._Players[sa.targetOwner].Bust=false情况下运行。请提前进行判断！
        /// </summary>
        /// <param name="player">玩家</param>
        /// <param name="car"></param>
        /// <param name="sa"></param>
        /// <param name="notifyMsg"></param>
        /// <param name="victimState"></param>
        /// <param name="reason"></param>
        RoomMainF.RoomMain.commandWithTime.ReturningOjb attack(RoleInGame player, Car car, SetAttack sa, ref List<string> notifyMsg, out MileResultReason Mrr)
        {
            AttackObj ao = new AttackObj(sa, this.SetAttackArrivalThread);
            return this.contact(player, car, ao, ref notifyMsg, out Mrr); 
        }

        internal commandWithTime.ReturningOjb randomWhenConfused(RoleInGame player, RoleInGame boss, Car car, SetAttack sa, ref List<string> notifyMsg, out MileResultReason Mrr)
        {
            AttackObj ao = new AttackObj(sa, this.SetAttackArrivalThread);
            return this.randomWhenConfused(player, boss, car, ao, ref notifyMsg, out Mrr);
        }

        





        private void SetAttackArrivalThread(int startT, Car car, SetAttack sa, int goMile, commandWithTime.ReturningOjb ro)
        {
            that.debtE.setDebtT(startT, car, sa, goMile, ro);

        }



        //private void SetAttackArrivalThread(int startT, Car car, SetAttack sa, List<Model.MapGo.nyrqPosition> returnToSelfAddrPath, int goMile)
        //{
        //    SetAttackArrivalThread(startT, car, sa, null, returnToSelfAddrPath, goMile, false, null);
        //    //Thread th = new Thread(() => setDebt(startT, new commandWithTime.debtOwner()
        //    //{
        //    //    c = "debtOwner",
        //    //    key = sa.Key,
        //    //    //  car = sa.car,
        //    //    returnPath = returnPath,
        //    //    target = car.targetFpIndex,//新的起点
        //    //    changeType = "Attack",
        //    //    victim = sa.targetOwner
        //    //}));
        //    //th.Start();
        //}

    }
}
