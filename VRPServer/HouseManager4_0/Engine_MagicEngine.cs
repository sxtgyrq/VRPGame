using CommonClass;
using CommonClass.driversource;
using HouseManager4_0.interfaceOfEngine;
using HouseManager4_0.RoomMainF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using static HouseManager4_0.Car;
using static HouseManager4_0.RoomMainF.RoomMain;
using OssModel = Model;

namespace HouseManager4_0
{
    public class Engine_MagicEngine : Engine_ContactEngine, interfaceOfEngine.engine, interfaceOfEngine.tryCatchAction, interfaceOfEngine.startNewThread
    {
        internal int DefencePhysicsAdd { get { return 40; } }

        public int DefenceMagicAdd { get { return 20; } }

        public Engine_MagicEngine(RoomMain roomMain)
        {
            this.roomMain = roomMain;
        }

        public bool carAbilitConditionsOk(RoleInGame player, Car car, Command c)
        {
            if (c.c == "MagicSkill")
                if (car.ability.leftVolume > 0)
                {
                    MagicSkill ms = (MagicSkill)c;
                    CommonClass.driversource.Skill skill;
                    if (ms.selectIndex == 1)
                    {
                        skill = car.ability.driver.skill1;
                    }
                    else
                    {
                        skill = car.ability.driver.skill2;
                    }
                    switch (skill.skillEnum)
                    {
                        case SkillEnum.Electic:
                        case SkillEnum.Water:
                        case SkillEnum.Fire:
                            {
                                var state = CheckTargetCanBeElecticMagiced(ms.targetOwner);
                                if (state == CarStateForBeMagiced.CanBeMagiced)
                                {
                                    if (that.isAtTheSameGroup(ms.Key, ms.targetOwner))
                                    {
                                        this.WebNotify(player, $"不能对队友【{that._Players[ms.targetOwner].PlayerName}】进行{skill.skillName}！");
                                        return false;
                                    }
                                    else
                                        return true;
                                }
                                else if (state == CarStateForBeMagiced.HasBeenBust)
                                {
                                    this.WebNotify(player, "攻击的对象已经破产！");
                                    return false;
                                }
                                else if (state == CarStateForBeMagiced.NotExisted)
                                {
                                    this.WebNotify(player, "攻击的对象已经退出游戏！");
                                    return false;
                                }
                                else
                                {
                                    throw new Exception($"{state.ToString()}未注册！");
                                }
                            };
                        case SkillEnum.Ambush:
                        case SkillEnum.Confusion:
                        case SkillEnum.Lose:
                            {
                                var state = CheckTargetCanBeControleMagiced(ms.targetOwner);
                                if (state == CarStateForBeMagiced.CanBeMagiced)
                                {
                                    if (that.isAtTheSameGroup(ms.Key, ms.targetOwner))
                                    {
                                        this.WebNotify(player, $"不能对队友【{that._Players[ms.targetOwner].PlayerName}】进行{skill.skillName}！");
                                        return false;
                                    }
                                    else
                                        return true;
                                }
                                else if (state == CarStateForBeMagiced.HasBeenBust)
                                {
                                    this.WebNotify(player, "攻击的对象已经破产！");
                                    return false;
                                }
                                else if (state == CarStateForBeMagiced.NotExisted)
                                {
                                    this.WebNotify(player, "攻击的对象已经退出游戏！");
                                    return false;
                                }
                                else
                                {
                                    throw new Exception($"{state.ToString()}未注册！");
                                }
                            };
                        case SkillEnum.Speed:
                        case SkillEnum.Defense:
                        case SkillEnum.Attack:
                            {
                                var state = CheckTargetCanBeImprovedByMagic(ms.targetOwner);
                                if (state == CarStateForBeMagiced.CanBeMagiced)
                                {
                                    if (that.isAtTheSameGroup(ms.Key, ms.targetOwner))
                                    {
                                        // this.WebNotify(player, $"不能对队友【{that._Players[ms.targetOwner].PlayerName}】进行{skill.skillName}！");
                                        return true;
                                    }
                                    else if (ms.targetOwner == ms.Key)
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        this.WebNotify(player, $"只能对自己或队友进行{skill.skillName}！");
                                        return false;
                                    }
                                }
                                else if (state == CarStateForBeMagiced.HasBeenBust)
                                {
                                    this.WebNotify(player, "提升的对象已经破产！");
                                    return false;
                                }
                                else if (state == CarStateForBeMagiced.NotExisted)
                                {
                                    this.WebNotify(player, "提升的对象已经退出游戏！");
                                    return false;
                                }
                                else
                                {
                                    throw new Exception($"{state.ToString()}未注册！");
                                }
                            }; break;
                        default:
                            {
                                return false;
                            }
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



        internal long enlarge(long attackMoney)
        {
            return attackMoney * 7 / 5;
            //throw new NotImplementedException();
        }

        internal int shotTime(int v, bool speedImproved)
        {
            if (speedImproved)
                return v * 5 / 6;
            else
                return v;
        }

        private CarStateForBeMagiced CheckTargetCanBeImprovedByMagic(string targetOwner)
        {
            return this.CheckTargetCanBeElecticMagiced(targetOwner);
        }

        enum CarStateForBeMagiced
        {
            CanBeMagiced,
            NotExisted,
            HasBeenBust,
        }
        private CarStateForBeMagiced CheckTargetCanBeControleMagiced(string targetOwner)
        {
            return this.CheckTargetCanBeElecticMagiced(targetOwner);
        }
        private CarStateForBeMagiced CheckTargetCanBeElecticMagiced(string targetOwner)
        {
            if (roomMain._Players.ContainsKey(targetOwner))
            {
                if (roomMain._Players[targetOwner].Bust)
                {
                    return CarStateForBeMagiced.HasBeenBust;
                }
                else
                {
                    return CarStateForBeMagiced.CanBeMagiced;
                }
            }
            else
            {
                return CarStateForBeMagiced.NotExisted;
            }
        }
        public bool conditionsOk(Command c, out string reason)
        {
            if (c.c == "MagicSkill")
            {
                MagicSkill ms = (MagicSkill)c;
                if (!(that._Players.ContainsKey(ms.targetOwner)))
                {
                    reason = "";
                    return false;
                }
                else if (that._Players[ms.targetOwner].StartFPIndex != ms.target)
                {
                    reason = "";
                    return false;
                }

                else if (that._Players[ms.Key].getCar().ability.driver == null)
                {
                    reason = "";
                    return false;
                }
                else if (ms.selectIndex != 1 && ms.selectIndex != 2)
                {
                    reason = "";
                    return false;
                }
                else if (that._Players[ms.Key].getCar().state != CarState.waitOnRoad)
                {
                    reason = "";
                    return false;
                }
                else if (ms.targetOwner == ms.Key && (that._Players[ms.Key].getCar().ability.driver.race == Race.immortal))
                {

                    reason = "";
                    return false;
                }
                else if (ms.targetOwner == ms.Key && (that._Players[ms.Key].getCar().ability.driver.race == Race.people))
                {

                    reason = "";
                    return false;
                }
                //else if (that._Players[ms.Key].getCar().ability.driver.race == Race.immortal) 
                //{
                    
                //}
                else
                {
                    reason = "";
                    return true;
                }
            }
            reason = "";
            return false;
        }

        public void failedThenDo(Car car, RoleInGame player, Command c, ref List<string> notifyMsg)
        {
            if (c.c == "MagicSkill")
            {
                MagicSkill ms = (MagicSkill)c;
                this.carDoActionFailedThenMustReturn(car, player, ref notifyMsg);
            }
        }

        public commandWithTime.ReturningOjb maindDo(RoleInGame player, Car car, Command c, ref List<string> notifyMsg, out MileResultReason mrr)
        {
            if (c.c == "MagicSkill")
            {
                var ms = (MagicSkill)c;
                if (player.confuseRecord.IsBeingControlled())
                {
                    if (player.confuseRecord.getControlType() == Manager_Driver.ConfuseManger.ControlAttackType.Confuse)
                    {
                        var boss = player.confuseRecord.getBoss();
                        var sa = new SetAttack()
                        {
                            c = "SetAttack",
                            Key = ms.Key,
                            target = ms.target,
                            targetOwner = ms.targetOwner
                        };
                        return that.attackE.randomWhenConfused(player, boss, car, sa, ref notifyMsg, out mrr);
                    }
                    else
                    {
                        return magic(player, car, ms, ref notifyMsg, out mrr);
                    }
                }
                else
                    return magic(player, car, ms, ref notifyMsg, out mrr);
            }
            else
            {
                throw new Exception($"数据传输错误！(传出类型为{c.c})");
            }
        }

        private commandWithTime.ReturningOjb magic(RoleInGame player, Car car, MagicSkill ms, ref List<string> notifyMsg, out MileResultReason Mrr)
        {
            if (player.confuseRecord.IsBeingControlled())
            {

            }
            {
                CommonClass.driversource.Skill skill;
                if (ms.selectIndex == 1)
                {
                    skill = car.ability.driver.skill1;
                }
                else
                {
                    skill = car.ability.driver.skill2;
                }
                switch (skill.skillEnum)
                {
                    case CommonClass.driversource.SkillEnum.Electic:
                        {
                            return electicMagic(player, car, ms, skill, ref notifyMsg, out Mrr);
                        };
                    case CommonClass.driversource.SkillEnum.Water:
                        {
                            return waterMagic(player, car, ms, skill, ref notifyMsg, out Mrr);
                        };
                    case SkillEnum.Fire:
                        {
                            return fireMagic(player, car, ms, skill, ref notifyMsg, out Mrr);
                        };
                    case SkillEnum.Confusion:
                        {
                            return confusionMagic(player, car, ms, skill, ref notifyMsg, out Mrr);
                        };
                    case SkillEnum.Lose:
                        {
                            return loseMagic(player, car, ms, skill, ref notifyMsg, out Mrr);
                        };
                    case SkillEnum.Ambush:
                        {
                            return ambushMagic(player, car, ms, skill, ref notifyMsg, out Mrr);
                        };
                    case SkillEnum.Speed:
                        {
                            return improveSpeedMagic(player, car, ms, skill, ref notifyMsg, out Mrr);
                        };
                    case SkillEnum.Attack:
                        {
                            return improveAttackMagic(player, car, ms, skill, ref notifyMsg, out Mrr);
                        };
                    case SkillEnum.Defense:
                        {
                            return improveDefenseMagic(player, car, ms, skill, ref notifyMsg, out Mrr);
                        };
                    default:
                        {
                            throw new Exception("意料之外！");
                        }
                }


            }
        }

        internal int ProtectedByAmbush()
        {
            return 20;
        }
        internal int ProtectedByConfuse()
        {
            return 20;
        }

        internal int ProtectedByLost()
        {
            return 20;
        }

        enum startArriavalThreadCommand
        {
            defenseSet,
            attackSet,
            speedSet
        }
        private void StartArriavalThread(startArriavalThreadCommand command, int startT, int step, RoleInGame player, Car car, MagicSkill ms, int goMile, Node goPath, commandWithTime.ReturningOjb ro)
        {
            System.Threading.Thread th = new System.Threading.Thread(() =>
            {
                if (step >= goPath.path.Count - 1)


                    switch (command)
                    {
                        case startArriavalThreadCommand.defenseSet:
                            {
                                this.startNewThread(startT, new commandWithTime.defenseSet()
                                {
                                    c = "defenseSet",
                                    changeType = commandWithTime.returnning.ChangeType.BeforeTax,
                                    costMile = goMile,
                                    key = ms.Key,
                                    returningOjb = ro,
                                    target = car.targetFpIndex,
                                    beneficiary = ms.targetOwner
                                }, this);
                            }; break;
                        case startArriavalThreadCommand.attackSet:
                            {
                                this.startNewThread(startT, new commandWithTime.attackSet()
                                {
                                    c = "attackSet",
                                    changeType = commandWithTime.returnning.ChangeType.BeforeTax,
                                    costMile = goMile,
                                    key = ms.Key,
                                    returningOjb = ro,
                                    target = car.targetFpIndex,
                                    beneficiary = ms.targetOwner
                                }, this);
                            }; break;
                        case startArriavalThreadCommand.speedSet:
                            {
                                this.startNewThread(startT, new commandWithTime.speedSet()
                                {
                                    c = "speedSet",
                                    changeType = commandWithTime.returnning.ChangeType.BeforeTax,
                                    costMile = goMile,
                                    key = ms.Key,
                                    returningOjb = ro,
                                    target = car.targetFpIndex,
                                    beneficiary = ms.targetOwner
                                }, this);
                            }; break;
                    }

                else
                {
                    if (step == 0)
                    {
                        this.ThreadSleep(startT);
                        if (player.playerType == RoleInGame.PlayerType.NPC || player.Bust)
                        {

                        }
                        else
                        {

                            StartSelectThread(goPath.path[step].selections, goPath.path[step].selectionCenter, (Player)player);
                        }

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
                        StartArriavalThread(command, newStartT, step, player, car, ms, goMile, goPath, ro);
                    }
                    else
                    {
                        this.ThreadSleep(startT);
                        if (player.playerType == RoleInGame.PlayerType.NPC || player.Bust)
                        {

                        }
                        else if (startT != 0)
                        {
                            StartSelectThread(goPath.path[step].selections, goPath.path[step].selectionCenter, (Player)player);
                        }
                        step++;
                        List<string> notifyMsg = new List<string>();
                        int newStartT;
                        if (step < goPath.path.Count)
                            EditCarStateAfterSelect(step, player, ref car, ref notifyMsg, out newStartT);
                        // else if(step==goPath.path.Count-1)
                        //EditCarStateAfterSelect(step,player,ref car,)
                        else
                            throw new Exception("这种情况不会出现");
                        //newStartT = 0;
                        car.setState(player, ref notifyMsg, CarState.working);
                        this.sendMsg(notifyMsg);
                        StartArriavalThread(command, newStartT, step, player, car, ms, goMile, goPath, ro);
                    }
                }
            });
            th.Start();
            //Thread th = new Thread(() => setArrive(startT, ));
            //th.Start();
        }


        private commandWithTime.ReturningOjb improveDefenseMagic(RoleInGame player, Car car, MagicSkill ms, Skill skill, ref List<string> notifyMsg, out MileResultReason Mrr)
        {
            DefenceObj dfo = new DefenceObj(ms, (int startT, Car car, MagicSkill ms2, int goMile, Node goPath, commandWithTime.ReturningOjb ro) =>
            {
                List<string> notifyMsg = new List<string>();
                car.setState(player, ref notifyMsg, CarState.working);
                this.sendMsg(notifyMsg);
                this.StartArriavalThread(startArriavalThreadCommand.defenseSet, startT, 0, player, car, ms2, goMile, goPath, ro);
                //this.startNewThread(startT, new commandWithTime.defenseSet()
                //{
                //    c = "defenseSet",
                //    changeType = commandWithTime.returnning.ChangeType.BeforeTax,
                //    costMile = goMile,
                //    key = ms.Key,
                //    returningOjb = ro,
                //    target = car.targetFpIndex,
                //    beneficiary = ms.targetOwner
                //}, this);
            });
            return this.contact(player, car, dfo, ref notifyMsg, out Mrr);
        }

        private commandWithTime.ReturningOjb improveAttackMagic(RoleInGame player, Car car, MagicSkill ms, Skill skill, ref List<string> notifyMsg, out MileResultReason Mrr)
        {
            AttackObj ao = new AttackObj(ms, (int startT, Car car, MagicSkill ms1, int goMile, Node goPath, commandWithTime.ReturningOjb ro) =>
            {
                List<string> notifyMsg = new List<string>();
                car.setState(player, ref notifyMsg, CarState.working);
                this.sendMsg(notifyMsg);
                this.StartArriavalThread(startArriavalThreadCommand.attackSet, startT, 0, player, car, ms1, goMile, goPath, ro);
                //beneficiary 
                //this.startNewThread(startT, new commandWithTime.attackSet()
                //{
                //    c = "attackSet",
                //    changeType = commandWithTime.returnning.ChangeType.BeforeTax,
                //    costMile = goMile,
                //    key = ms.Key,
                //    returningOjb = ro,
                //    target = car.targetFpIndex,
                //    beneficiary = ms.targetOwner
                //}, this);
            });
            return this.contact(player, car, ao, ref notifyMsg, out Mrr);
        }
        public class AttackObj : interfaceOfHM.ContactInterface
        {
            private MagicSkill _ms;
            SetAttackImproveArrivalThreadM _setAttackImproveArrivalThread;


            public AttackObj(MagicSkill ms, SetAttackImproveArrivalThreadM setAttackImproveArrivalThread)
            {
                this._ms = ms;
                this._setAttackImproveArrivalThread = setAttackImproveArrivalThread;
            }

            public string targetOwner
            {
                get { return this._ms.targetOwner; }
            }

            public int target
            {
                get { return this._ms.target; }
            }
            public delegate void SetAttackImproveArrivalThreadM(int startT, Car car, MagicSkill ms, int goMile, Node goPath, commandWithTime.ReturningOjb returningOjb);
            //public void SetArrivalThread(int startT, Car car, int goMile, commandWithTime.ReturningOjb returningOjb)
            //{
            //    this._setAttackImproveArrivalThread(startT, car, this._ms, goMile, returningOjb);
            //}

            public bool carLeftConditions(Car car)
            {
                return car.ability.leftBusiness > 0;
            }

            public void SetArrivalThread(int startT, Car car, int goMile, Node goPath, commandWithTime.ReturningOjb returningOjb)
            {
                //this._setAttackImproveArrivalThread(startT,car,)
                // throw new NotImplementedException();
                this._setAttackImproveArrivalThread(startT, car, this._ms, goMile, goPath, returningOjb);
            }
        }

        //public class AttackObj2 : AttackObj 
        //{
        //    private MagicSkill _ms;
        //}
        class SpeedObj : interfaceOfHM.ContactInterface
        {
            private MagicSkill _ms;
            SetSpeedImproveArrivalThreadM _setSpeedImproveArrivalThread;


            public SpeedObj(MagicSkill ms, SetSpeedImproveArrivalThreadM setSpeedImproveArrivalThread)
            {
                this._ms = ms;
                this._setSpeedImproveArrivalThread = setSpeedImproveArrivalThread;
            }

            public string targetOwner
            {
                get { return this._ms.targetOwner; }
            }

            public int target
            {
                get { return this._ms.target; }
            }
            public delegate void SetSpeedImproveArrivalThreadM(int startT, Car car, MagicSkill ms, int goMile, Node goPath, commandWithTime.ReturningOjb returningOjb);
            //public void SetArrivalThread(int startT, Car car, int goMile, commandWithTime.ReturningOjb returningOjb)
            //{
            //    //this._setAttackImproveArrivalThread(startT, car, this._ms, goMile, goPath, returningOjb);
            //    //this._setSpeedImproveArrivalThread(startT, car, this._ms, goMile, returningOjb);
            //}

            public bool carLeftConditions(Car car)
            {
                return car.ability.leftVolume > 0;
            }

            public void SetArrivalThread(int startT, Car car, int goMile, Node goPath, commandWithTime.ReturningOjb returningOjb)
            {
                this._setSpeedImproveArrivalThread(startT, car, this._ms, goMile, goPath, returningOjb);
            }
        }

        class DefenceObj : interfaceOfHM.ContactInterface
        {
            private MagicSkill _ms;
            SetDefenceObjImproveArrivalThreadM _setDefenceObjImproveArrivalThread;


            public DefenceObj(MagicSkill ms, SetDefenceObjImproveArrivalThreadM setDefencemproveArrivalThread)
            {
                this._ms = ms;
                this._setDefenceObjImproveArrivalThread = setDefencemproveArrivalThread;
            }

            public string targetOwner
            {
                get { return this._ms.targetOwner; }
            }

            public int target
            {
                get { return this._ms.target; }
            }
            public delegate void SetDefenceObjImproveArrivalThreadM(int startT, Car car, MagicSkill ms, int goMile, Node goPath, commandWithTime.ReturningOjb returningOjb);
            public void SetArrivalThread(int startT, Car car, int goMile, Node goPath, commandWithTime.ReturningOjb returningOjb)
            {
                this._setDefenceObjImproveArrivalThread(startT, car, this._ms, goMile, goPath, returningOjb);
            }

            public bool carLeftConditions(Car car)
            {
                return car.ability.leftVolume > 0;
            }
        }
        private commandWithTime.ReturningOjb improveSpeedMagic(RoleInGame player, Car car, MagicSkill ms, Skill skill, ref List<string> notifyMsg, out MileResultReason Mrr)
        {
            //  this.StartArriavalThread(startT, 1, player, car, ms2, goMile, goPath, ro);
            SpeedObj so = new SpeedObj(ms, (int startT, Car car, MagicSkill ms2, int goMile, Node goPath, commandWithTime.ReturningOjb ro) =>
            {
                List<string> notifyMsg = new List<string>();
                car.setState(player, ref notifyMsg, CarState.working);
                this.sendMsg(notifyMsg);
                this.StartArriavalThread(startArriavalThreadCommand.speedSet, startT, 0, player, car, ms2, goMile, goPath, ro);
            });
            return this.contact(player, car, so, ref notifyMsg, out Mrr);
        }

        private commandWithTime.ReturningOjb ambushMagic(RoleInGame player, Car car, MagicSkill ms, Skill skill, ref List<string> notifyMsg, out MileResultReason Mrr)
        {
            controlMagicTool ct = new controlMagicTool(skill);
            return controlMagic(player, car, ms, ct, ref notifyMsg, out Mrr);
        }

        class controlMagicTool : interfaceOfHM.ControlMagic
        {
            public Skill skill;

            public controlMagicTool(Skill skill_)
            {
                this.skill = skill_;
            }

            //public void addItem(RoleInGame victim)
            //{
            //    throw new NotImplementedException();
            //}

            public Manager_Driver.ConfuseManger.ControlAttackType GetAttackType()
            {
                switch (this.skill.skillEnum)
                {
                    case SkillEnum.Confusion:
                        {
                            return Manager_Driver.ConfuseManger.ControlAttackType.Confuse;
                        }
                    case SkillEnum.Lose:
                        {
                            return Manager_Driver.ConfuseManger.ControlAttackType.Lost;
                        }
                    case SkillEnum.Ambush:
                        {
                            return Manager_Driver.ConfuseManger.ControlAttackType.Ambush;
                        }
                    default:
                        {
                            throw new Exception("");
                        }
                }
            }

            public string GetName()
            {
                switch (this.skill.skillEnum)
                {
                    case SkillEnum.Confusion:
                        {
                            return this.skill.skillName;
                        }
                    case SkillEnum.Lose:
                        {
                            return this.skill.skillName;
                        }
                    case SkillEnum.Ambush:
                        {
                            return this.skill.skillName;
                        }
                    default:
                        {
                            throw new Exception("");
                        }
                }
            }
        }
        private commandWithTime.ReturningOjb loseMagic(RoleInGame player, Car car, MagicSkill ms, Skill skill, ref List<string> notifyMsg, out MileResultReason Mrr)
        {
            controlMagicTool ct = new controlMagicTool(skill);
            return controlMagic(player, car, ms, ct, ref notifyMsg, out Mrr);
        }

        private commandWithTime.ReturningOjb controlMagic(RoleInGame player, Car car, MagicSkill ms, interfaceOfHM.ControlMagic ct, ref List<string> notifyMsg, out MileResultReason Mrr)
        {
            if (that._Players.ContainsKey(ms.targetOwner))
            {
                var victim = that._Players[ms.targetOwner];
                OssModel.FastonPosition fpResult;
                var distanceIsEnoughToStart = that.theNearestToPlayerIsCarNotMoney(player, car, victim, out fpResult);
                if (distanceIsEnoughToStart)
                {
                    if (car.state == CarState.waitOnRoad)
                    {
                        switch (ct.GetAttackType())
                        {
                            case Manager_Driver.ConfuseManger.ControlAttackType.Confuse:
                            case Manager_Driver.ConfuseManger.ControlAttackType.Lost:
                                if (car.ability.leftVolume > 0)
                                {
                                    victim.confuseRecord.AddControlInfo(player, Program.dt.GetFpByIndex(car.targetFpIndex), car.ability.leftVolume, ct.GetAttackType());
                                    car.setControllingObj(player, car, ct.GetAttackType(), victim.Key, ref notifyMsg);// = victim;
                                    this.WebNotify(player, $"已经部署{ct.GetName()}计谋，等其车辆返回基地后开始实施！");
                                    Mrr = MileResultReason.Abundant;
                                    return player.returningOjb;
                                }
                                else
                                {
                                    Mrr = MileResultReason.MoneyIsNotEnougt;
                                    this.WebNotify(player, "你身上的剩余空间不够啦！");
                                    return player.returningOjb;
                                }
                            case Manager_Driver.ConfuseManger.ControlAttackType.Ambush:
                                if (car.ability.leftVolume > 0)
                                {
                                    victim.confuseRecord.AddAmbushInfo(player, Program.dt.GetFpByIndex(car.targetFpIndex), car.ability.leftVolume, victim.Key, ref notifyMsg);
                                    car.setControllingObj(player, car, ct.GetAttackType(), victim.Key, ref notifyMsg);
                                    this.WebNotify(player, $"已经部署{ct.GetName()}计谋，等待其被攻击！");
                                    Mrr = MileResultReason.Abundant;
                                    return player.returningOjb;
                                }
                                else
                                {
                                    Mrr = MileResultReason.MoneyIsNotEnougt;
                                    this.WebNotify(player, "你身上的剩余空间不够啦！");
                                    return player.returningOjb;
                                }
                            default:
                                throw new Exception("程序意料之外！");
                        }

                    }
                    else
                        throw new Exception("运行此方法，没有正确校验");

                }
                else
                {
                    Mrr = MileResultReason.NearestIsMoneyWhenAttack;
                    that.ViewPosition(player, fpResult, ref notifyMsg);
                    this.WebNotify(player, $"离【{victim.PlayerName}】最近的是[{fpResult.FastenPositionName}]处的钱，不是你的车，{ ct.GetName()}失败！");
                    return player.returningOjb;
                }
            }
            else
            {
                throw new Exception("准备运行条件里没有筛查？");
            }
        }

        private commandWithTime.ReturningOjb confusionMagic(RoleInGame player, Car car, MagicSkill ms, Skill skill, ref List<string> notifyMsg, out MileResultReason Mrr)
        {
            controlMagicTool ct = new controlMagicTool(skill);
            return controlMagic(player, car, ms, ct, ref notifyMsg, out Mrr);
        }

        private commandWithTime.ReturningOjb fireMagic(RoleInGame player, Car car, MagicSkill ms, Skill skill, ref List<string> notifyMsg, out MileResultReason Mrr)
        {
            attackMagicTool et = new attackMagicTool(skill);
            return attackMagic(player, car, ms, et, ref notifyMsg, out Mrr);
        }

        public class attackMagicTool : interfaceOfHM.AttackMagic
        {
            public Skill skill;

            public attackMagicTool(Skill skill_)
            {
                this.skill = skill_;
            }

            public bool isMagic { get { return true; } }

            public bool CheckCarState(Car car)
            {
                return car.state == CarState.waitOnRoad;
            }
            public long DealWithPercentValue(long percentValue, RoleInGame player, RoleInGame victim, RoomMain that)
            {
                var car = player.getCar();
                var rank = Program.rm.getPlayerClosestPositionRankNum(player, car, victim);

                for (var i = 0; i < rank; i++)
                {
                    percentValue = percentValue * 70 / 100;
                }
                percentValue = Math.Max(1, percentValue);
                return percentValue;
            }

            public Engine_DebtEngine.DebtCondition getCondition()
            {
                return Engine_DebtEngine.DebtCondition.magic;
            }
            public int GetDefensiveValue(Driver driver)
            {
                if (driver == null)
                {
                    return 0;
                }
                else
                {
                    switch (this.skill.skillEnum)
                    {
                        case SkillEnum.Electic:
                            {
                                return driver.defensiveOfElectic;
                            }
                        case SkillEnum.Water:
                            {
                                return driver.defensiveOfWater;
                            }
                        case SkillEnum.Fire:
                            {
                                return driver.defensiveOfFire;
                            }
                        default:
                            {
                                return 0;
                            }
                    }
                }
            }

            public int GetDefensiveValue(Driver driver, bool Defened)
            {
                if (Defened)
                {
                    int baseDefend;
                    if (driver == null)
                    {
                        baseDefend = 0;
                    }
                    else
                        switch (this.skill.skillEnum)

                        {
                            case SkillEnum.Electic:
                                {
                                    baseDefend = driver.defensiveOfElectic;
                                }; break;
                            case SkillEnum.Water:
                                {
                                    baseDefend = driver.defensiveOfWater;
                                }; break;
                            case SkillEnum.Fire:
                                {
                                    baseDefend = driver.defensiveOfFire;
                                }; break;
                            default:
                                {
                                    baseDefend = 0;
                                }; break;
                        }
                    int addDefend = Program.rm.magicE.DefenceMagicAdd;
                    return baseDefend + addDefend;
                }
                else
                {
                    return GetDefensiveValue(driver);
                }
            }

            public string GetSkillName()
            {
                return this.skill.skillName;
            }

            //public long getVolumeOrBussiness(Manager_Driver.ConfuseManger.AmbushInfomation ambushInfomation)
            //{
            //    return ambushInfomation.volumeValue;
            //}

            public long ImproveAttack(RoleInGame role, long attackMoney, ref List<string> notifyMsgs)
            {
                return attackMoney;
            }

            public long leftValue(AbilityAndState ability)
            {
                return ability.leftVolume;
            }

            public void MagicAnimateShow(RoleInGame player, RoleInGame victim, ref List<string> notifyMsgs)
            {
                switch (this.skill.skillEnum)
                {
                    case SkillEnum.Fire:
                        {
                            player.fireMagicChanged(player, victim, ref notifyMsgs);
                        }; break;
                    case SkillEnum.Water:
                        {
                            player.waterMagicChanged(player, victim, ref notifyMsgs);
                        }; break;
                    case SkillEnum.Electic:
                        {
                            player.electricMagicChanged(player, victim, ref notifyMsgs);
                        }; break;
                }
                //throw new NotImplementedException();
            }

            public void setCost(long reduce, RoleInGame player, Car car, ref List<string> notifyMsg)
            {
                car.ability.setCostVolume(car.ability.costVolume + reduce, player, car, ref notifyMsg);
            }
        }
        private commandWithTime.ReturningOjb waterMagic(RoleInGame player, Car car, MagicSkill ms, Skill skill, ref List<string> notifyMsg, out MileResultReason Mrr)
        {
            attackMagicTool et = new attackMagicTool(skill);
            return attackMagic(player, car, ms, et, ref notifyMsg, out Mrr);
        }

        private commandWithTime.ReturningOjb electicMagic(RoleInGame player, Car car, MagicSkill ms, Skill skill, ref List<string> notifyMsg, out MileResultReason Mrr)
        {
            attackMagicTool et = new attackMagicTool(skill);
            return attackMagic(player, car, ms, et, ref notifyMsg, out Mrr);
        }
        private commandWithTime.ReturningOjb attackMagic(RoleInGame player, Car car, MagicSkill ms, interfaceOfHM.AttackMagic am, ref List<string> notifyMsg, out MileResultReason Mrr)
        {
            if (that._Players.ContainsKey(ms.targetOwner))
            {
                var victim = that._Players[ms.targetOwner];
                //
                // string msg;
                bool distanceIsEnoughToStart;
                OssModel.FastonPosition fpResult = null;
                if (player.confuseRecord.IsBeingControlledByLose())
                {
                    if (that.theNearestToPlayerIsCarNotMoney(player, car, victim, out fpResult))
                    {
                        distanceIsEnoughToStart = true;
                    }
                    else
                    {
                        distanceIsEnoughToStart = false;
                    }
                }
                else
                {
                    distanceIsEnoughToStart = true;
                }
                if (distanceIsEnoughToStart)
                    if (car.ability.leftVolume > 0)
                    {
                        that.debtE.setDebt(new commandWithTime.debtOwner()
                        {
                            c = "debtOwner",
                            changeType = commandWithTime.returnning.ChangeType.BeforeTax,
                            costMile = 0,
                            key = player.Key,
                            returningOjb = player.returningOjb,
                            target = player.getCar().targetFpIndex,
                            victim = ms.targetOwner
                        }, am);
                        Mrr = MileResultReason.Abundant;
                        return player.returningOjb;
                    }
                    else
                    {
                        Mrr = MileResultReason.MoneyIsNotEnougt;
                        this.WebNotify(player, "你身上的剩余能量空间不够啦！");
                        return player.returningOjb;
                    }
                else
                {
                    if (fpResult == null)
                    {
                        throw new Exception("逻辑错误！！！");
                    }
                    Mrr = MileResultReason.NearestIsMoneyWhenAttack;
                    that.ViewPosition(player, fpResult, ref notifyMsg);
                    this.WebNotify(player, $"迷惑状态下只能近距离攻击，离【{victim.PlayerName}】最近的是[{fpResult.FastenPositionName}]处的钱，不是你的车，{am.GetSkillName()}失败！");
                    return player.returningOjb;
                }
            }
            else
            {
                throw new Exception("准备运行条件里没有筛查？");
            }
        }

        internal void AmbushSelf(RoleInGame victim, interfaceOfHM.AttackT at, ref List<string> notifyMsg, ref long reduceSumInput)
        {
            // var car = selfRole.getCar();
            victim.confuseRecord.AmbushSelf(victim, that, this, ref notifyMsg, at, ref reduceSumInput, (int i, ref List<string> notifyMsgPass, RoleInGame attacker, ref long reduceSum, bool protecedByDefendMagic) =>
             {
                 if (protecedByDefendMagic)
                 {
                     var car = attacker.getCar();
                     var m = victim.Money - reduceSum;
                     if (m > 0)
                     {
                         var percentValue = that.debtE.getAttackPercentValue(attacker, victim);

                         // var m = victim.Money;
                         var defendReduce = this.that.debtE.DealWithReduceWhenSimulationWithoutDefendMagic(at, attacker, car, victim, percentValue);
                         victim.improvementRecord.reduceDefend(victim, defendReduce, ref notifyMsgPass);
                     }
                 }
                 else
                 {
                     var car = attacker.getCar();
                     var m = victim.Money - reduceSum;
                     if (m > 0)
                     {
                         var percentValue = that.debtE.getAttackPercentValue(attacker, victim);
                         long reduce;
                         // var m = victim.Money;
                         this.that.debtE.DealWithReduceWhenAttack(at, attacker, car, victim, percentValue, ref notifyMsgPass, out reduce, m, ref reduceSum);
                     }
                 }
             });
        }

        /// <summary>
        /// this function only to imitate.not do action！
        /// </summary>
        /// <param name="victim"></param>
        /// <param name="at"></param>
        /// <param name="harmValue"></param>
        internal void AmbushSelf(RoleInGame victim, attackMagicTool at, ref long harmValue)
        {
            harmValue += victim.confuseRecord.AmbushSelf(victim, that, at);
        }
        internal string updateMagic(MagicSkill ms)
        {
            return this.updateAction(this, ms, ms.Key);
        }
        public delegate void SpeedMagicChanged(RoleInGame role, ref List<string> notifyMsgs);
        public delegate void AttackMagicChanged(RoleInGame role, ref List<string> notifyMsgs);
        public delegate void DefenceMagicChanged(RoleInGame role, ref List<string> notifyMsgs);

        public delegate void ConfusePrepareMagicChanged(RoleInGame role, ref List<string> notifyMsgs);
        public delegate void LostPrepareMagicChanged(RoleInGame role, ref List<string> notifyMsgs);
        public delegate void AmbushPrepareMagicChanged(RoleInGame role, ref List<string> notifyMsgs);
        public delegate void ControlPrepareMagicChanged(RoleInGame role, ref List<string> notifyMsgs);

        public delegate void ConfuseMagicChanged(RoleInGame role, ref List<string> notifyMsgs);

        public delegate void FireMagicChanged(RoleInGame player, RoleInGame victim, ref List<string> notifyMsgs);
        public delegate void WaterMagicChanged(RoleInGame player, RoleInGame victim, ref List<string> notifyMsgs);
        public delegate void ElectricMagicChanged(RoleInGame player, RoleInGame victim, ref List<string> notifyMsgs);

        public void newThreadDo(commandWithTime.baseC dObj)
        {
            if (dObj.c == "speedSet")
            {
                commandWithTime.speedSet ss = (commandWithTime.speedSet)dObj;
                List<string> notifyMsg = new List<string>();
                //  bool needUpdatePlayers = false;
                lock (that.PlayerLock)
                {
                    var player = that._Players[ss.key];
                    var car = that._Players[ss.key].getCar();
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
                            //attackTool at = new attackTool();
                            // var attackMoney = car.ability.Business;
                            if (that._Players.ContainsKey(ss.beneficiary))
                            {
                                var beneficiary = that._Players[ss.beneficiary];
                                if (!beneficiary.Bust)
                                {
                                    if (beneficiary.Money >= 4 * player.getCar().ability.leftVolume)
                                    {
                                        long costVolumeValue;
                                        beneficiary.improvementRecord.addSpeed(beneficiary, player.getCar().ability.leftVolume, out costVolumeValue, ref notifyMsg);
                                        if (costVolumeValue > 0)
                                        {
                                            this.WebNotify(beneficiary, $"【{player.PlayerName}】提高了你的速度，你向其支付{(costVolumeValue / 100).ToString()}.{(costVolumeValue % 100).ToString()}银两。");
                                            beneficiary.MoneySet(beneficiary.Money - costVolumeValue, ref notifyMsg);
                                            car.ability.setCostVolume(car.ability.costVolume + costVolumeValue, player, car, ref notifyMsg);
                                            this.WebNotify(player, $"你提高了【{player.PlayerName}】的速度，其向你的司机支付{(costVolumeValue / 100).ToString()}.{(costVolumeValue % 100).ToString()}银两。");
                                        }
                                        else
                                        {
                                            this.WebNotify(player, $"【{player.PlayerName}】状态已满！");
                                        }
                                    }
                                    else
                                    {
                                        this.WebNotify(player, $"【{player.PlayerName}】资金匮乏！");
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
                                    key = ss.key,
                                    // car = dOwner.car,
                                    //returnPath = dOwner.returnPath,//returnPath_Record,
                                    target = ss.target,
                                    changeType = ss.changeType,
                                    returningOjb = ss.returningOjb
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
                    //Consol.WriteLine($"url:{url}");
                    Startup.sendMsg(url, sendMsg);
                }
            }
            else if (dObj.c == "attackSet")
            {
                commandWithTime.attackSet ats = (commandWithTime.attackSet)dObj;
                List<string> notifyMsg = new List<string>();
                //  bool needUpdatePlayers = false;
                lock (that.PlayerLock)
                {
                    var player = that._Players[ats.key];
                    var car = that._Players[ats.key].getCar();
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
                            //attackTool at = new attackTool();
                            // var attackMoney = car.ability.Business;
                            if (that._Players.ContainsKey(ats.beneficiary))
                            {
                                var beneficiary = that._Players[ats.beneficiary];
                                if (!beneficiary.Bust)
                                {
                                    if (beneficiary.Money >= 4 * player.getCar().ability.leftBusiness)
                                    {
                                        long costBusinessValue;
                                        beneficiary.improvementRecord.addAttack(beneficiary, player.getCar().ability.leftBusiness, out costBusinessValue, ref notifyMsg);
                                        if (costBusinessValue > 0)
                                        {
                                            this.WebNotify(beneficiary, $"【{player.PlayerName}】提高了你的业务能力，你向其支付{(costBusinessValue / 100).ToString()}.{(costBusinessValue % 100).ToString()}银两。");
                                            beneficiary.MoneySet(beneficiary.Money - costBusinessValue, ref notifyMsg);
                                            car.ability.setCostVolume(car.ability.costBusiness + costBusinessValue, player, car, ref notifyMsg);
                                            this.WebNotify(player, $"你提高了【{player.PlayerName}】的速度，其向你的司机支付{(costBusinessValue / 100).ToString()}.{(costBusinessValue % 100).ToString()}银两。");
                                        }
                                        else
                                        {
                                            this.WebNotify(player, $"【{player.PlayerName}】状态已满！");
                                        }
                                    }
                                    else
                                    {
                                        this.WebNotify(player, $"【{player.PlayerName}】资金匮乏！");
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
                                    key = ats.key,
                                    // car = dOwner.car,
                                    //returnPath = dOwner.returnPath,//returnPath_Record,
                                    target = ats.target,
                                    changeType = ats.changeType,
                                    returningOjb = ats.returningOjb
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
                    //Consol.WriteLine($"url:{url}");
                    Startup.sendMsg(url, sendMsg);
                }
            }
            else if (dObj.c == "defenseSet")
            {
                commandWithTime.defenseSet ats = (commandWithTime.defenseSet)dObj;
                List<string> notifyMsg = new List<string>();
                //  bool needUpdatePlayers = false;
                lock (that.PlayerLock)
                {
                    var player = that._Players[ats.key];
                    var car = that._Players[ats.key].getCar();
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
                            //attackTool at = new attackTool();
                            // var attackMoney = car.ability.Business;
                            if (that._Players.ContainsKey(ats.beneficiary))
                            {
                                var beneficiary = that._Players[ats.beneficiary];
                                if (!beneficiary.Bust)
                                {
                                    if (beneficiary.Money >= 4 * player.getCar().ability.leftVolume)
                                    {
                                        long costVolumeValue;
                                        beneficiary.improvementRecord.addDefence(beneficiary, player.getCar().ability.leftVolume, out costVolumeValue, ref notifyMsg);
                                        if (costVolumeValue > 0)
                                        {
                                            this.WebNotify(beneficiary, $"【{player.PlayerName}】提高了你的防御能力，你向其支付{(costVolumeValue / 100).ToString()}.{(costVolumeValue % 100).ToString()}银两。");
                                            beneficiary.MoneySet(beneficiary.Money - costVolumeValue, ref notifyMsg);
                                            car.ability.setCostVolume(car.ability.costBusiness + costVolumeValue, player, car, ref notifyMsg);
                                            this.WebNotify(player, $"你提高了【{player.PlayerName}】的防御能力，其向你的司机支付{(costVolumeValue / 100).ToString()}.{(costVolumeValue % 100).ToString()}银两。");
                                        }
                                        else
                                        {
                                            this.WebNotify(player, $"【{player.PlayerName}】状态已满！");
                                        }
                                    }
                                    else
                                    {
                                        this.WebNotify(player, $"【{player.PlayerName}】资金匮乏！辅助失败");
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
                                    key = ats.key,
                                    // car = dOwner.car,
                                    //returnPath = dOwner.returnPath,//returnPath_Record,
                                    target = ats.target,
                                    changeType = ats.changeType,
                                    returningOjb = ats.returningOjb
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
                    //Consol.WriteLine($"url:{url}");
                    Startup.sendMsg(url, sendMsg);
                }
            }
        }
    }
}
