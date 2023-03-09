using CommonClass.databaseModel;
using HouseManager4_0.interfaceOfHM;
using HouseManager4_0.RoomMainF;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using RoomMain = HouseManager4_0.RoomMainF.RoomMain;
using static NBitcoin.Scripting.OutputDescriptor;
using static CommonClass.ModelTranstraction;
using System.Runtime.CompilerServices;
using System.IO;
using CommonClass;
using CityRunFunction;
using DalOfAddress;
using MySql.Data.MySqlClient;
using System.Linq;

namespace HouseManager4_0
{
    public partial class Manager_TaskCopy : Manager
    {
        StockTC st = null;
        Share sha = null;
        public Manager_TaskCopy(RoomMain roomMain)
        {
            try
            {
                this.roomMain = roomMain;
                try
                {
                    this.st = new StockTC(this.roomMain);
                    this.sha = new Share(this.roomMain);
                }
                catch
                {
                    this.st = null;
                    this.sha = null;
                }
            }
            catch { }
        }
        internal void Initialize(Player player)
        {
            player.initializeTaskCopy();
        }
        public List<CommonClass.databaseModel.taskcopy> GetALLItem(string address)
        {
            var allItem = DalOfAddress.TaskCopy.GetALLItem(address);
            return allItem.OrderBy(item => getTaskCopyOrder(item.taskCopyCode)).ToList();
        }
        int getTaskCopyOrder(string code)
        {
            // switch(stock)
            switch (code)
            {
                case StockTC.TaskCode: return 0;
                case Share.TaskCode: return 1;
                default: return 99;
            }
        }

        internal bool Add(string Code, string btcAddr, Player player)
        {
            try
            {
                TaskCopyCallF f = null;
                switch (Code)
                {
                    case StockTC.TaskCode:
                        {
                            f = this.st;

                        }; break;
                    case Share.TaskCode:
                        {
                            f = this.sha;
                        }; break;
                }
                if (f != null)
                {
                    var addResult = f.Add(btcAddr);
                    if (addResult)
                    {
                        this.WebNotify(player, $"您开启了新任务：{f.TaskName}。");
                    }
                    return addResult;
                }
                else return false;
            }
            catch
            {
                return false;
            }
        }
        internal void Pass(List<taskcopy> taskCopys)
        {
            try
            {
                for (int i = 0; i < taskCopys.Count; i++)
                {
                    var Item = taskCopys[i];
                    var Code = Item.taskCopyCode;
                    TaskCopyCallF f = null;
                    switch (Code)
                    {
                        case StockTC.TaskCode:
                            {
                                f = this.st;
                            }; break;
                        case Share.TaskCode:
                            {
                                f = this.sha;
                            }; break;

                    }
                    if (f != null)
                    {
                        f.Pass(Item);
                    }
                }
            }
            catch { }
        }
        internal void BindWordInfoF(Manager_TaskCopy taskM, List<taskcopy> taskCopys)
        {
            try
            {

                for (int i = 0; i < taskCopys.Count; i++)
                {
                    var Item = taskCopys[i];
                    var Code = Item.taskCopyCode;
                    TaskCopyCallF f = null;
                    switch (Code)
                    {
                        case StockTC.TaskCode:
                            {
                                f = this.st;
                            }; break;

                    }
                    if (f != null)
                    {
                        f.BindWordInfoF(Item);
                    }
                }

            }
            catch { }
        }

        public List<string> Display(Player player)
        {
            player.initializeTaskCopy();
            List<taskcopy> taskCopys = player.taskCopys;
            List<string> list = new List<string>();
            try
            {

                for (int i = 0; i < taskCopys.Count; i++)
                {
                    //  List<string> itemL = new List<string>();
                    var Item = taskCopys[i];
                    var Code = Item.taskCopyCode;
                    TaskCopyCallF f = null;
                    switch (Code)
                    {
                        case StockTC.TaskCode:
                            {
                                f = this.st;
                            }; break;
                        case Share.TaskCode:
                            {
                                f = this.sha;
                            }; break;

                    }
                    if (f != null)
                    {
                        list.Add(Code);
                        list.Add(f.GetTaskName(Item));
                        list.Add(f.Detail(Item));
                    }
                }

            }
            catch { }
            return list;
        }

