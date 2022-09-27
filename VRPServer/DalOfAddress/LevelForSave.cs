using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace DalOfAddress
{
    public class LevelForSave
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="address">BitcoinAddr</param> 
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        static CommonClass.databaseModel.LevelForSave Get(MySqlConnection con, MySqlTransaction tran, string address)
        {
            try
            {
                {
                    string sQL = @"SELECT BtcAddr,TimeStampStr,`Level` FROM levelforsave WHERE BtcAddr=@BtcAddr;";
                    // long moneycount;
                    using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                    {
                        command.Parameters.AddWithValue("@BtcAddr", address);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var obj = new CommonClass.databaseModel.LevelForSave()
                                {
                                    BtcAddr = reader.GetString("BtcAddr").Trim(),
                                    Level = reader.GetInt32("Level"),
                                    TimeStampStr = reader.GetString("TimeStampStr").Trim()
                                };
                                return obj;
                            }
                            else
                            {
                                return null;
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

        internal static int GetLevel(MySqlTransaction tran, MySqlConnection con, string address)
        {
            var getItem = Get(con, tran, address);
            if (getItem == null)
            {
                return 1;
            }
            else
            {
                return getItem.Level;
            }
        }
        public static int GetLevel(string address)
        {
            int r;
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    r = GetLevel(tran, con, address);
                }
            }
            return r;
        }
        public enum UpdateResultInDB
        {
            HasDataButUpdateError,
            Success,
            InsertError,
            LevelIsLow,
            WrongTimeStr,
            LevelHasUsedForRewad
        }
        public static CommonClass.databaseModel.LevelForSave Update(string address, string timeStr, int level, out UpdateResultInDB remarkInTran)
        {
            /*
             * 此方法的逻辑 
             *      
             *                hasData有数据                        无数据
             * timeStr 空     timeStr赋值，更新，等级取最大值     赋值，插入timeStr，更新
             * timeStr 有值   timeStr 作为条件进行更新            等级已经被使用 
             */

            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        bool hasData;
                        {
                            var getItem = Get(con, tran, address);
                            if (getItem != null)
                                hasData = true;
                            else
                                hasData = false;
                        }
                        if (hasData)
                        {
                            if (string.IsNullOrEmpty(timeStr))
                            {
                                int rows;
                                timeStr = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                                {

                                    string sQL = @"UPDATE levelforsave SET TimeStampStr=@TimeStampStr WHERE BtcAddr=@BtcAddr";

                                    using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                                    {
                                        command.Parameters.AddWithValue("@TimeStampStr", timeStr);
                                        command.Parameters.AddWithValue("@BtcAddr", address);
                                        rows = command.ExecuteNonQuery();
                                    }
                                }
                                if (rows == 1)
                                {
                                    var Item = Get(con, tran, address);
                                    if (Item.TimeStampStr != timeStr)
                                    {
                                        remarkInTran = UpdateResultInDB.HasDataButUpdateError;
                                        tran.Rollback();
                                        return null;
                                    }
                                    else
                                    {
                                        while (Item.Level < level)
                                        {
                                            string sQL = @"UPDATE levelforsave SET `Level`=@lev WHERE BtcAddr=@BtcAddr AND TimeStampStr=@TimeStampStr ";

                                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                                            {
                                                command.Parameters.AddWithValue("@lev", level);
                                                command.Parameters.AddWithValue("@BtcAddr", address);
                                                command.Parameters.AddWithValue("@TimeStampStr", timeStr);
                                                rows = command.ExecuteNonQuery();
                                            }
                                            Item = Get(con, tran, address);
                                        }
                                        if (rows == 1)
                                        {
                                            remarkInTran = UpdateResultInDB.Success;
                                            tran.Commit();
                                            return Item;
                                        }
                                        else
                                        {
                                            remarkInTran = UpdateResultInDB.HasDataButUpdateError;
                                            tran.Rollback();
                                            return null;
                                        }
                                    }
                                }
                                else
                                {
                                    remarkInTran = UpdateResultInDB.HasDataButUpdateError;
                                    tran.Rollback();
                                    return null;
                                }
                            }
                            else
                            {
                                int rows;
                                var newTimeStr = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                                string sQL = @"UPDATE levelforsave SET TimeStampStr=@tsNew,`Level`=@ll WHERE BtcAddr=@BtcAddr AND TimeStampStr=@tsOld";
                                // long moneycount;
                                using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                                {
                                    command.Parameters.AddWithValue("@tsNew", newTimeStr);
                                    command.Parameters.AddWithValue("@ll", level);
                                    command.Parameters.AddWithValue("@BtcAddr", address);
                                    command.Parameters.AddWithValue("@tsOld", timeStr);
                                    rows = command.ExecuteNonQuery();
                                }
                                if (rows == 1)
                                {
                                    var getItem = Get(con, tran, address);
                                    tran.Commit();
                                    remarkInTran = UpdateResultInDB.Success;
                                    return getItem;
                                }
                                else
                                {
                                    remarkInTran = UpdateResultInDB.WrongTimeStr;
                                    tran.Rollback();
                                    return null;
                                }
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(timeStr))
                                if (level >= 2)
                                {
                                    int row = 0;
                                    timeStr = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                                    string sQL = @"INSERT INTO levelforsave (BtcAddr,TimeStampStr,`Level`) VALUES (@BtcAddr,@TimeStampStr,@Leveel);";
                                    using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                                    {
                                        command.Parameters.AddWithValue("@BtcAddr", address);
                                        command.Parameters.AddWithValue("@TimeStampStr", timeStr);
                                        command.Parameters.AddWithValue("@Leveel", level); 
                                        row = command.ExecuteNonQuery();
                                    }
                                    if (row == 1)
                                    {
                                        var Item = Get(con, tran, address);
                                        tran.Commit();
                                        remarkInTran = UpdateResultInDB.Success;
                                        return Item;
                                    }
                                    else
                                    {
                                        remarkInTran = UpdateResultInDB.InsertError;
                                        tran.Rollback();
                                        return null;
                                    }
                                }
                                else
                                {
                                    remarkInTran = UpdateResultInDB.LevelIsLow;
                                    tran.Rollback();
                                    return null;
                                }
                            else
                            {
                                tran.Rollback();
                                remarkInTran = UpdateResultInDB.LevelHasUsedForRewad;
                                return null;
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
            //throw new NotImplementedException();
        }

        internal static int RemoveLevelWithAddr(MySqlConnection con, MySqlTransaction tran, string applyAddr, int levle)
        {
            int row;
            var sQL = $"DELETE FROM levelforsave  WHERE BtcAddr=@BtcAddr AND `Level`={levle}";
            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
            {
                command.Parameters.AddWithValue("@BtcAddr", applyAddr);
                row = command.ExecuteNonQuery();
            };
            return row;
        }

        /// <summary>
        /// 此方法，在测试时使用
        /// </summary>
        /// <param name="address"></param>
        public static void Del(string address)
        {
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        int rows;
                        {
                            string sQL = @"DELETE FROM levelforsave WHERE BtcAddr=@BtcAddr;";
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@BtcAddr", address);
                                rows = command.ExecuteNonQuery();
                            }
                        }
                        if (rows == 1)
                        {
                            tran.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                        throw new Exception("Del错误");
                    }
                }
            }
        }
    }
}
