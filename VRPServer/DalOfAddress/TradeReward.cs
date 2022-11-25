using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace DalOfAddress
{
    public class TradeReward
    {
        //public static void Add(string addrBussiness, string StartDate, string TradeAddress, long passCoin)
        //{
        //    //string mysql = "INSERT INTO tradereward(bussinessAddr,StartDate,TradeAddress,passCoin,waitingForAddition)VALUES(@bussinessAddr,@StartDate,@TradeAddress,@passCoin,1);";
        //    //// throw new NotImplementedException();

        //    ////    var mysql = "INSERT INTO traderecord(msg,sign,bussinessAddr,tradeIndex,addrFrom) VALUES(@msg,@sign,@bussinessAddr,@tradeIndex,@addrFrom);";

        //    //using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
        //    //{
        //    //    con.Open();
        //    //    using (MySqlTransaction tran = con.BeginTransaction())
        //    //    {
        //    //        try
        //    //        {
        //    //            {
        //    //                string sQL = mysql;
        //    //                // long moneycount;
        //    //                using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
        //    //                {
        //    //                    command.Parameters.AddWithValue("@bussinessAddr", addrBussiness);
        //    //                    command.Parameters.AddWithValue("@StartDate", StartDate);
        //    //                    command.Parameters.AddWithValue("@TradeAddress", TradeAddress);
        //    //                    command.Parameters.AddWithValue("@passCoin", passCoin);
        //    //                    command.ExecuteNonQuery();
        //    //                }
        //    //            }
        //    //            tran.Commit();
        //    //        }
        //    //        catch (Exception e)
        //    //        {
        //    //            throw e;
        //    //            throw new Exception("新增错误");
        //    //        }
        //    //    }
        //    //}
        //}

        internal static int CheckOccupied(MySqlTransaction tran, MySqlConnection con, string addrBussiness, string addrFrom)
        {
            // return 1;
            int resultFound;
            string sQL = "SELECT tradeIndex FROM tradereward WHERE bussinessAddr=@bussinessAddr AND TradeAddress=@TradeAddress AND waitingForAddition=1;";
            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
            {
                command.Parameters.AddWithValue("@bussinessAddr", addrBussiness);
                command.Parameters.AddWithValue("@TradeAddress", addrFrom);
                var result = command.ExecuteScalar();
                if (result == null)
                    resultFound = -1;
                else
                    resultFound = Convert.ToInt32(command.ExecuteScalar());
            }
            return resultFound;
            //return resultFound;
        }

        public static string Update(out bool success, int startDate, int tradeIndex, string tradeAddress, string bussinessAddr, long passCoin, string signOfTradeAddress, string signOfBussinessAddr, string orderMessage)
        {
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        bool indexIsRight;
                        {
                            string sQL = "SELECT COUNT(*) FROM traderecord WHERE bussinessAddr=@bussinessAddr AND addrFrom=@addrFrom";
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@bussinessAddr", bussinessAddr);
                                command.Parameters.AddWithValue("@addrFrom", tradeAddress);
                                indexIsRight = tradeIndex == Convert.ToInt32(command.ExecuteScalar());
                            }
                        }
                        if (indexIsRight)
                        {
                            int itemCount;
                            {
                                var sql = $"SELECT COUNT(*) FROM tradereward WHERE StartDate={startDate}";
                                using (MySqlCommand command = new MySqlCommand(sql, con, tran))
                                {
                                    itemCount = Convert.ToInt32(command.ExecuteScalar());
                                }

                            }
                            if (itemCount > 0)
                            {
                                tran.Rollback();
                                success = false;
                                return $"{startDate}已经再数据库中拥有数据了！";
                            }
                            else
                            {
                                string mysql = @"INSERT INTO tradereward(
startDate,
tradeIndex,
tradeAddress,
bussinessAddr,
passCoin, 
signOfTradeAddress,
signOfBussinessAddr,
orderMessage,
waitingForAddition) VALUES (@startDate,@tradeIndex,@tradeAddress,@bussinessAddr,@passCoin,@signOfTradeAddress,@signOfBussinessAddr,@orderMessage,1);";

                                string sQL = mysql;
                                // long moneycount;
                                using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                                {
                                    command.Parameters.AddWithValue("@startDate", startDate);
                                    command.Parameters.AddWithValue("@tradeIndex", tradeIndex);
                                    command.Parameters.AddWithValue("@tradeAddress", tradeAddress);
                                    command.Parameters.AddWithValue("@bussinessAddr", bussinessAddr);
                                    command.Parameters.AddWithValue("@passCoin", passCoin);
                                    command.Parameters.AddWithValue("@signOfTradeAddress", signOfTradeAddress);
                                    command.Parameters.AddWithValue("@signOfBussinessAddr", signOfBussinessAddr);
                                    command.Parameters.AddWithValue("@orderMessage", orderMessage);
                                    command.ExecuteNonQuery();
                                }
                                tran.Commit();
                                success = true;
                                return $"{startDate}录入数据成功！";
                            }
                        }
                        else
                        {
                            success = false;
                            return "序号错误";
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

        public static void RemoveByBussinessAddr(string bussinessAddr)
        {
            string sQL = "DELETE FROM tradereward WHERE bussinessAddr=@bussinessAddr";
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                    {
                        command.Parameters.AddWithValue("@bussinessAddr", bussinessAddr);
                        command.ExecuteNonQuery();
                    }
                    tran.Commit();
                }
            }
        }

        public static CommonClass.databaseModel.tradereward GetByStartDate(int startDate)
        {
            CommonClass.databaseModel.tradereward tw;
            var sQL = $"SELECT startDate,tradeIndex,tradeAddress,bussinessAddr,passCoin,waitingForAddition,signOfTradeAddress,signOfBussinessAddr,orderMessage FROM  tradereward WHERE startDate={startDate};";

            using (var r = MySqlHelper.ExecuteReader(Connection.ConnectionStr, sQL))
            {
                if (r.Read())
                {
                    tw = new CommonClass.databaseModel.tradereward()
                    {
                        bussinessAddr = Convert.ToString(r["bussinessAddr"]).Trim(),
                        orderMessage = Convert.ToString(r["orderMessage"]).Trim(),
                        passCoin = Convert.ToInt32(r["passCoin"]),
                        signOfBussinessAddr = Convert.ToString(r["signOfBussinessAddr"]).Trim(),
                        signOfTradeAddress = Convert.ToString(r["signOfTradeAddress"]).Trim(),
                        startDate = Convert.ToInt32(r["startDate"]),
                        tradeAddress = Convert.ToString(r["tradeAddress"]).Trim(),
                        tradeIndex = Convert.ToInt32(r["tradeIndex"]),
                        waitingForAddition = Convert.ToInt32(r["waitingForAddition"]),

                    };
                }
                else
                {
                    tw = null;
                }
            }
            return tw;
            //  throw new NotImplementedException();
        }
    }
}
