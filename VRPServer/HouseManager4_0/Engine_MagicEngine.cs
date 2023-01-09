using CommonClass;
using CommonClass.driversource;
using HouseManager4_0.interfaceOfEngine;
using HouseManager4_0.RoomMainF;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Security.Cryptography.Xml;
using static HouseManager4_0.Car;
using static HouseManager4_0.Engine_MagicEngine;
using static HouseManager4_0.Manager_Driver.ConfuseManger;
using static HouseManager4_0.RoomMainF.RoomMain;
using OssModel = Model;

namespace HouseManager4_0
{
    public partial class Engine_MagicEngine : Engine_ContactEngine, interfaceOfEngine.engine, interfaceOfEngine.tryCatchAction, interfaceOfEngine.startNewThread
    {


        public Engine_MagicEngine(RoomMain roomMain)
        {
            this.roomMain = roomMain;
        }

        public bool carAbilitConditionsOk(RoleInGame player, Car car, Command c, GetRandomPos grp)
        {
            if (c.c == "MagicSkill")
            //  if (car.ability.leftVolume > 0)
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
                            if (car.ability.leftVolume > 0)
                            {
                                var state = CheckTargetCanBeElecticMagiced(player, ms.targetOwner);
                                switch (state)
                                {
                                    case CarStateForBeMagiced.CanBeMagiced:
                                        {
                                            return true;
                                        };
                                    case CarStateForBeMagiced.IsBeingChallenged:
                                        {
                                            this.WebNotify(player, "施法的对象正在被别的玩家挑战！");
                                            return false;
                                        };
                                    case CarStateForBeMagiced.HasBeenBust:
                                        {
                                            this.WebNotify(player, "施法的对象已破产！");
                                            return false;
                                        };
                                    case CarStateForBeMagiced.IsGroupMate:
                                        {
                                            this.WebNotify(player, "不能对在线队友施法！");
                                            return false;
                                        };
                                    case CarStateForBeMagiced.LevelIsLow:
                                        {
                                            this.WebNotify(player, "等级低被忽略");
                                            return false;
                                        };
                                    case CarStateForBeMagiced.NotBoss:
                                        {
                                            this.WebNotify(player, "你又不是团队老大");
                                            return false;
                                        }
                                    case CarStateForBeMagiced.NotExisted:
                                        {
                                            this.WebNotify(player, "目标不存在！");
                                            return false;
                                        };
                                    default: return false;
                                }
                            }
                            else
                            {
                                this.WebNotify(player, "小车已经没有多余业务容量！");
                                return false;
                            }
                        };
                    case SkillEnum.Ambush:
                    case SkillEnum.Confusion:
                    case SkillEnum.Lose:
                        {
                            if (car.ability.leftVolume > 0)
                            {
                                var state = CheckTargetCanBeControleMagiced(player, ms.targetOwner);
                                switch (state)
                                {
                                    case CarStateForBeMagiced.CanBeMagiced:
                                        {
                                            return true;
                                        };
                                    case CarStateForBeMagiced.IsBeingChallenged:
                                        {
                                            this.WebNotify(player, "施法的对象正在被别的玩家挑战！");
                                            return false;
                                        };
                                    case CarStateForBeMagiced.HasBeenBust:
                                        {
                                            this.WebNotify(player, "施法的对象已破产！");
                                            return false;
                                        };
                                    case CarStateForBeMagiced.IsGroupMate:
                                        {
                                            this.WebNotify(player, "不能对在线队友施法！");
                                            return false;
                                        };
                                    case CarStateForBeMagiced.LevelIsLow:
                                        {
                                            this.WebNotify(player, "等级低被忽略");
                                            return false;
                                        };
                                    case CarStateForBeMagiced.NotBoss:
                                        {
                                            this.WebNotify(player, "你又不是团队老大");
                                            return false;
                                        }
                                    case CarStateForBeMagiced.NotExisted:
                                        {
                                            this.WebNotify(player, "目标不存在！");
                                            return false;
                                        };
                                    default: return false;
                                }
                            }
                            else
                            {
                                this.WebNotify(player, "小车已经没有多余业务容量！");
                                return false;
                            }

                        };
                    case SkillEnum.Speed:
                    case SkillEnum.Defense:
                        {
                            if (car.ability.leftVolume > 0)
                            {
                                var state = CheckTargetCanBeImprovedByMagic(player, ms.targetOwner);
                                switch (state)
                                {
                                    case CarStateForBeMagiced.CanBeMagiced:
                                        {
                                            return true;
                                        };
                                    case CarStateForBeMagiced.IsNotGroupMate:
                                        {
                                            this.WebNotify(player, $"只能对自己或队友进行{skill.skillName}！");
                                            return false;
                                        };
                                    case CarStateForBeMagiced.HasBeenBust:
                                        {
                                            this.WebNotify(player, "提升的对象已经破产！");
                                            return false;
                                        }
                                    case CarStateForBeMagiced.NotExisted:
                                        {
                                            this.WebNotify(player, "提升的对象已经退出游戏！");
                                            return false;
                                        };
                                    default: return false;
                                }
                            }
                            else
                            {
                                this.WebNotify(player, "小车已经没有多余业务容量！");
                                return false;
                            }
                        };
                    case SkillEnum.Attack:
                        {
                            if (car.ability.leftBusiness > 0)
                            {
                                var state = CheckTargetCanBeImprovedByMagic(player, ms.targetOwner);
                                switch (state)
                                {
                                    case CarStateForBeMagiced.CanBeMagiced:
                                        {
                                            return true;
                                        };
                                    case CarStateForBeMagiced.IsNotGroupMate:
                                        {
                                            this.WebNotify(player, $"只能对自己或队友进行{skill.skillName}！");
                                            return false;
                                        };
                                    case CarStateForBeMagiced.HasBeenBust:
                                        {
                                            this.WebNotify(player, "提升的对象已经破产！");
                                            return false;
                                        }
                                    case CarStateForBeMagiced.NotExisted:
                                        {
                                            this.WebNotify(player, "提升的对象已经退出游戏！");
                                            return false;
                                        };
                                    default: return false;
                                }
                            }
                            else
                            {
                                this.WebNotify(player, "小车已经没有多余普攻容量！");
                                return false;
                            }
                        };
                    default:
                        {
                            return false;
                        }
                }


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



        private CarStateForBeMagiced CheckTargetCanBeImprovedByMagic(RoleInGame role, string targetOwner)
        {
            if (role.playerType == RoleInGame.PlayerType.player)
            {
                var player = (Player)role;
                if (roomMain._Players.ContainsKey(targetOwner))
                {
                    if (roomMain._Players[targetOwner].Bust)
                    {
                        return CarStateForBeMagiced.HasBeenBust;
                    }
                    else
                    {
                        if (roomMain._Players[targetOwner].playerType == RoleInGame.PlayerType.NPC)
                        {
                            return CarStateForBeMagiced.IsNotGroupMate;
                        }
                        else
                        {
                            var targetPlayer = (Player)roomMain._Players[targetOwner];
                            if (that.isAtTheSameGroup(player.Key, targetPlayer.Key))
                            {
                                return CarStateForBeMagiced.CanBeMagiced;
                            }
                            else
                                return CarStateForBeMagiced.IsNotGroupMate;

                        }
                    }
                }
                else
                {
                    return CarStateForBeMagiced.NotExisted;
                }
            }
            else if (role.playerType == RoleInGame.PlayerType.NPC)
            {
                return CarStateForBeMagiced.CanBeMagiced;
            }
            else
            {
                throw new Exception("错误！");
            }
        }

        enum CarStateForBeMagiced
        {
            CanBeMagiced,
            NotExisted,
            HasBeenBust,
            LevelIsLow,
            NotBoss,
            IsBeingChallenged,
            IsGroupMate,
            IsNotGroupMate,
        }

        public bool conditionsOk(Command c, GetRandomPos grp, out string reason)
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

        public void failedThenDo(Car car, RoleInGame player, Command c, GetRandomPos grp, ref List<string> notifyMsg)
        {
            if (c.c == "MagicSkill")
            {
                MagicSkill ms = (MagicSkill)c;
                this.carDoActionFailedThenMustReturn(car, player, grp, ref notifyMsg);
            }
        }



