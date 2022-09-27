using CommonClass;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using static HouseManager4_0.Car;

namespace HouseManager4_0.RoomMainF
{
    public partial class RoomMain : interfaceOfHM.Attack
    {
        const string AttackFailedReturn = "attack-failed-return";
        public string updateAttack(SetAttack sa, GetRandomPos grp)
        {
            return this.attackE.updateAttack(sa, grp);
        }




        /// <summary>
        /// 此函数，必须在this._Players.ContainsKey(sa.targetOwner)=true且this._Players[sa.targetOwner].Bust=false情况下运行。请提前进行判断！
        /// </summary>
        /// <param name="player">玩家</param>
        /// <param name="car"></param>
        /// <param name="sa"></param>
        /// <param name="notifyMsg"></param>
        /// <param name="victimState"></param>
        /// <param name="reason"></param>
        //        void attack(RoleInGame player, Car car, SetAttack sa, ref List<string> notifyMsg, out MileResultReason Mrr)
        //        {
        //            RoleInGame boss;
        //            if (player.HasTheBoss(this._Players, out boss))
        //            {
        //                //    attackPassBossAddress(player, boss, car, sa, ref notifyMsg, out Mrr);
        //                //return promotePassBossAddress(player, boss, car, sp, ref notifyMsg, out reason);
        //            }
        //            else
        //            {
        //                if (car.ability.leftBusiness > 0)
        //                {
        //                    var from = this.getFromWhenAttack(player, car);
        //                    var to = sa.target;
        //                    var fp1 = Program.dt.GetFpByIndex(from);
        //                    var fp2 = Program.dt.GetFpByIndex(to);
        //                    var baseFp = Program.dt.GetFpByIndex(player.StartFPIndex);

        //                    // var goPath = Program.dt.GetAFromB(fp1, fp2.FastenPositionID);

        //                    //var goPath = Program.dt.GetAFromB(from, to);
        //                    var goPath = this.GetAFromB(from, to, player, ref notifyMsg);
        //                    //var returnPath = Program.dt.GetAFromB(fp2, baseFp.FastenPositionID);
        //                    //var returnPath = Program.dt.GetAFromB(to, player.StartFPIndex);
        //                    var returnPath = this.GetAFromB(to, player.StartFPIndex, player, ref notifyMsg);

        //                    var goMile = GetMile(goPath);
        //                    var returnMile = GetMile(returnPath);



        //                    //第一步，计算去程和回程。
        //                    if (car.ability.leftMile >= goMile + returnMile)
        //                    {
        //                        int startT;

        //                        EditCarStateWhenAttackStartOK(player, ref car, to, fp1, sa, goPath, out startT, ref notifyMsg);
        //                        SetAttackArrivalThread(startT, car, sa, returnPath, goMile);
        //                        // getAllCarInfomations(sa.Key, ref notifyMsg);
        //                        Mrr = MileResultReason.Abundant;
        //                    }

        //                    else if (car.ability.leftMile >= goMile)
        //                    {
        //                        //当攻击失败，必须返回
        //                        Console.Write($"去程{goMile}，回程{returnMile}");
        //                        Console.Write($"你去了回不来");
        //                        Mrr = MileResultReason.CanNotReturn;
        //                    }
        //                    else
        //                    {
        //#warning 这里要在web前台进行提示
        //                        //当攻击失败，必须返回
        //                        Console.Write($"去程{goMile}，回程{returnMile}");
        //                        Console.Write($"你去不了");
        //                        Mrr = MileResultReason.CanNotReach;
        //                    }
        //                }
        //                else
        //                {
        //                    Mrr = MileResultReason.MoneyIsNotEnougt;
        //                }
        //            }
        //        }







        //private void EditCarStateWhenAttackStartOK(RoleInGame player, ref Car car, int to, Model.FastonPosition fp1, SetAttack sa, List<Model.MapGo.nyrqPosition> goPath, out int startT, ref List<string> notifyMsg)
        //{
        //    car.targetFpIndex = to;//A.更改小车目标，在其他地方引用。

        //    // car.purpose = Purpose.attack;//B.更改小车目的，小车变为攻击状态！
        //    //  car.changeState++;//C.更改状态用去前台更新动画  

        //    /*
        //    * D.更新小车动画参数
        //    */
        //    var speed = car.ability.Speed;
        //    startT = 0;
        //    //List<Data.PathResult3> result;
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
        //        throw new Exception("错误的汽车类型！！！");
        //    }
        //    car.setState(this._Players[sa.Key], ref notifyMsg, CarState.working);
        //    //car.state = CarState.roadForAttack;
        //    //  this.SendStateAndPurpose(this._Players[sa.Key], car, ref notifyMsg);


        //    Program.dt.GetAFromBPoint(goPath, fp1, speed, ref result, ref startT);
        //    //  result.RemoveAll(item => item.t == 0);

        //    var animateData = new AnimateData2()
        //    {
        //        start = startPosition,
        //        animateData = result,
        //        recordTime = DateTime.Now
        //    };

        //    car.setAnimateData(player, ref notifyMsg, animateData);
        //}




    }
}
