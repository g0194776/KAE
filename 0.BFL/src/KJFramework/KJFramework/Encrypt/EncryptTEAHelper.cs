using System;
using System.Text;

namespace KJFramework.Encrypt
{
    /// <summary>
    ///     提供了关于TEA加密算法的相关操作
    /// </summary>
    public sealed class EncryptTEAHelper
    {
        private static Byte[] _key = Encoding.UTF8.GetBytes("123456789123456789");
        /// <summary>
        ///     密钥
        /// </summary>
        /// <remarks>
        ///     如果当前不指定, 则使用默认密钥
        /// </remarks>
        public static Byte[] key
        {
            get { return _key; }
            set { _key = value; }
        }

        /// <summary>
        ///     按照TEA加密算法，加密字节数组
        /// </summary>
        /// <param name="data" type="byte[]">
        ///     <para>
        ///         要加密的字节数组
        ///     </para>
        /// </param>
        /// <returns>
        ///     返回加密后的字节数组
        /// </returns>
        public static Byte[] Encrypt(Byte[] data)
        {
            if (data.Length == 0)
            {
                return data;
            }
            return ToByteArray(Encrypt(ToUInt32Array(data, true), ToUInt32Array(key, false)), false);
        }

        /// <summary>
        ///     按照TEA加密算法，解密字节数组
        /// </summary>
        /// <param name="data" type="byte[]">
        ///     <para>
        ///         要解密的字节数组
        ///     </para>
        /// </param>
        /// <returns>
        ///     返回解密后的字节数组
        /// </returns>
        public static Byte[] Decrypt(Byte[] data)
        {
            if (data.Length == 0)
            {
                return data;
            }
            return ToByteArray(Decrypt(ToUInt32Array(data, false), ToUInt32Array(key, false)), true);
        }

        /// <summary>
        ///     加密指定的字符串
        /// </summary>
        /// <param name="data">要加密的字符串</param>
        /// <returns>返回加密后的结果</returns>
        public static string EncryptToString(string data)
        {
            if (String.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException();
            }
            return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(data)));
        }

        /// <summary>
        ///     解密指定字符串
        /// </summary>
        /// <param name="data">要解密的字符串</param>
        /// <returns>返回解密后的结果</returns>
        public static string DecryptFromString(string data)
        {
            if (String.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException();
            }
            return Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(data)));
        }

        /// <summary>
        ///     将2个32位无符号整数集合混合加密
        /// </summary>
        /// <param name="v">第一个集合</param>
        /// <param name="k">第二个集合</param>
        /// <returns>返回加密后的结果</returns>
        public static UInt32[] Encrypt(UInt32[] v, UInt32[] k)
        {
            Int32 n = v.Length - 1;
            if (n < 1)
            {
                return v;
            }
            if (k.Length < 4)
            {
                UInt32[] Key = new UInt32[4];
                k.CopyTo(Key, 0);
                k = Key;
            }
            UInt32 z = v[n], y = v[0];
            const uint delta = 0x9E3779B9;
            UInt32 sum = 0, e;
            Int32 p, q = 6 + 52 / (n + 1);
            while (q-- > 0)
            {
                sum = unchecked(sum + delta);
                e = sum >> 2 & 3;
                for (p = 0; p < n; p++)
                {
                    y = v[p + 1];
                    z = unchecked(v[p] += (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z));
                }
                y = v[0];
                z = unchecked(v[n] += (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z));
            }
            return v;
        }


        /// <summary>
        ///     将2个32位无符号整数集合混合解密
        /// </summary>
        /// <param name="v">第一个集合</param>
        /// <param name="k">第二个集合</param>
        /// <returns>返回解密后的结果</returns>
        public static UInt32[] Decrypt(UInt32[] v, UInt32[] k)
        {
            Int32 n = v.Length - 1;
            if (n < 1)
            {
                return v;
            }
            if (k.Length < 4)
            {
                UInt32[] Key = new UInt32[4];
                k.CopyTo(Key, 0);
                k = Key;
            }
            UInt32 z = v[n], y = v[0], delta = 0x9E3779B9, sum, e;
            Int32 p, q = 6 + 52 / (n + 1);
            sum = unchecked((UInt32)(q * delta));
            while (sum != 0)
            {
                e = sum >> 2 & 3;
                for (p = n; p > 0; p--)
                {
                    z = v[p - 1];
                    y = unchecked(v[p] -= (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z));
                }
                z = v[n];
                y = unchecked(v[0] -= (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z));
                sum = unchecked(sum - delta);
            }
            return v;
        }

        private static UInt32[] ToUInt32Array(Byte[] Data, Boolean IncludeLength)
        {
            Int32 n = (((Data.Length & 3) == 0) ? (Data.Length >> 2) : ((Data.Length >> 2) + 1));
            UInt32[] Result;
            if (IncludeLength)
            {
                Result = new UInt32[n + 1];
                Result[n] = (UInt32)Data.Length;
            }
            else
            {
                Result = new UInt32[n];
            }
            n = Data.Length;
            for (Int32 i = 0; i < n; i++)
            {
                Result[i >> 2]|=  (UInt32)Data[i] << ((i & 3) << 3);
            }
            return Result;
        }

        private static Byte[] ToByteArray(UInt32[] Data, Boolean IncludeLength)
        {
            Int32 n;
            if (IncludeLength)
            {
                n = (Int32)Data[Data.Length - 1];
            }
            else
            {
                n = Data.Length << 2;
            }
            Byte[] Result = new Byte[n];
            for (Int32 i = 0; i < n; i++)
            {
                Result[i] = (Byte)(Data[i >> 2] >> ((i & 3) << 3));
            }
            return Result;
        }
    }
}