        internal void ChargingF(List<taskcopy> taskCopys, int chargingValue)
        {
            try
            {

                for (int i = 0; i < taskCopys.Count; i++)
                {
                    var Item = taskCopys[i];
                    var Code = Item.taskCopyCode;
                    TaskCopyCallF f = null;
                    switch (Code)
                    {
                        case StockTC.TaskCode:
                            {
                                f = this.st;
                            }; break;

                    }
                    if (f != null)
                    {
                        f.ChargingF(Item);
                    }
                }

            }
            catch { }
        }

        internal void DiamondCollected(Player player)
        {
            try
            {

                for (int i = 0; i < player.taskCopys.Count; i++)
                {
                    var Item = player.taskCopys[i];
                    var Code = Item.taskCopyCode;
                    TaskCopyCallF f = null;
                    switch (Code)
                    {
                        case StockTC.TaskCode:
                            {
                                f = this.st;
                            }; break;

                    }
                    if (f != null)
                    {
                        f.DoCollect(Item, player, this.WebNotify);
                    }
                }

            }
            catch { }

        }

        internal void DiamondSell(Player player)
        {
            try
            {
                for (int i = 0; i < player.taskCopys.Count; i++)
                {
                    var Item = player.taskCopys[i];
                    var Code = Item.taskCopyCode;
                    TaskCopyCallF f = null;
                    switch (Code)
                    {
                        case StockTC.TaskCode:
                            {
                                f = this.st;
                            }; break;
                        default:
                            {
                                f = null;
                                continue;
                            };
                    }
                    if (f != null)
                    {
                        f.DiamondSell(Item, player, this.WebNotify);
                    }
                }
            }
            catch { }
        }

        internal void DriverSelected(Player player)
        {
            try
            {
                for (int i = 0; i < player.taskCopys.Count; i++)
                {
                    var Item = player.taskCopys[i];
                    var Code = Item.taskCopyCode;
                    TaskCopyCallF f = null;
                    switch (Code)
                    {
                        case StockTC.TaskCode:
                            {
                                f = this.st;
                            }; break;
                        default:
                            {
                                f = null;
                                continue;
                            };
                    }
                    if (f != null)
                    {
                        f.DriverSelected(Item, player, this.WebNotify);
                    }
                }
            }
            catch { }
        }

        internal void GetRewardFromBuildingF(Player player)
        {
            try
            {
                for (int i = 0; i < player.taskCopys.Count; i++)
                {
                    var Item = player.taskCopys[i];
                    var Code = Item.taskCopyCode;
                    TaskCopyCallF f = null;
                    switch (Code)
                    {
                        case StockTC.TaskCode:
                            {
                                f = this.st;
                            }; break;
                        default:
                            {
                                f = null;
                                continue;
                            };
                    }
                    if (f != null)
                    {
                        f.GetRewardFromBuildingF(Item, player, this.WebNotify);
                    }
                }
            }
            catch { }
        }

        internal void MagicReleased(Player player)
        {
            try
            {
                for (int i = 0; i < player.taskCopys.Count; i++)
                {
                    var Item = player.taskCopys[i];
                    var Code = Item.taskCopyCode;
                    TaskCopyCallF f = null;
                    switch (Code)
                    {
                        case StockTC.TaskCode:
                            {
                                f = this.st;
                            }; break;
                        default:
                            {
                                f = null;
                                continue;
                            };
                    }
                    if (f != null)
                    {
                        f.MagicReleased(Item, player, this.WebNotify);
                    }
                }
            }
            catch { }
        }

        internal void MoneySet(Player player)
        {
            try
            {
                // player.initializeTaskCopy();
                for (int i = 0; i < player.taskCopys.Count; i++)
                {
                    var Item = player.taskCopys[i];
                    var Code = Item.taskCopyCode;
                    TaskCopyCallF f = null;
                    switch (Code)
                    {
                        case StockTC.TaskCode:
                            {
                                f = this.st;
                            }; break;
                    }
                    if (f != null)
                    {
                        f.MoneySet(Item, player, this.WebNotify);
                    }
                }
            }
            catch { }
        }

