using HouseManager4_0.interfaceOfHM;
using HouseManager4_0.RoomMainF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using static HouseManager4_0.RoomMainF.RoomMain;

namespace HouseManager4_0
{
    public class Manager_NPC : Manager, interfaceOfEngine.startNewCommandThread
    {
        List<string> NPCNames = new List<string>()
        {
            "和珅",
            "赵高",
            "李斯",
            "杨国忠",
            "蔡京",
            "秦桧",
            "严嵩",
            "魏忠贤",
            "高俅",
            "安禄山"
        };

        internal void ControlNPC(GetRandomPos grp)
        {
            var notifyMsgs = new List<string>();
            // that.ControlNPC();
            List<string> keys = new List<string>();
            foreach (var item in that._Players)
            {
                var role = item.Value;
                if (role.playerType == RoleInGame.PlayerType.NPC)
                {
                    if (role.getCar().state == Car.CarState.waitOnRoad)
                    {
                        if (role.getCar().ability.driver != null)
                        {
                            var npc = (NPC)role;
                            if (npc.getCar().ability.driver.race == CommonClass.driversource.Race.people)
                                keys.Add(item.Key);
                        }

                    }
                    // 
                }
            }
            for (int i = 0; i < keys.Count; i++)
            {
                var npc = (NPC)that._Players[keys[i]];
                this.ControlNPCItem(npc, grp, ref notifyMsgs);
                //   ControlNPC(npc);
            }
            this.sendSeveralMsgs(notifyMsgs);
            ///throw new NotImplementedException();
        }

        private void ControlNPCItem(NPC npc, GetRandomPos grp, ref List<string> notifyMsgs)
        {
            if (npc.attackTag.aType == NPC.AttackTag.AttackType.ambush)
            {
                var imortalPartners = that.GetAllParner(CommonClass.driversource.Race.immortal, npc);
                if (imortalPartners.Count(item => !item.confuseRecord.IsBeingControlled()) > 0)
                {
                    /*
                     * 有输出系，继续等待！
                     */
                }
                else
                {
                    /*
                     * 没有输出系，攻击！
                     */
                    NPC.AttackTag tag = new NPC.AttackTag()
                    {
                        aType = NPC.AttackTag.AttackType.attack,
                        HarmValue = 1,
                        fpPass = npc.attackTag.fpPass,
                        Target = npc.attackTag.Target
                    };
                    npc.attackTag = tag;//初始化自动包。
                    this.counterAttack(npc, ref notifyMsgs, Program.dt);


                }
            }
            else if (
                npc.attackTag.aType == NPC.AttackTag.AttackType.lose ||
                npc.attackTag.aType == NPC.AttackTag.AttackType.confuse)
            {
                if (that._Players.ContainsKey(npc.attackTag.Target))
                {
                    if (that._Players[npc.attackTag.Target].confuseRecord.IsBeingControlled())
                    {
                        NPC.AttackTag tag = new NPC.AttackTag()
                        {
                            aType = NPC.AttackTag.AttackType.attack,
                            HarmValue = 1,
                            fpPass = npc.attackTag.fpPass,
                            Target = npc.attackTag.Target
                        };
                        npc.attackTag = tag;//初始化自动包。
                        this.counterAttack(npc, ref notifyMsgs, Program.dt);
                    }
                    else
                    {
                        /*
                         * 继续等待！！！
                         */
                    }
                }
                else
                {
                    returnF(npc.Key, grp);
                }
            }
        }
        void returnF(string key, GetRandomPos grp)
        {
            CommonClass.OrderToReturn otr = new CommonClass.OrderToReturn()
            {
                c = "OrderToReturn",
                Key = key
            };
            this.startNewCommandThread(200, otr, this, grp);
        }
        internal void ClearNPC()
        {
            /*
             * 获取玩家的最大等级A
             * 找到所有NPC中级别大于A+1的NPC
             * 将找到的NPC删除
             * 同时删除没有敌人的NPC,拥有挑战者记录有，但无敌人的NPC
             */

            lock (that.PlayerLock)
            {
                {
                    int maxLevel = -1;
                    foreach (var item in that._Players)
                    {
                        if (item.Value.playerType == RoleInGame.PlayerType.player)
                        {
                            var level = getLevelOfRole((Player)item.Value);//
                            if (maxLevel <= level)
                            {
                                maxLevel = level;
                            }
                        }
                    }
                    List<string> keysOfNPCs = new List<string>();
                    foreach (var item in that._Players)
                    {
                        if (item.Value.playerType == RoleInGame.PlayerType.NPC)
                        {
                            if (item.Value.Level > maxLevel + 1)
                            {
                                keysOfNPCs.Add(item.Value.Key);
                            }
                        }
                    }
                    List<string> notifyMsgs = new List<string>();
                    for (int i = 0; i < keysOfNPCs.Count; i++)
                    {
                        if (!that._Players[keysOfNPCs[i]].Bust)
                            that._Players[keysOfNPCs[i]].SetBust(true, ref notifyMsgs);
                    }
                    this.sendSeveralMsgs(notifyMsgs);
                }
                {
                    List<string> keysOfNPCs = new List<string>();
                    foreach (var item in that._Players)
                    {
                        if (item.Value.playerType == RoleInGame.PlayerType.NPC)
                        {
                            var npc = (NPC)item.Value;
                            if (!string.IsNullOrEmpty(npc.challenger))
                            {
                                if (!that.HasEnemy(npc))
                                {
                                    keysOfNPCs.Add(item.Value.Key);
                                }
                            }
                        }
                    }
                    List<string> notifyMsgs = new List<string>();
                    for (int i = 0; i < keysOfNPCs.Count; i++)
                    {
                        if (!that._Players[keysOfNPCs[i]].Bust)
                            that._Players[keysOfNPCs[i]].SetBust(true, ref notifyMsgs);
                    }
                    this.sendSeveralMsgs(notifyMsgs);
                }
            }
        }

