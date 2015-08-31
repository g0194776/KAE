using System.Net;
using KJFramework.Net.Enums;

namespace KJFramework.Net.Identities
{
    /// <summary>
    ///     事物唯一标示
    /// </summary>
    public abstract class TransactionIdentity
    {
        /// <summary>
        ///   获取当前网络唯一事务标示所代表了网络类型
        /// </summary>
        public abstract TransactionIdentityTypes IdentityType { get; }
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
        ///     获取或设置远程终结点
        /// </summary>
        public EndPoint EndPoint { get; set; }
    }
}