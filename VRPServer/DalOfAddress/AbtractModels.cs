using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace DalOfAddress
{
    public class AbtractModels
    {
        public static void AddMoney(string modelName, string modelType, string imageBase64, string objText, string mtlText, string animation)
        {
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        bool hasValue;
                        // long moneycount;
                        {
                            string sQL = @"SELECT
                            	modelName 
                            FROM
                            	abtractmodels WHERE modelName=@modelName";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@modelName", modelName);

                                using (var reader = command.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        hasValue = true;
                                    }
                                    else
                                    {
                                        hasValue = false;
                                    }
                                }
                            }
                        }
                        if (hasValue)
                        {
                            string sQL = @"UPDATE abtractmodels SET 
modelType=@modelType,
imageBase64=@imageBase64,
objText=@objText,
mtlText=@mtlText,
animation=@animation
WHERE modelName=@modelName";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@modelType", modelType);
                                command.Parameters.AddWithValue("@imageBase64", imageBase64);
                                command.Parameters.AddWithValue("@objText", objText);
                                command.Parameters.AddWithValue("@mtlText", mtlText);
                                command.Parameters.AddWithValue("@animation", animation);
                                command.Parameters.AddWithValue("@modelName", modelName);
                                command.ExecuteNonQuery();
                            }

                        }
                        else
                        {
                            string sQL = @"INSERT INTO abtractmodels(modelName,modelType,imageBase64,objText,mtlText,animation)VALUES(@modelName,@modelType,@imageBase64,@objText,@mtlText,@animation)";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@modelName", modelName);
                                command.Parameters.AddWithValue("@modelType", modelType);
                                command.Parameters.AddWithValue("@imageBase64", imageBase64);
                                command.Parameters.AddWithValue("@objText", objText);
                                command.Parameters.AddWithValue("@mtlText", mtlText);
                                command.Parameters.AddWithValue("@animation", animation);
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

        public static CommonClass.databaseModel.abtractmodels GetAbtractModelItem(string modelName)
        {
            CommonClass.databaseModel.abtractmodels m = new CommonClass.databaseModel.abtractmodels();
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        {
                            string sQL = @"SELECT modelName,modelType,imageBase64,objText,mtlText,animation FROM abtractmodels WHERE modelName=@modelName";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@modelName", modelName);
                                using (var reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        m = new CommonClass.databaseModel.abtractmodels()
                                        {
                                            animation = Convert.ToString(reader["animation"]).Trim(),
                                            imageBase64 = Convert.ToString(reader["imageBase64"]).Trim(),
                                            modelName = Convert.ToString(reader["modelName"]).Trim(),
                                            modelType = Convert.ToString(reader["modelType"]).Trim(),
                                            mtlText = Convert.ToString(reader["mtlText"]).Trim(),
                                            objText = Convert.ToString(reader["objText"]).Trim(),
                                        }; 
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
            return m; 
        }

        public static List<string> GetCatege()
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
                            string sQL = @"SELECT modelName,modelType FROM abtractmodels";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                using (var reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        result.Add(Convert.ToString(reader["modelName"]).Trim());
                                        result.Add(Convert.ToString(reader["modelType"]).Trim());
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
    }
}
