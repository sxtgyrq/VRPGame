using CommonClass.databaseModel;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace DalOfAddress
{
    public class MoneyOfCustomerExtracted
    {
        internal static int Add(MySqlConnection con, MySqlTransaction tran, moneyofcustomerextractedM model)
        {
            int row;
            string sQL = @"INSERT INTO moneyofcustomerextracted ( bussinessAddr, tradeIndex, addrFrom, satoshi, isPayed, recordTime )
VALUES
	(
		@bussinessAddr,
		@tradeIndex,
		@addrFrom,
		@satoshi,
		@isPayed,
		@recordTime)";
            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
            {
                command.Parameters.AddWithValue("@bussinessAddr", model.bussinessAddr);
                command.Parameters.AddWithValue("@tradeIndex", model.tradeIndex);
                command.Parameters.AddWithValue("@addrFrom", model.addrFrom);
                command.Parameters.AddWithValue("@satoshi", model.satoshi);
                command.Parameters.AddWithValue("@isPayed", model.isPayed);
                command.Parameters.AddWithValue("@recordTime", model.recordTime);
                row= command.ExecuteNonQuery();
            }
            return row;
        }
    }
}
