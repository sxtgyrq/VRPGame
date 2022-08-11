﻿using CommonClass;
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
            lock (that.PlayerLock)
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
                                else
                                {
                                    if (that._Players.ContainsKey(r.KeyLookfor))
                                    {
                                        var role = that._Players[r.KeyLookfor];
                                        RoleInGame boss;
                                        string bossKey, bossName;

                                        if (role.HasTheBoss(that._Players, out boss))
                                        {
                                            bossKey = boss.Key;
                                            bossName = boss.PlayerName;
                                        }
                                        else
                                        {
                                            bossKey = role.TheLargestHolderKey;
                                            bossName = role.PlayerName;
                                        }
                                        string BTCAddr, Relation, OnLineStr;
                                        if (role.playerType == RoleInGame.PlayerType.player)
                                        {
                                            BTCAddr = ((Player)role).BTCAddress;
                                            if (role.Key == player.TheLargestHolderKey)
                                            {
                                                Relation = "老大";
                                            }
                                            else if (role.TheLargestHolderKey == player.TheLargestHolderKey)
                                            {
                                                Relation = "队友";
                                            }
                                            else
                                            {
                                                Relation = "玩家";
                                            }
                                            if (((Player)role).IsOnline())
                                            {
                                                OnLineStr = "在线";
                                            }
                                            else
                                            {
                                                OnLineStr = "离线";
                                            }
                                        }
                                        else
                                        {
                                            BTCAddr = "";
                                            Relation = "NPC";
                                            OnLineStr = "在线";
                                        }
                                        // int[] mileCount = new int[2] { 0, 0 };
                                        //role.getCar().ability.AbilityAdd
                                        ResistanceDisplay rd = new ResistanceDisplay()
                                        {
                                            Relation = Relation,
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
                                            OnLineStr = OnLineStr,
                                            KeyLookfor = r.KeyLookfor
                                        };
                                        var url = player.FromUrl;
                                        var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(rd);
                                        this.sendMsg(url, sendMsg);

                                    }
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
                                    //if(role)
                                    if (driver == null)
                                    {
                                        ResistanceDisplay2 rd = new ResistanceDisplay2()
                                        {
                                            defensiveOfAmbush = 0,
                                            defensiveOfConfuse = 0,
                                            defensiveOfLose = 0,
                                            defensiveOfElectic = 0,
                                            defensiveOfFire = 0,
                                            defensiveOfWater = 0,
                                            defensiveOfPhysics = 0,
                                            recruit = 100,
                                            AttackImprove = 0,
                                            DefenseImprove = 0,
                                            SpeedImprove = 0,
                                            ignoreAmbush = 0,
                                            ignoreLose = 0,
                                            ignoreConfuse = 0,
                                            ignoreElectic = 0,
                                            ignoreFire = 0,
                                            ignoreOfWater = 0,
                                            ignorePhysics = 0,
                                            magicViolent = 0,
                                            c = "ResistanceDisplay2",
                                            KeyLookfor = r.KeyLookfor,
                                            controlImprove = 0,
                                            WebSocketID = player.WebSocketID,
                                            buildingReward = role.buildingReward,
                                            race = 0
                                        };
                                        var url = player.FromUrl;
                                        var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(rd);
                                        this.sendMsg(url, sendMsg);
                                        return;
                                    }
                                    else
                                    {
                                        switch (driver.race)
                                        {
                                            case CommonClass.driversource.Race.devil:
                                                {
                                                    ResistanceDisplay2 rd = new ResistanceDisplay2()
                                                    {
                                                        defensiveOfAmbush = driver.defensiveOfAmbush,
                                                        defensiveOfConfuse = driver.defensiveOfConfuse,
                                                        defensiveOfLose = driver.defensiveOfLose,
                                                        defensiveOfElectic = driver.defensiveOfElectic,
                                                        defensiveOfFire = driver.defensiveOfFire,
                                                        defensiveOfWater = driver.defensiveOfWater,
                                                        defensiveOfPhysics = driver.defensiveOfWater,
                                                        recruit = that.driverM.GetRecruit(role.buildingReward[0]),
                                                        AttackImprove = that.magicE.GetAttackImprove(role),
                                                        DefenseImprove = that.magicE.GetDefenseImprove(role),
                                                        SpeedImprove = that.magicE.GetSpeedImprove(role),
                                                        ignoreAmbush = 0,
                                                        ignoreLose = 0,
                                                        ignoreConfuse = 0,
                                                        ignoreElectic = 0,
                                                        ignoreFire = 0,
                                                        ignoreOfWater = 0,
                                                        ignorePhysics = that.magicE.GetIgnorePhysics(role),
                                                        magicViolent = 0,
                                                        c = "ResistanceDisplay2",
                                                        KeyLookfor = r.KeyLookfor,
                                                        WebSocketID = player.WebSocketID,
                                                        controlImprove = 0,
                                                        buildingReward = role.buildingReward,
                                                        race = 1
                                                    };
                                                    var url = player.FromUrl;
                                                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(rd);
                                                    this.sendMsg(url, sendMsg);
                                                    return;
                                                };
                                            case CommonClass.driversource.Race.people:
                                                {
                                                    ResistanceDisplay2 rd = new ResistanceDisplay2()
                                                    {
                                                        defensiveOfAmbush = driver.defensiveOfAmbush,
                                                        defensiveOfConfuse = driver.defensiveOfConfuse,
                                                        defensiveOfLose = driver.defensiveOfLose,
                                                        defensiveOfElectic = driver.defensiveOfElectic,
                                                        defensiveOfFire = driver.defensiveOfFire,
                                                        defensiveOfWater = driver.defensiveOfWater,
                                                        defensiveOfPhysics = driver.defensiveOfWater,
                                                        recruit = that.driverM.GetRecruit(role.buildingReward[0]),
                                                        AttackImprove = 0,
                                                        DefenseImprove = 0,
                                                        SpeedImprove = 0,
                                                        ignoreAmbush = that.magicE.GetAmbushImprove(role),
                                                        ignoreLose = that.magicE.GetLoseIgnore(role),
                                                        ignoreConfuse = that.magicE.GetConfuseIgnore(role),
                                                        controlImprove = that.magicE.GetControlImprove(role),
                                                        ignoreElectic = 0,
                                                        ignoreFire = 0,
                                                        ignoreOfWater = 0,
                                                        ignorePhysics = 0,
                                                        magicViolent = 0,
                                                        c = "ResistanceDisplay2",
                                                        KeyLookfor = r.KeyLookfor,
                                                        buildingReward = role.buildingReward,
                                                        WebSocketID = player.WebSocketID,
                                                        race = 2
                                                    };
                                                    var url = player.FromUrl;
                                                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(rd);
                                                    this.sendMsg(url, sendMsg);
                                                    return;
                                                };
                                            case CommonClass.driversource.Race.immortal:
                                                {
                                                    ResistanceDisplay2 rd = new ResistanceDisplay2()
                                                    {
                                                        defensiveOfAmbush = driver.defensiveOfAmbush,
                                                        defensiveOfConfuse = driver.defensiveOfConfuse,
                                                        defensiveOfLose = driver.defensiveOfLose,
                                                        defensiveOfElectic = driver.defensiveOfElectic,
                                                        defensiveOfFire = driver.defensiveOfFire,
                                                        defensiveOfWater = driver.defensiveOfWater,
                                                        defensiveOfPhysics = driver.defensiveOfWater,
                                                        recruit = that.driverM.GetRecruit(role.buildingReward[0]),
                                                        AttackImprove = 0,
                                                        DefenseImprove = 0,
                                                        SpeedImprove = 0,
                                                        ignoreAmbush = 0,
                                                        ignoreLose = 0,
                                                        ignoreConfuse = 0,
                                                        controlImprove = 0,
                                                        ignoreElectic = that.magicE.GetIgnoreElectic(role),
                                                        ignoreFire = that.magicE.GetIgnoreFire(role),
                                                        ignoreOfWater = that.magicE.GetIgnoreWater(role),
                                                        ignorePhysics = 0,
                                                        magicViolent = that.magicE.GetMagicViolent(role),
                                                        c = "ResistanceDisplay2",
                                                        KeyLookfor = r.KeyLookfor,
                                                        buildingReward = role.buildingReward,
                                                        WebSocketID = player.WebSocketID,
                                                        race = 3
                                                    };
                                                    var url = player.FromUrl;
                                                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(rd);
                                                    this.sendMsg(url, sendMsg);
                                                    return;
                                                };
                                        }
                                    }
                                }
                            }
                        }; break;
                    default: return;
                }
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
            try
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
            catch
            {
                return false;
            }
        }
    }
}
