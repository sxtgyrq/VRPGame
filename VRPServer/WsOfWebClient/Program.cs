using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using System;

namespace WsOfWebClient
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine(@"
A.游戏WebSocket服务(默认)
B.地图编辑器WebSocket服务");
            var select = Console.ReadLine().ToUpper();

            if (select == "B")
            {
                //  Room.SetWhenStart();
                var ip = "127.0.0.1";
                int websocketPort = 21001;
                Console.WriteLine($"输入ip，如{ip}  (默认)");
                var inputIp = Console.ReadLine();
                if (string.IsNullOrEmpty(inputIp)) { }
                else
                {
                    ip = inputIp;
                }
                Console.WriteLine($"输入端口≠15000,如“{websocketPort}”");
                var inputWebsocketPort = Console.ReadLine();
                if (string.IsNullOrEmpty(inputWebsocketPort)) { }
                else
                {
                    int num;
                    if (int.TryParse(inputWebsocketPort, out num))
                    {
                        websocketPort = num;
                    }
                }
                //Consol.WriteLine($"地址为：http://{ip}:{websocketPort}");
                CreateWebHostBuilder2(new string[] { $"http://{ip}:{websocketPort}" }).Build().Run();

                // return;
            }
            else
            {
                var data = CommonClass.Img.DrawFont.FontCodeResult.Data.Get(Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.Img.DrawFont.FontCodeResult.Data.objTff2>);
                CommonClass.Img.DrawFont.Initialize(data);
                Team.Config();
                //  Room.SetWhenStart();
                Console.WriteLine("你好！此服务为网页端的webSocket服务！20220702");
                var ip = "127.0.0.1";
                int websocketPort = 11001;

                Console.WriteLine($"输入ip,如“{ip}”");
                var inputIp = Console.ReadLine();
                if (string.IsNullOrEmpty(inputIp)) { }
                else
                {
                    ip = inputIp;
                }

                Console.WriteLine($"输入端口≠15000,如“{websocketPort}”");
                var inputWebsocketPort = Console.ReadLine();
                if (string.IsNullOrEmpty(inputWebsocketPort)) { }
                else
                {
                    int num;
                    if (int.TryParse(inputWebsocketPort, out num))
                    {
                        websocketPort = num;
                    }
                }
                int tcpServerPort = 30000 - websocketPort;
                ConnectInfo.HostIP = ip;
                ConnectInfo.webSocketPort = websocketPort;
                ConnectInfo.tcpServerPort = tcpServerPort;

                var builder = CreateWebHostBuilder(new string[] { $"http://{ip}:{ConnectInfo.webSocketPort}" });

                var app = builder.Build();
                app.Run();

                //CreateWebHostBuilder(new string[] { $"http://{ip}:{ConnectInfo.webSocketPort}" })
                //    .Build().Run();

                //Consol.WriteLine("Hello World!");
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args).
            Configure(item => item.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
            })).UseKestrel(options =>
            {
                options.AllowSynchronousIO = true;
            })
.UseUrls(args[0])
   .UseStartup<Startup>();

        public static IWebHostBuilder CreateWebHostBuilder2(string[] args) =>
WebHost.CreateDefaultBuilder(args).Configure(item => item.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
})).UseKestrel(options =>
{
    options.AllowSynchronousIO = true;
})
.UseUrls(args[0])
 .UseStartup<MapEditor.Editor>();
    }
}
