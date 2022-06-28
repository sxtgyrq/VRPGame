using System;
using System.Numerics;
using System.Security.Cryptography;

namespace BitCoin
{
    public class PublicKeyF
    {
        //public static string GetBitcoinAddress(this ECPoint publicKey, bool compressed = true)
        //{
        //    var pubKeyHash = Hash160.Hash(publicKey.EncodePoint(compressed));

        //    byte[] addressBytes = new byte[pubKeyHash.Length + 1];
        //    Buffer.BlockCopy(pubKeyHash, 0, addressBytes, 1, pubKeyHash.Length);
        //    return Base58.EncodeWithCheckSum(addressBytes);
        //}
        public static string GetAddressOfP2SH(BigInteger[] publicKey)
        {
            return GetAddressOfP2SH(publicKey, true);
        }
        internal static string GetAddressOfP2SH(BigInteger[] publicKey, bool show)
        {
            var publicKeyArray1 = HexToByteArray.BigIntegerTo32ByteArray(publicKey[0]);
            HexToByteArray.ChangeDirection(ref publicKeyArray1);
            // var publicKeyArray2 = HexToByteArray.BigIntegerTo32ByteArray(publicKey[1]);

            byte[] resultAdd;
            if (publicKey[1].IsEven)
                resultAdd = Calculate.BiteSplitJoint(new byte[] { 0x02 }, publicKeyArray1);
            else
                resultAdd = Calculate.BiteSplitJoint(new byte[] { 0x03 }, publicKeyArray1);

            if (show)
            { }
            //Console.WriteLine($"压缩公钥为{ Hex.BytesToHex(resultAdd)}");
            //   //Consol.WriteLine($"压缩公钥为{ Calculate.Encode(resultAdd)}");

            var step3 = ripemd160.ComputeHash(sha256.ComputeHash(resultAdd));

            var step4 = Calculate.BiteSplitJoint(new byte[] { 0x00, 0x14 }, step3);

            var step5 = sha256.ComputeHash(step4);

            //   var step6 = Calculate.BiteSplitJoint(step4, new byte[] { step5[0], step5[1], step5[2], step5[3] });
            var step6 = ripemd160.ComputeHash(step5);
            var step7 = Calculate.BiteSplitJoint(new byte[] { 0x05 }, step6);
            // Console.WriteLine($"step7:{Hex.BytesToHex(step7)}");
            var step8 = sha256.ComputeHash(sha256.ComputeHash(step7));
            var step9 = Calculate.BiteSplitJoint(step7, new byte[] { step8[0], step8[1], step8[2], step8[3] });
            return Calculate.Encode5(step9);
        }
        internal static void GetAddress(BigInteger[] publicKey, bool compress)
        {
            if (compress)
            {

            }
            else
            {
                throw new Exception("");
            }
        }
        private static readonly RIPEMD160Managed ripemd160 = new RIPEMD160Managed();
        private static readonly SHA256 sha256 = new SHA256Managed();
        public static string GetAddressOfUncompressed(BigInteger[] publicKey)
        {



            var publicKeyArray1 = HexToByteArray.BigIntegerTo32ByteArray(publicKey[0]);
            HexToByteArray.ChangeDirection(ref publicKeyArray1);
            var publicKeyArray2 = HexToByteArray.BigIntegerTo32ByteArray(publicKey[1]);
            HexToByteArray.ChangeDirection(ref publicKeyArray2);
            //    var array = HexToByteArray.BigIntegerTo32ByteArray(privateKey);

            var resultAdd = Calculate.BiteSplitJoint(new byte[] { 0x04 }, publicKeyArray1);
            resultAdd = Calculate.BiteSplitJoint(resultAdd, publicKeyArray2);
            //Consol.WriteLine($"非压缩公钥为{ Hex.BytesToHex(resultAdd)}");


            var step3 = ripemd160.ComputeHash(sha256.ComputeHash(resultAdd));

            var step4 = Calculate.BiteSplitJoint(new byte[] { 0x00 }, step3);

            var step5 = sha256.ComputeHash(sha256.ComputeHash(step4));

            var step6 = Calculate.BiteSplitJoint(step4, new byte[] { step5[0], step5[1], step5[2], step5[3] });

            return Calculate.Encode(step6);

            //SHA256 sha256 = new SHA256Managed();
            //byte[] hash1 = sha256.ComputeHash(resultAdd);

            //var pubKeyHash = Hash160.Hash(publicKey.EncodePoint(hash1));
            //byte[] hash2 = sha256.ComputeHash(hash1);

            //var result = new byte[CheckSumSizeInBytes];
            //Buffer.BlockCopy(hash2, 0, result, 0, result.Length);

            //byte[] chechHash = Calculate.GetCheckSum(resultAdd);
        }
        public static string GetAddressOfcompressed(BigInteger[] publicKey)
        {
            return GetAddressOfcompressed(publicKey, true);
        }
        public static string GetAddressOfcompressed(BigInteger[] publicKey, bool show)
        {
            var publicKeyArray1 = HexToByteArray.BigIntegerTo32ByteArray(publicKey[0]);
            HexToByteArray.ChangeDirection(ref publicKeyArray1);
            // var publicKeyArray2 = HexToByteArray.BigIntegerTo32ByteArray(publicKey[1]);

            byte[] resultAdd;
            if (publicKey[1].IsEven)
                resultAdd = Calculate.BiteSplitJoint(new byte[] { 0x02 }, publicKeyArray1);
            else
                resultAdd = Calculate.BiteSplitJoint(new byte[] { 0x03 }, publicKeyArray1);

            if (show) { }
            //Consol.WriteLine($"压缩公钥为{ Hex.BytesToHex(resultAdd)}");
            //   //Consol.WriteLine($"压缩公钥为{ Calculate.Encode(resultAdd)}");

            var step3 = ripemd160.ComputeHash(sha256.ComputeHash(resultAdd));

            var step4 = Calculate.BiteSplitJoint(new byte[] { 0x00 }, step3);

            var step5 = sha256.ComputeHash(sha256.ComputeHash(step4));

            var step6 = Calculate.BiteSplitJoint(step4, new byte[] { step5[0], step5[1], step5[2], step5[3] });

            return Calculate.Encode(step6);
        }
    }
}
