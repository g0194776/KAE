using System.Net;
using KJFramework.Helpers;
using KJFramework.Messages.Helpers;
using KJFramework.Messages.Proxies;
using KJFramework.Net.Enums;

namespace KJFramework.Net.Identities
{
    /// <summary>
    ///   TCP网络通信事务唯一标示
    /// </summary>
    public class TCPTransactionIdentity : TransactionIdentity
    {
        #region Members.
        
        /// <summary>
        ///   获取当前网络唯一事务标示所代表了网络类型
        /// </summary>
        public override TransactionIdentityTypes IdentityType
        {
            get { return TransactionIdentityTypes.TCP; }
        }

        /// <summary>
        ///   序列化当前事务唯一标示
        /// </summary>
        /// <param name="id">如果是以智能对象的方式进行序列化，则此ID为标签ID</param>
        /// <param name="serializationType">序列化方式</param>
        /// <param name="identity">被序列化的事务唯一标示</param>
        /// <param name="proxy">内存片段代理器</param>
        public static void Serialize(byte id, IdentitySerializationTypes serializationType, TCPTransactionIdentity identity, IMemorySegmentProxy proxy)
        {
            if (serializationType == IdentitySerializationTypes.IntellectObject)
            {
                //writes attribute id.
                proxy.WriteByte(id);
                //writes TCP transaction identity total length.
                proxy.WriteInt32(19);
            }
            proxy.WriteByte((byte)identity.IdentityType);
            proxy.WriteByte((byte)(identity.IsRequest ? 1 : 0));
            proxy.WriteByte((byte)(identity.IsOneway ? 1 : 0));
            proxy.WriteIPEndPoint((IPEndPoint)identity.EndPoint);
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
            TCPTransactionIdentity identity = new TCPTransactionIdentity();
            unsafe
            {
                //ignores first byte of Identity-Type.
                fixed (byte* pData = &data[offset+1])
                {
                    identity.IsRequest = *pData == 1;
                    identity.IsOneway = *(pData + 1) == 1;
                    identity.EndPoint = new IPEndPoint(new IPAddress(*(long*)(pData + 2)), *(int*)(pData + 10));
                    identity.MessageId = *(uint*)(pData + 14);
                }
            }
            return identity;
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
            System.Buffer.BlockCopy(((IPEndPoint)EndPoint).Address.GetAddressBytes(), 0, data, 0, 4);
            BitConvertHelper.GetBytes(((IPEndPoint)EndPoint).Port, data, 5);
            BitConvertHelper.GetBytes(MessageId, data, 9);
            return ByteArrayHelper.ByteToHexStr(data);
        }

        #endregion
    }
}