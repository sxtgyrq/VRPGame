using HouseManager4_0;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace TestCore.LevelTest
{
    public class HasRewardAddr3
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
             *  此测试模拟一个玩家，数据库中已经3级了，但是他打败了2级后，进行了登录,数据库中有4级数据
             */

            List<string> msgs = new List<string>();
            //  this.check3Addr = BitCoin.Sign.checkSign("H3RsQHvimR1BztjXooOZWl4EYTB3v71lDfx4CYDXHvfNVeUg2J6JYfg+2T5btd1i63+gOfhaZV5D4+A6ahR7tgE=", "20220927", "354vT5hncSwmob6461WjhhfWmaiZgHuaSK");
            HasRewardAddr3.startTime = DateTime.Now;

            HasRewardAddr3.boundary = new Geometry.Boundary();
            boundary.load(true);

            HasRewardAddr3.dt = new Data();
            HasRewardAddr3.dt.LoadRoad(HasRewardAddr3.boundary, true);
            //Program.dt.LoadModel();
            //Program.dt.LoadCrossBackground();

            //Program.rm = new RoomMainF.RoomMain();

            rm = new HouseManager4_0.RoomMainF.RoomMain(HasRewardAddr3.dt);

            HasRewardAddr3.npcKey = rm.NPCM.AddNpcPlayer(2, rm, dt);

            rm.AddPlayer(new CommonClass.PlayerAdd_V2()
            {
                c = "PlayerAdd_V2",
                Key = HasRewardAddr3.playerKey
            }, rm, dt
            );
            player = (Player)rm._Players[playerKey];

            DalOfAddress.LevelForSave.Del("1BW4xpSiNBc9YMvMwzC7oatqT77jpzeJhH");
            DalOfAddress.LevelForSave.UpdateResultInDB r;
            DalOfAddress.LevelForSave.Update("1BW4xpSiNBc9YMvMwzC7oatqT77jpzeJhH", "", 4, out r);
        }

        [Test]
        public void TestLevel()
        {
            /*此方法用于验证，player在没有BtcAddr的情况下生到了2级*/


            List<string> msgs = new List<string>();
            npc = (NPC)rm._Players[npcKey];
            rm.NPCM.NPCBeingAttacked(playerKey, npc, ref msgs, rm, dt);

            rm.NPCM.afterBroke(npc, ref msgs, dt);

            rm.OrderToSubsidize(new CommonClass.OrderToSubsidize()
            {
                c = "OrderToSubsidize",
                address = "1BW4xpSiNBc9YMvMwzC7oatqT77jpzeJhH",
                Key = playerKey,
                signature = "IAyRSLp2U8opwn5XklCF8megj9tv7cjxvQKUjWi26EEtIdwampzIbTjIh9VHIobDX1yJRDaWIfZcmS7ZIDH+HK0="
            });
            if (player.levelObj.Level == 4 && (!string.IsNullOrEmpty(player.levelObj.TimeStampStr)))
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
