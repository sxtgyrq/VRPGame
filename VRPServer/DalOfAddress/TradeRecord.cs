using CommonClass;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace DalOfAddress
{
    public class TradeRecord
    {

        interface TradeTypeWithF
        {
            long Cost { get; }
            string MsgSuccess { get; }
            string MsgMoenyIsNotEnough { get; }

            string MsgMoneyIsLocked(string addr)
            {
                return $"{addr}资金锁定中，其正作为奖励使用中。";
            }

        }
        public const long TradeStockCost = 500000;
        public static string MsgSuccess
        {
            get
            { return $"花费{TradeStockCost / 100}.00积分，完成股权交易。"; }
        }
        public static string MsgMoenyIsNotEnough
        {
            get
            { return $"转让股份需要花费{TradeStockCost / 100}.00积分，积分储蓄不足。且身上最少得有{TradeStockCost / 100}.01积分。"; }
        }
        public static string MsgMoneyIsLocked(string addr)
        {
            return $"{addr}资金锁定中，其正作为奖励使用中。";
        }

        public class StockTrade : TradeTypeWithF
        {
            public long Cost { get { return TradeStockCost; } }

            public string MsgSuccess => MsgSuccess;

            public string MsgMoenyIsNotEnough => MsgMoenyIsNotEnough;
        }
        public static bool Add(int tradeIndex, string addrFrom, string addrBussiness, string sign, string msg, long passCoin, out string notifyMsg)
        {
            /*
             * 当你读到这里的时候，一定，会有疑问，不要判addrFrom的余额吗？
             * 再前天调用时余额已经判断。
             * 二者这里插入，只能按顺序0,1,2,3,4,5自然排序，从而避免了数据库双花！
             * addrBussiness addrFrom tradeIndex三个组成的主键，避免了数据库层面的双花！
             */

            var mysql = "INSERT INTO traderecord(msg,sign,bussinessAddr,tradeIndex,addrFrom,TimeStamping) VALUES(@msg,@sign,@bussinessAddr,@tradeIndex,@addrFrom,@TimeStamping);";

            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        if (TradeReward.CheckOccupied(tran, con, addrBussiness, addrFrom) >= tradeIndex)
                        {
                            notifyMsg = MsgMoneyIsLocked(addrFrom);
                            tran.Rollback();
                            return false;
                        }
                        else
                        {
                            int tradeIndexInDB;
                            string sQL = "SELECT count(*) FROM traderecord WHERE bussinessAddr=@bussinessAddr AND addrFrom=@addrFrom";
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@bussinessAddr", addrBussiness);
                                command.Parameters.AddWithValue("@addrFrom", addrFrom);
                                tradeIndexInDB = Convert.ToInt32(command.ExecuteScalar());
                            }
                            if (tradeIndexInDB != tradeIndex)
                            {
                                notifyMsg = "逻辑错误！！！";
                                tran.Rollback();
                                return false;
                            }
                        }
                        {
                            // const long costMoney = 1000000;
                            if (DalOfAddress.MoneyAdd.GetMoney(con, tran, addrFrom) > TradeStockCost)
                            {
                                long subsidizeGet, subsidizeLeft;
                                DalOfAddress.MoneyGet.GetSubsidizeAndLeft(con, tran, addrFrom, TradeStockCost, out subsidizeGet, out subsidizeLeft);
                                if (subsidizeLeft > 0)
                                {
                                    string sQL = mysql;
                                    // long moneycount;
                                    using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                                    {
                                        DateTime operateT = DateTime.Now;
                                        command.Parameters.AddWithValue("@msg", msg);
                                        command.Parameters.AddWithValue("@sign", sign);
                                        command.Parameters.AddWithValue("@bussinessAddr", addrBussiness);
                                        command.Parameters.AddWithValue("@tradeIndex", tradeIndex);
                                        command.Parameters.AddWithValue("@addrFrom", addrFrom);
                                        command.Parameters.AddWithValue("@TimeStamping", operateT);
                                        command.ExecuteNonQuery();
                                    }
                                    notifyMsg = MsgSuccess;
                                    tran.Commit();
                                    return true;
                                }
                                else
                                {
                                    tran.Rollback();
                                    notifyMsg = "系统逻辑错误！！！";
                                    return false;
                                }
                            }
                            else
                            {
                                tran.Rollback();
                                notifyMsg = MsgMoenyIsNotEnough;
                                return false;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                        throw new Exception("新增错误");
                    }
                }
            }
            //  notifyMsg = "";
        }

        public static bool AddBySystem(int tradeIndex, string addrFrom, string addrBussiness, string sign, string msg, long passCoin, out string notifyMsg)
        {
            /*
             * 当你读到这里的时候，一定，会有疑问，不要判addrFrom的余额吗？
             * 再前天调用时余额已经判断。
             * 二者这里插入，只能按顺序0,1,2,3,4,5自然排序，从而避免了数据库双花！
             * addrBussiness addrFrom tradeIndex三个组成的主键，避免了数据库层面的双花！
             */

            var mysql = "INSERT INTO traderecord(msg,sign,bussinessAddr,tradeIndex,addrFrom,TimeStamping) VALUES(@msg,@sign,@bussinessAddr,@tradeIndex,@addrFrom,@TimeStamping);";

            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        if (TradeReward.CheckOccupied(tran, con, addrBussiness, addrFrom) >= tradeIndex)
                        {
                            notifyMsg = MsgMoneyIsLocked(addrFrom);
                            tran.Rollback();
                            return false;
                        }
                        else
                        {
                            int tradeIndexInDB;
                            string sQL = "SELECT count(*) FROM traderecord WHERE bussinessAddr=@bussinessAddr AND addrFrom=@addrFrom";
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@bussinessAddr", addrBussiness);
                                command.Parameters.AddWithValue("@addrFrom", addrFrom);
                                tradeIndexInDB = Convert.ToInt32(command.ExecuteScalar());
                            }
                            if (tradeIndexInDB != tradeIndex)
                            {
                                notifyMsg = "逻辑错误！！！";
                                tran.Rollback();
                                return false;
                            }
                        }
                        {
                            string sQL = mysql;
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                DateTime operateT = DateTime.Now;
                                command.Parameters.AddWithValue("@msg", msg);
                                command.Parameters.AddWithValue("@sign", sign);
                                command.Parameters.AddWithValue("@bussinessAddr", addrBussiness);
                                command.Parameters.AddWithValue("@tradeIndex", tradeIndex);
                                command.Parameters.AddWithValue("@addrFrom", addrFrom);
                                command.Parameters.AddWithValue("@TimeStamping", operateT);
                                command.ExecuteNonQuery();
                            }
                            notifyMsg = MsgSuccess;
                            tran.Commit();
                            return true;
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                        throw new Exception("新增错误");
                    }
                }
            }
            //  notifyMsg = "";
        }

        /// <summary>
        /// 此方法用于测试。
        /// </summary>
        /// <param name="addrBussiness"></param>
        /// <exception cref="Exception"></exception>
        public static void RemoveByBussinessAddr(string addrBussiness)
        {
            /*
            * 当你读到这里的时候，一定，会有疑问，不要判addrFrom的余额吗？
            * 再前天调用时余额已经判断。
            * 二者这里插入，只能按顺序0,1,2,3,4,5自然排序，从而避免了数据库双花！
            * addrBussiness addrFrom tradeIndex三个组成的主键，避免了数据库层面的双花！
            */

            var sQL = "DELETE FROM traderecord WHERE bussinessAddr=@bussinessAddr";

            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {

                        using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                        {
                            command.Parameters.AddWithValue("@bussinessAddr", addrBussiness);
                            command.ExecuteNonQuery();
                        }

                    }
                    catch (Exception e)
                    {
                        throw e;
                        throw new Exception("新增错误");
                    }
                    tran.Commit();
                }
            }
        }

        public static bool AddWithBTCExtracted(int tradeIndex, string addrFrom, string addrBussiness, string sign, string msg, long passCoin, out string notifyMsg)
        {
            /*
             * 当你读到这里的时候，一定，会有疑问，不要判addrFrom的余额吗？
             * 再前天调用时余额已经判断。
             * 二者这里插入，只能按顺序0,1,2,3,4,5自然排序，从而避免了数据库双花！
             * addrBussiness addrFrom tradeIndex三个组成的主键，避免了数据库层面的双花！
             */

            var mysql = "INSERT INTO traderecord(msg,sign,bussinessAddr,tradeIndex,addrFrom,TimeStamping) VALUES(@msg,@sign,@bussinessAddr,@tradeIndex,@addrFrom,@TimeStamping);";

            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        if (TradeReward.CheckOccupied(tran, con, addrBussiness, addrFrom) >= tradeIndex)
                        {
                            notifyMsg = $"{addrBussiness}资金锁定中，其正作为奖励使用中。";
                            tran.Rollback();
                            return false;
                        }
                        else
                        {
                            int tradeIndexInDB;
                            string sQL = "SELECT count(*) FROM traderecord WHERE bussinessAddr=@bussinessAddr AND addrFrom=@addrFrom";
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@bussinessAddr", addrBussiness);
                                command.Parameters.AddWithValue("@addrFrom", addrFrom);
                                tradeIndexInDB = Convert.ToInt32(command.ExecuteScalar());
                            }
                            if (tradeIndexInDB != tradeIndex)
                            {
                                notifyMsg = "逻辑错误！！！";
                                tran.Rollback();
                                return false;
                            }
                        }
                        {
                            const long costMoney = 3000000;
                            var moneyNow = DalOfAddress.MoneyAdd.GetMoney(con, tran, addrFrom);
                            if (moneyNow > costMoney)
                            {
                                long subsidizeGet, subsidizeLeft;
                                DalOfAddress.MoneyGet.GetSubsidizeAndLeft(con, tran, addrFrom, costMoney, out subsidizeGet, out subsidizeLeft);
                                if (subsidizeLeft > 0)
                                {
                                    string sQL = mysql;
                                    int row;
                                    // long moneycount;
                                    using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                                    {
                                        var operateT = DateTime.Now;
                                        command.Parameters.AddWithValue("@msg", msg);
                                        command.Parameters.AddWithValue("@sign", sign);
                                        command.Parameters.AddWithValue("@bussinessAddr", addrBussiness);
                                        command.Parameters.AddWithValue("@tradeIndex", tradeIndex);
                                        command.Parameters.AddWithValue("@addrFrom", addrFrom);
                                        command.Parameters.AddWithValue("@TimeStamping", operateT);
                                        row = command.ExecuteNonQuery();
                                    }
                                    if (row != 1)
                                    {
                                        tran.Rollback();
                                        notifyMsg = "系统逻辑错误！！！";
                                        return false;
                                    }
                                    CommonClass.databaseModel.moneyofcustomerextractedM model = new CommonClass.databaseModel.moneyofcustomerextractedM()
                                    {
                                        addrFrom = addrFrom,
                                        bussinessAddr = addrBussiness,
                                        isPayed = 0,
                                        recordTime = DateTime.Now,
                                        satoshi = passCoin,
                                        tradeIndex = tradeIndex
                                    };
                                    row = MoneyOfCustomerExtracted.Add(con, tran, model);
                                    if (row == 1)
                                    {
                                        tran.Commit();
                                        notifyMsg = $"花费{costMoney / 100}.00积分，完成BTC提取。{passCoin}聪比特币，将在72小时内，汇入{addrFrom}。";
                                        return true;
                                    }
                                    else
                                    {
                                        tran.Rollback();
                                        notifyMsg = "系统逻辑错误！！！";
                                        return false;
                                    }
                                }
                                else
                                {
                                    tran.Rollback();
                                    notifyMsg = "系统逻辑错误！！！";
                                    return false;
                                }
                            }
                            else
                            {
                                tran.Rollback();
                                notifyMsg = $"BTC提取需要花费{costMoney / 100}.00积分，积分储蓄不足。现有积分{moneyNow / 100}.{(moneyNow % 100) / 10}{(moneyNow % 10)}。";
                                return false;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                        throw new Exception("新增错误");
                    }
                }
            }
        }


        static bool addFunction(int tradeIndex, string addrFrom, string addrBussiness, string sign, string msg, long passCoin, TradeTypeWithF passObj, out string notifyMsg)
        {
            var mysql = "INSERT INTO traderecord(msg,sign,bussinessAddr,tradeIndex,addrFrom,TimeStamping) VALUES(@msg,@sign,@bussinessAddr,@tradeIndex,@addrFrom,@TimeStamping);";
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        if (TradeReward.CheckOccupied(tran, con, addrBussiness, addrFrom) >= tradeIndex)
                        {
                            notifyMsg = passObj.MsgMoneyIsLocked(addrFrom);
                            tran.Rollback();
                            return false;
                        }
                        else
                        {
                            int tradeIndexInDB;
                            string sQL = "SELECT count(*) FROM traderecord WHERE bussinessAddr=@bussinessAddr AND addrFrom=@addrFrom";
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@bussinessAddr", addrBussiness);
                                command.Parameters.AddWithValue("@addrFrom", addrFrom);
                                tradeIndexInDB = Convert.ToInt32(command.ExecuteScalar());
                            }
                            if (tradeIndexInDB != tradeIndex)
                            {
                                notifyMsg = "逻辑错误！！！";
                                tran.Rollback();
                                return false;
                            }
                        }
                        {
                            // const long costMoney = 1000000;
                            if (DalOfAddress.MoneyAdd.GetMoney(con, tran, addrFrom) > passObj.Cost)
                            {
                                long subsidizeGet, subsidizeLeft;
                                DalOfAddress.MoneyGet.GetSubsidizeAndLeft(con, tran, addrFrom, passObj.Cost, out subsidizeGet, out subsidizeLeft);
                                if (subsidizeLeft > 0)
                                {
                                    string sQL = mysql;
                                    // long moneycount;
                                    using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                                    {
                                        DateTime operateT = DateTime.Now;
                                        command.Parameters.AddWithValue("@msg", msg);
                                        command.Parameters.AddWithValue("@sign", sign);
                                        command.Parameters.AddWithValue("@bussinessAddr", addrBussiness);
                                        command.Parameters.AddWithValue("@tradeIndex", tradeIndex);
                                        command.Parameters.AddWithValue("@addrFrom", addrFrom);
                                        command.Parameters.AddWithValue("@TimeStamping", operateT);
                                        command.ExecuteNonQuery();
                                    }
                                    notifyMsg = passObj.MsgSuccess;
                                    tran.Commit();
                                    return true;
                                }
                                else
                                {
                                    tran.Rollback();
                                    notifyMsg = "系统逻辑错误！！！";
                                    return false;
                                }
                            }
                            else
                            {
                                tran.Rollback();
                                notifyMsg = passObj.MsgMoenyIsNotEnough;
                                return false;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                        throw new Exception("新增错误");
                    }
                }
            }
        }

        public static List<string> GetAll(string bussinessAddr)
        {
            List<string> result = new List<string>();
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        {
                            string sQL = "SELECT msg,sign FROM traderecord WHERE bussinessAddr=@bussinessAddr ORDER BY TimeStamping ASC;";
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@bussinessAddr", bussinessAddr);
                                using (var reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        result.Add(Convert.ToString(reader["msg"]).Trim());
                                        result.Add(Convert.ToString(reader["sign"]).Trim());
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                        throw new Exception("新增错误");
                    }
                }
            }
            return result;
        }

        public static int GetCount(string bussinessAddr, string addrFrom)
        {
            int result;
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        if (administratorwallet.Exist(con, tran, bussinessAddr))
                        {
                            string sQL = "SELECT COUNT(*) FROM traderecord WHERE bussinessAddr=@bussinessAddr AND addrFrom=@addrFrom";
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@bussinessAddr", bussinessAddr);
                                command.Parameters.AddWithValue("@addrFrom", addrFrom);
                                result = Convert.ToInt32(command.ExecuteScalar());
                            }
                        }
                        else
                            result = -1;
                    }
                    catch (Exception e)
                    {
                        throw e;
                        throw new Exception("新增错误");
                    }
                }
            }
            return result;
        }

        //public static void Update(int dataInt, int tradeIndex, string addrReward, string addrBussiness, string signOfAddrReward, string signOfaddrBussiness, string msg, long passCoin)
        //{
        //    throw new NotImplementedException();
        //}

        public enum AddResult
        {
            HasNoData,
            Success,
            HasGiven,
            DataError
        }
        public static void Add(ModelTranstraction.AwardsGivingPass agp, out AddResult r)
        {
            var tr = DalOfAddress.TradeReward.GetByStartDate(int.Parse(agp.time));
            if (tr == null)
            {
                r = AddResult.HasNoData;
                return;
            }
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {

                    if (agp.msgs.Count == 0)
                    {
                        if (traderewardapply.Count(con, tran, Convert.ToInt32(agp.time)) == 0)
                        {
                            int updateRow;
                            string sQL = $"UPDATE tradereward SET waitingForAddition=0 WHERE startDate={agp.time} AND waitingForAddition=1;";
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                updateRow = command.ExecuteNonQuery();
                            }
                            if (updateRow == 1)
                            {
                                tran.Commit();
                                r = AddResult.Success;
                                return;
                            }
                            else
                            {
                                r = AddResult.HasGiven;
                                tran.Rollback();
                                return;
                            }
                        }
                        else
                        {
                            tran.Rollback();
                            r = AddResult.DataError;
                            return;
                        }
                    }
                    else
                    {
                        int count;
                        {
                            string sQL = $"SELECT COUNT(*) FROM traderewardapply WHERE startDate={agp.time};";
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                count = Convert.ToInt32(command.ExecuteScalar());
                            }
                        }
                        if (count == agp.msgs.Count)
                        {
                            //   CommonClass.Agreement
                            //  var splitChars = new char[] { '@', '-' };
                            int sumPassCoin = 0;
                            for (int i = 0; i < agp.msgs.Count; i++)
                            {
                                // var msgItem = agp.msgs[i].Split();
                                int index, passValue;
                                string tradeAddr, businessAddr, acceptAddr;
                                var formatIsWrite = CommonClass.Agreement.IsUseful(agp.msgs[i], out index, out tradeAddr, out businessAddr, out acceptAddr, out passValue);
                                sumPassCoin += passValue;
                                if (formatIsWrite)
                                {
                                    int tradeIndexInDB;
                                    {
                                        string sQL = "SELECT count(*) FROM traderecord WHERE bussinessAddr=@bussinessAddr AND addrFrom=@addrFrom";
                                        using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                                        {
                                            command.Parameters.AddWithValue("@bussinessAddr", tr.bussinessAddr);
                                            command.Parameters.AddWithValue("@addrFrom", tr.tradeAddress);
                                            tradeIndexInDB = Convert.ToInt32(command.ExecuteScalar());
                                        }
                                    }
                                    if (tradeIndexInDB != index)
                                    {
                                        r = AddResult.DataError;
                                        tran.Rollback();
                                        return;

                                    }
                                    else if (tradeAddr != tr.tradeAddress)
                                    {
                                        r = AddResult.DataError;
                                        tran.Rollback();
                                        return;
                                    }
                                    else if (businessAddr != tr.bussinessAddr)
                                    {
                                        r = AddResult.DataError;
                                        tran.Rollback();
                                        return;
                                    }
                                    else if (traderewardapply.UpdateItem(con, tran, Convert.ToInt32(agp.time), agp.ranks[i], acceptAddr, passValue) != 1)
                                    {
                                        r = AddResult.DataError;
                                        tran.Rollback();
                                        return;
                                    }
                                    else
                                    {
                                        {
                                            var sQL = "INSERT INTO traderecord(msg,sign,bussinessAddr,tradeIndex,addrFrom,TimeStamping) VALUES(@msg,@sign,@bussinessAddr,@tradeIndex,@addrFrom,@TimeStamping);";
                                            // string sQL = mysql;
                                            // long moneycount;
                                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                                            {
                                                DateTime TimeStamping = DateTime.Now;
                                                command.Parameters.AddWithValue("@msg", agp.msgs[i]);
                                                command.Parameters.AddWithValue("@sign", agp.list[i]);
                                                command.Parameters.AddWithValue("@bussinessAddr", tr.bussinessAddr);
                                                command.Parameters.AddWithValue("@tradeIndex", tradeIndexInDB);
                                                command.Parameters.AddWithValue("@addrFrom", tr.tradeAddress);
                                                command.Parameters.AddWithValue("@TimeStamping", TimeStamping);
                                                count = command.ExecuteNonQuery();
                                            }
                                            if (count != 1)
                                            {
                                                r = AddResult.DataError;
                                                tran.Rollback();
                                                return;
                                            }
                                        }
                                        {

                                        }
                                    }
                                }
                                else
                                {
                                    r = AddResult.DataError;
                                    tran.Rollback();
                                    return;
                                }

                            }
                            if (sumPassCoin == tr.passCoin)
                            {
                                int updateRow;
                                string sQL = $"UPDATE tradereward SET waitingForAddition=0 WHERE startDate={agp.time} AND waitingForAddition=1 AND passCoin={sumPassCoin};";
                                using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                                {
                                    updateRow = command.ExecuteNonQuery();
                                }
                                if (updateRow == 1)
                                {
                                    if (traderewardapply.HasAddrGetNoReward(tran, con, Convert.ToInt32(agp.time)))
                                    {
                                        r = AddResult.DataError;
                                        tran.Rollback();
                                        return;
                                    }
                                    else
                                    {
                                        r = AddResult.Success;
                                        tran.Commit();
                                        return;
                                    }
                                }
                                else
                                {
                                    r = AddResult.DataError;
                                    tran.Rollback();
                                    return;
                                }
                            }
                            else
                            {
                                tran.Rollback();
                                r = AddResult.DataError;
                                return;
                            }
                        }
                        else
                        {
                            r = AddResult.DataError;
                            tran.Rollback();
                            return;
                        }
                    }
                }
            }
        }
    }
}
