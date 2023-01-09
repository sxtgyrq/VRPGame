using MySql.Data.MySqlClient;
using System;

namespace DalOfAddress
{
    public class MoneyRefererAdd
    {
        const string tableName = "addressreferermoney";
        public static void AddMoney(string address, long money)
        {
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        bool hasValue;
                        long moneycount;
                        {
                            string sQL = $@"SELECT
                            	moneyaddress,
                            	moneycount 
                            FROM
                            	{tableName} WHERE moneyaddress=@moneyaddress";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@moneyaddress", address);

                                using (var reader = command.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {

                                        moneycount = Convert.ToInt64(reader["moneycount"]);

                                        hasValue = true;
                                    }
                                    else
                                    {
                                        moneycount = 0;
                                        hasValue = false;
                                    }
                                    moneycount += money;
                                }
                            }
                        }
                        if (hasValue)
                        {
                            string sQL = $@"UPDATE {tableName} SET moneycount=@moneycount WHERE moneyaddress=@moneyaddress";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@moneycount", moneycount);
                                command.Parameters.AddWithValue("@moneyaddress", address);
                                command.ExecuteNonQuery();
                            }

                        }
                        else
                        {
                            string sQL = $@"INSERT INTO {tableName}(moneyaddress,moneycount)VALUES(@moneyaddress,@moneycount)";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@moneyaddress", address);
                                command.Parameters.AddWithValue("@moneycount", moneycount);
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

        internal static long GetMoney(MySqlConnection con, MySqlTransaction tran, string address)
        {
            long moneycount;
            {
                string sQL = $@"SELECT
                            	moneyaddress,
                            	moneycount 
                            FROM
                            	{tableName} WHERE moneyaddress=@moneyaddress";
                // long moneycount;
                using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                {
                    command.Parameters.AddWithValue("@moneyaddress", address);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            moneycount = Convert.ToInt64(reader["moneycount"]);
                        }
                        else
                        {
                            moneycount = 0;
                        }

                    }
                }
            }
            return moneycount;
        }
        public static long GetMoney(string address)
        {
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        long moneycount;
                        {
                            string sQL = $@"SELECT
                            	moneyaddress,
                            	moneycount 
                            FROM
                            	{tableName} WHERE moneyaddress=@moneyaddress";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@moneyaddress", address);

                                using (var reader = command.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {

                                        moneycount = Convert.ToInt64(reader["moneycount"]);
                                    }
                                    else
                                    {
                                        moneycount = 0;
                                    }

                                }
                            }
                        }
                        return moneycount;
                    }
                    catch (Exception e)
                    {
                        throw e;
                        throw new Exception("新增错误");
                    }
                }
            }
        }
    }
}
