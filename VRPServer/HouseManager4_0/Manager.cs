using HouseManager4_0.interfaceOfEngine;
using System.Threading;
using static HouseManager4_0.RoomMainF.RoomMain;

namespace HouseManager4_0
{
    public abstract class Manager : EngineAndManger
    {
        public void startNewCommandThread(int startT, CommonClass.Command command, interfaceOfEngine.startNewCommandThread objNeedToStartNewCommandThread)
        {
            Thread th = new Thread(() => newThreadDoBefore(startT, command, objNeedToStartNewCommandThread));
            th.Start();
        }
        void newThreadDoBefore(int startT, CommonClass.Command command, interfaceOfEngine.startNewCommandThread objNeedToStartNewThread)
        {
            Thread.Sleep(startT);
            objNeedToStartNewThread.newThreadDo(command);
        }
    }
}
