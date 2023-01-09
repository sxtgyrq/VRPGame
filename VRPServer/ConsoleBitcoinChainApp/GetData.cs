using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ConsoleBitcoinChainApp
{
    public class GetData
    {
        static string _UriStr = null;
        static string UriStr
        {
            get
            {
                if (string.IsNullOrEmpty(_UriStr))
                {
                    _UriStr = File.ReadAllText("config/ConsoleBitcoinChainAppIPPort.txt");
                }
                return _UriStr;
            }
        }
        public static Dictionary<string, long> GetTradeInfomationFromChain(string addr)
        {
            var t = TcpFunction.WithResponse.SendInmationToUrlAndGetRes(UriStr, addr);
            var resultString = t.GetAwaiter().GetResult();
            
            //var resultString = t.Result;
            //var resultString = await TcpFunction.WithResponse.SendInmationToUrlAndGetRes(UriStr, addr);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, long>>(resultString);
        }

        public static Dictionary<string, long> SetTrade(ref Dictionary<string, long> tradeDetail, List<string> list)
        {
            for (int i = 0; i < list.Count; i += 2)
            {
                //Consol.WriteLine(list[i]);
                var mtsMsg = list[i];
                var parameter = mtsMsg.Split(new char[] { '@', '-', '>', ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (parameter.Length == 5)
                {
                    var sign = list[i + 1];
                    {
                        var tradeIndex = int.Parse(parameter[0]);
                        var addrFrom = parameter[1];
                        var addrBussiness = parameter[2];
                        var addrTo = parameter[3];

                        var passCoinStr = parameter[4];
                        if (passCoinStr.Substring(passCoinStr.Length - 7, 7) == "Satoshi")
                        {
                            var passCoin = Convert.ToInt64(passCoinStr.Substring(0, passCoinStr.Length - 7));

                            if (tradeDetail.ContainsKey(addrFrom))
                            {
                                if (tradeDetail[addrFrom] >= passCoin)
                                {
                                    tradeDetail[addrFrom] -= passCoin;
                                    if (tradeDetail.ContainsKey(addrTo))
                                    {
                                        tradeDetail[addrTo] += passCoin;
                                    }
                                    else
                                    {
                                        tradeDetail.Add(addrTo, passCoin);
                                    }
                                }
                            }

                        }
                    }
                }
            }
            return tradeDetail;
        }
    }
}
