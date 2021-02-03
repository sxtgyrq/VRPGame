﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace DalOfAddress
{
    public class MoneyAdd
    {
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
                            string sQL = @"SELECT
                            	moneyaddress,
                            	moneycount 
                            FROM
                            	addressmoney WHERE moneyaddress=@moneyaddress";
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
                            string sQL = @"UPDATE addressmoney SET moneycount=@moneycount WHERE moneyaddress=@moneyaddress";
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
                            string sQL = @"INSERT INTO addressmoney(moneyaddress,moneycount)VALUES(@moneyaddress,@moneycount)";
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
    }
}
