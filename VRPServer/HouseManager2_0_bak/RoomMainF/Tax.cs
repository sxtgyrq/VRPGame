﻿using CommonClass;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static HouseManager2_0.Car;

namespace HouseManager2_0.RoomMainF
{
    public partial class RoomMain
    {
        private static void TaxAdded(Player player, int placeIndex, long AddValue, ref List<string> msgsWithUrl)
        {
            var url = player.FromUrl;

            TaxNotify tn = new TaxNotify()
            {
                c = "TaxNotify",
                fp = Program.dt.GetFpByIndex(placeIndex),
                WebSocketID = player.WebSocketID,
                tax = AddValue,
                target = placeIndex
            };

            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(tn);
            msgsWithUrl.Add(url);
            msgsWithUrl.Add(sendMsg);
        }
        private void SendAllTax(string key)
        {
            List<string> notifyMsg = new List<string>();
            SendAllTax(key, ref notifyMsg);
            for (var i = 0; i < notifyMsg.Count; i += 2)
            {
                var url = notifyMsg[i];
                var sendMsg = notifyMsg[i + 1];
                Startup.sendMsg(url, sendMsg);
            }
        }
        private void SendAllTax(string key, ref List<string> msgsWithUrl)
        {
            if (this._Players[key].playerType == RoleInGame.PlayerType.player)
            {

                var url = ((Player)this._Players[key]).FromUrl;
                //   List<TaxWebObj> objs = new List<TaxWebObj>();
                var positions = this._Players[key].TaxInPositionForeach();
                for (var i = 0; i < positions.Count; i++)
                {
                    var tax = this._Players[key].GetTaxByPositionIndex(positions[i]);
                    TaxNotify tn = new TaxNotify()
                    {
                        c = "TaxNotify",
                        fp = Program.dt.GetFpByIndex(positions[i]),
                        WebSocketID = ((Player)this._Players[key]).WebSocketID,
                        tax = tax,
                        target = positions[i]
                    };
                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(tn);
                    msgsWithUrl.Add(url);
                    msgsWithUrl.Add(sendMsg);
                }
            }


        }




        private void arriveThenGetTax(ref RoleInGame player, ref Car car, commandWithTime.placeArriving pa, ref List<string> notifyMsg)
        {
            if (car.targetFpIndex == -1)
            {
                throw new Exception("这个地点应该是执行税收和即将要等待的地点！");
            }
            if (player.GetTaxByPositionIndex(car.targetFpIndex) > 0)
            {
                //确定税收的值
                var tax = Math.Min(car.ability.leftBusiness, player.GetTaxByPositionIndex(car.targetFpIndex));

                car.ability.setCostBusiness(car.ability.costBusiness + tax, player, car, ref notifyMsg);
                //   car.ability.costBusiness += tax;
                // AbilityChanged(player, car, ref notifyMsg, "business");

                // player.TaxInPosition[car.targetFpIndex] -= tax;
                player.SetTaxByPositionIndex(car.targetFpIndex, player.GetTaxByPositionIndex(car.targetFpIndex) - tax, ref notifyMsg);

                //if (player.TaxInPosition[car.targetFpIndex] == 0)
                //{
                //    player.TaxInPosition.Remove(car.targetFpIndex);
                //}
                printState(player, car, $"{Program.dt.GetFpByIndex(car.targetFpIndex).FastenPositionName}收取{tax}");

                //car.ability.costMiles += pa.costMile;//
                car.ability.setCostMiles(car.ability.costMiles + pa.costMile, player, car, ref notifyMsg);

                //AbilityChanged(player, car, ref notifyMsg, "mile");

                carParkOnRoad(pa.target, ref car, player, ref notifyMsg);
                //  SendSingleTax(player.Key, car.targetFpIndex, ref notifyMsg);
                if (car.purpose == Purpose.tax && car.state == CarState.roadForTax)
                {
                    car.setState(player, ref notifyMsg, CarState.waitForTaxOrAttack);
                    //car.state = CarState.waitForTaxOrAttack;
                    this._Players[pa.key].returningRecord = pa.returnPath;

                    //第二步，更改状态
                    //car.changeState++;
                    //getAllCarInfomations(pa.key, ref notifyMsg);
                }
            }
            else
            {
                var tax = 0;
                printState(player, car, $"{Program.dt.GetFpByIndex(car.targetFpIndex).FastenPositionName}收取{tax}");

                car.ability.setCostMiles(car.ability.costMiles + pa.costMile, player, car, ref notifyMsg);

                //AbilityChanged(player, car, ref notifyMsg, "mile");

                carParkOnRoad(pa.target, ref car, player, ref notifyMsg);
                //  SendSingleTax(player.Key, car.targetFpIndex, ref notifyMsg);
                if (car.purpose == Purpose.tax && car.state == CarState.roadForTax)
                {
                    car.setState(player, ref notifyMsg, CarState.waitForTaxOrAttack);
                    //car.state = CarState.waitForTaxOrAttack;
                    this._Players[pa.key].returningRecord = pa.returnPath;

                    //第二步，更改状态
                    //car.changeState++;
                    //getAllCarInfomations(pa.key, ref notifyMsg);
                }
            }

            NPCAutoControlTax(player);
            //if (role.playerType == RoleInGame.PlayerType.NPC)
            //{
            //    if (role.getCar().ability.costVolume >= role.getCar().ability.Volume)
            //    {
            //        SetNPCToDoSomeThing((NPC)role, NPCAction.Attack);
            //    }
            //    else
            //    {
            //        SetNPCToDoSomeThing((NPC)role, NPCAction.Collect);
            //    }
            //}
        }



