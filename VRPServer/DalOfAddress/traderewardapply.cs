
//using CityRunDAL;
using CommonClass;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace DalOfAddress
{
    public class traderewardapply
    {
        public enum AddResult
        {
            Success,
            UnknownReasons,
            HaveNotEnoughtSatoshi,
            HsaNotTheStartDate,
            LevelIsLow,
            IsFullInTheLevel
        }
        public static AddResult Add(CommonClass.ModelTranstraction.RewardApply apply, out int levle)
        {
            // return 0;
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        levle = LevelForSave.GetLevel(tran, con, apply.addr);
                        if (levle > 1)
                        {
                            int count;
                            {
                                var sQL = $"SELECT COUNT(*) FROM traderewardapply WHERE startDate={apply.msgNeedToSign} AND applyLevel={levle}";
                                using (var command = new MySqlCommand(sQL, con))
                                {
                                    var result = command.ExecuteScalar();
                                    if (result == null || result == DBNull.Value)
                                    {
                                        count = 0;
                                    }
                                    else
                                        count = Convert.ToInt32(result);
                                }
                            }
                            if (count >= levle - 1)
                            {
                                tran.Rollback();
                                return AddResult.IsFullInTheLevel;
                            }
                            else
                            {
                                var reward = TradeReward.GetByStartDate(Convert.ToInt32(apply.msgNeedToSign));
                                if (reward == null)
                                {
                                    tran.Rollback();
                                    return AddResult.HsaNotTheStartDate;
                                }
                                else
                                {
                                    int minNeedSatoshi;
                                    {
                                        var sQL = $"SELECT SUM(applyLevel) FROM traderewardapply WHERE startDate={apply.msgNeedToSign};";
                                        using (var command = new MySqlCommand(sQL, con))
                                        {
                                            var result = command.ExecuteScalar();
                                            if (result == null || result == DBNull.Value)
                                            {
                                                minNeedSatoshi = levle;
                                            }
                                            else
                                                minNeedSatoshi = Convert.ToInt32(result) + levle;
                                        }
                                    }
                                    if (minNeedSatoshi > reward.passCoin)
                                    {
                                        tran.Rollback();
                                        return AddResult.HaveNotEnoughtSatoshi;
                                    }
                                    else
                                    {
                                        int row = LevelForSave.RemoveLevelWithAddr(con, tran, apply.addr, levle);
                                        if (row == 1)
                                        {
                                            var sQL = "INSERT INTO traderewardapply(startDate,applyAddr,applyLevel,applySign) VALUES (@startDate,@applyAddr,@applyLevel,@applySign)";
                                            using (var command = new MySqlCommand(sQL, con))
                                            {
                                                command.Parameters.AddWithValue("@startDate", Convert.ToInt32(apply.msgNeedToSign));
                                                command.Parameters.AddWithValue("@applyAddr", apply.addr);
                                                command.Parameters.AddWithValue("@applyLevel", levle);
                                                command.Parameters.AddWithValue("@applySign", apply.signature);
                                                row = command.ExecuteNonQuery();
                                            }
                                            if (row == 1)
                                            {
                                                tran.Commit();
                                                return AddResult.Success;
                                            }
                                            else
                                            {
                                                tran.Rollback();
                                                return AddResult.UnknownReasons;
                                            }
                                        }
                                        else
                                        {
                                            tran.Rollback();
                                            return AddResult.UnknownReasons;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            tran.Rollback();
                            return AddResult.LevelIsLow;
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                        throw new Exception("新增错误");
                    }
                }
            }
            //if (apply.applyLevel > 1) { }
            //else
            //{
            //    // return 0;
            //}
            // var sQL = "INSERT INTO traderewardapply(startDate,applyAddr,applyLevel,applySign) VALUES (startDate,applyAddr,applyLevel,applySign);";
            //List<CommonClass.databaseModel.traderewardapply> list = new List<CommonClass.databaseModel.traderewardapply>();
            //var sQL = $"SELECT rankIndex,startDate,applyAddr,applyLevel,applySign FROM traderewardapply WHERE startDate={startDate};";

            //using (var r = MySqlHelper.ExecuteReader(Connection.ConnectionStr, sQL))
            //{
            //    while (r.Read())
            //    {
            //        var apply = new CommonClass.databaseModel.traderewardapply()
            //        {
            //            applyAddr = Convert.ToString(r["applyAddr"]).Trim(),
            //            applyLevel = Convert.ToInt32(r["applyLevel"]),
            //            applySign = Convert.ToString(r["applySign"]).Trim(),
            //            rankIndex = Convert.ToInt32(r["rankIndex"]),
            //            startDate = Convert.ToInt32(r["startDate"]),
            //        };
            //        list.Add(apply);
            //    }
            //}
            //return list;
        }

        internal static int Count(MySqlConnection con, MySqlTransaction tran, int startDate)
        {
            int count = 0;
            var sQL = $"SELECT rankIndex,startDate,applyAddr,applyLevel,applySign FROM traderewardapply WHERE startDate={startDate};";
            using (var command = new MySqlCommand(sQL, con, tran))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read()) { count++; }
                }
            }
            return count;
        }


        public static List<CommonClass.databaseModel.traderewardapply> GetByStartDate(int startDate)
        {
            List<CommonClass.databaseModel.traderewardapply> list = new List<CommonClass.databaseModel.traderewardapply>();
            var sQL = $"SELECT rankIndex,startDate,applyAddr,applyLevel,applySign FROM traderewardapply WHERE startDate={startDate};";

            using (var r = MySqlHelper.ExecuteReader(Connection.ConnectionStr, sQL))
            {
                while (r.Read())
                {
                    var apply = new CommonClass.databaseModel.traderewardapply()
                    {
                        applyAddr = Convert.ToString(r["applyAddr"]).Trim(),
                        applyLevel = Convert.ToInt32(r["applyLevel"]),
                        applySign = Convert.ToString(r["applySign"]).Trim(),
                        rankIndex = Convert.ToInt32(r["rankIndex"]),
                        startDate = Convert.ToInt32(r["startDate"]),
                    };
                    list.Add(apply);
                }
            }
            return list;
        }

        //public static void Give(ModelTranstraction.AwardsGiving aG)
        //{
        //    throw new NotImplementedException();
        //}

        internal static int UpdateItem(MySqlConnection con, MySqlTransaction tran, int startDate, int rankIndex, string applyAddr, int rewardGiven)
        {
            int rowAffected;
            string sQL = $"UPDATE traderewardapply SET rewardGiven={rewardGiven} WHERE rankIndex={rankIndex} AND startDate={startDate} AND applyAddr='{applyAddr}' AND rewardGiven=0;";
            using (var command = new MySqlCommand(sQL, con, tran))
            {
                rowAffected = command.ExecuteNonQuery();
            }
            return rowAffected;
            //throw new NotImplementedException();
        }

        internal static bool HasAddrGetNoReward(MySqlTransaction tran, MySqlConnection con, int startDate)
        {
            bool result;
            string sQL = $"SELECT * FROM traderewardapply WHERE startDate={startDate} AND rewardGiven=0;";
            using (var command = new MySqlCommand(sQL, con, tran))
            {
                using (var reader = command.ExecuteReader())
                {
                    result = reader.Read();
                }
            }
            return result;
        }
    }
}
