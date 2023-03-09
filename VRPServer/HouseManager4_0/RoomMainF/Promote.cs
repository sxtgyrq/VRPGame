using CommonClass;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using static HouseManager4_0.Car;
using OssModel = Model;

namespace HouseManager4_0.RoomMainF
{
    public partial class RoomMain : interfaceOfHM.Promote
    {
        public void SetLookForPromote(GetRandomPos gp)
        {
            this.promoteMilePosition = GetRandomPosition(true, gp);
            this.promoteBusinessPosition = GetRandomPosition(true, gp);
            this.promoteVolumePosition = GetRandomPosition(true, gp);
            this.promoteSpeedPosition = GetRandomPosition(true, gp);
        }

        public string updatePromote(SetPromote sp, GetRandomPos grp)
        {
            return this.promoteE.updatePromote(sp, grp);
        }

        ///// <summary>
        ///// 从player将钱转移到car,此功能已被启用
        ///// </summary>
        ///// <param name="player"></param>
        ///// <param name="car"></param>
        ///// <param name="pType"></param>
        ///// <returns></returns>
        //public bool giveMoneyFromPlayerToCarForPromoting(RoleInGame player, Car car, string pType, ref List<string> notifyMsg)
        //{
        //    var needMoney = this.promotePrice[pType];
        //    if (player.MoneyToPromote < needMoney)
        //    {
        //        return false;
        //    }
        //    else if (car.ability.SumMoneyCanForPromote != 0)
        //    {
        //        //Consol.WriteLine("小车从基站出发，身上的钱，没有清零！");
        //        //初始化失败，小车 comeback后，没有完成交接！！！
        //        throw new Exception("car.ability.costBusiness != 0m");
        //    }
        //    else
        //    {
        //        if (player.Money - needMoney < 0)
        //        {
        //            throw new Exception("逻辑错误");
        //        }
        //        player.MoneySet(player.Money - needMoney, ref notifyMsg);
        //        car.ability.setCostBusiness(car.ability.costBusiness + needMoney, player, car, ref notifyMsg);

        //        return true;
        //    }
        //}



        //private void EditCarStateWhenPromoteStartOK(RoleInGame role, ref Car car, int to, Model.FastonPosition fp1, int to2, SetPromote sp, List<Model.MapGo.nyrqPosition> goPath, ref List<string> nofityMsgs, out int startT)
        //{
        //    car.targetFpIndex = to;//A.更改小车目标，在其他地方引用。
        //                           //  car.changeState++;//B.更改状态用去前台更新动画    

        //    /*
        //     * C.更新小车动画参数
        //     */
        //    var speed = car.ability.Speed;
        //    startT = 0;
        //    List<int> result;
        //    Data.PathStartPoint2 startPosition;
        //    if (car.state == CarState.waitOnRoad)
        //    {
        //        result = new List<int>();
        //        getStartPositionByGoPath(out startPosition, goPath);
        //        //startPosition = new Data.PathStartPoint()
        //        //{
        //        //    x = goPath.First().BDlongitude,
        //        //    y = goPath.First().BDlatitude
        //        //};
        //    }
        //    else if (car.state == CarState.waitAtBaseStation)
        //    {
        //        result = getStartPositon(fp1, role.positionInStation, ref startT);
        //        getStartPositionByFp(out startPosition, fp1);
        //    }
        //    else
        //    {
        //        //Consol.WriteLine($"{Newtonsoft.Json.JsonConvert.SerializeObject(car)}");
        //        throw new Exception("错误的汽车类型！！！");
        //    }
        //    Program.dt.GetAFromBPoint(goPath, fp1, speed, ref result, ref startT);
        //    // result.RemoveAll(item => item.t == 0);

        //    var animateData = new AnimateData2()
        //    {
        //        start = startPosition,
        //        animateData = result,
        //        recordTime = DateTime.Now,

        //    };
        //    car.setAnimateData(role, ref nofityMsgs, animateData);
        //    //car.animateData = new AnimateData()
        //    //{
        //    //    animateData = result,
        //    //    recordTime = DateTime.Now
        //    //};
        //    car.setState(role, ref nofityMsgs, CarState.working);
        //    //  car.state = CarState.buying;//更改汽车状态

        //    //Thread th = new Thread(() => setDiamondOwner(startT, new commandWithTime.diamondOwner()
        //    //{
        //    //    c = "diamondOwner",
        //    //    key = sp.Key,
        //    //    car = sp.car,
        //    //    returnPath = returnPath,
        //    //    target = to,//新的起点
        //    //    changeType = sp.pType,
        //    //    costMile = goMile
        //    //}));
        //    //th.Start();
        //    //car.changeState++;//更改状态

