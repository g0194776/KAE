using KJFramework.IO.Helper;

namespace KJFramework.Net.Transaction.Identities
{
    /// <summary>
    ///     基于命名管道的事务唯一标示
    /// </summary>
    public class NamedPipeTransactionIdentity
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
        ///     获取或设置消息编号
        /// </summary>
        public uint MessageId { get; set; }        
        /// <summary>
        ///     获取或设置命名管道地址的编号
        /// </summary>
        public ulong AddressCode { get; set; }

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
                    *(ulong*) (pByte + 4) = AddressCode;
                }
            }
            return ByteArrayHelper.ByteToHexStr(data);
        }

        #endregion
    }
}