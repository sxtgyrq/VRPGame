using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace DalOfAddress
{

    public class mtlfiletext
    {
        static readonly char[] splitParameter = new char[] { '\r', '\n' };
        internal static void Add(MySqlConnection con, MySqlTransaction tran, string amID, string mtlText)
        {
            clear(con, tran, amID);
            var detail = mtlText.Split(splitParameter, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < detail.Length; i++)
            {
                var item = detail[i];
                if (item.Length < 2) { }
                else
                {
                    if (item.Length > 255)
                    {
                        item = item.Substring(0, 255);
                    }
                    string sQL = @"INSERT INTO mtlfiletext(amID,mtlTextValue,textIndex)VALUES(@amID,@mtlTextValue,@textIndex);";
                    // long moneycount;
                    using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                    {
                        int textIndex = i + 0;
                        command.Parameters.AddWithValue("@amID", amID);
                        command.Parameters.AddWithValue("@mtlTextValue", item);
                        command.Parameters.AddWithValue("@textIndex", textIndex);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
        static void clear(MySqlConnection con, MySqlTransaction tran, string amID)
        {
            string sQL = @"DELETE FROM mtlfiletext WHERE amID=@amID;";
            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
            {
                command.Parameters.AddWithValue("@amID", amID);
                command.ExecuteNonQuery();
            }
        }

        internal static string[] Get(MySqlConnection con, MySqlTransaction tran, string amID)
        {
            List<string> result = new List<string>();
            string sQL = @"SELECT mtlTextValue FROM mtlfiletext WHERE amID=@amID ORDER BY textIndex ASC;";
            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
            {
                command.Parameters.AddWithValue("@amID", amID);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(reader.GetString("mtlTextValue").Trim());
                    }
                }
            }
            return result.ToArray();
        }
    }
}
