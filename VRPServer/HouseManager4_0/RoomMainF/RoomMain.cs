﻿using CommonClass;
using Model;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using static HouseManager4_0.Car;

namespace HouseManager4_0.RoomMainF
{
    public partial class RoomMain : RoomMainBaseData, interfaceOfHM.ListenInterface
    {



        public RoomMain()
        {
            this.PlayerLock = new object();
            this._collectPosition = new Dictionary<int, int>();
            this.rm = new System.Random(DateTime.Now.GetHashCode());
            this.Market = new Market(this.priceChanged);
            this.Music = new Music();
            this.bg = new BackGround();
            this.attackE = new Engine_AttackEngine(this);
            this.debtE = new Engine_DebtEngine(this);
            this.retutnE = new Engine_Return(this);
            this.taxE = new Engine_Tax(this);
            this.collectE = new Engine_CollectEngine(this);
            this.promoteE = new Engine_PromoteEngine(this);
            this.diamondOwnerE = new Engine_DiamondOwnerEngine(this);
            this.attachE = new Engine_Attach(this);
            this.magicE = new Engine_MagicEngine(this);
            this.checkE = new Engine_Check(this);
            //  this.npcc = new NPCControle();

            this.NPCM = new Manager_NPC(this);
            this.frequencyM = new Manager_Frequency(this);
            this.driverM = new Manager_Driver(this);
            this.goodsM = new Manager_GoodsReward(this, this.DrawGoodsSelection);
            this.modelM = new Manager_Model(this);
            this.modelR = new Manager_Resistance(this);
            this.modelC = new Manager_Connection(this);

            lock (PlayerLock)
            {
                this._Players = new Dictionary<string, RoleInGame>();
                //    this._FpOwner = new Dictionary<int, string>();
                //this._PlayerFp = new Dictionary<string, int>();
            }
            LookFor();
            this.recordOfPromote = new Dictionary<string, List<DateTime>>()
            {
                {  "mile" ,new List<DateTime>()},
                {  "business" ,new List<DateTime>() },
                {  "volume" ,new List<DateTime>() },
                {  "speed" ,new List<DateTime>() },
            };
            this.promotePrice = new Dictionary<string, long>()
            {
                {  "mile" ,10 * 100},
                {  "business" ,10 * 100 },
                {  "volume" ,10 * 100 },
                {  "speed" ,10 * 100},
            };
        }



        public string CheckCarStateF(CheckCarState ccs)
        {
            return this.checkE.CheckCarStateF(ccs);
        }

