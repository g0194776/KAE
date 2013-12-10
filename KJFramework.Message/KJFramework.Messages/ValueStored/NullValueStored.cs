using System;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Enums;
using KJFramework.Messages.Proxies;
using KJFramework.Messages.Types;
using KJFramework.Messages.ValueStored.DataProcessor;

namespace KJFramework.Messages.ValueStored
{
    /// <summary>
    ///     空类型的存储
    /// </summary>
    public class NullValueStored : BaseValueStored
    {
        #region Members

        private static readonly Action<ResourceBlock, byte, byte[], int, uint> _toDataDelegate;

        #endregion

        #region Method

        /// <summary>
        ///     空类型存储的初始化构造器
        /// </summary>
        public NullValueStored()
        {
            IsNull = true;
            _typeId = (byte) PropertyTypes.Null;
        }

        static NullValueStored()
        {
            _toDataDelegate = ProcessorDictionary.DataActions[(byte)PropertyTypes.Null];
        }

        #endregion

        #region Methods

        /// <summary>
        ///     获取存储的对应类型的Value值
        /// </summary>
        public override T GetValue<T>()
        {
            return default(T);
        }

        /// <summary>
        ///   内部指定类型序列化方法
        /// </summary>
        /// <param name="proxy">内存代理器</param>
        public override void ToBytes(IMemorySegmentProxy proxy)
        {
            
        }

        /// <summary>
        ///   内部指定类型反序列化方法
        /// </summary>
        /// <param name="metadataObject">元数据集合</param>
        /// <param name="id">属性对应key</param>
        /// <param name="data">属性对应byte数组</param>
        /// <param name="offsetStart">属性在数组中的偏移值</param>
        /// <param name="length">属性在byte数组中的长度</param>
        public override void ToData(MetadataContainer metadataObject, byte id, byte[] data, int offsetStart, uint length)
        {
            _toDataDelegate(metadataObject, id, data, offsetStart, length);
        }

        /// <summary>
        ///   返回一个实例对象
        /// </summary>
        public override object Clone()
        {
            return new NullValueStored();
        }

        #endregion
    }
}
