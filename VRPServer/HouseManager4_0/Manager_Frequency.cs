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
        /// </summary>
        int frequency = 100;
        DateTime lastFrequencyRecord = new DateTime(2021, 1, 1);
        internal int GetFrequency()
        {
            return this.frequency;
        }
        const int baseFrequency = 100;
        const int frequencyRecordCord = 10;
        public void addFrequencyRecord()
        {
            var a = this.frequency + 0;
            var calCulateT = DateTime.Now;
            var t = Math.Abs((calCulateT - this.lastFrequencyRecord).TotalSeconds);
            var f = 1 / t;
            var fInt = Convert.ToInt32(f * 120 * 100);

            fInt = Math.Max(baseFrequency, fInt);
            this.frequency = (this.frequency * frequencyRecordCord + fInt) / (frequencyRecordCord + 1);
            var b = this.frequency + 0;
            this.lastFrequencyRecord = calCulateT;
        }
    }
}