        internal void PlayerLevelSynchronize(Player player)
        {
            try
            {
                //    player.initializeTaskCopy();
                for (int i = 0; i < player.taskCopys.Count; i++)
                {
                    var Item = player.taskCopys[i];
                    var Code = Item.taskCopyCode;
                    TaskCopyCallF f = null;
                    switch (Code)
                    {
                        case StockTC.TaskCode:
                            {
                                f = this.st;
                            }; break;
                    }
                    if (f != null)
                    {
                        f.OrderToUpdateLevel(Item, player, this.WebNotify);
                    }
                }
            }
            catch { }
        }

        internal void TakeApartF(Player player)
        {
            try
            {
                //    player.initializeTaskCopy();
                for (int i = 0; i < player.taskCopys.Count; i++)
                {
                    var Item = player.taskCopys[i];
                    var Code = Item.taskCopyCode;
                    TaskCopyCallF f = null;
                    switch (Code)
                    {
                        case StockTC.TaskCode:
                            {
                                f = this.st;
                            }; break;
                    }
                    if (f != null)
                    {
                        f.TakeApartF(Item, player, this.WebNotify);
                    }
                }
            }
            catch { }
        }

        internal void TradeCoinF(Player[] players, string addrBussiness, string addrTo, long passCoin, List<taskcopy> taskCopys)
        {
            try
            {
                // List<taskcopy> taskCopys;

                for (int i = 0; i < taskCopys.Count; i++)
                {
                    var Item = taskCopys[i];
                    var Code = Item.taskCopyCode;
                    TaskCopyCallF f = null;
                    switch (Code)
                    {
                        case StockTC.TaskCode:
                            {
                                f = this.st;
                            }; break;
                    }
                    if (f != null)
                    {
                        f.TradeCoinF(addrBussiness, addrTo, passCoin, Item, players, this.WebNotify);
                    }
                }
            }
            catch { }
        }

        internal void UseDiamondSuccess(Player player)
        {
            try
            {
                //   player.initializeTaskCopy();
                for (int i = 0; i < player.taskCopys.Count; i++)
                {
                    var Item = player.taskCopys[i];
                    var Code = Item.taskCopyCode;
                    TaskCopyCallF f = null;
                    switch (Code)
                    {
                        case StockTC.TaskCode:
                            {
                                f = this.st;
                            }; break;
                    }
                    if (f != null)
                    {
                        f.UseDiamondSuccess(Item, player, this.WebNotify);
                    }
                }
            }
            catch { }
        }

        internal void Delete(Player player, string code)
        {
            switch (code)
            {
                case StockTC.TaskCode:
                    {
                        DalOfAddress.TaskCopy.Del(player.BTCAddress, code);
                        player.initializeTaskCopy();
                    }; break;
                case Share.TaskCode:
                    {
                        DalOfAddress.TaskCopy.Del(player.BTCAddress, code);
                        player.initializeTaskCopy();
                    }; break;
                default:
                    {
                        code = "";
                        return;
                    }
            }


        }
    }

    public partial class Manager_TaskCopy
    {
        protected enum TaskCopyType
        {
            SelectDriver,
            MagicRelease,
            DiamondCollect,
            DiamondUse,
            AbilityTakeApart,
            DiamondSell,
            MoneySave,
            GetBless,
            ChallengeNPCL2,
            StockToStockAddr,
            AddrHasBindWord,
            RewardDeveloper,
            RedLuckyMoneyToPlayer,
            Finished,
            Null
        }

        protected class SingleTask
        {
            public TaskCopyType TType { get { return this.tType; } }
            TaskCopyType tType;

            public int TaskCopyTime { get { return this._taskCopyTime; } }
            int _taskCopyTime;
            void initialize(TaskCopyType t, int CopyTime)
            {
                this.tType = t;
                this._taskCopyTime = CopyTime;
            }
            public SingleTask(TaskCopyType t)
            {
                initialize(t, 1);
            }
            public SingleTask(TaskCopyType t, int CopyTime)
            {
                if (CopyTime < 1)
                {
                    CopyTime = 1;
                }
                initialize(t, CopyTime);
            }
            public bool isFinished(int finishedTime)
            {
                return finishedTime >= this._taskCopyTime;
            }
        }

        protected class TaskChain
        {
            protected List<SingleTask> chain;
            // protected Dictionary<string, int[][]> process;
        }

        interface TaskCopyCallF
        {
            string TaskName { get; }

