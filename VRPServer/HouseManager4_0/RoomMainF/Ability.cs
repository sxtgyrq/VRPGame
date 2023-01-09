using CommonClass;
using HouseManager4_0.interfaceTag;
using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager4_0.RoomMainF
{
    public partial class RoomMain : interfaceOfHM.Ability
    {
        public void AbilityChanged2_0(HasContactInfo player, Car car, ref List<string> notifyMsgs, string pType)
        {
            string fromUrl;
            int webSocketID;
            player.GetUrlAndWebsocket(out fromUrl, out webSocketID);
            var carIndexStr = car.IndexString;
            long costValue = 0;
            long sumValue = 1;
            switch (pType)
            {
                case "mile":
                    {
                        costValue = car.ability.costMiles;
                        sumValue = car.ability.mile;
                    }; break;
                case "business":
                    {
                        costValue = car.ability.costBusiness;
                        sumValue = car.ability.Business;
                    }; break;
                case "volume":
                    {
                        costValue = car.ability.costVolume;
                        sumValue = car.ability.Volume;
                    }; break;
                case "speed":
                    {
                        sumValue = car.ability.Speed;
                        costValue = car.ability.Speed;
                    }; break;
            }
            var obj = new BradCastAbility
            {
                c = "BradCastAbility",
                WebSocketID = webSocketID,
                pType = pType,
                carIndexStr = carIndexStr,
                costValue = costValue,
                sumValue = sumValue
            };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            notifyMsgs.Add(fromUrl);
            notifyMsgs.Add(json);
            //throw new NotImplementedException();
        }

        public string SetAbility(SetAbility sa)
        {
            if (string.IsNullOrEmpty(sa.pType))
            {
                return $"wrong pType:{sa.pType}";
            }
            else if (!(sa.pType == "mile" || sa.pType == "business" || sa.pType == "volume" || sa.pType == "speed"))
            {
                return $"wrong pType:{sa.pType}"; ;
            }
            else if (sa.count != 1 && sa.count != 2 && sa.count != 5 && sa.count != 10 && sa.count != 20 && sa.count != 50)
            {
                return $"wrong count:{sa.count}"; ;
            }

            else
            {
                sa.count = 1;
                List<string> notifyMsg = new List<string>();
                lock (this.PlayerLock)
                {
                    if (this._Players.ContainsKey(sa.Key))
                    {
                        var player = this._Players[sa.Key];
                        var car = player.getCar();
                        if (player.Bust)
                        {
                            WebNotify(player, "您已破产");
                            return $"{player.Key} go bust!";
#warning 这里要提示前台，已经进行破产清算了。
                        }
                        else if (player.playerType != RoleInGame.PlayerType.player)
                        {
                            return "wrong player!";
                        }
                        else
                        {
                            switch (sa.pType)
                            {
                                case "mile":
                                case "business":
                                case "volume":
                                case "speed":
                                    {
                                        if (player.PromoteDiamondCount[sa.pType] >= sa.count)
                                        {
                                            const double brokeProbability = 0.9683237857;
                                            bool broken = false;
                                            for (int i = 0; i < sa.count; i++)
                                            {
                                                var d = this.rm.NextDouble();
                                                if (d > brokeProbability)
                                                {
                                                    broken = true;
                                                }
                                                if (broken)
                                                {
                                                    break;
                                                }
                                            }
                                            if (broken)
                                            {
                                                car.ability.AbilityClear(sa.pType, player, car, ref notifyMsg);
                                                player.PromoteDiamondCount[sa.pType] -= sa.count;
                                                if (player.playerType == RoleInGame.PlayerType.player)
                                                {
                                                    SendPromoteCountOfPlayer(sa.pType, player.PromoteDiamondCount[sa.pType], (Player)player, ref notifyMsg);
                                                    this.WebNotify(player, "能力升级失败！");
                                                }
                                            }
                                            else
                                            {
                                                car.ability.AbilityAdd(sa.pType, sa.count, player, car, ref notifyMsg);

                                                player.PromoteDiamondCount[sa.pType] -= sa.count;
                                                if (player.playerType == RoleInGame.PlayerType.player)
                                                {
                                                    SendPromoteCountOfPlayer(sa.pType, player.PromoteDiamondCount[sa.pType], (Player)player, ref notifyMsg);
                                                    this.taskM.UseDiamondSuccess((Player)player);
                                                }
                                            }
                                        }
                                    }; break;
                            }
                        }
                    }
                    else
                    {
                        return $"not has player-{sa.Key}!";
                    }

                }
                Startup.sendSeveralMsgs(notifyMsg); 
                return "ok";
            }
        }

        public void SendPromoteCountOfPlayer(string pType, int count, HasContactInfo player, ref List<string> notifyMsgs)
        {
            if (!(pType == "mile" || pType == "business" || pType == "volume" || pType == "speed"))
            {

            }
            else
            {
                //var count = player.PromoteDiamondCount[pType];
                string fromUrl;
                int websocketID;
                player.GetUrlAndWebsocket(out fromUrl, out websocketID);
                var obj = new BradCastPromoteDiamondCount
                {
                    c = "BradCastPromoteDiamondCount",
                    count = count,
                    WebSocketID = websocketID,
                    pType = pType
                };
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                notifyMsgs.Add(fromUrl);
                notifyMsgs.Add(json);
            }
        }

        private void sendCarAbilityState(string key)
        {
            List<string> notifyMsg = new List<string>();
            var role = this._Players[key];
            if (role.playerType == RoleInGame.PlayerType.player)
            {
                var player = (Player)role;
                var car = player.getCar();
                AbilityChanged2_0(player, car, ref notifyMsg, "business");
                AbilityChanged2_0(player, car, ref notifyMsg, "volume");
                AbilityChanged2_0(player, car, ref notifyMsg, "mile");
                AbilityChanged2_0(player, car, ref notifyMsg, "speed");
            }
            Startup.sendSeveralMsgs(notifyMsg); 
        }
    }
}
