using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Text;

namespace WsOfWebClient.MapEditor
{
    partial class Editor
    {
        private static void WebSocketF(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                if (context.WebSockets.IsWebSocketRequest)
                {

                    var webSocket = await context.WebSockets.AcceptWebSocketAsync();

                    await Echo(webSocket);
                }
            });
        }
       
    }
}