        //    //getAllCarInfomations(sp.Key, ref notifyMsg);
        //}

        public class PromoteObj : interfaceOfHM.GetFPIndex
        {
            string _pType;
            public PromoteObj(string pType)
            {
                this._pType = pType;
            }
            public int GetFPIndex()
            {
                return Program.rm.GetPromotePositionTo(this._pType);
                //  throw new NotImplementedException();
            }
        }
        public int getPlayerClosestPositionRankNum(RoleInGame player, Car car, RoleInGame victim)
        {
            return getAttackerClosestPositionRankNum(player, car, victim);
        }
        int getAttackerClosestPositionRankNum(RoleInGame player, Car car, interfaceOfHM.GetFPIndex getF)
        {
            int rank = 0;
            if (car.state == CarState.waitAtBaseStation)
            {
                double distanceToDiamond;
                var from = Program.dt.GetFpByIndex(getF.GetFPIndex());
                {
                    var fpTo = Program.dt.GetFpByIndex(player.StartFPIndex); ;//this.GetPromotePositionTo(pType);

                    distanceToDiamond = CommonClass.Geography.getLengthOfTwoPoint.GetDistance(from.Latitde, from.Longitude, from.Height, fpTo.Latitde, fpTo.Longitude, fpTo.Height);
                }
                foreach (var item in this._collectPosition)
                {
                    //  var from = Program.dt.GetFpByIndex(player.StartFPIndex);
                    var fpTo = Program.dt.GetFpByIndex(item.Value);
                    var distanceToMoney = CommonClass.Geography.getLengthOfTwoPoint.GetDistance(from.Latitde, from.Longitude, from.Height, fpTo.Latitde, fpTo.Longitude, fpTo.Height);
                    if (distanceToMoney <= distanceToDiamond)
                    {
                        rank++;
                    }
                }
                return rank;
            }
            else if (car.state == CarState.waitOnRoad)
            {
                var from = Program.dt.GetFpByIndex(getF.GetFPIndex());
                double distanceToDiamond;
                {
                    var fpTo = Program.dt.GetFpByIndex(car.targetFpIndex);
                    distanceToDiamond = CommonClass.Geography.getLengthOfTwoPoint.GetDistance(from.Latitde, from.Longitude, from.Height, fpTo.Latitde, fpTo.Longitude, fpTo.Height);
                }
                foreach (var item in this._collectPosition)
                {
                    var fpTo = Program.dt.GetFpByIndex(item.Value);
                    var distanceToMoney = CommonClass.Geography.getLengthOfTwoPoint.GetDistance(from.Latitde, from.Longitude, from.Height, fpTo.Latitde, fpTo.Longitude, fpTo.Height);
                    if (distanceToMoney <= distanceToDiamond)
                    {
                        rank++;
                    }
                }
                return rank;
            }
            else
            {
                throw new Exception("非法调用");
            }
        }
        public bool theNearestToDiamondIsCarNotMoney(RoleInGame player, Car car, string pType, GetRandomPos grp, out OssModel.FastonPosition fp)
        {

            return theNearestToObjIsCarNotMoney(player, car, new PromoteObj(pType), grp, out fp);
        }
        bool theNearestToObjIsCarNotMoney(RoleInGame player, Car car, interfaceOfHM.GetFPIndex getF, GetRandomPos grp, out OssModel.FastonPosition fp)
        {
            fp = null;
            if (car.state == CarState.waitAtBaseStation)
            {
                double distanceToDiamond;
                var from = grp.GetFpByIndex(getF.GetFPIndex());
                {
                    var fpTo = grp.GetFpByIndex(player.StartFPIndex); ;//this.GetPromotePositionTo(pType);
                    distanceToDiamond = CommonClass.Geography.getLengthOfTwoPoint.GetDistance(from.Latitde, from.Longitude, from.Height, fpTo.Latitde, fpTo.Longitude, fpTo.Height);
                }
                foreach (var item in this._collectPosition)
                {
                    //  var from = Program.dt.GetFpByIndex(player.StartFPIndex);
                    var fpTo = grp.GetFpByIndex(item.Value);
                    var distanceToMoney = CommonClass.Geography.getLengthOfTwoPoint.GetDistance(from.Latitde, from.Longitude, from.Height, fpTo.Latitde, fpTo.Longitude, fpTo.Height);
                    if (distanceToMoney <= distanceToDiamond)
                    {
                        distanceToDiamond = distanceToMoney;
                        fp = fpTo;
                        //  return false;
                    }
                }
                if (fp == null)
                    car.DirectAttack = true;
                else
                    car.DirectAttack = false;
                return fp == null;
            }
            else if (car.state == CarState.waitOnRoad)
            {
                var from = grp.GetFpByIndex(getF.GetFPIndex());
                double distanceToDiamond;
                {
                    var fpTo = grp.GetFpByIndex(car.targetFpIndex);
                    distanceToDiamond = CommonClass.Geography.getLengthOfTwoPoint.GetDistance(from.Latitde, from.Longitude, from.Height, fpTo.Latitde, fpTo.Longitude, fpTo.Height);
                }
                foreach (var item in this._collectPosition)
                {
                    var fpTo = grp.GetFpByIndex(item.Value);
                    var distanceToMoney = CommonClass.Geography.getLengthOfTwoPoint.GetDistance(from.Latitde, from.Longitude, from.Height, fpTo.Latitde, fpTo.Longitude, fpTo.Height);
                    if (distanceToMoney <= distanceToDiamond)
                    {
                        distanceToDiamond = distanceToMoney;
                        fp = fpTo;
                    }
                }
                car.DirectAttack = false;
                return fp == null;
            }
            else
            {
                throw new Exception("非法调用");
            }
        }

