using System;
using System.Collections.Generic;
using System.Text;

namespace CommonClass
{
    public class PathCal
    {
        public enum ModelType { Start, End, Cross }
        public class CalModel
        {
            public string RoadCode { get; set; }
            public int RoadOrder { get; set; }
            public double Percent { get; set; }
            public string AnotherRoadCode { get; set; }
            public int AnotherRoadOrder { get; set; }
            public int AnotherIndex { get; set; }
            /// <summary>
            /// 距离上一个标志物的时间，单位秒
            /// </summary>
            public int ToPrevious { get; set; }
            /// <summary>
            /// 距离下一个标志物的时间，单位秒
            /// </summary>
            public int ToNext { get; set; }

            /// <summary>
            /// 最短路径指向的前一数据的Index，-2表示是路径的起点，-1表示未进行计算，[0,List.Count())表示上一个节点。
            /// </summary>
            public int Last { get; set; }

            public ModelType MType { get; set; }
            /// <summary>
            /// 用于标记是否已经进行计算。
            /// </summary>
            public int Pass { get; set; }
        }
        public class CalModel_V2 : CalModel
        {
            public string FastenPositionID { get; set; }
            public int EntranceORExit { get; set; }
        }
    }
}
