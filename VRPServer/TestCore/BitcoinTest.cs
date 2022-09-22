using NUnit.Framework;

namespace TestCore
{
    public class Tests
    {
        bool check3Addr = false;
        [SetUp]
        public void Setup()
        {
            this.check3Addr = BitCoin.Sign.checkSign("H3RsQHvimR1BztjXooOZWl4EYTB3v71lDfx4CYDXHvfNVeUg2J6JYfg+2T5btd1i63+gOfhaZV5D4+A6ahR7tgE=", "20220927", "354vT5hncSwmob6461WjhhfWmaiZgHuaSK");
        }

        [Test]
        public void Test1()
        {
            if (this.check3Addr)
                Assert.Pass();
            else
                Assert.Fail();
        }
    }
}