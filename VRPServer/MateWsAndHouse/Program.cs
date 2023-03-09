using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using System;
using System.Collections.Generic;
using System.Threading;

namespace MateWsAndHouse
{
    class Program
    {
        public static Dictionary<int, Team> allTeams = new Dictionary<int, Team>();
        public static object teamLock = new object();
        public static Random rm = new Random();
        static void Main(string[] args)
        {
            allTeams = new Dictionary<int, Team>();
            rm = new Random(DateTime.Now.GetHashCode());
            Console.WriteLine("你好！此服务为匹配用户和房间的服务！example:127.0.0.1:32200");

            var ip = "127.0.0.1:32200";
            Console.WriteLine("输入ip和端口");
            var input = Console.ReadLine();
            if (string.IsNullOrEmpty(input)) { }
            else
            {
                ip = input;
            }

            Thread startTcpServer = new Thread(() => Listen.IpAndPort(ip.Split(':')[0], int.Parse(ip.Split(':')[1])));
            startTcpServer.Start();

            while (true)
            {
                if (Console.ReadLine().ToLower() == "exit")
                {
                    break;
                }
            }
            //CreateWebHostBuilder(new string[] { ip }).Build().Run();
        }

    //    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
    //WebHost.CreateDefaultBuilder(args).Configure(item => item.UseForwardedHeaders(new ForwardedHeadersOptions
    //{
    //    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
    //})).UseKestrel(options =>
    //{
    //    options.AllowSynchronousIO = true;
    //})
    //.UseUrls(args[0])
    //    .UseStartup<Startup>();
    }
}
