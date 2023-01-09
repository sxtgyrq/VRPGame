using CommonClass;
using HouseManager4_0.RoomMainF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static HouseManager4_0.Car;

namespace HouseManager4_0
{
    public partial class Manager_Model : Manager
    {


        public Manager_Model(RoomMain roomMain)
        {
            this.roomMain = roomMain;
        }
        internal void setModels(RoleInGame roleInGame, List<Data.detailmodel> cloesdMaterial, ref List<string> notifyMsgs)
        {
            if (roleInGame.playerType == RoleInGame.PlayerType.player)
            {
                var player = (Player)roleInGame;
                for (int i = 0; i < cloesdMaterial.Count; i++)
                {
                    if (player.modelHasShowed.ContainsKey(cloesdMaterial[i].modelID)) { }
                    else
                    {
                        if (Program.dt.material.ContainsKey(cloesdMaterial[i].amodel))
                        {
                            var m1 = Program.dt.material[cloesdMaterial[i].amodel];
                            var m2 = cloesdMaterial[i];
                            player.DrawObj3DModelF(player, m2.modelID, m2.x, m2.y, m2.z, m2.amodel, m1.modelType, m2.rotatey, false, m1.imageBase64, m1.objText, m1.mtlText, ref notifyMsgs);
                        }
                        else
                        { }
                        player.modelHasShowed.Add(cloesdMaterial[i].modelID, true);
                    }
                }
            }
        }

        internal void setModels(List<Data.detailmodel> cloesdMaterial, ref List<string> notifyMsgs)
        {

            {
                for (int i = 0; i < cloesdMaterial.Count; i++)
                {
                    {
                        if (Program.dt.material.ContainsKey(cloesdMaterial[i].amodel))
                        {
                            var m1 = Program.dt.material[cloesdMaterial[i].amodel];
                            var m2 = cloesdMaterial[i];
                            this.that.DrawObj3DModelF(m2.modelID, m2.x, m2.y, m2.z, m2.amodel, m1.modelType, m2.rotatey, false, m1.imageBase64, m1.objText, m1.mtlText, ref notifyMsgs);
                            // player.DrawObj3DModelF(player, m2.modelID, m2.x, m2.y, m2.z, m2.amodel, m1.modelType, m2.rotatey, false, m1.imageBase64, m1.objText, m1.mtlText, ref notifyMsgs);
                        } 
                    }
                }
            }
        }


