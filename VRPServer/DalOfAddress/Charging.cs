using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace DalOfAddress
{
    public class Charging
    {
        public static int AddItem(CommonClass.Finance.Charging chargingObj)
        {
            int chargingOrder;
            string sQL = $@"INSERT INTO charging(chargingword,
chargingDatetime,
chargingMoney,
chargingType,
charginginfo,chargingAddr,chargingIsUsed) VALUES('{chargingObj.ChargingWord}','{chargingObj.ChargingDt}',{chargingObj.ChargingNum},'{chargingObj.ChargingType}','','{chargingObj.ChargingAddr}',0)";
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        {
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.ExecuteNonQuery();
                            }
                            BindWordInfo.Update(chargingObj.ChargingWord, tran, con);

                            sQL = $"SELECT chargingOrder FROM charging WHERE chargingType='{chargingObj.ChargingDt}';";
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                chargingOrder = Convert.ToInt32(command.ExecuteScalar());
                            }
                        }
                        tran.Commit();
                    }
                    catch (Exception e)
                    {
                        tran.Rollback();
                        throw e;
                        throw new Exception("新增错误");
                    }
                }
            }
            return chargingOrder;
        }

        public static CommonClass.Finance.ChargingLookFor.Result GetItem(int chargingOrder)
        {
            // int chargingOrder;
            string sQL = $@"SELECT
	B.bindWordAddr,
	A.chargingOrder,
	A.chargingword,
	A.chargingDatetime,
	A.chargingMoney,
	A.chargingType,
	A.charginginfo,
	A.chargingAddr,
	A.chargingIsUsed 
FROM
	charging AS A
	LEFT JOIN bindwordinfo AS B ON A.chargingword = B.bindWordMsg 
WHERE A.chargingOrder ={chargingOrder};";
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        var dtRow = MySqlHelper.ExecuteDataRow(Connection.ConnectionStr, sQL);
                        if (dtRow == null)
                        {
                            return null;
                        }
                        else
                        {
                            CommonClass.Finance.ChargingLookFor.Result r = new CommonClass.Finance.ChargingLookFor.Result()
                            {
                                bindWordAddr = dtRow.IsNull("bindWordAddr") ? "" : Convert.ToString(dtRow["bindWordAddr"]).Trim(),
                                chargingAddr = dtRow.IsNull("chargingAddr") ? "" : Convert.ToString(dtRow["chargingAddr"]).Trim(),
                                chargingDatetime = dtRow.IsNull("chargingDatetime") ? "" : Convert.ToDateTime(dtRow["chargingDatetime"]).ToString("yyyy-MM-dd HH:mm:ss"),
                                charginginfo = dtRow.IsNull("charginginfo") ? "" : Convert.ToString(dtRow["charginginfo"]).Trim(),
                                chargingIsUsed = dtRow.IsNull("chargingIsUsed") ? "" : Convert.ToString(dtRow["chargingIsUsed"]).Trim(),
                                chargingMoney = dtRow.IsNull("chargingMoney") ? "" : Convert.ToDecimal(dtRow["chargingMoney"]).ToString("#0.00"),
                                chargingOrder = Convert.ToInt32(dtRow["chargingOrder"]),
                                chargingType = dtRow.IsNull("chargingType") ? "" : Convert.ToString(dtRow["chargingType"]).Trim(),
                                chargingword = dtRow.IsNull("chargingword") ? "" : Convert.ToString(dtRow["chargingword"]).Trim(),
                            };
                            return r;
                        }
                    }
                    catch (Exception e)
                    {
                        tran.Rollback();
                        throw e;
                        throw new Exception("新增错误");
                    }
                }
            }
        }


        public static int GetMaxIndexNum()
        {
            string sQL = $@"SELECT MAX(chargingOrder) FROM charging;";
            var result = MySqlHelper.ExecuteScalar(Connection.ConnectionStr, sQL);
            if (result == null || result == DBNull.Value)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(result);
            }
        }
    }
}