        public bool theNearestToPlayerIsCarNotMoney(RoleInGame player, Car car, RoleInGame victim, GetRandomPos grp, out OssModel.FastonPosition fp)
        {
            return theNearestToObjIsCarNotMoney(player, car, victim, grp, out fp);
        }
        public int GetPromotePositionTo(string pType)
        {
            switch (pType)
            {
                case "mile": { return this.promoteMilePosition; }; ;
                case "business": { return this.promoteBusinessPosition; };
                case "volume": { return this.promoteVolumePosition; };
                case "speed": { return this.promoteSpeedPosition; };
                default:
                    {
                        throw new Exception($"{pType}没有定义");
                    }
            }
        }





        /// <summary>
        /// 更新所有玩家的功能提升点
        /// </summary>
        /// <param name="pType"></param>
        /// <returns></returns>
        public void CheckAllPlayersPromoteState(string pType)
        {
            var all = getGetAllRoles();
            for (var i = 0; i < all.Count; i++)
            {
                CheckPromoteState(all[i].Key, pType);
            }
        }

        private void CheckPromoteState(string key, string promoteType)
        {
            string url = "";
            string sendMsg = "";
            lock (this.PlayerLock)
                if (this._Players.ContainsKey(key))
                    if (this._Players[key].playerType == RoleInGame.PlayerType.player)
                        if (((Player)this._Players[key]).PromoteState[promoteType] == this.getPromoteState(promoteType))
                        {
                        }
                        else
                        {
                            var infomation = Program.rm.GetPromoteInfomation(((Player)this._Players[key]).WebSocketID, promoteType);
                            url = ((Player)this._Players[key]).FromUrl;
                            sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(infomation);
                            ((Player)this._Players[key]).PromoteState[promoteType] = this.getPromoteState(promoteType);
                        }
            if (!string.IsNullOrEmpty(url))
            {
                Startup.sendSingleMsg(url, sendMsg);
            }
        }



        public void DiamondInCarChanged(Player player, Car car, ref List<string> notifyMsgs, string value)
        {
            foreach (var item in this._Players)
            {
                if (item.Value.playerType == RoleInGame.PlayerType.player)
                {
                    var playerNeedToTold = (Player)item.Value;
                    var obj = new BradCastPromoteDiamondInCar
                    {
                        c = "BradCastPromoteDiamondOnCar",
                        WebSocketID = playerNeedToTold.WebSocketID,
                        pType = car.ability.diamondInCar,
                        roleID = player.Key
                    };
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                    notifyMsgs.Add(playerNeedToTold.FromUrl);
                    notifyMsgs.Add(json);
                }
            }
        }

