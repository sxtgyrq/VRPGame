using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace HouseManager4_0.RoomMainF
{
    public partial class RoomMain
    {
        //   AddNPC();

        internal void SetNPC()
        {
            //throw new NotImplementedException();
            //   AddNPC();
            this.NPCM.AddNPC(Program.rm, Program.dt);
            this.NPCM.ClearNPC();
            this.NPCM.ControlNPC(Program.dt);
        }



        public void GetNPCPosition(string key)
        {
            bool success;
            //  int OpenMore = -1;//第一次打开？
            var notifyMsgs = new List<string>();
            lock (this.PlayerLock)
            {
                if (this._Players.ContainsKey(key))
                {
                    if (this._Players[key].playerType == RoleInGame.PlayerType.NPC)
                    {
                        var npc = (NPC)this._Players[key];
                        var fp = Program.dt.GetFpByIndex(this._Players[key].StartFPIndex);
                        //var fromUrl = player.FromUrl;
                        //var webSocketID = player.WebSocketID;
                        //var carsNames = this._Players[getPosition.Key].CarsNames;
                        var playerName = this._Players[key].PlayerName;
                        /*
                         * 这已经走查过，在AddNewPlayer、UpdatePlayer时，others都进行了初始化
                         */
                        AddOtherPlayer(key, ref notifyMsgs);

                        GetAllCarInfomationsWhenInitialize(key, ref notifyMsgs);

                        SendMaxHolderInfoMation(npc, ref notifyMsgs);

                        var players = this._Players;
                        foreach (var item in players)
                        {
                            if (item.Value.TheLargestHolderKey == npc.Key)
                            {
                                // npc.TheLargestHolderKeyChanged(item.Key, item.Value.TheLargestHolderKey, item.Key, ref notifyMsgs);
                            }
                        }
                        success = true;

                    }
                    else
                        success = false;
                }
                else
                {
                    success = false;
                }
            }
            //   var notifyMsgs = GPResult.NotifyMsgs;
            Startup.sendSeveralMsgs(notifyMsgs); 
            if (success)
            {
                CheckAllPromoteState(key);
                CheckCollectState(key);
                sendCarAbilityState(key);
                sendCarStateAndPurpose(key);
            }
        }








    }
}