            void DriverSelected(taskcopy item, Player player, Action<RoleInGame, string> webNotify);
            void DoCollect(taskcopy Item, Player player, Action<RoleInGame, string> WebNotify);
            bool Add(string btcAddr);
            string GetTaskName(taskcopy Item);
            void MagicReleased(taskcopy item, Player player, Action<RoleInGame, string> webNotify);
            void UseDiamondSuccess(taskcopy item, Player player, Action<RoleInGame, string> webNotify);
            void TakeApartF(taskcopy item, Player player, Action<RoleInGame, string> webNotify);
            void DiamondSell(taskcopy item, Player player, Action<RoleInGame, string> webNotify);
            void MoneySet(taskcopy item, Player player, Action<RoleInGame, string> webNotify);
            void GetRewardFromBuildingF(taskcopy item, Player player, Action<RoleInGame, string> webNotify);
            void OrderToUpdateLevel(taskcopy item, Player player, Action<RoleInGame, string> webNotify);
            void TradeCoinF(string addrBussiness, string addrTo, long passCoin, taskcopy item, Player[] players, Action<RoleInGame, string> webNotify);
            bool BindWordInfoF(taskcopy item);
            void ChargingF(taskcopy item);
            string Detail(taskcopy item);
            void Pass(taskcopy item);
            void AddReferer(int refererCount, taskcopy item);
        }
        protected partial class StockTC : TaskChain
        {


            //   List<SingleTask> TaskChain;
            RoomMain that;

            string stockAddr;

            string stockAddrPrivateKey;

            /// <summary>
            /// 此参数只作为初始化使用。初始化，存入Tag.
            /// </summary>
            string currentBAddr;

            public StockTC(RoomMain roomMain)
            {
                that = roomMain;
                loadStockAddr();

                loadPrivateKey();

                /*
                 * A.选择司机
                 * B.释放法术
                 * C.宝石收集，5次
                 * D.宝石使用
                 * E.释放宝石
                 * F
                 */
                this.chain = new List<SingleTask>()
                {
                    new SingleTask(TaskCopyType.SelectDriver),//选取司机
                    new SingleTask(TaskCopyType.MagicRelease),//法术释放
                    new SingleTask( TaskCopyType.DiamondCollect,5),//宝石选择
                    new SingleTask(TaskCopyType.DiamondUse),//宝石使用
                    new SingleTask(TaskCopyType.AbilityTakeApart),//释放宝石
                    new SingleTask(TaskCopyType.DiamondSell),//宝石出售
                    new SingleTask(TaskCopyType.MoneySave),//积分存储
                    new SingleTask(TaskCopyType.GetBless),//祈福
                    new SingleTask(TaskCopyType.ChallengeNPCL2),//挑战2级NPC
                    new SingleTask(TaskCopyType.StockToStockAddr),//将股份转至StockAddr
                    new SingleTask(TaskCopyType.AddrHasBindWord),//拥有绑定词
                    new SingleTask(TaskCopyType.RewardDeveloper),//打赏
                    new SingleTask(TaskCopyType.RedLuckyMoneyToPlayer),//给发红包
                    new SingleTask(TaskCopyType.Finished),//给发红包
                };
            }

            private void loadStockAddr()
            {
                if (string.IsNullOrEmpty(this.stockAddr))
                {
                    //  this.stockAddrPrivateKey = "";
                    this.stockAddr = File.ReadAllText($"config/{TaskCode}_StockAddr.txt");
                }
                if (string.IsNullOrEmpty(this.currentBAddr))
                {
                    //  this.stockAddrPrivateKey = "";
                    this.currentBAddr = File.ReadAllText($"config/{TaskCode}_bussinessAddr.txt");
                }
                if (string.IsNullOrEmpty(this._taskName))
                {
                    this._taskName = File.ReadAllText($"config/{TaskCode}_taskName.txt");
                }
                if (string.IsNullOrEmpty(this._taskHappenPlaceName))
                {
                    this._taskHappenPlaceName = File.ReadAllText($"config/{TaskCode}_placeName.txt");
                }
            }

