using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using static CommonClass.MapEditor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WsOfWebClient.MapEditor
{
    partial class Editor
    {
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors(options =>
            {
                //options.AddPolicy(name: "MyPolicy",
                //    builder =>
                //    {
                //        builder.WithOrigins("http://*");
                //    });
                //options.AddPolicy("AllowSpecificOrigins",
                //builder =>
                //{
                //    builder.WithOrigins("http://www.nyrq123.com", "http://localhost:1978", "https://www.nyrq123.com", "*");
                //});
                options.AddPolicy("AllowAny", p => p.AllowAnyOrigin()
                                                                          .AllowAnyMethod()
                                                                          .AllowAnyHeader());
            });
            services.AddLogging(builder =>
            {
                builder.AddFilter("Microsoft", LogLevel.Error)
                .AddFilter("System", LogLevel.Error)
                .AddFilter("NToastNotify", LogLevel.Error)
                .AddConsole();
            });
        }
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IWebHostEnvironment env)
        {



            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseWebSockets();
            // app.useSt(); // For the wwwroot folder
            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(
            //"F:\\MyProject\\VRPWithZhangkun\\MainApp\\VRPWithZhangkun\\VRPServer\\WebApp\\webHtml"),
            //    RequestPath = "/StaticFiles"
            //});

            //app.Map("/postinfo", HandleMapdownload);
            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(3600 * 24),
                ReceiveBufferSize = webWsSize
            };
            app.UseWebSockets(webSocketOptions);

            app.Map("/editor", WebSocketF);

            app.Map("/file", DownLoad);

            app.Map("/upload", Upload);

            app.Map("/img", BackGroundImg);
            // app.Map("/notify", notify);

            //Console.WriteLine($"启动TCP连接！{ ConnectInfo.tcpServerPort}");
            //Thread th = new Thread(() => startTcp());
            //th.Start();
        }

    }
}
