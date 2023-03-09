using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DalOfAddress
{
    public class BindWordInfo
    {
        public static string Add(string bindWordSign, string bindWordMsg, string bindWordAddr, out bool success)
        {
            {
                /*
                 * A.用地址(bindWordAddr)，获取绑定词bindWordInDB
                 * B.用绑定词(bindWordMsg),获取地址bindAddrInDB
                 * 若A的结果不为空，判断数据存储是否与正要绑定的绑定词一致。若一致，更新日期。若不一致，提示。
                 * 若A为空，B不为空，提示，该绑定词已经与其他地址绑定了。
                 * 若A、B运行的结果均为空,执行插入。
                 * 
                 */
                string msg;
                using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
                {


                    con.Open();
                    using (MySqlTransaction tran = con.BeginTransaction())
                    {
                        try
                        {
                            var bindWordInDB = GetWordByAddr(bindWordAddr, tran, con);
                            var bindAddrInDB = GetAddrByWord(bindWordMsg, tran, con);

                            if (!string.IsNullOrEmpty(bindWordInDB))
                            {
                                if (bindWordInDB == bindWordMsg)
                                {
                                    DateTime endTime = DateTime.Now.AddMonths(6);
                                    string sQL = @"UPDATE bindwordinfo SET 
bindWordSign=@bindWordSign,
endTime=@endTime 
WHERE bindWordMsg=@bindWordMsg AND bindWordAddr=@bindWordAddr;";
                                    using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                                    {
                                        command.Parameters.AddWithValue("@bindWordSign", bindWordSign);
                                        command.Parameters.AddWithValue("@endTime", endTime);
                                        command.Parameters.AddWithValue("@bindWordMsg", bindWordMsg);
                                        command.Parameters.AddWithValue("@bindWordAddr", bindWordAddr);
                                        command.ExecuteNonQuery();
                                    }
                                    msg = $"【{bindWordAddr}】与【{bindWordMsg}】绑定成功，有效期至{endTime.ToString("yyyy年MM月dd日")}";
                                    success = true;
                                    tran.Commit();
                                }
                                else
                                {
                                    msg = $"【{bindWordAddr}】已经与“{bindWordInDB}”" + Environment.NewLine +
                                        $"绑定！";
                                    success = false;
                                }
                            }
                            else if (!string.IsNullOrEmpty(bindAddrInDB))
                            {
                                success = false;
                                msg = $"【{bindWordMsg}】已经与【{bindAddrInDB}】" + Environment.NewLine +
                                       $"绑定！" + Environment.NewLine +
                                      $"请您更换绑定词";
                                success = false;
                            }
                            else
                            {
                                DateTime endTime = DateTime.Now.AddMonths(6);
                                string sQL = @"INSERT INTO bindwordinfo(bindWordAddr,bindWordMsg,bindWordSign,endTime)VALUES(@bindWordAddr,@bindWordMsg,@bindWordSign,@endTime);";
                                bool InserSuccess;
                                using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                                {
                                    command.Parameters.AddWithValue("@bindWordAddr", bindWordAddr);
                                    command.Parameters.AddWithValue("@bindWordMsg", bindWordMsg);
                                    command.Parameters.AddWithValue("@bindWordSign", bindWordSign);
                                    command.Parameters.AddWithValue("@endTime", endTime);
                                    InserSuccess = command.ExecuteNonQuery() > 0;
                                }
                                if (InserSuccess)
                                {
                                    msg = $"【{bindWordAddr}】与【{bindWordMsg}】绑定成功，有效期至{endTime.ToString("yyyy年MM月dd日")}";
                                    success = true;
                                    tran.Commit();
                                }
                                else
                                {
                                    msg = "系统错误";
                                    success = false;
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
                return msg;
            }
        }



        internal static void Update(string bindWordMsg, MySqlTransaction tran, MySqlConnection con)
        {
            DateTime endTime = DateTime.Now.AddMonths(6);
            string sQL = @"UPDATE bindwordinfo SET 
endTime=@endTime 
WHERE bindWordMsg=@bindWordMsg;";
            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
            {
                command.Parameters.AddWithValue("@endTime", endTime);
                command.Parameters.AddWithValue("@bindWordMsg", bindWordMsg);
                command.ExecuteNonQuery();
            }
        }



        public static string LookForByWord(string bindWordMsg)
        {
            string sQL = $@"SELECT bindWordAddr,bindWordMsg,bindWordSign,endTime from bindwordinfo 
 WHERE bindWordMsg='{bindWordMsg}';";
            var ds = MySqlHelper.ExecuteDataset(Connection.ConnectionStr, sQL);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var bindWordAddr = Convert.ToString(ds.Tables[0].Rows[0]["bindWordAddr"]).Trim();
                    var bindWordSign = Convert.ToString(ds.Tables[0].Rows[0]["bindWordSign"]).Trim();
                    var endTime = Convert.ToDateTime(ds.Tables[0].Rows[0]["endTime"]);
                    return $"【{bindWordAddr}】与“{bindWordMsg}”绑定，有效期至{endTime.ToString("yyyy年MM月dd日")}";
                }
            }
            return $"没有查询到【{bindWordMsg}】的绑定关系";
        }


        public static string LookForByAddr(string bindWordAddr)
        {
            string sQL = $@"SELECT bindWordAddr,bindWordMsg,bindWordSign,endTime from bindwordinfo 
 WHERE bindWordAddr='{bindWordAddr}';";
            var ds = MySqlHelper.ExecuteDataset(Connection.ConnectionStr, sQL);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var bindWordMsg = Convert.ToString(ds.Tables[0].Rows[0]["bindWordMsg"]).Trim();
                    var bindWordSign = Convert.ToString(ds.Tables[0].Rows[0]["bindWordSign"]).Trim();
                    var endTime = Convert.ToDateTime(ds.Tables[0].Rows[0]["endTime"]);
                    return $"【{bindWordAddr}】与“{bindWordMsg}”绑定，有效期至{endTime.ToString("yyyy年MM月dd日")}";
                }
            }
            return $"没有查询到【{bindWordAddr}】的绑定关系";
        }

        static string LookForByAddr2(string bindWordAddr)
        {
            string sQL = $@"SELECT bindWordAddr,bindWordMsg,bindWordSign,endTime from bindwordinfo 
 WHERE bindWordAddr='{bindWordAddr}';";
            var ds = MySqlHelper.ExecuteDataset(Connection.ConnectionStr, sQL);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var bindWordMsg = Convert.ToString(ds.Tables[0].Rows[0]["bindWordMsg"]).Trim();
                    return $"{bindWordAddr}已与{bindWordMsg}绑定。";
                }
            }
            return "";
        }

        public static string GetAddrByWord(string words)
        {
            Regex reg = new System.Text.RegularExpressions.Regex("^[\u4e00-\u9fa5]{2,10}$");
            if (reg.IsMatch(words))
            {
                string sQL = $@"SELECT bindWordAddr,bindWordMsg,bindWordSign,endTime from bindwordinfo 
 WHERE bindWordMsg='{words}';";
                var ds = MySqlHelper.ExecuteDataset(Connection.ConnectionStr, sQL);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        var bindWordAddr = Convert.ToString(ds.Tables[0].Rows[0]["bindWordAddr"]).Trim();
                        return bindWordAddr;
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }
            }
            else return "";
        }
        public static string GetWordByAddr(string bindWordAddr)
        {
            string sQL = $@"SELECT bindWordAddr,bindWordMsg,bindWordSign,endTime from bindwordinfo 
 WHERE bindWordAddr='{bindWordAddr}';";
            var ds = MySqlHelper.ExecuteDataset(Connection.ConnectionStr, sQL);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var bindWordMsg = Convert.ToString(ds.Tables[0].Rows[0]["bindWordMsg"]).Trim();
                    var bindWordSign = Convert.ToString(ds.Tables[0].Rows[0]["bindWordSign"]).Trim();
                    var endTime = Convert.ToDateTime(ds.Tables[0].Rows[0]["endTime"]);
                    return bindWordMsg;
                }
            }
            return "";
        }

        /// <summary>
        /// 通过绑定词，获取地址
        /// </summary>
        /// <param name="words">绑定词</param>
        /// <param name="tran">数据库处理</param>
        /// <param name="con">数据库链接</param>
        /// <returns>如果为空字符串，则说明绑定词可用！如果不为空，返回该绑定词语对应的比特币地址！</returns>
        private static string GetWordByAddr(string addr, MySqlTransaction tran, MySqlConnection con)
        {
            string sQL = $@"SELECT bindWordAddr,bindWordMsg,bindWordSign,endTime from bindwordinfo 
 WHERE bindWordAddr='{addr}';";
            using (var command = new MySqlCommand(sQL, con, tran))
            {
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return Convert.ToString(reader["bindWordMsg"]).Trim();
                    }
                }
            }
            return "";
            //throw new NotImplementedException();
        }

        private static string GetAddrByWord(string words, MySqlTransaction tran, MySqlConnection con)
        {
            string sQL = $@"SELECT bindWordAddr,bindWordMsg,bindWordSign,endTime from bindwordinfo 
 WHERE bindWordMsg='{words}';";
            using (var command = new MySqlCommand(sQL, con, tran))
            {
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return Convert.ToString(reader["bindWordAddr"]).Trim();
                    }
                }
            }
            return "";
        }
    }
}
