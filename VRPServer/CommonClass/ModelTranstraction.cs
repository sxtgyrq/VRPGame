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

        public class GetTransctionFromChain : Command
        {
            public string bussinessAddr { get; set; }
        }

        public class TradeIndex : Command
        {
            public string addrBussiness { get; set; }
            public string addrFrom { get; set; }
        }
        public class AllBuiisnessAddr : Command
        {
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
                public string msg { get; set; }
                public bool success { get; set; }
            }
        }

        public class TradeSetAsReward : Command
        {
            public int tradeIndex { get; set; }
            public string addrReward { get; set; }
            public string addrBussiness { get; set; }
            public long passCoin { get; set; }
            public string signOfaddrBussiness { get; set; }
            public string signOfAddrReward { get; set; }
            public string msg { get; set; }
            public int afterWeek { get; set; }
            public class Result
            {
                public bool success { get; set; }
                public string msg { get; set; }
            }

        }

        public class RewardInfomation : Command
        {
            public int startDate { get; set; }
        }

        public class RewardApply : CommonClass.Command
        {
            public string addr { get; set; }
            public string msgNeedToSign { get; set; }
            public string signature { get; set; }

            public class Result
            {
                public bool success { get; set; }
                public string msg { get; set; }
            }

        }
        public class AwardsGivingPass : CommonClass.Command
        {
            public string time { get; set; }
            public List<string> list { get; set; }
            public List<string> msgs { get; set; }
            public List<int> ranks { get; set; }
            public List<string> applyAddr { get; set; }
            public class Result
            {
                public bool success { get; set; }
                public string msg { get; set; }
            }
        }
        public class AwardsGiving : CommonClass.Command
        {
            public string time { get; set; }
            public List<string> list { get; set; }
        }

        public class BindWordInfo : CommonClass.Command
        {
            public string bindWordAddr { get; set; }
            public string bindWordMsg { get; set; }
            public string bindWordSign { get; set; }
            public string verifyCodeValue { get; set; }
            public class Result
            {
                public bool success { get; set; }
                public string msg { get; set; }
            }
        }
        public class LookForBindInfo : CommonClass.Command
        {
            public string infomation { get; set; }
            public string verifyCodeValue { get; set; }
            public class Result
            {
                public bool success { get; set; }
                public string msg { get; set; }
            }
        }
        public class RewardBuildingShow : CommonClass.Command
        {
            public string Title { get; set; }
        }
    }
}
