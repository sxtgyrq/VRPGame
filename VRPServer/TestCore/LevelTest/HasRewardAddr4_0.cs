using NUnit.Framework;

namespace TestCore.LevelTest
{
    public class HasRewardAddr4_0 : HasRewardAddr4
    {
        [SetUp]
        public void Setup_0()
        {
            this.Setup();
        }
        [Test]
        public void TestPlayer2Level()
        {
            /*用于验证最后一次读，才有写的权力*/
            if (player2.levelObj.Level == 6 &&
                (!string.IsNullOrEmpty(player2.levelObj.TimeStampStr)) &&
                (!string.IsNullOrEmpty(player2.levelObj.BtcAddr)))
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
