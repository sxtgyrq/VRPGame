using CommonClass;
using Google.Protobuf.WellKnownTypes;
//using HouseManager4_0.interfaceOfHM;
using HouseManager4_0.RoomMainF;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
        internal bool setModel(Player player, Data.detailmodel cloesdMaterial, ref List<string> notifyMsgs)
        {
            if (player.modelHasShowed.ContainsKey(cloesdMaterial.modelID))
            {
                return false;
            }
            else
            {
                if (Program.dt.material.ContainsKey(cloesdMaterial.amodel))
                {
                    var m1 = Program.dt.material[cloesdMaterial.amodel];
                    var m2 = cloesdMaterial;
                    player.DrawObj3DModelF(player, m2.modelID, m2.x, m2.y, m2.z, m2.amodel, m1.modelType, m2.rotatey, false, m1.imageBase64, m1.objText, m1.mtlText, ref notifyMsgs);
                }
                else
                { }
                player.modelHasShowed.Add(cloesdMaterial.modelID, true);
                return true;
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
                                                var models = that.goodsM.GetConnectionModels(player.getCar().targetFpIndex, player);
                                                if (models.Count(item => item.modelID == m.selectObjName) > 0)
                                                {

                                                    var newList = (from item in Program.dt.models orderby CommonClass.Random.GetMD5HashFromStr(item.modelID + m.Key) ascending select item).ToList();

                                                    var hash = newList.FindIndex(item => item.modelID == m.selectObjName);
                                                    hash = hash % 5;
                                                    int defendLevel = 1;
                                                    string rewardLittleReason;

                                                    this.that.taskM.GetRewardFromBuildingF(player);

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
                                                            {
                                                                player.buildingReward[hash] = 0;
                                                                //player.buildingReward[hash] += defendLevel;
                                                                switch (hash)
                                                                {
                                                                    case 0:
                                                                        {
                                                                            //bool newProperty = false;
                                                                            int value = 0;
                                                                            switch (defendLevel)
                                                                            {
                                                                                case 1: value = that.rm.Next(1, 30); break;
                                                                                case 2: value = that.rm.Next(15, 40); break;
                                                                                case 3: value = that.rm.Next(29, 50); break;
                                                                                case 4: value = that.rm.Next(43, 60); break;
                                                                                case 5: value = that.rm.Next(57, 70); break;
                                                                                case 6: value = that.rm.Next(71, 80); break;
                                                                                case 7: value = that.rm.Next(80, 85); break;
                                                                                case 8: value = that.rm.Next(86, 89); break;
                                                                            }
                                                                            if (value > player.buildingReward[hash])
                                                                            {
                                                                                player.buildingReward[hash] = value;
                                                                                this.WebNotify(player, $"获得了新的招募成功率属性{(string.IsNullOrEmpty(rewardLittleReason) ? "" : rewardLittleReason)}");
                                                                            }
                                                                            else
                                                                            {
                                                                                player.buildingReward[hash] = value;
                                                                                this.WebNotify(player, $"未能获得了新的招募成功率属性{(string.IsNullOrEmpty(rewardLittleReason) ? "" : rewardLittleReason)}");
                                                                            }
                                                                        }; break;
                                                                }
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
                                                                //if (player.buildingReward[hash] > 100)
                                                                //{
                                                                //    player.buildingReward[hash] = 100;
                                                                //    this.WebNotify(player, $"储备已满");
                                                                //}
                                                                //else
                                                                player.buildingReward[hash] = 0;
                                                                int value = 0;
                                                                string prefix = "";
                                                                {
                                                                    switch (defendLevel)
                                                                    {
                                                                        case 1: value = that.rm.Next(2, 26); break;
                                                                        case 2: value = that.rm.Next(12, 33); break;
                                                                        case 3: value = that.rm.Next(22, 40); break;
                                                                        case 4: value = that.rm.Next(32, 47); break;
                                                                        case 5: value = that.rm.Next(42, 54); break;
                                                                        case 6: value = that.rm.Next(52, 61); break;
                                                                        case 7: value = that.rm.Next(62, 71); break;
                                                                        case 8: value = that.rm.Next(72, 75); break;
                                                                    }
                                                                    if (player.buildingReward[hash] < value)
                                                                    {
                                                                        player.buildingReward[hash] = value;
                                                                        prefix = "";
                                                                    }
                                                                    else
                                                                    {
                                                                        prefix = "未能";
                                                                    }
                                                                    // player.buildingReward[hash] += defendLevel;
                                                                    switch (player.getCar().ability.driver.race)
                                                                    {
                                                                        case CommonClass.driversource.Race.immortal:
                                                                            {
                                                                                switch (hash)
                                                                                {
                                                                                    case 1:
                                                                                        {
                                                                                            this.WebNotify(player, $"{prefix}获得了新的法术狂暴属性{(string.IsNullOrEmpty(rewardLittleReason) ? "" : rewardLittleReason)}");
                                                                                        }; break;
                                                                                    case 2:
                                                                                        {
                                                                                            this.WebNotify(player, $"{prefix}获得了新的忽视雷抗属性{(string.IsNullOrEmpty(rewardLittleReason) ? "" : rewardLittleReason)}");
                                                                                        }; break;
                                                                                    case 3:
                                                                                        {
                                                                                            this.WebNotify(player, $"{prefix}获得了新的忽视火抗属性{(string.IsNullOrEmpty(rewardLittleReason) ? "" : rewardLittleReason)}");
                                                                                        }; break;
                                                                                    case 4:
                                                                                        {
                                                                                            this.WebNotify(player, $"{prefix}获得了新的忽视水抗属性{(string.IsNullOrEmpty(rewardLittleReason) ? "" : rewardLittleReason)}");
                                                                                        }; break;
                                                                                };
                                                                            }; break;
                                                                        case CommonClass.driversource.Race.people:
                                                                            {
                                                                                switch (hash)
                                                                                {
                                                                                    case 1:
                                                                                        {
                                                                                            this.WebNotify(player, $"{prefix}获得了新的故技重施属性{(string.IsNullOrEmpty(rewardLittleReason) ? "" : rewardLittleReason)}");
                                                                                            // this.WebNotify(player, $"故技重施+{defendLevel}{(string.IsNullOrEmpty(rewardLittleReason) ? "" : rewardLittleReason)}");
                                                                                        }; break;
                                                                                    case 2:
                                                                                        {
                                                                                            this.WebNotify(player, $"{prefix}获得新的忽视抗混属性{(string.IsNullOrEmpty(rewardLittleReason) ? "" : rewardLittleReason)}");
                                                                                        }; break;
                                                                                    case 3:
                                                                                        {
                                                                                            this.WebNotify(player, $"{prefix}获得新的忽视抗迷属性{(string.IsNullOrEmpty(rewardLittleReason) ? "" : rewardLittleReason)}");
                                                                                        }; break;
                                                                                    case 4:
                                                                                        {
                                                                                            this.WebNotify(player, $"{prefix}获得新的忽视抗潜伏属性{(string.IsNullOrEmpty(rewardLittleReason) ? "" : rewardLittleReason)}");
                                                                                        }; break;
                                                                                };
                                                                            }; break;
                                                                        case CommonClass.driversource.Race.devil:
                                                                            {
                                                                                switch (hash)
                                                                                {
                                                                                    case 1:
                                                                                        {
                                                                                            this.WebNotify(player, $"{prefix}获得了新的忽视物理属性{(string.IsNullOrEmpty(rewardLittleReason) ? "" : rewardLittleReason)}");
                                                                                        }; break;
                                                                                    case 2:
                                                                                        {
                                                                                            this.WebNotify(player, $"{prefix}获得了新的加速提升属性{(string.IsNullOrEmpty(rewardLittleReason) ? "" : rewardLittleReason)}");
                                                                                            // this.WebNotify(player, $"加速强化+{defendLevel}{(string.IsNullOrEmpty(rewardLittleReason) ? "" : rewardLittleReason)}");
                                                                                        }; break;
                                                                                    case 3:
                                                                                        {
                                                                                            this.WebNotify(player, $"{prefix}获得了新的加防提升属性{(string.IsNullOrEmpty(rewardLittleReason) ? "" : rewardLittleReason)}");
                                                                                            //this.WebNotify(player, $"加防强化+{defendLevel}{(string.IsNullOrEmpty(rewardLittleReason) ? "" : rewardLittleReason)}");
                                                                                        }; break;
                                                                                    case 4:
                                                                                        {
                                                                                            this.WebNotify(player, $"{prefix}获得了新的红牛提升属性{(string.IsNullOrEmpty(rewardLittleReason) ? "" : rewardLittleReason)}");
                                                                                        }; break;
                                                                                };
                                                                            }; break;
                                                                    }
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
                var msgL = this.sendSeveralMsgs(notifyMsg).Count;
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

        internal void GetRewardFromBuildingByNPC(NPC npc)
        {
            List<string> notifyMsg = new List<string>();
            lock (that.PlayerLock)
            {
                if (that._Players.ContainsKey(npc.Key))
                {
                    if (that._Players[npc.Key].Bust) { }
                    else
                    {
                        if (npc.canGetReward)
                        {
                            var car = npc.getCar();
                            switch (car.state)
                            {
                                case CarState.waitOnRoad:
                                    {
                                        var models = that.goodsM.GetConnectionModels(npc.getCar().targetFpIndex, npc);

                                        if (models.Count > 0)
                                        {
                                            // var newList = (from item in models orderby CommonClass.Random.GetMD5HashFromStr(item.modelID + m.Key) ascending select item).ToList();

                                            //var hash = newList.FindIndex(item => item.modelID == m.selectObjName);
                                            //hash = hash % 5;
                                            models = (from item in models orderby item.x + item.y select item).ToList();
                                            var hash = (npc.Key + npc.PlayerName + models[0].modelID).GetHashCode();
                                            var newRm = new System.Random(hash);
                                            hash = newRm.Next(5);
                                            hash = hash % 4 + 1;


                                            //npc.buildingReward[hash] = 0;
                                            var defendLevel = npc.Level - 1;
                                            if (defendLevel < 1)
                                            {
                                                defendLevel = 1;
                                            }
                                            else if (defendLevel > 8)
                                            {
                                                defendLevel = 8;
                                            }
                                            int randomValue = 0;

                                            //              case 1: value = that.rm.Next(2, 26); break;
                                            //case 2: value = that.rm.Next(12, 33); break;
                                            //case 3: value = that.rm.Next(22, 40); break;
                                            //case 4: value = that.rm.Next(32, 47); break;
                                            //case 5: value = that.rm.Next(42, 54); break;
                                            //case 6: value = that.rm.Next(52, 61); break;
                                            //case 7: value = that.rm.Next(62, 68); break;
                                            //case 8:
                                            //    value = that.rm.Next(72, 75); break;
                                            switch (defendLevel)
                                            {
                                                case 1: randomValue = that.rm.Next(2, 26); break;
                                                case 2: randomValue = that.rm.Next(12, 33); break;
                                                case 3: randomValue = that.rm.Next(22, 40); break;
                                                case 4: randomValue = that.rm.Next(32, 47); break;
                                                case 5: randomValue = that.rm.Next(42, 54); break;
                                                case 6: randomValue = that.rm.Next(52, 61); break;
                                                case 7: randomValue = that.rm.Next(62, 71); break;
                                                case 8: randomValue = that.rm.Next(72, 75); break;
                                            }
                                            if (npc.buildingReward[hash] < randomValue)
                                                npc.buildingReward[hash] = randomValue;
                                        }
                                        npc.canGetReward = false;
                                    }; break;
                                default:
                                    {
                                        npc.canGetReward = false;
                                    }; break;
                            }
                            npc.canGetReward = false;
                        }
                        else
                        {
                        }
                    }
                }
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
        // public const int IgnorePhysics = 50;
        //public const int IgnoreElectricMagic = 30;
        //public const int IgnoreFireMagic = 30;
        //public const int IgnoreWaterMagic = 30;
        //public const int IgnoreControl = 25;
    }
}
