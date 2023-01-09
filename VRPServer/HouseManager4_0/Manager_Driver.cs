using CommonClass;
using CommonClass.driversource;
using HouseManager4_0.interfaceOfEngine;
using HouseManager4_0.RoomMainF;
using Model;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using static HouseManager4_0.Car;
using static HouseManager4_0.RoomMainF.RoomMain;
using OssModel = Model;

namespace HouseManager4_0
{
    public class Manager_Driver : Manager, startNewCommandThread, manager
    {
        public Manager_Driver(RoomMain roomMain)
        {
            this.roomMain = roomMain;

        }

        internal void SelectDriver(SetSelectDriver dm)
        {
            List<string> notifyMsg = new List<string>();
            lock (that.PlayerLock)
            {
                if (that._Players.ContainsKey(dm.Key))
                {
                    var player = that._Players[dm.Key];
                    if (player.Bust) { }
                    else
                    {

                        var car = player.getCar();
                        const int CostMoney = 5000;
                        if (car.state == Car.CarState.waitAtBaseStation)
                            if (car.ability.driver == null)
                            {
                                this.setDriver(player, car, dm.Index, ref notifyMsg);
                            }
                            else if (car.ability.driver.Index == dm.Index)
                            {
                                this.WebNotify(player, $"你现在的司机就是{car.ability.driver.Name}.");
                            }
                            else if (player.Money > CostMoney)
                            {
                                player.MoneySet(player.Money - CostMoney, ref notifyMsg);
                                //var recruit = player.buildingReward[0];
                                if (that.rm.Next(100) < Manager_Driver.GetRecruit(player))
                                {
                                    this.setDriver(player, car, dm.Index, ref notifyMsg);
                                    this.WebNotify(player, "招聘成功！");
                                }
                                else
                                {
                                    this.WebNotify(player, "招聘失败！到指定地点祈更多福可以提高成功率");
                                }
                            }
                            else
                            {
                                this.WebNotify(player, "换司机最少要消耗50.00点积分！");
                            }

                    }
                }
            }
            this.sendSeveralMsgs(notifyMsg); 
        }



