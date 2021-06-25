using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager4_0.interfaceOfHM
{
    interface Market
    {
        /// <summary>
        /// 当市场中价格发生变化时
        /// </summary>
        /// <param name="priceType"></param>
        /// <param name="value"></param>
        void priceChanged(string priceType, long value);
    }
}