        public Manager_NPC(RoomMainF.RoomMain roomMain)
        {
            this.roomMain = roomMain;
        }
        public void AddNPC(interfaceOfHM.Car cf, GetRandomPos gp)
        {
            //int level, interfaceOfHM.Car cf, GetRandomPos gp
            lock (that.PlayerLock)
            {
                /*
                 * 1.遍历player的级别+1
                 * 2.遍历NPC的级别
                 * 3.选择步骤1的数据，提出步骤2的数据
                 * 4.新增NPC。
                 */
                List<int> levels = new List<int>();
                foreach (var item in that._Players)
                {
                    if (item.Value.playerType == RoleInGame.PlayerType.player)
                    {
                        var levelOfRole = getLevelOfRole((Player)item.Value) + 1;
                        for (int level = 2; level <= levelOfRole; level++)
                        {
                            if (levels.Contains(level)) { }
                            else
                            {
                                levels.Add(level);
                            }
                        }
                    }
                }
                foreach (var item in that._Players)
                {
                    if (item.Value.playerType == RoleInGame.PlayerType.NPC)
                    {
                        var level = item.Value.Level;
                        levels.RemoveAll(item => item == level);
                    }
                }
                if (levels.Count > 0)
                {
                    for (var i = 0; i < levels.Count; i++)
                    {
                        var key = AddNpcPlayer(levels[i], cf, gp);
                        GetNPCPosition(key);
                    }
                }
            }
        }

        /// <summary>
        /// 获取玩家的等级。如1,2,3,4,……
        /// </summary>
        /// <param name="playerValue"></param>
        /// <returns></returns>
        private int getLevelOfRole(Player playerValue)
        {
            if (playerValue.TheLargestHolderKey == playerValue.Key)
            {
                return playerValue.Level;
            }
            else if (that._Players.ContainsKey(playerValue.TheLargestHolderKey))
            {
                if (that._Players[playerValue.TheLargestHolderKey].Bust)
                {
                    return playerValue.Level;
                }
                else
                {
                    return that._Players[playerValue.TheLargestHolderKey].Level;
                }
            }
            else
            {
                return playerValue.Level;
            }
        }

