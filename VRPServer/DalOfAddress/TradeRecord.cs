using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace DalOfAddress
{
    public class TradeRecord
    {
        public static void Add(int tradeIndex, string addrFrom, string addrBussiness, string sign, string msg, double passCoin)
        {
            var mysql = "INSERT INTO traderecord(msg,sign,bussinessAddr,tradeIndex,addrFrom) VALUES(@msg,@sign,@bussinessAddr,@tradeIndex,@addrFrom);";

            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
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
            int result = 0;
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        {
                            string sQL = "SELECT COUNT(*) FROM traderecord WHERE bussinessAddr=@bussinessAddr AND addrFrom=@addrFrom";
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@bussinessAddr", bussinessAddr);
                                command.Parameters.AddWithValue("@addrFrom", addrFrom);
                                result = Convert.ToInt32(command.ExecuteScalar());
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
    }
}