        private void setDriver(RoleInGame player, Car car, int index, ref List<string> notifyMsg)
        {
            switch (index)
            {
                case 510: { var name = "孙策"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.man, CommonClass.driversource.Race.devil, name); driverSet.DefensiveInitialize(25, 25, 25, 25, 25, 25, 70); driverSet.GrouthSet(86, 120, 126); driverSet.Index = 510; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 501: { var name = "明世隐"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.man, CommonClass.driversource.Race.devil, name); driverSet.DefensiveInitialize(25, 25, 25, 25, 25, 25, 70); driverSet.GrouthSet(106, 120, 106); driverSet.Index = 501; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 196: { var name = "百里约"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.man, CommonClass.driversource.Race.devil, name); driverSet.DefensiveInitialize(25, 25, 25, 45, 45, 25, 40); driverSet.GrouthSet(86, 110, 106); driverSet.Index = 196; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 180: { var name = "哪吒"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.man, CommonClass.driversource.Race.devil, name); driverSet.DefensiveInitialize(25, 25, 25, 25, 45, 45, 40); driverSet.GrouthSet(86, 110, 106); driverSet.Index = 180; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 173: { var name = "李元芳"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.man, CommonClass.driversource.Race.devil, name); driverSet.DefensiveInitialize(49, 49, 25, 25, 25, 25, 40); driverSet.GrouthSet(86, 110, 106); driverSet.Index = 173; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 170: { var name = "刘备"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.man, CommonClass.driversource.Race.devil, name); driverSet.DefensiveInitialize(25, 49, 49, 25, 25, 25, 40); driverSet.GrouthSet(86, 110, 106); driverSet.Index = 170; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 169: { var name = "后羿"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.man, CommonClass.driversource.Race.devil, name); driverSet.DefensiveInitialize(25, 25, 25, 35, 45, 35, 40); driverSet.GrouthSet(86, 110, 106); driverSet.Index = 169; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 166: { var name = "亚瑟"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.man, CommonClass.driversource.Race.devil, name); driverSet.DefensiveInitialize(25, 25, 25, 25, 25, 25, 70); driverSet.GrouthSet(96, 120, 116); driverSet.Index = 166; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 156: { var name = "张良"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.man, CommonClass.driversource.Race.devil, name); driverSet.DefensiveInitialize(25, 25, 25, 35, 35, 25, 55); driverSet.GrouthSet(86, 115, 116); driverSet.Index = 156; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 118: { var name = "孙膑"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.man, CommonClass.driversource.Race.devil, name); driverSet.DefensiveInitialize(25, 25, 25, 25, 35, 35, 55); driverSet.GrouthSet(86, 115, 116); driverSet.Index = 118; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 114: { var name = "刘禅"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.man, CommonClass.driversource.Race.devil, name); driverSet.DefensiveInitialize(37, 49, 37, 25, 25, 25, 40); driverSet.GrouthSet(86, 110, 106); driverSet.Index = 114; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 105: { var name = "廉颇"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.man, CommonClass.driversource.Race.devil, name); driverSet.DefensiveInitialize(25, 37, 37, 25, 35, 35, 40); driverSet.GrouthSet(86, 110, 106); driverSet.Index = 105; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 155: { var name = "艾琳"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.woman, CommonClass.driversource.Race.devil, name); driverSet.DefensiveInitialize(25, 25, 25, 25, 25, 25, 70); driverSet.GrouthSet(86, 120, 126); driverSet.Index = 155; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 533: { var name = "阿古朵"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.woman, CommonClass.driversource.Race.devil, name); driverSet.DefensiveInitialize(25, 25, 25, 25, 25, 25, 70); driverSet.GrouthSet(106, 120, 106); driverSet.Index = 533; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 523: { var name = "西施"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.woman, CommonClass.driversource.Race.devil, name); driverSet.DefensiveInitialize(25, 25, 25, 45, 45, 25, 40); driverSet.GrouthSet(86, 110, 106); driverSet.Index = 523; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 505: { var name = "阿瑶"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.woman, CommonClass.driversource.Race.devil, name); driverSet.DefensiveInitialize(25, 25, 25, 25, 45, 45, 40); driverSet.GrouthSet(86, 110, 106); driverSet.Index = 505; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 125: { var name = "元歌"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.woman, CommonClass.driversource.Race.devil, name); driverSet.DefensiveInitialize(49, 49, 25, 25, 25, 25, 40); driverSet.GrouthSet(86, 110, 106); driverSet.Index = 125; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 176: { var name = "杨玉环"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.woman, CommonClass.driversource.Race.devil, name); driverSet.DefensiveInitialize(25, 49, 49, 25, 25, 25, 40); driverSet.GrouthSet(86, 110, 106); driverSet.Index = 176; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 191: { var name = "大乔"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.woman, CommonClass.driversource.Race.devil, name); driverSet.DefensiveInitialize(25, 25, 25, 35, 45, 35, 40); driverSet.GrouthSet(86, 110, 106); driverSet.Index = 191; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 184: { var name = "蔡文姬"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.woman, CommonClass.driversource.Race.devil, name); driverSet.DefensiveInitialize(25, 25, 25, 25, 25, 25, 70); driverSet.GrouthSet(96, 120, 116); driverSet.Index = 184; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 183: { var name = "雅典娜"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.woman, CommonClass.driversource.Race.devil, name); driverSet.DefensiveInitialize(37, 49, 37, 25, 25, 25, 40); driverSet.GrouthSet(86, 110, 106); driverSet.Index = 183; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 121: { var name = "芈月"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.woman, CommonClass.driversource.Race.devil, name); driverSet.DefensiveInitialize(25, 37, 37, 25, 35, 35, 40); driverSet.GrouthSet(86, 110, 106); driverSet.Index = 121; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 116: { var name = "阿轲"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.woman, CommonClass.driversource.Race.devil, name); driverSet.DefensiveInitialize(25, 25, 25, 25, 35, 35, 55); driverSet.GrouthSet(96, 115, 106); driverSet.Index = 116; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 106: { var name = "小乔"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.woman, CommonClass.driversource.Race.devil, name); driverSet.DefensiveInitialize(25, 37, 37, 35, 35, 25, 40); driverSet.GrouthSet(86, 110, 106); driverSet.Index = 106; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 137: { var name = "司马懿"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.man, CommonClass.driversource.Race.people, name); driverSet.DefensiveInitialize(0, 0, 0, 40, 40, 40, 30); driverSet.GrouthSet(105, 116, 110); driverSet.Index = 137; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 190: { var name = "诸葛亮"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.man, CommonClass.driversource.Race.people, name); driverSet.DefensiveInitialize(0, 0, 0, 40, 40, 40, 30); driverSet.GrouthSet(125, 116, 90); driverSet.Index = 190; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 150: { var name = "韩信"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.man, CommonClass.driversource.Race.people, name); driverSet.DefensiveInitialize(0, 0, 0, 60, 60, 40, 0); driverSet.GrouthSet(105, 106, 90); driverSet.Index = 150; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 149: { var name = "刘邦"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.man, CommonClass.driversource.Race.people, name); driverSet.DefensiveInitialize(0, 0, 0, 40, 60, 60, 0); driverSet.GrouthSet(105, 106, 90); driverSet.Index = 149; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 134: { var name = "达摩"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.man, CommonClass.driversource.Race.people, name); driverSet.DefensiveInitialize(24, 24, 0, 40, 40, 40, 0); driverSet.GrouthSet(105, 106, 90); driverSet.Index = 134; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 133: { var name = "狄仁杰"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.man, CommonClass.driversource.Race.people, name); driverSet.DefensiveInitialize(0, 24, 24, 40, 40, 40, 0); driverSet.GrouthSet(105, 106, 90); driverSet.Index = 133; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 132: { var name = "马可波"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.man, CommonClass.driversource.Race.people, name); driverSet.DefensiveInitialize(0, 0, 0, 50, 60, 50, 0); driverSet.GrouthSet(105, 106, 90); driverSet.Index = 132; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 131: { var name = "李白"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.man, CommonClass.driversource.Race.people, name); driverSet.DefensiveInitialize(0, 0, 0, 40, 40, 40, 30); driverSet.GrouthSet(115, 116, 100); driverSet.Index = 131; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 128: { var name = "曹操"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.man, CommonClass.driversource.Race.people, name); driverSet.DefensiveInitialize(12, 24, 12, 40, 40, 40, 0); driverSet.GrouthSet(105, 106, 90); driverSet.Index = 128; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 124: { var name = "周瑜"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.man, CommonClass.driversource.Race.people, name); driverSet.DefensiveInitialize(12, 12, 0, 40, 50, 50, 0); driverSet.GrouthSet(105, 106, 90); driverSet.Index = 124; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 115: { var name = "高渐离"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.man, CommonClass.driversource.Race.people, name); driverSet.DefensiveInitialize(0, 12, 12, 50, 50, 40, 0); driverSet.GrouthSet(105, 106, 90); driverSet.Index = 115; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 110: { var name = "嬴政"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.man, CommonClass.driversource.Race.people, name); driverSet.DefensiveInitialize(12, 12, 0, 50, 50, 40, 0); driverSet.GrouthSet(105, 106, 90); driverSet.Index = 110; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 536: { var name = "夏洛特"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.woman, CommonClass.driversource.Race.people, name); driverSet.DefensiveInitialize(0, 0, 0, 40, 40, 40, 30); driverSet.GrouthSet(105, 116, 110); driverSet.Index = 536; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 506: { var name = "云中君"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.woman, CommonClass.driversource.Race.people, name); driverSet.DefensiveInitialize(0, 0, 0, 40, 40, 40, 30); driverSet.GrouthSet(125, 116, 90); driverSet.Index = 506; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 515: { var name = "嫦娥"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.woman, CommonClass.driversource.Race.people, name); driverSet.DefensiveInitialize(0, 0, 0, 60, 60, 40, 0); driverSet.GrouthSet(105, 106, 90); driverSet.Index = 515; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 513: { var name = "上官婉"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.woman, CommonClass.driversource.Race.people, name); driverSet.DefensiveInitialize(0, 0, 0, 40, 60, 60, 0); driverSet.GrouthSet(105, 106, 90); driverSet.Index = 513; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 312: { var name = "沈梦溪"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.woman, CommonClass.driversource.Race.people, name); driverSet.DefensiveInitialize(24, 24, 0, 40, 40, 40, 0); driverSet.GrouthSet(105, 106, 90); driverSet.Index = 312; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 504: { var name = "米莱狄"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.woman, CommonClass.driversource.Race.people, name); driverSet.DefensiveInitialize(0, 24, 24, 40, 40, 40, 0); driverSet.GrouthSet(105, 106, 90); driverSet.Index = 504; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 179: { var name = "女娲"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.woman, CommonClass.driversource.Race.people, name); driverSet.DefensiveInitialize(0, 0, 0, 50, 60, 50, 0); driverSet.GrouthSet(105, 106, 90); driverSet.Index = 179; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 162: { var name = "娜露露"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.woman, CommonClass.driversource.Race.people, name); driverSet.DefensiveInitialize(0, 0, 0, 40, 40, 40, 30); driverSet.GrouthSet(115, 116, 100); driverSet.Index = 162; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 157: { var name = "不火舞"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.woman, CommonClass.driversource.Race.people, name); driverSet.DefensiveInitialize(12, 24, 12, 40, 40, 40, 0); driverSet.GrouthSet(105, 106, 90); driverSet.Index = 157; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 146: { var name = "露娜"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.woman, CommonClass.driversource.Race.people, name); driverSet.DefensiveInitialize(0, 12, 12, 40, 50, 50, 0); driverSet.GrouthSet(105, 106, 90); driverSet.Index = 146; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 141: { var name = "貂蝉"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.woman, CommonClass.driversource.Race.people, name); driverSet.DefensiveInitialize(12, 12, 0, 40, 50, 50, 0); driverSet.GrouthSet(105, 106, 90); driverSet.Index = 141; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 136: { var name = "武则天"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.woman, CommonClass.driversource.Race.people, name); driverSet.DefensiveInitialize(0, 0, 0, 40, 50, 50, 15); driverSet.GrouthSet(105, 111, 100); driverSet.Index = 136; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 518: { var name = "马超"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.man, CommonClass.driversource.Race.immortal, name); driverSet.DefensiveInitialize(40, 40, 40, 0, 0, 0, 30); driverSet.GrouthSet(113, 95, 125); driverSet.Index = 518; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 194: { var name = "苏烈"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.man, CommonClass.driversource.Race.immortal, name); driverSet.DefensiveInitialize(40, 40, 40, 0, 0, 0, 30); driverSet.GrouthSet(133, 95, 105); driverSet.Index = 194; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 195: { var name = "百里玄"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.man, CommonClass.driversource.Race.immortal, name); driverSet.DefensiveInitialize(40, 40, 40, 20, 20, 0, 0); driverSet.GrouthSet(113, 85, 105); driverSet.Index = 195; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 193: { var name = "阿铠"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.man, CommonClass.driversource.Race.immortal, name); driverSet.DefensiveInitialize(40, 40, 40, 0, 20, 20, 0); driverSet.GrouthSet(113, 85, 105); driverSet.Index = 193; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 192: { var name = "黄忠"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.man, CommonClass.driversource.Race.immortal, name); driverSet.DefensiveInitialize(64, 64, 40, 0, 0, 0, 0); driverSet.GrouthSet(113, 85, 105); driverSet.Index = 192; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 178: { var name = "杨戬"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.man, CommonClass.driversource.Race.immortal, name); driverSet.DefensiveInitialize(40, 64, 64, 0, 0, 0, 0); driverSet.GrouthSet(113, 85, 105); driverSet.Index = 178; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 171: { var name = "张飞"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.man, CommonClass.driversource.Race.immortal, name); driverSet.DefensiveInitialize(40, 40, 40, 10, 20, 10, 0); driverSet.GrouthSet(113, 85, 105); driverSet.Index = 171; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 144: { var name = "程咬金"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.man, CommonClass.driversource.Race.immortal, name); driverSet.DefensiveInitialize(40, 40, 40, 0, 0, 0, 30); driverSet.GrouthSet(123, 95, 115); driverSet.Index = 144; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 140: { var name = "关羽"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.man, CommonClass.driversource.Race.immortal, name); driverSet.DefensiveInitialize(52, 64, 52, 0, 0, 0, 0); driverSet.GrouthSet(113, 85, 105); driverSet.Index = 140; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 135: { var name = "项羽"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.man, CommonClass.driversource.Race.immortal, name); driverSet.DefensiveInitialize(52, 52, 40, 10, 10, 0, 0); driverSet.GrouthSet(113, 85, 105); driverSet.Index = 135; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 123: { var name = "吕布"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.man, CommonClass.driversource.Race.immortal, name); driverSet.DefensiveInitialize(52, 52, 40, 0, 10, 10, 0); driverSet.GrouthSet(113, 85, 105); driverSet.Index = 123; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 107: { var name = "赵云"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.man, CommonClass.driversource.Race.immortal, name); driverSet.DefensiveInitialize(52, 52, 40, 0, 0, 0, 15); driverSet.GrouthSet(113, 90, 115); driverSet.Index = 107; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 538: { var name = "云缨"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.woman, CommonClass.driversource.Race.immortal, name); driverSet.DefensiveInitialize(40, 40, 40, 0, 0, 0, 30); driverSet.GrouthSet(113, 95, 125); driverSet.Index = 538; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 508: { var name = "伽罗"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.woman, CommonClass.driversource.Race.immortal, name); driverSet.DefensiveInitialize(40, 40, 40, 0, 0, 0, 30); driverSet.GrouthSet(133, 95, 105); driverSet.Index = 508; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 199: { var name = "公孙离"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.woman, CommonClass.driversource.Race.immortal, name); driverSet.DefensiveInitialize(40, 40, 40, 20, 20, 0, 0); driverSet.GrouthSet(113, 85, 105); driverSet.Index = 199; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 198: { var name = "梦奇"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.woman, CommonClass.driversource.Race.immortal, name); driverSet.DefensiveInitialize(40, 40, 40, 0, 20, 20, 0); driverSet.GrouthSet(113, 85, 105); driverSet.Index = 198; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 174: { var name = "虞姬"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.woman, CommonClass.driversource.Race.immortal, name); driverSet.DefensiveInitialize(64, 64, 40, 0, 0, 0, 0); driverSet.GrouthSet(113, 85, 105); driverSet.Index = 174; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 154: { var name = "花木兰"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.woman, CommonClass.driversource.Race.immortal, name); driverSet.DefensiveInitialize(40, 64, 64, 0, 0, 0, 0); driverSet.GrouthSet(113, 85, 105); driverSet.Index = 154; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 152: { var name = "王昭君"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.woman, CommonClass.driversource.Race.immortal, name); driverSet.DefensiveInitialize(40, 40, 40, 10, 20, 10, 0); driverSet.GrouthSet(113, 85, 105); driverSet.Index = 152; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 142: { var name = "安琪拉"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.woman, CommonClass.driversource.Race.immortal, name); driverSet.DefensiveInitialize(40, 40, 40, 0, 0, 0, 30); driverSet.GrouthSet(123, 95, 115); driverSet.Index = 142; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 127: { var name = "甄姬"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.woman, CommonClass.driversource.Race.immortal, name); driverSet.DefensiveInitialize(52, 64, 52, 0, 0, 0, 0); driverSet.GrouthSet(113, 85, 105); driverSet.Index = 127; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 117: { var name = "钟无艳"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.woman, CommonClass.driversource.Race.immortal, name); driverSet.DefensiveInitialize(40, 40, 40, 0, 10, 10, 15); driverSet.GrouthSet(123, 90, 105); driverSet.Index = 117; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 111: { var name = "孙尚香"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.woman, CommonClass.driversource.Race.immortal, name); driverSet.DefensiveInitialize(40, 52, 52, 10, 10, 0, 0); driverSet.GrouthSet(113, 85, 105); driverSet.Index = 111; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;
                case 109: { var name = "妲己"; CommonClass.driversource.Driver driverSet = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.woman, CommonClass.driversource.Race.immortal, name); driverSet.DefensiveInitialize(40, 40, 40, 0, 10, 10, 15); driverSet.GrouthSet(113, 90, 115); driverSet.Index = 109; car.ability.driver = driverSet; car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg); car.ability.driverSelected(player, car, ref notifyMsg); }; break;

