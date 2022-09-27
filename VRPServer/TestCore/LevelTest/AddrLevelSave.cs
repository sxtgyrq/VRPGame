using HouseManager4_0;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestCore.LevelTest
{
    public partial class AddrLevelSave
    {
        public class HasNoRewardAddr
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
                List<string> msgs = new List<string>();
                //  this.check3Addr = BitCoin.Sign.checkSign("H3RsQHvimR1BztjXooOZWl4EYTB3v71lDfx4CYDXHvfNVeUg2J6JYfg+2T5btd1i63+gOfhaZV5D4+A6ahR7tgE=", "20220927", "354vT5hncSwmob6461WjhhfWmaiZgHuaSK");
                HasNoRewardAddr.startTime = DateTime.Now;

                HasNoRewardAddr.boundary = new Geometry.Boundary();
                boundary.load(true);

                HasNoRewardAddr.dt = new Data();
                HasNoRewardAddr.dt.LoadRoad(HasNoRewardAddr.boundary, true);
                //Program.dt.LoadModel();
                //Program.dt.LoadCrossBackground();

                //Program.rm = new RoomMainF.RoomMain();

                rm = new HouseManager4_0.RoomMainF.RoomMain(HasNoRewardAddr.dt);

                HasNoRewardAddr.npcKey = rm.NPCM.AddNpcPlayer(2, rm, dt);

                rm.AddPlayer(new CommonClass.PlayerAdd_V2()
                {
                    c = "PlayerAdd_V2",
                    Key = HasNoRewardAddr.playerKey
                },
                rm, dt
                );



            }

            [Test]
            public void TestLevel()
            {
                /*此方法用于验证，player在没有BtcAddr的情况下生到了2级*/
                List<string> msgs = new List<string>();
                npc = (NPC)rm._Players[npcKey];
                rm.NPCM.NPCBeingAttacked(playerKey, npc, ref msgs, rm, dt);

                rm.NPCM.afterBroke(npc, ref msgs, dt);

                player = (Player)rm._Players[playerKey];
                if (player.levelObj.Level == 2 && string.IsNullOrEmpty(player.levelObj.TimeStampStr))
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
}
