using HouseManager4_0.RoomMainF;
using System.Collections.Generic;
using System.Threading;
using static HouseManager4_0.RoomMainF.RoomMain.commandWithTime;

namespace HouseManager4_0
{
    public abstract class EngineAndManger : SendMsg
    {
        protected RoomMain roomMain;
        protected RoomMain that
        {
            get { return this.roomMain; }
        }
        public void startNewThread(int startT, baseC dOwner, interfaceOfEngine.startNewThread objNeedToStartNewThread)
        {
            Thread th = new Thread(() => newThreadDoBefore(startT, dOwner, objNeedToStartNewThread));
            th.Start();
            //  throw new NotImplementedException();
        }
        public void newThreadDoBefore(int startT, baseC dOwner, interfaceOfEngine.startNewThread objNeedToStartNewThread)
        {
            Thread.Sleep(startT);
            objNeedToStartNewThread.newThreadDo(dOwner);
        }
        public void WebNotify(RoleInGame player, string Msg)
        {

            roomMain.WebNotify(player, Msg);
        }
        //public void sendMsg(string controllerUrl, string json)
        //{
        //    Startup.sendMsg(controllerUrl, json);
        //}
        public void ThreadSleep(int mSecondsWait)
        {
            Thread.Sleep(mSecondsWait);
        }


        //public void startNewCommandThread(int timeC, CommonClass.Command c, startNewThread self)
        //{
        //    throw new NotImplementedException();
        //}


        //public void newThreadDoBefore(int startT, baseC dOwner, interfaceOfEngine.startNewThread objNeedToStartNewThread)
        //{
        //    Thread.Sleep(startT);
        //    objNeedToStartNewThread.newThreadDo(dOwner);
        //}
    }

    public abstract class OperateObj : SendMsg
    {
        protected RoomMain roomMain;
        protected RoomMain that { get { return this.roomMain; } }
    }

    public abstract class SendMsg
    {
        public void sendMsg(string controllerUrl, string json)
        {
            Startup.sendMsg(controllerUrl, json);
        }
        public void sendMsg(List<string> notifyMsg)
        {
            for (var i = 0; i < notifyMsg.Count; i += 2)
            {
                var url = notifyMsg[i];
                var sendMsg = notifyMsg[i + 1];
                //Console.WriteLine($"url:{url}");
                if (!string.IsNullOrEmpty(url))
                {
                    this.sendMsg(url, sendMsg);
                    //  Startup.sendMsg(url, sendMsg);
                }
            }
        }
    }
}