                    //case 518:
                    //    {
                    //        //输出 马超 男  模拟数据男仙01
                    //        var name = "马超";
                    //        CommonClass.driversource.Driver machao = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.man, CommonClass.driversource.Race.immortal, name);
                    //        machao.DefensiveInitialize(66, 60, 52, 02, 14, 07, 0);
                    //        machao.GrouthSet(93, 93, 115);
                    //        machao.Index = index;
                    //        car.ability.driver = machao;
                    //        car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg);
                    //        car.ability.driverSelected(player, car, ref notifyMsg);
                    //    }; break;
                    //case 538:
                    //    {
                    //        /*
                    //         * 女仙02
                    //         */
                    //        var name = "云缨";
                    //        CommonClass.driversource.Driver yunying = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.woman, CommonClass.driversource.Race.immortal, name);
                    //        yunying.DefensiveInitialize(62, 62, 42, 15, 08, 02, 13);
                    //        yunying.GrouthSet(93, 115, 93);
                    //        yunying.Index = index;
                    //        car.ability.driver = yunying;
                    //        car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg);
                    //        car.ability.driverSelected(player, car, ref notifyMsg);
                    //    }; break;
                    //case 137:
                    //    {
                    //        /*
                    //         * 男人01
                    //         */
                    //        var name = "司马懿";
                    //        CommonClass.driversource.Driver simayi = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.man, CommonClass.driversource.Race.people, name);
                    //        simayi.DefensiveInitialize(13, 10, 14, 72, 68, 40, 18);
                    //        simayi.GrouthSet(110, 95, 95);
                    //        simayi.Index = index;
                    //        car.ability.driver = simayi;
                    //        car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg);
                    //        car.ability.driverSelected(player, car, ref notifyMsg);
                    //    }; break;
                    //case 536:
                    //    {
                    //        var name = "夏洛特";
                    //        CommonClass.driversource.Driver simayi = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.woman, CommonClass.driversource.Race.people, name);
                    //        simayi.DefensiveInitialize(13, 10, 14, 72, 68, 40, 18);
                    //        simayi.GrouthSet(110, 95, 95);
                    //        simayi.Index = index;
                    //        car.ability.driver = simayi;
                    //        car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg);
                    //        car.ability.driverSelected(player, car, ref notifyMsg);
                    //    }; break;
                    //case 510:
                    //    {
                    //        var name = "孙策";
                    //        CommonClass.driversource.Driver simayi = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.man, CommonClass.driversource.Race.devil, name);
                    //        simayi.DefensiveInitialize(13, 10, 14, 72, 68, 40, 18);
                    //        simayi.GrouthSet(110, 95, 95);
                    //        simayi.Index = index;
                    //        car.ability.driver = simayi;
                    //        car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg);
                    //        car.ability.driverSelected(player, car, ref notifyMsg);
                    //    }; break;
                    //case 155:
                    //    {
                    //        var name = "艾琳";
                    //        CommonClass.driversource.Driver simayi = new CommonClass.driversource.Driver(CommonClass.driversource.Sex.woman, CommonClass.driversource.Race.devil, name);
                    //        simayi.DefensiveInitialize(13, 10, 14, 72, 68, 40, 18);
                    //        simayi.GrouthSet(110, 110, 105);
                    //        simayi.Index = index;
                    //        car.ability.driver = simayi;
                    //        car.ability.RefreshAfterDriverArrived(player, car, ref notifyMsg);
                    //        car.ability.driverSelected(player, car, ref notifyMsg);
                    //    }; break;

            }
            if (player.playerType == RoleInGame.PlayerType.player)
                that.taskM.DriverSelected((Player)player);
        }


        //private void SetRecruit(int recruit, ref RoleInGame player)
        //{
        //    /*
        //     * 不会衰减，只有重新求福，才会衰减。
        //     */
        //    //player.buildingReward[0] -= 5;
        //    //player.buildingReward[0] = Math.Max(0, player.buildingReward[0]);
        //}
        internal static int GetRecruit(RoleInGame player)
        {
            return player.buildingReward[0];
        }

        internal void SetAsPeople(NPC npc, ref List<string> notifyMsg)
        {
            List<int> forSelect = new List<int>
            { 137,190,150,149,134,133,132,131,128,124,115,110,536,506,515,513,312,504,179,162,157,146,141,136};
            var selectIndex = forSelect[that.rm.Next(0, forSelect.Count)];
            var car = npc.getCar();
            setDriver(npc, car, selectIndex, ref notifyMsg);
            // throw new NotImplementedException();
        }

        internal void SetAsDevil(NPC npc, ref List<string> notifyMsg)
        {
            List<int> forSelect = new List<int>
            { 510,501,196,180,173,170,169,166,156,118,114,105,155,533,523,505,125,176,191,184,183,121,116,106};
            var selectIndex = forSelect[that.rm.Next(0, forSelect.Count)];
            var car = npc.getCar();
            setDriver(npc, car, selectIndex, ref notifyMsg);
            // throw new NotImplementedException();
        }

        //immortal
        internal void SetAsImmortal(NPC npc, ref List<string> notifyMsg)
        {
            List<int> forSelect = new List<int>
            { 518,194,195,193,192,178,171,144,140,135,123,107,538,508,199,198,174,154,152,142,127,117,111,109};
            var selectIndex = forSelect[that.rm.Next(0, forSelect.Count)];
            var car = npc.getCar();
            setDriver(npc, car, selectIndex, ref notifyMsg);
            // throw new NotImplementedException();
        }

        public void newThreadDo(CommonClass.Command c, GetRandomPos grp)
        {
            // throw new NotImplementedException();
        }

        public class ConfuseManger
        {
            public enum ControlAttackType
            {
                Confuse,
                Lost,
                Ambush,
                //  Ambush

            }
            public ConfuseManger()
            {
                this.controlInfomations = new List<ControlInfomation>();
                this._selectedControlItem = null;
                this.ambushInfomations = new List<AmbushInfomation>();
                this.simulate = new Simulate(this);
            }
            internal void Cancle(RoleInGame player_)
            {
                this.controlInfomations.RemoveAll(item => item.player.Key == player_.Key);
                this.ambushInfomations.RemoveAll(item => item.player.Key == player_.Key);
            }
            internal Int64 GetLoseValue()
            {
                if (this.IsBeingControlled())
                {
                    if (this._selectedControlItem.attackType == ControlAttackType.Lost)
                    {
                        return this._selectedControlItem.volumeValue;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else return 0;
            }
            internal bool IsBeingControlledByLose()
            {
                if (this.IsBeingControlled())
                {
                    if (this._selectedControlItem.attackType == ControlAttackType.Lost)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else return false;
            }

            internal Int64 GetConfuseValue()
            {
                if (this.IsBeingControlled())
                {
                    if (this._selectedControlItem.attackType == ControlAttackType.Confuse)
                    {
                        return this._selectedControlItem.volumeValue;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else return 0;
            }
            internal bool IsBeingControlledByConfuse()
            {
                if (this.IsBeingControlled())
                {
                    if (this._selectedControlItem.attackType == ControlAttackType.Confuse)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else return false;
            }
            #region 控制区
            List<ControlInfomation> controlInfomations = new List<ControlInfomation>();
            internal class ControlInfomation
            {
                public RoleInGame player { get; set; }
                public OssModel.FastonPosition fpResult { get; set; }
                public long volumeValue { get; set; }
                public ControlAttackType attackType { get; set; }
            }



            ControlInfomation _selectedControlItem = null;

            public RoleInGame getBoss()
            {
                if (this._selectedControlItem == null)
                {
                    return null;
                }
                else
                {
                    return this._selectedControlItem.player;
                }
            }

            internal void AddControlInfo(RoleInGame player_, FastonPosition fastonPosition_, long volumeValue_, ControlAttackType attackType_)
            {
                this.Cancle(player_);
                //this.AttackInfomations.
                this.controlInfomations.Add(new ControlInfomation()
                {
                    player = player_,
                    fpResult = fastonPosition_,
                    volumeValue = volumeValue_,
                    attackType = attackType_
                });
            }

            internal bool IsBeingControlled()
            {
                if (this._selectedControlItem == null)
                {
                    return false;
                }
                else
                {
                    if (this._selectedControlItem.player.Bust)
                    {
                        return false;
                    }
                    else if (this._selectedControlItem.volumeValue > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                        // return false;
                    }
                }
            }

            public ControlAttackType getControlType()
            {
                if (this.IsBeingControlled())
                {
                    return this._selectedControlItem.attackType;
                }
                else
                {
                    throw new Exception("非法调用");
                }
            }



            internal enum programResult { runContinue, runReturn };
            //programResult dealWithItem(RoleInGame self, ControlInfomation magicItem, RoomMain that, webnotify ex, Car enemyCar, RoleInGame enemy, ref List<string> notifyMsg, out bool protecedByDefendMagic)
            //{
            //    int defensiveOfControl;
            //    if (self.getCar().ability.driver == null)
            //    {
            //        defensiveOfControl = 0;
            //    }
            //    else
            //    {
            //        if (magicItem.attackType == ControlAttackType.Confuse)
            //            defensiveOfControl = self.getCar().ability.driver.defensiveOfConfuse;
            //        else if (magicItem.attackType == ControlAttackType.Lost)
            //            defensiveOfControl = self.getCar().ability.driver.defensiveOfLose;
            //        else throw new Exception("意料之外！");
            //    }
            //    string name = "";
            //    switch (magicItem.attackType)
            //    {
            //        case ControlAttackType.Confuse:
            //            {
            //                name = "混乱";
            //            }; break;
            //        case ControlAttackType.Lost:
            //            {
            //                name = "迷失";
            //            }; break;
            //    }
            //    var randomValue = that.rm.Next(0, 100);
            //    int defendedProbability;
            //    if (self.improvementRecord.defenceValue > 0)
            //    {
            //        if (magicItem.attackType == ControlAttackType.Confuse)
            //            defendedProbability = that.magicE.ProtectedByConfuse();//self.getCar().ability.driver.defensiveOfConfuse;
            //        else if (magicItem.attackType == ControlAttackType.Lost)
            //            defendedProbability = that.magicE.ProtectedByLost();
            //        else throw new Exception("意料之外！");
            //        //defendedProbability = // that.magicE.ProtectedByAmbush();
            //    }
            //    else
            //    {
            //        defendedProbability = 0;
            //    }
            //    if (randomValue > this.getBaseControlProbability(magicItem.attackType) - defensiveOfControl)
            //    {

            //        ex.WebNotify(magicItem.player, $"你对【{self.PlayerName}】实施了{name}计谋，没成功！");
            //        ex.WebNotify(self, $"【{magicItem.player.PlayerName}】对你实施了{name}阴谋，没成功！");

            //        enemyCar.setState(enemy, ref notifyMsg, CarState.returning);
            //        that.retutnE.SetReturnT(500, new commandWithTime.returnning()
            //        {
            //            c = "returnning",
            //            changeType = commandWithTime.returnning.ChangeType.BeforeTax,
            //            key = magicItem.player.Key,
            //            returningOjb = magicItem.player.returningOjb,
            //            target = enemyCar.targetFpIndex
            //        });
            //        protecedByDefendMagic = false;
            //        return programResult.runContinue;
            //    }
            //    else if (randomValue > this.getBaseControlProbability(magicItem.attackType) - defensiveOfControl - defendedProbability)
            //    {
            //        ex.WebNotify(magicItem.player, $"你对【{self.PlayerName}】实施了{name}计谋，被其保护光环阻挡，没成功！");
            //        ex.WebNotify(self, $"【{magicItem.player.PlayerName}】对你实施了{name}阴谋，被其保护光环阻挡，没成功！");

            //        enemyCar.setState(enemy, ref notifyMsg, CarState.returning);
            //        that.retutnE.SetReturnT(500, new commandWithTime.returnning()
            //        {
            //            c = "returnning",
            //            changeType = commandWithTime.returnning.ChangeType.BeforeTax,
            //            key = magicItem.player.Key,
            //            returningOjb = magicItem.player.returningOjb,
            //            target = enemyCar.targetFpIndex
            //        });
            //        protecedByDefendMagic = true;
            //        return programResult.runContinue;
            //    }
            //    else
            //    {
            //        ex.WebNotify(magicItem.player, $"你对【{self.PlayerName}】成功实施了{name}计谋！");
            //        ex.WebNotify(self, $"【{magicItem.player.PlayerName}】让你陷入了{name}！");
            //        this._selectedControlItem = magicItem;

            //        enemyCar.setState(enemy, ref notifyMsg, CarState.returning);
            //        that.retutnE.SetReturnT(500, new commandWithTime.returnning()
            //        {
            //            c = "returnning",
            //            changeType = commandWithTime.returnning.ChangeType.BeforeTax,
            //            key = magicItem.player.Key,
            //            returningOjb = magicItem.player.returningOjb,
            //            target = enemyCar.targetFpIndex
            //        });
            //        protecedByDefendMagic = false;
            //        return programResult.runReturn;
            //    }
            //}

            public class Simulate
            {
                private ConfuseManger _confuseManger;

                public Simulate(ConfuseManger confuseManger)
                {
                    this._confuseManger = confuseManger;
                }

                internal bool dealWithItem_simulate(RoleInGame self, RoomMain that, Car enemyCar, RoleInGame enemy)
                {
                    int defensiveOfAmbush;
                    if (self.getCar().ability.driver == null)
                    {
                        defensiveOfAmbush = 0;
                    }
                    else
                    {
                        defensiveOfAmbush = self.getCar().ability.driver.defensiveOfAmbush;
                    }

                    int defendedProbability;
                    if (self.improvementRecord.defenceValue > 0)
                    {
                        defendedProbability = Engine_MagicEngine.AmbushPropertyByDefendMagic;
                    }
                    else
                    {
                        defendedProbability = 0;
                    }
                    var randomValue = that.rm.Next(0, 100);
                    if (randomValue > _confuseManger.getBaseControlProbability(ControlAttackType.Ambush) - defensiveOfAmbush)
                    {
                        return false;
                    }
                    else if (randomValue > _confuseManger.getBaseControlProbability(ControlAttackType.Ambush) - defensiveOfAmbush - defendedProbability)
                    {

                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }

                interface controlMagic
                {
                    int getControl(Driver driver);
                    int getProtectedByDefendMagic(Engine_MagicEngine magicE);

                    ControlAttackType controlType { get; }
                }
                class confuseObj : controlMagic
                {
                    public ControlAttackType controlType { get { return ControlAttackType.Confuse; } }

                    public int getControl(Driver driver)
                    {
                        return driver.defensiveOfConfuse;
                        // throw new NotImplementedException();
                    }

                    public int getProtectedByDefendMagic(Engine_MagicEngine magicE)
                    {
                        return Engine_MagicEngine.ConfusePropertyByDefendMagic;   //magicE.ProtectedByConfuse();
                    }
                }
                class loseObj : controlMagic
                {
                    public ControlAttackType controlType { get { return ControlAttackType.Lost; } }

                    public int getControl(Driver driver)
                    {
                        return driver.defensiveOfLose;
                        // throw new NotImplementedException();
                    }

                    public int getProtectedByDefendMagic(Engine_MagicEngine magicE)
                    {
                        return Engine_MagicEngine.LostPropertyByDefendMagic;
                        // return magicE.ProtectedByLost();
                    }
                }
                internal bool confuse(RoleInGame self, RoomMain that, Car enemyCar, RoleInGame enemy)
                {
                    confuseObj co = new confuseObj();
                    return control(self, that, enemyCar, enemy, co);

                }

                private bool control(RoleInGame self, RoomMain that, Car enemyCar, RoleInGame enemy, controlMagic cm)
                {
                    int defensiveOfControl;
                    if (self.getCar().ability.driver == null)
                    {
                        defensiveOfControl = 0;
                    }
                    else
                    {
                        defensiveOfControl = cm.getControl(self.getCar().ability.driver);
                    }

                    int defendedProbability;
                    if (self.improvementRecord.defenceValue > 0)
                    {
                        defendedProbability = cm.getProtectedByDefendMagic(that.magicE);//.ProtectedByConfuse();
                    }
                    else
                    {
                        defendedProbability = 0;
                    }
                    var randomValue = that.rm.Next(0, 100);
                    if (randomValue > _confuseManger.getBaseControlProbability(cm.controlType) - defensiveOfControl)
                    {
                        return false;
                    }
                    else if (randomValue > _confuseManger.getBaseControlProbability(cm.controlType) - defensiveOfControl - defendedProbability)
                    {

                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }

                internal bool Lose(RoleInGame self, RoomMain that, Car enemyCar, NPC enemy)
                {
                    loseObj lo = new loseObj();
                    return control(self, that, enemyCar, enemy, lo);
                }

                interface improveMagic
                {

                }

                class improveAttackMagic : improveMagic
                {

                }
                internal void improveAttack(RoleInGame partner, RoomMain that, Car car, NPC npc_Operate)
                {
                    improveAttackMagic lo = new improveAttackMagic();
                    improve(partner, that, car, npc_Operate, lo);
                    // throw new NotImplementedException();

                }

                private long improve(RoleInGame partner, RoomMain that, Car car, NPC npc_Operate, improveMagic im)
                {
                    if (that._Players.ContainsKey(partner.Key))
                    {
                        var beneficiary = that._Players[partner.Key];
                        if (!beneficiary.Bust)
                        {

                            long costVolumeValue;
                            beneficiary.improvementRecord.simulateToAddSpeed(partner.getCar().ability.leftVolume, out costVolumeValue);
                            return costVolumeValue;
                        }
                        else
                        {
                            return 0;
                        }

                    }
                    else
                    {
                        return 0;
                    }
                }
            }

            //private int getBaseControlProbability(object controlType)
            //{
            //    throw new NotImplementedException();
            //}

            public Simulate simulate { get; private set; }
            // this.

            //            bool dealWithItem(RoleInGame self, AmbushInfomation magicItem, RoomMain that, webnotify ex, Car enemyCar, RoleInGame enemy, GetRandomPos grp, ref List<string> notifyMsg, out bool protecedByDefendMagic)
            //            {
            //#warning 这里没有Ignore
            //                int defensiveOfAmbush;
            //                if (self.getCar().ability.driver == null)
            //                {
            //                    defensiveOfAmbush = 0;
            //                }
            //                else
            //                {
            //                    defensiveOfAmbush = self.getCar().ability.driver.defensiveOfAmbush;
            //                }
            //                string name = "潜伏";

            //                int defendedProbability;
            //                if (self.improvementRecord.defenceValue > 0)
            //                {
            //                    defendedProbability = Engine_MagicEngine.AmbushPropertyByDefendMagic;//that.magicE.ProtectedByAmbush();
            //                }
            //                else
            //                {
            //                    defendedProbability = 0;
            //                }
            //                var randomValue = that.rm.Next(0, 100);
            //                if (randomValue > this.getBaseControlProbability(ControlAttackType.Ambush) - defensiveOfAmbush)
            //                {
            //                    ex.WebNotify(magicItem.player, $"你对【{self.PlayerName}】实施了{name}计谋，被其识破，未能成功！");
            //                    ex.WebNotify(self, $"【{magicItem.player.PlayerName}】对你实施了{name}阴谋，被你识破，未能成功！");

            //                    enemyCar.setState(enemy, ref notifyMsg, CarState.returning);
            //                    that.retutnE.SetReturnT(500, new commandWithTime.returnning()
            //                    {
            //                        c = "returnning",
            //                        changeType = commandWithTime.returnning.ChangeType.BeforeTax,
            //                        key = magicItem.player.Key,
            //                        returningOjb = magicItem.player.returningOjb,
            //                        target = enemyCar.targetFpIndex
            //                    }, grp);
            //                    protecedByDefendMagic = true;
            //                    return false;
            //                }
            //                else if (randomValue > this.getBaseControlProbability(ControlAttackType.Ambush) - defensiveOfAmbush - defendedProbability)
            //                {
            //                    ex.WebNotify(magicItem.player, $"你对【{self.PlayerName}】实施了{name}计谋，被其保护光环阻挡，未能成功！");
            //                    ex.WebNotify(self, $"【{magicItem.player.PlayerName}】对你实施了{name}阴谋，被保护光环阻挡，未能成功！");

            //                    //var attackMoneyBeforeDefend = (at.leftValue(car.ability) * (100 - at.GetDefensiveValue(victim.getCar().ability.driver)) / 100) * percentValue / 100;
            //                    //var attackMoneyAfterDefend = (at.leftValue(car.ability) * (100 - at.GetDefensiveValue(victim.getCar().ability.driver, victim.improvementRecord.defenceValue > 0)) / 100) * percentValue / 100;
            //                    //attackMoneyBeforeDefend = Math.Max(1, attackMoneyBeforeDefend);
            //                    //attackMoneyAfterDefend = Math.Max(1, attackMoneyAfterDefend);

            //                    enemyCar.setState(enemy, ref notifyMsg, CarState.returning);
            //                    that.retutnE.SetReturnT(500, new commandWithTime.returnning()
            //                    {
            //                        c = "returnning",
            //                        changeType = commandWithTime.returnning.ChangeType.BeforeTax,
            //                        key = magicItem.player.Key,
            //                        returningOjb = magicItem.player.returningOjb,
            //                        target = enemyCar.targetFpIndex
            //                    }, grp);
            //                    protecedByDefendMagic = false;
            //                    return false;
            //                }
            //                else
            //                {
            //                    ex.WebNotify(magicItem.player, $"你对【{self.PlayerName}】成功实施了{name}计谋！");
            //                    ex.WebNotify(self, $"【{magicItem.player.PlayerName}】让你陷入了{name}！");
            //                    enemyCar.setState(enemy, ref notifyMsg, CarState.returning);
            //                    that.retutnE.SetReturnT(500, new commandWithTime.returnning()
            //                    {
            //                        c = "returnning",
            //                        changeType = commandWithTime.returnning.ChangeType.BeforeTax,
            //                        key = magicItem.player.Key,
            //                        returningOjb = magicItem.player.returningOjb,
            //                        target = enemyCar.targetFpIndex
            //                    }, grp);
            //                    protecedByDefendMagic = false;
            //                    return true;
            //                }
            //            }
            const int baseConfuseProbability = 90;
            const int baseTemptationProbability = 90;
            const int baseAmbushProbability = 90;
            private int getBaseControlProbability(ControlAttackType attackType)
            {
                switch (attackType)
                {
                    case ControlAttackType.Confuse:
                        {
                            return baseConfuseProbability;
                        };
                    case ControlAttackType.Lost:
                        {
                            return baseTemptationProbability;
                        };
                    case ControlAttackType.Ambush:
                        {
                            return baseAmbushProbability;
                        };
                    default:
                        {
                            throw new Exception("");
                        }
                }
            }

            internal void ControllSelf(RoleInGame self, RoomMain that, GetRandomPos grp, ref List<string> notifyMsg, interfaceOfEngine.webnotify ex)
            {
                FastonPosition baseFp = Program.dt.GetFpByIndex(self.StartFPIndex);
                this.controlInfomations.RemoveAll(item => item.player.Bust);
                var orderedMagic = (from item in this.controlInfomations
                                    orderby Geography.getLengthOfTwoPoint.GetDistance(baseFp.Latitde, baseFp.Longitude, baseFp.Height, Program.dt.GetFpByIndex(item.player.StartFPIndex).Latitde, Program.dt.GetFpByIndex(item.player.StartFPIndex).Longitude, Program.dt.GetFpByIndex(item.player.StartFPIndex).Height)
                                    select item).ToList();
                for (var i = 0; i < orderedMagic.Count; i++)
                {

                    var magicItem = orderedMagic[i];
                    var enemy = magicItem.player;
                    var enemyCar = enemy.getCar();
                    var enemyFp = Program.dt.GetFpByIndex(enemy.StartFPIndex);
                    if ((
                        enemyCar.state == Car.CarState.waitOnRoad && Program.dt.GetFpByIndex(enemyCar.targetFpIndex).FastenPositionID == magicItem.fpResult.FastenPositionID))
                    {
                        if (magicItem.attackType == ControlAttackType.Confuse)
                        {

                            Engine_MagicEngine.confuseMagicTool t = new Engine_MagicEngine.confuseMagicTool(magicItem, self, enemy, that);
                            long v = 0;
                            var result = that.magicE.DealWithControlMagic(t, grp, enemy, ref v);
                            if (result == programResult.runContinue)
                                continue;
                            else
                                return;
                        }
                        else if (magicItem.attackType == ControlAttackType.Lost)
                        {
                            Engine_MagicEngine.loseMagicTool t = new Engine_MagicEngine.loseMagicTool(magicItem, self, enemy, that);
                            long v = 0;
                            var result = that.magicE.DealWithControlMagic(t, grp, enemy, ref v);
                            if (result == programResult.runContinue)
                                continue;
                            else
                                return;
                        }
                    }
                }

            }

            internal long getIndemnity()
            {
                return this._selectedControlItem.volumeValue;
                //throw new NotImplementedException();
            }
            internal void SelectItem(ControlInfomation magicItem)
            {
                this._selectedControlItem = magicItem;
            }
            internal void reduceValue(long reduceValue)
            {
                this._selectedControlItem.volumeValue -= reduceValue;
            }
            #endregion 控制区


            #region 埋伏区 ambush
            public class AmbushInfomation
            {
                public RoleInGame player { get; set; }
                public OssModel.FastonPosition fpResult { get; set; }
                public long volumeValue { get; set; }
                //public long bussinessValue { get; set; }
            }

            internal void AddAmbushInfo(RoleInGame player_, FastonPosition fastonPosition_, long volumeValue_, string thisRoleKey, ref List<string> notifyMsg)
            {
                this.Cancle(player_);
                //this.AttackInfomations.
                this.ambushInfomations.Add(new AmbushInfomation()
                {
                    player = player_,
                    fpResult = fastonPosition_,
                    volumeValue = volumeValue_,
                    //    bussinessValue = bussinessValue_
                });
            }

            //internal bool IsBeingAimed()
            //{
            //    throw new NotImplementedException();
            //}
            internal List<AmbushInfomation> ambushInfomations = new List<AmbushInfomation>();

            // internal delegate void AmbushAttack(int i, ref List<string> notifyMsg, RoleInGame enemy, ref long reduceSumInput, bool protecedByDefendMagic);
            internal void AmbushSelf(RoleInGame selfRole, RoomMain that, webnotify ex, ref List<string> notifyMsg, interfaceOfHM.AttackT at, GetRandomPos grp, ref long reduceSumInput)
            {
                List<AmbushInfomation> newList = new List<AmbushInfomation>();
                for (int i = 0; i < ambushInfomations.Count; i++)
                {
                    if (!ambushInfomations[i].player.Bust)
                        if (ambushInfomations[i].player.getCar().state == CarState.waitOnRoad)
                        {
                            if (Program.dt.GetFpByIndex(ambushInfomations[i].player.getCar().targetFpIndex).FastenPositionID == ambushInfomations[i].fpResult.FastenPositionID)
                            {
                                newList.Add(this.ambushInfomations[i]);
                            }
                        }
                }
                var baseFp = Program.dt.GetFpByIndex(selfRole.StartFPIndex);

                this.ambushInfomations = (from item in newList
                                          orderby Geography.getLengthOfTwoPoint.GetDistance(baseFp.Latitde, baseFp.Longitude, baseFp.Height, item.fpResult.Latitde, item.fpResult.Longitude, item.fpResult.Height) ascending,
                                          (item.volumeValue) descending
                                          select item).ToList();

                for (int i = 0; i < this.ambushInfomations.Count; i++)
                {
                    var enemyCar = this.ambushInfomations[i].player.getCar();
                    var enemy = this.ambushInfomations[i].player;
                    Engine_MagicEngine.ambushMagicTool amt = new Engine_MagicEngine.ambushMagicTool(enemy, selfRole, at, that);
                    that.magicE.DealWithControlMagic(amt, grp, enemy, ref reduceSumInput);//.de(amt, enemy); 
                }
            }

            internal long AmbushSelf(RoleInGame selfRole, RoomMain that, Engine_MagicEngine.attackMagicTool at, GetRandomPos gp)
            {
                List<AmbushInfomation> newList = new List<AmbushInfomation>();
                for (int i = 0; i < ambushInfomations.Count; i++)
                {
                    if (!ambushInfomations[i].player.Bust)
                        if (ambushInfomations[i].player.getCar().state == CarState.waitOnRoad)
                        {
                            if (Program.dt.GetFpByIndex(ambushInfomations[i].player.getCar().targetFpIndex).FastenPositionID == ambushInfomations[i].fpResult.FastenPositionID)
                            {
                                newList.Add(this.ambushInfomations[i]);
                            }
                        }
                }
                var baseFp = gp.GetFpByIndex(selfRole.StartFPIndex);

                this.ambushInfomations = (from item in newList
                                          orderby Geography.getLengthOfTwoPoint.GetDistance(baseFp.Latitde, baseFp.Longitude, baseFp.Height, item.fpResult.Latitde, item.fpResult.Longitude, item.fpResult.Height) ascending,
                                          (item.volumeValue) descending
                                          select item).ToList();
                long result = 0;
                for (int i = 0; i < this.ambushInfomations.Count; i++)
                {
                    var enemyCar = this.ambushInfomations[i].player.getCar();
                    var enemy = this.ambushInfomations[i].player;
                    result += dealWithItem(selfRole, this.ambushInfomations[i], that, enemyCar, enemy);
                }
                return result;
            }
            /// <summary>
            /// NPC决策时用于模仿潜伏技能！
            /// </summary>
            /// <param name="self"></param>
            /// <param name="ambushInfomation"></param>
            /// <param name="that"></param>
            /// <param name="enemyCar"></param>
            /// <param name="enemy"></param>
            /// <returns></returns>
            private long dealWithItem(RoleInGame self, AmbushInfomation ambushInfomation, RoomMain that, Car enemyCar, RoleInGame enemy)
            {
                int defensiveOfAmbush;
                if (self.getCar().ability.driver == null)
                {
                    defensiveOfAmbush = 0;
                }
                else
                {
                    defensiveOfAmbush = self.getCar().ability.driver.defensiveOfAmbush;
                }
                //string name = "潜伏";

                int defendedProbability;
                if (self.improvementRecord.defenceValue > 0)
                {
                    defendedProbability = Engine_MagicEngine.AmbushPropertyByDefendMagic;
                }
                else
                {
                    defendedProbability = 0;
                }
                var randomValue = that.rm.Next(0, 100);
                if (randomValue > this.getBaseControlProbability(ControlAttackType.Ambush) - defensiveOfAmbush)
                {
                    return 0;
                }
                else if (randomValue > this.getBaseControlProbability(ControlAttackType.Ambush) - defensiveOfAmbush - defendedProbability)
                {
                    return 0;
                }
                else
                {
                    return ambushInfomation.volumeValue;
                }
            }

            /// <summary>
            /// 此过程也是在模仿攻击
            /// </summary>
            /// <param name="amt"></param>
            /// <param name="npc_Operate"></param>
            /// <param name="car"></param>
            /// <param name="longCollectMoney"></param>
            /// <param name="victim"></param>
            /// <param name="v"></param>
            /// <returns></returns>
            internal long SimulationToMagicAttack(Engine_MagicEngine.attackMagicTool amt, NPC npc_Operate, Car car, int longCollectMoney, RoleInGame victim, int v)
            {
                long harmValue;
                //var car = npc_Operate.getCar();
                if (victim.improvementRecord.defenceValue > 0)
                {
                    harmValue = ((amt.leftValue(car.ability) - longCollectMoney) * (100 - amt.GetDefensiveValue(victim.getCar().ability.driver, victim.improvementRecord.defenceValue > 0)) / 100);
                }
                else
                {
                    harmValue = ((amt.leftValue(car.ability) - longCollectMoney) * (100 - amt.GetDefensiveValue(victim.getCar().ability.driver)) / 100);
                }
                return harmValue;
            }







            #endregion 埋伏区
        }

        public class ImproveManager
        {
            long _speedValue = 0;
            long _attackValue = 0;
            long _defenceValue = 0;
            public long speedValue { get { return this._speedValue; } }
            public long attackValue { get { return this._attackValue; } }
            public long defenceValue { get { return this._defenceValue; } }
            public ImproveManager()
            {
                this._speedValue = 0;
                this._attackValue = 0;
                this._defenceValue = 0;
                this.simulate = new Simulate(this);
            }

            const int speedImproveParameter = 7;
            internal void addSpeed(RoleInGame role, long leftVolume, out long costVolumeValue, ref List<string> notifyMsg)
            {
                if (add(ref this._speedValue, leftVolume, speedImproveParameter, out costVolumeValue))
                {
                    role.speedMagicChanged(role, ref notifyMsg);
                }
            }
            internal void addDefence(RoleInGame role, long leftVolume, out long costVolumeValue, ref List<string> notifyMsg)
            {
                if (add(ref this._defenceValue, leftVolume, defenceImproveParameter, out costVolumeValue))
                {
                    role.defenceMagicChanged(role, ref notifyMsg);
                }
            }
            internal void simulateToAddSpeed(long leftVolume, out long costVolumeValue)
            {
                var copySpeed = this._speedValue + 0;
                add(ref copySpeed, leftVolume, speedImproveParameter, out costVolumeValue);
            }
            private void simulateToAddDefend(long leftVolume, out long costVolumeValue)
            {
                var copyDefence = this._defenceValue + 0;
                add(ref copyDefence, leftVolume, defenceImproveParameter, out costVolumeValue);
            }
            private void simulateToAddAttack(long leftBusinessValue, out long costBusinessValue)
            {
                var copySpeed = this._attackValue + 0;
                add(ref copySpeed, leftBusinessValue, speedImproveParameter, out costBusinessValue);
            }

            internal void reduceSpeed(RoleInGame role, long changeValue, ref List<string> notifyMsg)
            {
                if (reduce(ref this._speedValue, changeValue))
                {
                    role.speedMagicChanged(role, ref notifyMsg);
                }
            }
            const int attackImproveParameter = 7;
            internal void addAttack(RoleInGame role, long leftBusiness, out long costBusinessValue, ref List<string> notifyMsg)
            {
                if (add(ref this._attackValue, leftBusiness, attackImproveParameter, out costBusinessValue))
                {
                    role.attackMagicChanged(role, ref notifyMsg);
                }
            }

            internal void reduceAttack(RoleInGame role, long changeValue, ref List<string> notifyMsg)
            {
                if (reduce(ref this._attackValue, changeValue))
                {
                    role.attackMagicChanged(role, ref notifyMsg);
                }
            }

            static bool reduce(ref long operateValue, long changeValue)
            {
                var valueBeforeImprove = operateValue;

                operateValue -= changeValue;
                if (operateValue < 0)
                {
                    operateValue = 0;
                }
                var valueAfterImprove = operateValue;
                if ((valueBeforeImprove == 0 && valueAfterImprove != 0) ||
                    (valueBeforeImprove != 0 && valueAfterImprove == 0))
                {
                    return true;
                }
                else return false;
            }


            static bool add(ref long operateValue, long leftValue, long ImproveParameter, out long costValue)
            {
                var beforeImprove = operateValue;
                if (operateValue < leftValue * ImproveParameter)
                {
                    costValue = leftValue - operateValue / ImproveParameter;
                    costValue = Math.Max(1, costValue);
                    operateValue = leftValue * ImproveParameter;
                }
                else
                {
                    costValue = 0;
                }
                var afterImprove = operateValue;
                if ((afterImprove == 0 && beforeImprove != 0) ||
                    (afterImprove != 0 && beforeImprove == 0))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            const int defenceImproveParameter = 7;


            internal void reduceDefend(RoleInGame role, long changeValue, ref List<string> notifyMsg)
            {
                if (reduce(ref this._defenceValue, changeValue))
                {
                    role.defenceMagicChanged(role, ref notifyMsg);
                }
            }

            public class Simulate
            {
                private ImproveManager _improveManager;

                public Simulate(ImproveManager improveManager_)
                {
                    this._improveManager = improveManager_;
                }


                interface improveMagic
                {
                    long getMoney(Simulate simulate, RoleInGame npc_Operate, long costVolume);
                }

                class improveAttackMagic : improveMagic
                {
                    public long getMoney(Simulate simulate, RoleInGame npc_Operate, long costVolume)
                    {
                        long costBusinessValue;
                        simulate._improveManager.simulateToAddAttack(npc_Operate.getCar().ability.leftBusiness, out costBusinessValue);
                        return costBusinessValue;
                    }
                }
                class improveSpeedMagic : improveMagic
                {
                    public long getMoney(Simulate simulate, RoleInGame npc_Operate, long costVolume)
                    {
                        long costVolumeValue;
                        simulate._improveManager.simulateToAddSpeed(npc_Operate.getCar().ability.leftVolume - costVolume, out costVolumeValue);
                        return costVolumeValue * 3 / 2;
                    }
                }
                class improveDefendMagic : improveMagic
                {
                    public long getMoney(Simulate simulate, RoleInGame npc_Operate, long costVolume)
                    {
                        long costVolumeValue;
                        simulate._improveManager.simulateToAddDefend(npc_Operate.getCar().ability.leftVolume - costVolume, out costVolumeValue);
                        return costVolumeValue * 3 / 2;
                        // throw new NotImplementedException();
                    }
                }
                internal double improveAttack(RoleInGame partner, RoomMain that, Car car, NPC npc_Operate, GetRandomPos grp, out FastonPosition fp)
                {

                    improveAttackMagic lo = new improveAttackMagic();
                    return improve(partner, that, car, npc_Operate, lo, grp, out fp);

                }

                private double improve(RoleInGame partner, RoomMain that, Car car, NPC npc_Operate, improveMagic im, GetRandomPos grp, out FastonPosition fp)
                {

                    if (that._Players.ContainsKey(partner.Key))
                    {
                        var listIndexes = that.getCollectPositionsByDistance(grp.GetFpByIndex(partner.StartFPIndex), grp);
                        fp = grp.GetFpByIndex(that._collectPosition[listIndexes[0]]);
                        var longCollectMoney = that.GetCollectReWard(listIndexes[0]) * 100;
                        double distance;
                        RoleInGame boss;
                        var fromTarget = grp.GetFpByIndex(npc_Operate.StartFPIndex);
                        var endTarget = fp;
                        if (npc_Operate.HasTheBoss(that._Players, out boss))
                        {
                            var bossPoint = grp.GetFpByIndex(boss.StartFPIndex);
                            distance =
                                CommonClass.Geography.getLengthOfTwoPoint.GetDistance(fromTarget.Latitde, fromTarget.Longitude, fromTarget.Height, endTarget.Latitde, endTarget.Longitude, endTarget.Height)
                                + CommonClass.Geography.getLengthOfTwoPoint.GetDistance(bossPoint.Latitde, bossPoint.Longitude, bossPoint.Height, endTarget.Latitde, endTarget.Longitude, endTarget.Height)
                                + CommonClass.Geography.getLengthOfTwoPoint.GetDistance(bossPoint.Latitde, bossPoint.Longitude, bossPoint.Height, fromTarget.Latitde, fromTarget.Longitude, fromTarget.Height);
                        }
                        else
                        {
                            distance = CommonClass.Geography.getLengthOfTwoPoint.GetDistance(endTarget.Latitde, endTarget.Longitude, endTarget.Height, fromTarget.Latitde, fromTarget.Longitude, fromTarget.Height) * 2;
                        }
                        var beneficiary = that._Players[partner.Key];
                        if (!beneficiary.Bust)
                        {
                            long moneyGet = im.getMoney(this, npc_Operate, longCollectMoney) + longCollectMoney;

                            return moneyGet / distance;
                        }
                        else
                        {
                            return 0;
                        }

                    }
                    else
                    {
                        fp = null;
                        return 0;
                    }
                }

                internal double improveSpeed(RoleInGame partner, RoomMain that, Car car, NPC npc_Operate, GetRandomPos grp, out FastonPosition fp)
                {

                    improveSpeedMagic ism = new improveSpeedMagic();
                    return improve(partner, that, car, npc_Operate, ism, grp, out fp);
                }

                internal double improveDefend(RoleInGame partner, RoomMain that, Car car, NPC npc_Operate, GetRandomPos grp, out FastonPosition fp)
                {
                    improveDefendMagic idm = new improveDefendMagic();
                    return improve(partner, that, car, npc_Operate, idm, grp, out fp);
                }
            }



            public Simulate simulate { get; private set; }
        }

        internal bool controlledByMagic(RoleInGame victim, Car car, GetRandomPos grp, ref List<string> notifyMsg)
        {
            if (victim.confuseRecord.IsBeingControlled())
            {
                return true;
            }
            else
            {
                victim.confuseRecord.ControllSelf(victim, that, grp, ref notifyMsg, this);
                return victim.confuseRecord.IsBeingControlled();
            }
        }
        internal enum AmbushMagicType
        {
            waterMagic,
            electicMagic,
            fireMagic,
            attack
        }
    }
}
