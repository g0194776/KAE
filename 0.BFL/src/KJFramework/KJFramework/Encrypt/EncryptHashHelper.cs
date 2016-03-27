using System.Security.Cryptography;
using System.Text;

namespace KJFramework.Encrypt
{
    /// <summary>
    ///      提供了基础的HASH加密操作
    /// </summary>
    public sealed class EncryptHashHelper
    {
        private static MD5 md5 = new MD5CryptoServiceProvider();
        /// <summary>
        /// 使用utf8编码将字符串散列
        /// </summary>
        /// <param name="sourceString">要散列的字符串</param>
        /// <returns>散列后的字符串</returns>
        public static string HashString(string sourceString)
        {
            return HashString(Encoding.UTF8, sourceString).ToUpper();
        }
        /// <summary>
        /// 使用指定的编码将字符串散列
        /// </summary>
        /// <param name="encode">编码</param>
        /// <param name="sourceString">要散列的字符串</param>
        /// <returns>散列后的字符串</returns>
        public static string HashString(Encoding encode, string sourceString)
        {
            byte[] source = md5.ComputeHash(encode.GetBytes(sourceString));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < source.Length; i++)
            {
                sBuilder.Append(source[i].ToString("x2"));
            }
            return sBuilder.ToString().ToUpper();
        }

    }
}
