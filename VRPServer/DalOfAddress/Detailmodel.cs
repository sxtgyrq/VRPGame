using CommonClass;
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
WHERE modelID=@modelID and locked=0;";
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
                            string sQL = @"INSERT INTO detailmodel(modelID,x,y,z,amodel,rotatey,locked)VALUES(@modelID,@x,@y,@z,@amodel,@rotatey,0);";
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
                            string sQL = @"DELETE FROM detailmodel WHERE modelID=@modelID and locked=0;";
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
        public static List<CommonClass.databaseModel.detailmodel> GetList()
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
                            string sQL = @"SELECT A.modelID,A.x,A.y,A.z,A.amodel,A.rotatey FROM detailmodel A LEFT JOIN abtractmodels B ON A.amodel=B.amID WHERE A.locked=1 AND B.amState=1 AND A.dmState=1;";
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
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

        public class ItemJsonFormat : CommonClass.MapEditor.ModelDetail
        {
        }
        public static ItemJsonFormat GetItem(string modelID)
        {
            ItemJsonFormat obj;
            string sQL = @"SELECT 
A.x,A.y,A.z,A.locked,A.dmState,A.bussinessAddress,C.Content,B.author,B.amState,B.modelName,B.createTime,B.amID 
FROM detailmodel as A
LEFT JOIN abtractmodels AS B on A.amodel=B.amID
LEFT JOIN modeltype AS C on B.modelType=C.modelType

WHERE A.modelID=@modelID;";
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                    {
                        command.Parameters.AddWithValue("@modelID", modelID);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                obj = new ItemJsonFormat()
                                {
                                    c = "modelDetail",
                                    x = Convert.ToDouble(reader["x"]),
                                    y = Convert.ToDouble(reader["y"]),
                                    z = Convert.ToDouble(reader["z"]),
                                    locked = Convert.ToBoolean(reader["locked"]),
                                    dmState = Convert.ToInt32(reader["dmState"]),
                                    bussinessAddress = reader["bussinessAddress"] == DBNull.Value ? "" : Convert.ToString(reader["bussinessAddress"]).Trim(),
                                    Content = Convert.ToString(reader["Content"]).Trim(),
                                    author = Convert.ToString(reader["author"]).Trim(),
                                    amState = Convert.ToInt32(reader["amState"]),
                                    modelName = Convert.ToString(reader["modelName"]),
                                    createTime = Convert.ToDateTime(reader["createTime"]).ToString("yyyy-MM-dd HH:mm:ss"),
                                    amID = Convert.ToString(reader["amID"])
                                };

                            }
                            else
                            {
                                obj = null;
                            }
                        }
                    }
                }
            }
            return obj;
        }

        internal static void Add(MySqlConnection con, MySqlTransaction tran, CommonClass.databaseModel.detailmodel item)
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
                //   int dmState = 0;
                string sQL = @"INSERT INTO detailmodel(modelID,x,y,z,amodel,rotatey,dmState)VALUES(@modelID,@x,@y,@z,@amodel,@rotatey,@dmState)";
                // long moneycount;
                using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                {
                    command.Parameters.AddWithValue("@modelID", item.modelID);
                    command.Parameters.AddWithValue("@x", item.x);
                    command.Parameters.AddWithValue("@y", item.y);
                    command.Parameters.AddWithValue("@z", item.z);
                    command.Parameters.AddWithValue("@amodel", item.amodel);
                    command.Parameters.AddWithValue("@rotatey", item.rotatey);
                    command.Parameters.AddWithValue("@dmState", item.dmState);
                    command.ExecuteNonQuery();
                }
            }

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
WHERE modelID=@modelID and locked=0;";
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

        public static void UpdateUsed(MapEditor.UseModelObj cn)
        {
            int dmState;
            if (cn.Used)
            {
                dmState = 1;
            }
            else
            {
                dmState = 0;
            }
            // throw new NotImplementedException();
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {

                        {
                            string sQL = @"UPDATE detailmodel b LEFT JOIN abtractmodels a  on b.amodel=a.amID
set b.dmState=@dmState,a.amState=1 WHERE  b.modelID=@modelID;";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@dmState", dmState);
                                command.Parameters.AddWithValue("@modelID", cn.modelID);
                                command.ExecuteNonQuery();
                            }

                        }
                        tran.Commit();
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
        }

        public static void UpdateLocked(MapEditor.UseModelObj cn)
        {
            int locked;
            if (cn.Used)
            {
                locked = 1;
            }
            else
            {
                locked = 0;
            }
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        {
                            string sQL = @"UPDATE detailmodel b LEFT JOIN abtractmodels a  on b.amodel=a.amID
set b.locked=@locked WHERE b.dmState=1 AND a.amState=1 AND b.modelID=@modelID;";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@locked", locked);
                                command.Parameters.AddWithValue("@modelID", cn.modelID);
                                command.ExecuteNonQuery();
                            }

                        }
                        if (locked == 1)
                        {
                            string address = administratorwallet.GetAddress(con, tran);
                            if (string.IsNullOrEmpty(address))
                            {
                                tran.Rollback();
                            }
                            else
                            {
                                //-- UPDATE detailmodel a,abtractmodels b set a.bussinessAddress='' WHERE a.amodel=b.amID AND b.modelType='direciton'
                                string sQL = @"
UPDATE detailmodel a,abtractmodels b 
SET a.bussinessAddress =@bussinessAddress 
WHERE
	a.amodel = b.amID 
	AND b.modelType = 'building' 
	AND a.modelID = @modelID
	AND ( a.bussinessAddress IS NULL OR a.bussinessAddress = '');";
                                // long moneycount;
                                using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                                {
                                    command.Parameters.AddWithValue("@bussinessAddress", address);
                                    command.Parameters.AddWithValue("@modelID", cn.modelID);
                                    command.ExecuteNonQuery();
                                }
                                tran.Commit();
                            }
                        }
                        else
                        {
                            tran.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
        }

        public static double[] GetPositionOfUnlockedObj(ref int indexOfInt, bool previous, out bool hasValue)
        {
            double[] result = new double[3] { 0, 0, 0 };
            hasValue = false;
            int countOfUnlockedObj;
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {

                        {
                            string sQL = @"SELECT COUNT(*) FROM detailmodel WHERE locked=0;";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                countOfUnlockedObj = Convert.ToInt32(command.ExecuteScalar());
                            }

                        }
                        if (countOfUnlockedObj > 0)
                        {
                            if (previous)
                            {
                                indexOfInt = (indexOfInt + countOfUnlockedObj * 2 - 1) % countOfUnlockedObj;
                            }
                            else
                            {
                                indexOfInt = (indexOfInt + 1) % countOfUnlockedObj;
                            }
                            //SELECT x,y,z FROM detailmodel WHERE locked=0 ORDER BY modelID ASC LIMIT 0,1
                            string sQL = $"SELECT x,y,z FROM detailmodel WHERE locked=0 ORDER BY modelID ASC LIMIT {indexOfInt},1";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                using (var reader = command.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        result[0] = Convert.ToDouble(reader["x"]);
                                        result[1] = Convert.ToDouble(reader["y"]);
                                        result[2] = Convert.ToDouble(reader["z"]);
                                        hasValue = true;
                                    }
                                }
                                //  countOfUnlockedObj = command.();
                            }
                        }
                        //tran.Commit();
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
            return result;
        }

        public static ModelTranstraction.GetModelByID.Result GetByID(string modelID)
        {
            ModelTranstraction.GetModelByID.Result r;
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {

                        {
                            string sQL = $@"SELECT  A.x,A.y,A.z ,A.amodel,A.rotatey,A.bussinessAddress,A.modelID,
B.author
FROM detailmodel A 
LEFT JOIN abtractmodels B ON A.amodel=B.amID
WHERE locked=1 AND dmState=1 AND A.modelID='{modelID}';";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                using (var reader = command.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        r = new ModelTranstraction.GetModelByID.Result()
                                        {
                                            x = Convert.ToDouble(reader["x"]),
                                            y = Convert.ToDouble(reader["y"]),
                                            z = Convert.ToDouble(reader["z"]),
                                            amodel = Convert.ToString(reader["amodel"]).Trim(),
                                            rotatey = Convert.ToDouble(reader["rotatey"]),
                                            bussinessAddress = Convert.ToString(reader["bussinessAddress"]).Trim(),
                                            modelID = Convert.ToString(reader["modelID"]).Trim(),
                                            author = Convert.ToString(reader["author"]).Trim(),
                                        };
                                    }
                                    else
                                        r = null;
                                }
                            }

                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
            return r;
        }

        //public static CommonClass.ModelTranstraction.GetFirstModelAddr.Result GetModel(string bTCAddr)
        //{
        //    ModelTranstraction.GetFirstModelAddr.Result  ;
        //    using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
        //    {
        //        con.Open();
        //        using (MySqlTransaction tran = con.BeginTransaction())
        //        {
        //            try
        //            {
        //                {
        //                    string sQL = @"SELECT bussinessAddress FROM detailmodel WHERE locked=1 AND dmState=1  ORDER BY bussinessAddress LIMIT 0,1;";
        //                    // long moneycount;
        //                    using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
        //                    {
        //                        var result = command.ExecuteScalar();
        //                        if (result == null)
        //                        {
        //                            address = "";
        //                        }
        //                        else if (result == DBNull.Value)
        //                        {
        //                            address = "";
        //                        }
        //                        else
        //                        {
        //                            address = Convert.ToString(result).Trim();
        //                        }
        //                    }

        //                }
        //            }
        //            catch (Exception e)
        //            {
        //                throw e;
        //            }
        //        }
        //    }
        //    return address;
        //}

        public static List<ModelTranstraction.GetAllModelPosition.Result> GetAll()
        {
            List<ModelTranstraction.GetAllModelPosition.Result> allmodelPoints
                = new List<ModelTranstraction.GetAllModelPosition.Result>();
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        {
                            var sQL = "SELECT modelID,x,z FROM detailmodel WHERE locked=1 AND dmState=1;";
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                using (var reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        var item = new ModelTranstraction.GetAllModelPosition.Result()
                                        {
                                            x = Convert.ToDouble(reader["x"]),
                                            z = Convert.ToDouble(reader["z"]),
                                            modelID = Convert.ToString(reader["modelID"]).Trim(),
                                        };
                                        allmodelPoints.Add(item);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
            return allmodelPoints;
        }

        public static List<string> GetAllAddr()
        {
            List<string> allAddr = new List<string>();
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        {
                            var sQL = "SELECT bussinessAddress FROM detailmodel WHERE locked=1 AND dmState=1;";
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                using (var reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        string addr = Convert.ToString(reader["bussinessAddress"]).Trim();
                                        if (!string.IsNullOrEmpty(addr)) allAddr.Add(addr);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
            return allAddr;
        }


        public static List<string> GetAllBussinessAddr()
        {
            var result = new List<string>();
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        {
                            var sQL = @"SELECT
	bussinessAddress 
FROM
	detailmodel 
WHERE
	locked = 1 
	AND dmState = 1 
	AND ( bussinessAddress IS NOT NULL ) 
ORDER BY
	bussinessAddress ASC;";
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                using (var reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        result.Add(Convert.ToString(reader["bussinessAddress"]).Trim());
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
            return result;
        }

        public static ModelTranstraction.GetModelByID.Result GetByAddr(string bussinessAddr)
        {
            ModelTranstraction.GetModelByID.Result r;
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {

                        {
                            string sQL = $@"SELECT  A.x,A.y,A.z ,A.amodel,A.rotatey,A.bussinessAddress,A.modelID,
B.author
FROM detailmodel A 
LEFT JOIN abtractmodels B ON A.amodel=B.amID
WHERE locked=1 AND dmState=1 AND A.bussinessAddress='{bussinessAddr}';";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                using (var reader = command.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        r = new ModelTranstraction.GetModelByID.Result()
                                        {
                                            x = Convert.ToDouble(reader["x"]),
                                            y = Convert.ToDouble(reader["y"]),
                                            z = Convert.ToDouble(reader["z"]),
                                            amodel = Convert.ToString(reader["amodel"]).Trim(),
                                            rotatey = Convert.ToDouble(reader["rotatey"]),
                                            bussinessAddress = Convert.ToString(reader["bussinessAddress"]).Trim(),
                                            modelID = Convert.ToString(reader["modelID"]).Trim(),
                                            author = Convert.ToString(reader["author"]).Trim(),
                                        };
                                    }
                                    else
                                        r = null;
                                }
                            }

                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
            return r;
        }
        //        public static CommonClass.ModelTranstraction.GetFirstModelAddr.Result GetFirst()
        //        {
        //            int index = 0;
        //            ModelTranstraction.GetFirstModelAddr.Result first;
        //            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
        //            {
        //                con.Open();
        //                using (MySqlTransaction tran = con.BeginTransaction())
        //                {
        //                    try
        //                    {
        //                        {
        //                            var sQL = "SELECT   COUNT(*) FROM detailmodel WHERE locked=1 AND dmState=1;";
        //                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
        //                            {
        //                                var count = Convert.ToInt32(command.ExecuteScalar());
        //                                if (count > 0)
        //                                    index = index % count;
        //                            }
        //                        }
        //                        {
        //                            string sQL = $@"SELECT  A.x,A.y,A.z ,A.amodel,A.rotatey,A.bussinessAddress,A.modelID,
        //B.author
        //FROM detailmodel A 
        //LEFT JOIN abtractmodels B ON A.amodel=B.amID
        //WHERE locked=1 AND dmState=1  ORDER BY bussinessAddress LIMIT {index},1;";
        //                            // long moneycount;
        //                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
        //                            {
        //                                using (var reader = command.ExecuteReader())
        //                                {
        //                                    if (reader.Read())
        //                                    {
        //                                        first = new ModelTranstraction.GetFirstModelAddr.Result()
        //                                        {
        //                                            x = Convert.ToDouble(reader["x"]),
        //                                            y = Convert.ToDouble(reader["y"]),
        //                                            z = Convert.ToDouble(reader["z"]),
        //                                            amodel = Convert.ToString(reader["amodel"]).Trim(),
        //                                            rotatey = Convert.ToDouble(reader["rotatey"]),
        //                                            bussinessAddress = Convert.ToString(reader["bussinessAddress"]).Trim(),
        //                                            modelID = Convert.ToString(reader["modelID"]).Trim(),
        //                                            author = Convert.ToString(reader["author"]).Trim(),
        //                                        };
        //                                    }
        //                                    else
        //                                        first = null;
        //                                }
        //                            }

        //                        }
        //                    }
        //                    catch (Exception e)
        //                    {
        //                        throw e;
        //                    }
        //                }
        //            }
        //            return first;
        //        }
    }
}
