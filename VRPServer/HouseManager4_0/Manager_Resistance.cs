using CommonClass;
using HouseManager4_0.RoomMainF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HouseManager4_0
{
    public class Manager_Resistance : Manager
    {
        public Manager_Resistance(RoomMain roomMain)
        {
            this.roomMain = roomMain;
        }

        internal string Display(GetResistanceObj r)
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
                                        KeyLookfor = r.KeyLookfor,

                                        // ss=role.confuseRecord.
                                    };
                                    var url = player.FromUrl;
                                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(rd);
                                    this.sendSingleMsg(url, sendMsg);

                                    var singleName = this.GetSingleName(player);
                                    ParameterToEditPlayerMaterial parameter = new ParameterToEditPlayerMaterial()
                                    {
                                        Key = role.Key,
                                        Driver = role.getCar().ability.driver == null ? -1 : role.getCar().ability.driver.Index,
                                        Relation = rd.Relation,
                                        singleName = singleName
                                    };
                                    return Newtonsoft.Json.JsonConvert.SerializeObject(parameter);
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
                                            KeyLookfor = r.KeyLookfor,
                                            //SpeedValue = role.improvementRecord.speedValue,
                                            //DefenceValue = role.improvementRecord.defenceValue,
                                            //AttackValue = role.improvementRecord.attackValue,
                                            //LoseValue = role.confuseRecord.GetLoseValue(),
                                            //ConfuseValue = role.confuseRecord.GetConfuseValue()
                                        };
                                        var url = player.FromUrl;
                                        var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(rd);
                                        this.sendSingleMsg(url, sendMsg);

                                        var singleName = this.GetSingleName(role);
                                        ParameterToEditPlayerMaterial parameter = new ParameterToEditPlayerMaterial()
                                        {
                                            Key = role.Key,
                                            Driver = role.getCar().ability.driver == null ? -1 : role.getCar().ability.driver.Index,
                                            Relation = rd.Relation,
                                            singleName = singleName
                                        };
                                        return Newtonsoft.Json.JsonConvert.SerializeObject(parameter);
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
                                            DefenseImproveProbability = 0,
                                            SpeedImproveProbability = 0,
                                            ignoreAmbush = 0,
                                            ignoreLose = 0,
                                            ignoreConfuse = 0,
                                            ignorePhysics = 0,
                                            c = "ResistanceDisplay2",
                                            KeyLookfor = r.KeyLookfor,
                                            controlImprove = 0,
                                            WebSocketID = player.WebSocketID,
                                            buildingReward = role.buildingReward,
                                            race = 0,
                                            SpeedValue = role.improvementRecord.speedValue,
                                            DefenceValue = role.improvementRecord.defenceValue,
                                            AttackValue = role.improvementRecord.attackValue,
                                            LoseValue = role.confuseRecord.GetLoseValue(),
                                            ConfuseValue = role.confuseRecord.GetConfuseValue(),
                                            LostPropertyByDefendMagic = Engine_MagicEngine.LostPropertyByDefendMagic,
                                            ConfusePropertyByDefendMagic = Engine_MagicEngine.ConfusePropertyByDefendMagic,
                                            AmbushPropertyByDefendMagic = Engine_MagicEngine.AmbushPropertyByDefendMagic,
                                            DefenceAttackMagicAdd = Engine_MagicEngine.DefenceAttackMagicAdd,
                                            DefencePhysicsAdd = Engine_MagicEngine.DefencePhysicsAdd
                                        };
                                        var url = player.FromUrl;
                                        var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(rd);
                                        this.sendSingleMsg(url, sendMsg);
                                        return "";
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
                                                        defensiveOfPhysics = driver.defensiveOfPhysics,
                                                        recruit = Manager_Driver.GetRecruit(role),
                                                        AttackImproveProbability = Engine_MagicEngine.GetAttackImproveProbability(role),
                                                        AttackImproveValue = Engine_MagicEngine.GetAttackImproveValue(role),
                                                        DefenseImproveProbability = Engine_MagicEngine.GetDefenseImproveProbability(role),
                                                        DefenseImproveValue = Engine_MagicEngine.GetDefenseImproveValue(role),
                                                        SpeedImproveProbability = Engine_MagicEngine.GetSpeedImproveProbability(role),
                                                        SpeedImproveValue = Engine_MagicEngine.GetSpeedImproveValue(role),
                                                        ignoreAmbush = 0,
                                                        ignoreLose = 0,
                                                        ignoreConfuse = 0,
                                                        ignorePhysics = Engine_DebtEngine.attackTool.GetIgnorePhysicsProbability(role),
                                                        c = "ResistanceDisplay2",
                                                        KeyLookfor = r.KeyLookfor,
                                                        WebSocketID = player.WebSocketID,
                                                        controlImprove = 0,
                                                        buildingReward = role.buildingReward,
                                                        race = 1,
                                                        SpeedValue = role.improvementRecord.speedValue,
                                                        DefenceValue = role.improvementRecord.defenceValue,
                                                        AttackValue = role.improvementRecord.attackValue,
                                                        LoseValue = role.confuseRecord.GetLoseValue(),
                                                        ConfuseValue = role.confuseRecord.GetConfuseValue(),
                                                        ignorePhysicsValue = Engine_DebtEngine.attackTool.GetIgnorePhysicsValue(role),
                                                        //IgnoreMagicValue = 0,
                                                        //   IgnoreControlValue = Manager_Model.IgnoreControl,
                                                        LostPropertyByDefendMagic = Engine_MagicEngine.LostPropertyByDefendMagic,
                                                        ConfusePropertyByDefendMagic = Engine_MagicEngine.ConfusePropertyByDefendMagic,
                                                        AmbushPropertyByDefendMagic = Engine_MagicEngine.AmbushPropertyByDefendMagic,
                                                        DefenceAttackMagicAdd = Engine_MagicEngine.DefenceAttackMagicAdd,
                                                        DefencePhysicsAdd = Engine_MagicEngine.DefencePhysicsAdd
                                                    };
                                                    //ignorePhysics
                                                    var url = player.FromUrl;
                                                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(rd);
                                                    this.sendSingleMsg(url, sendMsg);
                                                    return "";
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
                                                        defensiveOfPhysics = driver.defensiveOfPhysics,
                                                        recruit = Manager_Driver.GetRecruit(role),
                                                        AttackImprove = 0,
                                                        DefenseImproveProbability = 0,
                                                        SpeedImproveProbability = 0,
                                                        ignoreAmbush = Engine_MagicEngine.GetAmbushIgnore(role),
                                                        ignoreLose = Engine_MagicEngine.GetLoseIgnore(role),
                                                        ignoreConfuse = Engine_MagicEngine.GetConfuseIgnore(role),
                                                        controlImprove = Engine_MagicEngine.GetMagicDoubleProbability(role),
                                                        ignorePhysics = 0,
                                                        c = "ResistanceDisplay2",
                                                        KeyLookfor = r.KeyLookfor,
                                                        buildingReward = role.buildingReward,
                                                        WebSocketID = player.WebSocketID,
                                                        race = 2,
                                                        SpeedValue = role.improvementRecord.speedValue,
                                                        DefenceValue = role.improvementRecord.defenceValue,
                                                        AttackValue = role.improvementRecord.attackValue,
                                                        LoseValue = role.confuseRecord.GetLoseValue(),
                                                        ConfuseValue = role.confuseRecord.GetConfuseValue(),
                                                        ignorePhysicsValue = Engine_DebtEngine.attackTool.GetIgnorePhysicsValue(role),
                                                        //IgnoreMagicValue = 0,
                                                        //IgnoreControlValue = Manager_Model.IgnoreControl,
                                                        LostPropertyByDefendMagic = Engine_MagicEngine.LostPropertyByDefendMagic,
                                                        ConfusePropertyByDefendMagic = Engine_MagicEngine.ConfusePropertyByDefendMagic,
                                                        AmbushPropertyByDefendMagic = Engine_MagicEngine.AmbushPropertyByDefendMagic,
                                                        DefenceAttackMagicAdd = Engine_MagicEngine.DefenceAttackMagicAdd,
                                                        DefencePhysicsAdd = Engine_MagicEngine.DefencePhysicsAdd
                                                    };
                                                    var url = player.FromUrl;
                                                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(rd);
                                                    this.sendSingleMsg(url, sendMsg);
                                                    return "";
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
                                                        defensiveOfPhysics = driver.defensiveOfPhysics,
                                                        recruit = Manager_Driver.GetRecruit(role),
                                                        AttackImprove = 0,
                                                        DefenseImproveProbability = 0,
                                                        SpeedImproveProbability = 0,
                                                        ignoreAmbush = 0,
                                                        ignoreLose = 0,
                                                        ignoreConfuse = 0,
                                                        controlImprove = 0,
                                                        ignorePhysics = 0,
                                                        magicViolentValue = Engine_MagicEngine.GetAttackMagicImproveValue(role),
                                                        magicViolentProbability = Engine_MagicEngine.GetAttackImproveProbability(role),
                                                        c = "ResistanceDisplay2",
                                                        KeyLookfor = r.KeyLookfor,
                                                        buildingReward = role.buildingReward,
                                                        WebSocketID = player.WebSocketID,
                                                        race = 3,
                                                        SpeedValue = role.improvementRecord.speedValue,
                                                        DefenceValue = role.improvementRecord.defenceValue,
                                                        AttackValue = role.improvementRecord.attackValue,
                                                        LoseValue = role.confuseRecord.GetLoseValue(),
                                                        ConfuseValue = role.confuseRecord.GetConfuseValue(),
                                                        ignorePhysicsValue = Engine_DebtEngine.attackTool.GetIgnorePhysicsValue(role),
                                                        IgnoreElectricMagicValue = Engine_MagicEngine.attackMagicTool.GetIgnoreElectricMagicValue(role),
                                                        IgnoreWaterMagicValue = Engine_MagicEngine.attackMagicTool.GetIgnoreWaterMagicValue(role),
                                                        IgnoreFireMagicValue = Engine_MagicEngine.attackMagicTool.GetIgnoreFireMagicValue(role),
                                                        // IgnoreControlValue = Manager_Model.IgnoreControl,
                                                        LostPropertyByDefendMagic = Engine_MagicEngine.LostPropertyByDefendMagic,
                                                        ConfusePropertyByDefendMagic = Engine_MagicEngine.ConfusePropertyByDefendMagic,
                                                        AmbushPropertyByDefendMagic = Engine_MagicEngine.AmbushPropertyByDefendMagic,
                                                        DefenceAttackMagicAdd = Engine_MagicEngine.DefenceAttackMagicAdd,
                                                        DefencePhysicsAdd = Engine_MagicEngine.DefencePhysicsAdd
                                                    };
                                                    var url = player.FromUrl;
                                                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(rd);
                                                    this.sendSingleMsg(url, sendMsg);
                                                    return "";
                                                };
                                        }
                                    }
                                }
                            }
                        }; break;
                    default: return "";
                }
            }
            return "";
        }

        private string GetSingleName(RoleInGame role)
        {
            Regex reg = new Regex(@"^[\u4e00-\u9fa5]{0,}$");

            Dictionary<string, int> Characters = new Dictionary<string, int>();
            foreach (var item in that._Players)
            {
                if (item.Key == role.Key)
                {
                    continue;
                }
                else
                {
                    var playerName = item.Value.PlayerName;
                    for (int i = 0; i < playerName.Length; i++)
                    {
                        var c = playerName.Substring(i, 1);
                        if (reg.IsMatch(c))
                        {
                            if (Characters.ContainsKey(c))
                            {
                                Characters[c]++;
                            }
                            else
                            {
                                Characters.Add(c, 1);
                            }
                        }
                    }
                }
            }

            int minCount = int.MaxValue;
            string result = "玩";
            {
                var playerName = role.PlayerName;
                for (int i = 0; i < playerName.Length; i++)
                {
                    var c = playerName.Substring(i, 1);
                    if (reg.IsMatch(c))
                    {
                        if (Characters.ContainsKey(c))
                        {
                            if (Characters[c] < minCount)
                            {
                                minCount = Characters[c];
                                result = c;
                            }
                        }
                        else
                        {
                            minCount = 0;
                            result = c;
                        }
                    }
                }
            }
            return result;
            //var singleName = role.PlayerName
            //if (role.PlayerName.Length > 0) { }
            //else return "";
            //List<string> NameChracters = new List<string>();
            //for (int i = 0; i < role.PlayerName.Length; i++)
            //{
            //    NameChracters.Add(role.PlayerName.Substring(i, 1));
            //}
            //foreach (var item in that._Players)
            //{

            //}
            //return "";
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
                var r = this.sendSingleMsg(url, json);
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
