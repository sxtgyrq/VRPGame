﻿using CommonClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager4_0.interfaceOfHM
{
    interface Player
    {
        string AddPlayer(PlayerAdd_V2 addItem);
        string UpdatePlayer(PlayerCheck checkItem);
    }
}