        const string AddSuffix = "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public string AddNpcPlayer(int level, interfaceOfHM.Car cf, GetRandomPos gp)
        {

            string key = CommonClass.Random.GetMD5HashFromStr("ss" + DateTime.Now.ToString());
            while (that._Players.ContainsKey(key))
            {
                key = CommonClass.Random.GetMD5HashFromStr("ss" + DateTime.Now.ToString());
                Thread.Sleep(1);
            }
            //bool success;

            List<string> carsState = new List<string>();
            lock (that.PlayerLock)
            {

                {
                    //  success = true;

                    bool hasTheSameName = false;
                    var NPCName = $"{this.NPCNames[that.rm.Next(this.NPCNames.Count)]}-{level}级";
                    do
                    {

                        hasTheSameName = false;
                        foreach (var item in that._Players)
                        {
                            if (item.Value.PlayerName == NPCName)
                            {
                                hasTheSameName = true;
                                break;
                            }
                        }
                        if (hasTheSameName)
                        {
                            NPCName += AddSuffix[that.rm.Next(0, AddSuffix.Length)];
                        }

                    } while (hasTheSameName);

                    var npc = new NPC()
                    {
                        rm = that,
                        Key = key,
                        PlayerName = NPCName,

                        CreateTime = DateTime.Now,
                        ActiveTime = DateTime.Now,
                        StartFPIndex = -1,
                        //Collect = -1,
                        CollectPosition = new Dictionary<int, int>()
                        {
                            {0,-1},
                            {1,-1},
                            {2,-1},
                            {3,-1},
                            {4,-1},
                            {5,-1},
                            {6,-1},
                            {7,-1},
                            {8,-1},
                            {9,-1},
                            {10,-1},
                            {11,-1},
                            {12,-1},
                            {13,-1},
                            {14,-1},
                            {15,-1},
                            {16,-1},
                            {17,-1},
                            {18,-1},
                            {19,-1},
                            {20,-1},
                            {21,-1},
                            {22,-1},
                            {23,-1},
                            {24,-1},
                            {25,-1},
                            {26,-1},
                            {27,-1},
                            {28,-1},
                            {29,-1},
                            {30,-1},
                            {31,-1},
                            {32,-1},
                            {33,-1},
                            {34,-1},
                            {35,-1},
                            {36,-1},
                            {37,-1}
                        },
                        returningOjb = commandWithTime.ReturningOjb.ojbWithoutBoss(new Node() { path = new List<Node.pathItem>() }),
                        PromoteDiamondCount = new Dictionary<string, int>()
                        {
                            {"mile",0},
                            {"business",0 },
                            {"volume",0 },
                            {"speed",0 }
                        },
                        positionInStation = that.rm.Next(0, 5)
                    };
                    // BaseInfomation.rm.AddPlayer

                    that._Players.Add(key, npc);
                    that._Players[key].initializeCar(that, cf);
                    that._Players[key].initializeOthers();
                    // this._Players[addItem.Key].SysRemovePlayerByKeyF = BaseInfomation.rm.SysRemovePlayerByKey;
                    //System.Random rm = new System.Random(DateTime.Now.GetHashCode());

                    int fpIndex = that.GetRandomPosition(false, gp); // this.rm.Next(0, Program.dt.GetFpCount());

                    // this._FpOwner.Add(fpIndex, addItem.Key);
                    that._Players[key].StartFPIndex = fpIndex;
                    //SetMoneyCanSave 在InitializeDebt 之后，MoneySet之前
                    // ((NPC)this._Players[key]).SetMoneyCanSave = this.SetMoneyCanSave;// RoomMain.SetMoneyCanSave;
                    //((NPC)this._Players[key]).MoneyChanged = this.MoneyChanged;//  RoomMain.MoneyChanged;
                    var notifyMsgs = new List<string>();
                    that._Players[key].MoneySet((5 + level * 2) * 100 * 100, ref notifyMsgs);

                    // this._Players[addItem.Key].SupportChangedF = RoomMain.SupportChanged;

                    // that._Players[key].TheLargestHolderKeyChanged = that.TheLargestHolderKeyChanged;
                    that._Players[key].InitializeTheLargestHolder();

                    // this._Players[addItem.Key].Money

                    that._Players[key].BustChangedF = that.BustChangedF;
                    that._Players[key].SetBust(false, ref notifyMsgs);

                    //that._Players[key].DrawSingleRoadF = that.DrawSingleRoadF;
                    // that._Players[key].addUsedRoad(Program.dt.GetFpByIndex(fpIndex).RoadCode, ref notifyMsgs);

                    //   this._Players[addItem.Key].brokenParameterT1RecordChanged = this.brokenParameterT1RecordChanged;
                    //  this._Players[addItem.Key].DrawSingleRoadF = this.DrawSingleRoadF;
                    that._Players[key].setType(RoleInGame.PlayerType.NPC);
                    that._Players[key].SetLevel(level, ref notifyMsgs);
                    npc.ShowLevelOfPlayerF = that.ShowLevelOfPlayerF;
                    npc.BeingAttackedM = this.NPCBeingAttacked;
                    npc.afterWaitedM = this.afterWaitedM;
                    npc.afterReturnedM = this.afterReturnedM;
                    npc.afterBroke = this.afterBroke;
                    npc.BeingMolestedM = this.BeingMolestedM;
                    //npc.BeAttacked=

                    that.ConfigMagic(npc);

                    npc.buildingReward = new Dictionary<int, int>()
                    {
                        {0,0},
                        {1,0},
                        {2,0},
                        {3,0},
                        {4,0}
                    };
                    //npc.confuseUsing = null;
                }
            }
            return key;

            //  throw new NotImplementedException();
        }