            private void loadPrivateKey()
            {
                if (string.IsNullOrEmpty(this.stockAddrPrivateKey))
                {
                    //  this.stockAddrPrivateKey = "";
                    var secret = File.ReadAllText($"config/{TaskCode}_PrivateSec.txt");
                    var clearText = AES.AesDecrypt(secret, DalOfAddress.Connection.PasswordStr);
                    this.stockAddrPrivateKey = clearText;//clearText;
                }
            }



            internal bool DealWithSuccess(ref taskcopy item)
            {
                if (item.firstRound < this.chain.Count)
                {
                    var oldFirst = item.firstRound;
                    var oldSecond = item.secondRound;
                    item.secondRound++;
                    if (this.chain[item.firstRound].isFinished(item.secondRound))
                    {
                        item.firstRound++;
                        item.secondRound = 0;

                    }
                    return DalOfAddress.TaskCopy.UpdateRoundOfItem(item, oldFirst, oldSecond);
                }
                else
                {
                    return false;
                }
            }

            internal TaskCopyType GetCurrent(taskcopy item)
            {
                if (item.firstRound < this.chain.Count)
                    return this.chain[item.firstRound].TType;
                else return TaskCopyType.Null;
            }

            internal int GetCurrentRoundCount(taskcopy item)
            {
                if (item.firstRound < this.chain.Count)
                    return this.chain[item.firstRound].TaskCopyTime;
                else return 0;
            }


            public const string TaskCode = "GLSTOCK";

            // public string Name { get { return "太谷鼓楼股份"; } }
        }
        protected partial class StockTC : TaskCopyCallF
        {
            public string TaskName
            {
                /*
                 * 此项只是新增时需要，新增之时，写入Item的Tag。调用时，用GetTaskName(taskcopy item)
                 */
                get { return _taskName; }
                //return "鼓楼股份获取";
            }

            public void DriverSelected(taskcopy Item, Player player, Action<RoleInGame, string> WebNotify)
            {
                if (this.GetCurrent(Item) == TaskCopyType.SelectDriver)
                {
                    var s = this.DealWithSuccess(ref Item);
                    if (s)
                    {
                        player.hntts(player);
                        WebNotify(player, $"完成【{this.GetTaskName(Item)}】任务中的选取司机！详情查看任务！");
                    }
                }
            }

            public string GetTaskName(taskcopy item)
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<StockAddrC>(item.Tag).Name;
            }
            string GetItemBussinessAddr(taskcopy item)
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<StockAddrC>(item.Tag).BAddr;
            }

            public void DoCollect(taskcopy Item, Player player, Action<RoleInGame, string> WebNotify)
            {
                if (this.GetCurrent(Item) == TaskCopyType.DiamondCollect)
                {
                    var lastItem = this.GetCurrent(Item);
                    var count = this.GetCurrentRoundCount(Item);
                    var success = this.DealWithSuccess(ref Item);
                    if (success)
                    {
                        WebNotify(player, $"完成【{this.GetTaskName(Item)}】任务中的宝石收集({Item.secondRound}/{count})！详情查看任务！");
                        player.hntts(player);
                    }
                }
            }
            class StockAddrC
            {
                public string BAddr { get; set; }
                public string CheckWord { get; set; }
                public string Name { get; set; }
                public string HappenPlace { get; set; }
            }
            public bool Add(string btcAddr)
            {
                //const int AddMoney = 500;
                StockAddrC sac = new StockAddrC()
                {
                    BAddr = this.currentBAddr,
                    Name = this._taskName,
                    CheckWord = "",
                    HappenPlace = this._taskHappenPlaceName
                };
                string Tag = Newtonsoft.Json.JsonConvert.SerializeObject(sac);
                CommonClass.databaseModel.taskcopy tc = new CommonClass.databaseModel.taskcopy()
                {
                    btcAddr = btcAddr,
                    firstRound = 0,
                    secondRound = 0,
                    Result = null,
                    ResultDateTime = null,
                    Tag = Tag,
                    taskCopyCode = TaskCode
                };
                return DalOfAddress.TaskCopy.Add(tc);
            }

            string _taskName = "";
            string _taskHappenPlaceName = "";


            public void MagicReleased(taskcopy Item, Player player, Action<RoleInGame, string> WebNotify)
            {
                if (this.GetCurrent(Item) == TaskCopyType.MagicRelease)
                {
                    var s = this.DealWithSuccess(ref Item);
                    if (s)
                    {
                        player.hntts(player);
                        WebNotify(player, $"完成【{this.GetTaskName(Item)}】任务中的法术释放！详情查看任务！");
                    }
                }
            }

