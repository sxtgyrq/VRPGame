using System;
using System.Collections.Generic;
using System.Text;

namespace CommonClass.databaseModel
{
    public class detailmodel
    {
        public string modelID { get; set; }
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
        public string amodel { get; set; }
        public float rotatey { get; set; }
        public bool locked { get; set; }
        public int dmState { get; set; }
    }
}
