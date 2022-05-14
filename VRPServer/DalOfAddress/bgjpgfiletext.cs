using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace DalOfAddress
{
    internal class bgjpgfiletext
    {
        internal static void Clear(MySqlConnection con, MySqlTransaction tran, string crossID)
        {
            string sQL = @"DELETE FROM bgjpgfiletext WHERE crossID=@crossID;";
            // long moneycount;
            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
            {
                command.Parameters.AddWithValue("@crossID", crossID);
                command.ExecuteNonQuery();
            }
        }

        internal static void Insert(MySqlConnection con, MySqlTransaction tran, string crossID, string base64, string direction)
        {
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
                    string sQL = @"INSERT INTO bgjpgfiletext(crossID,jpgTextValue,textIndex,direction)VALUES(@crossID,@jpgTextValue,@textIndex,@direction);";
                    using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                    {
                        int textIndex = i / 255;
                        command.Parameters.AddWithValue("@crossID", crossID);
                        command.Parameters.AddWithValue("@jpgTextValue", item);
                        command.Parameters.AddWithValue("@textIndex", textIndex);
                        command.Parameters.AddWithValue("@direction", direction);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        internal static string Get(MySqlConnection con, MySqlTransaction tran, string crossID, string direction)
        {
            string text = "";
            string sQL = $"SELECT jpgTextValue FROM bgjpgfiletext WHERE direction='{direction}' AND crossID='{crossID}' ORDER BY textIndex";
            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        text += reader.GetString(0);
                    }
                }
            }
            return text;
        }
    }
}
