using HouseManager4_0;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace TestCore.RewardGivings
{
    internal class StockGet
    {
        protected DateTime startTime;
        protected Geometry.Boundary boundary;
        protected Data dt;
        protected HouseManager4_0.RoomMainF.RoomMain rm;
        //   static NPC npc;
        protected Player player1;
        // static string npcKey = "";
        protected string player1Key = "b0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b0";

        protected Player player2;
        protected string player2Key = "c0c0c0c0c0c0c0c0c0c0c0c0c0c0c0c0";


        CommonClass.ModelTranstraction.TradeCoin.Result r1 = null;
        CommonClass.ModelTranstraction.TradeCoin.Result r2 = null;
        CommonClass.ModelTranstraction.TradeCoin.Result r3 = null;
        CommonClass.ModelTranstraction.TradeSetAsReward.Result r4 = null;
        CommonClass.ModelTranstraction.TradeSetAsReward.Result r4_0 = null;
        CommonClass.ModelTranstraction.TradeCoin.Result r5 = null;
        CommonClass.ModelTranstraction.TradeCoin.Result r6 = null;
        CommonClass.ModelTranstraction.TradeCoin.Result r7 = null;
        CommonClass.ModelTranstraction.AwardsGivingPass.Result r8 = null;
        CommonClass.ModelTranstraction.AwardsGivingPass.Result r9 = null;
        [SetUp]
        public async void Setup()
        {
            /*
             *  此测试模拟股份交易，测试股份转让，包括资金够，资金不够
             *  此测试模拟股份交易，测试股份提取，包括资金够，资金不够，验证交易后的数据。
             */

            // Process.
            //   Process p = Process.Start("program.exe");

            List<string> msgs = new List<string>();
            //  this.check3Addr = BitCoin.Sign.checkSign("H3RsQHvimR1BztjXooOZWl4EYTB3v71lDfx4CYDXHvfNVeUg2J6JYfg+2T5btd1i63+gOfhaZV5D4+A6ahR7tgE=", "20220927", "354vT5hncSwmob6461WjhhfWmaiZgHuaSK");
            this.startTime = DateTime.Now;

            this.boundary = new Geometry.Boundary();
            boundary.load(true);

            this.dt = new Data();
            this.dt.LoadRoad(this.boundary, true);
            //Program.dt.LoadModel();
            //Program.dt.LoadCrossBackground();

            //Program.rm = new RoomMainF.RoomMain();

            rm = new HouseManager4_0.RoomMainF.RoomMain(this.dt);

            // HasRewardAddr4.npcKey = rm.NPCM.AddNpcPlayer(2, rm, dt);

            rm.AddPlayer(new CommonClass.PlayerAdd_V2()
            {
                c = "PlayerAdd_V2",
                Key = this.player1Key
            }, rm, dt
            );
            player1 = (Player)rm._Players[player1Key];

            rm.AddPlayer(new CommonClass.PlayerAdd_V2()
            {
                c = "PlayerAdd_V2",
                Key = this.player2Key
            }, rm, dt
            );
            player2 = (Player)rm._Players[player2Key];
            long moneyInDB = DalOfAddress.MoneyAdd.GetMoney("3FkXatYUQv81t7mDsQf8igumGnp1mE1gkk");
            long subsidizeGet, subsidizeLeft;
            DalOfAddress.MoneyGet.GetSubsidizeAndLeft("3FkXatYUQv81t7mDsQf8igumGnp1mE1gkk", moneyInDB, out subsidizeGet, out subsidizeLeft);

            DalOfAddress.MoneyAdd.AddMoney("3FkXatYUQv81t7mDsQf8igumGnp1mE1gkk", DalOfAddress.TradeRecord.TradeStockCost * 5 / 2);

            DalOfAddress.TradeRecord.RemoveByBussinessAddr("3BkRN6seLYYgSqGQNX3o1poEzCXhGK3vTs");

            DalOfAddress.TradeReward.RemoveByBussinessAddr("3BkRN6seLYYgSqGQNX3o1poEzCXhGK3vTs");

            var msg1 = rm.TradeCoinF(new CommonClass.ModelTranstraction.TradeCoin()
            {
                c = "TradeCoin",
                addrBussiness = "3BkRN6seLYYgSqGQNX3o1poEzCXhGK3vTs",
                addrFrom = "3FkXatYUQv81t7mDsQf8igumGnp1mE1gkk",
                addrTo = "3FkXatYUQv81t7mDsQf8igumGnp1mE1gkk",
                msg = "0@3FkXatYUQv81t7mDsQf8igumGnp1mE1gkk@3BkRN6seLYYgSqGQNX3o1poEzCXhGK3vTs->3FkXatYUQv81t7mDsQf8igumGnp1mE1gkk:100000Satoshi",
                sign = "H2+2s6cQkBDfEjk6z9yDLeQW9tQaEqMRGcGX/DIgPCp9eFXWIerlaT+zlDR5v7O23TY4bCT/JCh7B/JB0HkiHQk=",
                passCoin = 100000,
                tradeIndex = 0
            });
            this.r1 = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.TradeCoin.Result>(msg1);


            Thread.Sleep(100);

            var msg2 = rm.TradeCoinF(new CommonClass.ModelTranstraction.TradeCoin()
            {
                c = "TradeCoin",
                addrBussiness = "3BkRN6seLYYgSqGQNX3o1poEzCXhGK3vTs",
                addrFrom = "3FkXatYUQv81t7mDsQf8igumGnp1mE1gkk",
                addrTo = "354vT5hncSwmob6461WjhhfWmaiZgHuaSK",
                msg = "1@3FkXatYUQv81t7mDsQf8igumGnp1mE1gkk@3BkRN6seLYYgSqGQNX3o1poEzCXhGK3vTs->354vT5hncSwmob6461WjhhfWmaiZgHuaSK:100000Satoshi",
                sign = "IGFLMlNdKARDAjdEvwSB9KFoBCFKpGVJRoSzVEshaRyzYWvb7EuX/txVvBAHEduN0veMSEor4bGX93kKv4ZiAl4=",
                passCoin = 100000,
                tradeIndex = 1
            });
            this.r2 = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.TradeCoin.Result>(msg2);
            Thread.Sleep(100);

            var msg3 = rm.TradeCoinF(new CommonClass.ModelTranstraction.TradeCoin()
            {
                c = "TradeCoin",
                addrBussiness = "3BkRN6seLYYgSqGQNX3o1poEzCXhGK3vTs",
                addrFrom = "3FkXatYUQv81t7mDsQf8igumGnp1mE1gkk",
                addrTo = "1NyrqNVai7uCP4CwZoDfQVAJ6EbdgaG6bg",
                msg = "2@3FkXatYUQv81t7mDsQf8igumGnp1mE1gkk@3BkRN6seLYYgSqGQNX3o1poEzCXhGK3vTs->1NyrqNVai7uCP4CwZoDfQVAJ6EbdgaG6bg:400000Satoshi",
                sign = "ID2QV7nQHjpzcDmiZX8YXYNXzhJoMU8FD5w1OgLMxHjzJ9cRZMf8rCtvxXsdFYkxqDNiTvxpy10A6wn0RfipKyU=",
                passCoin = 400000,
                tradeIndex = 2
            });
            this.r3 = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.TradeCoin.Result>(msg3);
            Thread.Sleep(100);


            var msg4 = rm.TradeSetAsRewardF(new CommonClass.ModelTranstraction.TradeSetAsReward()
            {
                msg = "2@3FkXatYUQv81t7mDsQf8igumGnp1mE1gkk@3BkRN6seLYYgSqGQNX3o1poEzCXhGK3vTs->SetAsReward:100000Satoshi",
                addrBussiness = "3BkRN6seLYYgSqGQNX3o1poEzCXhGK3vTs",
                tradeIndex = 2,
                afterWeek = 3,
                c = "TradeSetAsReward",
                addrReward = "3FkXatYUQv81t7mDsQf8igumGnp1mE1gkk",
                passCoin = 100000,
                signOfAddrReward = "IB2exxQxuNqrEsToZHEPeVy39vTkvPpfpQT5zZYhBsiED2WTBNSD4fsH6db6Am5JGEoLrQip929d34dJi1nCqXk=",
                signOfaddrBussiness = "HwtQqGDxy/2gcvSxfQY8rdsl7CV0e4W7mPErRf6BNuQEMpYbLwpTf7A1Ceq7RSXSKRtS/0ODA42ikF/gFXIS4d4="
            });
            this.r4 = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.TradeSetAsReward.Result>(msg4);
            Thread.Sleep(100);

            msg3 = rm.TradeCoinF(new CommonClass.ModelTranstraction.TradeCoin()
            {
                c = "TradeCoin",
                addrBussiness = "3BkRN6seLYYgSqGQNX3o1poEzCXhGK3vTs",
                addrFrom = "3FkXatYUQv81t7mDsQf8igumGnp1mE1gkk",
                addrTo = "1NyrqNVai7uCP4CwZoDfQVAJ6EbdgaG6bg",
                msg = "2@3FkXatYUQv81t7mDsQf8igumGnp1mE1gkk@3BkRN6seLYYgSqGQNX3o1poEzCXhGK3vTs->1NyrqNVai7uCP4CwZoDfQVAJ6EbdgaG6bg:400000Satoshi",
                sign = "ID2QV7nQHjpzcDmiZX8YXYNXzhJoMU8FD5w1OgLMxHjzJ9cRZMf8rCtvxXsdFYkxqDNiTvxpy10A6wn0RfipKyU=",
                passCoin = 400000,
                tradeIndex = 2
            });
            this.r5 = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.TradeCoin.Result>(msg3);
            Thread.Sleep(100);

            DalOfAddress.TradeRecord.AddResult r;

            CommonClass.ModelTranstraction.AwardsGivingPass passObj;
            {
                var applyAddr = new List<string>();
                var list = new List<string>();
                var msgsPass = new List<string>();
                var ranks = new List<int>();
                var timeStr = r4.msg.Substring(0, 8);
                passObj = new CommonClass.ModelTranstraction.AwardsGivingPass()
                {
                    c = "AwardsGivingPass",
                    applyAddr = applyAddr,
                    list = list,
                    msgs = msgsPass,
                    ranks = ranks,
                    time = timeStr,

                };
            }
            var msg5 = rm.AwardsGive(passObj, true);
            r8 = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.AwardsGivingPass.Result>(msg5);
            Thread.Sleep(100);

            msg3 = rm.TradeCoinF(new CommonClass.ModelTranstraction.TradeCoin()
            {
                c = "TradeCoin",
                addrBussiness = "3BkRN6seLYYgSqGQNX3o1poEzCXhGK3vTs",
                addrFrom = "3FkXatYUQv81t7mDsQf8igumGnp1mE1gkk",
                addrTo = "1NyrqNVai7uCP4CwZoDfQVAJ6EbdgaG6bg",
                msg = "2@3FkXatYUQv81t7mDsQf8igumGnp1mE1gkk@3BkRN6seLYYgSqGQNX3o1poEzCXhGK3vTs->1NyrqNVai7uCP4CwZoDfQVAJ6EbdgaG6bg:400000Satoshi",
                sign = "ID2QV7nQHjpzcDmiZX8YXYNXzhJoMU8FD5w1OgLMxHjzJ9cRZMf8rCtvxXsdFYkxqDNiTvxpy10A6wn0RfipKyU=",
                passCoin = 400000,
                tradeIndex = 2
            });
            this.r6 = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.TradeCoin.Result>(msg3);
            Thread.Sleep(100);

            DalOfAddress.MoneyAdd.AddMoney("3FkXatYUQv81t7mDsQf8igumGnp1mE1gkk", DalOfAddress.TradeRecord.TradeStockCost * 5 / 2);
            msg3 = rm.TradeCoinF(new CommonClass.ModelTranstraction.TradeCoin()
            {
                c = "TradeCoin",
                addrBussiness = "3BkRN6seLYYgSqGQNX3o1poEzCXhGK3vTs",
                addrFrom = "3FkXatYUQv81t7mDsQf8igumGnp1mE1gkk",
                addrTo = "1NyrqNVai7uCP4CwZoDfQVAJ6EbdgaG6bg",
                msg = "2@3FkXatYUQv81t7mDsQf8igumGnp1mE1gkk@3BkRN6seLYYgSqGQNX3o1poEzCXhGK3vTs->1NyrqNVai7uCP4CwZoDfQVAJ6EbdgaG6bg:400000Satoshi",
                sign = "ID2QV7nQHjpzcDmiZX8YXYNXzhJoMU8FD5w1OgLMxHjzJ9cRZMf8rCtvxXsdFYkxqDNiTvxpy10A6wn0RfipKyU=",
                passCoin = 400000,
                tradeIndex = 2
            });
            this.r7 = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.TradeCoin.Result>(msg3);


            Thread.Sleep(100);

            //  DalOfAddress.TradeRecord.AddResult r;


            msg5 = rm.AwardsGive(passObj, true);
            r9 = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.AwardsGivingPass.Result>(msg5);
            Thread.Sleep(100);


            msg4 = rm.TradeSetAsRewardF(new CommonClass.ModelTranstraction.TradeSetAsReward()
            {
                msg = "2@3FkXatYUQv81t7mDsQf8igumGnp1mE1gkk@3BkRN6seLYYgSqGQNX3o1poEzCXhGK3vTs->SetAsReward:100000Satoshi",
                addrBussiness = "3BkRN6seLYYgSqGQNX3o1poEzCXhGK3vTs",
                tradeIndex = 2,
                afterWeek = 3,
                c = "TradeSetAsReward",
                addrReward = "3FkXatYUQv81t7mDsQf8igumGnp1mE1gkk",
                passCoin = 100000,
                signOfAddrReward = "IB2exxQxuNqrEsToZHEPeVy39vTkvPpfpQT5zZYhBsiED2WTBNSD4fsH6db6Am5JGEoLrQip929d34dJi1nCqXk=",
                signOfaddrBussiness = "HwtQqGDxy/2gcvSxfQY8rdsl7CV0e4W7mPErRf6BNuQEMpYbLwpTf7A1Ceq7RSXSKRtS/0ODA42ikF/gFXIS4d4="
            });
            this.r4_0 = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.TradeSetAsReward.Result>(msg4);
            Thread.Sleep(100);

            msg4 = rm.TradeSetAsRewardF(new CommonClass.ModelTranstraction.TradeSetAsReward()
            {
                msg = "3@3FkXatYUQv81t7mDsQf8igumGnp1mE1gkk@3BkRN6seLYYgSqGQNX3o1poEzCXhGK3vTs->SetAsReward:100000Satoshi",
                addrBussiness = "3BkRN6seLYYgSqGQNX3o1poEzCXhGK3vTs",
                tradeIndex = 3,
                afterWeek = 4,
                c = "TradeSetAsReward",
                addrReward = "3FkXatYUQv81t7mDsQf8igumGnp1mE1gkk",
                passCoin = 100000,
                signOfAddrReward = "H0nsf1+Dvu+8w4ASpNSBWRHroisi6j08a3XjxO/NmRPnMtsmVHGXRwgRAjEaC2yUvNsNhwdpjLvl0WP82u8TDvg=",
                signOfaddrBussiness = "IEvDTpHEOm7ZGV2gzxKKiM50C+kYb7js/CAjQz394VB5bo058VxGE1awt4DEm7x3gs55Ad5mCpXARBRIny4Luww="
            });
            this.r4_0 = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.TradeSetAsReward.Result>(msg4);


            Thread.Sleep(100);

            //List<string> privateKeys = new List<string>();
            //List<string> address = new List<string>();
            //List<int> levels = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                string addr;
                var p = BitCoin.PrivateKeyF.getPrivateByString($"test{i}", out addr);
                // levels.Add(i % 4 + 1);
                DalOfAddress.LevelForSave.UpdateResultInDB rInDB;
                DalOfAddress.LevelForSave.Update(addr, "", i % 4 + 2, out rInDB);
                //DalOfAddress.LevelForSave.a
                var msg6 = rm.RewardApplyF(new CommonClass.ModelTranstraction.RewardApply()
                {
                    c = "RewardApply",
                    addr = addr,
                    msgNeedToSign = this.r4_0.msg.Substring(0, 8),
                    signature = BitCoin.Sign.SignMessage(p, this.r4_0.msg.Substring(0, 8), addr)
                }, true);

            }
            Thread.Sleep(100);


            msg2 =   rm.TradeCoinF(new CommonClass.ModelTranstraction.TradeCoin()
            {
                c = "TradeCoin",
                addrBussiness = "3BkRN6seLYYgSqGQNX3o1poEzCXhGK3vTs",
                addrFrom = "354vT5hncSwmob6461WjhhfWmaiZgHuaSK",
                addrTo = "3BkRN6seLYYgSqGQNX3o1poEzCXhGK3vTs",
                msg = "0@354vT5hncSwmob6461WjhhfWmaiZgHuaSK@3BkRN6seLYYgSqGQNX3o1poEzCXhGK3vTs->3BkRN6seLYYgSqGQNX3o1poEzCXhGK3vTs:100000Satoshi",
                sign = "IBL9obJaeTvCRfGlOW6c9zhvBElagWE2TBAIMOvcbC7sfduxWLsiwjExARoRANpEkijTWqloUZMfgpZcpeBqKr0=",
                passCoin = 100000,
                tradeIndex = 0
            });
            var rrr = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.TradeCoin.Result>(msg2);

            if (rrr.success)
            {
                Thread.Sleep(120);
            }
            Thread.Sleep(100);
            //   this.rm.
            //0@354vT5hncSwmob6461WjhhfWmaiZgHuaSK@3BkRN6seLYYgSqGQNX3o1poEzCXhGK3vTs->3BkRN6seLYYgSqGQNX3o1poEzCXhGK3vTs:100000Satoshi
            //IBL9obJaeTvCRfGlOW6c9zhvBElagWE2TBAIMOvcbC7sfduxWLsiwjExARoRANpEkijTWqloUZMfgpZcpeBqKr0=

            //20221121
            //20221121	3	3FkXatYUQv81t7mDsQf8igumGnp1mE1gkk	3BkRN6seLYYgSqGQNX3o1poEzCXhGK3vTs	100000	1	H0nsf1+Dvu+8w4ASpNSBWRHroisi6j08a3XjxO/NmRPnMtsmVHGXRwgRAjEaC2yUvNsNhwdpjLvl0WP82u8TDvg=	IEvDTpHEOm7ZGV2gzxKKiM50C+kYb7js/CAjQz394VB5bo058VxGE1awt4DEm7x3gs55Ad5mCpXARBRIny4Luww=	3@3FkXatYUQv81t7mDsQf8igumGnp1mE1gkk@3BkRN6seLYYgSqGQNX3o1poEzCXhGK3vTs->SetAsReward:100000Satoshi

        }

        [Test]
        public void TestTradeMoneyCost1()
        {
            /*用于验证最后一次读，才有写的权力*/
            if (r1.success && r2.success && (!r3.success) && r4.success && (!r5.success) && (!r6.success) && r7.success && r8.success)
            {
                if (r1.msg == DalOfAddress.TradeRecord.MsgSuccess &&
                    r2.msg == DalOfAddress.TradeRecord.MsgSuccess &&
                    r3.msg == DalOfAddress.TradeRecord.MsgMoenyIsNotEnough &&
                    r4.msg.Contains("录入数据成功！") &&
                    r5.msg == DalOfAddress.TradeRecord.MsgMoneyIsLocked("3FkXatYUQv81t7mDsQf8igumGnp1mE1gkk") &&
                    r6.msg == DalOfAddress.TradeRecord.MsgMoenyIsNotEnough &&
                    r7.msg == DalOfAddress.TradeRecord.MsgSuccess &&
                    r8.msg == "颁奖成功")
                    Assert.Pass();
                else
                    Assert.Fail();
            }
            else
            {
                Assert.Fail();
            }
        }
    }
}
