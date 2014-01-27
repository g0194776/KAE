using System;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Proxies;

namespace KJFramework.Messages.ValueStored
{
    /// <summary>
    ///     存储对象字段的基类
    /// </summary>
    public abstract class BaseValueStored : ICloneable
    {
        #region Constructor

        /// <summary>
        ///     存储对象字段的基类
        /// </summary>
        protected BaseValueStored()
        {
            IsNull = false;
            IsExtension = false;
        }

        #endregion

        #region Members

        protected byte _typeId;
        /// <summary>
        ///     类型ID
        /// </summary>
        public byte TypeId
        {
            get { return _typeId; }
            protected set { _typeId = value; }
        }

        /// <summary>
        ///     是否为null值
        /// </summary>
        public bool IsNull { get; protected set; }

        /// <summary>
        ///     是否为扩展类型
        /// </summary>
        public bool IsExtension { get; protected set; }

        #endregion

        #region Method

        /// <summary>
        ///     获取当前类型的存储Value值
        /// </summary>
        /// <typeparam name="T">内部值类型</typeparam>
        /// <returns>返回内部所包含的值</returns>
        public abstract T GetValue<T>();
        /// <summary>
        ///   内部指定类型序列化方法
        /// </summary>
        /// <param name="proxy">内存代理器</param>
        public abstract void ToBytes(IMemorySegmentProxy proxy);
        /// <summary>
        ///   内部指定类型反序列化方法
        /// </summary>
        /// <param name="metadataObject">元数据集合</param>
        /// <param name="id">属性对应key</param>
        /// <param name="data">属性对应byte数组</param>
        /// <param name="offsetStart">属性在数组中的偏移值</param>
        /// <param name="length">属性在byte数组中的长度</param>
        public abstract void ToData(MetadataContainer metadataObject, byte id, byte[] data, int offsetStart, uint length);
        /// <summary>
        ///   返回一个实例对象
        /// </summary>
        public abstract object Clone();

        #endregion
    }
}
