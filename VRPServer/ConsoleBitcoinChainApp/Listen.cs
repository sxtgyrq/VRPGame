using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleBitcoinChainApp
{
    internal class Listen
    {
        internal static void IpAndPort(string hostIP, int tcpPort)
        {
            var dealWith = new TcpFunction.WithResponse.DealWith(DealWith);
            TcpFunction.WithResponse.ListenIpAndPort(hostIP, tcpPort, dealWith);
        }

        private static string DealWith(string addr, int port)
        {
            try
            {
                var regex = new Regex("^[A-HJ-NP-Za-km-z1-9]{1,50}$");
                if (regex.IsMatch(addr))
                {
                    // File.WriteAllText($"data/{addr}", json);
                    var path = $"data/{addr}";
                    if (File.Exists(path))
                    {
                        var json = File.ReadAllText(path, Encoding.UTF8);
                        return json;
                    }
                    else
                    {
                        return "{}";
                    }
                }
                else
                {
                    return "{}";
                }

            }
            catch
            {
                return "{}";
            }
        }
    }
}
