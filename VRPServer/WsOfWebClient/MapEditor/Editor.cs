using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using static CommonClass.MapEditor;

namespace WsOfWebClient.MapEditor
{
    partial class Editor
    {
        class SaveObj : CommonClass.Command
        {
            public double x { get; set; }
            public double y { get; set; }
            public double z { get; set; }
            public double rotationY { get; set; }
        }
        class EditModel : CommonClass.Command
        {
            public string name { get; set; }
        }
        class GetModelDetail : CommonClass.Command
        {
            public string name { get; set; }
        }
        class LookForHeight : CommonClass.Command
        {
            public double MercatorX { get; set; }
            public double MercatorY { get; set; }
        }
     
        class UseModelObj : CommonClass.Command
        {
            public string name { get; set; }
            public bool used { get; set; }
        }
        class DeleteModel : EditModel
        {
            public double x { get; set; }
            public double z { get; set; }
        }

        class SetBG : CommonClass.Command
        {
            //public string px { get; set; }
            //public string nx { get; set; }
            //public string py { get; set; }
            //public string ny { get; set; }
            //public string pz { get; set; }
            //public string nz { get; set; }
        }
        class CreateNewObj : SaveObj
        {
            public string[] objNew { get; set; }
        }

        const int webWsSize = 1024 * 1024 * 3;

        public class ReceiveObj
        {
            public WebSocketReceiveResult wr { get; set; }
            public string result { get; set; }
        }
        public static async Task<ReceiveObj> ReceiveStringAsync(System.Net.WebSockets.WebSocket socket, int size, CancellationToken ct = default(CancellationToken))
        {
            var buffer = new ArraySegment<byte>(new byte[size]);
            WebSocketReceiveResult result;
            using (var ms = new MemoryStream())
            {
                do
                {
                    // ct.IsCancellationRequested
                    ct.ThrowIfCancellationRequested();

                    result = await socket.ReceiveAsync(buffer, ct);

                    ms.Write(buffer.Array, buffer.Offset, result.Count);
                }
                while (!result.EndOfMessage);

                ms.Seek(0, SeekOrigin.Begin);
                if (result.MessageType != WebSocketMessageType.Text)
                {
                    return new ReceiveObj()
                    {
                        result = null,
                        wr = result
                    };
                }
                using (var reader = new StreamReader(ms, Encoding.UTF8))
                {
                    var strValue = await reader.ReadToEndAsync();
                    return new ReceiveObj()
                    {
                        result = strValue,
                        wr = result
                    };
                }
            }
        }

        enum stateOfSelection
        {
            roadCross,
            modelEdit
        }

        class aModel : abtractmodels
        {
            public aModel(string v1, string v2)
            {
                this.amID = v1;
                this.author = v2;
                this.initialized = false;
            }
            public bool initialized { get; set; }
            public string imageBase64 { get; private set; }
            public string objText { get; private set; }

            public string mtlText { get; private set; }

            internal void initialize(Random rm)
            {
                var index = rm.Next(0, roomUrls.Count);
                var roomUrl = roomUrls[index];
                var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(
                    new CommonClass.MapEditor.GetAbtractModels
                    {
                        c = "GetAbtractModels",
                        amID = this.amID
                    });
                var json = Startup.sendInmationToUrlAndGetRes(roomUrl, sendMsg);
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<abtractmodelsPassData>(json);
                this.imageBase64 = obj.imageBase64;
                this.objText = obj.objText;
                this.mtlText = obj.mtlText;

            }
        }
        class abtractmodels : CommonClass.databaseModel.abtractmodels
        {
        }
        class abtractmodelsPassData : CommonClass.databaseModel.abtractmodelsPassData
        {
        }


        private static async Task<string> GetBTCAddress(WebSocket webSocket)
        {
            //  mm.ID = Guid.NewGuid().ToString();
            var sn = new
            {
                c = "InputAddress"
            };
            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(sn);
            var sendData = Encoding.ASCII.GetBytes(sendMsg);
            await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
            // throw new NotImplementedException();
            var returnResult = await ReceiveStringAsync(webSocket, webWsSize);
            //Consol.WriteLine($"receive from web:{returnResult.result}");

            return returnResult.result;
        }

