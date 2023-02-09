using CommonClass.databaseModel;
using DalOfAddress;
using HouseManager4_0.RoomMainF;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager4_0
{
    public partial class Manager_TaskCopy
    {
        internal void AddReferer(int refererCount, List<taskcopy> taskCopys)
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
                        case Share.TaskCode:
                            {
                                f = this.sha;
                            }; break;

                    }
                    if (f != null)
                    {
                        f.AddReferer(refererCount, Item);
                    }
                }
            }
            catch { }
        }
        protected partial class Share : TaskChain
        {
            public const string TaskCode = "URLSHARE";
            private RoomMain roomMain;

            public Share(RoomMain roomMain)
            {
                this.roomMain = roomMain;
            }
        }
        protected partial class Share : TaskCopyCallF
        {
            //  string TaskCopyCallF.TaskName { get { } }
            public string TaskName { get { return "游戏链接热心分享"; } }

            public void AddReferer(int refererCount, taskcopy item)
            {
                item.Result = item.Result.Value + refererCount;
                DalOfAddress.TaskCopy.UpdateResultOfItem(item);
            }

            public void Pass(taskcopy item)
            {
                var shareValue = item.Result.Value;
                int roundValue = shareValue / passUnit;
                if (roundValue > item.firstRound && (!string.IsNullOrEmpty(item.Tag)))
                {
                    var oldFirst = item.firstRound;
                    var oldSecond = item.secondRound;
                    item.firstRound = item.firstRound + 1;
                    item.Tag = "";
                    DalOfAddress.TaskCopy.UpdateResultOfItem(item);
                    DalOfAddress.TaskCopy.UpdateRoundOfItem(item, oldFirst, oldSecond);
                }
            }

            //class StockAddrC
            //{
            //    public string BAddr { get; set; }
            //    public string CheckWord { get; set; }
            //    public string Name { get; set; }
            //    public string HappenPlace { get; set; }
            //}
            bool TaskCopyCallF.Add(string btcAddr)
            {


                string Tag = "";
                CommonClass.databaseModel.taskcopy tc = new CommonClass.databaseModel.taskcopy()
                {
                    btcAddr = btcAddr,
                    firstRound = 0,
                    secondRound = 0,
                    Result = 0,
                    ResultDateTime = DateTime.Now,
                    Tag = Tag,
                    taskCopyCode = TaskCode
                };
                return DalOfAddress.TaskCopy.Add(tc);
            }

            bool TaskCopyCallF.BindWordInfoF(taskcopy item)
            {
                return false;
            }

            void TaskCopyCallF.ChargingF(taskcopy item)
            {
            }

            const int passUnit = 50000;//这里是积分，省略了后面的两位。
            string TaskCopyCallF.Detail(taskcopy item)
            {
                var shareValue = item.Result.Value;
                int roundValue = shareValue / passUnit;
                if (roundValue > item.firstRound)
                {
                    if (string.IsNullOrEmpty(item.Tag))
                    {
                        item.Tag = CommonClass.Random.GetMD5HashFromStr("Adler_Yao" + DateTime.Now.ToString() + shareValue).Substring(0, 6).ToUpper();
                        DalOfAddress.TaskCopy.UpdateResultOfItem(item);
                    }

                    return $"由于您热心分享附带{item.btcAddr}分享的链接，您总共额外获得了{item.Result.Value}.00积分。提供验证码与地址，找微信号(Adler_Yao)，索要￥{(50.00).ToString("F2")}元红包。验证码为{item.Tag},地址为{item.btcAddr}";
                }
                else
                {
                    return $"由于您热心分享附带{item.btcAddr}分享的链接，您总共额外获得了{item.Result.Value}.00积分。通过分享，达到{(item.firstRound + 1) * passUnit}.00积分，可以获得￥{(50.00).ToString("F2")}元红包";
                }
                // 

            }

            void TaskCopyCallF.DiamondSell(taskcopy item, Player player, Action<RoleInGame, string> webNotify)
            {
            }

            void TaskCopyCallF.DoCollect(taskcopy Item, Player player, Action<RoleInGame, string> WebNotify)
            {
            }

            void TaskCopyCallF.DriverSelected(taskcopy item, Player player, Action<RoleInGame, string> webNotify)
            {
            }

            void TaskCopyCallF.GetRewardFromBuildingF(taskcopy item, Player player, Action<RoleInGame, string> webNotify)
            {
            }

            string TaskCopyCallF.GetTaskName(taskcopy Item)
            {
                return this.TaskName;
            }

            void TaskCopyCallF.MagicReleased(taskcopy item, Player player, Action<RoleInGame, string> webNotify)
            {
            }

            void TaskCopyCallF.MoneySet(taskcopy item, Player player, Action<RoleInGame, string> webNotify)
            {
            }

            void TaskCopyCallF.OrderToUpdateLevel(taskcopy item, Player player, Action<RoleInGame, string> webNotify)
            {
            }

            void TaskCopyCallF.TakeApartF(taskcopy item, Player player, Action<RoleInGame, string> webNotify)
            {
            }

            void TaskCopyCallF.TradeCoinF(string addrBussiness, string addrTo, long passCoin, taskcopy item, Player[] players, Action<RoleInGame, string> webNotify)
            {
            }

            void TaskCopyCallF.UseDiamondSuccess(taskcopy item, Player player, Action<RoleInGame, string> webNotify)
            {
            }
        }
    }
}
