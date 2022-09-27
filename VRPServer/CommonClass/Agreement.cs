using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CommonClass
{
    public class Agreement
    {
        public static bool IsUseful(string msg, out int index, out string tradeAddr, out string businessAddr, out string acceptAddr, out int passValue)
        {
            //1@3FkXatYUQv81t7mDsQf8igumGnp1mE1gkk@3BkRN6seLYYgSqGQNX3o1poEzCXhGK3vTs->354vT5hncSwmob6461WjhhfWmaiZgHuaSK:100000Satoshi
            // var regex = new Regex("^[0-9]{1,6}@[13][a-km-zA-HJ-NP-Z1-9]{25,34}@[13][a-km-zA-HJ-NP-Z1-9]{25,34}->[13][a-km-zA-HJ-NP-Z1-9]{25,34}:[1-9][0-9]{1,8}Satoshi$");
            //(?<middle>\d+)
            var regex = new Regex("^(?<index>[0-9]{1,6})@(?<tradeAddr>[13][a-km-zA-HJ-NP-Z1-9]{25,34})@(?<businessAddr>[13][a-km-zA-HJ-NP-Z1-9]{25,34})->(?<acceptAddr>[13][a-km-zA-HJ-NP-Z1-9]{25,34}):(?<passValue>[1-9][0-9]{1,8})Satoshi$");
            Match match = regex.Match(msg);
            if (match.Success)
            {
                index = Convert.ToInt32(match.Groups["index"].Value);
                tradeAddr = Convert.ToString(match.Groups["tradeAddr"].Value);
                businessAddr = Convert.ToString(match.Groups["businessAddr"].Value);
                acceptAddr = Convert.ToString(match.Groups["acceptAddr"].Value);
                passValue = Convert.ToInt32(match.Groups["passValue"].Value);
                return true;
            }
            else
            {
                index = -1;
                tradeAddr = null;
                businessAddr = null;
                acceptAddr = null;
                passValue = 0;
                return false;
            }
        }
    }
}
