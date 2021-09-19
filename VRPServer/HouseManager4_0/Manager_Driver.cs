using CommonClass;
using HouseManager4_0.interfaceOfEngine;
using HouseManager4_0.RoomMainF;
using Model;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using static HouseManager4_0.Car;
using static HouseManager4_0.RoomMainF.RoomMain;
using OssModel = Model;

namespace HouseManager4_0
{
    public class Manager_Driver : Manager, startNewCommandThread, manager
    {
        public Manager_Driver(RoomMain roomMain)
        {
            this.roomMain = roomMain;

        }

        internal void SelectDriver(SetSelectDriver dm)
        {
            List<string> notifyMsg = new List<string>();
            lock (that.PlayerLock)
            {
                if (that._Players.ContainsKey(dm.Key))
                {
                    var player = that._Players[dm.Key];
                    if (player.Bust) { }
                    else
                    {

                        var car = player.getCar();
                        if (car.state == Car.CarState.waitAtBaseStation)
                            if (car.ability.driver == null)
                                this.setDriver(player, car, dm.Index, ref notifyMsg);
                    }
                }
            }
            for (var i = 0; i < notifyMsg.Count; i += 2)
            {
                var url = notifyMsg[i];
                var sendMsg = notifyMsg[i + 1];
                Console.WriteLine($"url:{url}");
                Startup.sendMsg(url, sendMsg);
            }
            //  throw new NotImplementedException();
        }

