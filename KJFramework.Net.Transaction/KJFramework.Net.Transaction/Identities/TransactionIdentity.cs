using System.Net;
using KJFramework.IO.Helper;
using KJFramework.Messages.Helpers;

namespace KJFramework.Net.Transaction.Identities
{
    /// <summary>
    ///     事物唯一标示
    /// </summary>
    public class TransactionIdentity
    {
        /// <summary>
        ///     获取或设置一个值，该值标示了当前的消息是否为请求消息
        /// </summary>
        public bool IsRequest { get; set; }
        /// <summary>
        ///     获取或设置一个值，该值标示了当前的消息是否需要响应
        /// </summary>
        public bool IsOneway { get; set; }
        /// <summary>
        ///     获取或设置远程终结点
        /// </summary>
        public IPEndPoint EndPoint { get; set; }
        /// <summary>
        ///     获取或设置消息编号
        /// </summary>
        public int MessageId { get; set; }

        #region Methods 

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
    }
}