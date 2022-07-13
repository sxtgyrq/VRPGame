using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager4_0.RoomMainF
{
    public partial class RoomMain
    {
        /// <summary>
        /// Frequency 用于新玩家进入房间的时候选择新房间
        /// </summary>
        /// <returns></returns>
        internal int GetFrequency()
        {
            return this.frequencyM.GetFrequency();
        }

    }
}
