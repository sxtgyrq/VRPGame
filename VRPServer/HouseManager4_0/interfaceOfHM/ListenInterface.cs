using CommonClass;
using System;
using System.Collections.Generic;
using System.Text;
using static HouseManager4_0.RoomMainF.RoomMain;

namespace HouseManager4_0.interfaceOfHM
{
    interface ListenInterface
    {
        /// <summary>
        /// 新增玩家
        /// </summary>
        /// <param name="addItem"></param>
        /// <returns></returns>
        string AddPlayer(PlayerAdd_V2 addItem);

        /// <summary>
        /// 实际功能是初始化！
        /// </summary>
        /// <param name="getPosition"></param>
        /// <returns></returns>
        GetPositionResult GetPosition(GetPosition getPosition);

        /// <summary>
        /// 寻找宝石
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        string updatePromote(SetPromote sp);

        /// <summary>
        /// 提升能力
        /// </summary>
        /// <param name="sa"></param>
        /// <returns></returns>
        string SetAbility(SetAbility sa);

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="checkItem"></param>
        /// <returns></returns>
        string UpdatePlayer(PlayerCheck checkItem);

        /// <summary>
        /// 收集
        /// </summary>
        /// <param name="sc"></param>
        /// <returns></returns>
        string updateCollect(SetCollect sc);

        /// <summary>
        /// 攻击
        /// </summary>
        /// <param name="sa"></param>
        /// <returns></returns>
        string updateAttack(SetAttack sa);



        /// <summary>
        /// 命令其返回！
        /// </summary>
        /// <param name="otr"></param>
        /// <returns></returns>
        string OrderToReturn(OrderToReturn otr);

        /// <summary>
        /// 保存金钱
        /// </summary>
        /// <param name="saveMoney"></param>
        /// <returns></returns>
        string SaveMoney(SaveMoney saveMoney);

        /// <summary>
        /// 更新价格
        /// </summary>
        /// <param name="sa"></param>
        void MarketUpdate(MarketPrice sa);

        /// <summary>
        /// 从市场上购买宝石
        /// </summary>
        /// <param name="bd"></param>
        void Buy(SetBuyDiamond bd);

        /// <summary>
        /// 出售给市场！
        /// </summary>
        /// <param name="ss"></param>
        void Sell(SetSellDiamond ss);

        /// <summary>
        /// 命名为捐赠，实为存储！
        /// </summary>
        /// <param name="ots"></param>
        void OrderToSubsidize(OrderToSubsidize ots);

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="dm"></param>
        void SendMsg(DialogMsg dm);
        void SelectDriver(SetSelectDriver dm);
        string updateMagic(MagicSkill ms);
        string updateView(View v);
    }
}