            public void UseDiamondSuccess(taskcopy Item, Player player, Action<RoleInGame, string> WebNotify)
            {
                if (this.GetCurrent(Item) == TaskCopyType.DiamondUse)
                {
                    var s = this.DealWithSuccess(ref Item);
                    if (s)
                    {
                        player.hntts(player);
                        WebNotify(player, $"完成【{this.GetTaskName(Item)}】任务中的使用宝石！详情查看任务！");
                    }
                }
            }

            public void TakeApartF(taskcopy Item, Player player, Action<RoleInGame, string> WebNotify)
            {
                if (this.GetCurrent(Item) == TaskCopyType.AbilityTakeApart)
                {
                    var s = this.DealWithSuccess(ref Item);
                    if (s)
                    {
                        player.hntts(player);
                        WebNotify(player, $"完成【{this.GetTaskName(Item)}】任务中的释玉！详情查看任务！");
                    }
                }
            }

            public void DiamondSell(taskcopy Item, Player player, Action<RoleInGame, string> WebNotify)
            {
                if (this.GetCurrent(Item) == TaskCopyType.DiamondSell)
                {
                    var s = this.DealWithSuccess(ref Item);
                    if (s)
                    {
                        player.hntts(player);
                        WebNotify(player, $"完成【{this.GetTaskName(Item)}】任务中的出售宝石！详情查看任务！");
                    }
                }
            }

            public void MoneySet(taskcopy Item, Player player, Action<RoleInGame, string> WebNotify)
            {
                if (this.GetCurrent(Item) == TaskCopyType.MoneySave)
                {
                    var s = this.DealWithSuccess(ref Item);
                    if (s)
                    {
                        player.hntts(player);
                        WebNotify(player, $"完成【{this.GetTaskName(Item)}】任务中的积分存储！详情查看任务！");
                    }
                }
            }

            public void GetRewardFromBuildingF(taskcopy Item, Player player, Action<RoleInGame, string> WebNotify)
            {
                if (this.GetCurrent(Item) == TaskCopyType.GetBless)
                {
                    var s = this.DealWithSuccess(ref Item);
                    WebNotify(player, $"完成【{this.GetTaskName(Item)}】任务中的求福！详情查看任务！");

                    //if (s && Item.firstRound == 8)
                    //{
                    //    OrderToUpdateLevel(Item, player, WebNotify);
                    //}
                }
            }

            public void OrderToUpdateLevel(taskcopy Item, Player player, Action<RoleInGame, string> WebNotify)
            {
                if (this.GetCurrent(Item) == TaskCopyType.ChallengeNPCL2 && player.levelObj.Level >= 2)
                {
                    var giveSuccess = StockGive(Item);
                    if (giveSuccess)
                    {
                        var s = this.DealWithSuccess(ref Item);
                        if (s)
                        {
                            player.hntts(player);
                            WebNotify(player, $"完成【{this.GetTaskName(Item)}】任务中的挑战2级以上的NPC任务！");
                            WebNotify(player, $"完成【{this.GetTaskName(Item)}】任务中的挑战2级。你获得了太谷鼓楼100聪的股份！");
                        }
                    }
                    else
                    {
                        WebNotify(player, $"打败了2级及以上的NPC，但没获得股权！再接再厉！");
                    }
                }
            }
            internal bool StockGive(taskcopy Item)
            {
                try
                {
                    var addrTo = Item.btcAddr;
                    var bussinessAddr = this.GetItemBussinessAddr(Item);
                    const double tranNum = 0.000001;
                    if (
                        BitCoin.CheckAddress.CheckAddressIsUseful(bussinessAddr) &&
                        BitCoin.CheckAddress.CheckAddressIsUseful(this.stockAddr) &&
                        BitCoin.CheckAddress.CheckAddressIsUseful(addrTo) &&
                       tranNum >= 0.00000001
                        )
                    {
                        int indexNumber = 0;
                        indexNumber = GetIndexOfTrade(bussinessAddr, this.stockAddr);
                        if (indexNumber >= 0)
                        {
                            var agreement = $"{indexNumber}@{this.stockAddr}@{bussinessAddr}->{addrTo}:{tranNum * 100000000}Satoshi";
                            var sign = BitCoin.Sign.SignMessage(this.stockAddrPrivateKey, agreement, this.stockAddr);
                            // DalOfAddress
                            var r = that.TradeCoinF(new TradeCoin()
                            {
                                addrBussiness = bussinessAddr,
                                tradeIndex = indexNumber,
                                addrFrom = this.stockAddr,
                                addrTo = addrTo,
                                c = "TradeCoin",
                                msg = agreement,
                                passCoin = Convert.ToInt64(tranNum * 100000000),
                                sign = sign,
                            }, true);
                            return Newtonsoft.Json.JsonConvert.DeserializeObject<TradeCoin.Result>(r).success;
                        }
                    }
                    return false;
                }
                catch
                {
                    return false;
                }
            }

