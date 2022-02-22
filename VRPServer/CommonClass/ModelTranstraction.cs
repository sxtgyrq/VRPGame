using System;
using System.Collections.Generic;
using System.Text;

namespace CommonClass
{
    public class ModelTranstraction
    {
        public class GetAllModelPosition : Command
        {
            public class Result
            {
                public double x { get; set; }
                public double z { get; set; }
                public string modelID { get; set; }
            }
        }
        public class GetModelByID : Command
        {
            public string modelID { get; set; }
            public class Result
            {
                public double rotatey { get; set; }
                public string amodel { get; set; }
                public double x { get; set; }
                public double y { get; set; }
                public double z { get; set; }
                public string bussinessAddress { get; set; }
                public string modelID { get; set; }
                public string author { get; set; }
            }
        }

        public class GetRoadNearby : Command
        {
            public string key { get; set; }
            public double x { get; set; }
            public double z { get; set; }
            public class Result
            {
                public List<string> RoadCodes { get; set; }
            }
        }
        public class GetTransctionModelDetail : Command
        {
            public string bussinessAddr { get; set; }
        }

        public class TradeIndex : Command
        {
            public string addrBussiness { get; set; }
            public string addrFrom { get; set; }
        }

        public class TradeCoin : Command
        {
            public int tradeIndex { get; set; }
            public string addrFrom { get; set; }
            public string addrBussiness { get; set; }
            public string addrTo { get; set; }
            public long passCoin { get; set; }
            public string sign { get; set; }
            public string msg { get; set; }
            public class Result
            {
                //public double rotatey { get; set; }
                //public string amodel { get; set; }
                //public double x { get; set; }
                //public double y { get; set; }
                //public double z { get; set; }
                //public string bussinessAddress { get; set; }
                //public string modelID { get; set; }
            }
        }


    }
}
