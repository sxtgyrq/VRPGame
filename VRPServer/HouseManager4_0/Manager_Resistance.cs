using CommonClass;
using HouseManager4_0.RoomMainF;
using System;

namespace HouseManager4_0
{
    public class Manager_Resistance : Manager
    {
        public Manager_Resistance(RoomMain roomMain)
        {
            this.roomMain = roomMain;
        }

        internal void Display(GetResistanceObj r)
        {
            switch (r.RequestType)
            {
                case 0:
                    {
                        if (that._Players.ContainsKey(r.key))
                        {
                            var player = (Player)that._Players[r.key];
                            if (r.key == r.KeyLookfor)
                            {
                                RoleInGame boss;
                                string bossKey, bossName;

                                if (that._Players[r.key].HasTheBoss(that._Players, out boss))
                                {
                                    bossKey = boss.Key;
                                    bossName = boss.PlayerName;
                                }
                                else
                                {
                                    bossKey = that._Players[r.KeyLookfor].TheLargestHolderKey;
                                    bossName = that._Players[r.KeyLookfor].PlayerName;
                                }
                                string BTCAddr;
                                var role = that._Players[r.KeyLookfor];
                                if (role.playerType == RoleInGame.PlayerType.player)
                                {
                                    BTCAddr = ((Player)role).BTCAddress;
                                }
                                else
                                {
                                    BTCAddr = "";
                                }
                                // int[] mileCount = new int[2] { 0, 0 };
                                //role.getCar().ability.AbilityAdd
                                ResistanceDisplay rd = new ResistanceDisplay()
                                {
                                    Relation = "自己",
                                    BossKey = bossKey,
                                    BossName = bossName,
                                    BTCAddr = BTCAddr,
                                    Level = role.Level,
                                    Driver = role.getCar().ability.driver == null ? -1 : role.getCar().ability.driver.Index,
                                    DriverName = role.getCar().ability.driver == null ? "" : role.getCar().ability.driver.Name.Trim(),
                                    c = "ResistanceDisplay",
                                    Name = role.PlayerName,
                                    MileCount = role.getCar().ability.getDataCount("mile"),
                                    BusinessCount = role.getCar().ability.getDataCount("business"),
                                    VolumeCount = role.getCar().ability.getDataCount("volume"),
                                    SpeedCount = role.getCar().ability.getDataCount("speed"),
                                    Money = role.Money,
                                    PlayerType = role.playerType.ToString(),
                                    WebSocketID = player.WebSocketID,
                                    Mile = role.getCar().ability.mile,
                                    Business = role.getCar().ability.Business,
                                    Volume = role.getCar().ability.Volume,
                                    Speed = role.getCar().ability.Speed,
                                    OnLineStr = "在线",
                                    KeyLookfor = r.KeyLookfor
                                };
                                var url = player.FromUrl;
                                var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(rd);
                                this.sendMsg(url, sendMsg);
                            }
                        }
                    }; break;
                case 1:
                    {
                        if (that._Players.ContainsKey(r.key))
                        {
                            var player = (Player)that._Players[r.key];
                            if (that._Players.ContainsKey(r.KeyLookfor))
                            {
                                var role = that._Players[r.KeyLookfor];
                                var driver = role.getCar().ability.driver;
                                //  role.getCar.
                            }

                            if (r.key == r.KeyLookfor)
                            {
                                RoleInGame boss;
                                string bossKey, bossName;

                                if (that._Players[r.key].HasTheBoss(that._Players, out boss))
                                {
                                    bossKey = boss.Key;
                                    bossName = boss.PlayerName;
                                }
                                else
                                {
                                    bossKey = that._Players[r.KeyLookfor].TheLargestHolderKey;
                                    bossName = that._Players[r.KeyLookfor].PlayerName;
                                }
                                string BTCAddr;
                                var role = that._Players[r.KeyLookfor];
                                if (role.playerType == RoleInGame.PlayerType.player)
                                {
                                    BTCAddr = ((Player)role).BTCAddress;
                                }
                                else
                                {
                                    BTCAddr = "";
                                }
                                // int[] mileCount = new int[2] { 0, 0 };
                                //role.getCar().ability.AbilityAdd
                                ResistanceDisplay rd = new ResistanceDisplay()
                                {
                                    Relation = "自己",
                                    BossKey = bossKey,
                                    BossName = bossName,
                                    BTCAddr = BTCAddr,
                                    Level = role.Level,
                                    Driver = role.getCar().ability.driver == null ? -1 : role.getCar().ability.driver.Index,
                                    DriverName = role.getCar().ability.driver == null ? "" : role.getCar().ability.driver.Name.Trim(),
                                    c = "ResistanceDisplay",
                                    Name = role.PlayerName,
                                    MileCount = role.getCar().ability.getDataCount("mile"),
                                    BusinessCount = role.getCar().ability.getDataCount("business"),
                                    VolumeCount = role.getCar().ability.getDataCount("volume"),
                                    SpeedCount = role.getCar().ability.getDataCount("speed"),
                                    Money = role.Money,
                                    PlayerType = role.playerType.ToString(),
                                    WebSocketID = player.WebSocketID,
                                    Mile = role.getCar().ability.mile,
                                    Business = role.getCar().ability.Business,
                                    Volume = role.getCar().ability.Volume,
                                    Speed = role.getCar().ability.Speed,
                                    OnLineStr = "在线",
                                    KeyLookfor = r.KeyLookfor
                                };
                                var url = player.FromUrl;
                                var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(rd);
                                this.sendMsg(url, sendMsg);
                            }
                        }
                    }; break;
            }
        }
    }

    public class Manager_Connection : Manager
    {
        public Manager_Connection(RoomMain roomMain)
        {
            this.roomMain = roomMain;
        }

        internal bool IsOnline(Player player)
        {
            var obj = new CommandNotify()
            {
                c = "WhetherOnLine",
                WebSocketID = player.WebSocketID,
            };
            var url = player.FromUrl;
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            var r = this.sendMsg(url, json);
            if (r == "on")
            {
                return true;
            }
            else if (r == "off")
            {
                return false;
            }
            else
            {
                throw new Exception("");
            }
        }
    }
}
