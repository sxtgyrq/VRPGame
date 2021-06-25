using CommonClass;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HouseManager2_0.RoomMainF
{
    public partial class RoomMain
    {
        public HouseManager2_0.Market Market { get; internal set; }

        internal void Sell(SetSellDiamond ss)
        {
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
                                    if (this.Market.mile_Price != null)
                                    {
                                        var calValue = this.Market.mile_Price.Value;
                                        if (role.PromoteDiamondCount[ss.pType] > 0)
                                        {
                                            role.MoneySet(role.Money + calValue, ref notifyMsg);
                                            role.PromoteDiamondCount[ss.pType]--;
                                            if (role.playerType == RoleInGame.PlayerType.player)
                                                SendPromoteCountOfPlayer(ss.pType, (Player)role, ref notifyMsg);
                                            this.Market.Receive(ss.pType, 1);
                                        }
                                        else
                                        {
                                            Console.WriteLine($"没有宝石");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("市場沒開放");
                                    }
                                }; break;
                            case "business":
                                {
                                    if (this.Market.business_Price != null)
                                    {
                                        var calValue = this.Market.business_Price.Value;
                                        if (role.PromoteDiamondCount[ss.pType] > 0)
                                        {
                                            role.MoneySet(role.Money + calValue, ref notifyMsg);
                                            role.PromoteDiamondCount[ss.pType]--;
                                            if (role.playerType == RoleInGame.PlayerType.player)
                                                SendPromoteCountOfPlayer(ss.pType, (Player)role, ref notifyMsg);
                                            this.Market.Receive(ss.pType, 1);
                                        }
                                        else
                                        {
                                            Console.WriteLine($"没有宝石");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("市場沒開放");
                                    }
                                }; break;
                            case "volume":
                                {
                                    if (this.Market.volume_Price != null)
                                    {
                                        var calValue = this.Market.volume_Price.Value;
                                        if (role.PromoteDiamondCount[ss.pType] > 0)
                                        {
                                            role.MoneySet(role.Money + calValue, ref notifyMsg);
                                            role.PromoteDiamondCount[ss.pType]--;
                                            if (role.playerType == RoleInGame.PlayerType.player)
                                                SendPromoteCountOfPlayer(ss.pType, (Player)role, ref notifyMsg);
                                            this.Market.Receive(ss.pType, 1);
                                        }
                                        else
                                        {
                                            Console.WriteLine($"没有宝石");
#warning 这里要提示前台
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("市場沒開放");
                                    }
                                }; break;
                            case "speed":
                                {
                                    if (this.Market.speed_Price != null)
                                    {
                                        var calValue = this.Market.speed_Price.Value;
                                        if (role.PromoteDiamondCount[ss.pType] > 0)
                                        {
                                            role.MoneySet(role.Money + calValue, ref notifyMsg);
                                            role.PromoteDiamondCount[ss.pType]--;
                                            if (role.playerType == RoleInGame.PlayerType.player)
                                                SendPromoteCountOfPlayer(ss.pType, (Player)role, ref notifyMsg);
                                            this.Market.Receive(ss.pType, 1);
                                        }
                                        else
                                        {
                                            Console.WriteLine($"没有宝石");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("市場沒開放");
                                    }
                                }; break;
                        }
                    }
                }

            for (var i = 0; i < notifyMsg.Count; i += 2)
            {
                var url = notifyMsg[i];
                var sendMsg = notifyMsg[i + 1];
                Console.WriteLine($"url:{url}");
                Startup.sendMsg(url, sendMsg);
            }
        }



        internal void Buy(SetBuyDiamond bd)
        {
            List<string> notifyMsg = new List<string>();
            lock (this.PlayerLock)
                if (this._Players.ContainsKey(bd.Key))
                {
                    var role = this._Players[bd.Key];
                    if (role.Bust) { }
                    //else if(player.)
                    else
                    {
                        switch (bd.pType)
                        {
                            case "mile":
                                {
                                    if (this.Market.mile_Price != null)
                                    {
                                        var calValue = this.Market.mile_Price.Value;
                                        if (role.Money >= calValue)
                                        {
                                            role.MoneySet(role.Money - calValue, ref notifyMsg);
                                            role.PromoteDiamondCount[bd.pType]++;
                                            if (role.playerType == RoleInGame.PlayerType.player)
                                                SendPromoteCountOfPlayer(bd.pType, (Player)role, ref notifyMsg);
                                            this.Market.Send(bd.pType, 1);
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("市場沒開放");
                                    }
                                }; break;
                            case "business":
                                {
                                    if (this.Market.business_Price != null)
                                    {
                                        var calValue = this.Market.business_Price.Value;
                                        if (role.Money >= calValue)
                                        {
                                            role.MoneySet(role.Money - calValue, ref notifyMsg);
                                            role.PromoteDiamondCount[bd.pType]++;
                                            if (role.playerType == RoleInGame.PlayerType.player)
                                                SendPromoteCountOfPlayer(bd.pType, (Player)role, ref notifyMsg);
                                            this.Market.Send(bd.pType, 1);
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("市場沒開放");
                                    }
                                }; break;
                            case "volume":
                                {
                                    if (this.Market.volume_Price != null)
                                    {
                                        var calValue = this.Market.volume_Price.Value;
                                        if (role.Money >= calValue)
                                        {
                                            role.MoneySet(role.Money - calValue, ref notifyMsg);
                                            role.PromoteDiamondCount[bd.pType]++;
                                            if (role.playerType == RoleInGame.PlayerType.player)
                                                SendPromoteCountOfPlayer(bd.pType, (Player)role, ref notifyMsg);
                                            this.Market.Send(bd.pType, 1);
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("市場沒開放");
                                    }
                                }; break;
                            case "speed":
                                {
                                    if (this.Market.speed_Price != null)
                                    {
                                        var calValue = this.Market.speed_Price.Value;
                                        if (role.Money >= calValue)
                                        {
                                            role.MoneySet(role.Money - calValue, ref notifyMsg);
                                            role.PromoteDiamondCount[bd.pType]++;
                                            if (role.playerType == RoleInGame.PlayerType.player)
                                                SendPromoteCountOfPlayer(bd.pType, (Player)role, ref notifyMsg);
                                            this.Market.Send(bd.pType, 1);
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("市場沒開放");
                                    }
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
}
