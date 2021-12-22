using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace DalOfAddress
{
    public class detailmodel
    {
        public static void Add(CommonClass.databaseModel.detailmodel item)
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
                            	modelID 
                            FROM
                            	detailmodel WHERE modelID=@modelID";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@modelID", item.modelID);

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
                            string sQL = @"UPDATE detailmodel SET 
x=@x,
y=@y,
z=@z,
rotatey=@rotatey 
WHERE modelID=@modelID";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@x", item.x);
                                command.Parameters.AddWithValue("@y", item.y);
                                command.Parameters.AddWithValue("@z", item.z);
                                command.Parameters.AddWithValue("@rotatey", item.rotatey);
                                command.Parameters.AddWithValue("@modelID", item.rotatey);
                                command.ExecuteNonQuery();
                            }

                        }
                        else
                        {
                            string sQL = @"INSERT INTO detailmodel(modelID,x,y,z,amodel,rotatey)VALUES(@modelID,@x,@y,@z,@amodel,@rotatey)";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@modelID", item.modelID);
                                command.Parameters.AddWithValue("@x", item.x);
                                command.Parameters.AddWithValue("@y", item.y);
                                command.Parameters.AddWithValue("@z", item.z);
                                command.Parameters.AddWithValue("@amodel", item.amodel);
                                command.Parameters.AddWithValue("@rotatey", item.rotatey);
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

        public static void Delete(string modelID)
        {
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {

                        {
                            string sQL = @"DELETE FROM detailmodel WHERE modelID=@modelID";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            { 
                                command.Parameters.AddWithValue("@modelID", modelID);
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

        public static List<CommonClass.databaseModel.detailmodel> GetList(double x, double z)
        {
            List<CommonClass.databaseModel.detailmodel> result = new List<CommonClass.databaseModel.detailmodel>();
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        {
                            string sQL = @"SELECT modelID,x,y,z,amodel,rotatey FROM detailmodel WHERE (x-@x)*(x-@x)+(z-@z)*(z-@z)<100*100;";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@x", x);
                                command.Parameters.AddWithValue("@z", z);

                                using (var reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        result.Add(new CommonClass.databaseModel.detailmodel()
                                        {
                                            modelID = Convert.ToString(reader["modelID"]).Trim(),
                                            amodel = Convert.ToString(reader["amodel"]).Trim(),
                                            rotatey = Convert.ToSingle(reader["rotatey"]),
                                            x = Convert.ToSingle(reader["x"]),
                                            y = Convert.ToSingle(reader["y"]),
                                            z = Convert.ToSingle(reader["z"]),
                                        });
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

        public static void Update(CommonClass.databaseModel.detailmodel item)
        {
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {

                        {
                            string sQL = @"UPDATE detailmodel SET 
x=@x,
y=@y,
z=@z,
rotatey=@rotatey 
WHERE modelID=@modelID";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@x", item.x);
                                command.Parameters.AddWithValue("@y", item.y);
                                command.Parameters.AddWithValue("@z", item.z);
                                command.Parameters.AddWithValue("@rotatey", item.rotatey);
                                command.Parameters.AddWithValue("@modelID", item.modelID);
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
