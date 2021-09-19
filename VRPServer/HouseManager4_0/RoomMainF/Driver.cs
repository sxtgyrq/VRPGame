using CommonClass;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace HouseManager4_0.RoomMainF
{
    public partial class RoomMain
    {
        public void SelectDriver(SetSelectDriver dm)
        {
            this.driverM.SelectDriver(dm);
        }
        public void DriverSelected(RoleInGame role, Car car, ref List<string> notifyMsg)
        {
            if (role.playerType == RoleInGame.PlayerType.player)
            {
                var player = (Player)role;
                if (car.ability.driver != null)
                {
                    var url = player.FromUrl;

                    DriverNotify mn = new DriverNotify()
                    {
                        c = "DriverNotify",
                        WebSocketID = player.WebSocketID,
                        index = car.ability.driver.Index,
                        name = car.ability.driver.Name,
                        race = car.ability.driver.race.ToString(),
                        sex = car.ability.driver.sex.ToString(),
                        skill1Index = car.ability.driver.skill1.Index,
                        skill1Name = car.ability.driver.skill1.skillName,
                        skill2Index = car.ability.driver.skill2.Index,
                        skill2Name = car.ability.driver.skill2.skillName,
                    };

                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(mn);
                    notifyMsg.Add(url);
                    notifyMsg.Add(sendMsg);
                }
            }

        }
    }
}
