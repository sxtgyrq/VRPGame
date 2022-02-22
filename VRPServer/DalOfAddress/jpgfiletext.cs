using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace DalOfAddress
{
    public class jpgfiletext
    {
        internal static void Add(MySqlConnection con, MySqlTransaction tran, string amID, string base64)
        {
            clear(con, tran, amID);
            for (int i = 0; i < base64.Length; i += 255)
            {
                var length = base64.Length - i;
                if (length < 255) { }
                else
                {
                    length = 255;
                }
                var item = base64.Substring(i, length);
                {
                    string sQL = @"INSERT INTO jpgfiletext(amID,jpgTextValue,textIndex)VALUES(@amID,@jpgTextValue,@textIndex);";
                    using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                    {
                        int textIndex = i / 255;
                        command.Parameters.AddWithValue("@amID", amID);
                        command.Parameters.AddWithValue("@jpgTextValue", item);
                        command.Parameters.AddWithValue("@textIndex", textIndex);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
        static void clear(MySqlConnection con, MySqlTransaction tran, string amID)
        {
            string sQL = @"DELETE FROM jpgfiletext WHERE amID=@amID;";
            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
            {
                command.Parameters.AddWithValue("@amID", amID);
                command.ExecuteNonQuery();
            }
        }

        internal static string[] Get(MySqlConnection con, MySqlTransaction tran, string amID)
        {
            List<string> result = new List<string>();
            string sQL = @"SELECT jpgTextValue FROM jpgfiletext WHERE amID=@amID ORDER BY textIndex ASC;";
            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
            {
                command.Parameters.AddWithValue("@amID", amID);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(reader.GetString("jpgTextValue").Trim());
                    }
                }
            }
            return result.ToArray();
        }
    }
}
