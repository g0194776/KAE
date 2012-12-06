using System.Collections.Generic;

namespace KJFramework.Net.Channels.Caches
{
    /// <summary>
    ///     封包片存根元接口，提供了相关的基本操作
    /// </summary>
    /// <typeparam name="T">消息类型</typeparam>
    public interface IMultiPacketStub<T>
    {
        /// <summary>
        ///     获取当前完整消息的编号
        /// </summary>
        int SessionId { get; }
        /// <summary>
        ///     获取最大封包片数目
        /// </summary>
        int MaxPacketCount { get; }
        /// <summary>
        ///     添加一个封包片
        /// </summary>
        /// <param name="message">封包片消息</param>
        /// <returns>如果返回值不为false, 则证明已经接收一个完整的消息</returns>
        bool AddPacket(T message);
        /// <summary>
        ///     获取内部所有的封包片
        /// </summary>
        /// <returns>返回封包片集合</returns>
        IList<T> GetPackets();
    }
}