        public string Statictis(ServerStatictis ss)
        {
            var r = new List<int>(4) { 0, 0, 0, 0 };
            foreach (var item in this._Players)
            {
                r[0]++;
                if (item.Value.playerType == RoleInGame.PlayerType.player)
                {
                    r[1]++;
                    if (((Player)item.Value).IsOnline())
                    {
                        r[3]++;
                    }
                }
                else
                {
                    r[2]++;
                }
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(r);
            //  throw new NotImplementedException();
        }

        public void SystemBradcast(SystemBradcast sb)
        {
            foreach (var item in this._Players)
            {
                if (item.Value.playerType == RoleInGame.PlayerType.player)
                {
                    if (!item.Value.Bust)
                    {
                        WebNotify(item.Value, sb.msg);
                    }
                }
            }
            // throw new NotImplementedException();
        }

        public string updateView(View v)
        {
            lock (this.PlayerLock)
            {
                if (this._Players.ContainsKey(v.Key))
                {
                    var player = this._Players[v.Key];
                    if (player.playerType == RoleInGame.PlayerType.player)
                    {
                        ((Player)player).direciton = getComplex(v, ((Player)player).direciton);
                        if (((Player)player).getCar().state == CarState.selecting)
                        {
                            if (((Player)player).playerSelectDirectionTh != null)
                            {
                                if (!((Player)player).playerSelectDirectionTh.IsAlive)
                                {
                                    if (((Player)player).playerSelectDirectionTh.ThreadState == System.Threading.ThreadState.Unstarted)
                                    {
                                        ((Player)player).playerSelectDirectionTh.Start();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return "";
            //   throw new NotImplementedException();
        }



        public class commandWithTime
        {
            abstract public class baseC
            {
                public string c { get; set; }
                public string key { get; set; }
                //public string car { get; set; }
            }
            public class ReturningOjb
            {
                internal Node returnToBossAddrPath { get; private set; }
                internal Node returnToSelfAddrPath { get; private set; }
                public bool NeedToReturnBoss { get; private set; }
                public RoleInGame Boss { get; private set; }

                public static ReturningOjb ojbWithBoss(
                    Node returnToBossAddrPath,
                   Node returnToSelfAddrPath,
                     RoleInGame Boss
                    )
                {
                    return new ReturningOjb()
                    {
                        Boss = Boss,
                        NeedToReturnBoss = true,
                        returnToBossAddrPath = returnToBossAddrPath,
                        returnToSelfAddrPath = returnToSelfAddrPath
                    };
                }
                internal static ReturningOjb ojbWithoutBoss(Node returnToSelfAddrPath)
                {
                    return new ReturningOjb()
                    {
                        Boss = null,
                        NeedToReturnBoss = false,
                        returnToSelfAddrPath = returnToSelfAddrPath,
                        returnToBossAddrPath = null
                    };
                }
                //public static ReturningOjb ojbWithoutBoss(List<Model.MapGo.nyrqPosition> returnToSelfAddrPath)
                //{
                //    return new ReturningOjb()
                //    {
                //        Boss = null,
                //        NeedToReturnBoss = false,
                //        returnToSelfAddrPath = returnToSelfAddrPath,
                //        returnToBossAddrPath = null
                //    };
                //}
                public static ReturningOjb ojbWithoutBoss(ReturningOjb oldObj)
                {
                    return new ReturningOjb()
                    {
                        Boss = null,
                        NeedToReturnBoss = false,
                        returnToSelfAddrPath = oldObj.returnToSelfAddrPath,
                        returnToBossAddrPath = null
                    };
                }


            }
            public class returnning : baseC
            {

                public ReturningOjb returningOjb { get; set; }
                /// <summary>
                /// 返回路程的起点
                /// </summary>
                internal int target { get; set; }

                /// <summary>
                /// 取值如mile
                /// </summary>
                public ChangeType changeType { get; internal set; }


                /// <summary>
                /// 表征是否在税收方法执行之后！
                /// </summary>
                public enum ChangeType
                {
                    /// <summary>
                    /// 在收取税收之后
                    /// </summary>
                    AfterTax,
                    BeforeTax,
                }
            }

            //public class returnningWithBoss:

            public class diamondOwner : returnning
            {
                public int costMile { get; internal set; }
                public string diamondType { get; set; }

            }

            public class debtOwner : returnning
            {
                //  public int costMile { get; internal set; }
                public string victim { get; internal set; }
                public int costMile { get; internal set; }
            }
            public class speedSet : returnning
            {
                //  public int costMile { get; internal set; }
                public string beneficiary { get; internal set; }
                public int costMile { get; internal set; }
            }
            public class attackSet : returnning
            {
                //  public int costMile { get; internal set; }
                public string beneficiary { get; internal set; }
                public int costMile { get; internal set; }
            }
            public class defenseSet : returnning
            {
                //  public int costMile { get; internal set; }
                public string beneficiary { get; internal set; }
                public int costMile { get; internal set; }
            }
            public class bustSet : returnning
            {
                //  public int costMile { get; internal set; }
                public string victim { get; internal set; }
            }

            public class taxSet : returnning { }

            public class placeArriving : baseC
            {
                public int costMile { get; set; }
                internal int target { get; set; }

                public ReturningOjb returningOjb { get; set; }
                //internal int target { get; set; }

                ///// <summary>
                ///// 取值如mile
                ///// </summary>
                //public string changeType { get; internal set; }
            }

            public class comeBack : baseC
            {

            }
        }

        ///// <summary>
        ///// 当没有抢到宝石-或者收集、保护费，在路上待命。
        ///// </summary>
        ///// <param name="target"></param>
        ///// <param name="car"></param>
        //private void carParkOnRoad(int target, ref Car car, RoleInGame player, ref List<string> notifyMsgs)
        //{
        //    var fp = Program.dt.GetFpByIndex(target);
        //    double endX, endY;
        //    CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(fp.positionLongitudeOnRoad, fp.positionLatitudeOnRoad, out endX, out endY);


        //    var animate = new AnimateData2()
        //    {
        //        start = new Data.PathStartPoint2()
        //        {
        //            x = Convert.ToInt32(endX * 256),
        //            y = Convert.ToInt32(endY * 256)
        //        },
        //        animateData = new List<int>()
        //        {
        //            0,0,20000
        //        },
        //        recordTime = DateTime.Now,
        //        isParking = true
        //    };
        //    car.setAnimateData(player, ref notifyMsgs, animate);
        //    //car.animateData = new AnimateData()
        //    //{
        //    //    animateData = new List<Data.PathResult>()
        //    //            {
        //    //                  new Data.PathResult()
        //    //                  {
        //    //                      t0=0,
        //    //                      x0=endX,
        //    //                      y0=endY,
        //    //                      t1=200000,
        //    //                      x1=endX,
        //    //                      y1=endY
        //    //                  }
        //    //            },
        //    //    recordTime = DateTime.Now
        //    //};

        //    //if (this.debug)
        //    {
        //        //var goPath = Program.dt.GetAFromB(car.targetFpIndex, this.collectPosition);
        //        //var returnPath = Program.dt.GetAFromB(this.collectPosition, player.StartFPIndex);

        //        //var goMile = GetMile(goPath);
        //        //var returnMile = GetMile(returnPath);
        //        //if (goMile + returnMile > car.ability.leftMile) 
        //        //{
        //        //    for (int i = 0; i < 3; i++) 
        //        //    {
        //        //        //Consol.WriteLine($"现在回收是要返回的！");
        //        //    }
        //        //}
        //    }
        //}

    }


}
