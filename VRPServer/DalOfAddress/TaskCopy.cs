using CommonClass.databaseModel;
using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace DalOfAddress
{
    public class TaskCopy
    {
        public static bool Add(taskcopy tc)
        {
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        int countOfItem;
                        {
                            string sQL = $"SELECT COUNT(*) FROM taskcopy WHERE btcAddr='{tc.btcAddr}' AND taskCopyCode='{tc.taskCopyCode}'";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                countOfItem = Convert.ToInt32(command.ExecuteScalar());
                            }
                        }
                        if (countOfItem != 0)
                        {
                            return false;
                        }
                        else
                        {
                            string sQL = @"
INSERT INTO taskcopy (btcAddr,taskCopyCode,firstRound,secondRound,Tag,Result,ResultDateTime)
VALUES
	(
		@btcAddr,
		@taskCopyCode,
		@firstRound,
		@secondRound,
		@Tag,
		@Result,
	@ResultDateTime 
	)
";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@btcAddr", tc.btcAddr);
                                command.Parameters.AddWithValue("@taskCopyCode", tc.taskCopyCode);
                                command.Parameters.AddWithValue("@firstRound", tc.firstRound);
                                command.Parameters.AddWithValue("@secondRound", tc.secondRound);
                                command.Parameters.AddWithValue("@Tag", tc.Tag);
                                command.Parameters.AddWithValue("@Result", tc.Result);
                                command.Parameters.AddWithValue("@ResultDateTime", tc.ResultDateTime);

                                countOfItem = command.ExecuteNonQuery();
                            }
                            if (countOfItem == 1)
                            {
                                tran.Commit();
                                return true;
                            }
                            else
                            {
                                tran.Rollback();
                                return false;
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

        public static List<CommonClass.databaseModel.taskcopy> GetALLItem(string address)
        {
            List<CommonClass.databaseModel.taskcopy> result = new List<CommonClass.databaseModel.taskcopy>();
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        {
                            string sQL = $"SELECT btcAddr,taskCopyCode,firstRound,secondRound,Tag,Result,ResultDateTime FROM taskcopy WHERE btcAddr='{address}'";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {

                                using (var reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        result.Add(new CommonClass.databaseModel.taskcopy()
                                        {
                                            btcAddr = Convert.ToString(reader["btcAddr"]).Trim(),
                                            taskCopyCode = Convert.ToString(reader.GetString("taskCopyCode")).Trim(),
                                            firstRound = Convert.ToInt32(reader["firstRound"]),
                                            secondRound = Convert.ToInt32(reader["secondRound"]),
                                            Tag = reader["Tag"] == DBNull.Value ? "" : Convert.ToString(reader["Tag"]).Trim(),
                                            Result = reader["Result"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["Result"]),
                                            ResultDateTime = reader["ResultDateTime"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(reader["ResultDateTime"]),
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

        public static List<CommonClass.databaseModel.taskcopy> GetALLItem(string address, string code)
        {
            List<CommonClass.databaseModel.taskcopy> result = new List<CommonClass.databaseModel.taskcopy>();
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        {
                            string sQL = $"SELECT btcAddr,taskCopyCode,firstRound,secondRound,Tag,Result,ResultDateTime FROM taskcopy WHERE btcAddr='{address}' AND taskCopyCode='{code}'";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {

                                using (var reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        result.Add(new CommonClass.databaseModel.taskcopy()
                                        {
                                            btcAddr = Convert.ToString(reader["btcAddr"]).Trim(),
                                            taskCopyCode = Convert.ToString(reader.GetString("taskCopyCode")).Trim(),
                                            firstRound = Convert.ToInt32(reader["firstRound"]),
                                            secondRound = Convert.ToInt32(reader["secondRound"]),
                                            Tag = reader["Tag"] == DBNull.Value ? "" : Convert.ToString(reader["Tag"]).Trim(),
                                            Result = reader["Result"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["Result"]),
                                            ResultDateTime = reader["ResultDateTime"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(reader["ResultDateTime"]),
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

        public static bool UpdateRoundOfItem(taskcopy tc, int oldFirst, int oldSecond)
        {
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        int countOfItem;
                        {
                            string sQL = $"UPDATE taskcopy SET firstRound={tc.firstRound},secondRound={tc.secondRound} WHERE btcAddr='{tc.btcAddr}' AND taskCopyCode='{tc.taskCopyCode}' AND firstRound='{oldFirst}' AND secondRound='{oldSecond}' ;";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                countOfItem = command.ExecuteNonQuery();
                            }
                            if (countOfItem == 1)
                            {
                                tran.Commit();
                                return true;
                            }
                            else
                            {
                                tran.Rollback();
                                return false;
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

        public static bool UpdateResultOfItem(taskcopy tc)
        {
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        int countOfItem;
                        {
                            string sQL = $"UPDATE taskcopy SET Tag=@Tag,Result=@Result,ResultDateTime=@ResultDateTime WHERE btcAddr='{tc.btcAddr}' AND taskCopyCode='{tc.taskCopyCode}';";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@Tag", string.IsNullOrEmpty(tc.Tag) ? null : tc.Tag);
                                command.Parameters.AddWithValue("@Result", tc.Result.HasValue ? tc.Result : null);
                                command.Parameters.AddWithValue("@ResultDateTime", tc.ResultDateTime.HasValue ? tc.ResultDateTime : null);
                                countOfItem = command.ExecuteNonQuery();
                            }
                            if (countOfItem == 1)
                            {
                                tran.Commit();
                                return true;
                            }
                            else
                            {
                                tran.Rollback();
                                return false;
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

        public static void Del(string bTCAddress, string code)
        {
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        int countOfItem;
                        {
                            string sQL = $"Delete FROM taskcopy WHERE btcAddr='{bTCAddress}' AND taskCopyCode='{code}'";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                countOfItem = command.ExecuteNonQuery();
                            }
                        }
                        if (countOfItem != 1)
                        {
                            tran.Rollback();
                        }
                        else
                        {
                            tran.Commit();
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

        public static List<CommonClass.databaseModel.taskcopy> GetItem(string code, string addr)
        {
            List<CommonClass.databaseModel.taskcopy> result = new List<CommonClass.databaseModel.taskcopy>();
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        {
                            string sQL = $"SELECT btcAddr,taskCopyCode,firstRound,secondRound,Tag,Result,ResultDateTime FROM taskcopy WHERE taskCopyCode='{code}' AND btcAddr LIKE'%{addr}%';";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {

                                using (var reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        result.Add(new CommonClass.databaseModel.taskcopy()
                                        {
                                            btcAddr = Convert.ToString(reader["btcAddr"]).Trim(),
                                            taskCopyCode = Convert.ToString(reader.GetString("taskCopyCode")).Trim(),
                                            firstRound = Convert.ToInt32(reader["firstRound"]),
                                            secondRound = Convert.ToInt32(reader["secondRound"]),
                                            Tag = reader["Tag"] == DBNull.Value ? "" : Convert.ToString(reader["Tag"]).Trim(),
                                            Result = reader["Result"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["Result"]),
                                            ResultDateTime = reader["ResultDateTime"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(reader["ResultDateTime"]),
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
    }
}
