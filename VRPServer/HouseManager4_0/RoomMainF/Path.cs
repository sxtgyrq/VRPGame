using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using OssModel = Model;

namespace HouseManager4_0.RoomMainF
{
    public partial class RoomMain : interfaceOfHM.Path
    {
        public List<OssModel.MapGo.nyrqPosition> GetAFromB(int from, int to, RoleInGame player, ref List<string> notifyMsgs)
        {
            var path = Program.dt.GetAFromB(from, to);
            for (var i = 0; i < path.Count; i++)
            {
                player.addUsedRoad(path[i].roadCode, ref notifyMsgs);
            }
            return path;
        }

        public int GetMile(List<Model.MapGo.nyrqPosition> path)
        {
            double sumMiles = 0;
            for (var i = 1; i < path.Count; i++)
            {
                sumMiles += CommonClass.Geography.getLengthOfTwoPoint.GetDistance(path[i].BDlatitude, path[i].BDlongitude, path[i - 1].BDlatitude, path[i - 1].BDlongitude);
            }
            return Convert.ToInt32(sumMiles) / 1000;
        }

        //public void getStartPositionByFp(out Data.PathStartPoint2 startPosition, Model.FastonPosition fp1)
        //{
        //    double startX, startY;
        //    CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(fp1.Longitude, fp1.Latitde, out startX, out startY);
        //    startPosition = new Data.PathStartPoint2()
        //    {
        //        x = Convert.ToInt32(startX * 256),
        //        y = Convert.ToInt32(startY * 256)
        //    };
        //}

        /// <summary>
        /// 获取从基地出来时的路径！
        /// </summary>
        /// <param name="fp">初始地点</param>
        /// <param name="car">carA？-carE</param>
        /// <param name="startTInput">时间</param>
        /// <returns></returns>
        public List<int> getStartPositon(Model.FastonPosition fp, int positionInStation, ref int startTInput, out Data.PathStartPoint2 startPosition)
        {
            double startX, startY;
            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(fp.Longitude, fp.Latitde, out startX, out startY);
            int startT0, startT1;

            double endX, endY;
            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(fp.positionLongitudeOnRoad, fp.positionLatitudeOnRoad, out endX, out endY);
            int endT0, endT1;

            //这里要考虑前台坐标系（左手坐标系）。
            var cc = new Complex(endX - startX, (-endY) - (-startY));

            cc = ToOne(cc);

            var positon1 = cc * (new Complex(-0.309016994, 0.951056516));
            var positon2 = positon1 * (new Complex(0.809016994, 0.587785252));
            var positon3 = positon2 * (new Complex(0.809016994, 0.587785252));
            var positon4 = positon3 * (new Complex(0.809016994, 0.587785252));
            var positon5 = positon4 * (new Complex(0.809016994, 0.587785252));
            Complex position;

            switch (positionInStation)
            {
                case 0:
                    {
                        position = positon1;
                    }; break;
                case 1:
                    {
                        position = positon2;
                    }; break;
                case 2:
                    {
                        position = positon3;
                    }; break;
                case 3:
                    {
                        position = positon4;
                    }; break;
                case 4:
                    {
                        position = positon5;
                    }; break;
                default:
                    {
                        position = positon1;
                    }; break;
            }
            var percentOfPosition = 0.25;
            double carPositionX = startX + position.Real * percentOfPosition;
            double carPositionY = startY - position.Imaginary * percentOfPosition;

            startPosition = new Data.PathStartPoint2()
            {
                x = Convert.ToInt32(carPositionX * 256),
                y = Convert.ToInt32(carPositionY * 256)
            };

            List<int> animateResult = new List<int>();
            startT0 = startTInput;
            endT0 = startT0 + 500;
            startTInput += 500;
            var animate1 = new Data.PathResult3()
            {
                x = Convert.ToInt32(-(position.Real * percentOfPosition) * 256),
                y = Convert.ToInt32(position.Imaginary * percentOfPosition * 256),
                t = endT0 - startT0
            };
            //var animate1 = new Data.PathResult()
            //{
            //    t0 = startT0,
            //    x0 = carPositionX,
            //    y0 = carPositionY,
            //    t1 = endT0,
            //    x1 = startX,
            //    y1 = startY
            //};
            if (animate1.t != 0)
            {
                animateResult.Add(animate1.x);
                animateResult.Add(animate1.y);
                animateResult.Add(animate1.t);
            }
            // animateResult.Add(animate1);
            /*
             * 上道路的速度为10m/s 即36km/h
             */
            var interview = Convert.ToInt32(CommonClass.Geography.getLengthOfTwoPoint.GetDistance(fp.Latitde, fp.Longitude, fp.positionLatitudeOnRoad, fp.positionLongitudeOnRoad) / 10 * 1000);
            startT1 = startTInput;
            endT1 = startT1 + interview;
            startTInput += interview;

            var animate2 = new Data.PathResult3()
            {
                x = Convert.ToInt32((endX - startX) * 256),
                y = Convert.ToInt32((endY - startY) * 256),
                t = interview
            };
            //var animate2 = new Data.PathResult()
            //{
            //    t0 = startT1,
            //    x0 = startX,
            //    y0 = startY,
            //    t1 = endT1,
            //    x1 = endX,
            //    y1 = endY
            //};
            if (animate2.t != 0)
            {
                animateResult.Add(animate2.x);
                animateResult.Add(animate2.y);
                animateResult.Add(animate2.t);
            }
            //  animateResult.Add(animate2);
            return animateResult;
        }

