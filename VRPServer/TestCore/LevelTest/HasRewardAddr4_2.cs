using NUnit.Framework;

namespace TestCore.LevelTest
{
    public class HasRewardAddr4_2 : HasRewardAddr4
    {
        [SetUp]
        public void Setup_2()
        {
            this.Setup();
        }
        [Test]
        public void TestPlayer1Level()
        {
            /*用于验证最后一次读，才有写的权力*/
            int level = DalOfAddress.LevelForSave.GetLevel("1NyrqNVai7uCP4CwZoDfQVAJ6EbdgaG6bg");
            if (
                player1.levelObj.Level == 5 &&
                (!string.IsNullOrEmpty(player1.levelObj.TimeStampStr)) &&
                (!string.IsNullOrEmpty(player1.levelObj.BtcAddr)))
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
