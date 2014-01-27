using System;
using System.Net;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Proxies;
using KJFramework.Messages.Types;
using KJFramework.Messages.ValueStored;
using KJFramework.Messages.ValueStored.StoredHelper;
using KJFramework.Net.Transaction.Identities;

namespace KJFramework.Net.Transaction.ValueStored
{
    /// <summary>
    ///     TransactionIdentity类型的存储
    /// </summary>
    public class SocketTransactionIdentityValueStored : BaseValueStored
    {
        #region Members

        private readonly TransactionIdentity _value;
        private static readonly PropertyValueStored<TransactionIdentity> _instance;
        private static readonly Action<IMemorySegmentProxy, BaseValueStored> _toBytesDelegate;
        private static readonly Action<ResourceBlock, byte, byte[], int, uint> _toDataDelegate;

        #endregion

        #region Constructors

        /// <summary>
        ///     默认TypeId的构造函数
        /// </summary>
        public SocketTransactionIdentityValueStored()
        {
            _typeId = 254;
            IsExtension = true;
        }

        /// <summary>
        ///     TransactionIdentity类型存储的初始化构造器
        /// </summary>
        public SocketTransactionIdentityValueStored(TransactionIdentity value)
        {
            _value = value;
            _typeId = 254;
            IsExtension = true;
        }

        /// <summary>
        ///     TransactionIdentity类型存储的静态构造器
        /// </summary>
        static SocketTransactionIdentityValueStored()
        {
            _instance = ValueStoredHelper.BuildMethod<TransactionIdentity>();
            _toBytesDelegate = delegate(IMemorySegmentProxy proxy, BaseValueStored messageIdentityValueStored)
            {
                SocketTransactionIdentity identity = messageIdentityValueStored.GetValue<SocketTransactionIdentity>();
                proxy.WriteByte((byte)(identity.IsRequest ? 1 : 0));
                proxy.WriteByte((byte)(identity.IsOneway ? 1 : 0));
                proxy.WriteIPEndPoint(identity.EndPoint);
                proxy.WriteInt32(identity.MessageId);
            };
            _toDataDelegate = delegate(ResourceBlock metadataObject, byte id, byte[] byteData, int offsetStart, uint offsetLength)
            {
                SocketTransactionIdentity identity = new SocketTransactionIdentity();
                unsafe
                {
                    fixed (byte* pData = &byteData[offsetStart])
                    {
                        identity.IsRequest = *pData == 1;
                        identity.IsOneway = *(pData + 1) == 1;
                        identity.EndPoint = new IPEndPoint(new IPAddress(*(long*)(pData + 2)), *(int*)(pData + 10));
                        identity.MessageId = *(int*)(pData + 14);
                    }
                }
                metadataObject.SetAttribute(id, new SocketTransactionIdentityValueStored(identity));
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
            return new SocketTransactionIdentityValueStored();
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
            return _value == null ? string.Empty : _value.ToString();
        }

        #endregion
    }
}