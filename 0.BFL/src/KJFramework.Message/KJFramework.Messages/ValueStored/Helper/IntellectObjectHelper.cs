using System;
using System.Collections.Generic;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Engine;
using KJFramework.Messages.Exceptions;
using KJFramework.Messages.Proxies;

namespace KJFramework.Messages.ValueStored.Helper
{
    /// <summary>
    ///         处理智能对象帮助类
    /// </summary>
    public static class IntellectObjectHelper
    {
        #region Methods

        /// <summary>
        ///   将一个IntellectObject转换成byte数组的形式
        /// </summary>
        /// <param name="value">值</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public static byte[] BindIntellectObject(IntellectObject value)
        {
            if (value == null) throw new ArgumentNullException("value");
            value.Bind();
            return value.Body;
        }

        /// <summary>
        ///   将一个IntellectObject数组转换成byte数组的形式
        /// </summary>
        /// <param name="value">值</param>
        /// <exception cref="UnexpectedValueException">结果错误</exception>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public static byte[] BindIntellectObjectArray(IntellectObject[] value)
        {
            if(value == null) throw new ArgumentNullException("value");
            MemoryAllotter.Instance.Initialize();
            IMemorySegmentProxy proxy = MemorySegmentProxyFactory.Create();
            proxy.WriteUInt32((uint) value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                IntellectObject elementObj = value[i];
                if (elementObj == null) proxy.WriteUInt16(0);
                else
                {
                    MemoryPosition currentPostion = proxy.GetPosition();
                    proxy.Skip(2U);
                    int length = IntellectObjectEngine.ToBytes(elementObj, proxy);
                    proxy.WriteBackUInt16(currentPostion, (ushort)length);
                }
            }
            return proxy.GetBytes();
        }

        /// <summary>
        ///   将一个IntellectObject数组转换成byte数组的形式
        /// </summary>
        /// <param name="value">值</param>
        /// <exception cref="UnexpectedValueException">结果错误</exception>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        private static byte[] BindIntellectObjectArrayMethod(IntellectObject[] value)
        {
            if (value == null) throw new ArgumentNullException("value");
            List<byte[]> byteList = new List<byte[]>();
            int totalLength = value.Length * 2 + 4;
            byte[] placeHolderByte = { 0x00, 0x00 };
            for (int i = 0; i < value.Length; i++)
            {
                IntellectObject elementObj = value[i];
                if (elementObj == null)
                {
                    byteList.Add(placeHolderByte);
                    continue;
                }
                elementObj.Bind();
                if (!elementObj.IsBind) throw new UnexpectedValueException("elementObj");
                byteList.Add(elementObj.Body);
                totalLength += elementObj.Body.Length;
            }
            byte[] bytes = new byte[totalLength];
            int dstOffset = 0;
            Buffer.BlockCopy(BitConverter.GetBytes(value.Length), 0, bytes, dstOffset, 4);
            dstOffset += 4;
            for (int i = 0; i < byteList.Count; i++)
            {
                if (byteList[i] == placeHolderByte)
                {
                    Buffer.BlockCopy(byteList[i], 0, bytes, dstOffset, 2);
                    dstOffset += 2;
                }
                else
                {
                    Buffer.BlockCopy(BitConverter.GetBytes(byteList[i].Length), 0, bytes, dstOffset, 2);
                    dstOffset += 2;
                    Buffer.BlockCopy(byteList[i], 0, bytes, dstOffset, byteList[i].Length);
                    dstOffset += byteList[i].Length;
                }
            }
            if (bytes.Length != totalLength) throw new UnexpectedValueException("value");
            return bytes;
        }

        /// <summary>
        ///     返回一个IntellectObject
        /// </summary>
        /// <param name="intellectObjectValueStored">智能对象存储类型</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public static T GetAttributeAsIntellectObject<T>(IntellectObjectValueStored intellectObjectValueStored)
            where T : IntellectObject
        {
            if (intellectObjectValueStored == null) throw new ArgumentNullException("intellectObjectValueStored");
            byte[] intellectObject = intellectObjectValueStored.GetValue<byte[]>();
            return IntellectObjectEngine.GetObject<T>(intellectObject);
        }

        /// <summary>
        ///   返回一个IntellectObject数组
        /// </summary>
        /// <param name="intellectObjectArrayValueStored">智能对象数组存储类型</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public static T[] GetAttributeAsIntellectObjectArray<T>(IntellectObjectArrayValueStored intellectObjectArrayValueStored)
            where T : IntellectObject
        {
            if (intellectObjectArrayValueStored == null) throw new ArgumentNullException("intellectObjectArrayValueStored");
            byte[] intellectObject = intellectObjectArrayValueStored.GetValue<byte[]>();
            int arrLen = BitConverter.ToInt32(intellectObject, 0);
            if (arrLen == 0) return null;
            T[] array = new T[arrLen];
            int innerOffset = 4;
            ushort size;
            for (int i = 0; i < array.Length; i++)
            {
                size = BitConverter.ToUInt16(intellectObject, innerOffset);
                innerOffset += 2;
                if (size == 0) array[i] = null;
                else array[i] = IntellectObjectEngine.GetObject<T>(typeof(T), intellectObject, innerOffset, size);
                innerOffset += size;
            }
            return array;
        }

        #endregion
    }
}
