using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleTestAPP
{
    internal class ModelScaleCal
    {
        internal static void Test()
        {
            var d = CityRunFunction.Geography.getLengthOfTwoPoint.GetDistance(32, 112, 33, 113);
            //CityRunFunction.Geography.
            double startX, startY;
            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(112, 32, out startX, out startY);

            //var length=

            double endX, endY;
            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(113, 33, out endX, out endY);

            var vitualLength = Math.Sqrt((endX - startX) * (endX - startX) + (endY - startY) * (endY - startY));
            var result = vitualLength / d;

            Console.WriteLine($"比例为：{result}");
            Console.ReadLine();
        }
    }
}
