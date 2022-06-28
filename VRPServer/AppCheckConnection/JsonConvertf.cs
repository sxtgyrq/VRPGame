using System;
using System.Collections.Generic;
using System.Text;

namespace AppCheckConnection
{
    internal class JsonConvertf
    {
        internal static T DeserializeObject<T>(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json); 
        }
    }
}
