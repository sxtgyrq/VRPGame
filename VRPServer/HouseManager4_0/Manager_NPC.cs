using HouseManager4_0.interfaceOfEngine;
using HouseManager4_0.RoomMainF;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using static HouseManager4_0.RoomMainF.RoomMain;

namespace HouseManager4_0
{
    public class Manager_NPC : Manager, interfaceOfEngine.startNewCommandThread
    {
        List<string> NPCNames = new List<string>()
        {
            "尧帝",
            "重耳",
            "张仪",
            "霍去病",
            "尉迟恭",
            "关羽",
            "王维",
            "白居易",
            "罗贯中",
            "柳宗元"
        };

        internal void ClearNPC()
        {
            lock (that.PlayerLock)
            {
                int maxLevel = -1;
                foreach (var item in that._Players)
                {
                    if (item.Value.playerType == RoleInGame.PlayerType.player)
                    {
                        var level = getLevelOfRole((Player)item.Value);
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
            }
        }

        public Manager_NPC(RoomMain roomMain)
        {
            this.roomMain = roomMain;
        }
        public void AddNPC()
        {
            lock (that.PlayerLock)
            {
                List<int> levels = new List<int>();
                foreach (var item in that._Players)
                {
                    if (item.Value.playerType == RoleInGame.PlayerType.player)
                    {
                        var level = getLevelOfRole((Player)item.Value) + 1;
                        if (levels.Contains(level)) { }
                        else
                        {
                            levels.Add(level);
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
                        var key = AddNpcPlayer(levels[i]);
                        GetNPCPosition(key);
                    }
                }
            }
        }

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
        internal string AddNpcPlayer(int level)
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
                        returningOjb = commandWithTime.ReturningOjb.ojbWithoutBoss(new List<Model.MapGo.nyrqPosition>()),
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
                    that._Players[key].initializeCar(that);
                    that._Players[key].initializeOthers();
                    // this._Players[addItem.Key].SysRemovePlayerByKeyF = BaseInfomation.rm.SysRemovePlayerByKey;
                    //System.Random rm = new System.Random(DateTime.Now.GetHashCode());

                    int fpIndex = that.GetRandomPosition(false); // this.rm.Next(0, Program.dt.GetFpCount());

                    // this._FpOwner.Add(fpIndex, addItem.Key);
                    that._Players[key].StartFPIndex = fpIndex;
                    //SetMoneyCanSave 在InitializeDebt 之后，MoneySet之前
                    // ((NPC)this._Players[key]).SetMoneyCanSave = this.SetMoneyCanSave;// RoomMain.SetMoneyCanSave;
                    //((NPC)this._Players[key]).MoneyChanged = this.MoneyChanged;//  RoomMain.MoneyChanged;
                    var notifyMsgs = new List<string>();
                    that._Players[key].MoneySet((5 + level * 2) * 100 * 100, ref notifyMsgs);

                    // this._Players[addItem.Key].SupportChangedF = RoomMain.SupportChanged;

                    that._Players[key].TheLargestHolderKeyChanged = that.TheLargestHolderKeyChanged;
                    that._Players[key].InitializeTheLargestHolder();

                    // this._Players[addItem.Key].Money

                    that._Players[key].BustChangedF = that.BustChangedF;
                    that._Players[key].SetBust(false, ref notifyMsgs);

                    that._Players[key].DrawSingleRoadF = that.DrawSingleRoadF;
                    that._Players[key].addUsedRoad(Program.dt.GetFpByIndex(fpIndex).RoadCode, ref notifyMsgs);

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
                }
            }
            return key;

            //  throw new NotImplementedException();
        }

        private void BeingMolestedM(NPC npc, ref List<string> notifyMsg)
        {
            counterAttack(npc, ref notifyMsg);
        }

        private void afterBroke(NPC npc, ref List<string> notifyMsgs)
        {
            //   throw new NotImplementedException();
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
                // that._
                ///that.
                ///
                if (that._Players[keys[i]].Level < npc.Level)
                {
                    that._Players[keys[i]].SetLevel(npc.Level, ref notifyMsgs);
                }
            }
        }

        internal void Moleste(Player player, int target, ref List<string> notifyMsg)
        {
            RoleInGame bossOfPlayer;
            if (player.HasTheBoss(this.that._Players, out bossOfPlayer))
            {

            }
            else
            {
                bossOfPlayer = player;
            }
            var from = Program.dt.GetFpByIndex(bossOfPlayer.StartFPIndex);
            double minDistance = double.MaxValue;
            NPC nearestNPC = null;
            foreach (var item in that._Players)
            {
                if (item.Value.playerType == RoleInGame.PlayerType.NPC)
                {
                    var fpTo = Program.dt.GetFpByIndex(item.Value.StartFPIndex);
                    var distance = CommonClass.Geography.getLengthOfTwoPoint.GetDistance(from.Latitde, from.Longitude, fpTo.Latitde, fpTo.Longitude);
                    if (distance < minDistance)
                    {
                        nearestNPC = (NPC)item.Value;
                        minDistance = distance;
                    }
                }
            }
            if (nearestNPC != null)
            {

                bool notNeedToMoleste;
                if (bossOfPlayer.Level >= nearestNPC.Level)
                {
                    notNeedToMoleste = true;
                }
                else if (bossOfPlayer.Level + 1 == nearestNPC.Level)
                {
                    if (that.rm.Next(0, 100) < 90)
                    {
                        notNeedToMoleste = true;
                    }
                    else
                    {
                        notNeedToMoleste = false;
                    }
                }
                else
                {
                    notNeedToMoleste = false;
                }
                if (notNeedToMoleste) { }
                else if (nearestNPC.BeingMolestedF(player.Key, ref notifyMsg))
                {
                    this.WebNotify(player, $"你骚扰了【{nearestNPC.PlayerName}】，他会对你进行一次攻击！");
                }
            }
            //throw new NotImplementedException();

        }