        public commandWithTime.ReturningOjb maindDo(RoleInGame player, Car car, Command c, GetRandomPos grp, ref List<string> notifyMsg, out MileResultReason mrr)
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
                        return that.attackE.randomWhenConfused(player, boss, car, sa, grp, ref notifyMsg, out mrr);
                    }
                    else
                    {
                        return magic(player, car, ms, grp, ref notifyMsg, out mrr);
                    }
                }
                else
                    return magic(player, car, ms, grp, ref notifyMsg, out mrr);
            }
            else
            {
                throw new Exception($"数据传输错误！(传出类型为{c.c})");
            }
        }

        private commandWithTime.ReturningOjb magic(RoleInGame player, Car car, MagicSkill ms, GetRandomPos grp, ref List<string> notifyMsg, out MileResultReason Mrr)
        {
            if (player.confuseRecord.IsBeingControlled())
            {

            }
            {
                if (player.playerType == RoleInGame.PlayerType.player)
                {
                    that.taskM.MagicReleased((Player)player);
                }
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
                            return electicMagic(player, car, ms, skill, grp, ref notifyMsg, out Mrr);
                        };
                    case CommonClass.driversource.SkillEnum.Water:
                        {
                            return waterMagic(player, car, ms, skill, grp, ref notifyMsg, out Mrr);
                        };
                    case SkillEnum.Fire:
                        {
                            return fireMagic(player, car, ms, skill, grp, ref notifyMsg, out Mrr);
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
                            return improveSpeedMagic(player, car, ms, skill, grp, ref notifyMsg, out Mrr);
                        };
                    case SkillEnum.Attack:
                        {
                            return improveAttackMagic(player, car, ms, skill, grp, ref notifyMsg, out Mrr);
                        };
                    case SkillEnum.Defense:
                        {
                            return improveDefenseMagic(player, car, ms, skill, grp, ref notifyMsg, out Mrr);
                        };
                    default:
                        {
                            throw new Exception("意料之外！");
                        }
                }


            }
        }

        /// <summary>
        /// 代表加防法术，对潜伏的增益值
        /// </summary>
        internal const int AmbushPropertyByDefendMagic = 12;

        /// <summary>
        /// 代表加防法术，对抵抗混乱的增益值
        /// </summary>
        internal const int ConfusePropertyByDefendMagic = 12;

        /// <summary>
        /// 代表加防法术，对抵抗迷失的增益值
        /// </summary>
        internal const int LostPropertyByDefendMagic = 12;

        enum startArriavalThreadCommand
        {
            defenseSet,
            attackSet,
            speedSet
        }
        private void StartArriavalThread(startArriavalThreadCommand command, int startT, int step, RoleInGame player, Car car, MagicSkill ms, int goMile, Node goPath, commandWithTime.ReturningOjb ro, GetRandomPos grp)
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
                                }, this, grp);
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
                                }, this, grp);
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
                                }, this, grp);
                            }; break;
                    }

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
                               this.sendSeveralMsgs(notifyMsg);
                               //string command, int startT, int step, RoleInGame player, Car car, MagicSkill ms, int goMile, Node goPath, commandWithTime.ReturningOjb ro
                               StartArriavalThread(command, newStartT, step, player, car, ms, goMile, goPath, ro, grp);
                           };
                    this.loop(p, step, startT, player, goPath);
                }
            });
            th.Start();
            //Thread th = new Thread(() => setArrive(startT, ));
            //th.Start();
        }


        private commandWithTime.ReturningOjb improveDefenseMagic(RoleInGame player, Car car, MagicSkill ms, Skill skill, GetRandomPos grp, ref List<string> notifyMsg, out MileResultReason Mrr)
        {
            DefenceObj dfo = new DefenceObj(ms, (int startT, Car car, MagicSkill ms2, int goMile, Node goPath, commandWithTime.ReturningOjb ro) =>
            {
                List<string> notifyMsg = new List<string>();
                car.setState(player, ref notifyMsg, CarState.working);
                this.sendSeveralMsgs(notifyMsg);
                this.StartArriavalThread(startArriavalThreadCommand.defenseSet, startT, 0, player, car, ms2, goMile, goPath, ro, grp);
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
            return this.contact(player, car, dfo, grp, ref notifyMsg, out Mrr);
        }

        private commandWithTime.ReturningOjb improveAttackMagic(RoleInGame player, Car car, MagicSkill ms, Skill skill, GetRandomPos grp, ref List<string> notifyMsg, out MileResultReason Mrr)
        {
            AttackObj ao = new AttackObj(ms, (int startT, Car car, MagicSkill ms1, int goMile, Node goPath, commandWithTime.ReturningOjb ro) =>
            {
                List<string> notifyMsg = new List<string>();
                car.setState(player, ref notifyMsg, CarState.working);
                this.sendSeveralMsgs(notifyMsg);
                this.StartArriavalThread(startArriavalThreadCommand.attackSet, startT, 0, player, car, ms1, goMile, goPath, ro, grp);
            });
            return this.contact(player, car, ao, grp, ref notifyMsg, out Mrr);
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

            public bool carLeftConditions(Car car)
            {
                return car.ability.leftBusiness > 0;
            }

            public void SetArrivalThread(int startT, Car car, int goMile, Node goPath, commandWithTime.ReturningOjb returningOjb)
            {
                this._setAttackImproveArrivalThread(startT, car, this._ms, goMile, goPath, returningOjb);
            }
        }

        //public class AttackObj2 : AttackObj 
        //{
        //    private MagicSkill _ms;
        //}
        partial class SpeedObj : interfaceOfHM.ContactInterface
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

        partial class DefenceObj : interfaceOfHM.ContactInterface
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


        private commandWithTime.ReturningOjb improveSpeedMagic(RoleInGame player, Car car, MagicSkill ms, Skill skill, GetRandomPos grp, ref List<string> notifyMsg, out MileResultReason Mrr)
        {
            //  this.StartArriavalThread(startT, 1, player, car, ms2, goMile, goPath, ro);
            SpeedObj so = new SpeedObj(ms, (int startT, Car car, MagicSkill ms2, int goMile, Node goPath, commandWithTime.ReturningOjb ro) =>
            {
                List<string> notifyMsg = new List<string>();
                car.setState(player, ref notifyMsg, CarState.working);
                this.sendSeveralMsgs(notifyMsg);
                this.StartArriavalThread(startArriavalThreadCommand.speedSet, startT, 0, player, car, ms2, goMile, goPath, ro, grp);
            });
            return this.contact(player, car, so, grp, ref notifyMsg, out Mrr);
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
                var distanceIsEnoughToStart = that.theNearestToPlayerIsCarNotMoney(player, car, victim, Program.dt, out fpResult);
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
                    this.WebNotify(player, $"离【{victim.PlayerName}】最近的是[{fpResult.FastenPositionName}]处的钱，不是你的车，{ct.GetName()}失败！");
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

        private commandWithTime.ReturningOjb fireMagic(RoleInGame player, Car car, MagicSkill ms, Skill skill, GetRandomPos grp, ref List<string> notifyMsg, out MileResultReason Mrr)
        {
            attackMagicTool et = new attackMagicTool(skill, player);
            return attackMagic(player, car, ms, et, grp, ref notifyMsg, out Mrr);
        }

        private commandWithTime.ReturningOjb waterMagic(RoleInGame player, Car car, MagicSkill ms, Skill skill, GetRandomPos grp, ref List<string> notifyMsg, out MileResultReason Mrr)
        {
            attackMagicTool et = new attackMagicTool(skill, player);
            return attackMagic(player, car, ms, et, grp, ref notifyMsg, out Mrr);
        }

        private commandWithTime.ReturningOjb electicMagic(RoleInGame player, Car car, MagicSkill ms, Skill skill, GetRandomPos grp, ref List<string> notifyMsg, out MileResultReason Mrr)
        {
            attackMagicTool et = new attackMagicTool(skill, player);
            return attackMagic(player, car, ms, et, grp, ref notifyMsg, out Mrr);
        }
        private commandWithTime.ReturningOjb attackMagic(RoleInGame player, Car car, MagicSkill ms, interfaceOfHM.AttackMagic am, GetRandomPos grp, ref List<string> notifyMsg, out MileResultReason Mrr)
        {
            if (that._Players.ContainsKey(ms.targetOwner))
            {
                var victim = that._Players[ms.targetOwner];
                //
                // string msg;
                bool distanceIsEnoughToStart;
                OssModel.FastonPosition fpResult = null;
                distanceIsEnoughToStart = true;
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
                        }, am, grp);
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

        internal void AmbushSelf(RoleInGame victim, interfaceOfHM.AttackT at, GetRandomPos grp, ref List<string> notifyMsg, ref long reduceSumInput)
        {
            victim.confuseRecord.AmbushSelf(victim, that, this, ref notifyMsg, at, grp, ref reduceSumInput);
        }



        /// <summary>
        /// this function only to imitate.not do action！
        /// </summary>
        /// <param name="victim"></param>
        /// <param name="at"></param>
        /// <param name="harmValue"></param>
        internal void AmbushSelf(RoleInGame victim, attackMagicTool at, ref long harmValue, GetRandomPos gp)
        {
            harmValue += victim.confuseRecord.AmbushSelf(victim, that, at, gp);
        }
        internal string updateMagic(MagicSkill ms, GetRandomPos grp)
        {
            return this.updateAction(this, ms, grp, ms.Key);
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

        public void newThreadDo(commandWithTime.baseC dObj, GetRandomPos grp)
        {
            interfaceOfHM.ImproveT it;
            //  const int startNewReturnThInteview = 50;
            switch (dObj.c)
            {
                case "speedSet":
                    {
                        commandWithTime.speedSet ss = (commandWithTime.speedSet)dObj;
                        it = new improveSpeedMagicTool(ss);
                    }; break;
                case "defenseSet":
                    {
                        commandWithTime.defenseSet ss = (commandWithTime.defenseSet)dObj;
                        it = new improveDefenceMagicTool(ss);

                    }; break;
                case "attackSet":
                    {
                        commandWithTime.attackSet ss = (commandWithTime.attackSet)dObj;
                        it = new improveAttackMagicTool(ss);

                    }; break;
                default:
                    {
                        it = null;
                    }; break;
            }
            Improved(it, grp);
        }

        internal int GetIgnorePhysicsValue(RoleInGame role)
        {
            return Engine_DebtEngine.attackTool.GetIgnorePhysicsValue(role);
            //throw new NotImplementedException();
        }
    }

    /// <summary>
    /// race immortal
    /// </summary>
    public partial class Engine_MagicEngine
    {

        public partial class attackMagicTool : interfaceOfHM.AttackT
        {
            public Skill skill;
            RoleInGame oprateRole;

            public attackMagicTool(Skill skill_, RoleInGame role_)
            {
                this.skill = skill_;
                this.oprateRole = role_;
            }

            public bool isMagic { get { return true; } }

            public int IgnoreValue
            {
                get
                {
                    int Index;
                    switch (this.skill.skillEnum)
                    {
                        case SkillEnum.Electic:
                            {
                                Index = 2;
                            }; break;
                        case SkillEnum.Fire:
                            {
                                Index = 3;
                            }; break;
                        case SkillEnum.Water:
                            {
                                Index = 4;
                            }; break;
                        default:
                            {
                                return 0;
                            }
                    }
                    if (this.oprateRole.buildingReward[Index] > 0)
                    {
                        return 5 + 25 * this.oprateRole.buildingReward[Index] / 75;
                    }
                    else return 0;
                }
            }

            internal static int GetIgnoreElectricMagicValue(RoleInGame role)
            {
                attackMagicTool at = new attackMagicTool(new Skill(SkillEnum.Electic), role);
                return at.IgnoreValue;
                //  at.
            }
            internal static int GetIgnoreWaterMagicValue(RoleInGame role)
            {
                attackMagicTool at = new attackMagicTool(new Skill(SkillEnum.Water), role);
                return at.IgnoreValue;
            }
            internal static int GetIgnoreFireMagicValue(RoleInGame role)
            {
                attackMagicTool at = new attackMagicTool(new Skill(SkillEnum.Fire), role);
                return at.IgnoreValue;
            }

            public bool CheckCarState(Car car)
            {
                return car.state == CarState.waitOnRoad;
            }
            public long DealWithPercentValue(long percentValue, RoleInGame player, RoleInGame victim, RoomMain that, GetRandomPos grp, ref List<string> notifyMsg)
            {
                switch (this.skill.skillEnum)
                {
                    case SkillEnum.Water:
                        {
                            var carPosition = grp.GetFpByIndex(player.getCar().targetFpIndex);
                            var basePosition = grp.GetFpByIndex(player.StartFPIndex);
                            var targetPosition = grp.GetFpByIndex(victim.StartFPIndex);
                            double carMX, carMY, carMZ;
                            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(carPosition.Longitude, carPosition.Latitde, 0, out carMX, out carMY, out carMZ);

                            double baseMX, baseMY, baseMZ;
                            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(basePosition.Longitude, basePosition.Latitde, 0, out baseMX, out baseMY, out baseMZ);

                            double targetMX, targetMY, targetMZ;
                            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(targetPosition.Longitude, targetPosition.Latitde, 0, out targetMX, out targetMY, out targetMZ);

                            var l1 = Math.Sqrt((carMX - baseMX) * (carMX - baseMX) + (carMY - baseMY) * (carMY - baseMY));
                            if (l1 < 1e-6) break;
                            var c1 = new System.Numerics.Complex((carMX - baseMX) / l1, (carMY - baseMY) / l1);

                            var l2 = Math.Sqrt((targetMX - baseMX) * (targetMX - baseMX) + (targetMY - baseMY) * (targetMY - baseMY));
                            if (l2 < 1e-6) break;
                            var c2 = new System.Numerics.Complex((targetMX - baseMX) / l2, (targetMY - baseMY) / l2);

                            var c3 = c2 / c1;
                            double angle;
                            if (c3.Real <= -1)
                            {
                                angle = 180;
                            }
                            else if (c3.Real < 1)
                            {

                                //percentValue = Convert.ToInt64(-c3.Real * percentValue);
                                angle = Math.Acos(c3.Real) / Math.PI * 180;
                            }
                            else angle = 0;

                            angle = 180 - angle;
                            int efficiency = 0;
                            if (angle < 90)
                            {
                                efficiency = Convert.ToInt32((90 - angle) / 90 * 100);
                                percentValue = Convert.ToInt64(percentValue * (90 - angle) / 90);
                            }
                            else
                            {
                                efficiency = 0;
                                percentValue = 1;
                            }
                            percentValue = Math.Max(1, percentValue);
                            // if (player.playerType == RoleInGame.PlayerType.player)
                            {
                                that.WebNotify(player, $"水法攻击角度为{angle.ToString("F0")}°，攻击效力为{efficiency}%");
                            }
                            that.DrawMagicPolyLine(new double[] {
                                 carMX,carMY,baseMX,baseMY,targetMX,targetMY,
                            }, ref notifyMsg);
                            return percentValue;
                        };
                    case SkillEnum.Electic:
                        {
                            var carPosition = grp.GetFpByIndex(player.getCar().targetFpIndex);
                            var targetPosition = grp.GetFpByIndex(victim.StartFPIndex);
                            double carMX, carMY, carMZ;
                            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(carPosition.Longitude, carPosition.Latitde, 0, out carMX, out carMY, out carMZ);

                            //double baseMX, baseMY, baseMZ;
                            //CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(basePosition.Longitude, basePosition.Latitde, 0, out baseMX, out baseMY, out baseMZ);

                            double targetMX, targetMY, targetMZ;
                            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(targetPosition.Longitude, targetPosition.Latitde, 0, out targetMX, out targetMY, out targetMZ);

                            //var l1 = Math.Sqrt((carMX - baseMX) * (carMX - baseMX) + (carMY - baseMY) * (carMY - baseMY));
                            //if (l1 < 1e-6) break;
                            //var c1 = new System.Numerics.Complex((carMX - baseMX) / l1, (carMY - baseMY) / l1);

                            //var l2 = Math.Sqrt((targetMX - baseMX) * (targetMX - baseMX) + (targetMY - baseMY) * (targetMY - baseMY));
                            //if (l2 < 1e-6) break;

                            var l3 = Math.Sqrt((carMX - targetMX) * (carMX - targetMX) + (carMY - targetMY) * (carMY - targetMY));
                            // if (l2 < 1e-6) break;

                            //  var p = (l1 + l2 + l3) / 2;

                            //  var s = Math.Sqrt(p * (p - l1) * (p - l2) * (p - l3));
                            //if (s == double.NaN) break;

                            //var d = s / l2;
                            var d = l3 / 3;


                            var c2 = new System.Numerics.Complex((targetMX - carMX) / l3, (targetMY - carMY) / l3);
                            var c3 = c2 * new System.Numerics.Complex(0, 1);

                            var A = c3.Real;
                            var B = c3.Imaginary;

                            var x1 = carMX + d * c3.Real;
                            var y1 = carMY + d * c3.Imaginary;

                            var C1 = -(A * x1 + B * y1);

                            var x2 = carMX - d * c3.Real;
                            var y2 = carMY - d * c3.Imaginary;
                            var C2 = -(A * x2 + B * y2);

                            int containItem = 0;
                            foreach (var item in that._collectPosition)
                            {
                                //  var from = Program.dt.GetFpByIndex(player.StartFPIndex);
                                var calItem = Program.dt.GetFpByIndex(item.Value);
                                double calMX, calMY, calMZ;
                                //double targetMX, targetMY, targetMZ;
                                CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(calItem.Longitude, calItem.Latitde, 0, out calMX, out calMY, out calMZ);
                                if (A * calMX + B * calMY + C1 < 0 && A * calMX + B * calMY + C2 > 0)
                                {
                                    percentValue = percentValue * 80 / 100;
                                    containItem++;
                                }
                            }
                            percentValue = Math.Max(1, percentValue);
                            {
                                that.WebNotify(player, $"雷法攻击宽度度为{(d * 2).ToString("F0")}，有{containItem}个点减少雷法威力。");
                            }
                            that.DrawMagicDoubleLine(new double[] {
                                x1 + c2.Real * 10000 ,y1+c2.Imaginary * 10000, x1 - c2.Real * 10000, y1 - c2.Imaginary * 10000,
                                x2 - c2.Real * 10000 ,y2-c2.Imaginary * 10000, x2 + c2.Real * 10000, y2 + c2.Imaginary * 10000
                            }, ref notifyMsg);
                            return percentValue;

                        };
                    case SkillEnum.Fire:
                        {
                            var carPosition = grp.GetFpByIndex(player.getCar().targetFpIndex);
                            //var basePosition = grp.GetFpByIndex(player.StartFPIndex);
                            var targetPosition = grp.GetFpByIndex(victim.StartFPIndex);
                            double carMX, carMY, carMZ;
                            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(carPosition.Longitude, carPosition.Latitde, 0, out carMX, out carMY, out carMZ);

                            //double baseMX, baseMY, baseMZ;
                            //CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(basePosition.Longitude, basePosition.Latitde, 0, out baseMX, out baseMY, out baseMZ);

                            double targetMX, targetMY, targetMZ;
                            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(targetPosition.Longitude, targetPosition.Latitde, 0, out targetMX, out targetMY, out targetMZ);

                            var r = Math.Sqrt((carMX - targetMX) * (carMX - targetMX) + (carMY - targetMY) * (carMY - targetMY));


                            int containItem = 0;
                            foreach (var item in that._collectPosition)
                            {
                                //  var from = Program.dt.GetFpByIndex(player.StartFPIndex);
                                var calItem = Program.dt.GetFpByIndex(item.Value);
                                double calMX, calMY, calMZ;
                                //double targetMX, targetMY, targetMZ;
                                CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(calItem.Longitude, calItem.Latitde, 0, out calMX, out calMY, out calMZ);
                                if ((calMX - targetMX) * (calMX - targetMX) + (calMY - targetMY) * (calMY - targetMY) < r * r)
                                {
                                    percentValue = percentValue * 70 / 100;
                                    containItem++;
                                }
                            }
                            percentValue = Math.Max(1, percentValue);
                            {
                                that.WebNotify(player, $"火法攻击半径为{r.ToString("F0")}，有{containItem}个点减少火法威力。");
                            }

                            var circle = new List<double>();
                            var singleAngle = Math.PI / 90;
                            for (int i = 0; i < 181; i++)
                            {
                                var x = Math.Cos(i * singleAngle) * r + targetMX;
                                var y = Math.Sin(i * singleAngle) * r + targetMY;
                                circle.Add(x);
                                circle.Add(y);
                            }
                            that.DrawMagicCircle(circle.ToArray(), ref notifyMsg);
                            return percentValue;
                        };
                }
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
                    if (this.Ignored())
                    {
                        switch (this.skill.skillEnum)
                        {
                            case SkillEnum.Electic:
                                {
                                    return Math.Max(driver.defensiveOfElectic - this.IgnoreValue, 0);
                                }
                            case SkillEnum.Water:
                                {
                                    return Math.Max(driver.defensiveOfWater - this.IgnoreValue, 0);
                                }
                            case SkillEnum.Fire:
                                {
                                    return Math.Max(driver.defensiveOfFire - this.IgnoreValue, 0);
                                }
                            default:
                                {
                                    return 0;
                                }
                        }
                    }
                    else
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
                    int addDefend = DefenceAttackMagicAdd;
                    if (this.Ignored())
                        return Math.Max(0, baseDefend + addDefend - this.IgnoreValue);
                    else
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

        public partial class attackMagicTool : interfaceOfHM.AttackMagic
        {
            bool _electricIsIgnored = false;
            public bool ElectricIsIgnored()
            {
                return _electricIsIgnored;
            }
            public void IgnoreElectric(ref RoleInGame role, ref System.Random rm)
            {

                var driver = role.getCar().ability.driver;
                if (driver == null) _electricIsIgnored = false;
                else if (role.buildingReward[2] > 0) _electricIsIgnored = true;
                else _electricIsIgnored = false;
            }
            bool _fireIsIgnored = false;
            public bool FireIsIgnored()
            {
                return this._fireIsIgnored;
            }
            public void IgnoreFire(ref RoleInGame role, ref System.Random rm)
            {
                var driver = role.getCar().ability.driver;
                if (driver == null) _fireIsIgnored = false;
                else if (role.buildingReward[3] > 0) _fireIsIgnored = true;
                else _fireIsIgnored = false;
            }
            bool _waterIsIgnored = false;
            public bool WaterIsIgnored()
            {
                return _waterIsIgnored;
            }
            public void IgnoreWater(ref RoleInGame role, ref System.Random rm)
            {
                var driver = role.getCar().ability.driver;
                if (driver == null) _waterIsIgnored = false;
                else if (role.buildingReward[4] > 0) _waterIsIgnored = true;
                else _waterIsIgnored = false;
            }
            public int MagicImprovedProbabilityAndValue(RoleInGame role, ref System.Random rm)
            {
                if (role.getCar().ability.driver == null)
                {
                    return 0;
                }
                else if (role.getCar().ability.driver.race == Race.immortal)
                {
                    var p = Engine_MagicEngine.GetAttackMagicImproveProbability(role);
                    if (rm.Next(0, 100) < p)
                    {
                        return Engine_MagicEngine.GetAttackMagicImproveValue(role);
                    }
                    else
                    {
                        return 0;
                    }
                }
                else return 0;
            }
        }



        public partial class attackMagicTool : interfaceOfHM.AttackIgnore
        {
            public void Ignore(ref RoleInGame attacker, ref System.Random rm)
            {
                switch (this.skill.skillEnum)
                {
                    case SkillEnum.Electic:
                        {
                            this.IgnoreElectric(ref attacker, ref rm);
                        }; break;
                    case SkillEnum.Water:
                        {
                            this.IgnoreWater(ref attacker, ref rm);
                        }; break;
                    case SkillEnum.Fire:
                        {
                            this.IgnoreFire(ref attacker, ref rm);
                        }; break;
                    default:
                        {
                            return;
                        };
                }
            }

            public bool Ignored()
            {
                switch (this.skill.skillEnum)
                {
                    case SkillEnum.Electic:
                        {
                            return this.ElectricIsIgnored();
                        };
                    case SkillEnum.Water:
                        {
                            return this.WaterIsIgnored();
                        };
                    case SkillEnum.Fire:
                        {
                            return this.FireIsIgnored();
                        };
                    default:
                        {
                            return false;
                        };
                }
            }

            public void ReduceIgnore(ref RoleInGame player)
            {

            }
        }
        private CarStateForBeMagiced CheckTargetCanBeElecticMagiced(RoleInGame role, string targetOwner)
        {
            if (role.playerType == RoleInGame.PlayerType.player)
            {
                var player = (Player)role;
                if (roomMain._Players.ContainsKey(targetOwner))
                {
                    if (roomMain._Players[targetOwner].Bust)
                    {
                        return CarStateForBeMagiced.HasBeenBust;
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
                                        return CarStateForBeMagiced.CanBeMagiced;
                                    else
                                        return CarStateForBeMagiced.LevelIsLow;
                                }
                                else
                                {
                                    return CarStateForBeMagiced.NotBoss;
                                }
                            }
                            else
                            {
                                if (player.TheLargestHolderKey == targetNPC.challenger)
                                {
                                    return CarStateForBeMagiced.CanBeMagiced;
                                }
                                else
                                    return CarStateForBeMagiced.IsBeingChallenged;
                            }
                        }
                        else
                        {
                            var targetPlayer = (Player)roomMain._Players[targetOwner];
                            if (targetPlayer.IsOnline())
                            {
                                if (that.isAtTheSameGroup(player.Key, targetPlayer.Key))
                                {
                                    return CarStateForBeMagiced.IsGroupMate;
                                }
                                else if (player.Level >= targetPlayer.Level)
                                    return CarStateForBeMagiced.CanBeMagiced;
                                else
                                    return CarStateForBeMagiced.LevelIsLow;
                            }
                            else
                            {
                                return CarStateForBeMagiced.CanBeMagiced;
                            }
                        }
                    }
                }
                else
                {
                    return CarStateForBeMagiced.NotExisted;
                }
            }
            else if (role.playerType == RoleInGame.PlayerType.NPC)
            {
                return CarStateForBeMagiced.CanBeMagiced;
            }
            else
            {
                throw new Exception("错误！");
            }
        }

    }
    /// <summary>
    /// race people
    /// </summary>
    public partial class Engine_MagicEngine
    {
        internal partial class ambushMagicTool : controleMT, interfaceOfHM.ControlExpand
        {
            //private RoleInGame enemy;
            //private RoleInGame selfRole;
            private interfaceOfHM.AttackT at;
            //  private RoomMain that;
            private long sumHarmValue = 0;
            internal ambushMagicTool(RoleInGame enemy, RoleInGame selfRole, interfaceOfHM.AttackT at, RoomMain that_)
            {
                this.enemy = enemy;
                this.self = selfRole;
                this.at = at;
                this.that = that_;
                this.sumHarmValue = 0;
                this.name = "潜伏";
                this.index = 4;
            }
            public ambushMagicTool(RoleInGame enemy_)
            {
                this.enemy = enemy_;
                this.index = 4;
            }
            //  bool protecedByDefendMagic = false;

            //long attackMoneyBeforeBeingControled = 0;
            //  long attackMoneyAfterBeingControled = 0;

            //long attackMoneyWithDefence = 0;
            long attackMoneyWithoutDefence = 0;
            long harmValue = 0;

            public bool DealWith()
            {
                var selfRole = this.self;
                int defensiveOfAmbush;
                if (selfRole.getCar().ability.driver == null)
                {
                    defensiveOfAmbush = 0;
                }
                else
                {
                    defensiveOfAmbush = selfRole.getCar().ability.driver.defensiveOfAmbush;
                }
                // string name = "潜伏";

                int defendedProbability;
                if (selfRole.improvementRecord.defenceValue > 0)
                {
                    defendedProbability = AmbushPropertyByDefendMagic;
                }
                else
                {
                    defendedProbability = 0;
                }
                var randomValue = that.rm.Next(0, 100);

                /*
                 * example base=90,ignore=25,defence=40,defendedProbability=20
                 * [90+25-40-20,90+25-40]=[55,75]
                 * 在这个区间内是由于受到了保护。
                 * 之所以施法不成功，是因为受到了保护。
                 */
                if (that.magicE.getBaseControlProbability(ControlAttackType.Ambush) + this.IgnoreValue - defensiveOfAmbush - defendedProbability < randomValue)
                {
                    return false;
                }
                else if (that.magicE.getBaseControlProbability(ControlAttackType.Ambush) + this.IgnoreValue - defensiveOfAmbush < randomValue)
                {
                    this.protecedByDefendMagic = true;
                    var car = this.enemy.getCar();
                    var victim = this.self;
                    if (victim.improvementRecord.defenceValue > 0)
                    {
                        this.attackMoneyBeforeBeingControled = (at.leftValue(car.ability) * (100 - at.GetDefensiveValue(victim.getCar().ability.driver, victim.improvementRecord.defenceValue > 0)) / 100);
                    }
                    else
                    {
                        this.attackMoneyBeforeBeingControled = (at.leftValue(car.ability) * (100 - at.GetDefensiveValue(victim.getCar().ability.driver)) / 100);
                    }
                    return false;
                }
                else
                {
                    var car = this.enemy.getCar();
                    var victim = this.self;
                    this.attackMoneyWithoutDefence = (at.leftValue(car.ability) * (100 - at.GetDefensiveValue(victim.getCar().ability.driver)) / 100);
                    if (victim.improvementRecord.defenceValue > 0)
                    {
                        this.harmValue = (at.leftValue(car.ability) * (100 - at.GetDefensiveValue(victim.getCar().ability.driver, victim.improvementRecord.defenceValue > 0)) / 100);
                    }
                    else
                    {
                        this.harmValue = (at.leftValue(car.ability) * (100 - at.GetDefensiveValue(victim.getCar().ability.driver)) / 100);
                    }
                    this.harmValue = Math.Max(1, this.harmValue);
                    return true;
                }
            }
            public override void SetReturn(bool success, GetRandomPos grp, ref List<string> notifyMsg)//enemy success
            {
                if (success)
                {
                    that.WebNotify(this.enemy, $"你对【{this.self.PlayerName}】成功实施了{name}计谋！并造{this.harmValue}点{this.at.GetSkillName()}伤害！");
                    that.WebNotify(this.self, $"【{this.enemy.PlayerName}】对你成功实施了{name}计谋！并造{this.harmValue}点{this.at.GetSkillName()}伤害！");
                    //  that.WebNotify(self, $"【{magicItem.player.PlayerName}】让你陷入了{name}！");
                    // if (this.attackMoneyWithoutDefence - this.attackMoneyWithDefence > 0)
                    {
                        var victim = this.self;
                        var defendReduce = this.attackMoneyWithoutDefence;
                        if (defendReduce > 0)
                            victim.improvementRecord.reduceDefend(victim, defendReduce, ref notifyMsg);
                    }
                    if (this.at.isMagic)
                    {
                        at.MagicAnimateShow(this.enemy, this.self, ref notifyMsg);
                    }

                }
                else
                {

                    if (this.attackMoneyBeforeBeingControled > 0)
                    {
                        //这种情况，说明，防御法术在抵御控制法术过程中，起到了作用。
                        //this.selfRole
                        var victim = this.self;
                        var defendReduce = this.attackMoneyBeforeBeingControled;
                        victim.improvementRecord.reduceDefend(victim, defendReduce, ref notifyMsg);
                    }
                    that.WebNotify(this.enemy, $"你对【{this.self.PlayerName}】实施了{name}计谋，被其识破，未能成功！");
                    that.WebNotify(this.self, $"【{this.enemy.PlayerName}】对你实施了{name}阴谋，被你识破！");
                }
                this.enemy.getCar().setState(enemy, ref notifyMsg, CarState.returning);
                that.retutnE.SetReturnT(500, new commandWithTime.returnning()
                {
                    c = "returnning",
                    changeType = commandWithTime.returnning.ChangeType.BeforeTax,
                    key = this.enemy.Key,
                    returningOjb = this.enemy.returningOjb,
                    target = this.enemy.getCar().targetFpIndex
                }, grp);
            }

            public void SetHarm(ref long reduceSumInput, ref List<string> notifyMsg)
            {
                if (this.harmValue > 0)
                    if (this.self.Money - reduceSumInput > 0)
                    {
                        this.harmValue = Math.Min(this.harmValue, this.self.Money - reduceSumInput);
                        reduceSumInput += this.harmValue;
                        var car = this.enemy.getCar();
                        this.at.setCost(harmValue, this.enemy, car, ref notifyMsg);
                    }
            }

        }
        abstract internal class controleMT
        {
            public interface DWCInput
            {
                bool GetIgnore();
                int GetMagicDefence();
                int GetBaseSuccess();
                int GetDriverDefence();
            }

            protected RoleInGame self;
            protected RoleInGame enemy;
            protected RoomMain that;
            protected ControlInfomation magicItem;
            protected bool protecedByDefendMagic = false;
            protected long attackMoneyBeforeBeingControled = 0;
            protected bool isControled = false;
            protected int index = -1;
            protected string name = "混乱";



            public bool DealWithCommon(DWCInput inputObj)
            {
                int defensiveOfControl;
                if (self.getCar().ability.driver == null)
                {
                    defensiveOfControl = 0;
                }
                else
                {
                    defensiveOfControl = inputObj.GetDriverDefence();//self.getCar().ability.driver.defensiveOfConfuse;
                }
                var randomValue = that.rm.Next(0, 100);
                int defendedProbability;
                if (self.improvementRecord.defenceValue > 0)
                {
                    defendedProbability = inputObj.GetMagicDefence(); //that.magicE.ProtectedByConfuse();
                }
                else
                {
                    defendedProbability = 0;
                }

                if (inputObj.GetBaseSuccess() + this.IgnoreValue - defensiveOfControl - defendedProbability < randomValue)
                {
                    this.isControled = false;
                }
                else if (inputObj.GetBaseSuccess() + this.IgnoreValue - defensiveOfControl < randomValue)
                {
                    this.protecedByDefendMagic = true;
                    var car = this.enemy.getCar();
                    var victim = this.self;
                    if (victim.improvementRecord.defenceValue > 0)
                    {
                        this.attackMoneyBeforeBeingControled = this.enemy.getCar().ability.leftVolume;//(at.leftValue(car.ability) * (100 - at.GetDefensiveValue(victim.getCar().ability.driver, victim.improvementRecord.defenceValue > 0)) / 100);
                    }
                    else
                    {
                        this.attackMoneyBeforeBeingControled = this.enemy.getCar().ability.leftVolume;
                    }
                    this.isControled = false;
                }
                else
                {
                    this.isControled = true;
                }
                return this.isControled;
            }

            bool _controleMagicIsIgnored = false;

            //public int GetIgnoreValue { get; private set; }
            public int IgnoreValue
            {
                get
                {
                    if (this.Ignored())
                    {
                        return Engine_MagicEngine.GetControlIgnore(this.enemy, this.index);
                    }
                    else
                    {
                        return 0;
                    }
                }
            }

            public void Ignore(ref RoleInGame role, ref System.Random rm)
            {
                //throw new Exception();
                //var driver = role.getCar().ability.driver;
                //if (driver == null) _controleMagicIsIgnored = false;
                //else if (role.buildingReward[index] > 0) _controleMagicIsIgnored = true;
                //else _controleMagicIsIgnored = false;
            }
            public bool Ignored()
            {
                var role = this.enemy;
                var driver = role.getCar().ability.driver;
                if (driver == null) _controleMagicIsIgnored = false;
                else if (role.buildingReward[index] > 0) _controleMagicIsIgnored = true;
                else _controleMagicIsIgnored = false;
                return this._controleMagicIsIgnored;
            }
            public void Ignore(ref System.Random rm)
            {
                this.Ignore(ref this.enemy, ref rm);
            }
            public bool MagicDouble(ref System.Random rm)
            {
                var role = this.enemy;
                if (role.getCar().ability.driver == null)
                {
                    return false;
                }
                else if (role.getCar().ability.driver.race == Race.people)
                {
                    return rm.Next(0, 100) < Engine_MagicEngine.GetMagicDoubleProbability(role);
                }
                else return false;
            }
            public void ReduceDouble()
            {
                /*
                 * 这里不进行衰减
                 */
                //var role = this.enemy;
                //role.buildingReward[1] -= 5;
                //if (role.buildingReward[1] < 0)
                //    role.buildingReward[1] = 0;
            }
            public void ReduceIgnore()
            {
                this.ReduceIgnore(ref this.enemy);
            }
            public void ReduceIgnore(ref RoleInGame role)
            {
            }
            public virtual void SetReturn(bool success, GetRandomPos grp, ref List<string> notifyMsg)
            {
                {
                    var enemyCar = this.enemy.getCar();
                    enemyCar.setState(enemy, ref notifyMsg, CarState.returning);
                    that.retutnE.SetReturnT(500, new commandWithTime.returnning()
                    {
                        c = "returnning",
                        changeType = commandWithTime.returnning.ChangeType.BeforeTax,
                        key = magicItem.player.Key,
                        returningOjb = magicItem.player.returningOjb,
                        target = enemyCar.targetFpIndex
                    }, grp);
                }
            }
        }



        internal partial class confuseMagicTool : controleMT, interfaceOfHM.ControlExpand
        {
            //   private string name = "混乱"; 
            public confuseMagicTool(ControlInfomation magicItem_, RoleInGame self_, RoleInGame enemy_, RoomMain that_)
            {
                this.self = self_;
                this.enemy = enemy_;
                this.that = that_;
                this.magicItem = magicItem_;
                this.index = 2;
                this.name = "混乱";
            }
            public confuseMagicTool(RoleInGame enemy_)
            {
                this.enemy = enemy_;
                this.index = 2;
            }
            // this.Gcc
            public bool DealWith()
            {
                return this.DealWithCommon(this);
            }
            public void SetHarm(ref long reduceSumInput, ref List<string> notifyMsg)
            {
                if (this.isControled)
                {
                    that.WebNotify(this.enemy, $"你对【{self.PlayerName}】成功地实施了{name}计谋！");
                    that.WebNotify(self, $"【{this.enemy.PlayerName}】对你成功地实施了{name}计谋！");
                    //self.getCar().ability.driver
                    self.confuseRecord.SelectItem(this.magicItem);
                    self.confuseMagicChanged(self, ref notifyMsg);
                }
                else if (this.protecedByDefendMagic)
                {
                    self.improvementRecord.reduceDefend(self, this.attackMoneyBeforeBeingControled, ref notifyMsg);
                    that.WebNotify(enemy, $"你对【{self.PlayerName}】实施了{name}计谋，被其保护光环阻挡，未能成功！");
                    that.WebNotify(self, $"【{enemy.PlayerName}】对你实施了{name}阴谋，被保护光环阻挡，未能成功！");
                }
                else
                {
                    that.WebNotify(this.enemy, $"你对【{self.PlayerName}】实施了{name}计谋，未得逞！");
                    that.WebNotify(self, $"【{this.enemy.PlayerName}】对你实施了{name}计谋，未得逞！");
                }
            }

        }

        internal partial class confuseMagicTool : controleMT.DWCInput
        {
            public bool GetIgnore()
            {
                return this.Ignored();
                // return this._confuseIsIgnored;
            }
            public int GetMagicDefence()
            {
                return Engine_MagicEngine.ConfusePropertyByDefendMagic;
            }
            public int GetBaseSuccess()
            {
                return that.magicE.getBaseControlProbability(ControlAttackType.Confuse);
            }
            public int GetDriverDefence()
            {
                return this.self.getCar().ability.driver.defensiveOfConfuse;
            }
        }

        internal partial class loseMagicTool : controleMT, interfaceOfHM.ControlExpand
        {
            public loseMagicTool(ControlInfomation magicItem_, RoleInGame self_, RoleInGame enemy_, RoomMain that_)
            {
                this.magicItem = magicItem_;
                this.self = self_;
                this.enemy = enemy_;
                this.that = that_;
                this.index = 3;
                this.name = "迷失";
            }
            public loseMagicTool(RoleInGame enemy_)
            {
                this.enemy = enemy_;
                this.index = 3;
            }

            public bool DealWith()
            {
                return this.DealWithCommon(this);
            }

            public void SetHarm(ref long reduceSumInput, ref List<string> notifyMsg)
            {
                if (this.isControled)
                {
                    that.WebNotify(this.enemy, $"你对【{self.PlayerName}】成功地实施了{name}计谋！");
                    that.WebNotify(self, $"【{this.enemy.PlayerName}】对你成功地实施了{name}计谋！");
                    //self.getCar().ability.driver
                    self.confuseRecord.SelectItem(this.magicItem);
                    self.loseMagicChanged(self, ref notifyMsg);
                }
                else if (this.protecedByDefendMagic)
                {
                    self.improvementRecord.reduceDefend(self, this.attackMoneyBeforeBeingControled, ref notifyMsg);
                    that.WebNotify(enemy, $"你对【{self.PlayerName}】实施了{name}计谋，被其保护光环阻挡，未能成功！");
                    that.WebNotify(self, $"【{enemy.PlayerName}】对你实施了{name}阴谋，被保护光环阻挡，未能成功！");
                }
                else
                {
                    that.WebNotify(this.enemy, $"你对【{self.PlayerName}】实施了{name}计谋，未得逞！");
                    that.WebNotify(self, $"【{this.enemy.PlayerName}】对你实施了{name}计谋，未得逞！");
                }
            }
        }

        internal partial class loseMagicTool : controleMT.DWCInput
        {
            public int GetBaseSuccess()
            {
                return that.magicE.getBaseControlProbability(ControlAttackType.Lost);
            }

            public int GetDriverDefence()
            {
                return this.self.getCar().ability.driver.defensiveOfLose;
            }

            public bool GetIgnore()
            {
                return this.Ignored();
            }

            public int GetMagicDefence()
            {
                return LostPropertyByDefendMagic;
            }
        }

        internal Manager_Driver.ConfuseManger.programResult DealWithControlMagic(interfaceOfHM.ControlExpand ce, GetRandomPos grp, RoleInGame enemy, ref long reduceSumInput)
        {
            List<string> notifyMsg = new List<string>();
            var magicDoublePlayed = ce.MagicDouble(ref Program.rm.rm);
            ce.Ignore(ref Program.rm.rm);
            //var ignore = ce.Ignored();
            //  bool magicDoublePlayed = Program.rm.rm.Next(100) < magicDouble;
            var success = ce.DealWith();
            bool secondMagicPlayed = false;
            if (!success)
            {
                if (magicDoublePlayed)
                {
                    success = ce.DealWith();
                    secondMagicPlayed = true;
                }
            }
            ce.SetHarm(ref reduceSumInput, ref notifyMsg);
            ce.SetReturn(success, grp, ref notifyMsg);
            if (ce.Ignored())
                ce.ReduceIgnore();

            if (magicDoublePlayed && secondMagicPlayed)
            {
                ce.ReduceDouble();
                this.WebNotify(enemy, "故技重施");
            }
            this.sendSeveralMsgs(notifyMsg);


            if (success)
            {
                return programResult.runReturn;
            }
            else
            {
                return programResult.runContinue;
            }
        }
        const int baseConfuseProbability = 90;
        const int baseTemptationProbability = 90;
        const int baseAmbushProbability = 90;
        private int getBaseControlProbability(ControlAttackType attackType)
        {
            switch (attackType)
            {
                case ControlAttackType.Confuse:
                    {
                        return baseConfuseProbability;
                    };
                case ControlAttackType.Lost:
                    {
                        return baseTemptationProbability;
                    };
                case ControlAttackType.Ambush:
                    {
                        return baseAmbushProbability;
                    };
                default:
                    {
                        throw new Exception("");
                    }
            }
        }
        private CarStateForBeMagiced CheckTargetCanBeControleMagiced(RoleInGame role, string targetOwner)
        {
            return this.CheckTargetCanBeElecticMagiced(role, targetOwner);
        }
        internal void TakeHalfMoneyWhenIsControlled(RoleInGame player, Car car, ref List<string> notifyMsg)
        {
            if (player.playerType == RoleInGame.PlayerType.player)
                if (player.confuseRecord.IsBeingControlled())
                    if (car.state == CarState.waitAtBaseStation)
                    {
                        long reduceMoney = 0;
                        if (player.Money > car.ability.Business / 2)
                        {
                            reduceMoney += car.ability.Business / 2;
                            car.ability.setCostBusiness(car.ability.Business / 2, player, car, ref notifyMsg);
                        }
                        if (player.Money - reduceMoney > car.ability.Volume / 2)
                        {
                            reduceMoney += car.ability.Volume / 2;
                            car.ability.setCostVolume(car.ability.Volume / 2, player, car, ref notifyMsg);
                        }
                        if (reduceMoney > 0)
                            player.MoneySet(player.Money - reduceMoney, ref notifyMsg);
                    }
        }
    }


    public partial class Engine_MagicEngine
    {
        public const int DefencePhysicsAdd = 40;

        /// <summary>
        /// 受到防御法术保护的程度。
        /// </summary>
        public const int DefenceAttackMagicAdd = 20;//{ get { return 20; } }
        // public int DefenceControlMagicAdd { get { return 10; } }

        public class improveSpeedMagicTool : interfaceOfHM.ImproveT
        {
            private commandWithTime.speedSet ss;

            public improveSpeedMagicTool(commandWithTime.speedSet ss)
            {
                this.ss = ss;
            }

            public string key { get { return this.ss.key; } }

            public string beneficiaryKey { get { return this.ss.beneficiary; } }

            public int target
            {
                get
                {
                    return this.ss.target;
                }
            }

            public commandWithTime.returnning.ChangeType changeType
            {
                get { return this.ss.changeType; }
            }

            public commandWithTime.ReturningOjb returningOjb
            {
                get
                {
                    return this.ss.returningOjb;
                }
            }

            public void AddValue(int improvedValue, out long costVolumeValue, ref List<string> notifyMsg)
            {
                var beneficiary = Program.rm._Players[this.beneficiaryKey];
                var player = Program.rm._Players[this.key];
                beneficiary.improvementRecord.addSpeed(beneficiary, player.getCar().ability.leftVolume * (100 + improvedValue) / 100, out costVolumeValue, ref notifyMsg);
            }

            //public void AddValue(out long costValue, ref List<string> notifyMsg)
            //{
            //    throw new NotImplementedException();
            //}

            public void AddValueWithMaxV(out long costVolumeValue, ref List<string> notifyMsg)
            {
                var beneficiary = Program.rm._Players[this.beneficiaryKey];
                beneficiary.improvementRecord.addSpeed(beneficiary, beneficiary.Money / 4, out costVolumeValue, ref notifyMsg);
            }

            public void BalanceAccounts(ref List<string> notifyMsg, long costValue, out string[] msgToSelf, out string[] msgToBeneficiary)
            {
                var beneficiary = Program.rm._Players[this.beneficiaryKey];
                var player = Program.rm._Players[this.key];
                var car = player.getCar();
                //this.WebNotify(beneficiary, $"【{player.PlayerName}】提高了你的业务能力，你向其支付{(costBusinessValue / 100).ToString()}.{(costBusinessValue % 100).ToString()}银两。");
                beneficiary.MoneySet(beneficiary.Money - costValue, ref notifyMsg);
                car.ability.setCostVolume(car.ability.costVolume + costValue, player, car, ref notifyMsg);
                //this.WebNotify(player, $"你提高了【{player.PlayerName}】的速度，其向你的司机支付{(costBusinessValue / 100).ToString()}.{(costBusinessValue % 100).ToString()}银两。");
                msgToSelf = new string[1]
                {
                    $"你提高了【{player.PlayerName}】的速度，其向你的司机支付{(costValue / 100).ToString()}.{(costValue % 100).ToString("D2")}银两。"
                };
                msgToBeneficiary = new string[1]
                {
                    $"【{player.PlayerName}】提高了你的速度，你向其支付{(costValue / 100).ToString()}.{(costValue % 100).ToString("D2")}银两。"
                };
            }

            public bool ConditionIsEnoughToReleaseAll(long money, long leftVolume, int ImprovedValue)
            {
                return money / 4 >= leftVolume * (100 + ImprovedValue) / 100;
            }

            const int operateIndex = 2;
            public int MagicImprovedProbabilityAndValue(RoleInGame role, ref System.Random rm)
            {
                if (role.getCar().ability.driver == null)
                {
                    return 0;
                }
                else if (role.getCar().ability.driver.race == Race.devil)
                {
                    var p = Engine_MagicEngine.GetSpeedImproveProbability(role);
                    if (rm.Next(0, 100) < p)
                    {
                        return Engine_MagicEngine.GetSpeedImproveValue(role);
                    }
                    else
                    {
                        return 0;
                    }
                }
                else return 0;
            }

            public void ReduceMagicImprovedValue(RoleInGame role)
            {
                /*
                 * 这里不进行衰减。
                 */
                //role.buildingReward[operateIndex] -= 5;
                //if (role.buildingReward[operateIndex] < 0)
                //{
                //    role.buildingReward[operateIndex] = 0;
                //}
            }

            public bool ConditionIsEnoughToReleaseAll(long money, AbilityAndState ability, int improvedValue)
            {
                return this.ConditionIsEnoughToReleaseAll(money, ability.leftVolume, improvedValue);
                //throw new NotImplementedException();
            }
        }

        public class improveDefenceMagicTool : interfaceOfHM.ImproveT
        {
            private commandWithTime.defenseSet ats;

            public improveDefenceMagicTool(commandWithTime.defenseSet ats)
            {
                this.ats = ats;
            }

            public string key { get { return this.ats.key; } }

            public string beneficiaryKey { get { return this.ats.beneficiary; } }

            public int target { get { return this.ats.target; } }

            public commandWithTime.returnning.ChangeType changeType
            {
                get { return this.ats.changeType; }
            }

            public commandWithTime.ReturningOjb returningOjb
            {
                get { return this.ats.returningOjb; }
            }

            public void AddValue(int improvedValue, out long costValue, ref List<string> notifyMsg)
            {
                var beneficiary = Program.rm._Players[this.beneficiaryKey];
                var player = Program.rm._Players[this.key];
                beneficiary.improvementRecord.addDefence(beneficiary, player.getCar().ability.leftVolume * (100 + improvedValue) / 100, out costValue, ref notifyMsg);
            }

            public void AddValueWithMaxV(out long costValue, ref List<string> notifyMsg)
            {
                var beneficiary = Program.rm._Players[this.beneficiaryKey];
                //   var player = Program.rm._Players[this.key];
                beneficiary.improvementRecord.addDefence(beneficiary, beneficiary.Money / 4, out costValue, ref notifyMsg);
            }

            public void BalanceAccounts(ref List<string> notifyMsg, long costValue, out string[] msgToSelf, out string[] msgToBeneficiary)
            {
                var beneficiary = Program.rm._Players[this.beneficiaryKey];
                var player = Program.rm._Players[this.key];
                var car = player.getCar();
                //this.WebNotify(beneficiary, $"【{player.PlayerName}】提高了你的业务能力，你向其支付{(costBusinessValue / 100).ToString()}.{(costBusinessValue % 100).ToString()}银两。");
                beneficiary.MoneySet(beneficiary.Money - costValue, ref notifyMsg);
                car.ability.setCostVolume(car.ability.costVolume + costValue, player, car, ref notifyMsg);
                //this.WebNotify(player, $"你提高了【{player.PlayerName}】的速度，其向你的司机支付{(costBusinessValue / 100).ToString()}.{(costBusinessValue % 100).ToString()}银两。");
                msgToSelf = new string[1]
                {
                    $"你提高了【{player.PlayerName}】的防御，其向你的司机支付{(costValue / 100).ToString()}.{(costValue % 100).ToString("D2")}银两。"
                };
                msgToBeneficiary = new string[1]
                {
                    $"【{player.PlayerName}】提高了你的防御，你向其支付{(costValue / 100).ToString()}.{(costValue % 100).ToString("D2")}银两。"
                };
            }

            public bool ConditionIsEnoughToReleaseAll(long money, long leftVolume, int ImprovedValue)
            {
                return money / 4 >= leftVolume * (100 + ImprovedValue) / 100;
            }

            const int operateIndex = 3;
            public int MagicImprovedProbabilityAndValue(RoleInGame role, ref System.Random rm)
            {
                if (role.getCar().ability.driver == null)
                {
                    return 0;
                }
                else if (role.getCar().ability.driver.race == Race.devil)
                {
                    var p = Engine_MagicEngine.GetDefenseImproveProbability(role);
                    if (rm.Next(0, 100) < p)
                    {
                        return Engine_MagicEngine.GetDefenseImproveValue(role);
                    }
                    else
                    {
                        return 0;
                    }
                }
                else return 0;
            }

            public void ReduceMagicImprovedValue(RoleInGame role)
            {
                /*
                 * 这里不会缩减
                 */
                //role.buildingReward[operateIndex] -= 5;
                //if (role.buildingReward[operateIndex] < 0)
                //{
                //    role.buildingReward[operateIndex] = 0;
                //};
            }

            public bool ConditionIsEnoughToReleaseAll(long money, AbilityAndState ability, int improvedValue)
            {
                return this.ConditionIsEnoughToReleaseAll(money, ability.leftVolume, improvedValue);
            }
        }

        public class improveAttackMagicTool : interfaceOfHM.ImproveT
        {
            private commandWithTime.attackSet ats;

            public improveAttackMagicTool(commandWithTime.attackSet ats)
            {
                this.ats = ats;
            }

            public string key { get { return this.ats.key; } }

            public string beneficiaryKey { get { return this.ats.beneficiary; } }

            public int target { get { return this.ats.target; } }

            public commandWithTime.returnning.ChangeType changeType { get { return this.ats.changeType; } }

            public commandWithTime.ReturningOjb returningOjb { get { return this.ats.returningOjb; } }

            public void AddValue(int improvedValue, out long costValue, ref List<string> notifyMsg)
            {
                var beneficiary = Program.rm._Players[this.beneficiaryKey];
                var player = Program.rm._Players[this.key];
                beneficiary.improvementRecord.addAttack(beneficiary, player.getCar().ability.leftBusiness * (100 + improvedValue) / 100, out costValue, ref notifyMsg);
            }

            public void AddValueWithMaxV(out long costValue, ref List<string> notifyMsg)
            {
                var beneficiary = Program.rm._Players[this.beneficiaryKey];
                beneficiary.improvementRecord.addAttack(beneficiary, beneficiary.Money / 4, out costValue, ref notifyMsg);
            }

            public void BalanceAccounts(ref List<string> notifyMsg, long costValue, out string[] msgToSelf, out string[] msgToBeneficiary)
            {
                var beneficiary = Program.rm._Players[this.beneficiaryKey];
                var player = Program.rm._Players[this.key];
                var car = player.getCar();
                //this.WebNotify(beneficiary, $"【{player.PlayerName}】提高了你的业务能力，你向其支付{(costBusinessValue / 100).ToString()}.{(costBusinessValue % 100).ToString()}银两。");
                beneficiary.MoneySet(beneficiary.Money - costValue, ref notifyMsg);
                car.ability.setCostBusiness(car.ability.costBusiness + costValue, player, car, ref notifyMsg);
                //this.WebNotify(player, $"你提高了【{player.PlayerName}】的速度，其向你的司机支付{(costBusinessValue / 100).ToString()}.{(costBusinessValue % 100).ToString()}银两。");
                msgToSelf = new string[1]
                {
                    $"你提高了【{player.PlayerName}】的普攻能力，其向你的司机支付{(costValue / 100).ToString()}.{(costValue % 100).ToString("D2")}银两。"
                };
                msgToBeneficiary = new string[1]
                {
                    $"【{player.PlayerName}】提高了你的普攻能力，你向其支付{(costValue / 100).ToString()}.{(costValue % 100).ToString("D2")}银两。"
                };
            }

            public bool ConditionIsEnoughToReleaseAll(long money, long leftV, int ImprovedValue)
            {
                return money / 4 >= leftV * (100 + ImprovedValue) / 100;
            }

            public bool ConditionIsEnoughToReleaseAll(long money, AbilityAndState ability, int improvedValue)
            {
                return ConditionIsEnoughToReleaseAll(money, ability.leftBusiness, improvedValue);
            }
            const int operateIndex = 4;
            public int MagicImprovedProbabilityAndValue(RoleInGame role, ref System.Random rm)
            {
                if (role.getCar().ability.driver == null)
                {
                    return 0;
                }
                else if (role.getCar().ability.driver.race == Race.devil)
                {
                    var p = Engine_MagicEngine.GetAttackImproveProbability(role);
                    if (rm.Next(0, 100) < p)
                    {
                        return Engine_MagicEngine.GetAttackImproveValue(role);
                    }
                    else
                    {
                        return 0;
                    }
                }
                else return 0;
            }

            public void ReduceMagicImprovedValue(RoleInGame role)
            {
                /*
                 * 这里不进行衰减
                 * 一次求福，属性恒久。
                 */
                //role.buildingReward[operateIndex] -= 5;
                //if (role.buildingReward[operateIndex] < 0)
                //{
                //    role.buildingReward[operateIndex] = 0;
                //};
            }
        }
        private void Improved(interfaceOfHM.ImproveT ss, GetRandomPos grp)
        {
            if (ss == null) { return; }
            const int startNewReturnThInteview = 50;
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
                        if (that._Players.ContainsKey(ss.beneficiaryKey))
                        {
                            /*
                             * 收益者
                             */
                            var beneficiary = that._Players[ss.beneficiaryKey];
                            if (!beneficiary.Bust)
                            {
                                var improvedValue = ss.MagicImprovedProbabilityAndValue(player, ref that.rm);
                                if (ss.ConditionIsEnoughToReleaseAll(beneficiary.Money, player.getCar().ability, improvedValue))
                                {
                                    long costValue;
                                    ss.AddValue(improvedValue, out costValue, ref notifyMsg);
                                    if (costValue > 0)
                                    {
                                        string[] msgToSelf, msgToBeneficiary;
                                        ss.BalanceAccounts(ref notifyMsg, costValue, out msgToSelf, out msgToBeneficiary);
                                        for (var i = 0; i < msgToSelf.Length; i++)
                                        {
                                            this.WebNotify(player, msgToSelf[i]);
                                        }
                                        for (var i = 0; i < msgToBeneficiary.Length; i++)
                                        {
                                            this.WebNotify(beneficiary, msgToBeneficiary[i]);
                                        }
                                        if (improvedValue > 0)
                                        {
                                            //ss.ReduceMagicImprovedValue(player);
                                        }
                                    }
                                    else
                                    {
                                        this.WebNotify(player, $"【{player.PlayerName}】状态已满！");
                                    }
                                }
                                else
                                {
                                    long costValue;
                                    ss.AddValueWithMaxV(out costValue, ref notifyMsg);
                                    if (costValue > 0)
                                    {
                                        string[] msgToSelf, msgToBeneficiary;
                                        ss.BalanceAccounts(ref notifyMsg, costValue, out msgToSelf, out msgToBeneficiary);
                                        for (var i = 0; i < msgToSelf.Length; i++)
                                        {
                                            this.WebNotify(player, msgToSelf[i]);
                                        }
                                        for (var i = 0; i < msgToBeneficiary.Length; i++)
                                        {
                                            this.WebNotify(beneficiary, msgToBeneficiary[i]);
                                        }
                                    }
                                    else
                                    {
                                        this.WebNotify(player, $"你已尽最大努力提升【{player.PlayerName}】的能力！");
                                    }
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
                            that.retutnE.SetReturnT(startNewReturnThInteview, new commandWithTime.returnning()
                            {
                                c = "returnning",
                                key = ss.key,
                                // car = dOwner.car,
                                //returnPath = dOwner.returnPath,//returnPath_Record,
                                target = ss.target,
                                changeType = ss.changeType,
                                returningOjb = ss.returningOjb
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
            //List<string> notifyMsg = new List<string>();
            ////  bool needUpdatePlayers = false;
            //lock (that.PlayerLock)
            //{
            //    var player = that._Players[ss.key];
            //    var car = that._Players[ss.key].getCar();
            //    if (car.state == CarState.working)
            //    {
            //        if (car.targetFpIndex == -1)
            //        {
            //            throw new Exception("居然来了一个没有目标的车！！！");
            //        }
            //        else
            //        {
            //            /*
            //             * 当到达地点时，有可能攻击对象不存在。
            //             * 也有可能攻击对象已破产。
            //             * 还有正常情况。
            //             * 这三种情况都要考虑到。
            //             */
            //            //attackTool at = new attackTool();
            //            // var attackMoney = car.ability.Business;
            //            if (that._Players.ContainsKey(ss.beneficiary))
            //            {
            //                var beneficiary = that._Players[ss.beneficiary];
            //                if (!beneficiary.Bust)
            //                {
            //                    if (beneficiary.Money >= 4 * player.getCar().ability.leftVolume)
            //                    {
            //                        long costVolumeValue;

            //                        beneficiary.improvementRecord.addSpeed(beneficiary, player.getCar().ability.leftVolume, out costVolumeValue, ref notifyMsg);
            //                        if (costVolumeValue > 0)
            //                        {
            //                            this.WebNotify(beneficiary, $"【{player.PlayerName}】提高了你的速度，你向其支付{(costVolumeValue / 100).ToString()}.{(costVolumeValue % 100).ToString()}银两。");
            //                            beneficiary.MoneySet(beneficiary.Money - costVolumeValue, ref notifyMsg);
            //                            car.ability.setCostVolume(car.ability.costVolume + costVolumeValue, player, car, ref notifyMsg);
            //                            this.WebNotify(player, $"你提高了【{player.PlayerName}】的速度，其向你的司机支付{(costVolumeValue / 100).ToString()}.{(costVolumeValue % 100).ToString()}银两。");
            //                        }
            //                        else
            //                        {
            //                            this.WebNotify(player, $"【{player.PlayerName}】状态已满！");
            //                        }
            //                    }
            //                    else
            //                    {
            //                        this.WebNotify(player, $"【{player.PlayerName}】资金匮乏！");
            //                    }
            //                }
            //                else
            //                {
            //                    //这种情况也有可能存在。
            //                }

            //            }
            //            else
            //            {
            //                //这种情况有可能存在.
            //            }
            //            /*
            //             * 无论什么情况，直接返回。
            //             */
            //            //  if (car.ability.leftBusiness <= 0 && car.ability.leftVolume <= 0)
            //            {
            //                car.setState(player, ref notifyMsg, CarState.returning);
            //                that.retutnE.SetReturnT(startNewReturnThInteview, new commandWithTime.returnning()
            //                {
            //                    c = "returnning",
            //                    key = ss.key,
            //                    // car = dOwner.car,
            //                    //returnPath = dOwner.returnPath,//returnPath_Record,
            //                    target = ss.target,
            //                    changeType = ss.changeType,
            //                    returningOjb = ss.returningOjb
            //                });

            //            }
            //            //else
            //            //{
            //            //    car.setState(player, ref notifyMsg, CarState.waitOnRoad);
            //            //}
            //        }
            //    }
            //    else
            //    {
            //        throw new Exception("car.state == CarState.buying!或者 dor.changeType不是四种类型");
            //    }
            //}
            //for (var i = 0; i < notifyMsg.Count; i += 2)
            //{
            //    var url = notifyMsg[i];
            //    var sendMsg = notifyMsg[i + 1];
            //    //Consol.WriteLine($"url:{url}");
            //    Startup.sendMsg(url, sendMsg);
            //}
        }
    }

    public partial class Engine_MagicEngine
    {
        //public int GetAttackImprove(RoleInGame role)
        //{
        //    return role.buildingReward[4] / 5;
        //}
        //public static int GetDefenseImprove(RoleInGame role)
        //{
        //    return role.buildingReward[3] / 5;
        //}
        public static int GetAttackMagicImproveProbability(RoleInGame role)
        {
            return role.buildingReward[1];
        }
        public static int GetAttackMagicImproveValue(RoleInGame role)
        {
            var value = 25 + role.buildingReward[1];
            return value;
        }
        public static int GetAttackImproveProbability(RoleInGame role)
        {
            return role.buildingReward[4];
        }
        public static int GetAttackImproveValue(RoleInGame role)
        {
            var value = 25 + role.buildingReward[4];
            return value;
        }
        public static int GetDefenseImproveProbability(RoleInGame role)
        {
            return role.buildingReward[3];
        }
        public static int GetDefenseImproveValue(RoleInGame role)
        {
            var value = 25 + role.buildingReward[3];
            return value;
        }

        public static int GetSpeedImproveProbability(RoleInGame role)
        {
            return role.buildingReward[2];
        }
        public static int GetSpeedImproveValue(RoleInGame role)
        {
            var value = 25 + role.buildingReward[2];
            return value;
        }

        public static int GetMagicDoubleProbability(RoleInGame role)
        {
            return role.buildingReward[1];
        }

        static int GetControlIgnore(RoleInGame role, int index)
        {
            return 3 + 15 * role.buildingReward[index] / 75;
        }
        public static int GetLoseIgnore(RoleInGame role)
        {
            loseMagicTool lmt = new loseMagicTool(role);
            return lmt.IgnoreValue;
        }
        public static int GetAmbushIgnore(RoleInGame role)
        {
            ambushMagicTool lmt = new ambushMagicTool(role);
            return lmt.IgnoreValue;
        }
        public static int GetConfuseIgnore(RoleInGame role)
        {
            confuseMagicTool lmt = new confuseMagicTool(role);
            return lmt.IgnoreValue;
        }
    }
}
