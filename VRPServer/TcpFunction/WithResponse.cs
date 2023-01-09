using CommonClass;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TcpFunction
{
    public class WithResponse
    {
        public static async Task<string> SendInmationToUrlAndGetRes(string roomUrl, string sendMsg)
        {

            var startTime = DateTime.Now;
            string result = "";

            IPAddress ipa;
            if (IPAddress.TryParse(roomUrl.Split(':')[0], out ipa))
            {
                TcpClient tc = new TcpClient();
                tc.Connect(ipa, int.Parse(roomUrl.Split(':')[1]));

                if (tc.Connected)
                {
                    NetworkStream ns = tc.GetStream();
                    var sendData = Encoding.UTF8.GetBytes(sendMsg);
                    Common.SendLength(sendData.Length, ns);
                    //  Common.CheckBeforeReadReason reason;
                    var length = Common.ReceiveLength(ns);
                    if (sendData.Length == length) { }
                    else
                    {
                        var msg = $"sendData.Length ({sendData.Length})!= length({length})";
                        //Consol.WriteLine(msg);
                        throw new Exception(msg);
                    }
                    //  Common.CheckBeforeSend(ns);
                    await ns.WriteAsync(sendData, 0, sendData.Length);

                    var length2 = Common.ReceiveLength(ns);
                    Common.SendLength(length2, ns);
                    byte[] bytes = Common.ByteReader(length2, ns);
                    result = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                    ns.Close(6000);
                }
                tc.Close();
            }
            var endTime = DateTime.Now;
            return result;
        }




        public delegate string DealWith(string notifyJson, int tcpPort);
        public static void ListenIpAndPort(string hostIP, int tcpPort, DealWith dealWith)
        {
            Int32 port = tcpPort;
            IPAddress localAddr = IPAddress.Parse(hostIP);
            var server = new TcpListener(localAddr, port);
            server.Start();
            while (true)
            {
                //   Console.Write("Waiting for a connection... ");

                string notifyJson;
                TcpClient client = server.AcceptTcpClient();
                {
                    using (NetworkStream ns = client.GetStream())
                    {

                        notifyJson = GetMsg01(client, ns);
                        var outPut = dealWith(notifyJson, tcpPort);
                        GetMsg02(client, ns, outPut);
                        ns.Close(6000);
                    }
                }
                client.Close();
            }
        }

        private static void GetMsg02(TcpClient client, NetworkStream ns, string outPut)
        {
            var sendData = Encoding.UTF8.GetBytes(outPut);
            Common.SendLength(sendData.Length, ns);
            var length2 = Common.ReceiveLength(ns);
            if (length2 != sendData.Length)
            {
                var msg = $"length2({length2})!= sendData.Length({sendData.Length})";
                //Consol.WriteLine(msg);
                throw new Exception(msg);
            }
            var t = ns.WriteAsync(sendData, 0, sendData.Length);
            t.GetAwaiter().GetResult();
        }

        private static string GetMsg01(TcpClient client, NetworkStream ns)
        {
            var length = Common.ReceiveLength(ns);
            Common.SendLength(length, ns);
            byte[] bytes = new byte[length];

            bytes = Common.ByteReader(length, ns);
            //  int bytesRead = await ns.ReadAsync(bytes, 0, length);
            //if (length != bytesRead)
            //{
            //    throw new Exception("length != bytesRead");
            //}
            var notifyJson = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            return notifyJson;
        }
    }
    /*
     * 这里淘汰WithoutResponse，统一使用 WithResponse
     */
    //public class WithoutResponse
    //{
    //    public static async void SendInmationToUrl(string controllerUrl, string json)
    //    {

    //        //   Let’s use that to filter the records returned using the netstat command - netstat - ano | findstr 185.190.83.2
    //        //Consol.WriteLine($"controllerUrl:{controllerUrl}");
    //        //Consol.WriteLine($"json:{json}");
    //        //  try
    //        {
    //            string server = controllerUrl.Split(':')[0];
    //            Int32 port = int.Parse(controllerUrl.Split(':')[1]);
    //            TcpClient client = new TcpClient(server, port);
    //            //client.
    //            if (client.Connected) { }
    //            else
    //            {
    //                //Consol.WriteLine($"{controllerUrl},没有连接！");
    //                return;
    //            }
    //            using (NetworkStream ns = client.GetStream())
    //            {
    //                var sendData = Encoding.UTF8.GetBytes(json);
    //                await Common.SendLength(sendData.Length, ns);
    //                var length = Common.ReceiveLength(ns);
    //                if (length == sendData.Length) { }
    //                else
    //                {
    //                    var msg = $"length:({length})!= sendData.Length({sendData.Length})";
    //                    //Consol.WriteLine(msg);
    //                    //throw new Exception($"length:({length})!= sendData.Length({sendData.Length})");
    //                    throw new Exception(msg);
    //                }
    //                await ns.WriteAsync(sendData, 0, sendData.Length);
    //                ns.Close(6000);
    //            }
    //            client.Close();
    //        }
    //        //catch (ArgumentNullException e)
    //        //{
    //        //    //Consol.WriteLine("ArgumentNullException: {0}", e);
    //        //}
    //        //catch (SocketException e)
    //        //{
    //        //    //Consol.WriteLine("SocketException: {0}", e);
    //        //}
    //    }

    //    public delegate Task DealWith(string notifyJson);
    //    public static void startTcp(string ip, int port, DealWith dealWith)
    //    {
    //        // throw new NotImplementedException();
    //        // Int32 port = port;
    //        IPAddress localAddr = IPAddress.Parse(ip);
    //        var server = new TcpListener(localAddr, port);
    //        server.Start();
    //        //AutoResetEvent allDone = new AutoResetEvent(false);
    //        while (true)
    //        {
    //            Console.Write("Waiting for a connection... ");


    //            //string notifyJson;
    //            //  bool isRight;
    //            try
    //            {
    //                TcpClient client = server.AcceptTcpClient();
    //                {
    //                    //Consol.WriteLine("Connected!");
    //                    SetMsgAndIsRight smr = new SetMsgAndIsRight(SetMsgAndIsRightF);
    //                    GetMsg(client, smr, dealWith);

    //                }
    //                client.Close();
    //            }
    //            catch (SocketException e)
    //            {
    //                //Consol.WriteLine("SocketException: {0}", e);
    //            }
    //        }
    //    }
    //    static void SetMsgAndIsRightF(string notifyJson, DealWith dealWith)
    //    {
    //        //Consol.WriteLine($"notify receive:{notifyJson}");
    //        dealWith(notifyJson);
    //    }

    //    delegate void SetMsgAndIsRight(string notifyJson, DealWith dealWith);
    //    private static async void GetMsg(TcpClient client, SetMsgAndIsRight smr, DealWith dealWith)
    //    {
    //        using (NetworkStream stream = client.GetStream())
    //        {
    //            var length = Common.ReceiveLength(stream);
    //            await Common.SendLength(length, stream);
    //            byte[] bytes = new byte[length];
    //            bytes = Common.ByteReader(length, stream);
    //            var notifyJson = Encoding.UTF8.GetString(bytes, 0, length);
    //            smr(notifyJson, dealWith);
    //            stream.Close(6000);
    //        }
    //    }
    //}
    class Common
    {
        internal static byte[] ByteReader(int length, Stream stream)
        {
            byte[] data = new byte[length];
            using (MemoryStream ms = new MemoryStream())
            {
                int numBytesRead;
                int numBytesReadsofar = 0;
                while (true)
                {
                    numBytesRead = stream.Read(data, 0, data.Length);
                    numBytesReadsofar += numBytesRead;
                    ms.Write(data, 0, numBytesRead);
                    if (numBytesReadsofar == length)
                    {
                        break;
                    }
                }
                return ms.ToArray();
            }
        }
        public static int ReceiveLength(NetworkStream ns)
        {

            byte[] bytes = new byte[4];
            bytes = ByteReader(4, ns);
            var length = bytes[0] * 256 * 256 * 256 + bytes[1] * 256 * 256 + bytes[2] * 256 + bytes[3] * 1;
            return length;
        }
        public static void SendLength(int length, NetworkStream ns)
        {
            var sendDataPreviw = new byte[]
            {
                Convert.ToByte((length>>24)%256),
                Convert.ToByte((length>>16)%256),
                Convert.ToByte((length>>8)%256),
                Convert.ToByte((length>>0)%256),
            };
            var t = ns.WriteAsync(sendDataPreviw, 0, 4);
            t.GetAwaiter().GetResult();
        }
    }
}

