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
    public class Engine_MagicEngine : Engine_ContactEngine, interfaceOfEngine.engine, interfaceOfEngine.tryCatchAction, interfaceOfEngine.startNewThread
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
                    default:
                        {
                            Mrr = MileResultReason.NearestIsMoneyWhenAttack;
                            return player.returningOjb;
                        }
                }


            }
        }

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
            public delegate void SetSpeedImproveArrivalThreadM(int startT, Car car, MagicSkill ms, int goMile, commandWithTime.ReturningOjb ro);
            public void SetArrivalThread(int startT, Car car, int goMile, commandWithTime.ReturningOjb returningOjb)
            {
                this._setSpeedImproveArrivalThread(startT, car, this._ms, goMile, returningOjb);
            }

            public bool carLeftConditions(Car car)
            {
                return car.ability.leftVolume > 0;
            }
        }
        private commandWithTime.ReturningOjb improveSpeedMagic(RoleInGame player, Car car, MagicSkill ms, Skill skill, ref List<string> notifyMsg, out MileResultReason Mrr)
        {
            SpeedObj so = new SpeedObj(ms, (int startT, Car car, MagicSkill ms2, int goMile, commandWithTime.ReturningOjb ro) =>
             {
                 //beneficiary 
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
             });
            return this.contact(player, car, so, ref notifyMsg, out Mrr);
            //if (that._Players.ContainsKey(ms.targetOwner))
            //{
            //    var beneficiary = that._Players[ms.targetOwner];
            //    OssModel.FastonPosition fpResult;
            //    var distanceIsEnoughToStart = that.theNearestToPlayerIsCarNotMoney(player, car, beneficiary, out fpResult);
            //    if (distanceIsEnoughToStart)
            //        if (car.ability.leftVolume > 0)
            //        {
            //            if (car.state == CarState.waitOnRoad)
            //            {
            //                if (beneficiary.Key == player.Key)
            //                {

            //                    //   beneficiary.improvementRecord.addSpeed(player.getCar().ability.leftVolume);
            //                    //   that._Players.
            //                }
            //            }
            //            else
            //                throw new Exception("运行此方法，没有正确校验");
            //            //player.confuseUsing = victim.confuseRecord;
            //            Mrr = MileResultReason.Abundant;
            //            return player.returningOjb;
            //        }
            //        else
            //        {
            //            Mrr = MileResultReason.MoneyIsNotEnougt;
            //            this.WebNotify(player, "你身上的剩余能量空间不够啦！");
            //            return player.returningOjb;
            //        }
            //    else
            //    {
            //        Mrr = MileResultReason.NearestIsMoneyWhenAttack;
            //        this.WebNotify(player, $"离【{beneficiary.PlayerName}】最近的是[{fpResult.FastenPositionName}]处的钱，不是你的车，{ ct.GetName()}失败！");
            //        return player.returningOjb;
            //    }
            //}
            //else
            //{
            //    throw new Exception("准备运行条件里没有筛查？");
            //}
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

        internal void AmbushSelf(RoleInGame victim, interfaceOfHM.AttackT at, ref List<string> notifyMsg, ref long reduceSumInput)
        {
            // var car = selfRole.getCar();
            victim.confuseRecord.AmbushSelf(victim, that, this, ref notifyMsg, at, ref reduceSumInput, (int i, ref List<string> notifyMsgPass, RoleInGame attacker, ref long reduceSum) =>
             {
                 var m = victim.Money - reduceSum;
                 if (m > 0)
                 {
                     long reduce;
                     // var m = victim.Money;
                     {
                         var percentValue = that.debtE.getAttackPercentValue(attacker, victim);
                         var attackMoney = (at.leftValue(attacker.getCar().ability) * (100 - at.GetDefensiveValue(victim.getCar().ability.driver)) / 100) * percentValue / 100;
                         // var attackMoney = at.getVolumeOrBussiness(victim.confuseRecord.ambushInfomations[i]) * (100 - at.GetDefensiveValue(victim.getCar().ability.driver)) / 100;
                         reduce = attackMoney;
                         if (reduce > m)
                         {
                             reduce = m;
                         }
                     }
                     reduce = Math.Max(1, reduce);

                     at.setCost(reduce, attacker, attacker.getCar(), ref notifyMsgPass);
                     // car.ability.setCostVolume(car.ability.costVolume + reduce, selfRole, car, ref notifyMsgPass);
                     this.WebNotify(attacker, $"你对【{victim.PlayerName}】进行了{at.GetSkillName()}，获得{(reduce / 100.00).ToString("f2")}金币。其还有{(victim.Money / 100.00).ToString("f2")}金币。");
                     this.WebNotify(victim, $"【{attacker.PlayerName}】对你进行了{at.GetSkillName()}，损失{(reduce / 100.00).ToString("f2")}金币 ");
                     reduceSum += reduce;
                 }
             });
        }

        internal string updateMagic(MagicSkill ms)
        {
            return this.updateAction(this, ms, ms.Key);
        }
        public delegate void SpeedMagicChanged(RoleInGame role, ref List<string> notifyMsgs);
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
                    Console.WriteLine($"url:{url}");
                    Startup.sendMsg(url, sendMsg);
                }
            }
        }
    }
}
