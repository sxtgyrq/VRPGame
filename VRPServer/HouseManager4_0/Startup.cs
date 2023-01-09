using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HouseManager4_0
{
    class Startup
    {
        public static string sendSingleMsg(string controllerUrl, string json)
        {
            if (string.IsNullOrEmpty(controllerUrl))
            {
                return "";
            }
            else
            {
                var t1 = TcpFunction.WithResponse.SendInmationToUrlAndGetRes(controllerUrl, json);
                return t1.GetAwaiter().GetResult();
            }
        }

        public static List<string> sendSeveralMsgs(List<string> sendMsgs)
        {
            List<Task<string>> tasks = new List<Task<string>>();
            for (var i = 0; i < sendMsgs.Count; i += 2)
            {
                var url = sendMsgs[i];
                var msg = sendMsgs[i + 1];
                Task<string> t1 = new Task<string>(() => Startup.sendSingleMsg(url, msg));
                tasks.Add(t1);
            }
            for (int i = 0; i < tasks.Count; i++)
            {
                tasks[i].Start();
            }
            Task.WaitAll(tasks.ToArray());
            List<string> Result = new List<string>();
            for (int i = 0; i < tasks.Count; i++)
            {
                Result.Add(tasks[i].Result);
            }
            return Result;
        }
    }
}
