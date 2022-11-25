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
            string msg;
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        bool hasValue;
                        bool addrIsTheCurrent = false;
                        // long moneycount;
                        {
                            string sQL = @"SELECT
                            	bindWordMsg,bindWordAddr 
                            FROM
                            	bindwordinfo WHERE bindWordMsg=@bindWordMsg";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@bindWordMsg", bindWordMsg);

                                using (var reader = command.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        addrIsTheCurrent = Convert.ToString(reader["bindWordAddr"]).Trim() == bindWordAddr.Trim();
                                        hasValue = true;
                                    }
                                    else
                                    {
                                        hasValue = false;
                                    }
                                }
                            }
                        }
                        if (!hasValue)
                        {

                            DateTime endTime = DateTime.Now.AddMonths(6);
                            string sQL = @"INSERT INTO bindwordinfo(bindWordAddr,bindWordMsg,bindWordSign,endTime)VALUES(@bindWordAddr,@bindWordMsg,@bindWordSign,@endTime);";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@bindWordAddr", bindWordAddr);
                                command.Parameters.AddWithValue("@bindWordMsg", bindWordMsg);
                                command.Parameters.AddWithValue("@bindWordSign", bindWordSign);
                                command.Parameters.AddWithValue("@endTime", endTime);
                                command.ExecuteNonQuery();
                            }
                            msg = $"【{bindWordAddr}】与【{bindWordMsg}】绑定成功，有效期至{endTime.ToString("yyyy年MM月dd日")}";
                            success = true;
                        }
                        else if (addrIsTheCurrent)
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
                        }
                        else
                        {
                            msg = $"【{bindWordMsg}】与其他地址绑定。请更换绑定词。";
                            success = false;
                        }
                        {
                            DateTime endTime = DateTime.Now;
                            string sQL = $"DELETE FROM bindwordinfo WHERE endTime<'{endTime.ToString("yyyy-MM-dd HH:mm:ss")}';";
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
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
            return msg;
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
                    return $"{bindWordAddr}与{bindWordMsg}绑定，有效期至{endTime.ToString("yyyy年MM月dd日")}";
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
                    return $"{bindWordAddr}与{bindWordMsg}绑定，有效期至{endTime.ToString("yyyy年MM月dd日")}";
                }
            }
            return $"没有查询到【{bindWordAddr}】的绑定关系";
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
    }
}
