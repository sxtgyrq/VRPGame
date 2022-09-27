using System;
using System.Collections.Generic;
using System.Text;

namespace CommonClass
{
    public class Geography
    {
        public class getLengthOfTwoPoint
        {
            private const double EARTH_RADIUS = 6371393;
            private static double rad(double d)
            {
                return d * Math.PI / 180.0;
            }
            /// <summary>
            /// 获取两点的距离，单位为米
            /// </summary>
            /// <param name="lat1">第一位置的纬度</param>
            /// <param name="lng1">第一位置的经度</param>
            /// <param name="lat2">第二位置的纬度</param>
            /// <param name="lng2">第二位置的经度</param>
            /// <returns></returns>
            public static double GetDistance(double lat1, double lng1, double h1, double lat2, double lng2, double h2)
            {
                double radLat1 = rad(lat1);
                double radLat2 = rad(lat2);
                double a = radLat1 - radLat2;
                double b = rad(lng1) - rad(lng2);

                double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
                 Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
                s = s * EARTH_RADIUS;
                return Math.Sqrt(s * s + (h2 - h1) * (h2 - h1));
                return s;
            }
        }

        public class calculatBaideMercatorIndex
        {
            static double LongitudeK = 18.25621546434640;
            private const double EARTH_RADIUS = 6371393;

            static double MercatorGetXbyLongitude(double Longitude)
            {
                var Zoom = 19;
                return Math.Pow(2, LongitudeK + (Zoom - 19)) * Longitude / 360;
            }

            static double LatitudeE = 0.0822699;

            static double LatitudeKContent = 114737.187;

            static double MercatorGetYbyLatitude(double Lagitude)
            {
                var Zoom = 19;

                var r_lagitudu = (Lagitude) / 180.0 * Math.PI;
                return (Math.Log10(1.0 / Math.Cos(r_lagitudu) + Math.Tan(r_lagitudu))
                    + Math.Log10((1 - LatitudeE * Math.Sin(r_lagitudu)) / (1 + LatitudeE * Math.Sin(r_lagitudu))) * LatitudeE / 2) * Math.Pow(2, (Zoom - 19)) * LatitudeKContent;
            }
            public static void getBaiduPicIndex(double longitude, double latitude, double height, out double x, out double y, out double z)
            {
                x = MercatorGetXbyLongitude(longitude);
                y = MercatorGetYbyLatitude(latitude);
                z = MercatorGetXbyLongitude(height * 360 / EARTH_RADIUS / (Math.PI * 2));
            }

            public static double getBaiduPositionLatWithAccuracy(double MercatorY, double accuracy)
            {

                var maxLat = 89.9;
                var minLat = -89.9;
                var midLat = (maxLat + minLat) / 2;

                var maxM = MercatorGetYbyLatitude(maxLat);
                var midM = MercatorGetYbyLatitude(midLat);
                var minM = MercatorGetYbyLatitude(minLat);


                while (Math.Abs(midM - MercatorY) > accuracy)
                {
                    if (MercatorY > maxM)
                    {
                        return midLat;
                    }
                    else if (MercatorY > midM)
                    {
                        minLat = midLat;
                        midLat = (maxLat + minLat) / 2;
                        midM = MercatorGetYbyLatitude(midLat);
                        minM = MercatorGetYbyLatitude(minLat);
                    }
                    else if (MercatorY > minM)
                    {
                        maxLat = midLat;
                        midLat = (maxLat + minLat) / 2;
                        maxM = MercatorGetYbyLatitude(maxLat);
                        midM = MercatorGetYbyLatitude(midLat);
                    }
                    else
                    {
                        return midLat;
                    }
                }
                return midLat;
            }
            public static double getBaiduPositionLon(double MercatorX)
            {
                var Zoom = 19;
                return MercatorX * 360 / Math.Pow(2, LongitudeK + (Zoom - 19));
            }
        }
    }
}
