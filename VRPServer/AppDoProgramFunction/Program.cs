using System;

namespace AppDoProgramFunction
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                //Consol.WriteLine("A/加密，其他解密,exit，退出");

                var input = Console.ReadLine().Trim();
                if (input == "exit")
                {
                    return;
                }
                if (input == "A")
                {

                    //Consol.WriteLine("输入明文");
                    var content = Console.ReadLine();
                    //Consol.WriteLine("输入密钥");
                    var key = Console.ReadLine();
                    var result = CommonClass.AES.AesEncrypt(content, key);
                    //Consol.WriteLine($"{result}");
                    //Consol.WriteLine("Hello World!");
                    Console.ReadLine();
                }
                else
                {
                    //Consol.WriteLine("输入密文");
                    var content = Console.ReadLine();
                    //Consol.WriteLine("输入密钥");
                    var key = Console.ReadLine();
                    var result = CommonClass.AES.AesDecrypt(content, key);
                    //Consol.WriteLine("以下为解密结果");
                    //Consol.WriteLine($"{result}");
                    Console.ReadLine();
                }
            }

        }
    }
}
