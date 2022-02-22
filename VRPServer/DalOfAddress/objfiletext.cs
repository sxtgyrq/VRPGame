using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace DalOfAddress
{
    public class objfiletext
    {
        static readonly char[] splitParameter = new char[] { '\r', '\n' };
        internal static void Add(MySqlConnection con, MySqlTransaction tran, string amID, string objText)
        {
            clear(con, tran, amID);
            var detail = objText.Split(splitParameter, StringSplitOptions.RemoveEmptyEntries);
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
                    string sQL = @"INSERT INTO objfiletext(amID,objTextValue,textIndex)VALUES(@amID,@objTextValue,@textIndex);";
                    // long moneycount;
                    using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                    {
                        int textIndex = i + 0;
                        command.Parameters.AddWithValue("@amID", amID);
                        command.Parameters.AddWithValue("@objTextValue", item);
                        command.Parameters.AddWithValue("@textIndex", textIndex);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
        static void clear(MySqlConnection con, MySqlTransaction tran, string amID)
        {
            string sQL = @"DELETE FROM objfiletext WHERE amID=@amID;";
            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
            {
                command.Parameters.AddWithValue("@amID", amID);
                command.ExecuteNonQuery();
            }
        }

        internal static string[] Get(MySqlConnection con, MySqlTransaction tran, string amID)
        {
            List<string> result = new List<string>();
            string sQL = @"SELECT objTextValue FROM objfiletext WHERE amID=@amID ORDER BY textIndex ASC;";
            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
            {
                command.Parameters.AddWithValue("@amID", amID);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(reader.GetString("objTextValue").Trim());
                    }
                }
            }
            return result.ToArray();
        }
    }
}
