using NBitcoin;
using QBitNinja.Client;
using QBitNinja.Client.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace BitCoinTrasfer
{
    public class Class1
    {
        public static async Task<string> Test()
        {
            //  var c=new NBitcoin.
            string r;
            var client = new QBitNinjaClient("http://api.qbit.ninja/", Network.Main);
            // var address1 = new BitcoinPubKeyAddress("1LVw2vRu53VESgFLq8mJYGZRgvk4xzW5W1", Network.Main);

            // var address1 = new BitcoinPubKeyAddress("356irRFazab63B3m95oyiYeR5SDKJRFa99", Network.Main);
            var address1 = BitcoinAddress.Create("1LVw2vRu53VESgFLq8mJYGZRgvk4xzW5W1", Network.Main);
            var balance = client.GetBalance(address1).Result;
            for (int i = 0; i < balance.Operations.Count; i++)
            {
                var tid = balance.Operations[i].TransactionId;
                var cc = balance.Operations[i].BlockId;

                var rr = balance.Operations[i].ReceivedCoins;


                var xx = balance.Operations[i].SpentCoins;
                for (int j = 0; j < rr.Count; j++)
                {
                    //rr[i].
                    //  rr[i].TxOut.Value;
                    //xx[i].
                }
            }

            //Coin[] aliceCoins = balance.Operations
            //            .Outputs
            //            .Select((o, i) => new Coin(new OutPoint(aliceFunding.GetHash(), i), o))
            //            .ToArray();
            r = balance.Operations.Count.ToString();
            return r;
            // balance.Operations.

        }
        public static bool SendBTC(string secret, string toAddress, decimal amount, string fundingTransactionHash)
        {
            Network bitcoinNetwork = Network.TestNet;
            // List<string>
            var bitcoinPrivateKey = new BitcoinSecret(secret, bitcoinNetwork);
            var address = bitcoinPrivateKey.GetAddress(ScriptPubKeyType.Legacy);

            var client = new QBitNinjaClient(bitcoinNetwork);
            var transactionId = uint256.Parse(fundingTransactionHash);
            var transactionResponse = client.GetTransaction(transactionId).Result;

            var receivedCoins = transactionResponse.ReceivedCoins;

            OutPoint outPointToSpend = null;
            foreach (var coin in receivedCoins)
            {
                if (coin.TxOut.ScriptPubKey == bitcoinPrivateKey.GetAddress(ScriptPubKeyType.SegwitP2SH).ScriptPubKey)
                {
                    outPointToSpend = coin.Outpoint;
                    // coin.Outpoint.
                }
            }

            var transaction = Transaction.Create(bitcoinNetwork);
            transaction.Inputs.Add(new TxIn()
            {
                PrevOut = outPointToSpend
            });

            var receiverAddress = BitcoinAddress.Create(toAddress, bitcoinNetwork);


            var txOutAmount = new Money(amount, MoneyUnit.BTC);

            // Tx fee
            var minerFee = new Money(0.0005m, MoneyUnit.BTC);

            // Change
            var txInAmount = (Money)receivedCoins[(int)outPointToSpend.N].Amount;
            var changeAmount = txInAmount - txOutAmount - minerFee;

            transaction.Outputs.Add(txOutAmount, receiverAddress.ScriptPubKey);
            transaction.Outputs.Add(changeAmount, bitcoinPrivateKey.GetAddress(ScriptPubKeyType.Legacy).ScriptPubKey);


            //    scriptIn = new Script(
            //"OP_HASH160 "
            //+ Op.GetPushOp(additionalParameter.Hash.ToBytes())
            //+ " OP_EQUALVERIFY"
            //+ " OP_DUP"
            //+ " OP_HASH160 "
            //+ Op.GetPushOp(pubkeyhash.ToBytes())
            //+ " OP_EQUALVERIFY"
            //+ " OP_CHECKSIG"
            //);

            transaction.Inputs[0].ScriptSig = address.ScriptPubKey;
            //  transaction.Inputs[1].ScriptSig = new Script() { };
            //transaction.Inputs[0].ScriptSig = address.;
            //  transaction.Inputs.Add(})
            //Sign Tx
            transaction.Sign(bitcoinPrivateKey, receivedCoins.ToArray());
            transaction.Sign(new[] { bitcoinPrivateKey, bitcoinPrivateKey }, receivedCoins);
            // transaction.Sign()
            //  transaction.Sign()
            // transaction.
            //Broadcast Tx

            BroadcastResponse broadcastResponse = client.Broadcast(transaction).Result;

            return broadcastResponse.Success;

            // TransactionBuilder builder = Network.CreateTransactionBuilder();
        }

        public static async Task<string> SendBTC(string[] secret, string[] toAddress, long[] amount, string fundingTransactionHash)
        {
            Dictionary<string, BitcoinSecret> secretsWithAddr = new Dictionary<string, BitcoinSecret>();
            // Dictionary<>
            Dictionary<int, BitcoinAddress> addressesFrom = new Dictionary<int, BitcoinAddress>();
            int Length = secret.Length;
            for (var i = 0; i < secret.Length; i++)
            {
                Network bitcoinNetwork = Network.TestNet;
                var bitcoinPrivateKey = new BitcoinSecret(secret[i], bitcoinNetwork);
                var address = bitcoinPrivateKey.GetAddress(ScriptPubKeyType.SegwitP2SH);
                addressesFrom.Add(i, address);
                secretsWithAddr.Add(address.ToString(), bitcoinPrivateKey);
            }
            BitcoinAddress changeAddr = addressesFrom[0];
            Dictionary<string, Dictionary<uint256, ICoin>> MoneyCanCost
                = new Dictionary<string, Dictionary<uint256, ICoin>>();
            var client = new QBitNinjaClient("http://api.qbit.ninja/", Network.Main);
            for (int i = 0; i < Length; i++)
            {
                var address = addressesFrom[i];

                var balance = await client.GetBalance(address);
                Dictionary<uint256, ICoin> Money = new Dictionary<uint256, ICoin>();
                for (var j = 0; j < balance.Operations.Count; j++)
                {
                    var tid = balance.Operations[j].TransactionId;
                    //List uint256 HasID=

                    for (var k = 0; k < balance.Operations[j].ReceivedCoins.Count; k++)
                    {
                        if (balance.Operations[j].ReceivedCoins[k].TxOut.ScriptPubKey == address.ScriptPubKey)
                        {
                            Money.Add(tid, balance.Operations[j].ReceivedCoins[k]);
                            // balance.Operations[j].TransactionId
                            //  var 
                        }
                    }
                    //for (int k = 0; k < balance.Operations[j].SpentCoins.Count; k++)
                    //{
                    //    //    balance.Operations[j].SpentCoins[k].Outpoint.
                    //    if (balance.Operations[j].SpentCoins[k].TxOut.ScriptPubKey == address.ScriptPubKey)
                    //    {
                    //        Money.Remove(balance.Operations[j].SpentCoins[k].Outpoint.Hash);
                    //    }
                    //}
                    // balance.Operations[j].
                }
                for (var j = 0; j < balance.Operations.Count; j++)
                {
                    for (int k = 0; k < balance.Operations[j].SpentCoins.Count; k++)
                    {
                        if (balance.Operations[j].SpentCoins[k].TxOut.ScriptPubKey == address.ScriptPubKey)
                        {
                            Money.Remove(balance.Operations[j].SpentCoins[k].Outpoint.Hash);
                        }
                    }
                }
                MoneyCanCost.Add(address.ToString(), Money);
            }
            var t = TransactionF(MoneyCanCost, secretsWithAddr, toAddress, amount, changeAddr);
            BroadcastResponse broadcastResponse = client.Broadcast(t).Result;

            return broadcastResponse.Success.ToString();
            return "";
        }

        public class ICoinOrderObj
        {
            public string Address { get; set; }
            public ICoin Money { get; set; }
        }
        private static Transaction TransactionF(Dictionary<string, Dictionary<uint256, ICoin>> moneyCanCost, Dictionary<string, BitcoinSecret> secretsWithAddr, string[] toAddress, long[] amount, BitcoinAddress changeAddr)
        {
            List<ICoinOrderObj> allOutPut = new List<ICoinOrderObj>();
            // List<string> fromAddr = new List<string>();
            foreach (var addrMoney in moneyCanCost)
            {
                foreach (var dd in addrMoney.Value)
                {
                    allOutPut.Add(new ICoinOrderObj
                    {
                        Address = addrMoney.Key,
                        Money = dd.Value
                    });
                    //  fromAddr.Add(addrMoney.Key);
                }
            }
            var xx = (from item in allOutPut orderby item.Money.Amount descending select item).ToList();
            var sum = amount.Sum();//单位是聪
            var neededMoney = new List<ICoinOrderObj>();
            long minerFee = 10000;//聪
            long usedSotashi = 0;
            List<ICoin> moneyNeedToSign = new List<ICoin>();
            for (var i = 0; i < xx.Count; i++)
            {
                neededMoney.Add(xx[i]);
                usedSotashi += xx[i].Money.TxOut.Value.Satoshi;
                moneyNeedToSign.Add(xx[i].Money);
                if (usedSotashi >= sum + minerFee)
                {
                    break;
                }
            }
            Dictionary<string, BitcoinSecret> needSecret = new Dictionary<string, BitcoinSecret>();

            var transaction = Transaction.Create(Network.Main);
            for (int i = 0; i < neededMoney.Count; i++)
            {
                transaction.Inputs.Add(new TxIn()
                {
                    PrevOut = neededMoney[i].Money.Outpoint,
                    ScriptSig = neededMoney[i].Money.TxOut.ScriptPubKey
                });
                var xx11 = new TxIn()
                {
                    PrevOut = new OutPoint() { Hash = 10, N = 10 },
                    // ScriptSig=
                };
                if (needSecret.ContainsKey(neededMoney[i].Address)) { }
                else
                {
                    needSecret.Add(neededMoney[i].Address, secretsWithAddr[neededMoney[i].Address]);
                }
            }
            for (int i = 0; i < toAddress.Length; i++)
            {
                var toAddr = BitcoinAddress.Create(toAddress[i], Network.Main);
                transaction.Outputs.Add(new Money(amount[i]), toAddr);
            }
            var changeAmount = sum + minerFee - usedSotashi;
            if (changeAmount > 0)
            {
                transaction.Outputs.Add(new Money(changeAmount), changeAddr);
            }
            transaction.Sign(needSecret.Values.ToArray(), moneyNeedToSign.ToArray());
            var fr = transaction.GetFeeRate(moneyNeedToSign.ToArray());
            //fr.SatoshiPerByte;
            return transaction;
            //transaction.Outputs.Add(txOutAmount, receiverAddress.ScriptPubKey);
            //transaction.Outputs.Add(changeAmount, bitcoinPrivateKey.GetAddress(ScriptPubKeyType.Legacy).ScriptPubKey);
            //for(int i=0;i<)
            // transaction.Sign(new[] { bitcoinPrivateKey, bitcoinPrivateKey }, receivedCoins);
            //var needM = new List<ICoin>();
            //long sumMoney = 0;
            //for (var i = 0; i < xx.Count; i++)
            //{

            //}
            // throw new NotImplementedException();
        }
    }
    public class YrqTransObj
    {
        string[] _secret;
        Dictionary<string, BitcoinSecret> secretsWithAddr;
        Dictionary<int, BitcoinAddress> addressesFrom;
        public int Length { get { return this._secret.Length; } }
        QBitNinjaClient _client;

        Network netWork { get { return Network.Main; } }

        List<string> toAddressList;
        List<long> amountList;
        public YrqTransObj(string[] secret_)
        {
            this._secret = secret_;
            this.secretsWithAddr = new Dictionary<string, BitcoinSecret>();
            // Dictionary<>
            this.addressesFrom = new Dictionary<int, BitcoinAddress>();

            for (int i = 0; i < this.Length; i++)
            {
                //  Network bitcoinNetwork = Network.TestNet;
                var bitcoinPrivateKey = new BitcoinSecret(_secret[i], netWork);
                var address = bitcoinPrivateKey.GetAddress(ScriptPubKeyType.SegwitP2SH);

                addressesFrom.Add(i, address);
                secretsWithAddr.Add(address.ToString(), bitcoinPrivateKey);
                Console.WriteLine($"{i}-{address.ToString()}");
            }
            this._client = new QBitNinjaClient("http://api.qbit.ninja/", this.netWork);
            this.toAddressList = new List<string>();
            this.amountList = new List<long>();
        }
        Dictionary<string, Dictionary<uint256, ICoin>> MoneyCanCost;
        //  = new Dictionary<string, Dictionary<uint256, ICoin>>();
        public async Task<string> CalMoneyCanCost()
        {
            this.MoneyCanCost = new Dictionary<string, Dictionary<uint256, ICoin>>();
            for (int i = 0; i < Length; i++)
            {
                var address = addressesFrom[i];
                //var BitcoinPubKeyAddress = new BitcoinPubKeyAddress()
                var pk = BitcoinPubKeyAddress.Create(address.ToString(), this.netWork);
                var balance = await this._client.GetBalance(pk, true);
                Dictionary<uint256, ICoin> Money = new Dictionary<uint256, ICoin>();
                for (var j = 0; j < balance.Operations.Count; j++)
                {
                    var tid = balance.Operations[j].TransactionId;
                    //List uint256 HasID=

                    for (var k = 0; k < balance.Operations[j].ReceivedCoins.Count; k++)
                    {
                        if (balance.Operations[j].ReceivedCoins[k].TxOut.ScriptPubKey == address.ScriptPubKey)
                        {
                            Money.Add(tid, balance.Operations[j].ReceivedCoins[k]);
                        }
                    }
                }
                for (var j = 0; j < balance.Operations.Count; j++)
                {
                    for (int k = 0; k < balance.Operations[j].SpentCoins.Count; k++)
                    {
                        if (balance.Operations[j].SpentCoins[k].TxOut.ScriptPubKey == address.ScriptPubKey)
                        {
                            Money.Remove(balance.Operations[j].SpentCoins[k].Outpoint.Hash);
                        }
                    }
                }
                if (Money.Count > 0)
                    MoneyCanCost.Add(address.ToString(), Money);
            }
            return $"MoneyCanCost{MoneyCanCost.Count}";
        }

        public void SumMoney()
        {
            long sum = 0;
            foreach (var item in MoneyCanCost)
            {
                foreach (var coin in item.Value)
                {
                    sum += coin.Value.TxOut.Value.Satoshi;
                }
            }
            Console.WriteLine($"可已花{sum}");
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
            public ICoin Money { get; set; }
        }
        string[] toAddress { get { return this.toAddressList.ToArray(); } }
        long[] amount { get { return this.amountList.ToArray(); } }
        private Transaction TransactionF()
        {
            List<ICoinOrderObj> allOutPut = new List<ICoinOrderObj>();
            // List<string> fromAddr = new List<string>();
            foreach (var addrMoney in this.MoneyCanCost)
            {
                foreach (var dd in addrMoney.Value)
                {
                    allOutPut.Add(new ICoinOrderObj
                    {
                        Address = addrMoney.Key,
                        Money = dd.Value
                    });
                    //  fromAddr.Add(addrMoney.Key);
                }
            }
            var xx = (from item in allOutPut orderby item.Money.Amount descending select item).ToList();
            var sum = amount.Sum();//单位是聪
            var neededMoney = new List<ICoinOrderObj>();
            long minerFee = 10000;//聪
            long usedSotashi = 0;
            List<ICoin> moneyNeedToSign = new List<ICoin>();
            for (var i = 0; i < xx.Count; i++)
            {
                neededMoney.Add(xx[i]);
                usedSotashi += xx[i].Money.TxOut.Value.Satoshi;
                moneyNeedToSign.Add(xx[i].Money);
                //if (usedSotashi >= sum + minerFee)
                //{
                //    break;
                //}
            }
            Dictionary<string, BitcoinSecret> needSecret = new Dictionary<string, BitcoinSecret>();

            var transaction = Transaction.Create(Network.Main);
            for (int i = 0; i < neededMoney.Count; i++)
            {
                transaction.Inputs.Add(new TxIn()
                {
                    PrevOut = neededMoney[i].Money.Outpoint,
                    ScriptSig = neededMoney[i].Money.TxOut.ScriptPubKey
                });
                if (needSecret.ContainsKey(neededMoney[i].Address)) { }
                else
                {
                    needSecret.Add(neededMoney[i].Address, secretsWithAddr[neededMoney[i].Address]);
                }
            }
            for (int i = 0; i < toAddress.Length; i++)
            {
                var toAddr = BitcoinAddress.Create(toAddress[i], Network.Main);
                transaction.Outputs.Add(new Money(amount[i]), toAddr);
            }
            var changeAmount = sum + minerFee - usedSotashi;
            if (changeAmount > 0)
            {
                var toAddr = BitcoinAddress.Create(changeAddr, Network.Main);
                transaction.Outputs.Add(new Money(changeAmount), toAddr);
            }
            transaction.Sign(needSecret.Values.ToArray(), moneyNeedToSign.ToArray());
            var fr = transaction.GetFeeRate(moneyNeedToSign.ToArray());
            return transaction;
        }
    }
}
