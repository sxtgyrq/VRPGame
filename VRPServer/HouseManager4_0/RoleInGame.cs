using CommonClass;
using CommonClass.databaseModel;
using Google.Protobuf.WellKnownTypes;
using HouseManager4_0.interfaceOfEngine;
using HouseManager4_0.RoomMainF;
using Model;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager4_0
{
    public abstract class RoleInGame : interfaceOfHM.GetFPIndex
    {
        public RoomMain rm;

        public delegate void ShowLevelOfPlayer(Player player, int level, ref List<string> notifyMsg);
        public ShowLevelOfPlayer ShowLevelOfPlayerF = null;
        public void ShowLevelOfPlayerDetail(ref List<string> notifyMsg)
        {
            if (this.playerType == PlayerType.player && this.ShowLevelOfPlayerF != null)
                this.ShowLevelOfPlayerF((Player)this, this.levelObj.Level, ref notifyMsg);
        }
        LevelObj _level = new LevelObj();

        public class LevelObj : CommonClass.databaseModel.LevelForSave
        {
            // public bool InsertToSave { get; internal set; }

            public LevelObj()
            {
                this.BtcAddr = "";
                this.Level = 1;
                this.TimeStampStr = "";
                //   this.InsertToSave = true;
            }

            internal void SetLevel(int newLevel)
            {
                this.Level = newLevel;
            }

            /// <summary>
            /// 地址只能使用一次
            /// </summary>
            /// <param name="btcAddr"></param>
            internal void SetAddr(string btcAddr)
            {
                if (this.BtcAddr == "")
                    this.BtcAddr = btcAddr;
            }

            internal void SetTimeStamp(string timeStr)
            {
                /*
                 * 此处用正则表达式限制，是为了启动一下作用
                 * this.TimeStampStr赋值后，不可能在改回空字符串。
                 * this.TimeStampStr 在空字符串时，代表未同步。
                 * this.TimeStampStr 非空字符串是，代表等级数据是聪数据库获取。
                 */
                var regex = new System.Text.RegularExpressions.Regex("^[0-9]{18}$");
                if (regex.IsMatch(timeStr))
                    this.TimeStampStr = timeStr;
            }
        }
        public LevelObj levelObj
        {
            get
            {
                return this._level;
            }
        }
        public int Level
        {
            get
            {
                return this.levelObj.Level;
            }
        }
        public void SetLevel(int newLevel, ref List<string> notifyMsg)
        {
            if (this._level.Level == newLevel)
            {

            }
            else
            {
                this._level.SetLevel(newLevel);
                ShowLevelOfPlayerDetail(ref notifyMsg);
                if (this.playerType == PlayerType.NPC)
                {
                    ((NPC)this).initializeCarOfNPC();
                }
            }
            //if (this._level == newLevel) { }
            //else
            //{
            //    this._level = newLevel;
            //    ShowLevelOfPlayerDetail(ref notifyMsg);

            //}
        }
        public string Key { get; internal set; }

        public string PlayerName { get; internal set; }
        public DateTime CreateTime { get; internal set; }
        public DateTime ActiveTime { get; internal set; }
        public int StartFPIndex { get; internal set; }
        public Car getCar()
        {
            return this._Car;
        }
        /// <summary>
        /// 有0，1，2，3，4  五个选项，代表不同的位置
        /// </summary>
        public int positionInStation = 0;

        protected Car _Car;
        /// <summary>
        /// 玩家初始携带金额，单位分。
        /// </summary>
        const long intializedMoney = 50000;
        internal void initializeCar(interfaceOfHM.CarAndRoomInterface roomMain, interfaceOfHM.Car cafF)
        {
            this._Car = new Car(this);
            //{
            //    ability = new AbilityAndState(),
            //    targetFpIndex = -1,

            //};
            var notifyMsg = new List<string>();
            this._Car.SendStateAndPurpose = cafF.SendStateOfCar;
            this._Car.setState(this, ref notifyMsg, Car.CarState.waitAtBaseStation);
            //this._Car.SendPurposeOfCar = RoomMainF.RoomMain.SendPurposeOfCar;

            this._Car.setState(this, ref notifyMsg, Car.CarState.waitAtBaseStation);

            // this._Car.SendPurposeOfCar = RoomMainF.RoomMain.SendPurposeOfCar;
            // this._Car.setPurpose(this, ref notifyMsg, Car.Purpose.@null);

            this._Car.SetAnimateChanged = roomMain.SetAnimateChanged;
            this._Car.setAnimateData(this, ref notifyMsg, null, DateTime.Now);

            this._Car.ability.MileChanged = roomMain.AbilityChanged2_0;
            this._Car.ability.BusinessChanged = roomMain.AbilityChanged2_0;
            this._Car.ability.VolumeChanged = roomMain.AbilityChanged2_0;
            this._Car.ability.SpeedChanged = roomMain.AbilityChanged2_0;

            this._Car.ability.driverSelected = roomMain.DriverSelected;

            // car.ability.SubsidizeChanged = RoomMain.SubsidizeChanged;
            this._Car.ability.DiamondInCarChanged = roomMain.DiamondInCarChanged;


            this._Money = intializedMoney;
            // this.Money = intializedMoney;
            this.SupportToPlay = null;

        }

        internal void initializeOthers()
        {
            this.others = new Dictionary<string, OtherPlayers>();
        }

        /// <summary>
        /// 获取别的玩家
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public OtherPlayers GetOthers(string key)
        {
            return this.others[key];
        }

        Dictionary<string, OtherPlayers> others = new Dictionary<string, OtherPlayers>();

        public bool othersContainsKey(string key)
        {
            return this.others.ContainsKey(key);
        }
        internal void othersAdd(string key, OtherPlayers otherPlayer)
        {
            this.others.Add(key, otherPlayer);
        }
        // public delegate void SysRemovePlayerByKey(string key, ref List<string> msgsWithUrl);
        // public SysRemovePlayerByKey SysRemovePlayerByKeyF;

        internal void othersRemove(string key, ref List<string> notifyMsg)
        {
            if (this.others.ContainsKey(key))
            {
                //  SysRemovePlayerByKeyF(key, ref notifyMsg);
                if (this.playerType == PlayerType.player)
                {
                    ((Player)this).PlayerOthersRemove(key, ref notifyMsg);
                }
                this.others.Remove(key);
            }
        }




        public Dictionary<int, int> CollectPosition { get; internal set; }

        // Dictionary<int, int> _collectPosition = new Dictionary<int, int>();



        long _Money = 0;

        //internal void InitializeDebt()
        //{
        //    this.Debts = new Dictionary<string, long>();
        //    //throw new NotImplementedException();
        //}



        /// <summary>
        /// 单位是分
        /// </summary>
        public long Money
        {
            get
            {
                return this._Money;
            }
            //set
            //{
            //    if (value < 0)
            //    {
            //        throw new Exception("金钱怎么能负，为何不做判断？");
            //    }
            //    this._Money = value;
            //}
        }

        //public delegate void TheLargestHolderKeyChangedF(string keyFrom, string keyTo, string roleKey, ref List<string> notifyMsg);



        // public TheLargestHolderKeyChangedF TheLargestHolderKeyChanged;



        string _theLargestHolderKey = "";
        /// <summary>
        /// 最大股东,收三个因素印象，钱，债，比值（责任）
        /// </summary>
        public string TheLargestHolderKey
        {

            get
            {
                if (this.playerType == PlayerType.player)
                {
                    if (string.IsNullOrEmpty(_theLargestHolderKey))
                    {

                    }
                    else if (this.rm._Players.ContainsKey(_theLargestHolderKey))
                    {
                        var boss = this.rm._Players[_theLargestHolderKey];
                        {
                            var playerBoss = (Player)boss;
                            if (playerBoss.Bust)
                            {
                            }
                            else if (playerBoss._theLargestHolderKey == playerBoss.Key)
                            {
                                return this._theLargestHolderKey;
                            }
                            else
                            {
                            }
                        }
                    }
                    this._theLargestHolderKey = this.Key;
                    return this.Key;
                }
                else if (this.playerType == PlayerType.NPC)
                {
                    if (!string.IsNullOrEmpty(_theLargestHolderKey))
                    {
                        return _theLargestHolderKey;
                    }
                    this._theLargestHolderKey = this.Key;
                    return this.Key;
                }
                else return this.Key;
            }
        }



        //ref notifyMsgs
        internal void InitializeTheLargestHolder(ref List<string> notifyMsg)
        {
            this._theLargestHolderKey = this.Key;
            if (this.playerType == PlayerType.player)
            {
                var player = (Player)this;
                player.ValueChanged(ref notifyMsg);

            }
        }
        internal void InitializeTheLargestHolder()
        {
            this._theLargestHolderKey = this.Key;
        }

        internal void SetTheLargestHolder(RoleInGame boss, ref List<string> notifyMsg)
        {
            var child = new List<Player>();
            SetTheLargestHolder(boss, ref notifyMsg, out child);
        }

        /// <summary>
        /// 设置老大。
        /// </summary>
        /// <param name="boss"></param>
        internal void SetTheLargestHolder(RoleInGame boss, ref List<string> notifyMsg, out List<Player> child)
        {
            child = new List<Player>();
            foreach (var item in Program.rm._Players)
            {
                if (item.Value.Key != this.Key && item.Value.TheLargestHolderKey == this.Key && item.Value.playerType == PlayerType.player)
                {
                    child.Add((Player)item.Value);
                }
            }
            this._theLargestHolderKey = boss.Key;
            if (this.playerType == PlayerType.player)
            {
                ((Player)this).ValueChanged(ref notifyMsg);
                for (int i = 0; i < child.Count; i++)
                {
                    child[i].InitializeTheLargestHolder(ref notifyMsg);
                }
            }

        }

        public bool HasTheBoss(Dictionary<string, RoleInGame> _Players, out RoleInGame boss)
        {
            if (this.confuseRecord.IsBeingControlled())
            {
                boss = this.confuseRecord.getBoss();
                return true;
            }
            else if (this.TheLargestHolderKey == this.Key)
            {
                boss = null;
                return false;
            }
            else
            {
                if (string.IsNullOrEmpty(this.TheLargestHolderKey))
                {
                    throw new Exception("InitializeTheLargestHolder() 还没有运行！");
                }
                if (_Players.ContainsKey(this.TheLargestHolderKey))
                {
                    boss = _Players[this.TheLargestHolderKey];
                    if (boss.Bust)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    boss = null;
                    return false;
                }
            }
        }

        public void MoneySet(long value, ref List<string> notifyMsg)
        {
            if (value < 0)
            {
                throw new Exception("金钱怎么能负，为何不做判断？");
            }
            this._Money = value;
            if (this.playerType == PlayerType.player)
                ((Player)this).MoneyChanged((Player)this, this.Money, ref notifyMsg);
            if (this.playerType == PlayerType.player)
                ((Player)this).SetMoneyCanSave((Player)this, ref notifyMsg);

            // GetTheLargestHolderKey(ref notifyMsg);
        }


        /// <summary>
        /// 可用于攻击的金钱。
        /// </summary>
        public long MoneyToAttack
        {
            get
            {
                return this.LastMoneyCanUseForAttack;
            }
        }



        /// <summary>
        /// 可用于购买能力宝石的钱，总钱-总债-系统扶持+外部扶持为可用户购买宝石的钱！
        /// </summary>
        public long MoneyToPromote
        {
            get
            {
                //总钱+外部扶持为可用户购买宝石的钱！
                return this.Money;// + (this.SupportToPlay == null ? 0 : this.SupportToPlay.Money);
            }
        }

        /// <summary>
        /// 表征玩家玩耍是不是有外部支持！
        /// </summary>
        Support SupportToPlay { get; set; }
        public class Support
        {
            //long _Money = 0;
            ///// <summary>
            ///// 单位是分
            ///// </summary>
            //public long Money
            //{
            //    get
            //    {
            //        return this._Money;
            //    }
            //    set
            //    {
            //        if (value < 0)
            //        {
            //            throw new Exception("金钱怎么能负，为何不做判断？");
            //        }
            //        this._Money = value;
            //    }
            //}
        }



        long LastMoneyCanUseForAttack
        {
            get
            {
                return this.Money;
                //if (this.Money > 0)
                //{
                //    return Math.Max(1, this.Money - this.sumDebets * brokenParameterT1 / brokenParameterT2);
                //}
                //else if (this.Money == 0)
                //    return this.Money;//;Math.Max(0, this.Money - this.sumDebets * brokenParameterT1 / brokenParameterT2);
                //else
                //{
                //    throw new Exception("");
                //}
            }

            //get { return  }
        }

        /// <summary>
        /// 表征玩家已破产。已破产后，系统接管玩家进行还债。玩家的操作权被收回。
        /// </summary>
        public bool Bust
        {
            get; private set;
        }
        public delegate void BustChanged(RoleInGame player, bool bustValue, ref List<string> msgsWithUrl);
        public BustChanged BustChangedF;
        internal void SetBust(bool v, ref List<string> notifyMsg)
        {
            if (v)
            {
                if (this.playerType == PlayerType.NPC)
                {
                    ((NPC)this).afterBrokeM(ref notifyMsg);
                }
                else if (this.playerType == PlayerType.player)
                {

                    ((Player)this).beforeBrokeM(ref notifyMsg);
                }
            }
            this.Bust = v;
            BustChangedF(this, this.Bust, ref notifyMsg);
            //if (this.Bust)


        }












        DateTime _BustTime { get; set; }
        public DateTime BustTime
        {
            get
            {
                if (this.Bust)
                {
                    return this._BustTime;
                }
                else
                {
                    return DateTime.Now.AddDays(1);
                }
            }
            set
            {
                if (this.Bust)
                {
                    this._BustTime = DateTime.Now;
                }
                else
                {
                    this._BustTime = DateTime.Now.AddDays(1);
                }
            }
        }







        public long MoneyForSave
        {
            get
            {
                if (this.Money - 50000 < 0)
                {
                    return 0;
                }
                else
                {
                    return this.Money - 50000;
                }
            }
        }



        public RoomMainF.RoomMain.commandWithTime.ReturningOjb returningOjb { get; set; }


        /// <summary>
        /// 当小车执行完宝石获取任务，回到基地后。用相应增加。
        /// </summary>
        public Dictionary<string, int> PromoteDiamondCount { get; set; }



        public delegate void DrawSingleRoad(Player player, string roadCode, ref List<string> notifyMsg);
        public DrawSingleRoad DrawSingleRoadF;
        Dictionary<string, bool> usedRoad = new Dictionary<string, bool>();



        internal void addUsedRoad(string roadCode, ref List<string> notifyMsgs)
        {
            if (this.playerType == PlayerType.player)
                if (this.usedRoad.ContainsKey(roadCode)) { }
                else
                {
                    this.usedRoad.Add(roadCode, true);
                    this.DrawSingleRoadF((Player)this, roadCode, ref notifyMsgs);
                }
        }

        //private List<Data.detailmodel> getMaterialNearby(Dictionary<int, SaveRoad.RoadInfo> roads, List<Data.detailmodel> models)
        //{
        //    //List<Data.detailmodel> result = new List<Data.detailmodel>();
        //    //foreach (var road in roads)
        //    //{
        //    //    var v = road.Value;
        //    //    //
        //    //    MaterialNearby mn = new MaterialNearby(v.startLongitude, v.startLatitude, v.endLongitude, v.endLatitude);

        //    //    foreach (var model in models)
        //    //    {
        //    //        if (mn.Contain(model.lon, model.lat))
        //    //        {
        //    //            result.Add(model);
        //    //        }
        //    //    }
        //    //}
        //    //return result;
        //}
        //class MaterialNearby
        //{
        //    double startLongitude { get; set; }
        //    double startLatitude { get; set; }
        //    double endLongitude { get; set; }
        //    double endLatitude { get; set; }


        //    double length { get; set; }

        //    System.Numerics.Complex c1;
        //    System.Numerics.Complex c2;
        //    System.Numerics.Complex c3;
        //    System.Numerics.Complex c4;

        //    double D1;
        //    double D2;
        //    double D3;
        //    double D4;

        //    double c1Longitude { get; set; }
        //    double c1Latitude { get; set; }

        //    double c2Longitude { get; set; }
        //    double c2Latitude { get; set; }

        //    const double limitedLength = 0.02;//道路两侧将近2km，实际宽度为4km
        //    public MaterialNearby(double startLongitude, double startLatitude, double endLongitude, double endLatitude)
        //    {
        //        this.startLongitude = startLongitude;
        //        this.startLatitude = startLatitude;
        //        this.endLongitude = endLongitude;
        //        this.endLatitude = endLatitude;
        //        var v = this;
        //        this.length = Math.Sqrt((v.endLongitude - v.startLongitude) * (v.endLongitude - v.startLongitude) + (v.endLatitude - v.startLatitude) * (v.endLatitude - v.startLatitude));

        //        if (this.length >= 1e-8)
        //        {
        //            System.Numerics.Complex c0 = new System.Numerics.Complex((v.endLongitude - v.startLongitude) / length, (v.endLatitude - v.startLatitude) / length);
        //            c1 = c0 * new System.Numerics.Complex(0, 1);
        //            c2 = c1 * new System.Numerics.Complex(0, 1);
        //            c3 = c2 * new System.Numerics.Complex(0, 1);
        //            c4 = c3 * new System.Numerics.Complex(0, 1);

        //            c1Longitude = this.endLongitude + limitedLength * (c4 + c1).Real;
        //            c1Latitude = this.endLatitude + limitedLength * (c4 + c1).Imaginary;

        //            c2Longitude = this.startLongitude + limitedLength * (c2 + c3).Real;
        //            c2Latitude = this.startLatitude + limitedLength * (c2 + c3).Imaginary;
        //            //
        //            //一个虚数 a+bi
        //            //其对应的斜率为b/a 那么其直线为 y=b/ax+d;
        //            //D=-(ay-bx),实现ay-bx+D=0;
        //            // a b
        //            // x y



        //            D1 = -(c1Latitude * c1.Real - c1Longitude * c1.Imaginary);
        //            D2 = -(c1Latitude * c2.Real - c1Longitude * c2.Imaginary);
        //            D3 = -(c2Latitude * c3.Real - c2Longitude * c3.Imaginary);
        //            D4 = -(c2Latitude * c4.Real - c2Longitude * c4.Imaginary);
        //        }
        //    }

        //    internal bool Contain(double lon, double lat)
        //    {
        //        if (this.length <= 1e-8)
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            return
        //             lat * this.c1.Real - lon * this.c1.Imaginary + this.D1 > 0 &&
        //             lat * this.c2.Real - lon * this.c2.Imaginary + this.D2 > 0 &&
        //             lat * this.c3.Real - lon * this.c3.Imaginary + this.D3 > 0 &&
        //             lat * this.c4.Real - lon * this.c4.Imaginary + this.D4 > 0;
        //        }
        //    }
        //}

        //private object getMaterialNearby(Dictionary<int, SaveRoad.RoadInfo> roads, Dictionary<string, detailmodel> models)
        //{
        //    foreach (var road in roads)
        //    {
        //        //road.Value.
        //    }
        //    // throw new NotImplementedException();
        //}

        internal void clearUsedRoad()
        {
            this.usedRoad.Clear();
            var roadCode = Program.dt.GetFpByIndex(this.StartFPIndex).RoadCode;
            this.usedRoad.Add(roadCode, true);
        }

        public List<string> usedRoadsList
        {
            get
            {
                var result = new List<string>();
                foreach (var item in this.usedRoad)
                {
                    result.Add(item.Key);
                }
                return result;
            }
        }

        public const long ShareBaseValue = 10000;


        public enum PlayerType { NPC, player }
        public PlayerType playerType { get; private set; }
        public void setType(PlayerType t)
        {
            this.playerType = t;
        }

        public int GetFPIndex()
        {
            return this.StartFPIndex;
            // throw new NotImplementedException();
        }

        /// <summary>
        /// 混乱记录器，主要用于被混乱释放的对象
        /// </summary>
        public Manager_Driver.ConfuseManger confuseRecord;


        public Manager_Driver.ImproveManager improvementRecord;
        ///// <summary>
        ///// 混乱记录器，主要用于主动混乱
        ///// </summary>
        //public Manager_Driver.ConfuseManger confuseUsing = null;
        public Engine_MagicEngine.SpeedMagicChanged speedMagicChanged;
        public Engine_MagicEngine.AttackMagicChanged attackMagicChanged;
        public Engine_MagicEngine.DefenceMagicChanged defenceMagicChanged;

        public Engine_MagicEngine.ConfusePrepareMagicChanged confusePrepareMagicChanged;
        public Engine_MagicEngine.LostPrepareMagicChanged lostPrepareMagicChanged;
        public Engine_MagicEngine.AmbushPrepareMagicChanged ambushPrepareMagicChanged;
        public Engine_MagicEngine.ControlPrepareMagicChanged controlPrepareMagicChanged;

        public Engine_MagicEngine.ConfuseMagicChanged confuseMagicChanged;
        public Engine_MagicEngine.ConfuseMagicChanged loseMagicChanged;

        public Engine_MagicEngine.FireMagicChanged fireMagicChanged;
        public Engine_MagicEngine.WaterMagicChanged waterMagicChanged;
        public Engine_MagicEngine.ElectricMagicChanged electricMagicChanged;
        // fireMagicChanged

        public Dictionary<int, int> buildingReward { get; set; }

        /// <summary>
        /// 玩家是否可以祈福！
        /// </summary>
        public bool canGetReward { get; internal set; }
    }
    public class Player : RoleInGame, interfaceTag.HasContactInfo
    {
        /// <summary>
        /// 能力提升宝石的状态，用于前台刷新
        /// </summary>
        public Dictionary<string, int> PromoteState { get; set; }
        public string FromUrl { get; internal set; }
        public int WebSocketID { get; internal set; }

        public void GetUrlAndWebsocket(out string fromUrl, out int websocketID)
        {
            fromUrl = this.FromUrl;
            websocketID = this.WebSocketID;
        }


        internal void PlayerOthersRemove(string key, ref List<string> notifyMsg)
        {
            var url = this.FromUrl;

            OthersRemove or = new OthersRemove()
            {
                c = "OthersRemove",
                WebSocketID = this.WebSocketID,
                othersKey = key
            };

            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(or);
            notifyMsg.Add(url);
            notifyMsg.Add(sendMsg);
        }
        public delegate void SetMoneyCanSaveF(Player player, ref List<string> notifyMsg);
        public SetMoneyCanSaveF SetMoneyCanSave;

        public delegate void MoneyChangedF(Player player, long money, ref List<string> msgsWithUrl);



        public MoneyChangedF MoneyChanged;
        /// <summary>
        /// 用于表征，玩家是第一次打开还是第二次打开。
        /// </summary>
        public int OpenMore { get; set; }

        public delegate void PlayerOperateF(Player player, ref List<string> notifyMsgs);

        public PlayerOperateF beforeBroke;
        public void beforeBrokeM(ref List<string> notifyMsg)
        {
            //   this.InitializeTheLargestHolder(ref notifyMsg);
            this.beforeBroke(this, ref notifyMsg);
        }

        internal void SendBG(Player player, ref List<string> notifyMsg, string crossKey, string Md5)
        {
            SetCrossBG obj;
            if (backgroundData.ContainsKey(crossKey))
            {
                obj = new SetCrossBG
                {
                    c = "SetCrossBG",
                    WebSocketID = player.WebSocketID,
                    CrossID = crossKey,
                    //nx = null,
                    //ny = null,
                    //nz = null,
                    //px = null,
                    //py = null,
                    //pz = null,
                    IsDetalt = false,
                    Md5Key = Md5,
                    AddNew = false
                };
            }
            else
            {
                obj = new SetCrossBG
                {
                    c = "SetCrossBG",
                    WebSocketID = player.WebSocketID,
                    CrossID = crossKey,
                    //nx = Data["nx"],
                    //ny = Data["ny"],
                    //nz = Data["nz"],
                    //px = Data["px"],
                    //py = Data["py"],
                    //pz = Data["pz"],
                    IsDetalt = false,
                    Md5Key = Md5,
                    AddNew = true
                };
                backgroundData.Add(crossKey, true);
            }
            var url = player.FromUrl;
            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            notifyMsg.Add(url);
            notifyMsg.Add(sendMsg);
        }

        internal void SendBG(Player player, ref List<string> notifyMsg)
        {
            SetCrossBG obj;
            obj = new SetCrossBG
            {
                c = "SetCrossBG",
                WebSocketID = player.WebSocketID,
                CrossID = "",
                //nx = null,
                //ny = null,
                //nz = null,
                //px = null,
                //py = null,
                //pz = null,
                IsDetalt = true,
                //  AddNew = false
            };
            var url = player.FromUrl;
            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            notifyMsg.Add(url);
            notifyMsg.Add(sendMsg);
        }

        public System.Numerics.Complex direciton = 0;

        public Dictionary<string, bool> modelHasShowed { get; set; }
        //public Dictionary<string, bool> aModelHasShowed { get; set; }

        public delegate void DrawModel(Player player, string modelID, double x, double y, double z, string amodel, string modelType, double rotatey, bool existed, string imageBase64, string objText, string mtlText, ref List<string> notifyMsg);
        public DrawModel DrawObj3DModelF { get; set; }

        string bTCAddressValue = "";
        public string BTCAddress
        {
            get { return this.bTCAddressValue; }
            set
            {
                if (string.IsNullOrEmpty(value.Trim())) { }
                else if (string.IsNullOrEmpty(this.bTCAddressValue))
                {
                    if (BitCoin.CheckAddress.CheckAddressIsUseful(value))
                    {
                        this.bTCAddressValue = value;
                        this.nntl(this);
                    }
                }
            }
        }
        public delegate void NoNeedToLogin(Player player);
        public NoNeedToLogin nntl { get; set; }
        public void nntlF()
        {
            if (BitCoin.CheckAddress.CheckAddressIsUseful(this.bTCAddressValue))
            {
                this.nntl(this);
            }
        }
        public delegate void HasNewTaskToShow(Player player);
        public HasNewTaskToShow hntts { get; set; }

        public Dictionary<string, bool> backgroundData { get; set; }

        /// <summary>
        /// 推荐者的比特币地址
        /// </summary>
        public string RefererAddr { get; internal set; }

        int refererCountPrivate = 0;
        public int RefererCount
        {
            get
            {
                return this.refererCountPrivate;
            }
            set
            {
                if (BitCoin.CheckAddress.CheckAddressIsUseful(RefererAddr))
                {
                    this.refererCountPrivate = value;
                }
                else
                {
                    this.refererCountPrivate = 0;
                }
            }
        }

        public Action ShowCrossAfterWebUpdate = null;



        //  public enum RewardByModel
        public delegate bool GetConnection(Player player);
        public GetConnection GetConnectionF;
        public bool IsOnline()
        {
            return this.GetConnectionF(this);
        }

        internal void ValueChanged(ref List<string> notifyMsg)
        {
            this.getCar().ability.MileChanged(this, this.getCar(), ref notifyMsg, "mile");
            this.getCar().ability.BusinessChanged(this, this.getCar(), ref notifyMsg, "business");
            this.getCar().ability.VolumeChanged(this, this.getCar(), ref notifyMsg, "volume");
            this.getCar().ability.SpeedChanged(this, this.getCar(), ref notifyMsg, "speed");
        }

        internal void DrawTarget(int targetFpIndex, ref List<string> notifyMsg)
        {
            if (targetFpIndex >= 0)
            {
                var fp = Program.dt.GetFpByIndex(targetFpIndex);
                if (fp == null) return;
                var url = this.FromUrl;
                double x, y, z;
                CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(fp.Longitude, fp.Latitde, fp.Height, out x, out y, out z);
                DrawTarget dt = new DrawTarget()
                {
                    c = "DrawTarget",
                    WebSocketID = this.WebSocketID,
                    x = x,
                    y = y,
                    h = z
                };

                var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                notifyMsg.Add(url);
                notifyMsg.Add(sendMsg);
            }
        }

        public System.Threading.Thread playerSelectDirectionTh = null;
        //  internal Action NavigationAction = null;
        internal RoomMain.Node NavigationData = null;
        public int SendTransmitMsg = 100;

        public List<CommonClass.databaseModel.taskcopy> taskCopys = new List<taskcopy>();
        public void initializeTaskCopy()
        {
            this.taskCopys = new List<taskcopy>();
            if (BitCoin.CheckAddress.CheckAddressIsUseful(this.BTCAddress))
            {
                this.taskCopys = DalOfAddress.TaskCopy.GetALLItem(this.BTCAddress);
                //鼓楼Stock,此副本作为新手引导而用。
                var addSuccess1 = Program.rm.taskM.Add("GLSTOCK", this.BTCAddress.ToString(), this);
                var addSuccess2 = Program.rm.taskM.Add("URLSHARE", this.BTCAddress.ToString(), this);
                if (addSuccess1 || addSuccess2)
                {
                    this.taskCopys = DalOfAddress.TaskCopy.GetALLItem(this.BTCAddress);
                    this.hntts(this);
                }
            }
        }
    }
    public class NPC : RoleInGame
    {
        //string keyOfAttacker, NPC npc, ref List<string> notifyMsg, interfaceOfHM.Car cf
        public delegate void BeingAttacked(string keyOfAttacker, NPC npc, ref List<string> notifyMsg, interfaceOfHM.Car cf, GetRandomPos gp);
        public BeingAttacked BeingAttackedM;

        internal void CopyChanlleger(string challenger_)
        {
            this._challenger = challenger_;
        }

        string _challenger = "";
        /// <summary>
        /// 挑战者，npc对挑战者的态度是不死方休
        /// </summary>
        public string challenger { get { return this._challenger; } }

        public void BeingAttackedF(string keyOfAttacker, ref List<string> notifyMsgs, interfaceOfHM.Car cf, GetRandomPos gp)
        {
            //string keyOfAttacker, NPC npc, ref List<string> notifyMsg, interfaceOfHM.Car cf, 
            this.BeingAttackedM(keyOfAttacker, this, ref notifyMsgs, cf, gp);
        }

        internal void setChallenger(string key, ref List<string> notifyMsg)
        {
            this._challenger = key;
            this._molester = "";
        }

        //NPC npc, ref List<string> notifyMsgs
        public delegate void NPCOperateF(NPC npc, ref List<string> notifyMsgs, GetRandomPos grp);
        public NPCOperateF afterWaitedM;
        /// <summary>
        ///  NPC的状态为CarState.waitOnRoad时，对NPC发布命令。
        /// </summary>
        /// <param name="notifyMsgs"></param>
        public void dealWithWaitedNPC(ref List<string> notifyMsgs)
        {
            this.afterWaitedM(this, ref notifyMsgs, Program.dt);
            //throw new NotImplementedException();
        }

        public NPCOperateF afterReturnedM;
        internal void dealWithReturnedNPC(ref List<string> notifyMsg)
        {
            //if (!string.IsNullOrEmpty(this.molester)) { }
            //else
            if (!string.IsNullOrEmpty(this.challenger))
            {
                this.afterReturnedM(this, ref notifyMsg, Program.dt);
            }
        }

        public NPCOperateF afterBroke;
        public void afterBrokeM(ref List<string> notifyMsg)
        {
            this.afterBroke(this, ref notifyMsg, Program.dt);
        }

        string _molester = "";
        //internal void setMolester(string key, ref List<string> notifyMsg)
        //{
        //    if (string.IsNullOrEmpty(this._challenger))
        //        this._molester = key;
        //}
        /// <summary>
        /// 骚扰者，npc对待骚扰者的态度是教训一下即可！
        /// </summary>
       // public string molester { get { return this._molester; } }

        public NPCOperateF BeingMolestedM;

        public class AttackTag
        {
            public string Target { get; set; }
            public enum AttackType
            {
                attack,
                electric,
                fire,
                water,
                ambush,
                confuse,
                lose,
                speed,
                attackImprove,
                defendImprove
            }
            public AttackType aType { get; set; }
            public double HarmValue { get; internal set; }
            public FastonPosition fpPass { get; internal set; }
        }
        internal AttackTag attackTag = null;

        public bool BeingMolestedF(string keyOfMolester, ref List<string> notifyMsgs)
        {
            if (!this.Bust)
                if (string.IsNullOrEmpty(this._challenger) && string.IsNullOrEmpty(this._molester))
                {
                    this._molester = keyOfMolester;
                    this.BeingMolestedM(this, ref notifyMsgs, Program.dt);
                    return true;
                }
            return false;
            // this.BeingAttackedM(keyOfAttacker, this, ref notifyMsgs);
        }

        internal void initializeCarOfNPC()
        {
            List<string> notifyMsg = new List<string>();
            var car = this.getCar();

            for (var i = 2; i < this.levelObj.Level; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    switch (Program.rm.rm.Next(0, 4))
                    {
                        case 0:
                            {
                                car.ability.AbilityAdd("mile", 1, this, car, ref notifyMsg);
                            }; break;
                        case 1:
                            {
                                car.ability.AbilityAdd("business", 1, this, car, ref notifyMsg);
                            }; break;
                        case 2:
                            {
                                car.ability.AbilityAdd("volume", 1, this, car, ref notifyMsg);
                            }; break;
                        case 3:
                            {
                                car.ability.AbilityAdd("speed", 1, this, car, ref notifyMsg);
                            }; break;
                    }
                }
            }
            notifyMsg = null;
        }
    }

    public class OtherPlayers
    {
        string selfKey, otherKey;

        public OtherPlayers(string selfKeyInput, string otherKeyInput)
        {
            this.selfKey = selfKeyInput;
            this.otherKey = otherKeyInput;
            this.carChangeState = new Car.ChangeStateC()
            {
                md5 = "",
                privatekeysLength = -1
            };
            // this._brokenParameterT1Record = -1;
        }
        public Car.ChangeStateC carChangeState { get; private set; }
        // long _brokenParameterT1Record { get; set; }
        //public long brokenParameterT1Record
        //{
        //    get
        //    {
        //        return this._brokenParameterT1Record;
        //    }
        //}
        //public void setBrokenParameterT1Record(long value, ref List<string> notifyMsg)
        //{
        //    this._brokenParameterT1Record = value;
        //    // brokenParameterT1RecordChangedF(selfKey, otherKey, value, ref notifyMsg);
        //}
        /// <summary>
        /// 告诉自己，别人的社会责任发生变化了
        /// </summary>
        /// <param name="msgToPlayerKey"></param>
        /// <param name="contentKey"></param>
        /// <param name="value"></param>
        /// <param name="notifyMsg"></param>
    //    public delegate void BrokenParameterT1RecordChanged(string msgToPlayerKey, string contentKey, long value, ref List<string> notifyMsg);
        /// <summary>
        /// 此方法的目的是告诉自己，别人的社会责任发生变化了
        /// </summary>
      //  public BrokenParameterT1RecordChanged brokenParameterT1RecordChangedF;

        internal Car.ChangeStateC getCarState()
        {
            return this.carChangeState;
        }

        internal void setCarState(string md5, int privateKeysLength)
        {
            this.carChangeState = new Car.ChangeStateC()
            {
                md5 = md5,
                privatekeysLength = privateKeysLength
            };
        }


    }
}
