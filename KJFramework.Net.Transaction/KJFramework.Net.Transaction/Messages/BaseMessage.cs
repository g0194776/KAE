using System;

using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;
using KJFramework.Net.Channels.Identities;

namespace KJFramework.Net.Transaction.Messages
{
    /// <summary>
    ///     Server Side基础通讯消息，任何通讯消息都应该继承此类
    ///     <para>* 建议派生类的Intellect Property Id从10开始</para>
    /// </summary>
    public class BaseMessage : IntellectObject
    {
        #region Members

        /// <summary>
        ///     获取或设置与客户端通信的消息唯一标示
        /// </summary>
        [IntellectProperty(0)]
        public MessageIdentity MessageIdentity { get; set; }
        /// <summary>
        ///     获取或设置服务器内部的事务唯一标示
        /// </summary>
        [IntellectProperty(1)]
        public TransactionIdentity TransactionIdentity { get; set; }
        /// <summary>
        ///    获取或设置当前网络事务的过期时间
        /// </summary>
        [IntellectProperty(2)]
        public DateTime ExpireTime { get; set; }
        /// <summary>
        ///    获取或设置KPP的唯一ID
        ///     <para>* 如果当前事务当前不属于KAE范畴则此值为NULL</para>
        /// </summary>
        [IntellectProperty(3)]
        public Guid KPPUniqueId { get; set; }

        #endregion
    }
}