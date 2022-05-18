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
        internal string updateAction(interfaceOfEngine.tryCatchAction actionDo, Command c, string operateKey)
        {
            string conditionNotReason;
            if (actionDo.conditionsOk(c, out conditionNotReason))
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
                                        if (actionDo.carAbilitConditionsOk(player, car, c))
                                        {
                                            MileResultReason mrr;
                                            RoomMainF.RoomMain.commandWithTime.ReturningOjb returningOjb = actionDo.maindDo(player, car, c, ref notifyMsg, out mrr);
                                            if (mrr == MileResultReason.Abundant)
                                            {
                                                player.returningOjb = returningOjb;
                                            }
                                            else if (mrr == MileResultReason.CanNotReach)
                                            {
                                                actionDo.failedThenDo(car, player, c, ref notifyMsg);
                                                this.WebNotify(player, "小车不能到达目的地，被安排返回！");
                                            }
                                            else if (mrr == MileResultReason.CanNotReturn)
                                            {
                                                actionDo.failedThenDo(car, player, c, ref notifyMsg);
                                                this.WebNotify(player, "小车到达目的地后不能返回，在当前地点安排返回！");
                                            }
                                            else if (mrr == MileResultReason.MoneyIsNotEnougt)
                                            {
                                                actionDo.failedThenDo(car, player, c, ref notifyMsg);
                                            }
                                            else if (mrr == MileResultReason.NearestIsMoneyWhenPromote)
                                            {

                                            }
                                            else if (mrr == MileResultReason.NearestIsMoneyWhenAttack)
                                            {
                                                // this.WebNotify(player, $"离宝石最近的是钱，不是你的车。请离宝石再近点儿！");
                                            }
                                        }
                                        else
                                        {
                                            actionDo.failedThenDo(car, player, c, ref notifyMsg);
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
                this.sendMsg(notifyMsg);
                //for (var i = 0; i < notifyMsg.Count; i += 2)
                //{
                //    var url = notifyMsg[i];
                //    var sendMsg = notifyMsg[i + 1];
                //    //Console.WriteLine($"url:{url}");
                //    if (!string.IsNullOrEmpty(url))
                //    {
                //        Startup.sendMsg(url, sendMsg);
                //    }
                //}
                return "";
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
        public void EditCarStateWhenActionStartOK(RoleInGame player, ref Car car, int to, Model.FastonPosition fp1, Node goPath, ref List<string> notifyMsg, out int startT_FirstPath)
        {
            car.targetFpIndex = to;//A.更改小车目标，在其他地方引用。

            //car.purpose = Purpose.collect;//B.更改小车目的，用户操作控制
            //    car.changeState++;//C.更改状态用去前台更新动画   
            /*
             * 步骤C已经封装进 car.setAnimateData
             */
            /*
             * D.更新小车动画参数
             */
            List<AnimateDataItem> animations = new List<AnimateDataItem>();
            {
                var speed = car.ability.Speed;
                startT_FirstPath = 0;
                List<int> result;
                Data.PathStartPoint2 startPosition;
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
                Program.dt.GetAFromBPoint(goPath.path[0].path, goPath.path[0].position, speed, ref result, ref startT_FirstPath, player.improvementRecord.speedValue > 0);
                //  result.RemoveAll(item => item.t == 0);
                var animation = new AnimateDataItem(startPosition, result, false, startT_FirstPath);
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
                    Data.PathStartPoint2 startPosition;
                    {
                        result = new List<int>();
                        that.getStartPositionByGoPath(out startPosition, goPath.path[indexValue]);
                    }
                    Program.dt.GetAFromBPoint(goPath.path[indexValue].path, goPath.path[indexValue].path[0], speed, ref result, ref startT_PathLast, player.improvementRecord.speedValue > 0);
                    var animation = new AnimateDataItem(startPosition, result, false, startT_PathLast);
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
            double endX, endY;
            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(fp.positionLongitudeOnRoad, fp.positionLatitudeOnRoad, out endX, out endY);


            var animate = new AnimateDataItem(
                new Data.PathStartPoint2() { x = Convert.ToInt32(endX * 256), y = Convert.ToInt32(endY * 256) },
                new List<int>() { 0, 0, 20000 },
                true,
                20000
                );
            animations.Add(animate);
            car.setAnimateData(player, ref notifyMsgs, animations, DateTime.Now);
        }

        protected void carDoActionFailedThenMustReturn(Car car, RoleInGame player, ref List<string> notifyMsg)
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
                });
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
        protected void StartSelectThread(  List<Node.direction> selections, Node.pathItem.Postion selectionCenter, Player player)
        {
            selections.RemoveAll(item => that.isZero(item));
            int k = 0;
            var oldState = player.getCar().state;
            //  bool bgHasSet = false;
            while (true)
            {

                if (isRight(selections, player.direciton))
                {  
                    break;
                }
                else
                {

                    if (player.getCar().state != CarState.selecting)
                    {
                        // if (!bgHasSet) { }
                        List<string> notifyMsg = new List<string>();
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
                            if (Program.dt.AllCrossesBGData.ContainsKey(selectionCenter.crossKey))
                            {
                                player.SendBG(player, ref notifyMsg, Program.dt.AllCrossesBGData[selectionCenter.crossKey], selectionCenter.crossKey);

                            }
                            else if (Program.dt.AllCrossesBGData.ContainsKey(selectionCenter.postionCrossKey))
                            {
                                player.SendBG(player, ref notifyMsg, Program.dt.AllCrossesBGData[selectionCenter.postionCrossKey], selectionCenter.postionCrossKey);
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
                                if (Program.dt.CrossesNotHaveBGData[selectionCenter.postionCrossKey] % 15 == 0)
                                {
                                    DalOfAddress.backgroundneedjpg.Insert(selectionCenter.postionCrossKey, string.IsNullOrEmpty(selectionCenter.crossKey) ? "" : selectionCenter.crossKey);
                                }
                            }
                        }
                        this.sendMsg(notifyMsg);
                    }
                    this.ThreadSleep(80);
                }
                //ThreadSleep(250);
                k++;
                //Console.WriteLine($"提示，让玩家进行选择！！！");
                if (k >= 250 * 3)
                {
                    break;
                }

            }
            if (player.getCar().state == CarState.selecting)
            {
                List<string> notifyMsg = new List<string>();
                player.getCar().setState(player, ref notifyMsg, oldState);
                player.SendBG(player, ref notifyMsg);

                this.sendMsg(notifyMsg);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="speed"></param>
        /// <param name="player"></param>
        /// <param name="animations"></param>
        protected void EndWithRightPosition(Node node, int speed, RoleInGame player, RoleInGame targetPlayer, ref List<AnimateDataItem> animations)
        {
            if (node.path.Count > 0)
            {
                var indexValue = node.path.Count - 1;
                // var speed = car.ability.Speed;
                int startT_PathLast = 0;
                List<int> result;
                Data.PathStartPoint2 startPosition;
                {
                    result = new List<int>();
                    that.getStartPositionByGoPath(out startPosition, node.path[indexValue]);
                }
                Program.dt.GetAFromBPoint(node.path[indexValue].path, node.path[indexValue].path[0], speed, ref result, ref startT_PathLast, player.improvementRecord.speedValue > 0);
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
                var animation = new AnimateDataItem(startPosition, result, false, startT_PathLast);
                animations.Add(animation);
            }
        }
        private bool isRight(List<Node.direction> selections, System.Numerics.Complex c2)
        {
            if (selections.Count == 0)
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
            return first.right;
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
    }
}
