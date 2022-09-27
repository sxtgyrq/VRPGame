using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HouseManager4_0.RoomMainF
{
    public partial class RoomMain
    {
        /// <summary>
        /// 对npc的attackTag进行编辑，为NPC自动运行提供参考依据。
        /// </summary>
        /// <param name="npc_Operate"></param>
        internal void GetMaxHarmInfomation(NPC npc_Operate, GetRandomPos grp)
        {
            var driver = npc_Operate.getCar().ability.driver;//获取npc的司机；
            npc_Operate.attackTag = null;//初始化自动包。
            if (driver == null)
            {
                /*
                 * 加入npc没有选择司机，那么，只进行业务判断。
                 */
                getBussinessAttack(npc_Operate, grp);
            }
            else if (driver.race == CommonClass.driversource.Race.immortal)
            {
                /*
                 * 选择的司机为输出系，则要考虑业务输出和法术输出。
                 */
                getBussinessAttack(npc_Operate, grp);
                getVolumeAttack(npc_Operate, grp);
            }
            else if (driver.race == CommonClass.driversource.Race.people)
            {
                getBussinessAttack(npc_Operate, grp);
                getControlAttack(npc_Operate);
            }
            else if (driver.race == CommonClass.driversource.Race.devil)
            {
                getBussinessAttack(npc_Operate, grp);
                getImproveHelp(npc_Operate, grp);
            }
        }
        private void getImproveHelp(NPC npc_Operate, GetRandomPos grp)
        {
            //  throw new NotImplementedException();
            var roles = this.getGetAllRoles();
            for (int i = 0; i < roles.Count; i++)
            {
                if (CheckIsPartner(npc_Operate, roles[i]))
                {
                    getItemImproveAttack(npc_Operate, roles[i], grp);
                    getItemImproveSpeed(npc_Operate, roles[i], grp);
                    getItemImproveDefend(npc_Operate, roles[i], grp);
                }
            }
        }

        private void getItemImproveDefend(NPC npc_Operate, RoleInGame roleInGame, GetRandomPos grp)
        {
            if (npc_Operate.getCar().ability.driver.sex == CommonClass.driversource.Sex.woman)
            {
                var car = npc_Operate.getCar();
                var partner = roleInGame;
                Model.FastonPosition fp;
                var harmValue = partner.improvementRecord.simulate.improveDefend(partner, this, car, npc_Operate, grp, out fp);
                if (npc_Operate.attackTag == null || npc_Operate.attackTag.HarmValue < harmValue)
                {
                    npc_Operate.attackTag = new NPC.AttackTag()
                    {
                        aType = NPC.AttackTag.AttackType.defendImprove,
                        HarmValue = harmValue,
                        Target = partner.Key,
                        fpPass = fp
                    };
                }
            }
        }

        private void getItemImproveSpeed(NPC npc_Operate, RoleInGame roleInGame, GetRandomPos grp)
        {
            if (npc_Operate.getCar().ability.driver.sex == CommonClass.driversource.Sex.man)
            {
                var car = npc_Operate.getCar();
                var partner = roleInGame;
                Model.FastonPosition fp;
                var harmValue = partner.improvementRecord.simulate.improveSpeed(partner, this, car, npc_Operate, grp, out fp);
                if (npc_Operate.attackTag == null || npc_Operate.attackTag.HarmValue < harmValue)
                {
                    npc_Operate.attackTag = new NPC.AttackTag()
                    {
                        aType = NPC.AttackTag.AttackType.speed,
                        HarmValue = harmValue,
                        Target = partner.Key,
                        fpPass = fp
                    };
                }
            }
        }

        private void getItemImproveAttack(NPC npc_Operate, RoleInGame roleInGame, GetRandomPos grp)
        {
            //throw new NotImplementedException();
            var car = npc_Operate.getCar();
            var partner = roleInGame;
            Model.FastonPosition fp;
            var harmValue = partner.improvementRecord.simulate.improveAttack(partner, this, car, npc_Operate, grp, out fp);
            if (npc_Operate.attackTag == null || npc_Operate.attackTag.HarmValue < harmValue)
            {
                npc_Operate.attackTag = new NPC.AttackTag()
                {
                    aType = NPC.AttackTag.AttackType.attackImprove,
                    HarmValue = harmValue,
                    Target = partner.Key,
                    fpPass = fp
                };
            }
        }

        private void getControlAttack(NPC npc_Operate)
        {
            var roles = this.getGetAllRoles();
            for (int i = 0; i < roles.Count; i++)
            {
                if (CheckIsEnemy(npc_Operate, roles[i]))
                {
                    getItemAmbushAttack(npc_Operate, roles[i]);
                    getItemConfuseAttack(npc_Operate, roles[i]);
                    getItemLoseAttack(npc_Operate, roles[i]);
                }
            }
        }

        private void getItemLoseAttack(NPC npc_Operate, RoleInGame roleInGame)
        {
            var victim = roleInGame;
            if (victim.confuseRecord.IsBeingControlled()) { }
            else
            {
                var car = npc_Operate.getCar();
                if (car.ability.driver.sex == CommonClass.driversource.Sex.woman)
                {
                    if (victim.confuseRecord.simulate.Lose(victim, this, car, npc_Operate))
                    {
                        var listIndexes = this.getCollectPositionsByDistance(Program.dt.GetFpByIndex(victim.StartFPIndex), Program.dt);
                        var fp = Program.dt.GetFpByIndex(this._collectPosition[listIndexes[0]]);
                        RoleInGame boss;
                        double distance;
                        var fromTarget = Program.dt.GetFpByIndex(npc_Operate.StartFPIndex);
                        var endTarget = fp;
                        if (npc_Operate.HasTheBoss(this._Players, out boss))
                        {
                            var bossPoint = Program.dt.GetFpByIndex(boss.StartFPIndex);
                            distance =
                                CommonClass.Geography.getLengthOfTwoPoint.GetDistance(fromTarget.Latitde, fromTarget.Longitude, fromTarget.Height, endTarget.Latitde, endTarget.Longitude, endTarget.Height)
                                + CommonClass.Geography.getLengthOfTwoPoint.GetDistance(bossPoint.Latitde, bossPoint.Longitude, bossPoint.Height, endTarget.Latitde, endTarget.Longitude, endTarget.Height)
                                + CommonClass.Geography.getLengthOfTwoPoint.GetDistance(bossPoint.Latitde, bossPoint.Longitude, bossPoint.Height, fromTarget.Latitde, fromTarget.Longitude, fromTarget.Height);
                        }
                        else
                        {
                            distance = CommonClass.Geography.getLengthOfTwoPoint.GetDistance(endTarget.Latitde, endTarget.Longitude, endTarget.Height, fromTarget.Latitde, fromTarget.Longitude, fromTarget.Height) * 2;
                        }
                        if (npc_Operate.attackTag == null || npc_Operate.attackTag.aType != NPC.AttackTag.AttackType.lose)
                        {

                            npc_Operate.attackTag = new NPC.AttackTag()
                            {
                                aType = NPC.AttackTag.AttackType.lose,
                                HarmValue = 1 / distance,
                                Target = victim.Key,
                                fpPass = fp
                            };
                        }
                        else if (npc_Operate.attackTag.aType == NPC.AttackTag.AttackType.lose)
                        {
                            var harmValue = 1 / distance;
                            if (harmValue > npc_Operate.attackTag.HarmValue)
                                npc_Operate.attackTag = new NPC.AttackTag()
                                {
                                    aType = NPC.AttackTag.AttackType.lose,
                                    HarmValue = harmValue,
                                    Target = victim.Key,
                                    fpPass = fp
                                };
                        }
                    }

                }


                //var harmValuePerLengthReduced = getharmValuePerLength(roleInGame);
                //// var harmValuePerLengthAdded = getharmValuePerLength(roleInGame);
            }
        }

        private void getItemConfuseAttack(NPC npc_Operate, RoleInGame roleInGame)
        {
            //throw new NotImplementedException();
            var victim = roleInGame;
            if (victim.confuseRecord.IsBeingControlled()) { }
            else
            {
                var car = npc_Operate.getCar();
                if (car.ability.driver.sex == CommonClass.driversource.Sex.man)
                {
                    if (victim.confuseRecord.simulate.confuse(victim, this, car, npc_Operate))
                    {
                        var listIndexes = this.getCollectPositionsByDistance(Program.dt.GetFpByIndex(victim.StartFPIndex), Program.dt);
                        var fp = Program.dt.GetFpByIndex(this._collectPosition[listIndexes[0]]);
                        RoleInGame boss;
                        double distance;
                        var fromTarget = Program.dt.GetFpByIndex(npc_Operate.StartFPIndex);
                        var endTarget = fp;
                        if (npc_Operate.HasTheBoss(this._Players, out boss))
                        {
                            var bossPoint = Program.dt.GetFpByIndex(boss.StartFPIndex);
                            distance =
                                CommonClass.Geography.getLengthOfTwoPoint.GetDistance(fromTarget.Latitde, fromTarget.Longitude, fromTarget.Height, endTarget.Latitde, endTarget.Longitude, endTarget.Height)
                                + CommonClass.Geography.getLengthOfTwoPoint.GetDistance(bossPoint.Latitde, bossPoint.Longitude, bossPoint.Height, endTarget.Latitde, endTarget.Longitude, endTarget.Height)
                                + CommonClass.Geography.getLengthOfTwoPoint.GetDistance(bossPoint.Latitde, bossPoint.Longitude, bossPoint.Height, fromTarget.Latitde, fromTarget.Longitude, fromTarget.Height);
                        }
                        else
                        {
                            distance = CommonClass.Geography.getLengthOfTwoPoint.GetDistance(endTarget.Latitde, endTarget.Longitude, endTarget.Height, fromTarget.Latitde, fromTarget.Longitude, fromTarget.Height) * 2;
                        }
                        if (npc_Operate.attackTag == null || npc_Operate.attackTag.aType != NPC.AttackTag.AttackType.confuse)
                        {

                            npc_Operate.attackTag = new NPC.AttackTag()
                            {
                                aType = NPC.AttackTag.AttackType.confuse,
                                HarmValue = 1 / distance,
                                Target = victim.Key,
                                fpPass = fp
                            };
                        }
                        else if (npc_Operate.attackTag.aType == NPC.AttackTag.AttackType.confuse)
                        {
                            var harmValue = 1 / distance;
                            if (harmValue > npc_Operate.attackTag.HarmValue)
                            {
                                npc_Operate.attackTag = new NPC.AttackTag()
                                {
                                    aType = NPC.AttackTag.AttackType.confuse,
                                    HarmValue = 1 / distance,
                                    Target = victim.Key,
                                    fpPass = fp
                                };
                            }
                        }
                    }

                }
            }
        }

        /// <summary>
        /// 校验是不是敌人。
        /// </summary>
        /// <param name="npc_Operate">npc_，可也理解为self</param>
        /// <param name="roleItem">被识别的对象</param>
        /// <returns>是否为敌人。</returns>
        private bool CheckIsEnemy(NPC npc_Operate, RoleInGame roleItem)
        {
            return roleItem.TheLargestHolderKey == npc_Operate.challenger || roleItem.Key == npc_Operate.challenger;
        }

        private bool CheckIsPartner(NPC npc_Operate, RoleInGame roleItem)
        {
            if (npc_Operate.Key == roleItem.Key)
            {
                return true;
            }
            else if (roleItem.TheLargestHolderKey == npc_Operate.TheLargestHolderKey)
            {
                return true;
            }
            else return false;
            //return roleItem.TheLargestHolderKey == npc_Operate.Key || roleItem.Key == npc_Operate.challenger;
        }

        private void getItemAmbushAttack(NPC npc_Operate, RoleInGame roleInGame)
        {

            var car = npc_Operate.getCar();
            Model.FastonPosition fp;
            var victim = roleInGame;

            if (victim.confuseRecord.simulate.dealWithItem_simulate(victim, this, car, npc_Operate))
            {
                var listIndexes = this.getCollectPositionsByDistance(Program.dt.GetFpByIndex(victim.StartFPIndex), Program.dt);
                fp = Program.dt.GetFpByIndex(this._collectPosition[listIndexes[0]]);
                var longCollectMoney = this.GetCollectReWard(listIndexes[0]) * 100;
                double distance;
                RoleInGame boss;
                var fromTarget = Program.dt.GetFpByIndex(npc_Operate.StartFPIndex);
                var endTarget = fp;
                if (npc_Operate.HasTheBoss(this._Players, out boss))
                {
                    var bossPoint = Program.dt.GetFpByIndex(boss.StartFPIndex);
                    distance =
                        CommonClass.Geography.getLengthOfTwoPoint.GetDistance(fromTarget.Latitde, fromTarget.Longitude, fromTarget.Height, fp.Latitde, fp.Longitude, fp.Height)
                        + CommonClass.Geography.getLengthOfTwoPoint.GetDistance(fp.Latitde, fp.Longitude, fp.Height, endTarget.Latitde, endTarget.Longitude, endTarget.Height)
                        + CommonClass.Geography.getLengthOfTwoPoint.GetDistance(bossPoint.Latitde, bossPoint.Longitude, bossPoint.Height, endTarget.Latitde, endTarget.Longitude, endTarget.Height)
                        + CommonClass.Geography.getLengthOfTwoPoint.GetDistance(bossPoint.Latitde, bossPoint.Longitude, bossPoint.Height, fromTarget.Latitde, fromTarget.Longitude, fromTarget.Height);
                }
                else
                {
                    distance =
                        CommonClass.Geography.getLengthOfTwoPoint.GetDistance(fromTarget.Latitde, fromTarget.Longitude, fromTarget.Height, fp.Latitde, fp.Longitude, fp.Height)
                        + CommonClass.Geography.getLengthOfTwoPoint.GetDistance(fp.Latitde, fp.Longitude, fp.Height, endTarget.Latitde, endTarget.Longitude, endTarget.Height)
                        + CommonClass.Geography.getLengthOfTwoPoint.GetDistance(endTarget.Latitde, endTarget.Longitude, endTarget.Height, fromTarget.Latitde, fromTarget.Longitude, fromTarget.Height);
                }

                var roles = this.getGetAllRoles();
                for (int i = 0; i < roles.Count; i++)
                {
                    if (CheckIsPartner(npc_Operate, roles[i]))
                    {
                        /*
                         * 要是伙伴，才能潜伏攻击。且伙伴必须是输出系。
                         */
                        if (roles[i].getCar().ability.driver != null)
                        {
                            if (roles[i].getCar().ability.driver.race == CommonClass.driversource.Race.immortal)
                            {
                                if (!roles[i].Bust)
                                {
                                    if (!roles[i].confuseRecord.IsBeingControlled())
                                    {
                                        Engine_MagicEngine.attackMagicTool amt = new Engine_MagicEngine.attackMagicTool(roles[i].getCar().ability.driver.skill1);
                                        // var harmValue = 0;//this.debtE.DealWithReduceWhenSimulationWithoutDefendMagic(amt, npc_Operate, car, longCollectMoney, victim, 100);
                                        //  this.GetCollectReWard(item.Key) * 100
                                        var harmValue = npc_Operate.confuseRecord.SimulationToMagicAttack(amt, npc_Operate, car, longCollectMoney, victim, 100);
                                        //var harmValue = this.debtE.SimulationToMagicAttack(amt, npc_Operate, car, longCollectMoney, victim, 100);
                                        var itemHarmValue = harmValue / distance;
                                        if (npc_Operate.attackTag == null || npc_Operate.attackTag.HarmValue < itemHarmValue)
                                        {
                                            npc_Operate.attackTag = new NPC.AttackTag()
                                            {
                                                aType = NPC.AttackTag.AttackType.ambush,
                                                HarmValue = itemHarmValue,
                                                Target = victim.Key,
                                                fpPass = fp
                                            };
                                        }
                                    }
                                }
                            }
                        }
                        //getItemAmbushAttack(npc_Operate, roles[i]);
                        //  roles[i].
                        //Engine_MagicEngine.attackMagicTool at = new Engine_MagicEngine.attackMagicTool(npc_Operate.getCar().ability.driver.skill1);

                        //getItemVolumeAttack(npc_Operate, roles[i], at);
                        //Engine_MagicEngine.attackMagicTool at2 = new Engine_MagicEngine.attackMagicTool(npc_Operate.getCar().ability.driver.skill2);

                        //getItemVolumeAttack(npc_Operate, roles[i], at2);
                    }
                }
            }
            else { }


        }

        /// <summary>
        /// 模拟输出系法术攻击
        /// </summary>
        /// <param name="npc_Operate">被模拟的NPC角色</param>
        private void getVolumeAttack(NPC npc_Operate, GetRandomPos gp)
        {
            var roles = this.getGetAllRoles();
            for (int i = 0; i < roles.Count; i++)
            {
                if (CheckIsEnemy(npc_Operate, roles[i]))
                {

                    Engine_MagicEngine.attackMagicTool at = new Engine_MagicEngine.attackMagicTool(npc_Operate.getCar().ability.driver.skill1);

                    getItemVolumeAttack(npc_Operate, roles[i], at, gp);
                    Engine_MagicEngine.attackMagicTool at2 = new Engine_MagicEngine.attackMagicTool(npc_Operate.getCar().ability.driver.skill2);

                    getItemVolumeAttack(npc_Operate, roles[i], at2, gp);
                }
            }
        }

        private void getItemVolumeAttack(NPC npc_Operate, RoleInGame itemValue, Engine_MagicEngine.attackMagicTool at, GetRandomPos gp)
        {
            foreach (var item in this._collectPosition)
            {
                var centerPosition = gp.GetFpByIndex(item.Value);
                var fromTarget = gp.GetFpByIndex(npc_Operate.StartFPIndex);
                var endTarget = gp.GetFpByIndex(itemValue.StartFPIndex);

                var costVolumn = this.GetCollectReWard(item.Key) * 100;

                RoleInGame boss;
                double distance;
                if (npc_Operate.HasTheBoss(this._Players, out boss))
                {
                    var bossPoint = gp.GetFpByIndex(boss.StartFPIndex);
                    distance =
                        CommonClass.Geography.getLengthOfTwoPoint.GetDistance(fromTarget.Latitde, fromTarget.Longitude, fromTarget.Height, centerPosition.Latitde, centerPosition.Longitude, centerPosition.Height)
                        + CommonClass.Geography.getLengthOfTwoPoint.GetDistance(centerPosition.Latitde, centerPosition.Longitude, centerPosition.Height, endTarget.Latitde, endTarget.Longitude, endTarget.Height)
                        + CommonClass.Geography.getLengthOfTwoPoint.GetDistance(bossPoint.Latitde, bossPoint.Longitude, bossPoint.Height, endTarget.Latitde, endTarget.Longitude, endTarget.Height)
                        + CommonClass.Geography.getLengthOfTwoPoint.GetDistance(bossPoint.Latitde, bossPoint.Longitude, bossPoint.Height, fromTarget.Latitde, fromTarget.Longitude, fromTarget.Height);
                }
                else
                {
                    distance =
                       GetDistance(fromTarget, centerPosition)
                       + GetDistance(centerPosition, endTarget)
                       + GetDistance(fromTarget, endTarget);
                }

                var victim = itemValue;
                long harmValue;
                var car = npc_Operate.getCar();
                if (victim.improvementRecord.defenceValue > 0)
                {
                    harmValue = ((at.leftValue(car.ability) - costVolumn) * (100 - at.GetDefensiveValue(victim.getCar().ability.driver, victim.improvementRecord.defenceValue > 0)) / 100);
                }
                else
                {
                    harmValue = ((at.leftValue(car.ability) - costVolumn) * (100 - at.GetDefensiveValue(victim.getCar().ability.driver)) / 100);
                }
                this.magicE.AmbushSelf(victim, at, ref harmValue, gp);
                //harmValue = at.ImproveAttack(npc_Operate, harmValue);
                var itemHarmValue = harmValue / distance;

                if (npc_Operate.attackTag == null || npc_Operate.attackTag.HarmValue < itemHarmValue)
                {
                    switch (at.skill.skillEnum)
                    {
                        case CommonClass.driversource.SkillEnum.Electic:
                            {
                                npc_Operate.attackTag = new NPC.AttackTag()
                                {
                                    aType = NPC.AttackTag.AttackType.electric,
                                    HarmValue = itemHarmValue,
                                    Target = victim.Key,
                                    fpPass = gp.GetFpByIndex(item.Value)
                                };
                            }; break;
                        case CommonClass.driversource.SkillEnum.Fire:
                            {
                                npc_Operate.attackTag = new NPC.AttackTag()
                                {
                                    aType = NPC.AttackTag.AttackType.fire,
                                    HarmValue = itemHarmValue,
                                    Target = victim.Key,
                                    fpPass = gp.GetFpByIndex(item.Value)
                                };
                            }; break;
                        case CommonClass.driversource.SkillEnum.Water:
                            {
                                npc_Operate.attackTag = new NPC.AttackTag()
                                {
                                    aType = NPC.AttackTag.AttackType.water,
                                    HarmValue = itemHarmValue,
                                    Target = victim.Key,
                                    fpPass = gp.GetFpByIndex(item.Value)
                                };
                            }; break;
                        default:
                            {
                                throw new Exception("输入异常！！！");
                            };
                    }
                }
            }
        }

        private double GetDistance(Model.FastonPosition fromTarget, Model.FastonPosition centerPosition)
        {
            return CommonClass.Geography.getLengthOfTwoPoint.GetDistance(fromTarget.Latitde, fromTarget.Longitude, fromTarget.Height, centerPosition.Latitde, centerPosition.Longitude, centerPosition.Height);
        }

        /// <summary>
        /// 此方法，是获取NPC的业务攻击水平。
        /// </summary>
        /// <param name="npc_Operate">输出的npc</param>
        private void getBussinessAttack(NPC npc_Operate, GetRandomPos grp)
        {
            var roles = this.getGetAllRoles();
            for (int i = 0; i < roles.Count; i++)
            {
                if (this.CheckIsEnemy(npc_Operate, roles[i]))
                {
                    var itemHarmValue = getItemBussinessAttack(npc_Operate, roles[i], grp);
                    if (npc_Operate.attackTag == null || npc_Operate.attackTag.HarmValue < itemHarmValue)
                    {
                        npc_Operate.attackTag = new NPC.AttackTag()
                        {
                            aType = NPC.AttackTag.AttackType.attack,
                            HarmValue = itemHarmValue,
                            Target = roles[i].Key
                        };
                    }
                }
            }
        }
        /// <summary>
        /// 获取单对单伤害。
        /// </summary>
        /// <param name="npc_Operate">输入的npc，理解为self</param>
        /// <param name="itemValue">判断来的对象</param>
        /// <returns></returns>
        private double getItemBussinessAttack(NPC npc_Operate, RoleInGame itemValue, GetRandomPos grp)
        {
            var car = npc_Operate.getCar();//获取小车
            var victim = itemValue;
            Model.FastonPosition fp;
            double distance;
            var fromTarget = grp.GetFpByIndex(npc_Operate.StartFPIndex);
            var endTarget = grp.GetFpByIndex(itemValue.StartFPIndex);
            if (this.theNearestToPlayerIsCarNotMoney(npc_Operate, car, victim, grp, out fp))
            {
                RoleInGame boss;
                if (npc_Operate.HasTheBoss(this._Players, out boss))
                {
                    var bossPoint = grp.GetFpByIndex(boss.StartFPIndex);
                    distance =
                        GetDistance(fromTarget, endTarget) +
                        GetDistance(bossPoint, endTarget) +
                        GetDistance(bossPoint, fromTarget);
                }
                else
                    distance = GetDistance(fromTarget, endTarget) * 2;
            }
            else
            {
                RoleInGame boss;
                if (npc_Operate.HasTheBoss(this._Players, out boss))
                {
                    var bossPoint = grp.GetFpByIndex(boss.StartFPIndex);
                    distance =
                         GetDistance(fromTarget, fp) +
                         GetDistance(endTarget, fp) +
                         GetDistance(bossPoint, endTarget) +
                         GetDistance(bossPoint, fromTarget);
                }
                else
                {
                    distance =
                        GetDistance(fromTarget, fp) +
                        GetDistance(endTarget, fp) +
                         GetDistance(endTarget, fromTarget);
                }
            }
            //  hasValue = true;
            var centerTarget = this._collectPosition[getCollectPositionsByDistance(grp.GetFpByIndex(itemValue.StartFPIndex), grp)[0]];
            var centerPosition = grp.GetFpByIndex(centerTarget);

            var at = new Engine_DebtEngine.attackTool();
            // long 
            long harmValue;

            if (victim.improvementRecord.defenceValue > 0)
            {
                harmValue = (at.leftValue(car.ability) * (100 - at.GetDefensiveValue(victim.getCar().ability.driver, victim.improvementRecord.defenceValue > 0)) / 100);
            }
            else
            {
                harmValue = (at.leftValue(car.ability) * (100 - at.GetDefensiveValue(victim.getCar().ability.driver)) / 100);
            }
            harmValue = at.ImproveAttack(npc_Operate, harmValue);
            var itemHarmValue = harmValue / distance;
            return itemHarmValue;
        }

        internal bool HasEnemy(NPC npc)
        {
            var roles = this.getGetAllRoles();
            for (var i = 0; i < roles.Count; i++)
            {
                if (roles[i].Key == npc.Key)
                {
                    continue;
                }
                else
                {
                    if (CheckIsEnemy(npc, roles[i]))
                    {
                        return true;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            return false;
        }
        public List<RoleInGame> GetAllParner(CommonClass.driversource.Race race, NPC npc)
        {
            List<RoleInGame> roles = new List<RoleInGame>();
            foreach (var item in this._Players)
            {
                if (item.Key == npc.Key)
                {
                    continue;
                }
                else if (CheckIsPartner(npc, item.Value))
                {
                    if (item.Value.getCar().ability.driver != null)
                    {
                        if (item.Value.getCar().ability.driver.race == race)
                            roles.Add(item.Value);
                    }
                }
                else
                {
                    continue;
                }
            }
            return roles;
            // throw new NotImplementedException();
        }

        /// <summary>
        /// 包括破产的角色，false,表示是NPC队伍中的最后一位。
        /// </summary>
        /// <param name="npc"></param>
        internal bool HasPartnerIsInGame(NPC npc)
        {
            foreach (var item in this._Players)
            {
                if (item.Key == npc.Key)
                {
                    continue;
                }
                else if (CheckIsPartner(npc, item.Value))
                {
                    return true;
                }
                else
                {
                    continue;
                }
            }
            return false;
        }
    }

    /*
     * NPC
     */
}
