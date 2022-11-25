using netDxf;
using netDxf.Entities;
using System;
using System.Collections.Generic;
using System.IO;

namespace ModelHelper
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //{
            //    var allLines = File.ReadAllLines("l.obj");
            //    for (int i = 0; i < allLines.Length; i++)
            //    {
            //        var rowV = allLines[i];
            //        var values = rowV.Split('\t', StringSplitOptions.RemoveEmptyEntries);
            //        if (values.Length == 4 && values[0] == "v") 
            //        { }
            //    }
            //}
            Console.ReadLine();
            string file = "E:\\W202211\\凤山塔\\simple\\b\\point2.txt";

            // create a new document, by default it will create an AutoCad2000 DXF version
            DxfDocument doc = new DxfDocument();
            // an entity
            //Line entity = new Line(new Vector2(5, 5), new Vector2(10, 5));
            // add your entities here
            {
                var parameters = File.ReadAllLines(file);

                //  var parameters = stringV.Split(',');
                for (int i = 0; i < parameters.Length; i++)
                {
                    if (!string.IsNullOrEmpty(parameters[i]))
                    {
                        var values = parameters[i].Split(' ');
                        double X = Convert.ToDouble(values[0]);
                        double Y = Convert.ToDouble(values[1]);
                        double Z = Convert.ToDouble(values[2]);
                        var points = new netDxf.Entities.Point(X, Y, Z);
                        doc.AddEntity(points);
                    }  
                } 
            }
            //{
            //    file = "E:\\Project\\ModelSimple\\ConsoleApp1\\bin\\Release\\netcoreapp3.1\\publish\\PointsB.txt";
            //    var stringV = File.ReadAllText(file);
            //    var parameters = stringV.Split(',');
            //    for (int i = 0; i < parameters.Length; i += 3)
            //    {
            //        double x = Convert.ToDouble(parameters[i + 0]);
            //        double y = Convert.ToDouble(parameters[i + 1]);
            //        double z = Convert.ToDouble(parameters[i + 2]);
            //        Point p = new Point(new Vector3(x, y, z));
            //        doc.AddEntity(p);
            //    }
            //}
            doc.Save("E:\\W202211\\凤山塔\\simple\\b\\point2Result.txt");
            Console.ReadLine();


        }

        private static List<Vector3> GetCornerABC(double A, double B, double C, double D)
        {
            Vector3 v1, v2, v3, v4;
            {
                int y = 0, z = 0;
                v1 = new Vector3((-D - y * B - z * C) / A, y, z);
            }
            {
                int y = 1, z = 0;
                v2 = new Vector3((-D - y * B - z * C) / A, y, z);
            }
            {
                int y = 1, z = 1;
                v3 = new Vector3((-D - y * B - z * C) / A, y, z);
            }
            {
                int y = 0, z = 1;
                v4 = new Vector3((-D - y * B - z * C) / A, y, z);
            }
            return new List<Vector3>(4) { v1, v2, v3, v4 };
        }
        private static List<Vector3> GetCornerBCA(double B, double C, double A, double D)
        {
            Vector3 v1, v2, v3, v4;
            {
                int z = 0, x = 0;
                v1 = new Vector3(x, (-D - C * z - A * x) / B, z);
            }
            {
                int z = 1, x = 0;
                v2 = new Vector3(x, (-D - C * z - A * x) / B, z);
            }
            {
                int z = 1, x = 1;
                v3 = new Vector3(x, (-D - C * z - A * x) / B, z);
            }
            {
                int z = 0, x = 1;
                v4 = new Vector3(x, (-D - C * z - A * x) / B, z);
            }
            return new List<Vector3>(4) { v1, v2, v3, v4 };
        }
        private static List<Vector3> GetCornerCAB(double C, double A, double B, double D)
        {
            Vector3 v1, v2, v3, v4;
            {
                int x = 0, y = 0;
                v1 = new Vector3(x, y, (-D - A * x - B * y) / C);
            }
            {
                int x = 1, y = 0;
                v2 = new Vector3(x, y, (-D - A * x - B * y) / C);
            }
            {
                int x = 1, y = 1;
                v3 = new Vector3(x, y, (-D - A * x - B * y) / C);
            }
            {
                int x = 0, y = 1;
                v4 = new Vector3(x, y, (-D - A * x - B * y) / C);
            }
            return new List<Vector3>(4) { v1, v2, v3, v4 };
        }
    }
}