            private int GetIndexOfTrade(string addrBussiness, string addrFrom)
            {
                var Index = DalOfAddress.TradeRecord.GetCount(addrBussiness, addrFrom);
                return Index;
            }
            // void TradeCoinF(string addrBussiness, string addrTo, long passCoin, taskcopy item, Player player, Action<RoleInGame, string> webNotify);
            public void TradeCoinF(string addrBussiness, string addrTo, long passCoin, taskcopy Item, Player[] players, Action<RoleInGame, string> webNotify)
            {
                var bussinessAddr = this.GetItemBussinessAddr(Item);
                if (this.GetCurrent(Item) == TaskCopyType.StockToStockAddr &&
                    bussinessAddr == addrBussiness &&
                    this.stockAddr == addrTo
                    )
                {
                    var s = this.DealWithSuccess(ref Item);
                    if (s)
                    {
                        for (int i = 0; i < players.Length; i++)
                        {
                            players[i].initializeTaskCopy();
                            webNotify(players[i], $"完成了股权转让！");
                            players[i].hntts(players[i]);
                        }
                    }
                }
            }

            public bool BindWordInfoF(taskcopy Item)
            {
                if (this.GetCurrent(Item) == TaskCopyType.AddrHasBindWord)
                {
                    if (!string.IsNullOrEmpty(DalOfAddress.BindWordInfo.GetWordByAddr(Item.btcAddr)))
                    {
                        this.DealWithSuccess(ref Item);
                        return true;
                    }

                }
                return false;
            }

