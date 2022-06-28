using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DalOfAddress
{
    internal class Connection
    {
        static string ConnectionStrValue_ = "";
        public static string ConnectionStr
        {
            get
            {
                if (string.IsNullOrEmpty(ConnectionStrValue_))
                {
                    var content = File.ReadAllText("config/connect.txt");
                    ConnectionStrValue_ = CommonClass.AES.AesDecrypt(content, "Yrq123");
                    // Console.WriteLine($"{ConnectionStrValue_}");

                    using (MySqlConnection con = new MySqlConnection(ConnectionStrValue_))
                    {
                        con.Open();
                        Console.WriteLine($"Database:{con.Database}");
                        Console.WriteLine($"DataSource:{con.DataSource}");
                    }
                }
                // Console.WriteLine
                return ConnectionStrValue_;
            }
        }
    }
}
