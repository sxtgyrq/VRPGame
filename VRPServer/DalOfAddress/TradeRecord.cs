using CommonClass;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace DalOfAddress
{
    public class TradeRecord
    {
        public static void Add(int tradeIndex, string addrFrom, string addrBussiness, string sign, string msg, double passCoin, out string notifyMsg)
        {
            /*
             * 当你读到这里的时候，一定，会有疑问，不要判addrFrom的余额吗？
             * 再前天调用时余额已经判断。
             * 二者这里插入，只能按顺序0,1,2,3,4,5自然排序，从而避免了数据库双花！
             * addrBussiness addrFrom tradeIndex三个组成的主键，避免了数据库层面的双花！
             */
            var mysql = "INSERT INTO traderecord(msg,sign,bussinessAddr,tradeIndex,addrFrom) VALUES(@msg,@sign,@bussinessAddr,@tradeIndex,@addrFrom);";

            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        if (TradeReward.Count(tran, con, addrBussiness, addrFrom) > 0)
                        {
                            notifyMsg = $"{addrBussiness}资金锁定中，其正作为奖励使用中。";
                            tran.Rollback();
                            return;
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
                                notifyMsg = "";
                                return;
                            }
                        }
                        {
                            string sQL = mysql;
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@msg", msg);
                                command.Parameters.AddWithValue("@sign", sign);
                                command.Parameters.AddWithValue("@bussinessAddr", addrBussiness);
                                command.Parameters.AddWithValue("@tradeIndex", tradeIndex);
                                command.Parameters.AddWithValue("@addrFrom", addrFrom);
                                command.ExecuteNonQuery();
                            }
                        }
                        tran.Commit();
                    }
                    catch (Exception e)
                    {
                        throw e;
                        throw new Exception("新增错误");
                    }
                }
            }
            notifyMsg = "";
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
                            string sQL = "SELECT msg,sign FROM traderecord WHERE bussinessAddr=@bussinessAddr;";
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
                                            var sQL = "INSERT INTO traderecord(msg,sign,bussinessAddr,tradeIndex,addrFrom) VALUES(@msg,@sign,@bussinessAddr,@tradeIndex,@addrFrom);";
                                            // string sQL = mysql;
                                            // long moneycount;
                                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                                            {
                                                command.Parameters.AddWithValue("@msg", agp.msgs[i]);
                                                command.Parameters.AddWithValue("@sign", agp.list[i]);
                                                command.Parameters.AddWithValue("@bussinessAddr", tr.bussinessAddr);
                                                command.Parameters.AddWithValue("@tradeIndex", tradeIndexInDB);
                                                command.Parameters.AddWithValue("@addrFrom", tr.tradeAddress);
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
