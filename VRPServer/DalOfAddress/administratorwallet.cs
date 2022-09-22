using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace DalOfAddress
{
    internal class administratorwallet
    {
        internal static string GetAddress(MySqlConnection con, MySqlTransaction tran)
        {
            string sQL = @"SELECT A.btcAdd from administratorwallet A LEFT JOIN detailmodel B ON A.btcAdd=B.bussinessAddress WHERE B.modelID is NULL ORDER BY A.keyStr ASC;";
            string reuslt = "";
            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
            {
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        reuslt = Convert.ToString(reader["btcAdd"]);
                    }
                    else
                    {
                        reuslt = "";
                    }
                }
            }
            return reuslt;
        }

        internal static bool Exist(MySqlConnection con, MySqlTransaction tran, string addr)
        {
            string sQL = "SELECT count(*) FROM administratorwallet WHERE btcAdd=@addr;";
            int count;
            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
            {
                command.Parameters.AddWithValue("@addr", addr);
                count = Convert.ToInt32(command.ExecuteScalar());
            }
            if (count == 0)
                return false;
            else
                return true;
        }
    }
}
