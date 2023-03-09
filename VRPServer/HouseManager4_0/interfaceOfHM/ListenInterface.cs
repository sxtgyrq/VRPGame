using CommonClass;
using System;
using System.Collections.Generic;
using System.Text;
using static CommonClass.Finance;
using static HouseManager4_0.RoomMainF.RoomMain;

namespace HouseManager4_0.interfaceOfHM
{
    interface ListenInterface : MapEditor, ModelTranstractionI, Resistance
    {
        /// <summary>
        /// 新增玩家
        /// </summary>
        /// <param name="addItem"></param>
        /// <returns></returns>
        string AddPlayer(PlayerAdd_V2 addItem, interfaceOfHM.Car cf, GetRandomPos gp);

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
        string updatePromote(SetPromote sp, GetRandomPos grp);

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
        string updateCollect(SetCollect sc, GetRandomPos grp);

        /// <summary>
        /// 攻击
        /// </summary>
        /// <param name="sa"></param>
        /// <returns></returns>
        string updateAttack(SetAttack sa, GetRandomPos grp);



        /// <summary>
        /// 命令其返回！
        /// </summary>
        /// <param name="otr"></param>
        /// <returns></returns>
        string OrderToReturn(OrderToReturn otr, GetRandomPos grp);

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
        /// 释玉--web前台对应
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        string TakeApartF(TakeApart t);
        /// <summary>
        /// 取款，同步等级！
        /// </summary>
        /// <param name="ots"></param>
        void OrderToSubsidize(OrderToSubsidize ots);
        //  void OrderToUpdateLevel(OrderToUpdateLevel oul);
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="dm"></param>
        void SendMsg(DialogMsg dm);
        void SelectDriver(SetSelectDriver dm);
        string updateMagic(MagicSkill ms, GetRandomPos grp);
        string updateView(View v);
        string CheckCarStateF(CheckCarState ccs);
        void SystemBradcast(SystemBradcast sb);
        string Statictis(ServerStatictis ss);
        string GetFightSituationF(GetFightSituation fs);
        string GetTaskCopyDetailF(GetTaskCopyDetail gtd);
        string RemoveTaskCopyF(RemoveTaskCopyM gtd);
        string ExitF(ExitObj obj);
        string GetOnLineStateF(GetOnLineState obj);
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
        string GetBG(SetCrossBG ss);
        string GetHeightAtPositionF(CommonClass.MapEditor.GetHeightAtPosition gh, Data dt); 
        string LookForTaskCopyF(LookForTaskCopy lftc);
        string TaskCopyPassOrNGF(TaskCopyPassOrNG pOrNG);
    }

    interface ModelTranstractionI
    {
        // string GetFirstModelAddr(ModelTranstraction.GetFirstModelAddr gfm);
        string GetTransctionModelDetail(ModelTranstraction.GetTransctionModelDetail gtmd);
        string GetTransctionFromChainF(ModelTranstraction.GetTransctionFromChain gtfc);
        string GetRoadNearby(ModelTranstraction.GetRoadNearby grn);
        string TradeCoinF(ModelTranstraction.TradeCoin tc);
        string TradeSetAsRewardF(ModelTranstraction.TradeSetAsReward tsar);
        string GetAllModelPosition();
        string GetModelByID(ModelTranstraction.GetModelByID gmbid);
        string TradeIndex(ModelTranstraction.TradeIndex tc);
        string GetRewardFromBuildingF(GetRewardFromBuildingM m);
        string GetAllBuiisnessAddr();
        // string GetAllStockAddr(AllStockAddr ss);
        string GetRewardInfomationByStartDate(ModelTranstraction.RewardInfomation ri);
        string GetRewardApplyInfomationByStartDate(ModelTranstraction.RewardInfomation ri);
        string RewardApplyF(ModelTranstraction.RewardApply rA, bool ignoreDataCheck);
        string AwardsGive(ModelTranstraction.AwardsGivingPass aG, bool ignoreDataCheck);
        string BindWordInfoF(ModelTranstraction.BindWordInfo bwi, Data dt);
        string LookForBindInfoF(ModelTranstraction.LookForBindInfo lfbi, Data dt);
        string ChargingF(Finance.Charging chargingObj, Data dt);
        string ChargingLookForF(Finance.ChargingLookFor condition);
        string ChargingMax();
        string RewardBuildingShowF(ModelTranstraction.RewardBuildingShow rbs);
    }

    interface Resistance
    {
        string GetResistance(GetResistanceObj r);
    }
}
