using CommonClass.driversource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HouseManager4_0
{
    public class AbilityAndState
    {
        RoleInGame role;
        Dictionary<string, List<DateTime>> Data { get; set; }
        public int[] getDataCount(string key)
        {

            if (this.Data.ContainsKey(key))
            {

                return new int[1] {
                    this.Data[key].Count,

                };
            }
            else
            {
                return new int[1] { 0 };
                // return 0;
            }
        }

        public void AbilityAdd(string pType, int count, RoleInGame role, Car car, ref List<string> notifyMsg)
        {
            if (this.Data.ContainsKey(pType))
            {
                for (int i = 0; i < count; i++)
                {
                    this.Data[pType].Add(DateTime.Now);
                }
                switch (pType)
                {

                    case "mile":
                        {
                            if (role.playerType == RoleInGame.PlayerType.player)
                            {
                                var player = (Player)role;
                                this.MileChanged(player, car, ref notifyMsg, pType);
                                ChangeTheUnder(role.Key, this.MileChanged, ref notifyMsg, pType);

                            }

                        }; break;
                    case "business":
                        {
                            if (role.playerType == RoleInGame.PlayerType.player)
                            {
                                var player = (Player)role;
                                this.BusinessChanged(player, car, ref notifyMsg, pType);
                                ChangeTheUnder(role.Key, this.BusinessChanged, ref notifyMsg, pType);
                            }
                        }; break;
                    case "volume":
                        {
                            if (role.playerType == RoleInGame.PlayerType.player)
                            {
                                var player = (Player)role;
                                this.VolumeChanged(player, car, ref notifyMsg, pType);
                                ChangeTheUnder(role.Key, this.VolumeChanged, ref notifyMsg, pType);
                            }

                        }; break;
                    case "speed":
                        {
                            if (role.playerType == RoleInGame.PlayerType.player)
                            {
                                var player = (Player)role;
                                this.SpeedChanged(player, car, ref notifyMsg, pType);
                                ChangeTheUnder(role.Key, this.SpeedChanged, ref notifyMsg, pType);
                            }

                        }; break;
                }
            }
        }
        /// <summary>
        /// except self
        /// </summary>
        /// <param name="key"></param>
        /// <param name="mileChangedF"></param>
        /// <param name="notifyMsg"></param>
        /// <param name="pType"></param>
        static void ChangeTheUnder(string key, AbilityChangedF mileChangedF, ref List<string> notifyMsg, string pType)
        {
            foreach (var item in Program.rm._Players)
            {
                if (item.Value.playerType == RoleInGame.PlayerType.player)
                {
                    if (item.Value.TheLargestHolderKey == key && item.Value.Key != key)
                    {
                        mileChangedF((Player)item.Value, item.Value.getCar(), ref notifyMsg, pType);
                    }
                }
            }
        }

        internal void AbilityClear(string pType, RoleInGame role, Car car, ref List<string> notifyMsg)
        {
            if (this.Data.ContainsKey(pType))
            {
                if (this.Data[pType].Count != 0)
                {
                    this.Data[pType].Clear();
                    switch (pType)
                    {
                        case "mile":
                            {
                                if (role.playerType == RoleInGame.PlayerType.player)
                                {
                                    var player = (Player)role;
                                    this.MileChanged(player, car, ref notifyMsg, pType);
                                    ChangeTheUnder(role.Key, this.MileChanged, ref notifyMsg, pType);
                                }

                            }; break;
                        case "business":
                            {
                                if (role.playerType == RoleInGame.PlayerType.player)
                                {
                                    var player = (Player)role;
                                    this.BusinessChanged(player, car, ref notifyMsg, pType);
                                    ChangeTheUnder(role.Key, this.BusinessChanged, ref notifyMsg, pType);
                                }
                            }; break;
                        case "volume":
                            {
                                if (role.playerType == RoleInGame.PlayerType.player)
                                {
                                    var player = (Player)role;
                                    this.VolumeChanged(player, car, ref notifyMsg, pType);
                                    ChangeTheUnder(role.Key, this.VolumeChanged, ref notifyMsg, pType);
                                }

                            }; break;
                        case "speed":
                            {
                                if (role.playerType == RoleInGame.PlayerType.player)
                                {
                                    var player = (Player)role;
                                    this.SpeedChanged(player, car, ref notifyMsg, pType);
                                    ChangeTheUnder(role.Key, this.SpeedChanged, ref notifyMsg, pType);
                                }

                            }; break;
                    }
                }

            }
        }

        string _diamondInCar = "";

        public delegate void DiamondInCarChangedF(Player player, Car car, ref List<string> notifyMsgs, string value);

        public DiamondInCarChangedF DiamondInCarChanged;
        /// <summary>
        /// 车上有没有已经完成的能力提升任务！""代表无，如mile则代表有！
        /// </summary>
        public string diamondInCar { get { return this._diamondInCar; } }


        public void setDiamondInCar(string diamondInCarInput, RoleInGame role, Car car, ref List<string> notifyMsg)
        {
            bool changed = this._diamondInCar != diamondInCarInput;
            this._diamondInCar = diamondInCarInput;
            if (role.playerType == RoleInGame.PlayerType.player && changed)
                this.DiamondInCarChanged((Player)role, car, ref notifyMsg, this.diamondInCar);
        }

        DateTime CreateTime { get; set; }

        long _costMiles = 0;
        /// <summary>
        /// 已经花费的里程！
        /// </summary>
        public long costMiles
        {
            get { return _costMiles; }

        }

        long _costBusiness = 0;
        /// <summary>
        /// 在车上的通过初始携带、税收获得的钱。单位为分，1/100元
        /// </summary>
        public long costBusiness
        {
            get
            {
                return _costBusiness;
            }
            //private set

            //{
            //    if (value < 0)
            //    {
            //        throw new Exception("错误的输入");
            //    }
            //    this._costBusiness = value;
            //}
        }
        long _costVolume = 0;
        /// <summary>
        /// 在车上的通过收集获得的钱。单位为分，1/100元
        /// </summary>
        internal long costVolume
        {
            get
            {
                return _costVolume;
            }
            //private set

            //{
            //    if (value < 0)
            //    {
            //        throw new Exception("错误的输入");
            //    }
            //    this._costVolume = value;
            //}
        }
        public AbilityAndState(RoleInGame roleInGame)
        {
            this.role = roleInGame;
            this.CreateTime = DateTime.Now;
            this.Data = new Dictionary<string, List<DateTime>>()
            {
                {
                    "mile",new List<DateTime>()
                },
                {
                    "business",new List<DateTime>()
                },
                {
                    "volume",new List<DateTime>()
                },
                {
                    "speed",new List<DateTime>()
                }
            };
            this._costMiles = 0;//this.costMiles = 0;
            this._costVolume = 0;//this.costVolume = 0;
            this._costBusiness = 0;
            this._diamondInCar = "";
            // this._subsidize = 0; ;
            //this.costBusiness = 0;
            //this.diamondInCar = "";
            //this.subsidize = 0;
        }



        /// <summary>
        /// 刷新时，会更新宝石状况（diamondInCar=""）。
        /// </summary>
        public void Refresh(RoleInGame player, Car car, ref List<string> notifyMsg)
        {

            //this.Data["mile"].RemoveAll(item => (item - this.CreateTime).TotalMinutes > 120);
            //this.Data["business"].RemoveAll(item => (item - this.CreateTime).TotalMinutes > 120);
            //this.Data["volume"].RemoveAll(item => (item - this.CreateTime).TotalMinutes > 120);
            //this.Data["speed"].RemoveAll(item => (item - this.CreateTime).TotalMinutes > 120);
            this._costMiles = 0;
            this._costBusiness = 0;
            this._costVolume = 0;
            if (player.playerType == RoleInGame.PlayerType.player)
            {
                MileChanged((Player)player, car, ref notifyMsg, "mile");
                BusinessChanged((Player)player, car, ref notifyMsg, "business");
                VolumeChanged((Player)player, car, ref notifyMsg, "volume");
                SpeedChanged((Player)player, car, ref notifyMsg, "speed");
            }
            this.setCostMiles(0, player, car, ref notifyMsg);
            // this.costMiles = 0;
            this.setCostBusiness(0, player, car, ref notifyMsg);
            //this.set
            //this.costBusiness = 0;
            this.setCostVolume(0, player, car, ref notifyMsg);
            //this.costVolume = 0;
            this.setDiamondInCar("", player, car, ref notifyMsg);
            //  this.diamondInCar = "";
            //  this.setSubsidize(0, player, car, ref notifyMsg);
            //   this.subsidize = 0;
        }

        public void RefreshAfterDriverArrived(RoleInGame player, Car car, ref List<string> notifyMsg)
        {
            if (player.playerType == RoleInGame.PlayerType.player)
            {
                MileChanged((Player)player, car, ref notifyMsg, "mile");
                BusinessChanged((Player)player, car, ref notifyMsg, "business");
                VolumeChanged((Player)player, car, ref notifyMsg, "volume");
                SpeedChanged((Player)player, car, ref notifyMsg, "speed");
            }
        }

        public delegate void AbilityChangedF(Player player, Car car, ref List<string> notifyMsgs, string pType);
        public AbilityChangedF MileChanged;
        public void setCostMiles(long costMileInput, RoleInGame role, Car car, ref List<string> notifyMsg)
        {
            this._costMiles = costMileInput;
            if (role.playerType == RoleInGame.PlayerType.player)
            {
                MileChanged((Player)role, car, ref notifyMsg, "mile");
            }
        }

        public AbilityChangedF BusinessChanged;
        public void setCostBusiness(long costBusinessCostInput, RoleInGame player, Car car, ref List<string> notifyMsg)
        {
            this._costBusiness = costBusinessCostInput;
            if (player.playerType == RoleInGame.PlayerType.player)
            {
                BusinessChanged((Player)player, car, ref notifyMsg, "business");
            }
        }

        public AbilityChangedF VolumeChanged;
        public void setCostVolume(long costVolumeCostInput, RoleInGame role, Car car, ref List<string> notifyMsg)
        {
            this._costVolume = costVolumeCostInput;
            if (role.playerType == RoleInGame.PlayerType.player)
                VolumeChanged((Player)role, car, ref notifyMsg, "volume");
        }

        public AbilityChangedF SpeedChanged;
        internal Driver driver = null;

        public delegate void DriverSelected(RoleInGame player, Car car, ref List<string> notifyMsgs);

        public DriverSelected driverSelected;
        //internal delegate void AbilityChanged(Player player, Car car, ref List<string> notifyMsgs, string pType);
        /// <summary>
        /// 依次用辅助、business、volume来支付。
        /// </summary>
        /// <param name="needMoney"></param>
        internal void payForPromote(long needMoney, RoleInGame player, Car car, ref List<string> notifyMsgs)
        {
            //var pay1 = needMoney;// Math.Min(needMoney, this.subsidize);
            // this.subsidize -= pay1;

            //var subsidizeNew = this.subsidize - pay1;
            //if (subsidizeNew != this.subsidize)
            //{
            //    this.setSubsidize(subsidizeNew, player, car, ref notifyMsgs);
            //}

            //   needMoney -= pay1;

            var pay2 = Math.Min(needMoney, this.costBusiness);
            // this.costBusiness -= pay2;
            var costBusinessNew = this.costBusiness - pay2;
            if (costBusinessNew != this.costBusiness)
            {
                this.setCostBusiness(costBusinessNew, player, car, ref notifyMsgs);
            }
            needMoney -= pay2;
            if (pay2 > 0)
            {
                //needToUpdateCostBussiness = true;
            }
            /*
             * 在获得能力提升宝石过程中，不可能动costVolume上的钱。
             * 状态变成收集后，只能攻击或者继续收集
             */
            //var pay3 = Math.Min(needMoney, this.costVolume);
            //this.costVolume -= pay3;
            //needMoney -= pay3;

            if (needMoney != 0)
            {
                throw new Exception("");
            }
        }

        /// <summary>
        /// 小车能跑的最大距离，最小值为350km！确保地图中的最长路径有个来回！
        /// </summary>
        public long mile
        {
            get
            {
                var selfValue = this.Data["mile"].Count * 7 + 350;
                if (!string.IsNullOrEmpty(role.TheLargestHolderKey))
                {
                    // this.role.rm
                    if (this.role.rm._Players.ContainsKey(role.TheLargestHolderKey))
                    {
                        if (this.role.rm._Players[role.TheLargestHolderKey].playerType == RoleInGame.PlayerType.player)
                        {
                            if (!this.role.rm._Players[role.TheLargestHolderKey].Bust)
                            {
                                return Math.Max(selfValue, this.role.rm._Players[role.TheLargestHolderKey].getCar().ability.Data["mile"].Count * 7 + 350);
                            }
                        }
                    }
                }
                return selfValue;
            }
        }
        public long leftMile
        {
            get
            {
                return this.mile - this.costMiles;
            }
        }
        /// <summary>
        /// 通过税收、携带，还能带多少钱。单位为分，即1/100元
        /// </summary>
        public long leftBusiness
        {
            get
            {
                return this.Business - this.costBusiness;
            }
        }
        /// <summary>
        /// 通过收集，还能收集多少钱。单位为分，即1/100元
        /// </summary>
        public long leftVolume
        {
            get
            {
                return this.Volume - this.costVolume;
            }
        }
        /// <summary>
        /// 小车能携带的金钱数量！单位为分，即1/100元。
        /// </summary>
        public long Business
        {
            get
            {
                if (this.driver == null)
                {
                    var selfValue = (this.Data["business"].Count * 500 + 10000);
                    if (!string.IsNullOrEmpty(role.TheLargestHolderKey))
                    {
                        if (Program.rm._Players.ContainsKey(role.TheLargestHolderKey))
                        {
                            if (Program.rm._Players[role.TheLargestHolderKey].playerType == RoleInGame.PlayerType.player)
                            {
                                if (!Program.rm._Players[role.TheLargestHolderKey].Bust)
                                {
                                    var bossValue = Program.rm._Players[role.TheLargestHolderKey].getCar().ability.Data["business"].Count * 500 + 10000;
                                    return Math.Max(selfValue, bossValue);
                                }
                            }
                        }
                    }
                    return selfValue;
                }
                else
                {
                    var selfValue = driver.improveBusiness((this.Data["business"].Count * 500 + 10000));
                    if (!string.IsNullOrEmpty(role.TheLargestHolderKey))
                    {
                        if (this.role.rm._Players.ContainsKey(role.TheLargestHolderKey))
                        {
                            if (this.role.rm._Players[role.TheLargestHolderKey].playerType == RoleInGame.PlayerType.player)
                            {
                                if (!this.role.rm._Players[role.TheLargestHolderKey].Bust)
                                {
                                    var bossValue = driver.improveBusiness(this.role.rm._Players[role.TheLargestHolderKey].getCar().ability.Data["business"].Count * 500 + 10000);
                                    return Math.Max(selfValue, bossValue);
                                }
                            }
                        }
                    }
                    return selfValue;
                }
            }
        }
        /// <summary>
        /// 小车能装载的最大容量，默认为10000分，即1/100元。
        /// </summary>
        public long Volume
        {
            get
            {
                if (this.driver == null)
                {
                    var selfValue = this.Data["volume"].Count * 500 + 10000;
                    if (!string.IsNullOrEmpty(role.TheLargestHolderKey))
                    {
                        if (this.role.rm._Players.ContainsKey(role.TheLargestHolderKey))
                        {
                            if (this.role.rm._Players[role.TheLargestHolderKey].playerType == RoleInGame.PlayerType.player)
                            {
                                if (!this.role.rm._Players[role.TheLargestHolderKey].Bust)
                                {
                                    var bossValue = this.role.rm._Players[role.TheLargestHolderKey].getCar().ability.Data["volume"].Count * 500 + 10000;
                                    return Math.Max(selfValue, bossValue);
                                }
                            }
                        }
                    }
                    return selfValue;
                }
                else
                {
                    var selfValue = driver.improveVolume((this.Data["volume"].Count * 500 + 10000));
                    if (!string.IsNullOrEmpty(role.TheLargestHolderKey))
                    {
                        if (this.role.rm._Players.ContainsKey(role.TheLargestHolderKey))
                        {
                            if (this.role.rm._Players[role.TheLargestHolderKey].playerType == RoleInGame.PlayerType.player)
                            {
                                if (!this.role.rm._Players[role.TheLargestHolderKey].Bust)
                                {
                                    var bossValue = driver.improveVolume(Program.rm._Players[role.TheLargestHolderKey].getCar().ability.Data["volume"].Count * 500 + 10000);
                                    return Math.Max(selfValue, bossValue);
                                }
                            }
                        }
                    }
                    return selfValue;
                }
            }
        }

        internal long ReduceBusinessAndVolume(Player player, Car car, ref List<string> notifyMsg)
        {
            long reduceValue = 0;
            var reduceBusiness = this.costBusiness / 5;
            var reduceVolume = this.costVolume / 5;

            if (this.costBusiness > 0)
            {
                reduceBusiness = Math.Max(1, reduceBusiness);
            }
            if (this.costVolume > 0)
            {
                reduceVolume = Math.Max(1, reduceVolume);
            }
            reduceValue += reduceBusiness;
            reduceValue += reduceVolume;
            //this.setCostMiles(this.costMiles + this.mile / 20, player, car, ref notifyMsg);
            this.setCostBusiness(this.costBusiness - reduceBusiness, player, car, ref notifyMsg);
            this.setCostVolume(this.costVolume - reduceVolume, player, car, ref notifyMsg);
            return reduceValue;
        }

        internal bool HasDiamond()
        {
            foreach (var item in this.Data)
            {
                if (item.Value.Count > 0)
                {
                    return true;
                }
            }
            return false;
            // throw new NotImplementedException();
        }

        /// <summary>
        /// 小车能跑的最快速度！
        /// </summary>
        public int Speed
        {
            get
            {
                if (this.driver == null)
                {
                    var selfValue = this.Data["speed"].Count * 2 + 50;
                    if (!string.IsNullOrEmpty(role.TheLargestHolderKey))
                    {
                        //  if(role.TheLargestHolderKey)
                        if (Program.rm._Players.ContainsKey(role.TheLargestHolderKey))
                        {
                            if (Program.rm._Players[role.TheLargestHolderKey].playerType == RoleInGame.PlayerType.player)
                            {
                                if (!Program.rm._Players[role.TheLargestHolderKey].Bust)
                                {
                                    var bossValue = Program.rm._Players[role.TheLargestHolderKey].getCar().ability.Data["speed"].Count * 2 + 50;
                                    return Math.Max(selfValue, bossValue);
                                }
                            }
                        }
                    }
                    return selfValue;
                }
                else
                {
                    var selfValue = driver.improveSpeed(this.Data["speed"].Count * 2 + 50);
                    if (!string.IsNullOrEmpty(role.TheLargestHolderKey))
                    {
                        // this.role.rm
                        if (this.role.rm._Players.ContainsKey(role.TheLargestHolderKey))
                        {
                            if (this.role.rm._Players[role.TheLargestHolderKey].playerType == RoleInGame.PlayerType.player)
                            {
                                if (!this.role.rm._Players[role.TheLargestHolderKey].Bust)
                                {
                                    var bossValue = driver.improveSpeed(this.role.rm._Players[role.TheLargestHolderKey].getCar().ability.Data["speed"].Count * 2 + 50);
                                    return Math.Max(selfValue, bossValue);
                                }
                            }
                        }
                    }
                    return selfValue;
                }
            }
        }

        /// <summary>
        /// 单位为分，是身上business（业务）。
        /// </summary>
        public long SumMoneyCanForPromote
        {
            get
            {
                return this.costBusiness;//+ this.subsidize;
            }
        }
        public long SumMoneyCanForAttack
        {
            get
            {
                return this.costVolume + this.costBusiness;
            }
        }

    }
}
