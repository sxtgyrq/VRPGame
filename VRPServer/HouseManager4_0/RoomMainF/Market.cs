using CommonClass;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;

namespace HouseManager4_0.RoomMainF
{
    public partial class RoomMain : interfaceOfHM.Market
    {
        public void priceChanged(string priceType, long value)
        {
            List<string> msgs = new List<string>();
            lock (this.PlayerLock)
            {
                foreach (var item in this._Players)
                {
                    var role = item.Value;
                    if (role.playerType == RoleInGame.PlayerType.player)
                    {
                        var player = (Player)role;
                        var obj = new BradDiamondPrice
                        {
                            c = "BradDiamondPrice",
                            WebSocketID = player.WebSocketID,
                            priceType = priceType,
                            price = value
                        };
                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                        msgs.Add(player.FromUrl);
                        msgs.Add(json);
                    }
                }
            }
            for (var i = 0; i < msgs.Count; i += 2)
            {
                Startup.sendMsg(msgs[i], msgs[i + 1]);
            }
        }

        public void MarketUpdate(MarketPrice sa)
        {
            this.Market.Update(sa);
            // throw new NotImplementedException();
        }

        interface Transaction
        {
            long? Price(RoomMain roomMain);
        }
        class MileTrade : Transaction
        {
            public MileTrade(RoomMain roomMain)
            {
                this.parent = roomMain;
            }
            RoomMain parent;
            public long? Price(RoomMain roomMain)
            {
                return this.parent.Market.mile_Price;
            }
        }
        class BusinessTrade : Transaction
        {
            public BusinessTrade(RoomMain roomMain)
            {
                this.parent = roomMain;
            }
            RoomMain parent;
            public long? Price(RoomMain roomMain)
            {
                return this.parent.Market.business_Price;
            }
        }
        class SpeedTrade : Transaction
        {
            public SpeedTrade(RoomMain roomMain)
            {
                this.parent = roomMain;
            }
            RoomMain parent;
            public long? Price(RoomMain roomMain)
            {
                return this.parent.Market.speed_Price;
            }
        }
        class VolumeTrade : Transaction
        {
            public VolumeTrade(RoomMain roomMain)
            {
                this.parent = roomMain;
            }
            RoomMain parent;
            public long? Price(RoomMain roomMain)
            {
                return this.parent.Market.volume_Price;
            }
        }
        void DoBuyTrade(Transaction t, SetBuyDiamond bd, RoleInGame role, ref List<string> notifyMsg)
        {
            if (t.Price(this) != null)
            {
                var calValue = t.Price(this).Value;
                var oldCount = bd.count + 0;

                var roleMoney = role.Money + 0;
                while (bd.count > 0)
                {
                    if (roleMoney >= calValue)
                    {
                        // role.MoneySet(role.Money - calValue, ref notifyMsg);
                        role.PromoteDiamondCount[bd.pType]++;
                        bd.count--;
                        roleMoney -= calValue;
                    }
                    else
                    {
                        break;
                    }

                }
                if (bd.count != oldCount)
                {
                    role.MoneySet(roleMoney, ref notifyMsg);
                    if (role.playerType == RoleInGame.PlayerType.player)
                        SendPromoteCountOfPlayer(bd.pType, (Player)role, ref notifyMsg);
                    this.Market.Send(bd.pType, oldCount - bd.count);
                    Thread th = new Thread(() => this.Market.Send(bd.pType, oldCount - bd.count));
                    th.Start();
                }
            }
            else
            {
                WebNotify(role, "市场没开放！");
                //Consol.WriteLine("市場沒開放");
            }
        }



