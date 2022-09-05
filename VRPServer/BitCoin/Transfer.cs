//using NBitcoin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BitCoin
{
    public class Transfer
    {
        public class YrqTransObj
        {
            string[] _secret;
            Dictionary<string, NBitcoin.BitcoinSecret> secretsWithAddr;
            Dictionary<int, NBitcoin.BitcoinAddress> addressesFrom;
            public int Length { get { return this._secret.Length; } }
            //  QBitNinjaClient _client;

            NBitcoin.Network netWork { get { return NBitcoin.Network.Main; } }

            List<string> toAddressList;
            List<long> amountList;
            public YrqTransObj(string[] secret_)
            {
                this._secret = secret_;
                this.secretsWithAddr = new Dictionary<string, NBitcoin.BitcoinSecret>();
                // Dictionary<>
                this.addressesFrom = new Dictionary<int, NBitcoin.BitcoinAddress>();

                for (int i = 0; i < this.Length; i++)
                {
                    //  Network bitcoinNetwork = Network.TestNet;
                    var bitcoinPrivateKey = new NBitcoin.BitcoinSecret(_secret[i], netWork);
                    var address = bitcoinPrivateKey.GetAddress(NBitcoin.ScriptPubKeyType.SegwitP2SH);

                    addressesFrom.Add(i, address);
                    secretsWithAddr.Add(address.ToString(), bitcoinPrivateKey);
                    Console.WriteLine($"{i}-{address.ToString()}");
                }
                this.toAddressList = new List<string>();
                this.amountList = new List<long>();
            }

            public void BradCast()
            {
                //throw new NotImplementedException();
            }

            Dictionary<string, List<BitCoin.Transtraction.TradeInfo.ReturnResult>> MoneyCanCost;
            //  = new Dictionary<string, Dictionary<uint256, ICoin>>();
            public async Task<string> CalMoneyCanCost()
            {
                int current_block_count;
                {
                    var url = $"https://blockchain.info/q/getblockcount?timsSaaadd={DateTime.Now.ToString("yyyyMMddHHmmssff")}";
                    using (WebClient web1 = new WebClient())
                    {
                        current_block_count = Convert.ToInt32(Encoding.UTF8.GetString(await web1.DownloadDataTaskAsync(url)));
                    }
                    //Consol.WriteLine($"current_block_count:{current_block_count}");
                }

                this.MoneyCanCost = new Dictionary<string, List<BitCoin.Transtraction.TradeInfo.ReturnResult>>();
                for (int i = 0; i < Length; i++)
                {
                    var address = addressesFrom[i];
                    var r = await getFromWeb(address.ToString(), current_block_count);
                    this.MoneyCanCost.Add(address.ToString(), r);
                }
                return $"MoneyCanCost{MoneyCanCost.Count}";
            }

            long minerFee = 0;
            public void SetMinerFee(long fee)
            {
                this.minerFee = fee;
            }

            static async Task<List<BitCoin.Transtraction.TradeInfo.ReturnResult>> getFromWeb(string adress, int current_block_count)
            {


                List<BitCoin.Transtraction.TradeInfo.ReturnResult> rReturn = new List<Transtraction.TradeInfo.ReturnResult>();
                Dictionary<string, bool> record = new Dictionary<string, bool>();
                int limit = 45; //max 50
                int offset = 0;
                bool isEmpty = true;
                {
                    while (true)
                    {
                        string url = $"https://blockchain.info/address/{adress}?format=json&yrq={DateTime.Now.GetHashCode()}&limit={limit}&offset={offset}";

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
                                    var r = BitCoin.Transtraction.TradeInfo.GetDataFromJson(current_block_count, data, adress, out isEmpty);
                                    for (var j = 0; j < r.Count; j++)
                                    {
                                        if (record.ContainsKey(r[j].hashHex))
                                        { }
                                        else
                                        {
                                            record.Add(r[j].hashHex, true);
                                            rReturn.Add(r[j]);
                                        }
                                    }
                                }
                                //Consol.WriteLine(data);
                                break;
                            }
                            catch (Exception e)
                            {
                                if (i < 9) { }
                                else
                                {
                                    throw e;
                                }
                                data = "";
                            }
                        }
                        offset += 40;
                        if (isEmpty)
                        {
                            break;
                        }
                    }

                }
                return rReturn;
            }

            long sum = 0;
            public void SumMoney()
            {
                this.sum = 0;
                foreach (var item in MoneyCanCost)
                {
                    foreach (var coin in item.Value)
                    {
                        this.sum += coin.value;
                    }
                }
                Console.WriteLine($"可已花{sum}Satoshi/{sum / 100000000}.{(sum % 100000000).ToString("D8")}BTC");
                //Console.WriteLine($"")
            }

            public void AddAddrPayTo(string addr, long satoshi)
            {
                this.toAddressList.Add(addr);
                this.amountList.Add(satoshi);
            }

            string _changeAddr = null;
            string changeAddr
            {
                get
                {
                    if (string.IsNullOrEmpty(_changeAddr))
                    {
                        return this.addressesFrom[0].ToString();
                    }
                    else
                    {
                        return this._changeAddr;
                    }
                }
            }
            public void SetChangeAddr(string addr)
            {
                this._changeAddr = addr;
            }
            //
            public class ICoinOrderObj
            {
                public string Address { get; set; }
                public NBitcoin.ICoin Money { get; set; }
            }
            string[] toAddress { get { return this.toAddressList.ToArray(); } }
            long[] amount { get { return this.amountList.ToArray(); } }
            public NBitcoin.Transaction TransactionF()
            {
                List<NBitcoin.Coin> coinsSpend = new List<NBitcoin.Coin>();
                //  NBitcoin.ICoin

                var transaction = NBitcoin.Transaction.Create(NBitcoin.Network.Main);
                foreach (var item in MoneyCanCost)
                {
                    foreach (var coin in item.Value)
                    {
                        var tIn = new NBitcoin.TxIn()
                        {
                            PrevOut = new NBitcoin.OutPoint(coin.hash, coin.n),
                            ScriptSig = coin.ScriptSig
                        };
                        transaction.Inputs.Add(tIn);
                        // usedSotashi += coin.value;
                        NBitcoin.Coin c = new NBitcoin.Coin(coin.hash, Convert.ToUInt32(coin.n), new NBitcoin.Money(coin.value), coin.ScriptSig);
                        //   var c = new NBitcoin.Coin(coin.hash, new NBitcoin.TxOut(), new NBitcoin.Money(coin.value), coin.ScriptSig);
                        coinsSpend.Add(c);
                    }
                }
                long sotashiNeedToSend = 0;
                for (int i = 0; i < toAddress.Length; i++)
                {
                    var toAddr = NBitcoin.BitcoinAddress.Create(toAddress[i], NBitcoin.Network.Main);
                    transaction.Outputs.Add(new NBitcoin.Money(amount[i]), toAddr);
                    sotashiNeedToSend += amount[i];
                }
                var changeAmount = sum - minerFee - sotashiNeedToSend;
                if (changeAmount > 0)
                {
                    var toAddr = NBitcoin.BitcoinAddress.Create(changeAddr, NBitcoin.Network.Main);
                    transaction.Outputs.Add(new NBitcoin.Money(changeAmount), toAddr);
                }
                if (changeAmount < 0)
                {
                    Console.WriteLine($"资金不对");
                    Console.ReadLine();
                }
                var needSecret = this.secretsWithAddr.Values.ToArray();
                //var Coins=transaction.Inputs.
                // transaction.s
                // var fr = transaction.GetFeeRate(moneyNeedToSign.ToArray(), );
                var hex = transaction.ToHex();
                Console.WriteLine(hex);
                transaction.Sign(needSecret, coinsSpend);
                //transaction.
                hex = transaction.ToHex();
                Console.WriteLine($"转账手续费为{transaction.GetFeeRate(coinsSpend.ToArray()).SatoshiPerByte.ToString()}sat/B");
                Console.WriteLine(hex);
                return transaction;
            }


        }
    }
}