        //NPC npc, ref List<string> notifyMsgs
        private void BeingMolestedM(NPC npc, ref List<string> notifyMsg, GetRandomPos grp)
        {
            counterAttack(npc, ref notifyMsg, grp);
        }
        //NPC npc, ref List<string> notifyMsgs
        public void afterBroke(NPC npc, ref List<string> notifyMsgs, GetRandomPos grp)
        {
            //   throw new NotImplementedException();
            //  if(this.)
            if (!that.HasPartnerIsInGame(npc))
            {
                List<string> keys = new List<string>();

                foreach (var item in that._Players)
                {
                    if (item.Value.Key == npc.challenger)
                    {
                        if (item.Value.playerType == RoleInGame.PlayerType.player)
                        {
                            keys.Add(item.Key);
                        }
                    }
                }
                for (int i = 0; i < keys.Count; i++)
                {
                    if (that._Players[keys[i]].Level < npc.Level)
                    {
                        that._Players[keys[i]].SetLevel(npc.Level, ref notifyMsgs);
                        that.modelL.OrderToUpdateLevel(that._Players[keys[i]], ref notifyMsgs);
                    }
                    this.that.taskM.PlayerLevelSynchronize((Player)that._Players[keys[i]]);
                }
            }
        }

        //internal void Moleste(Player player, int target, ref List<string> notifyMsg)
        //{
        //    RoleInGame bossOfPlayer;
        //    if (player.HasTheBoss(this.that._Players, out bossOfPlayer))
        //    {

        //    }
        //    else
        //    {
        //        bossOfPlayer = player;
        //    }
        //    var from = Program.dt.GetFpByIndex(bossOfPlayer.StartFPIndex);
        //    double minDistance = double.MaxValue;
        //    NPC nearestNPC = null;
        //    foreach (var item in that._Players)
        //    {
        //        if (item.Value.playerType == RoleInGame.PlayerType.NPC)
        //        {
        //            var fpTo = Program.dt.GetFpByIndex(item.Value.StartFPIndex);
        //            var distance = CommonClass.Geography.getLengthOfTwoPoint.GetDistance(from.Latitde, from.Longitude, from.Height, fpTo.Latitde, fpTo.Longitude, fpTo.Height);
        //            if (distance < minDistance)
        //            {
        //                nearestNPC = (NPC)item.Value;
        //                minDistance = distance;
        //            }
        //        }
        //    }
        //    if (nearestNPC != null)
        //    {

        //        bool notNeedToMoleste;
        //        if (bossOfPlayer.Level >= nearestNPC.Level)
        //        {
        //            notNeedToMoleste = true;
        //        }
        //        else if (bossOfPlayer.Level + 1 == nearestNPC.Level)
        //        {
        //            if (that.rm.Next(0, 100) < 90)
        //            {
        //                notNeedToMoleste = true;
        //            }
        //            else
        //            {
        //                notNeedToMoleste = false;
        //            }
        //        }
        //        else
        //        {
        //            notNeedToMoleste = false;
        //        }
        //        if (notNeedToMoleste) { }
        //        else if (nearestNPC.BeingMolestedF(player.Key, ref notifyMsg))
        //        {
        //            this.WebNotify(player, $"你骚扰了【{nearestNPC.PlayerName}】，他会对你进行一次攻击！");
        //        }
        //    }
        //    //throw new NotImplementedException();

        //}

        //NPC npc, ref List<string> notifyMsgs
        private void afterReturnedM(NPC npc, ref List<string> notifyMsgs, GetRandomPos grp)
        {
            this.counterAttack(npc, ref notifyMsgs, grp);
        }

        private void afterWaitedM(NPC npc, ref List<string> notifyMsgs, GetRandomPos grp)
        {
            if (waitedFunctionMWithChanllenger(npc, ref notifyMsgs, npc.challenger, grp)) { }
            //    else if (waitedFunctionM(npc, ref notifyMsgs, npc.molester)) { }
        }

