using System;
using System.Collections.Generic;
using System.Text;

namespace CommonClass.databaseModel
{
    public class LevelForSave
    {
        public string BtcAddr { get; set; }

        /// <summary>
        /// 如果此字符串为空字符串，要执行Insert Sql语句。不为空要执行Update Sql语句
        /// </summary>
        public string TimeStampStr { get; set; }
        public int Level { get; set; }

    }
}
