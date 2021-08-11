using CommonClass;
using System;
using System.Collections.Generic;
using System.Text;
using static HouseManager4_0.RoomMainF.RoomMain.commandWithTime;

namespace HouseManager4_0.interfaceOfEngine
{
    interface engine : webnotify
    {
    }

    interface webnotify : sendMsg, wait
    {
        void WebNotify(RoleInGame player, string Msg);
    }

    interface sendMsg
    {
        void sendMsg(string controllerUrl, string json);
    }

    interface tryCatchAction
    {
        RoomMainF.RoomMain.commandWithTime.ReturningOjb maindDo(RoleInGame player, Car car, Command c, ref List<string> notifyMsg, out RoomMainF.RoomMain.MileResultReason mrr);
        void failedThenDo(Car car, RoleInGame player, Command c, ref List<string> notifyMsg);
        bool conditionsOk(Command c, out string reason);

        bool carAbilitConditionsOk(RoleInGame player, Car car, Command c);
        //  Command get
    }

    interface wait
    {
        void ThreadSleep(int mSecondsWait);
    }
    public interface startNewThread
    {
        void startNewThread(int timeC, baseC baseObj, startNewThread self);
        void newThreadDo(baseC dObj);
    }

    public interface startNewCommandThread
    {
        void startNewCommandThread(int timeC, CommonClass.Command c, startNewCommandThread self);
        void newThreadDo(CommonClass.Command c);
    }
}
