using System;
using System.Collections.Generic;
using System.Text;

namespace CommonClass.databaseModel
{
    public class tradereward
    {
        public int startDate { get; set; }
        public int tradeIndex { get; set; }
        public string tradeAddress { get; set; }
        public string bussinessAddr { get; set; }
        public int passCoin { get; set; }
        public int waitingForAddition { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string signOfTradeAddress { get; set; }
        /// <summary>
        /// 即对应Building的地址
        /// </summary>
        public string signOfBussinessAddr { get; set; }
        public string orderMessage { get; set; }

    }
}
