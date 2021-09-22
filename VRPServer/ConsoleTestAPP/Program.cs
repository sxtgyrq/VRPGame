using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleTestAPP
{
    class Program
    {
        class tt
        {
            public int a;
            public int b;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("你好啊，测试员！！！");

            Random rm = new Random(DateTime.Now.GetHashCode());

            for (var i = 0; i < 100; i++)
            {
                var r = rm.NextDouble() * 2 - 1;
                var angle = Math.Acos(r);
                Console.WriteLine($"{r}--{angle}");
            }
            Console.ReadLine();
        }
    }
}
