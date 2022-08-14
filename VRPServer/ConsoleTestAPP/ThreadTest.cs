using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ConsoleTestAPP
{
    internal class ThreadTest
    {
        internal static void Test()
        {
            Thread th = new Thread(f1);
            int k = 0;
            do
            {
                k++;
                Thread.Sleep(500);
                Console.WriteLine($"k:{k}, {th.ThreadState}");
                if (k < 20)
                {
                    if (th.IsAlive)
                    {
                        Console.WriteLine($"k:{k}线程活");
                    }
                    else
                    {
                        Console.WriteLine($"k:{k}线程死");
                    }
                }
                else if (k == 20)
                    if (th.IsAlive) { }
                    else
                    {

                        th.Start();
                    }
                else if (k == 25) 
                {
                    th = new Thread(f1);
                }
                else if (k == 27)
                {
                    th = new Thread(f1);
                }
                else if (k > 20)
                {
                    if (th.IsAlive)
                    {
                        Console.WriteLine($"线程活");
                    }
                    else
                    {
                        Console.WriteLine($"线程死");
                        //th.Start();
                    }
                }
            }
            while (true);
            // throw new NotImplementedException();
        }

        static void f1()
        {
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(500);
                Console.WriteLine(($"{i},"));
            }
        }
    }
}
