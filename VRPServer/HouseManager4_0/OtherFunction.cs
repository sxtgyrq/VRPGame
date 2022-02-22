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
            Console.WriteLine("输入地理经纬度坐标，如 112,37");
            var content = Console.ReadLine();
            var lon = double.Parse(content.Split(',')[0]);
            var lat = double.Parse(content.Split(',')[1]);
            double x, y;
            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(lon, lat, out x, out y);
            Console.WriteLine($"x:{x},y:{y}");
            Console.WriteLine($"E退出，任意键继续");
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

//        [Obsolete]
//        internal static void addModel()
//        {
//            while (true)
//            {
//                Console.WriteLine("拖入obj文件");
//                var path1 = Console.ReadLine();
//                Console.WriteLine("拖入mtl文件");
//                var path2 = Console.ReadLine();
//                Console.WriteLine("拖入jpg文件");
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
//                    Console.WriteLine($"{i}-{modelTypes[i]}");
//                }
//                var modelType = "";
//                do
//                {

//                    modelType = Console.ReadLine();
//                }
//                while (!modelTypes.Contains(modelType));


//                Console.WriteLine($@"
//fileName:{fileName}
//modelType:{modelType}
//objText:{objText}
//mtlText:{mtlText}
//Base64:{Base64}
//---按任意键继续！
//");

//                Console.ReadLine();
//                DalOfAddress.AbtractModels.AddMoney(fileName.Split('.')[0], modelType, Base64, objText, mtlText, "");
//                Console.WriteLine($"存储成功！E退出，任意键继续");
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
