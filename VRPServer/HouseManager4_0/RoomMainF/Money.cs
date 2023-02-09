using CommonClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager4_0.RoomMainF
{
    public partial class RoomMain : interfaceOfHM.Money
    {
        // Dictionary<int, int> _collectPosition;

        public void SetMoneyCanSave(Player player, ref List<string> notifyMsg)
        {
            var url = player.FromUrl;
            MoneyForSaveNotify tn = new MoneyForSaveNotify()
            {
                c = "MoneyForSaveNotify",
                WebSocketID = player.WebSocketID,
                MoneyForSave = player.MoneyForSave
            };
            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(tn);
            notifyMsg.Add(url);
            notifyMsg.Add(sendMsg);
        }
        public void MoneyChanged(Player player, long money, ref List<string> notifyMsg)
        {
            var url = player.FromUrl;

            MoneyNotify mn = new MoneyNotify()
            {
                c = "MoneyNotify",
                WebSocketID = player.WebSocketID,
                Money = money
            };

            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(mn);
            notifyMsg.Add(url);
            notifyMsg.Add(sendMsg);
            // throw new NotImplementedException();
        }

        public void LookFor(GetRandomPos gp)
        {
            lock (this.PlayerLock)
            {
                this._collectPosition = new Dictionary<int, int>();
                this.SetLookForPromote(gp);
                SetLookForMoney(gp);
            }
        }
        public void SetLookForMoney(GetRandomPos gp)
        {
            /*
             * 0->100.00
             * 1,2->50.00
             * 3,4,5,6,7->20.00
             * 8,9,10,11,12,13,14,15,16,17->10.00
             * 18-37->5.00
             */


            for (var i = 0; i < 38; i++)
            {
                this._collectPosition.Add(i, GetRandomPosition(true, gp));
                //  throw new NotImplementedException();
            }
        }

        public string SaveMoney(SaveMoney saveMoney)
        {
            // var doSaveMoney = false;
            long money = 0;
            List<string> notifyMsg = new List<string>();
            lock (this.PlayerLock)
            {
                if (this._Players.ContainsKey(saveMoney.Key))
                {
                    if (this._Players[saveMoney.Key].Bust) { }
                    else
                    {
                        var role = this._Players[saveMoney.Key];
                        switch (saveMoney.dType)
                        {
                            case "half":
                                {
                                    money = this._Players[saveMoney.Key].MoneyForSave / 2;
                                    if (money > 0)
                                    {
                                        //  doSaveMoney = true;
                                        role.MoneySet(role.Money - money, ref notifyMsg);
                                        if (role.playerType == RoleInGame.PlayerType.player)
                                            taskM.MoneySet((Player)role);
                                    }
                                }; break;
                            case "all":
                                {
                                    money = this._Players[saveMoney.Key].MoneyForSave;
                                    if (money > 0)
                                    {
                                        //  doSaveMoney = true;
                                        role.MoneySet(role.Money - money, ref notifyMsg);
                                        if (role.playerType == RoleInGame.PlayerType.player)
                                            taskM.MoneySet((Player)role);
                                    }
                                }; break;
                        }
                        if (role.playerType == RoleInGame.PlayerType.player)
                        {
                            var player = (Player)role;
                            if (player.RefererCount > 0)
                            {
                                if (BitCoin.CheckAddress.CheckAddressIsUseful(player.RefererAddr))
                                {
                                    DalOfAddress.MoneyRefererAdd.AddMoney(player.RefererAddr, player.RefererCount * 100);
                                    var tasks = DalOfAddress.TaskCopy.GetALLItem(player.RefererAddr);
                                    this.taskM.AddReferer(player.RefererCount, tasks);
                                    player.RefererCount = 0;
                                }


                            }
                        }
                    }
                }
            }
            Startup.sendSeveralMsgs(notifyMsg);

            if (money > 0)
            {
                DalOfAddress.MoneyAdd.AddMoney(saveMoney.address, money);
                //Thread th = new Thread(() => );
                //th.Start();
            }
            return "";
        }

        public void OrderToSubsidize(OrderToSubsidize ots)
        {
            List<string> notifyMsg = new List<string>();
            if (BitCoin.Sign.checkSign(ots.signature, ots.Key, ots.address))
            {
                lock (this.PlayerLock)
                    if (this._Players.ContainsKey(ots.Key))
                    {
                        if (!this._Players[ots.Key].Bust)
                        {
                            var success = this.modelL.OrderToUpdateLevel(ots.Key, ots.address, ots.signature);
                            if (success)
                            {
                                var Referer = DalOfAddress.MoneyRefererAdd.GetMoney(ots.address);
                                if (Referer > 0)
                                {
                                    DalOfAddress.MoneyRefererGet.GetSubsidizeAndLeft(ots.address, Referer);
                                }
                                {
                                    long subsidizeGet, subsidizeLeft;
                                    DalOfAddress.MoneyGet.GetSubsidizeAndLeft(ots.address, ots.value, out subsidizeGet, out subsidizeLeft);
                                    var player = this._Players[ots.Key];
                                    ((Player)player).BTCAddress = ots.address;
                                    this.taskM.Initialize(((Player)player));
                                    //this.taskM.MoneyRefererAdd(ots.address);
                                    player.MoneySet(player.Money + subsidizeGet + Referer, ref notifyMsg);
                                    if (Referer > 0)
                                    {
                                        this.WebNotify(player, $"您的热心分享使您获得了额外的{Referer / 100}.{(Referer % 100) / 10}{(Referer % 100) % 10}积分。");
                                    }
                                    //  player.setSupportToPlayMoney(player.SupportToPlayMoney + subsidizeGet, ref notifyMsg);
                                    if (player.playerType == RoleInGame.PlayerType.player)
                                        this.SendLeftMoney((Player)player, subsidizeLeft, ots.address, ref notifyMsg);
                                }
                            }
                        }
                    }
            }
            else
            {
                //这里在web前台进行校验。
            }

            Startup.sendSeveralMsgs(notifyMsg);
        }

        private void SendLeftMoney(Player player, long subsidizeLeft, string address, ref List<string> notifyMsg)
        {
            var url = player.FromUrl;
            LeftMoneyInDB lmdb = new LeftMoneyInDB()
            {
                c = "LeftMoneyInDB",
                WebSocketID = player.WebSocketID,
                Money = subsidizeLeft,
                address = address
            };
            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(lmdb);
            notifyMsg.Add(url);
            notifyMsg.Add(sendMsg);
        }
    }
}