            public void ChargingF(taskcopy Item)
            {
                if (this.GetCurrent(Item) == TaskCopyType.RewardDeveloper)
                {
                    if (!string.IsNullOrEmpty(DalOfAddress.BindWordInfo.GetWordByAddr(Item.btcAddr)))
                    {
                        this.DealWithSuccess(ref Item);
                    }

                }
            }
            int Money = 2000;//单位为分
            public string Detail(taskcopy item)
            {
                //new SingleTask(TaskCopyType.SelectDriver),//选取司机  0
                //    new SingleTask(TaskCopyType.MagicRelease),//法术释放  1
                //    new SingleTask(TaskCopyType.DiamondCollect, 5),//宝石选择  2
                //    new SingleTask(TaskCopyType.DiamondUse),//宝石使用  3
                //    new SingleTask(TaskCopyType.AbilityTakeApart),//释放宝石  4
                //    new SingleTask(TaskCopyType.DiamondSell),//宝石出售  5
                //    new SingleTask(TaskCopyType.MoneySave),//积分存储  6
                //    new SingleTask(TaskCopyType.GetBless),//祈福  7
                //    new SingleTask(TaskCopyType.ChallengeNPCL2),//挑战2级NPC  //8
                //    new SingleTask(TaskCopyType.StockToStockAddr),//将股份转至StockAddr //9
                //    new SingleTask(TaskCopyType.AddrHasBindWord),//拥有绑定词  //10
                //    new SingleTask(TaskCopyType.RewardDeveloper),//打赏 //11
                //    new SingleTask(TaskCopyType.RedLuckyMoneyToPlayer),//给发红包 //12
                //    new SingleTask(TaskCopyType.Finished),//完成 //13
                switch (item.firstRound)
                {
                    case 0:
                        {
                            return "调整视角，选择自己的旗帜(绿色)，点击招募后，选取司机。";
                        };
                    case 1:
                        {
                            return "选取司机后，释放任意一次法术。包括雷法、水法、火法、混乱、迷惑、潜伏、加防、红牛、提速。";
                        };
                    case 2:
                        {
                            return $"将视角对准宝石，完成收集宝石({item.secondRound}/{this.chain[item.firstRound].TaskCopyTime})。";
                        };
                    case 3: return $"调整视角，将视角对准自己旗帜旁边的立柱，使用宝石。";
                    case 4: return $"在使用完宝石后，有能力提升后，调整视角，将视角对准自己的旗帜，完成一次释玉操作。";
                    case 5: return $"将宝石进行出售，获取积分。";
                    case 6: return $"将积分进行存储。点击左下角，从左往右第二按钮，点击，然后存储。";
                    case 7: return $"视角对准建筑物，进行一次求福";
                    case 8: return $"挑战一次二级NPC，或者二级以上的NPC";
                    case 9: return $"将{this.GetTaskHappenPlaceName(item)}的股份（地址对应的比特币地址为{this.GetItemBussinessAddr(item)}），从{item.btcAddr}转至{this.stockAddr}，至少转1Satoshi。(视角对准{this.GetTaskHappenPlaceName(item)}，点击详情，然后生成转让协议，并用私钥签名)。可查看攻略。";
                    case 10:
                        {
                            string bindWord = DalOfAddress.BindWordInfo.GetWordByAddr(item.btcAddr);
                            if (string.IsNullOrEmpty(bindWord))
                            {
                                return $"请赋予地址:{item.btcAddr}绑定词。主页面，攻略->打赏->绑定地址与汉字词语";
                            }
                            else
                            {
                                this.DealWithSuccess(ref item);
                                return Detail(item);
                            }
                        };
                    case 11:
                        {
                            {
                                string bindWord = DalOfAddress.BindWordInfo.GetWordByAddr(item.btcAddr);
                                if (string.IsNullOrEmpty(bindWord))
                                {
                                    var oldFirst = item.firstRound;
                                    var oldSecond = item.secondRound;

                                    item.firstRound--;
                                    item.secondRound = 0;
                                    DalOfAddress.TaskCopy.UpdateRoundOfItem(item, oldFirst, oldSecond);
                                    return Detail(item);
                                }
                                else
                                {
                                    if (item.Result == null)
                                    {
                                        StockAddrC sac = Newtonsoft.Json.JsonConvert.DeserializeObject<StockAddrC>(item.Tag);
                                        sac.CheckWord = CommonClass.Random.GetMD5HashFromStr(bindWord + DateTime.Now.ToString() + this.Money).Substring(0, 6).ToUpper();

                                        item.ResultDateTime = DateTime.Now;
                                        item.Result = this.Money;
                                        item.Tag = Newtonsoft.Json.JsonConvert.SerializeObject(sac);
                                        DalOfAddress.TaskCopy.UpdateResultOfItem(item);
                                    }
                                    int moneyValue = item.Result.Value;
                                    return $"通过微信赞赏码打赏或支付宝转账。至少打赏0.01元。可获{(moneyValue / 100.00m).ToString("F2")}元红包。记得将备注填写为绑定词({bindWord})。打赏后，管理员会再24小时内确认。";
                                }
                            }
                        };
                    case 12:
                        {
                            StockAddrC sac = Newtonsoft.Json.JsonConvert.DeserializeObject<StockAddrC>(item.Tag);
                            return $"提供验证码与地址，找要瑞卿,微信号(Adler_Yao)，索要${(item.Result.Value / 100m).ToString("F2")}元红包。验证码为{sac.CheckWord},地址为{item.btcAddr}";
                        }
                    default:
                        {
                            return "任务完成，取消任务后，可以重复领取任务。";
                        }
                }
                //  throw new NotImplementedException();
            }

            private string GetTaskHappenPlaceName(taskcopy item)
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<StockAddrC>(item.Tag).HappenPlace;
                //throw new NotImplementedException();
            }

            public void Pass(taskcopy Item)
            {
                if (this.GetCurrent(Item) == TaskCopyType.RedLuckyMoneyToPlayer)
                {
                    this.DealWithSuccess(ref Item);
                }
            }

            public void AddReferer(int refererCount, taskcopy item)
            {
                //throw new NotImplementedException();
            }
        }
    }


}
