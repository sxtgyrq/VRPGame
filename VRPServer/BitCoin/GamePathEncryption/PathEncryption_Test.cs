using System;
using System.Collections.Generic;

namespace BitCoin.GamePathEncryption
{
    class PathEncryption_Test
    {
        static bool IsPrime(Int64 number)
        {
            if (number <= 1) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            var boundary = (int)Math.Floor(Math.Sqrt(number));

            for (int i = 3; i <= boundary; i += 2)
                if (number % i == 0)
                    return false;

            return true;
        }
        static void Main()
        {
            Parameter2.MsgToCurve();
            Console.ReadLine();
            //for (Int64 i = 90000000; i <= 100000000; i++)
            //{
            //    if (i % 4 == 3)
            //    {
            //        if (IsPrime(i))
            //        {
            //            //Consol.WriteLine($"-----------------------------------");
            //            //Consol.WriteLine($"-------------开始计算{i}-----------");
            //            Parameter.SetP(i);
            //            Parameter.getXY();
            //            var q = Parameter.getQ();
            //            //Consol.WriteLine($"p={Parameter.p},x={Parameter.x},y={Parameter.y},q={q}");
            //            if (q >= i)
            //            {
            //                if (IsPrime(q))
            //                {
            //                    //Consol.WriteLine($"素数结果-- p={Parameter.p},x={Parameter.x},y={Parameter.y},q={q}");
            //                    //Consol.WriteLine("按任意键继续！");

            //                    var msg = $"素数结果-- p={Parameter.p},x={Parameter.x},y={Parameter.y},q={q}";
            //                    File.WriteAllText("calResult.txt", msg);
            //                    while (true)
            //                        Console.ReadLine();
            //                }
            //                else
            //                {
            //                    //Consol.WriteLine($"{q}不是素数");
            //                }
            //            }
            //            else
            //            {
            //                //Consol.WriteLine($"{q}<85000000,结果不符！");
            //            }
            //        }
            //        else
            //        {
            //            //Consol.WriteLine($"{i}不是素数");
            //        }
            //    }
            //    else
            //    {
            //        //Consol.WriteLine($"{i}%4≠3");
            //    }


            //}
        }


        class Parameter_0
        {
            //素数结果-- p=90151669,x=25737828,y=65731303,q=90165217

            //素数结果-- p=90006799,x=29106958,y=23443935,q=90021457

            public static Int64 p = 90151669;
            public const Int64 a = 0;
            public const Int64 b = 7;
            //public static Int64 x = 400;
            //public static System.Numerics.BigInteger x = HexToBigInteger.inputHex("79BE667EF9DCBBAC55A06295CE870B07029BFCDB2DCE28D959F2815B16F81798");
            //public static System.Numerics.BigInteger y = HexToBigInteger.inputHex("483ADA7726A3C4655DA4FBFC0E1108A8FD17B448A68554199C47D08FFB10D4B8");
            //public static System.Numerics.BigInteger q = HexToBigInteger.inputHex("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEBAAEDCE6AF48A03BBFD25E8CD0364141");
            public const Int64 q = 90165217;
            public static Int64 x = 25737828;
            public static Int64 y = 65731303;


            private static bool[] get(Int64 Integers64)
            {
                List<bool> result = new List<bool>();
                while (Integers64 != 0)
                {
                    if (Integers64 % 2 == 0)
                    {
                        result.Add(true);
                    }
                    else
                    {
                        result.Add(false);
                    }
                    Integers64 = Integers64 / 2;
                }
                return result.ToArray();
            }
            internal static Int64[] getMulValue(Int64 rValue, Int64[] baseP)
            {
                if (rValue > 0)
                {
                    bool isZero;
                    Int64[] result = null;
                    var r = get(rValue);
                    //bool isZero = false;
                    for (int i = 0; i < r.Length; i++)
                    {
                        if (!r[i])
                        {
                            if (baseP == null)
                            {
                            }
                            else
                            {
                                if (result == null)
                                {
                                    result = baseP;
                                }
                                else
                                {
                                    result = pointPlus(result, baseP, out isZero);
                                }
                            }
                        }
                        baseP = getDoubleP(baseP, out isZero);
                    }
                    return result;
                }
                else if (rValue == 0)
                {
                    return null;
                }
                else
                {
                    throw new Exception("privateKey的值不能为0和负数");
                }
            }

