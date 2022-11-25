using netDxf;
using netDxf.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelHelper
{
    internal class Polygon
    {
        public void testc()
        {
            string file = "l.obj";

            // create a new document, by default it will create an AutoCad2000 DXF version
            DxfDocument doc = new DxfDocument();
            // an entity
            //Line entity = new Line(new Vector2(5, 5), new Vector2(10, 5));
            // add your entities here
            {
                var allLines = File.ReadAllLines(file);
                for (int i = 0; i < allLines.Length; i++)
                {
                    var rowV = allLines[i];
                    var values = rowV.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (values.Length == 4 && values[0] == "v")
                    {
                        netDxf.Entities.Face3d f3d = new Face3d(new Vector3(float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3])),
                            new Vector3(float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3])),
                            new Vector3(float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3])));
                        Point p = new Point(new Vector3(float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3])));
                        doc.AddEntity(p);
                    }
                }
            }
            doc.Save("rr.dxf");
            Console.ReadLine();
        }
    }
}
