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
            public string modelName { get; set; }
        }

        public class GetCatege { }

        public class SaveObjInfo : Command
        {
            public string modelID { get; set; }
            public string amodel { get; set; }
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
        public class ShowOBJFile : Command
        {
            public double x { get; set; }
            public double z { get; set; }
        }
        public class ObjResult : Command
        {
            public List<databaseModel.detailmodel> detail { get; set; }
        }
    }
}