            public static Int64[] pointPlus(Int64[] point_P, Int64[] point_Q, out bool isZero)
            {
                // throw new Exception("");
                if (point_P[0] == point_Q[0])
                {
                    if (point_P[1] == point_Q[1])
                    {
                        return getDoubleP(point_P, out isZero);
                    }
                    else
                    {
                        isZero = true;
                        return null;
                    }
                }
                else
                {
                    isZero = false;
                    var s = (point_P[1] - point_Q[1]) * (Parameter_0.ex_gcd((point_P[0] - point_Q[0] + Parameter_0.p) % Parameter_0.p, Parameter_0.p));
                    s = s % Parameter_0.p;
                    if (s < 0)
                    {
                        s += Parameter_0.p;
                    }
                    var Xr = (s * s - (point_P[0] + point_Q[0])) % Parameter_0.p;

                    var Yr = (s * (point_P[0] - Xr) - point_P[1]) % Parameter_0.p;
                    while (Xr < 0)
                    {
                        Xr += Parameter_0.p;
                    }
                    while (Yr < 0)
                    {
                        Yr += Parameter_0.p;
                    }
                    return new Int64[] { Xr, Yr };
                }
                //  var s=a[]
                //var x = bigIntegers[0];
                //var y = bigIntegers[1];
                //var s = ((3 * x * x + a) * (ECCMain.Inverse.ex_gcd(2 * y, p))) % p;
                //var Xr = (s * s - 2 * x) % p;
                //while (Xr < 0)
                //{
                //    Xr += p;
                //}
                ////Xr = Xr % p;
                //var Yr = (s * (x - Xr) - y) % p;
                //while (Yr < 0)
                //{
                //    Yr += p;
                //}
                //return new BigInteger[] { Xr, Yr };
            }


            public static Int64 ex_gcd(Int64 a, Int64 b)
            {
                //while (a < 0)
                //{
                //    a += b;
                //}
                a = a % b;
                if (a < 0)
                {
                    a += b;
                }
                Int64 x, y;
                ex_gcd(a, b, out x, out y);
                if (x > 0)
                {
                    return x;
                }
                else
                {
                    return b + x;
                }
            }

            public static Int64 ex_gcd(Int64 a, Int64 b, out Int64 x, out Int64 y)
            {
                Int64 ret, tmp;
                if (b == 0)
                {
                    x = 1;
                    y = 0;
                    return a;
                }
                ret = ex_gcd(b, a % b, out x, out y);
                tmp = x;
                x = y;
                y = tmp - a / b * y;
                return ret;
            }



