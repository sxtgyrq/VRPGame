﻿using CommonClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager4_0.RoomMainF
{
    public partial class RoomMain : interfaceOfHM.Car
    {
        public void SendStateOfCar(Player player, HouseManager4_0.Car car, ref List<string> notifyMsg)
        {
            var carIndexStr = car.IndexString;
            var obj = new BradCarState
            {
                c = "BradCarState",
                WebSocketID = player.WebSocketID,
                State = car.state.ToString(),
                carID = carIndexStr
            };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            notifyMsg.Add(player.FromUrl);
            notifyMsg.Add(json);
        }

        public void SetAnimateChanged(RoleInGame player, Car car, ref List<string> notifyMsg)
        {
            lock (this.PlayerLock)
            {
                var key = player.Key;
                var players = getGetAllRoles();
                for (var i = 0; i < players.Count; i++)
                {
                    if (players[i].Key == key)
                    {

                    }
                    else
                    {
                        {
                            /*
                             * 告诉自己，场景中有哪些别人的车！
                             * 告诉别人，场景中有哪些车是我的的！
                             */
                            {
                                var self = this._Players[key];
                                var other = players[i];
                                if (self.playerType == RoleInGame.PlayerType.player)
                                    addPlayerCarRecord((Player)self, other, ref notifyMsg);

                            }
                            {
                                var self = players[i];
                                var other = this._Players[key];
                                if (self.playerType == RoleInGame.PlayerType.player)
                                    addPlayerCarRecord((Player)self, other, ref notifyMsg);
                            }

                        }
                    }
                }
                {
                    var self = this._Players[key];
                    if (self.playerType == RoleInGame.PlayerType.player)
                        addSelfCarSingleRecord((Player)self, car, ref notifyMsg);
                }
            }
        }

        private void addPlayerCarRecord(Player self, RoleInGame other, ref List<string> msgsWithUrl)
        {
            //这是发送给self的消息
            //throw new NotImplementedException();
            if (self.othersContainsKey(other.Key))
            {
                //for (var indexOfCar = 0; indexOfCar < 5; indexOfCar++)
                {
                    if (self.GetOthers(other.Key).getCarState() == other.getCar().changeState)
                    {

                    }
                    else
                    {
                        if (other.getCar().animateData == null)
                        {

                        }
                        else
                        {
                            var deltaT = (DateTime.Now - other.getCar().animateData.recordTime).TotalMilliseconds;
                            var result = new
                            {
                                deltaT = deltaT > Int32.MaxValue ? Int32.MaxValue : Convert.ToInt32(deltaT),
                                animateData = other.getCar().animateData.animateData,
                                start = other.getCar().animateData.start,
                                isParking = other.getCar().animateData.isParking
                            };
                            var obj = new BradCastAnimateOfOthersCar2
                            {
                                c = "BradCastAnimateOfOthersCar2",
                                Animate = result,
                                WebSocketID = self.WebSocketID,
                                carID = getCarName() + "_" + other.Key,
                                parentID = other.Key,

                            };
                            var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                            msgsWithUrl.Add(self.FromUrl);
                            msgsWithUrl.Add(json);
                        }
                        self.GetOthers(other.Key).setCarState(other.getCar().changeState);
                    }
                }
            }
        }

        private void addSelfCarSingleRecord(Player self, Car car, ref List<string> msgsWithUrl)
        {
            if (car.animateData == null)
            { }
            else
            {
                var deltaT = (DateTime.Now - car.animateData.recordTime).TotalMilliseconds;
                var result = new
                {
                    deltaT = deltaT > Int32.MaxValue ? Int32.MaxValue : Convert.ToInt32(deltaT),
                    animateData = car.animateData.animateData,
                    start = car.animateData.start,
                    isParking = car.animateData.isParking
                };
                var obj = new BradCastAnimateOfSelfCar
                {
                    c = "BradCastAnimateOfSelfCar",
                    Animate = result,
                    WebSocketID = self.WebSocketID,
                    carID = getCarName() + "_" + self.Key,
                    parentID = self.Key,
                    CostMile = car.ability.costMiles,

                };
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                msgsWithUrl.Add(self.FromUrl);
                msgsWithUrl.Add(json);
            }
        }
        private string getCarName()
        {
            return "car";
        }

        /// <summary>
        /// 广播小车状态
        /// </summary>
        /// <param name="key"></param>
        /// <param name="msgsWithUrl"></param>
        private void GetAllCarInfomationsWhenInitialize(string key, ref List<string> msgsWithUrl)
        {
            var players = getGetAllRoles();
            for (var i = 0; i < players.Count; i++)
            {
                if (players[i].Key == key)
                {

                }
                else
                {
                    {
                        /*
                         * 告诉自己，场景中有哪些别人的车！
                         * 告诉别人，场景中有哪些车是我的的！
                         */
                        {
                            var self = this._Players[key];
                            var other = players[i];
                            if (self.playerType == RoleInGame.PlayerType.player)
                                addPlayerCarRecord((Player)self, other, ref msgsWithUrl);

                        }
                        {
                            var self = players[i];
                            var other = this._Players[key];
                            if (self.playerType == RoleInGame.PlayerType.player)
                                addPlayerCarRecord((Player)self, other, ref msgsWithUrl);
                        }

                    }
                }
            }
            {
                var self = this._Players[key];
                if (self.playerType == RoleInGame.PlayerType.player)
                    addSelfCarRecord((Player)self, ref msgsWithUrl);
            }
        }

        private void addSelfCarRecord(Player self, ref List<string> msgsWithUrl)
        {
            //  for (var indexOfCar = 0; indexOfCar < 5; indexOfCar++)
            if (self.getCar().animateData == null)
            {

            }
            else
            {
                var deltaT = (DateTime.Now - self.getCar().animateData.recordTime).TotalMilliseconds;
                var result = new
                {
                    deltaT = deltaT>Int32.MaxValue?Int32.MaxValue:Convert.ToInt32(deltaT),
                    animateData = self.getCar().animateData.animateData,
                    start = self.getCar().animateData.start,
                    isParking = self.getCar().animateData.isParking,
                };
                var obj = new BradCastAnimateOfSelfCar
                {
                    c = "BradCastAnimateOfSelfCar",
                    Animate = result,
                    WebSocketID = self.WebSocketID,
                    carID = getCarName() + "_" + self.Key,
                    parentID = self.Key,
                    CostMile = self.getCar().ability.costMiles,

                };
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                msgsWithUrl.Add(self.FromUrl);
                msgsWithUrl.Add(json);
            }
        }

        private void sendCarStateAndPurpose(string key)
        {
            List<string> notifyMsg = new List<string>();
            var player = this._Players[key];
            {
                var car = this._Players[key].getCar();
                if (player.playerType == RoleInGame.PlayerType.player)
                    SendStateOfCar((Player)player, car, ref notifyMsg);
                if (player.playerType == RoleInGame.PlayerType.player)
                    SendPurposeOfCar((Player)player, car, ref notifyMsg);
            }
            for (var i = 0; i < notifyMsg.Count; i += 2)
            {
                var url = notifyMsg[i];
                var sendMsg = notifyMsg[i + 1];
                Startup.sendMsg(url, sendMsg);
            }
        }

        void SendPurposeOfCar(Player player, Car car, ref List<string> notifyMsg)
        {
            //var carIndexStr = car.IndexString;

            //var obj = new BradCarPurpose
            //{
            //    c = "BradCarPurpose",
            //    WebSocketID = player.WebSocketID,
            //    Purpose = car.purpose.ToString(),
            //    carID = carIndexStr
            //};
            //var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            //notifyMsg.Add(player.FromUrl);
            //notifyMsg.Add(json);
        }
    }
}