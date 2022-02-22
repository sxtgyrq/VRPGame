using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DalOfAddress
{
    public class AbtractModels
    {
        //[Obsolete]
        //public static void AddMoney(string modelName, string modelType, string imageBase64, string objText, string mtlText, string animation)
        //{
        //    using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
        //    {
        //        con.Open();
        //        using (MySqlTransaction tran = con.BeginTransaction())
        //        {
        //            try
        //            {
        //                bool hasValue;
        //                // long moneycount;
        //                {
        //                    string sQL = @"SELECT
        //                                	modelName 
        //                                FROM
        //                                	abtractmodels WHERE modelName=@modelName";
        //                    // long moneycount;
        //                    using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
        //                    {
        //                        command.Parameters.AddWithValue("@modelName", modelName);

        //                        using (var reader = command.ExecuteReader())
        //                        {
        //                            if (reader.Read())
        //                            {
        //                                hasValue = true;
        //                            }
        //                            else
        //                            {
        //                                hasValue = false;
        //                            }
        //                        }
        //                    }
        //                }
        //                if (hasValue)
        //                {
        //                    string sQL = @"UPDATE abtractmodels SET 
        //    modelType=@modelType,
        //    imageBase64=@imageBase64,
        //    objText=@objText,
        //    mtlText=@mtlText,
        //    animation=@animation
        //    WHERE modelName=@modelName";
        //                    // long moneycount;
        //                    using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
        //                    {
        //                        command.Parameters.AddWithValue("@modelType", modelType);
        //                        command.Parameters.AddWithValue("@imageBase64", imageBase64);
        //                        command.Parameters.AddWithValue("@objText", objText);
        //                        command.Parameters.AddWithValue("@mtlText", mtlText);
        //                        command.Parameters.AddWithValue("@animation", animation);
        //                        command.Parameters.AddWithValue("@modelName", modelName);
        //                        command.ExecuteNonQuery();
        //                    }

        //                }
        //                else
        //                {
        //                    string sQL = @"INSERT INTO abtractmodels(modelName,modelType,imageBase64,objText,mtlText,animation)VALUES(@modelName,@modelType,@imageBase64,@objText,@mtlText,@animation)";
        //                    // long moneycount;
        //                    using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
        //                    {
        //                        command.Parameters.AddWithValue("@modelName", modelName);
        //                        command.Parameters.AddWithValue("@modelType", modelType);
        //                        command.Parameters.AddWithValue("@imageBase64", imageBase64);
        //                        command.Parameters.AddWithValue("@objText", objText);
        //                        command.Parameters.AddWithValue("@mtlText", mtlText);
        //                        command.Parameters.AddWithValue("@animation", animation);
        //                        command.ExecuteNonQuery();
        //                    }
        //                }
        //                tran.Commit();
        //            }
        //            catch (Exception e)
        //            {
        //                throw e;
        //                throw new Exception("新增错误");
        //            }
        //        }
        //    }
        //}

        public static void Insert(string amID, string modelType, string imageBase64, string objText, string mtlText, string animation, string author, int amState, string modelName, CommonClass.databaseModel.detailmodel dm)
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
                                            	amID 
                                            FROM
                                            	abtractmodels WHERE amID=@amID";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@amID", amID);

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
                        }
                        else
                        {
                            string sQL = @"INSERT INTO abtractmodels(amID,modelType,author,amState,modelName,createTime)VALUES(@amID,@modelType,@author,@amState,@modelName,NOW());";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@amID", amID);
                                command.Parameters.AddWithValue("@modelType", modelType);
                                command.Parameters.AddWithValue("@author", author);
                                command.Parameters.AddWithValue("@amState", amState);
                                command.Parameters.AddWithValue("@modelName", modelName);
                                command.ExecuteNonQuery();
                            }
                            objfiletext.Add(con, tran, amID, objText);
                            mtlfiletext.Add(con, tran, amID, mtlText);
                            jpgfiletext.Add(con, tran, amID, imageBase64);
                            detailmodel.Add(con, tran, dm);
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

        public static CommonClass.databaseModel.abtractmodelsPassData GetAbtractModelItem(string amID)
        {
            CommonClass.databaseModel.abtractmodels m = new CommonClass.databaseModel.abtractmodels();
            string[] objFileTexts;
            string[] mtlFileTexts;
            string[] jpgTexts;
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        {
                            string sQL = @"SELECT modelName,modelType,amID,amState,author FROM abtractmodels WHERE amID=@amID";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@amID", amID);
                                using (var reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        m = new CommonClass.databaseModel.abtractmodels()
                                        {
                                            //animation = Convert.ToString(reader["animation"]).Trim(),
                                            //imageBase64 = Convert.ToString(reader["imageBase64"]).Trim(),
                                            modelName = Convert.ToString(reader["modelName"]).Trim(),
                                            modelType = Convert.ToString(reader["modelType"]).Trim(),
                                            //mtlText = Convert.ToString(reader["mtlText"]).Trim(),
                                            //objText = Convert.ToString(reader["objText"]).Trim(),
                                            amState = Convert.ToInt32(reader["amState"]),
                                            author = Convert.ToString(reader["author"]).Trim(),
                                            amID = Convert.ToString(reader["amID"]).Trim(),
                                        };
                                    }
                                }
                            }
                            objFileTexts = objfiletext.Get(con, tran, amID);
                            mtlFileTexts = mtlfiletext.Get(con, tran, amID);
                            jpgTexts = jpgfiletext.Get(con, tran, amID);
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                        throw new Exception("新增错误");
                    }
                }
            }
            var mr = new CommonClass.databaseModel.abtractmodelsPassData()
            {
                modelName = m.modelName,
                modelType = m.modelType,
                imgBase64 = jpgTexts,
                mtlTexts = mtlFileTexts,
                objTexts = objFileTexts,
            };
            return mr;
        }

        public static List<string> GetModelType()
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
                            string sQL = @"SELECT modeltype,Content FROM modeltype";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                using (var reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        result.Add(Convert.ToString(reader["modeltype"]).Trim());
                                        result.Add(Convert.ToString(reader["Content"]).Trim());
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
                            //modelID
                            //amID
                            string sQL = @"SELECT amID,author FROM abtractmodels";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                using (var reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        result.Add(Convert.ToString(reader["amID"]).Trim());
                                        result.Add(Convert.ToString(reader["author"]).Trim());
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
        public static List<string> GetCategeAm1()
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
                            //modelID
                            //amID
                            string sQL = @"SELECT amID,author FROM abtractmodels WHERE amState=1";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                using (var reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        result.Add(Convert.ToString(reader["amID"]).Trim());
                                        result.Add(Convert.ToString(reader["author"]).Trim());
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
        public static void ClearModelObj()
        {
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        {
                            var lastDate = DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd HH:mm:ss");
                            //modelID
                            //amID
                            string sQL = $"DELETE FROM abtractmodels WHERE createTime< '{lastDate}' AND amState=0";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.ExecuteNonQuery();
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
        }
    }
}
