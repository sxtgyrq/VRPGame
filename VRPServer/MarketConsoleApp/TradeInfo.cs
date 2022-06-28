using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MarketConsoleApp
{
    static class SerializeLibrary
    {
        public static bool ContainProperty(this object instance, string propertyName)
        {
            if (instance != null && !string.IsNullOrEmpty(propertyName))
            {
                PropertyInfo _findedPropertyInfo = instance.GetType().GetProperty(propertyName);
                return (_findedPropertyInfo != null);
            }
            return false;
        }
    }
    internal class TradeInfo
    {
        private string adress;

        public TradeInfo(string adress)
        {
            this.adress = adress;
        }

        public async Task<string> GetTradeInfomation(string address)
        {
            string url = $"https://blockchain.info/address/{address}?format=json&yrq={DateTime.Now.GetHashCode()}";
            string data;
            using (WebClient web1 = new WebClient())
            {
                data = Encoding.UTF8.GetString(await web1.DownloadDataTaskAsync(url));
            }
            //Consol.WriteLine(data);
            return data;
        }

        internal async Task GetTradeInfomationFromChain()
        {

            //https://blockchain.info/q/getblockcount
            int current_block_count;
            {
                var url = "https://blockchain.info/q/getblockcount";
                using (WebClient web1 = new WebClient())
                {
                    current_block_count = Convert.ToInt32(Encoding.UTF8.GetString(await web1.DownloadDataTaskAsync(url)));
                }
                //Consol.WriteLine($"current_block_count:{current_block_count}");
            }
            StringBuilder detailOfTrade = new StringBuilder();
            Dictionary<string, bool> record = new Dictionary<string, bool>();
            Dictionary<string, long> valuesInput = new Dictionary<string, long>();
            int limit = 45; //max 50
            int offset = 0;
            while (true)
            {
                // try
                {
                    string url = $"https://blockchain.info/address/{this.adress}?format=json&yrq={DateTime.Now.GetHashCode()}&limit={limit}&offset={offset}";

                    string data = "";
                    for (int i = 0; i < 10; i++)
                    {
                        try
                        {
                            using (WebClient web1 = new WebClient())
                            {
                                // ((HttpWebRequest)web1).ReadWriteTimeout
                                var dataGet = await web1.DownloadDataTaskAsync(url);
                                data = Encoding.UTF8.GetString(await web1.DownloadDataTaskAsync(url));
                            }
                            //Consol.WriteLine(data);
                            break;
                        }
                        catch (Exception e)
                        {
                            if (i < 9) 
                            { }
                            //Consol.WriteLine($"重新尝试第{i + 2}次");
                            else
                            {
                                throw e;
                            }
                            data = "";
                        }
                    }
                    Transaction t = Newtonsoft.Json.JsonConvert.DeserializeObject<Transaction>(data);
                    if (t.txs.Count == 0)
                    {
                        break;
                    }
                    for (var i = 0; i < t.txs.Count; i++)
                    {

                        var item = t.txs[i];
                        if (record.ContainsKey(item.hash))
                        {
                            continue;
                        }
                        else
                        {
                            record.Add(item.hash, true);
                            if (item.block_height.HasValue)
                            {
                                int transaction_block_height = item.block_height.Value;
                                if (current_block_count - transaction_block_height + 1 > 3)
                                {
                                    long inputSum = 0;
                                    for (int j = 0; j < item.inputs.Count; j++)
                                    {
                                        inputSum += item.inputs[j].prev_out.value;
                                    }
                                    if (inputSum <= 0)
                                    {
                                        continue;
                                    }
                                    Dictionary<string, long> outs = new Dictionary<string, long>();
                                    for (int j = 0; j < item.inputs.Count; j++)
                                    {
                                        var addr = item.inputs[j].prev_out.addr;
                                        if (!string.IsNullOrEmpty(addr))
                                        {
                                            // var itemOut = item.inputs[j].prev_out.value * item.result / inputSum;
                                            if (outs.ContainsKey(addr))
                                                outs[addr] += item.inputs[j].prev_out.value;
                                            else
                                                outs.Add(addr, item.inputs[j].prev_out.value);
                                        }
                                        else
                                        {
                                            outs.Add(this.adress, item.inputs[j].prev_out.value);
                                        }
                                    }
                                    for (int j = 0; j < item.@out.Count; j++)
                                    {
                                        if (string.IsNullOrEmpty(item.@out[j].addr))
                                        {
                                            continue;
                                        }
                                        if (item.@out[j].addr.Trim() == adress.Trim())
                                        {
                                            if (item.@out[j].value > 0)
                                            {
                                                foreach (var outItem in outs)
                                                {
                                                    var key = outItem.Key;
                                                    var value = outItem.Value;
                                                    var GiveValue = value * item.@out[j].value / inputSum;
                                                    if (valuesInput.ContainsKey(key))
                                                    {
                                                        valuesInput[key] += GiveValue;
                                                    }
                                                    else
                                                    {
                                                        valuesInput.Add(key, GiveValue);
                                                    }
                                                    //  var detailRecord = $"{item.block_height},{item.hash},{key},{GiveValue},{}";
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }


                    }
                    offset += 40;
                    //Consol.WriteLine($"offset:{offset}");
                    if (offset == 225)
                    {
                        Console.ReadLine();
                    }
                }
                //catch (Exception ex)
                //{
                //    //Consol.WriteLine(ex.Message);
                //    throw ex;
                //}

            }
            StringBuilder sb = new StringBuilder();
            long sumV = 0;
            foreach (var item in valuesInput)
            {
                //Consol.WriteLine($"{item.Key}:{item.Value}");
                sumV += item.Value;
                sb.Append($"{item.Key},{item.Value}{Environment.NewLine}");
            }

            //Consol.WriteLine($"sum:{sumV / 100000000}.{sumV % 100000000}");
            File.WriteAllText($"result_{this.adress}.csv", sb.ToString());
            //{

            //    //https://blockchain.info/address/357a3So9CbsNfBBgFYACGvxxS6tMaDoa1P?format=json&yrq=112&limit=50&offset=16195
            //    string url = $"https://blockchain.info/address/{this.adress}?format=json&yrq={DateTime.Now.GetHashCode()}";
            //    string data;
            //    using (WebClient web1 = new WebClient())
            //    {
            //        data = Encoding.UTF8.GetString(await web1.DownloadDataTaskAsync(url));
            //    }
            //    //var data = File.ReadAllText(@"E:\Project\VRPGame\VRPServer\MarketConsoleApp\bin\Release\netcoreapp3.1\publish\jj.txt");
            //    //Consol.WriteLine(data);
            //    //  return data;
            //    //    var obj = Newtonsoft.Json.JsonConvert.DeserializeObject(data);
            //    //if (obj.ContainProperty("error"))
            //    //{
            //    //    return;
            //    //}
            //    //else if (obj.ContainProperty("total_received"))
            //    try
            //    {
            //        Dictionary<string, long> valuesInput = new Dictionary<string, long>();
            //        Transaction t = Newtonsoft.Json.JsonConvert.DeserializeObject<Transaction>(data);
            //        var sum = t.total_received;

            //    }
            //    catch (Exception ex)
            //    {
            //        //Consol.WriteLine($" ex:{ex.ToString()}");
            //    }
            //}
        }



    }
    public class Transaction
    {
        public string hash160 { get; set; }
        public string address { get; set; }
        public long n_tx { get; set; }
        public long n_unredeemed { get; set; }
        public long total_received { get; set; }
        public long total_sent { get; set; }
        public long final_balance { get; set; }

        public List<txItem> txs { get; set; }

    }
    public class txItem
    {
        public string hash { get; set; }
        public long ver { get; set; }
        public long vin_sz { get; set; }

        public long vout_sz { get; set; }
        public long size { get; set; }

        public long weight { get; set; }
        public long fee { get; set; }
        public string relayed_by { get; set; }
        public long lock_time { get; set; }
        public long tx_index { get; set; }
        public bool double_spend { get; set; }
        public long time { get; set; }
        public long? block_index { get; set; }
        public int? block_height { get; set; }

        public List<inputItem> inputs { get; set; }

        public List<outItem> @out { get; set; }


        public long result { get; set; }
        public long balance { get; set; }
    }
    public class spending_outpointsObj
    {
        public long tx_index { get; set; }
        public long n { get; set; }
    }
    public class inputItem
    {
        public long sequence { get; set; }
        public string witness { get; set; }
        public string script { get; set; }
        public long index { get; set; }
        public prev_outObj prev_out { get; set; }
    }
    public class prev_outObj
    {
        public bool spent { get; set; }
        public string script { get; set; }



        public List<spending_outpointsObj> spending_outpoints { get; set; }

        public long tx_index { get; set; }
        public long value { get; set; }
        public string addr { get; set; }
        public long n { get; set; }
        public long type { get; set; }
    }

    public class outItem
    {
        public long type { get; set; }
        public bool spent { get; set; }
        public long value { get; set; }

        public List<spending_outpointsObj> spending_outpoints { get; set; }
        public long n { get; set; }
        public long tx_index { get; set; }
        public string script { get; set; }
        public string addr { get; set; }
    }
}