            internal static bool CheckXYIsRight(Int64 x, Int64 y)
            {
                var r1 = (y * y) % Parameter_0.p;
                var r2 = ((((x * x) % Parameter_0.p) * x) % Parameter_0.p + Parameter_0.a * x + Parameter_0.b) % Parameter_0.p;
                return r1 == r2;
            }
            private static Int64[] getDoubleP(Int64[] bigIntegers, out bool isZero)
            {
                var x = bigIntegers[0];
                var y = bigIntegers[1];
                {
                    //   var s = ((3 * x * x + Secp256k1.a) * (Inverse.ex_gcd((2 * y) % Secp256k1.p, Secp256k1.p))) % Secp256k1.p;
                    var s = (((3 * x * x + Parameter_0.a) % Parameter_0.p) * (Parameter_0.ex_gcd((2 * y) % Parameter_0.p, Parameter_0.p))) % Parameter_0.p;
                    var Xr = (s * s - 2 * x) % Parameter_0.p;
                    while (Xr < 0)
                    {
                        Xr += Parameter_0.p;
                    }
                    //Xr = Xr % p;
                    var Yr = (s * (x - Xr) - y) % Parameter_0.p;
                    while (Yr < 0)
                    {
                        Yr += Parameter_0.p;
                    }
                    isZero = false;
                    return new Int64[] { Xr, Yr };
                }
            }
            public static Int64 getQ()
            {
                {
                    Dictionary<Int64, Int64> Result = new Dictionary<long, long>();
                    {

                        if ((y * y) % p == (((x * x) % p) * x + a * x + b) % p)
                        {
                            //Consol.WriteLine($"找到x={x},y={y}");
                            // Console.ReadLine();

                            var basePoint = new Int64[] { x, y };
                            //List<Int64[]> points2 = new List<Int64[]>();
                            //points2.Add(new Int64[] { basePoint[0], basePoint[1] });
                            //Dictionary<Int64, Int64[]> allDoubleValue = new Dictionary<long, long[]>();
                            // while()
                            var result = new Int64[] { x, y };
                            bool isZero = false;
                            //  int indexValue = 0;
                            {
                                var right = CheckXYIsRight(x, y);
                                //Consol.WriteLine($"基点[{right}]在曲线上");
                            }
                            var index = 1;
                            for (int i = 0; i < Int32.MaxValue; i++)
                            {
                                result = Parameter_0.pointPlus(basePoint, result, out isZero);
                                if (isZero) { }
                                else
                                {
                                    index++;
                                    //var mul = Parameter.getMulValue(index, basePoint);
                                    //if (mul[0] != result[0] || mul[1] != result[1])
                                    //{
                                    //    //Consol.WriteLine($"{i}-累加与乘法结果不等！");
                                    //    Console.ReadLine();
                                    //}
                                }
                                if (isZero)
                                {
                                    break;
                                }
                                else
                                {
                                    //var right = CheckXYIsRight(result[0], result[1]);
                                    //// Console.WriteLine($"result[{ i + 2}]={{{result[0]},{result[1]}}}---在曲线上{right}");
                                    //if (!right)
                                    //{
                                    //    Console.ReadLine();
                                    //}
                                }
                            }
                            //Consol.WriteLine($"所求结果{index}");
                            return index + 1;
                            // for (int i = -2; i <= 2; i++)
                            //{
                            //    Console.ReadLine();
                            //    var another = Parameter.getMulValue(count, basePoint);
                            //    var onCurve = CheckXYIsRight(another[0], another[1]);
                            //    //Consol.WriteLine($"相乘结果{another[0]},{another[1]},{onCurve}");
                            //    //Consol.WriteLine($"累加结果{result[0]},{result[1]}");
                            //}
                            //  Console.ReadLine();
                            //for (int i = 0; i < q - 1; i++)
                            //{
                            //    var addValue = new Int64[] { x, y };
                            //    var sum =
                            //}
                            //for (int i = 1; i < q; i *= 2)
                            //{
                            //    //   Dictionary<>
                            //    bool isZero;
                            //    var doubleValue = getDoubleP(basePoint, out isZero);
                            //    if (isZero)
                            //    {
                            //        Console.Write("isZero");
                            //        Console.ReadLine();
                            //    }
                            //    allDoubleValue.Add(2 << (i - 0), new Int64[] { doubleValue[0], doubleValue[1] });
                            //    points2.Add(new Int64[] { doubleValue[0], doubleValue[1] });
                            //    basePoint = doubleValue;
                            //}
                            //   Console.ReadLine();
                            //foreach(var )

                        }
                        else
                        {
                            //Consol.WriteLine($"找到不相等");
                            Console.ReadLine();
                            throw new Exception("找到不相等");
                        }
                    }
                }
            }

