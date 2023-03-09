using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MarketConsoleApp
{

    class Market
    {

        string[] servers;
        internal void loadSevers()
        {
            this.servers = File.ReadAllLines("config/servers.txt");
            for (var i = 0; i < this.servers.Length; i++)
            {
                Console.WriteLine($"服务器{i}-{this.servers[i]}");
            }
            Console.WriteLine($"请确认服务器，输入任意键继续");
            Console.ReadLine();
        }

        internal void GetDetailOfPayer()
        {
            Thread th = new Thread(() => this.RefreshPayerInfo());
            th.Start();
        }

        private void RefreshPayerInfo()
        {
            Thread th = new Thread(() => this.RefreshEndTime());
            th.Start();
        }

        private void RefreshEndTime()
        {
            int sleepTime = 3600;
            DateTime endTime;
            if (File.Exists("config/endTime.txt"))
            {
                var text = File.ReadAllText("config/endTime.txt");
                endTime = DateTime.Parse(text);
            }
            else
            {
                endTime = DateTime.Now;
                while (true)
                {
                    endTime = endTime.AddDays(1);
                    if (endTime.DayOfWeek == DayOfWeek.Thursday)
                    {
                        endTime = new DateTime(endTime.Year, endTime.Month, endTime.Day, 9, 0, 0);
                        var text = endTime.ToString("yyyy-MM-dd HH:mm:ss");
                        File.WriteAllText("config/endTime.txt", text);
                        break;
                    }
                }
            }
            while (true)
            {
                var now = DateTime.Now;
                if (now >= endTime)
                {
                    sleepTime = 30000;//60s
                    tellAllPlayer($"服务器随时可能关闭。请立即将收入保存。");
                }
                else if ((endTime - now).TotalSeconds < 1800)
                {
                    sleepTime = 60000;//60s
                    tellAllPlayer($"服务器将于当日{endTime.ToString("HH点mm分")}进行维护。请将收入保存。");
                }
                else if ((endTime - now).TotalSeconds < 7200)
                {
                    sleepTime = 60000 * 10;//10min
                    tellAllPlayer($"服务器将于当日{endTime.ToString("HH点mm分")}进行维护。请将收入保存。");
                }
                else
                {
                    sleepTime = 60000 * 15;//15min

                }
                Thread.Sleep(sleepTime);
                var text = File.ReadAllText("config/endTime.txt");
                endTime = DateTime.Parse(text);
            }
        }

        private void tellAllPlayer(string msg)
        {
            //  throw new NotImplementedException();
            // Console.WriteLine(msg);

            for (var j = 0; j < this.servers.Length; j++)
            {
                var server = this.servers[j];

                sendMsg(server,
                  Newtonsoft.Json.JsonConvert.SerializeObject(
                      new CommonClass.SystemBradcast()
                      {
                          c = "SystemBradcast",
                          msg = msg
                      }));
                ;
            }
        }

        internal void startStatictis()
        {
            (new Thread(() => this.StaticAllSever())).Start();
        }
        private void StaticAllSever()
        {
            while (true)
            {
                //  throw new NotImplementedException();
                // Console.WriteLine(msg);
                for (var j = 0; j < this.servers.Length; j++)
                {
                    var server = this.servers[j];
                    var controllerUrl = server;
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(
                            new CommonClass.ServerStatictis()
                            {
                                c = "ServerStatictis"
                            });
                    var t = TcpFunction.WithResponse.SendInmationToUrlAndGetRes(controllerUrl, json);

                    var rResult = t.GetAwaiter().GetResult();
                    // var r = t;
                    //var r =  <string>(() => TcpFunction.WithResponse.SendInmationToUrlAndGetRes(controllerUrl, json));
                    if (string.IsNullOrEmpty(rResult)) { }
                    else
                    {
                        List<int> count = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(rResult);
                        var fileName = server.Replace('.', '_').Replace(':', '_');
                        fileName = $"log/{fileName}.txt";
                        var msg = $"{server},{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},总共:{count[0]},玩家:{count[1]},NPC:{count[2]},在线玩家:{count[3]}.{Environment.NewLine}";
                        Console.WriteLine(msg);
                        File.AppendAllText(fileName, msg);
                    }
                }
                Thread.Sleep(60 * 1000);
            }
        }
        internal void sendInteview()
        {
            Thread th = new Thread(() => this.sendInteview(true));
            th.Start();
        }

        Dictionary<string, Dictionary<string, long>> ModelInputSatoshi = new Dictionary<string, Dictionary<string, long>>();
        object ModelInputLock = new object();
        internal void getAllBitcoinThread()
        {
            //var t =  () => getAllBitInfomation());

            Thread th = new Thread(() => getAllBitInfomation());
            th.Start();
            //  throw new NotImplementedException();
        }

        private async void getAllBitInfomation()
        {
            while (true)
            {

                var allItem = DalOfAddress.detailmodel.GetAll();

                for (int i = 0; i < allItem.Count; i++)
                {
                    try
                    {
                        Dictionary<string, long> tradeDetail;

                        Dictionary<string, long> stocksOriginal = new Dictionary<string, long>();
                        string bussinessAddress;
                        {
                            var item = allItem[i];
                            var detail = DalOfAddress.detailmodel.GetByID(item.modelID);
                            bussinessAddress = detail.bussinessAddress;
                            if (string.IsNullOrEmpty(bussinessAddress))
                            {
                                continue;
                            }
                            //detail.bussinessAddress;
                            var dicResult = ConsoleBitcoinChainApp.GetData.GetTradeInfomationFromChain(detail.bussinessAddress);

                            //BitCoin.Transtraction.TradeInfo t = new BitCoin.Transtraction.TradeInfo(detail.bussinessAddress);
                            ////   < Dictionary<string, long>(() => t.GetTradeInfomationFromChain());
                            //var result =  <Dictionary<string, long>>(() => t.GetTradeInfomationFromChain());
                            lock (ModelInputLock)
                            {
                                if (ModelInputSatoshi.ContainsKey(item.modelID))
                                {
                                    ModelInputSatoshi[item.modelID] = dicResult;
                                }
                                else
                                {
                                    ModelInputSatoshi.Add(item.modelID, dicResult);
                                }
                                tradeDetail = ModelInputSatoshi[item.modelID];
                            }
                        }
                        // var tradeDetail = result.Result;
                        long sumValue = 0;
                        {
                            //long sumValue = 0;
                            {
                                var tradeDetailList = new List<string>();
                                sumValue = 0;
                                foreach (var item in tradeDetail)
                                {
                                    tradeDetailList.Add(item.Key);
                                    tradeDetailList.Add($"{item.Value / 100000000}.{(item.Value % 100000000).ToString("D8")}");
                                    sumValue += item.Value;
                                    stocksOriginal.Add(item.Key, item.Value);
                                }
                            }
                        }
                        if (sumValue == 0)
                        {
                            continue;
                        }
                        else
                        {
                            var r = DalOfAddress.TradeRecord.GetAll(bussinessAddress);
                            List<string> list = r;
                            for (int j = 0; j < list.Count; j += 2)
                            {
                                var mtsMsg = list[j];
                                var parameter = mtsMsg.Split(new char[] { '@', '-', '>', ':' }, StringSplitOptions.RemoveEmptyEntries);
                                if (parameter.Length == 5)
                                {
                                    var sign = list[j + 1];
                                    //    if (BitCoin.Sign.checkSign(sign, mtsMsg, parameter[1]))
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
                            var stocks = new Dictionary<string, long>();
                            foreach (var stockItem in tradeDetail)
                            {
                                if (stockItem.Value <= 0)
                                {

                                }
                                else
                                {
                                    stocks.Add(stockItem.Key, stockItem.Value);
                                }
                            }
                            for (var j = 0; j < this.servers.Length; j++)
                            {
                                var server = this.servers[j];
                                sendMsg(server,
                                  Newtonsoft.Json.JsonConvert.SerializeObject(
                                      new CommonClass.ModelStock()
                                      {
                                          c = "ModelStock",
                                          modelID = allItem[i].modelID,
                                          stocks = stocks,
                                          stocksOriginal = stocksOriginal,
                                          bussinessAddress = bussinessAddress,
                                      }));
                            }
                        }
                    }
                    catch
                    {
                        //Consol.WriteLine($"获取信息失败");
                    }
                }
                Thread.Sleep(1000 * 60 * 20);
            }
        }

        internal void sendInteview(bool waitT)
        {
            while (true)
            {
                Thread.Sleep(1000 * 60 * 5);
                tellMarketIsOn();
            }
        }

        string IP = "127.0.0.1";
        int Port = 12630;
        internal void waitToBeTelled()
        {
            Console.WriteLine($"请输入IP地址！默认{this.IP}");
            var input = Console.ReadLine().Trim();
            if (!string.IsNullOrEmpty(input))
            {
                this.IP = input;
            }
            Console.WriteLine($"请输入端口。默认{this.Port}");
            input = Console.ReadLine().Trim();
            if (!string.IsNullOrEmpty(input))
            {
                this.Port = int.Parse(input);
            }
            var dealWith = new TcpFunction.WithResponse.DealWith(this.DealWith);
            TcpFunction.WithResponse.ListenIpAndPort(this.IP, this.Port, dealWith);

            // Listen.IpAndPort(ip, tcpPort)
            // throw new NotImplementedException();
        }

        private string DealWith(string notifyJson, int tcpPort)
        {
            //  Console.WriteLine($"DealWith-Msg-{notifyJson}");
            CommonClass.Command c = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.Command>(notifyJson);
            switch (c.c)
            {
                case "MarketIn":
                    {
                        CommonClass.MarketIn mi = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MarketIn>(notifyJson);
                        switch (mi.pType)
                        {
                            case "mile":
                                {
                                    var price1 = getPrice(this.mileCount);
                                    this.mileCount += mi.count;
                                    var price2 = getPrice(this.mileCount);
                                    if (price1 != price2)
                                    {
                                        Thread th = new Thread(() => tellMarketItem(mi.pType));
                                        th.Start();
                                        saveCount(mi.pType, this.mileCount);
                                    }
                                }; break;
                            case "business":
                                {
                                    var price1 = getPrice(this.businessCount);
                                    this.businessCount += mi.count;
                                    var price2 = getPrice(this.businessCount);
                                    if (price1 != price2)
                                    {
                                        Thread th = new Thread(() => tellMarketItem(mi.pType));
                                        th.Start();
                                        saveCount(mi.pType, this.businessCount);
                                    }
                                }; break;
                            case "volume":
                                {
                                    var price1 = getPrice(this.volumeCount);
                                    this.volumeCount += mi.count;
                                    var price2 = getPrice(this.volumeCount);
                                    if (price1 != price2)
                                    {
                                        Thread th = new Thread(() => tellMarketItem(mi.pType));
                                        th.Start();
                                        saveCount(mi.pType, this.volumeCount);
                                    }
                                }; break;
                            case "speed":
                                {
                                    var price1 = getPrice(this.speedCount);
                                    this.speedCount += mi.count;
                                    var price2 = getPrice(this.speedCount);
                                    if (price1 != price2)
                                    {
                                        Thread th = new Thread(() => tellMarketItem(mi.pType));
                                        th.Start();
                                        saveCount(mi.pType, this.speedCount);
                                    }
                                }; break;
                        }
                    }; break;
                case "MarketOut":
                    {
                        CommonClass.MarketOut mo = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MarketOut>(notifyJson);
                        switch (mo.pType)
                        {
                            case "mile":
                                {
                                    var price1 = getPrice(this.mileCount);
                                    this.mileCount -= mo.count;
                                    var price2 = getPrice(this.mileCount);
                                    if (price1 != price2)
                                    {
                                        Thread th = new Thread(() => tellMarketItem(mo.pType));
                                        th.Start();
                                        saveCount(mo.pType, this.mileCount);
                                    }
                                }; break;
                            case "business":
                                {
                                    var price1 = getPrice(this.businessCount);
                                    this.businessCount -= mo.count;
                                    var price2 = getPrice(this.businessCount);
                                    if (price1 != price2)
                                    {
                                        Thread th = new Thread(() => tellMarketItem(mo.pType));
                                        th.Start();
                                        saveCount(mo.pType, this.businessCount);
                                    }
                                }; break;
                            case "volume":
                                {
                                    var price1 = getPrice(this.volumeCount);
                                    this.volumeCount -= mo.count;
                                    var price2 = getPrice(this.volumeCount);
                                    if (price1 != price2)
                                    {
                                        Thread th = new Thread(() => tellMarketItem(mo.pType));
                                        th.Start();
                                        saveCount(mo.pType, this.volumeCount);
                                    }
                                }; break;
                            case "speed":
                                {
                                    var price1 = getPrice(this.speedCount);
                                    this.speedCount -= mo.count;
                                    var price2 = getPrice(this.speedCount);
                                    if (price1 != price2)
                                    {
                                        Thread th = new Thread(() => tellMarketItem(mo.pType));
                                        th.Start();
                                        saveCount(mo.pType, this.speedCount);
                                    }
                                }; break;
                        }
                    }; break;
                case "Transaction":
                    {
                        CommonClass.Transaction trant = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.Transaction>(notifyJson);
                        if (BitCoin.CheckAddress.CheckAddressIsUseful(trant.adress))
                        {
                            TradeInfo tradeInfo = new TradeInfo(trant.adress);
                            var t = tradeInfo.GetTradeInfomationFromChain();
                            t.GetAwaiter().GetResult();

                        }
                    }; break;
            }
            return "";
        }

        private void saveCount(string pType, int count)
        {
            DalOfAddress.Diamoudcount.UpdateItem(pType, count);
        }

        private void tellMarketItem(string pType)
        {
            for (var i = 0; i < this.servers.Length; i++)
            {
                //string ip = this.servers[i].Split(':')[0];
                //int port = int.Parse(this.servers[i].Split(':')[1]);
                switch (pType)
                {
                    case "mile":
                        {
                            sendMsg(this.servers[i],
                              Newtonsoft.Json.JsonConvert.SerializeObject(
                                  new CommonClass.MarketPrice()
                                  {
                                      c = "MarketPrice",
                                      count = this.mileCount,
                                      price = getPrice(this.mileCount),
                                      sellType = "mile",
                                  }));
                        }; break;
                    case "business":
                        {
                            sendMsg(this.servers[i],
                              Newtonsoft.Json.JsonConvert.SerializeObject(
                                  new CommonClass.MarketPrice()
                                  {
                                      c = "MarketPrice",
                                      count = this.businessCount,
                                      price = getPrice(this.businessCount),
                                      sellType = "business",
                                  }));
                        }; break;
                    case "volume":
                        {
                            sendMsg(this.servers[i],
                              Newtonsoft.Json.JsonConvert.SerializeObject(
                                  new CommonClass.MarketPrice()
                                  {
                                      c = "MarketPrice",
                                      count = this.volumeCount,
                                      price = getPrice(this.volumeCount),
                                      sellType = "volume",
                                  }));
                        }; break;
                    case "speed":
                        {
                            sendMsg(this.servers[i],
                              Newtonsoft.Json.JsonConvert.SerializeObject(
                                  new CommonClass.MarketPrice()
                                  {
                                      c = "MarketPrice",
                                      count = this.speedCount,
                                      price = getPrice(this.speedCount),
                                      sellType = "speed",
                                  }));
                        }; break;
                }
            }
        }

        int mileCount = 8000;
        int businessCount = 8000;
        int volumeCount = 8000;
        int speedCount = 8000;
        internal void loadCount()
        {
            var rootPath = System.IO.Directory.GetCurrentDirectory();
            //Consol.WriteLine($"path:{rootPath}");
            this.mileCount = getCount("mile", this.mileCount);
            this.businessCount = getCount("business", this.businessCount);
            this.volumeCount = getCount("volume", this.volumeCount);
            this.speedCount = getCount("speed", this.speedCount);
            Console.WriteLine($"mile:{this.mileCount};business:{this.businessCount};volume:{this.volumeCount};speed:{this.speedCount}");
            Console.WriteLine($"以上为库存数量，按任意键继续！");
            Console.ReadLine();
        }

        private int getCount(string v, int value)
        {
            return DalOfAddress.Diamoudcount.GetCount(v);
        }

        internal void tellMarketIsOn()
        {
            //  this.servers = File.ReadAllLines("servers.txt");
            for (var i = 0; i < this.servers.Length; i++)
            {
                //string ip = this.servers[i].Split(':')[0];
                //int port = int.Parse(this.servers[i].Split(':')[1]);

                sendMsg(this.servers[i],
                  Newtonsoft.Json.JsonConvert.SerializeObject(
                      new CommonClass.MarketPrice()
                      {
                          c = "MarketPrice",
                          count = this.mileCount,
                          price = getPrice(this.mileCount),
                          sellType = "mile",
                      }));
                sendMsg(this.servers[i],
                 Newtonsoft.Json.JsonConvert.SerializeObject(
                     new CommonClass.MarketPrice()
                     {
                         c = "MarketPrice",
                         count = this.businessCount,
                         price = getPrice(this.businessCount),
                         sellType = "business",
                     }));
                sendMsg(this.servers[i],
                 Newtonsoft.Json.JsonConvert.SerializeObject(
                     new CommonClass.MarketPrice()
                     {
                         c = "MarketPrice",
                         count = this.volumeCount,
                         price = getPrice(this.volumeCount),
                         sellType = "volume",
                     }));
                sendMsg(this.servers[i],
                 Newtonsoft.Json.JsonConvert.SerializeObject(
                     new CommonClass.MarketPrice()
                     {
                         c = "MarketPrice",
                         count = this.speedCount,
                         price = getPrice(this.speedCount),
                         sellType = "speed",
                     }));
            }


        }

        public static void sendMsg(string controllerUrl, string json)
        {
            // TcpFunction.WithResponse.SendInmationToUrlAndGetRes
            try
            {
                var t = TcpFunction.WithResponse.SendInmationToUrlAndGetRes(controllerUrl, json);
                t.GetAwaiter().GetResult();

                //await ) => TcpFunction.WithResponse.SendInmationToUrlAndGetRes(controllerUrl, json));
                //Consol.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}往{controllerUrl}发送消息--成功！");
            }
            catch (Exception e)
            {
                //Consol.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}往{controllerUrl}发送消息--{json}失败！");
            }
        }
        private long getPrice(int mileCount)
        {
            if (mileCount < 1000)
            {

                //max:100000分,min:10000分。
                return ((100000 - 90 * mileCount) / 50) * 50;
            }
            else if (mileCount < 8000)
            {
                //max:10000分,min:1250分。
                //return 
                return ((11250 - 5 * mileCount / 4) / 50) * 50;
            }
            else
            {
                return 1250;
            }
        }
    }
}
