using CommonClass;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using static HouseManager4_0.Car;

namespace HouseManager4_0.RoomMainF
{
    public partial class RoomMain : interfaceOfHM.Promote
    {
        public void SetLookForPromote()
        {
            this.promoteMilePosition = GetRandomPosition(true);
            this.promoteBusinessPosition = GetRandomPosition(true);
            this.promoteVolumePosition = GetRandomPosition(true);
            this.promoteSpeedPosition = GetRandomPosition(true);
        }

        public string updatePromote(SetPromote sp)
        {
            return this.promoteE.updatePromote(sp);
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
        //        Console.WriteLine("小车从基站出发，身上的钱，没有清零！");
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



        private void EditCarStateWhenPromoteStartOK(RoleInGame role, ref Car car, int to, Model.FastonPosition fp1, int to2, SetPromote sp, List<Model.MapGo.nyrqPosition> goPath, ref List<string> nofityMsgs, out int startT)
        {
            car.targetFpIndex = to;//A.更改小车目标，在其他地方引用。
                                   //  car.changeState++;//B.更改状态用去前台更新动画    

            /*
             * C.更新小车动画参数
             */
            var speed = car.ability.Speed;
            startT = 0;
            List<int> result;
            Data.PathStartPoint2 startPosition;
            if (car.state == CarState.waitOnRoad)
            {
                result = new List<int>();
                getStartPositionByGoPath(out startPosition, goPath);
                //startPosition = new Data.PathStartPoint()
                //{
                //    x = goPath.First().BDlongitude,
                //    y = goPath.First().BDlatitude
                //};
            }
            else if (car.state == CarState.waitAtBaseStation)
            {
                result = getStartPositon(fp1, role.positionInStation, ref startT);
                getStartPositionByFp(out startPosition, fp1);
            }
            else
            {
                Console.WriteLine($"{Newtonsoft.Json.JsonConvert.SerializeObject(car)}");
                throw new Exception("错误的汽车类型！！！");
            }
            Program.dt.GetAFromBPoint(goPath, fp1, speed, ref result, ref startT);
            // result.RemoveAll(item => item.t == 0);

            var animateData = new AnimateData2()
            {
                start = startPosition,
                animateData = result,
                recordTime = DateTime.Now
            };
            car.setAnimateData(role, ref nofityMsgs, animateData);
            //car.animateData = new AnimateData()
            //{
            //    animateData = result,
            //    recordTime = DateTime.Now
            //};
            car.setState(role, ref nofityMsgs, CarState.working);
            //  car.state = CarState.buying;//更改汽车状态

            //Thread th = new Thread(() => setDiamondOwner(startT, new commandWithTime.diamondOwner()
            //{
            //    c = "diamondOwner",
            //    key = sp.Key,
            //    car = sp.car,
            //    returnPath = returnPath,
            //    target = to,//新的起点
            //    changeType = sp.pType,
            //    costMile = goMile
            //}));
            //th.Start();
            //car.changeState++;//更改状态

            //getAllCarInfomations(sp.Key, ref notifyMsg);
        }

        internal bool theNearestIsDiamondNotCar(RoleInGame player, Car car, string pType, out int collectIndex)
        {
            if (car.state == CarState.waitAtBaseStation)
            {
                double distanceToDiamond;
                {
                    var from = Program.dt.GetFpByIndex(player.StartFPIndex);
                    var to = this.GetPromotePositionTo(pType);
                    var fpTo = Program.dt.GetFpByIndex(to);
                    distanceToDiamond = CommonClass.Geography.getLengthOfTwoPoint.GetDistance(from.Latitde, from.Longitude, fpTo.Latitde, fpTo.Longitude);
                }
                foreach (var item in this._collectPosition)
                {
                    var from = Program.dt.GetFpByIndex(player.StartFPIndex);
                    var fpTo = Program.dt.GetFpByIndex(item.Value);
                    var distanceToMoney = CommonClass.Geography.getLengthOfTwoPoint.GetDistance(from.Latitde, from.Longitude, fpTo.Latitde, fpTo.Longitude);
                    if (distanceToMoney <= distanceToDiamond)
                    {
                        collectIndex = item.Key;
                        return false;
                    }
                }
                collectIndex = -1;
                return true;
            }
            else if (car.state == CarState.waitOnRoad)
            {
                double distanceToDiamond;
                {
                    var from = Program.dt.GetFpByIndex(car.targetFpIndex);
                    var to = this.GetPromotePositionTo(pType);
                    var fpTo = Program.dt.GetFpByIndex(to);
                    distanceToDiamond = CommonClass.Geography.getLengthOfTwoPoint.GetDistance(from.Latitde, from.Longitude, fpTo.Latitde, fpTo.Longitude);
                }
                foreach (var item in this._collectPosition)
                {
                    var from = Program.dt.GetFpByIndex(car.targetFpIndex);
                    var fpTo = Program.dt.GetFpByIndex(item.Value);
                    var distanceToMoney = CommonClass.Geography.getLengthOfTwoPoint.GetDistance(from.Latitde, from.Longitude, fpTo.Latitde, fpTo.Longitude);
                    if (distanceToMoney <= distanceToDiamond)
                    {
                        collectIndex = item.Key;
                        return false;
                    }
                }
                collectIndex = -1;
                return true;
            }
            else
            {
                collectIndex = -2;
                return false;
            }
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
                Startup.sendMsg(url, sendMsg);
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
                this.promoteMilePosition = GetRandomPosition(true);
            else if (changeType == "business")
                this.promoteBusinessPosition = GetRandomPosition(true);
            else if (changeType == "volume")
                this.promoteVolumePosition = GetRandomPosition(true);
            else if (changeType == "speed")
                this.promoteSpeedPosition = GetRandomPosition(true);
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
