using HouseManager4_0.RoomMainF;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
        public void startNewThread(int startT, baseC dOwner, interfaceOfEngine.startNewThread objNeedToStartNewThread, GetRandomPos grp)
        {
            Thread th = new Thread(() => newThreadDoBefore(startT, dOwner, objNeedToStartNewThread, grp));
            th.Start();
            //  throw new NotImplementedException();
        }
        public void newThreadDoBefore(int startT, baseC dOwner, interfaceOfEngine.startNewThread objNeedToStartNewThread, GetRandomPos grp)
        {
            Thread.Sleep(startT);
            objNeedToStartNewThread.newThreadDo(dOwner, grp);
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
        public string sendSingleMsg(string controllerUrl, string json)
        {
            return Startup.sendSingleMsg(controllerUrl, json);
        }

        public List<int> sendSeveralMsgs(List<string> notifyMsg)
        {
            return Startup.sendSeveralMsgs(notifyMsg);
        }
        //public List<string> sendMsg(List<string> notifyMsg)
        //{
        //    List<string> result = new List<string>();
        //    for (var i = 0; i < notifyMsg.Count; i += 2)
        //    {
        //        var url = notifyMsg[i];
        //        var sendMsg = notifyMsg[i + 1];
        //        if (!string.IsNullOrEmpty(url))
        //        {
        //            result.Add(this.sendMsg(url, sendMsg));
        //        }
        //    }
        //    return result;
        //}
    }
}
