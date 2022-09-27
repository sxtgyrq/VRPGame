using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BitCoin.GamePathEncryption
{
    public class PathEncryption
    {
        interface CurveParameter
        {
            //public const Int64 p = 90006799;
            //public const Int64 a = 0;
            //public const Int64 b = 7;
            ////public static Int64 x = 400;
            ////public static System.Numerics.BigInteger x = HexToBigInteger.inputHex("79BE667EF9DCBBAC55A06295CE870B07029BFCDB2DCE28D959F2815B16F81798");
            ////public static System.Numerics.BigInteger y = HexToBigInteger.inputHex("483ADA7726A3C4655DA4FBFC0E1108A8FD17B448A68554199C47D08FFB10D4B8");
            ////public static System.Numerics.BigInteger q = HexToBigInteger.inputHex("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEBAAEDCE6AF48A03BBFD25E8CD0364141");
            //public const Int64 q = 90021457;
            //public const Int64 x = 29106958;
            //public const Int64 y = 23443935;


            public Int64 P { get; }
            public Int64 A { get; }
            public Int64 B { get; }
            public Int64 X { get; }
            public Int64 Y { get; }
            public Int64 Q { get; }
            public Int64 QHalf { get; }
            public Int64 PHalf { get; }
            Int64[] BasePoint { get; }
        }

        public class MainC
        {
            static CurveParameter Parameter = new Parameter_0();

            static Int64 GetYByX(Int64 x)
            {
                ///这种情况只使用与 p≡3（mod4）
                ///这里要感谢NB的数学家！！！
                //https://wenku.baidu.com/view/856c1b737fd5360cba1adbd5.html
                // throw new Exception("");
                var yy = ((((x * x) % Parameter.P) * x) % Parameter.P + (Parameter.A * x) % Parameter.P + Parameter.B) % Parameter.P;//(((x * x    * x) % Secp256k1.p + Secp256k1.a * x + Secp256k1.b) % Secp256k1.p;
                                                                                                                                     //var y=

                //28948022309329048855892746252171976963317496166410141009864396001977208667916
                // Console.WriteLine((ECCMain.Secp256k1.p + 1) / 4);
                long y = 1;
                // var baseP = new BigInteger(1);
                //bool isZero = false;
                var r = get((Parameter.P + 1) / 4);
                for (int i = 0; i < r.Length; i++)
                {
                    if (!r[i])
                    {
                        y = (y * yy) % Parameter.P;
                    }
                    yy = (yy * yy) % Parameter.P;

                }
                return y;
            }
            static bool[] get(Int64 Integers64)
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
            static Int64[] getMulValue(Int64 rValue, Int64[] baseP)
            {
                if (rValue > 0)
                {
                    bool isZero;
                    Int64[] result = null;
                    var r = get(rValue);
                    //bool isZero = false;
                    for (int i = 0; i < r.Length; i++)
                    {
                        //if (i == 6)
                        //{
                        //    var x = 0;
                        //    x++;
                        //}
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
                                    // Console.WriteLine($"{i}-result-{result[0]},{result[1]}");
                                }
                            }
                        }

                        baseP = getDoubleP(baseP, out isZero);
                        //   //Consol.WriteLine($"{i}-baseP-{baseP[0]},{baseP[1]}");
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

            static Int64[] pointPlus(Int64[] point_P, Int64[] point_Q, out bool isZero)
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
                    var s = (point_P[1] - point_Q[1]) * (ex_gcd((point_P[0] - point_Q[0] + Parameter.P) % Parameter.P, Parameter.P));
                    s = s % Parameter.P;
                    if (s < 0)
                    {
                        s += Parameter.P;
                    }
                    var Xr = (s * s - (point_P[0] + point_Q[0])) % Parameter.P;

                    var Yr = (s * (point_P[0] - Xr) - point_P[1]) % Parameter.P;
                    while (Xr < 0)
                    {
                        Xr += Parameter.P;
                    }
                    while (Yr < 0)
                    {
                        Yr += Parameter.P;
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


            static Int64 ex_gcd(Int64 a, Int64 b)
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

            static Int64 ex_gcd(Int64 a, Int64 b, out Int64 x, out Int64 y)
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



            static bool CheckXYIsRight(Int64 x, Int64 y)
            {
                var r1 = (y * y) % Parameter.P;
                var r2 = ((((x * x) % Parameter.P) * x) % Parameter.P + Parameter.A * x + Parameter.B) % Parameter.P;
                return r1 == r2;
            }
            static Int64[] getDoubleP(Int64[] bigIntegers, out bool isZero)
            {
                var x = bigIntegers[0];
                var y = bigIntegers[1];
                {
                    //   var s = ((3 * x * x + Secp256k1.a) * (Inverse.ex_gcd((2 * y) % Secp256k1.p, Secp256k1.p))) % Secp256k1.p;
                    var s = (((3 * x * x + Parameter.A) % Parameter.P) * (ex_gcd((2 * y) % Parameter.P, Parameter.P))) % Parameter.P;
                    var Xr = (s * s - 2 * x) % Parameter.P;
                    while (Xr < 0)
                    {
                        Xr += Parameter.P;
                    }
                    //Xr = Xr % p;
                    var Yr = (s * (x - Xr) - y) % Parameter.P;
                    while (Yr < 0)
                    {
                        Yr += Parameter.P;
                    }
                    isZero = false;
                    return new Int64[] { Xr, Yr };
                }
            }
            public static List<long> Decrypt(List<long> dataEncrypted, long privateKey)
            {
                var newData = new List<long>();
                var array = dataEncrypted.ToArray();
                // array.Length
                for (var i = 0; i < dataEncrypted.Count; i += 5)
                {
                    var rP = new Int64[] { array[i], array[i + 1] };
                    var MplusrQ = new Int64[] { array[i + 2], array[i + 3] };
                    var delta = array[i + 4];

                    var krp = getMulValue(Parameter.Q - privateKey, rP);
                    bool isZero;
                    var result = pointPlus(MplusrQ, krp, out isZero);

                    newData.Add((result[0] + delta) % Parameter.P - Parameter.PHalf);

                }
                return newData;
            }

            static readonly int[] primeNumbers = { 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97 };
            public static long getPreviousPrivateKey(long a)
            {
                var q = Parameter.Q;
                // const int[] primeNumbers = { 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97 };
                var primeCount = primeNumbers.Length;

                var pIndex = a % primeCount;

                long a0 = 1;
                List<long> aArray = new List<long>();
                List<long> pArray = new List<long>();
                for (var i = 0; i < 50; i++)
                {
                    aArray.Add(a0);
                    a0 = (a0 * a) % q;
                    var pv = primeNumbers[(pIndex + i) % primeCount];
                    pArray.Add(pv);
                }
                long sum = 0;
                for (var i = 0; i < 50; i++)
                {
                    sum += (pArray[i] * aArray[i]);
                    sum = sum % q;
                }
                return sum % q;
            }
            public static List<long> GetPrivateKeys(ref Random rm, int length)
            {
                var privateKey = Convert.ToInt64(rm.Next(Convert.ToInt32(Parameter.QHalf), Convert.ToInt32(Parameter.Q)));
                var result = new List<long>();
                while (result.Count < length)
                {
                    result.Insert(0, privateKey);
                    privateKey = getPreviousPrivateKey(privateKey);
                }
                return result;
            }
            public static List<long> Encrypt(ref Random rm, List<long> data, long privateKey)
            {

                //privateKey = rm.Next(Convert.ToInt32(Parameter.QHalf), Convert.ToInt32(Parameter.Q));
                var newData = new List<long>();

                //   //Consol.WriteLine("解密之前");
                for (int i = 0; i < data.Count; i++)
                {
                    //Consol.WriteLine(data[i]);
                    data[i] += Parameter.PHalf;
                    if (data[i] < 0 || data[i] >= Parameter.P)
                    {
                        throw new Exception("参数异常");
                    }
                    bool isZero;
                    do
                    {
                        var r = rm.Next(Convert.ToInt32(Parameter.QHalf), Convert.ToInt32(Parameter.Q));//随机值
                        var M = getMulValue(23, getMulValue(r - 17, getMulValue(r - 29, Parameter.BasePoint)));//确保M为随机信息,这里之所以要二次，是rp是明文
                        var Q = getMulValue(privateKey, Parameter.BasePoint);//Q 为公钥
                        var rQ = getMulValue(r, Q);
                        var MplusrQ = pointPlus(M, rQ, out isZero);

                        if (isZero) { }
                        else
                        {
                            var delta = (data[i] - M[0] + Parameter.P) % Parameter.P; //delta为差值，方便从M(信息)计算真实值
                            var rP = getMulValue(r, Parameter.BasePoint);
                            newData.Add(rP[0]);
                            newData.Add(rP[1]);
                            newData.Add(MplusrQ[0]);
                            newData.Add(MplusrQ[1]);
                            newData.Add(delta);
                            break;
                        }
                    } while (isZero);
                    //R(真是信息)=Δ+M[0]
                    //M=(r-1)((r-2)P)
                    //M+rQ

                    //M+rQ  -krP=M
                }

                var array = newData.ToArray();
                var str = "";
                for (int i = 0; i < array.Length; i++)
                {
                    str += $"{array[i]},";
                }
                //  Console.WriteLine($"{Environment.NewLine}{str}");
                str = "";
                for (int i = 0; i < data.Count; i++)
                {
                    str += $"{data[i]},";
                }
                //Console.WriteLine($"{Environment.NewLine}{str}");
                //Console.WriteLine($"{Environment.NewLine}私钥:{privateKey}");
                //Console.WriteLine("加密结束");

                /*
                  //for (var i = 0; i < array.Length; i += 5)
                //{
                //    var rP = new Int64[] { array[i], array[i + 1] };
                //    var MplusrQ = new Int64[] { array[i + 2], array[i + 3] };
                //    var delta = array[i + 4];

                //    var krp = getMulValue(Parameter.Q - privateKey, rP);
                //    bool isZero;
                //    var result = pointPlus(MplusrQ, krp, out isZero);

                //    if ((result[0] + delta - Parameter.PHalf + Parameter.P) % Parameter.P == data[i / 5] - Parameter.PHalf)
                //    {
                //        //Consol.WriteLine($"加密解密前后相等,{(result[0] + delta - Parameter.PHalf + Parameter.P) % Parameter.P }！={data[i / 5] - Parameter.PHalf}");
                //    }
                //    else
                //    {
                //        //Consol.WriteLine($"加密解密前后不等,{(result[0] + delta - Parameter.PHalf + Parameter.P) % Parameter.P }！={data[i / 5] - Parameter.PHalf}");
                //        //  throw new Exception("加密解密前后不等");
                //    }
                //    //  console.log(i, (result[0] + delta - pHalf + p) % p);
                //    //var va
                //}
                 */

                //   //Consol.WriteLine("校验OK");
                return newData;
            }
            //public static Int64 getQ()
            //{
            //    {
            //        Dictionary<Int64, Int64> Result = new Dictionary<long, long>();
            //        {

            //            if ((y * y) % p == (((x * x) % p) * x + a * x + b) % p)
            //            {
            //                //Consol.WriteLine($"找到x={x},y={y}");
            //                // Console.ReadLine();

            //                var basePoint = new Int64[] { x, y };
            //                //List<Int64[]> points2 = new List<Int64[]>();
            //                //points2.Add(new Int64[] { basePoint[0], basePoint[1] });
            //                //Dictionary<Int64, Int64[]> allDoubleValue = new Dictionary<long, long[]>();
            //                // while()
            //                var result = new Int64[] { x, y };
            //                bool isZero = false;
            //                //  int indexValue = 0;
            //                {
            //                    var right = CheckXYIsRight(x, y);
            //                    //Consol.WriteLine($"基点[{right}]在曲线上");
            //                }
            //                var index = 1;
            //                for (int i = 0; i < Int32.MaxValue; i++)
            //                {
            //                    result = Parameter.pointPlus(basePoint, result, out isZero);
            //                    if (isZero) { }
            //                    else
            //                    {
            //                        index++;
            //                        var mul = Parameter.getMulValue(index, basePoint);
            //                        if (mul[0] != result[0] || mul[1] != result[1])
            //                        {
            //                            //Consol.WriteLine($"{i}-累加与乘法结果不等！");
            //                            Console.ReadLine();
            //                        }
            //                    }
            //                    if (isZero)
            //                    {
            //                        break;
            //                    }
            //                    else
            //                    {
            //                        var right = CheckXYIsRight(result[0], result[1]);
            //                        //Consol.WriteLine($"result[{ i + 2}]={{{result[0]},{result[1]}}}---在曲线上{right}");
            //                        if (!right)
            //                        {
            //                            Console.ReadLine();
            //                        }
            //                    }
            //                }
            //                //Consol.WriteLine($"所求结果{index}");
            //                return index + 1;
            //                // for (int i = -2; i <= 2; i++)
            //                //{
            //                //    Console.ReadLine();
            //                //    var another = Parameter.getMulValue(count, basePoint);
            //                //    var onCurve = CheckXYIsRight(another[0], another[1]);
            //                //    //Consol.WriteLine($"相乘结果{another[0]},{another[1]},{onCurve}");
            //                //    //Consol.WriteLine($"累加结果{result[0]},{result[1]}");
            //                //}
            //                //  Console.ReadLine();
            //                //for (int i = 0; i < q - 1; i++)
            //                //{
            //                //    var addValue = new Int64[] { x, y };
            //                //    var sum =
            //                //}
            //                //for (int i = 1; i < q; i *= 2)
            //                //{
            //                //    //   Dictionary<>
            //                //    bool isZero;
            //                //    var doubleValue = getDoubleP(basePoint, out isZero);
            //                //    if (isZero)
            //                //    {
            //                //        Console.Write("isZero");
            //                //        Console.ReadLine();
            //                //    }
            //                //    allDoubleValue.Add(2 << (i - 0), new Int64[] { doubleValue[0], doubleValue[1] });
            //                //    points2.Add(new Int64[] { doubleValue[0], doubleValue[1] });
            //                //    basePoint = doubleValue;
            //                //}
            //                //   Console.ReadLine();
            //                //foreach(var )

            //            }
            //            else
            //            {
            //                //Consol.WriteLine($"找到不相等");
            //                Console.ReadLine();
            //                throw new Exception("找到不相等");
            //            }
            //        }
            //    }
            //}
        }
        class Parameter_0 : CurveParameter
        {

            //素数结果-- p=90006799,x=29106958,y=23443935,q=90021457

            const Int64 p = 90006799;
            const Int64 a = 0;
            const Int64 b = 7;
            //public static Int64 x = 400;
            //public static System.Numerics.BigInteger x = HexToBigInteger.inputHex("79BE667EF9DCBBAC55A06295CE870B07029BFCDB2DCE28D959F2815B16F81798");
            //public static System.Numerics.BigInteger y = HexToBigInteger.inputHex("483ADA7726A3C4655DA4FBFC0E1108A8FD17B448A68554199C47D08FFB10D4B8");
            //public static System.Numerics.BigInteger q = HexToBigInteger.inputHex("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEBAAEDCE6AF48A03BBFD25E8CD0364141");
            const Int64 q = 90021457;
            const Int64 x = 29106958;
            const Int64 y = 23443935;

            public long QHalf { get { return q / 2; } }

            public long PHalf { get { return p / 2; } }

            public long[] BasePoint { get { return new long[] { x, y }; } }

            //    public long BasePoint { get };

            long CurveParameter.P { get { return p; } }

            long CurveParameter.A { get { return a; } }

            long CurveParameter.B { get { return b; } }

            long CurveParameter.X { get { return x; } }

            long CurveParameter.Y { get { return y; } }

            long CurveParameter.Q { get { return q; } }
        }
    }
}
