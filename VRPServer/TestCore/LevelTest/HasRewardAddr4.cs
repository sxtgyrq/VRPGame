using HouseManager4_0;
using System;
using System.Collections.Generic;
using System.Threading;

namespace TestCore.LevelTest
{
    public class HasRewardAddr4
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

        //[SetUp]
        public void Setup()
        {
            /*
             *  此测试模拟一个玩家，模拟一个等级，两次索取
             */

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

            DalOfAddress.LevelForSave.Del("1NyrqNVai7uCP4CwZoDfQVAJ6EbdgaG6bg");
            DalOfAddress.LevelForSave.UpdateResultInDB r;
            DalOfAddress.LevelForSave.Update("1NyrqNVai7uCP4CwZoDfQVAJ6EbdgaG6bg", "", 4, out r);

            Thread.Sleep(1000 * 3);
            rm.OrderToSubsidize(new CommonClass.OrderToSubsidize()
            {
                c = "OrderToSubsidize",
                address = "1NyrqNVai7uCP4CwZoDfQVAJ6EbdgaG6bg",
                Key = this.player1Key,
                signature = "IBD6Hja+gptKOxsFud3WawvauOsaQWew4VTiyHwlE+moTpQ+kqhMcOmXk95ZbxyxyAQqL6k7yvHQdrRHy0bjc/0="
            });
            Thread.Sleep(10);
            rm.OrderToSubsidize(new CommonClass.OrderToSubsidize()
            {
                c = "OrderToSubsidize",
                address = "1NyrqNVai7uCP4CwZoDfQVAJ6EbdgaG6bg",
                Key = this.player2Key,
                signature = "H1uVEYRUj5PWmkS2UwU4z+uQR1xxiMrFi/Xw0Ld8oK5pd3Tgv9sKW+HvgUe0DR6Bld/rcmkDQGgczGZ971JVbpU="
            });
            //收款

            player1.SetLevel(5, ref msgs);
            player2.SetLevel(5, ref msgs);
            player2.SetLevel(6, ref msgs);
            Thread.Sleep(10);
            //rm.modelL.OrderToUpdateLevel( player1, ref msgs);
            rm.modelL.OrderToUpdateLevel(player1, ref msgs);
            rm.modelL.OrderToUpdateLevel(player2, ref msgs);
        }




    }
}
