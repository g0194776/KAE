using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace KJFramework.Encrypt
{
    /// <summary>
    ///     Rijndael对称加密算法类
    /// </summary>
    public class EncrptyRijndaelHelper
    {
        private static SymmetricAlgorithm mobjCryptoService;
        private static string Key;

        /// <summary>
        /// 对称加密类的构造函数
        /// </summary>
        static EncrptyRijndaelHelper()
        {
            mobjCryptoService = new RijndaelManaged();
            Key = "Guz(%&amp;hj7x89H$yuBI0456FtmaT5&amp;fvHUFCy76*h%(HilJ$lhj!y6&amp;(*jkP87jH7";
        }

        /// <summary>
        /// 获得密钥
        /// </summary>
        /// <returns>密钥</returns>
        private static byte[] GetLegalKey()
        {
            string sTemp = Key;
            mobjCryptoService.GenerateKey();
            byte[] bytTemp = mobjCryptoService.Key;
            int keyLength = bytTemp.Length;
            if (sTemp.Length > keyLength)
                sTemp = sTemp.Substring(0, keyLength);
            else if (sTemp.Length < keyLength)
                sTemp = sTemp.PadRight(keyLength, ' ');
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }

        /// <summary>
        /// 获得初始向量IV
        /// </summary>
        /// <returns>初试向量IV</returns>
        private static byte[] GetLegalIv()
        {
            string sTemp = "E4ghj*Ghg7!rNIfb&amp;95GUY86GfghUb#er57HBh(u%g6HJ($jhWk7&amp;!hg4ui%$hjk";
            mobjCryptoService.GenerateIV();
            byte[] bytTemp = mobjCryptoService.IV;
            int ivLength = bytTemp.Length;
            if (sTemp.Length > ivLength)
                sTemp = sTemp.Substring(0, ivLength);
            else if (sTemp.Length < ivLength)
                sTemp = sTemp.PadRight(ivLength, ' ');
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }

        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="source">待加密的串</param>
        /// <returns>经过加密的串</returns>
        public static string Encrypto(string source)
        {
            byte[] bytIn = UTF8Encoding.UTF8.GetBytes(source);
            using (MemoryStream ms = new MemoryStream())
            {
                mobjCryptoService.Key = GetLegalKey();
                mobjCryptoService.IV = GetLegalIv();
                ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
                cs.Write(bytIn, 0, bytIn.Length);
                cs.FlushFinalBlock();
                ms.Close();
                byte[] bytOut = ms.ToArray();
                return Convert.ToBase64String(bytOut);
            }
        }

        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="source">待解密的串</param>
        /// <returns>经过解密的串</returns>
        public static string Decrypto(string source)
        {
            byte[] bytIn = Convert.FromBase64String(source);
            using (MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length))
            {
                mobjCryptoService.Key = GetLegalKey();
                mobjCryptoService.IV = GetLegalIv();
                ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cs);
                return sr.ReadToEnd();
            }
        }
    }
}
