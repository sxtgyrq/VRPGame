using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WsOfWebClient
{
    public partial class Room
    {
        internal static string GetRandom(int count)
        {
            int maxV = 9999;
            int v = Math.Abs(DateTime.Now.GetHashCode()) % maxV;
            v = v * Math.Abs("abcdefg".GetHashCode()) % maxV;
            Random rm = new Random(v);
            if (count < 4) count = 4;
            string result = "";
            const string pszBase58 = "123456789ABCDEFGHJKMNPQRSTUVWXYZabcdedfhjkmnpqrstuvwxyz";
            for (int i = 0; i < count; i++)
            {
                result += pszBase58[rm.Next(pszBase58.Length)];
            }
            return result;
            // return v % maxV;
            // throw new NotImplementedException();
        }

        internal static void setRandomPic(IntroState iState, WebSocket webSocket)
        {
            string checkCode = iState.randomValue.Trim();
            string base64String;
            using (System.Drawing.Bitmap image = new System.Drawing.Bitmap(Convert.ToInt32(Math.Ceiling((decimal)(checkCode.Length * 14))), 22))
            {
                using (Graphics g = Graphics.FromImage(image))
                {
                    Random random = new Random(DateTime.Now.GetHashCode());
                    g.Clear(Color.AliceBlue);

                    for (int i = 0; i < 7; i++)
                    {
                        int x1 = random.Next(image.Width);
                        int x2 = random.Next(image.Width);
                        int y1 = random.Next(image.Height);
                        int y2 = random.Next(image.Height);

                        g.DrawLine(new Pen(Color.Orange), x1, y1, x2, y2);
                    }

                    Font font = new System.Drawing.Font("Tahoma", 12, System.Drawing.FontStyle.Bold);
                    System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
                    g.DrawString(checkCode, font, new SolidBrush(Color.Orange), 2, 2);

                    for (int i = 0; i < 15; i++)
                    {
                        int x = random.Next(image.Width);
                        int y = random.Next(image.Height);

                        image.SetPixel(x, y, Color.Orange);
                    }
                    g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                    // g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width – 1, image.Height – 1);
                    byte[] imageBytes;
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                    {
                        image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                        imageBytes = ms.ToArray();
                    }
                    base64String = Convert.ToBase64String(imageBytes);
                }
            }
            var passObj = new
            {
                base64String = base64String,
                c = "VerifyCodePic"
            };
            var returnMsg = Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
            CommonF.SendData(returnMsg, webSocket, 0);
        }

        
    }
}
