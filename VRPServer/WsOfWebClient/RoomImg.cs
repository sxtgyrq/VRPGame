using System;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace WsOfWebClient
{
    public partial class Room
    {
        public const string ImgPath = "img";
        internal static async Task<byte[]> getImg(int index, string Md5, string fileName)
        {
            var gfma = new CommonClass.SetCrossBG()
            {
                c = "GetCrossBG",
                Md5Key = Md5
            };
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(gfma);
            var info = await Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
            if (string.IsNullOrEmpty(info))
            {
                return new byte[] { };
            }
            else
            {
                var data = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(info);

                string[] keys = new string[6] { "px", "py", "pz", "nx", "ny", "nz" };
                for (int i = 0; i < keys.Length; i++)
                {
                    var pkey = keys[i];
                    if (data.ContainsKey(pkey))
                    {
                        var base64 = data[pkey].Split(',')[1];
                        byte[] bytes = Convert.FromBase64String(base64);
                        var dicPath = $"{Room.ImgPath}/{Md5}/";
                        if (!Directory.Exists(dicPath))
                            Directory.CreateDirectory(dicPath);
                        var path = $"{Room.ImgPath}/{Md5}/{pkey}.jpg";

                        File.WriteAllBytes(path, bytes);
                        if ($"{pkey}.jpg" == fileName.ToString().Trim())
                        {
                            return bytes;
                        }
                    }
                    else
                    {
                        return new byte[] { };
                    }
                }
                return new byte[] { };
            }
        }

        
    }
}
