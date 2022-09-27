using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestCore
{
    public class Agreement
    {
        string msg;
        int index;
        string tradeAddr;
        string businessAddr;
        string acceptAddr;
        int passValue;
        [SetUp]
        public void Setup()
        {
            msg = "1@3FkXatYUQv81t7mDsQf8igumGnp1mE1gkk@3BkRN6seLYYgSqGQNX3o1poEzCXhGK3vTs->354vT5hncSwmob6461WjhhfWmaiZgHuaSK:100000Satoshi";
            CommonClass.Agreement.IsUseful(msg, out index, out tradeAddr, out businessAddr, out acceptAddr, out passValue);
        }
        [Test]
        public void GetValue()
        {
            //  CommonClass.is
        }
        //  CommonClass.Agreement.
    }
}
