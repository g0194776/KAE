using System.Net;
using KJFramework.IO.Helper;
using KJFramework.Messages.Helpers;
using KJFramework.Messages.Proxies;

namespace KJFramework.Net.Transaction.Identities
{
    /// <summary>
    ///     事物唯一标示
    /// </summary>
    public class TransactionIdentity : BasicIdentity
    {
        #region Members.

        /// <summary>
        ///     获取或设置远程终结点
        /// </summary>
        public IPEndPoint EndPoint { get; set; }

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
            System.Buffer.BlockCopy(EndPoint.Address.GetAddressBytes(), 0, data, 0, 4);
            BitConvertHelper.GetBytes(EndPoint.Port, data, 5);
            BitConvertHelper.GetBytes(MessageId, data, 9);
            return ByteArrayHelper.ByteToHexStr(data);
        }

        #endregion

        /// <summary>
        ///   序列化特殊唯一标示字段
        ///   <para>*此处只能写入12个字节的数据，因为必须要保证整体的TransactionIdentity长度为18</para>
        /// </summary>
        /// <param name="proxy">内存片段代理器</param>
        public override void Serialize(IMemorySegmentProxy proxy)
        {
            proxy.WriteIPEndPoint(EndPoint);
        }

        /// <summary>
        ///   反序列化特殊唯一标示字段
        /// </summary>
        /// <param name="data">字节数据</param>
        /// <param name="offset">可用数据起始偏移</param>
        /// <param name="length">可用数据长度</param>
        public override void Deserialize(byte[] data, int offset, int length)
        {
            unsafe
            {
                fixed (byte* pData = &data[offset])
                    EndPoint = new IPEndPoint(new IPAddress(*(long*)(pData)), *(int*)(pData + 8));
            }
        }
    }
}