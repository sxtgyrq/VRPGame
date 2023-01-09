using System;
using System.Collections.Generic;
using System.Text;

namespace CommonClass.databaseModel
{
    public class taskcopy
    {
        public string btcAddr { get; set; }
        public string taskCopyCode { get; set; }
        public int firstRound { get; set; }
        public int secondRound { get; set; }
        public string Tag { get; set; }
        public int? Result { get; set; }
        public DateTime? ResultDateTime { get; set; }
    }
}
