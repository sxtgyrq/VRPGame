using HouseManager4_0;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace TestCore.RewardGivings
{
    internal class RewardGive
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
        public void Setup()
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
            rm.AwardsGive(new CommonClass.ModelTranstraction.AwardsGivingPass()
            {
                c = "AwardsGivingPass",
                applyAddr = new List<string>()
                {"1KJggTU6BvyH5GgREBo6wvqgzAg1s7BadX",
                "14tUca4QMMoFGYqAADnZfQM3o4g8L11USS",
                "1MuyvRhHdHRHDp9LNGS3kHBgnjw24Wm3EC",
                "1D2GBdBh5DHcZmXAhwtUVMVDJ9WpUWDAcb",
                "1LcP8Y6pzbngmM87WoFUHeM2yxAhMKSCk8",
                }
            }, true);


            //   this.rm.
            //0@354vT5hncSwmob6461WjhhfWmaiZgHuaSK@3BkRN6seLYYgSqGQNX3o1poEzCXhGK3vTs->3BkRN6seLYYgSqGQNX3o1poEzCXhGK3vTs:100000Satoshi
            //IBL9obJaeTvCRfGlOW6c9zhvBElagWE2TBAIMOvcbC7sfduxWLsiwjExARoRANpEkijTWqloUZMfgpZcpeBqKr0=

            //20221121
            //20221121	3	3FkXatYUQv81t7mDsQf8igumGnp1mE1gkk	3BkRN6seLYYgSqGQNX3o1poEzCXhGK3vTs	100000	1	H0nsf1+Dvu+8w4ASpNSBWRHroisi6j08a3XjxO/NmRPnMtsmVHGXRwgRAjEaC2yUvNsNhwdpjLvl0WP82u8TDvg=	IEvDTpHEOm7ZGV2gzxKKiM50C+kYb7js/CAjQz394VB5bo058VxGE1awt4DEm7x3gs55Ad5mCpXARBRIny4Luww=	3@3FkXatYUQv81t7mDsQf8igumGnp1mE1gkk@3BkRN6seLYYgSqGQNX3o1poEzCXhGK3vTs->SetAsReward:100000Satoshi

        }
    }
}
