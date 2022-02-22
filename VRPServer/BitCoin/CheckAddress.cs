using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BitCoin
{
    public class CheckAddress
    {
        public static bool CheckAddressIsUseful(string address)
        {
            if (address == null)
            {
                return false;
            }
            var regx = new Regex("^[13][a-km-zA-HJ-NP-Z1-9]{25,34}$");
            if (regx.Match(address).Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
