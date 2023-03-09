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


            //   ModelScaleCal.Test();

            // BitCoin.Sign.verify_message
            // Task.Run(() => TestYrqObj());
            // Console.WriteLine(a);
            //Console.Read();
            //Console.Read();
            var c = new CommonClass.Img.Combine("Car_01.png", "105.jpg");
            var base64 = c.GetBase64();
            byte[] imageArray = Convert.FromBase64String(base64);
            File.WriteAllBytes("r22change.png", imageArray);

            //   CommonClass.Img.DrawFont.Draw("你");
            var data = CommonClass.Img.DrawFont.FontCodeResult.Data.Get(Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.Img.DrawFont.FontCodeResult.Data.objTff2>);
            CommonClass.Img.DrawFont.Initialize(data);
            var dr = new CommonClass.Img.DrawFont("膩", data,"red");
            dr.SaveAsImg();
            // CommonClass.Img.DrawFont.Draw("你", data);


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
