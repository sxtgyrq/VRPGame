using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleTestAPP
{
    class Program
    {
        class tt
        {
            public int a;
            public int b;
        }
        static void Main(string[] args)
        {


            // BitCoin.Sign.verify_message
            // Task.Run(() => TestYrqObj());
            // Console.WriteLine(a);
            //Console.Read();
            //Console.Read();
            while (Console.ReadLine().ToLower() == "exit")
            {
                break;
            }
            //   ModelScaleCal.Test();
            //Consol.WriteLine("你好啊，测试员！！！");
            //ThreadTest.Test();
        }
        static async void TestYrqObj()
        {
            string[] secret = new string[] { };
            secret = File.ReadAllLines("E:\\W202209\\PrivateKey.txt");

            BitCoin.Transfer.YrqTransObj tObj = new BitCoin.Transfer.YrqTransObj(secret);

            // BitCoinTrasfer.YrqTransObj tObj = new BitCoinTrasfer.YrqTransObj(secret);

            var s = await tObj.CalMoneyCanCost();
            // s.SetFee(500);
            tObj.SumMoney();
            tObj.SetMinerFee(416);
            //tObj.SumMoney();
            tObj.AddAddrPayTo("1NyrqneGRxTpCohjJdwKruM88JyARB2Ljr", 17425);
            tObj.AddAddrPayTo("1NyrqNVai7uCP4CwZoDfQVAJ6EbdgaG6bg", 17425);
            tObj.AddAddrPayTo("354vT5hncSwmob6461WjhhfWmaiZgHuaSK", 17425);

            tObj.TransactionF();
            tObj.BradCast();
            //tObj.SetChangeAddr("356irRFazab63B3m95oyiYeR5SDKJRFa99");


        }
    }
}
