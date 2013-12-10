using System;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Engine;
using KJFramework.Messages.Enums;
using KJFramework.Messages.Proxies;
using KJFramework.Messages.Types;

namespace KJFramework.Messages.ValueStored.DataProcessor
{
    /// <summary>
    ///     ResourceBlock数组关于元数据的处理
    /// </summary>
    public class ResourceBlockArrayDataProcessor : IDataProcessor
    {
        #region Members

        /// <summary>
        ///     获取当前处理的第三方数据类型
        /// </summary>
        public PropertyTypes TypeId { get { return PropertyTypes.ResourceBlockArray; } }

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
        public void DataProcessor(MetadataContainer metadataObject, byte id, byte[] data, int offsetStart, uint length)
        {
            if (metadataObject == null) throw new ArgumentNullException("metadataObject");
            uint arrayLength = BitConverter.ToUInt32(data, offsetStart);
            ResourceBlock[] resourceBlocks = new ResourceBlock[arrayLength];
            offsetStart += 4;
            for (int j = 0; j < arrayLength; j++)
            {
                uint resouceBlockLength = BitConverter.ToUInt32(data, offsetStart);
                if (resouceBlockLength == 0)
                {
                    resourceBlocks[j] = null;
                    offsetStart += 4;
                }
                else
                {
                    resourceBlocks[j] = MetadataObjectEngine.GetObject(data, (uint)offsetStart, resouceBlockLength);
                    offsetStart += (int)resouceBlockLength;
                }
            }
            metadataObject.SetAttribute(id, new ResourceBlockArrayStored(resourceBlocks));
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
            ResourceBlock[] value = baseValueMessage.GetValue<ResourceBlock[]>();
            proxy.WriteUInt32((uint)value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] == null)
                {
                    proxy.WriteUInt32(0);
                    continue;
                }
                MetadataObjectEngine.ToBytes(value[i], proxy);
            }
        }

        #endregion
    }
}
