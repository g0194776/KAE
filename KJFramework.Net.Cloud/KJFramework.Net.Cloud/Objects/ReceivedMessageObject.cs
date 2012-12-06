using System;
using KJFramework.Net.Channels;

namespace KJFramework.Net.Cloud.Objects
{
    /// <summary>
    ///   接受消息，负责传递接收消息相关上下文参数
    /// </summary>
    public struct ReceivedMessageObject<T>
    {
        #region Members

        /// <summary>
        ///     获取或设置传输信道
        /// </summary>
        public IMessageTransportChannel<T> Channel { get; set; }
        /// <summary>
        ///     获取或设置网络节点编号
        /// </summary>
        public Guid NodeId { get; set; }
        /// <summary>
        ///     获取或设置接收到的消息实体
        /// </summary>
        public T Message { get; set; }

        #endregion
    }
}