        private void setDriver(RoleInGame player, Car car, int index, ref List<string> notifyMsg)
        {
            switch (index)
            {
                case 518:
                    {
                        //输出 马超 男  模拟数据男仙01
                        var name = "马超";
                        CommonClass.driversource.Driver machao = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.man, CommonClass.driversource.Race.immortal, name);
                        machao.DefensiveInitialize(66, 60, 52, 02, 14, 07, 0);
                        machao.GrouthSet(93, 93, 115);
                        machao.Index = index;
                        car.ability.driver = machao;
                        car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg);
                        car.ability.driverSelected(player, car, ref notifyMsg);
                    }; break;
                case 538:
                    {
                        /*
                         * 女仙02
                         */
                        var name = "云缨";
                        CommonClass.driversource.Driver yunying = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.woman, CommonClass.driversource.Race.immortal, name);
                        yunying.DefensiveInitialize(62, 62, 42, 15, 08, 02, 13);
                        yunying.GrouthSet(93, 115, 93);
                        yunying.Index = index;
                        car.ability.driver = yunying;
                        car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg);
                        car.ability.driverSelected(player, car, ref notifyMsg);
                    }; break;
                case 137:
                    {
                        /*
                         * 男人01
                         */
                        var name = "司马懿";
                        CommonClass.driversource.Driver simayi = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.man, CommonClass.driversource.Race.people, name);
                        simayi.DefensiveInitialize(13, 10, 14, 72, 68, 40, 18);
                        simayi.GrouthSet(110, 95, 95);
                        simayi.Index = index;
                        car.ability.driver = simayi;
                        car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg);
                        car.ability.driverSelected(player, car, ref notifyMsg);
                    }; break;
                case 536:
                    {
                        var name = "夏洛特";
                        CommonClass.driversource.Driver simayi = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.woman, CommonClass.driversource.Race.people, name);
                        simayi.DefensiveInitialize(13, 10, 14, 72, 68, 40, 18);
                        simayi.GrouthSet(110, 95, 95);
                        simayi.Index = index;
                        car.ability.driver = simayi;
                        car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg);
                        car.ability.driverSelected(player, car, ref notifyMsg);
                    }; break;
                case 510:
                    {
                        var name = "孙策";
                        CommonClass.driversource.Driver simayi = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.man, CommonClass.driversource.Race.devil, name);
                        simayi.DefensiveInitialize(13, 10, 14, 72, 68, 40, 18);
                        simayi.GrouthSet(110, 95, 95);
                        simayi.Index = index;
                        car.ability.driver = simayi;
                        car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg);
                        car.ability.driverSelected(player, car, ref notifyMsg);
                    }; break;

            }
        }

        public void newThreadDo(CommonClass.Command c)
        {
            // throw new NotImplementedException();
        }

        public class ConfuseManger
        {
            public enum ControlAttackType
            {
                Confuse,
                Lose,
                Ambush

            }
            public ConfuseManger()
            {
                this.controlInfomations = new List<ControlInfomation>();
                this._selectedControlItem = null;
                this.ambushInfomations = new List<AmbushInfomation>();
            }
            internal void Cancle(RoleInGame player_)
            {
                this.controlInfomations.RemoveAll(item => item.player.Key == player_.Key);
                this.ambushInfomations.RemoveAll(item => item.player.Key == player_.Key);
            }
            #region 控制区
            List<ControlInfomation> controlInfomations = new List<ControlInfomation>();
            class ControlInfomation
            {
                public RoleInGame player { get; set; }
                public OssModel.FastonPosition fpResult { get; set; }
                public long volumeValue { get; set; }
                public ControlAttackType attackType { get; set; }
            }



            ControlInfomation _selectedControlItem = null;
            ControlInfomation getSelectedControlItem()
            {
                return this._selectedControlItem;
            }
            public RoleInGame getBoss()
            {
                if (this._selectedControlItem == null)
                {
                    return null;
                }
                else
                {
                    return this._selectedControlItem.player;
                }
            }

            internal void AddControlInfo(RoleInGame player_, FastonPosition fastonPosition_, long volumeValue_, ControlAttackType attackType_)
            {
                this.Cancle(player_);
                //this.AttackInfomations.
                this.controlInfomations.Add(new ControlInfomation()
                {
                    player = player_,
                    fpResult = fastonPosition_,
                    volumeValue = volumeValue_,
                    attackType = attackType_
                });
            }

            internal bool IsBeingControlled()
            {
                if (this._selectedControlItem == null)
                {
                    return false;
                }
                else
                {
                    if (this._selectedControlItem.player.Bust)
                    {
                        return false;
                    }
                    else if (this._selectedControlItem.volumeValue > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                        // return false;
                    }
                }
            }

            public ControlAttackType getControlType()
            {
                if (this.IsBeingControlled())
                {
                    return this._selectedControlItem.attackType;
                }
                else
                {
                    throw new Exception("非法调用");
                }
            }



            enum programResult { runContinue, runReturn };
            programResult dealWithItem(RoleInGame self, ControlInfomation magicItem, RoomMain that, webnotify ex, Car enemyCar, RoleInGame enemy, ref List<string> notifyMsg)
            {
                int defensiveOfControl;
                if (self.getCar().ability.driver == null)
                {
                    defensiveOfControl = 0;
                }
                else
                {
                    if (magicItem.attackType == ControlAttackType.Confuse)
                        defensiveOfControl = self.getCar().ability.driver.defensiveOfConfuse;
                    else if (magicItem.attackType == ControlAttackType.Lose)
                        defensiveOfControl = self.getCar().ability.driver.defensiveOfLose;
                    else throw new Exception("意料之外！");
                }
                string name = "";
                switch (magicItem.attackType)
                {
                    case ControlAttackType.Confuse:
                        {
                            name = "混乱";
                        }; break;
                    case ControlAttackType.Lose:
                        {
                            name = "迷失";
                        }; break;
                }
                if (that.rm.Next(0, 100) > this.getBaseControlProbability(magicItem.attackType) - defensiveOfControl)
                {

                    ex.WebNotify(magicItem.player, $"你对【{self.PlayerName}】实施了{name}计谋，没成功！");
                    ex.WebNotify(self, $"【{magicItem.player.PlayerName}】对你实施了{name}阴谋，没成功！");

                    enemyCar.setState(enemy, ref notifyMsg, CarState.returning);
                    that.retutnE.SetReturnT(500, new commandWithTime.returnning()
                    {
                        c = "returnning",
                        changeType = commandWithTime.returnning.ChangeType.BeforeTax,
                        key = magicItem.player.Key,
                        returningOjb = magicItem.player.returningOjb,
                        target = enemyCar.targetFpIndex
                    });
                    return programResult.runContinue;
                }
                else
                {
                    ex.WebNotify(magicItem.player, $"你对【{self.PlayerName}】成功实施了{name}计谋！");
                    ex.WebNotify(self, $"【{magicItem.player.PlayerName}】让你陷入了{name}！");
                    this._selectedControlItem = magicItem;

                    enemyCar.setState(enemy, ref notifyMsg, CarState.returning);
                    that.retutnE.SetReturnT(500, new commandWithTime.returnning()
                    {
                        c = "returnning",
                        changeType = commandWithTime.returnning.ChangeType.BeforeTax,
                        key = magicItem.player.Key,
                        returningOjb = magicItem.player.returningOjb,
                        target = enemyCar.targetFpIndex
                    });
                    return programResult.runReturn;
                }
            }
            bool dealWithItem(RoleInGame self, AmbushInfomation magicItem, RoomMain that, webnotify ex, Car enemyCar, RoleInGame enemy, ref List<string> notifyMsg)
            {
                int defensiveOfAmbush;
                if (self.getCar().ability.driver == null)
                {
                    defensiveOfAmbush = 0;
                }
                else
                {
                    defensiveOfAmbush = self.getCar().ability.driver.defensiveOfAmbush;
                }
                string name = "潜伏";
                if (that.rm.Next(0, 100) > this.getBaseControlProbability(ControlAttackType.Ambush) - defensiveOfAmbush)
                {

                    ex.WebNotify(magicItem.player, $"你对【{self.PlayerName}】实施了{name}计谋，没成功！");
                    ex.WebNotify(self, $"【{magicItem.player.PlayerName}】对你实施了{name}阴谋，没成功！");

                    enemyCar.setState(enemy, ref notifyMsg, CarState.returning);
                    that.retutnE.SetReturnT(500, new commandWithTime.returnning()
                    {
                        c = "returnning",
                        changeType = commandWithTime.returnning.ChangeType.BeforeTax,
                        key = magicItem.player.Key,
                        returningOjb = magicItem.player.returningOjb,
                        target = enemyCar.targetFpIndex
                    });
                    return false;
                }
                else
                {
                    ex.WebNotify(magicItem.player, $"你对【{self.PlayerName}】成功实施了{name}计谋！");
                    ex.WebNotify(self, $"【{magicItem.player.PlayerName}】让你陷入了{name}！");
                    enemyCar.setState(enemy, ref notifyMsg, CarState.returning);
                    that.retutnE.SetReturnT(500, new commandWithTime.returnning()
                    {
                        c = "returnning",
                        changeType = commandWithTime.returnning.ChangeType.BeforeTax,
                        key = magicItem.player.Key,
                        returningOjb = magicItem.player.returningOjb,
                        target = enemyCar.targetFpIndex
                    });
                    return true;
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
                    case ControlAttackType.Lose:
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

            internal void ControllSelf(RoleInGame self, RoomMain that, ref List<string> notifyMsg, interfaceOfEngine.webnotify ex)
            {
                FastonPosition baseFp = Program.dt.GetFpByIndex(self.StartFPIndex);
                this.controlInfomations.RemoveAll(item => item.player.Bust);
                var orderedMagic = (from item in this.controlInfomations
                                    orderby Geography.getLengthOfTwoPoint.GetDistance(baseFp.Latitde, baseFp.Longitude, Program.dt.GetFpByIndex(item.player.StartFPIndex).Latitde, Program.dt.GetFpByIndex(item.player.StartFPIndex).Longitude)
                                    select item).ToList();
                for (var i = 0; i < orderedMagic.Count; i++)
                {

                    var magicItem = orderedMagic[i];


                    var enemy = magicItem.player;
                    var enemyCar = enemy.getCar();
                    var enemyFp = Program.dt.GetFpByIndex(enemy.StartFPIndex);
                    if ((
                        enemyCar.state == Car.CarState.waitOnRoad && Program.dt.GetFpByIndex(enemyCar.targetFpIndex).FastenPositionID == magicItem.fpResult.FastenPositionID))
                    {
                        if (magicItem.attackType == ControlAttackType.Confuse)
                        {
                            var result = dealWithItem(self, magicItem, that, ex, enemyCar, enemy, ref notifyMsg);
                            if (result == programResult.runContinue)
                            {
                                continue;
                            }
                            else
                            {
                                return;
                            }
                        }
                        else if (magicItem.attackType == ControlAttackType.Lose)
                        {
                            var result = dealWithItem(self, magicItem, that, ex, enemyCar, enemy, ref notifyMsg);
                            if (result == programResult.runContinue)
                            {
                                continue;
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                }

            }

            internal long getIndemnity()
            {
                return this._selectedControlItem.volumeValue;
                //throw new NotImplementedException();
            }

            internal void reduceValue(long reduceValue)
            {
                this._selectedControlItem.volumeValue -= reduceValue;
            }
            #endregion 控制区


            #region 埋伏区 ambush
            internal class AmbushInfomation
            {
                public RoleInGame player { get; set; }
                public OssModel.FastonPosition fpResult { get; set; }
                public long volumeValue { get; set; }
                public long bussinessValue { get; set; }
            }

            internal void AddAmbushInfo(RoleInGame player_, FastonPosition fastonPosition_, long bussinessValue_, long volumeValue_)
            {
                this.Cancle(player_);
                //this.AttackInfomations.
                this.ambushInfomations.Add(new AmbushInfomation()
                {
                    player = player_,
                    fpResult = fastonPosition_,
                    volumeValue = volumeValue_,
                    bussinessValue = bussinessValue_
                });
            }

            //internal bool IsBeingAimed()
            //{
            //    throw new NotImplementedException();
            //}
            internal List<AmbushInfomation> ambushInfomations = new List<AmbushInfomation>();

            internal delegate void AmbushAttack(int i, ref List<string> notifyMsg, RoleInGame enemy, ref long reduceSumInput);
            internal void AmbushSelf(RoleInGame selfRole, RoomMain that, webnotify ex, ref List<string> notifyMsg, interfaceOfHM.AttackT at, ref long reduceSumInput, AmbushAttack aa)
            {
                List<AmbushInfomation> newList = new List<AmbushInfomation>();
                for (int i = 0; i < ambushInfomations.Count; i++)
                {
                    if (!ambushInfomations[i].player.Bust)
                        if (ambushInfomations[i].player.getCar().state == CarState.waitOnRoad)
                        {
                            if (Program.dt.GetFpByIndex(ambushInfomations[i].player.getCar().targetFpIndex).FastenPositionID == ambushInfomations[i].fpResult.FastenPositionID)
                            {
                                newList.Add(this.ambushInfomations[i]);
                            }
                        }
                }
                var baseFp = Program.dt.GetFpByIndex(selfRole.StartFPIndex);

                this.ambushInfomations = (from item in newList
                                          orderby Geography.getLengthOfTwoPoint.GetDistance(baseFp.Latitde, baseFp.Longitude, item.fpResult.Latitde, item.fpResult.Longitude) ascending,
                                          (at.getCondition() == Engine_DebtEngine.DebtCondition.magic ? item.volumeValue : item.bussinessValue) descending
                                          select item).ToList();

                for (int i = 0; i < this.ambushInfomations.Count; i++)
                {
                    var enemyCar = this.ambushInfomations[i].player.getCar();
                    var enemy = this.ambushInfomations[i].player;
                    var success = dealWithItem(selfRole, this.ambushInfomations[i], that, ex, enemyCar, enemy, ref notifyMsg);
                    if (success)
                    {
                        aa(i, ref notifyMsg, enemy, ref reduceSumInput);
                    }
                }
            }
            #endregion 埋伏区
        }

        public class ImproveManager
        {
            public long speedValue { get; private set; }
            public long attackValue { get; private set; }
            public long defenceValue { get; private set; }
            public ImproveManager()
            {
                this.speedValue = 0;
                this.attackValue = 0;
                this.defenceValue = 0;
            }

            const int speedImproveParameter = 7;
            internal void addSpeed(RoleInGame role, long leftVolume, out long costVolumeValue, ref List<string> notifyMsg)
            {
                var speedBeforeImprove = this.speedValue;
                if (this.speedValue < leftVolume * speedImproveParameter)
                {
                    costVolumeValue = leftVolume - this.speedValue / speedImproveParameter;
                    costVolumeValue = Math.Max(1, costVolumeValue);
                    this.speedValue = leftVolume * speedImproveParameter;
                }
                else
                {
                    costVolumeValue = 0;
                }
                var speedAfterImprove = this.speedValue;
                if ((speedBeforeImprove == 0 && speedAfterImprove != 0) ||
                    (speedBeforeImprove != 0 && speedAfterImprove == 0))
                {
                    role.speedMagicChanged(role, ref notifyMsg);
                }
            }

            internal void reduceSpeed(RoleInGame role, long changeValue, ref List<string> notifyMsg)
            {
                var speedBeforeImprove = this.speedValue;
                this.speedValue -= changeValue;
                if (this.speedValue < 0) 
                {
                    this.speedValue = 0;
                } 
                var speedAfterImprove = this.speedValue;
                if ((speedBeforeImprove == 0 && speedAfterImprove != 0) ||
                    (speedBeforeImprove != 0 && speedAfterImprove == 0))
                {
                    role.speedMagicChanged(role, ref notifyMsg);
                }
            }
        }

        internal bool controlledByMagic(RoleInGame player, Car car, ref List<string> notifyMsg)
        {
            if (player.confuseRecord.IsBeingControlled())
            {
                return true;
            }
            else
            {
                player.confuseRecord.ControllSelf(player, that, ref notifyMsg, this);
                return player.confuseRecord.IsBeingControlled();
            }
            //throw new NotImplementedException();
        }
        internal enum AmbushMagicType
        {
            waterMagic,
            electicMagic,
            fireMagic,
            attack
        }
        internal bool harmByAmbushMagic(RoleInGame player, Car car, ref List<string> notifyMsg, AmbushMagicType amt)
        {
            if (player.confuseRecord.IsBeingControlled())
            {
                return true;
            }
            else
            {
                player.confuseRecord.ControllSelf(player, that, ref notifyMsg, this);
                return player.confuseRecord.IsBeingControlled();
            }
            //throw new NotImplementedException();
        }

        //internal bool ambushSuccess(RoleInGame self)
        //{
        //    int defensiveOfAmbush;
        //    if (self.getCar().ability.driver == null)
        //    {
        //        defensiveOfAmbush = 0;
        //    }
        //    else
        //    {
        //        defensiveOfAmbush = self.getCar().ability.driver.defensiveOfAmbush;

        //        if (magicItem.attackType == ControlAttackType.Confuse)


        //        else if (magicItem.attackType == ControlAttackType.Lose)
        //                    defensiveOfControl = self.getCar().ability.driver.defensiveOfLose;
        //                else throw new Exception("意料之外！");
        //    }
        //    throw new Exception("");
        //}
    }
}
