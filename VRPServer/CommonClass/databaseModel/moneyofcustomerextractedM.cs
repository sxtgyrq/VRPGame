using System;
using System.Collections.Generic;
using System.Text;

namespace CommonClass.databaseModel
{
    public class moneyofcustomerextractedM
    {
        public string bussinessAddr { get; set; }
        public int tradeIndex { get; set; }
        public string addrFrom { get; set; }
        public long satoshi { get; set; }
        public int isPayed { get; set; }
        public DateTime recordTime { get; set; }

    }
}
