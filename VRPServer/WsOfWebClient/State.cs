using System;
using System.Collections.Generic;
using System.Text;

namespace WsOfWebClient
{
    public class State
    {
        public int WebsocketID { get; set; }
        public LoginState Ls { get; set; }
        public int roomIndex { get; set; }
        public string mapRoadAndCrossMd5 { get; internal set; }

        /// <summary>
        /// AddPlayer产生的唯一ID
        /// </summary>
        public string Key { get; internal set; }
        public string CommandStart { get; internal set; }


        public int indexOfModelToTranstract = 0;

        /// <summary>
        /// 队员离开队伍时，用于传递队伍ID参数。
        /// </summary>
        internal string teamID = "";
    }
    public class IntroState
    {
        public string randomValue { get; internal set; }
        public int randomCharacterCount { get; set; }
    }
    public enum LoginState
    {
        empty,
        selectSingleTeamJoin,
        OnLine,
        WaitingToStart,
        WaitingToGetTeam,
        LookForBuildings,
        QueryReward,
        Guid
    }
}
