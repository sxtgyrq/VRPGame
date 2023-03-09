using CommonClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager4_0.RoomMainF
{
    public partial class RoomMain : interfaceOfHM.Car
    {

        public void SendStateOfCar(Player player, HouseManager4_0.Car car, ref List<string> notifyMsg)
        {
            // lock (car.countStamp)
            {
                var carIndexStr = car.IndexString;
                var obj = new BradCarState
                {
                    c = "BradCarState",
                    WebSocketID = player.WebSocketID,
                    State = car.state.ToString(),
                    carID = carIndexStr,
                    countStamp = car.countStamp++
                };
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                notifyMsg.Add(player.FromUrl);
                notifyMsg.Add(json);
            }
        }

        public void SetAnimateStepChanged(RoleInGame player, Car car, ref List<string> notifyMsg)
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

        private void addPlayerCarRecord_bak(Player self, RoleInGame other, ref List<string> msgsWithUrl)
        {
            //这是发送给self的消息
            //throw new NotImplementedException(); 
            /*
            if (self.othersContainsKey(other.Key))
            {
                //for (var indexOfCar = 0; indexOfCar < 5; indexOfCar++)
                {
                    if (self.GetOthers(other.Key).getCarState() == other.getCar().changeState)
                    {

                    }
                    else
                    {
                        if (other.getCar().animateObj == null)
                        {

                        }
                        else
                        {
                            if (other.getCar().animateData.Count > 0)
                            {
                                var deltaT = (DateTime.Now - other.getCar().animateData[0].recordTime).TotalMilliseconds;
                                var currentHash = other.getCar().animateData[0].hasCode;
                                var result = new AnimationData
                                {
                                    deltaT = deltaT > int.MaxValue ? int.MaxValue : Convert.ToInt32(deltaT),
                                    animateData = other.getCar().animateData.animateData.ToArray(),
                                    startX = other.getCar().animateData.start.x,
                                    startY = other.getCar().animateData.start.y,
                                    isParking = other.getCar().animateData.isParking,
                                    currentHash = currentHash,
                                    previousHash = other.getCar().PreviousHash()
                                };
                            }





                            other.getCar().SetHashCode(currentHash);
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
*/
        }
        private void addPlayerCarRecord(Player self, RoleInGame other, ref List<string> msgsWithUrl)
        {
            //这是发送给self的消息
            //throw new NotImplementedException(); 

            if (self.othersContainsKey(other.Key))
            {
                if (self.GetOthers(other.Key).getCarState().md5 != other.getCar().changeState.md5)
                {
                    var deltaT = (DateTime.Now - other.getCar().animateObj.recordTime).TotalMilliseconds;
                    //  var currentMd5 = other.getCar().animateObj.Md5;

                    var animateData = new List<AnimationEncryptedItem>();
                    for (int i = 0; i < other.getCar().animateObj.animateDataItems.Length; i++)
                    {
                        AnimationEncryptedItem item = new AnimationEncryptedItem
                        {
                            dataEncrypted = other.getCar().animateObj.animateDataItems[i].dataEncrypted,
                            startT = other.getCar().animateObj.animateDataItems[i].startT,
                            privateKey = i < other.getCar().animateObj.LengthOfPrivateKeys ? other.getCar().animateObj.animateDataItems[i].privateKey : -1,
                            Md5Code = other.getCar().animateObj.animateDataItems[i].Md5Code,
                            isParking = other.getCar().animateObj.animateDataItems[i].isParking
                        };
                        animateData.Add(item);
                    }
                    var result = new AnimationData
                    {
                        deltaT = deltaT > int.MaxValue ? int.MaxValue : Convert.ToInt32(deltaT),
                        animateData = animateData.ToArray(),
                        currentMd5 = other.getCar().animateObj.Md5,
                        previousMd5 = other.getCar().animateObj.PreviousMd5,
                        privateKeys = other.getCar().animateObj.privateKeys
                    };
                    var obj = new BradCastAnimateOfOthersCar3
                    {
                        c = "BradCastAnimateOfOthersCar3",
                        Animate = result,
                        WebSocketID = self.WebSocketID,
                        carID = getCarName() + "_" + other.Key,
                        parentID = other.Key,
                    };
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                    msgsWithUrl.Add(self.FromUrl);
                    msgsWithUrl.Add(json);
                    self.GetOthers(other.Key).setCarState(other.getCar().changeState.md5, other.getCar().changeState.privatekeysLength);
                }
                else
                {
                    if (other.getCar().animateObj == null)
                    {

                    }
                    else
                    {
                        if (self.GetOthers(other.Key).getCarState().privatekeysLength != other.getCar().changeState.privatekeysLength)
                        {
                            var result = new AnimationKeyData
                            {
                                deltaT = 0,
                                privateKeyIndex = other.getCar().animateObj.LengthOfPrivateKeys - 1,
                                privateKeyValue = other.getCar().animateObj.animateDataItems[other.getCar().animateObj.LengthOfPrivateKeys - 1].privateKey,
                                currentMd5 = other.getCar().animateObj.Md5,
                                previousMd5 = "",
                            };

                            var obj = new BradCastAnimateOfOthersCar4
                            {
                                c = "BradCastAnimateOfOthersCar4",
                                Animate = result,
                                WebSocketID = self.WebSocketID,
                                carID = getCarName() + "_" + other.Key,
                                parentID = other.Key,
                            };
                            var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                            msgsWithUrl.Add(self.FromUrl);
                            msgsWithUrl.Add(json);
                            self.GetOthers(other.Key).setCarState(other.getCar().changeState.md5, other.getCar().changeState.privatekeysLength);
                        }
                    }
                    //   self.GetOthers(other.Key).setCarState(other.getCar().changeState);
                }
            }

        }

        private int GetHashCode(List<int> animateData)
        {
            return animateData.ToArray().GetHashCode();
            // throw new NotImplementedException();
        }

        private void addSelfCarSingleRecord_bak(Player self, Car car, ref List<string> msgsWithUrl)
        {
            //if (car.animateData == null)
            //{ }
            //else
            //{
            //    var deltaT = (DateTime.Now - car.animateData.recordTime).TotalMilliseconds;
            //    var currentHash = GetHashCode(car.animateData.animateData);
            //    var result = new AnimationData
            //    {
            //        deltaT = deltaT > int.MaxValue ? int.MaxValue : Convert.ToInt32(deltaT),
            //        animateData = car.animateData.animateData.ToArray(),
            //        startX = car.animateData.start.x,
            //        startY = car.animateData.start.y,
            //        isParking = car.animateData.isParking,
            //        currentHash = currentHash,
            //        previousHash = car.PreviousHash()
            //    };
            //    car.SetHashCode(currentHash);
            //    var obj = new BradCastAnimateOfSelfCar
            //    {
            //        c = "BradCastAnimateOfSelfCar",
            //        Animate = result,
            //        WebSocketID = self.WebSocketID,
            //        carID = getCarName() + "_" + self.Key,
            //        parentID = self.Key,
            //        CostMile = car.ability.costMiles,

            //    };
            //    var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            //    msgsWithUrl.Add(self.FromUrl);
            //    msgsWithUrl.Add(json);
            //}
        }
        private void addSelfCarSingleRecord(Player self, Car car, ref List<string> msgsWithUrl)
        {
            if (car.animateObj == null)
            { }
            else
            {
                if (car.WebSelf.md5 != car.animateObj.Md5)
                {
                    var deltaT = (DateTime.Now - car.animateObj.recordTime).TotalMilliseconds;

                    var currentMd5 = self.getCar().animateObj.Md5;

                    var animateData = new List<AnimationEncryptedItem>();
                    for (int i = 0; i < self.getCar().animateObj.animateDataItems.Length; i++)
                    {
                        AnimationEncryptedItem item = new AnimationEncryptedItem
                        {
                            dataEncrypted = self.getCar().animateObj.animateDataItems[i].dataEncrypted,
                            startT = self.getCar().animateObj.animateDataItems[i].startT,
                            privateKey = i < self.getCar().animateObj.LengthOfPrivateKeys ? self.getCar().animateObj.animateDataItems[i].privateKey : -1,
                            Md5Code = self.getCar().animateObj.animateDataItems[i].Md5Code,
                            isParking = self.getCar().animateObj.animateDataItems[i].isParking
                        };
                        animateData.Add(item);
                    }
                    var result = new AnimationData
                    {
                        deltaT = deltaT > int.MaxValue ? int.MaxValue : Convert.ToInt32(deltaT),
                        animateData = animateData.ToArray(),
                        currentMd5 = self.getCar().animateObj.Md5,
                        previousMd5 = self.getCar().animateObj.PreviousMd5,
                        privateKeys = self.getCar().animateObj.privateKeys
                    };
                    var obj = new BradCastAnimateOfOthersCar3
                    {
                        c = "BradCastAnimateOfOthersCar3",
                        Animate = result,
                        WebSocketID = self.WebSocketID,
                        carID = getCarName() + "_" + self.Key,
                        parentID = self.Key,
                        //   passPrivateKeysOnly = false
                    };
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                    msgsWithUrl.Add(self.FromUrl);
                    msgsWithUrl.Add(json);
                    self.getCar().WebSelf = new Car.ChangeStateC()
                    {
                        md5 = car.animateObj.Md5,
                        privatekeysLength = self.getCar().animateObj.LengthOfPrivateKeys
                    };
                }
                else if (car.WebSelf.privatekeysLength != car.animateObj.LengthOfPrivateKeys)
                {
                    var result = new AnimationKeyData
                    {
                        deltaT = 0,
                        privateKeyIndex = self.getCar().animateObj.LengthOfPrivateKeys - 1,
                        privateKeyValue = self.getCar().animateObj.animateDataItems[self.getCar().animateObj.LengthOfPrivateKeys - 1].privateKey,
                        currentMd5 = self.getCar().animateObj.Md5,
                        previousMd5 = "",
                    };
                    var obj = new BradCastAnimateOfOthersCar4
                    {
                        c = "BradCastAnimateOfOthersCar4",
                        Animate = result,
                        WebSocketID = self.WebSocketID,
                        carID = getCarName() + "_" + self.Key,
                        parentID = self.Key,
                    };
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                    msgsWithUrl.Add(self.FromUrl);
                    msgsWithUrl.Add(json);
                    self.getCar().WebSelf = new Car.ChangeStateC()
                    {
                        md5 = car.animateObj.Md5,
                        privatekeysLength = self.getCar().animateObj.LengthOfPrivateKeys
                    };
                }
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
            if (self.getCar().animateObj == null)
            {

            }
            else
            {
#warning 这里没完成！
                // var deltaT = 0;
                //var currentHash = GetHashCode(self.getCar().animateData.animateData);
                //var result = new AnimationData
                //{
                //    deltaT = 0,
                //    animateData = self.getCar().animateData.animateData.ToArray(),
                //    startX = self.getCar().animateData.start.x,
                //    startY = self.getCar().animateData.start.y,
                //    isParking = self.getCar().animateData.isParking,
                //    currentHash = currentHash,
                //    previousHash = self.getCar().PreviousHash()
                //};
                //self.getCar().SetHashCode(currentHash);
                //var obj = new BradCastAnimateOfSelfCar
                //{
                //    c = "BradCastAnimateOfSelfCar",
                //    Animate = result,
                //    WebSocketID = self.WebSocketID,
                //    carID = getCarName() + "_" + self.Key,
                //    parentID = self.Key,
                //    CostMile = self.getCar().ability.costMiles,
                //};
                //var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                //msgsWithUrl.Add(self.FromUrl);
                //msgsWithUrl.Add(json);
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
            Startup.sendSeveralMsgs(notifyMsg);
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