            public static void getXY()
            {
                var random = new Random();
                while (true)
                {
                    var x = random.Next(0, Convert.ToInt32(Parameter_0.p));
                    var y = random.Next(0, Convert.ToInt32(Parameter_0.p));
                    if (Parameter_0.CheckXYIsRight(x, y))
                    {
                        Parameter_0.x = x;
                        Parameter_0.y = y;
                        //Consol.WriteLine($"碰撞结果x:{x},y:{y}");
                        break;
                    }
                }
            }

            internal static void SetP(long i)
            {
                Parameter_0.p = i;
            }
        }

        class Parameter2
        {

            //素数结果-- p=90006799,x=29106958,y=23443935,q=90021457

            public const Int64 p = 90006799;
            public const Int64 a = 0;
            public const Int64 b = 7;
            //public static Int64 x = 400;
            //public static System.Numerics.BigInteger x = HexToBigInteger.inputHex("79BE667EF9DCBBAC55A06295CE870B07029BFCDB2DCE28D959F2815B16F81798");
            //public static System.Numerics.BigInteger y = HexToBigInteger.inputHex("483ADA7726A3C4655DA4FBFC0E1108A8FD17B448A68554199C47D08FFB10D4B8");
            //public static System.Numerics.BigInteger q = HexToBigInteger.inputHex("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEBAAEDCE6AF48A03BBFD25E8CD0364141");
            public const Int64 q = 90021457;
            public const Int64 x = 29106958;
            public const Int64 y = 23443935;



            internal static Int64 GetYByX(Int64 x)
            {
                ///这种情况只使用与 p≡3（mod4）
                ///这里要感谢NB的数学家！！！
                //https://wenku.baidu.com/view/856c1b737fd5360cba1adbd5.html
                // throw new Exception("");
                var yy = ((((x * x) % Parameter2.p) * x) % Parameter2.p + (Parameter2.a * x) % Parameter2.p + Parameter2.b) % Parameter2.p;//(((x * x    * x) % Secp256k1.p + Secp256k1.a * x + Secp256k1.b) % Secp256k1.p;
                                                                                                                                           //var y=

                //28948022309329048855892746252171976963317496166410141009864396001977208667916
                // Console.WriteLine((ECCMain.Secp256k1.p + 1) / 4);
                long y = 1;
                // var baseP = new BigInteger(1);
                //bool isZero = false;
                var r = get((Parameter2.p + 1) / 4);
                for (int i = 0; i < r.Length; i++)
                {
                    if (!r[i])
                    {
                        y = (y * yy) % Parameter2.p;
                    }
                    yy = (yy * yy) % Parameter2.p;

                }
                return y;
            }
            private static bool[] get(Int64 Integers64)
            {
                List<bool> result = new List<bool>();
                while (Integers64 != 0)
                {
                    if (Integers64 % 2 == 0)
                    {
                        result.Add(true);
                    }
                    else
                    {
                        result.Add(false);
                    }
                    Integers64 = Integers64 / 2;
                }
                return result.ToArray();
            }
            internal static Int64[] getMulValue(Int64 rValue, Int64[] baseP)
            {
                if (rValue > 0)
                {
                    bool isZero;
                    Int64[] result = null;
                    var r = get(rValue);
                    //bool isZero = false;
                    for (int i = 0; i < r.Length; i++)
                    {
                        if (!r[i])
                        {
                            if (baseP == null)
                            {
                            }
                            else
                            {
                                if (result == null)
                                {
                                    result = baseP;
                                }
                                else
                                {
                                    result = pointPlus(result, baseP, out isZero);
                                }
                            }
                        }
                        baseP = getDoubleP(baseP, out isZero);
                    }
                    return result;
                }
                else if (rValue == 0)
                {
                    return null;
                }
                else
                {
                    throw new Exception("privateKey的值不能为0和负数");
                }
            }