        private BradCastPromoteInfoDetail GetPromoteInfomation(int webSocketID, string resultType)
        {
            switch (resultType)
            {
                case "mile":
                    {
                        var obj = new BradCastPromoteInfoDetail
                        {
                            c = "BradCastPromoteInfoDetail",
                            WebSocketID = webSocketID,
                            resultType = resultType,
                            Fp = Program.dt.GetFpByIndex(this.promoteMilePosition),
                            Price = this.promotePrice[resultType]
                        };
                        return obj;
                    };
                case "business":
                    {
                        var obj = new BradCastPromoteInfoDetail
                        {
                            c = "BradCastPromoteInfoDetail",
                            WebSocketID = webSocketID,
                            resultType = resultType,
                            Fp = Program.dt.GetFpByIndex(this.promoteBusinessPosition),
                            Price = this.promotePrice[resultType]
                        };
                        return obj;
                    };
                case "volume":
                    {
                        var obj = new BradCastPromoteInfoDetail
                        {
                            c = "BradCastPromoteInfoDetail",
                            WebSocketID = webSocketID,
                            resultType = resultType,
                            Fp = Program.dt.GetFpByIndex(this.promoteVolumePosition),
                            Price = this.promotePrice[resultType]
                        };
                        return obj;
                    };
                case "speed":
                    {
                        var obj = new BradCastPromoteInfoDetail
                        {
                            c = "BradCastPromoteInfoDetail",
                            WebSocketID = webSocketID,
                            resultType = resultType,
                            Fp = Program.dt.GetFpByIndex(this.promoteSpeedPosition),
                            Price = this.promotePrice[resultType]
                        };
                        return obj;
                    };
                default: { }; break;
            }
            throw new Exception("");
        }
        public int getPromoteState(string pType)
        {
            switch (pType)
            {
                case "mile":
                    {
                        return this.promoteMilePosition;
                    }
                case "business":
                    {
                        return this.promoteBusinessPosition;
                    }; ;
                case "volume":
                    {
                        return this.promoteVolumePosition;
                    };
                case "speed":
                    {
                        return this.promoteSpeedPosition;
                    };
                default:
                    {
                        throw new Exception($"{pType}是什么类型");
                    };
            }
        }

        public void setPromtePosition(string changeType)
        {
            if (changeType == "mile")
                this.promoteMilePosition = GetRandomPosition(true, Program.dt);
            else if (changeType == "business")
                this.promoteBusinessPosition = GetRandomPosition(true, Program.dt);
            else if (changeType == "volume")
                this.promoteVolumePosition = GetRandomPosition(true, Program.dt);
            else if (changeType == "speed")
                this.promoteSpeedPosition = GetRandomPosition(true, Program.dt);
            else
            {
                throw new Exception($"{changeType}是什么类型？");
            }
            this.promotePrice[changeType] = this.GetPriceOfPromotePosition(changeType);
        }

        /// <summary>
        /// 各种原因，使小车从基地里没有出发。小车上的钱必须从下车返回为玩家！
        /// </summary>
        /// <param name="role"></param>
        /// <param name="car"></param>
        private void giveMoneyFromCarToPlayer(RoleInGame role, Car car, ref List<string> notifyMsg)
        {
            //   var m1 = player.GetMoneyCanSave();
            role.MoneySet(role.Money + car.ability.leftBusiness + car.ability.leftVolume, ref notifyMsg);
            //player.Money += car.ability.leftBusiness;
            //player.Money += car.ability.leftVolume;

            //if (car.ability.leftBusiness > 0)
            //    printState(role, car, $"返回基站，通过leftBusiness，是流动资金增加{car.ability.leftBusiness}");
            //if (car.ability.leftVolume > 0)
            //    printState(role, car, $"返回基站，通过leftBusiness，是流动资金增加{car.ability.leftVolume}");
            //if (car.ability.subsidize > 0)
            //{
            //    /*
            //     * 从逻辑上，必须要保证car.ability.subsidize>0,player.SupportToPlay!=null
            //     */
            //    player.setSupportToPlayMoney(player.SupportToPlayMoney + car.ability.subsidize, ref notifyMsg);
            //    //player.SupportToPlayMoney += car.ability.subsidize;
            //    printState(player, car, $"返回基站，返还资助{car.ability.subsidize}");
            //}
            car.Refresh(role, ref notifyMsg);

            if (!string.IsNullOrEmpty(car.ability.diamondInCar))
            {
                role.PromoteDiamondCount[car.ability.diamondInCar]++;
            }
            car.ability.Refresh(role, car, ref notifyMsg);
            //  var m2 = player.GetMoneyCanSave();
            //  if (m1 != m2)
            {
                // MoneyCanSaveChanged(player, m2, ref notifyMsg);
            }
        }


        private void SendPromoteCountOfPlayer(string pType, Player player, ref List<string> notifyMsgs)
        {
            if (!(pType == "mile" || pType == "business" || pType == "volume" || pType == "speed"))
            {

            }
            else
            {
                var count = player.PromoteDiamondCount[pType];
                var obj = new BradCastPromoteDiamondCount
                {
                    c = "BradCastPromoteDiamondCount",
                    count = count,
                    WebSocketID = player.WebSocketID,
                    pType = pType
                };
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                notifyMsgs.Add(player.FromUrl);
                notifyMsgs.Add(json);
            }
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 获取一个玩家的 四个能力提升点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private void CheckAllPromoteState(string key)
        {
            CheckPromoteState(key, "mile");
            CheckPromoteState(key, "business");
            CheckPromoteState(key, "volume");
            CheckPromoteState(key, "speed");
        }
    }
}
