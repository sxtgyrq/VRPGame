using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace WsOfWebClient.MapEditor
{
    partial class Editor
    {


        private static void Upload(IApplicationBuilder app)
        {
            app.UseCors("AllowAny");
            app.Run(async context =>
            {
                var date = context.Request;
                //  var author = context.Request.Query["author"][0];
                var name = context.Request.Form["fname"].ToString();
                var crossName = context.Request.Form["crossName"].ToString();
#warning 这里要取消显示输出
                Console.WriteLine($"crossName:{crossName}");
                Console.WriteLine($"name:{name}");
                var files = context.Request.Form.Files;

                Regex r = new Regex("^[A-Z]{10}[0-9]{1,4}[A-Z]{10}[0-9]{1,4}$");
                if (r.IsMatch(crossName))
                {
                    r = new Regex("^[pn]{1}[xyz]{1}$");
                    if (r.IsMatch(name))
                    {
                        if (Directory.Exists($"imgT/{crossName}/"))
                        {
                            if (files.Count > 0)
                            {
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    var file = files[0];
                                    await file.CopyToAsync(ms);
                                    Image i;
                                    i = Image.FromStream(ms);
                                    var n = ResizeImage(i, 1024, 1024);
                                    n.Save($"imgT/{crossName}/{name}.jpg", ImageFormat.Jpeg);
                                }
                            }
                        }
                    }
                }
            });
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private static void BackGroundImg(IApplicationBuilder app)
        {
            app.UseCors("AllowAny");
            app.Run(async context =>
            {
                try
                {
                    var pathValue = context.Request.Path.Value;
                    //Console.Write($" {context.Request.Path.Value}");
                    var regex = new System.Text.RegularExpressions.Regex("^/[A-Z]{10}[0-9]{1,4}[A-Z]{10}[0-9]{1,4}/[np]{1}[xyz]{1}.jpg$");
                    if (regex.IsMatch(pathValue))
                    {
                        var filePath = $"{Room.ImgPath}T{pathValue}";
                        if (File.Exists(filePath))
                        {
                            context.Response.ContentType = "image/jpeg";
                            {
                                var bytes = await File.ReadAllBytesAsync(filePath);
                                await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
                            }
                        }
                        else
                        {
                            context.Response.ContentType = "image/jpeg";
                            {
                                var bytes = await File.ReadAllBytesAsync("noData.jpg");
                                await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
                            }

                        }
                    }
                }
                catch (Exception e)
                {
                    
                } 
            });
        }
    }
}
