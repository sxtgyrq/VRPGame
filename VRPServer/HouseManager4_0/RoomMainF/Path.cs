using CommonClass;
using Model;
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
        /// <summary>
        /// 返回路径，并且按照路口（cross)进行拆分
        /// </summary>
        /// <param name="from">起始地点</param>
        /// <param name="to">目标地点</param>
        /// <param name="player"></param>
        /// <param name="notifyMsgs"></param>
        /// <returns></returns>
        List<OssModel.MapGo.nyrqPosition> GetAFromB_Path(int from, int to, RoleInGame player, GetRandomPos grp, ref List<string> notifyMsgs)
        {
            var path = grp.GetAFromB(from, to);
            for (var i = 0; i < path.Count; i++)
            {
                player.addUsedRoad(path[i].roadCode, ref notifyMsgs);
            }

            return path;
        }
        public class Node
        {
            public class direction
            {
                public OssModel.MapGo.nyrqPosition start { get; set; }
                public OssModel.MapGo.nyrqPosition end { get; set; }
                public bool right { get; set; }
                public bool isTheEndOfTheRoad { get; set; }
            }
            public List<pathItem> path { get; set; }
            public class pathItem
            {
                public class Postion
                {
                    public double longitude { get; set; }
                    public double latitude { get; set; }
                    public double height { get; set; }
                    public string crossKey { get; set; }

                    public string postionCrossKey { get { return $"{longitude.ToString("F5")},{latitude.ToString("F5")}"; } }


                }
                /// <summary>
                /// 供选择的矢量，这里的start是矢量的起点，并不是选择的起点。end为矢量的终点。
                /// </summary>
                public List<direction> selections { get; set; }
                /// <summary>
                /// 基于选择的中心点
                /// </summary>
                public Postion selectionCenter { get; set; }
                public List<OssModel.MapGo.nyrqPosition> path { get; set; }
                /// <summary>
                /// 这个可以理解为线路的起点
                /// </summary>
                public OssModel.MapGo.nyrqPosition position { get; set; }
            }
        }
        public class CalCross
        {
            public SaveRoad.DictCross cross { get; set; }
            public int calType { get; set; }
        }

        public Node GetAFromB_v2(int from, int to, RoleInGame player, GetRandomPos grp, ref List<string> notifyMsgs)
        {
            //from = 209;
            //to = 444;
            // throw new Exception("");
            int cursor = 0;//光标所在位置

            var path = this.GetAFromB_Path(from, to, player, grp, ref notifyMsgs);


            //for (int i = 0; i < path.Count; i++)
            //{
            //    //Consol.WriteLine($"{path[i].BDlongitude},{path[i].BDlatitude}");
            //} 
            //Console.ReadLine();
            if (path.Count > 1)
            {
                var lastPoint = path[0];//第一个点作为起点
                var node = new Node()
                {
                    path = new List<Node.pathItem>()
                };//初始化node
                for (var indexOfPath = 0; indexOfPath < path.Count; indexOfPath++)
                {

                    if (indexOfPath + 1 < path.Count)
                    {
                        if (indexOfPath == 0)
                        {
                            /*
                             * 第一个点
                             */
                            var firstRoad = grp.GetItemRoadInfo(path[0]);
                            if (firstRoad.CarInOpposeDirection == 1)
                            {
                                var right = new Node.direction()
                                {
                                    right = true,
                                    start = path[indexOfPath],
                                    end = path[indexOfPath + 1]
                                };
                                var wrong = new Node.direction()
                                {
                                    right = false,
                                    start = path[indexOfPath + 1],
                                    end = path[indexOfPath],
                                };
                                List<Node.direction> selections;
                                if (FoundCross(wrong, path[indexOfPath], grp))
                                {
                                    selections = new List<Node.direction>()
                                    {
                                        right, wrong
                                    };
                                }
                                else
                                {
                                    selections = new List<Node.direction>()
                                    {
                                        right
                                    };
                                }
                                node.path.Add(new Node.pathItem()
                                {
                                    path = new List<MapGo.nyrqPosition>() { },
                                    selections = selections,
                                    position = path[indexOfPath],
                                    selectionCenter = new Node.pathItem.Postion()
                                    {
                                        longitude = path[indexOfPath].BDlongitude,
                                        latitude = path[indexOfPath].BDlatitude,
                                        height = path[indexOfPath].BDheight,
                                        crossKey = null
                                    }
                                });
                            }
                            else
                            {
                                var right = new Node.direction()
                                {
                                    right = true,
                                    start = path[indexOfPath],
                                    end = path[indexOfPath + 1]
                                };
                                /*
                                 * 这里只有1个选项是为了不进行选择。
                                 */
                                node.path.Add(new Node.pathItem()
                                {
                                    path = new List<MapGo.nyrqPosition>() { },
                                    selections = new List<Node.direction>()
                                    {
                                        right
                                    },
                                    position = path[indexOfPath],
                                    selectionCenter = new Node.pathItem.Postion()
                                    {
                                        longitude = path[indexOfPath].BDlongitude,
                                        latitude = path[indexOfPath].BDlatitude,
                                        height = path[indexOfPath].BDheight,
                                        crossKey = null
                                    }
                                });
                            }
                            lastPoint = path[0];
                            cursor = indexOfPath + 1;
                        }
                        else
                        {

                            var current = grp.GetItemRoadInfo(path[indexOfPath]);
                            var next = grp.GetItemRoadInfo(path[indexOfPath + 1]);
                            var position = lastPoint.copy();
                            if (current.RoadCode == next.RoadCode)
                            {
                                //   cursor = i;
                                List<CalCross> calCross = new List<CalCross>();

                                double ascendingValue;
                                if (path[indexOfPath].roadOrder + path[indexOfPath].percent < path[indexOfPath + 1].roadOrder + path[indexOfPath + 1].percent)
                                {
                                    ascendingValue = 1;
                                }
                                else if (path[indexOfPath].roadOrder + path[indexOfPath].percent > path[indexOfPath + 1].roadOrder + path[indexOfPath + 1].percent)
                                {
                                    ascendingValue = -1;
                                }
                                else
                                {
                                    continue;
                                }
                                {
                                    var findCrosses = findCrossesF(current.Cross1,
                                        current,
                                        path[indexOfPath], path[indexOfPath + 1], ascendingValue,
                                         (SaveRoad.DictCross c) =>
                                         {
                                             return c.RoadCode1;
                                         },
                                         (SaveRoad.DictCross c) =>
                                         {
                                             return c.RoadOrder1;
                                         },
                                         (SaveRoad.DictCross c) =>
                                         {
                                             return c.Percent1;
                                         }
                                        );
                                    for (var indexOfC = 0; indexOfC < findCrosses.Count; indexOfC++)
                                    {
                                        calCross.Add(new CalCross()
                                        {
                                            cross = findCrosses[indexOfC],
                                            calType = 1
                                        });
                                    }
                                }
                                {
                                    var findCrosses = findCrossesF(current.Cross2,
                                        current,
                                        path[indexOfPath], path[indexOfPath + 1], ascendingValue,
                                         (SaveRoad.DictCross c) =>
                                         {
                                             return c.RoadCode2;
                                         },
                                         (SaveRoad.DictCross c) =>
                                         {
                                             return c.RoadOrder2;
                                         },
                                         (SaveRoad.DictCross c) =>
                                         {
                                             return c.Percent2;
                                         }
                                        );
                                    for (var indexOfC = 0; indexOfC < findCrosses.Count; indexOfC++)
                                    {
                                        calCross.Add(new CalCross()
                                        {
                                            cross = findCrosses[indexOfC],
                                            calType = 2
                                        });
                                    }
                                }

                                calCross = (from item in calCross
                                            orderby (item.calType == 1 ? (item.cross.RoadOrder1 + item.cross.Percent1) : (item.cross.RoadOrder2 + item.cross.Percent2)) * ascendingValue ascending
                                            select item).ToList();
                                for (int indexOfCalCross = 0; indexOfCalCross < calCross.Count; indexOfCalCross++)
                                {
                                    var pathItem = new List<MapGo.nyrqPosition>();
                                    pathItem.Add(lastPoint.copy());//增加最后一点。
                                    for (int start = cursor; start < indexOfPath; start++)
                                    {
                                        pathItem.Add(path[start]);
                                    }
                                    if (cursor < indexOfPath)
                                    {
                                        cursor = indexOfPath + 1;//将光标指向下一个位置。在一个线段内的第二个cross，不会执行上面的循环
                                    }
                                    double height = calCross[indexOfCalCross].calType == 1 ?
                                        calCross[indexOfCalCross].cross.Percent1 * (grp.GetItemRoadInfo(calCross[indexOfCalCross].cross.RoadCode1, calCross[indexOfCalCross].cross.RoadOrder1).endHeight - grp.GetItemRoadInfo(calCross[indexOfCalCross].cross.RoadCode1, calCross[indexOfCalCross].cross.RoadOrder1).startHeight) + grp.GetItemRoadInfo(calCross[indexOfCalCross].cross.RoadCode1, calCross[indexOfCalCross].cross.RoadOrder1).startHeight :
                                        calCross[indexOfCalCross].cross.Percent2 * (grp.GetItemRoadInfo(calCross[indexOfCalCross].cross.RoadCode2, calCross[indexOfCalCross].cross.RoadOrder2).endHeight - grp.GetItemRoadInfo(calCross[indexOfCalCross].cross.RoadCode2, calCross[indexOfCalCross].cross.RoadOrder2).startHeight) + grp.GetItemRoadInfo(calCross[indexOfCalCross].cross.RoadCode2, calCross[indexOfCalCross].cross.RoadOrder2).startHeight;


                                    var newLast = new MapGo.nyrqPosition
                                    (
                                    calCross[indexOfCalCross].calType == 1 ? calCross[indexOfCalCross].cross.RoadCode1 : calCross[indexOfCalCross].cross.RoadCode2,
                                    calCross[indexOfCalCross].calType == 1 ? calCross[indexOfCalCross].cross.RoadOrder1 : calCross[indexOfCalCross].cross.RoadOrder2,
                                    calCross[indexOfCalCross].calType == 1 ? calCross[indexOfCalCross].cross.Percent1 : calCross[indexOfCalCross].cross.Percent2,
                                    calCross[indexOfCalCross].cross.BDLongitude,
                                    calCross[indexOfCalCross].cross.BDLatitude,
                                    height,
                                    lastPoint.maxSpeed);
                                    pathItem.Add(newLast);
                                    lastPoint = newLast.copy();
                                    Node.pathItem.Postion selectionCenter = new Node.pathItem.Postion()
                                    {
                                        longitude = calCross[indexOfCalCross].cross.BDLongitude,
                                        latitude = calCross[indexOfCalCross].cross.BDLatitude,
                                        height = height,
                                        crossKey = getID(calCross[indexOfCalCross].cross)
                                    };
                                    var selections = new List<Node.direction>();
                                    {
                                        var directionCreate = new Node.direction()
                                        {
                                            start = path[indexOfPath],
                                            end = path[indexOfPath + 1],
                                            right = true
                                        };
                                        if (FoundCross(directionCreate, path[indexOfPath], grp))
                                        {
                                            selections.Add(directionCreate);
                                        }
                                    }

                                    string otherRoadCode;
                                    int otherRoadOrder;
                                    double otherPencent;
                                    double otherLon, otherLat, otherHeight;
                                    otherLon = calCross[indexOfCalCross].cross.BDLongitude;
                                    otherLat = calCross[indexOfCalCross].cross.BDLatitude;
                                    if (calCross[indexOfCalCross].calType == 1)
                                    {
                                        otherRoadCode = calCross[indexOfCalCross].cross.RoadCode2;
                                        otherRoadOrder = calCross[indexOfCalCross].cross.RoadOrder2;
                                        otherPencent = calCross[indexOfCalCross].cross.Percent2;
                                        otherHeight = calCross[indexOfCalCross].cross.Percent2 * (grp.GetItemRoadInfo(calCross[indexOfCalCross].cross.RoadCode2, calCross[indexOfCalCross].cross.RoadOrder2).endHeight - grp.GetItemRoadInfo(calCross[indexOfCalCross].cross.RoadCode2, calCross[indexOfCalCross].cross.RoadOrder2).startHeight) + grp.GetItemRoadInfo(calCross[indexOfCalCross].cross.RoadCode2, calCross[indexOfCalCross].cross.RoadOrder2).startHeight;

                                    }
                                    else
                                    {
                                        otherRoadCode = calCross[indexOfCalCross].cross.RoadCode1;
                                        otherRoadOrder = calCross[indexOfCalCross].cross.RoadOrder1;
                                        otherPencent = calCross[indexOfCalCross].cross.Percent1;
                                        otherHeight = calCross[indexOfCalCross].cross.Percent1 * (grp.GetItemRoadInfo(calCross[indexOfCalCross].cross.RoadCode1, calCross[indexOfCalCross].cross.RoadOrder1).endHeight - grp.GetItemRoadInfo(calCross[indexOfCalCross].cross.RoadCode1, calCross[indexOfCalCross].cross.RoadOrder1).startHeight) + grp.GetItemRoadInfo(calCross[indexOfCalCross].cross.RoadCode1, calCross[indexOfCalCross].cross.RoadOrder1).startHeight;

                                    }
                                    var otherRoad = grp.GetItemRoadInfo(otherRoadCode, otherRoadOrder);
                                    {
                                        var directionCreate = new Node.direction()
                                        {
                                            start = new MapGo.nyrqPosition(otherRoad.RoadCode, otherRoad.RoadOrder, 0, otherRoad.startLongitude, otherRoad.startLatitude, otherRoad.startHeight, otherRoad.MaxSpeed),
                                            end = new MapGo.nyrqPosition(otherRoad.RoadCode, otherRoad.RoadOrder, 1, otherRoad.endLongitude, otherRoad.endLatitude, otherRoad.endHeight, otherRoad.MaxSpeed),
                                            right = false
                                        };
                                        var otherPosition = new MapGo.nyrqPosition(otherRoad.RoadCode, otherRoad.RoadOrder, otherPencent, otherLon, otherLat, otherHeight, otherRoad.MaxSpeed);
                                        if (FoundCross(directionCreate, otherPosition, grp))
                                            selections.Add(directionCreate);
                                    }
                                    if (otherRoad.CarInOpposeDirection == 1)
                                    {
                                        var directionCreate = new Node.direction()
                                        {
                                            end = new MapGo.nyrqPosition(otherRoad.RoadCode, otherRoad.RoadOrder, 0, otherRoad.startLongitude, otherRoad.startLatitude, otherRoad.startHeight, otherRoad.MaxSpeed),
                                            start = new MapGo.nyrqPosition(otherRoad.RoadCode, otherRoad.RoadOrder, 1, otherRoad.endLongitude, otherRoad.endLatitude, otherRoad.endHeight, otherRoad.MaxSpeed),
                                            right = false
                                        };
                                        var otherPosition = new MapGo.nyrqPosition(otherRoad.RoadCode, otherRoad.RoadOrder, otherPencent, otherLon, otherLat, otherHeight, otherRoad.MaxSpeed);
                                        if (FoundCross(directionCreate, otherPosition, grp))
                                            selections.Add(directionCreate);
                                    }
                                    node.path.Add(new Node.pathItem()
                                    {
                                        path = pathItem,
                                        selections = selections,
                                        position = position,
                                        selectionCenter = selectionCenter
                                    });

                                    OutPutLast(node.path.Count - 1, node.path);
                                    //  outPutNode
                                }
                            }
                            else
                            {
                                var pathItem = new List<MapGo.nyrqPosition>();
                                pathItem.Add(lastPoint);
                                for (int start = cursor; start < indexOfPath; start++)
                                {
                                    pathItem.Add(path[start]);
                                }
                                cursor = indexOfPath + 2;//将光标指向下一个位置。在一个线段内的第二个cross，不会执行上面的循环
                                var newLast = path[indexOfPath].copy();
                                pathItem.Add(newLast);

                                var selections = new List<Node.direction>();
                                Node.pathItem.Postion selectionCenter = new Node.pathItem.Postion()
                                {
                                    longitude = path[indexOfPath].BDlongitude,
                                    latitude = path[indexOfPath].BDlatitude,
                                    height = path[indexOfPath].BDheight,
                                    crossKey = getID(current, next)
                                };
                                selections.Add(new Node.direction()
                                {
                                    start = path[indexOfPath + 1],
                                    end = path[indexOfPath + 2],
                                    right = true
                                });
                                if (next.CarInOpposeDirection == 1)
                                {
                                    var direction = new Node.direction()
                                    {
                                        start = path[indexOfPath + 2],
                                        end = path[indexOfPath + 1],
                                        right = false
                                    };
                                    if (FoundCross(direction, path[indexOfPath + 1], grp))
                                    {
                                        selections.Add(direction);
                                    }
                                }
                                {
                                    var direction = new Node.direction()
                                    {
                                        start = path[indexOfPath - 1],
                                        end = path[indexOfPath],
                                        right = false
                                    };
                                    if (FoundCross(direction, path[indexOfPath], grp))
                                    {
                                        selections.Add(direction);
                                    }
                                }
                                node.path.Add(new Node.pathItem()
                                {
                                    path = pathItem,
                                    selections = selections,
                                    position = position,
                                    selectionCenter = selectionCenter
                                });
                                OutPutLast(node.path.Count - 1, node.path);
                                lastPoint = path[indexOfPath + 1].copy();
                            }
                        }
                    }
                }

                {
                    var pathItem = new List<MapGo.nyrqPosition>();
                    pathItem.Add(lastPoint);
                    for (var indexOfLeft = cursor; indexOfLeft < path.Count; indexOfLeft++)
                    {
                        pathItem.Add(path[indexOfLeft]);
                    }
                    var selections = new List<Node.direction>();
                    node.path.Add(new Node.pathItem()
                    {
                        path = pathItem,
                        selections = selections,
                        position = lastPoint,
                        selectionCenter = new Node.pathItem.Postion()
                        {
                            longitude = path[path.Count - 1].BDlongitude,
                            latitude = path[path.Count - 1].BDlatitude,
                            height = path[path.Count - 1].BDheight,
                            crossKey = null
                        }
                    });
                }

                //for (int i = 0; i < node.path.Count; i++)
                //{
                //    for (int j = 0; j < node.path[i].path.Count; j++)
                //    {
                //        //Consol.WriteLine($"{node.path[i].path[j].BDlongitude},{node.path[i].path[j].BDlatitude}");
                //    }
                //}
                return node;
            }
            else
            {
                /*
                 * 如果path.count=1或2，返回空结果。
                 */
                var node = new Node()
                {
                    path = new List<Node.pathItem>()
                };
                return node;
            }
        }

        private void OutPutLast(int index, List<Node.pathItem> path)
        {
            //var value = path[index];
            //for (int i = 0; i < value.path.Count; i++)
            //{
            //    //Consol.WriteLine($"{ value.path[i].BDlongitude},{ value.path[i].BDlatitude}");
            //}
            //Console.ReadLine();
            //for (int i = 0; i < node.path.Count; i++)
            //{
            //    for (int j = 0; j < node.path[i].path.Count; j++)
            //    {
            //        //Consol.WriteLine($"{node.path[i].path[j].BDlongitude},{node.path[i].path[j].BDlatitude}");
            //    }
            //}
        }

        private string getID(SaveRoad.DictCross cross)
        {
            string crossKey;
            if (cross.RoadCode1.CompareTo(cross.RoadCode2) > 0)
            {
                crossKey = $"{cross.RoadCode1}{cross.RoadOrder1}{cross.RoadCode2}{cross.RoadOrder2}";
            }
            else
            {
                crossKey = $"{cross.RoadCode2}{cross.RoadOrder2}{cross.RoadCode1}{cross.RoadOrder1}";
            }
            return crossKey;
        }
        private string getID(SaveRoad.RoadInfo current, SaveRoad.RoadInfo next)
        {
            string crossKey;
            if (current.RoadCode.CompareTo(next.RoadCode) > 0)
            {
                crossKey = $"{current.RoadCode}{current.RoadOrder}{next.RoadCode}{next.RoadOrder}";
            }
            else
            {
                crossKey = $"{next.RoadCode}{next.RoadOrder}{current.RoadCode}{current.RoadOrder}";
            }
            return crossKey;
        }
        private bool FoundCross(Node.direction wrong, MapGo.nyrqPosition startPosition, GetRandomPos grp)
        {
            var roadCode = startPosition.roadCode;
            var roadOrder = startPosition.roadOrder + 0;
            var percent = startPosition.percent;
            MapGo.nyrqPosition firstEndPosition;


            double ascendingValue;
            if (wrong.end.roadOrder + wrong.end.percent > wrong.start.roadOrder + wrong.start.percent)
            {
                ascendingValue = 1;
                //  endPosition=new MapGo.nyrqPosition(roadCode,roadOrder,1)
                var current = grp.GetItemRoadInfo(roadCode, roadOrder);
                firstEndPosition = new MapGo.nyrqPosition(roadCode, roadOrder, 1, current.endLongitude, current.endLatitude, current.endHeight, current.MaxSpeed);
            }
            else
            {
                ascendingValue = -1;
                var current = grp.GetItemRoadInfo(roadCode, roadOrder);
                firstEndPosition = new MapGo.nyrqPosition(roadCode, roadOrder, 0, current.startLongitude, current.startLatitude, current.startHeight, current.MaxSpeed);
            }
            //  if (wrong.end.roadOrder + wrong.end.percent > wrong.start.roadOrder + wrong.start.percent)
            {
                {
                    //var start=
                    var current = grp.GetItemRoadInfo(roadCode, roadOrder);
                    var findCrosses1 = findCrossesF(current.Cross1,
                                           current,
                                           startPosition, firstEndPosition, ascendingValue,
                                            (SaveRoad.DictCross c) =>
                                            {
                                                return c.RoadCode1;
                                            },
                                            (SaveRoad.DictCross c) =>
                                            {
                                                return c.RoadOrder1;
                                            },
                                            (SaveRoad.DictCross c) =>
                                            {
                                                return c.Percent1;
                                            }
                                           );
                    var findCrosses2 = findCrossesF(current.Cross2,
                                           current,
                                           startPosition, firstEndPosition, ascendingValue,
                                            (SaveRoad.DictCross c) =>
                                            {
                                                return c.RoadCode2;
                                            },
                                            (SaveRoad.DictCross c) =>
                                            {
                                                return c.RoadOrder2;
                                            },
                                            (SaveRoad.DictCross c) =>
                                            {
                                                return c.Percent2;
                                            }
                                           );
                    if (findCrosses1.Count + findCrosses2.Count == 0)
                    {
                        if (ascendingValue > 0)
                            roadOrder++;
                        else
                            roadOrder--;
                    }
                    else
                    {
                        return true;
                    }
                }
                while (true)
                {
                    bool existed;
                    var current = grp.GetItemRoadInfo(roadCode, roadOrder, out existed);
                    if (existed)
                    {
                        if (current.Cross1.Length + current.Cross2.Length > 0)
                        {
                            return true;
                        }
                        else
                        {
                            if (ascendingValue > 0)
                                roadOrder++;
                            else
                                roadOrder--;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        //public Node GetAFromB_v3(int from, int to, RoleInGame player, ref List<string> notifyMsgs)
        //{
        //    // throw new Exception("");
        //    int cursor = 0;//光标所在位置

        //    var path = this.GetAFromB_Path(from, to, player, ref notifyMsgs);
        //    if (path.Count > 1)
        //    {
        //        var lastPoint = path[0];//第一个点作为起点
        //        var node = new Node()
        //        {
        //            path = new List<Node.pathItem>()
        //        };//初始化node
        //        for (var indexOfPath = 0; indexOfPath < path.Count; indexOfPath++)
        //        {

        //            if (indexOfPath + 1 < path.Count)
        //            {
        //                if (indexOfPath == 0)
        //                {
        //                    /*
        //                     * 第一个点
        //                     */
        //                    var firstRoad = Program.dt.GetItemRoadInfo(path[0]);
        //                    if (firstRoad.CarInOpposeDirection == 1)
        //                    {
        //                        var right = new Node.direction()
        //                        {
        //                            right = true,
        //                            start = path[indexOfPath],
        //                            end = path[indexOfPath + 1]
        //                        };
        //                        var wrong = new Node.direction()
        //                        {
        //                            right = false,
        //                            start = path[indexOfPath + 1],
        //                            end = path[indexOfPath],
        //                        };
        //                        node.path.Add(new Node.pathItem()
        //                        {
        //                            path = new List<MapGo.nyrqPosition>() { },
        //                            selections = new List<Node.direction>()
        //                            {
        //                                right,
        //                                wrong
        //                            },
        //                            position = path[indexOfPath],
        //                            selectionCenter = new Node.pathItem.Postion()
        //                            {
        //                                longitude = path[indexOfPath].BDlongitude,
        //                                latitude = path[indexOfPath].BDlatitude,
        //                                crossKey = null
        //                            }
        //                        });
        //                    }
        //                    else
        //                    {
        //                        var right = new Node.direction()
        //                        {
        //                            right = true,
        //                            start = path[indexOfPath],
        //                            end = path[indexOfPath + 1]
        //                        };
        //                        /*
        //                         * 这里只有1个选项是为了不进行选择。
        //                         */
        //                        node.path.Add(new Node.pathItem()
        //                        {
        //                            path = new List<MapGo.nyrqPosition>() { },
        //                            selections = new List<Node.direction>()
        //                            {
        //                                right
        //                            },
        //                            position = path[indexOfPath],
        //                            selectionCenter = new Node.pathItem.Postion()
        //                            {
        //                                longitude = path[indexOfPath].BDlongitude,
        //                                latitude = path[indexOfPath].BDlatitude,
        //                                crossKey = null
        //                            }
        //                        });
        //                    }
        //                    lastPoint = path[0];
        //                    cursor = indexOfPath + 1;
        //                }
        //                else
        //                {

        //                    var current = Program.dt.GetItemRoadInfo(path[indexOfPath]);
        //                    var next = Program.dt.GetItemRoadInfo(path[indexOfPath + 1]);
        //                    var position = lastPoint.copy();
        //                    if (current.RoadCode == next.RoadCode)
        //                    {
        //                        //   cursor = i;
        //                        List<CalCross> calCross = new List<CalCross>();

        //                        double ascendingValue;
        //                        if (path[indexOfPath].roadOrder + path[indexOfPath].percent < path[indexOfPath + 1].roadOrder + path[indexOfPath + 1].percent)
        //                        {
        //                            ascendingValue = 1;
        //                        }
        //                        else if (path[indexOfPath].roadOrder + path[indexOfPath].percent > path[indexOfPath + 1].roadOrder + path[indexOfPath + 1].percent)
        //                        {
        //                            ascendingValue = -1;
        //                        }
        //                        else
        //                        {
        //                            continue;
        //                        }
        //                        {
        //                            var findCrosses = findCrossesF(current.Cross1,
        //                                current,
        //                                path[indexOfPath], path[indexOfPath + 1], ascendingValue,
        //                                 (SaveRoad.DictCross c) =>
        //                                 {
        //                                     return c.RoadCode1;
        //                                 },
        //                                 (SaveRoad.DictCross c) =>
        //                                 {
        //                                     return c.RoadOrder1;
        //                                 },
        //                                 (SaveRoad.DictCross c) =>
        //                                 {
        //                                     return c.Percent1;
        //                                 }
        //                                );
        //                            for (var indexOfC = 0; indexOfC < findCrosses.Count; indexOfC++)
        //                            {
        //                                calCross.Add(new CalCross()
        //                                {
        //                                    cross = findCrosses[indexOfC],
        //                                    calType = 1
        //                                });
        //                            }
        //                        }
        //                        {
        //                            var findCrosses = findCrossesF(current.Cross2,
        //                                current,
        //                                path[indexOfPath], path[indexOfPath + 1], ascendingValue,
        //                                 (SaveRoad.DictCross c) =>
        //                                 {
        //                                     return c.RoadCode2;
        //                                 },
        //                                 (SaveRoad.DictCross c) =>
        //                                 {
        //                                     return c.RoadOrder2;
        //                                 },
        //                                 (SaveRoad.DictCross c) =>
        //                                 {
        //                                     return c.Percent2;
        //                                 }
        //                                );
        //                            for (var indexOfC = 0; indexOfC < findCrosses.Count; indexOfC++)
        //                            {
        //                                calCross.Add(new CalCross()
        //                                {
        //                                    cross = findCrosses[indexOfC],
        //                                    calType = 2
        //                                });
        //                            }
        //                        }

        //                        calCross = (from item in calCross
        //                                    orderby (item.calType == 1 ? (item.cross.RoadOrder1 + item.cross.Percent1) : (item.cross.RoadOrder2 + item.cross.Percent2)) * ascendingValue ascending
        //                                    select item).ToList();
        //                        for (int indexOfCalCross = 0; indexOfCalCross < calCross.Count; indexOfCalCross++)
        //                        {
        //                            var pathItem = new List<MapGo.nyrqPosition>();
        //                            //pathItem.Add(lastPoint.copy());//增加最后一点。
        //                            for (int start = cursor; start < indexOfPath; start++)
        //                            {
        //                                pathItem.Add(path[start]);
        //                            }
        //                            cursor = indexOfPath + 1;//将光标指向下一个位置。在一个线段内的第二个cross，不会执行上面的循环
        //                            var newLast = new MapGo.nyrqPosition
        //                                (
        //                                calCross[indexOfCalCross].calType == 1 ? calCross[indexOfCalCross].cross.RoadCode1 : calCross[indexOfCalCross].cross.RoadCode2,
        //                                calCross[indexOfCalCross].calType == 1 ? calCross[indexOfCalCross].cross.RoadOrder1 : calCross[indexOfCalCross].cross.RoadOrder2,
        //                                calCross[indexOfCalCross].calType == 1 ? calCross[indexOfCalCross].cross.Percent1 : calCross[indexOfCalCross].cross.Percent2,
        //                                calCross[indexOfCalCross].cross.BDLongitude,
        //                                calCross[indexOfCalCross].cross.BDLatitude,
        //                                lastPoint.maxSpeed);
        //                            pathItem.Add(newLast);
        //                            lastPoint = newLast.copy();
        //                            Node.pathItem.Postion selectionCenter = new Node.pathItem.Postion()
        //                            {
        //                                longitude = calCross[indexOfCalCross].cross.BDLongitude,
        //                                latitude = calCross[indexOfCalCross].cross.BDLatitude,
        //                                crossKey = getID(calCross[indexOfCalCross].cross)
        //                            };
        //                            var selections = new List<Node.direction>();
        //                            if (ascendingValue > 0)
        //                            {
        //                                selections.Add(new Node.direction()
        //                                {
        //                                    start = path[indexOfPath],
        //                                    end = path[indexOfPath + 1],
        //                                    right = true
        //                                });
        //                                if (current.CarInOpposeDirection == 1)
        //                                {
        //                                    selections.Add(new Node.direction()
        //                                    {
        //                                        start = path[indexOfPath + 1],
        //                                        end = path[indexOfPath],
        //                                        right = false
        //                                    });
        //                                }
        //                            }
        //                            else if (ascendingValue < 0)
        //                            {
        //                                selections.Add(new Node.direction()
        //                                {
        //                                    start = path[indexOfPath],
        //                                    end = path[indexOfPath + 1],
        //                                    right = true
        //                                });
        //                                selections.Add(new Node.direction()
        //                                {
        //                                    start = path[indexOfPath + 1],
        //                                    end = path[indexOfPath],
        //                                    right = false
        //                                });
        //                            }
        //                            string otherRoadCode;
        //                            int otherRoadOrder;
        //                            if (calCross[indexOfCalCross].calType == 1)
        //                            {
        //                                otherRoadCode = calCross[indexOfCalCross].cross.RoadCode2;
        //                                otherRoadOrder = calCross[indexOfCalCross].cross.RoadOrder2;
        //                            }
        //                            else
        //                            {
        //                                otherRoadCode = calCross[indexOfCalCross].cross.RoadCode1;
        //                                otherRoadOrder = calCross[indexOfCalCross].cross.RoadOrder1;
        //                            }
        //                            var otherRoad = Program.dt.GetItemRoadInfo(otherRoadCode, otherRoadOrder);

        //                            selections.Add(new Node.direction()
        //                            {
        //                                start = new MapGo.nyrqPosition(otherRoad.RoadCode, otherRoad.RoadOrder, 0, otherRoad.startLongitude, otherRoad.startLatitude, otherRoad.MaxSpeed),
        //                                end = new MapGo.nyrqPosition(otherRoad.RoadCode, otherRoad.RoadOrder, 1, otherRoad.endLongitude, otherRoad.endLatitude, otherRoad.MaxSpeed),
        //                                right = false
        //                            });
        //                            if (otherRoad.CarInOpposeDirection == 1)
        //                                selections.Add(new Node.direction()
        //                                {
        //                                    end = new MapGo.nyrqPosition(otherRoad.RoadCode, otherRoad.RoadOrder, 0, otherRoad.startLongitude, otherRoad.startLatitude, otherRoad.MaxSpeed),
        //                                    start = new MapGo.nyrqPosition(otherRoad.RoadCode, otherRoad.RoadOrder, 1, otherRoad.endLongitude, otherRoad.endLatitude, otherRoad.MaxSpeed),
        //                                    right = false
        //                                });

        //                            node.path.Add(new Node.pathItem()
        //                            {
        //                                path = pathItem,
        //                                selections = selections,
        //                                position = position,
        //                                selectionCenter = selectionCenter
        //                            });
        //                        }
        //                    }
        //                    else
        //                    {
        //                        var pathItem = new List<MapGo.nyrqPosition>();
        //                        //pathItem.Add(lastPoint);
        //                        for (int start = cursor; start < indexOfPath; start++)
        //                        {
        //                            pathItem.Add(path[start]);
        //                        }
        //                        cursor = indexOfPath + 2;//将光标指向下一个位置。在一个线段内的第二个cross，不会执行上面的循环
        //                        var newLast = path[indexOfPath].copy();
        //                        pathItem.Add(newLast);

        //                        var selections = new List<Node.direction>();
        //                        Node.pathItem.Postion selectionCenter = new Node.pathItem.Postion()
        //                        {
        //                            longitude = path[indexOfPath].BDlongitude,
        //                            latitude = path[indexOfPath].BDlatitude,
        //                            crossKey = getID(current, next)
        //                        };
        //                        selections.Add(new Node.direction()
        //                        {
        //                            start = path[indexOfPath + 1],
        //                            end = path[indexOfPath + 2],
        //                            right = true
        //                        });
        //                        if (next.CarInOpposeDirection == 1)
        //                            selections.Add(new Node.direction()
        //                            {
        //                                start = path[indexOfPath + 2],
        //                                end = path[indexOfPath + 1],
        //                                right = false
        //                            });

        //                        selections.Add(new Node.direction()
        //                        {
        //                            start = path[indexOfPath - 1],
        //                            end = path[indexOfPath],
        //                            right = false
        //                        });

        //                        selections.Add(new Node.direction()
        //                        {
        //                            start = path[indexOfPath],
        //                            end = path[indexOfPath - 1],
        //                            right = false
        //                        }); ;
        //                        node.path.Add(new Node.pathItem()
        //                        {
        //                            path = pathItem,
        //                            selections = selections,
        //                            position = position,
        //                            selectionCenter = selectionCenter
        //                        });
        //                        lastPoint = path[indexOfPath + 1].copy();
        //                    }
        //                }
        //            }
        //        }

        //        {
        //            var pathItem = new List<MapGo.nyrqPosition>();
        //            pathItem.Add(lastPoint);
        //            for (var indexOfLeft = cursor; indexOfLeft < path.Count; indexOfLeft++)
        //            {
        //                pathItem.Add(path[indexOfLeft]);
        //            }
        //            var selections = new List<Node.direction>();
        //            node.path.Add(new Node.pathItem()
        //            {
        //                path = pathItem,
        //                selections = selections,
        //                position = lastPoint,
        //                selectionCenter = new Node.pathItem.Postion()
        //                {
        //                    longitude = path[path.Count - 1].BDlongitude,
        //                    latitude = path[path.Count - 1].BDlatitude,
        //                    crossKey = null
        //                }
        //            });
        //        }
        //        return node;
        //    }
        //    else
        //    {
        //        /*
        //         * 如果path.count=1或2，返回空结果。
        //         */
        //        var node = new Node()
        //        {
        //            path = new List<Node.pathItem>()
        //        };
        //        return node;
        //    }
        //}



        delegate string getRoadCodeParameter(SaveRoad.DictCross c);
        delegate int getRoadOrderParameter(SaveRoad.DictCross c);
        delegate double getPercentParameter(SaveRoad.DictCross c);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cross"></param>
        /// <param name="current"></param>
        /// <param name="p1">起点</param>
        /// <param name="p2">终点</param>
        /// <param name="ascendingValue"></param>
        /// <param name="codeF"></param>
        /// <param name="orderF"></param>
        /// <param name="percentF"></param>
        /// <returns></returns>
        private List<SaveRoad.DictCross> findCrossesF(SaveRoad.DictCross[] cross, SaveRoad.RoadInfo current, MapGo.nyrqPosition p1, MapGo.nyrqPosition p2, double ascendingValue, getRoadCodeParameter codeF, getRoadOrderParameter orderF, getPercentParameter percentF)
        {
            var findCrosses = (from item in cross
                               where (item.CrossState == 1
                               && codeF(item) == current.RoadCode
                               && (orderF(item) + percentF(item)) * ascendingValue > (p1.roadOrder + p1.percent) * ascendingValue
                              && (orderF(item) + percentF(item)) * ascendingValue < (p2.roadOrder + p2.percent) * ascendingValue)
                              ||
                              (false
#warning 这里要更改
                              //item.CrossState == 2 &&//item.RoadCode1>item.RoadCode2
                              )

                               orderby (orderF(item) + percentF(item)) * ascendingValue ascending
                               select item).ToList();
            return findCrosses;
        }

        internal int GetMile(Node goPath)
        {
            double sumMiles = 0;
            for (var j = 0; j < goPath.path.Count; j++)
            {
                var path = goPath.path[j].path;
                for (var i = 1; i < path.Count; i++)
                {
                    sumMiles += CommonClass.Geography.getLengthOfTwoPoint.GetDistance(path[i].BDlatitude, path[i].BDlongitude, path[i].BDheight, path[i - 1].BDlatitude, path[i - 1].BDlongitude, path[i - 1].BDheight);
                }
            }
            return Convert.ToInt32(sumMiles) / 1000;
        }

        /// <summary>
        /// 获取从基地出来时的路径！
        /// </summary>
        /// <param name="fp">初始地点</param>
        /// <param name="car">carA？-carE</param>
        /// <param name="startTInput">时间</param>
        /// <returns></returns>
        public List<int> getStartPositon(Model.FastonPosition fp, int positionInStation, ref int startTInput, out Data.PathStartPoint3 startPosition, bool speedImproved)
        {
            double startX, startY, startZ;
            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(fp.Longitude, fp.Latitde, fp.Height, out startX, out startY, out startZ);
            int startT0;// startT1;

            double endX, endY, endZ;
            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(fp.positionLongitudeOnRoad, fp.positionLatitudeOnRoad, fp.Height, out endX, out endY, out endZ);
            int endT0;// endT1;

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

            startPosition = new Data.PathStartPoint3()
            {
                x = Convert.ToInt32(carPositionX * 256),
                y = Convert.ToInt32(carPositionY * 256),
                z = Convert.ToInt32(startZ * 256)
            };

            List<int> animateResult = new List<int>();
            startT0 = startTInput;
            endT0 = startT0 + this.magicE.shotTime(350, speedImproved);
            startTInput += this.magicE.shotTime(350, speedImproved);
            var animate1 = new Data.PathResult4()
            {
                x = Convert.ToInt32(-(position.Real * percentOfPosition) * 256),
                y = Convert.ToInt32(position.Imaginary * percentOfPosition * 256),
                z = 0,
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
            if (animate1.x != 0 || animate1.y != 0) //  if (animate1.t != 0)
            {
                animateResult.Add(animate1.x);
                animateResult.Add(animate1.y);
                animateResult.Add(animate1.z);
                animateResult.Add(animate1.t);
            }
            // animateResult.Add(animate1);

            //  var interview = Convert.ToInt32(CommonClass.Geography.getLengthOfTwoPoint.GetDistance(fp.Latitde, fp.Longitude, fp.positionLatitudeOnRoad, fp.positionLongitudeOnRoad) / 10 * 1000);
            int interview = 350;
            {
                /*原先代码！*/
                //var calInterview = CommonClass.Geography.getLengthOfTwoPoint.GetDistance(fp.Latitde, fp.Longitude, fp.Height, fp.positionLatitudeOnRoad, fp.positionLongitudeOnRoad, fp.Height) / 10 * 1000;
                //if (calInterview < 1e-8)
                //{
                //    interview = 0;
                //}
                //else if (calInterview < 1)
                //{
                //    interview = 1;
                //}
                //else
                //{
                //    interview = Convert.ToInt32(calInterview + 1);

                //}
            }
            interview = this.magicE.shotTime(interview, speedImproved);

            startTInput += interview;

            var animate2 = new Data.PathResult4()
            {
                x = Convert.ToInt32((endX - startX) * 256),
                y = Convert.ToInt32((endY - startY) * 256),
                z = Convert.ToInt32((endZ - startZ) * 256),
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
            if (animate2.x != 0 || animate2.y != 0)
            {
                animateResult.Add(animate2.x);
                animateResult.Add(animate2.y);
                animateResult.Add(animate2.z);
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

        internal void getStartPositionByFp(out Data.PathStartPoint3 startPosition, MapGo.nyrqPosition position)
        {
            double startX, startY, startZ;
            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(position.BDlongitude, position.BDlatitude, position.BDheight, out startX, out startY, out startZ);
            startPosition = new Data.PathStartPoint3()
            {
                x = Convert.ToInt32(startX * 256),
                y = Convert.ToInt32(startY * 256),
                z = Convert.ToInt32(startZ * 256)
            };
        }
        //[Obsolete]
        //void getStartPositionByFp(out Data.PathStartPoint2 startPosition, FastonPosition fastonPosition)
        //{
        //    double startX, startY;
        //    CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(fastonPosition.positionLongitudeOnRoad, fastonPosition.positionLatitudeOnRoad, out startX, out startY);
        //    startPosition = new Data.PathStartPoint2()
        //    {
        //        x = Convert.ToInt32(startX * 256),
        //        y = Convert.ToInt32(startY * 256)
        //    };
        //}
        //public void getStartPositionByGoPath(out Data.PathStartPoint2 startPosition, List<Model.MapGo.nyrqPosition> goPath)
        //{
        //    var firstPosition = goPath.First();
        //    double startX, startY;
        //    CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(firstPosition.BDlongitude, firstPosition.BDlatitude, out startX, out startY);
        //    startPosition = new Data.PathStartPoint2()
        //    {
        //        x = Convert.ToInt32(startX * 256),
        //        y = Convert.ToInt32(startY * 256)
        //    };
        //}
        internal void getStartPositionByGoPath(out Data.PathStartPoint3 startPosition, Node.pathItem pathItem)
        {
            double startX, startY, startZ;
            if (pathItem.path.Count > 0)
                CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(pathItem.path[0].BDlongitude, pathItem.path[0].BDlatitude, pathItem.path[0].BDheight, out startX, out startY, out startZ);
            else
                CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(pathItem.position.BDlongitude, pathItem.position.BDlatitude, pathItem.position.BDheight, out startX, out startY, out startZ);
            startPosition = new Data.PathStartPoint3()
            {
                x = Convert.ToInt32(startX * 256),
                y = Convert.ToInt32(startY * 256),
                z = Convert.ToInt32(startZ * 256)
            };
        }

        public void getEndPositon(Model.FastonPosition fp, int initPosition, ref List<int> animateResult, ref int startTInput, bool speedImproved)
        {
            if (initPosition > 5)
            {
                initPosition = initPosition % 5;
            }
            double endX, endY, endZ;
            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(fp.Longitude, fp.Latitde, fp.Height, out endX, out endY, out endZ);
            int startT1;

            double startX, startY, startZ;
            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(fp.positionLongitudeOnRoad, fp.positionLatitudeOnRoad, fp.Height, out startX, out startY, out startZ);
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
            // var interview = Convert.ToInt32(CommonClass.Geography.getLengthOfTwoPoint.GetDistance(fp.Latitde, fp.Longitude, fp.positionLatitudeOnRoad, fp.positionLongitudeOnRoad) / 10 * 1000);
            int interview = 350;
            // var calInterview = CommonClass.Geography.getLengthOfTwoPoint.GetDistance(fp.Latitde, fp.Longitude, fp.Height, fp.positionLatitudeOnRoad, fp.positionLongitudeOnRoad, fp.Height) / 10 * 1000;
            //if (calInterview < 1e-8)
            //{
            //    interview = 350;
            //}
            //else if (calInterview < 1)
            //{
            //    interview = 350;
            //}
            //else
            //{
            //    interview = 350;

            //}
            interview = this.magicE.shotTime(interview, speedImproved);

            startT1 = startTInput;
            endT1 = startT1 + interview;
            startTInput += interview;
            var animate2 = new Data.PathResult4()
            {
                x = Convert.ToInt32((endX - startX) * 256),
                y = Convert.ToInt32((endY - startY) * 256),
                z = Convert.ToInt32((endZ - startZ) * 256),
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
            if (animate2.x != 0 || animate2.y != 0)   // if (animate2.t != 0)
            {
                animateResult.Add(animate2.x);
                animateResult.Add(animate2.y);
                animateResult.Add(animate2.z);
                animateResult.Add(animate2.t);
            }

            //  startT0 = startTInput;
            //    endT0 = startT0 + 500;
            startTInput += this.magicE.shotTime(350, speedImproved);

            var animate1 = new Data.PathResult4()
            {
                x = Convert.ToInt32((carPositionX - endX) * 256),
                y = Convert.ToInt32((carPositionY - endY) * 256),
                z = 0,
                t = this.magicE.shotTime(350, speedImproved)
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
            if (animate1.x != 0 || animate1.y != 0)  // if (animate1.t != 0)
            {
                animateResult.Add(animate1.x);
                animateResult.Add(animate1.y);
                animateResult.Add(animate1.z);
                animateResult.Add(animate1.t);
            }

        }

        internal void ViewPosition(RoleInGame role, FastonPosition fpResult, ref List<string> notifyMsg)
        {
            if (role.playerType == RoleInGame.PlayerType.player)
            {
                var player = (Player)role;
                var url = player.FromUrl;
                ViewSearch sn = new ViewSearch()
                {
                    c = "ViewSearch",
                    WebSocketID = player.WebSocketID,
                    mctX = fpResult.MacatuoX,
                    mctY = fpResult.MacatuoY
                };

                var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(sn);
                notifyMsg.Add(url);
                notifyMsg.Add(sendMsg);
            }
        }

        internal void showDirecitonAndSelection(Player player, List<Node.direction> selections, Node.pathItem.Postion selectionCenter, ref List<string> notifyMsg)
        {
            List<double> direction = new List<double>();
            for (var i = 0; i < selections.Count; i++)
            {
                double x1, y1, z1, x2, y2, z2;
                var start = selections[i].start;
                CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(start.BDlongitude, start.BDlatitude, start.BDheight, out x1, out y1, out z1);
                var end = selections[i].end;
                CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(end.BDlongitude, end.BDlatitude, end.BDheight, out x2, out y2, out z2);

                var l = Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
                if (l > 1e-8)
                {
                    var sinA = (y2 - y1) / l;
                    var cosA = (x2 - x1) / l;
                    double angle;
                    if (sinA >= 0)
                        angle = Math.Acos(cosA);
                    else
                        angle = Math.PI * 2 - Math.Acos(cosA);
                    direction.Add(angle);
                }

            }
            double positionX, positionY, positionZ;
            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(selectionCenter.longitude, selectionCenter.latitude, selectionCenter.height, out positionX, out positionY, out positionZ);
            var obj = new ShowDirectionOperator
            {
                c = "ShowDirectionOperator",
                WebSocketID = player.WebSocketID,
                direction = direction.ToArray(),
                positionX = positionX,
                positionY = positionY,
                positionZ = positionZ
            };
            var url = player.FromUrl;
            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            notifyMsg.Add(url);
            notifyMsg.Add(sendMsg);
        }
    }
}
