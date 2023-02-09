using System;
using System.Collections.Generic;
using System.Text;

namespace CommonClass
{
    public class MapEditor
    {
        public class Position : Command
        {
            public string roadCode { get; set; }
            public int roadOrder { get; set; }
            public string anotherRoadCode { get; set; }
            public int anotherRoadOrder { get; set; }
            public double longitude { get; set; }
            public double latitude { get; set; }
            public double height { get; set; }
        }
        public class DrawRoad : Command
        {
            public string roadCode { get; set; }
        }
        public class NextCross : Command
        {
            public string roadCode { get; set; }
            public int roadOrder { get; set; }
            public string anotherRoadCode { get; set; }
            public int anotherRoadOrder { get; set; }
        }
        public class PreviousCross : NextCross
        {
        }
        public class GetAbtractModels : Command
        {
            public string amID { get; set; }
        }

        public class GetCatege { }

        public class SaveObjInfo : Command
        {
            public string modelID { get; set; }
            public string amID { get; set; }
            public double x { get; set; }
            public double y { get; set; }
            public double z { get; set; }
            public double rotatey { get; set; }
        }
        public class UpdateObjInfo : Command
        {
            public string modelID { get; set; }
            public double x { get; set; }
            public double y { get; set; }
            public double z { get; set; }
            public double rotatey { get; set; }
        }
        public class DelObjInfo : Command
        {
            public string modelID { get; set; }
        }

        public class UseModelObj : Command
        {
            public string modelID { get; set; }
            public bool Used { get; set; }
        }

        public class GetModelDetail : Command
        {
            public string modelID { get; set; }
        }
        public class ShowOBJFile : Command
        {
            public double x { get; set; }
            public double z { get; set; }
        }
        public class ObjResult : Command
        {
            public List<databaseModel.detailmodel> detail { get; set; }
        }

        public class CreateNew : Command
        {
            public string modelType { get; set; }
            public string imageBase64 { get; set; }
            public string objText { get; set; }
            public string mtlText { get; set; }
            public string animation { get; set; }
            public string author { get; set; }
            public int amState { get; set; }
            public string modelName { get; set; }

            public string modelID { get; set; }
            public double x { get; set; }
            public double y { get; set; }
            public double z { get; set; }
            public double rotatey { get; set; }
        }

        public class ModelDetail
        {
            public string c { get; set; }
            public double x { get; set; }
            public double y { get; set; }
            public double z { get; set; }
            public bool locked { get; set; }
            public int dmState { get; set; }
            public string bussinessAddress { get; set; }
            public string Content { get; set; }
            public string author { get; set; }
            public int amState { get; set; }
            public string modelName { get; set; }
            public string createTime { get; set; }
            public string amID { get; set; }
        }

        public class GetUnLockedModel : Command
        {
            public int startIndex;
            public string direction { get; set; }
        }
        public class GetUnLockedModelResult
        {
            public double x { get; set; }
            public double y { get; set; }
            public double z { get; set; }
            public bool hasValue { get; set; }
            public int newStartIndex { get; set; }
        }

        public abstract class SetBackgroundScene : Command
        {
            public string author { get; set; }
            public string px { get; set; }
            public string nx { get; set; }
            public string py { get; set; }
            public string ny { get; set; }
            public string pz { get; set; }
            public string nz { get; set; }
        }
        public class SetBackgroundScene_BLL : SetBackgroundScene
        {
            public string firstRoadcode { get; set; }
            public int firstRoadorder { get; set; }
            public string secondRoadcode { get; set; }
            public int secondRoadorder { get; set; }
        }
        public class SetBackgroundScene_DAL : SetBackgroundScene
        {
            public string crossID { get; set; }
        }
        public class GetBackgroundScene : Command
        {
            public string firstRoadcode { get; set; }
            public int firstRoadorder { get; set; }
            public string secondRoadcode { get; set; }
            public int secondRoadorder { get; set; }
            public class Result
            {
                public bool hasValue { get; set; }
                public int crossState { get; set; }
                public string px { get; set; }
                public string nx { get; set; }
                public string py { get; set; }
                public string ny { get; set; }
                public string pz { get; set; }
                public string nz { get; set; }
            }
        }
        public class GetHeightAtPosition : Command
        {
            public double MercatorX { get; set; }
            public double MercatorY { get; set; }

            public class GetHeightAtPositionResult : Command
            {
                public double height { get; set; }
            }
        }
        public class UseBackgroundScene : Command
        {
            public string firstRoadcode { get; set; }
            public int firstRoadorder { get; set; }
            public string secondRoadcode { get; set; }
            public int secondRoadorder { get; set; }
            public bool used { get; set; }
            //public class Result : Command
            //{
            //    public bool hasValue { get; set; }
            //    public int crossState { get; set; }
            //    public string px { get; set; }
            //    public string nx { get; set; }
            //    public string py { get; set; }
            //    public string ny { get; set; }
            //    public string pz { get; set; }
            //    public string nz { get; set; }
            //}
        }
    }

    public class Finance
    {
        public class Charging : Command
        {
            public string ChargingType { get; set; }
            public string ChargingWord { get; set; }
            public decimal ChargingNum { get; set; }
            public string ChargingDt { get; set; }
            public string ChargingAddr { get; set; }
            public class Result
            {
                public bool success { get; set; }
                public string msg { get; set; }
                public int chargingOrder { get; set; }
            }

        }
        public class ChargingLookFor : Command
        {
            public int chargingOrder { get; set; }
            public class Result
            {
                public string bindWordAddr { get; set; }
                public int chargingOrder { get; set; }

                public string chargingword { get; set; }

                public string chargingDatetime { get; set; }

                public string chargingMoney { get; set; }

                public string chargingType { get; set; }

                public string charginginfo { get; set; }

                public string chargingAddr { get; set; }

                public string chargingIsUsed { get; set; }

            }

        }
        public class LookForTaskCopy : CommonClass.Command
        {
            public string code { get; set; }
            public string addr { get; set; }

            public class LookForTaskCopyResult : LookForTaskCopy
            {
                public string json { get; set; }
            }
        }
        public class TaskCopyPassOrNG : CommonClass.Command
        {
            public string code { get; set; }
            public string addr { get; set; }
            public bool pass { get; set; }

            //public class TaskCopyPassOrNGResult : LookForTaskCopy
            //{
            //    public string json { get; set; }
            //}
        }
    }
}
