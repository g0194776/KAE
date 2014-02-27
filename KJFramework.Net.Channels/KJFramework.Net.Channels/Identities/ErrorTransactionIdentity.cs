using KJFramework.Net.Channels.Enums;

namespace KJFramework.Net.Channels.Identities
{
    /// <summary>
    ///   错误场景所使用的事务唯一标示
    /// </summary>
    public class ErrorTransactionIdentity : TransactionIdentity
    {
        /// <summary>
        ///   获取当前网络唯一事务标示所代表了网络类型
        /// </summary>
        public override TransactionIdentityTypes IdentityType
        {
            get { return TransactionIdentityTypes.Unknown; }
        }
    }
}