        internal string updateTax(SetTax st)
        {

            {
                List<string> notifyMsg = new List<string>();
                lock (this.PlayerLock)
                {
                    if (this._Players.ContainsKey(st.Key))
                    {
                        if (this._Players[st.Key].Bust) { }
                        else if (this._Players[st.Key].TaxContainsKey(st.target))
                        {
                            var player = this._Players[st.Key];
                            var car = this._Players[st.Key].getCar();
                            switch (car.state)
                            {
                                case CarState.waitAtBaseStation:
                                    {
                                        if (car.purpose == Purpose.@null)
                                        {
                                            if (car.ability.leftBusiness > 0)
                                            {
                                                MileResultReason reason;
                                                DoCollectTaxF(player, car, st, ref notifyMsg, out reason);
                                                if (reason == MileResultReason.Abundant)
                                                {
                                                    printState(player, car, "已经在收税的路上了");
                                                }
                                                else
                                                {
                                                    printState(player, car, "没有能启动税收！");
                                                }
                                            }
                                            else
                                            {
                                                throw new Exception("从基地里出来，leftBusiness 就不为0！");
                                            }

                                        }
                                    }; break;
                                case CarState.waitOnRoad:
                                    {
                                        if (car.purpose == Purpose.@null || car.purpose == Purpose.tax)
                                        {
                                            if (car.ability.leftBusiness > 0)
                                            {
                                                MileResultReason reason;
                                                DoCollectTaxF(player, car, st, ref notifyMsg, out reason);
                                                if (reason == MileResultReason.Abundant)
                                                {
                                                    printState(player, car, "已经在税收的路上了");
                                                }
                                                else
                                                {
                                                    printState(player, car, $"里程问题，被安排回去");
                                                    getTaxFailedThenReturn(car, player, st, ref notifyMsg);
                                                }
                                            }
                                            else
                                            {
                                                printState(player, car, $"业务已满，被安排回去");
                                                getTaxFailedThenReturn(car, player, st, ref notifyMsg);
                                            }
                                        }
                                    }; break;
                                case CarState.waitForTaxOrAttack:
                                    {
                                        if (car.purpose == Purpose.tax)
                                        {
                                            if (car.ability.leftBusiness > 0)
                                            {
                                                MileResultReason reason;
                                                DoCollectTaxF(player, car, st, ref notifyMsg, out reason);
                                                if (reason == MileResultReason.Abundant)
                                                {
                                                    printState(player, car, "已经在税收的路上了");
                                                }
                                                else
                                                {
                                                    printState(player, car, $"里程问题，被安排回去");
                                                    getTaxFailedThenReturn(car, player, st, ref notifyMsg);
                                                }
                                            }
                                            else
                                            {
                                                printState(player, car, $"业务已满，被安排回去");
                                                getTaxFailedThenReturn(car, player, st, ref notifyMsg);
                                            }
                                        }
                                    }; break;

                            }
                        }

                    }
                }

                for (var i = 0; i < notifyMsg.Count; i += 2)
                {
                    var url = notifyMsg[i];
                    var sendMsg = notifyMsg[i + 1];
                    Startup.sendMsg(url, sendMsg);
                }
                return "";
            }
        }


