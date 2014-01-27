using System;
using KJFramework.Core.Native;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Enums;
using KJFramework.Messages.Helpers;
using KJFramework.Messages.Proxies;

namespace KJFramework.Messages.ValueStored.DataProcessor
{
    /// <summary>
    ///     UInt64数组关于元数据的处理
    /// </summary>
    public class UInt64ArrayDataProcessor : IDataProcessor
    {
        #region Members

        /// <summary>
        ///     获取当前处理的第三方数据类型
        /// </summary>
        public PropertyTypes TypeId { get { return PropertyTypes.UInt64Array; } }

        #endregion

        #region Methods

        /// <summary>
        ///     元数据转换成第三方数据
        /// </summary>
        /// <param name="metadataObject">元数据集合</param>
        /// <param name="id">属性对应key</param>
        /// <param name="data">属性对应byte数组</param>
        /// <param name="offsetStart">属性在数组中的偏移值</param>
        /// <param name="length">属性在byte数组中的长度</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public unsafe void DataProcessor(MetadataContainer metadataObject, byte id, byte[] data, int offsetStart, uint length)
        {
            if (metadataObject == null) throw new ArgumentNullException("metadataObject");
            if (length == 0)
            {
                metadataObject.SetAttribute(id, new UInt64ArrayValueStored(new ulong[0]));
                return;
            }
            ulong[] array = new ulong[length / Size.UInt64];
            if (array.Length <= 10)
            {
                fixed (byte* pByte = (&data[offsetStart]))
                {
                    ulong* pData = (ulong*)pByte;
                    for (int j = 0; j < array.Length; j++) array[j] = *pData++;
                }
            }
            else fixed (byte* pArr = &data[offsetStart])
            {
                fixed (ulong* point = &array[0])
                {
                    Native.Win32API.memcpy(new IntPtr((byte*)point), new IntPtr(pArr), length);
                }
            }
            metadataObject.SetAttribute(id, new UInt64ArrayValueStored(array));
        }

        /// <summary>
        ///     第三方数据转换成元数据
        /// </summary>
        /// <param name="proxy">内存段实例</param>
        /// <param name="baseValueMessage">存储属性的实例对象</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public unsafe void ValueProcessor(IMemorySegmentProxy proxy, BaseValueStored baseValueMessage)
        {
            if (proxy == null) throw new ArgumentNullException("proxy");
            if (baseValueMessage == null) throw new ArgumentNullException("baseValueMessage");
            fixed (ulong* pByte = baseValueMessage.GetValue<ulong[]>())
                proxy.WriteMemory(pByte, (uint)baseValueMessage.GetValue<ulong[]>().Length * Size.UInt64);
        }

        #endregion
    }
}
