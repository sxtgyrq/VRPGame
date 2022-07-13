using CommonClass;
using System;
using System.Collections.Generic;
using System.Text;
using static HouseManager4_0.RoomMainF.RoomMain;

namespace HouseManager4_0.interfaceOfHM
{
    interface ListenInterface : MapEditor, ModelTranstractionI
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
        string CheckCarStateF(CheckCarState ccs);
        void SystemBradcast(SystemBradcast sb);
    }

    interface MapEditor
    {
        string UseBackgroundSceneF(CommonClass.MapEditor.UseBackgroundScene sbs);
        string GetBackgroundSceneF(CommonClass.MapEditor.GetBackgroundScene gbs);
        string SetBackgroundSceneF(CommonClass.MapEditor.SetBackgroundScene_BLL sbs);
        string GetFirstRoad();
        string DrawRoad(CommonClass.MapEditor.DrawRoad dr);
        string NextCross(CommonClass.MapEditor.NextCross dr);
        string PreviousCross(CommonClass.MapEditor.PreviousCross dr);
        string GetCatege(CommonClass.MapEditor.GetCatege gc);
        string GetModelType(CommonClass.MapEditor.GetCatege gc);
        string GetAbtractModels(CommonClass.MapEditor.GetAbtractModels gam);
        string SaveObjInfo(CommonClass.MapEditor.SaveObjInfo soi);
        string ShowOBJFile(CommonClass.MapEditor.ShowOBJFile sof);
        string UpdateObjInfo(CommonClass.MapEditor.UpdateObjInfo uoi);
        string DelObjInfo(CommonClass.MapEditor.DelObjInfo doi);
        string CreateNew(CommonClass.MapEditor.CreateNew cn);
        string GetModelDetail(CommonClass.MapEditor.GetModelDetail cn);
        string UseModelObj(CommonClass.MapEditor.UseModelObj cn);
        string LockModelObj(CommonClass.MapEditor.UseModelObj cn);
        string ClearModelObj();
        string GetUnLockedModel(CommonClass.MapEditor.GetUnLockedModel gulm);
        void UpdateModelStock(ModelStock sa);
    }

    interface ModelTranstractionI
    {
        // string GetFirstModelAddr(ModelTranstraction.GetFirstModelAddr gfm);
        string GetTransctionModelDetail(ModelTranstraction.GetTransctionModelDetail gtmd);
        string GetTransctionFromChainF(ModelTranstraction.GetTransctionFromChain gtfc);
        string GetRoadNearby(ModelTranstraction.GetRoadNearby grn);
        string TradeCoinF(ModelTranstraction.TradeCoin tc);
        string GetAllModelPosition();
        string GetModelByID(ModelTranstraction.GetModelByID gmbid);
        string TradeIndex(ModelTranstraction.TradeIndex tc);
    }
}
