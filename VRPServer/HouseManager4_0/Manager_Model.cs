using HouseManager4_0.RoomMainF;
using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager4_0
{
    public class Manager_Model : Manager
    {
        public Manager_Model(RoomMain roomMain)
        {
            this.roomMain = roomMain;
        }
        internal void setModels(RoleInGame roleInGame, List<Data.detailmodel> cloesdMaterial, ref List<string> notifyMsgs)
        {
            if (roleInGame.playerType == RoleInGame.PlayerType.player)
            {
                var player = (Player)roleInGame;
                for (int i = 0; i < cloesdMaterial.Count; i++)
                {
                    if (player.modelHasShowed.ContainsKey(cloesdMaterial[i].modelID)) { }
                    else
                    {
                        // var player = (Player)this;
                        if (Program.dt.material.ContainsKey(cloesdMaterial[i].amodel))
                        {
                            //if (player.aModelHasShowed.ContainsKey(cloesdMaterial[i].amodel))
                            //{
                            //    var m2 = cloesdMaterial[i];
                            //    player.DrawObj3DModelF(player, m2.modelID, m2.x, m2.y, m2.z, m2.amodel, m2.rotatey, true, "", "", "", ref notifyMsgs);

                            //}
                            //else
                            {
                               // player.aModelHasShowed.Add(cloesdMaterial[i].amodel, true);
                                var m1 = Program.dt.material[cloesdMaterial[i].amodel];
                                var m2 = cloesdMaterial[i];
                                player.DrawObj3DModelF(player, m2.modelID, m2.x, m2.y, m2.z, m2.amodel, m2.rotatey, false, m1.imageBase64, m1.objText, m1.mtlText, ref notifyMsgs);
                            }
                        }
                        else
                        { }
                        player.modelHasShowed.Add(cloesdMaterial[i].modelID, true);
                    }
                }
            }

        }
    }
}
