using CommonClass;
using CommonClass.driversource;
using HouseManager4_0.RoomMainF;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.Xml;
using static HouseManager4_0.Car;
using static HouseManager4_0.RoomMainF.RoomMain;
using OssModel = Model;

namespace HouseManager4_0
{
    public class Engine_MagicEngine : Engine, interfaceOfEngine.engine, interfaceOfEngine.tryCatchAction
    {
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
                            }; break;
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
                else if (ms.targetOwner == ms.Key)
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
                        }; break;
                    default:
                        {
                            Mrr = MileResultReason.NearestIsMoneyWhenAttack;
                            return player.returningOjb;
                        }
                }


            }
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
                            return Manager_Driver.ConfuseManger.ControlAttackType.Lose;
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
                    if (car.ability.leftVolume > 0)
                    {
                        if (car.state == CarState.waitOnRoad)
                        {
                            switch (ct.GetAttackType())
                            {
                                case Manager_Driver.ConfuseManger.ControlAttackType.Confuse:
                                case Manager_Driver.ConfuseManger.ControlAttackType.Lose:
                                    victim.confuseRecord.AddControlInfo(player, Program.dt.GetFpByIndex(car.targetFpIndex), car.ability.leftVolume, ct.GetAttackType());
                                    break;
                                case Manager_Driver.ConfuseManger.ControlAttackType.Ambush:
                                    victim.confuseRecord.AddAmbushInfo(player, Program.dt.GetFpByIndex(car.targetFpIndex), car.ability.leftBusiness, car.ability.leftVolume);
                                    break;
                                default:
                                    throw new Exception("程序意料之外！");
                            }
                            this.WebNotify(player, $"已经部署{ct.GetName()}计谋，等其车辆返回基地后开始实施！");
                        }
                        else
                            throw new Exception("运行此方法，没有正确校验");
                        //player.confuseUsing = victim.confuseRecord;
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
                    Mrr = MileResultReason.NearestIsMoneyWhenAttack;
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

        class attackMagicTool : interfaceOfHM.AttackMagic
        {
            public Skill skill;

            public attackMagicTool(Skill skill_)
            {
                this.skill = skill_;
            }

            public bool CheckCarState(Car car)
            {
                return car.state == CarState.waitOnRoad;
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

            public string GetSkillName()
            {
                return this.skill.skillName;
            }

            public long getVolumeOrBussiness(Manager_Driver.ConfuseManger.AmbushInfomation ambushInfomation)
            {
                return ambushInfomation.volumeValue;
            }

            public long leftValue(AbilityAndState ability)
            {
                return ability.leftVolume;
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
                OssModel.FastonPosition fpResult;
                var distanceIsEnoughToStart = that.theNearestToPlayerIsCarNotMoney(player, car, victim, out fpResult);
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
                    Mrr = MileResultReason.NearestIsMoneyWhenAttack;
                    this.WebNotify(player, $"离【{victim.PlayerName}】最近的是[{fpResult.FastenPositionName}]处的钱，不是你的车，{am.GetSkillName()}失败！");
                    return player.returningOjb;
                }
            }
            else
            {
                throw new Exception("准备运行条件里没有筛查？");
            }
        }

        internal void AmbushSelf(RoleInGame victim, interfaceOfHM.AttackT at, ref List<string> notifyMsg, ref long mResult, ref long reduceSumResult)
        {
            long m = mResult;
            long reduceSum = reduceSumResult;
            // var car = selfRole.getCar();
            victim.confuseRecord.AmbushSelf(victim, that, this, ref notifyMsg, at, (int i, ref List<string> notifyMsgPass, RoleInGame selfRole) =>
             {
                 if (m > 0)
                 {
                     long reduce;
                     // var m = victim.Money;
                     {
                         var attackMoney = at.getVolumeOrBussiness(victim.confuseRecord.ambushInfomations[i]) * (100 - at.GetDefensiveValue(victim.getCar().ability.driver)) / 100;
                         reduce = attackMoney;
                         if (reduce > m)
                         {
                             reduce = m;
                         }
                     }
                     reduce = Math.Max(1, reduce);

                     at.setCost(reduce, selfRole, selfRole.getCar(), ref notifyMsgPass);
                     // car.ability.setCostVolume(car.ability.costVolume + reduce, selfRole, car, ref notifyMsgPass);
                     this.WebNotify(selfRole, $"你对【{victim.PlayerName}】进行了{at.GetSkillName()}，获得{(reduce / 100.00).ToString("f2")}金币。其还有{(victim.Money / 100.00).ToString("f2")}金币。");
                     this.WebNotify(victim, $"【{selfRole.PlayerName}】对你进行了{at.GetSkillName()}，损失{(reduce / 100.00).ToString("f2")}金币 ");
                     m -= reduce;
                     reduceSum += reduce;
                 }
             });
            mResult = m;
            reduceSumResult = reduceSum;
        }

        internal string updateMagic(MagicSkill ms)
        {
            return this.updateAction(this, ms, ms.Key);
        }
    }
}
