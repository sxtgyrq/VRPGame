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
                    //                    if (player.playerType == RoleInGame.PlayerType.NPC)
                    //                    {
                    //#warning 这里要考虑是否直接提升玩家等级。
                    //                        ((NPC)player).SetBust(true, ref notifyMsg);
                    //                    }
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
                    var state = CheckTargetState(player, sa.targetOwner);

                    switch (state)
                    {
                        case CarStateForBeAttacked.CanBeAttacked:
                            return true;
                        case CarStateForBeAttacked.HasBeenBust:
                            {
                                this.WebNotify(player, "攻击的对象已经破产！");
                                return false;
                            };
                        case CarStateForBeAttacked.NotExisted:
                            {
                                this.WebNotify(player, "攻击的对象已经退出游戏！");
                                return false;
                            };
                        case CarStateForBeAttacked.IsBeingChallenged:
                            {
                                this.WebNotify(player, "攻击的对象正在被挑战！");
                                return false;
                            };
                        case CarStateForBeAttacked.LevelIsLow:
                            {
                                this.WebNotify(player, "你等级太低，被忽略！");
                                return false;
                            };
                        case CarStateForBeAttacked.NotBoss:
                            {
                                this.WebNotify(player, "挑战是团队老大干来的事儿！");
                                return false;
                            };
                        case CarStateForBeAttacked.IsGroupMate:
                            {
                                this.WebNotify(player, "不能攻击队友！");
                                return false;
                            };
                        default:
                            {
                                throw new Exception($"{state.ToString()}未注册！");
                            };
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
                else if (that._Players[sa.targetOwner].TheLargestHolderKey != sa.targetOwner)
                {
                    if (that._Players[sa.targetOwner].playerType == RoleInGame.PlayerType.NPC)
                    {
                        reason = "";
                        return true;
                    }
                    else
                    {
                        var bossKey = that._Players[sa.targetOwner].TheLargestHolderKey;
                        if (that._Players.ContainsKey(bossKey))
                        {
                            var boss = that._Players[bossKey];
                            WebNotify(that._Players[sa.Key], $"不能攻击拜了老大的玩家，其老大为{boss.PlayerName}，你可以直接攻击其老大！");
                            reason = "";
                            return false;
                        }
                    }
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
            NotBoss,
            IsBeingChallenged,
            LevelIsLow,
            IsGroupMate,
        }
        private CarStateForBeAttacked CheckTargetState(RoleInGame role, string targetOwner)
        {
            if (role.playerType == RoleInGame.PlayerType.player)
            {
                var player = (Player)role;
                if (roomMain._Players.ContainsKey(targetOwner))
                {
                    if (roomMain._Players[targetOwner].Bust)
                    {
                        return CarStateForBeAttacked.HasBeenBust;
                    }
                    else
                    {
                        if (roomMain._Players[targetOwner].playerType == RoleInGame.PlayerType.NPC)
                        {
                            var targetNPC = (NPC)roomMain._Players[targetOwner];
                            if (string.IsNullOrEmpty(targetNPC.challenger))
                            {
                                if (player.TheLargestHolderKey == player.Key)
                                {
                                    if (player.Level + 1 >= targetNPC.Level)
                                        return CarStateForBeAttacked.CanBeAttacked;
                                    else
                                        return CarStateForBeAttacked.LevelIsLow;
                                }
                                else
                                {
                                    return CarStateForBeAttacked.NotBoss;
                                }
                            }
                            else
                            {
                                if (targetNPC.challenger == player.TheLargestHolderKey)
                                    return CarStateForBeAttacked.CanBeAttacked;
                                else
                                    return CarStateForBeAttacked.IsBeingChallenged;
                            }
                        }
                        else
                        {
                            var targetPlayer = (Player)roomMain._Players[targetOwner];
                            if (targetPlayer.IsOnline())
                            {
                                if (that.isAtTheSameGroup(player.Key, targetPlayer.Key))
                                {
                                    return CarStateForBeAttacked.IsGroupMate;
                                }
                                else if (player.Level >= targetPlayer.Level)
                                    return CarStateForBeAttacked.CanBeAttacked;
                                else
                                    return CarStateForBeAttacked.LevelIsLow;
                            }
                            else
                            {
                                return CarStateForBeAttacked.CanBeAttacked;
                            }
                        }
                    }
                }
                else
                {
                    return CarStateForBeAttacked.NotExisted;
                }
            }
            else if (role.playerType == RoleInGame.PlayerType.NPC)
            {
                return CarStateForBeAttacked.CanBeAttacked;
            }
            else
            {
                throw new Exception("错误！");
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
            //public delegate void SetSpeedImproveArrivalThreadM(int startT, Car car, MagicSkill ms, int goMile, Node goPath, commandWithTime.ReturningOjb returningOjb);

            public delegate void SetAttackArrivalThreadM(int startT, Car car, SetAttack sa, int goMile, Node goPath, commandWithTime.ReturningOjb ro);
            //public void SetArrivalThread(int startT, Car car, int goMile, commandWithTime.ReturningOjb returningOjb)
            //{
            //    this._setAttackArrivalThread(startT, car, this._sa, goMile, returningOjb);
            //}

            public bool carLeftConditions(Car car)
            {
                return car.ability.leftBusiness > 0;
            }

            public void SetArrivalThread(int startT, Car car, int goMile, Node goPath, commandWithTime.ReturningOjb returningOjb)
            {
                this._setAttackArrivalThread(startT, car, this._sa, goMile, goPath, returningOjb);
                // this._
                // throw new NotImplementedException();
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
            AttackObj ao = new AttackObj(sa,
                (int startT, Car car, SetAttack sa, int goMile, Node goPath, commandWithTime.ReturningOjb ro) =>
                {
                    //this.SetAttackArrivalThread()
                    List<string> notifyMsg = new List<string>();
                    car.setState(player, ref notifyMsg, CarState.working);
                    this.sendMsg(notifyMsg);
                    this.SetAttackArrivalThread(startT, 0, player, car, sa, goMile, goPath, ro);
                }
              );
            return this.contact(player, car, ao, ref notifyMsg, out Mrr);
        }

        internal commandWithTime.ReturningOjb randomWhenConfused(RoleInGame player, RoleInGame boss, Car car, SetAttack sa, ref List<string> notifyMsg, out MileResultReason Mrr)
        {
            AttackObj ao = new AttackObj(sa,
              (int startT, Car car, SetAttack sa, int goMile, Node goPath, commandWithTime.ReturningOjb ro) =>
              {
                  //this.SetAttackArrivalThread()
                  List<string> notifyMsg = new List<string>();
                  car.setState(player, ref notifyMsg, CarState.working);
                  this.sendMsg(notifyMsg);
                  this.SetAttackArrivalThread(startT, 0, player, car, sa, goMile, goPath, ro);
              }
            );
            return this.randomWhenConfused(player, boss, car, ao, ref notifyMsg, out Mrr);
        }
        private void SetAttackArrivalThread(int startT, int step, RoleInGame player, Car car, SetAttack sa, int goMile, Node goPath, commandWithTime.ReturningOjb ro)
        {

            System.Threading.Thread th = new System.Threading.Thread(() =>
            {
                if (step >= goPath.path.Count - 1)
                    that.debtE.setDebtT(startT, car, sa, goMile, ro);
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
                    Action selectionIsRight = () =>
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
                                SetAttackArrivalThread(newStartT, step, player, car, sa, goMile, goPath, ro);
                            };
                    loop(selectionIsRight, step, startT, player, goPath); 
                }
            });
            th.Start();
        }


    }
}
