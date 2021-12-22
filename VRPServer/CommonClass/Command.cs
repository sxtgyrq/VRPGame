﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CommonClass
{
    public class Command
    {
        public string c { get; set; }
    }
    public class Register : Command
    {
        public string ControllerUrl { get; set; }
    }
    public class Map : Command
    {
        public string DataType { get; set; }
    }

    public class CommandNotify : Command
    {
        public int WebSocketID { get; set; }
    }

    public class GetPositionNotify : CommandNotify
    {
        public Model.FastonPosition fp { get; set; }
        public string[] carsNames { get; set; }
        public string key { get; set; }
        public string PlayerName { get; set; }
    }
    public class GetPositionNotify_v2 : CommandNotify
    {
        public Model.FastonPosition fp { get; set; }
        public int fPIndex { get; set; }
        public string key { get; set; }
        public string PlayerName { get; set; }
        public int positionInStation { get; set; }
    }
    public class GetOthersPositionNotify : CommandNotify
    {
        public Model.FastonPosition fp { get; set; }
        public string[] carsNames { get; set; }
        public string key { get; set; }
        public string PlayerName { get; set; }
        public int fPIndex { get; set; }
    }
    public class GetOthersPositionNotify_v2 : CommandNotify
    {
        public Model.FastonPosition fp { get; set; }
        public string key { get; set; }
        public string PlayerName { get; set; }
        public int fPIndex { get; set; }
        public int positionInStation { get; set; }
        public bool isPlayer { get; set; }
        public bool isNPC { get; set; }
        public int Level { get; set; }
    }

    public class TaxNotify : CommandNotify
    {
        public Model.FastonPosition fp { get; set; }
        public long tax { get; set; }
        public int target { get; set; }
    }
    public class FrequencyNotify : CommandNotify
    {
        //public Model.FastonPosition fp { get; set; }
        //public long tax { get; set; }
        //public int target { get; set; }
        public int frequency { get; set; }
    }
    public class MoneyForSaveNotify : CommandNotify
    {
        public long MoneyForSave { get; set; }
    }
    public class MoneyNotify : CommandNotify
    {
        public long Money { get; set; }
    }

    public class DriverNotify : CommandNotify
    {
        public int index { get; set; }
        public string name { get; set; }
        public int skill1Index { get; set; }
        public int skill2Index { get; set; }
        public string skill1Name { get; set; }
        public string skill2Name { get; set; }
        public string sex { get; set; }
        public string race { get; set; }
    }
    public class SpeedNotify : CommandNotify
    {
        public string Key { get; set; }
        public bool On { get; set; }
    }
    public class ConfuseNotify : CommandNotify
    {
        public string Key { get; set; }
        public bool On { get; set; }
    }
    public class LoseNotify : CommandNotify
    {
        public string Key { get; set; }
        public bool On { get; set; }
    }
    public class FireNotify : CommandNotify
    {
        public string targetRoleID { get; set; }
        public string actionRoleID { get; set; }
    }
    public class WaterNotify : CommandNotify
    {
        public string targetRoleID { get; set; }
        public string actionRoleID { get; set; }
    }
    public class ElectricNotify : CommandNotify
    {
        public string targetRoleID { get; set; }
        public string actionRoleID { get; set; }
    }
    public class ViewSearch : CommandNotify
    {
        public int mctX { get; set; }
        public int mctY { get; set; }
    }
    public class AttackNotify : CommandNotify
    {
        public string Key { get; set; }
        public bool On { get; set; }
    }
    public class DefenceNotify : CommandNotify
    {
        public string Key { get; set; }
        public bool On { get; set; }
    }
    public class ConfusePrepareNotify : CommandNotify
    {
        public string Key { get; set; }
        public bool On { get; set; }
        public int StartX { get; set; }
        public int StartY { get; set; }
        public int EndX { get; set; }
        public int EndY { get; set; }
    }
    public class LostPrepareNotify : CommandNotify
    {
        public string Key { get; set; }
        public bool On { get; set; }
        public int StartX { get; set; }
        public int StartY { get; set; }
        public int EndX { get; set; }
        public int EndY { get; set; }
    }
    public class AmbushPrepareNotify : CommandNotify
    {
        public string Key { get; set; }
        public bool On { get; set; }
        public int StartX { get; set; }
        public int StartY { get; set; }
        public int EndX { get; set; }
        public int EndY { get; set; }
    }
    public class ControlPrepareNotify : CommandNotify
    {
        public string Key { get; set; }
        public bool On { get; set; }
    }
    public class OthersRemove : CommandNotify
    {
        public string othersKey;
    }
    public class WMsg : CommandNotify
    {
        public string Msg;
    }
    public class DebtsRemove : CommandNotify
    {
        public string othersKey;
    }

    public class BustStateNotify : CommandNotify
    {
        public bool Bust { get; set; }
        public string Key { get; set; }
    }

    public class TheLargestHolderChangedNotify : CommandNotify
    {
        public string operateKey { get; set; }
        public string ChangeTo { get; set; }
        public string operateName { get; set; }
        public string nameTo { get; set; }
    }
    public class SingleRoadPathData : CommandNotify
    {
        //  public List<double[]> meshPoints { get; set; }
        public List<int> meshPoints { get; set; }
        public List<int> basePoint { get; set; }
    }

    public class ModelDataShow : CommandNotify
    {
        public string modelID { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
        public string amodel { get; set; }
        public double rotatey { get; set; }
        public bool existed { get; set; }
        public string imageBase64 { get; set; }
        public string objText { get; set; }
        public string mtlText { get; set; }
    }

    public class SupportNotify : CommandNotify
    {
        public long Money { get; set; }
    }
    public class LeftMoneyInDB : CommandNotify
    {
        public long Money { get; set; }
        public string address { get; set; }
    }

    public class BradCastAnimateOfCar : CommandNotify
    {
        public string carID;

        public object Animate { get; set; }
    }
    public class BradCastAnimateOfOthersCar2 : CommandNotify
    {
        public string carID;

        public object Animate { get; set; }
        public string parentID { get; set; }


    }

    public class BradCastMoneyForSave : CommandNotify
    {
        public long Money { get; set; }
    }
    public class BradCastPromoteDiamondCount : CommandNotify
    {
        public int count { get; set; }
        public string pType { get; set; }
    }

    public class BradCastAbility : CommandNotify
    {
        public string pType { get; set; }
        public string carIndexStr { get; set; }
        public long costValue { get; set; }
        public long sumValue { get; set; }
    }
    public class BradCarState : CommandNotify
    {
        public string State { get; set; }
        public string carID { get; set; }
    }
    public class BradDiamondPrice : CommandNotify
    {
        public string State { get; set; }
        public string carID { get; set; }
        public string priceType { get; set; }
        public long price { get; set; }
    }
    public class BradCarPurpose : CommandNotify
    {
        public string Purpose { get; set; }
        public string carID { get; set; }
    }
    public class BradCastCarAbility : CommandNotify
    {
        public object Number { get; set; }
        public string pType { get; set; }
        public string sumOrCost { get; set; }
    }
    public class BradCastAnimateOfSelfCar : CommandNotify
    {
        public string carID;

        public object Animate { get; set; }
        public string parentID { get; set; }
        public decimal CostMile { get; set; }
        public int LeftMile { get; set; }
    }
    public class BradCastSocialResponsibility : CommandNotify
    {
        public long socialResponsibility { get; set; }
        // public string key { get; set; }
        public string otherKey { get; set; }
    }
    public class BradCastRightAndDuty : CommandNotify
    {
        public string playerKey { get; set; }
        public long right { get; set; }
        public long duty { get; set; }
        public int rightPercent { get; set; }
        public int dutyPercent { get; set; }
    }

    public class BradCastPromoteInfoDetail : CommandNotify
    {
        /// <summary>
        /// "mile","business","volume","speed"四种状态
        /// </summary>
        public string resultType { get; set; }
        public Model.FastonPosition Fp { get; set; }
        public decimal Price { get; set; }
    }
    public class BradCastCollectInfoDetail : CommandNotify
    {
        public Model.FastonPosition Fp { get; set; }
        public int collectMoney { get; set; }
    }
    public class BradCastCollectInfoDetail_v2 : CommandNotify
    {
        public Model.FastonPosition Fp { get; set; }
        public int collectMoney { get; set; }
        public int collectIndex { get; set; }
    }

    public class BradCastMusicTheme : CommandNotify
    {
        public string theme { get; set; }
    }

    public class ShowDirectionOperator : CommandNotify
    {
        /// <summary>
        /// item为 旋转的弧度。0~2π
        /// </summary>
        public double[] direction { get; set; }
        public double positionX { get; set; }
        public double positionY { get; set; }
    }
    public class BradCastBackground : CommandNotify
    {
        public string path { get; set; }
    }

    public class PlayerAdd : Command
    {
        public string Key { get; set; }
        public string FromUrl { get; set; }
        public int RoomIndex { get; set; }

        public string Check { get; set; }
        public int WebSocketID { get; set; }
        public string PlayerName { get; set; }
        public string[] CarsNames { get; set; }
    }
    public class PlayerAdd_V2 : Command
    {
        public string Key { get; set; }
        public string FromUrl { get; set; }
        public int RoomIndex { get; set; }

        public string Check { get; set; }
        public int WebSocketID { get; set; }
        public string PlayerName { get; set; }
    }

    public class GetFrequency : Command
    {
    }

    public class PlayerCheck : PlayerAdd_V2 { }

    public class GetPosition : Command
    {
        public string Key { get; set; }
    }

    public class SetPromote : Command
    {
        public string Key { get; set; }
        /// <summary>
        /// 取值如mile
        /// </summary>
        public string pType { get; set; }
        //   public string car { get; set; }
    }
    public class SetCollect : Command
    {
        public string Key { get; set; }
        /// <summary>
        /// 取值如findWork
        /// </summary>
        public string cType { get; set; }

        public string fastenpositionID { get; set; }
        /// <summary>
        /// 取值0~38
        /// </summary>
        public int collectIndex { get; set; }
        //public string car { get; set; }
    }

    public class SetAttack : Command
    {
        public string Key { get; set; }
        //public string car { get; set; }
        public string targetOwner { get; set; }
        public int target { get; set; }
    }
    public class MagicSkill : SetAttack
    {
        /// <summary>
        /// 魔法技能的选择。取值只能是1或2，出现其他值，系统报错不处理就对了。1一般为种族技能，2为性别技能。
        /// </summary>
        public int selectIndex { get; set; }
    }
    public class View : Command
    {
        public string Key { get; set; }
        /// <summary>
        /// camera X
        /// </summary>
        public double x1 { get; set; }
        /// <summary>
        /// camera Y
        /// </summary>
        public double y1 { get; set; }
        /// <summary>
        /// Target X
        /// </summary>
        public double x2 { get; set; }
        /// <summary>
        /// TargetY
        /// </summary>
        public double y2 { get; set; }
    }

    public class SetBust : Command
    {
        public string Key { get; set; }
        //  public string car { get; set; }
        public string targetOwner { get; set; }
        public int target { get; set; }
    }

    public class SetTax : Command
    {
        public string Key { get; set; }
        //public string car { get; set; }
        public int target { get; set; }
    }

    public class SetAbility : Command
    {
        public string Key { get; set; }
        //public string car { get; set; }
        public string pType { get; set; }
    }


    public class OrderToReturn : Command
    {
        public string Key { get; set; }
        //public string car { get; set; }
    }
    public class OrderToReturnBySystem : OrderToReturn
    {
    }

    public class OrderToSubsidize : Command
    {
        public string Key { get; set; }
        public string address { get; set; }
        public string signature { get; set; }
        public long value { get; set; }
    }
    public class SaveMoney : Command
    {
        public string Key { get; set; }
        public string dType { get; set; }
        public string address { get; set; }
    }
    public class GetTax : Command
    {
        public string Key { get; set; }
        public string car { get; set; }
        public string targetOwner { get; set; }
        public int target { get; set; }
    }
    public class GetBtns : Command
    {
        public string FromUrl { get; set; }
        public int RoomIndex { get; set; }
        public int WebSocketID { get; set; }
    }

    public class TeamCreate : Command
    {
        public string FromUrl { get; set; }
        public string CommandStart { get; set; }
        public int WebSocketID { get; set; }
        public string PlayerName { get; set; }
    }
    public class GetPromoteMiles : CommonClass.Command
    {
        public string FromUrl { get; set; }
        public int WebSocketID { get; set; }
    }

    public class TeamBegain : Command
    {
        public int TeamNum { get; set; }
        public int RoomIndex { get; set; }
    }

    public class TeamJoin : Command
    {
        public string FromUrl { get; set; }
        public string CommandStart { get; set; }
        public int WebSocketID { get; set; }
        public string PlayerName { get; set; }
        public string TeamIndex { get; set; }

    }

    public class TeamCreateFinish : CommandNotify
    {
        public string CommandStart { get; set; }
        public int TeamNum { get; set; }
        public string PlayerName { get; set; }
    }
    public class TeamJoinBroadInfo : CommandNotify
    {
        public string PlayerName { get; set; }
    }
    public class TeamJoinFinish : CommandNotify
    {
        public int TeamNum { get; set; }
        public List<string> PlayerNames { get; set; }
    }
    public class TeamNumWithSecret : CommandNotify
    {
        public string Secret { get; set; }
    }
    public class TeamResult : Command
    {
        public string FromUrl { get; set; }
        public int WebSocketID { get; set; }
        /// <summary>
        /// 作为队伍的索引
        /// </summary>
        public int TeamNumber { get; set; }
    }
    public class TeamResultForGameBegain : TeamResult
    {
        public int roomIndex { get; set; }
    }

    public class TeamFoundResult : Command
    {
        public bool HasResult { get; set; }
        /// <summary>
        /// 作为队伍的索引
        /// </summary>
        public int TeamNumber { get; set; }
    }
    //public class TeamBegain : Command
    //{
    //    public int TeamNumber { get; set; }
    //}

    public class RoomNumberResult : Command
    {
        public int RoomIndex { get; set; }
        public string PassMd5 { get; set; }
        public string CheckMd5 { get; set; }

    }

    public class DialogMsg : CommandNotify
    {
        public string Key { get; set; }
        public string To { get; set; }
        public string Msg { get; set; }
    }
    public class PassRoomMd5Check
    {
        public int RoomIndex { get; set; }
        public string StartMd5 { get; set; }
        public string CheckMd5 { get; set; }

        public string RoomIndexWithAes { get; set; }
    }
}