        private void afterReturnedM(NPC npc, ref List<string> notifyMsgs)
        {
            this.counterAttack(npc, ref notifyMsgs);
        }

        private void afterWaitedM(NPC npc, ref List<string> notifyMsgs)
        {
            if (waitedFunctionM(npc, ref notifyMsgs, npc.challenger)) { }
            else if (waitedFunctionM(npc, ref notifyMsgs, npc.molester)) { }
        }
        bool waitedFunctionM(NPC npc2, ref List<string> notifyMsgs, string operateKye)
        {
            bool doNext;
            if (!string.IsNullOrEmpty(operateKye))
            {
                if (that._Players.ContainsKey(operateKye))
                {
                    var operatePlayer = that._Players[operateKye];
                    if (operatePlayer.Bust)
                    {
                        npc2.SetBust(true, ref notifyMsgs);
                    }
                    else
                    {
                        if (this.moneyIsEnoughToAttack(npc2))
                        {
                            Model.FastonPosition fp;
                            if (that.theNearestToPlayerIsCarNotMoney(npc2, npc2.getCar(), operatePlayer, out fp))
                            {
                                var sa = new CommonClass.SetAttack()
                                {
                                    c = "SetAttack",
                                    Key = npc2.Key,
                                    target = operatePlayer.StartFPIndex,
                                    targetOwner = operatePlayer.Key,
                                };
                                this.startNewCommandThread(200, sa, this);
                                //that.attackE.updateAttack();
                            }
                            else
                            {
                                CollectFp(npc2, fp, ref notifyMsgs);
                            }
                        }
                        else
                        {
                            this.CollectNearSelf(npc2);
                        }
                        // CollectNearSelf(npc);
                    }
                }
                else
                {
                    npc2.SetBust(true, ref notifyMsgs);
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
        private void CollectFp(NPC npc, Model.FastonPosition fp, ref List<string> notifyMsgs)
        {
            var collectIndex = -1;
            foreach (var item in that._collectPosition)
            {
                if (Program.dt.GetFpByIndex(item.Value).FastenPositionID == fp.FastenPositionID)
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
                this.startNewCommandThread(200, sc, this);
            }
        }

        private void GetNPCPosition(string key)
        {
            that.GetNPCPosition(key);
        }
        public void NPCBeingAttacked(string keyOfAttacker, NPC npc, ref List<string> notifyMsg)
        {
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
                                attacker.SetLevel(npc.Level, ref notifyMsg);
                        }
                        else
                        {
                            counterAttack(npc, ref notifyMsg);
                        }

                    }
                    else
                    {
#warning 这里要提示。
                    }
                }
            }
            // throw new NotImplementedException();
        }

        private void counterAttack(NPC npc, ref List<string> notifyMsg)
        {
            if (attackFunctionF(npc, ref notifyMsg, npc.challenger))
            { }
            else if (attackFunctionF(npc, ref notifyMsg, npc.molester)) { }
        }
        bool attackFunctionF(NPC npc_Operate, ref List<string> notifyMsg, string operateKey)
        {
            bool next;
            if (!string.IsNullOrEmpty(operateKey))
            {

                if (npc_Operate.Bust) { }
                else if (!that._Players.ContainsKey(operateKey))
                {
                    npc_Operate.SetBust(true, ref notifyMsg);
                }
                else
                {
                    if (that._Players[operateKey].Bust)
                    {
                        npc_Operate.SetBust(true, ref notifyMsg);
                    }
                    else if (that._Players[operateKey].playerType == RoleInGame.PlayerType.player)
                    {
                        this.afterWaitedM(npc_Operate, ref notifyMsg);
                    }
                }
                next = false;
            }
            else
            {
                next = true;
            }
            return next;
        }



        private void CollectNearSelf(NPC npc)
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
                var length = CommonClass.Geography.getLengthOfTwoPoint.GetDistance(carPosition.Latitde, carPosition.Longitude, collectPosition.Latitde, collectPosition.Longitude);
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
            this.startNewCommandThread(100, sc, this);
        }

        public void newThreadDo(CommonClass.Command c)
        {
            if (c.c == "SetCollect")
            {
                var sc = (CommonClass.SetCollect)c;
                that.collectE.updateCollect(sc);
            }
            else if (c.c == "SetAttack")
            {
                var sa = (CommonClass.SetAttack)c;
                that.attackE.updateAttack(sa);
            }
        }


    }
}
