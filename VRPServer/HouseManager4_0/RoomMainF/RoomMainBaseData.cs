using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager4_0.RoomMainF
{
    public abstract class RoomMainBaseData
    {
        /// <summary>
        /// 随机数生产器
        /// </summary>
        public System.Random rm;

        /// <summary>
        /// 操作锁！
        /// </summary>
        public object PlayerLock = new object();

        /// <summary>
        /// 玩家字典
        /// </summary>
        public Dictionary<string, RoleInGame> _Players;

        /// <summary>
        /// 游戏官方市场
        /// </summary>
        public HouseManager4_0.Market Market { get; internal set; }

        /// <summary>
        /// 背景音乐选择器
        /// </summary>
        public HouseManager4_0.Music Music { get; internal set; }

        /// <summary>
        /// 背景Cube
        /// </summary>
        public BackGround bg;


        int _promoteMilePosition = -1;
        int _promoteBusinessPosition = -1;
        int _promoteVolumePosition = -1;
        int _promoteSpeedPosition = -1;

        public int promoteMilePosition
        {
            get
            {
                return this._promoteMilePosition;
            }
            set
            {
                lock (this.PlayerLock)
                {
                    this._promoteMilePosition = value;
                }
            }
        }
        public int promoteBusinessPosition
        {
            get { return this._promoteBusinessPosition; }
            set
            {
                lock (this.PlayerLock)
                {
                    this._promoteBusinessPosition = value;
                }
            }
        }
        public int promoteVolumePosition
        {
            get { return this._promoteVolumePosition; }
            set
            {
                lock (this.PlayerLock)
                {
                    this._promoteVolumePosition = value;
                }
            }
        }
        public int promoteSpeedPosition
        {
            get { return this._promoteSpeedPosition; }
            set
            {
                lock (this.PlayerLock)
                {
                    this._promoteSpeedPosition = value;
                }
            }
        }

        /// <summary>
        /// 宝石收集时间记录器
        /// </summary>
        public Dictionary<string, List<DateTime>> recordOfPromote = new Dictionary<string, List<DateTime>>();

        public Dictionary<string, long> promotePrice = new Dictionary<string, long>()
        {
            { "mile",1000},
            { "business",1000},
            { "volume",1000},
            { "speed",1000},
        };
        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <returns></returns>
        protected List<RoleInGame> getGetAllRoles()
        {
            List<RoleInGame> players = new List<RoleInGame>();
            foreach (var item in this._Players)
            {
                players.Add(item.Value);
            }
            return players;
        }

        /// <summary>
        /// 依据频率，获取价格。这个是随机获取地址的时候，就会获得。
        /// </summary>
        /// <param name="resultType">mile，business，volume，speed</param>
        /// <returns>返回结果为分，即1/100元</returns>
        protected long GetPriceOfPromotePosition(string resultType)
        {
            if (resultType == "mile" || resultType == "business" || resultType == "volume" || resultType == "speed")
            {
                this.recordOfPromote[resultType].Add(DateTime.Now);
            }
            else
            {
                throw new Exception($"错误地调用{resultType}");
            }
            if (this.recordOfPromote[resultType].Count < 10)
            {
                //  this.recordOfPromote[resultType].Add(DateTime.Now);
                return 10 * 100;
            }
            else
            {
                if (this.recordOfPromote[resultType].Count > 10)
                {
                    this.recordOfPromote[resultType].RemoveAt(0);
                }
                double sumHz = 0;
                for (var i = 1; i < this.recordOfPromote[resultType].Count; i++)
                {
                    var timeS = (this.recordOfPromote[resultType][i] - this.recordOfPromote[resultType][i - 1]).TotalSeconds;
                    timeS = Math.Max(1, timeS);
                    var itemHz = 1 / timeS;
                    sumHz += itemHz;
                }
                var averageValue = sumHz / (this.recordOfPromote[resultType].Count - 1);
                return Convert.ToInt32(50 * 100 * 60 * averageValue); //确保1分钟 的价格是50元
                //var calResult = Math.Round(Convert.ToDecimal(Math.Round(50 * 60 * averageValue, 2)), 2);
                //return Math.Max(0.01m, calResult);
            }
        }

        /// <summary>
        /// 收集金钱的东西
        /// </summary>
        public Dictionary<int, int> _collectPosition;
        public bool FpIsUsing(int fpIndex)
        {

            var A = false
                  || fpIndex == this._promoteMilePosition
                  || fpIndex == this._promoteBusinessPosition
                  || fpIndex == this._promoteVolumePosition
                  || fpIndex == this._promoteSpeedPosition
                  || this._collectPosition.ContainsValue(fpIndex);
            ;
            foreach (var item in this._Players)
            {

                A = item.Value.StartFPIndex == fpIndex || A;
                A = item.Value.getCar().targetFpIndex == fpIndex || A;
            }
            return A;
        }

        public Engine_AttackEngine attackE;

        public Engine_DebtEngine debtE;

        public Engine_Return retutnE;

        public Engine_Tax taxE;

        public Engine_CollectEngine collectE;

        public Engine_PromoteEngine promoteE;

        public Engine_DiamondOwnerEngine diamondOwnerE;

        public Engine_Attach attachE;

        public Engine_MagicEngine magicE;

        public Engine_Check checkE;
        //以上为Engine
        //以下为Manager

        public Manager_NPC NPCM;
        public Manager_Frequency frequencyM;
        public Manager_Driver driverM;
        public Manager_GoodsReward goodsM;
        public Manager_Model modelM;
        public Manager_Resistance modelR;
        public Manager_Connection modelC;
        public Manager_Level modelL;
        public Manager_TaskCopy taskM;
    }
}
