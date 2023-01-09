using CommonClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static HouseManager4_0.Car;
using static HouseManager4_0.RoomMainF.RoomMain;
using static HouseManager4_0.RoomMainF.RoomMain.commandWithTime;

namespace HouseManager4_0
{
    public abstract class Engine : EngineAndManger
    {
        internal string updateAction(interfaceOfEngine.tryCatchAction actionDo, Command c, GetRandomPos grp, string operateKey)
        {
            string conditionNotReason;
            if (actionDo.conditionsOk(c, grp, out conditionNotReason))
            {
                List<string> notifyMsg = new List<string>();
                lock (that.PlayerLock)
                {
                    if (that._Players.ContainsKey(operateKey))
                    {
                        if (that._Players[operateKey].Bust) { }
                        else
                        {
                            var player = that._Players[operateKey];
                            var car = that._Players[operateKey].getCar();
                            switch (car.state)
                            {
                                case CarState.waitAtBaseStation:
                                case CarState.waitOnRoad:
                                    {
                                        if (actionDo.carAbilitConditionsOk(player, car, c, grp))
                                        {
                                            MileResultReason mrr;
                                            RoomMainF.RoomMain.commandWithTime.ReturningOjb returningOjb = actionDo.maindDo(player, car, c, grp, ref notifyMsg, out mrr);

                                            switch (mrr)
                                            {
                                                case MileResultReason.Abundant:
                                                    {
                                                        player.returningOjb = returningOjb;
                                                    }; break;
                                                case MileResultReason.CanNotReach:
                                                    {
                                                        actionDo.failedThenDo(car, player, c, grp, ref notifyMsg);
                                                        this.WebNotify(player, "小车不能到达目的地，被安排返回！");
                                                    }
                                                    break;
                                                case MileResultReason.CanNotReturn:
                                                    {
                                                        actionDo.failedThenDo(car, player, c, grp, ref notifyMsg);
                                                        this.WebNotify(player, "小车到达目的地后不能返回，在当前地点安排返回！");
                                                    }; break;
                                                case MileResultReason.MoneyIsNotEnougt:
                                                    {
                                                        actionDo.failedThenDo(car, player, c, grp, ref notifyMsg);
                                                    }; break;
                                                case MileResultReason.NearestIsMoneyWhenPromote: { }; break;
                                                case MileResultReason.NearestIsMoneyWhenAttack:
                                                    {
                                                        if (mrr == MileResultReason.NearestIsMoneyWhenAttack)
                                                        {
                                                            if (player.playerType == RoleInGame.PlayerType.NPC)
                                                            {
                                                                actionDo.failedThenDo(car, player, c, grp, ref notifyMsg);
                                                            }
                                                            // this.WebNotify(player, $"离宝石最近的是钱，不是你的车。请离宝石再近点儿！");
                                                        }
                                                    }; break;
                                            }
                                        }
                                        else
                                        {
                                            if (player.playerType == RoleInGame.PlayerType.NPC)
                                            {
                                                actionDo.failedThenDo(car, player, c, grp, ref notifyMsg);
                                            };
                                            // 
                                        }
                                    }; break;
                                case CarState.working:
                                    {
                                        this.WebNotify(player, "您的小车正在赶往目标！");
                                    }; break;
                                case CarState.returning:
                                    {
                                        WebNotify(player, "您的小车正在返回！");
                                    }; break;
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
            else
            {
                return conditionNotReason;
            }
        }
        /// <summary>
        /// 这个方法主要针对去程，不针对回程。
        /// </summary>
        /// <param name="player"></param>
        /// <param name="car"></param>
        /// <param name="to"></param>
        /// <param name="fp1"></param>
        /// <param name="goPath"></param>
        /// <param name="notifyMsg"></param>
        /// <param name="startT_FirstPath"></param>
        /// <exception cref="Exception"></exception>
        public void EditCarStateWhenActionStartOK(RoleInGame player, ref Car car, int to, Model.FastonPosition fp1, Node goPath, GetRandomPos grp, ref List<string> notifyMsg, out int startT_FirstPath)
        {
            car.targetFpIndexSet(to, ref notifyMsg);//A.更改小车目标，在其他地方引用。

            //car.purpose = Purpose.collect;//B.更改小车目的，用户操作控制
            //    car.changeState++;//C.更改状态用去前台更新动画   
            /*
             * 步骤C已经封装进 car.setAnimateData
             */
            /*
             * D.更新小车动画参数
             */

            var privateKeys = BitCoin.GamePathEncryption.PathEncryption.MainC.GetPrivateKeys(ref that.rm, goPath.path.Count);
            List<AnimateDataItem> animations = new List<AnimateDataItem>();
            {
                var speed = car.ability.Speed;
                startT_FirstPath = 0;
                List<int> result;
                Data.PathStartPoint3 startPosition;
                if (car.state == CarState.waitAtBaseStation)
                {
                    result = that.getStartPositon(fp1, player.positionInStation, ref startT_FirstPath, out startPosition, player.improvementRecord.speedValue > 0);

                }
                else if (car.state == CarState.waitOnRoad)
                {
                    result = new List<int>();
                    that.getStartPositionByGoPath(out startPosition, goPath.path[0]);
                    // that.getStartPositionByGoPath(out startPosition, goPath);
                }
                else
                {
                    throw new Exception($"未知情况！{Newtonsoft.Json.JsonConvert.SerializeObject(car)}");
                }
                car.setState(player, ref notifyMsg, CarState.working);
                //car.state = CarState.roadForCollect;
                //  var position = new Model.MapGo.nyrqPosition(fp1.RoadCode, fp1.RoadOrder, fp1.RoadPercent, fp1.positionLongitudeOnRoad, fp1.positionLatitudeOnRoad, Program.dt.GetItemRoadInfo(fp1.RoadCode, fp1.RoadOrder).MaxSpeed);
                grp.GetAFromBPoint(goPath.path[0].path, goPath.path[0].position, speed, ref result, ref startT_FirstPath, player.improvementRecord.speedValue > 0, that);
                //  result.RemoveAll(item => item.t == 0);
                var animation = new AnimateDataItem(startPosition, result, false, startT_FirstPath, goPath.path.Count > 0 ? privateKeys[0] : 255, ref that.rm);
                animations.Add(animation);
            }
            for (int i = 1; i < goPath.path.Count; i++)
            {
                //if (i == goPath.path.Count - 1) 
                //{

                //}
                //else
                {
                    var indexValue = i;
                    var speed = car.ability.Speed;
                    int startT_PathLast = 0;
                    List<int> result;
                    Data.PathStartPoint3 startPosition;
                    {
                        result = new List<int>();
                        that.getStartPositionByGoPath(out startPosition, goPath.path[indexValue]);
                    }
                    grp.GetAFromBPoint(goPath.path[indexValue].path, goPath.path[indexValue].path[0], speed, ref result, ref startT_PathLast, player.improvementRecord.speedValue > 0, that);
                    var animation = new AnimateDataItem(startPosition, result, false, startT_PathLast, privateKeys[indexValue], ref that.rm);
                    animations.Add(animation);
                }
            }
            car.setAnimateData(player, ref notifyMsg, animations, DateTime.Now);
        }

        //public void EditCarStateWhenActionStartOK_Last(RoleInGame player, ref Car car, int to, Model.FastonPosition fp1, Node goPath, ref List<string> notifyMsg, out int startT)
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
        //        result = that.getStartPositon(fp1, player.positionInStation, ref startT, out startPosition, player.improvementRecord.speedValue > 0);

        //    }
        //    else if (car.state == CarState.waitOnRoad)
        //    {
        //        result = new List<int>();
        //        that.getStartPositionByGoPath(out startPosition, goPath.path[0]);
        //        // that.getStartPositionByGoPath(out startPosition, goPath);
        //    }
        //    else
        //    {
        //        throw new Exception($"未知情况！{Newtonsoft.Json.JsonConvert.SerializeObject(car)}");
        //    }
        //    car.setState(player, ref notifyMsg, CarState.working);
        //    //car.state = CarState.roadForCollect;
        //    //  var position = new Model.MapGo.nyrqPosition(fp1.RoadCode, fp1.RoadOrder, fp1.RoadPercent, fp1.positionLongitudeOnRoad, fp1.positionLatitudeOnRoad, Program.dt.GetItemRoadInfo(fp1.RoadCode, fp1.RoadOrder).MaxSpeed);
        //    Program.dt.GetAFromBPoint(goPath.path[0].path, goPath.path[0].position, speed, ref result, ref startT, player.improvementRecord.speedValue > 0);
        //    //  result.RemoveAll(item => item.t == 0);

        //    car.setAnimateData(player, ref notifyMsg, new AnimateData2(startPosition, result, DateTime.Now, false));

        //}

        /// <summary>
        /// 这个方法没有targetPlayer，所以不进入station
        /// </summary>
        /// <param name="indexValue"></param>
        /// <param name="player"></param>
        /// <param name="car"></param>
        /// <param name="goPath"></param>
        /// <param name="notifyMsg"></param>
        /// <param name="startT"></param>
        public void EditCarStateAfterSelect(int indexValue, RoleInGame player, ref Car car, ref List<string> notifyMsg, out int startT)
        {
            car.animateObj.LengthOfPrivateKeys = indexValue;
            car.setAnimateData(player, ref notifyMsg);
            startT = car.animateObj.animateDataItems[indexValue].startT;
        }
        ///// <summary>
        ///// 这个方法进入targetPlayer 的station
        ///// </summary>
        ///// <param name="indexValue"></param>
        ///// <param name="player"></param>
        ///// <param name="car"></param>
        ///// <param name="goPath"></param>
        ///// <param name="targetPlayer"></param>
        ///// <param name="targetPosition"></param>
        ///// <param name="notifyMsg"></param>
        ///// <param name="startT"></param>
        //public void EditCarStateAfterSelect(int indexValue, RoleInGame player, ref Car car, Node goPath, RoleInGame targetPlayer, int targetPosition, ref List<string> notifyMsg, out int startT)
        //{
        //    //var speed = car.ability.Speed;
        //    //startT = 0;
        //    //List<int> result;
        //    //Data.PathStartPoint2 startPosition;
        //    //{
        //    //    result = new List<int>();
        //    //    that.getStartPositionByGoPath(out startPosition, goPath.path[indexValue]);
        //    //}

        //    //Program.dt.GetAFromBPoint(goPath.path[indexValue].path, goPath.path[indexValue].position, speed, ref result, ref startT, player.improvementRecord.speedValue > 0);
        //    ////  result.RemoveAll(item => item.t == 0);
        //    //that.getEndPositon(Program.dt.GetFpByIndex(targetPlayer.StartFPIndex), targetPosition, ref result, ref startT, player.improvementRecord.speedValue > 0);
        //    //car.setAnimateData(player, ref notifyMsg, new AnimateData2(startPosition, result, DateTime.Now, false));
        //}

        protected void carParkOnRoad(int target, ref Car car, RoleInGame player, ref List<string> notifyMsgs)
        {
            List<AnimateDataItem> animations = new List<AnimateDataItem>();
            var fp = Program.dt.GetFpByIndex(target);
            double endX, endY, endZ;
            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(fp.positionLongitudeOnRoad, fp.positionLatitudeOnRoad, fp.Height, out endX, out endY, out endZ);
            //var privateKeys = BitCoin.GamePathEncryption.PathEncryption.MainC.GetPrivateKeys(ref that.rm, 1);


            var animate = new AnimateDataItem(
                new Data.PathStartPoint3()
                {
                    x = Convert.ToInt32(endX * 256),
                    y = Convert.ToInt32(endY * 256),
                    z = Convert.ToInt32(endZ * 256)
                },
                new List<int>() { 0, 0, 20000 },
                true,
                20000, 255, ref that.rm
                );
            animations.Add(animate);
            car.setAnimateData(player, ref notifyMsgs, animations, DateTime.Now);
        }

        protected void carDoActionFailedThenMustReturn(Car car, RoleInGame player, GetRandomPos grp, ref List<string> notifyMsg)
        {

            if (car.state == CarState.waitOnRoad)
            {
                var from = getFromWhenAction(player, car);
                int startT = 1;
                //var carKey = $"{}_{}";
                var returnPath_Record = player.returningOjb;
                car.setState(player, ref notifyMsg, CarState.returning);
                that.retutnE.SetReturnT(startT, new commandWithTime.returnning()
                {
                    c = "returnning",
                    changeType = commandWithTime.returnning.ChangeType.BeforeTax,
                    key = player.Key,
                    returningOjb = returnPath_Record,
                    target = from
                }, grp);
            }
        }

        protected int getFromWhenAction(RoleInGame role, Car car)
        {
            switch (car.state)
            {
                case CarState.waitAtBaseStation:
                    {
                        return role.StartFPIndex;
                    };
                case CarState.waitOnRoad:
                    {
                        //小车的上一个的目标
                        if (car.targetFpIndex == -1)
                        {
                            throw new Exception("参数混乱");
                        }
                        else
                        {
                            return car.targetFpIndex;
                        }
                    };
                default:
                    {
                        throw new Exception($"错误的汽车状态:{car.state.ToString()}");
                    }
            }
        }

        /// <summary>
        /// 进行方向选择
        /// </summary>
        /// <param name="selections"></param>
        /// <param name="selectionCenter"></param>
        /// <param name="player"></param>
        protected bool StartSelectThreadA(List<Node.direction> selections, Node.pathItem.Postion selectionCenter, Player player, Action selectionIsRight, Node navigationData)
        {
            selections.RemoveAll(item => that.isZero(item));
            //  int k = 0;
            var oldState = player.getCar().state;
            //  bool bgHasSet = false;
            // while (true)
            {

                if (isRight(selections, player.direciton, true))
                {
                    selectionIsRight();
                    return false;
                }
                else
                {
                    if (player.getCar().state != CarState.selecting)
                    {
                        // if (!bgHasSet) { }
                        Action showCross = () =>
                        {
                            List<string> notifyMsg = new List<string>();
                            for (int i = 0; i < selections.Count; i++)
                            {
                                player.addUsedRoad(selections[i].start.roadCode, ref notifyMsg);
                            }
                            player.getCar().setState(player, ref notifyMsg, CarState.selecting);
                            that.showDirecitonAndSelection(player, selections, selectionCenter, ref notifyMsg);

                            //selectionCenter.
                            if (string.IsNullOrEmpty(selectionCenter.crossKey))
                            {
                                try
                                {
#warning 记录  

                                }
                                catch { }
                            }
                            else
                            {
                                if (Program.dt.AllCrossesBGData.ContainsKey(selectionCenter.postionCrossKey))
                                {
                                    player.SendBG(player, ref notifyMsg, selectionCenter.postionCrossKey, Program.dt.AllCrossesBGData[selectionCenter.postionCrossKey]);
                                }
                                else
                                {
                                    if (Program.dt.CrossesNotHaveBGData.ContainsKey(selectionCenter.postionCrossKey))
                                    {
                                        Program.dt.CrossesNotHaveBGData[selectionCenter.postionCrossKey] += 1;
                                    }
                                    else
                                    {
                                        Program.dt.CrossesNotHaveBGData.Add(selectionCenter.postionCrossKey, 1);
                                    }
                                    if (Program.dt.CrossesNotHaveBGData[selectionCenter.postionCrossKey] % 3 == 0)
                                    {
                                        DalOfAddress.backgroundneedjpg.Insert(selectionCenter.postionCrossKey, string.IsNullOrEmpty(selectionCenter.crossKey) ? "" : selectionCenter.crossKey);
                                    }
                                }
                            }
                            this.sendMsg(notifyMsg);
                        };
                        showCross();
                        player.ShowCrossAfterWebUpdate = showCross;
                        while (player.playerSelectDirectionTh != null && player.playerSelectDirectionTh.IsAlive)
                        {
                            this.ThreadSleep(40);
                        }
                        player.playerSelectDirectionTh = new Thread(() => StartSelectThreadB(selections, selectionCenter, player, oldState, selectionIsRight));
                        player.NavigationData = navigationData;
                    }
                    return true;
                }
            }

        }



        protected void StartSelectThreadB(List<Node.direction> selections, Node.pathItem.Postion selectionCenter, Player player, CarState oldState, Action p)
        {
            if (isRight(selections, player.direciton, false) || player.Bust)
            {
                List<string> notifyMsg = new List<string>();
                if (player.getCar().state == CarState.selecting)
                {
                    // List<string> notifyMsg = new List<string>();
                    player.getCar().setState(player, ref notifyMsg, oldState);
                    player.SendBG(player, ref notifyMsg);
                }
                this.sendMsg(notifyMsg);
                p();
            }
            else
            {
                List<string> notifyMsg = new List<string>();
                var reduceValue = player.getCar().ability.ReduceBusinessAndVolume(player, player.getCar(), ref notifyMsg);
                reduceValue = Math.Max(0, reduceValue);
                SelectionIsWrong(player, reduceValue, notifyMsg);
                this.sendMsg(notifyMsg);
                player.playerSelectDirectionTh = new Thread(() => StartSelectThreadB(selections, selectionCenter, player, oldState, p));
            }

        }

        private void SelectionIsWrong(Player player, long reduceValue, List<string> notifyMsg)
        {
            var obj = new SelectionIsWrong
            {
                c = "SelectionIsWrong",
                WebSocketID = player.WebSocketID,
                reduceValue = reduceValue
            };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            notifyMsg.Add(player.FromUrl);
            notifyMsg.Add(json);
            this.WebNotify(player, $"错误的选择让您损失了{reduceValue / 100}.{(reduceValue % 100) / 10}{(reduceValue % 10)}。");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="speed"></param>
        /// <param name="player"></param>
        /// <param name="animations"></param>
        protected void EndWithRightPosition(Node node, int speed, RoleInGame player, RoleInGame targetPlayer, ref List<AnimateDataItem> animations, List<long> privateKeys)
        {
            if (node.path.Count > 0)
            {
                var indexValue = node.path.Count - 1;
                // var speed = car.ability.Speed;
                int startT_PathLast = 0;
                List<int> result;
                Data.PathStartPoint3 startPosition;
                {
                    result = new List<int>();
                    that.getStartPositionByGoPath(out startPosition, node.path[indexValue]);
                }
                Program.dt.GetAFromBPoint(node.path[indexValue].path, node.path[indexValue].path[0], speed, ref result, ref startT_PathLast, player.improvementRecord.speedValue > 0, that);
                {
                    int positionInStation;
                    if (player.Key == targetPlayer.Key)
                    {
                        positionInStation = targetPlayer.positionInStation;
                        // that.getEndPositon(Program.dt.GetFpByIndex(targetPlayer.StartFPIndex), positionInStation, ref result, ref startT_PathLast, player.improvementRecord.speedValue > 0);
                    }
                    else
                    {
                        positionInStation = (targetPlayer.positionInStation + 2) % 5;
                        //   that.getEndPositon(Program.dt.GetFpByIndex(player.StartFPIndex), positionInStation, ref result, ref startT_PathLast, player.improvementRecord.speedValue > 0);
                    }
                    that.getEndPositon(Program.dt.GetFpByIndex(targetPlayer.StartFPIndex), positionInStation, ref result, ref startT_PathLast, player.improvementRecord.speedValue > 0);
                }
                var animation = new AnimateDataItem(startPosition, result, false, startT_PathLast, privateKeys[indexValue], ref that.rm);
                animations.Add(animation);
            }
        }
        private bool isRight(List<Node.direction> selections, System.Numerics.Complex c2, bool firstCheck)
        {
            if (firstCheck)
            {
                //if (that.rm.Next(100) < 20)
                //{
                //    return false;
                //}
                //else 
                if (selections.Count < 2)
                {
                    return true;
                }
                else if (selections.Count(item => item.right) == 0)
                {
                    return true;
                }
                var first = (from item in selections
                             orderby that.getAngle(that.getComplex(item) / c2) ascending
                             select item)
                       .ToList()[0];
                if (that.rm.Next(100) < 20)
                {
                    return false;
                }
                else
                    return first.right;
            }
            else
            {
                var rightItem = (from item in selections
                                 where item.right
                                 select item).ToList()[0];
                ////                var angle = that.getAngle(that.getComplex(rightItem) / c2);
                ////#warning 这里要调试完 要删除
                ////                Console.WriteLine(angle);
                return that.getAngle(that.getComplex(rightItem) / c2) < 0.0005;
            }
        }

        public class notifyMsg
        {
            public notifyMsg()
            {
                this.notifyMsgs = new List<string>();
            }
            public List<string> notifyMsgs = new List<string>();
            public void send(Engine e)
            {
                for (var i = 0; i < notifyMsgs.Count; i += 2)
                {
                    var url = notifyMsgs[i];
                    var sendMsg = notifyMsgs[i + 1];
                    e.sendMsg(url, sendMsg);
                }
            }
        }

        protected void loop(Action p, int step, int startT, RoleInGame player, Node goPath)
        {
            if (step == 0)
            {
                this.ThreadSleep(startT + 50);

                if (player.playerType == RoleInGame.PlayerType.NPC || player.Bust)
                {
                    p();
                }
                else
                {
                    StartSelectThreadA(goPath.path[step].selections, goPath.path[step].selectionCenter, (Player)player, p, goPath);
                }
            }
            else
            {
                this.ThreadSleep(Math.Max(5, startT));
                if (player.playerType == RoleInGame.PlayerType.NPC || player.Bust)
                {
                    this.ThreadSleep(500);
                    p();
                }
                else
                {
                    StartSelectThreadA(goPath.path[step].selections, goPath.path[step].selectionCenter, (Player)player, p, goPath);
                }
            }
        }
    }
}