        public void Buy(SetBuyDiamond bd)
        {
            //   bd.count = Math.Max(bd.count, 50);
            if (bd.count > 50 || bd.count < 0)
            {
                return;
            }
            else
            {
                List<string> notifyMsg = new List<string>();
                lock (this.PlayerLock)
                    if (this._Players.ContainsKey(bd.Key))
                    {
                        var role = this._Players[bd.Key];
                        if (role.Bust) { }
                        else
                        {
                            switch (bd.pType)
                            {
                                case "mile":
                                    {
                                        MileTrade mt = new MileTrade(this);
                                        DoBuyTrade(mt, bd, role, ref notifyMsg);
                                    }; break;
                                case "business":
                                    {
                                        BusinessTrade bt = new BusinessTrade(this);
                                        DoBuyTrade(bt, bd, role, ref notifyMsg);
                                    }; break;
                                case "volume":
                                    {
                                        VolumeTrade vt = new VolumeTrade(this);
                                        DoBuyTrade(vt, bd, role, ref notifyMsg);
                                    }; break;
                                case "speed":
                                    {
                                        SpeedTrade st = new SpeedTrade(this);
                                        DoBuyTrade(st, bd, role, ref notifyMsg);
                                    }; break;
                            }
                        }
                    }

                for (var i = 0; i < notifyMsg.Count; i += 2)
                {
                    var url = notifyMsg[i];
                    var sendMsg = notifyMsg[i + 1];
                    Startup.sendMsg(url, sendMsg);
                }
            }
        }



        internal void ClearPlayers()
        {
            //   return;
            List<string> notifyMsg = new List<string>();
            lock (this.PlayerLock)
            {
                List<string> keysOfAll = new List<string>();
                List<string> keysNeedToClear = new List<string>();
                foreach (var item in this._Players)
                {

                    if (item.Value.Bust)
                    {
                        if (item.Value.getCar().state == Car.CarState.waitAtBaseStation)
                        {
                            keysNeedToClear.Add(item.Key);
                        }
                        else
                        {
                            keysOfAll.Add(item.Key);
                        }
                    }
                    else
                    {
                        keysOfAll.Add(item.Key);
                    }
                }

                for (var i = 0; i < keysNeedToClear.Count; i++)
                {
                    this._Players.Remove(keysNeedToClear[i]);

                    for (var j = 0; j < keysOfAll.Count; j++)
                    {
                        if (this._Players[keysOfAll[j]].othersContainsKey(keysNeedToClear[i]))
                        {
                            this._Players[keysOfAll[j]].othersRemove(keysNeedToClear[i], ref notifyMsg);
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
        }


        void DoSellTrade(Transaction t, SetSellDiamond ss, RoleInGame role, ref List<string> notifyMsg)
        {
            if (t.Price(this) != null)
            {
                var calValue = t.Price(this).Value * 97 / 100;
                var sellCount = Math.Min(role.PromoteDiamondCount[ss.pType], ss.count);
                role.PromoteDiamondCount[ss.pType] -= sellCount;

                role.MoneySet(role.Money + calValue * sellCount, ref notifyMsg);

                if (sellCount > 0)
                {
                    if (role.playerType == RoleInGame.PlayerType.player)
                        SendPromoteCountOfPlayer(ss.pType, (Player)role, ref notifyMsg);
                    Thread th = new Thread(() => this.Market.Receive(ss.pType, sellCount));
                    th.Start();
                }
            }
            else
            {
                WebNotify(role, "市场没开放！");
            }
        }
        public void Sell(SetSellDiamond ss)
        {
            if (ss.count > 50 || ss.count < 0)
            {
                return;
            }
            List<string> notifyMsg = new List<string>();
            lock (this.PlayerLock)
                if (this._Players.ContainsKey(ss.Key))
                {
                    var role = this._Players[ss.Key];
                    if (role.Bust) { }
                    //else if(player.)
                    else
                    {
                        switch (ss.pType)
                        {
                            case "mile":
                                {
                                    MileTrade mt = new MileTrade(this);
                                    DoSellTrade(mt, ss, role, ref notifyMsg);
                                }; break;
                            case "business":
                                {
                                    BusinessTrade bt = new BusinessTrade(this);
                                    DoSellTrade(bt, ss, role, ref notifyMsg);
                                }; break;
                            case "volume":
                                {
                                    VolumeTrade vt = new VolumeTrade(this);
                                    DoSellTrade(vt, ss, role, ref notifyMsg);
                                }; break;
                            case "speed":
                                {
                                    SpeedTrade st = new SpeedTrade(this);
                                    DoSellTrade(st, ss, role, ref notifyMsg);
                                }; break;
                        }
                    }
                }

            for (var i = 0; i < notifyMsg.Count; i += 2)
            {
                var url = notifyMsg[i];
                var sendMsg = notifyMsg[i + 1];
                //Consol.WriteLine($"url:{url}");
                Startup.sendMsg(url, sendMsg);
            }
        }


    }
}
