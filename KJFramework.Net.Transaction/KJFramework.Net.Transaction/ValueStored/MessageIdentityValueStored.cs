using System;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Proxies;
using KJFramework.Messages.Types;
using KJFramework.Messages.ValueStored;
using KJFramework.Messages.ValueStored.StoredHelper;
using KJFramework.Net.Transaction.Identities;

namespace KJFramework.Net.Transaction.ValueStored
{
    /// <summary>
    ///     MessageIdentity类型的存储
    /// </summary>
    public class MessageIdentityValueStored : BaseValueStored
    {
        #region Members

        private readonly MessageIdentity _value;
        private static readonly PropertyValueStored<MessageIdentity> _instance;
        private static readonly Action<IMemorySegmentProxy, BaseValueStored> _toBytesDelegate;
        private static readonly Action<ResourceBlock, byte, byte[], int, uint> _toDataDelegate;

        #endregion

        #region Constructors

        /// <summary>
        ///     默认TypeId的构造函数
        /// </summary>
        public MessageIdentityValueStored()
        {
            _typeId = 255;
            IsExtension = true;
        }

        /// <summary>
        ///     MessageIdentity类型存储的初始化构造器
        /// </summary>
        public MessageIdentityValueStored(MessageIdentity value)
        {
            _value = value;
            _typeId = 255;
            IsExtension = true;
        }

        /// <summary>
        ///     MessageIdentity类型存储的静态构造器
        /// </summary>
        static MessageIdentityValueStored()
        {
            _instance = ValueStoredHelper.BuildMethod<MessageIdentity>();
            _toBytesDelegate = delegate(IMemorySegmentProxy proxy, BaseValueStored messageIdentityValueStored)
            {
                proxy.WriteByte(messageIdentityValueStored.GetValue<MessageIdentity>().ProtocolId);
                proxy.WriteByte(messageIdentityValueStored.GetValue<MessageIdentity>().ServiceId);
                proxy.WriteByte(messageIdentityValueStored.GetValue<MessageIdentity>().DetailsId); 
                proxy.WriteInt16(messageIdentityValueStored.GetValue<MessageIdentity>().Tid);
            };
            _toDataDelegate = delegate(ResourceBlock metadataObject, byte id, byte[] byteData, int offsetStart, uint offsetLength)
            {
                MessageIdentity messageIdentity = new MessageIdentity
                {
                    ProtocolId = byteData[offsetStart++],
                    ServiceId = byteData[offsetStart++],
                    DetailsId = byteData[offsetStart++],
                    Tid = BitConverter.ToInt16(byteData, offsetStart)
                };
                metadataObject.SetAttribute(id, new MessageIdentityValueStored(messageIdentity));
            };
        }

        #endregion

        #region Methods

        /// <summary>
        ///     获取存储的对应类型的Value值
        /// </summary>
        public override T GetValue<T>()
        {
            return _instance.Get<T>(_value);
        }

        /// <summary>
        ///   扩展类型序列化方法
        /// </summary>
        /// <param name="proxy">内存代理器</param>
        public override void ToBytes(IMemorySegmentProxy proxy)
        {
            _toBytesDelegate(proxy, this);
        }

        /// <summary>
        ///   扩展类型反序列化方法
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
            return new MessageIdentityValueStored();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return string.Format("(P: {0}, S: {1}, D: {2}, T: {3})", _value.ProtocolId, _value.ServiceId,
                        _value.DetailsId, _value.Tid);

        }

        #endregion
    }
}
