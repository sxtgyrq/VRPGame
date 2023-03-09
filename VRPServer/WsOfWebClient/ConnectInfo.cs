using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;

namespace WsOfWebClient
{
    public static class ConnectInfo
    {
        public static string HostIP { get; set; }
        public static int webSocketID = 0;
        public static object connectedWs_LockObj = new object();
        public class ConnectInfoDetail
        {
            public ConnectInfoDetail(WebSocket webSocket)
            {
                this.ws = webSocket;
                this.BitcoinAddr = "";
                //this.aModle = new Dictionary<string, bool>();
                //this.msgs = new List<string>();
            }

            public WebSocket ws { get; private set; }
            public string BitcoinAddr { get; private set; }
            //public List<string> msgs = new List<string>();
            //  public Dictionary<string, bool> aModle { get; private set; }
        }
        public static Dictionary<int, ConnectInfoDetail> connectedWs = new Dictionary<int, ConnectInfoDetail>();

        // public static HttpClient Client = new HttpClient();

        //  static string _mapRoadAndCrossJson = "";
        //public static string mapRoadAndCrossJson
        //{
        //    get
        //    {
        //        lock (mapRoadAndCrossJsonLock)
        //        {
        //            return _mapRoadAndCrossJson;
        //        }
        //    }
        //    set
        //    {
        //        lock (mapRoadAndCrossJsonLock)
        //        {
        //            _mapRoadAndCrossJson = value;
        //            mapRoadAndCrossJsonMd5 = CommonClass.Random.GetMD5HashFromStr(_mapRoadAndCrossJson);
        //        }
        //    }
        //}
        static object mapRoadAndCrossJsonLock = "";
        public static string mapRoadAndCrossJsonMd5
        {
            get;
            private set;
        }
        public static string[] RobotBase64 = new string[] { };

        public static string DiamondObj = "";
        public static string DiamondMtl = "";
        public static string[] DiamondJpg = new string[] { };

        public static string YuanModel = "";
        public static string[] RMB100 = new string[] { };
        public static string[] RMB50 = new string[] { };
        public static string[] RMB20 = new string[] { };
        public static string[] RMB10 = new string[] { };
        public static string[] RMB5 = new string[] { };
        public static string[] RMB1 = new string[] { };

        //   public static string LeaveGameModel = "";
        public static string[] LeaveGameModel = new string[] { };

        public static string[] ProfileModel = new string[] { };



        internal static int webSocketPort;
        internal static int tcpServerPort;

        public static string SpeedIconBase64 = "";
        public static string SpeedObj = "";
        public static string SpeedMtl = "";

        public static string AttackIconBase64 = "";
        public static string AttackObj = "";
        public static string AttackMtl = "";

        public static string ShieldIconBase64 = "";
        public static string ShieldObj = "";
        public static string ShieldMtl = "";

        public static string ConfusePrepareIconBase64 = "";
        public static string ConfusePrepareObj = "";
        public static string ConfusePrepareMtl = "";

        public static string LostPrepareIconBase64 = "";
        public static string LostPrepareObj = "";
        public static string LostPrepareMtl = "";

        public static string AmbushPrepareIconBase64 = "";
        public static string AmbushPrepareObj = "";
        public static string AmbushPrepareMtl = "";

        public static string WaterIconBase64 = "";
        public static string WaterObj = "";
        public static string WaterMtl = "";

        public static string DirectionIconBase64 = "";
        public static string DirectionObj = "";
        public static string DirectionMtl = "";

        public static string DirectionArrowIconBase64 = "";
        public static string DirectionArrowObj = "";
        public static string DirectionArrowMtl = "";

        public static string OpponentIconBase64 = "";
        public static string OpponentIconObj = "";
        public static string OpponentIconMtl = "";

        public static string TeammateIconBase64 = "";
        public static string TeammateIconObj = "";
        public static string TeammateIconMtl = "";
    }
}
