using CommonClass;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static HouseManager4_0.Car;

namespace HouseManager4_0.RoomMainF
{
    public partial class RoomMain : interfaceOfHM.Collect

    {

        /// <summary>
        /// 获取收集金钱的状态
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private void CheckCollectState(string key)
        {

            //string url = "";
            //string sendMsg = "";
            List<string> sendMsgs = new List<string>();
            lock (this.PlayerLock)
                if (this._Players.ContainsKey(key))
                    for (var i = 0; i < 38; i++)
                    {
                        if (this._Players[key].CollectPosition[i] == this._collectPosition[i])
                        { }
                        else
                        {
                            if (this._Players[key].playerType == RoleInGame.PlayerType.player)
                            {
                                var infomation = Program.rm.GetCollectInfomation(((Player)this._Players[key]).WebSocketID, i);
                                var url = ((Player)this._Players[key]).FromUrl;
                                var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(infomation);
                                sendMsgs.Add(url);
                                sendMsgs.Add(sendMsg);
                            }
                            this._Players[key].CollectPosition[i] = this._collectPosition[i];
                        }
                    }

            Startup.sendSeveralMsgs(sendMsgs); 
        }
        private BradCastCollectInfoDetail_v2 GetCollectInfomation(int webSocketID, int collectIndex)
        {
            var obj = new BradCastCollectInfoDetail_v2
            {
                c = "BradCastCollectInfoDetail_v2",
                WebSocketID = webSocketID,
                Fp = Program.dt.GetFpByIndex(this._collectPosition[collectIndex]),
                collectMoney = this.GetCollectReWard(collectIndex),
                collectIndex = collectIndex
            };
            return obj;
        }

        public int GetCollectReWard(int collectIndex)
        {
            switch (collectIndex)
            {
                case 0:
                    {
                        return 100;
                    };
                case 1:
                case 2:
                    {
                        return 50;
                    }
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                    {
                        return 20;
                    }
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                    {
                        return 10;
                    }
                case 18:
                case 19:
                case 20:
                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                case 26:
                case 27:
                case 28:
                case 29:
                case 30:
                case 31:
                case 32:
                case 33:
                case 34:
                case 35:
                case 36:
                case 37:
                    { return 5; }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }
        public void CheckAllPlayersCollectState()
        {
            var all = getGetAllRoles();
            for (var i = 0; i < all.Count; i++)
            {
                CheckCollectState(all[i].Key);
            }
        }
        public enum MileResultReason
        {
            /// <summary>
            /// 里程充足
            /// </summary>
            Abundant,
            /// <summary>
            /// 不能到达
            /// </summary>
            CanNotReach,
            /// <summary>
            /// 能到达但是不能返回
            /// </summary>
            CanNotReturn,
            MoneyIsNotEnougt,
            NearestIsMoneyWhenPromote,
            NearestIsMoneyWhenAttack
        }
        public string updateCollect(SetCollect sc, GetRandomPos grp)
        {
            return this.collectE.updateCollect(sc, grp);
        }




        /// <summary>
        /// 将序号按距离进行排序
        /// </summary>
        /// <param name="target"></param>
        /// <returns>返回的值为0至37的排序</returns>
        internal List<int> getCollectPositionsByDistance(FastonPosition target, GetRandomPos grp)
        {
            List<int> positions = new List<int>();
            for (int i = 0; i < 38; i++)
            {
                positions.Add(i);
                //var collectP = Program.dt.GetFpByIndex(this._collectPosition[i]);
                //positions.Add(collectP);
            }
            positions = (from item in positions orderby CommonClass.Geography.getLengthOfTwoPoint.GetDistance(target.Latitde, target.Longitude, target.Height, grp.GetFpByIndex(this._collectPosition[item]).Latitde, grp.GetFpByIndex(this._collectPosition[item]).Longitude, grp.GetFpByIndex(this._collectPosition[item]).Height) select item).ToList();
            return positions;
        }

        //private long getCollectReWardByReward(int target)
        //{
        //    foreach (var item in this._collectPosition)
        //    {
        //        if (item.Value == target)
        //        {
        //            return this.GetCollectReWard(item.Key) * 100;
        //        }
        //    }
        //    return 0;
        //    // throw new NotImplementedException();
        //}
        ///// <summary>
        ///// 修改小车相关属性
        ///// </summary>
        ///// <param name="car"></param>
        ///// <param name="to"></param>
        ///// <param name="fp1"></param>
        ///// <param name="sc"></param>
        ///// <param name="goPath"></param>
        //private void EditCarStateWhenCollectStartOK(RoleInGame player, ref Car car, int to, Model.FastonPosition fp1, SetCollect sc, List<Model.MapGo.nyrqPosition> goPath, ref List<string> notifyMsg, out int startT)
        //{

        //    car.targetFpIndex = to;//A.更改小车目标，在其他地方引用。

        //    //car.purpose = Purpose.collect;//B.更改小车目的，用户操作控制
        //    //    car.changeState++;//C.更改状态用去前台更新动画   
        //    /*
        //     * 步骤C已经封装进 car.setAnimateData
        //     */
        //    /*
        //     * D.更新小车动画参数
        //     */

        //    var speed = car.ability.Speed;
        //    startT = 0;
        //    List<int> result;
        //    Data.PathStartPoint2 startPosition;
        //    if (car.state == CarState.waitAtBaseStation)
        //    {
        //        getStartPositionByFp(out startPosition, fp1);
        //        result = getStartPositon(fp1, player.positionInStation, ref startT);
        //    }
        //    else if (car.state == CarState.waitOnRoad)
        //    {
        //        result = new List<int>();
        //        getStartPositionByGoPath(out startPosition, goPath);
        //    }
        //    else
        //    {
        //        throw new Exception($"未知情况！{Newtonsoft.Json.JsonConvert.SerializeObject(car)}");
        //    }
        //    car.setState(player, ref notifyMsg, CarState.working);
        //    //car.state = CarState.roadForCollect;

        //    Program.dt.GetAFromBPoint(goPath, fp1, speed, ref result, ref startT);
        //    //  result.RemoveAll(item => item.t == 0);

        //    car.setAnimateData(player, ref notifyMsg, new AnimateData2()
        //    {
        //        start = startPosition,
        //        animateData = result,
        //        recordTime = DateTime.Now
        //    });
        //}

        private int getCollectPositionTo(int collectIndex)
        {
            if (collectIndex >= 0 && collectIndex < 38)
            {
                return this._collectPosition[collectIndex];
            }
            else
                throw new Exception("parameter is wrong!");
        }


    }
}
