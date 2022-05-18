using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager4_0.RoomMainF
{
    public partial class RoomMain
    {
        /// <summary>
        /// 新增其他玩家信息，且这些信息是用于web前台展现的。不能用于战斗计分。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="msgsWithUrl"></param>
        private void AddOtherPlayer(string key, ref List<string> msgsWithUrl)
        {
            var players = getGetAllRoles();
            for (var i = 0; i < players.Count; i++)
            {
                if (players[i].Key == key)
                {
                    /*
                     * 保证自己不会算作其他人
                     */
                }
                else
                {
                    {
                        /*
                         * 告诉自己，场景中有哪些人！
                         * 告诉场景中的其他人，场景中有我！
                         */
                        {
                            var self = this._Players[key];
                            var other = players[i];
                            addPlayerRecord(self, other, ref msgsWithUrl);

                        }
                        {
                            var self = players[i];
                            var other = this._Players[key];
                            addPlayerRecord(self, other, ref msgsWithUrl);
                        }
                    }
                }
            }


        }

        private void addPlayerRecord(RoleInGame self, RoleInGame other, ref List<string> msgsWithUrl)
        {
            if (self.Key == other.Key)
            { 
                return;
            }
            else if (self.othersContainsKey(other.Key))
            { 
            }
            else
            {
                var otherPlayer = new OtherPlayers(self.Key, other.Key);
                //   otherPlayer.brokenParameterT1RecordChangedF = self.brokenParameterT1RecordChanged;
                self.othersAdd(other.Key, otherPlayer);
                //otherPlayer.setBrokenParameterT1Record(other.brokenParameterT1, ref msgsWithUrl);

                var fp = Program.dt.GetFpByIndex(other.StartFPIndex);
                // fromUrl = this._Players[getPosition.Key].FromUrl;
                if (self.playerType == RoleInGame.PlayerType.player)
                {
                    var webSocketID = ((Player)self).WebSocketID;
                    //var carsNames = other.ca;

                    //  var fp=  players[i].StartFPIndex
                    CommonClass.GetOthersPositionNotify_v2 notify = new CommonClass.GetOthersPositionNotify_v2()
                    {
                        c = "GetOthersPositionNotify_v2",
                        fp = fp,
                        WebSocketID = webSocketID,
                        key = other.Key,
                        PlayerName = other.PlayerName,
                        fPIndex = other.StartFPIndex,
                        positionInStation = other.positionInStation,
                        isNPC = other.playerType == RoleInGame.PlayerType.NPC,
                        isPlayer = other.playerType == RoleInGame.PlayerType.player,
                        Level = other.Level

                        // var xx=  getPosition.Key
                    };
                    msgsWithUrl.Add(((Player)self).FromUrl);
                    msgsWithUrl.Add(Newtonsoft.Json.JsonConvert.SerializeObject(notify));
                }
            }


        }

    }
}