        void DoCollectTaxF(RoleInGame player, Car car, SetTax st, ref List<string> notifyMsg, out MileResultReason reason)
        {
            var from = this.getFromWhenDoCollectTax(player, car);
            var to = st.target;
            var fp1 = Program.dt.GetFpByIndex(from);
            var fp2 = Program.dt.GetFpByIndex(to);
            var baseFp = Program.dt.GetFpByIndex(player.StartFPIndex);

            // var goPath = Program.dt.GetAFromB(from, to);
            var goPath = this.GetAFromB(from, to, player, ref notifyMsg);
            // var returnPath = Program.dt.GetAFromB(to, player.StartFPIndex);
            var returnPath = this.GetAFromB(to, player.StartFPIndex, player, ref notifyMsg);

            var goMile = GetMile(goPath);
            var returnMile = GetMile(returnPath);

            if (car.ability.leftMile >= goMile + returnMile)
            {
                car.targetFpIndex = to;
                Console.WriteLine($"car的目标设置成了{Program.dt.GetFpByIndex(to).FastenPositionName}");

                car.setPurpose(player, ref notifyMsg, Purpose.tax);
                //car.purpose = Purpose.tax;
                var speed = car.ability.Speed;
                int startT = 0;
                List<int> result;
                Data.PathStartPoint2 startPosition;
                if (car.state == CarState.waitAtBaseStation)
                {
                    result = getStartPositon(fp1, player.positionInStation, ref startT);
                    this.getStartPositionByFp(out startPosition, fp1);
                }
                else if (car.state == CarState.waitForTaxOrAttack)
                {
                    result = new List<int>();
                    if (from == to)
                    {
                        startPosition = null;
                        // this.getStartPositionByFp(out startPosition, fp1);
                    }
                    else
                    {
                        this.getStartPositionByGoPath(out startPosition, goPath);
                    }
                }
                else if (car.state == CarState.waitOnRoad && car.ability.diamondInCar == "" && (car.purpose == Purpose.@null || car.purpose == Purpose.tax))
                {
                    result = new List<int>();
                    if (from == to)
                    {
                        startPosition = null;
                    }
                    else
                    {
                        this.getStartPositionByGoPath(out startPosition, goPath);
                    }
                }
                else
                {
                    throw new Exception($"未知情况！{Newtonsoft.Json.JsonConvert.SerializeObject(car)}");
                }
                car.setState(player, ref notifyMsg, CarState.roadForTax);
                //car.state = CarState.roadForTax;
                Program.dt.GetAFromBPoint(goPath, fp1, speed, ref result, ref startT);
                // result.RemoveAll(item => item.t == 0);

                var animateData = new AnimateData2()
                {
                    start = startPosition,
                    animateData = result,
                    recordTime = DateTime.Now
                };
                car.setAnimateData(player, ref notifyMsg, animateData);

                //car.animateData = new AnimateData()
                //{
                //    animateData = result,
                //    recordTime = DateTime.Now
                //};
                Thread th = new Thread(() => setArrive(startT, new commandWithTime.placeArriving()
                {
                    c = "placeArriving",
                    key = st.Key,
                    //   car = st.car,
                    returnPath = returnPath,
                    target = to,
                    costMile = goMile
                }));
                th.Start();


                reason = MileResultReason.Abundant;

                //   car.changeState++;//更改状态  
                car.setPurpose(player, ref notifyMsg, Purpose.tax);
                //car.purpose = Purpose.tax;

                //getAllCarInfomations(st.Key, ref notifyMsg);
            }

            else if (car.ability.leftMile >= goMile)
            {
                //当攻击失败，必须返回
                Console.Write($"去程{goMile}，回程{returnMile}");
                Console.Write($"你去了回不来");

#warning 这里要在前台进行提示 
                reason = MileResultReason.CanNotReturn;
            }
            else
            {
#warning 这里要在web前台进行提示
                //当攻击失败，必须返回
                Console.Write($"去程{goMile}，回程{returnMile}");
                Console.Write($"你去不了");
                reason = MileResultReason.CanNotReach;
            }
        }

        private int getFromWhenDoCollectTax(RoleInGame player, Car car)
        {
            switch (car.state)
            {
                case CarState.waitAtBaseStation:
                    {
                        return player.StartFPIndex;
                    };
                case CarState.waitOnRoad:
                    {
                        //小车的上一个的目标
                        if (car.targetFpIndex == -1)
                        {
                            throw new Exception("参数混乱");
                        }
                        else if (car.purpose == Purpose.tax || car.purpose == Purpose.@null)
                        {
                            return car.targetFpIndex;
                        }
                        else
                        {
                            //出现这种情况，应该是回了基站里没有初始
                            throw new Exception("参数混乱");
                        }
                    };
                case CarState.waitForTaxOrAttack:
                    {
                        if (car.targetFpIndex == -1)
                        {
                            throw new Exception("参数混乱");
                        }
                        else if (car.purpose == Purpose.tax)
                        {
                            return car.targetFpIndex;
                        }
                        else
                        {
                            throw new Exception("参数混乱");
                        }
                    };
                default:
                    {
                        throw new Exception("错误的汽车状态");
                    }
            }
        }

        private void getTaxFailedThenReturn(Car car, RoleInGame player, SetTax st, ref List<string> notifyMsg)
        {
            if (car.state == CarState.waitForTaxOrAttack || car.state == CarState.waitOnRoad)
            {
                //Console.Write($"现在剩余业务为{car.ability.leftBusiness}，总业务为{car.ability.Business}");
                //Console.Write($"你装不下了！");
                Console.Write($"该汽车被安排回去了");
                var from = getFromWhenDoCollectTax(this._Players[st.Key], car);
                int startT = 1;
                var returnPath_Record = this._Players[st.Key].returningRecord;
                Thread th = new Thread(() => setReturn(startT, new commandWithTime.returnning()
                {
                    c = "returnning",
                    key = st.Key,
                    // car = st.car,
                    returnPath = returnPath_Record,
                    target = from,
                    changeType = "tax-return",
                }));
                th.Start();

                // car.changeState++;//更改状态  

            }
        }


    }
}