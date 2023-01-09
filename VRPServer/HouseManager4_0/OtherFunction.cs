using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HouseManager4_0
{
    class OtherFunction
    {
        internal static void calMercator()
        {
            //Consol.WriteLine("输入地理经纬度坐标，如 112,37");
            var content = Console.ReadLine();
            var lon = double.Parse(content.Split(',')[0]);
            var lat = double.Parse(content.Split(',')[1]);
            double x, y, z;
            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(lon, lat, 0, out x, out y, out z);
            //Consol.WriteLine($"x:{x},y:{y}");
            //Consol.WriteLine($"E退出，任意键继续");
            if (Console.ReadLine().ToUpper() == "E")
            {

            }
            else
            {
                calMercator();
            }

        }
        internal static void readConnectInfomation()
        {
            Console.WriteLine("输入密文");
            var content = Console.ReadLine();
            Console.WriteLine("输入密钥");
            var secretValue = Console.ReadLine();
            var result = CommonClass.AES.AesDecrypt(content, secretValue);
            Console.WriteLine($"{result}");
            Console.ReadLine();
        }

        class ObjInput
        {
            public string addr { get; set; }
            public string time { get; set; }
            public List<string> list { get; set; }
        }
        class ObjOutput
        {
            public string c { get; set; }
            public string time { get; set; }
            public List<string> list { get; set; }
        }
        internal static void sign()
        {
            Console.WriteLine("输入密钥");
            var privateKey = Console.ReadLine();

            Console.WriteLine("拖入要加密内容路径");
            var path = Console.ReadLine();
            var content = File.ReadAllText(path);
            var oI = Newtonsoft.Json.JsonConvert.DeserializeObject<ObjInput>(content);
            ObjOutput oo = new ObjOutput()
            {
                c = "AwardsGiving",
                time = oI.time,
                list = new List<string>()
            };
            for (int i = 0; i < oI.list.Count; i++)
            {
                var msg = oI.list[i];
                char[] splitOp = { '@', '-', '>', ':' };
                StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries;
                var addr = msg.Split(splitOp, options)[1];
                var sign = BitCoin.Sign.SignMessage(privateKey, msg, addr);
                oo.list.Add(sign);
            }
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(oo));
            //BitCoin.Sign.SignMessage()
            // var jsonObj = "";
            //  throw new NotImplementedException();
        }

        //        [Obsolete]
        //        internal static void addModel()
        //        {
        //            while (true)
        //            {
        //                //Consol.WriteLine("拖入obj文件");
        //                var path1 = Console.ReadLine();
        //                //Consol.WriteLine("拖入mtl文件");
        //                var path2 = Console.ReadLine();
        //                //Consol.WriteLine("拖入jpg文件");
        //                var path3 = Console.ReadLine();

        //                var bytes = File.ReadAllBytes(path3);
        //                var Base64 = Convert.ToBase64String(bytes);
        //                // ConnectInfo.SpeedIconBase64 = Base64;
        //                var objText = File.ReadAllText(path1);

        //                var mtlText = File.ReadAllText(path2);

        //                var fileName = Path.GetFileName(path1);
        //                List<string> modelTypes = new List<string>
        //                         {
        //                "direciton",
        //                "building"
        //                         };
        //                for (var i = 0; i < modelTypes.Count; i++)
        //                {
        //                    //Consol.WriteLine($"{i}-{modelTypes[i]}");
        //                }
        //                var modelType = "";
        //                do
        //                {

        //                    modelType = Console.ReadLine();
        //                }
        //                while (!modelTypes.Contains(modelType));


        //                //Consol.WriteLine($@"
        //fileName:{fileName}
        //modelType:{modelType}
        //objText:{objText}
        //mtlText:{mtlText}
        //Base64:{Base64}
        //---按任意键继续！
        //");

        //                Console.ReadLine();
        //                DalOfAddress.AbtractModels.AddMoney(fileName.Split('.')[0], modelType, Base64, objText, mtlText, "");
        //                //Consol.WriteLine($"存储成功！E退出，任意键继续");
        //                if (Console.ReadLine().ToUpper() == "E")
        //                {
        //                    break;
        //                }
        //            }
        //            // DalOfAddress.
        //            //DalOfAddress.
        //            //ConnectInfo.SpeedMtl = await File.ReadAllTextAsync("model/speedicon/mfire.mtl");
        //            //ConnectInfo.SpeedObj = await File.ReadAllTextAsync("model/speedicon/mfire.obj");
        //            //  throw new NotImplementedException();
        //        }
    }
}
