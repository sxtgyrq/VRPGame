﻿using CommonClass;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace DalOfAddress
{
    public class backgroundjpg
    {
        public static void Insert(MapEditor.SetBackgroundScene sbs)
        {
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        Clear(con, tran, sbs.crossID);
                        bgjpgfiletext.Clear(con, tran, sbs.crossID);
                        {
                            string sQL = @"INSERT INTO backgroundjpg(crossID,
author,
crossState,
crossName,
createTime

) VALUES (@crossID,
@author,
@crossState,
@crossName,
@createTime)";
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                int crossState = 0;
                                string crossName = "";
                                DateTime createTime = DateTime.Now;
                                command.Parameters.AddWithValue("@crossID", sbs.crossID);
                                command.Parameters.AddWithValue("@author", sbs.author);
                                command.Parameters.AddWithValue("@crossState", crossState);
                                command.Parameters.AddWithValue("@crossName", crossName);
                                command.Parameters.AddWithValue("@createTime", createTime);
                                command.ExecuteNonQuery();
                            }
                        }
                        bgjpgfiletext.Insert(con, tran, sbs.crossID, sbs.nx, "nx");
                        bgjpgfiletext.Insert(con, tran, sbs.crossID, sbs.px, "px");
                        bgjpgfiletext.Insert(con, tran, sbs.crossID, sbs.ny, "ny");
                        bgjpgfiletext.Insert(con, tran, sbs.crossID, sbs.py, "py");
                        bgjpgfiletext.Insert(con, tran, sbs.crossID, sbs.nz, "nz");
                        bgjpgfiletext.Insert(con, tran, sbs.crossID, sbs.pz, "pz");
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

        private static void Clear(MySqlConnection con, MySqlTransaction tran, string crossID)
        {
            string sQL = @"DELETE FROM backgroundjpg WHERE crossID=@crossID;";
            // long moneycount;
            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
            {
                command.Parameters.AddWithValue("@crossID", crossID);
                command.ExecuteNonQuery();
            }
            //   bgjpgfiletext.Clear(con, tran, crossID);
        }

        public static MapEditor.GetBackgroundScene.Result Get(string crossID)
        {
            MapEditor.GetBackgroundScene.Result r = new MapEditor.GetBackgroundScene.Result();
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    {
                        var sQL = @"SELECT crossID,author,crossState,crossName,createTime FROM backgroundjpg WHERE crossID=@crossID";
                        using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                        {
                            command.Parameters.AddWithValue("@crossID", crossID);
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    r.hasValue = true;
                                    r.crossState = Convert.ToInt32(reader["crossState"]);

                                }
                                else
                                {
                                    r.hasValue = false;
                                }
                            }
                            if (r.hasValue)
                            {
                                r.px = DalOfAddress.bgjpgfiletext.Get(con, tran, crossID, "px");
                                r.py = DalOfAddress.bgjpgfiletext.Get(con, tran, crossID, "py");
                                r.pz = DalOfAddress.bgjpgfiletext.Get(con, tran, crossID, "pz");
                                r.nx = DalOfAddress.bgjpgfiletext.Get(con, tran, crossID, "nx");
                                r.ny = DalOfAddress.bgjpgfiletext.Get(con, tran, crossID, "ny");
                                r.nz = DalOfAddress.bgjpgfiletext.Get(con, tran, crossID, "nz");
                            }
                        }
                    }
                }
            }
            return r;
        }

        public static void SetUse(MapEditor.UseBackgroundScene sbs)
        {
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    int crossState;
                    if (sbs.used)
                        crossState = 1;
                    else
                        crossState = 0;
                    string sQL = @"UPDATE backgroundjpg SET crossState=@crossState WHERE crossID=@crossID;";
                    // long moneycount;
                    using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                    {
                        command.Parameters.AddWithValue("@crossState", crossState);
                        command.Parameters.AddWithValue("@crossID", sbs.crossID);
                        command.ExecuteNonQuery();
                    }
                    tran.Commit();
                }
            }

        }



        public static Dictionary<string, Dictionary<string, string>> GetAll()
        {
            Dictionary<string, Dictionary<string, string>> allData = new Dictionary<string, Dictionary<string, string>>();
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        {
                            string sQL = @"SELECT a.crossID,jpgTextValue,direction FROM backgroundjpg a LEFT JOIN bgjpgfiletext b on a.crossID=b.crossID WHERE a.crossState=1 ORDER BY a.crossID asc,direction ASC,textIndex asc;";
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                using (var reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        var crossID = Convert.ToString(reader["crossID"]).Trim();
                                        var jpgTextValue = Convert.ToString(reader["jpgTextValue"]).Trim();
                                        var direction = Convert.ToString(reader["direction"]).Trim();

                                        if (allData.ContainsKey(crossID))
                                        { }
                                        else
                                        {
                                            allData.Add(crossID, new Dictionary<string, string>());
                                        }
                                        if (allData[crossID].ContainsKey(direction))
                                        {
                                            allData[crossID][direction] += jpgTextValue;
                                        }
                                        else
                                        {
                                            allData[crossID].Add(direction, jpgTextValue);
                                        }
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
            return allData;
        }
    }
}
