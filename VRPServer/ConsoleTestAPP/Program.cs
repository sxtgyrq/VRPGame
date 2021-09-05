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


            var a0 = new tt()
            {
                a = 1,
                b = 1
            };
            var a1 = new tt()
            {
                a = 1,
                b = 2
            };
            var a2 = new tt()
            {
                a = 2,
                b = 1
            };
            var a3 = new tt()
            {
                a = 2,
                b = 2
            };
            List<tt> tList = new List<tt>() { a1, a0, a2, a3 };
            Random rm = new Random();
            for (var i = 0; i < 100; i++)
                tList.Add(new tt()
                {
                    a = rm.Next(3, 6),
                    b = rm.Next(0, 6)
                });

            tList = (from item in tList orderby item.a ascending, item.b descending select item).ToList();

            for (int i = 0; i < tList.Count; i++) 
            {
                Console.WriteLine($"{i.ToString("000")}--a:{tList[i].a}-b:{tList[i].b}");
            }
            Console.ReadLine();
        }
    }
}
