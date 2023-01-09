using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DalOfAddress
{
    public class Connection
    {
        static string ConnectionStrValue_ = "";
        static string password = "Yrq123";
        public static string PasswordStr
        {
            get { return password; }
        }
        public static string ConnectionStr
        {
            get
            {
                if (string.IsNullOrEmpty(ConnectionStrValue_))
                {
                    var content = File.ReadAllText("config/connect.txt");
                    ConnectionStrValue_ = CommonClass.AES.AesDecrypt(content, password);

                    using (MySqlConnection con = new MySqlConnection(ConnectionStrValue_))
                    {
                        con.Open();
                        Console.WriteLine($"Database:{con.Database}");
                        Console.WriteLine($"DataSource:{con.DataSource}");
                    }
                }
                return ConnectionStrValue_;
            }
        }

        public static void SetPassWord(string pass)
        {
            if (string.IsNullOrEmpty(pass.Trim()))
            { }
            else
            {
                Connection.password = pass;
            }
        }
    }
}
