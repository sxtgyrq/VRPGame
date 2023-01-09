using CommonClass;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static HouseManager4_0.RoomMainF.RoomMain.commandWithTime;

namespace HouseManager4_0.interfaceOfEngine
{
    interface engine : webnotify
    {
    }
    interface manager : webnotify
    {
    }
    interface webnotify : sendMsg, wait
    {
        void WebNotify(RoleInGame player, string Msg);
    }

    interface sendMsg
    {
        //string sendMsg(string controllerUrl, string json);
        string sendSingleMsg(string controllerUrl, string json);
    }

    interface tryCatchAction
    {
        RoomMainF.RoomMain.commandWithTime.ReturningOjb maindDo(RoleInGame player, Car car, Command c, GetRandomPos grp, ref List<string> notifyMsg, out RoomMainF.RoomMain.MileResultReason mrr);
        void failedThenDo(Car car, RoleInGame player, Command c, GetRandomPos grp, ref List<string> notifyMsg);
        /// <summary>
        /// 返回为真/假，但不一定要执行返回
        /// </summary>
        /// <param name="c"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        bool conditionsOk(Command c, GetRandomPos grp, out string reason);

        /// <summary>
        /// 返回为真/假，要执行failedThenDo
        /// </summary>
        /// <param name="player"></param>
        /// <param name="car"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        bool carAbilitConditionsOk(RoleInGame player, Car car, Command c, GetRandomPos grp);
        //  Command get
    }

    interface wait
    {
        void ThreadSleep(int mSecondsWait);
    }
    public interface startNewThread
    {
        void startNewThread(int timeC, baseC baseObj, startNewThread self, GetRandomPos grp);
        void newThreadDo(baseC dObj, GetRandomPos grp);
    }

    public interface startNewCommandThread
    {
        void startNewCommandThread(int timeC, CommonClass.Command c, startNewCommandThread self, GetRandomPos grp);
        void newThreadDo(CommonClass.Command c, GetRandomPos grp);
    }
}
