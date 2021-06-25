using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager4_0
{
    class Startup
    {
        public static void sendMsg(string controllerUrl, string json)
        {
            TcpFunction.WithoutResponse.SendInmationToUrl(controllerUrl, json);
        }
    }
}
