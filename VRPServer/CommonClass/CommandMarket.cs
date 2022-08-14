using System;
using System.Collections.Generic;
using System.Text;

namespace CommonClass
{
    //class CommandMarket
    //{
    //}
    public class MarketPrice : Command
    {
        public long price { get; set; }
        public int count { get; set; }
        public string sellType { get; set; }
        //public string key { get; set; }
        //public string PlayerName { get; set; }
    }

    public class ModelStock : Command
    {
        public string modelID { get; set; }
        public string bussinessAddress { get; set; }
        public Dictionary<string, long> stocks { get; set; }
        public Dictionary<string, long> stocksOriginal { get; set; }
    }
    public class SystemBradcast : Command
    {
        public string msg { get; set; }
    }
    public class ServerStatictis : Command
    {
        //public string msg { get; set; }
    }
    public class BuyDiamondInMarket : Command
    {
        public string buyType { get; set; }
        public string Key { get; set; }
    }

    public class SetBuyDiamond : Command
    {
        public string Key { get; set; }
        public string pType { get; set; }
        public int count { get; set; }
    }
    public class SetSellDiamond : SetBuyDiamond { }
    public class SetSelectDriver : Command
    {
        public string Key { get; set; }
        public int Index { get; set; }
    }

    public class MarketIn : Command
    {
        public int count { get; set; }
        public string pType { get; set; }
    }
    public class MarketOut : Command
    {
        public int count { get; set; }
        public string pType { get; set; }
    }
    public class Transaction : Command
    {
        public string adress { get; set; }
    }

}
