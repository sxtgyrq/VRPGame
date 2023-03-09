using System;
using System.Threading;

namespace HouseManager4_0
{
    class Program
    {
        public static DateTime startTime;
        public static Geometry.Boundary boundary;
        public static Data dt;
        public static RoomMainF.RoomMain rm;
        static void Main(string[] args)
        {
            Console.WriteLine(@"
--------- readConnectInfomation 
--------- calMercator
--------- sign
--------- 
");
            var commandInput = Console.ReadLine();
            switch (commandInput)
            {
                case "readConnectInfomation":
                    {
                        OtherFunction.readConnectInfomation();
                        return;
                    };
                //case "addModel":
                //    {
                //        OtherFunction.addModel();
                //        return;
                //    }; break;
                case "calMercator":
                    {
                        OtherFunction.calMercator();
                        return;
                    };
                case "sign":
                    {
                        OtherFunction.sign();
                    }; break;

            }

            // Console.WriteLine("Hello World!");
            var version = "4.0.0";
            string Text = $@"
版本号{version}
主要实现功能是寻宝、攻击、收集一体化。这是为前台提供新的服务！
";
            Console.WriteLine($"版本号：{version}");

            {
                Console.Write("输入密码:");
                var pass = string.Empty;
                ConsoleKey key;
                do
                {
                    var keyInfo = Console.ReadKey(intercept: true);
                    key = keyInfo.Key;

                    if (key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        Console.Write("\b \b");
                        pass = pass[0..^1];
                    }
                    else if (!char.IsControl(keyInfo.KeyChar))
                    {
                        Console.Write("*");
                        pass += keyInfo.KeyChar;
                    }
                } while (key != ConsoleKey.Enter);
                DalOfAddress.Connection.SetPassWord(pass);
            }
            Program.startTime = DateTime.Now;

            Program.boundary = new Geometry.Boundary();
            boundary.load();

            Program.dt = new Data();
            Program.dt.LoadRoad();
            Program.dt.LoadModel();
            Program.dt.LoadCrossBackground();

            Program.rm = new RoomMainF.RoomMain(Program.dt);

            {
                var ip = "127.0.0.1";
                int tcpPort = 11100;

                Console.WriteLine($"输入ip,如“{ip}”");
                var inputIp = Console.ReadLine();
                if (string.IsNullOrEmpty(inputIp)) { }
                else
                {
                    ip = inputIp;
                }

                Console.WriteLine($"输入端口≠15000,如“{tcpPort}”");
                var inputWebsocketPort = Console.ReadLine();
                if (string.IsNullOrEmpty(inputWebsocketPort)) { }
                else
                {
                    int num;
                    if (int.TryParse(inputWebsocketPort, out num))
                    {
                        tcpPort = num;
                    }
                }


                Data.SetRootPath();

                Thread startTcpServer = new Thread(() => Listen.IpAndPort(ip, tcpPort));
                startTcpServer.Start();

                Thread startMonitorTcpServer = new Thread(() => Listen.IpAndPortMonitor(ip, 30000 - tcpPort));
                startMonitorTcpServer.Start();

                Thread th = new Thread(() => PlayersSysOperate(Program.dt));
                th.Start();
                //int tcpServerPort = 30000 - websocketPort;
                //ConnectInfo.HostIP = ip;
                //ConnectInfo.webSocketPort = websocketPort;
                //ConnectInfo.tcpServerPort = tcpServerPort;
            }
            while (true)
            {
                if (Console.ReadLine().ToLower() == "exit")
                {
                    break;
                }
            }
        }

        private static void PlayersSysOperate(GetRandomPos grp)
        {
            while (true)
            {
                ; 
                Program.rm.SetReturn(grp);
                Program.rm.ClearPlayers();
                Program.rm.SetNPC();
                Thread.Sleep(30 * 1000);

            }
            //  throw new NotImplementedException();
        }
    }
}