            public static Int64[] pointPlus(Int64[] point_P, Int64[] point_Q, out bool isZero)
            {
                // throw new Exception("");
                if (point_P[0] == point_Q[0])
                {
                    if (point_P[1] == point_Q[1])
                    {
                        return getDoubleP(point_P, out isZero);
                    }
                    else
                    {
                        isZero = true;
                        return null;
                    }
                }
                else
                {
                    isZero = false;
                    var s = (point_P[1] - point_Q[1]) * (Parameter2.ex_gcd((point_P[0] - point_Q[0] + Parameter2.p) % Parameter2.p, Parameter2.p));
                    s = s % Parameter2.p;
                    if (s < 0)
                    {
                        s += Parameter2.p;
                    }
                    var Xr = (s * s - (point_P[0] + point_Q[0])) % Parameter2.p;

                    var Yr = (s * (point_P[0] - Xr) - point_P[1]) % Parameter2.p;
                    while (Xr < 0)
                    {
                        Xr += Parameter2.p;
                    }
                    while (Yr < 0)
                    {
                        Yr += Parameter2.p;
                    }
                    return new Int64[] { Xr, Yr };
                }
                //  var s=a[]
                //var x = bigIntegers[0];
                //var y = bigIntegers[1];
                //var s = ((3 * x * x + a) * (ECCMain.Inverse.ex_gcd(2 * y, p))) % p;
                //var Xr = (s * s - 2 * x) % p;
                //while (Xr < 0)
                //{
                //    Xr += p;
                //}
                ////Xr = Xr % p;
                //var Yr = (s * (x - Xr) - y) % p;
                //while (Yr < 0)
                //{
                //    Yr += p;
                //}
                //return new BigInteger[] { Xr, Yr };
            }


            public static Int64 ex_gcd(Int64 a, Int64 b)
            {
                //while (a < 0)
                //{
                //    a += b;
                //}
                a = a % b;
                if (a < 0)
                {
                    a += b;
                }
                Int64 x, y;
                ex_gcd(a, b, out x, out y);
                if (x > 0)
                {
                    return x;
                }
                else
                {
                    return b + x;
                }
            }

            public static Int64 ex_gcd(Int64 a, Int64 b, out Int64 x, out Int64 y)
            {
                Int64 ret, tmp;
                if (b == 0)
                {
                    x = 1;
                    y = 0;
                    return a;
                }
                ret = ex_gcd(b, a % b, out x, out y);
                tmp = x;
                x = y;
                y = tmp - a / b * y;
                return ret;
            }



