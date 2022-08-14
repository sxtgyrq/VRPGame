using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace CubeImageTool
{
    internal class CubeToPanorama
    {
        internal static void Generate()
        {
            Console.WriteLine($"输入文件夹，如 E:/W202208/house副本/");
             var path = Console.ReadLine();
            //var path = "E:\\W202208\\house - 副本\\";

            int imageHeight = 2048*1;
            int halfHeight = imageHeight / 2;

            string[] panelValue = new string[6] { "px", "nx", "py", "ny", "pz", "nz" };
            Dictionary<string, Bitmap> images = new Dictionary<string, Bitmap>();
            for (int i = 0; i < 6; i++)
            {
                Bitmap img = new Bitmap($"{path}{panelValue[i]}.png");
                images.Add(panelValue[i], img);
            }

            using (Bitmap bitmap = new Bitmap(imageHeight * 2, imageHeight, System.Drawing.Imaging.PixelFormat.Format32bppPArgb))
            {
                for (var heightIndex = 0; heightIndex < imageHeight; heightIndex++)
                {
                    for (var widthIndex = 0; widthIndex < imageHeight * 2; widthIndex++)
                    {
                        var angle2 = new double[]
                        {
                        widthIndex*Math.PI*2/(imageHeight * 2),
                        (halfHeight- heightIndex)*Math.PI/2/halfHeight
                        };
                        var vec3 = new double[]
                        {
                        Math.Sin(angle2[0])*Math.Cos(angle2[1]),
                        Math.Cos(angle2[0])*Math.Cos(angle2[1]),
                        Math.Sin(angle2[1]),
                        };
                        string panel;
                        var vec2_PanelPosition = MaxItemToOne(vec3, out panel);
                        var imgInput = images[panel];
                        var imagePosition = new int[]
                        {
                            Convert.ToInt32((vec2_PanelPosition[0]+1)/2*imgInput.Width)%imgInput.Width,
                            Convert.ToInt32((vec2_PanelPosition[1]-1)/-2*imgInput.Height)%imgInput.Height,
                        };
                        var color = imgInput.GetPixel(imagePosition[0], imagePosition[1]);
                        bitmap.SetPixel(widthIndex, heightIndex, color);
                    }
                }
                bitmap.Save($"{path}Result.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            }


            //using (Graphics graphics = Graphics.FromImage(bitmap)) 
            //{
            //    graphics.draw
            //}

            //  throw new NotImplementedException();
        }

        private static double[] MaxItemToOne(double[] vec3, out string panel)
        {
            var max = Math.Max(Math.Abs(vec3[0]), Math.Abs(vec3[1]));
            max = Math.Max(max, Math.Abs(vec3[2]));
            if (Math.Abs(vec3[0]) >= Math.Abs(vec3[1])
                && Math.Abs(vec3[0]) >= Math.Abs(vec3[2]))
            {
                if (vec3[0] > 0)
                {
                    panel = "pz";
                    return new double[] { vec3[1] / vec3[0] * -1, vec3[2] / vec3[0] };
                }
                else
                {
                    panel = "nz";
                    //return new double[] { vec3[1] / vec3[0], vec3[2] / vec3[0] };
                    return new double[] { vec3[1] / vec3[0] * -1, vec3[2] / vec3[0] * -1 };
                }
                //return new double[] { vec3[2] / vec3[0], vec3[1] / vec3[0] };
            }
            else if (Math.Abs(vec3[1]) >= Math.Abs(vec3[0])
                 && Math.Abs(vec3[1]) >= Math.Abs(vec3[2]))
            {
                if (vec3[1] > 0)
                {
                    panel = "nx";
                    return new double[] { vec3[0] / vec3[1], vec3[2] / vec3[1] * 1 };
                }
                else
                {
                    panel = "px";
                    return new double[] { vec3[0] / vec3[1], vec3[2] / vec3[1] * -1 };
                }
                //return new double[] { vec3[2] / vec3[1], vec3[0] / vec3[1] };
            }
            else
            {
                if (vec3[2] > 0)
                {
                    panel = "py";
                    return new double[] { -vec3[1] / vec3[2], -vec3[0] / vec3[2] };
                    // return new double[] { vec3[0] / vec3[2], -vec3[1] / vec3[2] };
                }
                else
                {
                    panel = "ny";
                    return new double[] { vec3[1] / vec3[2], -vec3[0] / vec3[2] };
                }
            }
        }
    }
}