        private Complex ToOne(Complex cc)
        {
            var m = Math.Sqrt(cc.Real * cc.Real + cc.Imaginary * cc.Imaginary);
            return new Complex(cc.Real / m, cc.Imaginary / m);
        }


        public void getStartPositionByGoPath(out Data.PathStartPoint2 startPosition, List<Model.MapGo.nyrqPosition> goPath)
        {
            var firstPosition = goPath.First();
            double startX, startY;
            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(firstPosition.BDlongitude, firstPosition.BDlatitude, out startX, out startY);
            startPosition = new Data.PathStartPoint2()
            {
                x = Convert.ToInt32(startX * 256),
                y = Convert.ToInt32(startY * 256)
            };
        }

        public void getEndPositon(Model.FastonPosition fp, int initPosition, ref List<int> animateResult, ref int startTInput)
        {
            if (initPosition > 5)
            {
                initPosition = initPosition % 5;
            }
            double endX, endY;
            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(fp.Longitude, fp.Latitde, out endX, out endY);
            int startT1;

            double startX, startY;
            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(fp.positionLongitudeOnRoad, fp.positionLatitudeOnRoad, out startX, out startY);
            int endT1;

            //这里要考虑前台坐标系（左手坐标系）。
            var cc = new Complex(startX - endX, (-startY) - (-endY));

            cc = ToOne(cc);

            var positon1 = cc * (new Complex(-0.309016994, 0.951056516));
            var positon2 = positon1 * (new Complex(0.809016994, 0.587785252));
            var positon3 = positon2 * (new Complex(0.809016994, 0.587785252));
            var positon4 = positon3 * (new Complex(0.809016994, 0.587785252));
            var positon5 = positon4 * (new Complex(0.809016994, 0.587785252));
            Complex position;
            switch (initPosition)
            {
                case 0:
                    {
                        position = positon1;
                    }; break;
                case 1:
                    {
                        position = positon2;
                    }; break;
                case 2:
                    {
                        position = positon3;
                    }; break;
                case 3:
                    {
                        position = positon4;
                    }; break;
                case 4:
                    {
                        position = positon5;
                    }; break;
                default:
                    {
                        position = positon1;
                    }; break;
            }
            var percentOfPosition = 0.25;
            double carPositionX = endX + position.Real * percentOfPosition;
            double carPositionY = endY - position.Imaginary * percentOfPosition;


            /*
             * 这里由于是返程，为了与getStartPositon 中的命名保持一致性，（位置上）end实际为start,时间上还保持一致
             */
            //  List<Data.PathResult> animateResult = new List<Data.PathResult>();

            /*
             * 上道路的速度为10m/s 即36km/h
             */
            var interview = Convert.ToInt32(CommonClass.Geography.getLengthOfTwoPoint.GetDistance(fp.Latitde, fp.Longitude, fp.positionLatitudeOnRoad, fp.positionLongitudeOnRoad) / 10 * 1000);
            startT1 = startTInput;
            endT1 = startT1 + interview;
            startTInput += interview;
            var animate2 = new Data.PathResult3()
            {
                x = Convert.ToInt32((endX - startX) * 256),
                y = Convert.ToInt32((endY - startY) * 256),
                t = interview
            };
            //var animate2 = new Data.PathResult()
            //{
            //    t0 = startT1,
            //    x0 = startX,
            //    y0 = startY,
            //    t1 = endT1,
            //    x1 = endX,
            //    y1 = endY
            //};
            //animateResult.Add(animate2);
            if (animate2.t != 0)
            {
                animateResult.Add(animate2.x);
                animateResult.Add(animate2.y);
                animateResult.Add(animate2.t);
            }

            //  startT0 = startTInput;
            //    endT0 = startT0 + 500;
            startTInput += 500;

            var animate1 = new Data.PathResult3()
            {
                x = Convert.ToInt32((carPositionX - endX) * 256),
                y = Convert.ToInt32((carPositionY - endY) * 256),
                t = 500
            };
            //var animate1 = new Data.PathResult()
            //{
            //    t0 = startT0,
            //    x0 = endX,
            //    y0 = endY,
            //    t1 = endT0,
            //    x1 = carPositionX,
            //    y1 = carPositionY
            //};
            //  animateResult.Add(animate1);
            if (animate1.t != 0)
            {
                animateResult.Add(animate1.x);
                animateResult.Add(animate1.y);
                animateResult.Add(animate1.t);
            }

        }
    }
}