        internal string GetRewardFromBuildingF(GetRewardFromBuildingM m)
        {
            //Program.dt.models.con

            //return "";
            // throw new NotImplementedException();
            //   string conditionNotReason;
            // if (actionDo.conditionsOk(c, out conditionNotReason))
            {
                List<string> notifyMsg = new List<string>();
                lock (that.PlayerLock)
                {
                    if (that._Players.ContainsKey(m.Key))
                    {
                        if (that._Players[m.Key].Bust) { }
                        else
                        {

                            var role = that._Players[m.Key];
                            if (role.playerType == RoleInGame.PlayerType.player)
                            {
                                var player = (Player)role;
                                if (player.canGetReward)
                                {
                                    var car = that._Players[m.Key].getCar();
                                    switch (car.state)
                                    {
                                        case CarState.waitOnRoad:
                                            {
                                                var models = that.goodsM.GetConnectionModels(player.getCar().targetFpIndex);
                                                if (models.Count(item => item.modelID == m.selectObjName) > 0)
                                                {

                                                    var hash = (m.Key + m.selectObjName).GetHashCode();
                                                    var newRm = new System.Random(hash);
                                                    hash = newRm.Next(5);
                                                    int defendLevel = 1;
                                                    string rewardLittleReason;
                                                    if (string.IsNullOrEmpty(player.BTCAddress))
                                                    {
                                                        rewardLittleReason = ",你还没有登录，登录可获取更多加成。";
                                                        defendLevel = 1;
                                                    }
                                                    else if (Program.dt.modelsStocks.ContainsKey(m.selectObjName))
                                                    {
                                                        if (Program.dt.modelsStocks[m.selectObjName].stocks.ContainsKey(player.BTCAddress))
                                                        {

                                                            defendLevel = 3;
                                                            var sum = Program.dt.modelsStocks[m.selectObjName].stocks.Sum(item => item.Value);
                                                            sum = Math.Min(sum, 1000000);
                                                            var itemV = Program.dt.modelsStocks[m.selectObjName].stocks[player.BTCAddress];
                                                            for (var i = 0; i < 5; i++)
                                                            {
                                                                if (sum * Program.rm.rm.Next(100) < itemV * 100)
                                                                {
                                                                    defendLevel++;
                                                                }
                                                            }
                                                            rewardLittleReason = "";
                                                        }
                                                        else
                                                        {
                                                            rewardLittleReason = ",你在此处还没有股份，成为股东获取更多加成！";
                                                            defendLevel = 2;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        rewardLittleReason = ",你在此处还没有股份，成为股东获取更多加成！";
                                                        defendLevel = 2;
                                                    }
                                                    {



                                                        if (hash < 1)
                                                        {
                                                            if (player.buildingReward.ContainsKey(hash)) { }
                                                            else
                                                            {
                                                                return "";
                                                            }
                                                            player.buildingReward[hash] += defendLevel;
                                                            switch (hash)
                                                            {
                                                                case 0:
                                                                    {
                                                                        this.WebNotify(player, $"招募力+{(string.IsNullOrEmpty(rewardLittleReason) ? "" : rewardLittleReason)}");
                                                                    }; break;
                                                            }
                                                        }
                                                        else if (hash < 5)
                                                        {
                                                            if (player.buildingReward.ContainsKey(hash)) { }
                                                            else
                                                            {
                                                                return "";
                                                            }
                                                            if (player.getCar().ability.driver == null)
                                                            {
                                                                this.WebNotify(player, "你还没有选司机，没有获得任何奖励");
                                                            }
                                                            else
                                                            {
                                                                player.buildingReward[hash] += defendLevel;
                                                                switch (player.getCar().ability.driver.race)
                                                                {
                                                                    case CommonClass.driversource.Race.immortal:
                                                                        {
                                                                            switch (hash)
                                                                            {
                                                                                case 1:
                                                                                    {
                                                                                        this.WebNotify(player, $"法术狂暴+{(string.IsNullOrEmpty(rewardLittleReason) ? "" : rewardLittleReason)}");
                                                                                    }; break;
                                                                                case 2:
                                                                                    {
                                                                                        this.WebNotify(player, $"忽视抗雷几率+{(string.IsNullOrEmpty(rewardLittleReason) ? "" : rewardLittleReason)}");
                                                                                    }; break;
                                                                                case 3:
                                                                                    {
                                                                                        this.WebNotify(player, $"忽视抗火几率+{(string.IsNullOrEmpty(rewardLittleReason) ? "" : rewardLittleReason)}");
                                                                                    }; break;
                                                                                case 4:
                                                                                    {
                                                                                        this.WebNotify(player, $"忽视抗水几率+{(string.IsNullOrEmpty(rewardLittleReason) ? "" : rewardLittleReason)}");
                                                                                    }; break;
                                                                            };
                                                                        }; break;
                                                                    case CommonClass.driversource.Race.people:
                                                                        {
                                                                            switch (hash)
                                                                            {
                                                                                case 1:
                                                                                    {
                                                                                        this.WebNotify(player, $"故技重施+{(string.IsNullOrEmpty(rewardLittleReason) ? "" : rewardLittleReason)}");
                                                                                    }; break;
                                                                                case 2:
                                                                                    {
                                                                                        this.WebNotify(player, $"忽视抗混几率+{(string.IsNullOrEmpty(rewardLittleReason) ? "" : rewardLittleReason)}");
                                                                                    }; break;
                                                                                case 3:
                                                                                    {
                                                                                        this.WebNotify(player, $"忽视抗迷几率+{(string.IsNullOrEmpty(rewardLittleReason) ? "" : rewardLittleReason)}");
                                                                                    }; break;
                                                                                case 4:
                                                                                    {
                                                                                        this.WebNotify(player, $"忽视潜伏几率+{(string.IsNullOrEmpty(rewardLittleReason) ? "" : rewardLittleReason)}");
                                                                                    }; break;
                                                                            };
                                                                        }; break;
                                                                    case CommonClass.driversource.Race.devil:
                                                                        {
                                                                            switch (hash)
                                                                            {
                                                                                case 1:
                                                                                    {
                                                                                        this.WebNotify(player, $"力争上游+{(string.IsNullOrEmpty(rewardLittleReason) ? "" : rewardLittleReason)}");
                                                                                    }; break;
                                                                                case 2:
                                                                                    {
                                                                                        this.WebNotify(player, $"加速强化几率+{(string.IsNullOrEmpty(rewardLittleReason) ? "" : rewardLittleReason)}");
                                                                                    }; break;
                                                                                case 3:
                                                                                    {
                                                                                        this.WebNotify(player, $"加防强化几率+{(string.IsNullOrEmpty(rewardLittleReason) ? "" : rewardLittleReason)}");
                                                                                    }; break;
                                                                                case 4:
                                                                                    {
                                                                                        this.WebNotify(player, $"强化冲撞几率+{(string.IsNullOrEmpty(rewardLittleReason) ? "" : rewardLittleReason)}");
                                                                                    }; break;
                                                                            };
                                                                        }; break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    player.canGetReward = false;
                                                }
                                                else
                                                {
                                                    WebNotify(player, "离得太远了！");
                                                }
                                            }; break;
                                        default:
                                            {
                                                WebNotify(player, "当前状态，求福不顶用！");
                                            }; break;
                                    }
                                    player.canGetReward = false;
                                }
                                else
                                {
                                    this.WebNotify(player, "在一个地点不能重复祈福");
                                }
                            }
                            //  MeetWithNPC(sa); 
                        }
                    }
                }
                var msgL = this.sendMsg(notifyMsg).Count;
                msgL++;
                //for (var i = 0; i < notifyMsg.Count; i += 2)
                //{
                //    var url = notifyMsg[i];
                //    var sendMsg = notifyMsg[i + 1]; 
                //    if (!string.IsNullOrEmpty(url))
                //    {
                //        Startup.sendMsg(url, sendMsg);
                //    }
                //}
                return $"{msgL}".Length > 0 ? "" : "";
            }

        }

        public enum RewardByModel
        {
            attackNoLengthLimit,//0 直接从基地进行攻击
            fireDefend,//1
            waterDefend,//2
            electricDefend,//3
            physicDefend,//4
            confuseDefend,//5
            lostDefend,//6
            ambushDefend,//7
            harmMagicImprove,//8
            ignorePhysic,//8
            controlMagicImprove,//8
            ignoreFire,//9
            ignoreWater,//10
            ignoreElectric,//11
            speedValueImprove,//9
            defendValueImprove,//10
            attackValueImprove,//11
            ignoreConfuse,//9
            ignoreLost,//10
            ingoreAmbush//11
        }
    }
    public partial class Manager_Model
    {
        public const int IgnorePhysics = 50;
        public const int IgnoreMagic = 30;
        public const int IgnoreControl = 25;
    }
}
