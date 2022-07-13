using System;

namespace AppDoProgramFunction
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("A/加密，其他解密,exit，退出");

                var input = Console.ReadLine().Trim();
                if (input == "exit")
                {
                    return;
                }
                if (input == "A")
                {

                    Console.WriteLine("输入明文");
                    var content = Console.ReadLine();
                    Console.WriteLine("输入密钥");
                    var key = Console.ReadLine();
                    var result = CommonClass.AES.AesEncrypt(content, key);
                    Console.WriteLine($"{result}"); 
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
