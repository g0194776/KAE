using System;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Enums;
using KJFramework.Messages.Proxies;
using KJFramework.Messages.ValueStored.DataProcessor;
using KJFramework.Messages.ValueStored.StoredHelper;

namespace KJFramework.Messages.ValueStored
{
    /// <summary>
    ///     Float类型的存储
    /// </summary>
    public class FloatValueStored : BaseValueStored
    {
        #region Members

        private readonly float _value;
        private static readonly PropertyValueStored<float> _instance;
        private static readonly Action<IMemorySegmentProxy, BaseValueStored> _toBytesDelegate;
        private static readonly Action<MetadataContainer, byte, byte[], int, uint> _toDataDelegate;

        #endregion

        #region Method

        /// <summary>
        ///     Float类型存储的初始化构造器
        /// </summary>
        public FloatValueStored()
        {
            _typeId = (byte)PropertyTypes.Float;
        }

        /// <summary>
        ///     Float类型存储的初始化构造器
        /// </summary>
        public FloatValueStored(float value)
        {
            _value = value;
            _typeId = (byte) PropertyTypes.Float;
        }

        /// <summary>
        ///     Float类型存储的静态构造器
        /// </summary>
        static FloatValueStored()
        {
            _instance = ValueStoredHelper.BuildMethod<float>();
            _toBytesDelegate = ProcessorDictionary.ValueActions[(byte)PropertyTypes.Float];
            _toDataDelegate = ProcessorDictionary.DataActions[(byte)PropertyTypes.Float];
        }

        /// <summary>
        ///     获取存储的对应类型的Value值
        /// </summary>
        public override T GetValue<T>()
        {
            return _instance.Get<T>(_value);
        }

        /// <summary>
        ///   内部指定类型序列化方法
        /// </summary>
        /// <param name="proxy">内存代理器</param>
        public override void ToBytes(IMemorySegmentProxy proxy)
        {
            _toBytesDelegate(proxy, this);
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
            return new FloatValueStored();
        }

        #endregion
    }
}
