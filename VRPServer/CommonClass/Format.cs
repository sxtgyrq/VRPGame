using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CommonClass
{
    public class Format
    {
        public static bool IsBase64(string input)
        {
            var reg = "^(?:[A-Za-z0-9+/]{4})*(?:[A-Za-z0-9+/]{2}==|[A-Za-z0-9+/]{3}=)?$";
            Regex r = new Regex(reg);
            return r.Match(input).Success;
        }

        public static bool IsModelID(string input)
        {
            var reg = "^n[A-Za-z0-9]{4,100}$";
            Regex r = new Regex(reg);
            return r.Match(input).Success;
        }
    }
}
