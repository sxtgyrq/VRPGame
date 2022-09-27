using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class SaveRoad
    {

        public interface rInfo
        {
            int CarInOpposeDirection { get; }
            double endLongitude { get; }
            double startLongitude { get; }
            double endLatitude { get; }
            double startLatitude { get; }
            int RoadOrder { get; }
            int MaxSpeed { get; }
            int startHeight { get; }
            int endHeight { get; }
        }
        /// <summary>
        /// 此类必须依据协议走
        /// </summary>
        public class RoadInfo : rInfo
        {

            public string VersionNumber { get; set; }
            public string RoadCode { get; set; }
            public int RoadOrder { get; set; }
            public double startLongitude { get; set; }
            public double startLatitude { get; set; }
            public double endLongitude { get; set; }
            public double endLatitude { get; set; }

            public int startHeight { get; set; }
            public int endHeight { get; set; }

            public int CarInDirection { get; set; }
            /// <summary>
            /// 0代表单行，1代表双向。
            /// </summary>
            public int CarInOpposeDirection { get; set; }
            public int EBicycleInDirection { get; set; }
            public int EBicycleInOpposeDirection { get; set; }
            public int WalkInDirection { get; set; }
            public int WalkInOpposeDirection { get; set; }
            public int MaxSpeed { get; set; }

            /// <summary>
            /// cross中的 RoadCode1指向本身的的RoadCode
            /// </summary>
            public DictCross[] Cross1 { get; set; }
            /// <summary>
            /// cross中的 RoadCode2指向本身的的RoadCode
            /// </summary>
            public DictCross[] Cross2 { get; set; }

            public FastenPositionCalculate[] PassShop { get; set; }
        }

        public class DictCross
        {
            public string RoadCode1 { get; set; }
            public string RoadCode2 { get; set; }
            public double BDLongitude { get; set; }
            public double BDLatitude { get; set; }
            public int IsChanged { get; set; }
            /// <summary>
            /// 有3个取值，-1,0,1，分别代表未设置、不想交、相交。-1，不会出现在这里。
            /// </summary>
            public int CrossState { get; set; }
            public int RoadOrder1 { get; set; }
            public int RoadOrder2 { get; set; }
            public double Percent1 { get; set; }
            public double Percent2 { get; set; }
          //  public double BDHeight { get; set; }
        }

        public class FastenPositionCalculate
        {
            public string FastenPositionName { get; set; }
            public string FastenPositionInfo { get; set; }
            public double Longitude { get; set; }
            public double Latitde { get; set; }
            public string RoadCode { get; set; }
            public double RoadPercent { get; set; }
            public int IsChanged { get; set; }
            public int FastenType { get; set; }
            public int RoadOrder { get; set; }
            public string UserName { get; set; }
            public string FastenPositionID { get; set; }
            public double positionLongitudeOnRoad { get; set; }
            public double positionLatitudeOnRoad { get; set; }
        }
    }
}
