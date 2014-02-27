using KJFramework.IO.Helper;
using KJFramework.Messages.Proxies;
using KJFramework.Net.Channels.EndPoints;
using KJFramework.Net.Channels.Enums;

namespace KJFramework.Net.Channels.Identities
{
    /// <summary>
    ///     基于命名管道的事务唯一标示
    /// </summary>
    public class NamedPipeTransactionIdentity : TransactionIdentity
    {
        #region Members.

        /// <summary>
        ///   获取当前网络唯一事务标示所代表了网络类型
        /// </summary>
        public override TransactionIdentityTypes IdentityType
        {
            get { return TransactionIdentityTypes.NamedPipe; }
        }

        #endregion

        #region Methods.

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            //真实的TransactionIdentity是14位的，然后在这里我们只显示12位
            //只显示12位的原因是，这里返回的string会被comparer使用来判断2个消息是否配对
            byte[] data = new byte[12];
            unsafe
            {
                fixed (byte* pByte = data)
                {
                    *(uint*) pByte = MessageId;
                    *(ulong*) (pByte + 4) = ((NamedPipeEndPoint)EndPoint).GetPipeCodeId();
                }
            }
            return ByteArrayHelper.ByteToHexStr(data);
        }

        /// <summary>
        ///   序列化当前事务唯一标示
        /// </summary>
        /// <param name="id">如果是以智能对象的方式进行序列化，则此ID为标签ID</param>
        /// <param name="serializationType">序列化方式</param>
        /// <param name="identity">被序列化的事务唯一标示</param>
        /// <param name="proxy">内存片段代理器</param>
        public static void Serialize(byte id, IdentitySerializationTypes serializationType, NamedPipeTransactionIdentity identity, IMemorySegmentProxy proxy)
        {
            if (serializationType == IdentitySerializationTypes.IntellectObject)
            {
                //writes attribute id.
                proxy.WriteByte(id);
                //writes NamedPipe transaction identity total length.
                proxy.WriteInt32(15);
            }
            proxy.WriteByte((byte)identity.IdentityType);
            proxy.WriteByte((byte)(identity.IsRequest ? 1 : 0));
            proxy.WriteByte((byte)(identity.IsOneway ? 1 : 0));
            proxy.WriteUInt64(((NamedPipeEndPoint)identity.EndPoint).GetPipeCodeId());
            proxy.WriteUInt32(identity.MessageId);
        }

        /// <summary>
        ///   反序列化当前事务唯一标示
        /// </summary>
        /// <param name="data">二进制数据</param>
        /// <param name="offset">起始位置</param>
        /// <param name="length">长度</param>
        public static TransactionIdentity Deserialize(byte[] data, int offset, int length)
        {
            NamedPipeTransactionIdentity identity = new NamedPipeTransactionIdentity();
            unsafe
            {
                //ignores first byte of Identity-Type.
                fixed (byte* pData = &data[offset + 1])
                {
                    identity.IsRequest = *pData == 1;
                    identity.IsOneway = *(pData + 1) == 1;
                    identity.EndPoint = new NamedPipeEndPoint(*(ulong*)(pData + 2));
                    identity.MessageId = *(uint*)(pData + 10);
                }
            }
            return identity;
        }

        #endregion
    }
}