        /// <summary>
        /// 给返回自己基地的NPC分配任务！
        /// </summary>
        /// <param name="npc2"></param>
        /// <param name="notifyMsgs"></param>
        /// <param name="operateKye"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        bool waitedFunctionMWithChanllenger(NPC npc2, ref List<string> notifyMsgs, string operateKye, GetRandomPos grp)
        {
            if (npc2.getCar().state == Car.CarState.waitAtBaseStation)
                that.GetMaxHarmInfomation(npc2, grp);
            bool doNext;
            if (!string.IsNullOrEmpty(operateKye))
            {
                if (npc2.attackTag != null)
                {
                    if (that._Players.ContainsKey(npc2.attackTag.Target))
                    {
                        var operatePlayer = that._Players[npc2.attackTag.Target];
                        if (operatePlayer.Bust)
                        {
                            this.CollectNearSelf(npc2, grp);
                        }
                        else
                        {
                            if (this.moneyIsEnoughToAttack(npc2))
                            {
                                switch (npc2.attackTag.aType)
                                {
                                    case NPC.AttackTag.AttackType.attack:
                                        {
                                            Model.FastonPosition fp;
                                            if (that.theNearestToPlayerIsCarNotMoney(npc2, npc2.getCar(), operatePlayer, grp, out fp))
                                            {
                                                var sa = new CommonClass.SetAttack()
                                                {
                                                    c = "SetAttack",
                                                    Key = npc2.Key,
                                                    target = operatePlayer.StartFPIndex,
                                                    targetOwner = operatePlayer.Key,
                                                };
                                                this.startNewCommandThread(200, sa, this, grp);
                                            }
                                            else
                                            {
                                                CollectFp(npc2, fp, ref notifyMsgs, grp);
                                            }
                                        }; break;
                                    case NPC.AttackTag.AttackType.fire:
                                    case NPC.AttackTag.AttackType.electric:
                                        {
                                            if (npc2.getCar().state == Car.CarState.waitAtBaseStation)
                                            {
                                                var fp = npc2.attackTag.fpPass;
                                                CollectFp(npc2, fp, ref notifyMsgs, grp);
                                            }
                                            else if (npc2.getCar().state == Car.CarState.waitOnRoad)
                                            {
                                                //   npc2.attackTag.Target
                                                var roleKey = npc2.attackTag.Target;
                                                if (that._Players.ContainsKey(roleKey))
                                                {
                                                    var role = that._Players[roleKey];
                                                    CommonClass.MagicSkill ms = new CommonClass.MagicSkill()
                                                    {
                                                        c = "MagicSkill",
                                                        Key = npc2.Key,
                                                        selectIndex = 2,
                                                        target = role.StartFPIndex,
                                                        targetOwner = roleKey
                                                    };
                                                    this.startNewCommandThread(200, ms, this, grp);
                                                }
                                                else
                                                {
                                                    /*
                                                     * return
                                                     */
                                                    CommonClass.OrderToReturn otr = new CommonClass.OrderToReturn()
                                                    {
                                                        c = "OrderToReturn",
                                                        Key = npc2.Key
                                                    };
                                                    this.startNewCommandThread(200, otr, this, grp);
                                                }

                                            }
                                            else
                                            {
                                                throw new Exception("非法调用");
                                            }
                                        }; break;
                                    case NPC.AttackTag.AttackType.water:
                                        {
                                            if (npc2.getCar().state == Car.CarState.waitAtBaseStation)
                                            {
                                                var fp = npc2.attackTag.fpPass;
                                                CollectFp(npc2, fp, ref notifyMsgs, grp);
                                            }
                                            else if (npc2.getCar().state == Car.CarState.waitOnRoad)
                                            {
                                                //   npc2.attackTag.Target
                                                var roleKey = npc2.attackTag.Target;
                                                if (that._Players.ContainsKey(roleKey))
                                                {
                                                    var role = that._Players[roleKey];
                                                    CommonClass.MagicSkill ms = new CommonClass.MagicSkill()
                                                    {
                                                        c = "MagicSkill",
                                                        Key = npc2.Key,
                                                        selectIndex = 1,
                                                        target = role.StartFPIndex,
                                                        targetOwner = roleKey
                                                    };
                                                    this.startNewCommandThread(200, ms, this, grp);
                                                }
                                                else
                                                {
                                                    /*
                                                     * return
                                                     */
                                                    CommonClass.OrderToReturn otr = new CommonClass.OrderToReturn()
                                                    {
                                                        c = "OrderToReturn",
                                                        Key = npc2.Key
                                                    };
                                                    this.startNewCommandThread(200, otr, this, grp);
                                                }

                                            }
                                            else
                                            {
                                                throw new Exception("非法调用");
                                            }
                                        }; break;
                                    case NPC.AttackTag.AttackType.lose:
                                    case NPC.AttackTag.AttackType.confuse:
                                    case NPC.AttackTag.AttackType.speed:
                                    case NPC.AttackTag.AttackType.defendImprove:
                                        {
                                            if (npc2.getCar().state == Car.CarState.waitAtBaseStation)
                                            {
                                                var fp = npc2.attackTag.fpPass;
                                                CollectFp(npc2, fp, ref notifyMsgs, grp);
                                            }
                                            else if (npc2.getCar().state == Car.CarState.waitOnRoad)
                                            {
                                                Model.FastonPosition fp;
                                                if (that.theNearestToPlayerIsCarNotMoney(npc2, npc2.getCar(), operatePlayer, Program.dt, out fp))
                                                {
                                                    var roleKey = npc2.attackTag.Target;
                                                    if (that._Players.ContainsKey(roleKey))
                                                    {
                                                        var role = that._Players[roleKey];
                                                        CommonClass.MagicSkill ms = new CommonClass.MagicSkill()
                                                        {
                                                            c = "MagicSkill",
                                                            Key = npc2.Key,
                                                            selectIndex = 2,
                                                            target = role.StartFPIndex,
                                                            targetOwner = roleKey
                                                        };
                                                        this.startNewCommandThread(200, ms, this, grp);
                                                    }
                                                    else
                                                    {
                                                        CommonClass.OrderToReturn otr = new CommonClass.OrderToReturn()
                                                        {
                                                            c = "OrderToReturn",
                                                            Key = npc2.Key
                                                        };
                                                        this.startNewCommandThread(200, otr, this, grp);
                                                    }
                                                }
                                                else
                                                {
                                                    CollectFp(npc2, fp, ref notifyMsgs, grp);
                                                }
                                            }
                                            else
                                            {
                                                throw new Exception("非法调用");
                                            }
                                        }; break;
                                    case NPC.AttackTag.AttackType.ambush:
                                    case NPC.AttackTag.AttackType.attackImprove:
                                        {
                                            if (npc2.getCar().state == Car.CarState.waitAtBaseStation)
                                            {
                                                var fp = npc2.attackTag.fpPass;
                                                CollectFp(npc2, fp, ref notifyMsgs, grp);
                                            }
                                            else if (npc2.getCar().state == Car.CarState.waitOnRoad)
                                            {
                                                Model.FastonPosition fp;
                                                if (that.theNearestToPlayerIsCarNotMoney(npc2, npc2.getCar(), operatePlayer, Program.dt, out fp))
                                                {
                                                    var roleKey = npc2.attackTag.Target;
                                                    if (that._Players.ContainsKey(roleKey))
                                                    {
                                                        var role = that._Players[roleKey];
                                                        CommonClass.MagicSkill ms = new CommonClass.MagicSkill()
                                                        {
                                                            c = "MagicSkill",
                                                            Key = npc2.Key,
                                                            selectIndex = 1,
                                                            target = role.StartFPIndex,
                                                            targetOwner = roleKey
                                                        };
                                                        this.startNewCommandThread(200, ms, this, grp);
                                                    }
                                                    else
                                                    {
                                                        CommonClass.OrderToReturn otr = new CommonClass.OrderToReturn()
                                                        {
                                                            c = "OrderToReturn",
                                                            Key = npc2.Key
                                                        };
                                                        this.startNewCommandThread(200, otr, this, grp);
                                                    }
                                                }
                                                else
                                                {
                                                    CollectFp(npc2, fp, ref notifyMsgs, grp);
                                                }
                                            }
                                            else
                                            {
                                                throw new Exception("非法调用");
                                            }
                                        }; break;
                                    default:
                                        {
                                            throw new Exception("运行错误！");
                                        };


                                }
                            }
                            else
                            {
                                this.CollectNearSelf(npc2, grp);
                            }
                        }
                    }
                    else
                    {
                        this.CollectNearSelf(npc2, grp);
                    }

                }
                else
                {
                    this.CollectNearSelf(npc2, grp);
                }
                doNext = false;
            }
            else
            {
                doNext = true;
            }
            return doNext;
        }
       

