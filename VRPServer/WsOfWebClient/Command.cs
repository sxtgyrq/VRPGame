﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WsOfWebClient
{

    public class CheckSession : CommonClass.Command
    {
        public string session { get; set; }
    }

    public class MapRoadAndCrossMd5 : CommonClass.Command
    {
        public string mapRoadAndCrossMd5 { get; set; }
    }

    public class JoinGameSingle : CommonClass.Command
    {
    }

    public class CreateTeam : CommonClass.Command
    {
    }

    public class JoinTeam : CommonClass.Command
    {
    }

    public class SetPlayerName : CommonClass.Command
    {
        public string Name { get; set; }
    }

    public class SetCarName : CommonClass.Command
    {
        public string Name { get; set; }
        public int CarIndex { get; set; }
    }
    public class Promote : CommonClass.Command
    {
        public string pType { get; set; }
        public string car { get; set; }
    }

    public class Collect : CommonClass.Command
    {
        public string cType { get; set; }
        public string car { get; set; }
    }

}