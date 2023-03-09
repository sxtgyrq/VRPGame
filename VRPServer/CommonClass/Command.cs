using System;
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
        //   public int TimeOut { get; set; }
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
    public class GoodsSelectionNotify : CommandNotify
    {
        public double x { get; set; }

        public double z { get; set; }
        public double y { get; set; }
        public string[] selections { get; set; }
        public double[] positions { get; set; }
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
    public class ElectricMarkNotify : CommandNotify
    {
        public double[] lineParameter { get; set; }
    }
    public class WaterMarkNotify : CommandNotify
    {
        public double[] lineParameter { get; set; }
    }
    public class FireMarkNotify : CommandNotify
    {
        public double[] lineParameter { get; set; }
    }
    public class ViewSearch : CommandNotify
    {
        public int mctX { get; set; }
        public int mctY { get; set; }
    }
    //public class SetBG:
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
        public int StartZ { get; set; }
        public int EndX { get; set; }
        public int EndY { get; set; }
        public int EndZ { get; set; }
    }
    public class LostPrepareNotify : CommandNotify
    {
        public string Key { get; set; }
        public bool On { get; set; }
        public int StartX { get; set; }
        public int StartY { get; set; }
        public int StartZ { get; set; }
        public int EndX { get; set; }
        public int EndY { get; set; }
        public int EndZ { get; set; }
    }
    public class AmbushPrepareNotify : CommandNotify
    {
        public string Key { get; set; }
        public bool On { get; set; }
        public int StartX { get; set; }
        public int StartY { get; set; }
        public int StartZ { get; set; }
        public int EndX { get; set; }
        public int EndY { get; set; }
        public int EndZ { get; set; }
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
    public class DrawTarget : CommandNotify
    {
        public double x { get; set; }
        public double y { get; set; }
        public double h { get; set; }
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
        public string KeyBust { get; set; }
        public string Name { get; set; }
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
        public string modelType { get; set; }
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

    public class AnimationEncryptedItem
    {
        public List<Int64> dataEncrypted { get; set; }
        public int startT { get; set; }
        public int privateKey { get; set; }
        public string Md5Code { get; set; }
        public bool isParking { get; set; }
    }
    public class AnimationData
    {
        public int deltaT { get; set; }
        public AnimationEncryptedItem[] animateData { get; set; }
        public string currentMd5 { get; set; }
        public string previousMd5 { get; set; }
        public int[] privateKeys { get; set; }
    }
    public class AnimationKeyData
    {
        public int deltaT { get; set; }
        public int privateKeyIndex { get; set; }
        public int privateKeyValue { get; set; }
        public string currentMd5 { get; set; }
        public string previousMd5 { get; set; }

    }
    //public class BradCastAnimateOfOthersCar2 : CommandNotify
    //{
    //    public string carID;

    //    public AnimationData Animate { get; set; }
    //    public string parentID { get; set; }


    //}
    public class BradCastAnimateOfOthersCar3 : CommandNotify
    {
        public string carID;

        public AnimationData Animate { get; set; }
        public string parentID { get; set; }
        //public bool passPrivateKeysOnly { get; set; }


    }
    public class BradCastAnimateOfOthersCar4 : CommandNotify
    {
        public string carID;

        public AnimationKeyData Animate { get; set; }
        public string parentID { get; set; }
        //public bool passPrivateKeysOnly { get; set; }


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
    public class BradCastPromoteDiamondInCar : CommandNotify
    {
        public string pType { get; set; }
        public string roleID { get; set; }
    }
    public class SelectionIsWrong : CommandNotify
    {
        public long reduceValue { get; set; }
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
        /// <summary>
        /// 作用是保证前台能按顺序接受状态！
        /// </summary>
        public int countStamp { get; set; }
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

    public class AnimateC
    {

    }
    public class BradCastAnimateOfSelfCar : CommandNotify
    {
        public string carID;

        public AnimationData Animate { get; set; }
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
        public double positionZ { get; set; }
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
        /// <summary>
        /// 
        /// </summary>
        public string RefererAddr { get; set; }
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
        public double rotationY { get; set; }
    }
    public class TakeApart : Command
    {
        public string Key { get; set; }
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

        public int count { get; set; }
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
    public class OrderToUpdateLevel : Command
    {
        public string Key { get; set; }
        public string address { get; set; }
        public string signature { get; set; }
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

    public class TeamExit : Command
    {
        public int TeamNum { get; set; }
    }

    public class TeamJoin : Command
    {
        public string FromUrl { get; set; }
        public string CommandStart { get; set; }
        public int WebSocketID { get; set; }
        public string PlayerName { get; set; }
        public string TeamIndex { get; set; }
        public string Guid { get; set; }
    }

    public class LeaveTeam : Command
    {
        public string FromUrl { get; set; }
        public int WebSocketID { get; set; }
        public string TeamIndex { get; set; }
    }
    public class TeamDisplayItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string GUID { get; set; }
        public bool IsSelf { get; set; }
    }
    public class TeamCreateFinish : CommandNotify
    {
        public string CommandStart { get; set; }
        public int TeamNum { get; set; }
        public TeamDisplayItem PlayerDetail { get; set; }
    }
    public class TeamJoinBroadInfo : CommandNotify
    {
        public TeamDisplayItem Player { get; set; }
        //public string Guid { get; set; }
    }
    public class TeamJoinRemoveInfo : CommandNotify
    {
        public string Guid { get; set; }
    }

    public class TeamJoinFinish : CommandNotify
    {
        public int TeamNum { get; set; }
        public List<TeamDisplayItem> Players { get; set; }
    }
    public class TeamNumWithSecret : CommandNotify
    {
        public string Secret { get; set; }
        public string RefererAddr { get; set; }
    }
    public class TeamResult : Command
    {
        public string FromUrl { get; set; }
        public int WebSocketID { get; set; }
        /// <summary>
        /// 作为队伍的索引
        /// </summary>
        public int TeamNumber { get; set; }
        public int Hash { get; set; }
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
    public class GetRewardFromBuildingM : Command
    {
        public string Key { get; set; }
        public string selectObjName { get; set; }
    }
    /// <summary>
    /// 用于传输对话。
    /// </summary>
    public class DialogMsg : CommandNotify
    {
        /// <summary>
        /// ResponMsg 采用Key的URl与WebsocketID To=表示谁的消息，实际上相当于From
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// Reques时，采用To的URl与WebsocketID To=表示谁的消息。To=Self，表示自己的消息。
        /// </summary>
        public string To { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Msg { get; set; }
    }
    public class PassRoomMd5Check
    {
        public int RoomIndex { get; set; }
        public string StartMd5 { get; set; }
        public string CheckMd5 { get; set; }

        public string RoomIndexWithAes { get; set; }
    }

    public class CheckCarState : Command
    {
        public string State { get; set; }
        public string Key { get; set; }
    }

    public class SetCrossBG : CommandNotify
    {
        public string CrossID { get; set; }
        public bool IsDetalt { get; set; }
        public string Md5Key { get; set; }
        public bool AddNew { get; set; }
        //public string px { get; set; }
        //public string nx { get; set; }
        //public string py { get; set; }
        //public string ny { get; set; }
        //public string pz { get; set; }
        //public string nz { get; set; }
    }
    public class GetResistanceObj : Command
    {
        public string KeyLookfor { get; set; }
        public string key { get; set; }
        /// <summary>
        /// 0代表基础信息，1代表战斗抗性
        /// </summary>
        public int RequestType { get; set; }
    };
    public class ResistanceDisplay : CommandNotify
    {
        /// <summary>
        /// 自己，队友，玩家，NPC，敌对NPC
        /// </summary>
        public string Relation { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public long Money { get; set; }
        /// <summary>
        /// NPC Or player
        /// </summary>
        public string PlayerType { get; set; }

        public int Driver { get; set; }
        public string DriverName { get; set; }
        public int[] MileCount { get; set; }
        public int[] BusinessCount { get; set; }
        public int[] VolumeCount { get; set; }
        public int[] SpeedCount { get; set; }

        public string BossKey { get; set; }
        public string BossName { get; set; }
        public string BTCAddr { get; set; }
        public long Mile { get; set; }
        public long Business { get; set; }
        public long Volume { get; set; }
        public int Speed { get; set; }

        public string OnLineStr { get; set; }
        public string KeyLookfor { get; set; }

        //public long SpeedValue { get; set; }
        //public long DefenceValue { get; set; }
        //public long AttackValue { get; set; }
        //public long LoseValue { get; set; }
        //public long ConfuseValue { get; set; }

        // public long SpeedValue { get; set; }
        // public long SpeedValue { get; set; }
        //  public int 
    }

    public class ParameterToEditPlayerMaterial
    {
        /// <summary>
        /// 自己，队友，玩家，NPC
        /// </summary>
        public string Relation { get; set; }
        public string singleName { get; set; }
        public int Driver { get; set; }
        public string Key { get; set; }
    }

    public class ResistanceDisplay2 : CommandNotify
    {
        //  public driversource.Resistance Resistance { get; set; }
        public string KeyLookfor { get; set; }

        /// <summary>
        /// 招募成功次数
        /// </summary> 
        public int recruit { get; set; }

        public int magicViolentValue { get; set; }
        public int magicViolentProbability { get; set; }

        public int controlImprove { get; set; }
        //public driversource.Resistance Ignore { get; set; }

        public int SpeedImproveProbability { get; set; }
        public int DefenseImproveProbability { get; set; }
        public int AttackImprove { get; set; }

        public int defensiveOfElectic { get; set; }
        public int defensiveOfWater { get; set; }
        public int defensiveOfFire { get; set; }
        public int defensiveOfLose { get; set; }
        public int defensiveOfConfuse { get; set; }
        public int defensiveOfAmbush { get; set; }
        public int defensiveOfPhysics { get; set; }

        //public int ignoreElectic { get; set; }
        //public int ignoreOfWater { get; set; }
        //public int ignoreFire { get; set; }
        public int ignoreLose { get; set; }
        public int ignoreConfuse { get; set; }
        public int ignoreAmbush { get; set; }
        public int ignorePhysics { get; set; }
        public Dictionary<int, int> buildingReward { get; set; }
        public int race { get; set; }

        public long SpeedValue { get; set; }
        public long DefenceValue { get; set; }
        public long AttackValue { get; set; }
        public long LoseValue { get; set; }
        public long ConfuseValue { get; set; }
        public int ignorePhysicsValue { get; set; }

        public int IgnoreFireMagicValue { get; set; }
        public int IgnoreElectricMagicValue { get; set; }
        public int IgnoreWaterMagicValue { get; set; }

        public int IgnoreAmbushValue { get; set; }
        public int IgnoreLostValue { get; set; }
        public int IgnoreConfuseValue { get; set; }


        public int SpeedImproveValue { get; set; }
        public int DefenseImproveValue { get; set; }
        public int AttackImproveProbability { get; set; }
        public int AttackImproveValue { get; set; }
        public int AmbushPropertyByDefendMagic { get; set; }
        public int ConfusePropertyByDefendMagic { get; set; }
        public int LostPropertyByDefendMagic { get; set; }
        public int DefenceAttackMagicAdd { get; set; }
        public int DefencePhysicsAdd { get; set; }
    }

    public class GetFightSituation : Command
    {
        public string Key { get; set; }
        public class GetFightSituationResult : Command
        {
            public string[] Parters { get; set; }
            public string[] Opponents { get; set; }
        }
    }

    public class GetTaskCopyDetail : Command
    {
        public string Key { get; set; }
        public class GetTaskCopyResult : Command
        {
            public string[] Detail { get; set; }
        }
    }
    public class RemoveTaskCopyM : Command
    {
        public string Key { get; set; }
        public string Code { get; set; }
    }

    public class SetParameterIsLogin : CommandNotify
    {

    }
    public class SetParameterHasNewTask : CommandNotify
    {

    }
    public class ExitObj : Command
    {
        public string Key { get; set; }
        public class ExitObjResult
        {
            public bool Success { get; set; }
            public string Msg { get; set; }
        }
    }

    public class GetOnLineState : Command
    {
        public string Key { get; set; }
        public class SetOnLineState : CommandNotify
        {
            public string Key { get; set; }
            public bool IsNPC { get; set; }
            public bool OnLine { get; set; }
            public bool IsPartner { get; set; }
            public bool IsEnemy { get; set; }
        }
    }
}
