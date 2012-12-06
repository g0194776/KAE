using System;
using System.Net;
using KJFramework.IO.Helper;
using KJFramework.Messages.Helpers;

namespace KJFramework.ServiceModel.Identity
{
    /// <summary>
    ///     事务唯一标示
    /// </summary>
    public class TransactionIdentity
    {
        #region Members

        /// <summary>
        ///     获取或设置终结点地址
        /// </summary>
        public IPEndPoint Iep { get; set; }
        /// <summary>
        ///     获取或设置消息编号
        /// </summary>
        public int MessageId { get; set; }
        /// <summary>
        ///     获取或设置一个值，该值标示了当前消息是否是请求
        /// </summary>
        public bool IsRequest { get; set; }
        /// <summary>
        ///     获取或设置一个值，该值标示了当前消息是否是单向的
        /// </summary>
        public bool IsOneway { get; set; }

        #endregion

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
            Buffer.BlockCopy(Iep.Address.GetAddressBytes(), 0, data, 0, 4);
            BitConvertHelper.GetBytes(Iep.Port, data, 5);
            BitConvertHelper.GetBytes(MessageId, data, 9);
            return ByteArrayHelper.ByteToHexStr(data);
        }

        #endregion
    }
}