using KJFramework.Messages.Proxies;

namespace KJFramework.Net.Transaction.Identities
{
    /// <summary>
    ///   基础的网络事务唯一标示
    /// </summary>
    public abstract class BasicIdentity
    {
        #region Members.

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
        public int MessageId { get; set; }

        /// <summary>
        ///   序列化特殊唯一标示字段
        ///   <para>*此处只能写入12个字节的数据，因为必须要保证整体的TransactionIdentity长度为18</para>
        /// </summary>
        /// <param name="proxy">内存片段代理器</param>
        public abstract void Serialize(IMemorySegmentProxy proxy);
        /// <summary>
        ///   反序列化特殊唯一标示字段
        /// </summary>
        /// <param name="data">字节数据</param>
        /// <param name="offset">可用数据起始偏移</param>
        /// <param name="length">可用数据长度</param>
        public abstract void Deserialize(byte[] data, int offset, int length);

        #endregion
    }
}