using HouseManager4_0;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace TestCore.LevelTest
{
    public class HasRewardAddr2
    {
        static DateTime startTime;
        static Geometry.Boundary boundary;
        static Data dt;
        static HouseManager4_0.RoomMainF.RoomMain rm;
        static NPC npc;
        static Player player;
        static string npcKey = "";
        static string playerKey = "b0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b0";

        [SetUp]
        public void Setup()
        {
            /*
             *  此测试模拟一个玩家，先登录，然后登录，数据库中更新！ 
             */

            List<string> msgs = new List<string>();
            //  this.check3Addr = BitCoin.Sign.checkSign("H3RsQHvimR1BztjXooOZWl4EYTB3v71lDfx4CYDXHvfNVeUg2J6JYfg+2T5btd1i63+gOfhaZV5D4+A6ahR7tgE=", "20220927", "354vT5hncSwmob6461WjhhfWmaiZgHuaSK");
            HasRewardAddr2.startTime = DateTime.Now;

            HasRewardAddr2.boundary = new Geometry.Boundary();
            boundary.load(true);

            HasRewardAddr2.dt = new Data();
            HasRewardAddr2.dt.LoadRoad(HasRewardAddr2.boundary, true);
            //Program.dt.LoadModel();
            //Program.dt.LoadCrossBackground();

            //Program.rm = new RoomMainF.RoomMain();

            rm = new HouseManager4_0.RoomMainF.RoomMain(HasRewardAddr2.dt);

            HasRewardAddr2.npcKey = rm.NPCM.AddNpcPlayer(2, rm, dt);

            rm.AddPlayer(new CommonClass.PlayerAdd_V2()
            {
                c = "PlayerAdd_V2",
                Key = HasRewardAddr2.playerKey
            }, rm, dt
            );
            player = (Player)rm._Players[playerKey];
            rm.OrderToSubsidize(new CommonClass.OrderToSubsidize()
            {
                c = "OrderToSubsidize",
                address = "1NyrqneGRxTpCohjJdwKruM88JyARB2Ljr",
                Key = playerKey,
                signature = "IFL0H3c9DYuLqRygW5FF4X/oCJQ9G43UVLZrGuh5ePNuQ+B5ibDkF56YqDtzi/qFI5xTZ4+7q7WEXziyXsvHRzU="
            });
            DalOfAddress.LevelForSave.Del("1NyrqneGRxTpCohjJdwKruM88JyARB2Ljr");

        }

        [Test]
        public void TestLevel()
        {
            /*此方法用于验证，player在没有BtcAddr的情况下生到了2级*/


            List<string> msgs = new List<string>();
            npc = (NPC)rm._Players[npcKey];
            rm.NPCM.NPCBeingAttacked(playerKey, npc, ref msgs, rm, dt);

            rm.NPCM.afterBroke(npc, ref msgs, dt);
            if (player.levelObj.Level == 2 && (!string.IsNullOrEmpty(player.levelObj.TimeStampStr)))
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }
    }
}