        private static async Task SetState(stateOfSelection ss, WebSocket webSocket)
        {
            //  mm.ID = Guid.NewGuid().ToString();
            var sn = new
            {
                c = "SetState",
                state = ss.ToString()
            };
            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(sn);
            var sendData = Encoding.ASCII.GetBytes(sendMsg);
            await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
            // throw new NotImplementedException();
        }



        private static CommonClass.MapEditor.Position getPreviousCross(Position firstRoad, Random rm)
        {
            var index = rm.Next(0, roomUrls.Count);
            var roomUrl = roomUrls[index];
            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(new NextCross()
            {
                c = "PreviousCross",
                roadCode = firstRoad.roadCode,
                roadOrder = firstRoad.roadOrder,
                anotherRoadCode = firstRoad.anotherRoadCode,
                anotherRoadOrder = firstRoad.anotherRoadOrder
            });
            var json = Startup.sendInmationToUrlAndGetRes(roomUrl, sendMsg);
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }
            else
            {
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.Position>(json);
                return obj;
            }
        }
        private static CommonClass.MapEditor.Position getNextCross(Position firstRoad, Random rm)
        {
            var index = rm.Next(0, roomUrls.Count);
            var roomUrl = roomUrls[index];
            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(new NextCross()
            {
                c = "NextCross",
                roadCode = firstRoad.roadCode,
                roadOrder = firstRoad.roadOrder,
                anotherRoadCode = firstRoad.anotherRoadCode,
                anotherRoadOrder = firstRoad.anotherRoadOrder
            });
            var json = Startup.sendInmationToUrlAndGetRes(roomUrl, sendMsg);
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }
            else
            {
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.Position>(json);
                return obj;
            }
        }

        private static async Task<Dictionary<string, bool>> Draw(Position firstRoad, Dictionary<string, bool> roads, WebSocket webSocket, Random rm)
        {
            var roadCode = firstRoad.roadCode;
            var roadOrder = firstRoad.roadOrder;
            var anotherRoadCode = firstRoad.anotherRoadCode;
            var anotherRoadOrder = firstRoad.anotherRoadOrder;
            if (roads.ContainsKey(roadCode))
            {

            }
            else
            {
                roads.Add(roadCode, true);
                var msg = await drawRoad(roadCode, rm);
                {
                    var sendData = Encoding.ASCII.GetBytes(msg);
                    await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
            if (roads.ContainsKey(anotherRoadCode))
            {

            }
            else
            {
                roads.Add(anotherRoadCode, true);
                var msg = await drawRoad(anotherRoadCode, rm);
                {
                    var sendData = Encoding.ASCII.GetBytes(msg);
                    await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
            return roads;
        }

        private static async Task SetView(Position firstRoad, WebSocket webSocket)
        {
            double MacatuoX, MacatuoY, MacatuoZ;
            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(firstRoad.longitude, firstRoad.latitude, firstRoad.height, out MacatuoX, out MacatuoY, out MacatuoZ);
            CommonClass.ViewSearch sn = new CommonClass.ViewSearch()
            {
                c = "ViewSearch",
                WebSocketID = 0,
                mctX = Convert.ToInt32(MacatuoX * 256),
                mctY = Convert.ToInt32(MacatuoY * 256),
            };
            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(sn);
            var sendData = Encoding.ASCII.GetBytes(sendMsg);
            await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        }
        private static async Task<string> drawRoad(string roadCode, Random rm)
        {
            var index = rm.Next(0, roomUrls.Count);
            var roomUrl = roomUrls[index];
            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(new DrawRoad()
            {
                c = "DrawRoad",
                roadCode = roadCode
            });
            var json = Startup.sendInmationToUrlAndGetRes(roomUrl, sendMsg);
            return json;
        }

        public static List<string> roomUrls
        {
            get { return Room.roomUrls; }
        }
        private static CommonClass.MapEditor.Position getFirstRoad(Random rm)
        {
            var index = rm.Next(0, roomUrls.Count);
            var roomUrl = roomUrls[index];
            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                c = "GetFirstRoad",
            });
            var json = Startup.sendInmationToUrlAndGetRes(roomUrl, sendMsg);
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.Position>(json);
            return obj;
        }
    }

}
