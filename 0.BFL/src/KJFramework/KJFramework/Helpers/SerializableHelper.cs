using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using KJFramework.Encrypt;
using KJFramework.Enums;

namespace KJFramework.Helpers
{
    /// <summary>
    ///     序列化帮助器, 支持了序列化 / 反序列化的相关操作。
    /// </summary>
    public class SerializableHelper
    {
        /// <summary>
        ///     序列化指定类
        /// </summary>
        /// <param name="fileName" type="string">
        ///     <para>
        ///         生成的文件名
        ///     </para>
        /// </param>
        /// <param name="clsSerializable" type="object">
        ///     <para>
        ///         要序列化的类
        ///     </para>
        /// </param>
        /// <param name="serializableType">序列化类型</param>
        /// <returns>
        ///     返回序列化结果
        /// </returns>
        public static bool Serializable(String fileName, Object clsSerializable, SerializableTypes serializableType)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            if (serializableType == SerializableTypes.Encrypt)
            {
                return Encrypt(fileName, clsSerializable);
            }
            try
            {
                BinaryFormatter tempSerializable = new BinaryFormatter();
                using (FileStream tempStream = new FileStream(fileName, FileMode.Create))
                {
                    tempSerializable.Serialize(tempStream, clsSerializable);
                }
                return true;
            }
            catch (SerializationException ex)
            {
                Debug.WriteLine("序列化对象出现错误：" + ex.Message);
                return false;
            }
            catch(System.Exception ex)
            {
                Debug.WriteLine("序列化对象出现错误：" + ex.Message);
                return false;
            }
        }

        /// <summary>
        ///     反序列化指定文件
        /// </summary>
        /// <param name="filePath" type="string">
        ///     <para>
        ///         要反序列化的指定文件路径
        ///     </para>
        /// </param>
        /// <param name="serializableType">序列化类型</param>
        /// <returns>
        ///     返回反序列化该文件后的基础类型
        /// </returns>
        public static T DeSerializable<T>(String filePath, SerializableTypes serializableType)
        {
            if (File.Exists(filePath))
            {
                T temp = default(T);
                if (serializableType == SerializableTypes.DeEncryptd)
                {
                    BinaryFormatter tempDeSerializable = new BinaryFormatter();
                    using (FileStream tempStream = new FileStream(filePath, FileMode.Open))
                    {
                        byte[] data = new byte[tempStream.Length];
                        tempStream.Read(data, 0, data.Length);
                        byte[] deCryptData = EncryptTEAHelper.Decrypt(data);
                        using(MemoryStream stream = new MemoryStream())
                        {
                            stream.Write(deCryptData, 0, deCryptData.Length);
                            stream.Position = 0;
                            return (T)tempDeSerializable.Deserialize(stream);
                        }
                    }
                }
                try
                {
                    BinaryFormatter tempDeSerializable = new BinaryFormatter();
                    using (FileStream tempStream = new FileStream(filePath, FileMode.Open))
                    {
                        temp = (T)tempDeSerializable.Deserialize(tempStream);
                    }

                    return temp;
                }
                catch (System.Exception ex)
                {
                    Debug.WriteLine("反序列化对象出现错误：" + ex.Message);
                    return temp;
                }
            }
            throw new FileNotFoundException("指定的文件 : " + filePath + " 不存在...!");
        }

        /// <summary>
        ///     将指定类型通过加密后写入文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="clsSerializable">要加密的序列化类型</param>
        /// <returns></returns>
        protected static bool Encrypt(String fileName, Object clsSerializable)
        {
            BinaryFormatter tempSerializable = new BinaryFormatter();
            //序列化到内存流
            try
            {
                using (MemoryStream tempStream = new MemoryStream())
                {
                    tempSerializable.Serialize(tempStream, clsSerializable);
                    byte[] data = tempStream.GetBuffer();
                    byte[] encryptData = EncryptTEAHelper.Encrypt(data);
                    using (FileStream fs = new FileStream(fileName, FileMode.Create))
                    {
                        fs.Write(encryptData, 0, encryptData.Length);
                        fs.Flush();
                        return true;
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.WriteLine("加密序列化类型时出错：" + e.Message);
                return false;
            }
        }
    }
}
