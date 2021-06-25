using CommonClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager4_0.RoomMainF
{
    public partial class RoomMain : interfaceOfHM.Position
    {
        public int GetRandomPosition(bool withWeight)
        {
            int index;
            do
            {
                index = rm.Next(0, Program.dt.GetFpCount());
                if (withWeight)
                    if (Program.dt.GetFpByIndex(index).Weight + 1 < rm.Next(100))
                    {
                        continue;
                    }
            }
            while (this.FpIsUsing(index));
            return index;
        }



        public GetPositionResult GetPosition(GetPosition getPosition)
        {
            GetPositionResult result;

            int OpenMore = -1;//第一次打开？
            var notifyMsgs = new List<string>();
            lock (this.PlayerLock)
            {
                if (this._Players.ContainsKey(getPosition.Key))
                {
                    if (this._Players[getPosition.Key].playerType == RoleInGame.PlayerType.player)
                    {
                        var player = (Player)this._Players[getPosition.Key];
                        var fp = Program.dt.GetFpByIndex(this._Players[getPosition.Key].StartFPIndex);
                        var fromUrl = player.FromUrl;
                        var webSocketID = player.WebSocketID;
                        //var carsNames = this._Players[getPosition.Key].CarsNames;
                        var playerName = this._Players[getPosition.Key].PlayerName;
                        /*
                         * 这已经走查过，在AddNewPlayer、UpdatePlayer时，others都进行了初始化
                         */
                        AddOtherPlayer(getPosition.Key, ref notifyMsgs);
                        //   this.brokenParameterT1RecordChanged(getPosition.Key, getPosition.Key, this._Players[getPosition.Key].brokenParameterT1, ref notifyMsgs);
                        GetAllCarInfomationsWhenInitialize(getPosition.Key, ref notifyMsgs);
                        //getAllCarInfomations(getPosition.Key, ref notifyMsgs);
                        OpenMore = ((Player)this._Players[getPosition.Key]).OpenMore;

                        // var player = this._Players[getPosition.Key];
                        //var m2 = player.GetMoneyCanSave();

                        //    MoneyCanSaveChanged(player, m2, ref notifyMsgs);

                        SendPromoteCountOfPlayer("mile", player, ref notifyMsgs);
                        SendPromoteCountOfPlayer("business", player, ref notifyMsgs);
                        SendPromoteCountOfPlayer("volume", player, ref notifyMsgs);
                        SendPromoteCountOfPlayer("speed", player, ref notifyMsgs);

                        //   BroadCoastFrequency(player, ref notifyMsgs);
                        player.SetMoneyCanSave(player, ref notifyMsgs);

                        // player.RunSupportChangedF(ref notifyMsgs);
                        //player.this._Players[addItem.Key].SetMoneyCanSave = RoomMain.SetMoneyCanSave;
                        //MoneyCanSaveChanged(player, player.MoneyForSave, ref notifyMsgs);

                        SendMaxHolderInfoMation(player, ref notifyMsgs);

                        var players = this._Players;
                        foreach (var item in players)
                        {
                            if (item.Value.TheLargestHolderKey == player.Key)
                            {
                                player.TheLargestHolderKeyChanged(item.Key, item.Value.TheLargestHolderKey, item.Key, ref notifyMsgs);
                            }
                        }
                        var list = player.usedRoadsList;
                        for (var i = 0; i < list.Count; i++)
                        {
                            this.DrawSingleRoadF(player, list[i], ref notifyMsgs);
                        }

                        //this._Players[getPosition.Key];
                        ((Player)this._Players[getPosition.Key]).MoneyChanged(player, this._Players[getPosition.Key].Money, ref notifyMsgs);
                        ((Player)this._Players[getPosition.Key]).ShowLevelOfPlayerDetail(ref notifyMsgs);
                        result = new GetPositionResult()
                        {
                            Success = true,
                            //CarsNames = carsNames,
                            Fp = fp,
                            FromUrl = fromUrl,
                            NotifyMsgs = notifyMsgs,
                            WebSocketID = webSocketID,
                            PlayerName = playerName,
                            positionInStation = this._Players[getPosition.Key].positionInStation
                        };
                    }
                    else
                        result = new GetPositionResult()
                        {
                            Success = false
                        };
                }
                else
                {
                    result = new GetPositionResult()
                    {
                        Success = false
                    };
                }
            }

            if (OpenMore == 0)
            {
                CheckAllPromoteState(getPosition.Key);
                CheckCollectState(getPosition.Key);
                sendCarAbilityState(getPosition.Key);
                sendCarStateAndPurpose(getPosition.Key);
            }
            else if (OpenMore > 0)
            {
                CheckAllPromoteState(getPosition.Key);
                CheckCollectState(getPosition.Key);

                sendCarAbilityState(getPosition.Key);
                sendCarStateAndPurpose(getPosition.Key);
            }
            return result;
        }

        private void SendMaxHolderInfoMation(RoleInGame player, ref List<string> notifyMsgs)
        {
            foreach (var item in this._Players)
            {
                //  if (player.Key == item.Key) { }
                //else 
                {
                    if (item.Value.TheLargestHolderKey == item.Key)
                    {
                        this.TheLargestHolderKeyChanged(item.Key, player.Key, player.Key, ref notifyMsgs);
                    }
                }
            }
        }

        public class GetPositionResult
        {
            public bool Success { get; set; }
            public string FromUrl { get; set; }
            public int WebSocketID { get; set; }
            public Model.FastonPosition Fp { get; set; }
            //public string[] CarsNames { get; set; }
            public List<string> NotifyMsgs { get; set; }
            public string PlayerName { get; set; }
            public int positionInStation { get; set; }
        }
    }
}
