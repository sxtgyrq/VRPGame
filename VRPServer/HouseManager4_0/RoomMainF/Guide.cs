using CommonClass;
using System;
using System.Collections.Generic;
using System.Text;
using static HouseManager4_0.Engine;

namespace HouseManager4_0.RoomMainF
{
    public partial class RoomMain
    {
        private void NoNeedToLogin(Player player)
        {
            SetParameterIsLogin spil = new SetParameterIsLogin()
            {
                c = "SetParameterIsLogin",
                WebSocketID = player.WebSocketID
                //  TimeOut
            };
            Startup.sendSingleMsg(player.FromUrl, Newtonsoft.Json.JsonConvert.SerializeObject(spil)); 
        }
        private void HasNewTaskToShow(Player player)
        {
            //SetParameterHasNewTask
            SetParameterHasNewTask spil = new SetParameterHasNewTask()
            {
                c = "SetParameterHasNewTask",
                WebSocketID = player.WebSocketID
                //  TimeOut
            };
            Startup.sendSingleMsg(player.FromUrl, Newtonsoft.Json.JsonConvert.SerializeObject(spil));
        }
    }
}
