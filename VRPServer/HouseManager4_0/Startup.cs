using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HouseManager4_0
{
    class Startup
    {
        public static string sendMsg(string controllerUrl, string json)
        {
            if (string.IsNullOrEmpty(controllerUrl))
            {
                return "";
            }
            else
            {
                var r = Task.Run<string>(() => TcpFunction.WithResponse.SendInmationToUrlAndGetRes(controllerUrl, json));
                return r.Result;
            }
        }
    }
}
