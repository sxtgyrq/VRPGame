using CommonClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager4_0.interfaceOfHM
{
    interface Attack
    {
        /*
       * 攻击的目的是为了增加股份。
       * A玩家，拿100块钱攻击B玩家，B玩家的债务增加100，使用金额都增加90
       * 当第一股东不是B玩家时，是A玩家时。A玩家有权对B进行破产清算。
       */
        string updateAttack(SetAttack sa, GetRandomPos grp);
    }
}
