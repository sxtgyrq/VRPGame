using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace CubeImageTool
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(
                @"A:下载
B:Cube To Panorama");
            var switchItem = Console.ReadLine().Trim().ToUpper();
            switch (switchItem)
            {
                case "B":
                    {
                        Console.WriteLine("Cube To Panorama，生成器！");
                        CubeToPanorama.Generate();
                    }; break;
                default:
                    {
                        Console.WriteLine("你好，欢迎使用720°下载器！");
                        while (true)
                        {
                            var t = Task.Run<string>(() => Contact()).Result;
                            Console.WriteLine(t);
                            Console.WriteLine("C继续");
                            if (Console.ReadLine().ToLower().Trim() == "c")
                            { }
                            else
                            {
                                break;
                            }
                        }
                    }; break;
            }

        }

        private static async Task<string> Contact()
        {
            string qt, sid, pos, z, udt, from, auth, seckey;

            Console.WriteLine("输入url");
            var url = Console.ReadLine();

            char[] ss = new char[] { '?', '&', '=' };
            var splitedString = url.Split(ss, StringSplitOptions.RemoveEmptyEntries);


            //Consol.WriteLine("qt=? 默认pdata");
            qt = "pdata";

            sid = "";
            for (int i = 0; i < splitedString.Length; i++)
            {
                if (splitedString[i] == "sid")
                {
                    sid = splitedString[i + 1];
                }
            }
            //Consol.WriteLine($"sid={sid}");
            //sid = Console.ReadLine();

            udt = DateTime.Now.ToString("yyyyMMdd");
            from = "PC";

            auth = "";
            for (int i = 0; i < splitedString.Length; i++)
            {
                if (splitedString[i] == "auth")
                {
                    auth = splitedString[i + 1];
                }
            }
            //Consol.WriteLine($"auth={auth}");


            seckey = "";
            for (int i = 0; i < splitedString.Length; i++)
            {
                if (splitedString[i] == "seckey")
                {
                    seckey = splitedString[i + 1];
                }
            }
            //Consol.WriteLine($"seckey={seckey}");
          //  Console.ReadLine();

            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(16 * 512, 8 * 512);

            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    pos = $"{j}_{i}";
                    z = "5";
                    var urlStr = $"https://mapsv0.bdimg.com/?qt={qt}&sid={sid}&pos={pos}&z={z}&udt={udt}&from={from}&auth={auth}&seckey={seckey}";
                    //Consol.WriteLine(urlStr);
                    var s = await getStream(urlStr);
                    System.Drawing.Image img = System.Drawing.Image.FromStream(s);
                    //bm.
                    using (var graphics = Graphics.FromImage(bmp))
                    {
                        graphics.DrawImage(img, i * 512, j * 512, 512, 512);
                    }
                }
            }
            bmp.Save($"cube{DateTime.Now.ToString("yyyyMMddHHmmss")}.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            return "finished";
        }

        static async Task<Stream> getStream(string url)
        {
            byte[] data;
            using (WebClient web1 = new WebClient())
            {
                data = await web1.DownloadDataTaskAsync(url);
            }
            Stream stream = new MemoryStream(data);
            return stream;
        }
    }
}