            internal static bool CheckXYIsRight(Int64 x, Int64 y)
            {
                var r1 = (y * y) % Parameter2.p;
                var r2 = ((((x * x) % Parameter2.p) * x) % Parameter2.p + Parameter2.a * x + Parameter2.b) % Parameter2.p;
                return r1 == r2;
            }
            private static Int64[] getDoubleP(Int64[] bigIntegers, out bool isZero)
            {
                var x = bigIntegers[0];
                var y = bigIntegers[1];
                {
                    //   var s = ((3 * x * x + Secp256k1.a) * (Inverse.ex_gcd((2 * y) % Secp256k1.p, Secp256k1.p))) % Secp256k1.p;
                    var s = (((3 * x * x + Parameter2.a) % Parameter2.p) * (Parameter2.ex_gcd((2 * y) % Parameter2.p, Parameter2.p))) % Parameter2.p;
                    var Xr = (s * s - 2 * x) % Parameter2.p;
                    while (Xr < 0)
                    {
                        Xr += Parameter2.p;
                    }
                    //Xr = Xr % p;
                    var Yr = (s * (x - Xr) - y) % Parameter2.p;
                    while (Yr < 0)
                    {
                        Yr += Parameter2.p;
                    }
                    isZero = false;
                    return new Int64[] { Xr, Yr };
                }
            }
            public static Int64 getQ()
            {
                {
                    Dictionary<Int64, Int64> Result = new Dictionary<long, long>();
                    {

                        if ((y * y) % p == (((x * x) % p) * x + a * x + b) % p)
                        {
                            //Consol.WriteLine($"找到x={x},y={y}");
                            // Console.ReadLine();

                            var basePoint = new Int64[] { x, y };
                            //List<Int64[]> points2 = new List<Int64[]>();
                            //points2.Add(new Int64[] { basePoint[0], basePoint[1] });
                            //Dictionary<Int64, Int64[]> allDoubleValue = new Dictionary<long, long[]>();
                            // while()
                            var result = new Int64[] { x, y };
                            bool isZero = false;
                            //  int indexValue = 0;
                            {
                                var right = CheckXYIsRight(x, y);
                                //Consol.WriteLine($"基点[{right}]在曲线上");
                            }
                            var index = 1;
                            for (int i = 0; i < Int32.MaxValue; i++)
                            {
                                result = Parameter2.pointPlus(basePoint, result, out isZero);
                                if (isZero) { }
                                else
                                {
                                    index++;
                                    var mul = Parameter2.getMulValue(index, basePoint);
                                    if (mul[0] != result[0] || mul[1] != result[1])
                                    {
                                        //Console.WriteLine($"{i}-累加与乘法结果不等！");
                                        Console.ReadLine();
                                    }
                                }
                                if (isZero)
                                {
                                    break;
                                }
                                else
                                {
                                    var right = CheckXYIsRight(result[0], result[1]);
                                    //Console.WriteLine($"result[{ i + 2}]={{{result[0]},{result[1]}}}---在曲线上{right}");
                                    if (!right)
                                    {
                                        Console.ReadLine();
                                    }
                                }
                            }
                            //Console.WriteLine($"所求结果{index}");
                            return index + 1;
                            // for (int i = -2; i <= 2; i++)
                            //{
                            //    Console.ReadLine();
                            //    var another = Parameter.getMulValue(count, basePoint);
                            //    var onCurve = CheckXYIsRight(another[0], another[1]);
                            //    //Console.WriteLine($"相乘结果{another[0]},{another[1]},{onCurve}");
                            //    //Console.WriteLine($"累加结果{result[0]},{result[1]}");
                            //}
                            //  Console.ReadLine();
                            //for (int i = 0; i < q - 1; i++)
                            //{
                            //    var addValue = new Int64[] { x, y };
                            //    var sum =
                            //}
                            //for (int i = 1; i < q; i *= 2)
                            //{
                            //    //   Dictionary<>
                            //    bool isZero;
                            //    var doubleValue = getDoubleP(basePoint, out isZero);
                            //    if (isZero)
                            //    {
                            //        Console.Write("isZero");
                            //        Console.ReadLine();
                            //    }
                            //    allDoubleValue.Add(2 << (i - 0), new Int64[] { doubleValue[0], doubleValue[1] });
                            //    points2.Add(new Int64[] { doubleValue[0], doubleValue[1] });
                            //    basePoint = doubleValue;
                            //}
                            //   Console.ReadLine();
                            //foreach(var )

                        }
                        else
                        {
                            //Consol.WriteLine($"找到不相等");
                            Console.ReadLine();
                            throw new Exception("找到不相等");
                        }
                    }
                }
            }

            public static void MsgToCurve()
            {
                for (int i = 0; i < Parameter2.p; i++)
                {
                    var x = i;
                    var y = Parameter2.GetYByX(x);
                    if (Parameter2.CheckXYIsRight(x, y))
                    {
                        //Consol.WriteLine($"{i}-{x}-{y}-正确,在曲线上");
                    }
                    else
                    {
                        //Consol.WriteLine($"{i}-{x}-{y}错误");
                        Console.ReadLine();
                    }
                }
            }


        }
    }
}
