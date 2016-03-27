using System;
using System.Text;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Enums;
using KJFramework.Messages.Proxies;

namespace KJFramework.Messages.ValueStored.DataProcessor
{
    /// <summary>
    ///     String数组关于元数据的处理
    /// </summary>
    public class StringArrayDataProcessor : IDataProcessor
    {
        #region Members

        /// <summary>
        ///     获取当前处理的第三方数据类型
        /// </summary>
        public PropertyTypes TypeId
        {
            get { return PropertyTypes.StringArray; }
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
            int stringArrayFlagOffset = offsetStart + 4;
            ushort len;
            string[] array = new string[BitConverter.ToInt32(data, offsetStart)];
            for (int j = 0; j < array.Length; j++)
            {
                len = BitConverter.ToUInt16(data, stringArrayFlagOffset);
                stringArrayFlagOffset += 2;
                if (len == 0)
                {
                    array[j] = null;
                    continue;
                }
                array[j] = Encoding.UTF8.GetString(data, stringArrayFlagOffset, len);
                stringArrayFlagOffset += len;
            }
            metadataObject.SetAttribute(id, new StringArrayValueStored(array));
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
            string[] value = baseValueMessage.GetValue<string[]>();
            proxy.WriteInt32(value.Length);
            if (value.Length == 0) return;
            for (int j = 0; j < value.Length; j++)
            {
                string elementValue = value[j];
                if (string.IsNullOrEmpty(elementValue)) proxy.WriteUInt16(0);
                else
                {
                    byte[] elementData = Encoding.UTF8.GetBytes(elementValue);
                    proxy.WriteUInt16((ushort)elementData.Length);
                    proxy.WriteMemory(elementData, 0U, (uint)elementData.Length);
                }
            }
        }

        #endregion
    }
}
