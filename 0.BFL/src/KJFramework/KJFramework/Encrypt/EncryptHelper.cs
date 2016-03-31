using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace KJFramework.Encrypt
{
    /// <summary>
    ///     加密帮助器，提供了很多零散算法的加密服务
    /// </summary>
    public class EncryptHelper
    {
        #region RC2

        /// <summary> 
        /// 进行RC2加密。 
        /// </summary> 
        /// <param name="pToEncrypt">要加密的字符串。</param> 
        /// <param name="key">初始化向量</param> 
        /// <param name="iv">密钥，且必须为8位。</param> 
        /// <returns>以Base64格式返回的加密字符串。</returns> 
        public static string Encrypt(string pToEncrypt, byte[] key, byte[] iv)
        {
            //创建UTF-16 编码，用来在byte[]和string之间转换 
            var textConverter = new UnicodeEncoding();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(pToEncrypt);
            using (var rc2Csp = new RC2CryptoServiceProvider())
            {
                ICryptoTransform encryptor = rc2Csp.CreateEncryptor(key, iv);

                var ms = new MemoryStream();
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Convert.ToBase64String(ms.ToArray());
                ms.Close();
                return str;
            }
        }

        /// <summary> 
        /// 进行RC2解密。 
        /// </summary> 
        /// <param name="pToDecrypt">要解密的以Base64</param> 
        /// <param name="key">初始化向量</param> 
        /// <param name="iv">密钥，且必须为8位。</param> 
        /// <returns>已解密的字符串。</returns> 
        public static string Decrypt(string pToDecrypt, byte[] key, byte[] iv)
        {
            byte[] inputByteArray = Convert.FromBase64String(pToDecrypt);
            using (var rc2Csp = new RC2CryptoServiceProvider())
            {
                ICryptoTransform encryptor = rc2Csp.CreateDecryptor(key, iv);
                var ms = new MemoryStream();
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
                return str;
            }
        }

        #endregion

        #region RSA

        /// <summary> 
        /// RSA加密 
        /// </summary> 
        /// <param name="dataToEncrypt"></param> 
        /// <param name="doOaepPadding"></param> 
        /// <returns></returns> 
        public static byte[] RsaEncrypt(byte[] dataToEncrypt, bool doOaepPadding)
        {
            try
            {
                var rsa = new RSACryptoServiceProvider();
                var reader = File.OpenText(@"d:\PublicKey.xml");
                string pKey = reader.ReadToEnd();
                rsa.FromXmlString(pKey);
                reader.Close();

                return rsa.Encrypt(dataToEncrypt, doOaepPadding);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        /// <summary> 
        /// RSA解密 
        /// </summary> 
        /// <param name="dataToDecrypt"></param> 
        /// <param name="doOaepPadding"></param> 
        /// <returns></returns> 
        public static byte[] RsaDecrypt(byte[] dataToDecrypt, bool doOaepPadding)
        {
            try
            {
                var rsa = new RSACryptoServiceProvider();
                var reader = File.OpenText(@"d:\PublicAndPrivateKey.xml");
                string ppKey = reader.ReadToEnd();
                rsa.FromXmlString(ppKey);
                reader.Close();

                return rsa.Decrypt(dataToDecrypt, doOaepPadding);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());

                return null;
            }
        }

        #endregion

        #region DES

        /// <summary> 
        /// 进行DES加密。 
        /// </summary> 
        /// <param name="pToEncrypt">要加密的字符串。</param> 
        /// <param name="sKey">密钥，且必须为8位。</param> 
        /// <returns>以Base64格式返回的加密字符串。</returns> 
        public static string Encrypt(string pToEncrypt, string sKey)
        {
            using (var des = new DESCryptoServiceProvider())
            {
                byte[] inputByteArray = Encoding.UTF8.GetBytes(pToEncrypt);
                des.Key = Encoding.ASCII.GetBytes(sKey);
                des.IV = Encoding.ASCII.GetBytes(sKey);
                var ms = new MemoryStream();
                using (var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Convert.ToBase64String(ms.ToArray());
                ms.Close();
                return str;
            }
        }

        /// <summary> 
        /// 进行DES解密。 
        /// </summary> 
        /// <param name="pToDecrypt">要解密的以Base64</param> 
        /// <param name="sKey">密钥，且必须为8位。</param> 
        /// <returns>已解密的字符串。</returns> 
        public static string Decrypt(string pToDecrypt, string sKey)
        {
            byte[] inputByteArray = Convert.FromBase64String(pToDecrypt);
            using (var des = new DESCryptoServiceProvider())
            {
                des.Key = Encoding.ASCII.GetBytes(sKey);
                des.IV = Encoding.ASCII.GetBytes(sKey);
                var ms = new MemoryStream();
                using (var cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
                return str;
            }
        }

        #endregion

        #region MD5

        private string EncodePassword(string originalPassword)
        {
            //Declarations 
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;

            //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password) 
            md5 = new MD5CryptoServiceProvider();
            originalBytes = Encoding.Default.GetBytes(originalPassword);
            encodedBytes = md5.ComputeHash(originalBytes);

            //Convert encoded bytes back to a 'readable' string 
            return BitConverter.ToString(encodedBytes);
        }

        #endregion

        #region RC4

        /// <summary> 
        /// 加密或解密（对称） 
        /// </summary> 
        /// <param name="data">明文或密文</param> 
        /// <param name="pass">密钥</param> 
        /// <returns>密文或明文</returns> 
        public Byte[] EncryptEx(Byte[] data, String pass)
        {
            if (data == null || pass == null) return null;
            var output = new Byte[data.Length];
            Int64 i = 0;
            Int64 j = 0;
            Byte[] mBox = GetKey(Encoding.UTF8.GetBytes(pass), 256);

            // 加密 
            for (Int64 offset = 0; offset < data.Length; offset++)
            {
                i = (i + 1)%mBox.Length;
                j = (j + mBox[i])%mBox.Length;
                Byte temp = mBox[i];
                mBox[i] = mBox[j];
                mBox[j] = temp;
                Byte a = data[offset];
                //Byte b = mBox[(mBox[i] + mBox[j] % mBox.Length) % mBox.Length]; 
                // mBox[j] 一定比 mBox.Length 小，不需要在取模 
                Byte b = mBox[(mBox[i] + mBox[j])%mBox.Length];
                output[offset] = (Byte) (a ^ b);
            }

            return output;
        }

        /// <summary> 
        /// 解密 
        /// </summary> 
        /// <param name="data"></param> 
        /// <param name="pass"></param> 
        /// <returns></returns> 
        public Byte[] DecryptEx(Byte[] data, String pass)
        {
            return EncryptEx(data, pass);
        }

        /// <summary> 
        /// 打乱密码 
        /// </summary> 
        /// <param name="pass">密码</param> 
        /// <param name="kLen">密码箱长度</param> 
        /// <returns>打乱后的密码</returns> 
        private static Byte[] GetKey(Byte[] pass, Int32 kLen)
        {
            var mBox = new Byte[kLen];

            for (Int64 i = 0; i < kLen; i++)
            {
                mBox[i] = (Byte) i;
            }
            Int64 j = 0;
            for (Int64 i = 0; i < kLen; i++)
            {
                j = (j + mBox[i] + pass[i%pass.Length])%kLen;
                Byte temp = mBox[i];
                mBox[i] = mBox[j];
                mBox[j] = temp;
            }
            return mBox;
        }

        #endregion

        #region AES

        /// <summary> 
        /// 获取密钥 
        /// </summary> 
        private static string Key
        {
            get { return @")O[NB]6,YF}+efcaj{+oESb9d8>Z'e9M"; }
        }

        /// <summary> 
        /// 获取向量 
        /// </summary> 
        private static string Iv
        {
            get { return @"L+\~f4,Ir)b$=pkf"; }
        }

        /// <summary> 
        /// AES加密 
        /// </summary> 
        /// <param name="plainStr">明文字符串</param> 
        /// <param name="returnNull">加密失败时是否返回 null，false 返回 String.Empty</param> 
        /// <returns>密文</returns> 
        public string AesEncrypt(string plainStr, bool returnNull)
        {
            string encrypt = AesEncrypt(plainStr);
            return returnNull ? encrypt : (encrypt == null ? String.Empty : encrypt);
        }

        /// <summary> 
        /// AES加密 
        /// </summary> 
        /// <param name="plainStr">明文字符串</param> 
        /// <returns>密文</returns> 
        public string AesEncrypt(string plainStr)
        {
            byte[] bKey = Encoding.UTF8.GetBytes(Key);
            byte[] bIv = Encoding.UTF8.GetBytes(Iv);
            byte[] byteArray = Encoding.UTF8.GetBytes(plainStr);

            string encrypt = null;
            Rijndael aes = Rijndael.Create();
            try
            {
                using (var mStream = new MemoryStream())
                {
                    using (
                        var cStream = new CryptoStream(mStream, aes.CreateEncryptor(bKey, bIv), CryptoStreamMode.Write))
                    {
                        cStream.Write(byteArray, 0, byteArray.Length);
                        cStream.FlushFinalBlock();
                        encrypt = Convert.ToBase64String(mStream.ToArray());
                    }
                }
            }
            catch
            {
            }
            aes.Clear();

            return encrypt;
        }

        /// <summary> 
        /// AES解密 
        /// </summary> 
        /// <param name="encryptStr">密文字符串</param> 
        /// <param name="returnNull">解密失败时是否返回 null，false 返回 String.Empty</param> 
        /// <returns>明文</returns> 
        public string AesDecrypt(string encryptStr, bool returnNull)
        {
            string decrypt = AesDecrypt(encryptStr);
            return returnNull ? decrypt : (decrypt == null ? String.Empty : decrypt);
        }

        /// <summary> 
        /// AES解密 
        /// </summary> 
        /// <param name="encryptStr">密文字符串</param> 
        /// <returns>明文</returns> 
        public string AesDecrypt(string encryptStr)
        {
            byte[] bKey = Encoding.UTF8.GetBytes(Key);
            byte[] bIv = Encoding.UTF8.GetBytes(Iv);
            byte[] byteArray = Convert.FromBase64String(encryptStr);

            string decrypt = null;
            Rijndael aes = Rijndael.Create();
            try
            {
                using (var mStream = new MemoryStream())
                {
                    using (
                        var cStream = new CryptoStream(mStream, aes.CreateDecryptor(bKey, bIv), CryptoStreamMode.Write))
                    {
                        cStream.Write(byteArray, 0, byteArray.Length);
                        cStream.FlushFinalBlock();
                        decrypt = Encoding.UTF8.GetString(mStream.ToArray());
                    }
                }
            }
            catch
            {
            }
            aes.Clear();

            return decrypt;
        }

        #endregion
    }
}