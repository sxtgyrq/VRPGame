using System;
using System.Collections.Generic;
using System.Text;

namespace AppCheckConnection.Model
{
    public class MapGo
    {
        public class passedRoad
        {
            public string roadCode { get; private set; }
            public int roadOrder { get; private set; }

            public passedRoad(string roadCode_, int roadOrder_)
            {
                this.roadCode = roadCode_;
                this.roadOrder = roadOrder_;
            }
            public override bool Equals(object obj)
            {
                if (obj.GetType() == typeof(passedRoad))
                {
                    return ((passedRoad)obj).roadCode.Trim() == this.roadCode.Trim() && ((passedRoad)obj).roadOrder == this.roadOrder;
                }
                else return false;
            }
            public override int GetHashCode()
            {
                int hash = 23;
                hash = (hash * 17) + this.roadCode.GetHashCode();
                hash = (hash * 17) + this.roadOrder.GetHashCode();
                return hash;
            }

            public passedRoad copy()
            {
                return new MapGo.passedRoad(this.roadCode, this.roadOrder);
            }
        }
    }
}
