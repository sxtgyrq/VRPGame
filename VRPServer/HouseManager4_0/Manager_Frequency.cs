using HouseManager4_0.RoomMainF;
using System;

namespace HouseManager4_0
{
    public class Manager_Frequency : Manager
    {

        public Manager_Frequency(RoomMain roomMain)
        {
            this.roomMain = roomMain;
        }
        /// <summary>
        /// 这里的100代表真实1/120 Hz，.
        /// 取100是为了方便计算整型。
        /// 100代表1/120hz,这里的2000，极值是1Hz(12000)。极限是2Hz(24000)
        /// </summary>
        int frequency = 100;
        DateTime lastFrequencyRecord = new DateTime(2021, 1, 1);
        /// <summary>
        /// Frequency 用于新玩家进入房间的时候选择新房间
        /// </summary>
        /// <returns></returns>
        internal int GetFrequency()
        {
            return this.frequency;
        }
        const int baseFrequency = 100;
        const int frequencyRecordCord = 100;
        public void addFrequencyRecord()
        {
            var calCulateT = DateTime.Now;
            var t = Math.Abs((calCulateT - this.lastFrequencyRecord).TotalSeconds);
            if (t <= 0.00001)
            {
                return;
            }
            var f = 1 / t;
            var fInt = Convert.ToInt32(f * 120 * 100);
            if (fInt > 240000)// this value equal 20Hz 
            {
                fInt = 240000;
            }
            this.frequency = (this.frequency * frequencyRecordCord + fInt) / (frequencyRecordCord + 1);
            //    var b = this.frequency + 0;
            if (this.frequency < 1)
            {
                this.frequency = 1;
            }
            this.lastFrequencyRecord = calCulateT;
        }
    }
}

//            for (int i = 0; i < modelsNeedToShow.Count; i++) 
//            {
//                if ( play).modelHasShowed.ContainsKey(cloesdMaterial[i].modelID)) { }
//                else
//                {
//                    var player = (Player)this;
//                    if (Program.dt.material.ContainsKey(cloesdMaterial[i].amodel))
//                    {
//                        if (player.aModelHasShowed.ContainsKey(cloesdMaterial[i].amodel))
//                        {
//                            var m2 = cloesdMaterial[i];
//                            ((Player)this).DrawObj3DModelF(player, m2.modelID, m2.x, m2.y, m2.z, m2.amodel, m2.rotatey, true, "", "", "", ref notifyMsgs);

//                        }
//                        else
//                        {
//                            player.aModelHasShowed.Add(cloesdMaterial[i].amodel, true);
//                            var m1 = Program.dt.material[cloesdMaterial[i].amodel];
//                            var m2 = cloesdMaterial[i];
//                            ((Player)this).DrawObj3DModelF(player, m2.modelID, m2.x, m2.y, m2.z, m2.amodel, m2.rotatey, false, m1.imageBase64, m1.objText, m1.mtlText, ref notifyMsgs);
//                        }
//                    }
//                    else
//                    { }
//                    ((Player)this).modelHasShowed.Add(cloesdMaterial[i].modelID, true);
//                }
//            }
//            if (player.playerType == RoleInGame.PlayerType.player)
//            {

//                //Dictionary<string, Dictionary<int, Model.SaveRoad.RoadInfo>> result;
//                Program.dt.GetData(out result);

//                var roads = result[roadCode];

//                var cloesdMaterial = getMaterialNearby(roads, Program.dt.models);

//                for (int i = 0; i < cloesdMaterial.Count; i++)
//                {

//                    if (((Player)this).modelHasShowed.ContainsKey(cloesdMaterial[i].modelID)) { }
//                    else
//                    {
//                        var player = (Player)this;
//                        if (Program.dt.material.ContainsKey(cloesdMaterial[i].amodel))
//                        {
//                            if (player.aModelHasShowed.ContainsKey(cloesdMaterial[i].amodel))
//                            {
//                                var m2 = cloesdMaterial[i];
//                                ((Player)this).DrawObj3DModelF(player, m2.modelID, m2.x, m2.y, m2.z, m2.amodel, m2.rotatey, true, "", "", "", ref notifyMsgs);

//                            }
//                            else
//                            {
//                                player.aModelHasShowed.Add(cloesdMaterial[i].amodel, true);
//                                var m1 = Program.dt.material[cloesdMaterial[i].amodel];
//                                var m2 = cloesdMaterial[i];
//                                ((Player)this).DrawObj3DModelF(player, m2.modelID, m2.x, m2.y, m2.z, m2.amodel, m2.rotatey, false, m1.imageBase64, m1.objText, m1.mtlText, ref notifyMsgs);
//                            }
//                        }
//                        else
//                        { }
//                        ((Player)this).modelHasShowed.Add(cloesdMaterial[i].modelID, true);
//                    }
//                }
//            }

//            // throw new NotImplementedException();
//        }
//    }
//}
