using System;
using System.Net;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Enums;
using KJFramework.Messages.Helpers;
using KJFramework.Messages.Proxies;

namespace KJFramework.Messages.ValueStored.DataProcessor
{
    /// <summary>
    ///     IPEndPoint数组关于元数据的处理
    /// </summary>
    public class IPEndPointArrayDataProcessor : IDataProcessor
    {
        #region Members

        /// <summary>
        ///     获取当前处理的第三方数据类型
        /// </summary>
        public PropertyTypes TypeId
        {
            get { return PropertyTypes.IPEndPointArray; }
        }

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
                metadataObject.SetAttribute(id, new IPEndPointArrayValueStored(new IPEndPoint[0]));
                return;
            }
            IPEndPoint[] array = new IPEndPoint[length / Size.IPEndPoint];
            int innerOffset = 0;
            fixed (byte* pByte = &data[offsetStart])
            {
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = new IPEndPoint(*(long*) (pByte + innerOffset), *(int*) (pByte + innerOffset + 8));
                    innerOffset += (int) Size.IPEndPoint;
                }
            }
            metadataObject.SetAttribute(id, new IPEndPointArrayValueStored(array));
        }

        /// <summary>
        ///     第三方数据转换成元数据
        /// </summary>
        /// <param name="proxy">内存段实例</param>
        /// <param name="baseValueMessage">存储属性的实例对象</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public void ValueProcessor(IMemorySegmentProxy proxy, BaseValueStored baseValueMessage)
        {
            if (proxy == null) throw new ArgumentNullException("proxy");
            if (baseValueMessage == null) throw new ArgumentNullException("baseValueMessage");
            IPEndPoint[] array = baseValueMessage.GetValue<IPEndPoint[]>();
            if (array == null) throw new NullReferenceException(string.Format("#The processed value is null, type Id is: {0}", baseValueMessage.TypeId));
            if (array.Length == 0) return;
            for (int i = 0; i < array.Length; i++) proxy.WriteIPEndPoint(array[i]);
        }

        #endregion
    }
}
