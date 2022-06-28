using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HouseManager4_0
{
    class Startup
    {
        public static void sendMsg(string controllerUrl, string json)
        {
            var r = Task.Run<string>(() => TcpFunction.WithResponse.SendInmationToUrlAndGetRes(controllerUrl, json));
        }
    }
}
