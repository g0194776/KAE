using KJFramework.Messages.Contracts;
using KJFramework.Messages.Enums;
using KJFramework.Messages.Proxies;

namespace KJFramework.Messages.ValueStored.DataProcessor
{
    /// <summary>
    ///     用于元数据转换成第三方数据的数据处理接口
    /// </summary>
    public interface IDataProcessor
    {
        #region Methods.

        /// <summary>
        ///     获取当前处理的第三方数据类型
        /// </summary>
        PropertyTypes TypeId { get; }
        /// <summary>
        ///     元数据转换成第三方数据
        /// </summary>
        /// <param name="metadataObject">元数据集合</param>
        /// <param name="id">属性对应key</param>
        /// <param name="data">属性对应byte数组</param>
        /// <param name="offsetStart">属性在数组中的偏移值</param>
        /// <param name="length">属性在byte数组中的长度</param>
        void DataProcessor(MetadataContainer metadataObject, byte id, byte[] data, int offsetStart, uint length);
        /// <summary>
        ///     第三方数据转换成元数据
        /// </summary>
        /// <param name="proxy">内存段实例</param>
        /// <param name="baseValueMessage">存储属性的实例对象</param>
        void ValueProcessor(IMemorySegmentProxy proxy, BaseValueStored baseValueMessage);

        #endregion
    }
}