        bool moneyIsEnoughToAttack(NPC npc)
        {
            return npc.Money >= npc.getCar().ability.Business * 5;
        }
        private void CollectFp(NPC npc, Model.FastonPosition fp, ref List<string> notifyMsgs, GetRandomPos grp)
        {
            var collectIndex = -1;
            foreach (var item in that._collectPosition)
            {
                if (grp.GetFpByIndex(item.Value).FastenPositionID == fp.FastenPositionID)
                {
                    collectIndex = item.Key;
                }
            }
            if (collectIndex >= 0)
            {
                var sc = new CommonClass.SetCollect()
                {
                    c = "SetCollect",
                    collectIndex = collectIndex,
                    cType = "findWork",
                    fastenpositionID = fp.FastenPositionID,
                    Key = npc.Key
                };
                this.startNewCommandThread(200, sc, this, grp);
            }
        }

        private void GetNPCPosition(string key)
        {
            that.GetNPCPosition(key);
        }
        public void NPCBeingAttacked(string keyOfAttacker, NPC npc, ref List<string> notifyMsg, interfaceOfHM.Car cf, GetRandomPos gp)
        {
            //int level, interfaceOfHM.Car cf, GetRandomPos gp
            //lock()
            if (that._Players.ContainsKey(keyOfAttacker))
            {
                if (that._Players[keyOfAttacker].playerType == RoleInGame.PlayerType.player)
                {
                    var attacker = (Player)that._Players[keyOfAttacker];
                    if (npc.challenger == "")
                    {

                        npc.setChallenger(attacker.Key, ref notifyMsg);

                        if (npc.Bust)
                        {
                            //说明已经一步到位。
                            //   attacker
                            if (npc.Level > attacker.Level)
                            {
                                attacker.SetLevel(npc.Level, ref notifyMsg);
                                if (attacker.playerType == RoleInGame.PlayerType.player)
                                {

                                }
                            }
                        }
                        else
                        {
                            if (npc.Level == 2)
                            {
                                /*
                                 * 最低级的NPC，而玩家的最低级为1级。
                                 * 2级的NPC不存在裂变。
                                 * 在输出系(Immortal)/提升系中(Devil)中二选一。
                                 */
                                switch (that.rm.Next(0, 2))
                                {
                                    case 0:
                                        {
                                            that.driverM.SetAsDevil(npc, ref notifyMsg);
                                            counterAttack(npc, ref notifyMsg, gp);
                                        }; break;
                                    case 1:
                                        {
                                            that.driverM.SetAsImmortal(npc, ref notifyMsg);
                                            counterAttack(npc, ref notifyMsg, gp);
                                        }; break;
                                }
                            }
                            else if (npc.Level == 3)
                            {
                                var addNpcKey = this.AddNpcPlayer(npc.Level, cf, gp);
                                this.GetNPCPosition(addNpcKey);
                                var addNpc = (NPC)that._Players[addNpcKey];
                                ///
                                addNpc.SetTheLargestHolder(npc, ref notifyMsg);
                                addNpc.CopyChanlleger(npc.challenger);
                                switch (that.rm.Next(0, 2))
                                {
                                    case 0:
                                        {
                                            that.driverM.SetAsDevil(npc, ref notifyMsg);
                                            counterAttack(npc, ref notifyMsg, gp);

                                            that.driverM.SetAsImmortal(addNpc, ref notifyMsg);
                                            counterAttack(addNpc, ref notifyMsg, gp);
                                        }; break;
                                    case 1:
                                        {
                                            that.driverM.SetAsImmortal(npc, ref notifyMsg);
                                            counterAttack(npc, ref notifyMsg, gp);

                                            that.driverM.SetAsDevil(addNpc, ref notifyMsg);
                                            counterAttack(addNpc, ref notifyMsg, gp);
                                        }; break;
                                }
                                // throw new Exception("");
                            }
                            else if (npc.Level == 4)
                            {
                                List<NPC> roles = new List<NPC>() { null, null, null };


                                {
                                    // List<int> indexOfRole = new List<int>();
                                    int indexPosition;
                                    do
                                    {
                                        indexPosition = that.rm.Next(0, 3);
                                    }
                                    while (roles[indexPosition] != null);
                                    roles[indexPosition] = npc;
                                }
                                for (var i = 0; i < 2; i++)
                                {
                                    int indexPosition;
                                    var addNpcKey = this.AddNpcPlayer(npc.Level, cf, gp);
                                    this.GetNPCPosition(addNpcKey);
                                    var addNpc = (NPC)that._Players[addNpcKey];
                                    addNpc.SetTheLargestHolder(npc, ref notifyMsg);
                                    addNpc.CopyChanlleger(npc.challenger);
                                    do
                                    {
                                        indexPosition = that.rm.Next(0, 3);
                                    }
                                    while (roles[indexPosition] != null);
                                    roles[indexPosition] = addNpc;
                                }
                                for (var i = 0; i < roles.Count; i++)
                                {
                                    if (i == 0)
                                    {
                                        that.driverM.SetAsImmortal(roles[i], ref notifyMsg);
                                    }
                                    else if (i == 1)
                                    {
                                        that.driverM.SetAsDevil(roles[i], ref notifyMsg);
                                    }
                                    else if (i == 2)
                                    {
                                        that.driverM.SetAsPeople(roles[i], ref notifyMsg);
                                    }
                                    counterAttack(roles[i], ref notifyMsg, gp);
                                }
                            }
                            else if (npc.Level > 4)
                            {

                                var max = npc.Level - 1;
                                max = Math.Min(5, max);

                                List<NPC> roles = new List<NPC>(max) { };

                                for (int indexOfRole = 0; indexOfRole < max; indexOfRole++)
                                {
                                    roles.Add(null);
                                }
                                {
                                    // List<int> indexOfRole = new List<int>();
                                    int indexPosition;
                                    do
                                    {
                                        indexPosition = that.rm.Next(0, max);
                                    }
                                    while (roles[indexPosition] != null);
                                    roles[indexPosition] = npc;
                                }
                                for (int i = 0; i < max - 1; i++)
                                {
                                    int indexPosition;
                                    var addNpcKey = this.AddNpcPlayer(npc.Level, cf, gp);
                                    this.GetNPCPosition(addNpcKey);
                                    var addNpc = (NPC)that._Players[addNpcKey];
                                    addNpc.SetTheLargestHolder(npc, ref notifyMsg);
                                    addNpc.CopyChanlleger(npc.challenger);
                                    do
                                    {
                                        indexPosition = that.rm.Next(0, max);
                                    }
                                    while (roles[indexPosition] != null);
                                    roles[indexPosition] = addNpc;
                                }
                                for (var i = 0; i < roles.Count; i++)
                                {
                                    if (i == 0)
                                    {
                                        that.driverM.SetAsImmortal(roles[i], ref notifyMsg);
                                    }
                                    else if (i == 1)
                                    {
                                        that.driverM.SetAsDevil(roles[i], ref notifyMsg);
                                    }
                                    else if (i == 2)
                                    {
                                        that.driverM.SetAsPeople(roles[i], ref notifyMsg);
                                    }
                                    else
                                    {
                                        var rmNum = that.rm.Next(3);
                                        switch (rmNum)
                                        {
                                            case 0:
                                                {
                                                    that.driverM.SetAsImmortal(roles[i], ref notifyMsg);
                                                }; break;
                                            case 1:
                                                {
                                                    that.driverM.SetAsDevil(roles[i], ref notifyMsg);
                                                }; break;
                                            case 2:
                                                {
                                                    that.driverM.SetAsPeople(roles[i], ref notifyMsg);
                                                }; break;
                                        }
                                    }
                                    counterAttack(roles[i], ref notifyMsg, gp);
                                }
                            }
                        }

                    }
                    else
                    {
                        var player = (Player)that._Players[keyOfAttacker];
                        if (npc.challenger == player.Key) { }
                        else if (npc.challenger == player.TheLargestHolderKey) { }
                        else
                            that.WebNotify(player, $"【{npc.PlayerName}】已有挑战者！");
                        //#warning 这里要提示。
                    }
                }
            }
            // throw new NotImplementedException();
        }

