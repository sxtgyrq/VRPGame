using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleBitcoinChainApp
{
    internal class Program
    {
        static object locker = new object();
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Thread t1 = new Thread(() => DownloadB());
            t1.Start();

            ;
            string ip = File.ReadAllText("config/ip.txt").Trim();
            int tcpPort = Convert.ToInt32(File.ReadAllText("config/port.txt").Trim());
            Thread startTcpServer = new Thread(() => Listen.IpAndPort(ip, tcpPort));
            startTcpServer.Start();
            while (true)
            {
                if (Console.ReadLine().ToLower() == "exit")
                {
                    break;
                }
            }
            //   Console.ReadLine();
        }

        static void DownloadB()
        {
            while (true)
            {
                var list = DalOfAddress.detailmodel.GetAllAddr();
                for (int i = 0; i < list.Count; i++)
                {
                    try
                    {
                        var addr = list[i];
                        BitCoin.Transtraction.TradeInfo t = new BitCoin.Transtraction.TradeInfo(addr);
                        //tradeDetail = Task.Run(() => t.GetTradeInfomationFromChain_v2()).Result;
                        var t1 = t.GetTradeInfomationFromChain_v2(); 
                        var tradeDetail = t1.GetAwaiter().GetResult();
                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(tradeDetail);
                        lock (Program.locker)
                            File.WriteAllText($"data/{addr}", json, System.Text.Encoding.UTF8);
                    }
                    catch
                    {
                        Console.WriteLine("读取错误");
                    }
                }
                Thread.Sleep(1000 * 60 * 13);
            }
        }
    }
}
