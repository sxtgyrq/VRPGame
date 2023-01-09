using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.IO.Compression;

namespace WsOfWebClient.MapEditor
{
    partial class Editor
    {


        static int roomIndex = 0;
        private static void DownLoad(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                {
                    byte[] fileForShow;
                    string amID = context.Request.Query["amid"].ToString().Trim();
                    if (string.IsNullOrEmpty(amID))
                    {
                        return;
                    }
                    else
                    {
                        var roomUrl = roomUrls[roomIndex % roomUrls.Count];
                        roomIndex++;

                        var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(
                            new CommonClass.MapEditor.GetAbtractModels
                            {
                                c = "GetAbtractModels",
                                amID = amID
                            });
                        var json = Startup.sendInmationToUrlAndGetRes(roomUrl, sendMsg);
                        //Consol.WriteLine(json);
                        var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<abtractmodelsPassData>(json);

                        using (var compressedFileStream = new MemoryStream())
                        {
                            using (var zipArchive = new ZipArchive(compressedFileStream, ZipArchiveMode.Update, false))
                            {
                                if (!string.IsNullOrEmpty(obj.objText))
                                {
                                    var zipEntry = zipArchive.CreateEntry("model.obj");
                                    using (var originalFileStream = new MemoryStream(Encoding.UTF8.GetBytes(obj.objText)))
                                    {
                                        using (var zipEntryStream = zipEntry.Open())
                                        {
                                            originalFileStream.CopyTo(zipEntryStream);
                                        }
                                    }
                                }
                                if (!string.IsNullOrEmpty(obj.mtlText))
                                {
                                    var zipEntry = zipArchive.CreateEntry("model.mtl");
                                    using (var originalFileStream = new MemoryStream(Encoding.UTF8.GetBytes(obj.mtlText)))
                                    {
                                        using (var zipEntryStream = zipEntry.Open())
                                        {
                                            originalFileStream.CopyTo(zipEntryStream);
                                        }
                                    }
                                }
                                if (!string.IsNullOrEmpty(obj.imageBase64))
                                {
                                    var zipEntry = zipArchive.CreateEntry("model.jpg");
                                    using (var originalFileStream = new MemoryStream(Base64ToImage(obj.imageBase64)))
                                    {
                                        using (var zipEntryStream = zipEntry.Open())
                                        {
                                            originalFileStream.CopyTo(zipEntryStream);
                                        }
                                    }
                                }
                            }
                            fileForShow = compressedFileStream.ToArray();
                        }
                        var response = context.Response;
                        response.Headers.Add("Content-Disposition", $"attachment;filename={amID}.rar");
                        response.ContentType = "application/octet-stream";
                        await response.Body.WriteAsync(fileForShow, 0, fileForShow.Length);
                    }
                    //this.imageBase64 = obj.imageBase64;
                    //this.objText = obj.objText;
                    //this.mtlText = obj.mtlText;
                }




            });
        }

        public static byte[] Base64ToImage(string base64String)
        {
            // Convert base 64 string to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            // Convert byte[] to Image
            return imageBytes;
        }
    }
}