        internal void counterAttack(NPC npc, ref List<string> notifyMsg, GetRandomPos grp)
        {
            if (waitedFunctionMWithChanllenger(npc, ref notifyMsg, npc.challenger, grp)) { }

        }
        void dealWithChallenger(NPC npc_Operate, ref List<string> notifyMsg, string operateKey)
        {
            that.GetMaxHarmInfomation(npc_Operate, Program.dt);
            //bool next;
            //if (!string.IsNullOrEmpty(operateKey))
            //{

            //    if (npc_Operate.Bust) { }
            //    else if (!that._Players.ContainsKey(operateKey))
            //    {
            //        npc_Operate.SetBust(true, ref notifyMsg);
            //    }
            //    else
            //    {
            //        if (that._Players[operateKey].Bust)
            //        {
            //            npc_Operate.SetBust(true, ref notifyMsg);
            //        }
            //        else if (that._Players[operateKey].playerType == RoleInGame.PlayerType.player)
            //        {
            //            this.afterWaitedM(npc_Operate, ref notifyMsg);
            //        }
            //    }
            //    next = false;
            //}
            //else
            //{
            //    next = true;
            //}
            //return next;
        }




        private void CollectNearSelf(NPC npc, GetRandomPos grp)
        {
            var minLength = double.MaxValue;
            var collectIndex = -1;
            string fastenPositionID = "";
            var car = npc.getCar();
            Model.FastonPosition carPosition;
            if (car.state == Car.CarState.waitOnRoad)
            {
                carPosition = Program.dt.GetFpByIndex(npc.getCar().targetFpIndex);
            }
            else if (car.state == Car.CarState.waitAtBaseStation)
            {
                carPosition = Program.dt.GetFpByIndex(npc.StartFPIndex);
            }
            else
            {
                throw new Exception("非法调用！");
            }
            foreach (var item in that._collectPosition)
            {
                var collectPosition = Program.dt.GetFpByIndex(item.Value);
                var length = CommonClass.Geography.getLengthOfTwoPoint.GetDistance(carPosition.Latitde, carPosition.Longitude, carPosition.Height, collectPosition.Latitde, collectPosition.Longitude, collectPosition.Height);
                if (length < minLength)
                {
                    minLength = length;
                    collectIndex = item.Key;
                    fastenPositionID = collectPosition.FastenPositionID;
                }
            }
            var sc = new CommonClass.SetCollect()
            {
                c = "SetCollect",
                collectIndex = collectIndex,
                cType = "findWork",
                fastenpositionID = fastenPositionID,
                Key = npc.Key
            };
            this.startNewCommandThread(100, sc, this, grp);
        }

        public void newThreadDo(CommonClass.Command c, GetRandomPos grp)
        {
            if (c.c == "SetCollect")
            {
                var sc = (CommonClass.SetCollect)c;
                that.collectE.updateCollect(sc, grp);
            }
            else if (c.c == "SetAttack")
            {
                var sa = (CommonClass.SetAttack)c;
                that.attackE.updateAttack(sa, grp);
            }
            else if (c.c == "MagicSkill")
            {
                var ms = (CommonClass.MagicSkill)c;
                that.magicE.updateMagic(ms, grp);
            }
            else if (c.c == "OrderToReturn")
            {
                var otr = (CommonClass.OrderToReturn)c;
                that.retutnE.OrderToReturn(otr, grp);
            }
        }


    }

    /*
     * 要触发NPC自动运行需要三个条件：
     * A.NPC创建时
     * B.NPC回到基地时
     * C.轮巡NPC之时。
     *   包括破产、取消、控制法术转攻击
     */
}
