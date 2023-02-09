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

        public static List<int> sendSeveralMsgs(List<string> sendMsgs)
        {
            List<Task<int>> tasks = new List<Task<int>>();
            for (var i = 0; i < sendMsgs.Count; i += 2)
            {
                int indexOfMsg = i + 0;
                Task<int> t1 = new Task<int>(() =>
                {
                    var url = Convert.ToString(sendMsgs[indexOfMsg] + "      ").Trim();
                    var msg = Convert.ToString(sendMsgs[indexOfMsg + 1] + "  ").Trim();
                    Startup.sendSingleMsg(url, msg);
                    return indexOfMsg;
                }
                );
                tasks.Add(t1);
            }
            for (int i = 0; i < tasks.Count; i++)
            {
                tasks[i].Start();
            }
            Task.WaitAll(tasks.ToArray());
            List<int> Result = new List<int>();
            for (int i = 0; i < tasks.Count; i++)
            {
                Result.Add(tasks[i].Result);
            }
            while (Result.Count > 1)
            {
                var current = Result[0];
                var next = Result[1];
                if (current + 2 == next)
                {
                    Result.RemoveAt(0);
                }
                else throw new Exception("sendSeveralMsgs 方法报异常！");
            } 
            return Result;
        }
    }
}
