using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AppCheckConnection.Map
{
    internal class Road
    {
        internal static Model.SaveRoad.RoadInfo Get(string roadPresentCode, int roadOrder)
        {
            var path = $"{Config.rootPath}\\road\\{roadPresentCode}_{roadOrder}.txt";
            var json = File.ReadAllText(path);
            return JsonConvertf.DeserializeObject<Model.SaveRoad.RoadInfo>(json);
        }
    }
    internal class FastonPosition
    {
        internal static bool Get(int fpIndex, out Model.FastonPosition fp)
        {
            var path = $"{Config.rootPath}\\fpindex\\fp_{fpIndex}.txt";
            if (File.Exists(path))
            {
                var jsonStr = File.ReadAllText(path);
                fp = JsonConvertf.DeserializeObject<Model.FastonPosition>(jsonStr);
                return true;
            }
            else
            {
                fp = null;
                return false;
            }
        }
    }
}
