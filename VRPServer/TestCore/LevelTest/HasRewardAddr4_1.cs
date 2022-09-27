using NUnit.Framework;

namespace TestCore.LevelTest
{
    public class HasRewardAddr4_1 : HasRewardAddr4
    {
        [SetUp]
        public void Setup_1()
        {
            this.Setup();
        }
        [Test]
        public void TestLevelInDB()
        {
            var l = DalOfAddress.LevelForSave.GetLevel("1NyrqNVai7uCP4CwZoDfQVAJ6EbdgaG6bg");
            if (l == 6)
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
