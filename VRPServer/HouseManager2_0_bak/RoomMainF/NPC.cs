using CommonClass;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HouseManager2_0.RoomMainF
{
    public partial class RoomMain
    {
        bool npcDebug = true;
        //  NPCControle npcc;
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
        internal void SetNPC()
        {
            AddNPC();
            ChangeRadius();
            CommandNPC();
            // throw new NotImplementedException();
        }

        private async void CommandNPC()
        {
            //throw new NotImplementedException();
            var npcs = this.getGetAllNPC();
            for (var i = 0; i < npcs.Count; i++)
            {
                {
                    var npc = npcs[i];
                    if (!npc.Bust)
                    {
                        if (npc.getCar().state == Car.CarState.waitAtBaseStation)
                        {
                            var xx = SetNPCToDoSomeThing(npc, NPCAction.Bust);
                        }
                    }
                }

            }
        }
        enum NPCAction
        {
            Bust, Attack, Tax, Collect, Wait
        }
        private string SetNPCToDoSomeThing(NPC npc, NPCAction selectV)
        {
            switch (selectV)
            {
                case NPCAction.Bust:
                    {
                        /*
                         * 0代表破产
                         */
                        var players = this.getGetAllPlayers();
                        string keyToSetBust;
                        if (SuitToSetBust(npc, players, out keyToSetBust))
                        {
                            /*
                             * 如果适合破产，将设置破产
                             */
                            NpcDebugWrite($"{npc.PlayerName}({npc.Key})可以bust,其要对{this._Players[keyToSetBust].PlayerName}({keyToSetBust})进行破产清算！！！");
                            var sb = new SetBust()
                            {
                                c = "SetBust",
                                Key = npc.Key,
                                target = this._Players[keyToSetBust].StartFPIndex,
                                targetOwner = keyToSetBust
                            };
                            return updateBust(sb);
                        }
                        else
                        {
                            NpcDebugWrite($"{npc.PlayerName}({npc.Key})不可以bust");
                            //执行攻击
                            return SetNPCToDoSomeThing(npc, NPCAction.Attack);
                        }
                    };
                case NPCAction.Attack:
                    {
                        //   lock (this.PlayerLock)
                        {
                            npc.ClearEnemiesAndMolester(this._Players);
                            if (npc.SuitToAttack())
                            {
                                if (npc.Enemies.Count > 0)
                                {
                                    string enemyKey = npc.Enemies[this.rm.Next(npc.Enemies.Count)];
                                    NpcDebugWrite($"{npc.PlayerName}({npc.Key})可以Attack,其要对{this._Players[enemyKey].PlayerName}({enemyKey})进行攻击！！！");
                                    var sa = new SetAttack()
                                    {
                                        c = "SetAttack",
                                        Key = npc.Key,
                                        target = this._Players[enemyKey].StartFPIndex,
                                        targetOwner = enemyKey
                                    };
                                    return updateAttack(sa);
                                }
                                else if (npc.Molester.Count > 0)
                                {
                                    string molesterKey = npc.Molester[this.rm.Next(npc.Molester.Count)];
                                    NpcDebugWrite($"{npc.PlayerName}({npc.Key})可以Attack,其要对{this._Players[molesterKey].PlayerName}({molesterKey})进行攻击！！！");
                                    var sa = new SetAttack()
                                    {
                                        c = "SetAttack",
                                        Key = npc.Key,
                                        target = this._Players[molesterKey].StartFPIndex,
                                        targetOwner = molesterKey
                                    };
                                    return updateAttack(sa);
                                }
                                else
                                {
                                    NpcDebugWrite($"{npc.PlayerName}({npc.Key})不可以Attack");
                                    return SetNPCToDoSomeThing(npc, NPCAction.Tax);
                                }
                            }
                            else
                            {
                                NpcDebugWrite($"{npc.PlayerName}({npc.Key})不可以Attack");
                                return SetNPCToDoSomeThing(npc, NPCAction.Tax);
                            }
                        }
                    };
                case NPCAction.Tax:
                    {
                        if (npc.SuitToCollectTax())
                        {
                            var taxInPositions = npc.TaxInPositionForeach();
                            var position = -1;
                            double minLength = double.MaxValue;
                            for (int indexOfTaxPosition = 0; indexOfTaxPosition < taxInPositions.Count; indexOfTaxPosition++)
                            {
                                var length = Distance(taxInPositions[indexOfTaxPosition], npc.StartFPIndex);
                                if (length < minLength)
                                {
                                    minLength = length;
                                    position = taxInPositions[indexOfTaxPosition];
                                }
                            }
                            if (position >= 0)
                            {
                                NpcDebugWrite($"{npc.PlayerName}({npc.Key})可以Tax,其要对{ Program.dt.GetFpByIndex(position).FastenPositionName}进行收取红包！！！");
                                var st = new SetTax()
                                {
                                    c = "SetTax",
                                    target = position,
                                    Key = npc.Key
                                };
                                return updateTax(st);
                            }
                            else
                            {
                                NpcDebugWrite($"{npc.PlayerName}({npc.Key})不可以Tax");
                                return SetNPCToDoSomeThing(npc, NPCAction.Collect);
                            }
                        }
                        else
                        {
                            NpcDebugWrite($"{npc.PlayerName}({npc.Key})不可以Tax");
                            return SetNPCToDoSomeThing(npc, NPCAction.Collect);
                        }
                    };
                case NPCAction.Collect:
                    {
                        //  lock (this.PlayerLock)
                        {
                            npc.ClearEnemiesAndMolester(this._Players);
                            if (npc.SuitToCollect())
                            {

                                //  var collectInPositions = npc.TaxInPositionForeach();
                                var position = -1;
                                var collectIndex = -1;
                                double minLength = double.MaxValue;
                                foreach (var item in this._collectPosition)
                                {
                                    if (item.Value >= 0)
                                    {
                                        var length = Distance(item.Value, npc.StartFPIndex);
                                        if (length < minLength)
                                        {
                                            minLength = length;
                                            position = item.Value;
                                            collectIndex = item.Key;
                                        }
                                    }
                                }
                                if (collectIndex >= 0)
                                {
                                    NpcDebugWrite($"{npc.PlayerName}({npc.Key})可以SetCollect");
                                    var ut = new SetCollect()
                                    {
                                        c = "SetCollect",
                                        collectIndex = collectIndex,
                                        cType = "findWork",
                                        fastenpositionID = Program.dt.GetFpByIndex(position).FastenPositionID,
                                        Key = npc.Key
                                    };
                                    return updateCollect(ut);
                                }
                                else
                                {
                                    return SetNPCToDoSomeThing(npc, NPCAction.Wait);
                                }
                            }
                            else
                            {
                                return SetNPCToDoSomeThing(npc, NPCAction.Wait);
                            }
                        }
                    };
                case NPCAction.Wait:
                    {
                        if (npc.getCar().state == Car.CarState.waitForCollectOrAttack)
                        {
                            var otr = new CommonClass.OrderToReturn()
                            {
                                c = "OrderToReturn",
                                Key = npc.Key
                            };
                            return OrderToReturn(otr);
                        }
                        else if (npc.getCar().state == Car.CarState.waitForTaxOrAttack)
                        {
                            var otr = new CommonClass.OrderToReturn()
                            {
                                c = "OrderToReturn",
                                Key = npc.Key
                            };
                            return OrderToReturn(otr);
                        }
                        else if (npc.getCar().state == Car.CarState.waitOnRoad)
                        {
                            var otr = new CommonClass.OrderToReturn()
                            {
                                c = "OrderToReturn",
                                Key = npc.Key
                            };
                            return OrderToReturn(otr);
                        }
                        else
                        {
                            return "";
                        }
                    };
                default:
                    {
                        return "";
                    }
            }
        }

        private void NpcDebugWrite(string v)
        {
            v = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}--{v}";
            Console.WriteLine(v);
            File.AppendAllText("npcDebug.txt", $"{v}{Environment.NewLine}");
        }

        private void NPCAutoControlCollect(RoleInGame role)
        {
            if (role.playerType == RoleInGame.PlayerType.NPC)
            {
                if (role.getCar().ability.costVolume >= role.getCar().ability.Volume)
                {
                    SetNPCToDoSomeThing((NPC)role, NPCAction.Attack);
                }
                else
                {
                    SetNPCToDoSomeThing((NPC)role, NPCAction.Collect);
                }
            }
        }

        private string NPCAutoControlTax(RoleInGame role)
        {
            if (role.playerType == RoleInGame.PlayerType.NPC)
            {
                if (role.getCar().ability.costBusiness >= role.getCar().ability.Business)
                {
                    return this.SetNPCToDoSomeThing((NPC)role, NPCAction.Attack);
                }
                else
                {
                    return this.SetNPCToDoSomeThing((NPC)role, NPCAction.Tax);
                }
            }
            else
            {
                return "";
            }
        }

        private bool SuitToSetBust(NPC npc, List<Player> players, out string keyToSetBust)
        {
            if (this.Market.mile_Price.HasValue)
                if (this.Market.mile_Price.Value < npc.Money)
                    if (npc.getCar().state == Car.CarState.waitAtBaseStation)
                        for (var i = 0; i < players.Count; i++)
                        {
                            if (!players[i].Bust)
                            {
                                if (players[i].TheLargestHolderKey == npc.Key)
                                {
                                    // if(npc.)
                                    if (npc.Enemies.Contains(players[i].Key))
                                    {
                                        keyToSetBust = players[i].Key;
                                        return true;
                                    }
                                }
                            }
                        }
            keyToSetBust = null;
            return false;
        }

        private static double Distance(Model.FastonPosition fp1, Model.FastonPosition fp2)
        {
            double x1, y1;
            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(fp1.Longitude, fp1.Latitde, out x1, out y1);

            double x2, y2;
            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(fp2.Longitude, fp2.Latitde, out x2, out y2);

            return Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }
        private static double Distance(int fp1, int fp2)
        {
            return Distance(Program.dt.GetFpByIndex(fp1), Program.dt.GetFpByIndex(fp2));
        }
        private async void IfIsNPCBuyThenUseDiamond(RoleInGame player, Car car)
        {
            if (player.playerType == RoleInGame.PlayerType.NPC)
            {
                Buy(new SetBuyDiamond()
                {
                    c = "SetBuyDiamond",
                    Key = player.Key,
                    pType = "mile"
                });
                var xx = Program.rm.SetAbility(new CommonClass.SetAbility()
                {
                    c = "SetAbility",
                    Key = player.Key,
                    pType = "mile"
                });
            }
            //throw new NotImplementedException();
        }

        private void ChangeRadius()
        {
            Dictionary<string, bool> inLimitsFp = new Dictionary<string, bool>();
            int Count = Program.dt.Get61Fp();
            var npcs = this.getGetAllNPC();
            for (var i = 0; i < Count; i++)
            {
                for (var j = 0; j < npcs.Count; j++)
                {
                    {
                        var npc = npcs[j];
                        if (npc.Contain(Program.dt.GetFpByIndex(i)))
                        {
                            npc.Radius = (npc.Radius + 1) * 11 / 10;
                        }
                    }
                }
            }
        }

        void AddNPC()
        {
            List<int> levels = new List<int>()
            {
                1,2,3,4,5,6,7,8,9,10
            };
            foreach (var item in this._Players)
            {
                if (item.Value.playerType == RoleInGame.PlayerType.NPC)
                {
                    var Npc = (NPC)item.Value;
                    levels.Remove(Npc.Level);
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
        internal string AddNpcPlayer(int level)
        {

            string key = CommonClass.Random.GetMD5HashFromStr("ss" + DateTime.Now.ToString());
            while (this._Players.ContainsKey(key))
            {
                key = CommonClass.Random.GetMD5HashFromStr("ss" + DateTime.Now.ToString());
                Thread.Sleep(1);
            }
            //bool success;

            List<string> carsState = new List<string>();
            lock (this.PlayerLock)
            {

                {
                    //  success = true;

                    bool hasTheSameName = false;
                    var NPCName = $"{this.NPCNames[this.rm.Next(this.NPCNames.Count)]}-{level}级";
                    do
                    {

                        hasTheSameName = false;
                        foreach (var item in this._Players)
                        {
                            if (item.Value.PlayerName == NPCName)
                            {
                                hasTheSameName = true;
                                break;
                            }
                        }
                        if (hasTheSameName)
                        {
                            NPCName += AddSuffix[this.rm.Next(0, AddSuffix.Length)];
                        }

                    } while (hasTheSameName);

                    // BaseInfomation.rm.AddPlayer
                    this._Players.Add(key, new NPC()
                    {
                        Key = key,
                        PlayerName = NPCName,

                        CreateTime = DateTime.Now,
                        ActiveTime = DateTime.Now,
                        StartFPIndex = -1,
                        //PromoteState = new Dictionary<string, int>()
                        //{
                        //    {"mile",-1},
                        //    {"business",-1 },
                        //    {"volume",-1 },
                        //    {"speed",-1 }
                        //},
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
                        returningRecord = new List<Model.MapGo.nyrqPosition>(),
                        //  OpenMore = 0,
                        PromoteDiamondCount = new Dictionary<string, int>()
                        {
                            {"mile",0},
                            {"business",0 },
                            {"volume",0 },
                            {"speed",0 }
                        },
                        positionInStation = this.rm.Next(0, 5),
                        Level = level,
                        Radius = 1,
                        Enemies = new List<string>(),
                        Molester = new List<string>()
                    });
                    this._Players[key].initializeCar(this);
                    this._Players[key].initializeOthers();
                    // this._Players[addItem.Key].SysRemovePlayerByKeyF = BaseInfomation.rm.SysRemovePlayerByKey;
                    //System.Random rm = new System.Random(DateTime.Now.GetHashCode());

                    int fpIndex = this.GetRandomPosition(false); // this.rm.Next(0, Program.dt.GetFpCount());

                    // this._FpOwner.Add(fpIndex, addItem.Key);
                    this._Players[key].StartFPIndex = fpIndex;

                    this._Players[key].TaxChanged = RoomMain.TaxAdded;
                    this._Players[key].TaxInPositionInit();// = RoomMain.TaxAdded;
                    this._Players[key].InitializeDebt();


                    //SetMoneyCanSave 在InitializeDebt 之后，MoneySet之前
                    // this._Players[key].SetMoneyCanSave = RoomMain.SetMoneyCanSave;
                    // this._Players[key].MoneyChanged = RoomMain.MoneyChanged;
                    var notifyMsgs = new List<string>();
                    this._Players[key].MoneySet(500 * 100, ref notifyMsgs);

                    // this._Players[addItem.Key].SupportChangedF = RoomMain.SupportChanged;

                    this._Players[key].TheLargestHolderKeyChanged = this.TheLargestHolderKeyChanged;
                    this._Players[key].InitializeTheLargestHolder();

                    // this._Players[addItem.Key].Money

                    this._Players[key].BustChangedF = this.BustChangedF;
                    this._Players[key].SetBust(false, ref notifyMsgs);

                    this._Players[key].DrawSingleRoadF = this.DrawSingleRoadF;
                    this._Players[key].addUsedRoad(Program.dt.GetFpByIndex(fpIndex).RoadCode, ref notifyMsgs);

                    this._Players[key].brokenParameterT1RecordChanged = this.brokenParameterT1RecordChanged;
                    //  this._Players[addItem.Key].DrawSingleRoadF = this.DrawSingleRoadF;
                    this._Players[key].setType(RoleInGame.PlayerType.NPC);

                }
            }
            return key;

            //  throw new NotImplementedException();
        }

        internal void GetNPCPosition(string key)
        {
            //  GetPositionResult result;

            bool success;
            //  int OpenMore = -1;//第一次打开？
            var notifyMsgs = new List<string>();
            lock (this.PlayerLock)
            {
                if (this._Players.ContainsKey(key))
                {
                    if (this._Players[key].playerType == RoleInGame.PlayerType.NPC)
                    {
                        var npc = (NPC)this._Players[key];
                        var fp = Program.dt.GetFpByIndex(this._Players[key].StartFPIndex);
                        //var fromUrl = player.FromUrl;
                        //var webSocketID = player.WebSocketID;
                        //var carsNames = this._Players[getPosition.Key].CarsNames;
                        var playerName = this._Players[key].PlayerName;
                        /*
                         * 这已经走查过，在AddNewPlayer、UpdatePlayer时，others都进行了初始化
                         */
                        AddOtherPlayer(key, ref notifyMsgs);
                        this.brokenParameterT1RecordChanged(key, key, this._Players[key].brokenParameterT1, ref notifyMsgs);
                        GetAllCarInfomationsWhenInitialize(key, ref notifyMsgs);
                        //getAllCarInfomations(getPosition.Key, ref notifyMsgs);
                        //   OpenMore = this._Players[key].OpenMore;

                        // var player = this._Players[getPosition.Key];
                        //var m2 = player.GetMoneyCanSave();

                        //    MoneyCanSaveChanged(player, m2, ref notifyMsgs);

                        //SendPromoteCountOfPlayer("mile", player, ref notifyMsgs);
                        //SendPromoteCountOfPlayer("business", player, ref notifyMsgs);
                        //SendPromoteCountOfPlayer("volume", player, ref notifyMsgs);
                        //SendPromoteCountOfPlayer("speed", player, ref notifyMsgs);

                        // BroadCoastFrequency(player, ref notifyMsgs);
                        //  npc.SetMoneyCanSave(player, ref notifyMsgs);

                        // player.RunSupportChangedF(ref notifyMsgs);
                        //player.this._Players[addItem.Key].SetMoneyCanSave = RoomMain.SetMoneyCanSave;
                        //MoneyCanSaveChanged(player, player.MoneyForSave, ref notifyMsgs);

                        SendMaxHolderInfoMation(npc, ref notifyMsgs);

                        var players = this._Players;
                        foreach (var item in players)
                        {
                            if (item.Value.TheLargestHolderKey == npc.Key)
                            {
                                npc.TheLargestHolderKeyChanged(item.Key, item.Value.TheLargestHolderKey, item.Key, ref notifyMsgs);
                            }
                        }
                        //var list = npc.usedRoadsList;
                        //for (var i = 0; i < list.Count; i++)
                        //{
                        //    this.DrawSingleRoadF(npc, list[i], ref notifyMsgs);
                        //}

                        //this._Players[getPosition.Key];

                        // this._Players[key].MoneyChanged(npc, this._Players[key].Money, ref notifyMsgs);

                        //result = new GetPositionResult()
                        //{
                        //    Success = true,
                        //    //CarsNames = carsNames,
                        //    Fp = fp,
                        //    FromUrl = fromUrl,
                        //    NotifyMsgs = notifyMsgs,
                        //    WebSocketID = webSocketID,
                        //    PlayerName = playerName,
                        //    positionInStation = this._Players[getPosition.Key].positionInStation
                        //};
                        success = true;
                    }
                    else
                        success = false;
                }
                else
                {
                    success = false;
                }
            }
            //   var notifyMsgs = GPResult.NotifyMsgs;
            for (var i = 0; i < notifyMsgs.Count; i += 2)
            {
                Startup.sendMsg(notifyMsgs[i], notifyMsgs[i + 1]);
            }
            if (success)
            {
                CheckAllPromoteState(key);
                CheckCollectState(key);
                sendCarAbilityState(key);
                sendCarStateAndPurpose(key);
                TellOtherPlayerMyFatigueDegree(key);
                TellMeOtherPlayersFatigueDegree(key);
                TellMeOthersRightAndDuty(key);
            }
            //return result;
        }

        private void MeetWithNPC(SetCollect sc)
        {
            var positionInt = this._collectPosition[sc.collectIndex];
            var npcs = this.getGetAllNPC();
            for (var i = 0; i < npcs.Count; i++)
            {
                var npc = npcs[i];
                if (npc.Radius > Distance(positionInt, npc.StartFPIndex))
                {
                    if (!npc.Molester.Contains(sc.Key))
                    {
                        npc.Molester.Add(sc.Key);
                    }
                }
            }
            //   throw new NotImplementedException();
        }
        private void MeetWithNPC(SetAttack sa)
        {
            // if (player.playerType == RoleInGame.PlayerType.player)
            {
                if (this._Players.ContainsKey(sa.targetOwner) && this._Players.ContainsKey(sa.Key))
                {
                    if (this._Players[sa.targetOwner].playerType == RoleInGame.PlayerType.NPC
                        && this._Players[sa.Key].playerType == RoleInGame.PlayerType.player)
                    {
                        var npc = ((NPC)this._Players[sa.targetOwner]);
                        if (npc.Enemies.Contains(sa.Key)) { }
                        else
                        {
                            npc.Enemies.Add(sa.Key);
                        }
                    }
                }
            }
        }
    }
}
