using System;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Enums;
using KJFramework.Messages.Proxies;

namespace KJFramework.Messages.ValueStored.DataProcessor
{
    /// <summary>
    ///     IntellectObject关于元数据的处理
    /// </summary>
    public class IntellectObjectDataProcessor : IDataProcessor
    {
        #region Members

        /// <summary>
        ///     获取当前处理的第三方数据类型
        /// </summary>
        public PropertyTypes TypeId
        {
            get { return PropertyTypes.IntellectObject; }
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
        public void DataProcessor(MetadataContainer metadataObject, byte id, byte[] data, int offsetStart, uint length)
        {
            byte[] newData = new byte[length];
            Buffer.BlockCopy(data, offsetStart, newData, 0, newData.Length);
            metadataObject.SetAttribute(id, new IntellectObjectValueStored(newData));
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
            proxy.WriteMemory(baseValueMessage.GetValue<byte[]>(), 0, (uint)baseValueMessage.GetValue<byte[]>().Length);
        }

        #endregion
    }
}
